using GB.SUTEL.BL.Mantenimientos;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GB.SUTEL.Entities;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.UI.Helpers;
using System.Text;
using GB.SUTEL.UI.Recursos.Utilidades;
using System.Globalization;
using GB.SUTEL.Resources;
using System.Configuration;
using System.IO;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using GB.SUTEL.Entities.DTO;

namespace GB.SUTEL.UI.Controllers
{
    public class SolicitudController : BaseController
    {
        #region atributos
        SolicitudIndicadorBL solicitudBL;
        RegistroIndicadorBL refRegistroIndicadorBL;
        DireccionBL direccionBL;
        FrecuenciaBL frecuenciaBL;
        ServicioBL servicioBL;
        OperadorBL operadorBL;
        Funcion func = new Funcion();
        #endregion

        #region Constructor
        public SolicitudController()
        {

            solicitudBL = new SolicitudIndicadorBL(AppContext);
            refRegistroIndicadorBL = new RegistroIndicadorBL(AppContext);
            direccionBL = new DireccionBL();
            frecuenciaBL = new FrecuenciaBL(AppContext);
            servicioBL = new ServicioBL(AppContext);
            operadorBL = new OperadorBL(AppContext);
        }


        #endregion

        #region Vistas

        #region Inicio
        [AuthorizeUserAttribute]
        public ActionResult Index(bool? e, bool? em)
        {
            GB.SUTEL.UI.Models.SolicitudViewModels modelo = new SolicitudViewModels();

            modelo.listadoSolicitudes = solicitudBL.gConsultarPorFiltros(string.Empty, DateTime.Today.AddMonths(-Properties.Settings.Default.MostrarMesesSolicitudOmision), null, null).objObjeto; // .gListar().objObjeto;
            modelo.listadoServicios = servicioBL.ConsultarTodos().objObjeto;
            //modelo.ServicioSeleccionados = modelo.listadoServicios.Select(s => s.IdServicio).Distinct().ToList();
            this.cargarDireccionesEnBag();
            this.cargarFrecuenciasEnBag();
            this.cargarServiciosEnBag();

            string user;
            user = User.Identity.GetUserId();
            try
            {
                func._index(user, "Solicitud", "Solicitud Procesos");
            }

            catch (Exception exe)
            {
                Console.WriteLine("{0} Exception caught.", exe);
            }

            //exito
            ViewBag.mostrarMensajeGuardo = false;
            ViewBag.MensajeGuardo = string.Empty;

            ViewBag.mostrarMensajeActualizo = false;
            ViewBag.MensajeActualizo = string.Empty;

            if (e != null)
            {
                ViewBag.mostrarMensajeGuardo = e;
                ViewBag.MensajeGuardo = Mensajes.ExitoInsertar;
            }

            if (em != null)
            {
                ViewBag.mostrarMensajeGuardo = em;
                ViewBag.MensajeGuardo = Mensajes.ExitoEditar;
            }

            return View(modelo);
        }


        #endregion

        #region Agregar
        [AuthorizeUserAttribute]
        public ActionResult Crear()
        {
            this.cargarDireccionesEnBag();
            this.cargarFrecuenciasEnBag();
            this.cargarServiciosEnBag();
            ViewBag.TerminosBusquedaOPERADOR = new Operador();
            //return View(new GB.SUTEL.UI.Models.SolicitudViewModels() { new List<SolicitudIndicador>().Add(new SolicitudIndicador()), new List<getListaIndicadoresSolicitud_Result>().Add(new getListaIndicadoresSolicitud_Result()), new List<getListaIndicadoresSolicitud_Result>().Add(new getListaIndicadoresSolicitud_Result()), new SolicitudIndicador() });
            return View(new GB.SUTEL.UI.Models.SolicitudViewModels());
        }

