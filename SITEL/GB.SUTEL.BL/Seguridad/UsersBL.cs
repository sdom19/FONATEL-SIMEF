using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GB.SUTEL.Entities;
using GB.SUTEL.DAL.Seguridad;
using GB.SUTEL.DAL.Communication;
using GB.SUTEL.Entities.Utilidades;
using System.Security.Cryptography;
using GB.SUTEL.Shared;
using GB.SUTEL.ExceptionHandler;
using System.Configuration;

namespace GB.SUTEL.BL.Seguridad
{
    public class UsersBL : LocalContextualizer
    {
        private Respuesta<Usuario> objRespuesta;
        private UsersDA objUserDA;
        private DAL.Mantenimientos.OperadorDA refOp;
        public UsersBL(ApplicationContext appContext)
            : base(appContext)
        {
            objRespuesta = new Respuesta<Usuario>();
            objUserDA = new UsersDA(appContext);
            refOp = new DAL.Mantenimientos.OperadorDA(appContext);
        }
        #region Atributos
        /// <summary>
        /// objeto global de Usuario en la capa de acceso a datos
        /// </summary>

        #endregion

        #region Agregar
        public Respuesta<Usuario> Agregar(Usuario objUser, string ACCESOAD, string NOMBREAD, string ISACTIVE, string renderedEmail)
        {
            try
            {
                objUser.Borrado = 0;
                objUser.Activo = ISACTIVE == null ? Convert.ToByte(false) : Convert.ToByte(true);
                if (ACCESOAD != null && ACCESOAD != "")
                {
                    objUser.AccesoUsuario = ACCESOAD.Trim();
                    objUser.NombreUsuario = ((NOMBREAD == null || NOMBREAD == "") ? ACCESOAD : NOMBREAD).Trim();
                    objUser.Contrasena = "Undefined";
                    objUser.CorreoUsuario = "Undefined";
                    //objUser.IdOperador = refOp.Single(x => x.Estado == true).objObjeto.IdOperador;   //Pulga: si el usuario es interno entones le asigna este ID de operador
                }
                else
                {
                    Rol rol = objUserDA.SingleRol(x => x.NombreRol.ToUpper().Contains("Operador")).objObjeto;
                    objUser.Rol.Add(rol);
                }
                //Hash password
                objUser.Contrasena = HashPassword(objUser.Contrasena);

                //revisa si el usuario ya existe                
                if (objUserDA.Single(x => x.AccesoUsuario == objUser.AccesoUsuario && x.Borrado == 0).objObjeto == null)
                {
                    objUser.AccesoUsuario = objUser.AccesoUsuario.Trim();
                    objUser.Contrasena = objUser.Contrasena.Trim();
                    objUser.NombreUsuario = objUser.NombreUsuario.Trim();

                    objRespuesta = objUserDA.Agregar(objUser);
                    objUserDA.SendMail(objUser.CorreoUsuario, "Bienvenido: Usuario Operador", renderedEmail);
                    objRespuesta.strMensaje = "Usuario creado correctamente.";
                }
                else
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.UsuarioDuplicado;
                    objRespuesta.blnIndicadorState = 300;
                }
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

        #region AgregarROL
        public Respuesta<Usuario> AgregarRol(int idUser, string[] Roles)
        {
            try
            {
                var user = objUserDA.Single(x => x.IdUsuario == idUser).objObjeto;
                //if (user.UsuarioInterno != 1)
                //{                    
                //    throw AppContext.ExceptionBuilder.BuildDataAccessException("Un operador no puede estar asociado a otro rol.",new Exception());
                //}
                List<Rol> userRoles = user.Rol.ToList();
                foreach (var item in userRoles)
                {
                    user.Rol.Remove(item);
                }
                if (Roles != null)
                {
                    List<int> intRoles = Roles.Select(int.Parse).ToList();
                    List<Rol> allRoles = objUserDA.ConsultarTodosLosRoles().objObjeto;
                    user.Rol = allRoles.Where(item => intRoles.Contains(item.IdRol)).ToList();
                }
                objRespuesta = objUserDA.Editar(user, 1);
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
        public Respuesta<Usuario[]> Editar(Usuario objUser, string ACCESOAD, string NOMBREAD, string ISACTIVE, string renderedEmail = "")
        {
            Respuesta<Usuario[]> objRespuesta = new Respuesta<Usuario[]>();
            try
            {
                //Crear procedimiento para enviar a algún lugar la contraseña 
                //autogenearda antes de hashearla y guardarla en DB
                var usuarioDA = objUserDA.Single(x => x.IdUsuario == objUser.IdUsuario && x.Borrado == 0).objObjeto;
                if (usuarioDA.AccesoUsuario.ToUpper().Equals("ADMIN") && usuarioDA.AccesoUsuario != objUser.AccesoUsuario && usuarioDA.UsuarioInterno == 1)
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.UsuarioDuplicado;
                    objRespuesta.blnIndicadorState = 300;
                }
                Usuario usuarioViejo = new Usuario();
                usuarioViejo.IdUsuario = usuarioDA.IdUsuario;
                usuarioViejo.AccesoUsuario = usuarioDA.AccesoUsuario.Trim();
                usuarioViejo.Activo = usuarioDA.Activo;
                usuarioViejo.Borrado = usuarioDA.Borrado;
                usuarioViejo.Contrasena = usuarioDA.Contrasena.Trim();
                usuarioViejo.CorreoUsuario = usuarioDA.CorreoUsuario.Trim();
                usuarioViejo.IdOperador = usuarioDA.IdOperador;
                usuarioViejo.NombreUsuario = usuarioDA.NombreUsuario.Trim();
                usuarioViejo.UsuarioInterno = usuarioDA.UsuarioInterno;

                if (objUser.Contrasena == null && objUser.UsuarioInterno == 0)
                {
                    objUser.Contrasena = usuarioDA.Contrasena;
                    objUser.Activo = ISACTIVE == null ? Convert.ToByte(false) : Convert.ToByte(true);
                    //if (ACCESOAD != null && ACCESOAD != "")
                    if (usuarioDA.UsuarioInterno == 1)
                    {
                        //objUser.AccesoUsuario = ACCESOAD;
                        objUser.AccesoUsuario = usuarioDA.AccesoUsuario.Trim();
                        objUser.NombreUsuario = usuarioDA.NombreUsuario.Trim();
                        objUser.Contrasena = usuarioDA.Contrasena.Trim();
                        objUser.CorreoUsuario = usuarioDA.CorreoUsuario;
                        objUser.IdOperador = usuarioDA.IdOperador;
                        objUser.UsuarioInterno = usuarioDA.UsuarioInterno;
                    }
                    var userNameQuery = objUserDA.Single(x => x.AccesoUsuario == objUser.AccesoUsuario && x.Borrado == 0).objObjeto;
                    if (userNameQuery == null || (userNameQuery.AccesoUsuario == objUser.AccesoUsuario && userNameQuery.AccesoUsuario == usuarioDA.AccesoUsuario))
                    {
                        usuarioDA.AccesoUsuario = objUser.AccesoUsuario.Trim();
                        usuarioDA.Activo = objUser.Activo;
                        usuarioDA.Borrado = objUser.Borrado;
                        usuarioDA.Contrasena = objUser.Contrasena;
                        usuarioDA.CorreoUsuario = objUser.CorreoUsuario;
                        usuarioDA.IdOperador = objUser.IdOperador;
                        usuarioDA.NombreUsuario = objUser.NombreUsuario.Trim();
                        usuarioDA.UsuarioInterno = objUser.UsuarioInterno;
                        objRespuesta = objUserDA.Editar(usuarioDA);
                        objRespuesta.objObjeto[1] = usuarioViejo;
                        //Modificación exitosa
                        objRespuesta.strMensaje = "Usuario modificado correctamente.";
                    }
                    else
                    {
                        objRespuesta.blnIndicadorTransaccion = false;
                        objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.UsuarioDuplicado;
                        objRespuesta.blnIndicadorState = 300;
                    }
                }
                else
                {
                    usuarioDA.Contrasena = HashPassword(objUser.Contrasena);
                    objRespuesta = objUserDA.Editar(usuarioDA);
                    objUserDA.SendMail(usuarioDA.CorreoUsuario, "Sutel: Recuperación de contraseña", renderedEmail);
                    objRespuesta.objObjeto[1] = usuarioViejo;
                    //modificación de contraseña exitosa
                }
                return objRespuesta;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
        }
        #endregion

        #region ConsultarTodos
        /// <summary>
        /// Consultar Todos
        /// </summary>
        /// <param name="objUser"></param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<List<Usuario>> ConsultarTodos()
        {
            Respuesta<List<Usuario>> objRespuesta = new Respuesta<List<Usuario>>();
            try
            {
                objRespuesta = objUserDA.ConsultarTodos();
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

        /// <summary>
        /// Filtro de usuarios
        /// </summary>
        /// <param name="psUsuario"></param>
        /// <param name="psNombreUsuario"></param>
        /// <returns></returns>
        public Respuesta<List<Usuario>> gFiltrarUsuarios(String psUsuario, String psNombre, String psCorreo, String psNombreOperador)
        {
            Respuesta<List<Usuario>> objRespuesta = new Respuesta<List<Usuario>>();
            try
            {
                objRespuesta = objUserDA.gFiltrarUsuarios(psUsuario, psNombre, psCorreo, psNombreOperador);
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

        #region ConsultarPorExpresion
        public Respuesta<Usuario> ConsultarPorExpresion(int id)
        {
            try
            {
                objRespuesta = objUserDA.Single(x => x.IdUsuario == id);
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
        public Respuesta<Usuario> ConsultarPorExpresion(System.Linq.Expressions.Expression<Func<Usuario, bool>> expression)
        {
            try
            {
                objRespuesta = objUserDA.Single(expression);
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

        #region Eliminar
        /// <summary>
        /// Elimina un usuario
        /// </summary>
        /// <param name="objUser">Recibe un objecto Usuario</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Usuario> Eliminar(Usuario objUser)
        {
            Respuesta<Usuario> objRespuesta = new Respuesta<Usuario>();
            try
            {
                Usuario user = objUserDA.Single(x => x.IdUsuario == objUser.IdUsuario).objObjeto;
                var adminUser = ConfigurationManager.AppSettings["AdministratorUser"].ToString();
                if (user.AccesoUsuario == adminUser)
                {
                    objRespuesta.blnIndicadorState = 500;
                    objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.CantDeleteUser;
                }
                else
                {
                    objRespuesta = objUserDA.Eliminar(objUser);
                }
                return objRespuesta;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
        }
        #endregion

        #region LOGIN
        public Usuario Login(Usuario objUsuario, string renderedEmail, string newPassword)
        {
            try
            {
                SutelAD objSutelAD = new SutelAD(AppContext);
                var user = objUserDA.Single(x => x.AccesoUsuario == objUsuario.AccesoUsuario && x.Borrado == 0 && x.Activo == 1).objObjeto;
                if (user != null)
                {
                    if (user.UsuarioInterno == 0)
                    {
                        if (user == null)
                            return null;
                        else
                        {
                            if (VerifyHashedPassword(user.Contrasena, objUsuario.Contrasena))
                            {
                                if (user.UsuarioLogin == null)
                                {
                                    user.UsuarioLogin = new UsuarioLogin()
                                    {
                                        IdUsuario = user.IdUsuario,
                                        Intentos = 3,
                                        PrimerLogueo = true,
                                        UltimoLogueoExitoso = DateTime.Now
                                    };
                                }
                                else
                                {
                                    user.UsuarioLogin.Intentos = 3;
                                    user.UsuarioLogin.UltimoLogueoExitoso = DateTime.Now;
                                }
                                objUserDA.Editar(user);
                                return user;
                            }
                            else
                            {
                                if (user.UsuarioLogin == null)
                                {
                                    user.UsuarioLogin = new UsuarioLogin()
                                    {
                                        IdUsuario = user.IdUsuario,
                                        Intentos = 2,
                                        UltimoLogueoFallido = DateTime.Now
                                    };
                                }
                                else
                                {
                                    user.UsuarioLogin.Intentos = user.UsuarioLogin.Intentos == 0
                                        ? Convert.ToByte("2")
                                        : Convert.ToByte(int.Parse((user.UsuarioLogin.Intentos - 1).ToString()).ToString());
                                    user.UsuarioLogin.UltimoLogueoFallido = DateTime.Now;
                                }
                                bool zero = user.UsuarioLogin.Intentos == 0 ? true : false;
                                if (zero)
                                {
                                    user.Contrasena = HashPassword(newPassword);
                                    user.UsuarioLogin.PrimerLogueo = true;
                                }
                                objUserDA.Editar(user);
                                if (zero)
                                    objUserDA.SendMail(user.CorreoUsuario, "Sutel: Reinicio de contraseña", renderedEmail);
                                return user;
                            }
                        }
                    }
                    else
                    {
                        bool isValid = objSutelAD.LogIn(objUsuario.AccesoUsuario, objUsuario.Contrasena);
                        if (isValid)
                            return user;
                        else return null;
                    }
                }
                else return null;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
        }
        #endregion

        #region Change Password 
        public Respuesta<Usuario[]> ChangePass(Usuario Usuario, string oldPassword, string renderedEmail = "")
        {
            try
            {
                Respuesta<Usuario[]> objRespuesta = new Respuesta<Usuario[]>();
                var usuarioDA = objUserDA.Single(x => x.AccesoUsuario == Usuario.AccesoUsuario && x.Borrado == 0
                    && x.Activo == 1 && x.UsuarioInterno == 0).objObjeto;
                if (usuarioDA != null)
                {
                    if (!VerifyHashedPassword(usuarioDA.Contrasena, Usuario.Contrasena))
                    {
                        if (VerifyHashedPassword(usuarioDA.Contrasena, oldPassword)
                            || usuarioDA.UsuarioLogin.PrimerLogueo == true)
                        {
                            Usuario UsuarioViejo = new Usuario();
                            UsuarioViejo.AccesoUsuario = usuarioDA.AccesoUsuario;
                            UsuarioViejo.Contrasena = usuarioDA.Contrasena;
                            UsuarioViejo.IdUsuario = usuarioDA.IdUsuario;
                            usuarioDA.Contrasena = HashPassword(Usuario.Contrasena);
                            usuarioDA.UsuarioLogin.PrimerLogueo = false;
                            objRespuesta = objUserDA.Editar(usuarioDA);
                            //objUserDA.SendMail(usuarioDA.CorreoUsuario, "Sutel: Cambio de contraseña", renderedEmail);
                            objRespuesta.objObjeto[1] = UsuarioViejo;
                            objRespuesta.strMensaje = "La contraseña se ha actualizado correctamente.";
                        }
                        else
                        {
                            objRespuesta.blnIndicadorState = 405;
                            objRespuesta.strMensaje = "La contraseña actual no coincide.";
                        }
                    }
                    else
                    {
                        objRespuesta.blnIndicadorState = 406;
                        objRespuesta.strMensaje = "La contraseña no puede ser igual a la actual.";
                    }
                }
                else
                {
                    objRespuesta.blnIndicadorState = 404;
                    objRespuesta.strMensaje = "No se puede cambiar la contraseña del usuario actual.";
                }
                return objRespuesta;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
        }
        #endregion

        #region Reset Password
        public Respuesta<Usuario[]> ResetPass(Usuario Usuario, string renderedEmail)
        {
            try
            {
                Respuesta<Usuario[]> objRespuesta = new Respuesta<Usuario[]>();
                if (!Usuario.AccesoUsuario.ToUpper().Equals(ConfigurationManager.AppSettings["passwordMailingDBProfile"].ToString().ToUpper()))
                {
                    var usuarioDA = objUserDA.Single(x => x.AccesoUsuario == Usuario.AccesoUsuario && x.Borrado == 0 && x.Activo == 1).objObjeto;
                    if (usuarioDA != null)
                    {
                        if (usuarioDA.UsuarioInterno == 0)
                        {
                            Usuario UsuarioViejo = new Usuario();
                            UsuarioViejo.AccesoUsuario = usuarioDA.AccesoUsuario;
                            UsuarioViejo.Contrasena = usuarioDA.Contrasena;
                            usuarioDA.IdUsuario = usuarioDA.IdUsuario;
                            usuarioDA.Contrasena = HashPassword(Usuario.Contrasena);

                            if (usuarioDA.UsuarioLogin == null)
                                usuarioDA.UsuarioLogin = new UsuarioLogin()
                                {
                                    IdUsuario = usuarioDA.IdUsuario
                                };
                            else
                            {
                                usuarioDA.UsuarioLogin.PrimerLogueo = true;
                                usuarioDA.UsuarioLogin.Intentos = 3;
                            }

                            objRespuesta = objUserDA.Editar(usuarioDA);
                            objUserDA.SendMail(usuarioDA.CorreoUsuario, "Sutel: Recuperación de contraseña", renderedEmail);
                            objRespuesta.objObjeto[1] = UsuarioViejo;
                            objRespuesta.strMensaje = "Un correo se ha enviado a su buzón con la nueva contraseña temporal.";

                        }
                        else
                        {
                            objRespuesta.strMensaje = "No se puede reestablecer la contraseña de un usuario interno.";
                            objRespuesta.blnIndicadorState = 407;
                        }
                    }
                    else
                    {
                        objRespuesta.strMensaje = "Este usuario no se encuentra.";
                        objRespuesta.blnIndicadorState = 404;
                    }
                }
                else
                {
                    objRespuesta.strMensaje = "No se puede realizar el reestablecimiento de la contraseña.";
                    objRespuesta.blnIndicadorState = 406;
                }
                return objRespuesta;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
        }
        #endregion

        public bool PrimerLogueo(string userName)
        {
            try
            {
                var usuarioDA = objUserDA.Single(x => x.AccesoUsuario == userName
                    && x.Borrado == 0 && x.Activo == 1 && x.UsuarioInterno == 0).objObjeto;
                if (usuarioDA != null)
                    return usuarioDA.UsuarioLogin == null ? false : usuarioDA.UsuarioLogin.PrimerLogueo;
                else
                    return false;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
        }
        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null || password == null)
            {
                return false;
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return buffer3.SequenceEqual(buffer4);
            //return ByteArraysEqual(buffer3, buffer4);
        }

        public string generatePassword()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var pass = new string(
                Enumerable.Repeat(chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return pass;
        }
    }
}
