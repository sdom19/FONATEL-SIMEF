using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL
{
    public class FormulasCalculoUnidadMedidaBL : IMetodos<FormulaCalculoUnidadMedida>
    {
        private readonly FormulasCalculoUnidadMedidaDAL formulasCalculoUnidadMedidaDAL;

        public FormulasCalculoUnidadMedidaBL()
        {
            formulasCalculoUnidadMedidaDAL = new FormulasCalculoUnidadMedidaDAL();
        }

        /// <summary>
        /// 12/01/2023
        /// José Navarro Acuña
        /// Función que permite obtener las unidades de medida de la defición de fechas
        /// </summary>
        /// <param name="pFormulasCalculoUnidadMedida"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<FormulaCalculoUnidadMedida>> ObtenerDatos(FormulaCalculoUnidadMedida pFormulasCalculoUnidadMedida)
        {
            RespuestaConsulta<List<FormulaCalculoUnidadMedida>> resultado = new RespuestaConsulta<List<FormulaCalculoUnidadMedida>>();

            try
            {
                resultado.Accion = (int)Accion.Consultar;
                List<FormulaCalculoUnidadMedida> listado = formulasCalculoUnidadMedidaDAL.ObtenerDatos();
                resultado.objetoRespuesta = listado;
                resultado.CantidadRegistros = listado.Count();
            }
            catch (Exception ex)
            {
                resultado.HayError = (int)Error.ErrorSistema;
                resultado.MensajeError = ex.Message;
            }
            return resultado;
        }

        public RespuestaConsulta<List<FormulaCalculoUnidadMedida>> ActualizarElemento(FormulaCalculoUnidadMedida objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FormulaCalculoUnidadMedida>> CambioEstado(FormulaCalculoUnidadMedida objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FormulaCalculoUnidadMedida>> ClonarDatos(FormulaCalculoUnidadMedida objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FormulaCalculoUnidadMedida>> EliminarElemento(FormulaCalculoUnidadMedida objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FormulaCalculoUnidadMedida>> InsertarDatos(FormulaCalculoUnidadMedida objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FormulaCalculoUnidadMedida>> ValidarDatos(FormulaCalculoUnidadMedida objeto)
        {
            throw new NotImplementedException();
        }
    }
}
