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
    public class ZonaIndicadorExternoController : BaseController
    {
        Funcion func = new Funcion();
        ZonaIndicadorExternoBL refZonaIndicadorExternoBL;
        BitacoraWriter bitacora;
        public ZonaIndicadorExternoController()
        {
            refZonaIndicadorExternoBL = new ZonaIndicadorExternoBL(AppContext);
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
                func._index(user, "Zona Indicador Externo", "Zona Indicador Externo");
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
                Respuesta<List<ZonaIndicadorExterno>> objRespuesta = new Respuesta<List<ZonaIndicadorExterno>>();
                objRespuesta = refZonaIndicadorExternoBL.ConsultarTodos();
                ViewBag.searchTerm = new ZonaIndicadorExterno();
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
        public ActionResult _table(ZonaIndicadorExterno ZonaIndicadorExterno)
        {
            try
            {
                Respuesta<List<ZonaIndicadorExterno>> objRespuesta = new Respuesta<List<ZonaIndicadorExterno>>();
                objRespuesta = refZonaIndicadorExternoBL.gFiltrarZonaIndicadorExterno(ZonaIndicadorExterno.IdZonaIndicadorExterno,ZonaIndicadorExterno.DescZonaIndicadorExterno==null?"":ZonaIndicadorExterno.DescZonaIndicadorExterno);
                ViewBag.searchTerm = ZonaIndicadorExterno;
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
        public ActionResult Crear([Bind(Include = "DescZonaIndicadorExterno")]ZonaIndicadorExterno ZonaIndicadorExterno)
        {
            try
            {
                Respuesta<ZonaIndicadorExterno> respuesta = new Respuesta<ZonaIndicadorExterno>();
                JSONResult<ZonaIndicadorExterno> jsonRespuesta = new JSONResult<ZonaIndicadorExterno>();
                respuesta = refZonaIndicadorExternoBL.Agregar(ZonaIndicadorExterno);
                if (respuesta.blnIndicadorState == 200)
                {
                    jsonRespuesta.strMensaje = respuesta.strMensaje;
                    bitacora.gRegistrarZona<ZonaIndicadorExterno>(HttpContext, 2, respuesta.objObjeto, null,"mensaje");
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
        // GET: ZonaIndicadorExterno/Editar/5
        public ActionResult Editar(int? id)
        {
            try
            {
                if (id == null)
                    return HttpNotFound();
                ZonaIndicadorExterno objZonaIndicadorExterno = refZonaIndicadorExternoBL.ConsultarPorExpresion(x => x.IdZonaIndicadorExterno == id).objObjeto;
                if (objZonaIndicadorExterno == null)
                    return HttpNotFound();
                return View(objZonaIndicadorExterno);
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

        // POST: ZonaIndicadorExterno/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "IdZonaIndicadorExterno,DescZonaIndicadorExterno")] ZonaIndicadorExterno objZonaIndicadorExterno)
        {
            try
            {
                Respuesta<ZonaIndicadorExterno[]> respuesta = new Respuesta<ZonaIndicadorExterno[]>();
                JSONResult<ZonaIndicadorExterno> jsonRespuesta = new JSONResult<ZonaIndicadorExterno>();
                respuesta = refZonaIndicadorExternoBL.Editar(objZonaIndicadorExterno);
                if (respuesta.blnIndicadorState == 200)
                {
                    jsonRespuesta.strMensaje = respuesta.strMensaje;
                    bitacora.gRegistrarZona<ZonaIndicadorExterno>(HttpContext, 3, respuesta.objObjeto[0], respuesta.objObjeto[1], "mensaje");
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
                Respuesta<ZonaIndicadorExterno> respuesta = new Respuesta<ZonaIndicadorExterno>();
                JSONResult<ZonaIndicadorExterno> jsonRespuesta = new JSONResult<ZonaIndicadorExterno>();
                respuesta = refZonaIndicadorExternoBL.Eliminar(id);
                jsonRespuesta.strMensaje = respuesta.strMensaje;
                bitacora.gRegistrarZona<ZonaIndicadorExterno>(HttpContext, 4, null, respuesta.objObjeto, "mensaje");
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