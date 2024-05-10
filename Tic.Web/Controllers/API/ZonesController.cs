using AutoMapper;
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
    [Route("api/zones")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
    [ApiController]
    public class ZonesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IMapper _mapper;

        public ZonesController(DataContext context, IUserHelper userHelper, IMapper mapper)
        {
            _context = context;
            _userHelper = userHelper;
            _mapper = mapper;
        }


        [HttpGet("Estado")]
        public async Task<ActionResult<List<StateDTOs>>> Get()
        {
            //Validando con el mismo toquen de seguridad para saber quien es el User
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            var pais = await _context.Corporates.FirstOrDefaultAsync(x=> x.CorporateId == user.CorporateId);

            var listState = await _context.States.Where(x => x.CountryId == pais!.CountryId)
                .OrderBy(x=> x.Name)
                .ToListAsync();

            return Ok(listState);
        }

        [HttpGet("Ciudad/{id:int}")]
        public async Task<ActionResult<List<CityDTOs>>> Get(int id)
        {
            //Validando con el mismo toquen de seguridad para saber quien es el User
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            var listCity = await _context.Cities.Where(x => x.StateId == id)
                .OrderBy(x=> x.Name)
                .ToListAsync();

            return Ok(listCity);
        }

        [HttpGet("zonadetalle/{id:int}")]
        public async Task<ActionResult<ZoneDetailsDTOs>> GetZonaDetalle(int id)
        {
            //Validando con el mismo toquen de seguridad para saber quien es el User
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            var zona = await _context.Zones.FirstOrDefaultAsync(x => x.ZoneId == id);
            ZoneDetailsDTOs modelo = _mapper.Map<ZoneDetailsDTOs>(zona);


            return Ok(modelo);
        }

        [HttpGet("zonas/{idstate:int}/{idcity:int}")]
        public async Task<ActionResult<List<ZoneDTO>>> Get(int idstate, int idcity)
        {
            //Validando con el mismo toquen de seguridad para saber quien es el User
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            var listZone = await _context.Zones.Where(x => x.StateId == idstate && x.CityId == idcity)
                .OrderBy(x=> x.ZoneName)
                .ToListAsync();

            return Ok(listZone);
        }
    }
}
