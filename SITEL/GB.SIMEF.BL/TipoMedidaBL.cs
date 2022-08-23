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
    public class TipoMedidaBL : IMetodos<TipoMedida>
    {
        readonly string modulo = "";
        readonly string user = "";
        private readonly TipoMedidaDAL tipoMedidaDAL;

        public TipoMedidaBL(string pModulo, string pUser)
        {
            modulo = pModulo;
            user = pUser;
            tipoMedidaDAL = new TipoMedidaDAL();
        }

        public RespuestaConsulta<List<TipoMedida>> ActualizarElemento(TipoMedida objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<TipoMedida>> CambioEstado(TipoMedida objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<TipoMedida>> ClonarDatos(TipoMedida objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<TipoMedida>> EliminarElemento(TipoMedida objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<TipoMedida>> InsertarDatos(TipoMedida objeto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 23/08/2022
        /// José Navarro Acuña
        /// Función que retorna todas las tipos de medidas de indicadores registrados en estado activo
        /// Se puede aplicar un filtro para obtener un único elemento a traves del ID.
        /// </summary>
        /// <param name="pTipoMedida"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<TipoMedida>> ObtenerDatos(TipoMedida pTipoMedida)
        {
            RespuestaConsulta<List<TipoMedida>> resultado = new RespuestaConsulta<List<TipoMedida>>();

            try
            {
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                var result = tipoMedidaDAL.ObtenerDatos(pTipoMedida);
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

        public RespuestaConsulta<List<TipoMedida>> ValidarDatos(TipoMedida objeto)
        {
            throw new NotImplementedException();
        }
    }
}
