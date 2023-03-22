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
    public class OperadorArismeticoBL : IMetodos<OperadorAritmetico>
    {
        readonly string modulo = "";
        readonly string user = "";
        private readonly OperadorArismeticoDAL OperadorArismeticoDAL;

        public OperadorArismeticoBL()
        {
            OperadorArismeticoDAL = new OperadorArismeticoDAL();
        }

        public RespuestaConsulta<List<OperadorAritmetico>> ActualizarElemento(OperadorAritmetico objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<OperadorAritmetico>> CambioEstado(OperadorAritmetico objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<OperadorAritmetico>> ClonarDatos(OperadorAritmetico objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<OperadorAritmetico>> EliminarElemento(OperadorAritmetico objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<OperadorAritmetico>> InsertarDatos(OperadorAritmetico objeto)
        {
            throw new NotImplementedException();
        }


        public RespuestaConsulta<List<OperadorAritmetico>> ObtenerDatos(OperadorAritmetico pOperadorArismetico)
        {
            RespuestaConsulta<List<OperadorAritmetico>> resultado = new RespuestaConsulta<List<OperadorAritmetico>>();

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

        public RespuestaConsulta<List<OperadorAritmetico>> ValidarDatos(OperadorAritmetico objeto)
        {
            throw new NotImplementedException();
        }
    }
}
