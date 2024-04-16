using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tic.Shared.EntitiesSoft;
using Tic.Web.Data;
using Tic.Web.Helpers;
using X.PagedList;

namespace Spi.Web.Controllers.EntitiesSoft
{
    [Authorize(Roles = "User")]

    public class MarksController : Controller
    {
        private readonly DataContext _context;
        private readonly INotyfService _notyfService;
        private readonly IComboHelper _comboHelper;

        public MarksController(DataContext context, INotyfService notyfService,
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
            var datoMag = (from modelo in _context.Marks
                           where modelo.MarkName.ToLower().Contains(Prefix.ToLower()) && modelo.CorporateId == user!.CorporateId
                           select new
                           {
                               label = modelo.MarkName,
                               val = modelo.MarkId
                           }).ToList();

            return Json(datoMag);

        }


        // GET: Marks
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
                return View(await _context.Marks
                    .Include(z => z.MarkModels)
                    .Where(c => c.MarkId == buscarId && c.CorporateId == user.CorporateId).OrderBy(o => o.MarkName)
                    .ToPagedListAsync(page ?? 1, 25));
            }
            else
            {
                return View(await _context.Marks
                    .Include(z => z.MarkModels)
                    .Where(c => c.CorporateId == user.CorporateId)
                    .OrderBy(o => o.MarkName).ToPagedListAsync(page ?? 1, 25));
            }
        }

        // GET: Marks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Marks == null)
            {
                return NotFound();
            }

            var mark = await _context.Marks
                .Include(m => m.Corporate)
                .Include(z => z.MarkModels)
                .FirstOrDefaultAsync(m => m.MarkId == id);
            if (mark == null)
            {
                return NotFound();
            }

            return View(mark);
        }

        // GET: Marks/Create
        public IActionResult Create()
        {
            var user = _context.Users.Include(u => u.Corporate).FirstOrDefault(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                _notyfService.Custom("Problemas de Autenticacion debe comprobar credenciales -  Notificacion", 5, "#D90000", "fa fa-trash");
                return RedirectToAction("Login", "Account");
            }

            Mark datos = new()
            {
                CorporateId = Convert.ToInt32(user.CorporateId),
                Active = true
            };


            return View(datos);
        }

        // POST: Marks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Mark modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(modelo);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("El Regitro se Guardado Con Exito -  Notificacion");

                    return RedirectToAction(nameof(Details), new { id = modelo.MarkId });
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

        // GET: Countries/Create
        public IActionResult CreateModel(int idMark)
        {
            var user = _context.Users.Include(u => u.Corporate).FirstOrDefault(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                _notyfService.Custom("Problemas de Autenticacion debe comprobar credenciales -  Notificacion", 5, "#D90000", "fa fa-trash");
                return RedirectToAction("Login", "Account");
            }

            var MarcaNombre = _context.Marks.Find(idMark);
            if (MarcaNombre == null)
            {
                _notyfService.Custom("Error al Conseguier la Marca -  Notificacion", 5, "#D90000", "fa fa-trash");
                return RedirectToAction(nameof(Index));
            }

            MarkModel modelo = new()
            {
                CorporateId = Convert.ToInt32(user.CorporateId),
                MarkId = idMark,
                MarkName = MarcaNombre!.MarkName,
                Active = true
            };

            return View(modelo);
        }

        // POST: Countries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateModel(MarkModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(modelo);
                    await _context.SaveChangesAsync();

                    _notyfService.Success("El Regitro se ha Creado Con Exito -  Notificacion");

                    return RedirectToAction(nameof(Details), new { id = modelo.MarkId });
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

        // GET: Marks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var user = _context.Users.Include(u => u.Corporate).FirstOrDefault(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                _notyfService.Custom("Problemas de Autenticacion debe comprobar credenciales -  Notificacion", 5, "#D90000", "fa fa-trash");
                return RedirectToAction("Login", "Account");
            }

            if (id == null || _context.Marks == null)
            {
                return NotFound();
            }

            var mark = await _context.Marks.FindAsync(id);
            if (mark == null)
            {
                return NotFound();
            }

            return View(mark);
        }

        // POST: Marks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Mark modelo)
        {
            if (id != modelo.MarkId)
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

        // GET: Marks/Edit/5
        public async Task<IActionResult> EditModel(int? id)
        {
            var user = _context.Users.Include(u => u.Corporate).FirstOrDefault(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                _notyfService.Custom("Problemas de Autenticacion debe comprobar credenciales -  Notificacion", 5, "#D90000", "fa fa-trash");
                return RedirectToAction("Login", "Account");
            }

            if (id == null || _context.Marks == null)
            {
                return NotFound();
            }

            var modelo = await _context.MarkModels.FindAsync(id);
            if (modelo == null)
            {
                return NotFound();
            }

            var MarcaNombre = _context.Marks.Find(modelo.MarkId);
            if (MarcaNombre == null)
            {
                _notyfService.Custom("Error al Conseguier la Marca -  Notificacion", 5, "#D90000", "fa fa-trash");
                return RedirectToAction(nameof(Index));
            }
            modelo.MarkName = MarcaNombre!.MarkName;
            return View(modelo);
        }

        // POST: Marks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditModel(int id, MarkModel modelo)
        {
            if (id != modelo.MarkModelId)
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

                    return RedirectToAction(nameof(Details), new { id = modelo.MarkId });
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
                var dato = await _context.Marks.FirstOrDefaultAsync(m => m.MarkId == id);
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

        // Post: SpeedDowns/Delete/5
        public async Task<IActionResult> DeleteModel(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var dato = await _context.MarkModels.FirstOrDefaultAsync(m => m.MarkModelId == id);
                if (dato == null)
                {
                    return NotFound();
                }

                _context.Remove(dato);
                await _context.SaveChangesAsync();
                _notyfService.Custom("El Regitro se ha Eliminado Con Exito -  Notificacion", 5, "#D90000", "fa fa-trash");

                return RedirectToAction(nameof(Details), new { id = dato.MarkId });

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

        private bool MarkExists(int id)
        {
            return (_context.Marks?.Any(e => e.MarkId == id)).GetValueOrDefault();
        }
    }
}
