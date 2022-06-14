using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GB.SUTEL.BL.Mantenimientos;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.BL.Seguridad;
using GB.SUTEL.UI.Helpers;
using GB.SUTEL.UI.Recursos.Utilidades;
using System.Text;
using GB.SUTEL.UI.Models;
using GB.SUTEL.BL;
using Microsoft.AspNet.Identity;

namespace GB.SUTEL.UI.Controllers
{
    public class ServicioController : BaseController
    {
        public ServicioController()
        {
            refServicioBL = new ServicioBL(AppContext);
            refBitacoraBL = new BitacoraBL(AppContext);
            refTipoIndicadorBL = new TipoIndicadorBL(AppContext);
        }

        #region Atributos
        ServicioBL refServicioBL;
        BitacoraBL refBitacoraBL;
        TipoIndicadorBL refTipoIndicadorBL;
        private static Servicio bitacoraServicio = new Servicio();
        Funcion func = new Funcion();
        #endregion

        #region Vistas

        //
        // GET: /Servicio/
        [AuthorizeUserAttribute]
        public ActionResult Index()
        {
            Respuesta<List<Servicio>> objRespuesta = new Respuesta<List<Servicio>>();
            try
            {                
                ViewBag.searchTipoIndicador = "";
                 List<TipoIndicador> olistTipoIndicadores = refTipoIndicadorBL.ConsultarTodos().objObjeto;
                ServicioViewModels oServicioModel = new ServicioViewModels();
                oServicioModel.listadoTipoIndicador = olistTipoIndicadores;
                string user;
                user = User.Identity.GetUserId();
                try
                {
                    func._index(user, "Servicios", "Servicios Mantenimiento");
                }
                
                catch (Exception e)
                {
                    Console.WriteLine("{0} Exception caught.", e);
                }
                return View(new Tuple<List<Servicio>, GB.SUTEL.UI.Models.ServicioViewModels>(objRespuesta.objObjeto, oServicioModel));
            }
            catch
            {
                return View();
            }
        } 

