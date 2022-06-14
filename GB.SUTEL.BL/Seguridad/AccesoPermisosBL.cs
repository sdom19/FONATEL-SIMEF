using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// GB Classes
using GB.SUTEL.Entities;
using GB.SUTEL.Shared;
using GB.SUTEL.DAL;
using GB.SUTEL.DAL.Seguridad;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.Entities.Utilidades;

namespace GB.SUTEL.BL.Seguridad
{
    public class AccesoPermisosBL : LocalContextualizer
    {
        #region Atributos
        /// <summary>
        /// objeto global de  en la capa de acceso a datos
        /// </summary>
        private RolDA objRolDA;
        private AccionDA objAccionDA;
        private PantallaDA objPantallaDA;
        private RolAccionPantallaDA objRolAccionPantallaDA;
        
        #endregion     
        
        public AccesoPermisosBL(ApplicationContext appContext):base(appContext)
        {
            objRolDA = new RolDA(appContext);
            objAccionDA = new AccionDA(appContext);
            objPantallaDA = new PantallaDA(appContext);
            objRolAccionPantallaDA = new RolAccionPantallaDA(appContext);
        }

        #region ActualizaPermisos
        /// <summary>
        /// Método para actualizar Permisos
        /// </summary>
        /// <param name="objRol">Objeto a agregar</param>
        /// <param name="objRolAccionPantallas">Objeto a agregar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Rol> ActualizaPermisos(Rol objRol, List<RolAccionPantalla> objRolAccionPantallas)
        {
            Respuesta<Rol> objRespuesta = new Respuesta<Rol>();
            try
            {
                if (objRolAccionPantallaDA.EliminarTodosPorRol(objRol).blnIndicadorTransaccion)
                {
                    objRolAccionPantallaDA.AgregarTodosPorRol(objRolAccionPantallas);                    
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
                objRespuesta.toError(msj, objRol);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        #endregion

        #region ConsultarAccionesPorRolYPantalla
        /// <summary>
        /// Consultar todas las acciones
        /// </summary>
        /// <param name="objRolAccionPantalla"></param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<List<Accion>> ConsultarAccionesPorRolYPantalla(RolAccionPantalla objRolAccionPantalla)
        {
            Respuesta<List<Accion>> objRespuesta = new Respuesta<List<Accion>>();
            try
            {
                objRespuesta = objAccionDA.ConsultarPorRolYPantalla(objRolAccionPantalla);
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

        #region ConsultarPantallas
        /// <summary>
        /// Consultar todas las acciones
        /// </summary>        
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<List<PANTALLAMENU>> ConsultarPantallas()
        {
            Respuesta<List<PANTALLAMENU>> objRespuesta = new Respuesta<List<PANTALLAMENU>>();
            try
            {
                objRespuesta = objPantallaDA.ConsultarTodosConPadre();
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
        /// Método que agrega un Pantalla a la base de datos
        /// </summary>
        /// <param name="objPantalla">Objeto tipo Pantalla</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<List<Pantalla>> ConsultarPantallasReporte()
        {
            Respuesta<List<Pantalla>> objRespuesta = new Respuesta<List<Pantalla>>();
            try
            {
                objRespuesta = objPantallaDA.ConsultarTodos();
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

        #region ConsultarAcciones
        /// <summary>
        /// Consultar todas las acciones
        /// </summary>        
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<List<Accion>> ConsultarAcciones()
        {
            Respuesta<List<Accion>> objRespuesta = new Respuesta<List<Accion>>();
            try
            {
                objRespuesta = objAccionDA.ConsultarTodos();
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

        #region ConsultarPermisosPorRol
        /// <summary>
        /// Consultar todas las acciones por rol
        /// </summary>        
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<List<RolAccionPantalla>> ConsultarPermisosPorRol(Rol objRol)
        {
            Respuesta<List<RolAccionPantalla>> objRespuesta = new Respuesta<List<RolAccionPantalla>>();
            try
            {
                objRespuesta = objRolAccionPantallaDA.ConsultaPorRol(objRol);
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
