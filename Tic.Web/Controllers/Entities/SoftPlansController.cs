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
    public class SoftPlansController : Controller
    {
        private readonly DataContext _context;
        private readonly INotyfService _notyfService;

        public SoftPlansController(DataContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        [HttpPost]
        public JsonResult Search(string Prefix)
        {
            var datoMag = (from modelo in _context.SoftPlans
                           where modelo.Name.ToLower().Contains(Prefix.ToLower())
                           select new
                           {
                               label = modelo.Name,
                               val = modelo.SoftPlanId
                           }).ToList();

            return Json(datoMag);

        }


        // GET: SoftPlans
        public async Task<IActionResult> Index(int? buscarId, int? page)
        {
            if (buscarId != null)
            {
                return View(await _context.SoftPlans.Where(c => c.SoftPlanId == buscarId).OrderBy(o => o.TimeMonth).ToPagedListAsync(page ?? 1, 25));
            }
            else
            {
                return View(await _context.SoftPlans.OrderBy(o => o.TimeMonth).ToPagedListAsync(page ?? 1, 25));
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SoftPlans == null)
            {
                return NotFound();
            }

            var softPlan = await _context.SoftPlans
                .FirstOrDefaultAsync(m => m.SoftPlanId == id);
            if (softPlan == null)
            {
                return NotFound();
            }

            return View(softPlan);
        }

        // GET: SoftPlans/Create
        public IActionResult Create()
        {
            SoftPlan modelo = new()
            {
                Activo = true
            };

            return View(modelo);
        }

        // POST: SoftPlans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SoftPlan softPlan)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(softPlan);
                    await _context.SaveChangesAsync();

                    _notyfService.Success("El Regitro se Guardado Con Exito -  Notificacion");
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

            return View(softPlan);
        }

        // GET: SoftPlans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SoftPlans == null)
            {
                return NotFound();
            }

            var softPlan = await _context.SoftPlans.FindAsync(id);
            if (softPlan == null)
            {
                return NotFound();
            }
            return View(softPlan);
        }

        // POST: SoftPlans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SoftPlan softPlan)
        {
            if (id != softPlan.SoftPlanId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(softPlan);
                    await _context.SaveChangesAsync();
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
            return View(softPlan);
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
                var dato = await _context.SoftPlans.FirstOrDefaultAsync(m => m.SoftPlanId == id);
                if (dato == null)
                {
                    return NotFound();
                }

                _context.SoftPlans.Remove(dato);
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

        private bool SoftPlanExists(int id)
        {
            return _context.SoftPlans.Any(e => e.SoftPlanId == id);
        }
    }
}
