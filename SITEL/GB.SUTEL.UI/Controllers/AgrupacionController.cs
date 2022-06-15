using GB.SUTEL.BL.Mantenimientos;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.UI.Recursos.Utilidades;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GB.SUTEL.ExceptionHandler;
using System.Text;
using GB.SUTEL.UI.Helpers;
using GB.SUTEL.UI.Recursos.Utilidades;
using Microsoft.AspNet.Identity;


namespace GB.SUTEL.UI.Controllers
{
    public class AgrupacionController : BaseController
    {
        #region atributos
        AgrupacionBL refAgrupacionBL;
        Funcion func = new Funcion();

        #endregion

        #region eventos
        public AgrupacionController()
        {
            refAgrupacionBL = new AgrupacionBL(AppContext);
        }

        /// <summary>
        /// Index de la pantalla
        /// </summary>
        /// <returns></returns>

        public ActionResult Index()
        {

            string user;
            user = User.Identity.GetUserId();
            try
            {
                func._index(user, "Agrupación", "Agrupación Mantenimiento");
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }

            return View(new Tuple<List<Agrupacion>, Agrupacion>(new List<Agrupacion>(), new Agrupacion()));

        }

        /// <summary>
        /// Listado de la pantalla
        /// </summary>
        /// <returns></returns>
        public ActionResult _table()
        {
            Respuesta<List<Agrupacion>> respuesta = new Respuesta<List<Agrupacion>>();
            Agrupacion agrupacion = new Agrupacion();
            try
            {


                respuesta = refAgrupacionBL.gObtenerAgrupaciones();
                ViewBag.searchTerm = new Agrupacion();
                return PartialView(new Tuple<List<Agrupacion>, Agrupacion>(respuesta.objObjeto, agrupacion));
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView(new Tuple<List<Agrupacion>, Agrupacion>(respuesta.objObjeto, agrupacion));
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView(new Tuple<List<Agrupacion>, Agrupacion>(respuesta.objObjeto, agrupacion));
            }
        }
        /// <summary>
        /// Listado de la pantalla con filtros
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _table(Agrupacion searchTerm)
        {
            Respuesta<List<Agrupacion>> respuesta = new Respuesta<List<Agrupacion>>();
            Agrupacion agrupacion = new Agrupacion();
            try
            {
                respuesta = refAgrupacionBL.gObtenerAgrupacionesPorFiltros(searchTerm.IdAgrupacion, searchTerm.DescAgrupacion != null ? searchTerm.DescAgrupacion : "");
                ViewBag.searchTerm = searchTerm;
                return PartialView(new Tuple<List<Agrupacion>, Agrupacion>(respuesta.objObjeto, agrupacion));
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView(new Tuple<List<Agrupacion>, Agrupacion>(respuesta.objObjeto, agrupacion));
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView(new Tuple<List<Agrupacion>, Agrupacion>(respuesta.objObjeto, agrupacion));
            }
        }
        /// <summary>
        /// LLamada al crear
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CrearAgrupacion()
        {

            return PartialView();
        }

