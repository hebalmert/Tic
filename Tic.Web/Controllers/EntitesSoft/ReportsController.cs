using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tic.Shared.ReportesDTOs;
using Tic.Web.Data;
using Tic.Web.Helpers;

namespace Tic.Web.Controllers.EntitesSoft
{
    [Authorize(Roles = "User, Cachier")]
    public class ReportsController : Controller
    {
        private readonly DataContext _context;
        private readonly INotyfService _notyfService;
        private readonly IComboHelper _comboHelper;

        public ReportsController(DataContext context, INotyfService notyfService, IComboHelper comboHelper)
        {
            _context = context;
            _notyfService = notyfService;
            _comboHelper = comboHelper;
        }

        [Authorize(Roles = "Cachier")]
        // GET: Taxes/Create
        public IActionResult RepCajero()
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var cajero = _context.Cachiers.FirstOrDefault(x => x.UserName == user.UserName);

            RepCachierDTOs modelo = new()
            {
                CorporateId = Convert.ToInt32(user.CorporateId),
                DateInicio = DateTime.Today,
                DateFin = DateTime.Today,
                CachierId = cajero!.CachierId
            };

            return View(modelo);
        }

        [Authorize(Roles = "Cachier")]
        // Post: RepCachierDate   Reporte Index de Ventas por Cajero por dia, monto total
        public async Task<IActionResult> RepCajeroDate(RepCachierDTOs modelo)
        {
            if (modelo == null)
            {
                return RedirectToAction("RepCachier", "Reports");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }


            var dataContext = _context.SellOneCachiers
                .Include(s => s.CachierPorcents)
                .Include(s => s.Cachier)
                .Include(s => s.PlanCategory)
                .Include(s => s.Corporate)
                .Include(s => s.OrderTicketDetail)
                .Include(s => s.Plan)
                .Include(s => s.Server)
                .Where(c => c.CorporateId == user.CorporateId &&
                c.Date >= modelo.DateInicio &&
                c.Date <= modelo.DateFin
                && c.Anulada == false && c.CachierId == modelo.CachierId);


            return View(await dataContext.OrderBy(o => o.SellControl).ToListAsync());
        }

        // Post: RepCachierDate   Reporte Index de Ventas por Cajero por dia, monto total
        public async Task<IActionResult> RepCachierDate(RepCachierDTOs modelo)
        {
            if (modelo == null)
            {
                return RedirectToAction("RepCachier", "Reports");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }


            var dataContext = _context.SellOneCachiers
                .Include(s => s.Cachier)
                .Include(s => s.PlanCategory)
                .Include(s => s.Corporate)
                .Include(s => s.OrderTicketDetail)
                .Include(s => s.Plan)
                .Include(s => s.Server)
                .Where(c => c.CorporateId == user.CorporateId &&
                c.Date >= modelo.DateInicio &&
                c.Date <= modelo.DateFin
                && c.Anulada == false && c.CachierId == modelo.CachierId);


            return View(await dataContext.OrderBy(o => o.SellControl).ToListAsync());
        }

        // Post: RepCachierDate   Reporte Index de Ventas por Cajero por dia, monto total
        public async Task<IActionResult> RepCachierTickets(RepCachierDTOs modelo)
        {
            if (modelo == null)
            {
                return RedirectToAction("RepCachierT", "Reports");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }


            var dataContext = _context.SellOneCachiers
                .Include(s => s.Cachier)
                .Include(s => s.PlanCategory)
                .Include(s => s.Corporate)
                .Include(s => s.OrderTicketDetail)
                .Include(s => s.Plan)
                .Include(s => s.Server)
                .Where(c => c.CorporateId == user.CorporateId &&
                c.Date >= modelo.DateInicio &&
                c.Date <= modelo.DateFin
                && c.Anulada == false && c.CachierId == modelo.CachierId);


            return View(await dataContext.OrderBy(o => o.SellControl).ToListAsync());
        }

        // GET: Taxes/Create
        public IActionResult RepCachier()
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            RepCachierDTOs modelo = new()
            {
                CorporateId = Convert.ToInt32(user.CorporateId),
                DateInicio = DateTime.Today,
                DateFin = DateTime.Today
            };

            ViewData["CachierId"] = new SelectList(_comboHelper.GetComboCachier(modelo.CorporateId), "CachierId", "FullName");
            return View(modelo);
        }

        // GET: Taxes/Create
        public IActionResult RepCachierT()
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            RepCachierDTOs modelo = new()
            {
                CorporateId = Convert.ToInt32(user.CorporateId),
                DateInicio = DateTime.Today,
                DateFin = DateTime.Today
            };

            ViewData["CachierId"] = new SelectList(_comboHelper.GetComboCachier(modelo.CorporateId), "CachierId", "FullName");
            return View(modelo);
        }
    }
}
