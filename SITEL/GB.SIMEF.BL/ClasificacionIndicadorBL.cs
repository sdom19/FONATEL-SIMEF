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
    public class ClasificacionIndicadorBL : IMetodos<ClasificacionIndicadores>
    {
        readonly string modulo = "";
        readonly string user = "";
        private readonly ClasificacionIndicadorDAL clasificacionIndicadorDAL;

        public ClasificacionIndicadorBL(string pModulo, string pUser)
        {
            modulo = pModulo;
            user = pUser;
            clasificacionIndicadorDAL = new ClasificacionIndicadorDAL();
        }

        public RespuestaConsulta<List<ClasificacionIndicadores>> ActualizarElemento(ClasificacionIndicadores objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<ClasificacionIndicadores>> CambioEstado(ClasificacionIndicadores objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<ClasificacionIndicadores>> ClonarDatos(ClasificacionIndicadores objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<ClasificacionIndicadores>> EliminarElemento(ClasificacionIndicadores objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<ClasificacionIndicadores>> InsertarDatos(ClasificacionIndicadores objeto)
        {
            throw new NotImplementedException();
        }

        /// 23/08/2022
        /// José Navarro Acuña
        /// Función que retorna todas las clasificaciones de indicadores registradas en estado activo
        /// Se puede aplicar un filtro para obtener un único elemento a traves del ID.
        public RespuestaConsulta<List<ClasificacionIndicadores>> ObtenerDatos(ClasificacionIndicadores pClasificacionIndicadores)
        {
            RespuestaConsulta<List<ClasificacionIndicadores>> resultado = new RespuestaConsulta<List<ClasificacionIndicadores>>();

            try
            {
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                var result = clasificacionIndicadorDAL.ObtenerDatos(pClasificacionIndicadores);
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

        public RespuestaConsulta<List<ClasificacionIndicadores>> ValidarDatos(ClasificacionIndicadores objeto)
        {
            throw new NotImplementedException();
        }
    }
}
