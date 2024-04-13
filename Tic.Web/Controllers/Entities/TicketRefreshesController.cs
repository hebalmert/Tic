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
    public class TicketRefreshesController : Controller
    {
        private readonly DataContext _context;
        private readonly INotyfService _notyfService;

        public TicketRefreshesController(DataContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: TicketRefreshes
        public async Task<IActionResult> Index(int? page)
        {
            return View(await _context.TicketRefreshes.OrderBy(c => c.Orden).ToPagedListAsync(page ?? 1, 10));
        }

        // GET: TicketRefreshes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TicketRefreshes == null)
            {
                return NotFound();
            }

            var ticketRefresh = await _context.TicketRefreshes
                .FirstOrDefaultAsync(m => m.TicketRefreshId == id);
            if (ticketRefresh == null)
            {
                return NotFound();
            }

            return View(ticketRefresh);
        }

        // GET: TicketRefreshes/Create
        public IActionResult Create()
        {
            TicketRefresh modelo = new()
            {
                Activo = true
            };
            return View(modelo);
        }

        // POST: TicketRefreshes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketRefresh ticketRefresh)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(ticketRefresh);
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

            return View(ticketRefresh);
        }

        // GET: TicketRefreshes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TicketRefreshes == null)
            {
                return NotFound();
            }

            var ticketRefresh = await _context.TicketRefreshes.FindAsync(id);
            if (ticketRefresh == null)
            {
                return NotFound();
            }
            return View(ticketRefresh);
        }

        // POST: TicketRefreshes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TicketRefresh ticketRefresh)
        {
            if (id != ticketRefresh.TicketRefreshId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticketRefresh);
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
            return View(ticketRefresh);
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
                var dato = await _context.TicketRefreshes.FirstOrDefaultAsync(m => m.TicketRefreshId == id);
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

        private bool TicketRefreshExists(int id)
        {
            return _context.TicketRefreshes.Any(e => e.TicketRefreshId == id);
        }
    }
}
