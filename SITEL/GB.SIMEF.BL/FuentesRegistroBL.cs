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
    public class FuentesRegistroBL : IMetodos<FuentesRegistro>
    {
        private readonly FuentesRegistroDAL clsDatos;
        private readonly FuentesRegistroDestinatarioDAL clsDatosUsuario;

        private RespuestaConsulta<List<FuentesRegistro>> ResultadoConsulta;
        string modulo = string.Empty;
        string user = string.Empty;

        public FuentesRegistroBL(string modulo, string user)
        {
            this.modulo = modulo;
            this.user = user;
            this.clsDatos = new FuentesRegistroDAL();
            this.clsDatosUsuario = new FuentesRegistroDestinatarioDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<FuentesRegistro>>();
        }

        private string SerializarObjetoBitacora(FuentesRegistro objFuente)
        {
            return JsonConvert.SerializeObject(objFuente, new JsonSerializerSettings
            { ContractResolver = new JsonIgnoreResolver(objFuente.NoSerialize) });
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
                if (!string.IsNullOrEmpty(objeto.id))
                {
                    objeto.id = Utilidades.Desencriptar(objeto.id);
                    int temp;
                    if (int.TryParse(objeto.id, out temp))
                    {
                        objeto.idFuente = temp;
                    }
                    ResultadoConsulta.Clase = modulo;
                    ResultadoConsulta.Accion =  (int) Constantes. Accion.Editar;
                    ResultadoConsulta.Usuario = user;

                    objeto.UsuarioModificacion = user;

                    string fuente = objeto.Fuente.Trim();
                    int Cantidad = objeto.CantidadDestinatario;

                    var resul = clsDatos.ObtenerDatos(new FuentesRegistro());
                    string valorAnterior = SerializarObjetoBitacora(resul.Where(x=>x.idFuente==objeto.idFuente).Single())  ;
                    objeto = resul.Where(x => x.idFuente == objeto.idFuente).Single();


                    if (objeto.CantidadDestinatario == Cantidad && objeto.Fuente == fuente.ToUpper())
                    {
                        ResultadoConsulta.objetoRespuesta= resul.Where(x => x.idFuente == objeto.idFuente).ToList();
                        ResultadoConsulta.CantidadRegistros = 1;
                        return ResultadoConsulta;
                    }
                    else if (resul.Where(x => x.idFuente == objeto.idFuente).Count() == 0)
                    {
                        ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                        throw new Exception(Errores.NoRegistrosActualizar);
                    }
                   else if (resul.Where(x => x.idFuente != objeto.idFuente && x.Fuente.ToUpper()==fuente.ToUpper()).Count() > 0)
                    {
                        ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                        throw new Exception(Errores.FuenteRegistrada);
                    }
                    else if (Cantidad <= 0)
                    {
                        ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                        throw new Exception(Errores.FuentesCantidadDestinatarios);
                    }
                    else if (Cantidad < objeto.DetalleFuentesRegistro.Count())
                    {
                        ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                        throw new Exception(Errores.CantidadRegistrosLimite);
                    }
                   
                    else
                    {

                        objeto.Fuente = fuente;
                        objeto.CantidadDestinatario = Cantidad;

                        clsDatos.ActualizarDatos(objeto);

                        var nuevovalor = clsDatos.ObtenerDatos(objeto).Single();

                        string jsonNuevoValor = SerializarObjetoBitacora(nuevovalor);
                    
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
                ResultadoConsulta.Usuario = user;     
                objeto.UsuarioModificacion=user;
                var resul = clsDatos.ObtenerDatos(objeto).ToList();
                var fuente = resul.Single();
                if (resul.Count() == 0)
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }
                else if (fuente.CantidadDestinatario != fuente.DetalleFuentesRegistro.Count())
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
                    objeto.idEstado = nuevoEstado;
                    objeto.UsuarioModificacion = ResultadoConsulta.Usuario;
                    ResultadoConsulta.Accion = (int)Constantes.Accion.Activar;
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
        public RespuestaConsulta<List<FuentesRegistro>> InsertarDatos(FuentesRegistro objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Constantes.Accion.Insertar;
                ResultadoConsulta.Usuario = user;

                objeto.UsuarioCreacion = user;
                objeto.Fuente = objeto.Fuente.Trim();
                objeto.idEstado = (int)EstadosRegistro.EnProceso;
                var consultardatos = clsDatos.ObtenerDatos(new FuentesRegistro());
                if (consultardatos.Where(x=>x.Fuente.ToUpper()==objeto.Fuente.ToUpper()).Count()>0)
                {
                    throw new Exception(Errores.FuenteRegistrada);
                }
                if (objeto.CantidadDestinatario<=0)
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                    throw new Exception(Errores.FuentesCantidadDestinatarios);
                }
                var resul = clsDatos.ActualizarDatos(objeto);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();
                string JsonNuevoValor = SerializarObjetoBitacora(resul.Where(x=>x.Fuente==objeto.Fuente.ToUpper()).Single());
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
    }
}
