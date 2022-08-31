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
    public class AnnoBL : IMetodos<Anno>
    {
        readonly string modulo = "";
        readonly string user = "";
        private readonly AnnoDAL AnnoDAL;

        public AnnoBL()
        {
            AnnoDAL = new AnnoDAL();
        }

        public RespuestaConsulta<List<Anno>> ActualizarElemento(Anno objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<Anno>> CambioEstado(Anno objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<Anno>> ClonarDatos(Anno objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<Anno>> EliminarElemento(Anno objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<Anno>> InsertarDatos(Anno objeto)
        {
            throw new NotImplementedException();
        }


        public RespuestaConsulta<List<Anno>> ObtenerDatos(Anno pAnno)
        {
            RespuestaConsulta<List<Anno>> resultado = new RespuestaConsulta<List<Anno>>();

            try
            {
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                var result = AnnoDAL.ObtenerDatos();
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

        public RespuestaConsulta<List<Anno>> ValidarDatos(Anno objeto)
        {
            throw new NotImplementedException();
        }
    }
}
