using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tic.Shared.EntitiesSoft;
using Tic.Web.Data;
using Tic.Web.Helpers;
using X.PagedList;

namespace Tic.Web.Controllers.EntitiesSoft
{
    [Authorize(Roles = "User")]

    public class IpNetworksController : Controller
    {
        private readonly DataContext _context;
        private readonly INotyfService _notyfService;
        private readonly IComboHelper _comboHelper;

        public IpNetworksController(DataContext context, INotyfService notyfService,
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
            var datoMag = (from modelo in _context.IpNetworks
                           where modelo.Ip!.Contains(Prefix) && modelo.CorporateId == user!.CorporateId
                           select new
                           {
                               label = modelo.Ip,
                               val = modelo.IpNetworkId
                           }).ToList();

            return Json(datoMag);

        }

        // GET: IpNetworks
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
                return View(await _context.IpNetworks
                    .Where(c => c.IpNetworkId == buscarId && c.CorporateId == user.CorporateId).OrderBy(o => o.Ip)
                    .ToPagedListAsync(page ?? 1, 25));
            }
            else
            {
                return View(await _context.IpNetworks
                    .Where(c => c.CorporateId == user.CorporateId)
                    .OrderBy(o => o.Ip).ToPagedListAsync(page ?? 1, 25));
            }
        }

        // GET: IpNetworks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.IpNetworks == null)
            {
                return NotFound();
            }

            var ipNetwork = await _context.IpNetworks
                .Include(i => i.Corporate)
                .FirstOrDefaultAsync(m => m.IpNetworkId == id);
            if (ipNetwork == null)
            {
                return NotFound();
            }

            return View(ipNetwork);
        }

        // GET: IpNetworks/Create
        public IActionResult Create()
        {
            var user = _context.Users.Include(u => u.Corporate).FirstOrDefault(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                _notyfService.Custom("Problemas de Autenticacion debe comprobar credenciales -  Notificacion", 5, "#D90000", "fa fa-trash");
                return RedirectToAction("Login", "Account");
            }

            IpNetwork datos = new()
            {
                CorporateId = Convert.ToInt32(user.CorporateId),
                Active = true
            };

            return View(datos);
        }

        // POST: IpNetworks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IpNetwork modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(modelo);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("El Regitro se Guardado Con Exito -  Notificacion");

                    return RedirectToAction(nameof(Details), new { id = modelo.IpNetworkId });
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

            return View(modelo);
        }

        // GET: IpNetworks/Create
        public IActionResult AddPool()
        {
            var user = _context.Users.Include(u => u.Corporate).FirstOrDefault(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                _notyfService.Custom("Problemas de Autenticacion debe comprobar credenciales -  Notificacion", 5, "#D90000", "fa fa-trash");
                return RedirectToAction("Login", "Account");
            }

            IpNetDTOs datos = new()
            {
                CorporateId = Convert.ToInt32(user.CorporateId)
            };

            return View(datos);
        }

        // POST: IpNetworks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPool(IpNetDTOs modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Se realiza el proceso de auto RollBack para algun fallo de Guardadoplanes de internet
                    using var transaction = _context.Database.BeginTransaction();

                    int Inicio = modelo.Desde;
                    int Fin = modelo.Hasta;
                    for (int i = Inicio; i < Fin + 1; ++i)
                    {
                        IpNetwork ipnetwork = new() 
                        { 
                            CorporateId = Convert.ToInt32(modelo.CorporateId),
                            Active = true,
                            Ip = modelo.Ip1 + "." + Convert.ToString(i)
                        };
                        _context.Add(ipnetwork);
                    }
                    await _context.SaveChangesAsync();

                    transaction.Commit();


                    _notyfService.Success("El Regitro se ha Guardado Con Exito -  Notificacion");

                    return RedirectToAction(nameof(Index));
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

            return View(modelo);
        }

        // GET: IpNetworks/Create
        public IActionResult DelPool()
        {
            var user = _context.Users.Include(u => u.Corporate).FirstOrDefault(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                _notyfService.Custom("Problemas de Autenticacion debe comprobar credenciales -  Notificacion", 5, "#D90000", "fa fa-trash");
                return RedirectToAction("Login", "Account");
            }

            IpNetDTOs datos = new()
            {
                CorporateId = Convert.ToInt32(user.CorporateId)
            };

            return View(datos);
        }

        // POST: IpNetworks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DelPool(IpNetDTOs modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Se realiza el proceso de auto RollBack para algun fallo de Guardadoplanes de internet
                    using var transaction = _context.Database.BeginTransaction();

                    int Inicio = modelo.Desde;
                    int Fin = modelo.Hasta;
                    for (int i = Inicio; i < Fin + 1; ++i)
                    {
                        IpNetwork ipnetwork = new()
                        {
                            CorporateId = Convert.ToInt32(modelo.CorporateId),
                            Active = true,
                            Ip = modelo.Ip1 + "." + Convert.ToString(i)
                        };
                        IpNetwork borrado = await _context.IpNetworks.FirstAsync(x => x.CorporateId == modelo.CorporateId && x.Ip == ipnetwork.Ip);
                        _context.Remove(borrado);
                    }
                    await _context.SaveChangesAsync();

                    transaction.Commit();


                    _notyfService.Custom("El Regitro se ha Eliminado Con Exito -  Notificacion", 5, "#D90000", "fa fa-trash");

                    return RedirectToAction(nameof(Index));
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

            return View(modelo);
        }
        // GET: IpNetworks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var user = _context.Users.Include(u => u.Corporate).FirstOrDefault(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                _notyfService.Custom("Problemas de Autenticacion debe comprobar credenciales -  Notificacion", 5, "#D90000", "fa fa-trash");
                return RedirectToAction("Login", "Account");
            }

            if (id == null || _context.IpNetworks == null)
            {
                return NotFound();
            }

            var ipNetwork = await _context.IpNetworks.FindAsync(id);
            if (ipNetwork == null)
            {
                return NotFound();
            }

            return View(ipNetwork);
        }

        // POST: IpNetworks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IpNetwork modelo)
        {
            if (id != modelo.IpNetworkId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(modelo);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("El Regitro se ha Actualizado con Exito -  Notificacion");

                    return RedirectToAction(nameof(Details), new { id = modelo.IpNetworkId });
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
                var dato = await _context.IpNetworks.FirstOrDefaultAsync(m => m.IpNetworkId == id);
                if (dato == null)
                {
                    return NotFound();
                }

                _context.Remove(dato);
                await _context.SaveChangesAsync();
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

        private bool IpNetworkExists(int id)
        {
            return (_context.IpNetworks?.Any(e => e.IpNetworkId == id)).GetValueOrDefault();
        }
    }
}
