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
    [Authorize(Roles = "Cachier")]
    public class SellOneCachiersController : Controller
    {
        private readonly DataContext _context;
        private readonly INotyfService _notyfService;
        private readonly IComboHelper _comboHelper;

        public SellOneCachiersController(DataContext context, INotyfService notyfService, IComboHelper comboHelper)
        {
            _context = context;
            _notyfService = notyfService;
            _comboHelper = comboHelper;
        }

        // GET: SellOneCachiers
        public async Task<IActionResult> Index(int? page)
        {
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
                .Where(c => c.CorporateId == user.CorporateId);


            return View(await dataContext.OrderBy(o => o.SellControl).ToPagedListAsync(page ?? 1, 10));
        }

        // GET: SellOneCachiers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var companyPic = _context.Corporates.Find(user.CorporateId);
            if (companyPic == null)
            {
                _notyfService.Custom("Problemas para Cargar Logo de la Empresa -  Notificacion", 5, "#D90000", "fa fa-trash");
                return RedirectToAction("Index", "Home");
            }

            var texto = _context.HeadTexts.FirstOrDefault(c => c.CorporateId == user.CorporateId);
            if (texto == null)
            {
                _notyfService.Custom("No Existe Un Embezado de Texto -  Notificacion", 5, "#D90000", "fa fa-trash");
                return RedirectToAction("Index", "Home");
            }


            var sellOneCachier = await _context.SellOneCachiers
                .Include(s => s.Cachier)
                .Include(s => s.PlanCategory)
                .Include(s => s.Corporate)
                .Include(s => s.OrderTicketDetail)
                .Include(s => s.Plan)
                .Include(s => s.Server)
                .FirstOrDefaultAsync(m => m.SellOneCachierId == id);
            if (sellOneCachier == null)
            {
                return NotFound();
            }
            sellOneCachier.ImageId = companyPic.ImageId;
            sellOneCachier.TextoHead = texto.TextoEncabezado;

            return View(sellOneCachier);
        }

        // GET: SellOneCachiers/Create
        public IActionResult Create()
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var texto = _context.HeadTexts.FirstOrDefault(c => c.CorporateId == user.CorporateId);
            if (texto == null)
            {
                _notyfService.Custom("No Existe Un Embezado de Texto -  Notificacion", 5, "#D90000", "fa fa-trash");
                return RedirectToAction("Index", "Home");
            }

            var usuario = _context.Cachiers.FirstOrDefault(m => m.UserName == user.UserName && m.Activo == true);
            SellOneCachier modelo = new()
            {
                CorporateId = Convert.ToInt32(user.CorporateId),
                Date = DateTime.Today,
                SellControl = 0,
                CachierId = usuario!.CachierId
            };

            modelo.ListCategory = _comboHelper.GetComboCatPlan(modelo.CorporateId);

            return View(modelo);
        }

        // POST: SellOneCachiers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SellOneCachier modelo)
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
                    var SumReg = NewReg.SellCachier + 1;
                    NewReg.SellCachier = SumReg;
                    _context.Registers.Update(NewReg);
                    await _context.SaveChangesAsync();
                    //actualizamos el valor en Register

                    modelo.SellControl = SumReg;

                    //actualizamos informacion del OrderDetail, para que el Ticket quede vendido
                    var ticket = await _context.OrderTicketDetails.FindAsync(modelo.OrderTicketDetailId);
                    ticket!.Vendido = true;
                    ticket.DateVenta = DateTime.Now;
                    ticket.SellOneCachier = true;
                    ticket.UserCachier = true;
                    ticket.CachierId = modelo.CachierId;
                    _context.OrderTicketDetails.Update(ticket);
                    await _context.SaveChangesAsync();
                    //fin

                    _context.Add(modelo);
                    await _context.SaveChangesAsync();

                    var ratecomision = await _context.Cachiers.FindAsync(modelo.CachierId);
                    //Porcentaje = false quiere decir que se le paga porcentaje y hay que guardar la operacion
                    //por en Falso se coloca el valos en Chachier.
                    if (ratecomision!.Porcentaje == false)
                    {
                        decimal comisionCajero = 0;
                        if (ratecomision.RateCachier != 0)
                        {
                            comisionCajero = Math.Round(((modelo.Total * ratecomision.RateCachier) / 100), 2);
                        }
                        else
                        {
                            comisionCajero = 0;
                        }


                        CachierPorcent comisiones = new()
                        {
                            Date = DateTime.Now,
                            CachierId = modelo.CachierId,
                            SellOneCachierId = modelo.SellOneCachierId,
                            OrderTicketDetailId = modelo.OrderTicketDetailId,
                            Porcentaje = ratecomision.RateCachier,
                            NamePlan = modelo.NamePlan,
                            Precio = modelo.Total,
                            Comision = comisionCajero,
                            CorporateId = modelo.CorporateId
                        };
                        _context.CachierPorcents.Add(comisiones);
                        await _context.SaveChangesAsync();
                    }


                    transaction.Commit();
                    //Se guardan todos los datos si todo esta successed.

                    _notyfService.Success("El Regitro se Guardado Con Exito -  Notificacion");
                    return RedirectToAction(nameof(Details), new { id = modelo.SellOneCachierId });
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

        // GET: SellOneCachiers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sellOneCachier = await _context.SellOneCachiers.FindAsync(id);
            if (sellOneCachier == null)
            {
                return NotFound();
            }
            ViewData["CachierId"] = new SelectList(_context.Cachiers, "CachierId", "Address", sellOneCachier.CachierId);
            ViewData["CorporateId"] = new SelectList(_context.Corporates, "CorporateId", "Address", sellOneCachier.CorporateId);
            ViewData["OrderTicketDetailId"] = new SelectList(_context.OrderTicketDetails, "OrderTicketDetailId", "Clave", sellOneCachier.OrderTicketDetailId);
            ViewData["PlanId"] = new SelectList(_context.Plans, "PlanId", "PlanName", sellOneCachier.PlanId);
            ViewData["PlanCategoryId"] = new SelectList(_context.PlanCategories, "PlanCategoryId", "PlanCategoryName", sellOneCachier.PlanCategoryId);
            ViewData["ServerId"] = new SelectList(_context.Servers, "ServerId", "Clave", sellOneCachier.ServerId);
            return View(sellOneCachier);
        }

        // POST: SellOneCachiers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SellOneCachierId,SellControl,Date,CachierId,PlanCategoryId,PlanId,NamePlan,ServerId,OrderTicketDetailId,Rate,SubTotal,Impuesto,Total,DateAnulado,Anulada,CorporateId")] SellOneCachier sellOneCachier)
        {
            if (id != sellOneCachier.SellOneCachierId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sellOneCachier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SellOneCachierExists(sellOneCachier.SellOneCachierId))
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
            ViewData["CachierId"] = new SelectList(_context.Cachiers, "CachierId", "Address", sellOneCachier.CachierId);
            ViewData["CorporateId"] = new SelectList(_context.Corporates, "CorporateId", "Address", sellOneCachier.CorporateId);
            ViewData["OrderTicketDetailId"] = new SelectList(_context.OrderTicketDetails, "OrderTicketDetailId", "Clave", sellOneCachier.OrderTicketDetailId);
            ViewData["PlanId"] = new SelectList(_context.Plans, "PlanId", "PlanName", sellOneCachier.PlanId);
            ViewData["PlanCategoryId"] = new SelectList(_context.PlanCategories, "PlanCategoryId", "PlanCategoryName", sellOneCachier.PlanCategoryId);
            ViewData["ServerId"] = new SelectList(_context.Servers, "ServerId", "Clave", sellOneCachier.ServerId);
            return View(sellOneCachier);
        }

        // GET: SellOneCachiers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sellOneCachier = await _context.SellOneCachiers
                .Include(s => s.Cachier)
                .Include(s => s.Corporate)
                .Include(s => s.OrderTicketDetail)
                .Include(s => s.Plan)
                .Include(s => s.PlanCategory)
                .Include(s => s.Server)
                .FirstOrDefaultAsync(m => m.SellOneCachierId == id);
            if (sellOneCachier == null)
            {
                return NotFound();
            }

            return View(sellOneCachier);
        }

        // POST: SellOneCachiers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sellOneCachier = await _context.SellOneCachiers.FindAsync(id);
            if (sellOneCachier != null)
            {
                _context.SellOneCachiers.Remove(sellOneCachier);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public JsonResult GetPlan(int idCategory, int idServer)
        {
            var data = _context.Plans
                .Where(c => c.PlanCategoryId == idCategory && c.Active == true && c.ServerId == idServer)
                .ToList();

            return Json(data.Select(x => new SelectListItem(x.PlanName, x.PlanId.ToString())));
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

        public JsonResult GetServidores(int idCorporate)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity!.Name);
            var vendedor = _context.Cachiers.FirstOrDefault(x => x.UserName == user!.UserName);
            if (vendedor!.MultiServer == false)
            {
                var servidor = _context.Servers
                .Where(c => c.Active == true && c.CorporateId == idCorporate && c.ServerId == vendedor.ServerId).ToList();
                if (servidor == null)
                {
                    return null!;
                }

                return Json(servidor.Select(x=> new SelectListItem( x.ServerName, x.ServerId.ToString())));
            }
            else
            {
                var servidor = _context.Servers
                .Where(c => c.Active == true && c.CorporateId == idCorporate).ToList();
                if (servidor == null)
                {
                    return null!;
                }

                return Json(servidor.Select(x => new SelectListItem(x.ServerName, x.ServerId.ToString())));
            }
        }

        private bool SellOneCachierExists(int id)
        {
            return _context.SellOneCachiers.Any(e => e.SellOneCachierId == id);
        }
    }
}
