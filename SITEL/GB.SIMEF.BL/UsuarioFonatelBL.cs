using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL
{
    public class UsuarioFonatelBL : IMetodos<UsuarioFonatel>
    {
        readonly string modulo = "";
        readonly string user = "";
        private readonly UsuarioFonatelDAL usuarioFonatelDAL;
        private readonly PlantillaHtmlDAL plantillaHtmlDAL;
        private RespuestaConsulta<List<UsuarioFonatel>> resultado;
        public UsuarioFonatelBL(string modulo, string usuario)
        {
            this.usuarioFonatelDAL = new UsuarioFonatelDAL();
            this.modulo = modulo;
            this.user = usuario;
            this.resultado = new RespuestaConsulta<List<UsuarioFonatel>>();
            this.plantillaHtmlDAL = new PlantillaHtmlDAL();
        }

        private UsuarioFonatel ValidarObjeto(UsuarioFonatel objeto, bool agregar=false)
        {
            var consultardatos = usuarioFonatelDAL.ObtenerDatos();
            objeto.CorreoUsuario = objeto.CorreoUsuario.Trim().ToUpper();
            objeto.NombreUsuario = objeto.NombreUsuario.Trim().ToUpper();
            if (agregar==false)
            {
                objeto.Activo = false;
                objeto.borrado = false;
                objeto.FONATEL = true;
                objeto.EnviaCorreo = false;
            }
            objeto.ContrasenaSinEncriptar = generatePassword();
            objeto.Contrasena = HashPassword(objeto.ContrasenaSinEncriptar);
            objeto.AccesoUsuario = objeto.CorreoUsuario;
            if (!Utilidades.ValidarEmail(objeto.CorreoUsuario))
            {
                resultado.HayError = (int)Constantes.Error.ErrorControlado;
                throw new Exception(string.Format(Errores.CampoConFormatoInvalido, "Correo electrónico"));
            }
            else if (consultardatos.Where(x => x.CorreoUsuario.ToUpper() == objeto.CorreoUsuario).Count() > 0 && agregar)
            {
                resultado.HayError = (int)Constantes.Error.ErrorControlado;
                throw new Exception(Errores.CorreoRegistrado);
            }
            else if (objeto.IdUsuario == 0 && !agregar)
            {
                resultado.HayError = (int)Constantes.Error.ErrorControlado;
                throw new Exception(Errores.NoRegistrosActualizar);
            }
            return objeto;
        }
        public RespuestaConsulta<List<UsuarioFonatel>> ActualizarElemento(UsuarioFonatel objeto)
        {
            try
            {
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Insertar;
                objeto = ValidarObjeto(objeto);
                resultado.objetoRespuesta = usuarioFonatelDAL.ActualizarUsuarioSitel(objeto);
                resultado.CantidadRegistros = resultado.objetoRespuesta.Count();
            }
            catch (Exception ex)
            {
                if (resultado.HayError != (int)Constantes.Error.ErrorControlado)
                {
                    resultado.HayError = (int)Constantes.Error.ErrorSistema;
                }

                resultado.MensajeError = ex.Message;
            }
            return resultado;
        }

        private void EnviarCorreo(UsuarioFonatel objeto)
        {
            PlantillaHtml plantilla = plantillaHtmlDAL.ObtenerDatos((int)Constantes.PlantillaCorreoEnum.CrearUsuario);

            plantilla.Html = string.Format(plantilla.Html,  objeto.NombreUsuario, objeto.CorreoUsuario, objeto.ContrasenaSinEncriptar);          
            CorreoDal correo = new CorreoDal(objeto.CorreoUsuario, "", plantilla.Html, "Envío de Credenciales");
            correo.EnviarCorreo();
        }

        public RespuestaConsulta<List<UsuarioFonatel>> CambioEstado(UsuarioFonatel objeto)
        {
            try
            {
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Activar;
                objeto = usuarioFonatelDAL.ObtenerDatos().Where(x => x.IdUsuario == objeto.IdUsuario).Single();
                objeto.Activo = true;
                objeto.ContrasenaSinEncriptar = generatePassword();
                objeto.Contrasena = HashPassword(objeto.ContrasenaSinEncriptar);
                resultado.objetoRespuesta = usuarioFonatelDAL.ActualizarUsuarioSitel(objeto);
                resultado.CantidadRegistros = resultado.objetoRespuesta.Count();
                EnviarCorreo(objeto);
            }
            catch (Exception ex)
            {
                if (resultado.HayError != (int)Constantes.Error.ErrorControlado)
                {
                    resultado.HayError = (int)Constantes.Error.ErrorSistema;
                }

                resultado.MensajeError = ex.Message;
            }
            return resultado;
        }

        public RespuestaConsulta<List<UsuarioFonatel>> ClonarDatos(UsuarioFonatel objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<UsuarioFonatel>> EliminarElemento(UsuarioFonatel objeto)
        {
            try
            {
                objeto = usuarioFonatelDAL.ObtenerDatos().Where(X => X.IdUsuario == objeto.IdUsuario).Single();
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Insertar;
                objeto.borrado = true;
                objeto.Activo = false;
                resultado.objetoRespuesta = usuarioFonatelDAL.ActualizarUsuarioSitel(objeto);
                resultado.CantidadRegistros = resultado.objetoRespuesta.Count();

            }
            catch (Exception ex)
            {
                if (resultado.HayError != (int)Constantes.Error.ErrorControlado)
                {
                    resultado.HayError = (int)Constantes.Error.ErrorSistema;
                }

                resultado.MensajeError = ex.Message;
            }
            return resultado;
        }



        public RespuestaConsulta<List<UsuarioFonatel>> ObtenerDatos(UsuarioFonatel objeto)
        {
           
            try
            {
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                var result = usuarioFonatelDAL.ObtenerDatos();
                resultado.objetoRespuesta = result;
                resultado.CantidadRegistros = result.Count();
            }
            catch (Exception ex)
            {
                resultado.HayError = (int)Constantes.Error.ErrorSistema;
                resultado.MensajeError = ex.Message;
            }
            return resultado;
        }

        public RespuestaConsulta<List<UsuarioFonatel>> InsertarDatos(UsuarioFonatel objeto)
        {
            try
            {
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Insertar;
                objeto = ValidarObjeto(objeto,true);
                resultado.objetoRespuesta = usuarioFonatelDAL.ActualizarUsuarioSitel(objeto);
                resultado.CantidadRegistros = resultado.objetoRespuesta.Count();

            }
            catch (Exception ex)
            {
                if (resultado.HayError != (int)Constantes.Error.ErrorControlado)
                {
                    resultado.HayError = (int)Constantes.Error.ErrorSistema;
                }
              
                resultado.MensajeError = ex.Message;
            }
            return resultado;
      



      
               
                //string html = string.Format(Resources.PlantillasCorreo.HtmlCrearUsuario, item.NombreDestinatario, contrasena);
                //CorreoDal correoDal = new CorreoDal(item.CorreoElectronico, "", html, "Sutel: Creación de Usuario");
                //int result = correoDal.EnviarCorreo();
                //if (result == 0)
                //{
                    //throw new Exception("Error enviando correo: " + item.CorreoElectronico);
                //}


            

        }

        private static string HashPassword(string password)
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



        public RespuestaConsulta<List<UsuarioFonatel>> ValidarDatos(UsuarioFonatel objeto)
        {
            throw new NotImplementedException();
        }
    }
}
