using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using System.Transactions;
using System.Data.Entity;
using GB.SUTEL.Shared;
using System.Data.SqlClient;
using GB.SUTEL.ExceptionHandler;

namespace GB.SUTEL.DAL.Mantenimientos
{
    public class TipoIndicadorAD : LocalContextualizer
    {
        private SUTEL_IndicadoresEntities objContext;
        public TipoIndicadorAD(ApplicationContext appContext)
            : base(appContext)
        {
            objContext = new SUTEL_IndicadoresEntities();
        }       

        #region Agregar
        /// <summary>
        /// Método que agrega un Tipo Indicador a la base de datos
        /// </summary>
        /// <param name="objTipoIndicador">Objeto tipo Tipo Indicador con los datos a agregar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<TipoIndicador> Agregar(TipoIndicador objTipoIndicador)
        {
            Respuesta<TipoIndicador> objRespuesta = new Respuesta<TipoIndicador>();
            try
            {
                Random random = new Random();
                objTipoIndicador.IdTipoInd = Convert.ToInt32(random.Next(1, 99999).ToString() + random.Next(1, 999).ToString());
                objTipoIndicador.Borrado = 0;

                //Set objeto en respuesta
                objRespuesta.objObjeto = objTipoIndicador;
                //Execute en la base de datos
                objContext.TipoIndicador.Add(objTipoIndicador);
                objContext.SaveChanges();
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
        /// Método que edita un Tipo Indicador a la base de datos
        /// </summary>
        /// <param name="objTipoIndicador">Objeto tipo Tipo Indicador con los datos a editar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<TipoIndicador> Editar(TipoIndicador objTipoIndicador)
        {
            Respuesta<TipoIndicador> objRespuesta = new Respuesta<TipoIndicador>();
            try
            {
                TipoIndicador consultarEditar = objContext.TipoIndicador.Where(x => x.IdTipoInd == objTipoIndicador.IdTipoInd).FirstOrDefault();
                if (consultarEditar != null)
                {
                    //Set objeto en respuesta
                    consultarEditar.Borrado = 0;
                    consultarEditar.DesTipoInd = objTipoIndicador.DesTipoInd;
                    objRespuesta.objObjeto = consultarEditar;

                    objContext.TipoIndicador.Attach(consultarEditar);
                    objContext.Entry(consultarEditar).State = EntityState.Modified;
                    //Execute en la base de datos                    
                    objContext.SaveChanges();
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
        /// Método que Elimina un Tipo Indicador a la base de datos
        /// </summary>
        /// <param name="objTipoIndicador">Objeto tipo Tipo Indicador con los datos a Eliminar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<TipoIndicador> Eliminar(TipoIndicador objTipoIndicador)
        {
            Respuesta<TipoIndicador> objRespuesta = new Respuesta<TipoIndicador>();
            try
            {
                TipoIndicador consultarEliminar = objContext.TipoIndicador.Where(x => x.IdTipoInd == objTipoIndicador.IdTipoInd).FirstOrDefault();
                if (consultarEliminar != null)
                {
                    //Set objeto en respuesta
                    consultarEliminar.Borrado = 1;
                    objRespuesta.objObjeto = consultarEliminar;

                    objContext.Dispose();
                    objContext = new SUTEL_IndicadoresEntities();

                    objContext.TipoIndicador.Attach(consultarEliminar);
                    objContext.Entry(consultarEliminar).State = EntityState.Modified;
                    //Execute en la base de datos                    
                    objContext.SaveChanges();
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

        #region ConsultaPorID
        /// <summary>
        /// Método consulta un Tipo Indicador a la base de datos
        /// </summary>
        /// <param name="objTipoIndicador">Objeto tipo Tipo Indicador con los datos para consultar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<TipoIndicador> ConsultaPorID(TipoIndicador objTipoIndicador)
        {
            Respuesta<TipoIndicador> objRespuesta = new Respuesta<TipoIndicador>();
            try
            {
                objRespuesta.objObjeto = (
                             from tipoIndicadorEntidad in objContext.TipoIndicador
                             where tipoIndicadorEntidad.IdTipoInd == objTipoIndicador.IdTipoInd
                             select tipoIndicadorEntidad
                             ).FirstOrDefault();

                objTipoIndicador.DesTipoInd = objRespuesta.objObjeto.DesTipoInd;
                objRespuesta.objObjeto = objTipoIndicador;
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

        #region ConsultaPorNombre
        /// <summary>
        /// Método consulta un Tipo Indicador a la base de datos
        /// </summary>
        /// <param name="objTipoIndicador">Objeto tipo Tipo Indicador con los datos para consultar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<TipoIndicador> ConsultaPorNombre(TipoIndicador objTipoIndicador)
        {
            Respuesta<TipoIndicador> objRespuesta = new Respuesta<TipoIndicador>();
            try
            {
                objRespuesta.objObjeto = (
                               from tipoIndicadorEntidad in objContext.TipoIndicador
                               where tipoIndicadorEntidad.DesTipoInd == objTipoIndicador.DesTipoInd &&
                                     tipoIndicadorEntidad.Borrado == 0 && tipoIndicadorEntidad.IdTipoInd != objTipoIndicador.IdTipoInd
                               select tipoIndicadorEntidad
                               ).FirstOrDefault();
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

        #region ConsultaPorDependenciaEnIndicador
        /// <summary>
        /// Método consulta un Tipo Indicador a la base de datos por dependencias
        /// </summary>
        /// <param name="objTipoIndicador">Objeto tipo Tipo Indicador con los datos para consultar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<TipoIndicador> ConsultaPorDependenciaEnIndicador(TipoIndicador objTipoIndicador)
        {
            Respuesta<TipoIndicador> objRespuesta = new Respuesta<TipoIndicador>();
            try
            {
                objRespuesta.objObjeto = (
                            from tipoIndicadorEntidad in objContext.TipoIndicador
                            join indicadorEntidad in objContext.Indicador on tipoIndicadorEntidad.IdTipoInd equals indicadorEntidad.IdTipoInd
                            where tipoIndicadorEntidad.IdTipoInd == objTipoIndicador.IdTipoInd
                            select tipoIndicadorEntidad
                            ).FirstOrDefault();
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

        #region ConsultaPorDependenciaEnServicio
        /// <summary>
        /// Método consulta un Tipo Indicador a la base de datos por dependencias
        /// </summary>
        /// <param name="objTipoIndicador">Objeto tipo Tipo Indicador con los datos para consultar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<TipoIndicador> ConsultaPorDependenciaEnServicio(TipoIndicador objTipoIndicador)
        {
            Respuesta<TipoIndicador> objRespuesta = new Respuesta<TipoIndicador>();
            try
            {
                objRespuesta.objObjeto = (
                                from tipoIndicadorEntidad in objContext.TipoIndicador
                                join servEntidad in objContext.TipoIndicadorServicio on tipoIndicadorEntidad.IdTipoInd equals servEntidad.IdTipoInd
                                where tipoIndicadorEntidad.IdTipoInd == objTipoIndicador.IdTipoInd
                                select tipoIndicadorEntidad
                                ).FirstOrDefault();
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
        /// Método que agrega un Tipo Indicador a la base de datos
        /// </summary>
        /// <param name="objTipoIndicador">Objeto tipo Tipo Indicador</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<List<TipoIndicador>> ConsultarTodos()
        {
            Respuesta<List<TipoIndicador>> objRespuesta = new Respuesta<List<TipoIndicador>>();
            List<TipoIndicador> oTipoIndicador = new List<TipoIndicador>();
            try
            {
                //Execute en la base de datos
                oTipoIndicador = (
                             from tipoIndicadorEntidad in objContext.TipoIndicador
                             where tipoIndicadorEntidad.Borrado == 0
                             select tipoIndicadorEntidad
                            ).ToList();

                objRespuesta.objObjeto = oTipoIndicador;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, oTipoIndicador);
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
            List<TipoIndicador> oTipoIndicador = new List<TipoIndicador>();
            try
            {
                //Execute en la base de datos
                oTipoIndicador =  objContext.TipoIndicador.Where(x=> x.Borrado==0 &&
                    (piIDTipoIndicador.Equals(0) || x.IdTipoInd.ToString().Contains(piIDTipoIndicador.ToString()))
                    && (psNombreTipoIndicador.Equals("") || x.DesTipoInd.ToUpper().Contains(psNombreTipoIndicador.ToUpper())))
                            .ToList();

                objRespuesta.objObjeto = oTipoIndicador;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, oTipoIndicador);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }

        #endregion       
    }
}
