using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;

using GB.SUTEL.Entities;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.BL.FuenteExternas;
using GB.SUTEL.BL.Mantenimientos;
using GB.SUTEL.UI.Helpers;
using GB.SUTEL.UI.Recursos.Utilidades;
using GB.SUTEL.Entities.Utilidades;
using System.Net;
using GB.SUTEL.BL.FuenteExternas;
using Microsoft.AspNet.Identity;

namespace GB.SUTEL.UI.Controllers
{
    [AuthorizeUserAttribute]
    public class IndicadorExternoController : BaseController
    {
        Funcion func = new Funcion();
        IndicadorExternoBL refIndicadorExternoBL;
        BitacoraWriter bitacora;
        public IndicadorExternoController()
        {
            refIndicadorExternoBL = new IndicadorExternoBL(AppContext);
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
                func._index(user, "Indicador Externo", "Indicador Externo Fuentes Externas");
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
                Respuesta<List<IndicadorExterno>> objRespuesta = new Respuesta<List<IndicadorExterno>>();
                objRespuesta = refIndicadorExternoBL.ConsultarTodos();
                ViewBag.searchTerm = new string[3];
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
        public ActionResult _table(string nombreFuente, string nombreIndicador, string IdIndicadorExterno)
        {
            try
            {
                Respuesta<List<IndicadorExterno>> objRespuesta = new Respuesta<List<IndicadorExterno>>();
                objRespuesta = refIndicadorExternoBL.gFiltrarIndicadorExterno(nombreFuente == null ? "" : nombreFuente, nombreIndicador == null ? "" : nombreIndicador, null, IdIndicadorExterno == null ? "" : IdIndicadorExterno);
                ViewBag.searchTerm = new string[3] { nombreFuente, nombreIndicador, IdIndicadorExterno };
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
                FuenteExternaBL objFuenteExternaBL = new FuenteExternaBL(AppContext);
                TipoValorBL objTipoValorBL = new TipoValorBL(AppContext);
                ViewBag.FuentesExternas = objFuenteExternaBL.ConsultarTodos().objObjeto;
                ViewBag.TipoValores = objTipoValorBL.gObtenerTiposValor().objObjeto;
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
        public ActionResult Crear([Bind(Include = "IdIndicadorExterno,IdFuenteExterna,IdTipoValor,Nombre,Descripcion")]IndicadorExterno IndicadorExterno)
        {
            try
            {
                Respuesta<IndicadorExterno> respuesta = new Respuesta<IndicadorExterno>();
                JSONResult<IndicadorExterno> jsonRespuesta = new JSONResult<IndicadorExterno>();
                respuesta = refIndicadorExternoBL.Agregar(IndicadorExterno);
                if (respuesta.blnIndicadorState == 200)
                {
                    jsonRespuesta.strMensaje = respuesta.strMensaje;
                    bitacora.gRegistrarIndicador<IndicadorExterno>(HttpContext, 2, respuesta.objObjeto, null);
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
        // GET: IndicadorExterno/Editar/5
        public ActionResult Editar(string id)
        {
            try
            {
                if (id == null)
                    return HttpNotFound();
                IndicadorExterno objIndicadorExterno = refIndicadorExternoBL.ConsultarPorExpresion(x => x.IdIndicadorExterno == id).objObjeto;
                if (objIndicadorExterno == null)
                    return HttpNotFound();
                FuenteExternaBL objFuenteExternaBL = new FuenteExternaBL(AppContext);
                TipoValorBL objTipoValorBL = new TipoValorBL(AppContext);
                ViewBag.FuentesExternas = objFuenteExternaBL.ConsultarTodos().objObjeto;
                ViewBag.TipoValores = objTipoValorBL.gObtenerTiposValor().objObjeto;
                return View(objIndicadorExterno);
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

        // POST: IndicadorExterno/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "IdIndicadorExterno,IdFuenteExterna,IdTipoValor,Nombre,Descripcion")] IndicadorExterno objIndicadorExterno)
        {
            Respuesta<IndicadorExterno[]> respuesta = new Respuesta<IndicadorExterno[]>();
            JSONResult<IndicadorExterno> jsonRespuesta = new JSONResult<IndicadorExterno>();
            try
            {
                respuesta = refIndicadorExternoBL.Editar(objIndicadorExterno);
                if (respuesta.blnIndicadorState != 200)
                    return new HttpStatusCodeResult(HttpStatusCode.Conflict, respuesta.strMensaje);
                else
                {
                    jsonRespuesta.strMensaje = "{ \"url\":\"" +
                        this.HttpContext.Request.RequestContext.RouteData.GetRequiredString("controller") + "\", \"msg\":\""+respuesta.strMensaje+"\"}";
                    bitacora.gRegistrarIndicador<IndicadorExterno>(HttpContext, 3, respuesta.objObjeto[0],respuesta.objObjeto[1]);
                }
            }
            catch (Exception ex)
            {   
                if (!(ex is CustomException))
                    AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, String.Format(((CustomException)ex).Message, ((CustomException)ex).Id));
            }
            return Content(jsonRespuesta.toJSON());            
        }

        // POST: Pantalla/Delete/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(string id)
        {
            try
            {
                Respuesta<IndicadorExterno> respuesta = new Respuesta<IndicadorExterno>();
                JSONResult<IndicadorExterno> jsonRespuesta = new JSONResult<IndicadorExterno>();
                respuesta = refIndicadorExternoBL.Eliminar(id);
                jsonRespuesta.strMensaje = respuesta.strMensaje;
                bitacora.gRegistrarIndicador<IndicadorExterno>(HttpContext, 4, null,respuesta.objObjeto);
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