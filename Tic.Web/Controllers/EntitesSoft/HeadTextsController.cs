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
    public class HeadTextsController : Controller
    {
        private readonly DataContext _context;
        private readonly INotyfService _notyfService;
        private readonly IComboHelper _comboHelper;

        public HeadTextsController(DataContext context, INotyfService notyfService, IComboHelper comboHelper)
        {
            _context = context;
            _notyfService = notyfService;
            _comboHelper = comboHelper;
        }

        // GET: HeadTexts
        public async Task<IActionResult> Index(int? page)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var dataContext = _context.HeadTexts.
                Include(h => h.Corporate)
                .Where(c => c.CorporateId == user.CorporateId);

            return View(await dataContext.ToPagedListAsync(page ?? 1, 20));
        }

        // GET: HeadTexts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.HeadTexts == null)
            {
                return NotFound();
            }

            var headText = await _context.HeadTexts
                .Include(h => h.Corporate)
                .FirstOrDefaultAsync(m => m.HeadTextId == id);
            if (headText == null)
            {
                return NotFound();
            }

            return View(headText);
        }

        // GET: HeadTexts/Create
        public IActionResult Create()
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            HeadText modelo = new()
            {
                CorporateId = Convert.ToInt32(user.CorporateId)
            };

            return View(modelo);
        }

        // POST: HeadTexts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HeadText headText)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(headText);
                    await _context.SaveChangesAsync();

                    _notyfService.Success("El Regitro se Guardado Con Exito -  Notificacion");
                    return RedirectToAction("Details", new { id = headText.HeadTextId });
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

            return View(headText);
        }

        // GET: HeadTexts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            {
                if (id == null || _context.HeadTexts == null)
                {
                    return NotFound();
                }

                var headText = await _context.HeadTexts.FindAsync(id);
                if (headText == null)
                {
                    return NotFound();
                }

                return View(headText);
            }
        }

        // POST: HeadTexts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HeadText headText)
        {
            if (id != headText.HeadTextId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(headText);
                    await _context.SaveChangesAsync();

                    _notyfService.Success("El Regitro se Actualizado Con Exito -  Notificacion");
                    return RedirectToAction("Details", new { id = headText.HeadTextId });
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

            return View(headText);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var dato = await _context.HeadTexts.FirstOrDefaultAsync(m => m.HeadTextId == id);
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

        private bool HeadTextExists(int id)
        {
            return _context.HeadTexts.Any(e => e.HeadTextId == id);
        }
    }
}
