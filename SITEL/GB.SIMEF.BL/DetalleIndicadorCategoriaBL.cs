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
        private readonly DetalleCategoriaTextoDAL detalleCategoriaTextoDAL;
        private readonly IndicadorFonatelDAL indicadorFonatelDAL;
        private readonly CategoriasDesagregacionDAL categoriasDesagregacionDAL;

        public DetalleIndicadorCategoriaBL(string pView, string pUser)
        {
            modulo = pView;
            user = pUser;
            detalleIndicadorCategoriaDAL = new DetalleIndicadorCategoriaDAL();
            indicadorFonatelDAL = new IndicadorFonatelDAL();
            detalleCategoriaTextoDAL = new DetalleCategoriaTextoDAL();
            categoriasDesagregacionDAL = new CategoriasDesagregacionDAL();
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


        /// <summary>
        /// 08/11/2022
        /// José Navarro Acuña
        /// Función que permite insertar los detalles de categorias de un indicador
        /// </summary>
        /// <param name="pDetalleIndicadorCategoria"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<DetalleIndicadorCategoria>> InsertarDatos(DetalleIndicadorCategoria pDetalleIndicadorCategoria)
        {
            RespuestaConsulta<List<DetalleIndicadorCategoria>> resultado = new RespuestaConsulta<List<DetalleIndicadorCategoria>>();

            bool errorControlado = false;
            try
            {
                PrepararObjetoDetalle(pDetalleIndicadorCategoria);
                resultado = ValidarDatos(pDetalleIndicadorCategoria);

                for (int i = 0; i < pDetalleIndicadorCategoria.listaDetallesCategoria.Count; i++)
                {
                    List<DetalleIndicadorCategoria> result = detalleIndicadorCategoriaDAL.ActualizarDatos(
                        new DetalleIndicadorCategoria()
                        {
                            idIndicador = pDetalleIndicadorCategoria.idIndicador,
                            idCategoria = pDetalleIndicadorCategoria.idCategoria,
                            idCategoriaDetalle = pDetalleIndicadorCategoria.listaDetallesCategoria[i],
                            Estado = true
                        }
                    );

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
                PrepararObjetoDetalle(pDetalleIndicadorCategoria);

                if (pDetalleIndicadorCategoria.idIndicador == 0) // ¿ID descencriptado con éxito?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                resultado.objetoRespuesta = detalleIndicadorCategoriaDAL.ObtenerDatos(pDetalleIndicadorCategoria).ToList();

                if (pDetalleIndicadorCategoria.Estado && resultado.objetoRespuesta.Count > 0)
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
                PrepararObjetoDetalle(pDetalleIndicadorCategoria);

                if (pDetalleIndicadorCategoria.idIndicador == 0) // ¿ID descencriptado con éxito?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

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

        /// <summary>
        /// 08/11/2022
        /// José Navarro Acuña
        /// Función que valida el objeto DetalleIndicadorCategoria. Valida la existencia del indicador relacionado y la cantidad de detalles restantes del mismo
        /// </summary>
        /// <param name="pDetalleIndicadorCategoria"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<DetalleIndicadorCategoria>> ValidarDatos(DetalleIndicadorCategoria pDetalleIndicadorCategoria)
        {
            RespuestaConsulta<List<DetalleIndicadorCategoria>> resultado = new RespuestaConsulta<List<DetalleIndicadorCategoria>>
            {
                HayError = (int)Error.NoError,
                objetoRespuesta = new List<DetalleIndicadorCategoria>()
            };
            bool errorControlado = false;

            try
            {
                // validar si el indicador existe
                Indicador indicadorExistente = indicadorFonatelDAL.VerificarExistenciaIndicadorPorID(pDetalleIndicadorCategoria.idIndicador);

                if (indicadorExistente == null)
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                // validar que el indicador tenga sus datos completos
                string msgIndicadorCompleto = IndicadorFonatelBL.VerificarDatosCompletosIndicador(indicadorExistente);

                if (!string.IsNullOrEmpty(msgIndicadorCompleto))
                {
                    errorControlado = true;
                    throw new Exception(msgIndicadorCompleto);
                }

                // validar la existencia del indicador
                CategoriasDesagregacion categoriaExistente = categoriasDesagregacionDAL.ObtenerDatos(new CategoriasDesagregacion()
                {
                    idCategoria = pDetalleIndicadorCategoria.idCategoria
                }).FirstOrDefault();

                if (categoriaExistente == null)
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                // validar la cantidad de detalles registrados actualmente
                List<DetalleIndicadorCategoria> detallesActuales = detalleIndicadorCategoriaDAL.ObtenerDatos(new DetalleIndicadorCategoria()
                    {
                        idIndicador = pDetalleIndicadorCategoria.idIndicador,
                        DetallesAgrupados = true, // contabilizar por categoria no por detalles (que pueden ser n)
                        Estado = true
                    }
                ).ToList();

                bool modoEdicion = detallesActuales.Exists(x => x.idCategoriaString.Equals(pDetalleIndicadorCategoria.id));

                if (!modoEdicion) // solo en modo creación
                {
                    // se realiza la validación de la cantidad de detalles establecidos
                    if (detallesActuales.Count + 1 > indicadorExistente.CantidadCategoriasDesagregacion) // se supera la cantidad establecida en el indicador?
                    {
                        errorControlado = true;
                        throw new Exception(Errores.CantidadRegistros);
                    }

                    // validar si la categoria ya se encuentra registrada
                    bool categoriaRegistrada = detallesActuales.Exists(x => x.idCategoriaString.Equals(categoriaExistente.id));

                    if (categoriaRegistrada)
                    {
                        errorControlado = true;
                        throw new Exception(Errores.NoRegistrosActualizar);
                    }
                }

                if (categoriaExistente.idTipoDetalle == (int)TipoDetalleCategoriaEnum.Numerico ||
                    categoriaExistente.idTipoDetalle == (int)TipoDetalleCategoriaEnum.Texto)
                {
                    // es obligatorio insertar detalles en detalles tipos numericos y texto
                    if (pDetalleIndicadorCategoria.listaDetallesCategoriaString.Count < 1)
                    {
                        errorControlado = true;
                        throw new Exception(Errores.NoRegistrosActualizar);
                    }

                    // validar que los detalles ingresados existan y esten disponibles
                    List<DetalleCategoriaTexto> detallesDisponibles = detalleCategoriaTextoDAL.ObtenerDatos(new DetalleCategoriaTexto()
                    {
                        idCategoria = pDetalleIndicadorCategoria.idCategoria,
                        Estado = true
                    });

                    for (int i = 0; i < pDetalleIndicadorCategoria.listaDetallesCategoriaString.Count; i++)
                    {
                        if (!detallesDisponibles.Exists(x => x.id.Equals(pDetalleIndicadorCategoria.listaDetallesCategoriaString[i])))
                        {
                            errorControlado = true;
                            throw new Exception(Errores.NoRegistrosActualizar);
                        }
                    }
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
            return resultado;
        }

        /// <summary>
        /// 12/09/2022
        /// José Navarro Acuña
        /// Prepara un objeto detalle categoria para ser enviado al servicio DAL.
        /// Se preparan los id's de las tablas relacionadas para poder efectuar consultas debido a la encriptación.
        /// </summary>
        /// <param name="pDetalleIndicadorVariables"></param>
        private void PrepararObjetoDetalle(DetalleIndicadorCategoria pDetalleIndicadorCategoria)
        {
            if (!string.IsNullOrEmpty(pDetalleIndicadorCategoria.id))
            {
                int.TryParse(Utilidades.Desencriptar(pDetalleIndicadorCategoria.id), out int number);
                pDetalleIndicadorCategoria.idDetalleIndicador = number;
            }

            if (!string.IsNullOrEmpty(pDetalleIndicadorCategoria.idIndicadorString))
            {
                int.TryParse(Utilidades.Desencriptar(pDetalleIndicadorCategoria.idIndicadorString), out int number);
                pDetalleIndicadorCategoria.idIndicador = number;
            }

            if (!string.IsNullOrEmpty(pDetalleIndicadorCategoria.idCategoriaString))
            {
                int.TryParse(Utilidades.Desencriptar(pDetalleIndicadorCategoria.idCategoriaString), out int number);
                pDetalleIndicadorCategoria.idCategoria = number;
            }

            for (int i = 0; i < pDetalleIndicadorCategoria.listaDetallesCategoriaString.Count; i++)
            {
                int.TryParse(Utilidades.Desencriptar(pDetalleIndicadorCategoria.listaDetallesCategoriaString[i]), out int number);
                pDetalleIndicadorCategoria.listaDetallesCategoria.Add(number);
            }
        }
    }
}
