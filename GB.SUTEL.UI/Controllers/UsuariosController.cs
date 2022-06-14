using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using GB.SUTEL.BL.Seguridad;
using GB.SUTEL.BL.Mantenimientos;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using System.Net;
using System.IO;
using System.Text;
using System.Web.UI;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.UI.Helpers;
using GB.SUTEL.UI.Recursos.Utilidades;
using System.Security.Claims;
using System.Net.Mail;
using System.Web.Routing;
using GB.SUTEL.UI.Models;
using Microsoft.AspNet.Identity;

namespace GB.SUTEL.UI.Controllers
{
    [AuthorizeUserAttribute]
    public class UsuariosController : BaseController
    {
        Funcion func = new Funcion();
        UsersBL refUserBL;
        RolBL refRolBl;
        BitacoraWriter bitacora;
        String tipoSitio = "https";
        public UsuariosController()
        {
            refUserBL = new UsersBL(AppContext);
            refOperadorBL = new OperadorBL(AppContext);
            refRolBl = new RolBL(AppContext);
            bitacora = new BitacoraWriter(AppContext);
        }
        #region atributos

        OperadorBL refOperadorBL;
        #endregion
        //
        // GET: /User/        
        public ActionResult Index()
        {
            try
            {


                string user;
                user = User.Identity.GetUserId();
                try
                {
                    func._index(user, "Usuarios", "Usuarios Seguridad");
                }
                
                catch (Exception e)
                {
                    Console.WriteLine("{0} Exception caught.", e);
                }




                List<Rol> Roles = refRolBl.ConsultarTodos().objObjeto;
                return View(Roles);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return View();
            }
            catch (Exception ex)
            {
                var newEx = AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                ViewBag.Error = ((CustomException)newEx);
                return View();
            }
        }
        public ActionResult _table()
        {
            try
            {
                Respuesta<List<Usuario>> objRespuesta = new Respuesta<List<Usuario>>();
                objRespuesta = refUserBL.ConsultarTodos();
                ViewBag.searchTerm = new Usuario();
                return PartialView(objRespuesta.objObjeto);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return View();
            }
            catch (Exception ex)
            {
                var newEx = AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                ViewBag.Error = ((CustomException)newEx);
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _table(Usuario user)
        {
            try
            {
                Respuesta<List<Usuario>> objRespuesta = new Respuesta<List<Usuario>>();
                objRespuesta = refUserBL.gFiltrarUsuarios((user.AccesoUsuario == null ? "" : user.AccesoUsuario),
                    (user.NombreUsuario == null ? "" : user.NombreUsuario),
                    (user.CorreoUsuario == null ? "" : user.CorreoUsuario), "");
                //    (Contrasena == null ? "" : Contrasena));
                //Contrasena = user.IdOperador;
                ViewBag.searchTerm = user;
                return PartialView(objRespuesta.objObjeto);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return View();
            }
            catch (Exception ex)
            {
                var newEx = AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                ViewBag.Error = ((CustomException)newEx);
                return View();
            }

        }
        public ActionResult Crear()
        {
            try
            {
                ViewBag.Operadores = refOperadorBL.ConsultarTodos().objObjeto;
                return View();
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return View();
            }
            catch (Exception ex)
            {
                var newEx = AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                ViewBag.Error = ((CustomException)newEx);
                return View();
            }
        }
        //
        // POST: /User/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Usuario nuevoUser, string ACCESOAD, string NOMBREAD, string ISACTIVE)
        {
            Respuesta<Usuario> respuesta = new Respuesta<Usuario>();
            JSONResult<Usuario> jsonRespuesta = new JSONResult<Usuario>();
            try
            {
                string host = Request.Url.Host;

                nuevoUser.Contrasena = refUserBL.generatePassword();
                string renderedEmail = "";
                if (nuevoUser.UsuarioInterno.Equals((byte)1))
                {
                    nuevoUser.AccesoUsuario = ACCESOAD;
                }
                MailingModel message = new MailingModel()
                {
                    UserName = nuevoUser.AccesoUsuario.Trim(),
                    Password = nuevoUser.Contrasena.Trim(),
                    ImagenSutel = tipoSitio + "://" + host + "/Content/Images/logos/logo-Sutel_11_3.png",
                    RutaSistema = tipoSitio + "://" + host + "/"
                };

                if (ACCESOAD != null && ACCESOAD != "")
                {
                    renderedEmail = RenderView.RenderViewToString("Emails", "UserCreatedFromActiveDirectory", message);
                }
                else
                {

                    renderedEmail = RenderView.RenderViewToString("Emails", "UserCreated", message);
                }
                respuesta = refUserBL.Agregar(nuevoUser, ACCESOAD, NOMBREAD, ISACTIVE, renderedEmail);

                jsonRespuesta.strMensaje = respuesta.strMensaje;
                if (respuesta.blnIndicadorState != 200)
                    return new HttpStatusCodeResult(HttpStatusCode.Conflict, respuesta.strMensaje);
                bitacora.gRegistrarBitacora<Usuario>(HttpContext, 2, respuesta.objObjeto, null, "mensaje");
                return Content(jsonRespuesta.toJSON());
            }
            catch (Exception ex)
            {
                if (!(ex is CustomException))
                {
                    AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, String.Format(GB.SUTEL.Shared.ErrorTemplate.InternalErrorUI, "usuarios"));
                }
                else
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, String.Format(((CustomException)ex).Message, ((CustomException)ex).Id));
            }
        }
        //
        // GET: Usuarios/Editar/5        
        public ActionResult Editar(int? id)
        {
            try
            {
                ADEntryBL refADentryBL = new ADEntryBL(AppContext);
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Usuario objUsuario = refUserBL.ConsultarPorExpresion(id ?? default(int)).objObjeto;
                if (objUsuario == null)
                {
                    return HttpNotFound();
                }
                if (objUsuario.NombreUsuario == User.Identity.Name)
                {
                    ViewBag.UsuarioActual = true;
                }
                else
                {
                    ViewBag.UsuarioActual = false;
                }
                ViewBag.UsuariosAD = refADentryBL.ConsultarTodos().objObjeto;
                ViewBag.Operadores = refOperadorBL.ConsultarTodos().objObjeto;
                return View(objUsuario);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = String.Format(cEx.Message, cEx.Id);
                return View();
            }
            catch (Exception ex)
            {
                var newEx = AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                ViewBag.Error = String.Format(((CustomException)ex).Message, ((CustomException)ex).Id);
                return View();
            }
        }

