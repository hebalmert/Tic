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
    [Route("api/taxes")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
    [ApiController]
    public class TaxAPIController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public TaxAPIController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        [HttpGet("listtaxes")]
        public async Task<ActionResult<List<TaxDTOs>>> Getlist() 
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            var listaTax = await _context.Taxes.Where(c=> c.CorporateId == user.CorporateId && c.Active == true)
                .Select(x=> new { x.TaxId, x.TaxName})
                .OrderBy(x=> x.TaxName)
                .ToListAsync();


            return Ok(listaTax);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TaxValorDTOs>> GetTaxValor(int id)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            var modelo = await _context.Taxes.FirstOrDefaultAsync(x => x.TaxId == id);

            return Ok(modelo);
        }

    }
}
