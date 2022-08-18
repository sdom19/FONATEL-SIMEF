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
    public class TipoIndicadorBL : IMetodos<TipoIndicadores>
    {
        readonly string modulo = "";
        readonly string user = "";
        private readonly TipoIndicadorDAL tipoIndicadorDAL;

        public TipoIndicadorBL(string pView = "", string pUser = "")
        {
            modulo = pView;
            user = pUser;
            tipoIndicadorDAL = new TipoIndicadorDAL();
        }

        public RespuestaConsulta<List<TipoIndicadores>> ActualizarElemento(TipoIndicadores objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<TipoIndicadores>> CambioEstado(TipoIndicadores objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<TipoIndicadores>> ClonarDatos(TipoIndicadores objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<TipoIndicadores>> EliminarElemento(TipoIndicadores objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<TipoIndicadores>> InsertarDatos(TipoIndicadores objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<TipoIndicadores>> ObtenerDatos(TipoIndicadores pTipoIndicadores)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 18/08/2022
        /// José Navarro Acuña
        /// Función que retorna todos los tipos indicadores registrados en estado activo
        /// </summary>
        /// <returns></returns>
        public RespuestaConsulta<List<TipoIndicadores>> ObtenerDatos()
        {
            RespuestaConsulta<List<TipoIndicadores>> resultado = new RespuestaConsulta<List<TipoIndicadores>>();

            try
            {
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                var result = tipoIndicadorDAL.ObtenerDatos();
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

        public RespuestaConsulta<List<TipoIndicadores>> ValidarDatos(TipoIndicadores objeto)
        {
            throw new NotImplementedException();
        }
    }
}