        // POST: Usuarios/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string Editar([Bind(Include = "IdUsuario,IdOperador,AccesoUsuario,NombreUsuario,Contrasena,CorreoUsuario,UsuarioInterno,Activo")] Usuario objUsuario,
         string ACCESOAD, string NOMBREAD, string ISACTIVE)
        {
            Respuesta<Usuario[]> respuesta = new Respuesta<Usuario[]>();
            JSONResult<Usuario> jsonRespuesta = new JSONResult<Usuario>();
            try
            {
                ADEntryBL refADentryBL = new ADEntryBL(AppContext);
                respuesta = refUserBL.Editar(objUsuario, ACCESOAD, NOMBREAD, ISACTIVE);
                if (respuesta.blnIndicadorState == 300)
                    jsonRespuesta.strMensaje = respuesta.strMensaje;
                else if (respuesta.blnIndicadorState == 301)
                {

                }
                else
                {
                    jsonRespuesta.strMensaje = "{ \"url\":\"" +
                        this.HttpContext.Request.RequestContext.RouteData.GetRequiredString("controller") + "\", \"msg\":\"Usuario actualizado correctamente\"}";
                    bitacora.gRegistrarBitacora<Usuario>(HttpContext, 3, respuesta.objObjeto[0], respuesta.objObjeto[1], "mensaje");
                }
                jsonRespuesta.data = null;
                jsonRespuesta.state = respuesta.blnIndicadorState;
            }
            catch (Exception ex)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.state = 500;
                jsonRespuesta.strMensaje = String.Format(((CustomException)ex).Message, ((CustomException)ex).Id);
                if (!(ex is CustomException))
                    AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
            }
            return jsonRespuesta.toJSON();
        }

        // POST: Pantalla/Delete/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar([Bind(Include = "IdUsuario")]Usuario objUsuario)
        {
            Respuesta<Usuario> respuesta = new Respuesta<Usuario>();
            JSONResult<Usuario> jsonRespuesta = new JSONResult<Usuario>();
            try
            {
                var user = ((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault();
                Usuario currentUser = refUserBL.ConsultarPorExpresion(objUsuario.IdUsuario).objObjeto;
                if ((user == null ? "" : user.Value) == currentUser.AccesoUsuario)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, GB.SUTEL.Shared.ErrorTemplate.CantDeleteUser);
                respuesta = refUserBL.Eliminar(objUsuario);
                if (respuesta.blnIndicadorState == 500)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, GB.SUTEL.Shared.ErrorTemplate.CantDeleteUser);
                jsonRespuesta.strMensaje = respuesta.strMensaje;
                jsonRespuesta.data = null;
                jsonRespuesta.state = respuesta.blnIndicadorState;
                bitacora.gRegistrarBitacora<Usuario>(HttpContext, 4, null, respuesta.objObjeto, "mensaje");
            }
            catch (Exception ex)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.state = 500;
                jsonRespuesta.strMensaje = String.Format(GB.SUTEL.Shared.ErrorTemplate.InternalErrorUI, "Usuario");
                if (!(ex is CustomException))
                    AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                else
                    jsonRespuesta.strMensaje = String.Format(((CustomException)ex).Message, ((CustomException)ex).Id);
            }
            return Content(jsonRespuesta.toJSON());
        }

        public string getADUsers()
        {
            ADEntryBL refADentryBL = new ADEntryBL(AppContext);
            Respuesta<string> respuesta = new Respuesta<string>();
            JSONResult<string> jsonRespuesta = new JSONResult<string>();
            try
            {
                respuesta = refADentryBL.ConsultarTodos();
                jsonRespuesta.data = respuesta.objObjeto;
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                jsonRespuesta.state = 500;
                jsonRespuesta.strMensaje = String.Format(cEx.Message, cEx.Id);
                jsonRespuesta.data = "<option value>No se cargaron los datos</option>";
            }
            catch (Exception ex)
            {
                var newEx = AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                jsonRespuesta.state = 500;
                jsonRespuesta.strMensaje = String.Format(ex.Message, ((CustomException)ex).Id);
                jsonRespuesta.data = "<option value>No se cargaron los datos</option>";
            }
            return jsonRespuesta.toJSON();
        }
        // POST: Usuarios/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string EditarRoles(int IdUsuario, string[] ROLES)
        {
            Respuesta<Usuario> respuesta = new Respuesta<Usuario>();
            JSONResult<Usuario> jsonRespuesta = new JSONResult<Usuario>();
            try
            {
                refUserBL.AgregarRol(IdUsuario, ROLES);
                jsonRespuesta.strMensaje = "Roles agregados correctamente.";
            }
            catch (Exception ex)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.state = 500;
                jsonRespuesta.strMensaje = String.Format(((CustomException)ex).Message, ((CustomException)ex).Id);
                if (!(ex is CustomException))
                    AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
            }
            return jsonRespuesta.toJSON();
        }
    }
}