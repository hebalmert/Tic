using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using Tic.Shared.Entites;
using Tic.Shared.EntitiesSoft;
using Tic.Shared.Enum;
using Tic.Shared.Responses;
using Tic.Web.Data;
using Tic.Web.Helpers;
using X.PagedList;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Tic.Web.Controllers.EntitesSoft
{
    [Authorize(Roles = "User")]
    public class CachiersController : Controller
    {
        private readonly DataContext _context;
        private readonly INotyfService _notyfService;
        private readonly IComboHelper _comboHelper;
        private readonly IFileStorage _fileStorage;
        private readonly IUserHelper _userHelper;
        private readonly IEmailHelper _emailHelper;
        private readonly string _container;

        public CachiersController(DataContext context, INotyfService notyfService,
            IComboHelper comboHelper, IFileStorage fileStorage, IUserHelper userHelper,
            IEmailHelper emailHelper)
        {
            _context = context;
            _notyfService = notyfService;
            _comboHelper = comboHelper;
            _fileStorage = fileStorage;
            _userHelper = userHelper;
            _emailHelper = emailHelper;
            _container = "wwwroot\\Images\\ImgCachier";
        }

        [HttpPost]
        public JsonResult Search(string Prefix)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity!.Name);
            var datoMag = (from modelo in _context.Cachiers
                           where modelo.FullName!.ToLower().Contains(Prefix.ToLower()) && modelo.CorporateId == user!.CorporateId
                           select new
                           {
                               label = modelo.FullName,
                               val = modelo.CachierId
                           }).ToList();

            return Json(datoMag);

        }

        // GET: Cachiers
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
                return View(await _context.Cachiers
                    .Include(z => z.DocumentType)
                    .Where(c => c.CachierId == buscarId && c.CorporateId == user.CorporateId).OrderBy(o => o.FullName)
                    .ToPagedListAsync(page ?? 1, 25));
            }
            else
            {
                return View(await _context.Cachiers
                    .Include(z => z.DocumentType)
                    .Where(c => c.CorporateId == user.CorporateId)
                    .OrderBy(o => o.FullName).ToPagedListAsync(page ?? 1, 25));
            }
        }

        // GET: Cachiers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cachier = await _context.Cachiers
                .Include(c => c.Corporate)
                .Include(c => c.DocumentType)
                .Include(c => c.Server)
                .FirstOrDefaultAsync(m => m.CachierId == id);
            if (cachier == null)
            {
                return NotFound();
            }

            return View(cachier);
        }

        // GET: Cachiers/Create
        public IActionResult Create()
        {
            var user = _context.Users.Include(u => u.Corporate).FirstOrDefault(u => u.UserName == User.Identity!.Name);
            if (user == null)
            {
                _notyfService.Custom("Problemas de Autenticacion debe comprobar credenciales -  Notificacion", 5, "#D90000", "fa fa-trash");
                return RedirectToAction("Login", "Account");
            }

            Cachier datos = new()
            {
                CorporateId = Convert.ToInt32(user.CorporateId),
                Activo = true
            };

            datos.ListDocument = _comboHelper.GetComboDocument(datos.CorporateId);
            datos.ListServer = _comboHelper.GetComboServerActivos(datos.CorporateId);

            return View(datos);
        }

        // POST: Cachiers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cachier modelo)
        {
            modelo.UserType = UserType.Cachier;
            modelo.DateCreated = DateTime.Now;
            if (ModelState.IsValid)
            {
                try
                {
                    if (modelo.MultiServer == false && modelo.ServerId == 0)
                    {
                        _notyfService.Custom("Debe Seleccionar Un Servidor o Multi Servidor -  Notificacion", 5, "#D90000", "fa fa-trash");
                        modelo.ListDocument = _comboHelper.GetComboDocument(modelo.CorporateId);
                        modelo.ListServer = _comboHelper.GetComboServerActivos(modelo.CorporateId);
                        return View(modelo);
                    }


                    User ChaeckUser = await _userHelper.GetUserAsync(modelo.UserName!);
                    if (ChaeckUser != null)
                    {
                        _notyfService.Error("Este Correo ya se encuentra usado - Notificacion");
                        modelo.ListDocument = _comboHelper.GetComboDocument(modelo.CorporateId);
                        modelo.ListServer = _comboHelper.GetComboServerActivos(modelo.CorporateId);
                        return View(modelo);
                    }

                    //Se realiza el proceso de auto RollBack para algun fallo de Guardadoplanes de internet
                    using var transaction = _context.Database.BeginTransaction();

                    //Guardamos el registro y si guarda sin errores seguimso con la imagen
                    modelo.FullName = $"{modelo.FirstName} {modelo.LastName}";
                    _context.Add(modelo);
                    await _context.SaveChangesAsync();

                    //Seguimos con imagen, de esta manera si hubo error en el guardado, la imagen no
                    if (modelo.ImageFile != null)
                    {
                        modelo.Photo = await _fileStorage.UploadImage(modelo.ImageFile, ".jpg", _container, modelo.Photo);
                    }
                    _context.Update(modelo);
                    await _context.SaveChangesAsync();

                    if (modelo.Activo == true)
                    {
                        Response response = await AcivateUser(modelo);
                        if (response.IsSuccess == false)
                        {
                            string ruta = "wwwroot\\Images\\ImgUser";
                            var guid = modelo.Photo;
                            _fileStorage.DeleteImage(ruta, guid!);

                            ModelState.AddModelError(string.Empty, "Vuelva a Intentarlo, el Usuario no ha sido Creado");
                            return RedirectToAction("Create", "Managers");
                        }
                    }

                    transaction.Commit();
                    //Se guardan todos los datos si todo esta successed.

                    _notyfService.Success("El Regitro se Guardado Con Exito -  Notificacion");
                    return RedirectToAction(nameof(Details), new { id = modelo.CachierId });
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

            modelo.ListDocument = _comboHelper.GetComboDocument(modelo.CorporateId);
            modelo.ListServer = _comboHelper.GetComboServerActivos(modelo.CorporateId);
            return View(modelo);
        }

        // GET: Cachiers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Managers == null)
            {
                return NotFound();
            }

            var modelo = await _context.Cachiers.FindAsync(id);
            if (modelo == null)
            {
                return NotFound();
            }

            modelo.ListDocument = _comboHelper.GetComboDocument(modelo.CorporateId);
            modelo.ListServer = _comboHelper.GetComboServerActivos(modelo.CorporateId);
            return View(modelo);
        }

        // POST: Cachiers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cachier modelo)
        {
            if (id != modelo.CachierId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Se realiza el proceso de auto RollBack para algun fallo de Guardadoplanes de internet
                    using var transaction = _context.Database.BeginTransaction();

                    modelo.FullName = $"{modelo.FirstName} {modelo.LastName}";

                    if (modelo.ImageFile != null)
                    {
                        if (modelo.ImageFile != null)
                        {
                            modelo.Photo = await _fileStorage.UploadImage(modelo.ImageFile, ".jpg", _container, modelo.Photo);
                        }
                    }

                    _context.Update(modelo);
                    await _context.SaveChangesAsync();

                    var user = await _userHelper.GetUserAsync(modelo.UserName!);
                    user.Activo = modelo.Activo;
                    if (modelo.ImageFile != null)
                    {
                        user.Photo = modelo.Photo;
                    }
                    IdentityResult response = await _userHelper.UpdateUserAsync(user);
                    if (!response.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, "Vuelva a Intentarlo, el Usuario no ha sido Creado");
                        modelo.ListDocument = _comboHelper.GetComboDocument(modelo.CorporateId);
                        modelo.ListServer = _comboHelper.GetComboServerActivos(modelo.CorporateId);
                        return View(modelo);
                    }

                    transaction.Commit();
                    //Se guardan todos los datos si todo esta successed.

                    _notyfService.Success("El Regitro se ha Actualizado con Exito -  Notificacion");
                    return RedirectToAction("Details", new { id = modelo.CachierId });
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
            modelo.ListDocument = _comboHelper.GetComboDocument(modelo.CorporateId);
            modelo.ListServer = _comboHelper.GetComboServerActivos(modelo.CorporateId);
            return View(modelo);
        }

        // Post: Managers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var DataRemove = await _context.Cachiers.FindAsync(id);
                if (DataRemove == null)
                {
                    return NotFound();
                }

                if (DataRemove.Photo != null)
                {
                    var response = _fileStorage.DeleteImage(_container, DataRemove.Photo);
                    if (response != true)
                    {
                        return NotFound();
                    }
                }

                await _userHelper.DeleteUser(DataRemove.UserName!);

                _context.Remove(DataRemove);
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

        private async Task<Response> AcivateUser(Cachier modelo)
        {
            User user = await _userHelper.AddUserSystemAsync(modelo.FirstName!, modelo.LastName!,
            modelo.UserName!, modelo.PhoneNumber!, modelo.UserType,
            modelo.Address!, "Cachier", modelo.CorporateId, modelo.Photo!, "Cachier", modelo.Activo);


            //Envio de Correo con Token de seguridad para Verificar el correo
            string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
            string tokenLink = Url.Action("ConfirmEmail", "Account", new
            {
                userid = user.Id,
                token = myToken
            }, protocol: HttpContext.Request.Scheme)!;

            string subject = "Activacion de Cuenta";
            string body = ($"De: NexxtPlanet" +
                $"<h1>Email Confirmation</h1>" +
                $"<p>" +
                $"Su Clave Temporal es: <h2> \"{user.Pass}\"</h2>" +
                $"</p>" +
                $"Para Activar su vuenta, " +
                $"Has Click en el siguiente Link:</br></br><strong><a href = \"{tokenLink}\">Confirmar Correo</a></strong>");

            Response response = await _emailHelper.ConfirmarCuenta(user.UserName!, user.FullName!, subject, body);
            if (response.IsSuccess == false)
            {
                _notyfService.Custom("Vuelva a Intentarlo, el Usuario no ha sido Creado -  Notificacion", 5, "#D90000", "fa fa-trash");
                return response;
            }

            return response;
        }
        private bool CachierExists(int id)
        {
            return _context.Cachiers.Any(e => e.CachierId == id);
        }
    }
}
