using GB.SUTEL.DAL.Mantenimientos;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SUTEL.Resources;
using GB.SUTEL.Shared;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.Entities.DTO;

namespace GB.SUTEL.BL.Mantenimientos
{
    public class SolicitudIndicadorBL : LocalContextualizer
    {
        #region atributos
        SolicitudIndicadorAD solicitudAD;

        private SolicitudIndicador solicitudIndicador;

        public SolicitudIndicador SolicitudIndicador
        {
            get
            {
                return this.solicitudIndicador;
            }
            set
            {
                this.solicitudIndicador = value;
            }
        }
        #endregion

        #region Constructores
        public SolicitudIndicadorBL(ApplicationContext appContext)
            : base(appContext)
        {

            solicitudAD = new SolicitudIndicadorAD(appContext);
        }

        public SolicitudIndicadorBL(SolicitudIndicador poSolicitudIndicador, ApplicationContext appContext)
            : base(appContext)
        {
            solicitudAD = new SolicitudIndicadorAD(appContext);
            this.solicitudIndicador = poSolicitudIndicador;

        }
        #endregion

        #region Metodos

        /// <summary>
        /// Lista todas las solicitudes 
        /// </summary>
        /// <returns></returns>
        public Respuesta<List<SolicitudIndicador>> gListar()
        {
            Respuesta<List<SolicitudIndicador>> respuesta = new Respuesta<List<SolicitudIndicador>>();

            try
            {
                respuesta = solicitudAD.gListar();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        /// <summary>
        /// Consulta un registro por identificador 
        /// </summary>
        /// <param name="poIdSolicitudIndicador"></param>
        /// <returns></returns>
        public Respuesta<SolicitudIndicador> gConsultarPorIdentificador(Guid poIdSolicitudIndicador)
        {
            Respuesta<SolicitudIndicador> respuesta = new Respuesta<SolicitudIndicador>();

            try
            {
                respuesta = solicitudAD.gConsultarPorIdentificador(poIdSolicitudIndicador);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }
        public Respuesta<ConfirmaDescargaWebDto> gConsultarConstructorPorIndicador(Guid IdSolicitudIndicador, string IdOperador)
        {
            Respuesta<ConfirmaDescargaWebDto> respuesta = new Respuesta<ConfirmaDescargaWebDto>();

            try
            {
                respuesta = solicitudAD.gConsultarConstructorPorIndicador(IdSolicitudIndicador, IdOperador);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }
        public Respuesta<ConfirmaDescargaWebDto> gConsultarListaDetalleRegistroIndicador(List<Guid> ListaIdSolicitudConstructor)
        {
            Respuesta<ConfirmaDescargaWebDto> respuesta = new Respuesta<ConfirmaDescargaWebDto>();

            try
            {
                respuesta = solicitudAD.gConsultarListaDetalleRegistroIndicador(ListaIdSolicitudConstructor);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }
        public Respuesta<ConfirmaDescargaWebDto> gConsultarDetalleRegistroIndicador(Guid IdSolicitudConstructor)
        {
            Respuesta<ConfirmaDescargaWebDto> respuesta = new Respuesta<ConfirmaDescargaWebDto>();

            try
            {
                respuesta = solicitudAD.gConsultarDetalleRegistroIndicador(IdSolicitudConstructor);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }
        public Respuesta<SolicitudConstructor> actualizarSemaforos(Guid IdSolicitudConstructor, string IdOperador, int idSemaforoActualizar, List<ListaIDDto> Valor, string Observacion,Usuario usuario)
        {
            Respuesta<SolicitudConstructor> respuesta = new Respuesta<SolicitudConstructor>();

            try
            {
                respuesta = solicitudAD.actualizarSemaforos(IdSolicitudConstructor, IdOperador, idSemaforoActualizar, Valor, Observacion,usuario);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }
        public Respuesta<SolicitudConstructor> cargarDatos(Guid IdSolicitud, string IdOperador, int poDireccion)
         {
            Respuesta<SolicitudConstructor> respuesta = new Respuesta<SolicitudConstructor>();

            try
            {
                respuesta = solicitudAD.cargarDatos(IdSolicitud, IdOperador, poDireccion);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        public Respuesta<List<SolicitudConstructorDto>> gConsultarOperadorFormularioWebPorIndicador(Guid IdSolicitudIndicador)
        {
            Respuesta<List<SolicitudConstructorDto>> respuesta = new Respuesta<List<SolicitudConstructorDto>>();

            try
            {
                respuesta = solicitudAD.gConsultarOperadorFormularioWebPorIndicador(IdSolicitudIndicador);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        /// <summary>
        /// Consulta de Solicitudes Indicador por Descripcion, Fecha Inicio, Fecha Final y Estado
        /// </summary>
        /// <param name="poDescripcionFormulario"></param>
        /// <param name="poFechaInicio"></param>
        /// <param name="poFechaFinal"></param>
        /// <param name="poEstado"></param>
        /// <returns></returns>
        public Respuesta<List<SolicitudIndicador>> gConsultarPorFiltros(string poDescripcionFormulario, DateTime? poFechaInicio, DateTime? poFechaFinal, List<int> poServicios)
        {
            Respuesta<List<SolicitudIndicador>> respuesta = new Respuesta<List<SolicitudIndicador>>();

            try
            {

                respuesta = solicitudAD.gConsultarPorFiltros(poDescripcionFormulario, poFechaInicio, poFechaFinal, poServicios);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }


        /// <summary>
        /// Inserta una solicitud de Indicador
        /// </summary>
        /// <param name="poSolicitudIndicador"></param>
        /// <returns></returns>
        public Respuesta<SolicitudIndicador> gAgregar(SolicitudIndicador poSolicitudIndicador)
        {

            Respuesta<SolicitudIndicador> respuesta = new Respuesta<SolicitudIndicador>();
            this.solicitudIndicador = poSolicitudIndicador;
            try
            {

                //se limpian espacios al inicio y final
                object aux = (object)poSolicitudIndicador;
                Utilidades.LimpiarEspacios(ref aux);

                if (solicitudAD.gConsultarPorDescripcion(poSolicitudIndicador.DescFormulario).objObjeto == null)
                {
                    respuesta = solicitudAD.gAgregar(solicitudIndicador);
                    if (respuesta.blnIndicadorTransaccion)
                    {
                        respuesta.strMensaje = Mensajes.ExitoInsertar;
                    }
                }
                else
                {
                    respuesta.blnIndicadorTransaccion = false;
                    respuesta.strMensaje = Mensajes.RegistroDuplicado;
                }
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, solicitudIndicador);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }

        /// <summary>
        /// Inserta un Solicitud del indicador con el objeto pasado por constructor
        /// </summary>
        /// <returns></returns>
        public Respuesta<SolicitudIndicador> gAgregar()
        {
            return this.gAgregar(this.solicitudIndicador);
        }

        /// <summary>
        /// Modifica una solicitud Indicador
        /// </summary>
        /// <param name="poSolicitudIndicador"></param>
        /// <returns></returns>
        public Respuesta<SolicitudIndicador> gModificar(SolicitudIndicador poSolicitudIndicador)
        {
            Respuesta<SolicitudIndicador> respuesta = new Respuesta<SolicitudIndicador>();
            this.solicitudIndicador = poSolicitudIndicador;

            try
            {
                //se limpian espacios al inicio y final
                object aux = (object)poSolicitudIndicador;
                Utilidades.LimpiarEspacios(ref aux);

                SolicitudIndicador solicitudAux = solicitudAD.gConsultarPorDescripcion(this.solicitudIndicador.DescFormulario).objObjeto;
                if (solicitudAux == null || (solicitudAux.IdSolicitudIndicador == poSolicitudIndicador.IdSolicitudIndicador))
                {

                    respuesta = solicitudAD.gModificar(this.solicitudIndicador);
                    if (respuesta.blnIndicadorTransaccion)
                    {
                        respuesta.strMensaje = Mensajes.ExitoEditar;
                    }
                }
                else
                {
                    respuesta.blnIndicadorTransaccion = false;
                    respuesta.strMensaje = Mensajes.RegistroDuplicado;
                }
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, poSolicitudIndicador);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        /// <summary>
        /// Modifica una solicitud Indicador con el objeto pasado por constructor
        /// </summary>
        /// <returns></returns>
        public Respuesta<SolicitudIndicador> gModificar()
        {
            return gModificar(this.solicitudIndicador);
        }

        /// <summary>
        /// Elimina Logicamente un registro de Solicitud Indicador
        /// </summary>
        /// <param name="poSolicitudIndicador"></param>
        /// <returns></returns>
        public Respuesta<SolicitudIndicador> gEliminar(SolicitudIndicador poSolicitudIndicador)
        {
            Respuesta<SolicitudIndicador> respuesta = new Respuesta<SolicitudIndicador>();
            this.solicitudIndicador = poSolicitudIndicador;
            try
            {
                respuesta = solicitudAD.gEliminar(this.solicitudIndicador);

                if (respuesta.blnIndicadorTransaccion)
                {
                    respuesta.strMensaje = Mensajes.ExitoEliminar;
                }
                else
                {
                    respuesta.strMensaje = Mensajes.RegistroNoSePuedeEliminar;
                }
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = Mensajes.RegistroNoSePuedeEliminar;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        /// <summary>
        /// Elimina una solicitud con el objeto pasado por constructor
        /// </summary>
        /// <returns></returns>
        public Respuesta<SolicitudIndicador> gEliminaropcional(Guid idSolicitud, string[] Operadores, bool Completa)
        {
            Respuesta<SolicitudIndicador> solicitudActual = new Respuesta<SolicitudIndicador>();
            Respuesta<SolicitudIndicador> respuesta = new Respuesta<SolicitudIndicador>();
            Respuesta<List<ArchivoExcel>> ArchivoExcel = new Respuesta<List<ArchivoExcel>>();
            string Mensaje = "La solicitud fue eliminada correctamente. </br> Los siguientes operadores ya habían descargado la plantilla: ";
            int contador = 0;

            try
            {
                solicitudActual = solicitudAD.gConsultarPorIdentificador(idSolicitud);
                ArchivoExcel = solicitudAD.gConsultarPorIdentificadorExcel(idSolicitud);
                respuesta = solicitudAD.gEliminarLogicamente(solicitudActual.objObjeto, Operadores, Completa);

                if (respuesta.strMensaje == "Dependencias")
                {
                    respuesta.objObjeto = new SolicitudIndicador();
                    respuesta.blnIndicadorTransaccion = false;
                    Mensaje = "La solicitud no puede ser eliminada, dado que ya al menos un operador cargó información en SITEL.";
                }
                else
                {
                    if (ArchivoExcel.objObjeto.Count > 0)
                    {
                        foreach (ArchivoExcel item in ArchivoExcel.objObjeto)
                        {
                            if ((bool)item.Descarga)
                            {
                                contador += 1;

                                if (contador > 1)
                                {
                                    Mensaje += ", " + item.IdOperador;
                                }
                                else
                                {
                                    Mensaje += item.IdOperador;
                                }
                            }
                        }
                    }
                }

                respuesta.strMensaje = Mensaje;
            }

            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = Mensajes.RegistroNoSePuedeEliminar;
                respuesta.toError(msj, solicitudActual.objObjeto);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }
        public Respuesta<SolicitudIndicador> gEditarFormularioWeb(Guid idSolicitud, string[] Operadores, bool Completa, bool editarOperadores)
        {
            Respuesta<SolicitudIndicador> solicitudActual = new Respuesta<SolicitudIndicador>();
            Respuesta<SolicitudIndicador> respuesta = new Respuesta<SolicitudIndicador>();
            string Mensaje = "";

            try
            {
                respuesta = solicitudAD.gEditarFormularioWeb(idSolicitud, Operadores, Completa, editarOperadores,Completa);

                if (respuesta.strMensaje == "Dependencias")
                {
                    respuesta.objObjeto = new SolicitudIndicador();
                    respuesta.blnIndicadorTransaccion = false;
                    Mensaje = "<span>No se puede actualizar, pues el o los operadores:</span><br/><br/>" + respuesta.strData + " <br/><span>ya cargaron la información.</span>";
                }
                else
                {
                    Mensaje = Mensaje + respuesta.strMensaje;
                }

                respuesta.strMensaje = Mensaje;
            }

            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = Mensajes.RegistroNoSePuedeEditar;
                respuesta.toError(msj, solicitudActual.objObjeto);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }
        /// <summary>
        /// Actualiza el estado de la solicitud
        /// </summary>
        /// <param name="poIdSolicitudIndicador"></param>
        /// <param name="idOperador"></param>
        /// <param name="piEstadoSolicitud"></param>
        /// <returns></returns>
        public Respuesta<bool> gActualizarEstadoSolicitud(Guid poIdSolicitudIndicador, String idOperador, int piEstadoSolicitud)
        {
            Respuesta<bool> respuesta = new Respuesta<bool>();
            try
            {
                respuesta.objObjeto = solicitudAD.gActualizarEstadoSolicitud(poIdSolicitudIndicador, idOperador, piEstadoSolicitud);
                respuesta.blnIndicadorTransaccion = false;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = Mensajes.RegistroNoSePuedeEliminar;
                respuesta.toError(msj, false);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }


        #region SolicituConstructor
        public Respuesta<List<pa_getListaIndicadoresSolicitud_Result>> gConsultarIndicadores(int poIdDireccion, int poIdFrecuencia)
        {

            Respuesta<List<pa_getListaIndicadoresSolicitud_Result>> respuesta = new Respuesta<List<pa_getListaIndicadoresSolicitud_Result>>();

            try
            {  // here
                respuesta = solicitudAD.gConsultarIndicadores(poIdDireccion, poIdFrecuencia);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        public Respuesta<List<pa_getListaIndicadoresSolicitud_Result>> gConsultarIndicadoresPorDirFrecServ(int poIdDireccion, int poIdFrecuencia, int idServicio)
        {

            Respuesta<List<pa_getListaIndicadoresSolicitud_Result>> respuesta = new Respuesta<List<pa_getListaIndicadoresSolicitud_Result>>();

            try
            {  // here
                respuesta = solicitudAD.gConsultarIndicadoresPorDirFrecServ(poIdDireccion, poIdFrecuencia, idServicio);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }


        public Respuesta<List<pa_getListaIndicadoresSolicitud_Result>> gConsultarIndicadores(string poIdOperador, int poIdDireccion, int poIdFrecuencia)
        {
            Respuesta<List<pa_getListaIndicadoresSolicitud_Result>> respuesta = new Respuesta<List<pa_getListaIndicadoresSolicitud_Result>>();

            try
            { //here
                respuesta = solicitudAD.gConsultarIndicadores(poIdOperador, poIdDireccion, poIdFrecuencia);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        public Respuesta<List<pa_getListaIndicadoresSolicitud_Result>> gFiltrarTodosIndicadores(int poIdDireccion, int poIdFrecuencia, string poDescripcion)
        {
            Respuesta<List<pa_getListaIndicadoresSolicitud_Result>> respuesta = new Respuesta<List<pa_getListaIndicadoresSolicitud_Result>>();

            try
            {
                // here
                respuesta = solicitudAD.gFiltrarTodosIndicadores(poIdDireccion, poIdFrecuencia, poDescripcion);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        public Respuesta<List<pa_getListaIndicadoresSolicitud_Result>> gFiltrarIndicadoresXOperador(string poIdOperador, int poIdDireccion, int poIdFrecuencia, string poDescripcion)
        {
            Respuesta<List<pa_getListaIndicadoresSolicitud_Result>> respuesta = new Respuesta<List<pa_getListaIndicadoresSolicitud_Result>>();

            try
            {
                // here
                respuesta = solicitudAD.gFiltrarIndicadoresXOperador(poIdOperador, poIdDireccion, poIdFrecuencia, poDescripcion);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        public Respuesta<List<pa_getListaIndicadoresXSolicitud_Result>> gFiltrarIndicadoresXSolicitud(string poIdOperador, string poIndicador)
        {
            Respuesta<List<pa_getListaIndicadoresXSolicitud_Result>> respuesta = new Respuesta<List<pa_getListaIndicadoresXSolicitud_Result>>();

            try
            {

                respuesta = solicitudAD.gFiltrarIndicadoresXSolicitud(poIdOperador, Guid.Parse(poIndicador));
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        /*metodos Nuevos*/

        /// <summary>
        /// Id solicitud 
        /// </summary>
        /// <param name="poIndicador"></param>
        /// <returns></returns>
        public Respuesta<List<string>> gFiltrarOperadoresXSolicitud(Guid poIndicador)
        {
            Respuesta<List<string>> respuesta = new Respuesta<List<string>>();

            try
            {

                respuesta = solicitudAD.gFiltrarOperadoresXSolicitud(poIndicador);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="poSolicitud"></param>
        /// <returns></returns>
        public Respuesta<List<string>> gFiltrarSoloIndicadoresXSolicitud(Guid poIndicador)
        {
            Respuesta<List<string>> respuesta = new Respuesta<List<string>>();

            try
            {

                respuesta = solicitudAD.gFiltrarsoloIndicadoresXSolicitud(poIndicador);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }



        public Respuesta<SolicitudConstructor> AgregarSolicitudConstructor(SolicitudConstructor poSolicitud)
        {
            Respuesta<SolicitudConstructor> respuesta = new Respuesta<SolicitudConstructor>();


            try
            {
                if (solicitudAD.gVerificarExistenciaSolicitudConstructor(poSolicitud).objObjeto == null)
                {
                    respuesta = solicitudAD.gAgregarSolicitudConstructor(poSolicitud);
                    if (respuesta.blnIndicadorTransaccion)
                    {
                        respuesta.strMensaje = Mensajes.ExitoInsertar;
                    }
                }
                else
                {
                    respuesta.blnIndicadorTransaccion = false;
                    respuesta.strMensaje = Mensajes.RegistroDuplicado;
                }
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, poSolicitud);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }





        public Respuesta<SolicitudConstructor> EliminarSolicitudConstructor(Guid poIdSolicitudConstructor)
        {
            Respuesta<SolicitudConstructor> respuesta = new Respuesta<SolicitudConstructor>();

            try
            {
                respuesta = solicitudAD.gEliminarSolicitudConstructor(poIdSolicitudConstructor);

                if (respuesta.blnIndicadorTransaccion)
                {
                    respuesta.strMensaje = Mensajes.ExitoEliminar;
                }
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        public Respuesta<SolicitudConstructor> EliminarSolicitudConstructor(Guid poIdSolicitudIndicador, Guid poIdConstructor, string poOperador)
        {
            Respuesta<SolicitudConstructor> respuesta = new Respuesta<SolicitudConstructor>();

            try
            {
                respuesta = solicitudAD.gEliminarSolicitudConstructor(poIdSolicitudIndicador, poIdConstructor, poOperador);

                if (respuesta.blnIndicadorTransaccion)
                {
                    respuesta.strMensaje = Mensajes.ExitoEliminar;
                }
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        public Respuesta<bool> EliminarSolicitudConstructor(Guid poIdSolicitudIndicador, string poOperador)
        {
            Respuesta<bool> respuesta = new Respuesta<bool>();

            try
            {
                respuesta = solicitudAD.gEliminarSolicitudConstructor(poIdSolicitudIndicador, poOperador);

                if (respuesta.blnIndicadorTransaccion)
                {
                    respuesta.strMensaje = Mensajes.ExitoEliminar;
                }
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.blnIndicadorTransaccion = false;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        public Respuesta<bool> gNotificar(Guid poIdSolicitud, string poHtml, string poAsunto)
        {
            Respuesta<bool> respuesta = new Respuesta<bool>();

            try
            {
                respuesta = solicitudAD.gNotificar(poIdSolicitud, poHtml, poAsunto);

                if (respuesta.blnIndicadorTransaccion)
                {
                    respuesta.strMensaje = Mensajes.Notificacion;
                }
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, false);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        public Respuesta<bool> gConfirmacion(string poIdSolicitud, string poHtml, string poAsunto, int poDireccion)
        {
            Respuesta<bool> respuesta = new Respuesta<bool>();

            try
            {
                respuesta = solicitudAD.gEnviarCorreo(poIdSolicitud, poHtml, poAsunto, poDireccion);

                if (respuesta.blnIndicadorTransaccion)
                {
                    respuesta.strMensaje = Mensajes.Notificacion;
                }
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, false);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        public string[] gOperadoresAsociado(string op)
        {
            try
            {
                string[] List = solicitudAD.obtenerOperadoresAsociados(op);

                if (List != null)
                {
                    return List;
                }
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return null;
        }

        public string[] gOperadoresNombres(string[] op)
        {
            try
            {
                string[] List = solicitudAD.obtenerNombreDeOperadores(op);

                if (List != null)
                {
                    return List;
                }
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return null;
        }


        #endregion


        #endregion

        #region ListarSolicitudesParaOperadores
        /// <summary>
        /// Lista todas las solicitudes 
        /// </summary>
        /// <returns></returns>
        public Respuesta<List<SolicitudIndicador>> ListarSolicitudesParaOperadores(Operador objOperador)
        {
            Respuesta<List<SolicitudIndicador>> respuesta = new Respuesta<List<SolicitudIndicador>>();

            try
            {
                respuesta = solicitudAD.ListarSolicitudesParaOperadores(objOperador);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }


        public Respuesta<List<SolicitudIndicador>> ListarSolicitudesParaOperadoresWeb(Operador objOperador)
        {
            Respuesta<List<SolicitudIndicador>> respuesta = new Respuesta<List<SolicitudIndicador>>();

            try
            {
                respuesta = solicitudAD.ListarSolicitudesParaOperadoresWEB(objOperador);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }
        #endregion
        ////

        /////
        public Respuesta<List<ArchivoExcel>> ListarDescargaSolicitud(Operador objOperador)
        {
            Respuesta<List<ArchivoExcel>> respuesta = new Respuesta<List<ArchivoExcel>>();

            try
            {
                respuesta = solicitudAD.ListarestadoSolicitudesParaOperadores(objOperador);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }
        public Respuesta<List<SolicitudConstructor>> ListarDescargaSolicitudWeb(Operador objOperador)
        {
            Respuesta<List<SolicitudConstructor>> respuesta = new Respuesta<List<SolicitudConstructor>>();

            try
            {
                respuesta = solicitudAD.ListarestadoSolicitudesParaOperadoresWeb(objOperador);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }
        #region modificacionIndicador

        /// <summary>
        /// Filtro de solicitudes
        /// </summary>
        /// <param name="poIDOperador"></param>
        /// <param name="poIDServicio"></param>
        /// <param name="poIdDireccion"></param>
        /// <param name="poIDIndicador"></param>
        /// <param name="poIdCriterio"></param>
        /// <param name="poIdFrecuencia"></param>
        /// <param name="poIdDesglose"></param>
        /// <param name="poFechaInicial"></param>
        /// <param name="poFechaFinal"></param>
        /// <returns></returns>
        public Respuesta<List<SolicitudIndicador>> gSolicitudModificarIndicador(string poIDOperador, int poIDServicio, int poIdDireccion, string poIDIndicador
             , string poIdCriterio, int poIdFrecuencia, int poIdDesglose, DateTime poFechaInicial, DateTime poFechaFinal)
        {
            Respuesta<List<SolicitudIndicador>> respuesta = new Respuesta<List<SolicitudIndicador>>();

            try
            {
                respuesta = gValidarDatosFiltrarSolicitudes(poIDOperador, poIDServicio, poIdDireccion, poIDIndicador, poIdCriterio, poIdFrecuencia, poIdDesglose);
                if (respuesta.blnIndicadorTransaccion == true)
                {
                    respuesta = solicitudAD.gSolicitudModificarIndicador(poIDOperador, poIDServicio, poIdDireccion, poIDIndicador, poIdCriterio, poIdFrecuencia, poIdDesglose, poFechaInicial, poFechaFinal);
                }
            }

            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }


        public Respuesta<List<SolicitudIndicador>> gValidarDatosFiltrarSolicitudes(string poIDOperador, int poIDServicio, int poIdDireccion, string poIDIndicador
           , string poIdCriterio, int poIdFrecuencia, int poIdDesglose)
        {
            Respuesta<List<SolicitudIndicador>> respuesta = new Respuesta<List<SolicitudIndicador>>();
            if (poIDOperador == null || poIDOperador.Equals(""))
            {
                respuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.ModificarIndicador_idOperador;
                respuesta.blnIndicadorTransaccion = false;
                return respuesta;
            }
            else if (poIDServicio == null || poIDServicio.Equals(0))
            {
                respuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.ModificarIndicador_idServicio;
                respuesta.blnIndicadorTransaccion = false;
                return respuesta;
            }
            else if (poIdDireccion == null || poIdDireccion.Equals(0))
            {
                respuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.ModificarIndicador_idDireccion;
                respuesta.blnIndicadorTransaccion = false;
                return respuesta;
            }
            else if (poIDIndicador == null || poIDIndicador.Equals(""))
            {
                respuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.ModificarIndicador_idIndicador;
                respuesta.blnIndicadorTransaccion = false;
                return respuesta;
            }
            else if (poIdCriterio == null || poIdCriterio.Equals(""))
            {
                respuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.ModificarIndicador_idCriterio;
                respuesta.blnIndicadorTransaccion = false;
                return respuesta;
            }
            else if (poIdFrecuencia == null || poIdFrecuencia.Equals(""))
            {
                respuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.ModificarIndicador_idFrecuencia;
                respuesta.blnIndicadorTransaccion = false;
                return respuesta;
            }
            else if (poIdDesglose == null || poIdDesglose.Equals(""))
            {
                respuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.ModificarIndicador_idDesglose;
                respuesta.blnIndicadorTransaccion = false;
                return respuesta;
            }
            return respuesta;

        }

        /// <summary>
        /// Obtiene la solicitud de constructor
        /// </summary>
        /// <param name="poIdOperador"></param>
        /// <param name="poIdSolicitudIndicador"></param>
        /// <param name="poIdDesglose"></param>
        /// <param name="poIdIndicador"></param>
        /// <returns></returns>
        public Respuesta<SolicitudConstructor> gObtenerSolicitudConstructor(String poIdOperador, Guid poIdSolicitudIndicador, int poIdDesglose, string poIdIndicador)
        {
            Respuesta<SolicitudConstructor> respuesta = new Respuesta<SolicitudConstructor>();

            try
            {

                respuesta = solicitudAD.gObtenerSolicitudConstructor(poIdOperador, poIdSolicitudIndicador, poIdDesglose, poIdIndicador);
            }

            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;

        }
        #endregion

        #region VerificarRelacionOperadorConstructorCriterio
        public Respuesta<SolicitudIndicador> VerificarRelacionOperadorConstructor(string idOperador, string idConstructor)
        {
            Respuesta<SolicitudIndicador> respuesta = new Respuesta<SolicitudIndicador>();

            try
            {
                respuesta.blnIndicadorTransaccion = solicitudAD.gVerificarRelacionOperadorConstructorCriterio(idOperador, idConstructor);

                return respuesta;

            }
            catch (Exception ex)
            {
                return respuesta;
            }


        }

        #endregion

        #region gNombreIndicador
        public Respuesta<string> gNombreIndicador(string idConstructor)
        {
            Respuesta<string> respuesta = new Respuesta<string>();

            try
            {
                respuesta = solicitudAD.gNombreIndicador(idConstructor);

                return respuesta;

            }
            catch (Exception ex)
            {
                return respuesta;
            }


        }

        #endregion


        #region gNombreOperador
        public Respuesta<string> gNombreOperador(string idOperador)
        {
            Respuesta<string> respuesta = new Respuesta<string>();

            try
            {
                respuesta = solicitudAD.gNombreOperador(idOperador);

                return respuesta;

            }
            catch (Exception ex)
            {

                respuesta.blnIndicadorTransaccion = false;
                respuesta.strMensaje = ex.Message;
                return respuesta;
            }


        }

        #endregion


        public void InsertarArchivoExcel(Guid idSolicitud, string idOperador)
        {
            solicitudAD.InsertarArchivoExcel(idSolicitud, idOperador);

        }
    }
}
