using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL
{
    public class RelacionCategoriaBL : IMetodos<RelacionCategoria>
    {
        private readonly RelacionCategoriaDAL clsDatos;
        private readonly CategoriasDesagregacionDAL clsDatosTexto;

        private RespuestaConsulta<List<RelacionCategoria>> ResultadoConsulta;
        string modulo = EtiquetasViewRelacionCategoria.RelacionCategoria;

        public RelacionCategoriaBL()
        {
            this.clsDatos = new RelacionCategoriaDAL();
            this.clsDatosTexto = new CategoriasDesagregacionDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<RelacionCategoria>>();
        }

        /// <summary>
        /// Fecha 22/08/2022
        /// Francisco Vindas Ruiz
        /// Metodo para editar la relacion categoria
        /// </summary>
        public RespuestaConsulta<List<RelacionCategoria>> ActualizarElemento(RelacionCategoria objeto)
        {
            try
            {

                //OBTENEMOS UNA LISTA DE RELACION CATEGORIA
                List<RelacionCategoria> Registros = clsDatos.ObtenerDatos(new RelacionCategoria());

                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Editar;
                ResultadoConsulta.Usuario = objeto.UsuarioCreacion;
                objeto.UsuarioModificacion = objeto.UsuarioCreacion;

                //DESENCRIPTAR EL ID
                if (!string.IsNullOrEmpty(objeto.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.id), out temp);
                    objeto.idRelacionCategoria = temp;
                }

                //GUARDAMOS EL OBJETO EN UNA VARIBALE SEGUN EL ID
                var result = Registros.Where(x => x.idRelacionCategoria == objeto.idRelacionCategoria).Single();

                //VALIDA SI NO SE ENCONTRARON REGISTROS
                if (Registros.Where(x => x.idRelacionCategoria == objeto.idRelacionCategoria).Count() == 0)
                {
                    throw new Exception(Errores.NoRegistrosActualizar);
                }
                else if (result.DetalleRelacionCategoria.Count() > objeto.CantidadCategoria)
                {
                    throw new Exception(Errores.CantidadRegistrosLimite);
                }

                //VALIDAR EL NOMBRE - SI BUSCAR REGISTRO NOMBRE ES IGUAL AL NOMBRE DEL OBJETO ES MAYOR A 0 
                //if (Registros.Where(X => X.Nombre.ToUpper() == objeto.Nombre.ToUpper()).ToList().Count() > 0)
                if (Registros.Where(X => X.Nombre.ToUpper() == objeto.Nombre.ToUpper() && !X.idRelacionCategoria.Equals(objeto.idRelacionCategoria)).ToList().Count() >= 1)   
                {
                    throw new Exception(Errores.NombreRegistrado);
                }

                //VALIDAR QUE NO EXCEDA EL LIMITE DE REGISTROS
                else if (result.idCategoriaValor.Count() > objeto.CantidadCategoria)
                {
                    throw new Exception(Errores.CantidadRegistrosLimite);
                }
                else
                {

                    //HACEMOS LA EDICION
                    result = clsDatos.ActualizarDatos(objeto)
                    .Where(x => x.Codigo.ToUpper() == objeto.Codigo.ToUpper()).FirstOrDefault();

                }

                //REGISTRAMOS EN BITACORA 1111
                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                        ResultadoConsulta.Usuario,
                            ResultadoConsulta.Clase, objeto.Codigo);

            }
            catch (Exception ex)
            {

                if (ex.Message == Errores.NoRegistrosActualizar || ex.Message == Errores.NombreRegistrado || ex.Message == Errores.CantidadRegistrosLimite)
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

        public RespuestaConsulta<List<RelacionCategoria>> CambioEstado(RelacionCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<RelacionCategoria>> ClonarDatos(RelacionCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<RelacionCategoria>> EliminarElemento(RelacionCategoria objeto)
        {
            RelacionCategoria objrelacion = (RelacionCategoria)objeto;

            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Eliminar;
                ResultadoConsulta.Usuario = objeto.UsuarioModificacion;
                RelacionCategoria registroActualizar;

                //DESENCRIPTAR EL ID
                if (!string.IsNullOrEmpty(objrelacion.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objrelacion.id), out temp);
                    objrelacion.idRelacionCategoria = temp;
                }

                var resul = clsDatos.ObtenerDatos(objrelacion);
                if (resul.Count() == 0)
                {
                    throw new Exception(Errores.NoRegistrosActualizar);

                }
                else
                {
                    registroActualizar = resul.SingleOrDefault();
                    registroActualizar.idEstado = 4;
                    resul = clsDatos.ActualizarDatos(registroActualizar);
                }

                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();

                //REGISTRAMOS EN BITACORA 
                //clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                //        ResultadoConsulta.Usuario,
                //            ResultadoConsulta.Clase, objeto.Codigo);

            }
            catch (Exception ex)
            {
                ResultadoConsulta.MensajeError = ex.Message;
                if (ex.Message == Errores.NoRegistrosActualizar)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                }
                else
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorSistema;

                }
            }

            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<RelacionCategoria>> InsertarDatos(RelacionCategoria objeto)
        {
            try
            {
                //VALIDACIONES DE CODIGOS REGISTRADOS Y NOMBRES REGISTRADOS NO SE REPITAN

                //OBTENEMOS UNA LISTA DE RELACION CATEGORIA
                List<RelacionCategoria> BuscarRegistros = clsDatos.ObtenerDatos(new RelacionCategoria());

                //VALIDAR EL CODIGO - SI BUSCAR REGISTRO CODIGO ES IGUAL AL CODIGO DEL OBJETO ES MAYOR A 0 
                if (BuscarRegistros.Where(X => X.Codigo.ToUpper() == objeto.Codigo.ToUpper() && !X.idRelacionCategoria.Equals(objeto.idRelacionCategoria)).ToList().Count() > 0)
                {
                    //ENVIE EL ERROR CODIGO REGISTRADO
                    throw new Exception(Errores.CodigoRegistrado);
                }

                //VALIDAR EL NOMBRE - SI BUSCAR REGISTRO NOMBRE ES IGUAL AL NOMBRE DEL OBJETO ES MAYOR A 0 
                if (BuscarRegistros.Where(X => X.Nombre.ToUpper() == objeto.Nombre.ToUpper() && !X.idRelacionCategoria.Equals(objeto.idRelacionCategoria)).ToList().Count() > 0)
                {
                    //ENVIE EL ERROR NOMBRE REGISTRADO
                    throw new Exception(Errores.NombreRegistrado);
                }
                else
                {
                    //HACEMOS LA INSERCION 

                    ResultadoConsulta.Clase = modulo;
                    ResultadoConsulta.Accion = (int)Accion.Insertar;
                    var resul = clsDatos.ActualizarDatos(objeto);
                    ResultadoConsulta.objetoRespuesta = resul;
                    ResultadoConsulta.Usuario = objeto.UsuarioCreacion;

                }

                //EVENTO PARA REGISTRAR EN BITACORA 
                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                            ResultadoConsulta.Usuario,
                                ResultadoConsulta.Clase, objeto.Codigo);


            }
            catch (Exception ex)
            {
                if (ex.Message == Errores.CantidadRegistros || ex.Message == Errores.NombreRegistrado || ex.Message == Errores.CodigoRegistrado)
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

        /// <summary>
        /// Fecha 10/08/2022
        /// Francisco Vindas Ruiz
        /// Metodo para obtener la lista Relacion Categorias
        /// </summary>
        public RespuestaConsulta<List<RelacionCategoria>> ObtenerDatos(RelacionCategoria objRelacionCategoria)
        {
            try
            {

                if (!string.IsNullOrEmpty(objRelacionCategoria.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objRelacionCategoria.id), out temp);
                    objRelacionCategoria.idRelacionCategoria = temp;
                }

                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;
                var resul = clsDatos.ObtenerDatos(objRelacionCategoria);
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
        /// Fecha 19/08/2022
        /// Francisco Vindas Ruiz
        /// Metodo para obtener la lista Relacion Categorias
        /// </summary>
        public RespuestaConsulta<List<string>> ObtenerListaCategoria(CategoriasDesagregacion obj)
        {
            RespuestaConsulta<List<string>> result = new RespuestaConsulta<List<string>>();

            result.objetoRespuesta = new List<string>();

            var ErrorControlado = false;

            try
            {
                CategoriasDesagregacion Categoria = clsDatosTexto.ObtenerDatos(obj).Single();

                var listaRelacionCategoria = clsDatos.ObtenerDatos(new RelacionCategoria() { idCategoria = Categoria.idCategoria }).ToList();


                if (Categoria.IdTipoCategoria == (int)TipoDetalleCategoriaEnum.Fecha)
                {
                    DateTime fecha = Categoria.DetalleCategoriaFecha.FechaMinima;

                    while (fecha <= Categoria.DetalleCategoriaFecha.FechaMaxima)
                    {
                        if (listaRelacionCategoria.Where(x => x.idCategoriaValor == fecha.ToString()).Count() == 0)
                        {

                            result.objetoRespuesta.Add(fecha.ToString());
                        }

                        fecha = fecha.AddDays(1);
                    }
                }
                else if (Categoria.IdTipoCategoria == (int)TipoDetalleCategoriaEnum.Numerico)
                {

                    int numeroMinimo = (int)Categoria.DetalleCategoriaNumerico.Minimo;
                    for (int i = numeroMinimo; i <= obj.DetalleCategoriaNumerico.Maximo; i++)
                    {

                        //if (listaRelacionCategoria.Where(x => x.idCategoriaValor == numeroMinimo.ToString()).Count() == 0)
                        //{
                        //    result.Add(i.ToString());
                        //}

                        result.objetoRespuesta.Add(i.ToString());

                    }
                }
                else
                {
                    //ACA FALTA UNA VALIDACION

                    result.objetoRespuesta = Categoria.DetalleCategoriaTexto.Select(x => x.Etiqueta).ToList();
                }

            }
            catch (Exception ex)
            {
                result.MensajeError = ex.Message;

                if (ErrorControlado)

                    result.HayError = (int)Error.ErrorControlado;

                else

                    result.HayError = (int)Error.ErrorSistema;
            }

            return result;

        }

        /// <summary>
        /// Fecha 25/08/2022
        /// Francisco Vindas Ruiz
        /// Metodo para obtener la lista Relacion Categorias
        /// </summary>
        public RespuestaConsulta<List<string>> ObtenerListaDetalleCategoria(CategoriasDesagregacion obj)
        {
            RespuestaConsulta<List<string>> result = new RespuestaConsulta<List<string>>();

            result.objetoRespuesta = new List<string>();

            var ErrorControlado = false;

            try
            {
                CategoriasDesagregacion Categoria = clsDatosTexto.ObtenerDatos(obj).Single();

                var listaRelacionCategoria = clsDatos.ObtenerDatos(new RelacionCategoria() { idCategoria = Categoria.idCategoria }).ToList();


                if (Categoria.IdTipoCategoria == (int)TipoDetalleCategoriaEnum.Fecha)
                {
                    DateTime fecha = Categoria.DetalleCategoriaFecha.FechaMinima;

                    while (fecha <= Categoria.DetalleCategoriaFecha.FechaMaxima)
                    {

                        result.objetoRespuesta.Add(fecha.ToString());

                        fecha = fecha.AddDays(1);
                    }
                }
                else if (Categoria.IdTipoCategoria == (int)TipoDetalleCategoriaEnum.Numerico)
                {

                    int numeroMinimo = (int)Categoria.DetalleCategoriaNumerico.Minimo;
                    for (int i = numeroMinimo; i <= obj.DetalleCategoriaNumerico.Maximo; i++)
                    {
                        result.objetoRespuesta.Add(i.ToString());
                    }
                }
                else
                {
                    result.objetoRespuesta = Categoria.DetalleCategoriaTexto.Select(x => x.Etiqueta).ToList();
                }

            }
            catch (Exception ex)
            {
                result.MensajeError = ex.Message;

                if (ErrorControlado)

                    result.HayError = (int)Error.ErrorControlado;

                else

                    result.HayError = (int)Error.ErrorSistema;
            }


            return result;

        }


        public RespuestaConsulta<List<RelacionCategoria>> ValidarDatos(RelacionCategoria objeto)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Fecha 16/09/2022
        /// Francisco Vindas Ruiz
        /// Validar existencia en Indicadores
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public RespuestaConsulta<List<string>> ValidarExistencia(RelacionCategoria objeto)
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
    }
}
