using GB.SUTEL.DAL.Mantenimientos;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.Shared;

namespace GB.SUTEL.BL.Mantenimientos
{
    public class AgrupacionBL : LocalContextualizer
    {
        #region atributos
        AgrupacionAD refAgrupacionAD ;
        AgrupacionDetalleAD refAgrupacionDetalleAD;
        #endregion

        #region metodos

        public AgrupacionBL(ApplicationContext appContext)
            : base(appContext)
        {
            refAgrupacionAD = new AgrupacionAD(appContext);
            refAgrupacionDetalleAD = new AgrupacionDetalleAD();
        }

        /// <summary>
        /// Agregar agrupación
        /// </summary>
        /// <param name="poAgrupacion"></param>
        /// <returns></returns>
        public Respuesta<Agrupacion> gAgregarAgrupacion(Agrupacion poAgrupacion)
        {
            Respuesta<Agrupacion> objRespuesta = new Respuesta<Agrupacion>();
            Respuesta<Agrupacion> objRespuestaExistencia = new Respuesta<Agrupacion>();
            try
            {
                object aux = (object)poAgrupacion;
                Utilidades.LimpiarEspacios(ref aux);
                objRespuestaExistencia = refAgrupacionAD.gObtenerAgrupacionNombre(poAgrupacion.DescAgrupacion);
                if (objRespuestaExistencia.blnIndicadorTransaccion == false && objRespuestaExistencia.objObjeto == null)
                {
                    objRespuesta = refAgrupacionAD.gAgregarAgrupacion(poAgrupacion);
                }
                else
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.objObjeto = poAgrupacion;
                    objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.RegistroDuplicado;
                }
                
            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(ex.Message, poAgrupacion);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
            return objRespuesta;
        }

        /// <summary>
        /// Edita una agrupación
        /// </summary>
        /// <param name="poAgrupacion"></param>
        /// <returns></returns>
        public Respuesta<Agrupacion> gEditarAgrupacion(Agrupacion poAgrupacion)
        {
            Respuesta<Agrupacion> objRespuesta = new Respuesta<Agrupacion>();
            Respuesta<Agrupacion> objRespuestaExistencia = new Respuesta<Agrupacion>();
            try
            {
                object aux = (object)poAgrupacion;
                Utilidades.LimpiarEspacios(ref aux);
                objRespuestaExistencia = refAgrupacionAD.gObtenerAgrupacionNombre(poAgrupacion.DescAgrupacion);
                if (objRespuestaExistencia.blnIndicadorTransaccion == false && objRespuestaExistencia.objObjeto == null)
                {
                    objRespuesta = refAgrupacionAD.gEditarAgrupacion(poAgrupacion);
                }
                else if (objRespuestaExistencia.objObjeto.IdAgrupacion.Equals(poAgrupacion.IdAgrupacion))
                {
                    objRespuesta = refAgrupacionAD.gEditarAgrupacion(poAgrupacion);
                }
                else {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.objObjeto = poAgrupacion;
                    objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.RegistroDuplicado;
                }
            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(ex.Message, poAgrupacion);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
            return objRespuesta;
        }

        /// <summary>
        /// Elimina logicamente la agrupacion
        /// </summary>
        /// <param name="poAgrupacion"></param>
        /// <returns></returns>
        public Respuesta<Agrupacion> gEliminarAgrupacion(Agrupacion poAgrupacion)
        {
            Respuesta<Agrupacion> objRespuesta = new Respuesta<Agrupacion>();
            Respuesta<List<DetalleAgrupacion>> objRespuestaDetalleAgrupacion = new Respuesta<List<DetalleAgrupacion>>();
            try
            {
                objRespuestaDetalleAgrupacion = refAgrupacionDetalleAD.gObtenerAgrupacionDetallePorAgrupacion(poAgrupacion.IdAgrupacion);
                if (objRespuestaDetalleAgrupacion.objObjeto == null || objRespuestaDetalleAgrupacion.objObjeto.Count==0)
                { // no tiene detallles agrupación asociadas.
                    objRespuesta = refAgrupacionAD.gEliminarAgrupacion(poAgrupacion);
                }
                else {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.Agrupacion_ConDetalle;
                }
            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
             
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
            return objRespuesta;
        }

        /// <summary>
        /// Obtiene las agrupaciones
        /// </summary>
        /// <returns></returns>
        public Respuesta<List<Agrupacion>> gObtenerAgrupaciones()
        {

            Respuesta<List<Agrupacion>> objRespuesta = new Respuesta<List<Agrupacion>>();
            try
            {
                objRespuesta = refAgrupacionAD.gObtenerAgrupaciones();
            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
             
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
            return objRespuesta;
        }

        /// <summary>
        /// Obtiene las agrupaciones
        /// </summary>
        /// <returns></returns>
        public Respuesta<List<Agrupacion>> gObtenerAgrupacionesPorFiltros(int piCodigo, String nombre)
        {
            Respuesta<List<Agrupacion>> objRespuesta = new Respuesta<List<Agrupacion>>();
            try
            {
                objRespuesta = refAgrupacionAD.gObtenerAgrupacionesPorFiltros(piCodigo, nombre);
            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;

                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
            return objRespuesta;
        }

        /// <summary>
        /// Obtiene las agrupaciones
        /// </summary>
        /// <returns></returns>
        public Respuesta<Agrupacion> gObtenerAgrupacion(int piIdAgrupacion)
        {
            Respuesta<Agrupacion> objRespuesta = new Respuesta<Agrupacion>();
            try
            {
                objRespuesta = refAgrupacionAD.gObtenerAgrupacion(piIdAgrupacion);
            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
            return objRespuesta;
        }

        /// <summary>
        /// Obtiene las agrupaciones por la descripción
        /// </summary>
        /// <returns></returns>
        public Respuesta<Agrupacion> gObtenerAgrupacionNombre(String psDescripcionAgrupacion)
        {
            Respuesta<Agrupacion> objRespuesta = new Respuesta<Agrupacion>();
            try
            {
                objRespuesta = refAgrupacionAD.gObtenerAgrupacionNombre(psDescripcionAgrupacion);
            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
               
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
            return objRespuesta;
        }

        #endregion
    }
}
