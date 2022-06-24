using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMLib.Extensions;

// GB Classes
using GB.SUTEL.Entities;
using GB.SUTEL.DAL;
using GB.SUTEL.DAL.Seguridad;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.Shared;

namespace GB.SUTEL.BL.Seguridad
{
    public class RolBL : LocalContextualizer
    {
        public RolBL(ApplicationContext appContext)
            : base(appContext) {
        objRolDA = new RolDA(appContext);
        }
        #region Atributos
        /// <summary>
        /// objeto global de Rol en la capa de acceso a datos
        /// </summary>
        private RolDA objRolDA;

        #endregion

        #region Agregar
        /// <summary>
        /// Método para agregar un Rol
        /// </summary>
        /// <param name="objRol">Objeto a agregar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Rol> Agregar(Rol objRol)
        {
            Respuesta<Rol> objRespuesta = new Respuesta<Rol>();
            try
            {
                if (objRolDA.ConsultaPorNombreRol(objRol).objObjeto == null)
                {
                    objRespuesta = objRolDA.Agregar(objRol);
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

        #region Editar
        /// <summary>
        /// Método para Editar un Rol
        /// </summary>
        /// <param name="objRol">Objeto a Editar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Rol> Editar(Rol objRol)
        {
            Respuesta<Rol> objRespuesta = new Respuesta<Rol>();
            try
            {
                if (objRolDA.ConsultaPorNombreRol(objRol).objObjeto == null)
                {
                    objRespuesta = objRolDA.Editar(objRol);
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

        #region Eliminar
        /// <summary>
        /// Método para Eliminar un Rol
        /// </summary>
        /// <param name="objRol">Objeto a Eliminar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Rol> Eliminar(Rol objRol)
        {
            Respuesta<Rol> objRespuesta = new Respuesta<Rol>();
            try
            {
                if (objRolDA.ConsultaPorDependenciaEnUsuario(objRol).objObjeto != null)
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.strMensaje = "Existe una dependencia con algún usuario.";
                }
                else
                {
                    if (objRolDA.ConsultaPorDependenciaEnAccionYPantalla(objRol).objObjeto != null)
                    {
                        objRespuesta.blnIndicadorTransaccion = false;
                        objRespuesta.strMensaje = "Existe una dependencia con alguna acción o pantalla.";
                    }
                    else
                    {
                        objRespuesta = objRolDA.Eliminar(objRol);
                    }
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

        #region ConsultarTodos
        /// <summary>
        /// Consultar Todos
        /// </summary>
        /// <param name="objRol"></param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<List<Rol>> ConsultarTodos()
        {
            Respuesta<List<Rol>> objRespuesta = new Respuesta<List<Rol>>();
            try
            {
                objRespuesta = objRolDA.ConsultarTodos();
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
        /// Filtra los roles
        /// </summary>
        /// <param name="piIdRol"></param>
        /// <param name="psNombreRol"></param>
        /// <returns></returns>
        public Respuesta<List<Rol>> gFiltrarRoles(int piIdRol, String psNombreRol)
        {
            Respuesta<List<Rol>> objRespuesta = new Respuesta<List<Rol>>();
            try
            {
                objRespuesta = objRolDA.gFiltrarRoles(piIdRol,psNombreRol);
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


        #region Verificacion de roles
        public bool VerifyRoles(List<string> roles, string controllerName)
        {
            try
            {
                var allRolees = ConsultarTodos().objObjeto.ToList();
                foreach (var item in allRolees)
                {
                    int p = item.RolAccionPantalla.Where(x => x.Pantalla.Nombre.RemoveDiacritics().ToUpper().Replace(" ", "").Replace("DE","").Replace("E","")
                        .Replace("S","") ==
                        controllerName.RemoveDiacritics().ToUpper().Replace(" ", "").Replace("DE","").Replace("E", "").Replace("S", "")).Count();
                    if (roles.Contains(item.NombreRol) && p > 0) return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                if (!(ex is CustomException))
                    AppContext.ExceptionBuilder.BuildException(ex.Message, ex);
                return false;
            }
        }
        public bool ValidateScreenFunctions(List<string> currentRoles, string pantalla, string accion)
        {
            try
            {
                var todos = ConsultarTodos().objObjeto.ToList();
                todos = todos.Where(item => currentRoles.Contains(item.NombreRol)).ToList();

                var asociaciones = todos.SelectMany(x => x.RolAccionPantalla).
                    Where(p => p.Pantalla.Nombre.RemoveDiacritics().ToUpper().Replace(" ","").Replace("E","").Replace("S","")
                        == pantalla.RemoveDiacritics().ToUpper().Replace(" ", "").Replace("E", "").Replace("S", "") 
                        && p.Accion.Nombre.ToUpper() == accion.ToUpper()).Count();
                if (asociaciones > 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                if (!(ex is CustomException))
                    AppContext.ExceptionBuilder.BuildException(ex.Message, ex);
                return false;
            }
        }
        public List<string> GetActionsRolScreen(List<string> roles, string pantalla)
        {
            try
            {
                var allRolees = ConsultarTodos().objObjeto.ToList();
                List<string> RolScreenActions = allRolees.Where(a => roles.Contains(a.NombreRol)).SelectMany(b => b.RolAccionPantalla)
                    .Where(c => c.Pantalla.Nombre.ToUpper() == pantalla.ToUpper()).Select(d => d.Accion.Nombre).ToList();
                if (RolScreenActions.Count() > 0)
                    return RolScreenActions;
                else 
                    return new List<string>();
            }
            catch (Exception ex)
            {
                if (!(ex is CustomException))
                    AppContext.ExceptionBuilder.BuildException(ex.Message, ex);
                return new List<string>();
            }

        }
        public List<string> GetRolScreen(List<string> roles)
        {
            try
            {
                var allRolees = ConsultarTodos().objObjeto.ToList();

                List<string> RolScreen = allRolees.Where(a => roles.Contains(a.NombreRol)).SelectMany(b => b.RolAccionPantalla).Select(c => c.Pantalla.Nombre).ToList();
                if (RolScreen.Count() > 0)
                    return RolScreen;
                else 
                    return new List<string>();
            }
            catch (Exception ex)
            {
                if (!(ex is CustomException))
                    AppContext.ExceptionBuilder.BuildException(ex.Message, ex);
                return new List<string>();
            }
        }
        #endregion
    }
}
