using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using OfficeOpenXml;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL
{
    public class SolicitudEnvioProgramadoBL : IMetodos<SolicitudEnvioProgramado>
    {
        private readonly SolicitudEnvioProgramadoDAL clsDatos;

        private readonly SolicitudDAL clsDatosSolicitud;
        private readonly FrecuenciaEnvioDAL frecuenciaEnvioDAL;

        string modulo = EtiquetasViewSolicitudes.Solicitudes;
        string user = string.Empty;

        private RespuestaConsulta<List<SolicitudEnvioProgramado>> ResultadoConsulta;

        public SolicitudEnvioProgramadoBL(string modulo, string user)
        {
            clsDatos = new SolicitudEnvioProgramadoDAL();
            clsDatosSolicitud = new SolicitudDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<SolicitudEnvioProgramado>>();
            this.user = user;
            this.modulo = modulo;
            frecuenciaEnvioDAL = new FrecuenciaEnvioDAL();
        }

        public RespuestaConsulta<List<SolicitudEnvioProgramado>> ActualizarElemento(SolicitudEnvioProgramado objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<SolicitudEnvioProgramado>> CambioEstado(SolicitudEnvioProgramado objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<SolicitudEnvioProgramado>> ClonarDatos(SolicitudEnvioProgramado objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<SolicitudEnvioProgramado>> EliminarElemento(SolicitudEnvioProgramado objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Eliminar;
                ResultadoConsulta.Usuario = user;

                SolicitudEnvioProgramado registroActualizar;

                if (!string.IsNullOrEmpty(objeto.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.id), out temp);
                    objeto.IdSolicitud = temp;
                }

                var resul = clsDatos.ObtenerDatos(objeto);
                registroActualizar = resul.Single();
                registroActualizar.Estado = false;
                resul = clsDatos.ActualizarDatos(registroActualizar);

                ResultadoConsulta.objetoRespuesta = resul;

                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                            ResultadoConsulta.Usuario,
                                ResultadoConsulta.Clase, objeto.CodigoSolicitud, "", "", "");

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

        public RespuestaConsulta<List<SolicitudEnvioProgramado>> InsertarDatos(SolicitudEnvioProgramado objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Insertar;
                ResultadoConsulta.Usuario = user;

                objeto.Estado = true;
                if (!String.IsNullOrEmpty(objeto.id))
                {
                    objeto.id = Utilidades.Desencriptar(objeto.id);
                    int temp;
                    if (int.TryParse(objeto.id, out temp))
                    {
                        objeto.IdSolicitud = temp;
                    }
                }

                if (objeto.FechaCiclo < DateTime.Today)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.FechaInicioCiclo);
                }

                var resul = clsDatos.ActualizarDatos(objeto);
                ResultadoConsulta.objetoRespuesta = resul;

                var objetoInicial = objeto;
                var frecuencia = frecuenciaEnvioDAL.ObtenerDatos(new FrecuenciaEnvio { idFrecuencia = objeto.IdFrecuencia });
                objetoInicial.Frecuencia = frecuencia.FirstOrDefault();
                string jsonInicial = objetoInicial.ToString();

                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                            ResultadoConsulta.Usuario,
                                ResultadoConsulta.Clase, objeto.CodigoSolicitud, "", "", jsonInicial);

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

        public RespuestaConsulta<List<SolicitudEnvioProgramado>> ObtenerDatos(SolicitudEnvioProgramado objeto)
        {
            RespuestaConsulta<List<SolicitudEnvioProgramado>> resultado = new RespuestaConsulta<List<SolicitudEnvioProgramado>>();

            try
            {
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                var result = clsDatos.ObtenerDatos(objeto);
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

        public RespuestaConsulta<List<SolicitudEnvioProgramado>> ValidarDatos(SolicitudEnvioProgramado objeto)
        {
            throw new NotImplementedException();
        }
    }
}