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
    public class DetalleIndicadorCategoriaBL : IMetodos<DetalleIndicadorCategoria>
    {
        private readonly string modulo = "";
        private readonly string user = "";
        private readonly DetalleIndicadorCategoriaDAL detalleIndicadorCategoriaDAL;

        public DetalleIndicadorCategoriaBL(string pView, string pUser)
        {
            modulo = pView;
            user = pUser;
            detalleIndicadorCategoriaDAL = new DetalleIndicadorCategoriaDAL();
        }

        public RespuestaConsulta<List<DetalleIndicadorCategoria>> ActualizarElemento(DetalleIndicadorCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleIndicadorCategoria>> CambioEstado(DetalleIndicadorCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleIndicadorCategoria>> ClonarDatos(DetalleIndicadorCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleIndicadorCategoria>> EliminarElemento(DetalleIndicadorCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleIndicadorCategoria>> InsertarDatos(DetalleIndicadorCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleIndicadorCategoria>> ObtenerDatos(DetalleIndicadorCategoria objeto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 06/09/2022
        /// José Navarro Acuña
        /// Función que permite obtener los detalles de categorias de un indicador
        /// </summary>
        /// <param name="pDetalleIndicadorCategoria"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<DetalleIndicadorCategoria>> ObtenerDatosPorIndicador(DetalleIndicadorCategoria pDetalleIndicadorCategoria)
        {
            RespuestaConsulta<List<DetalleIndicadorCategoria>> resultado = new RespuestaConsulta<List<DetalleIndicadorCategoria>>();
            bool errorControlado = false;

            try
            {
                int.TryParse(Utilidades.Desencriptar(pDetalleIndicadorCategoria.idIndicadorString), out int number);
                pDetalleIndicadorCategoria.idIndicador = number;

                if (pDetalleIndicadorCategoria.idIndicador == 0) // ¿ID descencriptado con éxito?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                resultado.objetoRespuesta = detalleIndicadorCategoriaDAL.ObtenerDatos(pDetalleIndicadorCategoria).ToList();
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
        /// 07/09/2022
        /// José Navarro Acuña
        /// Función que permite obtener los detalles de categorias de un indicador y categoria
        /// </summary>
        /// <param name="pDetalleIndicadorCategoria"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<DetalleIndicadorCategoria>> ObtenerDatosPorIndicadorYCategoria(DetalleIndicadorCategoria pDetalleIndicadorCategoria)
        {
            RespuestaConsulta<List<DetalleIndicadorCategoria>> resultado = new RespuestaConsulta<List<DetalleIndicadorCategoria>>();
            bool errorControlado = false;

            try
            {
                int.TryParse(Utilidades.Desencriptar(pDetalleIndicadorCategoria.idIndicadorString), out int number);
                pDetalleIndicadorCategoria.idIndicador = number;

                if (pDetalleIndicadorCategoria.idIndicador == 0) // ¿ID descencriptado con éxito?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                int.TryParse(Utilidades.Desencriptar(pDetalleIndicadorCategoria.idCategoriaString), out number);
                pDetalleIndicadorCategoria.idCategoria = number;

                if (pDetalleIndicadorCategoria.idCategoria == 0) // ¿ID descencriptado con éxito?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                resultado.objetoRespuesta = detalleIndicadorCategoriaDAL.ObtenerDatos(pDetalleIndicadorCategoria).ToList();
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

        public RespuestaConsulta<List<DetalleIndicadorCategoria>> ValidarDatos(DetalleIndicadorCategoria objeto)
        {
            throw new NotImplementedException();
        }
    }
}
