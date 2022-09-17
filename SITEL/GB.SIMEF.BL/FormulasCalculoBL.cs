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
    public class FormulasCalculoBL : IMetodos<FormulasCalculo>
    {
        readonly string modulo = "";
        readonly string user = "";
        private readonly FormulasCalculoDAL FormulasCalculoDAL;

        public FormulasCalculoBL(string modulo, string user)
        {
            FormulasCalculoDAL = new FormulasCalculoDAL();
        }

        public RespuestaConsulta<List<FormulasCalculo>> ActualizarElemento(FormulasCalculo objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FormulasCalculo>> CambioEstado(FormulasCalculo objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FormulasCalculo>> ClonarDatos(FormulasCalculo objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FormulasCalculo>> EliminarElemento(FormulasCalculo objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FormulasCalculo>> InsertarDatos(FormulasCalculo objeto)
        {
            throw new NotImplementedException();
        }


        public RespuestaConsulta<List<FormulasCalculo>> ObtenerDatos(FormulasCalculo pFormulasCalculo)
        {
            RespuestaConsulta<List<FormulasCalculo>> resultado = new RespuestaConsulta<List<FormulasCalculo>>();

            try
            {
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                var result = FormulasCalculoDAL.ObtenerDatos(pFormulasCalculo);
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

        public RespuestaConsulta<List<FormulasCalculo>> ValidarDatos(FormulasCalculo objeto)
        {
            throw new NotImplementedException();
        }
    }
}
