using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GB.SUTEL.BL.Mantenimientos;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.UI.Helpers;
using System.Text;
using GB.SUTEL.UI.Recursos.Utilidades;

using Microsoft.AspNet.Identity;

namespace GB.SUTEL.UI.Controllers
{
    public class IndicadorUITController : BaseController
    {
      
        #region atributos
        IndicadorUITBL refIndicadorUITBL;
        Funcion func = new Funcion();
        #endregion

        #region eventos
        public IndicadorUITController()

        {
            refIndicadorUITBL = new IndicadorUITBL(AppContext);
        }
        // GET: IndicadorUIT
        [AuthorizeUserAttribute]
        public ActionResult Index()
        {
            string user;
            user = User.Identity.GetUserId();
            try
            {
                func._index(user, "Indicador UIT", "Indicador UIT Mantenimiento");
            }
           
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }

            return View(new Tuple<List<IndicadorUIT>, IndicadorUIT>(new List<IndicadorUIT>(), new IndicadorUIT()));
        }

        /// <summary>
        /// Listado de la pantalla
        /// </summary>
        /// <returns></returns>
        public ActionResult _table()
        {
            Respuesta<List<IndicadorUIT>> respuesta = new Respuesta<List<IndicadorUIT>>();
            IndicadorUIT IndicadorUIT = new IndicadorUIT();
            try
            {


                respuesta = refIndicadorUITBL.gObtenerIndicadoresUIT();
                ViewBag.searchTerm = new IndicadorUIT();
                return PartialView( respuesta.objObjeto);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView(respuesta.objObjeto);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView(respuesta.objObjeto);
            }
        }
        /// <summary>
        /// Listado de la pantalla con filtros
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _table(IndicadorUIT searchTerm)
        {
            Respuesta<List<IndicadorUIT>> respuesta = new Respuesta<List<IndicadorUIT>>();
            
            try
            {
                respuesta = refIndicadorUITBL.gObtenerIndicadoresUITPorFiltros(searchTerm.IdIndicadorUIT, searchTerm.DescIndicadorUIT != null ? searchTerm.DescIndicadorUIT : "");
                ViewBag.searchTerm = searchTerm;
                return PartialView(respuesta.objObjeto);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView(respuesta.objObjeto);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView(respuesta.objObjeto);
            }
        }

        // GET: IndicadorUIT/Create
        public ActionResult _CrearIndicadorUIT()
        {
            return PartialView();
        }

        // POST: IndicadorUIT/Create
        [HttpPost]
        public string _CrearIndicadorUIT(IndicadorUIT nuevoIndicadorUIT)
        {
            Respuesta<IndicadorUIT> respuesta = new Respuesta<IndicadorUIT>();
            JSONResult<IndicadorUIT> jsonRespuesta = new JSONResult<IndicadorUIT>();
            jsonRespuesta.ok = false;
            try
            {

                respuesta = refIndicadorUITBL.gAgregarIndicadorUIT(nuevoIndicadorUIT);
                if (respuesta.blnIndicadorTransaccion)
                {
                    func.indicadoruitbit(ActionsBinnacle.Crear, respuesta.objObjeto, null);        
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
                jsonRespuesta.data = new IndicadorUIT();
                return jsonRespuesta.toJSON();
            }
            catch (Exception ex)
            {

                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                AppContext.ExceptionBuilder.BuildException(msj, ex);
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = msj;
                jsonRespuesta.data = new IndicadorUIT();
                return jsonRespuesta.toJSON();

            }
        }

        // GET: IndicadorUIT/Edit/5
        public ActionResult _EditarIndicadorUIT(int id)
        {
            Respuesta<IndicadorUIT> respuesta = new Respuesta<IndicadorUIT>();
            try
            {
                respuesta = refIndicadorUITBL.gObtenerIndicadorUIT(id);
            }
            catch (Exception ex)
            {

                throw;
            }

            return PartialView(respuesta.objObjeto);
        }

        // POST: IndicadorUIT/Edit/5
        [HttpPost]
        public String _EditarIndicadorUIT(IndicadorUIT indicadoruit)
        {
            Respuesta<IndicadorUIT> respuesta = new Respuesta<IndicadorUIT>();
            Respuesta<IndicadorUIT> respuestaAnterior = new Respuesta<IndicadorUIT>();
            JSONResult<IndicadorUIT> jsonRespuesta = new JSONResult<IndicadorUIT>();
            try
            {
                respuestaAnterior = refIndicadorUITBL.gObtenerIndicadorUIT(indicadoruit.IdIndicadorUIT);
                refIndicadorUITBL = new IndicadorUITBL(AppContext);
                respuesta = refIndicadorUITBL.gEditarIndicadorUIT(indicadoruit);
                if (respuesta.blnIndicadorTransaccion)
                {

                    func.indicadoruitbit(ActionsBinnacle.Editar, respuesta.objObjeto, respuestaAnterior.objObjeto);
                    respuesta.objObjeto.Indicador.Clear();
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
                jsonRespuesta.data = new IndicadorUIT();
                return jsonRespuesta.toJSON();
            }
            catch (Exception ex)
            {

                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                AppContext.ExceptionBuilder.BuildException(msj, ex);
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = msj;
                jsonRespuesta.data = new IndicadorUIT();
                return jsonRespuesta.toJSON();

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult _EliminarIndicadorUIT(int id)
        {
            return View();
        }

        /// <summary>
        /// Llamado al eliminar
        /// </summary>
        /// <param name="nuevaINDICADORUIT"></param>
        /// <returns></returns>
        [HttpPost]
        public String _EliminarIndicadorUIT(IndicadorUIT nuevaINDICADORUIT)
        {

            Respuesta<IndicadorUIT> respuesta = new Respuesta<IndicadorUIT>();
            JSONResult<IndicadorUIT> jsonRespuesta = new JSONResult<IndicadorUIT>();
            Respuesta<IndicadorUIT> respuestaAnterior = new Respuesta<IndicadorUIT>();
            try
            {

                // TODO: Add update logic here
                respuestaAnterior = refIndicadorUITBL.gObtenerIndicadorUIT(nuevaINDICADORUIT.IdIndicadorUIT);
                refIndicadorUITBL = new IndicadorUITBL(AppContext);
                respuesta = refIndicadorUITBL.gEliminarIndicadorUIT(nuevaINDICADORUIT);
                if (respuesta.blnIndicadorTransaccion)
                {
                    func.indicadoruitbit(ActionsBinnacle.Borrar, null, respuestaAnterior.objObjeto); 
                    jsonRespuesta.data = new IndicadorUIT();
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
                jsonRespuesta.data = new IndicadorUIT();
                return jsonRespuesta.toJSON();
            }
            catch (Exception ex)
            {

                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                AppContext.ExceptionBuilder.BuildException(msj, ex);
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = msj;
                jsonRespuesta.data = new IndicadorUIT();
                return jsonRespuesta.toJSON();

            }

        }
        #endregion


    }
}