        [HttpPost]
        [AuthorizeUserAttribute]
        public string Crear(SolicitudIndicador Item1)
        {
            string descFormulario = Request.Params["data[descFormulario]"];
            string fechaInicio = Request.Params["data[fechaInicio]"];
            string fechaFinal = Request.Params["data[fechaFinal]"];
            int idServicio = int.Parse(Request.Params["data[idServicio]"]);
            int idFrecuencia = int.Parse(Request.Params["data[idFrecuencia]"]);
            int idDireccion = int.Parse(Request.Params["data[idDireccion]"]);
            bool activo = bool.Parse(Request.Params["data[activo]"]);
            int formularioWeb = bool.Parse(Request.Params["data[formularioWeb]"]) ? int.Parse(GB.SUTEL.Shared.Logica.FormularioWeb) : int.Parse(GB.SUTEL.Shared.Logica.FormularioExcel);
            Session["FormularioWeb"] = bool.Parse(Request.Params["data[formularioWeb]"]);

            string ultimoMes = Request.Params["data[ultimoMes]"];
            int anno = int.Parse(Request.Params["data[annoDatosExcel]"]);


            Respuesta<SolicitudIndicador> respuesta = new Respuesta<SolicitudIndicador>();
            JSONResult<SolicitudIndicador> jsonRespuesta = new JSONResult<SolicitudIndicador>();

            try
            {
                SolicitudIndicador solicitudAux = new SolicitudIndicador();

                solicitudAux.DescFormulario = descFormulario;
                solicitudAux.FechaFin = DateTime.ParseExact(fechaFinal, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                solicitudAux.FechaInicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                ViewBag.TerminosBusquedaOPERADOR = new Operador();
                //validacion de fechas
                if (solicitudAux.FechaFin < solicitudAux.FechaInicio)
                {

                    jsonRespuesta.ok = false;
                    jsonRespuesta.data = null;
                    jsonRespuesta.strMensaje = "La fecha final no puede ser menor a la fecha inicial";
                    return jsonRespuesta.toJSON();
                }


                //validacion de operadores
                Respuesta<List<Operador>> listaOperadores = operadorBL.ConsultarXServicio(idServicio);

                if (listaOperadores.objObjeto == null || listaOperadores.objObjeto.Count() == 0)
                {
                    jsonRespuesta.ok = false;
                    jsonRespuesta.data = null;
                    jsonRespuesta.strMensaje = "No hay Operadores para el servicio seleccionado, intente nuevamente";
                    return jsonRespuesta.toJSON();
                }


                //validacion de existencia de constructores/indicadores
                Respuesta<List<pa_getListaIndicadoresSolicitud_Result>> listaIndicadores = this.solicitudBL.gConsultarIndicadoresPorDirFrecServ(idDireccion, idFrecuencia, idServicio);
                if (listaIndicadores.objObjeto.Count() == 0)
                {
                    jsonRespuesta.ok = false;
                    jsonRespuesta.data = null;
                    jsonRespuesta.strMensaje = "No hay Indicadores de constructor para el Servicio, Dirección y Frecuencia seleccionada, intente nuevamente";
                    return jsonRespuesta.toJSON();
                }


                if (ultimoMes == "-1")
                {
                    jsonRespuesta.ok = false;
                    jsonRespuesta.data = null;
                    jsonRespuesta.strMensaje = "Seleccione un Detalle Frecuencia, e intente nuevamente";
                    return jsonRespuesta.toJSON();
                }

                if (anno == -1)
                {
                    jsonRespuesta.ok = false;
                    jsonRespuesta.data = null;
                    jsonRespuesta.strMensaje = "Seleccione un año, e intente nuevamente";
                    return jsonRespuesta.toJSON();
                }

                //
                solicitudAux.IdServicio = idServicio;
                solicitudAux.IdFrecuencia = idFrecuencia;
                solicitudAux.IdDireccion = idDireccion;
                solicitudAux.Activo = activo == true ? (byte)1 : (byte)0;
                solicitudAux.FormularioWeb = formularioWeb;
                if (ultimoMes == "-2") // Entonces la frecuencia elegida fue Anual

                    solicitudAux.FechaBaseParaCrearExcel = DateTime.ParseExact("01/01/" + anno, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                else
                {

                    if (ultimoMes == "12")
                        solicitudAux.FechaBaseParaCrearExcel = DateTime.ParseExact("01/01/" + (anno + 1), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    else
                    {

                        if (ultimoMes == "10" || ultimoMes == "11")
                            solicitudAux.FechaBaseParaCrearExcel = DateTime.ParseExact("01/" + ultimoMes + "/" + anno, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        else
                            solicitudAux.FechaBaseParaCrearExcel = DateTime.ParseExact("01/" + ultimoMes + "/" + anno, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        //solicitudAux.FechaBaseParaCrearExcel = solicitudAux.FechaBaseParaCrearExcel;
                        solicitudAux.FechaBaseParaCrearExcel = solicitudAux.FechaBaseParaCrearExcel.AddMonths(1);
                    }

                }



                //registro
                solicitudBL = new SolicitudIndicadorBL(solicitudAux, this.AppContext);
                respuesta = solicitudBL.gAgregar();

                if (respuesta.blnIndicadorTransaccion)
                {
                    jsonRespuesta.data = new SolicitudIndicador();
                    jsonRespuesta.data.IdSolicitudIndicador = respuesta.objObjeto.IdSolicitudIndicador;
                    jsonRespuesta.strMensaje = respuesta.strMensaje + ", por favor haga clic en la pestaña Indicadores";
                    //bitacora
                    gBitacoraSolicitud(ActionsBinnacle.Crear, respuesta.objObjeto, null);
                    return jsonRespuesta.toJSON();
                }
                else
                {
                    jsonRespuesta.strMensaje = respuesta.strMensaje;
                    jsonRespuesta.ok = false;
                    jsonRespuesta.data = null;

                }
            }
            catch (Exception ex)
            {
                jsonRespuesta.ok = false;

            }

            return jsonRespuesta.toJSON();
        }

        #endregion

        #region Modificar
        [AuthorizeUserAttribute]
        public ActionResult Editar(string id)
        {
            string idSolicitud = id;
            this.cargarDireccionesEnBag();
            this.cargarFrecuenciasEnBag();
            this.cargarServiciosEnBag();
            ViewBag.adventencia = null;
            ViewBag.TerminosBusquedaOPERADOR = new Operador();

            Session["adventencia"] = null;

            ViewBag.listaIndicadores = convertListaIndicadores(IndicadoresXSolicitud(Guid.Parse(idSolicitud)));
            ViewBag.listaOperadores = OperadoresXSolicitud(Guid.Parse(idSolicitud)).objObjeto;

            Respuesta<SolicitudIndicador> solicitudIndicador = new Respuesta<SolicitudIndicador>();
            solicitudIndicador = solicitudBL.gConsultarPorIdentificador(Guid.Parse(id));
            if (solicitudIndicador.objObjeto.FechaInicio <= DateTime.Now)
            {
                ViewBag.adventencia = GB.SUTEL.Shared.ErrorTemplate.Solicitud_solicitudIniciada;
                Session["adventencia"] = GB.SUTEL.Shared.ErrorTemplate.Solicitud_solicitudIniciada;
            }


            return View(new GB.SUTEL.UI.Models.SolicitudViewModels()
            {
                itemSolicitudIndicador = solicitudIndicador.objObjeto,
                listadoIndicadores = new List<pa_getListaIndicadoresSolicitud_Result>(),
                listadoIndicadoresXOperador = new List<pa_getListaIndicadoresXSolicitud_Result>(),
                listadoSolicitudes = new List<SolicitudIndicador>()
            });
        }





        [HttpPost]
        [AuthorizeUserAttribute]
        public string Editar(SolicitudIndicador Item1)
        {
            string idSolicitudIndicador = Request.Params["data[idSolicitudIndicador]"];
            string descFormulario = Request.Params["data[descFormulario]"];
            string fechaInicio = Request.Params["data[fechaInicio]"];
            string fechaFinal = Request.Params["data[fechaFinal]"];
            int idServicio = int.Parse(Request.Params["data[idServicio]"]);
            int idFrecuencia = int.Parse(Request.Params["data[idFrecuencia]"]);
            int idDireccion = int.Parse(Request.Params["data[idDireccion]"]);
            bool activo = bool.Parse(Request.Params["data[activo]"]);

            string ultimoMes = Request.Params["data[ultimoMes]"];
            int anno = int.Parse(Request.Params["data[annoDatosExcel]"]);

            Respuesta<SolicitudIndicador> respuesta = new Respuesta<SolicitudIndicador>();
            JSONResult<SolicitudIndicador> jsonRespuesta = new JSONResult<SolicitudIndicador>();
            SolicitudIndicador objetoAnterior = null;
            try
            {
                ViewBag.TerminosBusquedaOPERADOR = new Operador();
                SolicitudIndicador solicitudAux = new SolicitudIndicador();
                SolicitudIndicadorBL logicaAux = new SolicitudIndicadorBL(this.AppContext);
                objetoAnterior = logicaAux.gConsultarPorIdentificador(Guid.Parse(idSolicitudIndicador)).objObjeto;

                solicitudAux.IdSolicitudIndicador = Guid.Parse(idSolicitudIndicador);
                solicitudAux.DescFormulario = descFormulario;


                solicitudAux.FechaFin = DateTime.ParseExact(fechaFinal, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                solicitudAux.FechaInicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                //validacion de fechas
                if (solicitudAux.FechaFin < solicitudAux.FechaInicio)
                {

                    jsonRespuesta.ok = false;
                    jsonRespuesta.data = null;
                    jsonRespuesta.strMensaje = "La fecha final no puede ser menor a la fecha inicial";
                    return jsonRespuesta.toJSON();
                }
                else if (solicitudAux.FechaInicio <= DateTime.Now)
                {

                    jsonRespuesta.ok = false;
                    jsonRespuesta.data = null;
                    jsonRespuesta.strMensaje = "La fecha inicial no puede ser igual o menor a la del día de hoy.";
                    return jsonRespuesta.toJSON();
                }

                if (ultimoMes == "-1")
                {
                    jsonRespuesta.ok = false;
                    jsonRespuesta.data = null;
                    jsonRespuesta.strMensaje = "Seleccione un Detalle Frecuencia, e intente nuevamente";
                    return jsonRespuesta.toJSON();
                }

                if (anno == -1)
                {
                    jsonRespuesta.ok = false;
                    jsonRespuesta.data = null;
                    jsonRespuesta.strMensaje = "Seleccione un año, e intente nuevamente";
                    return jsonRespuesta.toJSON();
                }

                solicitudAux.IdServicio = idServicio;
                solicitudAux.IdFrecuencia = idFrecuencia;
                solicitudAux.IdDireccion = idDireccion;
                solicitudAux.Activo = activo == true ? (byte)1 : (byte)0;


                if (ultimoMes == "-2") // Entonces la frecuencia elegida fue Anual

                    solicitudAux.FechaBaseParaCrearExcel = DateTime.ParseExact("01/01/" + anno, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                else
                {

                    if (ultimoMes == "12")
                        solicitudAux.FechaBaseParaCrearExcel = DateTime.ParseExact("01/01/" + (anno + 1), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    else
                    {

                        if (ultimoMes == "10" || ultimoMes == "11")
                            solicitudAux.FechaBaseParaCrearExcel = DateTime.ParseExact("01/" + ultimoMes + "/" + anno, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        else
                            solicitudAux.FechaBaseParaCrearExcel = DateTime.ParseExact("01/" + ultimoMes + "/" + anno, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        //ojo revisar los posibles conflictos de esto 
                        //solicitudAux.FechaBaseParaCrearExcel = solicitudAux.FechaBaseParaCrearExcel.AddMonths(1);
                        solicitudAux.FechaBaseParaCrearExcel = solicitudAux.FechaBaseParaCrearExcel;






                    }

                }

                //registro
                solicitudBL = new SolicitudIndicadorBL(solicitudAux, this.AppContext);
                respuesta = solicitudBL.gModificar();

                if (respuesta.blnIndicadorTransaccion)
                {
                    jsonRespuesta.data = null;
                    //bitacora
                    gBitacoraSolicitud(ActionsBinnacle.Editar, respuesta.objObjeto, objetoAnterior);
                    return jsonRespuesta.toJSON();
                }
                else
                {
                    jsonRespuesta.strMensaje = respuesta.strMensaje;
                    jsonRespuesta.ok = false;
                    jsonRespuesta.data = null;

                }
            }
            catch (Exception ex)
            {
                jsonRespuesta.ok = false;

            }

            return jsonRespuesta.toJSON();
        }
        #endregion

        #region eliminar
        [HttpPost]
        [AuthorizeUserAttribute]
        public string Eliminar(string ItemEliminar)
        {

            Respuesta<SolicitudIndicador> respuesta = new Respuesta<SolicitudIndicador>();
            JSONResult<SolicitudIndicador> jsonRespuesta = new JSONResult<SolicitudIndicador>();
            try
            {
                SolicitudIndicador solicitud = new SolicitudIndicador();
                solicitud.IdSolicitudIndicador = Guid.Parse(ItemEliminar);
                solicitudBL = new SolicitudIndicadorBL(solicitud, this.AppContext);
                //respuesta = solicitudBL.gEliminar(solicitud.IdSolicitudIndicador);

                if (respuesta.blnIndicadorTransaccion)
                {
                    jsonRespuesta.data = solicitud;
                    //bitacora
                    gBitacoraSolicitud(ActionsBinnacle.Borrar, null, respuesta.objObjeto);

                }
                else
                {
                    jsonRespuesta.ok = false;


                }

                jsonRespuesta.strMensaje = respuesta.strMensaje;
                return jsonRespuesta.toJSONLoopHandlingIgnore();

            }
            catch (Exception ex)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = ex.Message;
                return jsonRespuesta.toJSON();
            }
        }
        #endregion
        public string EliminarOpcional()
        {
            Guid ItemEliminar = Guid.Parse(Request.Params["data[ItemEliminar]"]);
            string idOperadores = Request.Params["data[Operadores][]"];
            bool Completa = bool.Parse(Request.Params["data[Completa]"]);
            Respuesta<SolicitudIndicador> respuesta = new Respuesta<SolicitudIndicador>();
            JSONResult<SolicitudIndicador> jsonRespuesta = new JSONResult<SolicitudIndicador>();

            if (Completa == false && idOperadores is null)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = "Por favor defina si es parcial o completa la eliminación de la solicitud";
                return jsonRespuesta.toJSON();
            }
            if (idOperadores is null) // esto es un parche para controlar que el erray nunca llegue vacio 
            {
                idOperadores = "O0002,O0002,O0002";

            }

            try
            {
                SolicitudIndicador solicitud = new SolicitudIndicador();
                solicitud.IdSolicitudIndicador = ItemEliminar;
                solicitudBL = new SolicitudIndicadorBL(solicitud, this.AppContext);
                respuesta = solicitudBL.gEliminaropcional(solicitud.IdSolicitudIndicador, idOperadores.Split(','), Completa);

                if (respuesta.blnIndicadorTransaccion)
                {
                    jsonRespuesta.data = solicitud;
                    //bitacora
                    gBitacoraSolicitud(ActionsBinnacle.Borrar, null, respuesta.objObjeto);

                }
                else
                {
                    jsonRespuesta.ok = false;
                }

                jsonRespuesta.strMensaje = respuesta.strMensaje;
                return jsonRespuesta.toJSONLoopHandlingIgnore();

            }
            catch (Exception ex)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = ex.Message;
                return jsonRespuesta.toJSON();
            }
        }
        #endregion
        public string EditarFormularioWeb()
        {
            Guid ItemEditar = Guid.Parse(Request.Params["data[ItemEditar]"]);
            string idOperadores = Request.Params["data[Operadores][]"];
            bool formularioWeb = bool.Parse(Request.Params["data[FormularioWeb]"]);
            bool editarOperadores = bool.Parse(Request.Params["data[EditarOperadores]"]);
            //bool Completa = bool.Parse(Request.Params["data[Completa]"]);
            Respuesta<SolicitudIndicador> respuesta = new Respuesta<SolicitudIndicador>();
            JSONResult<SolicitudIndicador> jsonRespuesta = new JSONResult<SolicitudIndicador>();

            if (idOperadores is null && !editarOperadores)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = "Por favor seleccione al menos un operador que desee actualizar";
                return jsonRespuesta.toJSON();
            }
            else if (editarOperadores)
            {
                idOperadores = "";
            }

            try
            {
                SolicitudIndicador solicitud = new SolicitudIndicador();
                solicitud.IdSolicitudIndicador = ItemEditar;
                solicitudBL = new SolicitudIndicadorBL(solicitud, this.AppContext);
                respuesta = solicitudBL.gEditarFormularioWeb(solicitud.IdSolicitudIndicador, idOperadores.Split(','), formularioWeb, editarOperadores);

                if (respuesta.blnIndicadorTransaccion)
                {
                    jsonRespuesta.data = solicitud;
                    //bitacora
                    gBitacoraSolicitud(ActionsBinnacle.Editar, null, respuesta.objObjeto);

                }
                else
                {
                    jsonRespuesta.ok = false;
                }

                jsonRespuesta.strMensaje = respuesta.strMensaje;
                return jsonRespuesta.toJSONLoopHandlingIgnore();

            }
            catch (Exception ex)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = ex.Message;
                return jsonRespuesta.toJSON();
            }
            //return jsonRespuesta.toJSON();
        }
        #region Listar

        #endregion

        #region Indicadores

        /// <summary>
        /// lista los indicadores(constructores) y los operadores en agregar
        /// </summary>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        public ActionResult Indicadores()
        {
            int idServicio = int.Parse(Request.Params["data[IdServicio]"]);
            int idFrecuencia = int.Parse(Request.Params["data[IdFrecuencia]"]);
            int idDireccion = int.Parse(Request.Params["data[IdDireccion]"]);
            Guid idIndicador = Guid.Parse(Request.Params["data[IdIndicador]"]);
            ViewBag.TerminosBusquedaOPERADOR = new Operador();
            ViewBag.IdOperadorSeleccionado = string.Empty;
            Respuesta<List<Operador>> listadoOperadores = operadorBL.ConsultarXServicio(idServicio);
            Respuesta<List<pa_getListaIndicadoresSolicitud_Result>> listaIndicadores = this.solicitudBL.gConsultarIndicadores(idDireccion, idFrecuencia);
            SolicitudViewModels modeloSolicitud = new SolicitudViewModels();

            modeloSolicitud.itemSolicitudIndicador = new SolicitudIndicador();
            modeloSolicitud.itemSolicitudIndicador.IdSolicitudIndicador = idIndicador;

            modeloSolicitud.listadoIndicadores = listaIndicadores.objObjeto;

            return PartialView(new Tuple<List<Operador>, GB.SUTEL.UI.Models.SolicitudViewModels>(listadoOperadores.objObjeto, modeloSolicitud));
        }



        /// <summary>
        /// lista los indicadores(constructores) y los operadores en agregar
        /// </summary>
        /// <param name="poOperador"></param>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        [HttpPost]
        public ActionResult Indicadores(Operador poOperador)
        {
            Respuesta<List<Operador>> listadoOperadores = new Respuesta<List<Operador>>();
            SolicitudViewModels modeloSolicitud = new SolicitudViewModels();
            try
            {
                int idServicio = int.Parse(Request.Params["data[IdServicio]"]);
                int idFrecuencia = int.Parse(Request.Params["data[IdFrecuencia]"]);
                int idDireccion = int.Parse(Request.Params["data[IdDireccion]"]);
                Guid idIndicador = Guid.Parse(Request.Params["data[IdIndicador]"]);
                string nombreOperador = (Request.Params["data[NombreOperador]"] == null ? "" : Request.Params["data[NombreOperador]"].ToString());

                ViewBag.TerminosBusquedaOPERADOR = poOperador;
                ViewBag.IdOperadorSeleccionado = string.Empty;
                listadoOperadores = operadorBL.ConsultarXServicio(idServicio);
                listadoOperadores.objObjeto = listadoOperadores.objObjeto.Where(x => (nombreOperador.Equals("") || x.NombreOperador.ToUpper().Contains(nombreOperador.ToUpper()))).ToList();
                Respuesta<List<pa_getListaIndicadoresSolicitud_Result>> listaIndicadores = this.solicitudBL.gConsultarIndicadoresPorDirFrecServ(idDireccion, idFrecuencia, idServicio);



                modeloSolicitud.itemSolicitudIndicador = new SolicitudIndicador();
                modeloSolicitud.itemSolicitudIndicador.IdSolicitudIndicador = idIndicador;

                modeloSolicitud.listadoIndicadores = listaIndicadores.objObjeto;

                return PartialView(new Tuple<List<Operador>, GB.SUTEL.UI.Models.SolicitudViewModels>(listadoOperadores.objObjeto, modeloSolicitud));
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView(new Tuple<List<Operador>, GB.SUTEL.UI.Models.SolicitudViewModels>(listadoOperadores.objObjeto, modeloSolicitud));
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView(new Tuple<List<Operador>, GB.SUTEL.UI.Models.SolicitudViewModels>(listadoOperadores.objObjeto, modeloSolicitud));
            }
        }

        /// <summary>
        /// lista los indicadores(constructores) y los operadores en agregar editar
        /// </summary>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        public ActionResult IndicadoresEditar()
        {
            Respuesta<List<Operador>> listadoOperadores = new Respuesta<List<Operador>>();
            SolicitudViewModels modeloSolicitud = new SolicitudViewModels();
            try
            {
                int idServicio = int.Parse(Request.Params["data[IdServicio]"]);
                int idFrecuencia = int.Parse(Request.Params["data[IdFrecuencia]"]);
                int idDireccion = int.Parse(Request.Params["data[IdDireccion]"]);
                Guid idIndicador = Guid.Parse(Request.Params["data[IdIndicador]"]);
                ViewBag.TerminosBusquedaOPERADOR = new Operador();
                ViewBag.IdOperadorSeleccionado = string.Empty;
                listadoOperadores = operadorBL.ConsultarXServicio(idServicio);
                Respuesta<List<pa_getListaIndicadoresSolicitud_Result>> listaIndicadores = this.solicitudBL.gConsultarIndicadores(idDireccion, idFrecuencia);


                modeloSolicitud.itemSolicitudIndicador = new SolicitudIndicador();
                modeloSolicitud.itemSolicitudIndicador.IdSolicitudIndicador = idIndicador;

                modeloSolicitud.listadoIndicadores = listaIndicadores.objObjeto;
                ViewBag.adventencia = Session["adventencia"];

                return PartialView(new Tuple<List<Operador>, GB.SUTEL.UI.Models.SolicitudViewModels>(listadoOperadores.objObjeto, modeloSolicitud));
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView(new Tuple<List<Operador>, GB.SUTEL.UI.Models.SolicitudViewModels>(listadoOperadores.objObjeto, modeloSolicitud));
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView(new Tuple<List<Operador>, GB.SUTEL.UI.Models.SolicitudViewModels>(listadoOperadores.objObjeto, modeloSolicitud));
            }
        }

        /// <summary>
        /// lista los indicadores(constructores) y los operadores en editar
        /// </summary>
        /// <param name="poOperador"></param>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        [HttpPost]
        public ActionResult IndicadoresEditar(Operador poOperador)
        {
            int idServicio = int.Parse(Request.Params["data[IdServicio]"]);
            int idFrecuencia = int.Parse(Request.Params["data[IdFrecuencia]"]);
            int idDireccion = int.Parse(Request.Params["data[IdDireccion]"]);
            Guid idIndicador = Guid.Parse(Request.Params["data[IdIndicador]"]);
            string nombreOperador = (Request.Params["data[NombreOperador]"] == null ? "" : Request.Params["data[NombreOperador]"].ToString());
            ViewBag.TerminosBusquedaOPERADOR = poOperador;
            ViewBag.IdOperadorSeleccionado = string.Empty;
            Respuesta<List<Operador>> listadoOperadores = operadorBL.ConsultarXServicio(idServicio);
            listadoOperadores.objObjeto = listadoOperadores.objObjeto.Where(x => (nombreOperador.Equals("") || x.NombreOperador.ToUpper().Contains(nombreOperador.ToUpper()))).ToList();
            Respuesta<List<pa_getListaIndicadoresSolicitud_Result>> listaIndicadores = this.solicitudBL.gConsultarIndicadoresPorDirFrecServ(idDireccion, idFrecuencia, idServicio);
            SolicitudViewModels modeloSolicitud = new SolicitudViewModels();

            modeloSolicitud.itemSolicitudIndicador = new SolicitudIndicador();
            modeloSolicitud.itemSolicitudIndicador.IdSolicitudIndicador = idIndicador;

            modeloSolicitud.listadoIndicadores = listaIndicadores.objObjeto;
            ViewBag.adventencia = Session["adventencia"];

            return PartialView(new Tuple<List<Operador>, GB.SUTEL.UI.Models.SolicitudViewModels>(listadoOperadores.objObjeto, modeloSolicitud));
        }

        [HttpPost]
        [AuthorizeUserAttribute]
        public string IndicadoresXOperadorXRegistro()
        {

            string idOperador = Request.Params["data[IdOperador]"].ToString();
            string idIndicador = Request.Params["data[IdIndicador]"];
            JSONResult<List<pa_getListaIndicadoresXSolicitud_Result>> jsonResult = new JSONResult<List<pa_getListaIndicadoresXSolicitud_Result>>();
            ViewBag.IdOperadorSeleccionado = string.Empty;

            Respuesta<List<pa_getListaIndicadoresXSolicitud_Result>> listaIndicadoresCargar = this.solicitudBL.gFiltrarIndicadoresXSolicitud(idOperador, idIndicador);


            jsonResult.data = listaIndicadoresCargar.objObjeto;

            return jsonResult.toJSON();

        }
        [HttpPost]
        public string actualizarEstadoDescarga(string idSolicitud, string idOperador)
        {
            var resultado = refRegistroIndicadorBL.actualizarEstadoDescargaExcels(idSolicitud, idOperador);
            return resultado;
        }
        [HttpPost]
        public string ActualizarFechas(string ItemActualizar)
        {
            Respuesta<bool> respuesta = new Respuesta<bool>();
            JSONResult<SolicitudIndicador> jsonRespuesta = new JSONResult<SolicitudIndicador>();
            jsonRespuesta.data = new SolicitudIndicador();
            try
            {

                string descFormulario = Request.Params["data[descFormulario]"];
                string fechaInicio = Request.Params["itemSolicitudIndicador.FechaInicio"];
                string fechaFinal = Request.Params["itemSolicitudIndicador.FechaFin"];
                respuesta = refRegistroIndicadorBL.actualizarSolicitud(ItemActualizar, fechaInicio, fechaFinal);
                if (respuesta.blnIndicadorTransaccion)
                {
                    jsonRespuesta.ok = true;
                    jsonRespuesta.strMensaje = respuesta.strMensaje;

                }

            }
            catch (Exception Ex)
            {


            }
            return jsonRespuesta.toJSON();
        }

        [HttpGet]
        /* public bool obtenerEstadoDescarga(string idSolicitud, string idOperador)
            {
                var resultado = refRegistroIndicadorBL.obtieneEstadoDescargaExcel(idSolicitud, idOperador);
                return resultado;
            }
        */
        [HttpPost]
        public string actualizarEstadoCarga(string idSolicitud, string idOperador)
        {
            var resultado = refRegistroIndicadorBL.actualizarEstadoCargaExcel(idSolicitud, idOperador);
            return resultado;
        }

        [HttpGet]
        /*  public bool obtenerEstadoCarga(string idSolicitud, string idOperador)
            {
                var resultado = refRegistroIndicadorBL.obtieneEstadoCargaExcel(idSolicitud, idOperador);
                return resultado;

            }*/

        /// <summary>
        /// 
        /// </summary>
        /// autor: Diego
        /// <returns></returns>

        public Respuesta<List<string>> IndicadoresXSolicitud(Guid indicador)
        {

            //string idOperador = Request.Params["data[IdOperador]"].ToString();
            // string idIndicador = Request.Params["data[IdIndicador]"];
            // JSONResult<List<string>> jsonResult = new JSONResult<List<string>>();

            Respuesta<List<string>> listaIndicadoresCargar = this.solicitudBL.gFiltrarSoloIndicadoresXSolicitud(indicador);

            // jsonResult.data = listaIndicadoresCargar.objObjeto;

            return listaIndicadoresCargar;

        }



        /// <summary>
        /// 
        /// </summary>
        /// autor: Diego
        /// <returns></returns>

        public Respuesta<List<string>> OperadoresXSolicitud(Guid indicador)
        {

            //string idOperador = Request.Params["data[IdOperador]"].ToString();
            // string idIndicador = Request.Params["data[IdIndicador]"];
            // JSONResult<List<string>> jsonResult = new JSONResult<List<string>>();
            //ViewBag.IdOperadorSeleccionado = string.Empty;

            Respuesta<List<string>> listaOperadoresCargar = this.solicitudBL.gFiltrarOperadoresXSolicitud(indicador);


            return listaOperadoresCargar;

        }

        /// <summary>
        /// Convierte la lista con el formato necesario 
        /// </summary>
        /// <param name="indicadores"></param>
        /// id|2,
        /// <returns></returns>
        public List<string> convertListaIndicadores(Respuesta<List<string>> indicadores)
        {

            string[] valor1;
            string[] valor2;
            string[] valor3;
            string Indicador = string.Empty;
            string Orden = string.Empty;


            List<string> list = indicadores.objObjeto;


            for (int i = 0; i < list.Count; i++)
            {
                string aux = list[i].Replace('{', ' ');
                string aux2 = aux.Replace('}', ' ');
                string aux3 = aux2.Trim();

                valor1 = aux3.Split(',');

                Indicador = valor1[0];
                Orden = valor1[1];
                //-----------------------------------//
                valor2 = Indicador.Split('=');
                valor3 = Orden.Split('=');

                Indicador = valor2[1].Trim();
                Orden = valor3[1].Trim();

                if (Orden == "")
                {
                    Orden = "0";
                }

                list[i] = Indicador + "|" + Orden;

            }

            return list;
        }


        #endregion

        [HttpGet]
        [AuthorizeUserAttribute]
        public ActionResult _table()
        {
            try
            {
                ViewBag.TerminosBusquedaDESCFORMULARIO = string.Empty;
                ViewBag.TerminosBusquedaFECHAINICIO = DateTime.Today.AddMonths(-Properties.Settings.Default.MostrarMesesSolicitudOmision).ToString("dd/MM/yyyy");
                ViewBag.TerminosBusquedaFECHAFIN = string.Empty;
                ViewBag.TerminosBusquedaACTIVO = string.Empty;

                GB.SUTEL.UI.Models.SolicitudViewModels modelo = new SolicitudViewModels();

                Respuesta<List<SolicitudIndicador>> respuesta = solicitudBL.gListar();


                if (respuesta.blnIndicadorTransaccion)
                {
                    modelo.listadoSolicitudes = respuesta.objObjeto;

                    return PartialView(modelo);

                }
                return PartialView();
            }
            catch (CustomException)
            {
                return PartialView();
            }
            catch (Exception)
            {
                return PartialView();
            }
        }
        [HttpPost]
        public ActionResult actualizarSemaforos(Guid IdSolicitudConstructor, string IdOperador, int idSemaforoActualizar, List<ListaIDDto> Valor, string Observacion)
        {
            Usuario usuario = null;

            if (((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault() != null)
            {

                var user = ((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").FirstOrDefault();
                var correo = user.Value;

                var nombreUsuario = ((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault().Value;
                BL.Seguridad.UsersBL refUsuario = new BL.Seguridad.UsersBL(AppContext);

                usuario = refUsuario.ConsultarPorExpresion(x => x.AccesoUsuario == nombreUsuario && x.Borrado == 0 && x.Activo == 1).objObjeto;
            }

            ConfirmaDescargaWebDto respuesta = new ConfirmaDescargaWebDto();

            solicitudBL.actualizarSemaforos(IdSolicitudConstructor, IdOperador, idSemaforoActualizar, Valor, Observacion, usuario);
            var registros = solicitudBL.gConsultarDetalleRegistroIndicador(IdSolicitudConstructor).objObjeto;

            respuesta.listaDetalleRegistroIndicador = registros.listaDetalleRegistroIndicador;


            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        private void ObtenerMensajeNotificarOperador(ref string poHtml, ref string poAsunto, Guid IDSolicitudIndicadorImportar)
        {
            SolicitudIndicador objSolicitudIndicador = new SolicitudIndicador();
            objSolicitudIndicador = solicitudBL.gConsultarPorIdentificador(IDSolicitudIndicadorImportar).objObjeto;

            string host = Request.Url.Host;
            String tipoSitio = "https";

            MailingModel message = new MailingModel()
            {
                ImagenSutel = tipoSitio + "://" + host + "/Content/Images/logos/logo-Sutel_11_3.png",
                RutaSistema = tipoSitio + "://" + host + "/"
            };

            string renderedEmail = RenderView.RenderViewToString("Emails", "notificacionCargarArchivo", new Tuple<SolicitudIndicador, MailingModel>(objSolicitudIndicador, message));

            //poHtml = GetHTMLParsedAsString(Server.MapPath(ConfigurationManager.AppSettings["URLPlantillaNotificacion"].ToString()));
            poHtml = renderedEmail;
            poAsunto = ConfigurationManager.AppSettings["AsuntoNotificacion"].ToString();
        }


        [HttpGet]
        public ActionResult listaConstructorPorIndicador(Guid IdSolicitud, string IdOperadorSeleccionado, string ventana)
        {
            //IdOperadorSeleccionado = "O0002";
            //IdSolicitud = new Guid("0a10642c-595a-4bc7-bc36-700617a5635e");

            ConfirmaDescargaWebDto resultado = new ConfirmaDescargaWebDto();
            var operador = solicitudBL.gConsultarOperadorFormularioWebPorIndicador(IdSolicitud).objObjeto;
            var listaIdSolicitudContructor = operador.Select(x => x.IdSolicitudContructor).Distinct().ToList();
            var ListaDetalleRegistroIndicador = solicitudBL.gConsultarListaDetalleRegistroIndicador(listaIdSolicitudContructor).objObjeto;
            resultado.listaOperador = new List<string>();

            resultado.listaIdOperador = operador.Select(x => x.listaOperador.IdOperador).Distinct().ToList();
            string idOperador;


            if (ventana == "RegistroIndicador")
            {
                resultado.listaIdOperador = new List<string>();
                var user = ((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").FirstOrDefault();
                var correo = user.Value;

                var nombreUsuario = ((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault().Value;
                BL.Seguridad.UsersBL refUsuario = new BL.Seguridad.UsersBL(AppContext);
                var usuario = refUsuario.ConsultarPorExpresion(x => x.AccesoUsuario == nombreUsuario && x.Borrado == 0 && x.Activo == 1).objObjeto;

                idOperador = usuario.IdOperador;

                resultado.listaOperador.Add(operador.Where(x => x.IdOperador == idOperador).Select(x => x.listaOperador.NombreOperador).Distinct().FirstOrDefault());
                resultado.listaIdOperador.Add(operador.Where(y => y.IdOperador == idOperador).Select(y => y.listaOperador.IdOperador).Distinct().FirstOrDefault());

            }
            else
            {

                foreach (var IdOperador in resultado.listaIdOperador)
                {
                    resultado.listaOperador.Add(operador.Where(x => x.IdOperador == IdOperador).Select(x => x.listaOperador.NombreOperador).Distinct().FirstOrDefault());
                }

                if (IdOperadorSeleccionado == null)
                {
                    idOperador = resultado.listaIdOperador[0];
                }
                else
                {
                    idOperador = IdOperadorSeleccionado;
                }
            }

            var source = solicitudBL.gConsultarConstructorPorIndicador(IdSolicitud, idOperador).objObjeto;


            if (ListaDetalleRegistroIndicador.listaDetalleRegistroIndicador != null)
            {
                var listaGuardados = (ListaDetalleRegistroIndicador.listaDetalleRegistroIndicador).Select(x => x.IdConstructorCriterio).ToList();
                var listaAgrepaciones = source.detalleAgrupacionesPorOperador.Where(x => listaGuardados.Contains(x.Id_ConstructorCriterio)).ToList();

                if (listaAgrepaciones != null)
                    foreach (var item in listaAgrepaciones)
                    {
                        //var Semaforohijo = "";
                        if (source.detalleAgrupacionesPorOperador != null)
                            foreach (var item2 in source.detalleAgrupacionesPorOperador)
                            {
                                if (item2.Id_ConstructorCriterio == item.Id_Padre_ConstructorCriterio)
                                {
                                    item2.Tiene_Hijos = true;
                                    var Semaforohijo = ListaDetalleRegistroIndicador.listaDetalleRegistroIndicador.Where(x => x.IdConstructorCriterio == item.Id_ConstructorCriterio).Select(x => x.IdSemaforo).FirstOrDefault();
                                    item2.IdSemaforohijo = Semaforohijo.ToString();
                                }if(item2.Tabla_Tipo_Nivel_Detalle != "")
                                {
                                    var Semaforohijo = ListaDetalleRegistroIndicador.listaDetalleRegistroIndicador.Where(x => x.IdConstructorCriterio == item2.Id_ConstructorCriterio).Select(x => x.IdSemaforo).FirstOrDefault();
                                    item2.IdSemaforohijo = Semaforohijo.ToString();

                                }
                                
                            }
                    }
            }
            resultado.direccion = source.solicitudIndicador.Direccion.Nombre;
            resultado.IdDireccion = source.solicitudIndicador.Direccion.IdDireccion;
            resultado.servicio = source.solicitudIndicador.Servicio.DesServicio;
            resultado.frecuencia = source.solicitudIndicador.Frecuencia.NombreFrecuencia;
            resultado.desglose = source.detalleAgrupacionesPorOperador[0].Nombre_Desglose;
            resultado.listaDetalleRegistroIndicador = ListaDetalleRegistroIndicador.listaDetalleRegistroIndicador;

            resultado.provincias = new List<Provincia>();
            resultado.cantones = new List<Canton>();
            resultado.distritos = new List<Distrito>();

            foreach (var provincias in source.provincias)
            {
                if (provincias.Nombre != "No Especificado")
                {
                    resultado.provincias.Add(new Provincia
                    {
                        Nombre = provincias.Nombre,
                        IdProvincia = provincias.IdProvincia

                    });
                }



            }
            foreach (var cantones in source.cantones)
            {
                if (cantones.Nombre != "No Especificado")
                {
                    if (cantones.Nombre != "Todos")
                    {
                        if (cantones.Nombre != "No Aplica")
                        {
                            resultado.cantones.Add(new Canton
                            {
                                IdCanton = cantones.IdCanton,
                                Nombre = cantones.Nombre,
                                IdProvincia = cantones.IdProvincia

                            });
                        }
                    }
                       
                }
                

            }
            foreach (var distritos in source.distritos)
            {
                resultado.distritos.Add(new Distrito
                {
                    IdDistrito = distritos.IdDistrito,
                    Nombre = distritos.Nombre,
                    IdCanton = distritos.IdCanton

                });
            }

            resultado.listaSolicitudConstructor = source.detalleAgrupacionesPorOperador.Where(x => x.IdOperador == idOperador).Select(x => x.Nombre_Indicador).Distinct().ToList();

            resultado.listaAyudaConstructor = new List<string>();
            resultado.listaIdSemaforo = new List<int?>();
            resultado.listaIdsSolicitudConstructor = new List<Guid>();

            foreach (var x in source.detalleAgrupacionesPorOperador) { if (x.IdSemaforo == null || x.IdSemaforo == "") x.IdSemaforo = "1"; }
            //foreach (var x in source.detalleAgrupacionesPorOperador) { if (x.NivelMaximo <= x.IdNivel && x.UltimoNivel==0) x.NivelMaximo = x.IdNivel; }

            foreach (var solicitudIndicador in resultado.listaSolicitudConstructor)
            {
                resultado.listaAyudaConstructor.Add(source.detalleAgrupacionesPorOperador.Where(x => x.Nombre_Indicador == solicitudIndicador && x.IdOperador == idOperador).Select(x => x.ayuda).Distinct().FirstOrDefault());
                resultado.listaIdSemaforo.Add(source.detalleAgrupacionesPorOperador.Where(x => x.Nombre_Indicador == solicitudIndicador && x.IdOperador == idOperador).Select(x => int.Parse(x.IdSemaforo)).Distinct().FirstOrDefault());

                resultado.listaIdsSolicitudConstructor.Add(source.detalleAgrupacionesPorOperador.Where(x => x.Nombre_Indicador == solicitudIndicador && x.IdOperador == idOperador).Select(x => Guid.Parse(x.Id_Solicitud_Constructor)).Distinct().FirstOrDefault());

            }
            var ListaIdIndicador = source.detalleAgrupacionesPorOperador.Select(x => x.ID_Indicador).Distinct().ToList();

            int nivelMin = 0;
            int nivelMax = 0;

            List<DetalleAgrupacionesPorOperadorDto> listaGeneral = new List<DetalleAgrupacionesPorOperadorDto>();

            foreach (var item in source.detalleAgrupacionesPorOperador)
            {
                //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                var NivelDetalle = "";
                var maximonivel = source.detalleAgrupacionesPorOperador.Where(x => x.ID_Indicador == item.ID_Indicador).ToList();
                if (maximonivel.Count() != 0)
                {
                    item.NivelMaximo = maximonivel.Where(x => x.ID_Indicador == item.ID_Indicador).Max(x => x.IdNivel);
                    item.NivelMinimo = maximonivel.Where(x => x.ID_Indicador == item.ID_Indicador).Min(x => x.IdNivel);
                    NivelDetalle = maximonivel.Select(x => x.Tabla_Tipo_Nivel_Detalle).FirstOrDefault();
                }
                //+++++++++++++++++++++++++++++++++++++++
                if (item.NivelMaximo == 1 && NivelDetalle == "")
                {
                    var ListaIndicador = source.detalleAgrupacionesPorOperador.Where(x => x.ID_Indicador == item.ID_Indicador && x.UltimoNivel == 1).ToList();
                    item.NivelMaximo = maximonivel.Where(x => x.ID_Indicador == item.ID_Indicador).Max(x => x.IdNivel);
                    item.NivelMinimo = maximonivel.Where(x => x.ID_Indicador == item.ID_Indicador).Min(x => x.IdNivel);
                    item.NivelDetalle = true;
                }
                else
                {
                    var ListaIndicador = source.detalleAgrupacionesPorOperador.Where(x => x.ID_Indicador == item.ID_Indicador && x.UltimoNivel == 0).ToList();
                    //resultado.NivelDetalle = false;
                    if (ListaIndicador.Count() != 0)
                    {
                        item.NivelMinimo = ListaIndicador.Where(x => x.ID_Indicador == item.ID_Indicador).Min(x => x.IdNivel);
                        item.NivelMaximo = ListaIndicador.Where(x => x.ID_Indicador == item.ID_Indicador).Max(x => x.IdNivel);
                        item.NivelDetalle = false;
                    }
                }
            }

            resultado.detalleAgrupacionesPorOperador = source.detalleAgrupacionesPorOperador;
            if (ListaDetalleRegistroIndicador.listaDetalleRegistroIndicador != null)
            {
                var listaDetalleRegistroIndicadorPorProvincias = ListaDetalleRegistroIndicador.listaDetalleRegistroIndicador.Where(x => x.IdProvincia != null).ToList();

                if (listaDetalleRegistroIndicadorPorProvincias.Count() > 0)
                {
                    var ZonasTienenDatos = new Zona();
                    foreach (var agrupacion in resultado.detalleAgrupacionesPorOperador)
                    {
                        //  agrupacion.listaIdCantonesTieneDatos = new List<int?>();
                        //agrupacion.listaIdDistritosTieneDatos = new List<int?>();
                        agrupacion.listaZonasTienenDatos = new List<Zona>();
                        foreach (var detalle in listaDetalleRegistroIndicadorPorProvincias)
                        {
                            if (agrupacion.Tabla_Tipo_Nivel_Detalle == "PROVINCIA")
                            {
                                if (detalle.IdProvincia > 0 && (detalle.IdCanton == null && detalle.IdDistrito == null))
                                {
                                    if (agrupacion.listaZonasTienenDatos.Where(x => x.idProvincia != detalle.IdProvincia && x.idCanton != detalle.IdCanton && x.idDistrito != detalle.IdDistrito).Count() == 0)
                                    {
                                        agrupacion.listaZonasTienenDatos.Add(new Zona
                                        {
                                            idProvincia = null,
                                            idCanton = null,
                                            idDistrito = null,
                                            idConstructorCriterio = detalle.IdConstructorCriterio

                                        });
                                    }
                                }

                            }
                            else if (agrupacion.Tabla_Tipo_Nivel_Detalle == "CANTON")
                            {
                                if (detalle.IdProvincia > 0 && detalle.IdCanton > 0 && detalle.IdDistrito == null)
                                {

                                    //     agrupacion.tieneDatos = true;


                                    //Se agrega el anterior debido a que lo que se van a pintar son los botones y por ende los botones en este caso son las provincias  
                                    var b = agrupacion.listaZonasTienenDatos.Where(x => x.idProvincia == detalle.IdProvincia).ToList();

                                    //if (agrupacion.listaZonasTienenDatos.Where(x => x.idProvincia == detalle.IdProvincia).ToList().Count() == 0 || agrupacion.listaZonasTienenDatos.Count() == 0)
                                    //{

                                        agrupacion.listaZonasTienenDatos.Add(new Zona
                                        {
                                            idProvincia = detalle.IdProvincia,
                                            idCanton = null,
                                            idDistrito = null,
                                            idConstructorCriterio = detalle.IdConstructorCriterio

                                        });

                                        //break;
                                    //}

                                }

                            }
                            else if (agrupacion.Tabla_Tipo_Nivel_Detalle == "DISTRITO")
                            {
                                if (detalle.IdProvincia > 0 && detalle.IdCanton > 0 && detalle.IdDistrito > 0)
                                {
                                    //   agrupacion.tieneDatos = true;
                                    //Se agrega el anterior debido a que lo que se van a pintar son los botones y por ende los botones en este caso son los cantones  
                                    if (agrupacion.listaZonasTienenDatos.Where(x => x.idProvincia == detalle.IdProvincia && x.idCanton == detalle.IdCanton).Count() == 0 || agrupacion.listaZonasTienenDatos.Count() == 0)
                                        //agrupacion.listaIdDistritosTieneDatos.Add(detalle.IdCanton);

                                        agrupacion.listaZonasTienenDatos.Add(new Zona
                                        {
                                            idProvincia = detalle.IdProvincia,
                                            idCanton = detalle.IdCanton,
                                            idDistrito = null,
                                            idConstructorCriterio = detalle.IdConstructorCriterio

                                        });

                                }

                            }
                        }
                    }
                }

            }

            resultado.nivelMax = nivelMax;
            resultado.nivelMin = nivelMin;


            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult seleccionarConstructor(Guid IdConstructorCriterio, Guid IdSolicitud, Guid IdSolicitudConstructor, string IdOperador, string Zona)
        {
            //IdOperador = "O0002";
            //IdSolicitud = new Guid("0a10642c-595a-4bc7-bc36-700617a5635e");

            ConfirmaDescargaWebDto resultado = new ConfirmaDescargaWebDto();
            var source = solicitudBL.gConsultarConstructorPorIndicador(IdSolicitud, IdOperador).objObjeto;
            var registros = solicitudBL.gConsultarDetalleRegistroIndicador(IdSolicitudConstructor).objObjeto;
            ////////////////+++++++++++++++++++++++++++++++++++////////////////////////
             var ListaIdIndicador = source.detalleAgrupacionesPorOperador.Where(x => x.Id_ConstructorCriterio == IdConstructorCriterio).ToList();
             var lista= ListaIdIndicador.Select(x => x.ID_Indicador).Distinct().ToList();
            var nivelMax = 0;
            var NivelDetalle = "";
            foreach (var item in lista)
            {

                var maximonivel = source.detalleAgrupacionesPorOperador.Where(x => x.UltimoNivel == 1).ToList();
                if (maximonivel.Count() != 0)
                {
                    nivelMax = maximonivel.Where(x => x.ID_Indicador == item).Max(x => x.IdNivel);
                    NivelDetalle = maximonivel.Where(x => x.ID_Indicador == item).Select(x => x.Tabla_Tipo_Nivel_Detalle).FirstOrDefault();
                }

            }
            ///++++++++++++++++++++++++++++++++++++++++++++++++++++++

            if (Zona == null)
            {
                if (nivelMax == 1 && NivelDetalle == "")
                {
                    resultado.detalleAgrupacionesPorOperador = source.detalleAgrupacionesPorOperador.Where(x => x.Id_ConstructorCriterio == IdConstructorCriterio).ToList();
                    resultado.listaDetalleRegistroIndicador = registros.listaDetalleRegistroIndicador;
                    resultado.observacion = registros.observacion;
                }
                else
                {
                    resultado.detalleAgrupacionesPorOperador = source.detalleAgrupacionesPorOperador.Where(x => x.Id_Padre_ConstructorCriterio == IdConstructorCriterio).ToList();
                    resultado.listaDetalleRegistroIndicador = registros.listaDetalleRegistroIndicador;
                    resultado.observacion = registros.observacion;
                }

            }
            else
            {

                resultado.detalleAgrupacionesPorOperador = source.detalleAgrupacionesPorOperador.Where(x => x.Id_ConstructorCriterio == IdConstructorCriterio).ToList();
                //  resultado.detalleAgrupacionesPorOperador = source.detalleAgrupacionesPorOperador.Where(x => x.Id_Padre_ConstructorCriterio == IdConstructorCriterio).ToList();

                resultado.listaDetalleRegistroIndicador = registros.listaDetalleRegistroIndicador;
                resultado.observacion = registros.observacion;
                //Este array va acontener el nombre de ;la zona y el id del predecesor ej: CANTON-IdProvincia
                var arrayZona = Zona.Split('-');

                if (arrayZona[0] == "PROVINCIA")
                {
                    resultado.provincias = new List<Provincia>();
                    foreach (var provincias in source.provincias)
                    {
                        if (provincias.Nombre != "No Especificado")
                        {
                            resultado.provincias.Add(new Provincia
                            {
                                Nombre = provincias.Nombre,
                                IdProvincia = provincias.IdProvincia

                            });
                        }
                       

                    }
                }
                else if (arrayZona[0] == "CANTON")
                {
                    resultado.cantones = new List<Canton>();
                    var cantonePorProvinciaSeleccionada = source.cantones.Where(x => x.IdProvincia == int.Parse(arrayZona[1])).ToList();

                    foreach (var cantones in cantonePorProvinciaSeleccionada)
                    {
                        //if(cantones.Nombre != "No Especificado")
                        //{
                            if(cantones.Nombre != "Todos")
                            {
                                if (cantones.Nombre != "No Aplica")
                                {
                                    resultado.cantones.Add(new Canton
                                    {
                                        IdCanton = cantones.IdCanton,
                                        Nombre = cantones.Nombre,
                                        IdProvincia = cantones.IdProvincia

                                    });
                                }

                            }
                                                          
                        //}
                        

                    }

                }
                else if (arrayZona[0] == "DISTRITO")
                {
                    resultado.distritos = new List<Distrito>();
                    var cantonePorCantonSeleccionado = source.distritos.Where(x => x.IdCanton == int.Parse(arrayZona[1]));

                    foreach (var distritos in cantonePorCantonSeleccionado)
                    {

                        resultado.distritos.Add(new Distrito
                        {
                            IdDistrito = distritos.IdDistrito,
                            Nombre = distritos.Nombre,
                            IdCanton = distritos.IdCanton

                        });
                    }

                }
            }



            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult cargarDatos(Guid IdSolicitud, string IdOperador, int poDireccion)
        {
            Respuesta<SolicitudConstructor> respuesta = new Respuesta<SolicitudConstructor>();

            respuesta = solicitudBL.cargarDatos(IdSolicitud, IdOperador, poDireccion);

            Usuario usuario = null;
            string nombreUsuario = "";
            string correo = "";
            string path = string.Empty;
            string mensaje = string.Empty;
            string asunto = string.Empty;
            int direccion = 0;

            if (((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault() != null)
            {

                var user = ((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").FirstOrDefault();
                correo = user.Value;

                nombreUsuario = ((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault().Value;
                BL.Seguridad.UsersBL refUsuario = new BL.Seguridad.UsersBL(AppContext);

                usuario = refUsuario.ConsultarPorExpresion(x => x.AccesoUsuario == nombreUsuario && x.Borrado == 0 && x.Activo == 1).objObjeto;
            }

            //respuesta = refRegistroIndicadorInternoBL.gRegistroIndicadorInterno(IDSolicitudIndicadorImportar, usuario.IdOperador, usuario.IdUsuario, fileName, file.InputStream, path, ref direccion);

            //if (respuesta.blnIndicadorTransaccion)
            //{

            //Inserción Bitácora
            //gBitacoraRegistroIndicador(ActionsBinnacle.Crear, respuesta.objObjeto, null);

            ObtenerMensajeNotificarOperador(ref mensaje, ref asunto, IdSolicitud /*IDSolicitudIndicadorImportar*/);


            var result = (solicitudBL.gConfirmacion(correo, mensaje, asunto, direccion));

            //}

            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult listarOperadoresXSolicitud(Guid IdSolicitud)
        {
            List<Combos> resultado = null;

            resultado = operadorBL.ConsultarXSolicitud(IdSolicitud).objObjeto.Select(m =>
                        new Combos { id = m.IdOperador, valor = string.Format("{0} {1} - {2}", m.IdOperador, m.NombreOperador, m.ArchivoExcel.FirstOrDefault().FormularioWeb ? "Formulario Web" : "Formulario Excel") }).ToList();


            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult FormularioWebXSolicitud(Guid IdSolicitud)
        {
            byte resultado = 0;

            resultado = Convert.ToByte(operadorBL.ConsultarFormularioWebXSolicitud(IdSolicitud).objObjeto);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AuthorizeUserAttribute]
        public ActionResult _table(SolicitudViewModels Solicitud)
        {
            try
            {
                ViewBag.TerminosBusquedaDESCFORMULARIO = Solicitud.FiltroFormulario;
                ViewBag.TerminosBusquedaFECHAINICIO = Solicitud.FiltroInicioPeriodo;
                ViewBag.TerminosBusquedaFECHAFIN = Solicitud.FiltroFinPeriodo;
                //ViewBag.TerminosBusquedaACTIVO = string.Empty;
                Respuesta<List<SolicitudIndicador>> respuesta = null;

                DateTime? fechaInicioAux = null;
                DateTime? fechaFinAux = null;
                GB.SUTEL.UI.Models.SolicitudViewModels modelo = Solicitud;

                if (string.IsNullOrEmpty(Solicitud.FiltroFormulario) && string.IsNullOrEmpty(Solicitud.FiltroInicioPeriodo) && string.IsNullOrEmpty(Solicitud.FiltroFinPeriodo) && (Solicitud.ServicioSeleccionados == null || Solicitud.ServicioSeleccionados.Count <= 0))//&& (string.IsNullOrEmpty(Activo) || Activo.Equals("0"))
                {
                    respuesta = solicitudBL.gListar();
                }
                else
                {
                    if (!string.IsNullOrEmpty(Solicitud.FiltroFinPeriodo))
                    {
                        try
                        {
                            fechaFinAux = DateTime.ParseExact(Solicitud.FiltroFinPeriodo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        catch (Exception)
                        {
                            try { fechaFinAux = DateTime.Parse(Solicitud.FiltroFinPeriodo); }
                            catch (Exception) { fechaFinAux = null; }
                        }
                    }

                    if (!string.IsNullOrEmpty(Solicitud.FiltroInicioPeriodo))
                    {
                        try
                        {
                            fechaInicioAux = DateTime.ParseExact(Solicitud.FiltroInicioPeriodo, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        }
                        catch (Exception)
                        {
                            try { fechaInicioAux = DateTime.Parse(Solicitud.FiltroInicioPeriodo); }
                            catch (Exception)
                            {
                                fechaInicioAux = null;
                            }
                        }
                    }

                    //bool? activoAux = null;

                    //if (Activo != "0")
                    //    activoAux = bool.Parse(Activo);

                    respuesta = solicitudBL.gConsultarPorFiltros(Solicitud.FiltroFormulario,
                        fechaInicioAux, fechaFinAux, Solicitud.ServicioSeleccionados);
                }

                if (modelo.listadoServicios == null || modelo.listadoServicios.Count == 0) modelo.listadoServicios = servicioBL.ConsultarTodos().objObjeto;
                if (modelo.ServicioSeleccionados == null) modelo.ServicioSeleccionados = new List<int>();
                if (respuesta.blnIndicadorTransaccion)
                {
                    modelo.listadoSolicitudes = respuesta.objObjeto;
                    return PartialView(modelo);

                }
                return PartialView();
            }
            catch (CustomException)
            {
                return PartialView();
            }
            catch (Exception ex)
            {
                return PartialView();
            }

        }

        [HttpPost]
        [AuthorizeUserAttribute]
        public string GuardarIndicadores()
        {

            string IdSolicitud = Request.Params["data[IdSolicitudIndicador]"];
            string Indicadores = Request.Params["data[Indicadores][]"];
            string Operadores = Request.Params["data[Operadores][]"];

            JSONResult<List<string>> jsonRespuesta = new JSONResult<List<string>>();


            Respuesta<SolicitudConstructor> respAgregar;
            List<string> indicadoresOperadoresNoRelacionados = new List<string>();
            bool algunIndicadorNoAsociado = false;
            string Operador = String.Empty;
            string IdConstructor = String.Empty;
            int OrdenIndicadorEnExcel = new Int32();
            bool existeCriterioConstructor = false;

            if (Indicadores == null)
            {
                jsonRespuesta.ok = false;

                jsonRespuesta.strMensaje = "Seleccione un Indicador";
                return jsonRespuesta.toJSON();
            }

            if (Operadores == null)
            {
                jsonRespuesta.ok = false;

                jsonRespuesta.strMensaje = "Seleccione un Operador";
                return jsonRespuesta.toJSON();
            }

            try
            {
                SolicitudConstructor solicitudAux = new SolicitudConstructor();

                solicitudAux.IdSolicitudIndicador = Guid.Parse(IdSolicitud);
                solicitudAux.IdEstado = 1;
                solicitudAux.FormularioWeb = Convert.ToByte(Session["FormularioWeb"]);

                foreach (string operador in Operadores.Split(','))
                    foreach (string objetoIndicador in Indicadores.Split(','))
                    {

                        Operador = operador;
                        IdConstructor = objetoIndicador.Split('|')[0];

                        existeCriterioConstructor = solicitudBL.
                                                                VerificarRelacionOperadorConstructor(Operador, IdConstructor).blnIndicadorTransaccion;
                        if (!existeCriterioConstructor)
                        {
                            algunIndicadorNoAsociado = true;
                            var opera = solicitudBL.gNombreOperador(Operador).objObjeto;
                            indicadoresOperadoresNoRelacionados.Add(solicitudBL.gNombreIndicador(IdConstructor).objObjeto.TrimEnd('.') + "|" + solicitudBL.gNombreOperador(Operador).objObjeto); //Se elimina el punto al final

                        }
                    }

                if (!algunIndicadorNoAsociado)
                {
                    foreach (string operador in Operadores.Split(','))
                        foreach (string objetoIndicador in Indicadores.Split(','))
                        {
                            IdConstructor = objetoIndicador.Split('|')[0];
                            int orden = int.Parse(objetoIndicador.Split('|')[1]);

                            solicitudAux.IdConstructor = Guid.Parse(IdConstructor);
                            solicitudAux.IdOperador = operador;
                            solicitudAux.OrdenIndicadorEnExcel = orden;  //Orden viene en la posicion 0
                            respAgregar = solicitudBL.AgregarSolicitudConstructor(solicitudAux);
                            jsonRespuesta.strMensaje = respAgregar.strMensaje;

                            if (respAgregar.blnIndicadorTransaccion)
                            {

                                jsonRespuesta.ok = true;

                            }
                            else
                            {

                                jsonRespuesta.ok = false;

                                jsonRespuesta.strMensaje = "Ha ocurrido un error al insertar la solicitud. 1";
                                return jsonRespuesta.toJSON();

                            }

                        }
                }
                else
                {

                    jsonRespuesta.ok = false;
                    jsonRespuesta.data = indicadoresOperadoresNoRelacionados;

                    jsonRespuesta.strMensaje = "NoAsociado";

                    //Cagada!!! No se hace la transacción dado que alguno no estaba asociado

                }
            }

            catch (Exception ex)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = "Ha ocurrido un error al insertar la solicitud. 2";

            }


            return jsonRespuesta.toJSON();
        }


        [HttpPost]
        [AuthorizeUserAttribute]
        public string Notificar(string ItemNotificar)
        {
            string IdSolicitud = ItemNotificar;
            List<string> FormularioyOperador = new List<string>();
            List<string> FormularioyOperadorFallido = new List<string>();
            string Descripcion = string.Empty;

            string mensaje = string.Empty;
            string email = string.Empty;
            string asunto = string.Empty;
            Respuesta<bool> respuesta = new Respuesta<bool>();
            JSONResult<SolicitudIndicador> jsonRespuesta = new JSONResult<SolicitudIndicador>();
            jsonRespuesta.data = new SolicitudIndicador();
            try
            {
                var user = ((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").FirstOrDefault();
                email = user.Value;



                string[] Operadores = solicitudBL.gOperadoresAsociado(ItemNotificar);
                string[] NombreOperadores = solicitudBL.gOperadoresNombres(Operadores);

                SolicitudIndicador objSolicitudIndicador = new SolicitudIndicador();
                objSolicitudIndicador = solicitudBL.gConsultarPorIdentificador(Guid.Parse(ItemNotificar)).objObjeto;
                Descripcion = objSolicitudIndicador.DescFormulario;

                jsonRespuesta.data.IdSolicitudIndicador = Guid.Parse(ItemNotificar);

                ObtenerMensajeNotificacion(ref mensaje, ref asunto, IdSolicitud);

                //se notifica
                if (Operadores.Length > 0)
                {

                    respuesta = (solicitudBL.gNotificar(Guid.Parse(IdSolicitud), mensaje, asunto));

                    if (respuesta.blnIndicadorTransaccion)
                    {
                        jsonRespuesta.ok = true;
                        jsonRespuesta.strMensaje = respuesta.strMensaje;
                        foreach (string item in NombreOperadores)
                        {
                            string nuevo = item + "/" + Descripcion;
                            FormularioyOperador.Add(nuevo);
                        }
                    }
                    else
                    {
                        //jsonRespuesta.ok = false;
                        foreach (string item in NombreOperadores)
                        {
                            string nuevo = item + "/" + Descripcion;
                            FormularioyOperadorFallido.Add(nuevo);
                        }
                    }
                }
                else
                {
                    string nuevo = "El Formulario no tiene un operador Asociado" + "/" + Descripcion;
                    FormularioyOperadorFallido.Add(nuevo);
                }

                ObtenerMensajeNotificacionOperador(ref mensaje, ref asunto, FormularioyOperador, FormularioyOperadorFallido);
                respuesta = (solicitudBL.gConfirmacion(email, mensaje, asunto, objSolicitudIndicador.IdDireccion));
            }
            catch (Exception ex)
            {
                //jsonRespuesta.ok = false;

            }

            return jsonRespuesta.toJSON();
        }

        [AuthorizeUserAttribute]
        public string NotificarMultiple()
        {
            string IdsSolicitud = Request.Params["data[IdsIndicador][]"];
            string email = "";

            List<string> FormularioyOperador = new List<string>();
            List<string> FormularioyOperadorFallido = new List<string>();

            string[] vectorSolicitudes = new string[0];

            if (!string.IsNullOrEmpty(IdsSolicitud))
            {
                vectorSolicitudes = IdsSolicitud.Split(',');
            }

            var user = ((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").FirstOrDefault();
            email = user.Value;

            string mensaje = string.Empty;
            string mensaje2 = string.Empty;
            string asunto = string.Empty;
            string Descripcion = string.Empty;
            int direccion = 0;
            Respuesta<bool> respuesta = new Respuesta<bool>();
            JSONResult<SolicitudIndicador> jsonRespuesta = new JSONResult<SolicitudIndicador>();
            jsonRespuesta.data = new SolicitudIndicador();
            try
            {
                foreach (string ItemNotificar in vectorSolicitudes)
                {

                    string[] Operadores = solicitudBL.gOperadoresAsociado(ItemNotificar);
                    string[] NombreOperadores = solicitudBL.gOperadoresNombres(Operadores);

                    SolicitudIndicador objSolicitudIndicador = new SolicitudIndicador();
                    objSolicitudIndicador = solicitudBL.gConsultarPorIdentificador(Guid.Parse(ItemNotificar)).objObjeto;
                    Descripcion = objSolicitudIndicador.DescFormulario;
                    direccion = objSolicitudIndicador.IdDireccion;

                    jsonRespuesta.data = new SolicitudIndicador();
                    jsonRespuesta.data.IdSolicitudIndicador = Guid.Parse(ItemNotificar);
                    ObtenerMensajeNotificacion(ref mensaje, ref asunto, ItemNotificar);


                    //se notifica
                    if (Operadores.Length > 0)
                    {
                        respuesta = (solicitudBL.gNotificar(Guid.Parse(ItemNotificar), mensaje, asunto));

                        if (respuesta.blnIndicadorTransaccion)
                        {
                            jsonRespuesta.ok = true;
                            jsonRespuesta.strMensaje = respuesta.strMensaje;
                            foreach (string item in NombreOperadores)
                            {
                                string nuevo = item + "/" + Descripcion;
                                FormularioyOperador.Add(nuevo);
                            }

                        }
                        else
                        {
                            //jsonRespuesta.ok = false;
                            foreach (string item in NombreOperadores)
                            {
                                string nuevo = item + "/" + Descripcion;
                                FormularioyOperadorFallido.Add(nuevo);
                            }

                        }

                    }
                    else
                    {
                        string nuevo = "El Formulario no tiene un operador Asociado" + "/" + Descripcion;
                        FormularioyOperadorFallido.Add(nuevo);
                    }

                }

                if (email != "Undefined" && email != "")
                {
                    ObtenerMensajeNotificacionOperador(ref mensaje2, ref asunto, FormularioyOperador, FormularioyOperadorFallido);
                    respuesta = (solicitudBL.gConfirmacion(email, mensaje2, asunto, direccion));
                }
                else
                {
                    jsonRespuesta.strMensaje = "Se ha notificado a los operadores, pero no recibirá correo de listado debido a que  su usuario no tiene un correo registrado";
                }



            }
            catch (Exception ex)
            {
                //jsonRespuesta.ok = false;
                //jsonRespuesta.strMensaje = "Hubo un error al tratar de enviar el correo";
                //jsonRespuesta.strMensaje = "Hubo un error: " + ex.Message;
            }

            return jsonRespuesta.toJSON();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>




        [AuthorizeUserAttribute]
        public string GuardarIndicadoresEditar()
        {
            string IdSolicitud = Request.Params["data[IdSolicitudIndicador]"];
            //string Indicadores = Request.Params["data[Indicadores][]"];
            //string Operadores = Request.Params["data[Operadores][]"];
            //string IndicadoresEliminar = Request.Params["data[IndicadoresEliminar][]"];
            Respuesta<SolicitudConstructor> respuesta = new Respuesta<SolicitudConstructor>();
            JSONResult<List<string>> jsonRespuesta = new JSONResult<List<string>>();
            //---------------------------------------------------------------------

            string IndicadoresInicial = Request.Params["data[IndicadorInicial][]"];//vector inicial trae los datos originales.
            string IndicadoresEditar = Request.Params["data[IndicadorEditar][]"];//vector espejo trae los checks tal y como estan chequeados en la vista
            string OperadoresEditar = Request.Params["data[OperadorEditar][]"];//vector espejo de operadores trae  los checks de operadores tal y como esta en la vista.
            string OperadoresInicial = Request.Params["data[OperadorInicial][]"];//vector operadores inicial trae los datos originales.

            //valida que existan checks 
            if (IndicadoresEditar == null)
            {
                jsonRespuesta.ok = false;

                jsonRespuesta.strMensaje = "Seleccione un Indicador";
                return jsonRespuesta.toJSON();
            }
            //valida que existan checks 
            if (OperadoresEditar == null)
            {
                jsonRespuesta.ok = false;

                jsonRespuesta.strMensaje = "Seleccione un Operador";
                return jsonRespuesta.toJSON();
            }


            if (IndicadoresInicial == null) { IndicadoresInicial = ""; }

            if (IndicadoresEditar == null) { IndicadoresEditar = ""; }

            if (OperadoresEditar == null) { IndicadoresEditar = ""; }

            if (OperadoresInicial == null) { OperadoresInicial = ""; }


            string InsertarIndicadores = string.Join(",", RetornarLista(IndicadoresInicial, IndicadoresEditar));// recibe lista inicial y editar y retorna los nuevos indicadores a Insertar         
            string EliminarIndicadores = string.Join(",", RetornarLista(IndicadoresEditar, IndicadoresInicial));// recibe de editar he inicial y retorna los indicadores ha eliminar          
            string InsertarOperadores = string.Join(",", RetornarLista(OperadoresInicial, OperadoresEditar));//recibe operadores incial y operadores editar y retorna los operadores ha insertar
            string EliminarOperadores = string.Join(",", RetornarLista(OperadoresEditar, OperadoresInicial));//recibe operadores editar y operadores inicial y retorna los operadores a eliminar 




            //JSONResult<SolicitudConstructor> jsonRespuesta = new JSONResult<SolicitudConstructor>();

            //------------variables necesarias-------------------//
            Respuesta<SolicitudConstructor> RespuestaEliminar;
            Respuesta<bool> RespuestaEliminar2;
            Respuesta<SolicitudConstructor> RespuestaAgregar;
            List<string> indicadoresOperadoresNoRelacionados = new List<string>();
            bool algunIndicadorNoAsociado = false;
            string Operador = String.Empty;
            string IdConstructor = String.Empty;
            int OrdenIndicadorEnExcel = new Int32();
            bool existeCriterioConstructor = false;


            try
            {
                SolicitudConstructor solicitudAux = new SolicitudConstructor();

                solicitudAux.IdSolicitudIndicador = Guid.Parse(IdSolicitud);
                solicitudAux.IdEstado = 1;


                //Se pregunta si lo nuevos Indicadores a Insertar estan relacionados con los operadores
                foreach (string operador in OperadoresEditar.Split(','))
                    foreach (string objetoIndicador in IndicadoresEditar.Split(','))
                    {

                        Operador = operador;
                        IdConstructor = objetoIndicador.Split('|')[0];

                        existeCriterioConstructor = solicitudBL.
                                                                VerificarRelacionOperadorConstructor(Operador, IdConstructor).blnIndicadorTransaccion;
                        if (!existeCriterioConstructor)
                        {
                            algunIndicadorNoAsociado = true;
                            var opera = solicitudBL.gNombreOperador(Operador).objObjeto;
                            indicadoresOperadoresNoRelacionados.Add(solicitudBL.gNombreIndicador(IdConstructor).objObjeto.TrimEnd('.') + "|" + solicitudBL.gNombreOperador(Operador).objObjeto); //Se elimina el punto al final

                        }
                    }

                // solo si los indicadores estan relacionados con los operadores entran ha este if
                if (!algunIndicadorNoAsociado)
                {

                    if (EliminarIndicadores != "")
                    { //elimina todos los Indicadores en esta lista             
                        foreach (string operador in OperadoresInicial.Split(','))
                            foreach (string objetoIndicador in EliminarIndicadores.Split(','))
                            {
                                IdConstructor = objetoIndicador.Split('|')[0];
                                int orden = int.Parse(objetoIndicador.Split('|')[1]);

                                solicitudAux.IdConstructor = Guid.Parse(IdConstructor);
                                solicitudAux.IdOperador = operador;
                                RespuestaEliminar = solicitudBL.EliminarSolicitudConstructor(solicitudAux.IdSolicitudIndicador, solicitudAux.IdConstructor, solicitudAux.IdOperador);// elimina los indicadores


                                jsonRespuesta.strMensaje = RespuestaEliminar.strMensaje;

                                if (RespuestaEliminar.blnIndicadorTransaccion)
                                {

                                    if (InsertarOperadores == "" && InsertarIndicadores == "")
                                        //crear excel
                                        solicitudBL.InsertarArchivoExcel(solicitudAux.IdSolicitudIndicador, solicitudAux.IdOperador);

                                    jsonRespuesta.ok = true;
                                    //bitacora
                                    gBitacoraSolicitudConstructor(ActionsBinnacle.Borrar, null, respuesta.objObjeto);
                                    solicitudAux = new SolicitudConstructor();

                                    solicitudAux.IdSolicitudIndicador = Guid.Parse(IdSolicitud);
                                    solicitudAux.IdEstado = 1;

                                }
                                else
                                {

                                    jsonRespuesta.ok = false;

                                    jsonRespuesta.strMensaje = "Ha ocurrido un error al editar la solicitud, con los indicadores. 1";
                                    return jsonRespuesta.toJSON();

                                }

                            }
                    }

                    // ingresa cuando existen nuevos indicadores
                    if (InsertarIndicadores != "")
                    {
                        foreach (string operador in OperadoresEditar.Split(','))
                            foreach (string objetoIndicador in InsertarIndicadores.Split(','))
                            {
                                IdConstructor = objetoIndicador.Split('|')[0];
                                int orden = int.Parse(objetoIndicador.Split('|')[1]);

                                solicitudAux.IdConstructor = Guid.Parse(IdConstructor);
                                solicitudAux.IdOperador = operador;
                                solicitudAux.OrdenIndicadorEnExcel = orden;  //Orden viene en la posicion 0

                                //eliminar

                                RespuestaAgregar = solicitudBL.AgregarSolicitudConstructor(solicitudAux);// inserta los nuevos indicadores
                                jsonRespuesta.strMensaje = RespuestaAgregar.strMensaje;

                                if (RespuestaAgregar.blnIndicadorTransaccion)
                                {

                                    jsonRespuesta.ok = true;

                                    //bitacora
                                    gBitacoraSolicitudConstructor(ActionsBinnacle.Crear, RespuestaAgregar.objObjeto, null);
                                    solicitudAux = new SolicitudConstructor();

                                    solicitudAux.IdSolicitudIndicador = Guid.Parse(IdSolicitud);
                                    solicitudAux.IdEstado = 1;

                                }
                                else
                                {

                                    jsonRespuesta.ok = false;

                                    jsonRespuesta.strMensaje = "Ha ocurrido un error al Editar agregando los nuevos indicadores a la solicitud. 2";
                                    return jsonRespuesta.toJSON();

                                }

                            }

                    }
                    //ingresa cuando existen operadores a eliminar 
                    if (EliminarOperadores != "")
                    {
                        foreach (string operador in EliminarOperadores.Split(','))
                        {

                            solicitudAux.IdOperador = operador;
                            RespuestaEliminar2 = solicitudBL.EliminarSolicitudConstructor(solicitudAux.IdSolicitudIndicador, operador);
                            jsonRespuesta.strMensaje = RespuestaEliminar2.strMensaje;// elimina los operadores 

                            if (RespuestaEliminar2.blnIndicadorTransaccion)
                            {
                                // if (InsertarOperadores == "" && InsertarIndicadores == "") se comento por que se duplica el excel
                                //crear excel
                                // solicitudBL.InsertarArchivoExcel(solicitudAux.IdSolicitudIndicador, solicitudAux.IdOperador);

                                jsonRespuesta.ok = true;
                                //bitacora
                                gBitacoraSolicitudConstructor(ActionsBinnacle.Borrar, null, respuesta.objObjeto);
                                solicitudAux = new SolicitudConstructor();
                                solicitudAux.IdSolicitudIndicador = Guid.Parse(IdSolicitud);
                                solicitudAux.IdEstado = 1;

                            }
                            else
                            {

                                jsonRespuesta.ok = false;

                                jsonRespuesta.strMensaje = "Ha ocurrido un error al editar la solicitud, con los operadores. 1";
                                return jsonRespuesta.toJSON();

                            }

                        }

                    }

                    //insertar operadores 
                    if (InsertarOperadores != "")
                    {
                        foreach (string operador in InsertarOperadores.Split(','))


                            foreach (string objetoIndicador in IndicadoresEditar.Split(','))
                            {

                                if (IndicadoresInicial.Contains(objetoIndicador) && !EliminarIndicadores.Contains(objetoIndicador))//ingresa solo si el indicador esta en inicial y no esta en eliminar indicadores 
                                {
                                    IdConstructor = objetoIndicador.Split('|')[0];
                                    int orden = int.Parse(objetoIndicador.Split('|')[1]);

                                    solicitudAux.IdConstructor = Guid.Parse(IdConstructor);
                                    solicitudAux.IdOperador = operador;
                                    solicitudAux.OrdenIndicadorEnExcel = orden;  //Orden viene en la posicion 0
                                    RespuestaAgregar = solicitudBL.AgregarSolicitudConstructor(solicitudAux);//inserta los operadores 
                                    jsonRespuesta.strMensaje = RespuestaAgregar.strMensaje;

                                    if (RespuestaAgregar.blnIndicadorTransaccion)
                                    {

                                        jsonRespuesta.ok = true;

                                        //bitacora
                                        gBitacoraSolicitudConstructor(ActionsBinnacle.Crear, RespuestaAgregar.objObjeto, null);
                                        solicitudAux = new SolicitudConstructor();

                                        solicitudAux.IdSolicitudIndicador = Guid.Parse(IdSolicitud);
                                        solicitudAux.IdEstado = 1;

                                    }
                                    else
                                    {

                                        jsonRespuesta.ok = false;

                                        jsonRespuesta.strMensaje = "Ha ocurrido un error al Editar agregando los nuevos operadores la solicitud. 2";
                                        return jsonRespuesta.toJSON();

                                    }

                                }



                            }

                    }

                }
                else
                {
                    jsonRespuesta.ok = false;
                    jsonRespuesta.data = indicadoresOperadoresNoRelacionados;

                    jsonRespuesta.strMensaje = "NoAsociado";

                    //Cagada!!! No se hace la transacción dado que alguno no estaba asociado

                }


            }
            catch (Exception ex)
            {
                jsonRespuesta.ok = false;

            }

            return jsonRespuesta.toJSON();
        }


        /// <summary>
        /// El metodo retornar Lista prodra ser utilizado tando 
        /// para crear la Lista de insertar y de eliminar tanto Indicadores
        /// como operadores, se recive por parametros una L1, que donde 
        /// se busca un dato si no existe se agrega a lista que se va retornar
        /// </summary>
        /// <param name="L1"></param>
        /// <param name="L2"></param>
        /// <returns></returns>
        private string[] RetornarLista(string L1, string L2)
        {

            string[] Lista1 = L1.Split(',');
            string[] Lista2 = L2.Split(',');


            bool yaexiste = false;
            List<string> ListaResult = new List<string>();

            for (int i = 0; i < Lista2.Length; i++)
            {
                for (int j = 0; j < Lista1.Length; j++)
                {
                    if (Lista2[i] == Lista1[j])
                    {
                        yaexiste = true;
                    }

                }

                if (!yaexiste)
                {
                    ListaResult.Add(Lista2[i]);
                }

                yaexiste = false;

            }

            return ListaResult.ToArray();
        }



        private void gBitacoraSolicitud(ActionsBinnacle accion, SolicitudIndicador nuevaSolicitud, SolicitudIndicador anteriorSolicitud)
        {
            StringBuilder newData = new StringBuilder();
            StringBuilder oldData = new StringBuilder();
            String pantalla = "";
            String descripcion = "";
            pantalla = CBitacora.GetValueAsString(CBitacora.BitacoraOpciones.solicitud);
            if (nuevaSolicitud != null)
            {
                newData.Append("Registro de una Solicitud con Id: ");
                newData.Append(nuevaSolicitud.IdSolicitudIndicador);
                newData.Append(" Descripción de la Solicitud: ");
                newData.Append(nuevaSolicitud.DescFormulario);

                newData.Append(" Id Direccion: ");
                newData.Append(nuevaSolicitud.IdDireccion);
                newData.Append(" Id Frecuencia: ");
                newData.Append(nuevaSolicitud.IdFrecuencia);

                newData.Append(" Id Servicio: ");
                newData.Append(nuevaSolicitud.IdServicio);

                newData.Append(" Con Fecha de Inicio: ");
                newData.Append(nuevaSolicitud.FechaInicio.ToString());

                newData.Append(" Con Fecha de Final: ");
                newData.Append(nuevaSolicitud.FechaFin.ToString());

                newData.Append(" Bandera de borrado de la solicitud: ");
                newData.Append(nuevaSolicitud.Borrado);
            }
            if (anteriorSolicitud != null)
            {
                oldData.Append("Registro de una Solicitud con Id: ");
                oldData.Append(anteriorSolicitud.IdSolicitudIndicador);
                oldData.Append(" Descripción de la Solicitud: ");
                oldData.Append(anteriorSolicitud.DescFormulario);

                oldData.Append(" Id Direccion: ");
                oldData.Append(anteriorSolicitud.IdDireccion);
                oldData.Append(" Id Frecuencia: ");
                oldData.Append(anteriorSolicitud.IdFrecuencia);

                oldData.Append(" Id Servicio: ");
                oldData.Append(anteriorSolicitud.IdServicio);

                oldData.Append(" Con Fecha de Inicio: ");
                oldData.Append(anteriorSolicitud.FechaInicio.ToString());

                oldData.Append(" Con Fecha de Final: ");
                oldData.Append(anteriorSolicitud.FechaFin.ToString());

                oldData.Append(" Bandera de borrado de la solicitud: ");
                oldData.Append(anteriorSolicitud.Borrado);
            }

            if (accion.Equals(ActionsBinnacle.Crear))
            {
                descripcion = "Proceso de creación de " + pantalla;
            }
            else if (accion.Equals(ActionsBinnacle.Editar))
            {
                descripcion = "Proceso de edición de " + pantalla;
            }
            else if (accion.Equals(ActionsBinnacle.Borrar))
            {
                descripcion = "Proceso de eliminación de " + pantalla;
            }
            else
            {
                descripcion = "Proceso en " + pantalla;
            }


            try
            {

                CBitacora.gRegistrarBitacora(AppContext, Convert.ToInt32(accion.GetHashCode()), pantalla,
                                                    descripcion, newData.ToString(), oldData.ToString());
            }
            catch (Exception)
            {



            }
        }

        private void gBitacoraSolicitudConstructor(ActionsBinnacle accion, SolicitudConstructor nuevaSolicitud, SolicitudConstructor anteriorSolicitud)
        {
            StringBuilder newData = new StringBuilder();
            StringBuilder oldData = new StringBuilder();
            String pantalla = "";
            String descripcion = "";
            pantalla = CBitacora.GetValueAsString(CBitacora.BitacoraOpciones.solicitud) + " sección Indicadores";
            if (nuevaSolicitud != null)
            {
                newData.Append("Registro de una Solicitud Constructor con Id: ");
                newData.Append(nuevaSolicitud.IdSolicitudContructor);
                newData.Append(" Para la Solicitud con Id: ");
                newData.Append(nuevaSolicitud.IdSolicitudIndicador);

                newData.Append(" Con el constructor: ");
                newData.Append(nuevaSolicitud.IdConstructor);

                newData.Append(" Con estado: ");
                newData.Append(nuevaSolicitud.IdEstado);

                newData.Append(" Con Id de Operador: ");
                newData.Append(nuevaSolicitud.IdOperador);

                newData.Append(" Bandera de borrado de la solicitud: ");
                newData.Append(nuevaSolicitud.Borrado);
            }
            if (anteriorSolicitud != null)
            {
                oldData.Append("Registro de una Solicitud Constructor con Id: ");
                oldData.Append(nuevaSolicitud.IdSolicitudContructor);
                oldData.Append(" Para la Solicitud con Id: ");
                oldData.Append(nuevaSolicitud.IdSolicitudIndicador);

                oldData.Append(" Con el constructor: ");
                oldData.Append(nuevaSolicitud.IdConstructor);

                oldData.Append(" Con estado: ");
                oldData.Append(nuevaSolicitud.IdEstado);

                oldData.Append(" Con Id de Operador: ");
                oldData.Append(nuevaSolicitud.IdOperador);

                oldData.Append(" Bandera de borrado de la solicitud: ");
                oldData.Append(nuevaSolicitud.Borrado);
            }

            if (accion.Equals(ActionsBinnacle.Crear))
            {
                descripcion = "Proceso de creación de " + pantalla;
            }
            else if (accion.Equals(ActionsBinnacle.Editar))
            {
                descripcion = "Proceso de edición de " + pantalla;
            }
            else if (accion.Equals(ActionsBinnacle.Borrar))
            {
                descripcion = "Proceso de eliminación de " + pantalla;
            }
            else
            {
                descripcion = "Proceso en " + pantalla;
            }


            try
            {

                CBitacora.gRegistrarBitacora(AppContext, Convert.ToInt32(accion.GetHashCode()), pantalla,
                                                    descripcion, newData.ToString(), oldData.ToString());
            }
            catch (Exception)
            {



            }
        }


        private void cargarDireccionesEnBag()
        {
            List<Direccion> myCollection = direccionBL.gListar().objObjeto;
            myCollection.Insert(0, new Direccion() { IdDireccion = 0, Nombre = "<Seleccione>" });

            ViewBag.listaDirecciones = new SelectList(myCollection, "IdDireccion", "Nombre").ToList();
        }

        private void cargarFrecuenciasEnBag()
        {
            List<Frecuencia> myCollection = frecuenciaBL.gListar().objObjeto;
            myCollection.Insert(0, new Frecuencia() { IdFrecuencia = 0, NombreFrecuencia = "<Seleccione>" });

            ViewBag.listaFrecuencias = new SelectList(myCollection, "IdFrecuencia", "NombreFrecuencia").ToList();
        }

        private void cargarServiciosEnBag()
        {
            List<Servicio> myCollection = servicioBL.ConsultarTodos().objObjeto;
            ViewBag.Servicios = new SelectList(myCollection, "IdServicio", "DesServicio").ToList();
            myCollection.Insert(0, new Servicio() { IdServicio = 0, DesServicio = "<Seleccione>" });

            ViewBag.listaServicios = new SelectList(myCollection, "IdServicio", "DesServicio").ToList();
        }


        #region Notificacion
        private void ObtenerMensajeNotificacion(ref string poHtml, ref string poAsunto, string IdSolicitud)
        {
            SolicitudIndicador objSolicitudIndicador = new SolicitudIndicador();
            objSolicitudIndicador = solicitudBL.gConsultarPorIdentificador(Guid.Parse(IdSolicitud)).objObjeto;

            string host = Request.Url.Host;
            String tipoSitio = "https";

            MailingModel message = new MailingModel()
            {
                ImagenSutel = tipoSitio + "://" + host + "/Content/Images/logos/logo-Sutel_11_3.png",
                RutaSistema = tipoSitio + "://" + host + "/"
            };

            string renderedEmail = RenderView.RenderViewToString("Emails", "notificaciones", new Tuple<SolicitudIndicador, MailingModel>(objSolicitudIndicador, message));

            //poHtml = GetHTMLParsedAsString(Server.MapPath(ConfigurationManager.AppSettings["URLPlantillaNotificacion"].ToString()));
            poHtml = renderedEmail;
            poAsunto = ConfigurationManager.AppSettings["AsuntoNotificacion"].ToString();
        }

        private void ObtenerMensajeNotificacionOperador(ref string poHtml, ref string poAsunto, List<string> nomOperadores, List<string> fallidos)
        {
            SolicitudIndicador objSolicitudIndicador = new SolicitudIndicador();
            // objSolicitudIndicador = solicitudBL.gConsultarPorIdentificador(Guid.Parse(IdSolicitud)).objObjeto;

            string host = Request.Url.Host;
            String tipoSitio = "https";

            MailingModel message = new MailingModel()
            {
                ImagenSutel = tipoSitio + "://" + host + "/Content/Images/logos/logo-Sutel_11_3.png",
                RutaSistema = tipoSitio + "://" + host + "/"
            };

            if (fallidos.Count == 0)
                fallidos = null;

            string renderedEmail = RenderView.RenderViewToString("Emails", "notificacionSendmail", new Tuple<List<string>, MailingModel, List<string>>(nomOperadores, message, fallidos));

            //poHtml = GetHTMLParsedAsString(Server.MapPath(ConfigurationManager.AppSettings["URLPlantillaNotificacion"].ToString()));
            poHtml = renderedEmail;
            poAsunto = ConfigurationManager.AppSettings["AsuntoNotificacion"].ToString();
        }



        private string GetHTMLParsedAsString(string templateUrl)
        {
            string template = System.IO.File.ReadAllText(templateUrl);

            return template;
        }

        #endregion

        #region DecargarExcel
        //
        //Descarga de Excel para la previsualización 
        [HttpGet]
        [AuthorizeUserAttribute]
        public ActionResult DescargarExcel(string IdSolicitud, string registroIndicadoresNombreExcel, string registroIndicadoresExtension)
        {
            Respuesta<Byte[]> respuesta = new Respuesta<Byte[]>();
            try
            {
                string nombreUsuario = "";
                string IdOperador = "";
                string URLSaveFormatXLS = Server.MapPath("~/Recursos/ConvertToOldExcel/");

                if (((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault() != null)
                {
                    nombreUsuario = ((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault().Value;
                    BL.Seguridad.UsersBL refUsuario = new BL.Seguridad.UsersBL(AppContext);
                    IdOperador = refUsuario.ConsultarPorExpresion(x => x.AccesoUsuario == nombreUsuario && x.Borrado == 0 && x.Activo == 1).objObjeto.IdOperador;
                }

                if (registroIndicadoresExtension == "xls")
                {
                    registroIndicadoresExtension = "." + registroIndicadoresExtension;
                }
                else
                {
                    registroIndicadoresExtension = ".xlsm";
                }

                if (IdSolicitud != "" && IdSolicitud != null)
                {
                    respuesta = refRegistroIndicadorBL.obtenerArchivoExcelPre(IdSolicitud, registroIndicadoresNombreExcel, registroIndicadoresExtension);
                    //respuesta = refRegistroIndicadorBL.CrearIndicadoresExcel(IdOperador, IdSolicitud, nombreUsuario, registroIndicadoresNombreExcel, registroIndicadoresExtension, URLSaveFormatXLS);
                    respuesta.strMensaje = respuesta.strMensaje;
                }
                else
                {
                    return RedirectToAction("Error", "RegistroIndicador");
                }

                if (respuesta.blnIndicadorTransaccion)
                {
                    if (respuesta.objObjeto != null)
                    {

                        Response.BinaryWrite(respuesta.objObjeto);
                        Response.ContentType = "application/vnd.ms-excel.sheet.macroEnabled.12";
                        Response.AddHeader("content-disposition", "attachment;  filename=" + registroIndicadoresNombreExcel + registroIndicadoresExtension);

                        return new EmptyResult();

                        //FileContentResult file = new FileContentResult(respuesta.objObjeto, "application/vnd.ms-excel.sheet.macroEnabled.12");                        
                        //file.FileDownloadName = registroIndicadoresNombreExcel + registroIndicadoresExtension                        
                    }
                    else
                    {
                        return RedirectToAction("ErrorData", "RegistroIndicador");
                    }
                }
                else
                {
                    return RedirectToAction("Error", "RegistroIndicador");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "RegistroIndicador");
            }
        }


        #endregion
    }
}
