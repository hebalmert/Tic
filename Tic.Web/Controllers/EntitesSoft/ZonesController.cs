using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tic.Shared.EntitiesSoft;
using Tic.Web.Data;
using Tic.Web.Helpers;
using X.PagedList;

namespace Tic.Web.Controllers.EntitesSoft
{
    [Authorize(Roles = "User")]

    public class ZonesController : Controller
    {
        private readonly DataContext _context;
        private readonly INotyfService _notyfService;
        private readonly IComboHelper _comboHelper;

        public ZonesController(DataContext context, INotyfService notyfService,
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
            var datoMag = (from modelo in _context.Zones
                           where modelo.ZoneName.ToLower().Contains(Prefix.ToLower()) && modelo.CorporateId == user!.CorporateId
                           select new
                           {
                               label = modelo.ZoneName,
                               val = modelo.ZoneId
                           }).ToList();

            return Json(datoMag);

        }

        // GET: Zones
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
                return View(await _context.Zones
                    .Include(z => z.City).Include(z => z.State)
                    .Where(c => c.ZoneId == buscarId && c.CorporateId == user.CorporateId).OrderBy(o => o.State!.Name)
                    .ThenBy(o => o.City!.Name)
                    .ToPagedListAsync(page ?? 1, 25));
            }
            else
            {
                return View(await _context.Zones
                    .Include(z => z.City).Include(z => z.State)
                    .Where(c => c.CorporateId == user.CorporateId)
                    .OrderBy(o => o.State!.Name)
                    .ThenBy(o => o.City!.Name).ToPagedListAsync(page ?? 1, 25));
            }
        }

        // GET: Zones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Zones == null)
            {
                return NotFound();
            }

            var zone = await _context.Zones
                .Include(z => z.City)
                .Include(z => z.Corporate)
                .Include(z => z.State)
                .FirstOrDefaultAsync(m => m.ZoneId == id);
            if (zone == null)
            {
                return NotFound();
            }

            return View(zone);
        }

        // GET: Zones/Create
        public IActionResult Create()
        {
            var user = _context.Users.Include(u => u.Corporate).FirstOrDefault(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                _notyfService.Custom("Problemas de Autenticacion debe comprobar credenciales -  Notificacion", 5, "#D90000", "fa fa-trash");
                return RedirectToAction("Login", "Account");
            }

            Zone datos = new()
            {
                CorporateId = Convert.ToInt32(user.CorporateId),
                Active = true
            };

            datos.ListState = _comboHelper.GetComboState(user.Corporate!.CountryId);
            datos.CountryId = user.Corporate!.CountryId;
            return View(datos);
        }

        // POST: Zones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Zone modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(modelo);
                    await _context.SaveChangesAsync();

                    _notyfService.Success("El Regitro se Guardado Con Exito -  Notificacion");
                    return RedirectToAction(nameof(Details), new { id = modelo.ZoneId });
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

            modelo.ListState = _comboHelper.GetComboState(modelo.CountryId);
            modelo.ListCities = _comboHelper.GetComboCity(modelo.StateId);

            return View(modelo);
        }

        // GET: Zones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var user = _context.Users.Include(u => u.Corporate).FirstOrDefault(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                _notyfService.Custom("Problemas de Autenticacion debe comprobar credenciales -  Notificacion", 5, "#D90000", "fa fa-trash");
                return RedirectToAction("Login", "Account");
            }

            if (id == null || _context.Zones == null)
            {
                return NotFound();
            }

            var modelo = await _context.Zones.FindAsync(id);
            if (modelo == null)
            {
                return NotFound();
            }

            modelo.ListState = _comboHelper.GetComboState(modelo.Corporate!.CountryId);
            modelo.ListCities = _comboHelper.GetComboCity(modelo.StateId);
            modelo.CountryId = modelo.Corporate!.CountryId;
            return View(modelo);
        }

        // POST: Zones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Zone modelo)
        {
            if (id != modelo.ZoneId)
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
                    return RedirectToAction(nameof(Details), new { id = modelo.ZoneId });
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

            modelo.ListState = _comboHelper.GetComboState(modelo.CountryId);
            modelo.ListCities = _comboHelper.GetComboCity(modelo.StateId);

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
                var dato = await _context.Zones.FirstOrDefaultAsync(m => m.ZoneId == id);
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

        [HttpPost]
        public JsonResult GetState(int IdState)
        {
            var dato = _context.Cities.Where(c => c.StateId == IdState);
            if (dato == null)
            {
                return null!;
            }

            return Json(dato.Select(x => new SelectListItem(x.Name, x.CityId.ToString())));
        }

        private bool ZoneExists(int id)
        {
            return _context.Zones.Any(e => e.ZoneId == id);
        }
    }
}
