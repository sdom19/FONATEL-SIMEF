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
    public class DetalleIndicadorCriteriosSitelBL : IMetodos<DetalleIndicadorVariables> // se adapta el modelo de detalles variables
    {
        private readonly DetalleIndicadorCriteriosSitelDAL detalleIndicadorCriteriosSitelDAL;

        public DetalleIndicadorCriteriosSitelBL()
        {
            detalleIndicadorCriteriosSitelDAL = new DetalleIndicadorCriteriosSitelDAL();
        }

        /// <summary>
        /// 23/12/2022
        /// José Navarro Acuña
        /// Función que retorna los detalles criterio de un indicador proveniente de mercado
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<DetalleIndicadorVariables>> ObtenerDatosMercado(DetalleIndicadorVariables pDetalleIndicadorVariables)
        {
            RespuestaConsulta<List<DetalleIndicadorVariables>> resultado = new RespuestaConsulta<List<DetalleIndicadorVariables>>();

            try
            {
                if (!string.IsNullOrEmpty(pDetalleIndicadorVariables.idIndicadorString))
                {
                    int.TryParse(Utilidades.Desencriptar(pDetalleIndicadorVariables.idIndicadorString), out int number);
                    pDetalleIndicadorVariables.idIndicador = number;
                }

                resultado.objetoRespuesta = detalleIndicadorCriteriosSitelDAL.ObtenerDatosMercado(pDetalleIndicadorVariables).ToList();
                resultado.CantidadRegistros = resultado.objetoRespuesta.Count;
            }
            catch (Exception ex)
            {
                resultado.MensajeError = ex.Message;
                resultado.HayError = (int)Error.ErrorSistema;
            }
            return resultado;
        }

        /// <summary>
        /// 23/12/2022
        /// José Navarro Acuña
        /// Función que retorna los detalles criterio de un indicador proveniente de calidad
        /// </summary>
        /// <param name="pDetalleIndicadorVariables"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<DetalleIndicadorVariables>> ObtenerDatosCalidad(DetalleIndicadorVariables pDetalleIndicadorVariables)
        {
            RespuestaConsulta<List<DetalleIndicadorVariables>> resultado = new RespuestaConsulta<List<DetalleIndicadorVariables>>();

            try
            {
                if (!string.IsNullOrEmpty(pDetalleIndicadorVariables.idIndicadorString))
                {
                    pDetalleIndicadorVariables.id = Utilidades.Desencriptar(pDetalleIndicadorVariables.idIndicadorString);
                }

                resultado.objetoRespuesta = detalleIndicadorCriteriosSitelDAL.ObtenerDatosCalidad(pDetalleIndicadorVariables).ToList();
                resultado.CantidadRegistros = resultado.objetoRespuesta.Count;
            }
            catch (Exception ex)
            {
                resultado.MensajeError = ex.Message;
                resultado.HayError = (int)Error.ErrorSistema;
            }
            return resultado;
        }

        /// <summary>
        /// 23/12/2022
        /// José Navarro Acuña
        /// Función que retorna los detalles criterio de un indicador proveniente de calidad
        /// </summary>
        /// <param name="pDetalleIndicadorVariables"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<DetalleIndicadorVariables>> ObtenerDatosUIT(DetalleIndicadorVariables pDetalleIndicadorVariables)
        {
            RespuestaConsulta<List<DetalleIndicadorVariables>> resultado = new RespuestaConsulta<List<DetalleIndicadorVariables>>();

            try
            {
                if (!string.IsNullOrEmpty(pDetalleIndicadorVariables.idIndicadorString))
                {
                    pDetalleIndicadorVariables.id = Utilidades.Desencriptar(pDetalleIndicadorVariables.idIndicadorString);
                }

                resultado.objetoRespuesta = detalleIndicadorCriteriosSitelDAL.ObtenerDatosIUT(pDetalleIndicadorVariables).ToList();
                resultado.CantidadRegistros = resultado.objetoRespuesta.Count;
            }
            catch (Exception ex)
            {
                resultado.MensajeError = ex.Message;
                resultado.HayError = (int)Error.ErrorSistema;
            }
            return resultado;
        }

        /// <summary>
        /// 23/12/2022
        /// José Navarro Acuña
        /// Función que retorna los detalles criterio de un indicador cruzado
        /// </summary>
        /// <param name="pDetalleIndicadorVariables"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<DetalleIndicadorVariables>> ObtenerDatosCruzado(DetalleIndicadorVariables pDetalleIndicadorVariables)
        {
            RespuestaConsulta<List<DetalleIndicadorVariables>> resultado = new RespuestaConsulta<List<DetalleIndicadorVariables>>();

            try
            {
                if (!string.IsNullOrEmpty(pDetalleIndicadorVariables.idIndicadorString))
                {
                    pDetalleIndicadorVariables.id = Utilidades.Desencriptar(pDetalleIndicadorVariables.idIndicadorString);
                }

                resultado.objetoRespuesta = detalleIndicadorCriteriosSitelDAL.ObtenerDatosCruzado(pDetalleIndicadorVariables).ToList();
                resultado.CantidadRegistros = resultado.objetoRespuesta.Count;
            }
            catch (Exception ex)
            {
                resultado.MensajeError = ex.Message;
                resultado.HayError = (int)Error.ErrorSistema;
            }
            return resultado;
        }

        /// <summary>
        /// 23/12/2022
        /// José Navarro Acuña
        /// Función que retorna los detalles criterio de un indicador externo
        /// </summary>
        /// <param name="pDetalleIndicadorVariables"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<DetalleIndicadorVariables>> ObtenerDatosExterno(DetalleIndicadorVariables pDetalleIndicadorVariables)
        {
            RespuestaConsulta<List<DetalleIndicadorVariables>> resultado = new RespuestaConsulta<List<DetalleIndicadorVariables>>();

            try
            {
                if (!string.IsNullOrEmpty(pDetalleIndicadorVariables.idIndicadorString))
                {
                    pDetalleIndicadorVariables.id = Utilidades.Desencriptar(pDetalleIndicadorVariables.idIndicadorString);
                }

                resultado.objetoRespuesta = detalleIndicadorCriteriosSitelDAL.ObtenerDatosExterno(pDetalleIndicadorVariables).ToList();
                resultado.CantidadRegistros = resultado.objetoRespuesta.Count;
            }
            catch (Exception ex)
            {
                resultado.MensajeError = ex.Message;
                resultado.HayError = (int)Error.ErrorSistema;
            }
            return resultado;
        }

        public RespuestaConsulta<List<DetalleIndicadorVariables>> ObtenerDatos(DetalleIndicadorVariables objeto)
        {
            throw new NotImplementedException();
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


        public RespuestaConsulta<List<DetalleIndicadorVariables>> ValidarDatos(DetalleIndicadorVariables objeto)
        {
            throw new NotImplementedException();
        }
    }
}