        [HttpPost]
        public ActionResult _tableTipoIndicadores(string TipoIndicador)
        {
            try
            {
                if (TipoIndicador == null)
                {
                    TipoIndicador = "";
                }
                ViewBag.searchTipoIndicador = TipoIndicador;

                List<TipoIndicador> olistTipoIndicadores = refTipoIndicadorBL.gFiltarTiposIndicador(0, TipoIndicador).objObjeto;
                ServicioViewModels oServicioModel = new ServicioViewModels();
                oServicioModel.listadoTipoIndicador = olistTipoIndicadores;
                func.serviciosbit(ActionsBinnacle.Consultar, null, null);
                return PartialView(oServicioModel);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView();
            }
            catch (Exception ex)
            {
                var newEx = AppContext.ExceptionBuilder.BuildException("Ha ocurrido un error", ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView();
            }
        }

        [HttpGet]
        public ActionResult _tableTipoIndicadoresEditar()
        {
            try
            {                
                List<TipoIndicador> olistTipoIndicadores = refTipoIndicadorBL.ConsultarTodos().objObjeto;
                ServicioViewModels oServicioModel = new ServicioViewModels();
                oServicioModel.listadoTipoIndicador = olistTipoIndicadores;
                func.serviciosbit(ActionsBinnacle.Consultar, null, null);
                return PartialView(oServicioModel);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView();
            }
            catch (Exception ex)
            {
                var newEx = AppContext.ExceptionBuilder.BuildException("Ha ocurrido un error", ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView();
            }
        }

    [HttpPost]
        public ActionResult _tableTipoIndicadoresEditar(string TipoIndicador)
        {
            try
            {
                if (TipoIndicador == null)
                {
                    TipoIndicador = "";
                }
                ViewBag.searchTipoIndicador = TipoIndicador;

                List<TipoIndicador> olistTipoIndicadores = refTipoIndicadorBL.gFiltarTiposIndicador(0, TipoIndicador).objObjeto;
                ServicioViewModels oServicioModel = new ServicioViewModels();
                oServicioModel.listadoTipoIndicador = olistTipoIndicadores;
                func.serviciosbit(ActionsBinnacle.Consultar, null, null);
                return PartialView(oServicioModel);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView();
            }
            catch (Exception ex)
            {
                var newEx = AppContext.ExceptionBuilder.BuildException("Ha ocurrido un error", ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView();
            }
        }
        public ActionResult _table()
        {
            try
            {
                ViewBag.searchTerm = new Servicio();

                Respuesta<List<Servicio>> objRespuesta = new Respuesta<List<Servicio>>();
                Servicio objServicio = new Servicio();
                objRespuesta = refServicioBL.ConsultarTodos();

                func.serviciosbit(ActionsBinnacle.Consultar, null, null);

                return PartialView(new Tuple<List<Servicio>, Servicio>(objRespuesta.objObjeto, objServicio));
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
        public ActionResult _table(Servicio objServiciop)
        {
            Respuesta<List<Servicio>> objRespuesta = new Respuesta<List<Servicio>>();
            Servicio objServicio = new Servicio();
            ViewBag.searchTerm = objServiciop;
            objRespuesta = refServicioBL.gFiltrarServicios(objServiciop.IdServicio, (objServiciop.DesServicio == null ? "" : objServiciop.DesServicio));
            return PartialView(new Tuple<List<Servicio>, Servicio>(objRespuesta.objObjeto, objServicio));
        }


        //
        // POST: /Servicio/Crear
        [HttpPost]
        public string Crear(ServicioViewModels nuevoServicio)
        {
            Respuesta<Servicio> respuesta = new Respuesta<Servicio>();
            Respuesta<TipoIndicadorServicio> respuestaIndicadores = new Respuesta<TipoIndicadorServicio>();

            string nombreServicio = Request.Params["data[txtIDNombreServicioCrear]"];

            string indicadores = Request.Params["data[indicadores][]"];

            JSONResult<Servicio> jsonRespuesta = new JSONResult<Servicio>();
            try
            {
                respuesta = refServicioBL.Agregar(nombreServicio);
                if (respuesta.blnIndicadorTransaccion && !(string.IsNullOrEmpty(indicadores)))
                {
                    respuestaIndicadores = refServicioBL.AgregarTipoIndicador(indicadores, respuesta.objObjeto.IdServicio);
                }
               // gBitacora(ActionsBinnacle.Insertar, nuevoServicio.itemServicio, nuevoServicio.itemServicio);
                if (respuesta.blnIndicadorTransaccion)
                {

                    func.serviciosbit(ActionsBinnacle.Crear, respuesta.objObjeto, null);
                    //Nueva información
                    //StringBuilder newData = new StringBuilder();
                    //newData.Append("Registro de un servicio con Id de servicio: ");
                    //newData.Append(respuesta.objObjeto.IdServicio);
                    //newData.Append(" Descripción del servicio: ");
                    //newData.Append(respuesta.objObjeto.DesServicio);
                    //newData.Append(" Bandera de borrado del servicio: ");
                    //newData.Append(respuesta.objObjeto.Borrado);
                    ////Inserción Bitácora
                    //refBitacoraBL.InsertarBitacora(Convert.ToInt32(ActionsBinnacle.Insertar.GetHashCode()), "Servicios", "Nombre usuario",
                    //                               "Proceso de insercion de servicios", newData.ToString(), String.Empty);
                    
                    jsonRespuesta.data = respuesta.objObjeto;
                    jsonRespuesta.strMensaje = respuesta.strMensaje;
                    return jsonRespuesta.toJSONLoopHandlingIgnore();
                }
                else
                {
                    jsonRespuesta.ok = false;
                    jsonRespuesta.data = respuesta.objObjeto;
                    return jsonRespuesta.toJSONLoopHandlingIgnore();
                }
            }
            catch(Exception ex)
            {
                jsonRespuesta.ok = false;
                return jsonRespuesta.toJSON();
            }
        }

        [HttpPost]
        public string ConsultarParaEditar(int data)
        {
            JSONResult<List<TipoIndicadorServicio>> jsonRespuesta = new JSONResult<List<TipoIndicadorServicio>>();
            try
            {


                Respuesta<List<TipoIndicadorServicio>> olistTipoIndicadoresServicio = refServicioBL.gObenerTipoIndicadoresPorServicio(data);
                List<TipoIndicador> olistTipoIndicadores = refTipoIndicadorBL.ConsultarTodos().objObjeto;

                ServicioViewModels oServicioModel = new ServicioViewModels();
               // oServicioModel.listadoTipoIndicador = olistTipoIndicadores;
                oServicioModel.listadoTipoIndicadorServicio = olistTipoIndicadoresServicio.objObjeto;
                func.serviciosbit(ActionsBinnacle.Consultar, null, null);
           
              //  return PartialView("Editar",oServicioModel);
                jsonRespuesta.data = olistTipoIndicadoresServicio.objObjeto;

                
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                jsonRespuesta.ok = false;
                //return PartialView("Editar");
            }
            catch (Exception ex)
            {
                var newEx = AppContext.ExceptionBuilder.BuildException("Ha ocurrido un error", ex);
                ViewBag.Error = ((CustomException)newEx);
                jsonRespuesta.ok = false;
                //return PartialView("Editar");
            }
            return jsonRespuesta.toJSON();
        }

        // POST: /Servicio/Editar
        [HttpPost]
        public string Editar(ServicioViewModels nuevoServicio)
        {
            Respuesta<Servicio> respuesta = new Respuesta<Servicio>();
            JSONResult<Servicio> jsonRespuesta = new JSONResult<Servicio>();
            Respuesta<TipoIndicadorServicio> respuestaIndicadores = new Respuesta<TipoIndicadorServicio>();
            int idServicio = int.Parse(Request.Params["data[IdServicio]"]);
            Servicio nuevoServicioAux = new Servicio();

            string nombreServicio = Request.Params["data[txtIDNombreServicioEditar]"];

            string indicadores = Request.Params["data[indicadores][]"];
            try
            {
                // Información antes de la edición
                Respuesta<Servicio> ServicioSelecionado = new Respuesta<Servicio>();
                ServicioSelecionado.objObjeto = new Servicio();
                ServicioSelecionado.objObjeto.IdServicio = idServicio;
                //
                nuevoServicioAux.IdServicio = idServicio;
                nuevoServicioAux.DesServicio = nombreServicio;
                nuevoServicioAux.Borrado = 0;

                ServicioSelecionado = refServicioBL.ConsultaPorID(ServicioSelecionado.objObjeto);
                bitacoraServicio = new Servicio();
                bitacoraServicio.IdServicio = ServicioSelecionado.objObjeto.IdServicio;
                bitacoraServicio.DesServicio = ServicioSelecionado.objObjeto.DesServicio;

                respuesta = refServicioBL.Editar(nuevoServicioAux);

                if (respuesta.blnIndicadorTransaccion)
                {
                    respuestaIndicadores = refServicioBL.EditarTipoIndicadorPorServicio(indicadores, idServicio);
                }
                if (respuesta.blnIndicadorTransaccion)
                {
                    //Nueva información
                    func.serviciosbit(ActionsBinnacle.Editar, nuevoServicioAux, bitacoraServicio);

                    jsonRespuesta.data = respuesta.objObjeto;
                    jsonRespuesta.strMensaje = respuesta.strMensaje;
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
        // POST: /Servicio/Eliminar
        [HttpPost]
        public string Eliminar(ServicioViewModels nuevoServicio)
        {
            Respuesta<Servicio> respuesta = new Respuesta<Servicio>();
            JSONResult<Servicio> jsonRespuesta = new JSONResult<Servicio>();
            try
            {
                Respuesta<Servicio> ServicioSelecionado = new Respuesta<Servicio>();
                ServicioSelecionado = refServicioBL.ConsultaPorID(nuevoServicio.itemServicio);
                bitacoraServicio = new Servicio();
                bitacoraServicio.IdServicio = ServicioSelecionado.objObjeto.IdServicio;
                bitacoraServicio.DesServicio = ServicioSelecionado.objObjeto.DesServicio;
                respuesta = refServicioBL.Eliminar(nuevoServicio.itemServicio);
               // 
                if (respuesta.blnIndicadorTransaccion)
                {
                    //Información antes de la eliminación
                    func.serviciosbit(ActionsBinnacle.Borrar, null, bitacoraServicio); 
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

        #endregion

     

    }
}
