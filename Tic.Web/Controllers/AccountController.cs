using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tic.Shared.Entites;
using Tic.Shared.Responses;
using Tic.Web.Data;
using Tic.Web.Helpers;
using Tic.Web.Models;

namespace Tic.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IEmailHelper _mailHelper;
        private readonly DataContext _context;
        private readonly INotyfService _notyfService;
        private readonly IConfiguration _configuration;
        private readonly string _SuperUsuarioEmail;

        public AccountController(IUserHelper userHelper,
            IEmailHelper mailHelper, DataContext Context,
            IConfiguration configuration, INotyfService notyfService)
        {
            _userHelper = userHelper;
            _mailHelper = mailHelper;
            _context = Context;
            _notyfService = notyfService;
            _configuration = configuration;
            _SuperUsuarioEmail = _configuration.GetValue<string>("UserControl:superUsuario")!;
        }

        //Para Redireccionamiento de paginas No Encontradas
        public IActionResult NotAuthorized()
        {
            return View();
        }

        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserAsync(model.Username);
                if (user != null)
                {
                    var corporateactive = await _context.Corporates.FindAsync(user.CorporateId);

                    //TODO: Soy Super Usuario Usuario y Clave verificamos compania activa
                    if (user.UserName != _SuperUsuarioEmail)
                    {
                        DateTime hoy = DateTime.Today;
                        DateTime current = corporateactive!.ToEnd;
                        if (corporateactive.Activo == false)
                        {
                            //ModelState.AddModelError("Error", "La Cuenta Caduco o esta Bloqueada...");
                            _notyfService.Custom("La Cuenta Caduco o esta Bloqueada... -  Notificacion", 5, "#D90000", "fa fa-trash");
                            return View();
                        }
                        if (current <= hoy)
                        {
                            //ModelState.AddModelError("Error", "La Cuenta Caduco o esta Bloqueada...");
                            _notyfService.Custom("La Cuenta Caduco o esta Bloqueada... -  Notificacion", 5, "#D90000", "fa fa-trash");
                            return View();
                        }

                        if (user.Activo == false)
                        {
                            //ModelState.AddModelError("Error", "El Usuario se Encuenta Inactivo...");
                            _notyfService.Custom("El Usuario se Encuenta Inactivo... -  Notificacion", 5, "#D90000", "fa fa-trash");
                            return View();
                        }
                    }
                    Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.LoginAsync(model);

                    if (result.Succeeded)
                    {
                        var TipoRole = user.UserType.ToString();
                        string imgUsuario = string.Empty;
                        if (user.UserType.ToString() == "User")
                        {
                            //TODO: Cambiar la URL de la Imagen en el Hosting
                            imgUsuario = user.UserType.ToString() == "User" ? $"http://tic.nexxtplanet.net/images/ImgUser/{user.Photo}" : $"http://tic.nexxtplanet.net/images/NoImage.png";
                        }
                        if (user.UserType.ToString() == "Cachier")
                        {
                            imgUsuario = user.UserType.ToString() == "Cachier" ? $"http://tic.nexxtplanet.net/images/ImgCachier/{user.Photo}" : $"http://tic.nexxtplanet.net/images/NoImage.png";
                        }
                        if (user.UserType.ToString() == "UserAux")
                        {
                            imgUsuario = user.UserType.ToString() == "UserAux" ? $"http://tic.nexxtplanet.net/images/ImgAuxUser/{user.Photo}" : $"http://tic.nexxtplanet.net/images/NoImage.png";
                        }
                        if (TipoRole != "Admin")
                        {
                            //var imgUsuario = user.ImageFullPath;
                            int corporacionId = Convert.ToInt32(user.CorporateId);
                            HttpContext.Session.SetString("Logo", corporateactive!.ImageFullPath);
                            HttpContext.Session.SetString("PicUser", imgUsuario);
                            HttpContext.Session.SetInt32("CorpId", corporacionId);
                            HttpContext.Session.SetString("NomUser", $"{user.FirstName} {user.LastName}");
                            HttpContext.Session.SetString("NomCorp", corporateactive.Name);
                            HttpContext.Session.SetString("CargoUser", user.Job);
                        }
                        else
                        {
                            //TODO:Change Addres to Image
                            string logoTemp = $"https://spi.nexxtplanet.net/Images/SiteLogo.png";
                            string perTemp = $"https://spi.nexxtplanet.net/Images/SiteLogo.png";
                            HttpContext.Session.SetString("Logo", logoTemp);
                            HttpContext.Session.SetString("PicUser", perTemp);
                            HttpContext.Session.SetString("NomUser", "Administrador");
                            HttpContext.Session.SetString("NomCorp", "NexxtPlanet");
                            HttpContext.Session.SetString("CargoUser", "Administrador");
                        }


                        if (Request.Query.Keys.Contains("ReturnUrl"))
                        {
                            return Redirect(Request.Query["ReturnUrl"].First()!);
                        }

                        return RedirectToAction("Index", "Home");

                    }
                    if (result.IsLockedOut)
                    {
                        _notyfService.Custom("Ha superado el máximo número de intentos, su cuenta está bloqueada, intente de nuevo en 5 minutos.. -  Notificacion", 5, "#D90000", "fa fa-trash");
                        return View(model);
                    }

                    if (result.IsNotAllowed)
                    {
                        _notyfService.Custom("El usuario no ha sido habilitado, debes de seguir las instrucciones del correo enviado para poder habilitar el usuario. -  Notificacion", 5, "#D90000", "fa fa-trash");
                        return View(model);
                    }
                    _notyfService.Custom("Email or password incorrect. Verifique -  Notificacion", 5, "#D90000", "fa fa-trash");
                    return View(model);
                }
                _notyfService.Custom("Email or password incorrect. Verifique -  Notificacion", 5, "#D90000", "fa fa-trash");
                //ModelState.AddModelError(string.Empty, "Email or password incorrect.");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            HttpContext.Session = null!;
            return RedirectToAction("Login", "Account");
        }

        public IActionResult ChangePasswordMVC()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordMVC(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserAsync(User.Identity!.Name!);
                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        //ModelState.AddModelError(string.Empty, "La Clave se ha Actualizado con Exito");
                        _notyfService.Success("La Clave se ha Actualizado con Exito -  Notificacion");
                        return View();
                    }
                    else
                    {
                        _notyfService.Success(result.Errors.FirstOrDefault()!.Description);
                        //ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault()!.Description);
                    }
                }
                else
                {
                    _notyfService.Custom("Usuario no Encontrado.... Verifique -  Notificacion", 5, "#D90000", "fa fa-trash");
                    //ModelState.AddModelError(string.Empty, "Usuario no Encontrado...");
                }
            }

            return View(model);
        }

        public IActionResult ConfirmEmailSend()
        {
            return View();
        }

        //Valida el Correo del usuario para Activar la cuenta
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            User user = await _userHelper.GetUserAsync(new Guid(userId));
            if (user == null)
            {
                return NotFound();
            }

            IdentityResult result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return NotFound();
            }
            //_flashMessage.Info(string.Empty, "Su cuenta ha sido Activada con Exito");
            return View();
        }

        public IActionResult RecoverPasswordMVC()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecoverPasswordMVC(RecoverPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userHelper.GetUserAsync(model.Email);
                if (user == null)
                {

                    _notyfService.Custom("Este Correo no Corresponde a ningun usuario... -  Notificacion", 5, "#D90000", "fa fa-trash");
                    return View(model);
                }

                string myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);
                string tokenLink = Url.Action("ResetPassword", "Account", new
                {
                    username = user.UserName,
                    token = myToken
                }, protocol: HttpContext.Request.Scheme)!;

                string subject = "Recuperacion de Cuenta";
                string body = ($"De: Nexxtplanet" +
                    $"<h1>Recuperar Clave</h1>" +
                    $"<p>" +
                    $"Podra Ingresar una nueva clave</h2>" +
                    $"</p>" +
                    $"Para continuar, " +
                    $"Has Click en el siguiente Link:</br></br><a href = \"{tokenLink}\"> ...Cmabio de Clave...</a>");

                Response response = await _mailHelper.ConfirmarCuenta(user.UserName!, user.FullName!, subject, body);
                if (response.IsSuccess == false)
                {
                    _notyfService.Custom("Hemos presentado un problema para Actualizar su Clave -  Notificacion", 5, "#D90000", "fa fa-trash");

                    return RedirectToAction("Index", "Home");
                }

                //_mailHelper.SendMail(model.Email, "Password Reset", $"<h1>Recuperacion Clave</h1>" +
                //    $"Para cambiar la Clave has click en el siguiente link: </br></br>" +
                //    $"<a href = \"{link}\">Reset Password</a>");

                _notyfService.Custom("Recuperacion de Clave. Revisar el Correo... -  Notificacion", 5, "#D90000", "fa fa-trash");

                return RedirectToAction("Login", "Account");

            }

            return View(model);
        }

        public IActionResult ResetPassword(string token, string username)
        {
            var modelo = new ResetPasswordViewModel
            {
                UserName = username,
                Token = token
            };
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            User user = await _userHelper.GetUserAsync(model.UserName);
            if (user != null)
            {
                IdentityResult result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {

                    _notyfService.Success("Clave cambiada con Exito. -  Notificacion");
                    return RedirectToAction("Login", "Account");
                }

                _notyfService.Custom("Error Mientras cambiaba su clave... -  Notificacion", 5, "#D90000", "fa fa-trash");
                return View(model);
            }

            _notyfService.Custom("Usuario no encontrado... -  Notificacion", 5, "#D90000", "fa fa-trash");
            return View(model);
        }
    }
}
