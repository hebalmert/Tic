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
    public class TicketTimesController : Controller
    {
        private readonly DataContext _context;
        private readonly INotyfService _notyfService;

        public TicketTimesController(DataContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: TicketTimes
        public async Task<IActionResult> Index(int? page)
        {
            return View(await _context.TicketTimes.OrderBy(c => c.Orden).ToPagedListAsync(page ?? 1, 10));
        }

        // GET: TicketTimes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TicketTimes == null)
            {
                return NotFound();
            }

            var ticketTime = await _context.TicketTimes
                .FirstOrDefaultAsync(m => m.TicketTimeId == id);
            if (ticketTime == null)
            {
                return NotFound();
            }

            return View(ticketTime);
        }

        // GET: TicketTimes/Create
        public IActionResult Create()
        {
            TicketTime modelo = new()
            {
                Activo = true
            };
            return View(modelo);
        }

        // POST: TicketTimes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketTime ticketTime)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(ticketTime);
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

            return View(ticketTime);
        }

        // GET: TicketTimes/Edit/5
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

        // POST: TicketTimes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TicketTime ticketTime)
        {
            if (id != ticketTime.TicketTimeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticketTime);
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
            return View(ticketTime);
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
                var dato = await _context.TicketTimes.FirstOrDefaultAsync(m => m.TicketTimeId == id);
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

        private bool TicketTimeExists(int id)
        {
            return _context.TicketTimes.Any(e => e.TicketTimeId == id);
        }
    }
}
