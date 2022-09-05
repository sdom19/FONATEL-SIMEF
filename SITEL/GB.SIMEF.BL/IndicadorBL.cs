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
    public class IndicadorBL : IMetodos<Indicador>
    {
        private readonly IndicadorDAL clsDatos;


        private RespuestaConsulta<List<Indicador>> ResultadoConsulta;
        string modulo = Etiquetas.Indicador;

        public IndicadorBL() 
        {
            this.clsDatos = new IndicadorDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<Indicador>>();
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

        public RespuestaConsulta<List<Indicador>> ObtenerDatos(Indicador objIndicador)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;
                var resultado = clsDatos.ObtenerDatos(objIndicador);
                ResultadoConsulta.objetoRespuesta = resultado;
                ResultadoConsulta.CantidadRegistros = resultado.Count();
            }

            catch (Exception ex)
            {
                ResultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<Indicador>> ValidarDatos(Indicador objeto)
        {
            throw new NotImplementedException();
        }
    }
}
