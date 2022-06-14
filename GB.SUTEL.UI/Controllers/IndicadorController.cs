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
    public class IndicadorController : BaseController
    {

        #region Atributos
        IndicadorBL refIndicadorBL;
        TipoIndicadorBL refTipoIndicadorBL;
        DireccionBL refDireccionBL;
        IndicadorUITBL refIndicadorUITBL;
        private static Indicador bitacoraOld = new Indicador();
        Funcion func = new Funcion();
        #endregion

        public IndicadorController()
        {            
            refIndicadorBL = new IndicadorBL(AppContext);
            refTipoIndicadorBL = new TipoIndicadorBL(AppContext);
            refIndicadorUITBL = new IndicadorUITBL(AppContext);
            refDireccionBL = new DireccionBL();
        }

        //
        // GET: /Indicador/
        [AuthorizeUserAttribute]
        public ActionResult Index(bool? mostrarMensajeGuardo, bool? mostrarMensajeActualizo)
        {
            ViewBag.mostrarMensajeGuardo = false;
            ViewBag.mostrarMensajeActualizo =false;
            if (mostrarMensajeGuardo != null)
            {
                ViewBag.mostrarMensajeGuardo = mostrarMensajeGuardo;
            }

            if (mostrarMensajeActualizo != null)
            {
                ViewBag.mostrarMensajeActualizo = mostrarMensajeActualizo;
            }                       

            Respuesta<List<Indicador>> objRespuesta = new Respuesta<List<Indicador>>();
            Respuesta<List<TipoIndicador>> oTipoIndicadores = new Respuesta<List<TipoIndicador>>();
            try
            {
                Indicador oIndicador = new Indicador();
                oIndicador.TipoIndicador = new TipoIndicador();
                oTipoIndicadores = refTipoIndicadorBL.ConsultarTodos();
                ViewBag.searchTermUIT = new IndicadorUIT();
                ViewBag.searchTermDireccion = new Direccion();
                ViewBag.searchTerm = new Indicador();
                if (objRespuesta.blnIndicadorTransaccion)
                {

                    string user;
                    user = User.Identity.GetUserId();
                    try
                    {
                        func._index(user, "Indicador", "Indicador Mantenimiento");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} Exception caught.", e);
                    }
                    return View(new Tuple<List<Indicador>, Indicador, List<TipoIndicador>>(objRespuesta.objObjeto, oIndicador, oTipoIndicadores.objObjeto));
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

        public ActionResult _table()
        {
            try
            {
               
                Respuesta<List<TipoIndicador>> oTipoIndicadores = new Respuesta<List<TipoIndicador>>();
                Respuesta<List<Indicador>> objRespuesta = new Respuesta<List<Indicador>>();
                Indicador objIndicador = new Indicador();
                objIndicador.TipoIndicador = new TipoIndicador();
                ViewBag.searchTerm = objIndicador;
                objRespuesta = refIndicadorBL.ConsultarTodos();

             
               

                return PartialView(new Tuple<List<Indicador>, Indicador, List<TipoIndicador>>(objRespuesta.objObjeto, objIndicador, oTipoIndicadores.objObjeto));
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView();
            }
            catch (Exception ex)
            {
                //var newEx = AppContext.ExceptionBuilder.BuildException("Ha ocurrido un error al guardar", ex);
                var newEx = "";
                //ViewBag.Error = ((CustomException)newEx);
                return PartialView();
            }
        }

        public ActionResult _tableIndicadoresUIT(IndicadorUIT searchTermUIT, Indicador objIndicador, string idIndicador)
        {
            try
            {
                if(idIndicador != null)
                {
                    objIndicador.IdIndicador = idIndicador;
                }
                ViewBag.searchTermUIT = searchTermUIT;
                Respuesta<List<IndicadorUIT>> objRespuesta = new Respuesta<List<IndicadorUIT>>();
                objRespuesta = refIndicadorUITBL.gObtenerIndicadoresUITPorFiltros(0, searchTermUIT.DescIndicadorUIT == null ? "" : searchTermUIT.DescIndicadorUIT);
                return PartialView(new Tuple<List<IndicadorUIT>, Indicador>(objRespuesta.objObjeto, objIndicador));
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView();
            }
            catch (Exception ex)
            {
                //var newEx = AppContext.ExceptionBuilder.BuildException("Ha ocurrido un error al guardar", ex);
                var newEx = "";
                //ViewBag.Error = ((CustomException)newEx);
                return PartialView();
            }
        }

        public ActionResult _tableDireccion(Direccion searchTermDireccion, Indicador objIndicador, string idIndicador)
        {
            try
            {
                if (idIndicador != null)
                {
                    objIndicador.IdIndicador = idIndicador;
                }

                ViewBag.searchTermDireccion = searchTermDireccion;
                Respuesta<List<Direccion>> objRespuesta = new Respuesta<List<Direccion>>();
                objRespuesta = refDireccionBL.gFiltrarDirecciones((searchTermDireccion.Nombre==null?"":searchTermDireccion.Nombre));

                return PartialView(new Tuple<List<Direccion>, Indicador>(objRespuesta.objObjeto, objIndicador));
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView();
            }
            catch (Exception ex)
            {
                //var newEx = AppContext.ExceptionBuilder.BuildException("Ha ocurrido un error al guardar", ex);
                var newEx = "";
                //ViewBag.Error = ((CustomException)newEx);
                return PartialView();
            }
        }

        [HttpPost]
        public ActionResult _table(Indicador objIndicadorp)
        {
            Respuesta<List<Indicador>> objRespuesta = new Respuesta<List<Indicador>>();
            Respuesta<List<TipoIndicador>> oTipoIndicadores = new Respuesta<List<TipoIndicador>>();
            oTipoIndicadores = refTipoIndicadorBL.ConsultarTodos();
            Indicador objIndicador = new Indicador();
            ViewBag.searchTerm = objIndicadorp;
            objRespuesta = refIndicadorBL.gFiltrarIndicadores((objIndicadorp.IdIndicador == null ? "" : objIndicadorp.IdIndicador), (objIndicadorp.NombreIndicador == null ? "" : objIndicadorp.NombreIndicador), (objIndicadorp.TipoIndicador == null || objIndicadorp.TipoIndicador.DesTipoInd==null ? "" : objIndicadorp.TipoIndicador.DesTipoInd));

            return PartialView(new Tuple<List<Indicador>, Indicador, List<TipoIndicador>>(objRespuesta.objObjeto, objIndicador, oTipoIndicadores.objObjeto));
        }

        [AuthorizeUserAttribute]
        public ActionResult Crear()
        {            
            Respuesta<List<TipoIndicador>> oTipoIndicadores = new Respuesta<List<TipoIndicador>>();
            oTipoIndicadores = refTipoIndicadorBL.ConsultarTodos();
            Indicador objIndicador = new Indicador();

            return View(new Tuple<Indicador, List<TipoIndicador>>(objIndicador, oTipoIndicadores.objObjeto));
        }
     
        //
        // POST: /Indicador/Crear
        [HttpPost]
        [AuthorizeUserAttribute]
        public string Crear(Indicador Item1)
        {
            string idIndicador = Request.Params["data[idIndicador]"];
            string nombreIndicador = Request.Params["data[nombreIndicador]"];
            string idTipoIndicador = Request.Params["data[idTipoIndicador]"];
            string descripcion = Request.Params["data[descripcion]"];
            string direcciones = Request.Params["data[direcciones][]"];
            string indicadoresUIT = Request.Params["data[indicadoresUIT][]"];

            Indicador objIndicador = new Indicador();
            objIndicador.IdIndicador = idIndicador.Trim();
            objIndicador.NombreIndicador = nombreIndicador.Trim();
            objIndicador.IdTipoInd = Convert.ToInt32(idTipoIndicador);
            objIndicador.DescIndicador = descripcion.Trim();

            int[] indicadoresUITSave = null;
            if (indicadoresUIT != null)
            {
                indicadoresUITSave = Array.ConvertAll(indicadoresUIT.Split(','), x => Int32.Parse(x));
            }

            int[] direccionesSave = null;
            if (direcciones != null)
            {
                direccionesSave = Array.ConvertAll(direcciones.Split(','), x => Int32.Parse(x));
            }

            Respuesta<Indicador> respuesta = new Respuesta<Indicador>();
            JSONResult<Indicador> jsonRespuesta = new JSONResult<Indicador>();
            try
            {
                respuesta = refIndicadorBL.Agregar(objIndicador, indicadoresUITSave, direccionesSave);

               func.indicadorbit(ActionsBinnacle.Crear, objIndicador, null);

                if (respuesta.blnIndicadorTransaccion)
                {                    
                    jsonRespuesta.data = null;
                    return jsonRespuesta.toJSON();
                }
                else
                {
                    jsonRespuesta.strMensaje = respuesta.strMensaje;
                    jsonRespuesta.ok = false;
                    jsonRespuesta.data = null;
                    return jsonRespuesta.toJSON();
                }
            }
            catch
            {
                jsonRespuesta.ok = false;
                return jsonRespuesta.toJSON();
            }        
        }

        [AuthorizeUserAttribute]
        public ActionResult Editar(string id)
        {            
            string idIndicador = id;
            
            Respuesta<List<TipoIndicador>> oTipoIndicadores = new Respuesta<List<TipoIndicador>>();
            Respuesta <Indicador> indicador = new Respuesta<Indicador>();
            oTipoIndicadores = refTipoIndicadorBL.ConsultarTodos();
            Indicador objIndicador = new Indicador();
            objIndicador.IdIndicador = idIndicador;
            objIndicador = refIndicadorBL.ConsultaPorID(objIndicador).objObjeto;
            bitacoraOld = new Indicador();
            bitacoraOld.DescIndicador = objIndicador.DescIndicador;
            bitacoraOld.IdIndicador = objIndicador.IdIndicador;

            return View(new Tuple<Indicador, List<TipoIndicador>>(objIndicador, oTipoIndicadores.objObjeto));
        }

        //
        // POST: /Indicador/Editar
        [HttpPost]        
        public string EditarIndicador(string inde)
        {
            string idIndicador = Request.Params["data[idIndicador]"];
            string nombreIndicador = Request.Params["data[nombreIndicador]"];
            string idTipoIndicador = Request.Params["data[idTipoIndicador]"];
            string descripcion = Request.Params["data[descripcion]"];
            string direcciones = Request.Params["data[direcciones][]"];
            string indicadoresUIT = Request.Params["data[indicadoresUIT][]"];

            Indicador objIndicador = new Indicador();
            objIndicador.IdIndicador = idIndicador.Trim();
            objIndicador.NombreIndicador = nombreIndicador.Trim();
            objIndicador.IdTipoInd = Convert.ToInt32(idTipoIndicador);
            objIndicador.DescIndicador = descripcion.Trim();

            int[] indicadoresUITSave = null;
            if (indicadoresUIT != null)
            {
                indicadoresUITSave = Array.ConvertAll(indicadoresUIT.Split(','), x => Int32.Parse(x));
            }

            int[] direccionesSave = null;
            if (direcciones !=null)
            {
                direccionesSave = Array.ConvertAll(direcciones.Split(','), x => Int32.Parse(x));
            }

            Respuesta<Indicador> respuesta = new Respuesta<Indicador>();
            JSONResult<Indicador> jsonRespuesta = new JSONResult<Indicador>();
            try
            {
                respuesta = refIndicadorBL.Editar(objIndicador, indicadoresUITSave, direccionesSave);
               func.indicadorbit(ActionsBinnacle.Editar, objIndicador, bitacoraOld);
                if (respuesta.blnIndicadorTransaccion)
                {                    
                    jsonRespuesta.data = null;
                    return jsonRespuesta.toJSON();
                }
                else
                {
                    jsonRespuesta.strMensaje = respuesta.strMensaje;
                    jsonRespuesta.ok = false;
                    jsonRespuesta.data = null;
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
        // POST: /Indicador/Eliminar
        [HttpPost]
        public string Eliminar(Indicador nuevoIndicador)
        {
            Respuesta<Indicador> respuesta = new Respuesta<Indicador>();
            JSONResult<Indicador> jsonRespuesta = new JSONResult<Indicador>();
            try
            {
                respuesta = refIndicadorBL.Eliminar(nuevoIndicador);
               func.indicadorbit(ActionsBinnacle.Borrar, nuevoIndicador, null);
                if (respuesta.blnIndicadorTransaccion)
                {
                    jsonRespuesta.strMensaje = respuesta.strMensaje;
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
