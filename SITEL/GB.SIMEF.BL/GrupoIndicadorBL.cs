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
    public class GrupoIndicadorBL : IMetodos<GrupoIndicadores>
    {
        readonly string modulo = "";
        readonly string user = "";
        private readonly GrupoIndicadorDAL grupoIndicadorDAL;

        public GrupoIndicadorBL(string pModulo, string pUser)
        {
            modulo = pModulo;
            user = pUser;
            grupoIndicadorDAL = new GrupoIndicadorDAL();
        }

        public RespuestaConsulta<List<GrupoIndicadores>> ActualizarElemento(GrupoIndicadores objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<GrupoIndicadores>> CambioEstado(GrupoIndicadores objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<GrupoIndicadores>> ClonarDatos(GrupoIndicadores objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<GrupoIndicadores>> EliminarElemento(GrupoIndicadores objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<GrupoIndicadores>> InsertarDatos(GrupoIndicadores objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<GrupoIndicadores>> ObtenerDatos(GrupoIndicadores objeto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 18/08/2022
        /// José Navarro Acuña
        /// Función que retorna todos los grupos de indicadores registrados en estado activo
        /// </summary>
        /// <returns></returns>
        public RespuestaConsulta<List<GrupoIndicadores>> ObtenerDatos()
        {
            RespuestaConsulta<List<GrupoIndicadores>> resultado = new RespuestaConsulta<List<GrupoIndicadores>>();

            try
            {
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                var result = grupoIndicadorDAL.ObtenerDatos();
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

        public RespuestaConsulta<List<GrupoIndicadores>> ValidarDatos(GrupoIndicadores objeto)
        {
            throw new NotImplementedException();
        }
    }
}
