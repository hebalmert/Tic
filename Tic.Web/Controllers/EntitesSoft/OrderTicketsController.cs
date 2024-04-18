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
    public class OrderTicketsController : Controller
    {
        private readonly DataContext _context;
        private readonly INotyfService _notyfService;
        private readonly IComboHelper _comboHelper;

        public OrderTicketsController(DataContext context, INotyfService notyfService,
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
            var datoMag = (from modelo in _context.Plans
                           where modelo.PlanName.Contains(Prefix) && modelo.CorporateId == user!.CorporateId
                           select new
                           {
                               label = modelo.PlanName,
                               val = modelo.PlanId
                           }).ToList();

            return Json(datoMag);

        }

        // GET: Plans
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
                return View(await _context.OrderTickets
                    .Include(z => z.PlanCategory)
                    .Include(z => z.Server)
                    .Include(z => z.Plan)
                    .Include(z => z.OrderTicketDetails)
                    .Where(c => c.OrderTicketId == buscarId && c.CorporateId == user.CorporateId).OrderByDescending(o => o.Date)
                    .ThenBy(o => o.NamePlan)
                    .ToPagedListAsync(page ?? 1, 25));
            }
            else
            {
                return View(await _context.OrderTickets
                    .Include(z => z.PlanCategory)
                    .Include(z => z.Server)
                    .Include(z => z.Plan)
                    .Include(z => z.OrderTicketDetails)
                    .Where(c => c.CorporateId == user.CorporateId).OrderByDescending(o => o.Date)
                    .ThenBy(o => o.NamePlan)
                    .ToPagedListAsync(page ?? 1, 25));
            }
        }

        // GET: Plans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OrderTickets == null)
            {
                return NotFound();
            }

            var plan = await _context.OrderTickets
                .Include(p => p.Corporate)
                .Include(p => p.PlanCategory)
                .Include(p => p.Server)
                .Include(p => p.Plan)
                .Include(p => p.OrderTicketDetails)
                .FirstOrDefaultAsync(m => m.OrderTicketId == id);
            if (plan == null)
            {
                return NotFound();
            }

            return View(plan);
        }

        // GET: OrderTickets/Create
        public IActionResult Create()
        {
            var user = _context.Users.Include(u => u.Corporate).FirstOrDefault(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                _notyfService.Custom("Problemas de Autenticacion debe comprobar credenciales -  Notificacion", 5, "#D90000", "fa fa-trash");
                return RedirectToAction("Login", "Account");
            }

            OrderTicket modelo = new()
            {
                CorporateId = Convert.ToInt32(user.CorporateId),
                Date = DateTime.Now
            };

            modelo.ListCategory = _comboHelper.GetComboCatPlan(modelo.CorporateId);
            //modelo.ListServer = _comboHelper.GetComboServerActivos(modelo.CorporateId);

            return View(modelo);
        }

        // POST: OrderTickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderTicket modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Se realiza el proceso de auto RollBack para algun fallo de Guardado
                    var transaction = await _context.Database.BeginTransactionAsync();

                    //Tomamos el consecutivo de Orden de tickets desde Register
                    var NewReg = await _context.Registers.Where(c => c.CorporateId == modelo.CorporateId).FirstOrDefaultAsync();
                    if (NewReg == null)
                    {
                        _notyfService.Success("Problemas para Asignar el Consecutivo de Orden de Tickets");
                        modelo.ListCategory = _comboHelper.GetComboCatPlan(modelo.CorporateId);
                        return View(modelo);
                    }
                    var SumReg = NewReg.OrderTickets + 1;
                    NewReg.OrderTickets = SumReg;
                    _context.Registers.Update(NewReg);
                    await _context.SaveChangesAsync();
                    //actualizamos el valor en Register

                    modelo.OrdenControl = SumReg;
                    _context.Add(modelo);
                    await _context.SaveChangesAsync();



                    await transaction.CommitAsync();

                    _notyfService.Success("El Regitro se Guardado Con Exito -  Notificacion");
                    return RedirectToAction(nameof(Details), new { id = modelo.OrderTicketId });
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
            return View(modelo);
        }

        // GET: OrderTickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modelo = await _context.OrderTickets.FindAsync(id);
            if (modelo == null)
            {
                return NotFound();
            }

            modelo.ListCategory = _comboHelper.GetComboCatPlan(modelo.CorporateId);
            modelo.ListServer = _comboHelper.GetComboServerActivos(modelo.CorporateId);
            modelo.ListPlan = _comboHelper.GetComboPlanOrdenes(modelo.CorporateId, modelo.ServerId, modelo.PlanCategoryId);
            return View(modelo);
        }

        // POST: SoftPlans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderTicket modelo)
        {
            if (id != modelo.OrderTicketId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(modelo);
                    await _context.SaveChangesAsync();

                    _notyfService.Success("El Regitro se Guardado Con Exito -  Notificacion");
                    return RedirectToAction(nameof(Details), new { id = modelo.OrderTicketId });
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

        // GET: OrderTickets/Create
        public async Task<IActionResult> AddTickets(int idOrderTicket, int tt)
        {
            var ordenTickets = await _context.OrderTickets
                .Include(x => x.Server)
                .ThenInclude(x => x!.IpNetwork)
                .Include(x => x.Plan)
                .ThenInclude(x => x!.TicketTime)
                .FirstOrDefaultAsync(x => x.OrderTicketId == idOrderTicket);

            OrderTicketDataViewModel modelo = new()
            {
                NamePlan = ordenTickets!.NamePlan,
                Up = ordenTickets.Plan!.VelocidadUp,
                Down = ordenTickets.Plan!.VelocidadUp,
                Tticket = ordenTickets.Plan.TicketTime!.Tiempo,
                PuertoApi = ordenTickets.Server!.ApiPort,
                Usuario = ordenTickets.Server.Usuario,
                Clave = ordenTickets.Server.Clave,
                ip = ordenTickets.Server.IpNetwork!.Ip,
                Cantidad = ordenTickets.Cantidad,
                CorporateId = ordenTickets.CorporateId,
                ServerId = ordenTickets.ServerId
            };

            //Se hace con conexion a la Mikroti y se deja abierto
            ////////////////////////////////////////////////////////////
            MK mikrotik = new MK(modelo.ip!, modelo.PuertoApi);
            if (!mikrotik.Login(modelo.Usuario, modelo.Clave))
            {
                _notyfService.Custom("Error en la Conexion al Servidor Mikrotik -  Notificacion", 5, "#D90000", "fa fa-trash");
                return RedirectToAction(nameof(Details), new { id = idOrderTicket });
            }

            var cadena = await _context.ChainCodes.FirstOrDefaultAsync(c => c.CorporateId == modelo.CorporateId);
            if (cadena == null)
            {
                _notyfService.Custom("Erro en Code Chain, Verifique en Configuracion -  Notificacion", 5, "#D90000", "fa fa-trash");
                return RedirectToAction(nameof(Details), new { id = idOrderTicket });
            }
            string velocidadPlan;
            string codigoTicket;
            bool sw;
            int total = 0;
            int rest = 0;
            string IdMk;
            string MkIndex;
            int Cnt = Convert.ToInt32(modelo.Cantidad) - tt;

            for (int i = 0; i < Cnt; i++)
            {
                MkIndex = string.Empty;
                sw = true;

                do
                {
                    codigoTicket = _comboHelper.GenerateTickets(cadena.Largo, cadena.Cadena!);
                    if (codigoTicket == null)
                    {
                        _notyfService.Custom("Error para Generar Codigos de Tickets -  Notificacion", 5, "#D90000", "fa fa-trash");
                        return RedirectToAction(nameof(Details), new { id = idOrderTicket });
                    }
                    var existCode = await _context.OrderTicketDetails.FirstOrDefaultAsync(x => x.Usuario == codigoTicket &&
                    x.CorporateId == modelo.CorporateId);

                    if (existCode == null)
                    {
                        sw = false;
                    }
                } while (sw == true);

                //Creamos la variable de velocidad desde el ADO 
                velocidadPlan = $"{modelo.Up}/{modelo.Down}";

                //Se realiza el proceso de auto RollBack para algun fallo de Guardado
                using var transaction = _context.Database.BeginTransaction();

                //Tomamo el numero de control del Ticket
                var NewReg = await _context.Registers.FirstOrDefaultAsync(c => c.CorporateId == modelo.CorporateId);
                if (NewReg == null)
                {
                    _notyfService.Custom("Error en la Asignacion de Control Tickets -  Notificacion", 5, "#D90000", "fa fa-trash");
                    return RedirectToAction(nameof(Details), new { id = idOrderTicket });
                }
                var SumReg = NewReg.Tickets + 1;
                NewReg.Tickets = SumReg;
                _context.Registers.Update(NewReg);
                await _context.SaveChangesAsync();
                //Guardamos el numero de control

                //;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
                //Creamos el Ticket en Mikrotik

                mikrotik.Send("/ip/hotspot/user/add");
                mikrotik.Send("=name=" + codigoTicket);
                mikrotik.Send("=password=" + "1234");
                mikrotik.Send("=limit-uptime=" + modelo.Tticket);
                mikrotik.Send("=profile=" + modelo.NamePlan);
                mikrotik.Send("/ip/hotspot/user/print", true);

                foreach (var item2 in mikrotik.Read())
                {
                    IdMk = item2;
                    total = IdMk.Length;
                    rest = total - 10;
                    MkIndex = IdMk.Substring(10, rest);
                }

                OrderTicketDetail NewRegister = new()
                {
                    CorporateId = modelo.CorporateId,
                    OrderTicketId = idOrderTicket,
                    Control = SumReg,
                    ServerId = modelo.ServerId,
                    Velocidad = velocidadPlan,
                    Usuario = codigoTicket,
                    Clave = "1234",
                    DateCreado = DateTime.Now,
                    MkId = MkIndex,
                };

                var nuevoReg = await _context.OrderTickets
                    .Include(c => c.OrderTicketDetails)
                    .FirstOrDefaultAsync(r => r.OrderTicketId == idOrderTicket);

                NewRegister.OrderTicketDetailId = 0;
                nuevoReg!.Mikrotik = true;
                nuevoReg.OrderTicketDetails!.Add(NewRegister);
                _context.Update(nuevoReg);

                await _context.SaveChangesAsync();

                transaction.Commit();
                //Se guardan todos los datos si todo esta successed.
            }
            mikrotik.Close();

            _notyfService.Success("El Regitro se Guardado Con Exito -  Notificacion");
            return RedirectToAction(nameof(Details), new { id = idOrderTicket });
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
                var dato = await _context.OrderTickets.FirstOrDefaultAsync(m => m.OrderTicketId == id);
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


        public JsonResult GetPrecio(int planId)
        {
            var data2 = _context.Plans
                .Include(p => p.Tax)
                .Where(p => p.PlanId == planId).FirstOrDefault();
            var dato = new
            {
                precio = data2!.Precio,
                rate = data2.Tax!.Rate
            };

            return Json(dato);
        }

        public JsonResult GetPlan(int idCategory, int idServer)
        {
            var data = _context.Plans.Where(c => c.PlanCategoryId == idCategory && c.Active == true && c.ServerId == idServer).ToList();

            return Json(data.Select(x => new SelectListItem(x.PlanName, x.PlanId.ToString())));
        }

        public JsonResult GetServidores(int idCorporate)
        {
            var servidor = _context.Servers
                .Where(c => c.Active == true && c.CorporateId == idCorporate).ToList();

            return Json(servidor.Select(x => new SelectListItem(x.ServerName, x.ServerId.ToString())));
        }

        private bool OrderTicketsExists(int id)
        {
            return _context.OrderTickets.Any(e => e.OrderTicketId == id);
        }
    }
}
