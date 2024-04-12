using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tic.Shared.Entites;
using Tic.Web.Data;
using X.PagedList;

namespace Tic.Web.Controllers.Entities
{
    [Authorize(Roles = "Admin")]
    public class CountriesController : Controller
    {
        private readonly DataContext _context;
        private readonly INotyfService _notyfService;

        public CountriesController(DataContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        [HttpPost]
        public JsonResult Search(string Prefix)
        {
            var datoMag = (from modelo in _context.Countries
                           where modelo.Name.Contains(Prefix)
                           select new
                           {
                               label = modelo.Name,
                               val = modelo.CountryId
                           }).ToList();

            return Json(datoMag);

        }

        // GET: Countries
        public async Task<IActionResult> Index(int? buscarId, int? page)
        {
            if (buscarId != null)
            {
                return View(await _context.Countries
                    .Include(c => c.States)
                    .Where(c => c.CountryId == buscarId).OrderBy(o => o.Name).ToPagedListAsync(page ?? 1, 25));
            }
            else
            {
                return View(await _context.Countries
                    .Include(o => o.States)
                    .OrderBy(o => o.Name).ToPagedListAsync(page ?? 1, 25));
            }
        }

        // GET: Countries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .Include(m => m.States!)
                .ThenInclude(m => m.Cities)
                .FirstOrDefaultAsync(m => m.CountryId == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // GET: Countries/Details/5
        public async Task<IActionResult> Details2(int? id)
        {
            if (id == null || _context.States == null)
            {
                return NotFound();
            }

            var modelo = await _context.States
                .Include(m => m.Cities)
                .FirstOrDefaultAsync(m => m.StateId == id);
            if (modelo == null)
            {
                return NotFound();
            }

            return View(modelo);
        }

        // GET: Countries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Countries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Country country)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(country);
                    await _context.SaveChangesAsync();

                    _notyfService.Success("El Regitro se ha Creado Con Exito -  Notificacion");

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
            return View(country);
        }

        // GET: Countries/Create
        public IActionResult CreateState(int countryid)
        {
            State modelo = new()
            {
                CountryId = countryid
            };

            return View(modelo);
        }



        // POST: Countries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateState(State state)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(state);
                    await _context.SaveChangesAsync();

                    _notyfService.Success("El Regitro se ha Creado Con Exito -  Notificacion");

                    return RedirectToAction(nameof(Details), new { id = state.CountryId });
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

            return View(state);
        }

        // GET: Countries/Create
        public IActionResult CreateCity(int stateid)
        {
            City modelo = new()
            {
                StateId = stateid
            };

            return View(modelo);
        }

        // POST: Countries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCity(City city)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(city);
                    await _context.SaveChangesAsync();

                    _notyfService.Success("El Regitro se ha Creado Con Exito -  Notificacion");

                    return RedirectToAction(nameof(Details2), new { id = city.StateId });
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

            return View(city);
        }

        // GET: Countries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Country country)
        {
            if (id != country.CountryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(country);
                    await _context.SaveChangesAsync();

                    _notyfService.Success("El Regitro se ha Editado Con Exito -  Notificacion");

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
            return View(country);
        }

        // GET: Countries/Edit/5
        public async Task<IActionResult> EditState(int? id)
        {
            if (id == null || _context.States == null)
            {
                return NotFound();
            }

            var modelo = await _context.States.FindAsync(id);
            if (modelo == null)
            {
                return NotFound();
            }
            return View(modelo);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditState(int id, State state)
        {
            if (id != state.StateId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(state);
                    await _context.SaveChangesAsync();

                    _notyfService.Success("El Regitro se ha Editado Con Exito -  Notificacion");

                    return RedirectToAction(nameof(Details), new { id = state.CountryId });
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
            return View(state);
        }

        // GET: Countries/Edit/5
        public async Task<IActionResult> EditCity(int? id)
        {
            if (id == null || _context.Cities == null)
            {
                return NotFound();
            }

            var modelo = await _context.Cities.FindAsync(id);
            if (modelo == null)
            {
                return NotFound();
            }
            return View(modelo);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCity(int id, City city)
        {
            if (id != city.CityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(city);
                    await _context.SaveChangesAsync();

                    _notyfService.Success("El Regitro se ha Editado Con Exito -  Notificacion");

                    return RedirectToAction(nameof(Details2), new { id = city.CityId });
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
            return View(city);
        }

        // GET: Countries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var dato = await _context.Countries.FirstOrDefaultAsync(m => m.CountryId == id);
                if (dato == null)
                {
                    return NotFound();
                }

                _context.Countries.Remove(dato);
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

        // Post: SpeedDowns/Delete/5
        public async Task<IActionResult> DeleteState(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var dato = await _context.States.FirstOrDefaultAsync(m => m.StateId == id);
                if (dato == null)
                {
                    return NotFound();
                }

                _context.States.Remove(dato);
                await _context.SaveChangesAsync();

                _notyfService.Custom("El Regitro se ha Eliminado Con Exito -  Notificacion", 5, "#D90000", "fa fa-trash");

                return RedirectToAction(nameof(Details), new { id = dato.CountryId });

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

        // Post: SpeedDowns/Delete/5
        public async Task<IActionResult> DeleteCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var dato = await _context.Cities.FirstOrDefaultAsync(m => m.CityId == id);
                if (dato == null)
                {
                    return NotFound();
                }

                _context.Cities.Remove(dato);
                await _context.SaveChangesAsync();

                _notyfService.Custom("El Regitro se ha Eliminado Con Exito -  Notificacion", 5, "#D90000", "fa fa-trash");

                return RedirectToAction(nameof(Details2), new { id = dato.StateId });

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

        private bool CountryExists(int id)
        {
            return (_context.Countries?.Any(e => e.CountryId == id)).GetValueOrDefault();
        }
    }
}
