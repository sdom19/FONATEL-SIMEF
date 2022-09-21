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
     
            RespuestaConsulta<List<DetalleIndicadorCategoria>> resultado = new RespuestaConsulta<List<DetalleIndicadorCategoria>>();
            bool errorControlado = false;
            try
            {
                int.TryParse(Utilidades.Desencriptar(objeto.idIndicadorString), out int idindicador);
                int.TryParse(Utilidades.Desencriptar(objeto.idCategoriaString), out int idcategoria);


                objeto.idIndicador = idindicador;
                objeto.idCategoria = idcategoria;
                objeto.DetallesAgrupados = false;
                var listaDetalle = detalleIndicadorCategoriaDAL.ObtenerDatos(objeto);


                resultado.objetoRespuesta = new List<DetalleIndicadorCategoria>();
                foreach (var item in listaDetalle)
                {
                    int.TryParse(Utilidades.Desencriptar(item.idCategoriaDetalleString), out int idcategoriaDetalle);
                    item.idCategoriaDetalle = idcategoriaDetalle;
                    item.idIndicador = idindicador;
                    item.idCategoria = idcategoria;
                    item.Estado = false;
                    resultado.objetoRespuesta.AddRange(detalleIndicadorCategoriaDAL.ActualizarDatos(item));
                }

                


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

        public RespuestaConsulta<List<DetalleIndicadorCategoria>> InsertarDatos(DetalleIndicadorCategoria objeto)
        {
            throw new NotImplementedException();
        }




        public RespuestaConsulta<List<DetalleIndicadorCategoria>> InsertarDatos(List< DetalleIndicadorCategoria> pDetalleIndicadorCategoria)
        {


            RespuestaConsulta<List<DetalleIndicadorCategoria>> resultado = new RespuestaConsulta<List<DetalleIndicadorCategoria>>();
            bool errorControlado = false;
            try
            {
                pDetalleIndicadorCategoria = pDetalleIndicadorCategoria.Where(x => x.idCategoriaDetalleString != "all").ToList();
                resultado.objetoRespuesta = new List<DetalleIndicadorCategoria>();
                foreach (var DetalleIndicadorCategoria in pDetalleIndicadorCategoria)
                {
                    int.TryParse(Utilidades.Desencriptar(DetalleIndicadorCategoria.idCategoriaString), out int idCategoria);
                    int.TryParse(Utilidades.Desencriptar(DetalleIndicadorCategoria.idIndicadorString), out int idindicador);
                    int.TryParse(Utilidades.Desencriptar(DetalleIndicadorCategoria.idCategoriaDetalleString), out int idCategoriaDetalle);
                    DetalleIndicadorCategoria.Estado = true;
                    DetalleIndicadorCategoria.idCategoria = idCategoria;
                    DetalleIndicadorCategoria.idIndicador = idindicador;
                    DetalleIndicadorCategoria.idCategoriaDetalle = idCategoriaDetalle;
                   var result = detalleIndicadorCategoriaDAL.ActualizarDatos(DetalleIndicadorCategoria);
                   resultado.objetoRespuesta.AddRange(result);
                }
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
                if (pDetalleIndicadorCategoria.Estado && resultado.objetoRespuesta.Count>0)
                {
                    resultado.objetoRespuesta = resultado.objetoRespuesta.Where(x => x.Estado == true).ToList();
                }
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
