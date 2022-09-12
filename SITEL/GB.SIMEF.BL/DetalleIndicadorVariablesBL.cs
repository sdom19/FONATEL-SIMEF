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
    public class DetalleIndicadorVariablesBL : IMetodos<DetalleIndicadorVariables>
    {
        private readonly string modulo = "";
        private readonly string user = "";
        private readonly DetalleIndicadorVariablesDAL detalleIndicadorVariablesDAL;

        public DetalleIndicadorVariablesBL(string pView, string pUser)
        {
            modulo = pView;
            user = pUser;
            detalleIndicadorVariablesDAL = new DetalleIndicadorVariablesDAL();
        }

        public RespuestaConsulta<List<DetalleIndicadorVariables>> ActualizarElemento(DetalleIndicadorVariables objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleIndicadorVariables>> CambioEstado(DetalleIndicadorVariables objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleIndicadorVariables>> ClonarDatos(DetalleIndicadorVariables objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleIndicadorVariables>> EliminarElemento(DetalleIndicadorVariables objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleIndicadorVariables>> InsertarDatos(DetalleIndicadorVariables objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleIndicadorVariables>> ObtenerDatos(DetalleIndicadorVariables objeto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 01/09/2022
        /// José Navarro Acuña
        /// Función que permite obtener los detalles variables dato de un indicador
        /// </summary>
        /// <param name="pDetalleIndicadorVariables"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<DetalleIndicadorVariables>> ObtenerDatosPorIndicador(DetalleIndicadorVariables pDetalleIndicadorVariables)
        {
            RespuestaConsulta<List<DetalleIndicadorVariables>> resultado = new RespuestaConsulta<List<DetalleIndicadorVariables>>();
            bool errorControlado = false;

            try
            {
                int.TryParse(Utilidades.Desencriptar(pDetalleIndicadorVariables.idIndicadorString), out int number);
                pDetalleIndicadorVariables.idIndicador = number;

                if (pDetalleIndicadorVariables.idIndicador == 0) // ¿ID descencriptado con éxito?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                resultado.objetoRespuesta = detalleIndicadorVariablesDAL.ObtenerDatos(pDetalleIndicadorVariables).ToList();
                resultado.CantidadRegistros = resultado.objetoRespuesta.Count;
            }
            catch (Exception ex)
            {
                resultado.MensajeError = ex.Message;

                if (errorControlado)
                    resultado.HayError = (int)Error.ErrorControlado;
                else
                    resultado.HayError = (int)Error.ErrorSistema;
            }
            return resultado;
        }

        public RespuestaConsulta<List<DetalleIndicadorVariables>> ValidarDatos(DetalleIndicadorVariables objeto)
        {
            throw new NotImplementedException();
        }
    }
}
