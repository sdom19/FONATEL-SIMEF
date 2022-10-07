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
                    throw new Exception(Errores.NoRegistrosActualizar);
                }
                else if (result.DetalleCategoriaTexto.Count()>objeto.CantidadDetalleDesagregacion)
                {
                    throw new Exception(Errores.CantidadRegistrosLimite);
                }
                else
                {
                    objeto.idEstado = objeto.EsParcial==true? (int) Constantes.EstadosRegistro.EnProceso  :result.idEstado;           
                    if (objeto.idTipoDetalle == (int)TipoDetalleCategoriaEnum.Fecha)
                    {
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

                if ( ex.Message == Errores.NoRegistrosActualizar || 
                    ex.Message == Errores.NombreRegistrado || ex.Message== Errores.CantidadRegistrosLimite 
                    || ex.Message==Errores.ValorMinimo || ex.Message==Errores.ValorFecha)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                }

                else
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
                string codigo = objeto.Codigo;
                string Nombre = objeto.NombreCategoria;
                if (!string.IsNullOrEmpty( objeto.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.id), out temp );
                    objeto.idCategoria = temp;
                }

                string jsonInicial = SerializarObjetoBitacora(listadoCategorias.Where(x => x.idCategoria == objeto.idCategoria).Single());
                objeto = listadoCategorias.Where(x => x.idCategoria == objeto.idCategoria).Single();
              

                if (listadoCategorias.Where(x=>x.Codigo.ToUpper()==codigo.ToUpper().Trim()).Count()>0)
                {
                    throw new Exception(Errores.CodigoRegistrado);
                }
                else if(listadoCategorias.Where(x => x.NombreCategoria.ToUpper() == Nombre.ToUpper()).Count() > 0)
                {
                    throw new Exception(Errores.NombreRegistrado);
                }
                else
                {
                    objeto.Codigo = codigo;
                    objeto.NombreCategoria = Nombre;
                    objeto.idCategoria = 0;
                    var result = clsDatos.ActualizarDatos(objeto)
                      .Where(x => x.Codigo.ToUpper() == objeto.Codigo.ToUpper()).FirstOrDefault();

                    if (objeto.idTipoDetalle == (int)TipoDetalleCategoriaEnum.Fecha)
                    {
                        objeto.DetalleCategoriaFecha.idCategoria = result.idCategoria;
                        clsDatos.InsertarDetalleFecha(objeto.DetalleCategoriaFecha);
                    }
                    else if (objeto.idTipoDetalle == (int)TipoDetalleCategoriaEnum.Numerico)
                    {
                        objeto.DetalleCategoriaNumerico.idCategoria = result.idCategoria;
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

                if (ex.Message == Errores.CantidadRegistros || ex.Message == Errores.CodigoRegistrado || ex.Message == Errores.NombreRegistrado)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                }

                else
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
                    throw new Exception(Errores.CodigoRegistrado);
                }
                else if (buscarRegistro.Where(x => x.NombreCategoria.ToUpper() == objeto.NombreCategoria.ToUpper()).ToList().Count() > 0)
                {
                    throw new Exception(Errores.NombreRegistrado);
                }
                else
                {
                    if (objeto.EsParcial || objeto.idTipoDetalle == (int)TipoDetalleCategoriaEnum.Texto || objeto.idTipoDetalle==(int)TipoDetalleCategoriaEnum.Alfanumerico)
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
                        clsDatos.InsertarDetalleFecha(objeto.DetalleCategoriaFecha);
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
                if (ex.Message == Errores.CantidadRegistros || ex.Message == Errores.CodigoRegistrado || ex.Message == Errores.NombreRegistrado 
                    || ex.Message== Errores.ValorMinimo || ex.Message==Errores.ValorFecha)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                }

                else
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorSistema;

                }
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }


     

    }
}
