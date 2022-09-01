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
    public class TipoReglaValidacionBL : IMetodos<TipoReglaValidacion>
    {
        readonly string modulo = "";
        readonly string user = "";
        private readonly TipoReglaValidacionDAL TipoReglaValidacionDAL;

        public TipoReglaValidacionBL()
        {
            TipoReglaValidacionDAL = new TipoReglaValidacionDAL();
        }

        public RespuestaConsulta<List<TipoReglaValidacion>> ActualizarElemento(TipoReglaValidacion objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<TipoReglaValidacion>> CambioEstado(TipoReglaValidacion objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<TipoReglaValidacion>> ClonarDatos(TipoReglaValidacion objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<TipoReglaValidacion>> EliminarElemento(TipoReglaValidacion objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<TipoReglaValidacion>> InsertarDatos(TipoReglaValidacion objeto)
        {
            throw new NotImplementedException();
        }


        public RespuestaConsulta<List<TipoReglaValidacion>> ObtenerDatos(TipoReglaValidacion pTipoReglaValidacion)
        {
            RespuestaConsulta<List<TipoReglaValidacion>> resultado = new RespuestaConsulta<List<TipoReglaValidacion>>();

            try
            {
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                var result = TipoReglaValidacionDAL.ObtenerDatos();
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

        public RespuestaConsulta<List<TipoReglaValidacion>> ValidarDatos(TipoReglaValidacion objeto)
        {
            throw new NotImplementedException();
        }
    }
}
