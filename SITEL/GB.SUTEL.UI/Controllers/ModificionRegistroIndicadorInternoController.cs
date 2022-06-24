
using ClosedXML.Excel;

using GB.SUTEL.BL.FuenteExternas;
using GB.SUTEL.BL.Mantenimientos;
using GB.SUTEL.BL.Proceso;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.UI.Helpers;
using GB.SUTEL.UI.Models;
using GB.SUTEL.UI.Recursos.Utilidades;
using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Security.Claims;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace GB.SUTEL.UI.Controllers
{
    public class ModificionRegistroIndicadorInternoController : BaseController
    {
        Funcion func = new Funcion();
        OperadorBL refOperadorBL;
        ServicioBL refServicioBL;
        DireccionBL refDireccionBL;
        IndicadorBL refIndicadorBL;
        CriterioBL refCriterioBL;
        FrecuenciaBL refFrecuenciaBL;
        SolicitudIndicadorBL refSolicitudIndicadorBL;
        ConstructorBL refConstructorBL;
        RegistroIndicadorInternoBL refRegistroIndicadorInternoBL;
        RegistroIndicadorExternoBL refRegistroIndicadorExternoBL;
        BitacoraIndicadorInternoBL refBitacoraIndicadorInternoBL;
        ServicioIndicadorBL refServicioIndicadorBL;



        #region contructor
        public ModificionRegistroIndicadorInternoController()
        {
            refOperadorBL = new OperadorBL(AppContext);
            refServicioBL = new ServicioBL(AppContext);
            refServicioIndicadorBL = new ServicioIndicadorBL(AppContext);
            refDireccionBL = new DireccionBL();
            refIndicadorBL = new IndicadorBL(AppContext);
            refCriterioBL = new CriterioBL(AppContext);
            refFrecuenciaBL = new FrecuenciaBL(AppContext);
            refSolicitudIndicadorBL = new SolicitudIndicadorBL(AppContext);
            refConstructorBL = new ConstructorBL(AppContext);
            refRegistroIndicadorInternoBL = new RegistroIndicadorInternoBL(AppContext);
            refRegistroIndicadorExternoBL = new RegistroIndicadorExternoBL(AppContext);
            refBitacoraIndicadorInternoBL = new BitacoraIndicadorInternoBL(AppContext);

        }

        [HttpGet]
        public ActionResult listarOperadoresServicio(int IdServicio)
        {
            List<Combos> resultado = null;
            if (IdServicio!=0)
            {
               resultado = 
                    refOperadorBL.ConsultarXServicio(IdServicio).objObjeto.Select(m=>
                        new Combos {id=m.IdOperador,valor= string.Format("{0} {1}", m.IdOperador, m.NombreOperador) })  .ToList();
            }
            else
            {
                resultado = refOperadorBL.ConsultarTodos().objObjeto.Select(m =>
                        new Combos { id = m.IdOperador, valor = string.Format("{0} {1}",m.IdOperador, m.NombreOperador) }).ToList();
            }
           
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

         [HttpGet]
        public ActionResult listarIndicadores(int IdServicio)
        {
            List<Combos> resultado = null;
            resultado =
                  refIndicadorBL.ConsultarModificadorMasivo(IdServicio).objObjeto.Select(m =>
                  new Combos { id = m.IdIndicador, valor = m.IdIndicador }).ToList();  
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion
        //
        // GET: /ModificionRegistroIndicadorInterno/
        [AuthorizeUserAttribute]
        public ActionResult Index()
        {
            ModificacionRegistroIndicadorViewModel nNuevaModificacion = new ModificacionRegistroIndicadorViewModel();
            Respuesta<List<Frecuencia>> frecuencias = new Respuesta<List<Frecuencia>>();
            Respuesta<List<Frecuencia>> desgloses = new Respuesta<List<Frecuencia>>();
            Respuesta<List<Direccion>> direcciones = new Respuesta<List<Direccion>>();
            Respuesta<List<Servicio>> servicio = new Respuesta<List<Servicio>>();
            try
            {
                frecuencias = refFrecuenciaBL.gListar();
                direcciones = refDireccionBL.gListar();
               
                nNuevaModificacion.listaFrecuencias = new List<Frecuencia>(frecuencias.objObjeto.OrderBy(x => x.NombreFrecuencia));
                nNuevaModificacion.listaDesglose = new List<Frecuencia>(frecuencias.objObjeto.OrderBy(x => x.NombreFrecuencia));
                nNuevaModificacion.listaDirecciones = new List<Direccion>(direcciones.objObjeto.OrderBy(x => x.Nombre));
                nNuevaModificacion.ListaServicios = refServicioBL.ConsultarTodos().objObjeto.OrderBy(x => x.DesServicio).ToList();
                nNuevaModificacion.idDireccion = 0;
                nNuevaModificacion.idFrecuencia = 0;
                nNuevaModificacion.idDesglose = 0;
                nNuevaModificacion.fechaInicial = DateTime.Now;
                nNuevaModificacion.fechaFinal = DateTime.Now;
                nNuevaModificacion.operador = new Operador();
                nNuevaModificacion.idServicioModificacionMasiva = 0;
                Session["respuestaSolicitudConstructor"] = null;
                Session["objRespuestaDetalleAgrupacion"] = null;
                Session["objRespuestaDetalleRegistroIndicador"] = null;
                string user;
                user = User.Identity.GetUserId();
                try
                {
                    func._index(user, "Indicador", "Modificación Indicador Interno Procesos");
                }
                
                catch (Exception e)
                {
                    Console.WriteLine("{0} Exception caught.", e);
                }

            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return View(nNuevaModificacion);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return View();
            }
            return View(nNuevaModificacion);
        }









        /// <summary>
        /// Cargar detalle agrupación desde Excel
        /// </summary>
        [HttpPost]
        public string  ImportarPlantillaExcel(HttpPostedFileBase fileImportar)
        {
            if (fileImportar == null)
            {
                return string.Format("<div class='alert alert-danger' role='alert'>no se selecciono ninguna plantilla </div>");
            }
            var rutaCarpeta = WebConfigurationManager.AppSettings["ModificadorMasivoDetalleRegistroIndicador"].ToString();
            if (Directory.Exists(rutaCarpeta))
            {
                var fileName = Path.GetFileName(fileImportar.FileName);
                fileImportar.SaveAs(Path.Combine(rutaCarpeta, fileName));

                return refRegistroIndicadorInternoBL.ModificarDetalleRegistroIndicadorMasivo(fileName, fileImportar.InputStream, ConsultarUsuario().IdUsuario).objObjeto;
            }
            else
            {
                return string.Format("<div class='alert alert-danger' role='alert'>no existe la carpeta {0}, favor configurarla </div>",rutaCarpeta);
            }

        }
        [HttpGet]
        [AuthorizeUserAttribute]
        public ActionResult DescargarExcel(string plantilla, string anno, string Operador, string Servicio, string indicador)
        {
            string detalleAgrupacionNombreExcel = plantilla;
       
            try
            {
                MemoryStream stream = new MemoryStream();
                ExcelPackage package;
                using (package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheetInicio = package.Workbook.Worksheets.Add("DetalleRegistroIndicador");

                    var listaDetalleRegistroIndicador = refRegistroIndicadorInternoBL.gListadoModificacionMasiva(anno, Operador, Servicio, indicador).objObjeto;

                    worksheetInicio.Cells["A1:R1"].LoadFromCollection(listaDetalleRegistroIndicador, true);
                    worksheetInicio.Cells["A1:R1"].Style.Font.Bold = true;
                    worksheetInicio.Cells["A1:R1"].Style.Font.Size = 12;
                    worksheetInicio.Cells["A1:R1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheetInicio.Cells["A1:R1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(6, 113, 174));
                    worksheetInicio.Cells["A1:P1"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                    worksheetInicio.Cells["A1:R1"].AutoFitColumns();
                    Response.BinaryWrite(package.GetAsByteArray());
                    Response.ContentType = "application/vnd.ms-excel.sheet.macroEnabled.12";
                    Response.AddHeader("content-disposition", "attachment;  filename=" + detalleAgrupacionNombreExcel + ".xlsx");

                }
                return new EmptyResult();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "RegistroIndicador");
            }
        }





        #region cargarDatos
        /// <summary>
        /// busca las solicitudes que coincidan con los parametros
        /// </summary>
        /// <param name="poSolicitud"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeUserAttribute]
        public ActionResult BuscarSolicitud(ModificacionRegistroIndicadorViewModel poSolicitud)
        {
            Respuesta<List<SolicitudIndicador>> respuesta = new Respuesta<List<SolicitudIndicador>>();
            Session["mensajeErrorBusqueda"] = null;

            try{
                respuesta = refSolicitudIndicadorBL.gSolicitudModificarIndicador(poSolicitud.idOperador, poSolicitud.idServicio, poSolicitud.idDireccion, poSolicitud.idIndicador
                    , poSolicitud.idCriterio, poSolicitud.idFrecuencia, poSolicitud.idDesglose, poSolicitud.fechaInicial, poSolicitud.fechaFinal);
                if (respuesta.blnIndicadorTransaccion == true && respuesta.objObjeto.Count == 0) {
                    Session["mensajeErrorBusqueda"] = GB.SUTEL.Shared.ErrorTemplate.ModificarIndicador_solicitudes;
                    
                 }
                else if (respuesta.blnIndicadorTransaccion == false) {
                    Session["mensajeErrorBusqueda"] = respuesta.strMensaje;
                }
                return PartialView("_tablaSolicitudIndicador", respuesta.objObjeto);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView("_tablaSolicitudIndicador", respuesta.objObjeto);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView("_tablaSolicitudIndicador", respuesta.objObjeto);
            }

            
        }




        /// <summary>
        /// Obtiene el mensaje del proceso de clonar
        /// </summary>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        public String _busquedaSolitudMensajeError()
        {
            JSONResult<String> jsonRespuesta = new JSONResult<String>();
            jsonRespuesta.ok = false;
            try
            {
                if (Session["mensajeErrorBusqueda"] != null)
                {
                    jsonRespuesta.strMensaje = (String)Session["mensajeErrorBusqueda"];
                    jsonRespuesta.ok = true;
                    jsonRespuesta.data = "";
                }
                else
                {
                    jsonRespuesta.strMensaje = "";
                    jsonRespuesta.ok = false;
                    jsonRespuesta.data = "";
                }
                Session["mensajeErrorBusqueda"] = null;
                return jsonRespuesta.toJSON();
            }

            catch (Exception ex)
            {

                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                AppContext.ExceptionBuilder.BuildException(msj, ex);
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = msj;
                jsonRespuesta.data = "";
                return jsonRespuesta.toJSON();

            }
        }

 
        /// <summary>
        /// Muestra los detalles agrupación
        /// </summary>
        /// <param name="poSolicitud"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeUserAttribute]
        public String gMostrarDetalleAgrupacion(ModificacionRegistroIndicadorViewModel poSolicitud)
        {
            Respuesta<SolicitudConstructor> respuestaSolicitudConstructor = new Respuesta<SolicitudConstructor>();
            Respuesta<List<ConstructorCriterioDetalleAgrupacion>> respuestaDetalleAgrupacion=new Respuesta<List<ConstructorCriterioDetalleAgrupacion>>();
            List<ConstructorCriterioDetalleAgrupacion> detallesTrasformados = new List<ConstructorCriterioDetalleAgrupacion>();
            List<ConstructorCriterioDetalleAgrupacion> detalles = new List<ConstructorCriterioDetalleAgrupacion>();
            ConstructorCriterioDetalleAgrupacion raiz = new ConstructorCriterioDetalleAgrupacion();
            List<ConstructorDetalleAgrupacionViewModels> jsTreeDetalleAgrupacion = new List<ConstructorDetalleAgrupacionViewModels>();
            JSONResult<CObjetoModels> jsonRespuesta = new JSONResult<CObjetoModels>();
            CObjetoModels respuesta=new CObjetoModels();
            List<CMes> meses = new List<CMes>();
            Session["respuestaSolicitudConstructor"] = null;
            try {
                respuestaSolicitudConstructor = refSolicitudIndicadorBL.gObtenerSolicitudConstructor(poSolicitud.idOperador, poSolicitud.idSolicitudIndicador, poSolicitud.idDesglose, poSolicitud.idIndicador);
                respuestaDetalleAgrupacion=refConstructorBL.gObtenerDetalleAgrupacionConstructor(respuestaSolicitudConstructor.objObjeto.IdConstructor,poSolicitud.idCriterio,poSolicitud.idOperador);
               // meses = lObtenerMes(respuestaSolicitudConstructor.objObjeto.SolicitudIndicador.FechaInicio, respuestaSolicitudConstructor.objObjeto.Constructor.Frecuencia.CantidadMeses, respuestaSolicitudConstructor.objObjeto.Constructor.Frecuencia1.CantidadMeses);
                //Cambio para la funcionalidad adecuada de la ventana los meses tienen que ser los ligados a la solicitud
                meses = lObtenerMes(respuestaSolicitudConstructor.objObjeto.IdSolicitudIndicador);
                detallesTrasformados = lTransformarConstructorDetalle(respuestaDetalleAgrupacion.objObjeto);
                raiz = detallesTrasformados[0];
                detalles = raiz.ConstructorCriterioDetalleAgrupacion1.ToList();
                jsTreeDetalleAgrupacion.Add(lRetornarNodo(raiz));
                for (int i = 0; i < detalles.Count; i++)
                {
                    jsTreeDetalleAgrupacion.Add(lRetornarNodo(detalles[i]));
                }
                respuesta.arbol = jsTreeDetalleAgrupacion;
                respuesta.meses = meses;
                jsonRespuesta.data=respuesta;
                jsonRespuesta.ok = true;
                Session["respuestaSolicitudConstructor"] = respuestaSolicitudConstructor;
                return jsonRespuesta.toJSON();
            }
            catch (CustomException cEx)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = cEx.Message;
                jsonRespuesta.data = new CObjetoModels();
                return jsonRespuesta.toJSON();
            }
            catch (Exception ex)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = ex.Message;
                jsonRespuesta.data = new CObjetoModels();
                return jsonRespuesta.toJSON();
            }
        }

        /// <summary>
        /// Obtiene el valor a modificar
        /// </summary>
        /// <param name="idConstructorDetalleAgrupacion"></param>
        /// <param name="mes"></param>
        [HttpPost]
        [AuthorizeUserAttribute]
        public String gMostrarValor(DetalleRegistroIndicador poDetalle) {
            JSONResult<CDetalleModificacionRegistroModels> jsonRespuesta = new JSONResult<CDetalleModificacionRegistroModels>();
            CDetalleModificacionRegistroModels detalleResgistro = new CDetalleModificacionRegistroModels();
            Respuesta<ConstructorCriterioDetalleAgrupacion> objRespuestaDetalleAgrupacion = new Respuesta<ConstructorCriterioDetalleAgrupacion>();
            Respuesta<DetalleRegistroIndicador> objRespuestaDetalleRegistroIndicador = new Respuesta<DetalleRegistroIndicador>();
             Respuesta<SolicitudConstructor> respuestaSolicitudConstructor = new Respuesta<SolicitudConstructor>();
             List<Provincia> listaProvincias = new List<Provincia>();
             List<Canton> listaCantones = new List<Canton>();
             List<Genero> listaGeneros = new List<Genero>();
             List<CItemModels> items = new List<CItemModels>();
            CItemModels item=new CItemModels();
            DetalleRegistroIndicador detalle = new DetalleRegistroIndicador();
            Session["objRespuestaDetalleAgrupacion"] = null;
            Session["objRespuestaDetalleRegistroIndicador"] = null;
            try {

                objRespuestaDetalleAgrupacion = refConstructorBL.gObtenerConstructorCriterioDetalleAgrupacion(poDetalle.IdConstructorCriterio);
                Session["objRespuestaDetalleAgrupacion"] = objRespuestaDetalleAgrupacion;
                if (objRespuestaDetalleAgrupacion.objObjeto.IdNivelDetalle != null)//
                {
                    //carga los combos
                    if (((int)CCatalogo.TipoNivelValor.Provincia).Equals(objRespuestaDetalleAgrupacion.objObjeto.IdNivelDetalle))
                    {
                        listaProvincias = refRegistroIndicadorExternoBL.gConsultarProvincias().objObjeto;
                        foreach (Provincia itemProvincia in listaProvincias)
                        {
                            item = new CItemModels();
                            item.idItem = itemProvincia.IdProvincia;
                            item.nombreItem = itemProvincia.Nombre;
                            items.Add(item);
                        }
                       
                    }

                    else if (((int)CCatalogo.TipoNivelValor.Canton).Equals(objRespuestaDetalleAgrupacion.objObjeto.IdNivelDetalle))
                    {
                        listaCantones = refRegistroIndicadorExternoBL.ConsultarCantones().objObjeto;
                        foreach (Canton itemCanton in listaCantones)
                        {
                            if (itemCanton.IdCanton != 0 && itemCanton.IdCanton != 84) { 
                            item = new CItemModels();
                            item.idItem = itemCanton.IdCanton;
                            item.nombreItem = itemCanton.Nombre;
                            items.Add(item);
                            }
                        }
                    }
                    else if (((int)CCatalogo.TipoNivelValor.Genero).Equals(objRespuestaDetalleAgrupacion.objObjeto.IdNivelDetalle))
                    {
                        listaGeneros = refRegistroIndicadorExternoBL.ConsultarGeneros().objObjeto;
                        foreach (Genero itemGenero in listaGeneros)
                        {
                            if (itemGenero.IdGenero != 3 && itemGenero.IdGenero != 4) { 
                                item = new CItemModels();
                                item.idItem = itemGenero.IdGenero;
                                item.nombreItem = itemGenero.Descripcion;
                                items.Add(item);
                            }
                        }
                    } 
                    detalleResgistro.items = items;
                }
                else { 
                    if(Session["respuestaSolicitudConstructor"]!=null){
                        
                        respuestaSolicitudConstructor=(Respuesta<SolicitudConstructor>)Session["respuestaSolicitudConstructor"];
                    }

                    objRespuestaDetalleRegistroIndicador = refRegistroIndicadorInternoBL.gObtenerDetalleRegistroIndicador(respuestaSolicitudConstructor.objObjeto.IdSolicitudContructor,objRespuestaDetalleAgrupacion.objObjeto.IdConstructorCriterio, poDetalle.NumeroDesglose, 0, 0, poDetalle.Anno);
                    Session["objRespuestaDetalleRegistroIndicador"] = objRespuestaDetalleRegistroIndicador;
                    detalle.IdConstructorCriterio = objRespuestaDetalleRegistroIndicador.objObjeto.IdConstructorCriterio;
                    detalle.IdDetalleRegistroindicador = objRespuestaDetalleRegistroIndicador.objObjeto.IdDetalleRegistroindicador;
                    detalle.Valor = objRespuestaDetalleRegistroIndicador.objObjeto.Valor;
                    detalleResgistro.detalle = detalle;
                }
                
              jsonRespuesta.data = detalleResgistro;
             return jsonRespuesta.toJSON();
            }
            catch (CustomException cEx)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = cEx.Message;
                jsonRespuesta.data = new CDetalleModificacionRegistroModels();
                return jsonRespuesta.toJSON();
            }
            catch (Exception ex)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = ex.Message;
                jsonRespuesta.data = new CDetalleModificacionRegistroModels();
                return jsonRespuesta.toJSON();
            }
        }


        

        /// <summary>
        /// Mustra el valor anterior segun el item seleccionado en el combo de
        /// provincia, genero o cantón
        /// </summary>
        /// <param name="piNivelDetalle"></param>
        /// <param name="piIdItem"></param>
        /// <param name="piMes"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeUserAttribute]
        public String gMostrarValorCombo(CObjetoModels poParametro) {
            JSONResult<CDetalleModificacionRegistroModels> jsonRespuesta = new JSONResult<CDetalleModificacionRegistroModels>();
            CDetalleModificacionRegistroModels detalleResgistro = new CDetalleModificacionRegistroModels();
            Respuesta<SolicitudConstructor> respuestaSolicitudConstructor = new Respuesta<SolicitudConstructor>();
            Respuesta<DetalleRegistroIndicador> objRespuestaDetalleRegistroIndicador = new Respuesta<DetalleRegistroIndicador>();
            DetalleRegistroIndicador detalle = new DetalleRegistroIndicador();
            Respuesta<ConstructorCriterioDetalleAgrupacion> objRespuestaDetalleAgrupacion = new Respuesta<ConstructorCriterioDetalleAgrupacion>();
            try {
                if (Session["respuestaSolicitudConstructor"] != null)
                {

                    respuestaSolicitudConstructor = (Respuesta<SolicitudConstructor>)Session["respuestaSolicitudConstructor"];
                }
                if (Session["objRespuestaDetalleAgrupacion"] != null) {
                  objRespuestaDetalleAgrupacion  =( Respuesta<ConstructorCriterioDetalleAgrupacion>) Session["objRespuestaDetalleAgrupacion"] ;
                }
                objRespuestaDetalleRegistroIndicador = refRegistroIndicadorInternoBL.gObtenerDetalleRegistroIndicador(respuestaSolicitudConstructor.objObjeto.IdSolicitudContructor,objRespuestaDetalleAgrupacion.objObjeto.IdConstructorCriterio, poParametro.idMes, poParametro.idNivelDetalle, poParametro.idItemSeleccionado, poParametro.idAnno);

                detalle.IdConstructorCriterio = objRespuestaDetalleRegistroIndicador.objObjeto.IdConstructorCriterio;
                detalle.IdDetalleRegistroindicador = objRespuestaDetalleRegistroIndicador.objObjeto.IdDetalleRegistroindicador;
                detalle.Valor = objRespuestaDetalleRegistroIndicador.objObjeto.Valor;

                detalleResgistro.detalle = detalle;
                Session["objRespuestaDetalleRegistroIndicador"] = objRespuestaDetalleRegistroIndicador;
                jsonRespuesta.data = detalleResgistro;
                return jsonRespuesta.toJSONLoopHandlingIgnore();
            }
            catch (CustomException cEx)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = cEx.Message;
                jsonRespuesta.data = new CDetalleModificacionRegistroModels();
                return jsonRespuesta.toJSON();
            }
            catch (Exception ex)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = ex.Message;
                jsonRespuesta.data = new CDetalleModificacionRegistroModels();
                return jsonRespuesta.toJSON();
            }
        
        }

        #region camposCarga
        /// <summary>
        /// Carga de SolitudesIndicador
        /// </summary>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        public ActionResult _tablaSolicitudIndicador()
        {
            return PartialView(new List<SolicitudIndicador>());
        }


        /// <summary>
        /// Carga de SolitudesIndicador
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeUserAttribute]
        public ActionResult _tablaSolicitudIndicador(SolicitudIndicador poSolicitud)
        {
            return PartialView(new List<SolicitudIndicador>());
        }

        /// <summary>
        /// Carga de operadores
        /// </summary>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        public ActionResult _tablaOperador()
        {
            Respuesta<List<Operador>> respuesta = new Respuesta<List<Operador>>();

            try
            {
                respuesta = refOperadorBL.ConsultarTodos();
                return PartialView("_tablaOperador", respuesta.objObjeto);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView("_tablaOperador", respuesta.objObjeto);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView("_tablaOperador", respuesta.objObjeto);
            }
        }
        /// <summary>
        /// Filtrar operadores
        /// </summary>
        /// <param name="poOperador"></param>
        /// <returns></returns>
       
        [HttpPost]
        [AuthorizeUserAttribute]
        public ActionResult _tablaOperador(Operador poOperador)
        {
            Respuesta<List<Operador>> respuesta = new Respuesta<List<Operador>>();

            try
            {
                respuesta = refOperadorBL.gFiltrarOperadores((poOperador.IdOperador == null ? "" : poOperador.IdOperador), (poOperador.NombreOperador == null ? "" : poOperador.NombreOperador));
                return PartialView("_tablaOperador", respuesta.objObjeto);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView("_tablaOperador", respuesta.objObjeto);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView("_tablaOperador", respuesta.objObjeto);
            }
        }

        /// <summary>
        /// lista los servicios por operador
        /// </summary>
        /// <param name="poOperador"></param>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        public String _listarServicios(Operador poOperador)
        {
            Respuesta<List<Servicio>> serviciosRespuesta = new Respuesta<List<Servicio>>();
            JSONResult<List<Servicio>> jsonRespuesta = new JSONResult<List<Servicio>>();
            List<Servicio> servicios = new List<Servicio>();
            Indicador nuevoIndicador = new Indicador();
            Servicio servicio = new Servicio();
            try
            {

                serviciosRespuesta = refServicioBL.ConsultarPorOperador(poOperador);
                if (serviciosRespuesta.objObjeto != null && serviciosRespuesta.objObjeto.Count > 0) {
                    foreach (Servicio item in serviciosRespuesta.objObjeto)
                    {
                        servicio = new Servicio();
                        servicio.IdServicio = item.IdServicio;
                        servicio.DesServicio = item.DesServicio;
                        servicios.Add(servicio);
                    }
                }


                jsonRespuesta.data = servicios.OrderBy(x=>x.DesServicio).ToList();
                jsonRespuesta.ok = true;
                return jsonRespuesta.toJSON();
            }
            catch (CustomException cEx)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = cEx.Message;
                jsonRespuesta.data = new List<Servicio>();
                return jsonRespuesta.toJSON();
            }
            catch (Exception ex)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = ex.Message;
                jsonRespuesta.data = new List<Servicio>();
                return jsonRespuesta.toJSON();
            }
        }
        /// <summary>
        /// lista los criterios
        /// </summary>
        /// <param name="poCriterio"></param>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        public String _listarCriterio(Criterio poCriterio)
        {
            Respuesta<List<Criterio>> criterioRespuesta = new Respuesta<List<Criterio>>();
            JSONResult<List<Criterio>> jsonRespuesta = new JSONResult<List<Criterio>>();

            Indicador nuevoIndicador = new Indicador();
            try
            {

                criterioRespuesta = refCriterioBL.gListarCriteriosPorDireccion((poCriterio.IdDireccion == null ? 0 : (int)poCriterio.IdDireccion), (poCriterio.IdIndicador == null ? "" : poCriterio.IdIndicador));




                jsonRespuesta.data = criterioRespuesta.objObjeto;
                jsonRespuesta.ok = true;
                return jsonRespuesta.toJSON();
            }
            catch (CustomException cEx)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = cEx.Message;
                jsonRespuesta.data = new List<Criterio>();
                return jsonRespuesta.toJSON();
            }
            catch (Exception ex)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = ex.Message;
                jsonRespuesta.data = new List<Criterio>();
                return jsonRespuesta.toJSON();
            }
        }


        /// <summary>
        /// tabla de indicadores 
        /// </summary>
        /// <returns></returns>

       [AuthorizeUserAttribute]
        public ActionResult _tablaIndicador()
        {
            Respuesta<List<Indicador>> respuesta = new Respuesta<List<Indicador>>();
            Direccion direccion = new Direccion();
            try
            {


                respuesta = refIndicadorBL.gObtenerIndicadorPorDireccion(direccion == null ? 0 : direccion.IdDireccion);
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

        /// <summary>
        /// tabla de indicadores por dirección
        /// </summary>
        /// <param name="direccion"></param>
        /// <returns></returns>
        
        [HttpPost]
        [AuthorizeUserAttribute]
        public ActionResult _tablaIndicador(ModificacionIndicadorFiltroViewModel poIndicador)
        {
            Respuesta<List<Indicador>> respuesta = new Respuesta<List<Indicador>>();

            try
            {


                respuesta = refIndicadorBL.gObtenerIndicadorModificacioIndicador((poIndicador.IdDireccion == null ? 0 : poIndicador.IdDireccion)
                                                                                , (poIndicador.IdServicio == null ? 0 : poIndicador.IdServicio)
                                                                                , (poIndicador.IdIndicador == null ? "" : poIndicador.IdIndicador)
                                                                                , (poIndicador.NombreIndicador == null ? "" : poIndicador.NombreIndicador)
                                                                                , (poIndicador.TipoIndicador==null?"":poIndicador.TipoIndicador));
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
        #endregion

#endregion

        #region guardarDatos
        /// <summary>
        /// Guarda el valor de la modificación
        /// </summary>
        /// <param name="poDetalle"></param>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        public String gGuardarNuevoValor(DetalleRegistroIndicador poDetalle) {
            Respuesta<DetalleRegistroIndicador> objRespuestaDetalleRegistroIndicador = new Respuesta<DetalleRegistroIndicador>();
            Respuesta<DetalleRegistroIndicador> objRespuestaDetalleRegistroIndicadorModificado = new Respuesta<DetalleRegistroIndicador>();
            JSONResult<DetalleRegistroIndicador> jsonRespuesta = new JSONResult<DetalleRegistroIndicador>();
            Respuesta<BitacoraIndicador> objRespuestaBitacora = new Respuesta<BitacoraIndicador>();
            BitacoraIndicador nuevaBitacora = new BitacoraIndicador();
       
     
            String justificacion = "";
            try {
                justificacion = poDetalle.Comentario;
                if (Session["objRespuestaDetalleRegistroIndicador"] != null) { 
                objRespuestaDetalleRegistroIndicador=(Respuesta<DetalleRegistroIndicador>) Session["objRespuestaDetalleRegistroIndicador"] ;
                objRespuestaDetalleRegistroIndicadorModificado= refRegistroIndicadorInternoBL.gModificarRegistroIndicador(objRespuestaDetalleRegistroIndicador.objObjeto.IdDetalleRegistroindicador, poDetalle.Valor, justificacion);
                if (objRespuestaDetalleRegistroIndicadorModificado.blnIndicadorTransaccion)
                {
                  
                    nuevaBitacora.ValorAnterior = objRespuestaDetalleRegistroIndicador.objObjeto.Valor;
                    nuevaBitacora.ValorNuevo = poDetalle.Valor;
                    
                    nuevaBitacora.IdUsuario = ConsultarUsuario().IdUsuario;
                    nuevaBitacora.Justificacion = justificacion;
                    nuevaBitacora.IdDetalleRegistroIndicador = objRespuestaDetalleRegistroIndicador.objObjeto.IdDetalleRegistroindicador;
                    objRespuestaBitacora = refBitacoraIndicadorInternoBL.gInsertarBitacoraIndicadorExterno(nuevaBitacora);
                    jsonRespuesta.data =new DetalleRegistroIndicador();
                }
                else {
                    jsonRespuesta.ok = false;
                    jsonRespuesta.strMensaje = objRespuestaDetalleRegistroIndicadorModificado.strMensaje;
                }
                    
             }
            }
             catch (CustomException cEx)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = cEx.Message;
                jsonRespuesta.data = new DetalleRegistroIndicador();
                return jsonRespuesta.toJSON();
            }
            catch (Exception ex)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = ex.Message;
                jsonRespuesta.data = new DetalleRegistroIndicador();
                return jsonRespuesta.toJSON();
            }
            return jsonRespuesta.toJSON();
        }


        private Usuario ConsultarUsuario()
        {
            Usuario usuario = new Usuario();
            string nombreUsuario = "";
            if (((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault() != null)
            {
                nombreUsuario = ((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault().Value;
                BL.Seguridad.UsersBL refUsuario = new BL.Seguridad.UsersBL(AppContext);
                usuario = refUsuario.ConsultarPorExpresion(x => x.AccesoUsuario == nombreUsuario && x.Borrado == 0 && x.Activo == 1).objObjeto;
            }

            return usuario;
        }

        #endregion
        #region transformacionDatos
        /// <summary>
        /// Calculas los meses
        /// </summary>
        /// <param name="poFechaInicial"></param>
        /// <param name="cantidadMesesFrecuencia"></param>
        /// <param name="cantidadMesesDesglose"></param>
        /// <returns></returns>
        private List<CMes> lObtenerMes_old(DateTime poFechaInicial, int cantidadMesesFrecuencia, int cantidadMesesDesglose) {
            List<String> mesesNombre = new List<string>(new String[] { "Enero", "Febrero", "Marzo","Abril","Mayo","Junio","Julio","Agosto","Setiembre","Octubre","Noviembre","Diciembre" });
            List<CMes> meses = new List<CMes>();
            CMes mes = new CMes();
            int cantidadMeses = 0;
            cantidadMeses = cantidadMesesFrecuencia / cantidadMesesDesglose;
            DateTime fecha = new DateTime();
            fecha = poFechaInicial.AddMonths(-cantidadMesesDesglose);
            mes.anno =fecha.Year ;
            mes.idMes = fecha.Month.ToString() + "|" + fecha.Year.ToString();
            mes.nombreMes = mesesNombre[fecha.Month - 1 ];
            meses.Add(mes);
            for (int i = 1; i < cantidadMeses; i++)
            {
                mes = new CMes();
                fecha = fecha.AddMonths(-cantidadMesesDesglose);
                mes.anno =fecha.Year ;
                mes.idMes = fecha.Month.ToString() + "|" + fecha.Year.ToString();
                mes.nombreMes = mesesNombre[fecha.Month -1];
                meses.Add(mes);
            }
            return meses.OrderBy(x=>x.anno).ThenBy(y=> y.idMes).ToList();
        }

        private List<CMes> lObtenerMes(Guid solicitudIndicador)
        {
            Respuesta<List<CMes>> respuesta = new Respuesta<List<CMes>>();
            List<CMes> meses = new List<CMes>();
            try
            {
                respuesta = refRegistroIndicadorInternoBL.lDetalleFechasSolicitudIndicador(solicitudIndicador);
                meses = respuesta.objObjeto;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
            }

            return meses;
        }
        private ConstructorDetalleAgrupacionViewModels lRetornarNodo(ConstructorCriterioDetalleAgrupacion detalleAgrupacion)
        {
            ConstructorDetalleAgrupacionViewModels nDetalle = new ConstructorDetalleAgrupacionViewModels();
            nDetalle.id = detalleAgrupacion.DetalleAgrupacion.IdOperador;
            nDetalle.parent = detalleAgrupacion.ConstructorCriterioDetalleAgrupacion2 == null ? "#" : detalleAgrupacion.ConstructorCriterioDetalleAgrupacion2.DetalleAgrupacion.IdOperador;
            nDetalle.text = detalleAgrupacion.DetalleAgrupacion.Operador != null ? detalleAgrupacion.DetalleAgrupacion.Operador.NombreOperador : detalleAgrupacion.DetalleAgrupacion.DescDetalleAgrupacion;
            nDetalle.ultimoNivel = detalleAgrupacion.UltimoNivel == null ? 0 : (int)detalleAgrupacion.UltimoNivel;
            nDetalle.idConstructorCriterioDetalleAgrupacion = detalleAgrupacion.IdConstructorCriterio;
            if (nDetalle.ultimoNivel == 0)
            {
                nDetalle.idTipoValor = 0;
                nDetalle.valorInferior = "";
                nDetalle.valorSuperior = "";
                nDetalle.idNivel = 0;
            }
            else
            {
                nDetalle.idTipoValor = detalleAgrupacion.IdTipoValor == null ? 0 : (int)detalleAgrupacion.IdTipoValor;
                nDetalle.idTipoNivelDetalle = detalleAgrupacion.IdNivelDetalle == null ? 0 : (int)detalleAgrupacion.IdNivelDetalle;
                nDetalle.idNivel = detalleAgrupacion.IdNivelDetalle == null ? 0 : (int)detalleAgrupacion.IdNivelDetalle;
                if (detalleAgrupacion.Regla != null)
                {
                    nDetalle.valorInferior = detalleAgrupacion.Regla.ValorLimiteInferior;
                    nDetalle.valorSuperior = detalleAgrupacion.Regla.ValorLimiteSuperior;
                }
                else
                {
                    nDetalle.valorInferior = "";
                    nDetalle.valorSuperior = "";

                }
            }
            return nDetalle;

        }

        /// <summary>
        /// Transforma los detalles agrupación de la forma que se pueda leer 
        /// luego en el arbol de detalles agrupación
        /// </summary>
        /// <param name="detalles"></param>
        /// <returns></returns>
        private List<ConstructorCriterioDetalleAgrupacion> lTransformarConstructorDetalle(List<ConstructorCriterioDetalleAgrupacion> detalles)
        {
            List<ConstructorCriterioDetalleAgrupacion> resultado = new List<ConstructorCriterioDetalleAgrupacion>();
            List<ConstructorCriterioDetalleAgrupacion> detallesXOperador = new List<ConstructorCriterioDetalleAgrupacion>();
            ConstructorCriterioDetalleAgrupacion nodoPadre = new ConstructorCriterioDetalleAgrupacion();
            ConstructorCriterioDetalleAgrupacion nodoHijo = new ConstructorCriterioDetalleAgrupacion();
            ConstructorCriterioDetalleAgrupacion nodo = new ConstructorCriterioDetalleAgrupacion();
            List<String> idOperadores = detalles.Select(x => x.IdOperador).Distinct().ToList();


            String id = "";
            String idPadre = "";
            foreach (String item in idOperadores)
            {
                detallesXOperador = detalles.Where(x => x.IdOperador.Equals(item)).OrderBy(x=> (x.Orden==null?x.IdNivel:x.Orden)).ToList();
                nodoPadre = new ConstructorCriterioDetalleAgrupacion();
                nodoPadre.DetalleAgrupacion = new DetalleAgrupacion();
                nodoPadre.DetalleAgrupacion.Operador = new Operador();
                nodoPadre.IdOperador = detallesXOperador[0].IdOperador;
                nodoPadre.DetalleAgrupacion.IdOperador = "0|0|" + detallesXOperador[0].IdOperador;
                nodoPadre.DetalleAgrupacion.Operador.NombreOperador = detallesXOperador[0].DetalleAgrupacion.Operador.NombreOperador;
                nodoPadre.ConstructorCriterioDetalleAgrupacion1 = new List<ConstructorCriterioDetalleAgrupacion>();
                for (int i = 0; i < detallesXOperador.Count; i++)
                {
                    nodoHijo = new ConstructorCriterioDetalleAgrupacion();
                    nodoHijo.DetalleAgrupacion = new DetalleAgrupacion();
                    nodoHijo.IdConstructorCriterio = detallesXOperador[i].IdConstructorCriterio;
                    nodoHijo.ConstructorCriterioDetalleAgrupacion2 = new ConstructorCriterioDetalleAgrupacion();
                    nodoHijo.ConstructorCriterioDetalleAgrupacion2.DetalleAgrupacion = new DetalleAgrupacion();
                    nodoHijo.ConstructorCriterioDetalleAgrupacion2.IdDetalleAgrupacion = (detallesXOperador[i].ConstructorCriterioDetalleAgrupacion2 == null ? 0 : detallesXOperador[i].ConstructorCriterioDetalleAgrupacion2.IdDetalleAgrupacion);
                    nodoHijo.ConstructorCriterioDetalleAgrupacion2.IdAgrupacion = (detallesXOperador[i].ConstructorCriterioDetalleAgrupacion2 == null ? 0 : detallesXOperador[i].ConstructorCriterioDetalleAgrupacion2.IdAgrupacion);
                    nodoHijo.ConstructorCriterioDetalleAgrupacion2.IdOperador = (detallesXOperador[i].ConstructorCriterioDetalleAgrupacion2 == null ? item : detallesXOperador[i].ConstructorCriterioDetalleAgrupacion2.IdOperador);
                    if (detallesXOperador[i].IdConstructorDetallePadre == null)
                    {
                        nodoHijo.ConstructorCriterioDetalleAgrupacion2.DetalleAgrupacion.IdOperador = nodoPadre.DetalleAgrupacion.IdOperador;
                    }
                    else
                    {
                        nodoHijo.IdConstructorDetallePadre = detallesXOperador[i].IdConstructorDetallePadre;
                    }
                   
                    nodoHijo.IdDetalleAgrupacion = detallesXOperador[i].IdDetalleAgrupacion;
                    nodoHijo.IdAgrupacion = detallesXOperador[i].IdAgrupacion;
                    nodoHijo.IdOperador = detallesXOperador[i].IdOperador;
                    nodoHijo.UltimoNivel = detallesXOperador[i].UltimoNivel;
                    nodoHijo.IdNivel = detallesXOperador[i].IdNivel;
                    if (detallesXOperador[i].UltimoNivel.Equals((byte)1))
                    {
                        nodoHijo.Regla = new Regla();

                        if (detallesXOperador[i].Regla != null)
                        {
                            nodoHijo.Regla.ValorLimiteInferior = detallesXOperador[i].Regla.ValorLimiteInferior;
                            nodoHijo.Regla.ValorLimiteSuperior = detallesXOperador[i].Regla.ValorLimiteSuperior;
                            nodoHijo.IdTipoValor = detallesXOperador[i].IdTipoValor;
                            nodoHijo.IdNivelDetalle = detallesXOperador[i].IdNivelDetalle;
                        }
                    }
                    nodo = lObtenerPadreDetalle(nodoPadre.ConstructorCriterioDetalleAgrupacion1.ToList(), nodoHijo);
                    //id
                    idPadre = lIdPadre(detallesXOperador[i]) + nodoPadre.DetalleAgrupacion.IdOperador;
                    id = detallesXOperador[i].IdDetalleAgrupacion.ToString() + "|" + detallesXOperador[i].IdAgrupacion.ToString() + "|" + detallesXOperador[i].IdOperador;
                    nodoHijo.DetalleAgrupacion.IdOperador = id + (idPadre != "" ? "|" + idPadre : "");
                    string[] ids = nodoHijo.DetalleAgrupacion.IdOperador.Split('|'); ;
                    nodoHijo.IdNivel = (ids.Length / 3) - 1;
                    //texto
                    nodoHijo.DetalleAgrupacion.DescDetalleAgrupacion = detallesXOperador[i].DetalleAgrupacion.Agrupacion.DescAgrupacion + "/" + detallesXOperador[i].DetalleAgrupacion.DescDetalleAgrupacion;
                    //padre
                    nodoHijo.ConstructorCriterioDetalleAgrupacion2.DetalleAgrupacion.IdOperador = (idPadre != "" ? idPadre : nodoPadre.DetalleAgrupacion.IdOperador);
                    nodoPadre.ConstructorCriterioDetalleAgrupacion1.Add(nodoHijo);
                }
                resultado.Add(nodoPadre);
            }
            return resultado;
        }
        /// <summary>
        /// obtiene el id del padre data la estructura
        /// </summary>
        /// <param name="detalle"></param>
        /// <returns></returns>
        private String lIdPadre(ConstructorCriterioDetalleAgrupacion detalle)
        {
            String id = "";
            if (detalle.IdConstructorDetallePadre == null)
            {
                id = "";
            }
            else
            {
                id = detalle.ConstructorCriterioDetalleAgrupacion2.IdDetalleAgrupacion + "|" + detalle.ConstructorCriterioDetalleAgrupacion2.IdAgrupacion + "|" + detalle.ConstructorCriterioDetalleAgrupacion2.IdOperador + "|" + lIdPadre(detalle.ConstructorCriterioDetalleAgrupacion2);
            }
            return id;
        }


        /// <summary>
        /// Obtiene el detalle agrupación padre deacuerdo al IdConstructorDetallePadre
        /// </summary>
        /// <param name="detalles"></param>
        /// <param name="detalle"></param>
        /// <returns></returns>
        public ConstructorCriterioDetalleAgrupacion lObtenerPadreDetalle(List<ConstructorCriterioDetalleAgrupacion> detalles, ConstructorCriterioDetalleAgrupacion detalle)
        {
            foreach (ConstructorCriterioDetalleAgrupacion item in detalles)
            {
                if (item.IdConstructorCriterio.Equals(detalle.IdConstructorDetallePadre))
                {
                    return item;
                }
            }
            return null;
        }
        #endregion


      

    }
}

public class Combos
{
    public string id { get; set; }

    public string valor { get; set; }
}
