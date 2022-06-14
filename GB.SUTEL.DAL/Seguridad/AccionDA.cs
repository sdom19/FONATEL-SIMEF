using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.Shared;
using System.Data.SqlClient;
using GB.SUTEL.ExceptionHandler;

namespace GB.SUTEL.DAL.Seguridad
{
    public class AccionDA : LocalContextualizer
    {
        private SUTEL_IndicadoresEntities objContext;
        public AccionDA(ApplicationContext appContext)
            : base(appContext)
        {
            objContext = new SUTEL_IndicadoresEntities();
        }   

        #region Agregar
        /// <summary>
        /// Método que agrega un Accion a la base de datos
        /// </summary>
        /// <param name="objAccion">Objeto tipo Accion con los datos a agregar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Accion> Agregar(Accion objAccion)
        {
            Respuesta<Accion> objRespuesta = new Respuesta<Accion>();
            try
            {
                Random random = new Random();
                objAccion.IdAccion = Convert.ToInt32(random.Next(1, 99999).ToString() + random.Next(1, 999).ToString());

                //Set objeto en respuesta
                objRespuesta.objObjeto = objAccion;
                //Execute en la base de datos
                objContext.Accion.Add(objAccion);
                objContext.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objAccion);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }
        #endregion        

        #region ConsultarTodos
        /// <summary>
        /// Método que consulta acciones de la base de datos
        /// </summary>       
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<List<Accion>> ConsultarTodos()
        {
            Respuesta<List<Accion>> objRespuesta = new Respuesta<List<Accion>>();
            List<Accion> oAccion = new List<Accion>();
            try
            {
                //Execute en la base de datos
                oAccion = (
                            from accionEntidad in objContext.Accion
                            select accionEntidad
                        ).ToList();

                objRespuesta.objObjeto = oAccion;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, oAccion);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }
        #endregion     
  
        #region ConsultarPorRolYPantalla
        /// <summary>
        /// Método que agrega un Accion a la base de datos
        /// </summary>
        /// <param name="objRolAccionPantalla">Objeto tipo Accion</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<List<Accion>> ConsultarPorRolYPantalla(RolAccionPantalla objRolAccionPantalla)
        {
            Respuesta<List<Accion>> objRespuesta = new Respuesta<List<Accion>>();
            List<Accion> oAccion = new List<Accion>();
            try
            {
                //Execute en la base de datos
                oAccion = (
                        from accionEntidad in objContext.Accion
                        join rolAccionPantallaEntidad in objContext.RolAccionPantalla on accionEntidad.IdAccion equals rolAccionPantallaEntidad.IdAccion
                        where rolAccionPantallaEntidad.IdRol == objRolAccionPantalla.IdRol &&
                            rolAccionPantallaEntidad.IdPantalla == objRolAccionPantalla.IdPantalla
                        select accionEntidad
                    ).ToList();

                objRespuesta.objObjeto = oAccion;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, oAccion);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }
        #endregion    
    }
}
