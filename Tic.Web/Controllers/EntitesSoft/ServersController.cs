using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tic.Shared.EntitiesSoft;
using Tic.Web.Data;
using Tic.Web.Helpers;
using System.Net.NetworkInformation;
using X.PagedList;

namespace Tic.Web.Controllers.EntitiesSoft
{
    [Authorize(Roles = "User")]
    public class ServersController : Controller
    {
        private readonly DataContext _context;
        private readonly INotyfService _notyfService;
        private readonly IComboHelper _comboHelper;

        public ServersController(DataContext context, INotyfService notyfService,
            IComboHelper comboHelper)
        {
            _context = context;
            _notyfService = notyfService;
            _comboHelper = comboHelper;
        }

        [HttpPost]
        public JsonResult Search(string Prefix)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity!.Name);
            var datoMag = (from modelo in _context.Servers
                           where modelo.ServerName!.Contains(Prefix) && modelo.CorporateId == user!.CorporateId
                           select new
                           {
                               label = modelo.ServerName,
                               val = modelo.ServerId
                           }).ToList();

            return Json(datoMag);

        }

        // GET: Servers
        public async Task<IActionResult> Index(int? buscarId, int? page)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                _notyfService.Custom("Problemas de Autenticacion debe comprobar credenciales -  Notificacion", 5, "#D90000", "fa fa-trash");
                return RedirectToAction("Login", "Account");
            }

            if (buscarId != null)
            {
                var dataContext = _context.Servers.Include(n => n.IpNetwork).Include(n => n.Mark)
                    .Include(n => n.MarkModel).Include(n => n.Zone)
                    .Where(c => c.ServerId == buscarId && c.CorporateId == user.CorporateId).OrderBy(o => o.ServerName);

                return View(await dataContext.ToPagedListAsync(page ?? 1, 25));
            }
            else
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                var dataContext = _context.Servers.Include(n => n.IpNetwork).Include(n => n.Mark)
                    .Include(n => n.MarkModel).Include(n => n.Zone)
                    .Where(n => n.CorporateId == user.CorporateId).OrderBy(o => o.IpNetwork.Ip);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                return View(await dataContext.ToPagedListAsync(page ?? 1, 25));
            }
        }

        // GET: Servers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Servers == null)
            {
                return NotFound();
            }

            var server = await _context.Servers
                .Include(s => s.Corporate)
                .Include(s => s.IpNetwork)
                .Include(s => s.Mark)
                .Include(s => s.MarkModel)
                .Include(n => n.Zone)
                .ThenInclude(n => n!.State)
                .Include(n => n.Zone)
                .ThenInclude(n => n!.City)
                .FirstOrDefaultAsync(m => m.ServerId == id);
            if (server == null)
            {
                return NotFound();
            }

            return View(server);
        }

        // GET: Servers/Create
        public IActionResult Create()
        {
            var user = _context.Users.Include(u => u.Corporate).FirstOrDefault(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                _notyfService.Custom("Problemas de Autenticacion debe comprobar credenciales -  Notificacion", 5, "#D90000", "fa fa-trash");
                return RedirectToAction("Login", "Account");
            }

            Server modelo = new()
            {
                CorporateId = Convert.ToInt32(user.CorporateId),
                Active = true
            };

            modelo.ListIpNetwork = _comboHelper.GetComboIpNetwork(modelo.CorporateId);
            modelo.ListMark = _comboHelper.GetComboMark(modelo.CorporateId);
            modelo.ListState = _comboHelper.GetComboState(user.Corporate!.CountryId);

            return View(modelo);
        }

        // POST: Servers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Server modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var transaction = await _context.Database.BeginTransactionAsync();

                    var ip = await _context.IpNetworks.FirstOrDefaultAsync(c => c.IpNetworkId == modelo.IpNetworkId);
                    ip!.Assigned = true;
                    ip.Description = modelo.ServerName;
                    _context.Update(ip);

                    _context.Add(modelo);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    _notyfService.Success("El Regitro se Guardado Con Exito -  Notificacion");

                    return RedirectToAction(nameof(Details), new { id = modelo.ServerId });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                    {
                        _notyfService.Error("Existe Algun Registro con el Mismo Nombre - Notificacion");
                    }
                    else
                    {
                        _notyfService.Error(dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    _notyfService.Error(exception.Message);
                }
            }
            modelo.ListIpNetwork = _comboHelper.GetComboIpNetwork(modelo.CorporateId);
            modelo.ListMark = _comboHelper.GetComboMark(modelo.CorporateId);
            modelo.ListState = _comboHelper.GetComboState(modelo.Corporate!.CountryId);

            return View(modelo);
        }

        // GET: Servers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Servers == null)
            {
                return NotFound();
            }

            Server? modelo = await _context.Servers
                .Include(c => c.Zone)
                .Include(c => c.Corporate).FirstOrDefaultAsync(c => c.ServerId == id);

            if (modelo == null)
            {
                return NotFound();
            }


            modelo.StateId = modelo.Zone!.StateId;
            modelo.CityId = modelo.Zone!.CityId;
            modelo!.ListIpNetwork = _comboHelper.GetComboIpNetworkUp(modelo.CorporateId, modelo.IpNetworkId);
            modelo.ListMark = _comboHelper.GetComboMark(modelo.CorporateId);
            modelo.ListMarkModel = _comboHelper.GetCombomarkModel(modelo.CorporateId);
            modelo.ListState = _comboHelper.GetComboState(modelo.Corporate!.CountryId);
            modelo.ListCity = _comboHelper.GetComboCity(modelo.StateId);
            modelo.ListZone = _comboHelper.GetComboZone(modelo.CityId);

            modelo.CurrentIpNetworkId = modelo.IpNetworkId;
            modelo.ClaveConfirm = modelo.Clave;

            return View(modelo);
        }

        // POST: Servers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Server modelo)
        {
            if (id != modelo.ServerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var transaction = await _context.Database.BeginTransactionAsync();

                    if (modelo.CurrentIpNetworkId != modelo.IpNetworkId)
                    {
                        var currenIp = await _context.IpNetworks.FindAsync(modelo.CurrentIpNetworkId);
                        currenIp!.Assigned = false;
                        currenIp.Description = "";
                        _context.Update(currenIp);

                        var upIp = await _context.IpNetworks.FindAsync(modelo.IpNetworkId);
                        upIp!.Assigned = true;
                        upIp.Description = modelo.ServerName;
                        _context.Update(upIp);
                    }

                    _context.Update(modelo);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    _notyfService.Success("El Regitro se ha Actualizado con Exito -  Notificacion");

                    return RedirectToAction(nameof(Details), new { id = modelo.ServerId });


                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                    {
                        _notyfService.Error("Existe Algun Registro con el Mismo Nombre - Notificacion");
                    }
                    else
                    {
                        _notyfService.Error(dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    _notyfService.Error(exception.Message);
                }
            }

            modelo.StateId = modelo.Zone!.StateId;
            modelo.CityId = modelo.Zone!.CityId;
            modelo!.ListIpNetwork = _comboHelper.GetComboIpNetwork(modelo.CorporateId);
            modelo.ListMark = _comboHelper.GetComboMark(modelo.CorporateId);
            modelo.ListMarkModel = _comboHelper.GetCombomarkModel(modelo.CorporateId);
            modelo.ListState = _comboHelper.GetComboState(modelo.Corporate!.CountryId);
            modelo.ListCity = _comboHelper.GetComboCity(modelo.StateId);
            modelo.ListZone = _comboHelper.GetComboZone(modelo.CityId);

            return View(modelo);
        }

        // Post: SpeedDowns/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var transaction = await _context.Database.BeginTransactionAsync();

                var dato = await _context.Servers.FirstOrDefaultAsync(m => m.ServerId == id);
                if (dato == null)
                {
                    return NotFound();
                }

                var ipDelete = await _context.IpNetworks.FirstOrDefaultAsync(m => m.IpNetworkId == dato.IpNetworkId);
                ipDelete!.Assigned = false;
                ipDelete.Description = "";
                _context.IpNetworks.Update(ipDelete!);

                _context.Remove(dato);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                _notyfService.Custom("El Regitro se ha Eliminado Con Exito -  Notificacion", 5, "#D90000", "fa fa-trash");

                return RedirectToAction(nameof(Index));

            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("REFERENCE"))
                {
                    _notyfService.Error("Existe Algun Registro Relacionado - Notificacion");
                }
                else
                {
                    _notyfService.Error(dbUpdateException.InnerException.Message);
                }

            }
            catch (Exception e)
            {
                _notyfService.Error(e.Message);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetMarkModel(int IdMark)
        {
            var dato = _context.MarkModels.Where(c => c.MarkId == IdMark).OrderBy(c => c.MarkModelName);
            if (dato == null)
            {
                return null!;
            }

            return Json(dato.Select(x => new SelectListItem(x.MarkModelName, x.MarkModelId.ToString())));
        }

        [HttpPost]
        public JsonResult GetCity(int IdState)
        {
            var dato = _context.Cities.Where(c => c.StateId == IdState);
            if (dato == null)
            {
                return null!;
            }

            return Json(dato.Select(x => new SelectListItem(x.Name, x.CityId.ToString())));
        }

        [HttpPost]
        public JsonResult GetZone(int IdState, int IdCity)
        {
            var dato = _context.Zones.Where(c => c.StateId == IdState && c.CityId == IdCity);
            if (dato == null)
            {
                return null!;
            }

            return Json(dato.Select(x => new SelectListItem(x.ZoneName, x.ZoneId.ToString())));
        }

        [HttpPost]
        public JsonResult GetPing(int ipnetwok)
        {
            var dato = _context.IpNetworks.FirstOrDefault(c => c.IpNetworkId == ipnetwok);
            if (dato == null)
            {
                return null!;
            }

            var ping = new Ping();
            PingReply reply = ping.Send(dato.Ip!, 4000);
            if (reply.RoundtripTime != 0)
            {
                var datos = new
                {
                    text = "Respuesta Exitosa de PING",
                    value = $"{dato.Ip} Tiempo: {reply.RoundtripTime} ms TTL: {reply.Options!.Ttl}",
                };

                return Json(datos);
            }
            else
            {
                var datos = new
                {
                    text = "Host no Alcanzado",
                    value = $"{dato.Ip} Tiempo: Agotado ms TTL: Agotado",
                };

                return Json(datos);
            }
        }

        [HttpPost]
        public JsonResult GetMikro(int ipServer)
        {
            var dato = _context.Servers
                .Include(c=> c.IpNetwork)
                .FirstOrDefault(c => c.ServerId == ipServer);
            if (dato == null)
            {
                return null!;
            }

            MK mikrotik = new MK(dato.IpNetwork!.Ip!, dato.ApiPort);
            if (!mikrotik.Login(dato.Usuario, dato.Clave))
            {
                var datos = new
                {
                    text = "Error de Conexion a Mikrotik",
                    value = "Verifique Puerto API, Ping o Usuario y Clave",
                };

                return Json(datos);
            }
            else
            {
                mikrotik.Send("/system/identity/getall");
                mikrotik.Send("/system/identity/print", true);
                List<string> listArray = new List<string>();
                foreach (string s in mikrotik.Read()) 
                {
                    listArray.Add(s);
                }
                var listArrayCount = listArray.Count;
                listArray.RemoveAt(listArrayCount - 1);
                var PrimerRegistro = listArray.FirstOrDefault();
                var NameServidor = PrimerRegistro!.Substring(9);


                mikrotik.Send("/ip/hotspot/ip-binding/getall");
                mikrotik.Send("/ip/hotspot/ip-binding/print");
                mikrotik.Send("=.proplist=address", true);
                List<string> list = new List<string>();
                foreach (var item in mikrotik.Read())
                {
                    list.Add(item);
                }
                var listAcount = list.Count;

                mikrotik.Close();
                //var listAcount = list.Count() - 1;
                var datos = new
                {
                    text = $"Conexion Exitosa a Mikrotik {NameServidor}",
                    value = $"Mikrotik tiene {listAcount - 1} Registros en Ip Binding",
                };
                return Json(datos);
            }
        }

        private bool ServerExists(int id)
        {
            return (_context.Servers?.Any(e => e.ServerId == id)).GetValueOrDefault();
        }
    }
}
