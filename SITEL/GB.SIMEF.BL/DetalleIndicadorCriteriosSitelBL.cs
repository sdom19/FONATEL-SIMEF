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
    public class DetalleIndicadorCriteriosSitelBL : IMetodos<DetalleIndicadorVariable> // se adapta el modelo de detalles variables
    {
        private readonly DetalleIndicadorCriteriosSitelDAL detalleIndicadorCriteriosSitelDAL;

        public DetalleIndicadorCriteriosSitelBL()
        {
            detalleIndicadorCriteriosSitelDAL = new DetalleIndicadorCriteriosSitelDAL();
        }

        /// <summary>
        /// 23/12/2022
        /// José Navarro Acuña
        /// Función que retorna los criterios de un indicador proveniente de mercado
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<DetalleIndicadorVariable>> ObtenerDatosMercado(DetalleIndicadorVariable pDetalleIndicadorVariables)
        {
            RespuestaConsulta<List<DetalleIndicadorVariable>> resultado = new RespuestaConsulta<List<DetalleIndicadorVariable>>();

            try
            {
                if (!string.IsNullOrEmpty(pDetalleIndicadorVariables.idIndicadorString))
                {
                    int.TryParse(Utilidades.Desencriptar(pDetalleIndicadorVariables.idIndicadorString), out int number);
                    pDetalleIndicadorVariables.IdIndicador = number;
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
        /// Función que retorna los detalles de un criterio proveniente de mercados
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<DetalleIndicadorCategoria>> ObtenerDetallesDeCriterioMercado(DetalleIndicadorCategoria pDetalleIndicadorCategoria)
        {
            RespuestaConsulta<List<DetalleIndicadorCategoria>> resultado = new RespuestaConsulta<List<DetalleIndicadorCategoria>>();

            try
            {
                if (!string.IsNullOrEmpty(pDetalleIndicadorCategoria.idIndicadorString))
                {
                    int.TryParse(Utilidades.Desencriptar(pDetalleIndicadorCategoria.idIndicadorString), out int number);
                    pDetalleIndicadorCategoria.IdIndicador = number;
                }

                if (!string.IsNullOrEmpty(pDetalleIndicadorCategoria.idCriterioString))
                {
                    int.TryParse(Utilidades.Desencriptar(pDetalleIndicadorCategoria.idCriterioString), out int number);
                    pDetalleIndicadorCategoria.idCriterioInt = number;
                }

                if (pDetalleIndicadorCategoria.IdIndicador == 0 || pDetalleIndicadorCategoria.idCriterioInt == 0)
                {
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                resultado.objetoRespuesta = detalleIndicadorCriteriosSitelDAL.ObtenerDetallesDeCriterioMercado(pDetalleIndicadorCategoria).ToList();
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
        public RespuestaConsulta<List<DetalleIndicadorVariable>> ObtenerDatosCalidad(DetalleIndicadorVariable pDetalleIndicadorVariables)
        {
            RespuestaConsulta<List<DetalleIndicadorVariable>> resultado = new RespuestaConsulta<List<DetalleIndicadorVariable>>();

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
        public RespuestaConsulta<List<DetalleIndicadorVariable>> ObtenerDatosUIT(DetalleIndicadorVariable pDetalleIndicadorVariables)
        {
            RespuestaConsulta<List<DetalleIndicadorVariable>> resultado = new RespuestaConsulta<List<DetalleIndicadorVariable>>();

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
        public RespuestaConsulta<List<DetalleIndicadorVariable>> ObtenerDatosCruzado(DetalleIndicadorVariable pDetalleIndicadorVariables)
        {
            RespuestaConsulta<List<DetalleIndicadorVariable>> resultado = new RespuestaConsulta<List<DetalleIndicadorVariable>>();

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
        public RespuestaConsulta<List<DetalleIndicadorVariable>> ObtenerDatosExterno(DetalleIndicadorVariable pDetalleIndicadorVariables)
        {
            RespuestaConsulta<List<DetalleIndicadorVariable>> resultado = new RespuestaConsulta<List<DetalleIndicadorVariable>>();

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

        public RespuestaConsulta<List<DetalleIndicadorVariable>> ObtenerDatos(DetalleIndicadorVariable objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleIndicadorVariable>> ActualizarElemento(DetalleIndicadorVariable objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleIndicadorVariable>> CambioEstado(DetalleIndicadorVariable objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleIndicadorVariable>> ClonarDatos(DetalleIndicadorVariable objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleIndicadorVariable>> EliminarElemento(DetalleIndicadorVariable objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleIndicadorVariable>> InsertarDatos(DetalleIndicadorVariable objeto)
        {
            throw new NotImplementedException();
        }


        public RespuestaConsulta<List<DetalleIndicadorVariable>> ValidarDatos(DetalleIndicadorVariable objeto)
        {
            throw new NotImplementedException();
        }
    }
}
