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
    public class DefinicionIndicadorBL : IMetodos<DefinicionIndicador>
    {
        readonly string modulo = "";
        readonly string user = "";
        private readonly DefinicionIndicadorDAL DefinicionIndicadorDAL;

        public DefinicionIndicadorBL()
        {
            DefinicionIndicadorDAL = new DefinicionIndicadorDAL();
        }

        public RespuestaConsulta<List<DefinicionIndicador>> ActualizarElemento(DefinicionIndicador objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DefinicionIndicador>> CambioEstado(DefinicionIndicador objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DefinicionIndicador>> ClonarDatos(DefinicionIndicador objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DefinicionIndicador>> EliminarElemento(DefinicionIndicador objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DefinicionIndicador>> InsertarDatos(DefinicionIndicador objeto)
        {
            throw new NotImplementedException();
        }


        public RespuestaConsulta<List<DefinicionIndicador>> ObtenerDatos(DefinicionIndicador pDefinicionIndicador)
        {
            RespuestaConsulta<List<DefinicionIndicador>> resultado = new RespuestaConsulta<List<DefinicionIndicador>>();

            try
            {
                


                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                var result = DefinicionIndicadorDAL.ObtenerDatos(pDefinicionIndicador);
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

        public RespuestaConsulta<List<DefinicionIndicador>> ValidarDatos(DefinicionIndicador objeto)
        {
            throw new NotImplementedException();
        }
    }
}
