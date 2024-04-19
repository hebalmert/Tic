using AspNetCoreHero.ToastNotification.Abstractions;
using Tic.Shared.EntitiesSoft;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tic.Web.Data;
using Tic.Web.Helpers;
using X.PagedList;

namespace Tic.Web.Controllers.EntitesSoft
{
    [Authorize(Roles = "User")]
    public class SellOnesController : Controller
    {
        private readonly DataContext _context;
        private readonly INotyfService _notyfService;
        private readonly IComboHelper _comboHelper;

        public SellOnesController(DataContext context, INotyfService notyfService, IComboHelper comboHelper)
        {
            _context = context;
            _notyfService = notyfService;
            _comboHelper = comboHelper;
        }

        // GET: SellOnes
        public async Task<IActionResult> Index(int? page)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var dataContext = _context.SellOnes.Include(s => s.Corporate).Include(s => s.OrderTicketDetail).Include(s => s.Plan)
                .Include(s => s.PlanCategory).Include(s => s.Server).Where(x => x.CorporateId == user.CorporateId)
                .OrderByDescending(x => x.Date).ThenByDescending(x => x.SellControl)
                .ToPagedListAsync(page ?? 1, 25);

            return View(await dataContext);
        }

        // GET: SellOnes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sellOne = await _context.SellOnes
                .Include(s => s.Corporate)
                .Include(s => s.OrderTicketDetail)
                .Include(s => s.Plan)
                .Include(s => s.PlanCategory)
                .Include(s => s.Server)
                .FirstOrDefaultAsync(m => m.SellOneId == id);
            if (sellOne == null)
            {
                return NotFound();
            }

