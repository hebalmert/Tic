using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tic.Shared.Entites;
using Tic.Shared.SystemDTOs;
using Tic.Web.Data;
using Tic.Web.Helpers;

namespace Tic.Web.Controllers.API
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _configuration;
        private readonly IFileStorage _fileStorage;
        private readonly IEmailHelper _emailHelper;
        private readonly DataContext _context;
        private readonly string _container;

        public AccountsController(IUserHelper userHelper, IConfiguration configuration, IFileStorage fileStorage,
            IEmailHelper emailHelper, DataContext context)
        {
            _userHelper = userHelper;
            _configuration = configuration;
            _fileStorage = fileStorage;
            _emailHelper = emailHelper;
            _context = context;
            //Podemos configurar la ruta en disco o el nombre del contenedor
            _container = "users";
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO modelo)
        {
            var result = await _userHelper.LoginAsync(modelo);
            if (result.Succeeded)
            {
                var user = await _userHelper.GetUserAsync(modelo.Email);
                if (user != null)
                {
                    var corporateActive = await _context.Corporates.FindAsync(user.CorporateId);

                }
                string ImagenPath = string.Empty;
                switch (user!.UserType.ToString())
                {
                    case "Admin":
                        ImagenPath = $"http://tic.nexxtplanet.net/images/NoImage.png";
                        break;
                    case "User":
                        ImagenPath = user.Photo == string.Empty || user.Photo == null ? $"http://tic.nexxtplanet.net/images/NoImage.png" :
                            $"http://tic.nexxtplanet.net/images/ImgUser/{user.Photo}";
                        break;
                    case "Cachier":
                        ImagenPath = user.Photo == string.Empty || user.Photo == null ? $"http://tic.nexxtplanet.net/images/NoImage.png" :
                            $"http://tic.nexxtplanet.net/images/ImgCachier/{user.Photo}";
                        break;
                }
                user.PhotoPath = ImagenPath;

                return Ok(BuildToken(user));
            }

            if (result.IsLockedOut)
            {
                return BadRequest("Ha superado el máximo número de intentos, su cuenta está bloqueada, intente de nuevo en 5 minutos.");
            }

            if (result.IsNotAllowed)
            {
                return BadRequest("El usuario no ha sido habilitado, debes de seguir las instrucciones del correo enviado para poder habilitar el usuario.");
            }

            return BadRequest("Email o contraseña incorrectos.");

        }

        private TokenDTO BuildToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, user.UserType.ToString()),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("Photo", user.PhotoPath!)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwtKey"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddDays(30);
            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials);

            return new TokenDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }

        [HttpPost("changePassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> ChangePasswordAsync(ChangePasswordDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string email = User.Claims.FirstOrDefault(x=> x.Type == ClaimTypes.Email)!.Value;
            User user = await _userHelper.GetUserAsync(email);
            if (user == null || user.Activo == false)
            {
                return BadRequest("Usuario Sin Permisos o Bloqueado");
            }

            var result = await _userHelper.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.FirstOrDefault()!.Description);
            }

            return NoContent();
        }
    }
}
