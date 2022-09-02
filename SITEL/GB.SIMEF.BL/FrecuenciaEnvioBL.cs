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
        readonly string modulo = "";
        readonly string user = "";
        private readonly FrecuenciaEnvioDAL frecuenciaEnvioDAL;

        public FrecuenciaEnvioBL(string pModulo, string pUser)
        {
            modulo = pModulo;
            user = pUser;
            frecuenciaEnvioDAL = new FrecuenciaEnvioDAL();
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

        /// 23/08/2022
        /// José Navarro Acuña
        /// Función que retorna todos las frecuencia de envio registradas en estado activo
        /// Se puede aplicar un filtro para obtener un único elemento a traves del ID.
        public RespuestaConsulta<List<FrecuenciaEnvio>> ObtenerDatos(FrecuenciaEnvio pFrecuenciaEnvio)
        {
            RespuestaConsulta<List<FrecuenciaEnvio>> resultado = new RespuestaConsulta<List<FrecuenciaEnvio>>();

            try
            {
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                var result = frecuenciaEnvioDAL.ObtenerDatos(pFrecuenciaEnvio);
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

        public RespuestaConsulta<List<FrecuenciaEnvio>> ValidarDatos(FrecuenciaEnvio objeto)
        {
            throw new NotImplementedException();
        }
    }
}