            return View(sellOne);
        }

        // GET: SellOnes/Create
        public IActionResult Create()
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var usuario = _context.Managers.FirstOrDefault(m => m.UserName == user.UserName);
            SellOne modelo = new()
            {
                CorporateId = Convert.ToInt32(user.CorporateId),
                Date = DateTime.Today,
                SellControl = 0
            };

            modelo.ListCategory = _comboHelper.GetComboCatPlan(modelo.CorporateId);

            return View(modelo);
        }

        // POST: SellOnes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SellOne modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Se realiza el proceso de auto RollBack para algun fallo de Guardadoplanes de internet
                    using var transaction = _context.Database.BeginTransaction();

                    //Tomamos el consecutivo de Orden de tickets desde Register
                    var NewReg = await _context.Registers.Where(c => c.CorporateId == modelo.CorporateId).FirstOrDefaultAsync();
                    if (NewReg == null)
                    {
                        _notyfService.Custom("Error en la Asignacion de Consecutivo -  Notificacion", 5, "#D90000", "fa fa-trash");
                        modelo.ListCategory = _comboHelper.GetComboCatPlan(modelo.CorporateId);
                        modelo.ListServer = _comboHelper.GetComboServerActivos(modelo.CorporateId);
                        modelo.ListPlan = _comboHelper.GetComboPlanOrdenes(modelo.CorporateId, modelo.ServerId, modelo.PlanCategoryId);
                        return View(modelo);
                    }
                    var SumReg = NewReg.Sells + 1;
                    NewReg.Sells = SumReg;
                    _context.Registers.Update(NewReg);
                    await _context.SaveChangesAsync();
                    //actualizamos el valor en Register

                    modelo.SellControl = SumReg;

                    var usuario = _context.Managers.FirstOrDefault(m => m.UserName == User.Identity!.Name);
                    if (usuario == null)
                    {
                        _notyfService.Custom("Error con el Tipo de Usuario -  Notificacion", 5, "#D90000", "fa fa-trash");
                        modelo.ListCategory = _comboHelper.GetComboCatPlan(modelo.CorporateId);
                        modelo.ListServer = _comboHelper.GetComboServerActivos(modelo.CorporateId);
                        modelo.ListPlan = _comboHelper.GetComboPlanOrdenes(modelo.CorporateId, modelo.ServerId, modelo.PlanCategoryId);
                        return View(modelo);
                    }
                    modelo.ManagerId = usuario.ManagerId;

                    //actualizamos informacion del OrderDetail, para que el Ticket quede vendido
                    var ticket = await _context.OrderTicketDetails.FindAsync(modelo.OrderTicketDetailId);
                    ticket!.Vendido = true;
                    ticket.DateVenta = DateTime.Now;
                    ticket.SellOne = true;
                    ticket.UserSystem = true;
                    ticket.ManagerId = usuario.ManagerId;
                    _context.OrderTicketDetails.Update(ticket);
                    await _context.SaveChangesAsync();
                    //fin

                    _context.Add(modelo);
                    await _context.SaveChangesAsync();

                    transaction.Commit();
                    //Se guardan todos los datos si todo esta successed.

                    _notyfService.Success("El Regitro se Guardado Con Exito -  Notificacion");
                    return RedirectToAction(nameof(Details), new { id = modelo.SellOneId });
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

            modelo.ListCategory = _comboHelper.GetComboCatPlan(modelo.CorporateId);
            modelo.ListServer = _comboHelper.GetComboServerActivos(modelo.CorporateId);
            modelo.ListPlan = _comboHelper.GetComboPlanOrdenes(modelo.CorporateId, modelo.ServerId, modelo.PlanCategoryId);
            return View(modelo);
        }

        // GET: SellOnes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sellOne = await _context.SellOnes.FindAsync(id);
            if (sellOne == null)
            {
                return NotFound();
            }
            ViewData["CorporateId"] = new SelectList(_context.Corporates, "CorporateId", "Address", sellOne.CorporateId);
            ViewData["OrderTicketDetailId"] = new SelectList(_context.OrderTicketDetails, "OrderTicketDetailId", "Clave", sellOne.OrderTicketDetailId);
            ViewData["PlanId"] = new SelectList(_context.Plans, "PlanId", "PlanName", sellOne.PlanId);
            ViewData["PlanCategoryId"] = new SelectList(_context.PlanCategories, "PlanCategoryId", "PlanCategoryName", sellOne.PlanCategoryId);
            ViewData["ServerId"] = new SelectList(_context.Servers, "ServerId", "Clave", sellOne.ServerId);
            return View(sellOne);
        }

        // POST: SellOnes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SellOne sellOne)
        {
            if (id != sellOne.SellOneId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sellOne);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SellOneExists(sellOne.SellOneId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CorporateId"] = new SelectList(_context.Corporates, "CorporateId", "Address", sellOne.CorporateId);
            ViewData["OrderTicketDetailId"] = new SelectList(_context.OrderTicketDetails, "OrderTicketDetailId", "Clave", sellOne.OrderTicketDetailId);
            ViewData["PlanId"] = new SelectList(_context.Plans, "PlanId", "PlanName", sellOne.PlanId);
            ViewData["PlanCategoryId"] = new SelectList(_context.PlanCategories, "PlanCategoryId", "PlanCategoryName", sellOne.PlanCategoryId);
            ViewData["ServerId"] = new SelectList(_context.Servers, "ServerId", "Clave", sellOne.ServerId);
            return View(sellOne);
        }

        // GET: SellOnes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sellOne = await _context.SellOnes
                .Include(s => s.Corporate)
                .Include(s => s.OrderTicketDetail)
                .Include(s => s.Plan)
                .Include(s => s.PlanCategory)
                .Include(s => s.Server)
                .FirstOrDefaultAsync(m => m.SellOneId == id);
            if (sellOne == null)
            {
                return NotFound();
            }

            return View(sellOne);
        }

        // POST: SellOnes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sellOne = await _context.SellOnes.FindAsync(id);
            if (sellOne != null)
            {
                _context.SellOnes.Remove(sellOne);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public JsonResult GetPrecio(int planId, int IdServer)
        {
            var data2 = _context.Plans
                .Include(p => p.Tax)
                .Where(p => p.PlanId == planId).FirstOrDefault();
            var dato = new
            {
                precio = data2!.Precio,
                rate = data2.Tax!.Rate
            };

            var data3 = _context.OrderTicketDetails
                .Include(c => c.OrderTickets)
                .Where(c => c.OrderTickets!.PlanId == planId && c.ServerId == IdServer
                && c.Vendido == false && c.Anulado == false).FirstOrDefault();

            return Json(dato);
        }

        public JsonResult GetPlan(int idCategory, int idServer)
        {
            var data = _context.Plans
                .Where(c => c.PlanCategoryId == idCategory && c.Active == true && c.ServerId == idServer)
                .ToList();

            return Json(data.Select(x => new SelectListItem(x.PlanName, x.PlanId.ToString())));
        }

        public JsonResult GetServidores(int idCorporate)
        {
            var servidor = _context.Servers
                .Where(c => c.Active == true && c.CorporateId == idCorporate).ToList();

            return Json(servidor.Select(x => new SelectListItem(x.ServerName, x.ServerId.ToString())));
        }

        public JsonResult GetPin(int IdPlan, int IdServer)
        {
            var data3 = _context.OrderTicketDetails
                .Include(c => c.OrderTickets)
                .Where(c => c.OrderTickets!.PlanId == IdPlan && c.ServerId == IdServer
                && c.Vendido == false && c.Anulado == false).FirstOrDefault();
            if (data3 == null)
            {
                return null!;
            }
            var data4 = _context.OrderTicketDetails
                .Include(c => c.OrderTickets)
                .Where(c => c.CorporateId == data3.CorporateId &&
                c.Vendido == false && c.Anulado == false &&
                c.ServerId == IdServer && c.OrderTickets!.PlanId == IdPlan)
                .ToList();


            var pin = data3.Usuario;
            var ordid = data3.OrderTicketDetailId;
            var stock = data4.Count;


            return Json(new { pin, ordid, stock });
        }

        private bool SellOneExists(int id)
        {
            return _context.SellOnes.Any(e => e.SellOneId == id);
        }
    }
}
