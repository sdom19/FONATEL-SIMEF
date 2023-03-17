using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL
{
    public class FormulasCalculoTipoFechaBL : IMetodos<FormulaCalculoTipoFecha>
    {
        private readonly FormulasCalculoTipoFechaDAL formulasCalculoTipoFechaDAL;

        public FormulasCalculoTipoFechaBL()
        {
            formulasCalculoTipoFechaDAL = new FormulasCalculoTipoFechaDAL();
        }

        /// <summary>
        /// 09/01/2023
        /// José Navarro Acuña
        /// Función que permite obtener los tipos de fechas para la defición de argumentos
        /// </summary>
        /// <param name="pFormulasCalculoTipoFecha"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<FormulaCalculoTipoFecha>> ObtenerDatos(FormulaCalculoTipoFecha pFormulasCalculoTipoFecha)
        {
            RespuestaConsulta<List<FormulaCalculoTipoFecha>> resultado = new RespuestaConsulta<List<FormulaCalculoTipoFecha>>();

            try
            {
                List<FormulaCalculoTipoFecha> result = formulasCalculoTipoFechaDAL.ObtenerDatos();
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

        public RespuestaConsulta<List<FormulaCalculoTipoFecha>> ActualizarElemento(FormulaCalculoTipoFecha objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FormulaCalculoTipoFecha>> CambioEstado(FormulaCalculoTipoFecha objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FormulaCalculoTipoFecha>> ClonarDatos(FormulaCalculoTipoFecha objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FormulaCalculoTipoFecha>> EliminarElemento(FormulaCalculoTipoFecha objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FormulaCalculoTipoFecha>> InsertarDatos(FormulaCalculoTipoFecha objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FormulaCalculoTipoFecha>> ValidarDatos(FormulaCalculoTipoFecha objeto)
        {
            throw new NotImplementedException();
        }
    }
}
