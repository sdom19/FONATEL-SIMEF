using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using System.Data.Entity;
using System.Security.Cryptography;
using GB.SUTEL.Shared;
using GB.SUTEL.ExceptionHandler;
using System.Configuration;

using Usuario = GB.SUTEL.Entities.Usuario;
using Rol = GB.SUTEL.Entities.Rol;
using System.DirectoryServices.AccountManagement;

namespace GB.SUTEL.DAL.Seguridad
{
    public class UsersDA : LocalContextualizer
    {
        private SUTEL_IndicadoresEntities objContext;
        private Respuesta<Usuario> objRespuesta;
        public UsersDA(ApplicationContext appContext) : base(appContext)
        {
            objContext = new SUTEL_IndicadoresEntities();
            objRespuesta = new Respuesta<Usuario>();
        }
        #region Agregar
        /// <summary>
        /// Método que agrega un User a la base de datos
        /// </summary>
        /// <param name="objUser">Objeto tipo User con los datos a agregar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Usuario> Agregar(Usuario objUser)
        {
            try
            {
                SUTEL_IndicadoresEntities db = new SUTEL_IndicadoresEntities();
                if (objUser.UsuarioInterno == 1)
                {
                    string name = ConfigurationManager.AppSettings["SutelActiveDirectory"].ToString();
                    string str = ConfigurationManager.AppSettings["SutelADuser"].ToString();
                    string password = ConfigurationManager.AppSettings["SutelADpassword"].ToString();
                    string userName = str.Replace("/", "\\");
                    using (PrincipalContext context = new PrincipalContext(ContextType.Domain, name, userName, password))
                    {
                        using (PrincipalSearcher principalSearcher = new PrincipalSearcher((Principal)new UserPrincipal(context)))
                        {
                            foreach (UserPrincipal userPrincipal in principalSearcher.FindAll().Cast<UserPrincipal>())
                            {
                                if (userPrincipal.SamAccountName == objUser.AccesoUsuario && userPrincipal.EmailAddress != null)
                                    objUser.CorreoUsuario = userPrincipal.EmailAddress;
                            }
                        }
                    }
                }
                //Datasoft
                //Crear usuario
                Usuario usuario = new Usuario()
                {
                    IdOperador = objUser.IdOperador,
                    AccesoUsuario = objUser.AccesoUsuario,
                    NombreUsuario = objUser.NombreUsuario,
                    Contrasena = objUser.Contrasena,
                    CorreoUsuario = objUser.CorreoUsuario,
                    UsuarioInterno = objUser.UsuarioInterno,
                    Activo = objUser.Activo,
                    Borrado = objUser.Borrado,
                    //Mercado = objUser.Mercados,
                    Calidad = objUser.Calidad,
                    FONATEL = objUser.FONATEL

                };
                //Agregar usuario a BD
                db.Usuario.Add(usuario);
                //Guardar cambios
                db.SaveChanges();
                objRespuesta.objObjeto = objContext.Usuario.Find(usuario.IdUsuario);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
            return objRespuesta;
        }
        #endregion

        #region Editar
        /// <summary>
        /// Método que edita un User a la base de datos
        /// </summary>
        /// <param name="objUser">Objeto tipo User con los datos a editar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Usuario[]> Editar(Usuario objUSUARIO)
        {
            try
            {
                Usuario[] arrUSUARIO = new Usuario[2];
                Respuesta<Usuario[]> objRespuestaEditar = new Respuesta<Usuario[]>();
                objContext.Entry(objUSUARIO).State = EntityState.Modified;
                objContext.SaveChanges();
                arrUSUARIO[0] = objUSUARIO;
                objRespuestaEditar.objObjeto = arrUSUARIO;
                return objRespuestaEditar;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
        }
        public Respuesta<Usuario> Editar(Usuario objUSUARIO, int? nullable)
        {
            try
            {
                objContext.Entry(objUSUARIO).State = EntityState.Modified;
                objContext.SaveChanges();
                objRespuesta.objObjeto = objUSUARIO;
                return objRespuesta;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
        }
        #endregion

        #region ConsultarPorExpresión
        /// <summary>
        /// Método consulta un User a la base de datos
        /// </summary>
        /// <param name="expression">Expresión Labda para consultar la base de datos</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Usuario> Single(System.Linq.Expressions.Expression<Func<Usuario, bool>> expression)
        {
            try
            {


                objRespuesta.objObjeto = objContext.Usuario.FirstOrDefault(expression);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
            return objRespuesta;
        }
        public Usuario Login(System.Linq.Expressions.Expression<Func<Usuario, bool>> expression)
        {
            try
            {
                return objContext.Usuario.SingleOrDefault(expression);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
        }
        #endregion

        #region ConsultarTodos
        /// <summary>
        /// Método que agrega un User a la base de datos
        /// </summary>
        /// <param name="objUser">Objeto tipo User</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<List<Usuario>> ConsultarTodos()
        {
            Respuesta<List<Usuario>> RespuestaObj = new Respuesta<List<Usuario>>();
            List<Usuario> oUseres = new List<Usuario>();
            try
            {
                oUseres = objContext.Usuario.OrderBy(x => x.NombreUsuario).Where(x => x.Borrado == 0).OrderBy(p => p.NombreUsuario).ToList();
                RespuestaObj.objObjeto = oUseres;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                RespuestaObj.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
            return RespuestaObj;
        }
        /// <summary>
        /// Filtro para usuarios
        /// </summary>
        /// <param name="psUsuario"></param>
        /// <param name="psNombreUsuario"></param>
        /// <returns></returns>
        public Respuesta<List<Usuario>> gFiltrarUsuarios(String psUsuario, String psNombre, String psCorreo, String psNombreOperador)
        {
            Respuesta<List<Usuario>> RespuestaObj = new Respuesta<List<Usuario>>();
            List<Usuario> oUseres = new List<Usuario>();
            try
            {
                oUseres = objContext.Usuario.Where(x => (x.Borrado == 0) && (psUsuario.Equals("") || x.AccesoUsuario.ToUpper().Contains(psUsuario.ToUpper()))
                     && (psNombre.Equals("") || (x.NombreUsuario.ToUpper().Contains(psNombre.ToUpper())))
                     && (psCorreo.Equals("") || (x.CorreoUsuario.ToUpper().Contains(psCorreo.ToUpper())))
                     ).OrderBy(x => x.NombreUsuario).ToList();
                //&& (psNombreOperador.Equals("") || x.Operador.NombreOperador.ToUpper().Contains(psNombreOperador.ToUpper()))
                RespuestaObj.objObjeto = oUseres;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                RespuestaObj.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
            return RespuestaObj;
        }

        #endregion

        #region ConsultarTodosLosRoles
        /// <summary>
        /// Método que agrega un Rol a la base de datos
        /// </summary>
        /// <param name="objRol">Objeto tipo Rol</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<List<Rol>> ConsultarTodosLosRoles()
        {

            Respuesta<List<Rol>> objRespuesta = new Respuesta<List<Rol>>();
            List<Rol> oRoles = new List<Rol>();
            try
            {
                //Execute en la base de datos
                oRoles = (
                        from rolEntidad in objContext.Rol
                        select rolEntidad
                    ).ToList();

                objRespuesta.objObjeto = oRoles;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, oRoles);
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }

            return objRespuesta;
        }

        #endregion ConsultarTodosLosRoles

        #region ConsultarUnRol
        public Respuesta<Rol> SingleRol(System.Linq.Expressions.Expression<Func<Rol, bool>> expression)
        {
            try
            {
                Respuesta<Rol> objRespuesta = new Respuesta<Rol>();
                objRespuesta.objObjeto = objContext.Rol.FirstOrDefault(expression);
                return objRespuesta;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
        }
        #endregion

        #region Eliminar
        public Respuesta<Usuario> Eliminar(Usuario objUser)
        {
            try
            {
                var usuarioHaEliminar = objContext.Usuario.SingleOrDefault(x => x.IdUsuario == objUser.IdUsuario);
                usuarioHaEliminar.Borrado = 1;
                usuarioHaEliminar.Activo = 0;
                //objContext.Entry(objUser).State = EntityState.Deleted;
                objRespuesta.objObjeto = usuarioHaEliminar;
                objContext.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
            return objRespuesta;
        }
        #endregion

        #region salthash

        #endregion salthash

        public bool SendMail(string to, string asunto, string html)
        {
            try
            {
                string profile = ConfigurationManager.AppSettings["passwordMailingDBProfile"].ToString();
                objContext.pa_SendEmail(to, asunto, html, profile, 100);
                return true;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
                return false;
            }
        }
    }
}
