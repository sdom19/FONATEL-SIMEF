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
    public class TipoGraficoBL : IMetodos<GraficoInforme>
    {
        readonly string modulo = "";
        readonly string user = "";
        private readonly TipoGraficoDAL tipoGraficoDAL;

        public TipoGraficoBL(string pModulo, string pUser)
        {
            modulo = pModulo;
            user = pUser;
            tipoGraficoDAL = new TipoGraficoDAL();
        }

        /// <summary>
        /// 04/04/2023
        /// Jenifer Mora Badilla
        /// Función que retorna todos tipos de graficos registrados en estado activo
        /// Se puede aplicar un filtro para obtener un único elemento a traves del ID.
        /// </summary>
        /// <returns></returns>
        public RespuestaConsulta<List<GraficoInforme>> ObtenerDatos(GraficoInforme pGraficoInforme)
        {
            RespuestaConsulta<List<GraficoInforme>> resultado = new RespuestaConsulta<List<GraficoInforme>>();

            try
            {
                resultado.Accion = (int)Accion.Consultar;
                var result = tipoGraficoDAL.ObtenerDatos(pGraficoInforme);
                resultado.objetoRespuesta = result;
                resultado.CantidadRegistros = result.Count();
            }
            catch (Exception ex)
            {
                resultado.HayError = (int)Error.ErrorSistema;
                resultado.MensajeError = ex.Message;
            }
            return resultado;
        }

        public RespuestaConsulta<List<GraficoInforme>> InsertarDatos(GraficoInforme objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<GraficoInforme>> ClonarDatos(GraficoInforme objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<GraficoInforme>> CambioEstado(GraficoInforme objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<GraficoInforme>> EliminarElemento(GraficoInforme objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<GraficoInforme>> ActualizarElemento(GraficoInforme objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<GraficoInforme>> ValidarDatos(GraficoInforme objeto)
        {
            throw new NotImplementedException();
        }
        

    }
}
