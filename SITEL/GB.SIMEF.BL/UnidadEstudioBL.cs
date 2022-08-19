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
    public class UnidadEstudioBL : IMetodos<UnidadEstudio>
    {
        readonly string modulo = "";
        readonly string user = "";
        private readonly UnidadEstudioDAL unidadEstudioDAL;

        public UnidadEstudioBL(string pModulo, string pUser)
        {
            modulo = pModulo;
            user = pUser;
            unidadEstudioDAL = new UnidadEstudioDAL();
        }

        public RespuestaConsulta<List<UnidadEstudio>> ActualizarElemento(UnidadEstudio objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<UnidadEstudio>> CambioEstado(UnidadEstudio objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<UnidadEstudio>> ClonarDatos(UnidadEstudio objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<UnidadEstudio>> EliminarElemento(UnidadEstudio objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<UnidadEstudio>> InsertarDatos(UnidadEstudio objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<UnidadEstudio>> ObtenerDatos(UnidadEstudio objeto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 19/08/2022
        /// José Navarro Acuña
        /// Función que retorna todos las unidades de estudio registradas en estado activo
        /// </summary>
        /// <returns></returns>
        public RespuestaConsulta<List<UnidadEstudio>> ObtenerDatos()
        {
            RespuestaConsulta<List<UnidadEstudio>> resultado = new RespuestaConsulta<List<UnidadEstudio>>();

            try
            {
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                var result = unidadEstudioDAL.ObtenerDatos();
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

        public RespuestaConsulta<List<UnidadEstudio>> ValidarDatos(UnidadEstudio objeto)
        {
            throw new NotImplementedException();
        }
    }
}
