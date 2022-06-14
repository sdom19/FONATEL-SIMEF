using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;

using GB.SUTEL.Entities;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.BL.FuenteExternas;
using GB.SUTEL.UI.Helpers;
using GB.SUTEL.UI.Recursos.Utilidades;
using GB.SUTEL.Entities.Utilidades;
using System.Net;
using Microsoft.AspNet.Identity;
using GB.SUTEL.DAL;

namespace GB.SUTEL.UI.Controllers
{
    [AuthorizeUserAttribute]
    public class FuenteExternaController : BaseController
    {
        FuenteExternaBL refFuenteExternaBL;
        BitacoraWriter bitacora;
        Funcion func = new Funcion();
        public FuenteExternaController()
        {
            refFuenteExternaBL = new FuenteExternaBL(AppContext);
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
                func._index(user, "Fuentes Externas", "Fuentes Externas");
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
                Respuesta<List<FuenteExterna>> objRespuesta = new Respuesta<List<FuenteExterna>>();
                objRespuesta = refFuenteExternaBL.ConsultarTodos();
                ViewBag.searchTerm = new FuenteExterna();
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
        public ActionResult _table(FuenteExterna FuenteExterna)
        {
            try
            {
                Respuesta<List<FuenteExterna>> objRespuesta = new Respuesta<List<FuenteExterna>>();
                objRespuesta = refFuenteExternaBL.gFiltrarFuenteExterna(FuenteExterna.IdFuenteExterna,FuenteExterna.NombreFuenteExterna==null?"":FuenteExterna.NombreFuenteExterna);
                ViewBag.searchTerm = FuenteExterna;
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
        public ActionResult Crear([Bind(Include="NombreFuenteExterna")]Entities.FuenteExterna FuenteExterna)
        {
          //  string us;
          //  us = User.Identity.GetUserId();

           // func.fuentes(us, "Proceso de creación en: Fuentes Externas", 2,"Nombre:  "+FuenteExterna.NombreFuenteExterna.ToString(), " ");

            try
            {
                Respuesta<Entities.FuenteExterna> respuesta = new Respuesta<Entities.FuenteExterna>();
                JSONResult<Entities.FuenteExterna> jsonRespuesta = new JSONResult<Entities.FuenteExterna>();
                respuesta = refFuenteExternaBL.Agregar(FuenteExterna);

               
                if (respuesta.blnIndicadorState == 200)
                {
                    jsonRespuesta.strMensaje = respuesta.strMensaje;
                    bitacora.gfuente<FuenteExterna>(HttpContext, 2, respuesta.objObjeto, null, "mensaje");

                   
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
        // GET: FuenteExterna/Editar/5
        public ActionResult Editar(int? id)
        {
            try
            {
                if (id == null)
                    return HttpNotFound();
                FuenteExterna objFuenteExterna = refFuenteExternaBL.ConsultarPorExpresion(x => x.IdFuenteExterna == id).objObjeto;
                if (objFuenteExterna == null)
                    return HttpNotFound();
                return View(objFuenteExterna);
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

        // POST: FuenteExterna/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "IdFuenteExterna,NombreFuenteExterna")] FuenteExterna objFuenteExterna)
        {
            try
            {
                Respuesta<FuenteExterna[]> respuesta = new Respuesta<FuenteExterna[]>();
                JSONResult<FuenteExterna> jsonRespuesta = new JSONResult<FuenteExterna>();
                respuesta = refFuenteExternaBL.Editar(objFuenteExterna);
                if (respuesta.blnIndicadorState == 200)
                {
                    jsonRespuesta.strMensaje = respuesta.strMensaje;
                    bitacora.gfuente<FuenteExterna>(HttpContext, 3, respuesta.objObjeto[0], respuesta.objObjeto[1], "mensaje");
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

        // POST: Pantalla/Delete/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id)
        {
            try
            {
                Respuesta<FuenteExterna> respuesta = new Respuesta<FuenteExterna>();
                JSONResult<FuenteExterna> jsonRespuesta = new JSONResult<FuenteExterna>();
                try
                {
                    SUTEL.DAL.SUTEL_IndicadoresEntities db = new SUTEL_IndicadoresEntities();
                    FuenteExterna fuente = db.FuenteExterna.Find(id);
                    string nombre = fuente.NombreFuenteExterna.ToString();
                    string user;

                    //string fuente=respuesta.objObjeto.NombreFuenteExterna.ToString();
                    user = User.Identity.GetUserId();
                    func.fuentes(user, "Proceso de eliminación en: Fuentes Externas", 4, " ", "ID: " + id + ",  Nombre: " + nombre);
                }

                catch (Exception e)
                {
                    Console.WriteLine("{0} Exception caught.", e);
                }


                //bitacora.gfuente<FuenteExterna>(HttpContext, 4, null, respuesta.objObjeto, "mensaje");
                respuesta = refFuenteExternaBL.Eliminar(id);
                jsonRespuesta.strMensaje = respuesta.strMensaje;



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