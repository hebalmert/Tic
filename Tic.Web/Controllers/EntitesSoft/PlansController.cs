using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using Tic.Shared.EntitiesSoft;
using Tic.Web.Data;
using Tic.Web.Helpers;
using X.PagedList;

namespace Spi.Web.Controllers.EntitiesSoft
{
    [Authorize(Roles = "User")]

    public class PlansController : Controller
    {
        private readonly DataContext _context;
        private readonly INotyfService _notyfService;
        private readonly IComboHelper _comboHelper;

        public PlansController(DataContext context, INotyfService notyfService,
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
            var datoMag = (from modelo in _context.Plans
                           where modelo.PlanName.Contains(Prefix) && modelo.CorporateId == user!.CorporateId
                           select new
                           {
                               label = modelo.PlanName,
                               val = modelo.PlanId
                           }).ToList();

            return Json(datoMag);

        }

        // GET: Plans
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
                return View(await _context.Plans
                    .Include(z => z.Tax).Include(z => z.PlanCategory)
                    .Where(c => c.PlanId == buscarId && c.CorporateId == user.CorporateId).OrderBy(o => o.PlanCategory!.PlanCategoryName)
                    .ThenBy(o => o.PlanName)
                    .ToPagedListAsync(page ?? 1, 25));
            }
            else
            {
                return View(await _context.Plans
                    .Include(z => z.Tax).Include(z => z.PlanCategory)
                    .Where(c => c.CorporateId == user.CorporateId)
                    .OrderBy(o => o.PlanCategory!.PlanCategoryName)
                    .ThenBy(o => o.PlanName).ToPagedListAsync(page ?? 1, 25));
            }
        }

        // GET: Plans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Plans == null)
            {
                return NotFound();
            }

            var plan = await _context.Plans
                .Include(p => p.Corporate)
                .Include(p => p.PlanCategory)
                .Include(p => p.Tax)
                .Include(p => p.Server)
                .Include(p => p.TicketInactive)
                .Include(p => p.TicketRefresh)
                .Include(p => p.TicketTime)
                .FirstOrDefaultAsync(m => m.PlanId == id);
            if (plan == null)
            {
                return NotFound();
            }

            return View(plan);
        }

        // GET: Plans/Create
        public IActionResult Create(int id) //id viene de Servidores y es el ServerId
        {
            var user = _context.Users.Include(u => u.Corporate).FirstOrDefault(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                _notyfService.Custom("Problemas de Autenticacion debe comprobar credenciales -  Notificacion", 5, "#D90000", "fa fa-trash");
                return RedirectToAction("Login", "Account");
            }

            Plan modelo = new()
            {
                CorporateId = Convert.ToInt32(user.CorporateId),
                ShareUser = 1,
                Proxy = true,
                MacCookies = true,
                ContinueTime = true,
                Active = true
            };

            modelo.ListTax = _comboHelper.GetComboTaxes(modelo.CorporateId);
            modelo.ListCatPlan = _comboHelper.GetComboCatPlan(modelo.CorporateId);
            modelo.ListDown = _comboHelper.GetComboSpeedDownType();
            modelo.ListUp = _comboHelper.GetComboSpeedUpType();
            modelo.ListTimeTicket = _comboHelper.GetComboTimeTicket();
            modelo.ListTimeRefresh = _comboHelper.GetComboTimeRefresh();
            modelo.ListTimeInactive = _comboHelper.GetComboTimeInactive();
            //modelo.ListServer = _comboHelper.GetComboServerActivos(modelo.CorporateId);
            modelo.ServerId = id;

            return View(modelo);
        }

        // POST: Plans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Plan modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var ServDato = await _context.Servers.Include(x => x.IpNetwork).FirstOrDefaultAsync(x => x.ServerId == modelo.ServerId);

                    //Para hacer pruebas de solo agregar planes primero al sistema
                    var ping = new Ping();
                    PingReply reply = ping.Send(ServDato!.IpNetwork!.Ip!, 3000);
                    if (reply == null)
                    {
                        _notyfService.Custom("Error en la Conexion al Servidor Mikrotik -  Notificacion", 5, "#D90000", "fa fa-trash");
                        return View(modelo);
                    }

                    var Iproxy = modelo.Proxy == true ? "yes" : "no";
                    var ImacCookies = modelo.MacCookies == true ? "yes" : "no";
                    var tiempoTicket = _context.TicketTimes.Where(x => x.TicketTimeId == modelo.TicketTimeId).Select(x => new
                    {
                        tiempo = x.Tiempo,
                        SContinuo = x.ScriptContinuo,
                        SConsumo = x.ScriptConsumo
                    }).FirstOrDefault();
                    var inactivo = _context.TicketInactives.Where(x => x.TicketInactiveId == modelo.TicketInactiveId)
                        .Select(x => x.Tiempo).FirstOrDefault();
                    var refrescar = _context.TicketRefreshes.Where(x => x.TicketRefreshId == modelo.TicketRefreshId)
                        .Select(x => x.Tiempo).FirstOrDefault();

                    string MkContinuo = "";
                    string MikrotikId = "";
                    string IScriptConsumo = tiempoTicket!.SContinuo!;

                    ////////////////////////////////////////////////////////////
                    MK mikrotik = new MK(ServDato.IpNetwork.Ip!, ServDato.ApiPort);
                    if (!mikrotik.Login(ServDato.Usuario, ServDato.Clave))
                    {
                        _notyfService.Custom("Error en la Conexion al Servidor Mikrotik -  Notificacion", 5, "#D90000", "fa fa-trash");
                        return View(modelo);
                    }
                    else
                    {
                        if (modelo.ContinueTime == false)
                        {
                            IScriptConsumo = "";
                            mikrotik.Send("/system/scheduler/add");
                            mikrotik.Send("=name=" + modelo.PlanName);
                            mikrotik.Send("=interval=" + "45s");
                            mikrotik.Send("=policy=" + "ftp,reboot,read,write,policy,test,password,sniff,sensitive,romo");
                            mikrotik.Send("=start-time=" + "startup");
                            mikrotik.Send("=on-event=" + tiempoTicket.SConsumo);
                            mikrotik.Send("/system/scheduler/print", true);

                            int total = 0;
                            int rest = 0;
                            string IdMk;
                            string MkIndex;
                            foreach (var item2 in mikrotik.Read())
                            {
                                IdMk = item2;
                                total = IdMk.Length;
                                rest = total - 10;
                                MkIndex = IdMk.Substring(10, rest);

                                MkContinuo = MkIndex;
                            }
                        }

                        mikrotik.Send("/ip/hotspot/user/profile/add");
                        mikrotik.Send("=name=" + modelo.PlanName);
                        mikrotik.Send("=session-timeout=" + tiempoTicket.tiempo);
                        mikrotik.Send("=keepalive-timeout=" + tiempoTicket.tiempo);
                        mikrotik.Send("=rate-limit=" + modelo.VelocidadUp + "/" + modelo.VelocidadDown);
                        mikrotik.Send("=shared-users=" + modelo.ShareUser);
                        mikrotik.Send("=idle-timeout=" + inactivo);
                        mikrotik.Send("=status-autorefresh=" + refrescar);
                        mikrotik.Send("=add-mac-cookie=" + ImacCookies);
                        if (modelo.MacCookies == false)
                        {
                            mikrotik.Send("=!mac-cookie-timeout=");
                        }
                        else
                        {
                            mikrotik.Send("=mac-cookie-timeout=" + tiempoTicket.tiempo);
                        }
                        mikrotik.Send("=transparent-proxy=" + Iproxy);
                        mikrotik.Send("=on-login=" + IScriptConsumo);
                        mikrotik.Send("/ip/hotspot/user/profile/print", true);

                        int total2 = 0;
                        int rest2 = 0;
                        string IdMk2;
                        string MkIndex2;
                        foreach (var item3 in mikrotik.Read())
                        {
                            IdMk2 = item3;
                            total2 = IdMk2.Length;
                            rest2 = total2 - 10;
                            MkIndex2 = IdMk2.Substring(10, rest2);

                            MikrotikId = MkIndex2;
                        }

                    }

                    mikrotik.Close();
                    modelo.MkId = MikrotikId;
                    modelo.MkContinuoId = MkContinuo;
                    modelo.DateCreated = DateTime.Now;
                    //para poder ingresar datos si usar Mikrotik

                    _context.Add(modelo);
                    await _context.SaveChangesAsync();

                    _notyfService.Success("El Regitro se Guardado Con Exito -  Notificacion");
                    return RedirectToAction(nameof(Details), new { id = modelo.PlanId });
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

            modelo.ListTax = _comboHelper.GetComboTaxes(modelo.CorporateId);
            modelo.ListCatPlan = _comboHelper.GetComboCatPlan(modelo.CorporateId);
            modelo.ListDown = _comboHelper.GetComboSpeedDownType();
            modelo.ListUp = _comboHelper.GetComboSpeedUpType();
            modelo.ListTimeTicket = _comboHelper.GetComboTimeTicket();
            modelo.ListTimeRefresh = _comboHelper.GetComboTimeRefresh();
            modelo.ListTimeInactive = _comboHelper.GetComboTimeInactive();
            modelo.ListServer = _comboHelper.GetComboTimeInactive();

            return View(modelo);
        }

        // GET: Plans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var user = _context.Users.Include(u => u.Corporate).FirstOrDefault(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                _notyfService.Custom("Problemas de Autenticacion debe comprobar credenciales -  Notificacion", 5, "#D90000", "fa fa-trash");
                return RedirectToAction("Login", "Account");
            }

            if (id == null || _context.Plans == null)
            {
                return NotFound();
            }

            var modelo = await _context.Plans.FindAsync(id);
            if (modelo == null)
            {
                return NotFound();
            }

            modelo.ListTax = _comboHelper.GetComboTaxes(modelo.CorporateId);
            modelo.ListCatPlan = _comboHelper.GetComboCatPlan(modelo.CorporateId);
            modelo.ListDown = _comboHelper.GetComboSpeedDownType();
            modelo.ListUp = _comboHelper.GetComboSpeedUpType();
            modelo.ListTimeTicket = _comboHelper.GetComboTimeTicket();
            modelo.ListTimeRefresh = _comboHelper.GetComboTimeRefresh();
            modelo.ListTimeInactive = _comboHelper.GetComboTimeInactive();
            //modelo.ListServer = _comboHelper.GetComboTimeInactive();

            return View(modelo);
        }

        // POST: Plans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Plan modelo)
        {
            if (id != modelo.PlanId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var ServDato = await _context.Servers.Include(x => x.IpNetwork).FirstOrDefaultAsync(x => x.ServerId == modelo.ServerId);

                    var ping = new Ping();
                    PingReply reply = ping.Send(ServDato!.IpNetwork!.Ip!, 3000);
                    if (reply == null)
                    {
                        _notyfService.Custom("Error en la Conexion al Servidor Mikrotik -  Notificacion", 5, "#D90000", "fa fa-trash");
                        return View(modelo);
                    }

                    var Iproxy = modelo.Proxy == true ? "yes" : "no";
                    var ImacCookies = modelo.MacCookies == true ? "yes" : "no";
                    var tiempoTicket = _context.TicketTimes.Where(x => x.TicketTimeId == modelo.TicketTimeId).Select(x => new
                    {
                        tiempo = x.Tiempo,
                        SContinuo = x.ScriptContinuo,
                        SConsumo = x.ScriptConsumo
                    }).FirstOrDefault();
                    var inactivo = _context.TicketInactives.Where(x => x.TicketInactiveId == modelo.TicketInactiveId)
                        .Select(x => x.Tiempo).FirstOrDefault();
                    var refrescar = _context.TicketRefreshes.Where(x => x.TicketRefreshId == modelo.TicketRefreshId)
                        .Select(x => x.Tiempo).FirstOrDefault();

                    string IScriptConsumo = tiempoTicket!.SContinuo!;

                    ////////////////////////////////////////////////////////////
                    MK mikrotik = new MK(ServDato.IpNetwork.Ip!, ServDato.ApiPort);
                    if (!mikrotik.Login(ServDato.Usuario, ServDato.Clave))
                    {
                        _notyfService.Custom("Error en la Conexion al Servidor Mikrotik -  Notificacion", 5, "#D90000", "fa fa-trash");
                        return View(modelo);
                    }
                    else
                    {
                        if (modelo.ContinueTime == false)
                        {
                            IScriptConsumo = "";
                            mikrotik.Send("/system/scheduler/set");
                            mikrotik.Send("=.id=" + modelo.MkContinuoId);
                            mikrotik.Send("=name=" + modelo.PlanName);
                            mikrotik.Send("=interval=" + "45s");
                            mikrotik.Send("=policy=" + "ftp,reboot,read,write,policy,test,password,sniff,sensitive,romo");
                            mikrotik.Send("=start-time=" + "startup");
                            mikrotik.Send("=on-event=" + tiempoTicket.SConsumo);
                            mikrotik.Send("/system/scheduler/print", true);

                            int total = 0;
                            int rest = 0;
                            string IdMk;
                            foreach (var item2 in mikrotik.Read())
                            {
                                IdMk = item2;
                                total = IdMk.Length;
                                rest = total - 10;
                            }
                        }

                        mikrotik.Send("/ip/hotspot/user/profile/set");
                        mikrotik.Send("=.id=" + modelo.MkId);
                        mikrotik.Send("=name=" + modelo.PlanName);
                        mikrotik.Send("=session-timeout=" + tiempoTicket.tiempo);
                        mikrotik.Send("=keepalive-timeout=" + tiempoTicket.tiempo);
                        mikrotik.Send("=rate-limit=" + modelo.VelocidadUp + "/" + modelo.VelocidadDown);
                        mikrotik.Send("=shared-users=" + modelo.ShareUser);
                        mikrotik.Send("=idle-timeout=" + inactivo);
                        mikrotik.Send("=status-autorefresh=" + refrescar);
                        mikrotik.Send("=add-mac-cookie=" + ImacCookies);
                        if (modelo.MacCookies == false)
                        {
                            mikrotik.Send("=!mac-cookie-timeout=");
                        }
                        else
                        {
                            mikrotik.Send("=mac-cookie-timeout=" + tiempoTicket.tiempo);
                        }
                        mikrotik.Send("=transparent-proxy=" + Iproxy);
                        mikrotik.Send("=on-login=" + IScriptConsumo);
                        mikrotik.Send("/ip/hotspot/user/profile/print", true);

                        int total2 = 0;
                        int rest2 = 0;
                        string IdMk2;
                        foreach (var item3 in mikrotik.Read())
                        {
                            IdMk2 = item3;
                            total2 = IdMk2.Length;
                            rest2 = total2 - 10;
                        }

                    }

                    mikrotik.Close();
                    modelo.DateEdit = DateTime.Now;
                    _context.Update(modelo);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("El Regitro se ha Actualizado con Exito -  Notificacion");

                    return RedirectToAction(nameof(Details), new { id = modelo.PlanId });
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

            modelo.ListTax = _comboHelper.GetComboTaxes(modelo.CorporateId);
            modelo.ListCatPlan = _comboHelper.GetComboCatPlan(modelo.CorporateId);
            modelo.ListDown = _comboHelper.GetComboSpeedDownType();
            modelo.ListUp = _comboHelper.GetComboSpeedUpType();
            modelo.ListTimeTicket = _comboHelper.GetComboTimeTicket();
            modelo.ListTimeRefresh = _comboHelper.GetComboTimeRefresh();
            modelo.ListTimeInactive = _comboHelper.GetComboTimeInactive();
            modelo.ListServer = _comboHelper.GetComboTimeInactive();

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

                var dato = await _context.Plans.FirstOrDefaultAsync(m => m.PlanId == id);
                if (dato == null)
                {
                    return NotFound();
                }

                var ServDato = await _context.Servers.Include(x => x.IpNetwork).FirstOrDefaultAsync(x => x.ServerId == dato.ServerId);

                var ping = new Ping();
                PingReply reply = ping.Send(ServDato!.IpNetwork!.Ip!, 3000);
                if (reply == null)
                {
                    _notyfService.Custom("Error en la Conexion al Servidor Mikrotik -  Notificacion", 5, "#D90000", "fa fa-trash");
                    return NotFound();
                }

                _context.Remove(dato);
                await _context.SaveChangesAsync();


                ////////////////////////////////////////////////////////////
                MK mikrotik = new MK(ServDato.IpNetwork.Ip!, ServDato.ApiPort);
                if (!mikrotik.Login(ServDato.Usuario, ServDato.Clave))
                {
                    _notyfService.Custom("Error en la Conexion al Servidor Mikrotik -  Notificacion", 5, "#D90000", "fa fa-trash");
                    return NotFound();
                }
                else
                {
                    if (dato.ContinueTime == false)
                    {
                        mikrotik.Send("/system/scheduler/remove");
                        mikrotik.Send("=.id=" + dato.MkContinuoId, true);

                        int total2 = 0;
                        int rest2 = 0;
                        string IdMk;
                        foreach (var item2 in mikrotik.Read())
                        {
                            IdMk = item2;
                            total2 = IdMk.Length;
                            rest2 = total2 - 10;
                        }
                    }

                    mikrotik.Send("/ip/hotspot/user/profile/remove");
                    mikrotik.Send("=.id=" + dato.MkId, true);

                    int total = 0;
                    int rest = 0;
                    string idmk;

                    foreach (var item2 in mikrotik.Read())
                    {
                        idmk = item2;
                        total = idmk.Length;
                        rest = total - 10;

                    }

                    mikrotik.Close();
                }

                await transaction.CommitAsync();

                _notyfService.Custom("El Regitro se ha Eliminado Con Exito -  Notificacion", 5, "#D90000", "fa fa-trash");
                return RedirectToAction("Details","Servers", new { id = dato.ServerId});

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

        private bool PlanExists(int id)
        {
            return (_context.Plans?.Any(e => e.PlanId == id)).GetValueOrDefault();
        }

        [HttpPost]
        public JsonResult GetTax(int IdTax)
        {
            var tasa = _context.Taxes
                .FirstOrDefault(c => c.TaxId == IdTax);

            var dato = new
            {
                tasa = tasa!.Rate
            };

            return Json(dato);
        }
    }
}
