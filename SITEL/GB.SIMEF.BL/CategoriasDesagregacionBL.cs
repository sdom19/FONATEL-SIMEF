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
    public class CategoriasDesagregacionBL:IMetodos<CategoriasDesagregacion>
    {
        private readonly CategoriasDesagregacionDAL clsDatos;
        private readonly DetalleCategoriaTextoDAL clsDatosTexto;


        private RespuestaConsulta<List<CategoriasDesagregacion>> ResultadoConsulta;
        string modulo = string.Empty;
        string user = string.Empty;



        

        public CategoriasDesagregacionBL(string modulo, string user)
        {
            this.modulo = modulo;
            this.user = user;
            this.clsDatos = new CategoriasDesagregacionDAL();
            this.clsDatosTexto = new DetalleCategoriaTextoDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<CategoriasDesagregacion>>();
        }






        private string SerializarObjetoBitacora(CategoriasDesagregacion objCategoria)
        {
          return  JsonConvert.SerializeObject(objCategoria, new JsonSerializerSettings 
          { ContractResolver = new JsonIgnoreResolver(objCategoria.NoSerialize) });
        }


        public RespuestaConsulta<List<CategoriasDesagregacion>> ObtenerDatos(CategoriasDesagregacion objeto)
        {
            try
            {
                if (!String.IsNullOrEmpty(objeto.id))
                {
                    objeto.id = Utilidades.Desencriptar(objeto.id);
                    int temp;
                    if (int.TryParse(objeto.id, out temp))
                    {
                        objeto.idCategoria = temp;
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

        public RespuestaConsulta<List<CategoriasDesagregacion>> ValidarDatos(CategoriasDesagregacion objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<string>> ValidarExistencia(CategoriasDesagregacion objeto)
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
                        objeto.idCategoria = temp;
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

        public RespuestaConsulta<List<CategoriasDesagregacion>> ActualizarElemento(CategoriasDesagregacion objeto)
        {
            try
            {
                List<CategoriasDesagregacion> listadoCategorias = clsDatos.ObtenerDatos(new CategoriasDesagregacion());
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Editar;
                ResultadoConsulta.Usuario = user;
                objeto.UsuarioModificacion = user;
                
                if (!string.IsNullOrEmpty(objeto.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.id), out temp);
                    objeto.idCategoria = temp;
                }
                var result = listadoCategorias.Where(x => x.idCategoria == objeto.idCategoria).Single();
                if (listadoCategorias.Where(x => x.idCategoria == objeto.idCategoria).Count()==0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }
                else if (result.DetalleCategoriaTexto.Count()>objeto.CantidadDetalleDesagregacion)
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
                   

                    if (objeto.idTipoDetalle == (int)TipoDetalleCategoriaEnum.Fecha)
                    {
                        objeto.idEstado = objeto.EsParcial == true ? (int)Constantes.EstadosRegistro.EnProceso : (int)Constantes.EstadosRegistro.Activo;
                        if (objeto.DetalleCategoriaFecha.FechaMinima >= objeto.DetalleCategoriaFecha.FechaMaxima && !objeto.EsParcial)
                        {
                            throw new Exception(Errores.ValorFecha);
                        }
                        clsDatos.ActualizarDatos(objeto);
                        objeto.DetalleCategoriaFecha.idCategoria = result.idCategoria;
                        objeto.DetalleCategoriaFecha.Estado = true;
                        clsDatos.InsertarDetalleFecha(objeto.DetalleCategoriaFecha);
                    }
                    else if (objeto.idTipoDetalle == (int)TipoDetalleCategoriaEnum.Numerico)
                    {
                        objeto.idEstado = objeto.EsParcial == true ? (int)Constantes.EstadosRegistro.EnProceso : (int)Constantes.EstadosRegistro.Activo;
                        if (objeto.DetalleCategoriaNumerico.Minimo >= objeto.DetalleCategoriaNumerico.Maximo && !objeto.EsParcial)
                        {
                            throw new Exception(Errores.ValorMinimo);
                        }
                        clsDatos.ActualizarDatos(objeto);
                        objeto.DetalleCategoriaNumerico.idCategoria = result.idCategoria;
                        objeto.DetalleCategoriaNumerico.Estado = true;
                        clsDatos.InsertarDetalleNumerico(objeto.DetalleCategoriaNumerico);
                    }
                    else
                    {

                        if (objeto.CantidadDetalleDesagregacion == 0 && (objeto.idTipoDetalle == (int)Constantes.TipoDetalleCategoriaEnum.Alfanumerico || objeto.idTipoDetalle == (int)Constantes.TipoDetalleCategoriaEnum.Texto))
                        {
                            objeto.idEstado = (int)Constantes.EstadosRegistro.Activo;
                        }
                        else if (objeto.CantidadDetalleDesagregacion > result.DetalleCategoriaTexto.Count())
                        {
                            objeto.idEstado = (int)Constantes.EstadosRegistro.EnProceso;
                        }
                        else
                        {
                            objeto.idEstado = objeto.EsParcial == true ? (int)Constantes.EstadosRegistro.EnProceso : result.idEstado;
                        }
                        clsDatos.ActualizarDatos(objeto);
                    }
                }
              
                objeto = clsDatos.ObtenerDatos(objeto).Single();
                string JsonActual = SerializarObjetoBitacora(objeto);
                string JsonAnterior= SerializarObjetoBitacora(result);

                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                        ResultadoConsulta.Usuario,
                            ResultadoConsulta.Clase, objeto.Codigo
                            ,JsonActual,JsonAnterior,"");
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

        public RespuestaConsulta<List<CategoriasDesagregacion>> CambioEstado(CategoriasDesagregacion objeto)
        {
            try
            {
                objeto.UsuarioModificacion =user;
                ResultadoConsulta.Usuario = user;
                ResultadoConsulta.Accion= (int)EstadosRegistro.Activo == objeto.idEstado ? (int)Accion.Activar : (int)Accion.Inactiva;
                //if (objeto.CantidadDetalleDesagregacion!=objeto.DetalleCategoriaTexto.Count() && objeto.idEstado == (int)Accion.Activar)
                //{
                //    throw new Exception("No se cumple con la cantidad de atributos configurados");
                //}

                var resul = clsDatos.ActualizarDatos(objeto);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();
                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                       ResultadoConsulta.Usuario,
                       ResultadoConsulta.Clase,objeto.Codigo,JsonConvert.SerializeObject(objeto),"","");

            }
            catch (Exception ex)
            { 
                ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }



        public RespuestaConsulta<List<CategoriasDesagregacion>> ClonarDatos(CategoriasDesagregacion objeto)
        {
            try
            {
                List<CategoriasDesagregacion> listadoCategorias = clsDatos.ObtenerDatos(new CategoriasDesagregacion());        
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Clonar;
                ResultadoConsulta.Usuario = user;
                objeto.UsuarioCreacion = user;

                CategoriasDesagregacion objetoClonar = objeto;           
                if (!string.IsNullOrEmpty( objeto.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.id), out temp );
                    objeto.idCategoria = temp;
                }

                string jsonInicial = SerializarObjetoBitacora(listadoCategorias.Where(x => x.idCategoria == objeto.idCategoria).Single());
                objeto = listadoCategorias.Where(x => x.idCategoria == objeto.idCategoria).Single();
              

                if (listadoCategorias.Where(x=>x.Codigo.ToUpper()== objetoClonar.Codigo.ToUpper().Trim()).Count()>0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.CodigoRegistrado);
                }
                else if (objetoClonar.CantidadDetalleDesagregacion<objeto.DetalleCategoriaTexto.Count() )
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.CantidadDestinatariosIncorrecta);
                }
                else if(listadoCategorias.Where(x => x.NombreCategoria.ToUpper() == objetoClonar.NombreCategoria.ToUpper()).Count() > 0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.NombreRegistrado);
                }
                else if (!Utilidades.rx_soloTexto.Match(objetoClonar.NombreCategoria.Trim()).Success)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(string.Format(Errores.CampoConFormatoInvalido,"nombre de Categoría"));
                }
                else if (!Utilidades.rx_alfanumerico.Match(objetoClonar.Codigo.Trim()).Success)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(string.Format(Errores.CampoConFormatoInvalido, "código de Categoría"));
                }
                else
                {
                    objeto.Codigo = objetoClonar.Codigo;
                    objeto.NombreCategoria = objetoClonar.NombreCategoria;
                    objeto.idCategoria = 0;
                    objeto.CantidadDetalleDesagregacion = objetoClonar.CantidadDetalleDesagregacion;
                    objeto.idEstado = objeto.DetalleCategoriaTexto.Count() == objeto.CantidadDetalleDesagregacion ? 
                            (int)Constantes.EstadosRegistro.Activo : (int)Constantes.EstadosRegistro.EnProceso;

                    var result = clsDatos.ActualizarDatos(objeto)
                      .Where(x => x.Codigo.ToUpper() == objeto.Codigo.ToUpper()).FirstOrDefault();

                    if (objeto.idTipoDetalle == (int)TipoDetalleCategoriaEnum.Fecha)
                    {
                       
                        objeto.DetalleCategoriaFecha.idCategoria = result.idCategoria;
                        objeto.DetalleCategoriaFecha.FechaMaxima = objetoClonar.DetalleCategoriaFecha.FechaMaxima;
                        objeto.DetalleCategoriaFecha.FechaMinima = objetoClonar.DetalleCategoriaFecha.FechaMinima;
                        clsDatos.InsertarDetalleFecha(objeto.DetalleCategoriaFecha);
                    }
                    else if (objeto.idTipoDetalle == (int)TipoDetalleCategoriaEnum.Numerico)
                    {
                        objeto.DetalleCategoriaNumerico.idCategoria = result.idCategoria;
                        objeto.DetalleCategoriaNumerico.Minimo = objetoClonar.DetalleCategoriaNumerico.Minimo;
                        objeto.DetalleCategoriaNumerico.Maximo = objetoClonar.DetalleCategoriaNumerico.Maximo;

                        clsDatos.InsertarDetalleNumerico(objeto.DetalleCategoriaNumerico);
                    }
                    else
                    {
                        foreach (var item in objeto.DetalleCategoriaTexto)
                        {
                            item.idCategoria = result.idCategoria;
                            clsDatosTexto.ActualizarDatos(item);
                        }
                    }
                    string JsonNuevoValor = SerializarObjetoBitacora(result);
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

        public RespuestaConsulta<List<CategoriasDesagregacion>> EliminarElemento(CategoriasDesagregacion objeto)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<CategoriasDesagregacion>> InsertarDatos(CategoriasDesagregacion objeto)
        {
            try
            {
                objeto.idCategoria = 0;
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Insertar;
                ResultadoConsulta.Usuario =user ;
                objeto.UsuarioCreacion = user;
                List<CategoriasDesagregacion> buscarRegistro = clsDatos.ObtenerDatos(new CategoriasDesagregacion());
                if (buscarRegistro.Where(x => x.Codigo.ToUpper() == objeto.Codigo.ToUpper()).ToList().Count() > 0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.CodigoRegistrado);
                }
                else if (buscarRegistro.Where(x => x.NombreCategoria.ToUpper() == objeto.NombreCategoria.ToUpper()).ToList().Count() > 0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.NombreRegistrado);
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
                    if (objeto.EsParcial || objeto.CantidadDetalleDesagregacion>0)
                    {
                        objeto.idEstado = (int)EstadosRegistro.EnProceso;
                    }
                    else
                    {
                        objeto.idEstado = (int)EstadosRegistro.Activo;
                    }
                    if (objeto.idTipoDetalle== (int)TipoDetalleCategoriaEnum.Fecha )
                    {
                        if (objeto.DetalleCategoriaFecha.FechaMinima>= objeto.DetalleCategoriaFecha.FechaMaxima & objeto.EsParcial==false)
                        {
                            throw new Exception(Errores.ValorFecha);
                        }
                        var result = clsDatos.ActualizarDatos(objeto)
                           .Where(x => x.Codigo.ToUpper() == objeto.Codigo.ToUpper()).FirstOrDefault();
                        objeto.DetalleCategoriaFecha.idCategoria = result.idCategoria;
                        objeto.DetalleCategoriaFecha.Estado = true;
                        if (objeto.DetalleCategoriaFecha.FechaMaxima!=null && objeto.DetalleCategoriaFecha.FechaMinima != null)
                        {
                            clsDatos.InsertarDetalleFecha(objeto.DetalleCategoriaFecha);
                        }
                       
                    }
                    else if(objeto.idTipoDetalle == (int)TipoDetalleCategoriaEnum.Numerico)
                    {
                        if (objeto.DetalleCategoriaNumerico.Minimo>=objeto.DetalleCategoriaNumerico.Maximo & objeto.EsParcial==false)
                        {
                            throw new Exception(Errores.ValorMinimo);
                        }
                        var result = clsDatos.ActualizarDatos(objeto)
                           .Where(x => x.Codigo.ToUpper() == objeto.Codigo.ToUpper()).FirstOrDefault();
                        objeto.DetalleCategoriaNumerico.idCategoria = result.idCategoria;
                        objeto.DetalleCategoriaNumerico.Estado = true;
                        clsDatos.InsertarDetalleNumerico(objeto.DetalleCategoriaNumerico);
                    }
                    else
                    {
                        var result = clsDatos.ActualizarDatos(objeto)
                                .Where(x => x.Codigo.ToUpper() == objeto.Codigo.ToUpper()).FirstOrDefault();
                    }

  
                }

                objeto = clsDatos.ObtenerDatos(objeto).Single();

                string jsonValorInicial = SerializarObjetoBitacora(objeto);

                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                            ResultadoConsulta.Usuario,
                                ResultadoConsulta.Clase, objeto.Codigo,"","",jsonValorInicial);
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

        public RespuestaConsulta<List<CategoriasDesagregacion>> ListaCategoriasParaRelacion(CategoriasDesagregacion objeto)
        {
            try
            {
                if (!String.IsNullOrEmpty(objeto.id))
                {
                    objeto.id = Utilidades.Desencriptar(objeto.id);
                    int temp;
                    if (int.TryParse(objeto.id, out temp))
                    {
                        objeto.idCategoria = temp;
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
        public RespuestaConsulta<List<CategoriasDesagregacion>> ObtenerCategoriasDeFormulaNivelCalculo(string pIdFormula, string pIdIndicador)
        {
            RespuestaConsulta<List<CategoriasDesagregacion>> resultado = new RespuestaConsulta<List<CategoriasDesagregacion>>();

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
                List<CategoriasDesagregacion> result = clsDatos.ObtenerCategoriasDeFormulaNivelCalculo(idFormula, idIndicador);
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
        public RespuestaConsulta<List<CategoriasDesagregacion>> ObtenerCategoriasDesagregacionDeIndicador(string pIdIndicador, bool pInsertarOpcionTodos = false)
        {
            RespuestaConsulta<List<CategoriasDesagregacion>> resultado = new RespuestaConsulta<List<CategoriasDesagregacion>>();

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
                List<CategoriasDesagregacion> result = clsDatos.ObtenerCategoriasDesagregacionDeIndicador(idIndicador);
                resultado.objetoRespuesta = result;
                resultado.CantidadRegistros = result.Count();

                if (resultado.CantidadRegistros > 0 && pInsertarOpcionTodos)
                {
                    resultado.objetoRespuesta.Insert(0, new CategoriasDesagregacion() { id = select2MultipleOptionTodosValue, NombreCategoria = select2MultipleOptionTodosText });
                }
            }
            catch (Exception ex)
            {
                resultado.HayError = (int)Error.ErrorSistema;
                resultado.MensajeError = ex.Message;
            }
            return resultado;
        }
    }
}
