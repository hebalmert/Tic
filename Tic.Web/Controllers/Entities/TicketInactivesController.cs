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
    public class TicketInactivesController : Controller
    {
        private readonly DataContext _context;
        private readonly INotyfService _notyfService;

        public TicketInactivesController(DataContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: TicketInactives
        public async Task<IActionResult> Index(int? page)
        {
            return View(await _context.TicketInactives.OrderBy(c => c.Orden).ToPagedListAsync(page ?? 1, 25));
        }

        // GET: TicketInactives/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TicketInactives == null)
            {
                return NotFound();
            }

            var ticketInactive = await _context.TicketInactives
                .FirstOrDefaultAsync(m => m.TicketInactiveId == id);
            if (ticketInactive == null)
            {
                return NotFound();
            }

            return View(ticketInactive);
        }

        // GET: TicketInactives/Create
        public IActionResult Create()
        {
            TicketInactive modelo = new()
            {
                Activo = true
            };
            return View(modelo);
        }

        // POST: TicketInactives/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketInactive ticketInactive)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(ticketInactive);
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
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(ticketInactive);
        }

        // GET: TicketInactives/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TicketInactives == null)
            {
                return NotFound();
            }

            var ticketInactive = await _context.TicketInactives.FindAsync(id);
            if (ticketInactive == null)
            {
                return NotFound();
            }
            return View(ticketInactive);
        }

        // POST: TicketInactives/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TicketInactive ticketInactive)
        {
            if (id != ticketInactive.TicketInactiveId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticketInactive);
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

            return View(ticketInactive);
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
                var DataRemove = await _context.TicketInactives
                    .FirstOrDefaultAsync(m => m.TicketInactiveId == id);
                if (DataRemove == null)
                {
                    return NotFound();
                }

                _context.Remove(DataRemove);
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

            return RedirectToAction(nameof(Index));
        }

        private bool TicketInactiveExists(int id)
        {
            return _context.TicketInactives.Any(e => e.TicketInactiveId == id);
        }
    }
}
