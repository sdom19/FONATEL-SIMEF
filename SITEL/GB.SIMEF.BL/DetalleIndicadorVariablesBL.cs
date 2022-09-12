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
        private readonly IndicadorFonatelDAL indicadorFonatelDAL;

        public DetalleIndicadorVariablesBL(string pView, string pUser)
        {
            modulo = pView;
            user = pUser;
            detalleIndicadorVariablesDAL = new DetalleIndicadorVariablesDAL();
            indicadorFonatelDAL = new IndicadorFonatelDAL();
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

        /// <summary>
        /// 01/09/2022
        /// José Navarro Acuña
        /// Función que permite registrar un nuevo detalle de variable dato a un indicador
        /// </summary>
        /// <param name="pDetalleIndicadorVariables"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<DetalleIndicadorVariables>> InsertarDatos(DetalleIndicadorVariables pDetalleIndicadorVariables)
        {
            RespuestaConsulta<List<DetalleIndicadorVariables>> resultado = new RespuestaConsulta<List<DetalleIndicadorVariables>>();
            bool errorControlado = false;

            try
            {
                PrepararObjetoDetalle(pDetalleIndicadorVariables);
                resultado = ValidarDatos(pDetalleIndicadorVariables);

                if (resultado.HayError != 0)
                {
                    return resultado;
                }

                resultado.objetoRespuesta = detalleIndicadorVariablesDAL.ActualizarDatos(pDetalleIndicadorVariables);

                resultado.Usuario = user;
                resultado.CantidadRegistros = resultado.objetoRespuesta.Count;
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Insertar;

                //indicadorFonatelDAL.RegistrarBitacora(resultado.Accion,
                //        resultado.Usuario, resultado.Clase, pDetalleIndicadorVariables.NombreVariable);
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

        /// <summary>
        /// 12/09/2022
        /// José Navarro Acuña
        /// Función que validar el objeto DetalleIndicadorVariables. Verifica si el
        /// </summary>
        /// <param name="pDetalleIndicadorVariables"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<DetalleIndicadorVariables>> ValidarDatos(DetalleIndicadorVariables pDetalleIndicadorVariables)
        {
            RespuestaConsulta<List<DetalleIndicadorVariables>> resultado = new RespuestaConsulta<List<DetalleIndicadorVariables>>
            {
                HayError = (int)Error.NoError,
                objetoRespuesta = new List<DetalleIndicadorVariables>()
            };
            bool errorControlado = false;
            Indicador indicadorExistente = null;

            try
            {
                indicadorExistente = indicadorFonatelDAL.VerificarExistenciaIndicadorPorID(pDetalleIndicadorVariables.idIndicador);

                if (indicadorExistente == null)
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }
            }
            catch (Exception ex)
            {
                resultado.MensajeError = ex.Message;

                if (errorControlado)
                    resultado.HayError = (int)Error.ErrorControlado;
                else
                    resultado.HayError = (int)Error.ErrorSistema;
            }

            resultado.objetoRespuesta.Add(pDetalleIndicadorVariables);
            return resultado;
        }

        /// <summary>
        /// 12/09/2022
        /// José Navarro Acuña
        /// Prepara un objeto detalle variable para ser enviado al servicio DAL.
        /// Se preparan los id's de las tablas relacionadas para poder efectuar consultas debido a la encriptación.
        /// </summary>
        /// <param name="pDetalleIndicadorVariables"></param>
        private void PrepararObjetoDetalle(DetalleIndicadorVariables pDetalleIndicadorVariables)
        {
            if (!string.IsNullOrEmpty(pDetalleIndicadorVariables.id))
            {
                int.TryParse(Utilidades.Desencriptar(pDetalleIndicadorVariables.id), out int number);
                pDetalleIndicadorVariables.idDetalleIndicador = number;
            }

            if (!string.IsNullOrEmpty(pDetalleIndicadorVariables.idIndicadorString))
            {
                int.TryParse(Utilidades.Desencriptar(pDetalleIndicadorVariables.idIndicadorString), out int number);
                pDetalleIndicadorVariables.idIndicador = number;
            }
        }
    }
}
