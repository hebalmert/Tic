using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Tic.Shared.ApiDTOs;
using Tic.Shared.EntitiesSoft;
using Tic.Web.Data;
using Tic.Web.Helpers;

namespace Tic.Web.Controllers.API
{
    [Route("api/ordertickets")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
    [ApiController]
    public class OrderTicketsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IMapper _mapper;
        private readonly IComboHelper _comboHelper;

        public OrderTicketsController(DataContext context, IUserHelper userHelper,
            IMapper mapper, IComboHelper comboHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _mapper = mapper;
            _comboHelper = comboHelper;
        }

        [HttpPost]
        public async Task<ActionResult<List<OrderTicketDetail>>> PostNewPlan([FromBody] OrderTicketsDTOs modeloOrder)
        {
            //Validando con el mismo toquen de seguridad para saber quien es el User
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            //Tomamos el consecutivo de Orden de tickets desde Register
            var NewReg = await _context.Registers.Where(c => c.CorporateId == user.CorporateId).FirstOrDefaultAsync();
            if (NewReg == null)
            {
                return BadRequest("Error en Asiganacion de Consecutivo de Orden");
            }
            var SumReg = NewReg.OrderTickets + 1;
            NewReg.OrderTickets = SumReg;
            _context.Registers.Update(NewReg);
            await _context.SaveChangesAsync();
            //actualizamos el valor en Register

            OrderTicket NuevoModelo = new()
            {
                OrdenControl = SumReg,
                Date = DateTime.Now,
                PlanCategoryId = modeloOrder.PlanCategoryId,
                ServerId = modeloOrder.ServerId,
                PlanId = modeloOrder.PlanId,
                NamePlan = modeloOrder.NamePlan,
                Rate = modeloOrder.Rate,
                Precio = modeloOrder.Precio,
                Cantidad = modeloOrder.Cantidad,
                SubTotal = modeloOrder.SubTotal,
                Impuesto = modeloOrder.Impuesto,
                Total = modeloOrder.Total,
                Mikrotik = false,
                CorporateId = (int)user.CorporateId!
            };

            _context.OrderTickets.Add(NuevoModelo);
            await _context.SaveChangesAsync();

            var ordenTickets = await _context.OrderTickets
                .Include(x => x.Server)
                .ThenInclude(x => x!.IpNetwork)
                .Include(x => x.Plan)
                .ThenInclude(x => x!.TicketTime)
                .FirstOrDefaultAsync(x => x.OrderTicketId == NuevoModelo.OrderTicketId);

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

            var cadena = await _context.ChainCodes.FirstOrDefaultAsync(c => c.CorporateId == modelo.CorporateId);
            if (cadena == null)
            {
                return BadRequest("Problemas con la Configuracion del Chain para Crear Codigos");
            }
            string velocidadPlan;
            string codigoTicket;
            bool sw;
            int Cnt = Convert.ToInt32(modelo.Cantidad);

            List<OrderTicketDetail> ListaTickets = new List<OrderTicketDetail>();

            for (int i = 0; i < Cnt; i++)
            {
                sw = true;

                do
                {
                    codigoTicket = _comboHelper.GenerateTickets(cadena.Largo, cadena.Cadena!);
                    if (codigoTicket == null)
                    {
                        return BadRequest("Error en la Generacion de Codigos");
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

                //Tomamo el numero de control del Ticket
                var NewReg2 = await _context.Registers.FirstOrDefaultAsync(c => c.CorporateId == modelo.CorporateId);
                if (NewReg2 == null)
                {
                    return BadRequest("Error en la creacion del Consecutivo de Control del Tickets");
                }
                var SumReg2 = NewReg2.Tickets + 1;
                NewReg2.Tickets = SumReg2;
                _context.Registers.Update(NewReg2);
                await _context.SaveChangesAsync();
                //Guardamos el numero de control

                OrderTicketDetail NewRegister = new()
                {
                    CorporateId = modelo.CorporateId,
                    OrderTicketId = NuevoModelo.OrderTicketId,
                    Control = SumReg2,
                    ServerId = modelo.ServerId,
                    Velocidad = velocidadPlan,
                    Usuario = codigoTicket,
                    Clave = "1234",
                    DateCreado = DateTime.Now
                };

                var nuevoReg = await _context.OrderTickets
                    .Include(c => c.OrderTicketDetails)
                    .FirstOrDefaultAsync(r => r.OrderTicketId == NuevoModelo.OrderTicketId);

                nuevoReg!.OrderTicketDetails!.Add(NewRegister);
                _context.Update(nuevoReg);

                await _context.SaveChangesAsync();

                ListaTickets.Add(NewRegister);
            }

            return Ok(ListaTickets);
        }

        [HttpGet("ordenticketdata/{id:int}")]
        public async Task<ActionResult<OrderTicketDataViewModel>> GetInactivo(int id)
        {
            //Validando con el mismo toquen de seguridad para saber quien es el User
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            var ordenTickets = await _context.OrderTickets
                .Include(x => x.Server)
                .ThenInclude(x => x!.IpNetwork)
                .Include(x => x.Plan)
                .ThenInclude(x => x!.TicketTime)
                .FirstOrDefaultAsync(x => x.OrderTicketId == id);  //id es el OrderTicketId

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

            return Ok(modelo);
        }

        [HttpGet("ordenTicketIndex")]
        public async Task<ActionResult<List<OrderTicketIndexDTOs>>> GetListIndex()
        {
            //Validando con el mismo toquen de seguridad para saber quien es el User
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            var ordenTickets = await _context.OrderTickets
            .Include(x => x.Server)
            .Include(x => x.Plan)
            .Where(x => x.CorporateId == user.CorporateId)
            .Select(x=> new OrderTicketIndexDTOs 
            { 
                OrderTicketId = x.OrderTicketId,
                OrdenControl = x.OrdenControl,
                Date = x.Date.ToString("dd-MM-yyyy"),
                NameServer = x.Server!.ServerName,
                NamePlan = x.Plan!.PlanName,
                Cantidad = x.Cantidad,
                Total = x.Total,
                Mikrotik = x.Mikrotik == true ? "Creado" : "Fallo"
            })
            .ToListAsync();  //id es el OrderTicketId

            
            return Ok(ordenTickets.OrderByDescending(x=> x.OrdenControl));
        }

        [HttpPut]
        public async Task<ActionResult> PutUpdateListaTickets([FromBody] List<OrderTicketDetail> modelo)
        {
            //Validando con el mismo toquen de seguridad para saber quien es el User
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }
            if (modelo == null)
            {
                return BadRequest("Lista de Codigos esta Vacio, error en la Crecion de Tickets.");
            }

            int countNul = modelo.Count(m => string.IsNullOrEmpty(m.MkId));
            if (countNul > 0)
            {
                return BadRequest("No se puede Guardar los Tickets en Servidor porque no se Crearon los Ticket en la Mikrotik.");
            }

            _context.OrderTicketDetails.UpdateRange(modelo);
            await _context.SaveChangesAsync();
            var idorder = modelo[0].OrderTicketId;
            var UpdateOrder = await _context.OrderTickets.FirstAsync(x => x.OrderTicketId == idorder);
            UpdateOrder.Mikrotik = true;
            _context.Update(UpdateOrder);
            await _context.SaveChangesAsync();


            return Created();
        }
    }
}
