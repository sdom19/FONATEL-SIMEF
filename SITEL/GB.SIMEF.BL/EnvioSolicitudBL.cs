using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL
{
    public class EnvioSolicitudBL : IMetodos<EnvioSolicitudes>
    {
        readonly string modulo = "";
        readonly string user = "";
        private readonly EnvioSolicitudDAL EnvioSolicitudDAL;
        RespuestaConsulta<List<EnvioSolicitudes>> resultado;
        public EnvioSolicitudBL()
        {
            EnvioSolicitudDAL = new EnvioSolicitudDAL(); 
            resultado = new RespuestaConsulta<List<EnvioSolicitudes>>();
        }

        public RespuestaConsulta<List<EnvioSolicitudes>> ActualizarElemento(EnvioSolicitudes objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<EnvioSolicitudes>> CambioEstado(EnvioSolicitudes objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<EnvioSolicitudes>> ClonarDatos(EnvioSolicitudes objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<EnvioSolicitudes>> EliminarElemento(EnvioSolicitudes objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<EnvioSolicitudes>> InsertarDatos(EnvioSolicitudes objeto)
        {
            throw new NotImplementedException();
        }


        public RespuestaConsulta<List<EnvioSolicitudes>> ObtenerDatos(EnvioSolicitudes objeto)
        {
           

            try
            {
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                resultado.objetoRespuesta = EnvioSolicitudDAL.ObtenerDatos();
                resultado.CantidadRegistros = resultado.objetoRespuesta.Count();
            }
            catch (Exception ex)
            {
                resultado.HayError = (int)Constantes.Error.ErrorSistema;
                resultado.MensajeError = ex.Message;
            }
            return resultado;
        }

        public RespuestaConsulta<List<EnvioSolicitudes>> ValidarDatos(EnvioSolicitudes objeto)
        {
            throw new NotImplementedException();
        }
    }
}
