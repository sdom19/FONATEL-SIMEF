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
    public class FrecuenciaEnvioBL : IMetodos<FrecuenciaEnvio>
    {
        private readonly FrecuenciaEnvioDAL clsDatos;


        private RespuestaConsulta<List<FrecuenciaEnvio>> ResultadoConsulta;
        string modulo = Etiquetas.Frecuencia;

        public FrecuenciaEnvioBL() 
        {
            this.clsDatos = new FrecuenciaEnvioDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<FrecuenciaEnvio>>();
        }

        public RespuestaConsulta<List<FrecuenciaEnvio>> ActualizarElemento(FrecuenciaEnvio objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FrecuenciaEnvio>> CambioEstado(FrecuenciaEnvio objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FrecuenciaEnvio>> ClonarDatos(FrecuenciaEnvio objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FrecuenciaEnvio>> EliminarElemento(FrecuenciaEnvio objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FrecuenciaEnvio>> InsertarDatos(FrecuenciaEnvio objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FrecuenciaEnvio>> ObtenerDatos(FrecuenciaEnvio objFrecuenciaEnvio)
        {
            try {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;
                var resultado = clsDatos.ObtenerDatos(objFrecuenciaEnvio);
                ResultadoConsulta.objetoRespuesta = resultado;
                ResultadoConsulta.CantidadRegistros = resultado.Count();
            }

            catch (Exception ex) 
            {
                ResultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<FrecuenciaEnvio>> ValidarDatos(FrecuenciaEnvio objeto)
        {
            throw new NotImplementedException();
        }
    }
}
