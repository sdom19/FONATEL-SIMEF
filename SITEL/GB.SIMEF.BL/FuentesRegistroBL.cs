using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;

using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL
{
    public class FuentesRegistroBL : IMetodos<FuentesRegistro>
    {
        private readonly FuentesRegistroDAL clsDatos;
        private readonly FuentesRegistroDestinatarioDAL clsDatosUsuario;

        private RespuestaConsulta<List<FuentesRegistro>> ResultadoConsulta;
        string modulo = EtiquetasViewFuentesRegistro.FuentesRegistro;

        public FuentesRegistroBL()
        {
            this.clsDatos = new FuentesRegistroDAL();
            this.clsDatosUsuario = new FuentesRegistroDestinatarioDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<FuentesRegistro>>();
        }
        /// <summary>
        /// Evalua si la fuente genero cambios para actualizar
        /// Michael Hernández Cordero
        /// 24/08/2022
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>

        public RespuestaConsulta<List<FuentesRegistro>> ActualizarElemento(FuentesRegistro objeto)
        {


            try
            {
                if (!String.IsNullOrEmpty(objeto.id))
                {
                    objeto.id = Utilidades.Desencriptar(objeto.id);
                    int temp;
                    if (int.TryParse(objeto.id, out temp))
                    {
                        objeto.idFuente = temp;
                    }
                    ResultadoConsulta.Clase = modulo;
                    ResultadoConsulta.Accion =  (int) Constantes. Accion.Editar;
                    ResultadoConsulta.Usuario = objeto.UsuarioModificacion;

                    string fuente = objeto.Fuente.Trim();
                    int Cantidad = objeto.CantidadDestinatario;

                    var resul = clsDatos.ObtenerDatos(new FuentesRegistro());
                    objeto = resul.Where(x => x.idFuente == objeto.idFuente).Single();


                    if (objeto.CantidadDestinatario == Cantidad && objeto.Fuente == fuente.ToUpper())
                    {
                        ResultadoConsulta.objetoRespuesta= resul.Where(x => x.idFuente == objeto.idFuente).ToList();
                        ResultadoConsulta.CantidadRegistros = 1;
                        return ResultadoConsulta;
                    }
                    else if (resul.Where(x => x.idFuente == objeto.idFuente).Count() == 0)
                    {
                        throw new Exception(Errores.NoRegistrosActualizar);
                    }
                   else if (resul.Where(x => x.idFuente != objeto.idFuente && x.Fuente.ToUpper()==fuente.ToUpper()).Count() > 0)
                    {
                        throw new Exception(Errores.FuenteRegistrada);
                    }
                    else if (Cantidad <= 0)
                    {
                        throw new Exception(Errores.FuentesCantidadDestiatarios);
                    }
                    else if (Cantidad < objeto.DetalleFuentesRegistro.Count())
                    {
                        throw new Exception(Errores.CantidadRegistrosLimite);
                    }
                   
                    else
                    {

                        objeto.Fuente = fuente;
                        objeto.CantidadDestinatario = Cantidad;

                        resul = clsDatos.ActualizarDatos(objeto);
                        ResultadoConsulta.objetoRespuesta = resul;
                        ResultadoConsulta.CantidadRegistros = resul.Count();
                        clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                               ResultadoConsulta.Usuario,
                               ResultadoConsulta.Clase, objeto.Fuente);
                    }
                }
            }
            catch (Exception ex)
            {

                if (ex.Message == Errores.NoRegistrosActualizar || ex.Message == Errores.FuentesCantidadDestiatarios 
                    || ex.Message == Errores.CantidadRegistrosLimite || ex.Message== Errores.FuenteRegistrada)
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                }
                else
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                }


                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }
        /// <summary>
        /// Activa la fuente en el proceso de finalizar
        /// Michael Hernández Cordero
        /// 24-08-2022
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<FuentesRegistro>> CambioEstado(FuentesRegistro objeto)
        {
            try
            {
                if (!String.IsNullOrEmpty(objeto.id))
                {
                    objeto.id = Utilidades.Desencriptar(objeto.id);
                    int temp;
                    if (int.TryParse(objeto.id, out temp))
                    {
                        objeto.idFuente = temp;
                    }
                }
                ResultadoConsulta.Clase = modulo;
                int nuevoEstado = (int)Constantes.EstadosRegistro.Activo;
                ResultadoConsulta.Usuario = objeto.UsuarioModificacion;
                var resul = clsDatos.ObtenerDatos(objeto).ToList();
                var fuente = resul.Single();
                if (resul.Count() == 0)
                {
                    throw new Exception(Errores.NoRegistrosActualizar);
                }
                else if (fuente.CantidadDestinatario != fuente.DetalleFuentesRegistro.Count())
                {
                    throw new Exception(Errores.CantidadDestinatariosIncorrecta);
                }
                else if (fuente.CantidadDestinatario ==0)
                {
                    throw new Exception(Errores.FuentesCantidadDestiatarios);
                }
                else
                {
                    objeto = fuente;
                    objeto.idEstado = nuevoEstado;
                    objeto.UsuarioModificacion = ResultadoConsulta.Usuario;
                    ResultadoConsulta.Accion = (int)Constantes.Accion.Activar;
                    resul = clsDatos.ActualizarDatos(objeto);
                    ResultadoConsulta.objetoRespuesta = resul;
                    ResultadoConsulta.CantidadRegistros = resul.Count();

                    CrearUsuarios(fuente);

                    clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                           ResultadoConsulta.Usuario,
                           ResultadoConsulta.Clase, objeto.Fuente);
                }

            }
            catch (Exception ex)
            {
                if (ex.Message == Errores.NoRegistrosActualizar || ex.Message== Errores.CantidadDestinatariosIncorrecta || ex.Message== Errores.FuentesCantidadDestiatarios)
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                }
                else
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                }


                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<FuentesRegistro>> ClonarDatos(FuentesRegistro objeto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Elimina la fuente desde de manera lógica, estado 4
        /// Michael Hernández Cordero
        /// 24/08/2022
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>

        public RespuestaConsulta<List<FuentesRegistro>> EliminarElemento(FuentesRegistro objeto)
        {
            try
            {
                if (!String.IsNullOrEmpty(objeto.id))
                {
                    objeto.id = Utilidades.Desencriptar(objeto.id);
                    int temp;
                    if (int.TryParse(objeto.id, out temp))
                    {
                        objeto.idFuente = temp;
                    }
                }
                ResultadoConsulta.Clase = modulo;
                int nuevoEstado = (int)Constantes.EstadosRegistro.Eliminado;
                ResultadoConsulta.Usuario = objeto.UsuarioModificacion;
                var resul = clsDatos.ObtenerDatos(objeto).ToList();

                if (resul.Count() == 0)
                {
                    throw new Exception(Errores.NoRegistrosActualizar);
                }
                else
                {

                    objeto = resul.Single();
                    objeto.idEstado = nuevoEstado;
                    objeto.UsuarioModificacion = ResultadoConsulta.Usuario;
                    ResultadoConsulta.Accion = (int)Constantes.Accion.Eliminar;
                    resul = clsDatos.ActualizarDatos(objeto);
                    ResultadoConsulta.objetoRespuesta = resul;
                    ResultadoConsulta.CantidadRegistros = resul.Count();
              
                    clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                           ResultadoConsulta.Usuario,
                           ResultadoConsulta.Clase, objeto.Fuente);
                  

                }

            }
            catch (Exception ex)
            {
                if (ex.Message == Errores.NoRegistrosActualizar)
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                }
                else
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                }


                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }
        /// <summary>
        /// Crea la fuente en estado en proceso 
        /// Michael Hernández Cordero
        /// 24-08-2022
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<FuentesRegistro>> InsertarDatos(FuentesRegistro objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Constantes.Accion.Insertar;
                ResultadoConsulta.Usuario = objeto.UsuarioCreacion;
                objeto.Fuente = objeto.Fuente.Trim();
                objeto.idEstado = (int)EstadosRegistro.EnProceso;
                var consultardatos = clsDatos.ObtenerDatos(new FuentesRegistro());

                if (consultardatos.Where(x=>x.Fuente.ToUpper()==objeto.Fuente.ToUpper()).Count()>0)
                {
                    throw new Exception(Errores.FuenteRegistrada);
                }


                if (objeto.CantidadDestinatario<=0)
                {
                    throw new Exception(Errores.FuentesCantidadDestiatarios);
                }



                var resul = clsDatos.ActualizarDatos(objeto);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();
                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                     ResultadoConsulta.Usuario,
                     ResultadoConsulta.Clase, objeto.Fuente);
            }
            catch (Exception ex)
            {
                if (ex.Message== Errores.FuenteRegistrada || ex.Message == Errores.FuentesCantidadDestiatarios)
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                }
                else
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                }

                
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }


        /// <summary>
        /// Fecha 10/08/2022
        /// Michael Hernández
        /// Metodo para obtener la lista de fuentes
        /// </summary>
        public RespuestaConsulta<List<FuentesRegistro>> ObtenerDatos(FuentesRegistro objFuentesRegistro)
        {
            try
            {
                if (!string.IsNullOrEmpty(objFuentesRegistro.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objFuentesRegistro.id), out temp);
                    objFuentesRegistro.idFuente = temp;
                }
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Constantes.Accion.Consultar;
                var resul = clsDatos.ObtenerDatos(objFuentesRegistro);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();
            }
            catch (Exception ex)
            {
                ResultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        /// <summary>
        /// Valida si existen solicitudes con la fuente para eliminar
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>


        public RespuestaConsulta<List<string>> ValidarExistencia(FuentesRegistro objeto)
        {
            RespuestaConsulta<List<string>> listaExistencias = new RespuestaConsulta<List<string>>();
            try
            {
                if (!String.IsNullOrEmpty(objeto.id))
                {
                    objeto.id = Utilidades.Desencriptar(objeto.id);
                    int temp;
                    if (int.TryParse(objeto.id, out temp))
                    {
                        objeto.idFuente = temp;
                    }
                }
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Constantes.Accion.Consultar;
                var resul = clsDatos.ObtenerDatos(objeto).Single();
                listaExistencias.objetoRespuesta = clsDatos.ValidarFuente(resul);

            }
            catch (Exception ex)
            {
                listaExistencias.HayError = (int)Constantes.Error.ErrorSistema;
                listaExistencias.MensajeError = ex.Message;
            }
            return listaExistencias;
        }



        public RespuestaConsulta<List<FuentesRegistro>> ValidarDatos(FuentesRegistro objeto)
        {
            throw new NotImplementedException();
        }



        public void CrearUsuarios( FuentesRegistro fuente)
        {
            foreach (var item in fuente.DetalleFuentesRegistro)
            {
                item.Estado = fuente.idEstado == (int)Constantes.EstadosRegistro.Activo ? true : false;
                item.Contrasena = generatePassword();
                item.Contrasena = HashPassword(item.Contrasena);
                clsDatosUsuario.ActualizarUsuario(item);
            }

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
    }
}
