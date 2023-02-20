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

        public RespuestaConsulta<List<DetalleIndicadorCategoria>> CambioEstado(DetalleIndicadorCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleIndicadorCategoria>> ClonarDatos(DetalleIndicadorCategoria objeto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 09/11/2022
        /// José Navarro Acuña
        /// Permite realizar un eliminado lógico de los detalles categoria de un indicador
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<DetalleIndicadorCategoria>> EliminarElemento(DetalleIndicadorCategoria pDetalleIndicadorCategoria)
        {
            RespuestaConsulta<List<DetalleIndicadorCategoria>> resultado = new RespuestaConsulta<List<DetalleIndicadorCategoria>>();
            bool errorControlado = false;
            try
            {
                PrepararObjetoDetalle(pDetalleIndicadorCategoria);

                if (pDetalleIndicadorCategoria.idIndicador == 0 || pDetalleIndicadorCategoria.idCategoria == 0)
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                pDetalleIndicadorCategoria.DetallesAgrupados = false; // listar cada detalle individualmente
                List<DetalleIndicadorCategoria> listaDetalle = detalleIndicadorCategoriaDAL.ObtenerDatos(pDetalleIndicadorCategoria);

                resultado.objetoRespuesta = new List<DetalleIndicadorCategoria>();

                foreach (DetalleIndicadorCategoria detalle in listaDetalle)
                {
                    PrepararObjetoDetalle(detalle);
                    detalle.Estado = false;
                    resultado.objetoRespuesta.AddRange(detalleIndicadorCategoriaDAL.ActualizarDatos(detalle));
                }

                // registro en bitacora
                resultado.Usuario = user;
                resultado.CantidadRegistros = resultado.objetoRespuesta.Count;
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Eliminar;

                //detalleIndicadorCategoriaDAL.RegistrarBitacora(resultado.Accion,
                //        resultado.Usuario, resultado.Clase, pDetalleIndicadorCategoria.NombreCategoria);
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

                if (resultado.HayError != 0)
                {
                    return resultado;
                }

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

                // registro en bitacora
                resultado.Usuario = user;
                resultado.CantidadRegistros = resultado.objetoRespuesta.Count;
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Insertar;

                var objeto = detalleIndicadorCategoriaDAL.ObtenerDatos(pDetalleIndicadorCategoria).FirstOrDefault();

                string jsonValorInicial = objeto.ToString();

                detalleIndicadorCategoriaDAL.RegistrarBitacora(resultado.Accion,
                            resultado.Usuario,
                                resultado.Clase, objeto.Codigo, "", "", jsonValorInicial);
                //detalleIndicadorCategoriaDAL.RegistrarBitacora(resultado.Accion,
                //        resultado.Usuario, resultado.Clase, pDetalleIndicadorCategoria.NombreCategoria);
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
        /// 10/11/2022
        /// José Navarro Acuña
        /// Función que permite editar los detalles de categoria de un indicador
        /// </summary>
        /// <param name="pDetalleIndicadorCategoria"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<DetalleIndicadorCategoria>> ActualizarElemento(DetalleIndicadorCategoria pDetalleIndicadorCategoria)
        {
            RespuestaConsulta<List<DetalleIndicadorCategoria>> resultado = new RespuestaConsulta<List<DetalleIndicadorCategoria>>();

            bool errorControlado = false;
            try
            {
                PrepararObjetoDetalle(pDetalleIndicadorCategoria);
                resultado = ValidarDatos(pDetalleIndicadorCategoria);

                if (resultado.HayError != 0)
                {
                    return resultado;
                }

                List<DetalleIndicadorCategoria> detallesActuales = detalleIndicadorCategoriaDAL.ObtenerDatos(new DetalleIndicadorCategoria()
                {
                    idIndicador = pDetalleIndicadorCategoria.idIndicador,
                    idCategoria = pDetalleIndicadorCategoria.idCategoria,
                    DetallesAgrupados = false // contabilizar por detalle, no por categoria
                }).ToList();

                List<DetalleIndicadorCategoria> detallesAnterioresBitacora = new List<DetalleIndicadorCategoria>(detallesActuales);

                if (pDetalleIndicadorCategoria.listaDetallesCategoriaString.Count > 0) // evitar ejecuciones cuando el tipo no es texto o numerico
                {
                    for (int i = 0; i < pDetalleIndicadorCategoria.listaDetallesCategoriaString.Count; i++)
                    {
                        int indexDetalleRegistrado = detallesActuales.FindIndex(x => x.idCategoriaDetalleString == pDetalleIndicadorCategoria.listaDetallesCategoriaString[i]);

                        if (indexDetalleRegistrado == -1) // no se encontró en los detalles registrados, por tanto, se registra el nuevo detalle o actualiza
                        {
                            List<DetalleIndicadorCategoria> result = detalleIndicadorCategoriaDAL.ActualizarDatos(
                            new DetalleIndicadorCategoria()
                            {
                                idIndicador = pDetalleIndicadorCategoria.idIndicador,
                                idCategoria = pDetalleIndicadorCategoria.idCategoria,
                                idCategoriaDetalle = pDetalleIndicadorCategoria.listaDetallesCategoria[i],
                                Estado = true
                            });

                            resultado.objetoRespuesta.AddRange(result);
                        }
                        else // el detalle ya esta registrado
                        {
                            detallesActuales.RemoveAt(indexDetalleRegistrado);
                        }
                    }

                    for (int i = 0; i < detallesActuales.Count; i++) // eliminar los detalles no seleccionados
                    {
                        PrepararObjetoDetalle(detallesActuales[i]);
                        detallesActuales[i].Estado = false;
                        detalleIndicadorCategoriaDAL.ActualizarDatos(detallesActuales[i]);
                    }

                    // registro en bitacora
                    resultado.Usuario = user;
                    resultado.CantidadRegistros = resultado.objetoRespuesta.Count;
                    resultado.Clase = modulo;
                    resultado.Accion = (int)Accion.Insertar;

                    var objeto = detalleIndicadorCategoriaDAL.ObtenerDatos(pDetalleIndicadorCategoria);

                    
                    string JsonAnterior = "";
                    string JsonNuevo = "";
                    if (objeto.Count() > detallesAnterioresBitacora.Count())
                    {
                        for(int i = detallesAnterioresBitacora.Count(); i < objeto.Count(); i++)
                        {
                            var objetoNuevo = objeto[i];
                            JsonAnterior = objetoNuevo.ToString();
                            detalleIndicadorCategoriaDAL.RegistrarBitacora(resultado.Accion,
                                                        resultado.Usuario,
                                                            resultado.Clase, objetoNuevo.Codigo
                                                            ,"" ,"", JsonAnterior);
                        }
                        

                    }
                    else if (objeto.Count() < detallesAnterioresBitacora.Count())
                    {
                        resultado.Accion = (int)Accion.Eliminar;
                        foreach(DetalleIndicadorCategoria item in detallesActuales)
                        {
                            JsonAnterior = item.ToString();
                            detalleIndicadorCategoriaDAL.RegistrarBitacora(resultado.Accion,
                                                        resultado.Usuario,
                                                            resultado.Clase, item.Codigo
                                                            , "", "", JsonAnterior);
                        }

                    }
                    else
                    {
                        resultado.Accion = (int)Accion.Editar;
                        for (int i = 0; i < objeto.Count(); i++)
                        {
                            if (objeto[i].Etiquetas != detallesAnterioresBitacora[i].Etiquetas)
                            {
                                var objetoNuevo = objeto[i];
                                JsonAnterior = objetoNuevo.ToString();
                                JsonNuevo = detallesAnterioresBitacora[i].ToString();
                                detalleIndicadorCategoriaDAL.RegistrarBitacora(resultado.Accion,
                                                            resultado.Usuario,
                                                                resultado.Clase, objetoNuevo.Codigo
                                                                , JsonAnterior, JsonNuevo, "");
                            }
                        }
                    }

                    
                    //detalleIndicadorCategoriaDAL.RegistrarBitacora(resultado.Accion,
                    //        resultado.Usuario, resultado.Clase, pDetalleIndicadorCategoria.NombreCategoria);
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
        /// 20/02/2023
        /// José Navarro Acuña
        /// Función que permiter obtener los detalles de categorias de un indicador y categoria, pero sin contemplar el campo 'No definido'
        /// </summary>
        /// <param name="pDetalleIndicadorCategoria"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<DetalleIndicadorCategoria>> ObtenerDetallesDeCategoria(DetalleIndicadorCategoria pDetalleIndicadorCategoria)
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

                resultado.objetoRespuesta = detalleIndicadorCategoriaDAL.ObtenerDetallesDeCategoria(pDetalleIndicadorCategoria).ToList();
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
            string mensajeValidacion = null;

            try
            {
                // validar si el indicador existe
                Indicador indicadorExistente = indicadorFonatelDAL.VerificarExistenciaIndicadorPorID(pDetalleIndicadorCategoria.idIndicador);

                if (indicadorExistente == null)
                {
                    errorControlado = true; throw new Exception(Errores.NoRegistrosActualizar);
                }

                // validar que el indicador tenga sus datos completos
                string msgIndicadorCompleto = IndicadorFonatelBL.VerificarDatosCompletosIndicador(indicadorExistente);

                if (!string.IsNullOrEmpty(msgIndicadorCompleto))
                {
                    errorControlado = true; throw new Exception(msgIndicadorCompleto);
                }

                // validar la existencia de la categoria
                CategoriasDesagregacion categoriaExistente = categoriasDesagregacionDAL.ObtenerDatos(new CategoriasDesagregacion()
                {
                    idCategoria = pDetalleIndicadorCategoria.idCategoria
                }).FirstOrDefault();

                if (categoriaExistente == null)
                {
                    errorControlado = true; throw new Exception(Errores.NoRegistrosActualizar);
                }

                // validar la cantidad de detalles registrados actualmente
                List<DetalleIndicadorCategoria> detallesActuales = detalleIndicadorCategoriaDAL.ObtenerDatos(new DetalleIndicadorCategoria()
                {
                    idIndicador = pDetalleIndicadorCategoria.idIndicador,
                    DetallesAgrupados = true // contabilizar por categoria no por detalles (que pueden ser n)
                }).ToList();

                bool modoEdicion = detallesActuales.Exists(x => x.idCategoriaString.Equals(pDetalleIndicadorCategoria.id));

                if (!modoEdicion) // solo en modo creación
                {
                    mensajeValidacion = ValidacionesModoCreacionDetalleCategoria(indicadorExistente, categoriaExistente, detallesActuales);

                    if (mensajeValidacion != null)
                    {
                        errorControlado = true; throw new Exception(mensajeValidacion);
                    }
                }
                else // modo edición
                {
                    mensajeValidacion = ValidacionesModoEdicionDetalleCategoria(categoriaExistente, detallesActuales);

                    if (mensajeValidacion != null)
                    {
                        errorControlado = true; throw new Exception(mensajeValidacion);
                    }
                }

                if (categoriaExistente.idTipoDetalle == (int)TipoDetalleCategoriaEnum.Texto)
                {
                    mensajeValidacion = ValidacionDetalleTipoCategoriaTextoNumerico(pDetalleIndicadorCategoria);

                    if (mensajeValidacion != null)
                    {
                        errorControlado = true; throw new Exception(mensajeValidacion);
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

        #region Métodos privados

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

            if (!string.IsNullOrEmpty(pDetalleIndicadorCategoria.idCategoriaDetalleString))
            {
                int.TryParse(Utilidades.Desencriptar(pDetalleIndicadorCategoria.idCategoriaDetalleString), out int number);
                pDetalleIndicadorCategoria.idCategoriaDetalle = number;
            }

            if (pDetalleIndicadorCategoria.listaDetallesCategoriaString.Count > 0)
            {
                for (int i = 0; i < pDetalleIndicadorCategoria.listaDetallesCategoriaString.Count; i++)
                {
                    int.TryParse(Utilidades.Desencriptar(pDetalleIndicadorCategoria.listaDetallesCategoriaString[i]), out int number);
                    pDetalleIndicadorCategoria.listaDetallesCategoria.Add(number);
                }
            }
            else
            {
                pDetalleIndicadorCategoria.listaDetallesCategoria.Add(0);
            }
        }

        /// <summary>
        /// 10/11/2022
        /// José Navarro Acuña
        /// Validar que los detalles hayan sido ingresados y esten registrados en el sistema
        /// </summary>
        /// <param name="pDetalleIndicadorCategoria"></param>
        /// <returns></returns>
        private string ValidacionDetalleTipoCategoriaTextoNumerico(DetalleIndicadorCategoria pDetalleIndicadorCategoria)
        {
            // es obligatorio insertar detalles en detalles tipos numericos y texto
            if (pDetalleIndicadorCategoria.listaDetallesCategoriaString.Count < 1)
            {
                return Errores.CamposIncompletos;
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
                    return Errores.NoRegistrosActualizar;
                }
            }
            return null;
        }

        /// <summary>
        /// 10/11/2022
        /// José Navarro Acuña
        /// Validaciones al momento crear un detalle categoria. 
        /// Se verifica si la categoria ya fue registrada y si existen cupos disponibles para crear el detalle segun el indicador
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <param name="pCategoriaExistente"></param>
        /// <param name="pDetallesRegistradoActualmente"></param>
        /// <returns></returns>
        private string ValidacionesModoCreacionDetalleCategoria(Indicador pIndicador, CategoriasDesagregacion pCategoriaExistente, List<DetalleIndicadorCategoria> pDetallesRegistradoActualmente)
        {
            // validar si la categoria ya se encuentra registrada
            bool categoriaRegistrada = pDetallesRegistradoActualmente.Exists(x => x.idCategoriaString.Equals(pCategoriaExistente.id));

            if (categoriaRegistrada)
            {
                return Errores.CategoriaYaRegistrada;
            }

            // se realiza la validación de la cantidad de detalles establecidos
            if (pDetallesRegistradoActualmente.Count + 1 > pIndicador.CantidadCategoriasDesagregacion) // se supera la cantidad establecida en el indicador?
            {
                return Errores.CantidadRegistros;
            }
            return null;
        }

        /// <summary>
        /// 10/11/2022
        /// José Navarro Acuña
        /// Validaciones al momento editar un detalle categoria. 
        /// Se verifica que la categoria esté registrada
        /// </summary>
        /// <param name="pCategoriaExistente"></param>
        /// <param name="pDetallesRegistradoActualmente"></param>
        /// <returns></returns>
        private string ValidacionesModoEdicionDetalleCategoria(CategoriasDesagregacion pCategoriaExistente, List<DetalleIndicadorCategoria> pDetallesRegistradoActualmente)
        {
            // la categoria debe estar registrada en el detalle, no se permiten seleccionar otras categorias aparte de las ya registradas
            bool categoriaRegistrada = pDetallesRegistradoActualmente.Exists(x => x.idCategoriaString.Equals(pCategoriaExistente.id));

            if (!categoriaRegistrada)
            {
                return Errores.NoRegistrosActualizar;
            }
            return null;
        }

        #endregion
    }
}
