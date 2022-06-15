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
    public class TipoIndicadorBL : LocalContextualizer
    {

        public TipoIndicadorBL(ApplicationContext appContext)
            : base(appContext) {
                objTipoIndicadorAD = new TipoIndicadorAD(appContext);
        }

        #region Atributos
        /// <summary>
        /// objeto global de Tipo Indicador en la capa de acceso a datos
        /// </summary>
        private TipoIndicadorAD objTipoIndicadorAD;

        #endregion

        #region Agregar
        /// <summary>
        /// Método para agregar un Tipo Indicador
        /// </summary>
        /// <param name="objTipoIndicador">Objeto a agregar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<TipoIndicador> Agregar(TipoIndicador objTipoIndicador)
        {
            Respuesta<TipoIndicador> objRespuesta = new Respuesta<TipoIndicador>();
            try
            {
                if (objTipoIndicadorAD.ConsultaPorNombre(objTipoIndicador).objObjeto == null)
                {
                    objRespuesta = objTipoIndicadorAD.Agregar(objTipoIndicador);
                }
                else
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                }
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objTipoIndicador);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        #endregion

        #region Editar
        /// <summary>
        /// Método para Editar un Tipo Indicador
        /// </summary>
        /// <param name="objTipoIndicador">Objeto a Editar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<TipoIndicador> Editar(TipoIndicador objTipoIndicador)
        {
            Respuesta<TipoIndicador> objRespuesta = new Respuesta<TipoIndicador>();
            try
            {
                if (objTipoIndicadorAD.ConsultaPorNombre(objTipoIndicador).objObjeto == null)
                {
                    objRespuesta = objTipoIndicadorAD.Editar(objTipoIndicador);
                }
                else
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                }
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objTipoIndicador);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        #endregion

        #region Eliminar
        /// <summary>
        /// Método para Eliminar un Tipo Indicador
        /// </summary>
        /// <param name="objTipoIndicador">Objeto a Eliminar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<TipoIndicador> Eliminar(TipoIndicador objTipoIndicador)
        {
            Respuesta<TipoIndicador> objRespuesta = new Respuesta<TipoIndicador>();
            try
            {
                if (objTipoIndicadorAD.ConsultaPorDependenciaEnIndicador(objTipoIndicador).objObjeto != null)
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.strMensaje = "Existe una dependencia con algún indicador.";

                    return objRespuesta;
                }

                if (objTipoIndicadorAD.ConsultaPorDependenciaEnServicio(objTipoIndicador).objObjeto != null)
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.strMensaje = "Existe una dependencia con algún servicio.";

                    return objRespuesta;
                }

                objTipoIndicador = objTipoIndicadorAD.ConsultaPorID(objTipoIndicador).objObjeto;
                if (objTipoIndicador != null)
                {
                    objRespuesta = objTipoIndicadorAD.Eliminar(objTipoIndicador);
                }
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objTipoIndicador);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        #endregion

        #region ConsultarTodos
        /// <summary>
        /// Consultar Todos
        /// </summary>
        /// <param name="objTipoIndicador"></param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<List<TipoIndicador>> ConsultarTodos()
        {
            Respuesta<List<TipoIndicador>> objRespuesta = new Respuesta<List<TipoIndicador>>();
            try
            {
                objRespuesta = objTipoIndicadorAD.ConsultarTodos();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objRespuesta.objObjeto);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }

         /// <summary>
        /// Filtro de tipos de indicador
        /// </summary>
        /// <param name="piIDTipoIndicador"></param>
        /// <param name="psNombreTipoIndicador"></param>
        /// <returns></returns>
        public Respuesta<List<TipoIndicador>> gFiltarTiposIndicador(int piIDTipoIndicador, String psNombreTipoIndicador)
        {
            Respuesta<List<TipoIndicador>> objRespuesta = new Respuesta<List<TipoIndicador>>();
            try
            {
                objRespuesta = objTipoIndicadorAD.gFiltarTiposIndicador(piIDTipoIndicador, psNombreTipoIndicador);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objRespuesta.objObjeto);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }

        #endregion
    }
}
