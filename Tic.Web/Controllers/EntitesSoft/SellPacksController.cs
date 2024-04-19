using AspNetCoreHero.ToastNotification.Abstractions;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Tic.Shared.EntitiesSoft;
using Tic.Web.Data;
using Tic.Web.Helpers;
using X.PagedList;

namespace Tic.Web.Controllers.EntitesSoft
{
    [Authorize(Roles = "User")]
    public class SellPacksController : Controller
    {
        private readonly DataContext _context;
        private readonly INotyfService _notyfService;
        private readonly IComboHelper _comboHelper;

        public SellPacksController(DataContext context, INotyfService notyfService, IComboHelper comboHelper)
        {
            _context = context;
            _notyfService = notyfService;
            _comboHelper = comboHelper;
        }

        // GET: SellPacks
        public async Task<IActionResult> Index(int? page)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var dataContext = _context.SellPacks
                .Include(s => s.PlanCategory)
                .Include(s => s.Corporate)
                .Include(s => s.Manager)
                .Include(s => s.Plan)
                .Include(s => s.Server)
                .Include(s => s.SellPackDetails)
                .Where(c => c.CorporateId == user.CorporateId);

            return View(await dataContext.OrderByDescending(x => x.Date).ThenByDescending(o => o.SellControl).ToPagedListAsync(page ?? 1, 20));
        }

        // GET: SellPacks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sellPack = await _context.SellPacks
                .Include(s => s.Corporate)
                .Include(s => s.Manager)
                .Include(s => s.Plan)
                .Include(s => s.PlanCategory)
                .Include(s => s.Server)
                .Include(s => s.SellPackDetails)!
                .ThenInclude(s => s.OrderTicketDetail)
                .FirstOrDefaultAsync(m => m.SellPackId == id);
            if (sellPack == null)
            {
                return NotFound();
            }

