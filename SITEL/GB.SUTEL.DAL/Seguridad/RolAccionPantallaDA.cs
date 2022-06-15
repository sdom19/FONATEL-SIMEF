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
    public class RolAccionPantallaDA : LocalContextualizer
    {
        private SUTEL_IndicadoresEntities objContext;
        public RolAccionPantallaDA(ApplicationContext appContext)
            : base(appContext)
        {
            objContext = new SUTEL_IndicadoresEntities();
        }      

        #region AgregarTodosPorRol
        /// <summary>
        /// Método que agrega un acciones en pantallas a la base de datos
        /// </summary>
        /// <param name="objRolAccionPantalla">Objeto tipo accion en pantalla con los datos a agregar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<RolAccionPantalla> AgregarTodosPorRol(List<RolAccionPantalla> objRolAccionPantallas)
        {
            Respuesta<RolAccionPantalla> objRespuesta = new Respuesta<RolAccionPantalla>();
            try
            {                
                //Set objeto en respuesta
                objRespuesta.objObjeto = null;

                foreach (RolAccionPantalla obj in objRolAccionPantallas)
                {
                    //Execute en la base de datos
                    objContext.RolAccionPantalla.Add(obj);
                    objContext.SaveChanges();
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
        #endregion     
        
        #region ConsultaPorRol
        /// <summary>
        /// Método que consulta acciones en pantallas por rol a la base de datos
        /// </summary>
        /// <param name="objRolAccionPantalla">Objeto tipo accion en pantalla con los datos a agregar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<List<RolAccionPantalla>> ConsultaPorRol(Rol objRol)
        {
            Respuesta<List<RolAccionPantalla>> objRespuesta = new Respuesta<List<RolAccionPantalla>>();
            RolAccionPantalla objAccionPantallaRol;
            try
            {
                //Set objeto en respuesta
                objRespuesta.objObjeto = null;

                //Execute en la base de datos
                objRespuesta.objObjeto = (
                             from entidad in objContext.RolAccionPantalla
                             where entidad.IdRol == objRol.IdRol
                             select entidad
                            ).ToList();                

                List<RolAccionPantalla> listadoPermisos = new List<RolAccionPantalla>();
                foreach (RolAccionPantalla objOne in objRespuesta.objObjeto)
                {
                    objAccionPantallaRol = new RolAccionPantalla();
                    objAccionPantallaRol.IdRol = objOne.IdRol;
                    objAccionPantallaRol.IdAccion = objOne.IdAccion;
                    objAccionPantallaRol.IdPantalla = objOne.IdPantalla;
                    listadoPermisos.Add(objAccionPantallaRol);                        
                }

                objRespuesta.objObjeto = listadoPermisos;
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

        #region EliminarTodosPorRol
        /// <summary>
        /// Método que Elimina un acciones en pantalla a la base de datos
        /// </summary>
        /// <param name="objRolAccionPantalla">Objeto tipo accion pantalla con los datos a Eliminar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Rol> EliminarTodosPorRol(Rol objRol)
        {
            Respuesta<Rol> objRespuesta = new Respuesta<Rol>();
            List<RolAccionPantalla> objListado = new List<RolAccionPantalla>();
            try
            {
                //Set objeto en respuesta
                objRespuesta.objObjeto = objRol;

                objListado = (
                               from entidad in objContext.RolAccionPantalla
                               where entidad.IdRol == objRol.IdRol
                               select entidad
                               ).ToList();
                //Execute en la base de datos  
                foreach (RolAccionPantalla obj in objListado)
                {
                    objContext.RolAccionPantalla.Attach(obj);
                    objContext.Entry(obj).State = EntityState.Deleted;
                    //Execute en la base de datos                    
                    objContext.SaveChanges();
                }   
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objRol);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }
        #endregion              
    }
}
