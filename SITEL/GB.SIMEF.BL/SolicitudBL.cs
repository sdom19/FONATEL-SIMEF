using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL
{
    public class SolicitudBL : IMetodos<Solicitud>
    {
        private readonly SolicitudDAL clsDatos;


        private RespuestaConsulta<List<Solicitud>> ResultadoConsulta;
        string modulo = string.Empty;
        string user = string.Empty;
        private CorreoDal correoDal;
        private PlantillaHtmlDAL plantillaDal;
       
        public SolicitudBL(string modulo, string user )
        {
            this.clsDatos = new SolicitudDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<Solicitud>>();
            this.plantillaDal = new PlantillaHtmlDAL();
            this.user = user;
            this.modulo = modulo;
        }

        private string SerializarObjetoBitacora(Solicitud objeto)
        {
            return JsonConvert.SerializeObject(objeto, new JsonSerializerSettings
            { ContractResolver = new JsonIgnoreResolver(objeto.NoSerialize) });
        }


        public RespuestaConsulta<bool> EnvioCorreo(Solicitud solicitud)
        {
            RespuestaConsulta<bool> envioCorreo = new RespuestaConsulta<bool>();
            try
            {
                envioCorreo.objetoRespuesta = false;
                PlantillaHtml plantilla = plantillaDal.ObtenerDatos((int)Constantes.PlantillaCorreoEnum.EnvioSolicitud);
                solicitud = clsDatos.ObtenerDatos(solicitud).Single();
                if (solicitud.Fuente.idEstado==(int)Constantes.EstadosRegistro.Activo )
                {
                    foreach (var detalleFuente in solicitud.Fuente.DetalleFuentesRegistro.Where(x=>x.Estado==true))
                    {
                      
                        correoDal = new CorreoDal(detalleFuente.CorreoElectronico, "", plantilla.Html, "Envío de solicitud");
                        var result=correoDal.EnviarCorreo();
                        envioCorreo.objetoRespuesta = result == 0 ? false : true;
                    }
                }
                else
                {
                    envioCorreo.HayError = (int)Constantes.Error.ErrorControlado;
                    throw new Exception(Errores.SolicitudesFuenteRegistrada);
                }


            }
            catch (Exception ex)
            {
                if (envioCorreo.HayError != (int)Constantes.Error.ErrorControlado)
                {
                    envioCorreo.HayError = (int)Constantes.Error.ErrorSistema;
                }
                envioCorreo.MensajeError = ex.Message;
            }
            return envioCorreo;

        }






        public RespuestaConsulta<List<Solicitud>> ObtenerDatos(Solicitud objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;

                if (!String.IsNullOrEmpty(objeto.id))
                {
                    objeto.id = Utilidades.Desencriptar(objeto.id);
                    int temp;
                    if (int.TryParse(objeto.id, out temp))
                    {
                        objeto.idSolicitud = temp;
                    }
                }

                var resul = clsDatos.ObtenerDatos(objeto);

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

        public RespuestaConsulta<List<string>> ValidarExistenciaSolicitudEliminar(Solicitud objeto)
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
                        objeto.idSolicitud = temp;
                    }
                }
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;
                listaExistencias.objetoRespuesta = clsDatos.ValidarSolicitud(objeto);

            }
            catch (Exception ex)
            {
                listaExistencias.HayError = (int)Constantes.Error.ErrorSistema;
                listaExistencias.MensajeError = ex.Message;
            }
            return listaExistencias;
        }

        public RespuestaConsulta<List<Solicitud>> ActualizarElemento(Solicitud objeto)
        {
            try
            {
                List<Solicitud> BuscarRegistros = clsDatos.ObtenerDatos(new Solicitud());
                List<Solicitud> ValoresIniciales = clsDatos.ObtenerDatos(new Solicitud());

                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Editar;
                objeto.UsuarioModificacion = user;
                objeto.UsuarioCreacion = user;
                ResultadoConsulta.Usuario = objeto.UsuarioCreacion;


                if (!string.IsNullOrEmpty(objeto.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.id), out temp);
                    objeto.idSolicitud = temp;
                }

                var objetoAnterior = ValoresIniciales.Where(x => x.idSolicitud == objeto.idSolicitud).Single();

                var result = BuscarRegistros.Where(x => x.idSolicitud == objeto.idSolicitud).Single();

                if (BuscarRegistros.Where(x => x.idSolicitud == objeto.idSolicitud).Count() == 0)
                {
                    throw new Exception(Errores.NoRegistrosActualizar);
                }
                else if (result.SolicitudFormulario.Count > objeto.CantidadFormularios)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.CantidadRegistrosLimite);
                }
                else if (BuscarRegistros.Where(X => X.Nombre.ToUpper() == objeto.Nombre.ToUpper() && !X.idSolicitud.Equals(objeto.idSolicitud)).ToList().Count() >= 1)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.NombreRegistrado);
                }
                else if (objeto.FechaFin < objeto.FechaInicio)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.ValorFecha);
                }
                else
                {
                    result = clsDatos.ActualizarDatos(objeto)
                    .Where(x => x.Codigo.ToUpper() == objeto.Codigo.ToUpper()).FirstOrDefault();
                }

                objeto = clsDatos.ObtenerDatos(objeto).Single();
                string JsonActual = SerializarObjetoBitacora(objeto);
                string JsonAnterior = SerializarObjetoBitacora(objetoAnterior);

                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                        ResultadoConsulta.Usuario,
                            ResultadoConsulta.Clase, objeto.Codigo
                            , JsonActual, JsonAnterior, "");
            }
            catch (Exception ex)
            {

                if (ResultadoConsulta.HayError!= (int)Error.ErrorControlado)
                { 
                   
                    ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                }
                ResultadoConsulta.MensajeError = ex.Message;
            }

            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<Solicitud>> CambioEstado(Solicitud objeto)
        {
            try
            {
                if (!String.IsNullOrEmpty(objeto.id))
                {
                    objeto.id = Utilidades.Desencriptar(objeto.id);
                    int temp;
                    if (int.TryParse(objeto.id, out temp))
                    {
                        objeto.idSolicitud = temp;
                    }
                }

                ResultadoConsulta.Clase = modulo;
                int nuevoEstado = objeto.IdEstado;
                objeto.IdEstado = 0;
                ResultadoConsulta.Usuario = user;


                var resul = clsDatos.ObtenerDatos(objeto);
                objeto = resul.Single();
                objeto.IdEstado = nuevoEstado;

                ResultadoConsulta.Accion = (int)EstadosRegistro.Activo == objeto.IdEstado ? (int)Accion.Activar : (int)Accion.Inactiva;
                resul = clsDatos.ActualizarDatos(objeto);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();



                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                ResultadoConsulta.Usuario,
                ResultadoConsulta.Clase, objeto.Codigo, JsonConvert.SerializeObject(objeto), "", "");

            }
            catch (Exception ex)
            {
                ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<Solicitud>> ClonarDatos(Solicitud objeto)
        {
            try
            {        
                List<Solicitud> BuscarRegistros = clsDatos.ObtenerDatos(new Solicitud());
                
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Clonar;
                ResultadoConsulta.Usuario = user;
                objeto.UsuarioCreacion = user;

                DesencriptarSolicitud(objeto);

                var ValorInicial = BuscarRegistros.Where(x => x.idSolicitud == objeto.idSolicitud).Single();

                objeto.id = string.Empty;
                objeto.idSolicitud = 0;

                if (BuscarRegistros.Where(X => X.Codigo.ToUpper() == objeto.Codigo.ToUpper() && !X.idSolicitud.Equals(objeto.idSolicitud)).ToList().Count() > 0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.CodigoRegistrado);
                }

                if (BuscarRegistros.Where(X => X.Nombre.ToUpper() == objeto.Nombre.ToUpper() && !X.idSolicitud.Equals(objeto.idSolicitud)).ToList().Count() > 0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.NombreRegistrado);
                }
                else if (objeto.FechaFin < objeto.FechaInicio)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.ValorFecha);
                }
                else
                {
                    var resul = clsDatos.ActualizarDatos(objeto);
                    ResultadoConsulta.objetoRespuesta = resul;
                    ResultadoConsulta.CantidadRegistros = resul.Count();
                }

                objeto = clsDatos.ObtenerDatos(objeto).Single();

                string jsonValorInicial = SerializarObjetoBitacora(ValorInicial);
                string JsonNuevoValor = SerializarObjetoBitacora(objeto);

                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                            ResultadoConsulta.Usuario,
                                ResultadoConsulta.Clase, objeto.Codigo, JsonNuevoValor, "", jsonValorInicial);

            }
            catch (Exception ex)
            {
                if (ResultadoConsulta.HayError != (int)Error.ErrorControlado)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorSistema;

                }
                ResultadoConsulta.MensajeError = ex.Message;
            }

            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<Solicitud>> EliminarElemento(Solicitud objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Eliminar;
                ResultadoConsulta.Usuario = user;
                objeto.UsuarioModificacion = user;

                if (!string.IsNullOrEmpty(objeto.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.id), out temp);
                    objeto.idSolicitud = temp;
                }

                var resul = clsDatos.ObtenerDatos(objeto);

                if (resul.Count() == 0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }
                else
                {
                    objeto = resul.SingleOrDefault();
                    objeto.IdEstado = (int)Constantes.EstadosRegistro.Eliminado;
                    resul = clsDatos.ActualizarDatos(objeto);
                    ResultadoConsulta.objetoRespuesta = resul;
                    ResultadoConsulta.CantidadRegistros = resul.Count();
                }

                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                ResultadoConsulta.Usuario,
                ResultadoConsulta.Clase, objeto.Codigo, JsonConvert.SerializeObject(objeto), "", "");

            }
            catch (Exception ex)
            {
                ResultadoConsulta.MensajeError = ex.Message;
                if (ResultadoConsulta.HayError != (int)Error.ErrorControlado)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorSistema;

                }
            }

            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<Solicitud>> InsertarDatos(Solicitud objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Insertar;
                ResultadoConsulta.Usuario = user;
                objeto.UsuarioCreacion = user;

                List<Solicitud> BuscarRegistros = clsDatos.ObtenerDatos(new Solicitud());

                if (BuscarRegistros.Where(X => X.Codigo.ToUpper() == objeto.Codigo.ToUpper() && !X.idSolicitud.Equals(objeto.idSolicitud)).ToList().Count() > 0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.CodigoRegistrado);
                }

                if (BuscarRegistros.Where(X => X.Nombre.ToUpper() == objeto.Nombre.ToUpper() && !X.idSolicitud.Equals(objeto.idSolicitud)).ToList().Count() > 0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.NombreRegistrado);
                }
                else if (objeto.FechaFin < objeto.FechaInicio)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.ValorFecha);
                }
                else
                {
                    var resul = clsDatos.ActualizarDatos(objeto);
                    ResultadoConsulta.objetoRespuesta = resul;
                }

                objeto = clsDatos.ObtenerDatos(objeto).Single();

                string jsonValorInicial = SerializarObjetoBitacora(objeto);

                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                            ResultadoConsulta.Usuario,
                                ResultadoConsulta.Clase, objeto.Codigo, "", "", jsonValorInicial);

            }
            catch (Exception ex)
            {
                if (ResultadoConsulta.HayError!= (int)Error.ErrorControlado)
                { 
                    ResultadoConsulta.HayError = (int)Error.ErrorSistema;

                }
                ResultadoConsulta.MensajeError = ex.Message;
            }

            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<Solicitud>> ValidarDatos(Solicitud objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<Solicitud> ClonarDetallesDeSolicitudes(string pidSolicitudAClonar, string pidSolicitudDestino)
        {
            RespuestaConsulta<Solicitud> resultado = new RespuestaConsulta<Solicitud>();

            int idSolicitudAClonar, idSolicitudDestino;
            bool errorControlado = false;

            try
            {
                int.TryParse(Utilidades.Desencriptar(pidSolicitudAClonar), out int number);
                idSolicitudAClonar = number;

                if (idSolicitudAClonar == 0) // ¿ID descencriptado con éxito?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                int.TryParse(Utilidades.Desencriptar(pidSolicitudDestino), out number);
                idSolicitudDestino = number;

                if (idSolicitudDestino == 0) // ¿ID descencriptado con éxito?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                clsDatos.ClonarDetallesDeSolicitudes(idSolicitudAClonar, idSolicitudDestino);

                resultado.objetoRespuesta = new Solicitud() { id = pidSolicitudDestino };

            }
            catch (Exception ex)
            {
                resultado.MensajeError = ex.Message;

                if (errorControlado)
                    resultado.HayError = (int)Error.ErrorControlado;
                else
                    resultado.HayError = (int)Error.ErrorSistema;
            }

            return resultado;
        }

        private void DesencriptarSolicitud(Solicitud objeto)
        {
            if (!string.IsNullOrEmpty(objeto.id))
            {
                objeto.id = Utilidades.Desencriptar(objeto.id);
                int temp;
                if (int.TryParse(objeto.id, out temp))
                {
                    objeto.idSolicitud = temp;
                }
            }

        }

    }
}