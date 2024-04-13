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
    public class DocumentTypesController : Controller
    {
        private readonly DataContext _context;
        private readonly INotyfService _notyfService;

        public DocumentTypesController(DataContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        [HttpPost]
        public JsonResult Search(string Prefix)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity!.Name);
            var datoMag = (from modelo in _context.DocumentTypes
                           where modelo.DocumentName.ToLower().Contains(Prefix.ToLower()) && modelo.CorporateId == user!.CorporateId
                           select new
                           {
                               label = modelo.DocumentName,
                               val = modelo.DocumentTypeId
                           }).ToList();

            return Json(datoMag);

        }

        // GET: DocumentTypes
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
                return View(await _context.DocumentTypes
                    .Where(c => c.DocumentTypeId == buscarId && c.CorporateId == user.CorporateId).OrderBy(o => o.DocumentName)
                    .ToPagedListAsync(page ?? 1, 25));
            }
            else
            {
                return View(await _context.DocumentTypes
                    .Where(c => c.CorporateId == user.CorporateId)
                    .OrderBy(o => o.DocumentName).ToPagedListAsync(page ?? 1, 25));
            }
        }

        // GET: DocumentTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DocumentTypes == null)
            {
                return NotFound();
            }

            var documentType = await _context.DocumentTypes
                .Include(d => d.Corporate)
                .FirstOrDefaultAsync(m => m.DocumentTypeId == id);
            if (documentType == null)
            {
                return NotFound();
            }

            return View(documentType);
        }

        // GET: DocumentTypes/Create
        public IActionResult Create()
        {
            var user = _context.Users.Include(u => u.Corporate).FirstOrDefault(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                _notyfService.Custom("Problemas de Autenticacion debe comprobar credenciales -  Notificacion", 5, "#D90000", "fa fa-trash");
                return RedirectToAction("Login", "Account");
            }

            DocumentType datos = new()
            {
                CorporateId = Convert.ToInt32(user.CorporateId),
                Active = true
            };

            return View(datos);
        }

        // POST: DocumentTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DocumentType modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(modelo);
                    await _context.SaveChangesAsync();

                    _notyfService.Success("El Regitro se Guardado Con Exito -  Notificacion");
                    return RedirectToAction(nameof(Details), new { id = modelo.DocumentTypeId });
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

        // GET: DocumentTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var user = _context.Users.Include(u => u.Corporate).FirstOrDefault(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                _notyfService.Custom("Problemas de Autenticacion debe comprobar credenciales -  Notificacion", 5, "#D90000", "fa fa-trash");
                return RedirectToAction("Login", "Account");
            }

            if (id == null || _context.DocumentTypes == null)
            {
                return NotFound();
            }

            var documentType = await _context.DocumentTypes.FindAsync(id);
            if (documentType == null)
            {
                return NotFound();
            }

            return View(documentType);
        }

        // POST: DocumentTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DocumentType modelo)
        {
            if (id != modelo.DocumentTypeId)
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
                    return RedirectToAction(nameof(Details), new { id = modelo.DocumentTypeId });
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

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var dato = await _context.DocumentTypes.FirstOrDefaultAsync(m => m.DocumentTypeId == id);
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

        private bool DocumentTypeExists(int id)
        {
            return _context.DocumentTypes.Any(e => e.DocumentTypeId == id);
        }
    }
}
