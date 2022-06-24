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
using GB.SUTEL.UI.Recursos.Utilidades;
using System.Text;
using Microsoft.AspNet.Identity;

namespace GB.SUTEL.UI.Controllers
{
    public class TipoIndicadorController : BaseController
    {

        #region Atributos
        TipoIndicadorBL refTipoIndicadorBL;
        private static TipoIndicador bitacoraTipo = new TipoIndicador();
        Funcion func = new Funcion();
        #endregion

        public TipoIndicadorController()
        {
            refTipoIndicadorBL = new TipoIndicadorBL(AppContext);            
        }

        public ActionResult _table()
        {
            try
            {
                ViewBag.searchTerm = new TipoIndicador();

                Respuesta<List<TipoIndicador>> objRespuesta = new Respuesta<List<TipoIndicador>>();
                TipoIndicador objTipoIndicador = new TipoIndicador();
                objRespuesta = refTipoIndicadorBL.ConsultarTodos();

                func.tipoindicadorbit(ActionsBinnacle.Consultar, null, null);

                return PartialView(new Tuple<List<TipoIndicador>, TipoIndicador>(objRespuesta.objObjeto, objTipoIndicador));
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView();
            }
            catch (Exception ex)
            {
                var newEx = AppContext.ExceptionBuilder.BuildException("Ha ocurrido un error al guardar", ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView();
            }
        }

        [HttpPost]
        public ActionResult _table(TipoIndicador objTipoIndicadorop)
        {
            Respuesta<List<TipoIndicador>> objRespuesta = new Respuesta<List<TipoIndicador>>();
            TipoIndicador objServicio = new TipoIndicador();
            ViewBag.searchTerm = objTipoIndicadorop;
            objRespuesta = refTipoIndicadorBL.gFiltarTiposIndicador(objTipoIndicadorop.IdTipoInd, (objTipoIndicadorop.DesTipoInd == null ? "" : objTipoIndicadorop.DesTipoInd));
            return PartialView(new Tuple<List<TipoIndicador>, TipoIndicador>(objRespuesta.objObjeto, objTipoIndicadorop));
        }

        //
        // GET: /Tipo Indicador/
        [AuthorizeUserAttribute]
        public ActionResult Index()
        {
            Respuesta<List<TipoIndicador>> objRespuesta = new Respuesta<List<TipoIndicador>>();
            try
            {
                TipoIndicador oTipoIndicador = new TipoIndicador();                
                if (objRespuesta.blnIndicadorTransaccion)
                {
                    string user;
                    user = User.Identity.GetUserId();
                    try
                    {
                        func._index(user, "Tipo Indicador", "Tipo Indicador Mantenimiento");
                    }
                    
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} Exception caught.", e);
                    }
                    return View(new Tuple<List<TipoIndicador>, TipoIndicador>(objRespuesta.objObjeto, oTipoIndicador));
                }
                else
                {                    
                    return View();
                }
            }
            catch
            {
                return View();
            }    
        }
     
        //
        // POST: /Tipo Indicador/Crear
        [HttpPost]
        public string Crear(TipoIndicador nuevoTipoIndicador)
        {
            Respuesta<TipoIndicador> respuesta = new Respuesta<TipoIndicador>();
            JSONResult<TipoIndicador> jsonRespuesta = new JSONResult<TipoIndicador>();
            try
            {
                respuesta = refTipoIndicadorBL.Agregar(nuevoTipoIndicador);
                func.tipoindicadorbit(ActionsBinnacle.Crear, nuevoTipoIndicador, null);
                if (respuesta.blnIndicadorTransaccion)
                {
                    jsonRespuesta.data = respuesta.objObjeto;
                    return jsonRespuesta.toJSON();
                }
                else
                {
                    jsonRespuesta.ok = false;
                    jsonRespuesta.data = respuesta.objObjeto;
                    return jsonRespuesta.toJSON();
                }
            }
            catch
            {
                jsonRespuesta.ok = false;                
                return jsonRespuesta.toJSON();
            }            
        }

        //
        // POST: /Tipo Indicador/Editar
        [HttpPost]
        public string Editar(TipoIndicador nuevoTipoIndicador)
        {
            Respuesta<TipoIndicador> respuesta = new Respuesta<TipoIndicador>();
            JSONResult<TipoIndicador> jsonRespuesta = new JSONResult<TipoIndicador>();
            try
            {
                bitacoraTipo = new TipoIndicador();
                List<TipoIndicador> consultar = refTipoIndicadorBL.gFiltarTiposIndicador(nuevoTipoIndicador.IdTipoInd, string.Empty).objObjeto;
                bitacoraTipo.IdTipoInd = consultar[0].IdTipoInd;
                bitacoraTipo.DesTipoInd = consultar[0].DesTipoInd;

                respuesta = refTipoIndicadorBL.Editar(nuevoTipoIndicador);
                func.tipoindicadorbit(ActionsBinnacle.Editar, nuevoTipoIndicador, bitacoraTipo);
                if (respuesta.blnIndicadorTransaccion)
                {
                    jsonRespuesta.data = new TipoIndicador();
                    return jsonRespuesta.toJSON();
                }
                else
                {
                    jsonRespuesta.ok = false;
                    jsonRespuesta.data = new TipoIndicador();
                    return jsonRespuesta.toJSON();
                }
            }
            catch
            {
                jsonRespuesta.ok = false;
                return jsonRespuesta.toJSON();
            }
        }

        //
        // POST: /Tipo Indicador/Eliminar
        [HttpPost]
        public string Eliminar(TipoIndicador nuevoTipoIndicador)
        {
            Respuesta<TipoIndicador> respuesta = new Respuesta<TipoIndicador>();
            JSONResult<TipoIndicador> jsonRespuesta = new JSONResult<TipoIndicador>();
            try
            {
                bitacoraTipo = new TipoIndicador();
                List<TipoIndicador> consultar = refTipoIndicadorBL.gFiltarTiposIndicador(nuevoTipoIndicador.IdTipoInd, string.Empty).objObjeto;
                bitacoraTipo.IdTipoInd = consultar[0].IdTipoInd;
                bitacoraTipo.DesTipoInd = consultar[0].DesTipoInd;

                respuesta = refTipoIndicadorBL.Eliminar(nuevoTipoIndicador);
               func.tipoindicadorbit(ActionsBinnacle.Borrar, null, bitacoraTipo);
                if (respuesta.blnIndicadorTransaccion)
                {
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
            catch
            {
                jsonRespuesta.ok = false;
                return jsonRespuesta.toJSON();
            }
        }

        
    }
}
