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
    public class OperadorArismeticoBL : IMetodos<OperadorArismetico>
    {
        readonly string modulo = "";
        readonly string user = "";
        private readonly OperadorArismeticoDAL OperadorArismeticoDAL;

        public OperadorArismeticoBL()
        {
            OperadorArismeticoDAL = new OperadorArismeticoDAL();
        }

        public RespuestaConsulta<List<OperadorArismetico>> ActualizarElemento(OperadorArismetico objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<OperadorArismetico>> CambioEstado(OperadorArismetico objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<OperadorArismetico>> ClonarDatos(OperadorArismetico objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<OperadorArismetico>> EliminarElemento(OperadorArismetico objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<OperadorArismetico>> InsertarDatos(OperadorArismetico objeto)
        {
            throw new NotImplementedException();
        }


        public RespuestaConsulta<List<OperadorArismetico>> ObtenerDatos(OperadorArismetico pOperadorArismetico)
        {
            RespuestaConsulta<List<OperadorArismetico>> resultado = new RespuestaConsulta<List<OperadorArismetico>>();

            try
            {
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                var result = OperadorArismeticoDAL.ObtenerDatos();
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

        public RespuestaConsulta<List<OperadorArismetico>> ValidarDatos(OperadorArismetico objeto)
        {
            throw new NotImplementedException();
        }
    }
}
