using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tic.Shared.Entites;
using Tic.Shared.Enum;
using Tic.Shared.Responses;
using Tic.Web.Data;
using Tic.Web.Helpers;
using X.PagedList;

namespace Tic.Web.Controllers.Entities
{
    public class ManagersController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IFileStorage _fileStorage;
        private readonly IComboHelper _comboHelper;
        private readonly IEmailHelper _mailHelper;
        private readonly INotyfService _notyfService;
        private readonly string _container;

        public ManagersController(DataContext context,
            IUserHelper userHelper, IFileStorage fileStorage,
            IComboHelper comboHelper, IEmailHelper mailHelper, INotyfService notyfService)
        {
            _context = context;
            _userHelper = userHelper;
            _fileStorage = fileStorage;
            _comboHelper = comboHelper;
            _mailHelper = mailHelper;
            _notyfService = notyfService;
            _container = "wwwroot\\Images\\ImgUser";
        }

        [HttpPost]
        public JsonResult Search(string Prefix)
        {
            var datoMag = (from manager in _context.Managers
                           where manager.FullName!.ToLower().Contains(Prefix.ToLower())
                           select new
                           {
                               label = manager.FullName,
                               val = manager.ManagerId
                           }).ToList();

            return Json(datoMag);

        }


        // GET: Managers
        public async Task<IActionResult> Index(int? buscarId, int? page)
        {
            if (buscarId != null)
            {
                var dataContext = _context.Managers
                    .Include(c => c.Corporate)
                    .Where(c => c.ManagerId == buscarId);
                return View(await dataContext.OrderBy(c => c.Corporate!.Name).ThenBy(c => c.FullName).ToPagedListAsync(page ?? 1, 25));
            }
            else
            {

                var dataContext = _context.Managers
                    .Include(c => c.Corporate);
                return View(await dataContext.OrderBy(c => c.Corporate!.Name).ThenBy(c => c.FullName).ToPagedListAsync(page ?? 1, 25));
            }
        }

        // GET: Managers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Managers == null)
            {
                return NotFound();
            }

            var manager = await _context.Managers
                .Include(m => m.Corporate)
                .FirstOrDefaultAsync(m => m.ManagerId == id);
            if (manager == null)
            {
                return NotFound();
            }

            return View(manager);
        }

        // GET: Managers/Create
        public IActionResult Create()
        {
            Manager modelo = new()
            {
                UserType = UserType.User,
                Activo = true
            };

            modelo.ListCorporate = _comboHelper.GetComboCorporate();
            return View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Manager manager)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User ChaeckUser = await _userHelper.GetUserAsync(manager.UserName);
                    if (ChaeckUser != null)
                    {
                        _notyfService.Error("Este Correo ya se encuentra usado - Notificacion");
                        manager.ListCorporate = _comboHelper.GetComboCorporate();
                        return View(manager);
                    }

                    //Se realiza el proceso de auto RollBack para algun fallo de Guardadoplanes de internet
                    using var transaction = _context.Database.BeginTransaction();

                    //Guardamos el registro y si guarda sin errores seguimso con la imagen
                    manager.FullName = $"{manager.FirstName} {manager.LastName}";
                    _context.Add(manager);
                    await _context.SaveChangesAsync();

                    //Seguimos con imagen, de esta manera si hubo error en el guardado, la imagen no
                    if (manager.ImageFile != null)
                    {
                        manager.Photo = await _fileStorage.UploadImage(manager.ImageFile, ".jpg", _container, manager.Photo);
                    }
                    _context.Update(manager);
                    await _context.SaveChangesAsync();

                    if (manager.Activo == true)
                    {
                        Response response = await AcivateUser(manager);
                        if (response.IsSuccess == false)
                        {
                            string ruta = "wwwroot\\Images\\ImgUser";
                            var guid = manager.Photo;
                            _fileStorage.DeleteImage(ruta, guid!);

                            ModelState.AddModelError(string.Empty, "Vuelva a Intentarlo, el Usuario no ha sido Creado");
                            return RedirectToAction("Create", "Managers");
                        }
                    }

                    transaction.Commit();
                    //Se guardan todos los datos si todo esta successed.

                    _notyfService.Success("El Regitro se Guardado Con Exito -  Notificacion");
                    return RedirectToAction(nameof(Details), new { id = manager.ManagerId });
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

            manager.ListCorporate = _comboHelper.GetComboCorporate();
            return View(manager);
        }

        // GET: Managers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Managers == null)
            {
                return NotFound();
            }

            var manager = await _context.Managers.FindAsync(id);
            if (manager == null)
            {
                return NotFound();
            }

            manager.ListCorporate = _comboHelper.GetComboCorporate();
            return View(manager);
        }

        // POST: Managers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Manager manager)
        {
            if (id != manager.ManagerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Se realiza el proceso de auto RollBack para algun fallo de Guardadoplanes de internet
                    using var transaction = _context.Database.BeginTransaction();

                    manager.FullName = $"{manager.FirstName} {manager.LastName}";

                    if (manager.ImageFile != null)
                    {
                        if (manager.ImageFile != null)
                        {
                            manager.Photo = await _fileStorage.UploadImage(manager.ImageFile, ".jpg", _container, manager.Photo);
                        }
                    }

                    _context.Update(manager);
                    await _context.SaveChangesAsync();

                    var user = await _userHelper.GetUserAsync(manager.UserName);
                    user.Activo = manager.Activo;
                    if (manager.ImageFile != null)
                    {
                        user.Photo = manager.Photo;
                    }
                    IdentityResult response = await _userHelper.UpdateUserAsync(user);
                    if (!response.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, "Vuelva a Intentarlo, el Usuario no ha sido Creado");
                        return View(manager);
                    }

                    transaction.Commit();
                    //Se guardan todos los datos si todo esta successed.

                    _notyfService.Success("El Regitro se ha Actualizado con Exito -  Notificacion");
                    return RedirectToAction("Details", new { id = manager.ManagerId });
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
            manager.ListCorporate = _comboHelper.GetComboCorporate();
            return View(manager);
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
                var DataRemove = await _context.Managers.FindAsync(id);
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

                await _userHelper.DeleteUser(DataRemove.UserName);

                _context.Managers.Remove(DataRemove);
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


        private async Task<Response> AcivateUser(Manager manager)
        {
            User user = await _userHelper.AddUserSystemAsync(manager.FirstName, manager.LastName,
            manager.UserName, manager.PhoneNumber, manager.UserType,
            manager.Address, manager.Job, manager.CorporateId, manager.Photo!, "Manager", manager.Activo);


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

            Response response = await _mailHelper.ConfirmarCuenta(user.UserName!, user.FullName!, subject, body);
            if (response.IsSuccess == false)
            {
                ModelState.AddModelError(string.Empty, "Vuelva a Intentarlo, el Usuario no ha sido Creado");
                return response;
            }

            return response;
        }


        [HttpGet]
        public async Task<IActionResult> CheckEmail(string UserName)
        {
            var usuario = await _userHelper.GetUserAsync(UserName);
            if (usuario != null)
            {
                return Json($"Este Correo ya se encuentra Ocupado");
            }

            return Json(true);
        }

        private bool ManagerExists(int id)
        {
            return _context.Managers.Any(e => e.ManagerId == id);
        }
    }
}