        /// <summary>
        /// Llamada al guardar del crear
        /// </summary>
        /// <param name="nuevaAgrupacion"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("CrearAgrupacion")]
        public String CrearAgrupacion(Agrupacion nuevaAgrupacion)
        {

            Respuesta<Agrupacion> respuesta = new Respuesta<Agrupacion>();
            JSONResult<Agrupacion> jsonRespuesta = new JSONResult<Agrupacion>();
            jsonRespuesta.ok = false;
            try
            {

                respuesta = refAgrupacionBL.gAgregarAgrupacion(nuevaAgrupacion);
                if (respuesta.blnIndicadorTransaccion)
                {
                    
                    func.agrupacionbit(ActionsBinnacle.Crear, respuesta.objObjeto, null);


                    jsonRespuesta.data = respuesta.objObjeto;
                    jsonRespuesta.ok = true;
                    return jsonRespuesta.toJSON();
                }
                else
                {
                    jsonRespuesta.ok = false;
                    jsonRespuesta.strMensaje = respuesta.strMensaje;
                    jsonRespuesta.data = respuesta.objObjeto;
                    return jsonRespuesta.toJSON();
                }
            }
            catch (CustomException ce)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = ce.Message;
                jsonRespuesta.data = new Agrupacion();
                return jsonRespuesta.toJSON();
            }
            catch (Exception ex)
            {

                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                AppContext.ExceptionBuilder.BuildException(msj, ex);
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = msj;
                jsonRespuesta.data = new Agrupacion();
                return jsonRespuesta.toJSON();

            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditarAgrupacion(int id)
        {
            Respuesta<Agrupacion> respuesta = new Respuesta<Agrupacion>();
            try
            {
                respuesta = refAgrupacionBL.gObtenerAgrupacion(id);
            }
            catch (Exception ex)
            {

                throw;
            }

            return PartialView(respuesta.objObjeto);
        }

        /// <summary>
        /// Guarda la edición
        /// </summary>
        /// <param name="nuevaAgrupacion"></param>
        /// <returns></returns>
        [HttpPost]
        public String EditarAgrupacion(Agrupacion nuevaAgrupacion)
        {
            Respuesta<Agrupacion> respuesta = new Respuesta<Agrupacion>();
            Respuesta<Agrupacion> respuestaAnteriorAgrupacion = new Respuesta<Agrupacion>();
            JSONResult<Agrupacion> jsonRespuesta = new JSONResult<Agrupacion>();
            int idAgrupacion = 0;
            try
            {
                idAgrupacion = nuevaAgrupacion.IdAgrupacion;
                respuestaAnteriorAgrupacion = refAgrupacionBL.gObtenerAgrupacion(idAgrupacion);
                refAgrupacionBL = new AgrupacionBL(AppContext);
                respuesta = refAgrupacionBL.gEditarAgrupacion(nuevaAgrupacion);
                if (respuesta.blnIndicadorTransaccion)
                {

                    func.agrupacionbit(ActionsBinnacle.Editar, respuesta.objObjeto, respuestaAnteriorAgrupacion.objObjeto);
                    respuesta.objObjeto.DetalleAgrupacion.Clear();
                    jsonRespuesta.data = respuesta.objObjeto;

                    return jsonRespuesta.toJSON();
                }
                else
                {
                    jsonRespuesta.ok = false;
                    jsonRespuesta.data = respuesta.objObjeto;
                    jsonRespuesta.strMensaje = respuesta.strMensaje;
                    return jsonRespuesta.toJSON();
                }
            }
            catch (CustomException ce)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = ce.Message;
                jsonRespuesta.data = new Agrupacion();
                return jsonRespuesta.toJSON();
            }
            catch (Exception ex)
            {

                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                AppContext.ExceptionBuilder.BuildException(msj, ex);
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = msj;
                jsonRespuesta.data = new Agrupacion();
                return jsonRespuesta.toJSON();

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EliminarAgrupacion(int id)
        {
            return View();
        }

        /// <summary>
        /// Llamado al eliminar
        /// </summary>
        /// <param name="nuevaAgrupacion"></param>
        /// <returns></returns>
        [HttpPost]
        public String EliminarAgrupacion(Agrupacion nuevaAgrupacion)
        {

            Respuesta<Agrupacion> respuesta = new Respuesta<Agrupacion>();
            JSONResult<Agrupacion> jsonRespuesta = new JSONResult<Agrupacion>();
            try
            {

                // TODO: Add update logic here
                respuesta = refAgrupacionBL.gEliminarAgrupacion(nuevaAgrupacion);
                if (respuesta.blnIndicadorTransaccion)
                {
                    func.agrupacionbit(ActionsBinnacle.Borrar, null, respuesta.objObjeto);
                    jsonRespuesta.data = new Agrupacion();
                    return jsonRespuesta.toJSON();
                }
                else
                {
                    jsonRespuesta.ok = false;
                    jsonRespuesta.data = respuesta.objObjeto;
                    jsonRespuesta.strMensaje = respuesta.strMensaje;
                    return jsonRespuesta.toJSON();
                }
            }
            catch (CustomException ce)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = ce.Message;
                jsonRespuesta.data = new Agrupacion();
                return jsonRespuesta.toJSON();
            }
            catch (Exception ex)
            {

                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                AppContext.ExceptionBuilder.BuildException(msj, ex);
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = msj;
                jsonRespuesta.data = new Agrupacion();
                return jsonRespuesta.toJSON();

            }

        }
        #endregion

      


    }

}
