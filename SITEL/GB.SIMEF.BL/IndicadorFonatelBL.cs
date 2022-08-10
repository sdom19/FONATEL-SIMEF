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
        private readonly IndicadorFonatelDAL clsDatos;
        string modulo = "";

        public IndicadorFonatelBL()
        {
            this.clsDatos = new IndicadorFonatelDAL();
        }

        public RespuestaConsulta<Indicador> ActualizarElemento(Indicador objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<Indicador> CambioEstado(Indicador objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<Indicador> ClonarDatos(Indicador objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<Indicador>> EliminarElemento(Indicador objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<Indicador> InsertarDatos(Indicador objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<Indicador>> ObtenerDatos(Indicador pIndicador)
        {
            RespuestaConsulta<List<Indicador>> resultado = new RespuestaConsulta<List<Indicador>>();

            try
            {
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                var result = clsDatos.ObtenerDatos(pIndicador);
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
    }
}
