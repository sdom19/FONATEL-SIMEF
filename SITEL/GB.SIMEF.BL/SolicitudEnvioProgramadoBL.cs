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

        string modulo = EtiquetasViewSolicitudes.Solicitudes;

        private RespuestaConsulta<List<SolicitudEnvioProgramado>> ResultadoConsulta;

        public SolicitudEnvioProgramadoBL()
        {
            clsDatos = new SolicitudEnvioProgramadoDAL();
            clsDatosSolicitud = new SolicitudDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<SolicitudEnvioProgramado>>();
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

            }
            catch (Exception ex)
            {
                //if (ex.Message == Errores.NoRegistrosActualizar)
                //{
                //    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                //}
                //else
                //{
                //    ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                //}

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

                if (!String.IsNullOrEmpty(objeto.id))
                {
                    objeto.id = Utilidades.Desencriptar(objeto.id);
                    int temp;
                    if (int.TryParse(objeto.id, out temp))
                    {
                        objeto.IdSolicitud = temp;
                    }
                }

                var resul = clsDatos.ActualizarDatos(objeto);
                ResultadoConsulta.objetoRespuesta = resul;

            }
            catch (Exception ex)
            {
                //if (ex.Message == Errores.NoRegistrosActualizar)
                //{
                //    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                //}
                //else
                //{
                //    ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                //}

                ResultadoConsulta.MensajeError = ex.Message;
            }

            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<SolicitudEnvioProgramado>> ObtenerDatos(SolicitudEnvioProgramado objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<SolicitudEnvioProgramado>> ValidarDatos(SolicitudEnvioProgramado objeto)
        {
            throw new NotImplementedException();
        }
    }
}
