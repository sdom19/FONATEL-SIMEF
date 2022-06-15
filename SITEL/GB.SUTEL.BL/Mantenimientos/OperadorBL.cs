using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SUTEL.Entities;
using GB.SUTEL.DAL.Mantenimientos;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.Shared;
using GB.SUTEL.ExceptionHandler;

namespace GB.SUTEL.BL.Mantenimientos
{
    public class OperadorBL : LocalContextualizer
    {
        #region Atributos
        /// <summary>
        /// objeto global de Usuario en la capa de acceso a datos
        /// </summary>
        private OperadorDA objOperadorDA;

        #endregion

        #region Constructores
        public OperadorBL(ApplicationContext appContext)
            : base(appContext)
        {
            objOperadorDA = new OperadorDA(appContext);
        }
        #endregion

        #region ConsultarTodos
        /// <summary>
        /// Consultar Todos
        /// </summary>
        /// <param name="objOperador"></param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<List<Operador>> ConsultarTodos()
        {
            Respuesta<List<Operador>> objRespuesta = new Respuesta<List<Operador>>();
            try
            {
                objRespuesta = objOperadorDA.ConsultarTodos();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }

        public Respuesta<List<Operador>> ConsultarTodosParaDetalleAgrupacion()
        {
            Respuesta<List<Operador>> objRespuesta = new Respuesta<List<Operador>>();
            try
            {
                objRespuesta = objOperadorDA.ConsultarTodosParaDetalleAgrupacion();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }

        public Respuesta<List<Operador>> ConsultarXServicio(int poIdServicio)
        {
            Respuesta<List<Operador>> objRespuesta = new Respuesta<List<Operador>>();
            try
            {
                objRespuesta = objOperadorDA.gConsultarPorServicio(poIdServicio);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }

        public Respuesta<List<Operador>> ConsultarXSolicitud(Guid PSolicitud)
        {
            Respuesta<List<Operador>> objRespuesta = new Respuesta<List<Operador>>();
            try
            {
                objRespuesta = objOperadorDA.gConsultarPorSolicitud(PSolicitud);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }

        public Respuesta<bool?> ConsultarFormularioWebXSolicitud(Guid PSolicitud)
        {
            Respuesta<bool?> objRespuesta = new Respuesta<bool?>();
            objRespuesta.objObjeto = false;
            try
            {
                // objRespuesta = objOperadorDA.gConsultarPorSolicitud(PSolicitud);
                 objRespuesta = objOperadorDA.gConsultarFormularioWebPorSolicitud(PSolicitud);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, false);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }


        /// <summary>
        /// Filtra los operadores
        /// </summary>
        /// <param name="piIdOperador"></param>
        /// <param name="psNombreOperador"></param>
        /// <returns></returns>
        public Respuesta<List<Operador>> gFiltrarOperadores(String psIdOperador, String psNombreOperador)
        {
            Respuesta<List<Operador>> objRespuesta = new Respuesta<List<Operador>>();
            List<Operador> aux = new List<Operador>();
            try
            {
                objRespuesta = objOperadorDA.gFiltrarOperadores(psIdOperador, psNombreOperador);
                
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }


        /// <summary>
        /// Inserta una serie de servicios a un operador
        /// </summary>
        /// <param name="poServicios"></param>
        /// <returns></returns>
        public Respuesta<ServicioOperador> AgregarServicio(string poIdOperador,params  int[] poServicios)
        {
            Respuesta<ServicioOperador> objRespuesta = new Respuesta<ServicioOperador>();
            Operador operadorProcesar = new Operador();
            operadorProcesar.IdOperador = poIdOperador;

            this.objOperadorDA = new OperadorDA(AppContext, operadorProcesar);

            try
            {
                foreach (int item in poServicios)
                {
                    objOperadorDA.InsertarServicio(item);
                }
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }



        /// <summary>
        /// Elimina servicios de un operador
        /// </summary>
        /// <param name="poIdOperador">Operador</param>
        /// <param name="poServicios">Lista de servicios</param>
        /// <returns></returns>
        public Respuesta<ServicioOperador> EliminarServicio(string poIdOperador, params  int[] poServicios)
        {
            Respuesta<ServicioOperador> objRespuesta = new Respuesta<ServicioOperador>();
            Operador operadorProcesar = new Operador();
            operadorProcesar.IdOperador = poIdOperador;

            this.objOperadorDA = new OperadorDA(AppContext, operadorProcesar);

            try
            {
                foreach (int item in poServicios)
                {
                    objOperadorDA.EliminarServicio(item);
                }
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }

        #endregion


        #region Verificar
        public Respuesta<ServicioOperador> Verificar(string strOperador, List<int> lstServicios)
        {
            Respuesta<ServicioOperador> objRespuesta = new Respuesta<ServicioOperador>();
            try
            {
                objRespuesta = objOperadorDA.VerificarServicio(strOperador, lstServicios);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        #endregion

    }
}
