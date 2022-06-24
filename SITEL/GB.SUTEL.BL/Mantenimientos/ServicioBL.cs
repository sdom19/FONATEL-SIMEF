using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// GB Classes
using GB.SUTEL.Entities;
using GB.SUTEL.Shared;
using GB.SUTEL.DAL;
using GB.SUTEL.DAL.Mantenimientos;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.Resources;

namespace GB.SUTEL.BL.Mantenimientos
{
    public class ServicioBL : LocalContextualizer
    {
        public ServicioBL(ApplicationContext appContext)
            : base(appContext)
        {
            objServicioAD = new ServicioAD(appContext);
        }

        #region Atributos
        /// <summary>
        /// objeto global de Servicio en la capa de acceso a datos
        /// </summary>
        private ServicioAD objServicioAD;

        #endregion

        #region Agregar
        public Respuesta<Servicio> Agregar(string objDesServicio)
        {
            Servicio oServicio = new Servicio();

            oServicio.DesServicio = objDesServicio;

            return this.Agregar(oServicio);
        }

        /// <summary>
        /// Método para agregar un Servicio
        /// </summary>
        /// <param name="objServicio">Objeto a agregar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Servicio> Agregar(Servicio objServicio)
        {
            Respuesta<Servicio> objRespuesta = new Respuesta<Servicio>();
            try
            {
                if (objServicioAD.ConsultaPorNombreServicio(objServicio).objObjeto == null)
                {
                    objRespuesta = objServicioAD.Agregar(objServicio);
                    objRespuesta.strMensaje = Mensajes.ExitoInsertar;
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
                objRespuesta.toError(msj, objServicio);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        #endregion

        #region Editar
        public Respuesta<Servicio> Editar(string objDesServicio)
        {
            Servicio oServicio = new Servicio();

            oServicio.DesServicio = objDesServicio;

            return this.Editar(oServicio);
        }


        /// <summary>
        /// Método para Editar un Servicio
        /// </summary>
        /// <param name="objServicio">Objeto a Editar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Servicio> Editar(Servicio objServicio)
        {
            Respuesta<Servicio> objRespuesta = new Respuesta<Servicio>();
            try
            {
                if (objServicioAD.ConsultaPorNombreServicio(objServicio).objObjeto == null)
                {
                    string nombreServicio = objServicio.DesServicio;
                    objServicio = objServicioAD.ConsultaPorID(objServicio).objObjeto;
                    objServicio.DesServicio = nombreServicio;
                    objRespuesta = objServicioAD.Editar(objServicio);
                    objRespuesta.objObjeto = objServicio = new Servicio();
                    objRespuesta.strMensaje = Mensajes.ExitoEditar;
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
                objRespuesta.toError(msj, objServicio);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        #endregion

        #region Eliminar
        /// <summary>
        /// Método para Eliminar un Servicio
        /// </summary>
        /// <param name="objServicio">Objeto a Eliminar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Servicio> Eliminar(Servicio objServicio)
        {
            Respuesta<Servicio> objRespuesta = new Respuesta<Servicio>();
            try
            {
                if (objServicioAD.ConsultaPorDependenciaEnSolicitudIndicador(objServicio).objObjeto != null)
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.strMensaje = "Existe una dependencia con alguna solicitud de indicador.";

                    return objRespuesta;
                }

                if (objServicioAD.ConsultaPorDependenciaEnOperador(objServicio).objObjeto != null)
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.strMensaje = "Existe una dependencia con algún operador.";

                    return objRespuesta;
                }

                if (objServicioAD.ConsultaPorDependenciaEnTipoIndicador(objServicio).objObjeto != null)
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.strMensaje = "Existe una dependencia con algún tipo de indicador";

                    return objRespuesta;
                }

                objServicio = objServicioAD.ConsultaPorID(objServicio).objObjeto;
                if (objServicio != null)
                {
                    objRespuesta = objServicioAD.Eliminar(objServicio);
                }
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objServicio);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        #endregion

        #region ConsultarPorOperador
        /// <summary>
        /// Consultar por operador
        /// </summary>
        /// <param name="objOperador"></param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<List<Servicio>> ConsultarPorOperador(Operador objOperador)
        {
            Respuesta<List<Servicio>> objRespuesta = new Respuesta<List<Servicio>>();
            try
            {
                objRespuesta = objServicioAD.ConsultaPorOperador(objOperador);
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

        #region ConsultarTodos
        /// <summary>
        /// Consultar Todos
        /// </summary>
        /// <param name="objServicio"></param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<List<Servicio>> ConsultarTodos()
        {
            Respuesta<List<Servicio>> objRespuesta = new Respuesta<List<Servicio>>();
            try
            {
                objRespuesta = objServicioAD.ConsultarTodos();
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
        /// Filtra los servicios
        /// </summary>
        /// <param name="piIDServicio"></param>
        /// <param name="psNombreServicio"></param>
        /// <returns></returns>
        public Respuesta<List<Servicio>> gFiltrarServicios(int piIDServicio, String psNombreServicio)
        {
            Respuesta<List<Servicio>> objRespuesta = new Respuesta<List<Servicio>>();
            try
            {
                objRespuesta = objServicioAD.gFiltrarServicios(piIDServicio, psNombreServicio);
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

        #region ConsultarPorOperador
        /// <summary>
        /// Consultar por ID
        /// </summary>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Servicio> ConsultaPorID(Servicio objServicio)
        {
            Respuesta<Servicio> objRespuesta = new Respuesta<Servicio>();
            try
            {
                objRespuesta = objServicioAD.ConsultaPorID(objServicio);
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

        #region TiposDeIndicadoresXServicio

        public Respuesta<TipoIndicadorServicio> EditarTipoIndicadorPorServicio(string poArregloIdTipoIndicador, int poIdServicio)
        {
            Respuesta<TipoIndicadorServicio> objRespuesta = new Respuesta<TipoIndicadorServicio>();
            TipoIndicadorServicio objAux = new TipoIndicadorServicio();


            try
            {
                if (objServicioAD.gEliminarTipoIndicadorPorServicio(poIdServicio).blnIndicadorTransaccion && !(string.IsNullOrEmpty(poArregloIdTipoIndicador)))
                {
                    this.AgregarTipoIndicador(poArregloIdTipoIndicador, poIdServicio);
                }
                else
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.strMensaje = Mensajes.RegistroNoSePuedeEliminar;
                }
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
        /// Recibe un string con una serie de identificadores de tipo indicador separados por una coma (,)
        /// </summary>
        /// <param name="poArregloIdTipoIndicador"></param>
        /// <returns></returns>
        public Respuesta<TipoIndicadorServicio> AgregarTipoIndicador(string poArregloIdTipoIndicador, int poIdServicio)
        {
            Respuesta<TipoIndicadorServicio> objRespuesta = new Respuesta<TipoIndicadorServicio>();
            TipoIndicadorServicio objAux = new TipoIndicadorServicio();

            string[] indicadoresAux = poArregloIdTipoIndicador.Split(',');
            try
            {
                foreach (string item in indicadoresAux)
                {
                    objAux.IdTipoInd = int.Parse(item);
                    objAux.IdServicio = poIdServicio;
                    objAux.Borrado = 0;
                    objRespuesta = this.AgregarTipoIndicador(objAux);
                    objAux = new TipoIndicadorServicio();
                }
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



        public Respuesta<TipoIndicadorServicio> AgregarTipoIndicador(TipoIndicadorServicio objTipoIndicadorServicio)
        {
            Respuesta<TipoIndicadorServicio> objRespuesta = new Respuesta<TipoIndicadorServicio>();
            try
            {

                objRespuesta = objServicioAD.gAgregarTipoIndicador(objTipoIndicadorServicio);

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objTipoIndicadorServicio);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }

        private Respuesta<TipoIndicadorServicio> EliminarTipoIndicador(TipoIndicadorServicio objTipoIndicadorServicio)
        {
            Respuesta<TipoIndicadorServicio> objRespuesta = new Respuesta<TipoIndicadorServicio>();
            try
            {

                objRespuesta = objServicioAD.gEliminarTipoIndicador(objTipoIndicadorServicio);

                

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objTipoIndicadorServicio);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }

        public Respuesta<List<TipoIndicadorServicio>> gObenerTipoIndicadoresPorServicio(int poIdServicio)
        {
            Respuesta<List<TipoIndicadorServicio>> objRespuesta = new Respuesta<List<TipoIndicadorServicio>>();
            try
            {

                objRespuesta = objServicioAD.gObenerTipoIndicadoresPorServicio(poIdServicio);


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
        /// Obtiene los tipos de indicador por un determinado servicio
        /// </summary>
        /// <param name="poIdServicio"></param>
        /// <returns></returns>
        public Respuesta<List<TipoIndicador>> gObenerTipoIndicadoresPorServicioReporte(int poIdServicio)
        {
            Respuesta<List<TipoIndicador>> objRespuesta = new Respuesta<List<TipoIndicador>>();
            try
            {

                objRespuesta = objServicioAD.gObenerTipoIndicadoresPorServicioReporte(poIdServicio);


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
        /// Obtiene los indicadores que pertenecen a un determinado servicio
        /// </summary>
        /// <param name="poIdServicio"></param>
        /// <returns></returns>
        public Respuesta<List<Indicador>> gObtenerIndicadoresPorServicioReporte(int poIdServicio)
        { 
           Respuesta<List<Indicador>> objRespuesta = new Respuesta<List<Indicador>>();
            try
            {

                objRespuesta = objServicioAD.gObtenerIndicadoresPorServicioReporte(poIdServicio);


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
        /// Obtiene los servicios de un determinado operador
        /// </summary>
        /// <param name="poIdOperador"></param>
        /// <returns></returns>
        public Respuesta<List<Servicio>> gObtenerServiciosPorOperadorReporte(String poIdOperador)
        {
            Respuesta<List<Servicio>> objRespuesta = new Respuesta<List<Servicio>>();
            try
            {
                objRespuesta = objServicioAD.gObtenerServiciosPorOperadorReporte(poIdOperador);
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

        public Respuesta<List<ServicioOperador>> ConsultarServicioOperador(string strOperador) 
        {
            Respuesta<List<ServicioOperador>> objRespuesta = new Respuesta<List<ServicioOperador>>();

            try
            {
                objRespuesta = objServicioAD.ConsultarServicioOperador(strOperador);
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
         public Respuesta<List<Servicio>> ObtenerNoVerificados(string strOperador)
        {
            Respuesta<List<Servicio>> objRespuesta = new Respuesta<List<Servicio>>();
            try
            {
                objRespuesta = objServicioAD.ObtenerNoVerificados(strOperador);
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
         public bool SendMail(string html) 
         {
             bool respuesta;
             try
             {
                 respuesta = objServicioAD.SendMail("Verificacion de Servicios", html);
                 return respuesta;
             }
             catch (Exception ex)
             {
                 if (ex is CustomException) throw;
                 string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                 throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
                 return false;
             }
         }

         public Respuesta<List<Servicio>> ConsultarServicioVerificado(string strOperador, string buscarServicio)
         {
             Respuesta<List<Servicio>> objRespuesta = new Respuesta<List<Servicio>>();

             try 
             {
                 objRespuesta = objServicioAD.ConsultarServicioVerificado(strOperador, buscarServicio);
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
    }
}
