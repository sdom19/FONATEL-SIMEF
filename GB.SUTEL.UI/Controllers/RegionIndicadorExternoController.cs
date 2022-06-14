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
    public class RegionIndicadorExternoController : BaseController
    {
        Funcion func = new Funcion();
        RegionIndicadorExternoBL refRegionIndicadorExternoBL;
        BitacoraWriter bitacora;
        public RegionIndicadorExternoController()
        {
            refRegionIndicadorExternoBL = new RegionIndicadorExternoBL(AppContext);
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
                func._index(user, "Región Indicador Externo", "Región Indicador Externo");
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
                Respuesta<List<RegionIndicadorExterno>> objRespuesta = new Respuesta<List<RegionIndicadorExterno>>();
                objRespuesta = refRegionIndicadorExternoBL.ConsultarTodos();
                ViewBag.searchTerm = new RegionIndicadorExterno();
           
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
        public ActionResult _table(RegionIndicadorExterno RegionIndicadorExterno)
        {
            try
            {
                Respuesta<List<RegionIndicadorExterno>> objRespuesta = new Respuesta<List<RegionIndicadorExterno>>();
                objRespuesta = refRegionIndicadorExternoBL.gFiltrarRegionIndicadorExterno(RegionIndicadorExterno.IdRegionIndicadorExterno,RegionIndicadorExterno.DescRegionIndicadorExterno==null?"":RegionIndicadorExterno.DescRegionIndicadorExterno);
                ViewBag.searchTerm = RegionIndicadorExterno;
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
        public ActionResult Crear([Bind(Include = "DescRegionIndicadorExterno")]RegionIndicadorExterno RegionIndicadorExterno)
        {
            try
            {
                Respuesta<RegionIndicadorExterno> respuesta = new Respuesta<RegionIndicadorExterno>();
                JSONResult<RegionIndicadorExterno> jsonRespuesta = new JSONResult<RegionIndicadorExterno>();
                respuesta = refRegionIndicadorExternoBL.Agregar(RegionIndicadorExterno);
                if (respuesta.blnIndicadorState == 200)
                {
                    jsonRespuesta.strMensaje = respuesta.strMensaje;
                    bitacora.gRegistrarRegion<RegionIndicadorExterno>(HttpContext, 2, respuesta.objObjeto, null);
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
        // GET: RegionIndicadorExterno/Editar/5
        public ActionResult Editar(int? id)
        {
            try
            {
                if (id == null)
                    return HttpNotFound();
                RegionIndicadorExterno objRegionIndicadorExterno = refRegionIndicadorExternoBL.ConsultarPorExpresion(x => x.IdRegionIndicadorExterno == id).objObjeto;
                if (objRegionIndicadorExterno == null)
                    return HttpNotFound();
                return View(objRegionIndicadorExterno);
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

        // POST: RegionIndicadorExterno/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "IdRegionIndicadorExterno,DescRegionIndicadorExterno")] RegionIndicadorExterno objRegionIndicadorExterno)
        {
            try
            {
                Respuesta<RegionIndicadorExterno[]> respuesta = new Respuesta<RegionIndicadorExterno[]>();
                JSONResult<RegionIndicadorExterno> jsonRespuesta = new JSONResult<RegionIndicadorExterno>();
                respuesta = refRegionIndicadorExternoBL.Editar(objRegionIndicadorExterno);
                if (respuesta.blnIndicadorState == 200)
                {
                    jsonRespuesta.strMensaje = respuesta.strMensaje;
                    bitacora.gRegistrarRegion<RegionIndicadorExterno>(HttpContext, 3, respuesta.objObjeto[0], respuesta.objObjeto[1]);
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
                Respuesta<RegionIndicadorExterno> respuesta = new Respuesta<RegionIndicadorExterno>();
                JSONResult<RegionIndicadorExterno> jsonRespuesta = new JSONResult<RegionIndicadorExterno>();
                respuesta = refRegionIndicadorExternoBL.Eliminar(id);
                jsonRespuesta.strMensaje = respuesta.strMensaje;
                bitacora.gRegistrarRegion<RegionIndicadorExterno>(HttpContext, 4, null, respuesta.objObjeto);
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