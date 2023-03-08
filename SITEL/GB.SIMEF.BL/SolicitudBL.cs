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

        public SolicitudBL(string modulo, string user)
        {
            this.clsDatos = new SolicitudDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<Solicitud>>();
            this.plantillaDal = new PlantillaHtmlDAL();
            this.user = user;
            this.modulo = modulo;
        }

        public RespuestaConsulta<bool> EnvioCorreo(Solicitud solicitud)
        {
            RespuestaConsulta<bool> envioCorreo = new RespuestaConsulta<bool>();
            try
            {
                envioCorreo.objetoRespuesta = false;
                PlantillaHtml plantilla = plantillaDal.ObtenerDatos((int)Constantes.PlantillaCorreoEnum.EnvioSolicitud);
                solicitud = clsDatos.ObtenerDatos(solicitud).Single();
                if (solicitud.Fuente.idEstado == (int)Constantes.EstadosRegistro.Activo)
                {

                    string formularios = string.Empty;
                    bool primerValor = true;
                    foreach (var item in solicitud.FormularioWeb.Select(x => x.Nombre))
                    {
                        if (primerValor)
                        {
                            formularios = item;
                            primerValor = false;
                        }
                        else
                        {
                            formularios = string.Format("{0}, {1}", formularios, item);
                        }

                    }
                    string fechaVigencia = string.Format("{0:MM/dd/yyyy} al {1:MM/dd/yyyy}", solicitud.FechaInicio, solicitud.FechaFin);
                    plantilla.Html = string.Format(plantilla.Html, Utilidades.Encriptar(solicitud.Fuente.Fuente), solicitud.Nombre, fechaVigencia, solicitud.Mes.Nombre + " " + solicitud.Anno.Nombre, formularios, solicitud.Mensaje);
                    foreach (var detalleFuente in solicitud.Fuente.DetalleFuentesRegistro.Where(x => x.Estado == true))
                    {
                        correoDal = new CorreoDal(detalleFuente.CorreoElectronico, "", plantilla.Html.Replace(Utilidades.Encriptar(solicitud.Fuente.Fuente), detalleFuente.NombreDestinatario), "Envío de solicitud");

                        var result = correoDal.EnviarCorreo();
                        envioCorreo.objetoRespuesta = result == 0 ? false : true;
                    }
                }
                else
                {
                    envioCorreo.HayError = (int)Constantes.Error.ErrorControlado;
                    throw new Exception(Errores.SolicitudesFuenteRegistrada);
                }

                clsDatos.RegistrarBitacora((int)Constantes.Accion.EnviarSolicitud, "Sistema automático", modulo, solicitud.Codigo);
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

                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Editar;
                objeto.UsuarioModificacion = user;
                objeto.UsuarioCreacion = user;
                ResultadoConsulta.Usuario = objeto.UsuarioCreacion;

                DesencriptarSolicitud(objeto);

                var objetoAnterior = BuscarRegistros.Where(x => x.idSolicitud == objeto.idSolicitud).Single();

                ValidarObjetoSolicitud(objeto);

                var result = BuscarRegistros.Where(x => x.idSolicitud == objeto.idSolicitud).Single();

                if (result.SolicitudFormulario.Count > objeto.CantidadFormularios)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.SolicitudesCantidadFormularios);
                }

                if (objetoAnterior.IdEstado == (int)EstadosRegistro.Activo && objetoAnterior.idFuente == objeto.idFuente && objetoAnterior.CantidadFormularios == objeto.CantidadFormularios)
                {
                    objeto.IdEstado = (int)EstadosRegistro.Activo;
                }
                else if (objetoAnterior.IdEstado == (int)EstadosRegistro.Desactivado)
                {
                    objeto.IdEstado = (int)EstadosRegistro.Desactivado;
                }
                else
                {
                    objeto.IdEstado = (int)EstadosRegistro.EnProceso;
                }

                ResultadoConsulta.objetoRespuesta = clsDatos.ActualizarDatos(objeto);
                ResultadoConsulta.CantidadRegistros = ResultadoConsulta.objetoRespuesta.Count();
                

                objeto = ResultadoConsulta.objetoRespuesta.Single();
                string JsonActual = objeto.ToString();
                string JsonAnterior = objetoAnterior.ToString();

                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                        ResultadoConsulta.Usuario,
                            ResultadoConsulta.Clase, objeto.Codigo
                            , JsonActual, JsonAnterior, "");
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

                var objetoAnterior = clsDatos.ObtenerDatos(new Solicitud { idSolicitud = objeto.idSolicitud, IdEstado = 0, Codigo = objeto.Codigo });
                string jsonAnterior = objetoAnterior.FirstOrDefault().ToString();

                var resul = clsDatos.ObtenerDatos(objeto);
                objeto = resul.Single();
                objeto.IdEstado = nuevoEstado;

                ResultadoConsulta.Accion = (int)EstadosRegistro.Activo == objeto.IdEstado ? (int)Accion.Activar : (int)Accion.Inactiva;
                resul = clsDatos.ActualizarDatos(objeto);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();


                string jsonActual = resul.FirstOrDefault().ToString();
                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                ResultadoConsulta.Usuario,
                ResultadoConsulta.Clase, objeto.Codigo, jsonActual, jsonAnterior, "");

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

                if (ValorInicial.SolicitudFormulario.Count > objeto.CantidadFormularios)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.CantidadRegistrosLimite);
                }

                if (objeto.FechaFin < objeto.FechaInicio)
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

                string jsonValorInicial = ValorInicial.ToString();
                string JsonNuevoValor = objeto.ToString();

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
                ResultadoConsulta.Clase, objeto.Codigo, "", "", "");

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

                DesencriptarSolicitud(objeto);

                ValidarObjetoSolicitud(objeto);

                if (objeto.idSolicitud == 0)
                {
                    ResultadoConsulta.objetoRespuesta = clsDatos.ActualizarDatos(objeto);
                }
                else
                {
                    var result = BuscarRegistros.Where(x => x.idSolicitud == objeto.idSolicitud).Single();

                    if (result.SolicitudFormulario.Count > objeto.CantidadFormularios)
                    {
                        ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                        throw new Exception(Errores.SolicitudesCantidadFormularios);
                    }
                    else
                    {
                        ResultadoConsulta.objetoRespuesta = clsDatos.ActualizarDatos(objeto);
                    }
                }

                string jsonValorInicial = ResultadoConsulta.objetoRespuesta.Single().ToString();
                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                            ResultadoConsulta.Usuario,
                                ResultadoConsulta.Clase, objeto.Codigo, "", "", jsonValorInicial);

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

        /// <summary>
        /// Autor: Francisco Vindas Ruix
        /// Fecha: 08/01/2023
        /// Metodo que descripta el id de la solucitud a utilizar
        /// </summary>
        /// <param name="objeto"></param>
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

        /// <summary>
        /// Fecha: 10-02-2023
        /// Autor: Francisco Vindas Ruiz
        /// Metodo que funciona para validar que el objeto solicitud cumpla con las validaciones correspondientes
        /// </summary>
        /// <param name="objeto"></param>
        private void ValidarObjetoSolicitud(Solicitud objeto)
        {

            var BuscarRegistros = clsDatos.ObtenerDatos(new Solicitud());

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

            if (objeto.FechaFin < objeto.FechaInicio)
            {
                ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                throw new Exception(Errores.ValorFecha);
            }

            if (objeto.Codigo == null || string.IsNullOrEmpty(objeto.Codigo.Trim()))
            {
                ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                throw new Exception(string.Format(Errores.CampoConValorInvalido, EtiquetasViewSolicitudes.Codigo));
            }

            if (!Utilidades.rx_alfanumerico.Match(objeto.Codigo).Success || objeto.Codigo.Trim().Length > 30)
            {
                ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                throw new Exception(string.Format(Errores.CampoConFormatoInvalido, EtiquetasViewSolicitudes.Codigo));
            }

            if (objeto.Nombre == null || string.IsNullOrEmpty(objeto.Nombre.Trim()))
            {

                ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                throw new Exception( string.Format(Errores.CampoConValorInvalido, EtiquetasViewSolicitudes.Nombre));
            }

            if (!Utilidades.rx_alfanumerico.Match(objeto.Nombre).Success || objeto.Nombre.Trim().Length > 500)
            {
                ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                throw new Exception(string.Format(Errores.CampoConFormatoInvalido, EtiquetasViewSolicitudes.Nombre));
            }

            if (!string.IsNullOrEmpty(objeto.Mensaje?.Trim())) // ¿se ingresó el dato?
            {
                if (!Utilidades.rx_alfanumerico.Match(objeto.Mensaje).Success          // la descripción solo debe contener texto como valor
                    || objeto.Mensaje.Trim().Length > 3000)                         // validar la cantidad de caracteres
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                    throw new Exception(string.Format(Errores.CampoConValorInvalido, EtiquetasViewSolicitudes.Mensaje));
                }
            }

        }

    }
}
    
