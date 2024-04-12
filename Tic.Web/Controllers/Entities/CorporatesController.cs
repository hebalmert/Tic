using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tic.Shared.Entites;
using Tic.Shared.EntitiesSoft;
using Tic.Web.Data;
using Tic.Web.Helpers;
using X.PagedList;

namespace Tic.Web.Controllers.Entities
{
    [Authorize(Roles = "Admin")]

    public class CorporatesController : Controller
    {
        private readonly DataContext _context;
        private readonly IComboHelper _comboHelper;
        private readonly IFileStorage _fileStorage;
        private readonly INotyfService _notyfService;
        private readonly string _container;

        public CorporatesController(DataContext context,
            IComboHelper comboHelper, IFileStorage fileStorage, INotyfService notyfService)
        {
            _context = context;
            _comboHelper = comboHelper;
            _fileStorage = fileStorage;
            _notyfService = notyfService;
            _container = "wwwroot\\Images\\ImgCorporate";
        }


        [HttpPost]
        public JsonResult Search(string Prefix)
        {
            var datoCorp = (from corporacion in _context.Corporates
                            where corporacion.Name.ToLower().Contains(Prefix.ToLower())
                            select new
                            {
                                label = corporacion.Name,
                                val = corporacion.CorporateId
                            }).ToList();

            return Json(datoCorp);
        }

        // GET: Corporates
        public async Task<IActionResult> Index(int? buscarId, int? page)
        {
            if (buscarId != null)
            {
                var dataContext = _context.Corporates
                    .Include(c => c.SoftPlan)
                    .Include(c => c.Country)
                    .ThenInclude(c => c!.States!)
                    .ThenInclude(c => c!.Cities)
                    .Where(c => c.CorporateId == buscarId);
                return View(await dataContext.OrderBy(c => c.Name).ToPagedListAsync(page ?? 1, 25));
            }
            else
            {

                var dataContext = _context.Corporates
                    .Include(c => c.SoftPlan);
                return View(await dataContext.OrderBy(c => c.Name).ToPagedListAsync(page ?? 1, 25));
            }
        }

        // GET: Corporates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Corporates == null)
            {
                return NotFound();
            }

            var corporate = await _context.Corporates
                .Include(c => c.Country)
                .ThenInclude(c => c!.States!)
                .ThenInclude(c => c!.Cities)
                .Include(c => c.SoftPlan)
                .FirstOrDefaultAsync(m => m.CorporateId == id);
            if (corporate == null)
            {
                return NotFound();
            }

            return View(corporate);
        }

        // GET: Corporates/Create
        public IActionResult Create()
        {
            Corporate modelo = new()
            {
                Softplans = _comboHelper.GetComboSoftPlan(),
                ListCountry = _comboHelper.GetComboCountry(),
                ToStar = DateTime.Now,
                ToEnd = DateTime.Now,
                Activo = true
            };
            return View(modelo);
        }

        // POST: Corporates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Corporate corporate)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Para cualquier fallo en el Guardado de alguna Tabla
                    using var transaction = _context.Database.BeginTransaction();

                    if (corporate.ImageFile != null)
                    {
                        corporate.ImageId = await _fileStorage.UploadImage(corporate.ImageFile, ".jpg", _container, corporate.ImageId);
                    }

                    _context.Add(corporate);
                    await _context.SaveChangesAsync();

                    //Creamos la tabla de Registros con todo en Cero
                    Register register = new()
                    {
                        CorporateId = corporate.CorporateId,
                        OrderTickets = 0,
                        Tickets = 0,
                        Sells = 0,
                        SellCachier = 0,
                        PorcentCacheir = 0,
                        PayPorcentCacheir = 0
                    };
                    _context.Add(register);
                    await _context.SaveChangesAsync();
                    //Fin Creacion de Register

                    //Grabado de Todo en DB
                    await transaction.CommitAsync();

                    _notyfService.Success("El Regitro se Guardado Con Exito -  Notificacion");
                    return RedirectToAction(nameof(Details), new { id = corporate.CorporateId });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Existe un Registro con el Mismo Nombre");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            corporate.Softplans = _comboHelper.GetComboSoftPlan();
            corporate.ListCountry = _comboHelper.GetComboCountry();
            corporate.ListState = _comboHelper.GetComboState();
            corporate.ListCities = _comboHelper.GetComboCity();

            return View(corporate);
        }

        // GET: Corporates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Corporates == null)
            {
                return NotFound();
            }

            var corporate = await _context.Corporates.FindAsync(id);
            if (corporate == null)
            {
                return NotFound();
            }

            corporate.Softplans = _comboHelper.GetComboSoftPlan();
            corporate.ListCountry = _comboHelper.GetComboCountry();
            corporate.ListState = _comboHelper.GetComboState();
            corporate.ListCities = _comboHelper.GetComboCity();

            return View(corporate);
        }

        // POST: Corporates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Corporate corporate)
        {
            if (id != corporate.CorporateId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    if (corporate.ImageFile != null)
                    {
                        corporate.ImageId = await _fileStorage.UploadImage(corporate.ImageFile, ".jpg", _container, corporate.ImageId);
                    }

                    _context.Update(corporate);
                    await _context.SaveChangesAsync();

                    _notyfService.Success("El Regitro se ha Actualizado con Exito -  Notificacion");
                    return RedirectToAction("Details", new { id = corporate.CorporateId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CorporateExists(corporate.CorporateId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            corporate.Softplans = _comboHelper.GetComboSoftPlan();
            corporate.ListCountry = _comboHelper.GetComboCountry();
            corporate.ListState = _comboHelper.GetComboState();
            corporate.ListCities = _comboHelper.GetComboCity();

            return View(corporate);
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
                var DataRemove = await _context.Corporates
                    .FirstOrDefaultAsync(m => m.CorporateId == id);
                if (DataRemove == null)
                {
                    return NotFound();
                }

                _context.Corporates.Remove(DataRemove);
                await _context.SaveChangesAsync();

                if (DataRemove.ImageId != null)
                {
                    var response = _fileStorage.DeleteImage(_container, DataRemove.ImageId);
                    if (response != true)
                    {
                        return NotFound();
                    }
                }

                _notyfService.Custom("El Regitro se ha Eliminado Con Exito -  Notificacion", 5, "#D90000", "fa fa-trash");
                return RedirectToAction(nameof(Index));

            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("REFERENCE"))
                {
                    ModelState.AddModelError(string.Empty, "Existe un Registro Relacionado.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> GetNewDate(int IdSoftPlan, DateTime finicio)
        {
            var meses = await _context.SoftPlans.FindAsync(IdSoftPlan);
            DateTime nuevafecha = finicio.AddMonths(meses!.TimeMonth);
            var ndate = nuevafecha.ToString("yyyy-MM-dd");
            return Json(new { ndate });
        }

        [HttpPost]
        public JsonResult GetState(int IdCountry)
        {
            var dato = _context.States.Where(c => c.CountryId == IdCountry).OrderBy(c => c.Name);
            if (dato == null)
            {
                return null!;
            }

            return Json(dato.Select(x => new SelectListItem(x.Name, x.StateId.ToString())));
        }

        [HttpPost]
        public JsonResult GetCity(int Idstate)
        {
            var dato = _context.Cities.Where(c => c.StateId == Idstate).OrderBy(c => c.Name);
            if (dato == null)
            {
                return null!;
            }

            return Json(dato.Select(x => new SelectListItem(x.Name, x.StateId.ToString())));
        }

        private bool CorporateExists(int id)
        {
            return _context.Corporates.Any(e => e.CorporateId == id);
        }
    }
}
