using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tic.Shared.EntitiesSoft;
using Tic.Web.Data;
using Tic.Web.Helpers;
using X.PagedList;

namespace Tic.Web.Controllers.EntitiesSoft
{
    [Authorize(Roles = "User, UserAux")]
    public class PlanCategoriesController : Controller
    {
        private readonly DataContext _context;
        private readonly INotyfService _notyfService;
        private readonly IComboHelper _comboHelper;

        public PlanCategoriesController(DataContext context, INotyfService notyfService,
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
            var datoMag = (from modelo in _context.PlanCategories
                           where modelo.PlanCategoryName.Contains(Prefix) && modelo.CorporateId == user!.CorporateId
                           select new
                           {
                               label = modelo.PlanCategoryName,
                               val = modelo.PlanCategoryId
                           }).ToList();

            return Json(datoMag);

        }

        // GET: PlanCategories
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
                return View(await _context.PlanCategories
                    .Where(c => c.PlanCategoryId == buscarId && c.CorporateId == user.CorporateId).OrderBy(o => o.PlanCategoryName)
                    .ToPagedListAsync(page ?? 1, 25));
            }
            else
            {
                return View(await _context.PlanCategories
                    .Where(c => c.CorporateId == user.CorporateId)
                    .OrderBy(o => o.PlanCategoryName).ToPagedListAsync(page ?? 1, 25));
            }
        }

        // GET: PlanCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PlanCategories == null)
            {
                return NotFound();
            }

            var planCategory = await _context.PlanCategories
                .Include(p => p.Corporate)
                .FirstOrDefaultAsync(m => m.PlanCategoryId == id);
            if (planCategory == null)
            {
                return NotFound();
            }

            return View(planCategory);
        }

        // GET: PlanCategories/Create
        public IActionResult Create()
        {
            var user = _context.Users.Include(u => u.Corporate).FirstOrDefault(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                _notyfService.Custom("Problemas de Autenticacion debe comprobar credenciales -  Notificacion", 5, "#D90000", "fa fa-trash");
                return RedirectToAction("Login", "Account");
            }

            PlanCategory datos = new()
            {
                CorporateId = Convert.ToInt32(user.CorporateId),
                Active = true
            };

            return View(datos);
        }

        // POST: PlanCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlanCategory modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(modelo);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("El Regitro se Guardado Con Exito -  Notificacion");

                    return RedirectToAction(nameof(Details), new { id = modelo.PlanCategoryId});
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

        // GET: PlanCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var user = _context.Users.Include(u => u.Corporate).FirstOrDefault(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                _notyfService.Custom("Problemas de Autenticacion debe comprobar credenciales -  Notificacion", 5, "#D90000", "fa fa-trash");
                return RedirectToAction("Login", "Account");
            }

            if (id == null || _context.PlanCategories == null)
            {
                return NotFound();
            }

            var planCategory = await _context.PlanCategories.FindAsync(id);
            if (planCategory == null)
            {
                return NotFound();
            }
            ViewData["CorporateId"] = new SelectList(_context.Corporates, "CorporateId", "Address", planCategory.CorporateId);
            return View(planCategory);
        }

        // POST: PlanCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PlanCategory modelo)
        {
            if (id != modelo.PlanCategoryId)
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

                    return RedirectToAction(nameof(Details), new { id = modelo.PlanCategoryId });
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
                var dato = await _context.PlanCategories.FirstOrDefaultAsync(m => m.PlanCategoryId == id);
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

        private bool PlanCategoryExists(int id)
        {
            return (_context.PlanCategories?.Any(e => e.PlanCategoryId == id)).GetValueOrDefault();
        }
    }
}
