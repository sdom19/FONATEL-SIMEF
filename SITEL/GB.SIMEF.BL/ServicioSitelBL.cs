using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.BL
{
    public class ServicioSitelBL
    {
        private readonly ServicioSitelDAL servicioSitelDAL;

        public ServicioSitelBL()
        {
            servicioSitelDAL = new ServicioSitelDAL();
        }

        /// <summary>
        /// 09/12/2022
        /// José Navarro Acuña
        /// Función que retorna todos los servicios de mercados
        /// </summary>
        /// <returns></returns>
        public RespuestaConsulta<List<ServicioSitel>> ObtenerDatosMercado()
        {
            RespuestaConsulta<List<ServicioSitel>> resultado = new RespuestaConsulta<List<ServicioSitel>>();

            try
            {
                var result = servicioSitelDAL.ObtenerDatosMercado();
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

        /// <summary>
        /// 09/12/2022
        /// José Navarro Acuña
        /// Función que retorna todos los servicios de calidad
        /// </summary>
        /// <returns></returns>
        public RespuestaConsulta<List<ServicioSitel>> ObtenerDatosCalidad()
        {
            RespuestaConsulta<List<ServicioSitel>> resultado = new RespuestaConsulta<List<ServicioSitel>>();

            try
            {
                var result = servicioSitelDAL.ObtenerDatosCalidad();
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

        /// <summary>
        /// 12/12/2022
        /// José Navarro Acuña
        /// Función que retorna los servicios de UIT
        /// </summary>
        /// <returns></returns>
        public RespuestaConsulta<List<ServicioSitel>> ObtenerDatosUIT()
        {
            RespuestaConsulta<List<ServicioSitel>> resultado = new RespuestaConsulta<List<ServicioSitel>>();

            try
            {
                var result = servicioSitelDAL.ObtenerDatosUIT();
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

        /// <summary>
        /// 12/12/2022
        /// José Navarro Acuña
        /// Función que retorna los servicios de indicadores cruzado
        /// </summary>
        /// <returns></returns>
        public RespuestaConsulta<List<ServicioSitel>> ObtenerDatosCruzado()
        {
            RespuestaConsulta<List<ServicioSitel>> resultado = new RespuestaConsulta<List<ServicioSitel>>();

            try
            {
                var result = servicioSitelDAL.ObtenerDatosCruzados();
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
    }
}
