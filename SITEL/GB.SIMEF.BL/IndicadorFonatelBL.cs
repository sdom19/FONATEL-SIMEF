using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL
{
    public class IndicadorFonatelBL : IMetodos<Indicador>
    {
        private readonly IndicadorFonatelDAL indicadorFonatelDAL;
        string modulo = "";

        public IndicadorFonatelBL()
        {
            indicadorFonatelDAL = new IndicadorFonatelDAL();
        }

        public RespuestaConsulta<List<Indicador>> ActualizarElemento(Indicador objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<Indicador>> CambioEstado(Indicador objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<Indicador>> ClonarDatos(Indicador objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<Indicador>> EliminarElemento(Indicador objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<Indicador>> InsertarDatos(Indicador objeto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 10/08/2022
        /// José Navarro Acuña
        /// Método que retorna todos los indicadores registrados en el sistema.
        /// Se puede realizar un filtrado de acuerdo al objecto que se envia.
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<Indicador>> ObtenerDatos(Indicador pIndicador)
        {
            RespuestaConsulta<List<Indicador>> resultado = new RespuestaConsulta<List<Indicador>>();

            try
            {
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                var result = indicadorFonatelDAL.ObtenerDatos(pIndicador);
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

        public RespuestaConsulta<Indicador> ValidarDatos(Indicador objeto)
        {
            throw new NotImplementedException();
        }

        RespuestaConsulta<List<Indicador>> IMetodos<Indicador>.ValidarDatos(Indicador objeto)
        {
            throw new NotImplementedException();
        }
    }
}
