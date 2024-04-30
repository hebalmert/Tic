using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Tic.Shared.ApiDTOs;
using Tic.Web.Data;
using Tic.Web.Helpers;

namespace Tic.Web.Controllers.API
{
    [Route("api/plans")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
    [ApiController]
    public class PlansController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public PlansController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        [HttpGet("Categoria")]
        public async Task<ActionResult<List<PlanCategoryDTOs>>> GetCategory()
        {
            //Validando con el mismo toquen de seguridad para saber quien es el User
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            var listState = await _context.PlanCategories.Where(x => x.CorporateId == user.CorporateId)
                .OrderBy(x => x.PlanCategoryName)
                .ToListAsync();

            return Ok(listState);
        }

        [HttpGet("inactivo")]
        public async Task<ActionResult<List<InactivePickerDTOs>>> GetInactivo()
        {
            //Validando con el mismo toquen de seguridad para saber quien es el User
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            var listmodelo = await _context.TicketInactives.Where(x => x.Activo == true) 
                .OrderBy(x => x.Orden)
                .ToListAsync();

            return Ok(listmodelo.Select(x => new { x.TicketInactiveId, x.Tiempo }));
        }

        [HttpGet("refresh")]
        public async Task<ActionResult<List<RefreshPicketDTOs>>> GetRefresh()
        {
            //Validando con el mismo toquen de seguridad para saber quien es el User
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            var listmodelo = await _context.TicketRefreshes.Where(x => x.Activo == true)
                .OrderBy(x => x.Orden)
                .ToListAsync();

            return Ok(listmodelo.Select(x => new { x.TicketRefreshId, x.Tiempo }));
        }

        [HttpGet("tiempo")]
        public async Task<ActionResult<List<TimePicketDTOs>>> GetTiempo()
        {
            //Validando con el mismo toquen de seguridad para saber quien es el User
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            var listmodelo = await _context.TicketTimes.Where(x => x.Activo == true)
                .OrderBy(x => x.Orden)
                .ToListAsync();

            return Ok(listmodelo.Select(x => new { x.TicketTimeId, x.Tiempo }));
        }

        [HttpGet]
        public async Task<ActionResult<List<PlanIndexDTOs>>> Get()
        {
            //Validando con el mismo toquen de seguridad para saber quien es el User
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            var listServer = (from pl in _context.Plans
                              join pc in _context.PlanCategories on pl.PlanCategoryId equals pc.PlanCategoryId
                              join sv in _context.Servers on pl.ServerId equals sv.ServerId
                              where pl.CorporateId == user.CorporateId
                              select new PlanIndexDTOs
                              {
                                  PlanId = pl.PlanId,
                                  Servidor = sv.ServerName,
                                  Categoria = pc.PlanCategoryName!,
                                  PlanName = pl.PlanName,
                                  SpeedUp = $"Up: {pl.VelocidadUp}",
                                  SpeedDown = $"Down: {pl.VelocidadDown}",
                                  TiempoTicket = $"Tiempo: {pl.TicketTime!.Tiempo}",
                                  ShareUser = $"User: {pl.ShareUser}",
                                  Precio = $"Valor: {pl.Precio}",
                                  Active = pl.Active == true ? "On" : "Off",
                                  MkId = !String.IsNullOrEmpty(pl.MkId) ? "On" : "Off",
                                  CorporateId = sv.CorporateId
                              }).OrderBy(x => x.Servidor).ThenBy(x => x.PlanName).ToList();

            return Ok(listServer);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PlanDTOs>> Get(int id)
        {
            //Validando con el mismo toquen de seguridad para saber quien es el User
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            var planDetail = (from pl in _context.Plans
                              join pc in _context.PlanCategories on pl.PlanCategoryId equals pc.PlanCategoryId
                              join ti in _context.TicketInactives on pl.TicketInactiveId equals ti.TicketInactiveId
                              join tr in _context.TicketRefreshes on pl.TicketRefreshId equals tr.TicketRefreshId
                              join tt in _context.TicketTimes on pl.TicketTimeId equals tt.TicketTimeId
                              join sv in _context.Servers on pl.ServerId equals sv.ServerId
                              where pl.CorporateId == user.CorporateId && pl.PlanId == id
                              select new PlanDTOs
                              {
                                  PlanId = pl.PlanId,
                                  DateCreated = pl.DateCreated,
                                  DateEdit = pl.DateEdit,
                                  Server = sv.ServerName,
                                  PlanCategory = pc.PlanCategoryName!,
                                  PlanName = pl.PlanName,
                                  SpeedUp = $"{pl.VelocidadUp}",
                                  SpeedDown = $"{pl.VelocidadDown}",
                                  TicketTime = $"{pl.TicketTime!.Tiempo}",
                                  TicketInactive = $"{pl.TicketInactive!.Tiempo}",
                                  TicketRefresh = $"{pl.TicketRefresh!.Tiempo}",
                                  ShareUser = $"{pl.ShareUser}",
                                  Proxy = pl.Proxy,
                                  MacCookies = pl.MacCookies,
                                  ContinueTime = pl.ContinueTime,
                                  RateTax =$"{pl.Tax!.Rate}",
                                  SubTotal = $"{pl.SubTotal}",
                                  Impuesto = $"{pl.Impuesto}",
                                  Precio = $"{pl.Precio}",
                                  Active = pl.Active == true ? "On" : "Off",
                                  MkId = !String.IsNullOrEmpty(pl.MkId) ? "On" : "Off",
                                  CorporateId = sv.CorporateId
                              }).FirstOrDefault();

            return Ok(planDetail);
        }
    }
}
