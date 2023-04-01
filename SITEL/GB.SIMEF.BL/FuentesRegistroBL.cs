using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using Newtonsoft.Json;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL
{
    public class FuentesRegistroBL : IMetodos<FuenteRegistro>
    {
        private readonly FuentesRegistroDAL clsDatos;
        private readonly FuentesRegistroDestinatarioDAL clsDatosDestinatario;
        private readonly UsuarioFonatelBL clsDatosUsuarioBL;

        private RespuestaConsulta<List<FuenteRegistro>> ResultadoConsulta;
        string modulo = string.Empty;
        string user = string.Empty;

        public FuentesRegistroBL(string modulo, string user)
        {
            this.modulo = modulo;
            this.user = user;
            this.clsDatos = new FuentesRegistroDAL();
            this.clsDatosDestinatario = new FuentesRegistroDestinatarioDAL();
            this.clsDatosUsuarioBL = new UsuarioFonatelBL(modulo, user);
            this.ResultadoConsulta = new RespuestaConsulta<List<FuenteRegistro>>();
        }

        /// <summary>
        /// Evalua si la fuente genero cambios para actualizar
        /// Michael Hernández Cordero
        /// 24/08/2022
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>

        public RespuestaConsulta<List<FuenteRegistro>> ActualizarElemento(FuenteRegistro objeto)
        {
            try
            {
                if (!string.IsNullOrEmpty(objeto.id))
                {
                    objeto.id = Utilidades.Desencriptar(objeto.id);
                    int temp;
                    if (int.TryParse(objeto.id, out temp))
                    {
                        objeto.IdFuenteRegistro = temp;
                    }
                    ResultadoConsulta.Clase = modulo;
                    ResultadoConsulta.Accion =  (int) Constantes. Accion.Editar;
                    ResultadoConsulta.Usuario = user;

                    objeto.UsuarioModificacion = user;

                    string fuente = objeto.Fuente.Trim();
                    if (objeto.CantidadDestinatario == null)
                    {
                        objeto.CantidadDestinatario = 0;
                    }
                    int Cantidad = (int)objeto.CantidadDestinatario;
                    
                    var consultardatos = clsDatos.ObtenerDatos(new FuenteRegistro());

                    var resul = consultardatos.Where(x => x.IdFuenteRegistro == objeto.IdFuenteRegistro).ToList();

                    string valorAnterior = resul.Where(x=>x.IdFuenteRegistro==objeto.IdFuenteRegistro).Single().ToString();

                    objeto = resul.Where(x => x.IdFuenteRegistro == objeto.IdFuenteRegistro).Single();


                    if (consultardatos.Where(x => x.IdFuenteRegistro == objeto.IdFuenteRegistro).Count() == 0)
                    {
                        ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                        throw new Exception(Errores.NoRegistrosActualizar);
                    }
                   else if (consultardatos.Where(x => x.IdFuenteRegistro != objeto.IdFuenteRegistro && x.Fuente.ToUpper()==fuente.ToUpper()).Count() > 0)
                    {
                        ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                        throw new Exception(Errores.FuenteRegistrada);
                    }
                    
                    else if (Cantidad < objeto.DetalleFuenteRegistro.Count())
                    {
                        ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                        throw new Exception(Errores.CantidadRegistrosLimiteFuente);
                    }
                   
                    else
                    {

                        objeto.Fuente = fuente;
                        objeto.CantidadDestinatario = Cantidad;
                        if (objeto.IdEstadoRegistro == 2)
                        {
                            if(objeto.CantidadDestinatario > objeto.DetalleFuenteRegistro.Count())
                            {
                                objeto.IdEstadoRegistro = (int)Constantes.EstadosRegistro.EnProceso;
                            }
                            
                        }
                        else
                        {
                            objeto.IdEstadoRegistro = (int)Constantes.EstadosRegistro.EnProceso;
                        }
                        
                        clsDatos.ActualizarDatos(objeto);

                        var nuevovalor = clsDatos.ObtenerDatos(objeto).Single();

                        string jsonNuevoValor = nuevovalor.ToString();
                    
                        ResultadoConsulta.objetoRespuesta = resul;
                        ResultadoConsulta.CantidadRegistros = resul.Count();
                        clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                               ResultadoConsulta.Usuario,
                               ResultadoConsulta.Clase,nuevovalor.Fuente, jsonNuevoValor,valorAnterior,"");
                    }
                }
            }
            catch (Exception ex)
            {

                if (ResultadoConsulta.HayError != (int)Constantes.Error.ErrorControlado)
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
        public RespuestaConsulta<List<FuenteRegistro>> CambioEstado(FuenteRegistro objeto)
        {
            try
            {
                string jsonAnterior = ObtenerDatos(objeto).objetoRespuesta.FirstOrDefault().ToString();

                if (!String.IsNullOrEmpty(objeto.id))
                {
                    objeto.id = Utilidades.Desencriptar(objeto.id);
                    int temp;
                    if (int.TryParse(objeto.id, out temp))
                    {
                        objeto.IdFuenteRegistro = temp;
                    }
                }
                ResultadoConsulta.Clase = modulo;
                int nuevoEstado = (int)Constantes.EstadosRegistro.Activo;
                ResultadoConsulta.Usuario = user;     
                objeto.UsuarioModificacion=user;
                var resul = clsDatos.ObtenerDatos(objeto).ToList();
                var fuente = resul.Single();
                if (resul.Count() == 0)
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }
                else if (fuente.CantidadDestinatario != fuente.DetalleFuenteRegistro.Count())
                {
                    ResultadoConsulta.HayError= (int)Constantes.Error.ErrorControlado;
                    throw new Exception(Errores.CantidadDestinatariosIncorrecta);
                }
                else if (fuente.CantidadDestinatario ==0)
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                    throw new Exception(Errores.FuentesCantidadDestinatarios);
                }
                else
                {
                    objeto = fuente;
                    objeto.IdEstadoRegistro = nuevoEstado;
                    objeto.UsuarioModificacion = ResultadoConsulta.Usuario;
                    ResultadoConsulta.Accion = (int)Constantes.Accion.Activar;
                    resul = clsDatos.ActualizarDatos(objeto);
                    string jsonActual = resul.FirstOrDefault().ToString();
                    ResultadoConsulta.objetoRespuesta = resul;
                    ResultadoConsulta.CantidadRegistros = resul.Count();
                    clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                           ResultadoConsulta.Usuario,
                           ResultadoConsulta.Clase, objeto.Fuente, jsonActual, jsonAnterior);
                }

            }
            catch (Exception ex)
            {
                if (ResultadoConsulta.HayError != (int)Constantes.Error.ErrorControlado)
                { 
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                }
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<FuenteRegistro>> ClonarDatos(FuenteRegistro objeto)
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

        public RespuestaConsulta<List<FuenteRegistro>> EliminarElemento(FuenteRegistro objeto)
        {
            try
            {
                if (!String.IsNullOrEmpty(objeto.id))
                {
                    objeto.id = Utilidades.Desencriptar(objeto.id);
                    int temp;
                    if (int.TryParse(objeto.id, out temp))
                    {
                        objeto.IdFuenteRegistro = temp;
                    }
                }
                ResultadoConsulta.Clase = modulo;
                int nuevoEstado = (int)Constantes.EstadosRegistro.Eliminado;
                ResultadoConsulta.Usuario = user;
                objeto.UsuarioModificacion=user;
                var resul = clsDatos.ObtenerDatos(objeto).ToList();

                if (resul.Count() == 0)
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }
                else
                {
                    //89152 Se agrega funcionalidad para cuando se elimina la fuente se elimine los usuarios sitel
                    DetalleFuenteRegistro destinatario = new DetalleFuenteRegistro();
                    destinatario.idFuenteRegistro = objeto.IdFuenteRegistro;
                    List<DetalleFuenteRegistro> destinatarios = clsDatosDestinatario.ObtenerDatos(destinatario);

                    foreach (var item in destinatarios)
                    {
                        Usuario nuevoUsuario = new Usuario();
                        nuevoUsuario.IdUsuario = item.idUsuario;
                        clsDatosUsuarioBL.EliminarElemento(nuevoUsuario);
                    }

                    objeto = resul.Single();
                    objeto.IdEstadoRegistro = nuevoEstado;
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
                if (ResultadoConsulta.HayError != (int)Constantes.Error.ErrorControlado)
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
        public RespuestaConsulta<List<FuenteRegistro>> InsertarDatos(FuenteRegistro objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Constantes.Accion.Insertar;
                ResultadoConsulta.Usuario = user;

                objeto.UsuarioCreacion = user;
                objeto.Fuente = objeto.Fuente.Trim();
                objeto.IdEstadoRegistro = (int)EstadosRegistro.EnProceso;
                var consultardatos = clsDatos.ObtenerDatos(new FuenteRegistro());
                if (consultardatos.Where(x=>x.Fuente.ToUpper()==objeto.Fuente.ToUpper()).Count()>0)
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                    throw new Exception(Errores.FuenteRegistrada);
                }
                if(objeto.CantidadDestinatario == null)
                {
                    objeto.CantidadDestinatario = 0;
                } 
                var resul = clsDatos.ActualizarDatos(objeto);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();
                string JsonNuevoValor = resul.Where(x=>x.Fuente==objeto.Fuente.ToUpper()).Single().ToString();
                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                     ResultadoConsulta.Usuario,
                     ResultadoConsulta.Clase, objeto.Fuente,"","",JsonNuevoValor);
            }
            catch (Exception ex)
            {
                if (ResultadoConsulta.HayError != (int)Constantes.Error.ErrorControlado)
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
        public RespuestaConsulta<List<FuenteRegistro>> ObtenerDatos(FuenteRegistro objFuentesRegistro)
        {
            try
            {
                if (!string.IsNullOrEmpty(objFuentesRegistro.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objFuentesRegistro.id), out temp);
                    objFuentesRegistro.IdFuenteRegistro = temp;
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


        public RespuestaConsulta<List<string>> ValidarExistencia(FuenteRegistro objeto)
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
                        objeto.IdFuenteRegistro = temp;
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



        public RespuestaConsulta<List<FuenteRegistro>> ValidarDatos(FuenteRegistro objeto)
        {
            throw new NotImplementedException();
        }
    }
}
