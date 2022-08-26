using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
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
        string modulo = Etiquetas.Categorias;

        public CategoriasDesagregacionBL()
        {
            this.clsDatos = new CategoriasDesagregacionDAL();
            this.clsDatosTexto = new DetalleCategoriaTextoDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<CategoriasDesagregacion>>();
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
                ResultadoConsulta.Usuario = objeto.UsuarioCreacion;
                objeto.UsuarioModificacion = objeto.UsuarioCreacion;
          
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
                    objeto.idEstado = result.idEstado;

                      result=  clsDatos.ActualizarDatos(objeto)
                      .Where(x => x.Codigo.ToUpper() == objeto.Codigo.ToUpper()).FirstOrDefault();

                    if (objeto.idTipoDetalle == (int)TipoDetalleCategoriaEnum.Fecha)
                    {
                        objeto.DetalleCategoriaFecha.idCategoria = result.idCategoria;
                        objeto.DetalleCategoriaFecha.Estado = true;
                        clsDatos.InsertarDetalleFecha(objeto.DetalleCategoriaFecha);
                    }
                    else if (objeto.idTipoDetalle == (int)TipoDetalleCategoriaEnum.Numerico)
                    {
                        objeto.DetalleCategoriaNumerico.idCategoria = result.idCategoria;
                        objeto.DetalleCategoriaNumerico.Estado = true;
                        clsDatos.InsertarDetalleNumerico(objeto.DetalleCategoriaNumerico);
                    }



                }


                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                        ResultadoConsulta.Usuario,
                            ResultadoConsulta.Clase, objeto.Codigo);
            }
            catch (Exception ex)
            {

                if ( ex.Message == Errores.NoRegistrosActualizar || ex.Message == Errores.NombreRegistrado || ex.Message== Errores.CantidadRegistrosLimite)
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
                int nuevoEstado = objeto.idEstado;
                objeto.idEstado = 0;
                ResultadoConsulta.Usuario = objeto.UsuarioModificacion;
                var resul = clsDatos.ObtenerDatos(objeto);
                objeto = resul.Single();
                objeto.idEstado = nuevoEstado;
                objeto.UsuarioModificacion = ResultadoConsulta.Usuario;
                ResultadoConsulta.Accion= (int)EstadosRegistro.Activo == objeto.idEstado ? (int)Accion.Activar : (int)Accion.Inactiva;
                resul = clsDatos.ActualizarDatos(objeto);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();
                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                       ResultadoConsulta.Usuario,
                       ResultadoConsulta.Clase,objeto.Codigo);

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
                ResultadoConsulta.Accion = (int)Accion.Editar;
                ResultadoConsulta.Usuario = objeto.UsuarioCreacion;
                string codigo = objeto.Codigo;
                string Nombre = objeto.NombreCategoria;
                if (!string.IsNullOrEmpty( objeto.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.id), out temp );
                    objeto.idCategoria = temp;
                }
            
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
                }
                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                        ResultadoConsulta.Usuario,
                            ResultadoConsulta.Clase, objeto.Codigo);
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
                ResultadoConsulta.Usuario = objeto.UsuarioCreacion;
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
                    objeto.idEstado = (int)EstadosRegistro.Desactivado;
                    var result =clsDatos.ActualizarDatos(objeto)
                        .Where(x=>x.Codigo.ToUpper()==objeto.Codigo.ToUpper()).FirstOrDefault();
                    if (objeto.idTipoDetalle== (int)TipoDetalleCategoriaEnum.Fecha)
                    {
                        objeto.DetalleCategoriaFecha.idCategoria = result.idCategoria;
                        objeto.DetalleCategoriaFecha.Estado = true;
                        clsDatos.InsertarDetalleFecha(objeto.DetalleCategoriaFecha);
                    }
                    else if(objeto.idTipoDetalle == (int)TipoDetalleCategoriaEnum.Numerico)
                    {
                        objeto.DetalleCategoriaNumerico.idCategoria = result.idCategoria;
                        objeto.DetalleCategoriaNumerico.Estado = true;
                        clsDatos.InsertarDetalleNumerico(objeto.DetalleCategoriaNumerico);
                    }

  
                }

                    clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                            ResultadoConsulta.Usuario,
                                ResultadoConsulta.Clase, objeto.Codigo);
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


     

    }
}
