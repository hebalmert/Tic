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
    [Route("api/ipnetwork")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
    [ApiController]
    public class IpNetworksController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public IpNetworksController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        [HttpGet("listIp")]
        public async Task<ActionResult<List<IpNetListDTOs>>> GetListIP()
        {
            //Validando con el mismo toquen de seguridad para saber quien es el User
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            var listIp = await _context.IpNetworks.
                Where(x => x.CorporateId == user.CorporateId && x.Active == true && x.Assigned == false)
                .ToListAsync();

            return Ok(listIp);
        }

        [HttpGet("listIpEdit/{idip:int}")]
        public async Task<ActionResult<List<IpNetListDTOs>>> GetListIP(int idip)
        {
            //Validando con el mismo toquen de seguridad para saber quien es el User
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            var listIp = await _context.IpNetworks.
                Where(x => x.CorporateId == user.CorporateId && x.Active == true && x.Assigned == false || x.IpNetworkId == idip)
                .ToListAsync();

            return Ok(listIp);
        }
    }
}
