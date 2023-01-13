using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL
{
    public class FormulasCalculoUnidadMedidaBL : IMetodos<FormulasCalculoUnidadMedida>
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
        public RespuestaConsulta<List<FormulasCalculoUnidadMedida>> ObtenerDatos(FormulasCalculoUnidadMedida pFormulasCalculoUnidadMedida)
        {
            RespuestaConsulta<List<FormulasCalculoUnidadMedida>> resultado = new RespuestaConsulta<List<FormulasCalculoUnidadMedida>>();

            try
            {
                resultado.Accion = (int)Accion.Consultar;
                List<FormulasCalculoUnidadMedida> listado = formulasCalculoUnidadMedidaDAL.ObtenerDatos();
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

        public RespuestaConsulta<List<FormulasCalculoUnidadMedida>> ActualizarElemento(FormulasCalculoUnidadMedida objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FormulasCalculoUnidadMedida>> CambioEstado(FormulasCalculoUnidadMedida objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FormulasCalculoUnidadMedida>> ClonarDatos(FormulasCalculoUnidadMedida objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FormulasCalculoUnidadMedida>> EliminarElemento(FormulasCalculoUnidadMedida objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FormulasCalculoUnidadMedida>> InsertarDatos(FormulasCalculoUnidadMedida objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FormulasCalculoUnidadMedida>> ValidarDatos(FormulasCalculoUnidadMedida objeto)
        {
            throw new NotImplementedException();
        }
    }
}
