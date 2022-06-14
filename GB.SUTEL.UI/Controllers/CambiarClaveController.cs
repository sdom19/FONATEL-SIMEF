using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;

using GB.SUTEL.Entities;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.BL.Seguridad;
using GB.SUTEL.UI.Helpers;
using GB.SUTEL.UI.Recursos.Utilidades;
using GB.SUTEL.Entities.Utilidades;
using System.Net;
using System.Security.Claims;


namespace GB.SUTEL.UI.Controllers
{
    [AuthorizeUserAttribute]
    public class CambiarClaveController : BaseController
    {
        UsersBL UsuariosBL;
        BitacoraWriter bitacora;
        public CambiarClaveController()
        {
            UsuariosBL = new UsersBL(AppContext);
            bitacora = new BitacoraWriter(AppContext);
        }
        public ActionResult Index(bool? cp)
        {
            if (cp == true)
                ViewBag.FirstChange = "Debe cambiar la contrase&ntilde;a para continuar.";
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Usuario Usuario,string oldPassword)
        {
            {
                Respuesta<Usuario[]> respuesta = new Respuesta<Usuario[]>();
                JSONResult<Usuario> jsonRespuesta = new JSONResult<Usuario>();
                try
                {
                    var user = ((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault();
                    Usuario.AccesoUsuario = user==null?"":user.Value;
                    respuesta = UsuariosBL.ChangePass(Usuario, oldPassword);
                    jsonRespuesta.state = respuesta.blnIndicadorState;
                    if (respuesta.blnIndicadorState != 200)
                        jsonRespuesta.strMensaje = respuesta.strMensaje;
                    else
                        jsonRespuesta.strMensaje = "{\"url\": \"Home/Index\",\"msg\":\"" + respuesta.strMensaje + "\"}";
                }
                catch (Exception ex)
                {
                    jsonRespuesta.state = 500;                    
                    if (!(ex is CustomException))
                    {
                        AppContext.ExceptionBuilder.BuildUIException(ex.Message,ex);
                        jsonRespuesta.strMensaje = String.Format(GB.SUTEL.Shared.ErrorTemplate.InternalErrorUI, "cambio de contraseña");
                    }
                    else
                        jsonRespuesta.strMensaje = String.Format(((CustomException)ex).Message, ((CustomException)ex).Id);
                }

                return Content(jsonRespuesta.toJSON());
            }

        }
        
    }
}