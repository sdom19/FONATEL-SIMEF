using System.Threading;
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
    public class IndicadorUITBL : LocalContextualizer
    {

        #region atributos
        IndicadorUITAD refIndicadorUITAD ;
        private IndicadorBL refIndicadorBL;
        #endregion
        #region metodos

        public IndicadorUITBL(ApplicationContext appContext)
            : base(appContext)
        {
            refIndicadorUITAD = new IndicadorUITAD(appContext);
            refIndicadorBL = new IndicadorBL(appContext);
        } 

        /// <summary>
        /// Agregar IndicadorUIT
        /// </summary>
        /// <param name="poIndicadorUIT"></param>
        /// <returns></returns>
        public Respuesta<IndicadorUIT> gAgregarIndicadorUIT(IndicadorUIT poIndicadorUIT)
        {
            Respuesta<IndicadorUIT> objRespuesta = new Respuesta<IndicadorUIT>();
            Respuesta<IndicadorUIT> objRespuestaExistencia = new Respuesta<IndicadorUIT>();
            try
            {
                object aux = (object)poIndicadorUIT;
                Utilidades.LimpiarEspacios(ref aux);
                objRespuestaExistencia = refIndicadorUITAD.gObtenerIndicadorUITNombre(poIndicadorUIT.DescIndicadorUIT);
                if (objRespuestaExistencia.blnIndicadorTransaccion == false && objRespuestaExistencia.objObjeto == null)
                {
                    objRespuesta = refIndicadorUITAD.gAgregarIndicadorUIT(poIndicadorUIT);
                }
                else
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.objObjeto = poIndicadorUIT;
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
                objRespuesta.toError(ex.Message, poIndicadorUIT);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
            return objRespuesta;
        }

        /// <summary>
        /// Edita una IndicadorUIT
        /// </summary>
        /// <param name="poIndicadorUIT"></param>
        /// <returns></returns>
        public Respuesta<IndicadorUIT> gEditarIndicadorUIT(IndicadorUIT poIndicadorUIT)
        {
            Respuesta<IndicadorUIT> objRespuesta = new Respuesta<IndicadorUIT>();
            Respuesta<IndicadorUIT> objRespuestaExistencia = new Respuesta<IndicadorUIT>();
            try
            {
                object aux = (object)poIndicadorUIT;
                Utilidades.LimpiarEspacios(ref aux);
                objRespuestaExistencia = refIndicadorUITAD.gObtenerIndicadorUITNombre(poIndicadorUIT.DescIndicadorUIT);
                if ( objRespuestaExistencia.objObjeto == null)
                {
                    objRespuesta = refIndicadorUITAD.gEditarIndicadorUIT(poIndicadorUIT);
                }
                else if (objRespuestaExistencia.objObjeto.IdIndicadorUIT.Equals(poIndicadorUIT.IdIndicadorUIT))
                {
                    objRespuesta = refIndicadorUITAD.gEditarIndicadorUIT(poIndicadorUIT);
                }
                else
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.objObjeto = poIndicadorUIT;
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
                objRespuesta.toError(ex.Message, poIndicadorUIT);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
            return objRespuesta;
        }

        /// <summary>
        /// Elimina logicamente la IndicadorUIT
        /// </summary>
        /// <param name="poIndicadorUIT"></param>
        /// <returns></returns>
        public Respuesta<IndicadorUIT> gEliminarIndicadorUIT(IndicadorUIT poIndicadorUIT)
        {
            Respuesta<IndicadorUIT> objRespuesta = new Respuesta<IndicadorUIT>();
            Respuesta<List<Indicador>> objRespuestaIndicador = new Respuesta<List<Indicador>>();
            try
            {
                objRespuestaIndicador = refIndicadorBL.gObtenerIndicadorPorIndicadorUIT(poIndicadorUIT.IdIndicadorUIT);
                if (objRespuestaIndicador.objObjeto == null || objRespuestaIndicador.objObjeto.Count==0)
                { // no tiene detallles IndicadorUIT asociadas.
                    objRespuesta = refIndicadorUITAD.gEliminarIndicadorUIT(poIndicadorUIT);
                }
                else
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.IndicadorUIT_ConIndicadores;
                }
            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(ex.Message, poIndicadorUIT);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
            return objRespuesta;
        }

        /// <summary>
        /// Obtiene las indicadores uit
        /// </summary>
        /// <returns></returns>
        public Respuesta<List<IndicadorUIT>> gObtenerIndicadoresUIT()
        {

            Respuesta<List<IndicadorUIT>> objRespuesta = new Respuesta<List<IndicadorUIT>>();
            try
            {
                objRespuesta = refIndicadorUITAD.gObtenerIndicadoresUIT();
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
        /// Obtiene los indicadores uit
        /// </summary>
        /// <returns></returns>
        public Respuesta<IndicadorUIT> gObtenerIndicadorUIT(int piIdIndicador)
        {
            Respuesta<IndicadorUIT> objRespuesta = new Respuesta<IndicadorUIT>();
            try
            {
                objRespuesta = refIndicadorUITAD.gObtenerIndicadorUIT(piIdIndicador);
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
        /// Obtiene los indicadores uit por la descripción
        /// </summary>
        /// <returns></returns>
        public Respuesta<IndicadorUIT> gObtenerIndicadorUITNombre(String psDescripcionIndicador)
        {
            Respuesta<IndicadorUIT> objRespuesta = new Respuesta<IndicadorUIT>();
            try
            {
                objRespuesta = refIndicadorUITAD.gObtenerIndicadorUITNombre(psDescripcionIndicador);
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
        /// Obtiene los indidores uit filtrados
        /// </summary>
        /// <returns></returns>
        public Respuesta<List<IndicadorUIT>> gObtenerIndicadoresUITPorFiltros(int piCodigo, String nombre)
        {

            Respuesta<List<IndicadorUIT>> objRespuesta = new Respuesta<List<IndicadorUIT>>();
            try
            {
                objRespuesta = refIndicadorUITAD.gObtenerIndicadoresUITPorFiltros(piCodigo,nombre);
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
