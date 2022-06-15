using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Linq;

using GB.SUTEL.BL.Seguridad;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using System.Text;
using System.Web.UI;
using GB.SUTEL.ExceptionHandler;
using System.Net;
using System.Threading.Tasks;
using GB.SUTEL.UI.Models;
using Newtonsoft.Json;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using GB.SUTEL.UI.Recursos.Utilidades;
using GB.SUTEL.UI.Helpers;
using GB.SUTEL.BL.Mantenimientos;
/*Using para uso de Captcha BotDetect*/
using BotDetect.Web.Mvc;

namespace GB.SUTEL.UI.Controllers
{
    public class LoginController : BaseController
    {
        UsersBL refUserBL;
        RolBL objRolRef;
        BitacoraWriter bitacora;
        ServicioBL servicioBL;
        public LoginController()
        {
            refUserBL = new UsersBL(AppContext);
            objRolRef = new RolBL(AppContext);
            bitacora = new BitacoraWriter(AppContext);
            servicioBL = new ServicioBL(AppContext);
        }
        [AllowAnonymous]
        public ActionResult Index(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        /// <summary>
        /// Método para inicio de sesión y autenticación de credencial con control de tipo captcha
        /// </summary>
        /// <modificadopor>Kevin Hernández Arias</modificadopor>
        /// <fechaModificación>31/01/2018</fechaModificación>
        /// <param name="log">Usuario</param>
        /// <param name="returnUrl">URL</param>
        /// <returns>Validación de credenciales, validación de captcha, Reset Password, Home del aplicativo</returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index([Bind(Include = "AccesoUsuario,Contrasena")]Usuario log, string returnUrl)
        {
            try
            {
                if (log.Contrasena == null || log.AccesoUsuario == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.HttpVersionNotSupported, "Ingrese usuario y contraseña.");
                }
                //Instancia de captcha 
                MvcCaptcha mvcCaptcha = new MvcCaptcha("LoginCaptcha");
                // Obtener valor del captcha
                string userInput = HttpContext.Request.Form["LoginCaptchaCode"];
                // Obtener instancia de validación 
                string validatingInstanceId = HttpContext.Request.Form[mvcCaptcha.ValidatingInstanceKey];
                // Validar Captcha
                if (mvcCaptcha.Validate(userInput, validatingInstanceId))
                {
                    MvcCaptcha.ResetCaptcha("LoginCaptcha");
                    MailingModel message = new MailingModel()
                    {
                        UserName = log.AccesoUsuario,
                        Password = refUserBL.generatePassword(),
                        RutaSITEL = this.Request.ApplicationPath
                    };
                    string renderedEmail = RenderView.RenderViewToString("Emails", "PasswordChanged", message);
                    Usuario authUser = refUserBL.Login(log, renderedEmail, message.Password);

                    if ((authUser != null && (authUser.UsuarioLogin == null ? false :
                        (authUser.UsuarioLogin.Intentos == 3))) || (authUser != null && authUser.UsuarioInterno == 1))
                    {
                        var roles = authUser.Rol.Select(x => x.NombreRol).ToArray();

                        JsonSerializerSettings settings = new JsonSerializerSettings
                        {
                            PreserveReferencesHandling = PreserveReferencesHandling.Objects
                        };

                        var serializer = JsonSerializer.Create(settings);
                        var ctx = Request.GetOwinContext();
                        var authManager = ctx.Authentication;
                        authManager.SignOut("ApplicationCookie");
                        var identity = new ClaimsIdentity(new[] 
                        {
                            new Claim(ClaimTypes.Name,authUser.NombreUsuario),
                            new Claim(ClaimTypes.NameIdentifier,authUser.AccesoUsuario),
                            new Claim(ClaimTypes.Email, authUser.CorreoUsuario),
                            new Claim(ClaimTypes.Role, String.Join(",",roles)),
                            new Claim(ClaimTypes.Surname,authUser.UsuarioInterno.ToString())
                        },"ApplicationCookie");
                        authManager.SignIn(identity);
                        Session["usuarioDominio"] = authUser.AccesoUsuario;
                        //Se valida que el usuario que se loguea sea externo
                        if (authUser.UsuarioInterno == 0)
                        {
                            List<ServicioOperador> verificados = new List<ServicioOperador>();
                            verificados = servicioBL.ConsultarServicioOperador(authUser.IdOperador).objObjeto;
                            bool validar = (from x in verificados where x.Verificar == null select x).Any();
                            if (validar)
                            {
                                return Content("{\"url\": \"ConfiguracionServicios/Index\"}");
                            }
                            else
                            {
                                return Content("{\"url\": \"Home/Index\"}");
                            }
                        }
                        else
                        {

                            return Content("{\"url\": \"Home/Index\"}");
                        }
                    }
                    else
                    {
                        if (authUser != null && authUser.UsuarioLogin.Intentos == 0)
                            return new HttpStatusCodeResult(HttpStatusCode.HttpVersionNotSupported, GB.SUTEL.Shared.ErrorTemplate.PasswordReset);
                        else
                            return new HttpStatusCodeResult(HttpStatusCode.HttpVersionNotSupported, GB.SUTEL.Shared.ErrorTemplate.InvalidCredencials);
                    }
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "Vuelva a ingresar el Captcha.");
                }
            }
            catch (Exception ex)
            {
                if (ex is CustomException)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, String.Format(((CustomException)ex).Message, ((CustomException)ex).Id));
                else
                {
                    var newex = AppContext.ExceptionBuilder.BuildBusinessException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, String.Format(((CustomException)newex).Message, ((CustomException)newex).Id));
                }
            }
        }

        [HttpGet]
        public ActionResult LogOffByFrontEnd()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;
            Session["usuarioDominio"] = null;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult LogOff()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;
            Session["usuarioDominio"] = null;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword([Bind(Include = "AccesoUsuario,CorreoUsuario")] Usuario Usuario)
        {
            try
            {
                Respuesta<Usuario[]> respuesta = new Respuesta<Usuario[]>();
                JSONResult<Usuario> jsonRespuesta = new JSONResult<Usuario>();
                Usuario.Contrasena = refUserBL.generatePassword();
                MailingModel message = new MailingModel()
                {
                    UserName = Usuario.AccesoUsuario,
                    Password = Usuario.Contrasena
                };
                string renderedEmail = RenderView.RenderViewToString("Emails", "PasswordChanged", message);
                respuesta = refUserBL.ResetPass(Usuario,renderedEmail);
                if(respuesta.blnIndicadorState == 200)
                    bitacora.gRegistrarBitacora<Usuario>(HttpContext, 4, respuesta.objObjeto[0], respuesta.objObjeto[1]);
                jsonRespuesta.state =respuesta.blnIndicadorState;
                jsonRespuesta.strMensaje = respuesta.strMensaje;
                return  Content(jsonRespuesta.toJSON());
            }
            catch (Exception ex)
            {
                if (ex is CustomException)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, String.Format(((CustomException)ex).Message, ((CustomException)ex).Id));
                else
                {
                    var newex = AppContext.ExceptionBuilder.BuildBusinessException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, String.Format(((CustomException)newex).Message, ((CustomException)newex).Id));
                }
            }
        }
    }
}




