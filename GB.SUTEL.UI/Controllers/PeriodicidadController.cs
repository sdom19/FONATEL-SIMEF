using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;

using GB.SUTEL.Entities;
using GB.SUTEL.ExceptionHandler;

using GB.SUTEL.UI.Helpers;
using GB.SUTEL.UI.Recursos.Utilidades;
using GB.SUTEL.Entities.Utilidades;
using System.Net;
using GB.SUTEL.BL.FuenteExternas;
using Microsoft.AspNet.Identity;

namespace GB.SUTEL.UI.Controllers
{
    [AuthorizeUserAttribute]
    public class PeriodicidadController : BaseController
    {
        Funcion func = new Funcion();
        PeriodicidadBL refPeriodicidadBL;
        BitacoraWriter bitacora;
        public PeriodicidadController()
        {
            refPeriodicidadBL = new PeriodicidadBL(AppContext);
            bitacora = new BitacoraWriter(AppContext);
        }
        //
        // GET: /User/
        public ActionResult Index()

        {
            string user;
            user = User.Identity.GetUserId();
            try
            {
                func._index(user, "Periodicidad", "Periodicidad Fuentes Externas");
            }

           
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }
            return View();
        }
        public ActionResult _table()
        {
            try
            {
                Respuesta<List<Periodicidad>> objRespuesta = new Respuesta<List<Periodicidad>>();
                objRespuesta = refPeriodicidadBL.ConsultarTodos();
                ViewBag.searchTerm = new Periodicidad();
                return PartialView(objRespuesta.objObjeto);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = String.Format(cEx.Message, cEx.Id);
                return View();
            }
            catch (Exception ex)
            {
                var newEx = AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                ViewBag.Error = String.Format(((CustomException)newEx).Message,((CustomException)newEx).Id);
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _table(Periodicidad Periodicidad)
        {
            try
            {
                Respuesta<List<Periodicidad>> objRespuesta = new Respuesta<List<Periodicidad>>();
                objRespuesta = refPeriodicidadBL.gFiltrarPeriodicidad(Periodicidad.IdPeridiocidad,Periodicidad.DescPeriodicidad==null?"":Periodicidad.DescPeriodicidad);
                ViewBag.searchTerm = Periodicidad;
                return PartialView(objRespuesta.objObjeto);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = String.Format(cEx.Message, cEx.Id);
                return View();
            }
            catch (Exception ex)
            {
                var newEx = AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                ViewBag.Error = String.Format(((CustomException)newEx).Message, ((CustomException)newEx).Id);
                return View();
            }

        }
        public ActionResult Crear()
        {
            try
            {                
                return View();
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = String.Format(cEx.Message, cEx.Id);
                return View();
            }
            catch (Exception ex)
            {
                var newEx = AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                ViewBag.Error = String.Format(((CustomException)newEx).Message, ((CustomException)newEx).Id);
                return View();
            }
        }
        //
        // POST: /User/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear([Bind(Include="DescPeriodicidad")]Periodicidad Periodicidad)
        {
            try
            {
                Respuesta<Periodicidad> respuesta = new Respuesta<Periodicidad>();
                JSONResult<Periodicidad> jsonRespuesta = new JSONResult<Periodicidad>();
                respuesta = refPeriodicidadBL.Agregar(Periodicidad);
                if (respuesta.blnIndicadorState == 200)
                {
                    jsonRespuesta.strMensaje = respuesta.strMensaje;
                    bitacora.gRegistrarBitacora<Periodicidad>(HttpContext, 2, respuesta.objObjeto, null, "mensaje");
                }
                else
                    return new HttpStatusCodeResult(HttpStatusCode.Conflict, respuesta.strMensaje);
                return Content(jsonRespuesta.toJSON());
            }
            catch (Exception ex)
            {
                if (!(ex is CustomException))
                    AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, String.Format(((CustomException)ex).Message, ((CustomException)ex).Id));
            }
        }
        //
        // GET: Periodicidad/Editar/5
        public ActionResult Editar(int? id)
        {
            try
            {
                if (id == null)
                    return HttpNotFound();
                Periodicidad objPeriodicidad = refPeriodicidadBL.ConsultarPorExpresion(x => x.IdPeridiocidad == id).objObjeto;
                if (objPeriodicidad == null)
                    return HttpNotFound();
                return View(objPeriodicidad);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = String.Format(cEx.Message, cEx.Id);
                return View();
            }
            catch (Exception ex)
            {
                var newEx = AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                ViewBag.Error = String.Format(((CustomException)newEx).Message, ((CustomException)newEx).Id);
                return View();
            }
        }

        // POST: Periodicidad/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "IdPeridiocidad,DescPeriodicidad")] Periodicidad objPeriodicidad)
        {
            try
            {
                Respuesta<Periodicidad[]> respuesta = new Respuesta<Periodicidad[]>();
                JSONResult<Periodicidad> jsonRespuesta = new JSONResult<Periodicidad>();
                respuesta = refPeriodicidadBL.Editar(objPeriodicidad);
                if (respuesta.blnIndicadorState == 200)
                {
                    jsonRespuesta.strMensaje = respuesta.strMensaje;
                    bitacora.gRegistrarBitacora<Periodicidad>(HttpContext, 3, respuesta.objObjeto[0], respuesta.objObjeto[1], "mensaje");
                }
                else
                    return new HttpStatusCodeResult(HttpStatusCode.Conflict, respuesta.strMensaje);
                return Content(jsonRespuesta.toJSON());
            }
            catch (Exception ex)
            {
                if (!(ex is CustomException))
                    AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, String.Format(((CustomException)ex).Message, ((CustomException)ex).Id));
            }
        }

        // POST: Pantalla/Delete/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id)
        {
            try
            {
                Respuesta<Periodicidad> respuesta = new Respuesta<Periodicidad>();
                JSONResult<Periodicidad> jsonRespuesta = new JSONResult<Periodicidad>();
                respuesta = refPeriodicidadBL.Eliminar(id);
                jsonRespuesta.strMensaje = respuesta.strMensaje;
                bitacora.gRegistrarBitacora<Periodicidad>(HttpContext, 4, null, respuesta.objObjeto, "mensaje");
                return Content(jsonRespuesta.toJSON());
            }
            catch (Exception ex)
            {
                if (!(ex is CustomException))
                    AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, String.Format(((CustomException)ex).Message, ((CustomException)ex).Id));
            }
        }
    }
}