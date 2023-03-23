using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL
{
    public class DetalleRegistroIndicadorFonatelBL : IMetodos<DetalleRegistroIndicadorFonatel>
    {
        readonly string modulo = "";
        readonly string user = "";
        private readonly DetalleRegistroIndicadorFonatelDAL DetalleRegistroIndicadorFonatelDAL;
        RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>> ResultadoConsulta;
        private readonly RegistroIndicadorFonatelBL registroIndicadorFonatelBl;

        public DetalleRegistroIndicadorFonatelBL(string modulo, string user)
        {
            this.modulo = modulo;
            this.user = user;
            this.DetalleRegistroIndicadorFonatelDAL = new DetalleRegistroIndicadorFonatelDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>>();
            this.registroIndicadorFonatelBl = new RegistroIndicadorFonatelBL(this.modulo, this.user);
        }

        public RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>> ActualizarElemento(DetalleRegistroIndicadorFonatel objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;

                DesencriptarRegistroIndicador(objeto);

                DetalleRegistroIndicadorFonatel detalle = DetalleRegistroIndicadorFonatelDAL.ObtenerDatoDetalleRegistroIndicador(objeto).FirstOrDefault();
                detalle.CantidadFila = objeto.CantidadFila;
                if (!string.IsNullOrEmpty(objeto.NotaInformante))
                {
                    detalle.NotaInformante = objeto.NotaInformante;
                }
                
                var result = DetalleRegistroIndicadorFonatelDAL.ActualizarDetalleRegistroIndicadorFonatel(detalle);
                ResultadoConsulta.objetoRespuesta = result;
                ResultadoConsulta.CantidadRegistros = result.Count();
            }
            catch (Exception ex)
            {
                ResultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>> CambioEstado(DetalleRegistroIndicadorFonatel objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>> ClonarDatos(DetalleRegistroIndicadorFonatel objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>> EliminarElemento(DetalleRegistroIndicadorFonatel objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>> InsertarDatos(DetalleRegistroIndicadorFonatel objeto)
        {
            throw new NotImplementedException();
        }


        public RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>> ObtenerDatos(DetalleRegistroIndicadorFonatel objeto)
        {

            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;

                DesencriptarRegistroIndicador(objeto);

                var result = DetalleRegistroIndicadorFonatelDAL.ObtenerDatoDetalleRegistroIndicador(objeto);

                ResultadoConsulta.objetoRespuesta = result;
                ResultadoConsulta.CantidadRegistros = result.Count();
            }
            catch (Exception ex)
            {
                ResultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>> ValidarDatos(DetalleRegistroIndicadorFonatel objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>> ActualizarDetalleRegistroIndicadorFonatelMultiple(List<DetalleRegistroIndicadorFonatel> lista)
        {
            try
            {
                foreach (var objeto in lista)
                {
                    ResultadoConsulta.Clase = modulo;
                    ResultadoConsulta.Accion = (int)Accion.Consultar;

                    DesencriptarRegistroIndicador(objeto);

                    DetalleRegistroIndicadorFonatel detalle = DetalleRegistroIndicadorFonatelDAL.ObtenerDatoDetalleRegistroIndicador(objeto).FirstOrDefault();

                    detalle.CantidadFila = objeto.CantidadFila;

                    if (!string.IsNullOrEmpty(objeto.NotaInformante))
                    {
                        detalle.NotaInformante = objeto.NotaInformante;
                    }

                    if (!string.IsNullOrEmpty(objeto.NotaEncargado))
                    {
                        detalle.NotaEncargado = objeto.NotaEncargado;
                    }

                    var result = DetalleRegistroIndicadorFonatelDAL.ActualizarDetalleRegistroIndicadorFonatel(detalle);

                    //Cambia de estado el registro indicador a Enviado
                    RegistroIndicadorFonatel reg = new RegistroIndicadorFonatel();
                    reg.idFormularioWeb = objeto.idFormularioWeb;
                    reg.IdSolicitud = objeto.IdSolicitud;
                    var BuscarRegistrosIndicador = registroIndicadorFonatelBl.ObtenerDatos(reg);

                    if(BuscarRegistrosIndicador.objetoRespuesta[0].IdEstado == (int)Constantes.EstadosRegistro.Enviado)
                    {
                        reg.IdEstado = (int)Constantes.EstadosRegistro.Enviado;
                    }
                    else
                    {
                        reg.IdEstado = (int)Constantes.EstadosRegistro.EnProceso;
                    }
                    registroIndicadorFonatelBl.ActualizarElemento(reg);

                    ResultadoConsulta.objetoRespuesta = result;
                    ResultadoConsulta.CantidadRegistros = result.Count();
                }
            }
            catch (Exception ex)
            {
                ResultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        /// <summary>
        /// Autor:Francisco Vindas Ruiz
        /// Fecha:26/01/2022
        /// Metodo: Funciona para realizar la carga total del registro indicador el cual incluirá el envio de correo a los usuarios
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>> CargaTotalRegistroIndicador(List<DetalleRegistroIndicadorFonatel> lista)
        {
            try
            {
                foreach (var objeto in lista)
                {
                    ResultadoConsulta.Clase = modulo;
                    ResultadoConsulta.Accion = (int)Accion.Consultar;

                    DesencriptarRegistroIndicador(objeto);

                    DetalleRegistroIndicadorFonatel detalle = DetalleRegistroIndicadorFonatelDAL.ObtenerDatoDetalleRegistroIndicador(objeto).FirstOrDefault();

                    detalle.CantidadFila = objeto.CantidadFila;

                    if (!string.IsNullOrEmpty(objeto.NotaInformante))
                    {
                        detalle.NotaInformante = objeto.NotaInformante;
                    }

                    if (!string.IsNullOrEmpty(objeto.NotaEncargado))
                    {
                        detalle.NotaEncargado = objeto.NotaEncargado;
                    }

                    var result = DetalleRegistroIndicadorFonatelDAL.ActualizarDetalleRegistroIndicadorFonatel(detalle);

                    RegistroIndicadorFonatel reg = new RegistroIndicadorFonatel();
                    reg.idFormularioWeb = objeto.idFormularioWeb;
                    reg.IdSolicitud = objeto.IdSolicitud;
                    reg.IdEstado = (int)Constantes.EstadosRegistro.Enviado;
                    registroIndicadorFonatelBl.ActualizarElemento(reg);

                    result = result.Where(x => x.IdSolicitud == detalle.IdSolicitud).ToList();

                    ResultadoConsulta.objetoRespuesta = result;
                    ResultadoConsulta.CantidadRegistros = result.Count();
                }


            }
            catch (Exception ex)
            {
                ResultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        /// <summary>
        /// Autor:Francisco Vindas Ruiz
        /// Fecha:26/01/2022
        /// Metodo: Funciona para desencriptar los Ids necesarios para los metodos que lo necesiten
        /// </summary>
        /// <param name="objeto"></param>
        private void DesencriptarRegistroIndicador(DetalleRegistroIndicadorFonatel objeto)
        {
            if (!string.IsNullOrEmpty(objeto.idFormularioWebString))
            {
                int.TryParse(Utilidades.Desencriptar(objeto.idFormularioWebString), out int temp);
                objeto.idFormularioWeb = temp;
            }
            if (!string.IsNullOrEmpty(objeto.IdSolicitudString))
            {
                int.TryParse(Utilidades.Desencriptar(objeto.IdSolicitudString), out int temp);
                objeto.IdSolicitud = temp;
            }
            if (!string.IsNullOrEmpty(objeto.IdIndicadorString))
            {
                int.TryParse(Utilidades.Desencriptar(objeto.IdIndicadorString), out int temp);
                objeto.IdIndicador = temp;
            }
        }

        /// <summary>
        /// Autor:Georgi Mesen Cerdas 
        /// Fecha:31/01/2023
        /// Metodo para obtener los valores fonatel de categorias y variables
        /// </summary>
        /// <param name="objeto"></param>
        public RespuestaConsulta<DetalleRegistroIndicadorFonatel> ObtenerListaDetalleRegistroIndicadorValoresFonatel(DetalleRegistroIndicadorFonatel objeto)
        {

            RespuestaConsulta<DetalleRegistroIndicadorFonatel> resultado = new RespuestaConsulta<DetalleRegistroIndicadorFonatel>();
            try
            {
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                if (!string.IsNullOrEmpty(objeto.idFormularioWebString))
                {
                    int.TryParse(Utilidades.Desencriptar(objeto.idFormularioWebString), out int temp);
                    objeto.idFormularioWeb = temp;
                }
                if (!string.IsNullOrEmpty(objeto.IdSolicitudString))
                {
                    int.TryParse(Utilidades.Desencriptar(objeto.IdSolicitudString), out int temp);
                    objeto.IdSolicitud = temp;
                }
                if (!string.IsNullOrEmpty(objeto.IdIndicadorString))
                {
                    int.TryParse(Utilidades.Desencriptar(objeto.IdIndicadorString), out int temp);
                    objeto.IdIndicador = temp;
                }

                DetalleRegistroIndicadorFonatel detalle = new DetalleRegistroIndicadorFonatel();
                DetalleRegistroIndicadorCategoriaValorFonatel valor = new DetalleRegistroIndicadorCategoriaValorFonatel();
                valor.idFormularioWeb = objeto.idFormularioWeb;
                valor.IdSolicitud = objeto.IdSolicitud;
                valor.IdIndicador = objeto.IdIndicador;
                DetalleRegistroIndicadorVariableValorFonatel variable = new DetalleRegistroIndicadorVariableValorFonatel();
                variable.idFormularioWeb = objeto.idFormularioWeb;
                variable.IdSolicitud = objeto.IdSolicitud;
                variable.IdIndicador = objeto.IdIndicador;

                detalle.DetalleRegistroIndicadorCategoriaValorFonatel = DetalleRegistroIndicadorFonatelDAL.ObtenerDetalleRegistroIndicadorCategoriaValorFonatel(valor);
                detalle.DetalleRegistroIndicadorVariableValorFonatel = DetalleRegistroIndicadorFonatelDAL.ObtenerDetalleRegistroIndicadorVariableValorFonatel(variable);


                resultado.objetoRespuesta = detalle;
                resultado.CantidadRegistros = 1;
            }
            catch (Exception ex)
            {
                resultado.HayError = (int)Constantes.Error.ErrorSistema;
                resultado.MensajeError = ex.Message;
            }
            return resultado;
        }

        /// <summary>
        /// Autor:Adofo Cunquero
        /// Fecha:23/02/2023
        /// Metodo para consumir API reglas de validacion
        /// </summary>
        /// <param name="objeto"></param>
        /// 
        public async Task<RespuestaConsulta<string>> AplicarReglasValidacion(DetalleRegistroIndicadorFonatel objeto)
        {
            DesencriptarRegistroIndicador(objeto);
            var result = new RespuestaConsulta<string>();
            JObject task = new JObject();
            try
            {
                var finalizado = false;
                //crear la tarea
                using (var client = new HttpClient())
                {
                    var payload = new {
                        usuario = user,
                        aplicacion = WebConfigurationManager.AppSettings["APIReglasValidacionApplicationId"].ToString(),
                        despacho = Constantes.ParametrosReglaValidacionDispatch,
                        periodicidad = Constantes.ParametrosReglaValidacionPeriodicity,
                        iniciarAhora = true,
                        parametros = new object[] {
                            new {
                                nombre = Constantes.ParametrosReglaValidacion.Solicitud,
                                valor = objeto.IdSolicitud.ToString()
                            },
                            new {
                                nombre = Constantes.ParametrosReglaValidacion.Formulario,
                                valor = objeto.idFormularioWeb.ToString()
                            },
                            new {
                                nombre = Constantes.ParametrosReglaValidacion.Indicador,
                                valor = objeto.IdIndicador.ToString()
                            }
                        }
                    };
                    var stringPayload = JsonConvert.SerializeObject(payload);
                    var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(WebConfigurationManager.AppSettings["APIReglasValidacionRuta"].ToString(), httpContent);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        task = JObject.Parse(content);
                    }
                    else
                    {
                        result.HayError = (int)Constantes.Error.ErrorSistema;
                        return result;
                    }
                }

                //verificar si ya finalizo
                while (!finalizado)
                {
                    using (var client = new HttpClient())
                    {
                        HttpResponseMessage response = await client.GetAsync(WebConfigurationManager.AppSettings["APIReglasValidacionRuta"].ToString() +"/"+ task["idJob"].ToString());
                        if (response.IsSuccessStatusCode)
                        {
                            JObject jobStatus = JObject.Parse(await response.Content.ReadAsStringAsync());
                            var tasks = jobStatus["tareas"].ToArray();
                            if(tasks.Count() > 0)
                            {
                                var item = tasks[0];
                                if (item["estado"].ToString() == Constantes.RespuestaEstadoReglasValidacion.Finalizado || item["estado"].ToString() == Constantes.RespuestaEstadoReglasValidacion.Detenido)
                                {
                                    result.objetoRespuesta = jobStatus.ToString();
                                    finalizado = true;
                                }
                            }
                        }
                        else
                        {
                            result.HayError = (int)Constantes.Error.ErrorSistema;
                        }
                    }

                    await Task.Delay(1000);
                }
            }
            catch (Exception ex)
            {
                result.MensajeError = ex.Message;
                result.HayError = (int)Constantes.Error.ErrorSistema;
            }

            return result;
        }
    }
}
