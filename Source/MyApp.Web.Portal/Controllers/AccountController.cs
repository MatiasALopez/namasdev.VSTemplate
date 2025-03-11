using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

using namasdev.Core.Validation;
using namasdev.Web.Helpers;
using MyApp.Datos;
using MyApp.Negocio;
using MyApp.Web.Portal.ViewModels;

namespace MyApp.Web.Portal.Controllers
{
    [Authorize]
    public class AccountController : ControllerBase
    {
        public const string NAME = "Account";

        private ApplicationSignInManager _signInManager;

        private readonly IUsuariosRepositorio _usuariosRepositorio;
        private readonly IUsuariosNegocio _usuariosNegocio;
        private readonly ICorreosNegocio _correosNegocio;

        public AccountController(IUsuariosRepositorio usuariosRepositorio, IUsuariosNegocio usuariosNegocio, ICorreosNegocio correosNegocio)
        {
            Validador.ValidarArgumentRequeridoYThrow(usuariosRepositorio, nameof(usuariosRepositorio));
            Validador.ValidarArgumentRequeridoYThrow(usuariosNegocio, nameof(usuariosNegocio));
            Validador.ValidarArgumentRequeridoYThrow(correosNegocio, nameof(correosNegocio));

            _usuariosRepositorio = usuariosRepositorio;
            _usuariosNegocio = usuariosNegocio;
            _correosNegocio = correosNegocio;
        }

        public AccountController(ApplicationSignInManager signInManager)
        {
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        #region Acciones

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl, model.Email);

                case SignInStatus.LockedOut:
                    return View(Views.Shared.ACCESO_DENEGADO);

                case SignInStatus.RequiresVerification:
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Los datos ingresados no son válidos.");
                    return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            SignOutAndClearSession();

            return RedirectToAction(nameof(HomeController.Index), HomeController.NAME);
        }

        [AllowAnonymous]
        public ActionResult ActivarCuenta(string id, string code)
        {
            if (String.IsNullOrWhiteSpace(id) || String.IsNullOrWhiteSpace(code))
            {
                return View(Views.Shared.ERROR);
            }

            var usuarioEntidad = _usuariosRepositorio.Obtener(id);
            if (usuarioEntidad == null)
            {
                return View(Views.Shared.ERROR);
            }

            var model = new ActivarCuentaViewModel
            {
                Code = code,
                Email = usuarioEntidad.Email,
            };

            return View(model);
        }

        [HttpPost,
        AllowAnonymous]
        public async Task<ActionResult> ActivarCuenta(string id, ActivarCuentaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuarioEntidad = _usuariosRepositorio.Obtener(id);
                var usuarioIdentity = UserManager.FindById(id);
                if (usuarioEntidad == null || usuarioIdentity == null)
                {
                    return View(Views.Shared.ERROR);
                }

                var result = await UserManager.ConfirmEmailAsync(id, model.Code);
                if (result.Succeeded)
                {
                    result = UserManager.AddPassword(id, model.Password);
                    if (result.Succeeded)
                    {
                        IdentityResult resultado;

                        string emailAnterior = usuarioIdentity.Email;
                        bool emailEditado = !String.Equals(emailAnterior, model.Email, StringComparison.CurrentCultureIgnoreCase);
                        if (emailEditado)
                        {
                            usuarioIdentity.Email = usuarioIdentity.UserName =
                            usuarioEntidad.Email =
                                model.Email;
                            resultado = UserManager.Update(usuarioIdentity);
                            ValidarIdentityResult(resultado);
                        }

                        try
                        {
                            usuarioEntidad.UltimaModificacionPor = id;
                            usuarioEntidad.UltimaModificacionFecha = DateTime.Now;
                            _usuariosRepositorio.Actualizar(usuarioEntidad);
                        }
                        catch (Exception)
                        {
                            // NOTA: "rollback manual" de datos de Identity
                            usuarioIdentity.Email = usuarioIdentity.UserName = emailAnterior;
                            resultado = UserManager.Update(usuarioIdentity);
                            ValidarIdentityResult(resultado);

                            throw;
                        }

                        return View(Views.Account.ACTIVAR_CUENTA_CONFIRMACION);
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
                else
                {
                    AddErrors(result);
                }
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult OlvidoPassword()
        {
            return View();
        }

        //
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> OlvidoPassword(ForgotPasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await UserManager.FindByNameAsync(model.Email);
                    if (user == null)
                    {
                        throw new Exception("El email ingresado no pertenece a ningún usuario registrado.");
                    }

                    if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                    {
                        throw new Exception("Su cuenta no se encuentra activada aún.");
                    }

                    var usuarioEntidad = _usuariosRepositorio.Obtener(user.Id);

                    _correosNegocio.EnviarCorreoResetearPassword(
                        model.Email,
                        nombreYApellido: usuarioEntidad.ToString(),
                        resetearPasswordUrl: URLHelper.GenerarRutaAbsoluta(Url.Action(nameof(ResetearPassword), new { id = user.Id, code = UserManager.GeneratePasswordResetToken(user.Id) })));

                    return RedirectToAction(nameof(OlvidoPasswordConfirmacion));
                }
            }
            catch (Exception ex)
            {
                ControllerHelper.CargarMensajesError(ex.Message);
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult OlvidoPasswordConfirmacion()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetearPassword(string id, string code)
        {
            if (String.IsNullOrWhiteSpace(id) || String.IsNullOrWhiteSpace(code))
            {
                return View(Views.Shared.ERROR);
            }

            var usuario = UserManager.FindById(id);
            if (usuario == null)
            {
                return View(Views.Shared.ERROR);
            }

            var model = new ResetPasswordViewModel
            {
                Code = code,
                Email = usuario.Email
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetearPassword(string id, ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usuarioEntidad = _usuariosRepositorio.Obtener(id);

            var result = await UserManager.ResetPasswordAsync(usuarioEntidad.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return View(Views.Account.RESETEAR_PASSWORD_CONFIRMACION);
            }

            AddErrors(result);

            return View(model);
        }

        #endregion

        #region Metodos

        private void ValidarIdentityResult(IdentityResult resultado)
        {
            if (!resultado.Succeeded)
            {
                throw new Exception(String.Join(Environment.NewLine, resultado.Errors));
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl, string usuarioNombre)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(HomeController.Index), HomeController.NAME);
        }

        #endregion
    }
}