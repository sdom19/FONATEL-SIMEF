using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using Newtonsoft.Json;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL
{
    public class CategoriasDesagregacionBL : IMetodos<CategoriaDesagregacion>
    {
        private readonly CategoriasDesagregacionDAL clsDatos;
        private readonly DetalleCategoriaTextoDAL clsDatosTexto;

        private RespuestaConsulta<List<CategoriaDesagregacion>> ResultadoConsulta;
        string modulo = string.Empty;
        string user = string.Empty;


        public CategoriasDesagregacionBL(string modulo, string user)
        {
            this.modulo = modulo;
            this.user = user;
            this.clsDatos = new CategoriasDesagregacionDAL();
            this.clsDatosTexto = new DetalleCategoriaTextoDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<CategoriaDesagregacion>>();
        }

        public RespuestaConsulta<List<CategoriaDesagregacion>> ObtenerDatos(CategoriaDesagregacion objeto)
        {
            try
            {
                if (!String.IsNullOrEmpty(objeto.id))
                {
                    objeto.id = Utilidades.Desencriptar(objeto.id);
                    int temp;
                    if (int.TryParse(objeto.id, out temp))
                    {
                        objeto.idCategoriaDesagregacion = temp;
                    }
                }
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;
                var resul = clsDatos.ObtenerDatos(objeto);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();

            }
            catch (Exception ex)
            {
                ResultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<CategoriaDesagregacion>> ValidarDatos(CategoriaDesagregacion objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<string>> ValidarExistencia(CategoriaDesagregacion objeto)
        {
            RespuestaConsulta<List<string>> listaExistencias = new RespuestaConsulta<List<string>>();
            try
            {
                if (!String.IsNullOrEmpty(objeto.id))
                {
                    objeto.id = Utilidades.Desencriptar(objeto.id);
                    int temp;
                    if (int.TryParse(objeto.id, out temp))
                    {
                        objeto.idCategoriaDesagregacion = temp;
                    }
                }
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;
                var resul = clsDatos.ObtenerDatos(objeto).Single();
                listaExistencias.objetoRespuesta = clsDatos.ValidarCategoria(resul);

            }
            catch (Exception ex)
            {
                listaExistencias.HayError = (int)Constantes.Error.ErrorSistema;
                listaExistencias.MensajeError = ex.Message;
            }
            return listaExistencias;
        }

        public RespuestaConsulta<List<CategoriaDesagregacion>> ActualizarElemento(CategoriaDesagregacion objeto)
        {
            try
            {
                List<CategoriaDesagregacion> listadoCategorias = clsDatos.ObtenerDatos(new CategoriaDesagregacion());
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Editar;
                ResultadoConsulta.Usuario = user;
                objeto.UsuarioModificacion = user;
                objeto.IdTipoDetalleCategoria = objeto.IdTipoCategoria == (int)Constantes.TipoCategoriaEnum.VariableDato ? (int)TipoDetalleCategoriaEnum.Numerico : objeto.IdTipoDetalleCategoria;

                if (!string.IsNullOrEmpty(objeto.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.id), out temp);
                    objeto.idCategoriaDesagregacion = temp;
                }
                List<CategoriaDesagregacion> buscarRegistro = clsDatos.ObtenerDatos(new CategoriaDesagregacion());
                
                if (buscarRegistro.Where(x => x.Codigo.ToUpper().Equals(objeto.Codigo.ToUpper()) && x.idCategoriaDesagregacion != objeto.idCategoriaDesagregacion).ToList().Count() > 0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.CodigoRegistrado);
                }
                var result = listadoCategorias.Where(x => x.idCategoriaDesagregacion == objeto.idCategoriaDesagregacion).Single();
                if (listadoCategorias.Where(x => x.idCategoriaDesagregacion == objeto.idCategoriaDesagregacion).Count() == 0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }
                else if (result.DetalleCategoriaTexto.Count() > objeto.CantidadDetalleDesagregacion)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.CantidadRegistrosLimiteCategoria);
                }
                else if (!Utilidades.rx_alfanumerico.Match(objeto.NombreCategoria.Trim()).Success)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(string.Format(Errores.CampoConFormatoInvalido, "nombre de Categoría"));
                }
                else if (!Utilidades.rx_alfanumerico.Match(objeto.Codigo.Trim()).Success)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(string.Format(Errores.CampoConFormatoInvalido, "código de Categoría"));
                }
                else
                {
                    if (objeto.IdTipoDetalleCategoria == (int)TipoDetalleCategoriaEnum.Fecha)
                    {
                        objeto.idEstadoRegistro = objeto.EsParcial == true ? (int)Constantes.EstadosRegistro.EnProceso : (int)Constantes.EstadosRegistro.Activo;
                        if (objeto.DetalleCategoriaFecha.FechaMinima >= objeto.DetalleCategoriaFecha.FechaMaxima)
                        {
                            ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                            throw new Exception(Errores.ValorFecha);
                        }
                        clsDatos.ActualizarDatos(objeto);
                        objeto.DetalleCategoriaFecha.idCategoriaDesagregacion = result.idCategoriaDesagregacion;
                        objeto.DetalleCategoriaFecha.Estado = true;
                        clsDatos.InsertarDetalleFecha(objeto.DetalleCategoriaFecha);
                    }
                    else if (objeto.IdTipoDetalleCategoria == (int)TipoDetalleCategoriaEnum.Numerico)
                    {
                        objeto.idEstadoRegistro = objeto.EsParcial == true ? (int)Constantes.EstadosRegistro.EnProceso : (int)Constantes.EstadosRegistro.Activo;
                        if (objeto.DetalleCategoriaNumerico.Minimo >= objeto.DetalleCategoriaNumerico.Maximo)
                        {
                            ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                            throw new Exception(Errores.ValorMinimo);
                        }
                        objeto.idEstadoRegistro = objeto.IdTipoCategoria == (int)Constantes.TipoCategoriaEnum.VariableDato ? (int)Constantes.EstadosRegistro.Activo : objeto.idEstadoRegistro;
                        clsDatos.ActualizarDatos(objeto);
                        objeto.DetalleCategoriaNumerico.idCategoriaDesagregacion = result.idCategoriaDesagregacion;
                        objeto.DetalleCategoriaNumerico.Estado = true;
                        clsDatos.InsertarDetalleNumerico(objeto.DetalleCategoriaNumerico);
                    }
                    else
                    {
                        if (objeto.CantidadDetalleDesagregacion == 0 && (objeto.IdTipoDetalleCategoria == (int)Constantes.TipoDetalleCategoriaEnum.Alfanumerico || objeto.IdTipoDetalleCategoria == (int)Constantes.TipoDetalleCategoriaEnum.Texto))
                        {
                            objeto.idEstadoRegistro = (int)Constantes.EstadosRegistro.Activo;
                        }
                        else if (objeto.CantidadDetalleDesagregacion > result.DetalleCategoriaTexto.Count())
                        {
                            objeto.idEstadoRegistro = (int)Constantes.EstadosRegistro.EnProceso;
                        }
                        else
                        {
                            objeto.idEstadoRegistro = objeto.EsParcial == true ? (int)Constantes.EstadosRegistro.EnProceso : result.idEstadoRegistro;
                        }
                        clsDatos.ActualizarDatos(objeto);
                    }
                }

                objeto = clsDatos.ObtenerDatos(objeto).Single();
                if (ResultadoConsulta.Accion == (int)Accion.Editar)
                {
                    result.EstadoRegistro.IdEstadoRegistro = objeto.EstadoRegistro.IdEstadoRegistro;
                }
                string JsonActual = objeto.ToString();
                string JsonAnterior = result.ToString();

                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                        ResultadoConsulta.Usuario,
                            ResultadoConsulta.Clase, objeto.Codigo
                            , JsonActual, JsonAnterior, "");
            }
            catch (Exception ex)
            {

                if (ResultadoConsulta.HayError != (int)Error.ErrorControlado)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                }
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<CategoriaDesagregacion>> CambioEstado(CategoriaDesagregacion objeto)
        {
            try
            {
                objeto.UsuarioModificacion = user;
                ResultadoConsulta.Usuario = user;
                ResultadoConsulta.Accion = (int)EstadosRegistro.Activo == objeto.idEstadoRegistro ? (int)Accion.Activar : (int)Accion.Desactivar;
                //if (objeto.CantidadDetalleDesagregacion!=objeto.DetalleCategoriaTexto.Count() && objeto.idEstado == (int)Accion.Activar)
                //{
                //    throw new Exception("No se cumple con la cantidad de atributos configurados");
                //}

                string JsonAnterior = objeto.ToString();
                var resul = clsDatos.ActualizarDatos(objeto);
                objeto = resul.Single();

                string JsonActual = objeto.ToString();
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();
                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                       ResultadoConsulta.Usuario,
                       ResultadoConsulta.Clase, objeto.Codigo, JsonActual, JsonAnterior, "");
            }
            catch (Exception ex)
            {
                ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<CategoriaDesagregacion>> ClonarDatos(CategoriaDesagregacion objeto)
        {
            try
            {
                List<CategoriaDesagregacion> listadoCategorias = clsDatos.ObtenerDatos(new CategoriaDesagregacion());
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Clonar;
                ResultadoConsulta.Usuario = user;
                objeto.UsuarioCreacion = user;

                CategoriaDesagregacion objetoClonar = objeto;
                if (!string.IsNullOrEmpty(objeto.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.id), out temp);
                    objeto.idCategoriaDesagregacion = temp;
                }

                string jsonInicial = listadoCategorias.Where(x => x.idCategoriaDesagregacion == objeto.idCategoriaDesagregacion).Single().ToString();
                objeto = listadoCategorias.Where(x => x.idCategoriaDesagregacion == objeto.idCategoriaDesagregacion).Single();


                if (listadoCategorias.Where(x => x.Codigo.ToUpper() == objetoClonar.Codigo.ToUpper().Trim()).Count() > 0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.CodigoRegistrado);
                }
                else if (objetoClonar.CantidadDetalleDesagregacion < objeto.DetalleCategoriaTexto.Count())
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.CantidadRegistrosLimiteCategoria);
                }
                /*else if (listadoCategorias.Where(x => x.NombreCategoria.ToUpper() == objetoClonar.NombreCategoria.ToUpper()).Count() > 0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.NombreRegistrado);
                }*/
                else if (!Utilidades.rx_alfanumerico.Match(objetoClonar.Codigo.Trim()).Success)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(string.Format(Errores.CampoConFormatoInvalido, "código de Categoría"));
                }
                else
                {
                    objeto.Codigo = objetoClonar.Codigo;
                    objeto.NombreCategoria = objetoClonar.NombreCategoria;
                    objeto.idCategoriaDesagregacion = 0;
                    objeto.CantidadDetalleDesagregacion = objetoClonar.CantidadDetalleDesagregacion;
                    objeto.idEstadoRegistro = objeto.DetalleCategoriaTexto.Count() == objeto.CantidadDetalleDesagregacion ?
                            (int)Constantes.EstadosRegistro.Activo : (int)Constantes.EstadosRegistro.EnProceso;

                    var result = new CategoriaDesagregacion();

                    if (objeto.IdTipoDetalleCategoria == (int)TipoDetalleCategoriaEnum.Fecha)
                    {
                        if (objetoClonar.DetalleCategoriaFecha.FechaMinima >= objetoClonar.DetalleCategoriaFecha.FechaMaxima)
                        {
                            ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                            throw new Exception(Errores.ValorFecha);
                        }
                        result = clsDatos.ActualizarDatos(objeto)
                            .Where(x => x.Codigo.ToUpper() == objeto.Codigo.ToUpper()).FirstOrDefault();

                        objeto.DetalleCategoriaFecha.idCategoriaDesagregacion = result.idCategoriaDesagregacion;
                        objeto.DetalleCategoriaFecha.FechaMaxima = objetoClonar.DetalleCategoriaFecha.FechaMaxima;
                        objeto.DetalleCategoriaFecha.FechaMinima = objetoClonar.DetalleCategoriaFecha.FechaMinima;
                        clsDatos.InsertarDetalleFecha(objeto.DetalleCategoriaFecha);
                    }
                    else if (objeto.IdTipoDetalleCategoria == (int)TipoDetalleCategoriaEnum.Numerico)
                    {
                        if (objetoClonar.DetalleCategoriaNumerico.Minimo >= objetoClonar.DetalleCategoriaNumerico.Maximo)
                        {
                            ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                            throw new Exception(Errores.ValorMinimo);
                        }
                        result = clsDatos.ActualizarDatos(objeto)
                            .Where(x => x.Codigo.ToUpper() == objeto.Codigo.ToUpper()).FirstOrDefault();

                        objeto.DetalleCategoriaNumerico.idCategoriaDesagregacion = result.idCategoriaDesagregacion;
                        objeto.DetalleCategoriaNumerico.Minimo = objetoClonar.DetalleCategoriaNumerico.Minimo;
                        objeto.DetalleCategoriaNumerico.Maximo = objetoClonar.DetalleCategoriaNumerico.Maximo;

                        clsDatos.InsertarDetalleNumerico(objeto.DetalleCategoriaNumerico);
                    }
                    else
                    {
                        result = clsDatos.ActualizarDatos(objeto)
                            .Where(x => x.Codigo.ToUpper() == objeto.Codigo.ToUpper()).FirstOrDefault();

                        foreach (var item in objeto.DetalleCategoriaTexto)
                        {
                            item.idCategoriaDesagregacion = result.idCategoriaDesagregacion;
                            clsDatosTexto.ActualizarDatos(item);
                        }
                    }
                    string JsonNuevoValor = result.ToString();
                    clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                            ResultadoConsulta.Usuario,
                                ResultadoConsulta.Clase, objeto.Codigo, JsonNuevoValor, "", jsonInicial);
                }
            }
            catch (Exception ex)
            {
                if (ResultadoConsulta.HayError != (int)Error.ErrorControlado)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                }
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<CategoriaDesagregacion>> EliminarElemento(CategoriaDesagregacion objeto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<CategoriaDesagregacion>> InsertarDatos(CategoriaDesagregacion objeto)
        {
            try
            {
                objeto.idCategoriaDesagregacion = 0;
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Crear;
                ResultadoConsulta.Usuario = user;
                objeto.UsuarioCreacion = user;
                objeto.IdTipoDetalleCategoria = objeto.IdTipoCategoria == (int)Constantes.TipoCategoriaEnum.VariableDato ? (int)TipoDetalleCategoriaEnum.Numerico : objeto.IdTipoDetalleCategoria;
                List<CategoriaDesagregacion> buscarRegistro = clsDatos.ObtenerDatos(new CategoriaDesagregacion());

                if (buscarRegistro.Where(x => x.Codigo.ToUpper() == objeto.Codigo.ToUpper()).ToList().Count() > 0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.CodigoRegistrado);
                }
                /*else if (buscarRegistro.Where(x => x.NombreCategoria.ToUpper() == objeto.NombreCategoria.ToUpper()).ToList().Count() > 0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.NombreRegistrado);
                }*/
                else if (!Utilidades.rx_alfanumerico.Match(objeto.NombreCategoria.Trim()).Success)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(string.Format(Errores.CampoConFormatoInvalido, "nombre de Categoría"));
                }
                else if (!Utilidades.rx_alfanumerico.Match(objeto.Codigo.Trim()).Success)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(string.Format(Errores.CampoConFormatoInvalido, "código de Categoría"));
                }

                if (objeto.EsParcial || objeto.CantidadDetalleDesagregacion > 0)
                {
                    objeto.idEstadoRegistro = (int)EstadosRegistro.EnProceso;
                }
                else
                {
                    objeto.idEstadoRegistro = (int)EstadosRegistro.Activo;
                }

                if (objeto.IdTipoDetalleCategoria == (int)TipoDetalleCategoriaEnum.Fecha)
                {
                    if (objeto.DetalleCategoriaFecha.FechaMinima >= objeto.DetalleCategoriaFecha.FechaMaxima)
                    {
                        ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                        throw new Exception(Errores.ValorFecha);
                    }

                    objeto.idEstadoRegistro = objeto.EsParcial ? (int)Constantes.EstadosRegistro.EnProceso : (int)Constantes.EstadosRegistro.Activo;
                    var result = clsDatos.ActualizarDatos(objeto)
                        .Where(x => x.Codigo.ToUpper() == objeto.Codigo.ToUpper()).FirstOrDefault();
                    objeto.DetalleCategoriaFecha.idCategoriaDesagregacion = result.idCategoriaDesagregacion;
                    objeto.DetalleCategoriaFecha.Estado = true;
                    if (objeto.DetalleCategoriaFecha.FechaMaxima != null && objeto.DetalleCategoriaFecha.FechaMinima != null)
                    {
                        clsDatos.InsertarDetalleFecha(objeto.DetalleCategoriaFecha);
                    }

                }
                else if (objeto.IdTipoDetalleCategoria == (int)TipoDetalleCategoriaEnum.Numerico)
                {
                    if (objeto.DetalleCategoriaNumerico.Minimo >= objeto.DetalleCategoriaNumerico.Maximo)
                    {
                        ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                        throw new Exception(Errores.ValorMinimo);
                    }

                    var estadoNumerico = objeto.EsParcial ? (int)Constantes.EstadosRegistro.EnProceso : (int)Constantes.EstadosRegistro.Activo;
                    objeto.idEstadoRegistro = objeto.IdTipoCategoria == (int)Constantes.TipoCategoriaEnum.VariableDato ? (int)Constantes.EstadosRegistro.Activo : estadoNumerico;
                    var result = clsDatos.ActualizarDatos(objeto)
                        .Where(x => x.Codigo.ToUpper() == objeto.Codigo.ToUpper()).FirstOrDefault();
                    objeto.DetalleCategoriaNumerico.idCategoriaDesagregacion = result.idCategoriaDesagregacion;
                    objeto.DetalleCategoriaNumerico.Estado = true;
                    clsDatos.InsertarDetalleNumerico(objeto.DetalleCategoriaNumerico);
                }
                else
                {
                    var result = clsDatos.ActualizarDatos(objeto)
                            .Where(x => x.Codigo.ToUpper() == objeto.Codigo.ToUpper()).FirstOrDefault();
                }
                
                objeto = clsDatos.ObtenerDatos(objeto).Single();

                string jsonValorInicial = objeto.ToString();

                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                            ResultadoConsulta.Usuario,
                                ResultadoConsulta.Clase, objeto.Codigo, "", "", jsonValorInicial);
            }
            catch (Exception ex)
            {
                if (ResultadoConsulta.HayError != (int)Error.ErrorControlado)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                }
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<CategoriaDesagregacion>> ListaCategoriasParaRelacion(CategoriaDesagregacion objeto)
        {
            try
            {
                if (!String.IsNullOrEmpty(objeto.id))
                {
                    objeto.id = Utilidades.Desencriptar(objeto.id);
                    int temp;
                    if (int.TryParse(objeto.id, out temp))
                    {
                        objeto.idCategoriaDesagregacion = temp;
                    }
                }
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;
                var resul = clsDatos.ObtenerDatos(objeto).Where(x => x.IdTipoCategoria == 2 && x.CantidadDetalleDesagregacion > 0).ToList();
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();

            }
            catch (Exception ex)
            {
                ResultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        /// <summary>
        /// José Navarro Acuña
        /// 17/11/2022
        /// Consulta las categorias de desagregación relacionadas a una formula respecto al nivel de calculo  
        /// </summary>
        /// <param name="pIdFormula"></param>
        /// <param name="pIdIndicador"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<CategoriaDesagregacion>> ObtenerCategoriasDeFormulaNivelCalculo(string pIdFormula, string pIdIndicador)
        {
            RespuestaConsulta<List<CategoriaDesagregacion>> resultado = new RespuestaConsulta<List<CategoriaDesagregacion>>();

            try
            {
                int.TryParse(Utilidades.Desencriptar(pIdFormula), out int idFormula);
                int.TryParse(Utilidades.Desencriptar(pIdIndicador), out int idIndicador);

                if (idFormula == 0 || idIndicador == 0)
                {
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                List<CategoriaDesagregacion> result = clsDatos.ObtenerCategoriasDeFormulaNivelCalculo(idFormula, idIndicador);
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

        /// <summary>
        /// 18/10/2022
        /// José Navarro Acuña
        /// Obtener las categorias de desagregación asociadas a un indicador
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<CategoriaDesagregacion>> ObtenerCategoriasDesagregacionDeIndicador(string pIdIndicador, bool pInsertarOpcionTodos = false)
        {
            RespuestaConsulta<List<CategoriaDesagregacion>> resultado = new RespuestaConsulta<List<CategoriaDesagregacion>>();

            try
            {
                int idIndicador = 0;
                if (!string.IsNullOrEmpty(pIdIndicador))
                {
                    int.TryParse(Utilidades.Desencriptar(pIdIndicador), out int number);
                    idIndicador = number;
                }

                if (idIndicador == 0)
                {
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                List<CategoriaDesagregacion> result = clsDatos.ObtenerCategoriasDesagregacionDeIndicador(idIndicador);
                resultado.objetoRespuesta = result;
                resultado.CantidadRegistros = result.Count();

                if (resultado.CantidadRegistros > 0 && pInsertarOpcionTodos)
                {
                    resultado.objetoRespuesta.Insert(0, new CategoriaDesagregacion() { id = select2MultipleOptionTodosValue, NombreCategoria = select2MultipleOptionTodosText });
                }
            }
            catch (Exception ex)
            {
                resultado.HayError = (int)Error.ErrorSistema;
                resultado.MensajeError = ex.Message;
            }
            return resultado;
        }

        /// <summary>
        /// 10/01/2022
        /// Jósé Navarro Acuña
        /// Obtener las categorias de desagregación de tipo fecha asociadas a un indicador
        /// </summary>
        /// <param name="pIdIndicador"></param>
        /// <param name="pIdTipoDetalleCategoria"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<CategoriaDesagregacion>> ObtenerCategoriasDesagregacionTipoFechaDeIndicador(string pIdIndicador, string pIdTipoDetalleCategoria)
        {
            RespuestaConsulta<List<CategoriaDesagregacion>> resultado = new RespuestaConsulta<List<CategoriaDesagregacion>>();

            try
            {
                int idIndicador = 0, idTipoDetalleCategoria = 0;

                if (!string.IsNullOrEmpty(pIdIndicador))
                {
                    int.TryParse(Utilidades.Desencriptar(pIdIndicador), out idIndicador);
                }

                if (!string.IsNullOrEmpty(pIdTipoDetalleCategoria))
                {
                    int.TryParse(Utilidades.Desencriptar(pIdTipoDetalleCategoria), out idTipoDetalleCategoria);
                }

                if (idIndicador == 0 || idTipoDetalleCategoria == 0)
                {
                    throw new Exception(Errores.NoRegistrosActualizar);
                }
                
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                List<CategoriaDesagregacion> result = clsDatos.ObtenerCategoriasDesagregacionDeIndicador(idIndicador, idTipoDetalleCategoria);
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

        /// <summary>
        /// Fecha 31-03-2023
        /// Georgi Mesen Cerdas
        /// Metodo para registrar la bitacora cuando se descarga la plantilla de categorias
        /// </summary>
        /// <returns></returns>
        public void BitacoraDescargar(CategoriaDesagregacion objeto)
        {
            RespuestaConsulta<List<CategoriaDesagregacion>> resultado = new RespuestaConsulta<List<CategoriaDesagregacion>>();
            resultado.Clase = modulo;
            resultado.Accion = (int)Accion.Descargar;
            resultado.Usuario = user;
            objeto.id = Utilidades.DesencriptarArray(objeto.id);

            clsDatos.RegistrarBitacora(resultado.Accion,
                        resultado.Usuario,
                            resultado.Clase, objeto.id, "", "", "");
        }
    }
}