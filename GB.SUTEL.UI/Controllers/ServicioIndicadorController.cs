using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using GB.SUTEL.Entities;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.BL.Mantenimientos;
using GB.SUTEL.UI.Helpers;
using GB.SUTEL.UI.Recursos.Utilidades;
using GB.SUTEL.Entities.Utilidades;
using System.Net;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;

namespace GB.SUTEL.UI.Controllers
{
    [AuthorizeUserAttribute]
    public class ServicioIndicadorController : BaseController
    {
        Funcion func = new Funcion();
        ServicioIndicadorBL ServicioIndicadorBL;
        BitacoraWriter bitacora;
        public ServicioIndicadorController()
        {
            ServicioIndicadorBL = new ServicioIndicadorBL(AppContext);
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
                func._index(user, "Servicios Tipo Indicador", "Servicios Indicador Mantenimiento");
            }
           
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }
            return View();
        }
        public ActionResult _table(string Servicio = null, string Indicador = null, string direccion = null)
        {
            try
            {
                ViewBag.searchTerm = new string[2] { Servicio, Indicador };
                return PartialView(ServicioIndicadorBL.Filtrar(Servicio == null ? "" : Servicio, Indicador == null ? "" : Indicador, direccion == null ? "" : direccion));
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = String.Format(GB.SUTEL.Shared.ErrorTemplate.InternalErrorUI, "servicio indicador");
                return PartialView();
            }
            catch (Exception ex)
            {
                var newEx = AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                ViewBag.Error = String.Format(((CustomException)newEx).Message, ((CustomException)newEx).Id);
                return PartialView();
            }

        }
        public ActionResult _tableIndicadores(string Nombre = "", int? searchid = null)
        {
            try
            {
                IndicadorCruzadoBL refIndicadorCruzadoBL = new IndicadorCruzadoBL(AppContext);
                if (searchid != null)
                    //  ViewBag.IndicadoresSeleccionados = JsonConvert.SerializeObject(ServicioIndicadorBL.ObtenerIndicadoresSeleccionados(searchid ?? 0));
                    ViewBag.searchTerm = Nombre;
                   // ViewBag.searchTerm = new string[2] { Nombre, searchid };
                 //return PartialView(refIndicadorCruzadoBL.gFiltrarIndicadorInterno(Nombre, searchid ?? 0));
                return PartialView(refIndicadorCruzadoBL.ConsultarTodosIndicadores());
                // return PartialView(ServicioIndicadorBL.ObtenerIndicadoresSeleccionados(searchid ?? 0));
            }
            catch (CustomException Ex)
            {
                ViewBag.Error = String.Format(GB.SUTEL.Shared.ErrorTemplate.InternalErrorUI, "servicio indicador");
                return PartialView();
            }
            catch (Exception ex)
            {
                var newEx = AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                ViewBag.Error = String.Format(((CustomException)newEx).Message, ((CustomException)newEx).Id);
                return PartialView();
            }
        }


        public ActionResult Crear()
        {
            try
            {
                ViewBag.Servicios = ServicioIndicadorBL.ConsultarTodosServicios();
               // ViewBag.Servicios = ServicioIndicadorBL.ObtenerIndicadoresSeleccionados();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(int Servicio, string[] INDICADORES)
        {
            try
            {
                Respuesta<Servicio> respuesta = new Respuesta<Servicio>();
                JSONResult<Servicio> jsonRespuesta = new JSONResult<Servicio>();
                respuesta = ServicioIndicadorBL.AgregarEditar(Servicio, INDICADORES);
                jsonRespuesta.strMensaje = respuesta.strMensaje;
                if (respuesta.blnIndicadorState == 200)
                    bitacora.gRegistrarBitacora<Servicio>(HttpContext, 1, respuesta.objObjeto, null, "mensaje");
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
        public ActionResult Editar(int? id)
        {
            try
            {
                if (id == null)
                    return HttpNotFound();
                ViewBag.Servicios = ServicioIndicadorBL.ConsultarTodosServicios();
                ViewBag.IsEdit = true;
                return View(id);
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


        // POST: Pantalla/Delete/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int IdServicio, string IdIndicador)
        {
            try
            {
                Respuesta<Servicio> respuesta = new Respuesta<Servicio>();
                JSONResult<Servicio> jsonRespuesta = new JSONResult<Servicio>();
                respuesta = ServicioIndicadorBL.Eliminar(IdServicio, IdIndicador);
                jsonRespuesta.strMensaje = respuesta.strMensaje;
                bitacora.gRegistrarBitacora<Servicio>(HttpContext, 3, null, respuesta.objObjeto, "mensaje");
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