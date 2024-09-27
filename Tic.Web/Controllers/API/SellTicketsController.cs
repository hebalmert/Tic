using AutoMapper;
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
    [Route("api/SellTickets")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User, UserAux, Cachier")]
    [ApiController]
    public class SellTicketsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IMapper _mapper;

        public SellTicketsController(DataContext context, IUserHelper userHelper, IMapper mapper)
        {
            _context = context;
            _userHelper = userHelper;
            _mapper = mapper;
        }


        [HttpGet("ventaTickets/{idplan:int}/{idser:int}/{idcat:int}")]  //Recibe el Id de Categoria y Servidor para enviar lista de Planes de esa categoria
        public async Task<ActionResult<TicketDTOs>> GetPlanInvPrecio(int idplan, int idser, int idcat)
        {
            //Validando con el mismo toquen de seguridad para saber quien es el User
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            var cajero = await _context.Cachiers.FirstOrDefaultAsync(x => x.UserName == user.UserName && x.Activo == true);
            if (cajero == null)
            {
                return BadRequest("Cajero no se Encuentra o No Esta Activo");
            }

            var plandetalle = await _context.Plans.FirstOrDefaultAsync(x => x.PlanId == idplan);

            //Tomamos el consecutivo de Orden de tickets desde Register
            var NewReg = await _context.Registers.Where(c => c.CorporateId == user.CorporateId).FirstOrDefaultAsync();
            if (NewReg == null)
            {
                return BadRequest("Error en la Asignacion de Consecutivo");
            }
            var SumReg = NewReg.SellCachier + 1;
            NewReg.SellCachier = SumReg;
            _context.Registers.Update(NewReg);
            await _context.SaveChangesAsync();
            //actualizamos el valor en Register

            //actualizamos informacion del OrderDetail, para que el Ticket quede vendido
            var ticket = await _context.OrderTicketDetails
                .Include(x => x.OrderTickets)
                .Where(x => x.ServerId == idser && x.OrderTickets!.PlanId == idplan
                 && x.Vendido == false && x.Anulado == false)
                .FirstOrDefaultAsync();

            ticket!.Vendido = true;
            ticket.DateVenta = DateTime.Now;
            ticket.SellOneCachier = true;
            ticket.UserCachier = true;
            ticket.CachierId = cajero.CachierId;
            _context.OrderTicketDetails.Update(ticket);
            await _context.SaveChangesAsync();
            //fin

            SellOneCachier modelo = new()
            {
                SellControl = SumReg,
                Date = DateTime.Now,
                CachierId = cajero.CachierId,
                PlanCategoryId = idcat,
                PlanId = idplan,
                NamePlan = plandetalle!.PlanName,
                ServerId = idser,
                OrderTicketDetailId = ticket.OrderTicketDetailId,
                Rate = plandetalle.RateTax,
                SubTotal = plandetalle.SubTotal,
                Impuesto = plandetalle.Impuesto,
                Total = plandetalle.Precio,
                CorporateId = Convert.ToInt32(user.CorporateId)
            };

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

            TicketDTOs venta = new()
            {
                Control = Convert.ToString(SumReg),
                fechaTicket = DateTime.Now,
                PinTicket = ticket.Usuario,
                VendedorName = cajero.FullName
            };

            return Ok(venta);
        }


    }
}
