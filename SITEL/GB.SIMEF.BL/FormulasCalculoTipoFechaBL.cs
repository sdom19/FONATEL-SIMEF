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
    public class FormulasCalculoTipoFechaBL : IMetodos<FormulasCalculoTipoFecha>
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
        public RespuestaConsulta<List<FormulasCalculoTipoFecha>> ObtenerDatos(FormulasCalculoTipoFecha pFormulasCalculoTipoFecha)
        {
            RespuestaConsulta<List<FormulasCalculoTipoFecha>> resultado = new RespuestaConsulta<List<FormulasCalculoTipoFecha>>();

            try
            {
                List<FormulasCalculoTipoFecha> result = formulasCalculoTipoFechaDAL.ObtenerDatos();
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

        public RespuestaConsulta<List<FormulasCalculoTipoFecha>> ActualizarElemento(FormulasCalculoTipoFecha objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FormulasCalculoTipoFecha>> CambioEstado(FormulasCalculoTipoFecha objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FormulasCalculoTipoFecha>> ClonarDatos(FormulasCalculoTipoFecha objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FormulasCalculoTipoFecha>> EliminarElemento(FormulasCalculoTipoFecha objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FormulasCalculoTipoFecha>> InsertarDatos(FormulasCalculoTipoFecha objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FormulasCalculoTipoFecha>> ValidarDatos(FormulasCalculoTipoFecha objeto)
        {
            throw new NotImplementedException();
        }
    }
}