            return View(sellPack);
        }

        // GET: OrderTickets/Create
        public async Task<IActionResult> AddTickets(int idSellPack, decimal tt)
        {
            var Numeroregistros = Convert.ToInt32(tt);
            var DatosSellPask = await _context.SellPacks.FindAsync(idSellPack);
            var DatoTickets = await _context.OrderTicketDetails
                .Where(d => d.OrderTickets!.PlanId == DatosSellPask!.PlanId &&
                d.CorporateId == DatosSellPask.CorporateId &&
                d.ServerId == DatosSellPask.ServerId &&
                d.Vendido == false &&
                d.Anulado == false)
                .ToListAsync();

            if (DatoTickets.Count < tt)
            {
                _notyfService.Custom($"Los Tickets disponibles son:{DatoTickets.Count}, modifique la cantidad de Venta", 5, "#D90000", "fa fa-trash");
                return RedirectToAction(nameof(Details), new { id = idSellPack });
            }

            var result = DatoTickets.Take(Numeroregistros);
            var UserActivo = await _context.Managers.Where(m => m.UserName == User.Identity!.Name).FirstOrDefaultAsync();

            //tt es la cantidad de tickets que se vendieron
            //El For dara las vueltas segun la cantidad de Ticket vendidos
            //El Foreach accedera a la lista de Tickets disponibles en la red

            foreach (var item in result)
            {
                SellPackDetail sellPackDetail = new()
                {
                    CorporateId = item.CorporateId,
                    SellPackId = DatosSellPask!.SellPackId,
                    OrderTicketDetailId = item.OrderTicketDetailId
                };

                _context.SellPackDetails.Add(sellPackDetail);
                await _context.SaveChangesAsync();

                var DatoUpdate = await _context.OrderTicketDetails.FindAsync(item.OrderTicketDetailId);
                DatoUpdate!.Vendido = true;
                DatoUpdate.DateVenta = DateTime.Now;
                DatoUpdate.SellTotal = true;
                DatoUpdate.VentaId = DatosSellPask.SellPackId;
                DatoUpdate.UserSystem = true;
                DatoUpdate.ManagerId = UserActivo!.ManagerId;

                _context.OrderTicketDetails.Update(DatoUpdate);
                await _context.SaveChangesAsync();
            }

            _notyfService.Success("Se ha Creado con Exito la Venta -  Notificacion");
            return RedirectToAction(nameof(Details), new { id = idSellPack });
        }

        // GET: SellPacks/Create
        public IActionResult Create()
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var usuario = _context.Managers.FirstOrDefault(m => m.UserName == user.UserName);
            SellPack modelo = new()
            {
                CorporateId = Convert.ToInt32(user.CorporateId),
                Date = DateTime.Today,
                SellControl = 0,
                ManagerId = usuario!.ManagerId
            };

            modelo.ListCategory = _comboHelper.GetComboCatPlan(modelo.CorporateId);
            return View(modelo);
        }

        // POST: SellPacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SellPack modelo)
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

                    _context.Add(modelo);
                    await _context.SaveChangesAsync();

                    transaction.Commit();
                    //Se guardan todos los datos si todo esta successed.

                    _notyfService.Success("El Regitro se Guardado Con Exito -  Notificacion");
                    return RedirectToAction(nameof(Details), new { id = modelo.SellPackId });
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

        // GET: SellPacks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sellPack = await _context.SellPacks.FindAsync(id);
            if (sellPack == null)
            {
                return NotFound();
            }
            ViewData["CorporateId"] = new SelectList(_context.Corporates, "CorporateId", "Address", sellPack.CorporateId);
            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "Address", sellPack.ManagerId);
            ViewData["PlanId"] = new SelectList(_context.Plans, "PlanId", "PlanName", sellPack.PlanId);
            ViewData["PlanCategoryId"] = new SelectList(_context.PlanCategories, "PlanCategoryId", "PlanCategoryName", sellPack.PlanCategoryId);
            ViewData["ServerId"] = new SelectList(_context.Servers, "ServerId", "Clave", sellPack.ServerId);
            return View(sellPack);
        }

        // POST: SellPacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SellPack sellPack)
        {
            if (id != sellPack.SellPackId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sellPack);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SellPackExists(sellPack.SellPackId))
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
            ViewData["CorporateId"] = new SelectList(_context.Corporates, "CorporateId", "Address", sellPack.CorporateId);
            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "Address", sellPack.ManagerId);
            ViewData["PlanId"] = new SelectList(_context.Plans, "PlanId", "PlanName", sellPack.PlanId);
            ViewData["PlanCategoryId"] = new SelectList(_context.PlanCategories, "PlanCategoryId", "PlanCategoryName", sellPack.PlanCategoryId);
            ViewData["ServerId"] = new SelectList(_context.Servers, "ServerId", "Clave", sellPack.ServerId);
            return View(sellPack);
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
                var dato = await _context.SellPacks.FirstOrDefaultAsync(m => m.SellPackId == id);
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

        // Post: SellPacks/Details/5
        public async Task<IActionResult> ExportExcel(int? idSellPack, int? tt, int Sc)
        {
            if (idSellPack == null || tt == null)
            {
                return NotFound();
            }

            var sellPackItem = await _context.SellPackDetails
                .Include(c => c.SellPack)
                .ThenInclude(c => c.Plan)
                .Include(c => c.OrderTicketDetail)
                .Where(m => m.SellPackId == idSellPack).ToListAsync();
            if (sellPackItem.Count != tt || sellPackItem == null)
            {
                return NotFound();
            }

            var FileName = $"TicketOrden-#{Sc}-Fecha{DateTime.Today.ToString("dd-MMMM-yyyy")}.xlsx";

            //Se arma la DataTable y se carga sus filas  "TicketVenta es el nombre del Sheet"
            DataTable tablaExcel = new DataTable("TicketVenta");
            tablaExcel.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Venta"),
                new DataColumn("Fecha"),
                new DataColumn("Plan"),
                new DataColumn("Ticket")
            });

            foreach (var item in sellPackItem)
            {
                tablaExcel.Rows.Add(
                    item.SellPack!.SellControl,
                    item.SellPack.Date.ToString("dd-MMMM-yyyy"),
                    item.SellPack.Plan!.PlanName,
                    item.OrderTicketDetail!.Usuario
                    );
            }
            //fin de la creacion del datatable
            //se puede enviar string NombreArchivo y DataTable tablaExcel

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(tablaExcel);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        FileName);
                }
            }
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

        private bool SellPackExists(int id)
        {
            return _context.SellPacks.Any(e => e.SellPackId == id);
        }
    }
}
