using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tic.Shared.ReportesDTOs;
using Tic.Web.Data;
using Tic.Web.Helpers;

namespace Tic.Web.Controllers.EntitiesSoft
{
    [Authorize(Roles = "User")]

    public class StockTicketsController : Controller
    {
        private readonly DataContext _context;
        private readonly INotyfService _notyfService;
        private readonly IComboHelper _comboHelper;

        public StockTicketsController(DataContext context, INotyfService notyfService,
            IComboHelper comboHelper)
        {
            _context = context;
            _notyfService = notyfService;
            _comboHelper = comboHelper;
        }

        // GET: Servers/Create
        public IActionResult TicketsPlan()
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            StockPlans modelo = new()
            {
                CorporateId = Convert.ToInt32(user.CorporateId)
            };

            modelo.CategoryList = _comboHelper.GetComboCatPlan(modelo.CorporateId);

            return View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TicketsPlan(StockPlans modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return RedirectToAction(nameof(IndexTicketPlan), modelo);

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

        // GET: Servers
        public async Task<IActionResult> IndexTicketPlan(StockPlans modelo)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var datos = await _context.OrderTicketDetails
                .Include(x => x.OrderTickets!.Plan)
                .Where(x => x.OrderTickets!.PlanId == modelo.PlanId).ToListAsync();

            var total = datos.Count();

            return View(datos.OrderByDescending(x => x.DateCreado));
        }

        // GET: Servers/Create
        public IActionResult TicketsPlanServer()
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            StockPlansServer modelo = new()
            {
                CorporateId = Convert.ToInt32(user.CorporateId)
            };

            modelo.CategoryList = _comboHelper.GetComboCatPlan(modelo.CorporateId);
            modelo.ServerList = _comboHelper.GetComboServerActivos(modelo.CorporateId);

            return View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TicketsPlanServer(StockPlansServer modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return RedirectToAction(nameof(IndexTicketPlanServer), modelo);

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

        // GET: Servers
        public async Task<IActionResult> IndexTicketPlanServer(StockPlansServer modelo)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var datos = await _context.OrderTicketDetails
                .Include(x => x.OrderTickets!.Plan)
                .Include(x => x.OrderTickets!.Server)
                .Where(x => x.OrderTickets!.PlanId == modelo.PlanId && x.ServerId == modelo.ServerId).ToListAsync();

            var total = datos.Count();

            return View(datos.OrderByDescending(x => x.DateCreado));
        }

        public JsonResult GetPlanes(int categoryId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                return null!;
            }
            var data = _context.Plans
                .Where(c => c.PlanCategoryId == categoryId && c.Active == true && c.CorporateId == user.CorporateId).ToList();
            if (data == null)
            {
                return null!;
            }

            return Json(data.OrderBy(c => c.PlanName));
        }
    }
}
