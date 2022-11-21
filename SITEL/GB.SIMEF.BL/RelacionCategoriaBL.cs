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
        string modulo = string.Empty;
        string user = string.Empty;

        public RelacionCategoriaBL(string modulo, string user)
        {
            this.modulo = modulo;
            this.user = user;
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
                objeto.idEstado = (int)Constantes.EstadosRegistro.EnProceso;
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Editar;
                ResultadoConsulta.Usuario = user;
                objeto.UsuarioModificacion = user ;
                objeto.Codigo = objeto.Codigo.Trim();
                objeto.Nombre = objeto.Nombre.Trim();

                //DESENCRIPTAR EL ID
                if (!string.IsNullOrEmpty(objeto.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.id), out temp);
                    objeto.IdRelacionCategoria = temp;
                }



                //GUARDAMOS EL OBJETO EN UNA VARIBALE SEGUN EL ID
                var result = Registros.Where(x => x.IdRelacionCategoria == objeto.IdRelacionCategoria).Single();

                objeto.idEstado = result.idEstado;

                //VALIDA SI NO SE ENCONTRARON REGISTROS
                if (Registros.Where(x => x.IdRelacionCategoria == objeto.IdRelacionCategoria).Count() == 0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }
                else if (result.DetalleRelacionCategoria.Count() > objeto.CantidadCategoria)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.CantidadRegistrosLimiteRelaciones);
                }
                else if (!Utilidades.rx_alfanumerico.Match(objeto.Nombre.Trim()).Success)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(string.Format(Errores.CampoConFormatoInvalido, "nombre de Categoría"));
                }
                else if (!Utilidades.rx_alfanumerico.Match(objeto.Codigo.Trim()).Success)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(string.Format(Errores.CampoConFormatoInvalido, "código de Categoría"));
                }
                //VALIDAR EL NOMBRE - SI BUSCAR REGISTRO NOMBRE ES IGUAL AL NOMBRE DEL OBJETO ES MAYOR A 0 
                //if (Registros.Where(X => X.Nombre.ToUpper() == objeto.Nombre.ToUpper()).ToList().Count() > 0)
                if (Registros.Where(X => X.Nombre.ToUpper() == objeto.Nombre.ToUpper() && X.IdRelacionCategoria!=objeto.IdRelacionCategoria).ToList().Count() >0)   
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.NombreRegistrado);
                }
                else
                {

                    //HACEMOS LA EDICION
                    ResultadoConsulta.objetoRespuesta = clsDatos.ActualizarDatos(objeto);
                    ResultadoConsulta.CantidadRegistros = ResultadoConsulta.objetoRespuesta.Count;

                }


                //REGISTRAMOS EN BITACORA 1111
                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                        ResultadoConsulta.Usuario,
                            ResultadoConsulta.Clase, objeto.Codigo);

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
                ResultadoConsulta.Usuario = user;
                objeto.UsuarioModificacion = user;
                RelacionCategoria registroActualizar;

                //DESENCRIPTAR EL ID
                if (!string.IsNullOrEmpty(objrelacion.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objrelacion.id), out temp);
                    objrelacion.IdRelacionCategoria = temp;
                }

                var resul = clsDatos.ObtenerDatos(objrelacion);
                if (resul.Count() == 0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
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
                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                       ResultadoConsulta.Usuario,
                            ResultadoConsulta.Clase, ResultadoConsulta.objetoRespuesta.Single().Codigo);

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

        public RespuestaConsulta<List<RelacionCategoria>> InsertarDatos(RelacionCategoria objeto)
        {
            try
            {
                objeto.idEstado = (int)Constantes.EstadosRegistro.EnProceso;

                //OBTENEMOS UNA LISTA DE RELACION CATEGORIA
                List<RelacionCategoria> BuscarRegistros = clsDatos.ObtenerDatos(new RelacionCategoria());

                //VALIDAR EL CODIGO - SI BUSCAR REGISTRO CODIGO ES IGUAL AL CODIGO DEL OBJETO ES MAYOR A 0 
                if (BuscarRegistros.Where(X => X.Codigo.ToUpper() == objeto.Codigo.ToUpper() && !X.IdRelacionCategoria.Equals(objeto.IdRelacionCategoria)).ToList().Count() > 0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.CodigoRegistrado);
                }

                //VALIDAR EL NOMBRE - SI BUSCAR REGISTRO NOMBRE ES IGUAL AL NOMBRE DEL OBJETO ES MAYOR A 0 
                if (BuscarRegistros.Where(X => X.Nombre.ToUpper() == objeto.Nombre.ToUpper() && !X.IdRelacionCategoria.Equals(objeto.IdRelacionCategoria)).ToList().Count() > 0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.NombreRegistrado);
                }
                else if (!Utilidades.rx_alfanumerico.Match(objeto.Nombre.Trim()).Success)
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
                    //HACEMOS LA INSERCION 

                    ResultadoConsulta.Clase = modulo;
                    objeto.UsuarioCreacion = user;
                    ResultadoConsulta.Accion = (int)Accion.Insertar;
                    var resul = clsDatos.ActualizarDatos(objeto);
                    ResultadoConsulta.objetoRespuesta = resul;
                    ResultadoConsulta.Usuario = user;
                }

                //EVENTO PARA REGISTRAR EN BITACORA 
                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                            ResultadoConsulta.Usuario,
                                ResultadoConsulta.Clase, objeto.Codigo);


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
                    objRelacionCategoria.IdRelacionCategoria = temp;
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
                        objeto.IdRelacionCategoria = temp;
                    }
                }

                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;
                var resul = clsDatos.ObtenerDatos(objeto).Single();

                listaExistencias.objetoRespuesta = clsDatos.ValidarRelacion(resul);

            }
            catch (Exception ex)
            {
                listaExistencias.HayError = (int)Constantes.Error.ErrorSistema;
                listaExistencias.MensajeError = ex.Message;
            }
            return listaExistencias;
        }

        /// <summary>
        /// Fecha 29/09/2022
        /// Francisco Vindas Ruiz
        /// Cambia a estado Activo la relacion entre categoria
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<RelacionCategoria>> CambiarEstado(RelacionCategoria objeto)
        {
            RelacionCategoria objrelacion = objeto;

            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Activar;
                ResultadoConsulta.Usuario = user;
                objeto.UsuarioModificacion = user;
                //DESENCRIPTAR EL ID
                if (!string.IsNullOrEmpty(objrelacion.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objrelacion.id), out temp);
                    objrelacion.IdRelacionCategoria = temp;
                }
                objrelacion.idEstado = (int)Constantes.EstadosRegistro.Activo;
                ResultadoConsulta.objetoRespuesta = clsDatos.ActualizarDatos(objrelacion);
                ResultadoConsulta.CantidadRegistros = ResultadoConsulta.objetoRespuesta.Count();

                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                 ResultadoConsulta.Usuario,
                      ResultadoConsulta.Clase, objeto.Codigo);

            }
            catch (Exception ex)
            {
                
                if (ResultadoConsulta.HayError!= (int)Error.ErrorControlado)
                {
          
                    ResultadoConsulta.HayError = (int)Error.ErrorSistema;

                }

                ResultadoConsulta.MensajeError = ex.Message;
            }

            return ResultadoConsulta;
        }




    }


    public class DetalleRelacionCategoriaBL : IMetodos<RelacionCategoria>
    {


        private readonly RelacionCategoriaDAL clsDatos;
        private readonly CategoriasDesagregacionDAL clsDatosTexto;

        private RespuestaConsulta<List<RelacionCategoria>> ResultadoConsulta;
        string modulo = string.Empty;
        string user = string.Empty;

        public DetalleRelacionCategoriaBL(string modulo, string user)
        {
            this.modulo = modulo;
            this.user = user;
            this.clsDatos = new RelacionCategoriaDAL();
            this.clsDatosTexto = new CategoriasDesagregacionDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<RelacionCategoria>>();
        }


        public RespuestaConsulta<List<RelacionCategoria>> ActualizarElemento(RelacionCategoria objeto)
        {
            throw new NotImplementedException();
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
            try
            {
                DetalleRelacionCategoria detalleRelacion = objeto.DetalleRelacionCategoria.FirstOrDefault();
                

                objeto.idEstado = (int)Constantes.EstadosRegistro.EnProceso;
                ResultadoConsulta.Usuario = user;
                ResultadoConsulta.Clase = modulo;

                if (!string.IsNullOrEmpty(detalleRelacion.relacionid))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(detalleRelacion.relacionid), out temp);
                    detalleRelacion.IdRelacionCategoria = temp;
                    objeto.IdRelacionCategoria = temp;
                }

                if (!string.IsNullOrEmpty(detalleRelacion.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(detalleRelacion.id), out temp);
                    detalleRelacion.idDetalleRelacionCategoria = temp;
                }

                objeto = clsDatos.ActualizarDatos(objeto).FirstOrDefault();
                detalleRelacion = objeto.DetalleRelacionCategoria.Where(x => x.idDetalleRelacionCategoria == detalleRelacion.idDetalleRelacionCategoria).FirstOrDefault();
                detalleRelacion.Estado = false;
                ResultadoConsulta.objetoRespuesta = clsDatos.ActualizarDatosDetalle(detalleRelacion);
                ResultadoConsulta.CantidadRegistros = ResultadoConsulta.objetoRespuesta.Count();
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

        public RespuestaConsulta<List<RelacionCategoria>> InsertarDatos(RelacionCategoria objeto)
        {
            try
            {
                DetalleRelacionCategoria detalleRelacion = objeto.DetalleRelacionCategoria.FirstOrDefault();
                detalleRelacion.Estado = true;
                ResultadoConsulta.Usuario = user;
                ResultadoConsulta.Clase = modulo;

                if (!string.IsNullOrEmpty(detalleRelacion.relacionid))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(detalleRelacion.relacionid), out temp);
                    detalleRelacion.IdRelacionCategoria = temp;
                    objeto.IdRelacionCategoria = temp;
                }

                objeto = clsDatos.ObtenerDatos(objeto).FirstOrDefault();


                if (objeto.DetalleRelacionCategoria.Where(p=>p.idCategoriaAtributo==detalleRelacion.idCategoriaAtributo).Count()>0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception("La categoría ya está asignada a la Relación");   
                }
                else if(objeto.CantidadCategoria<=objeto.DetalleRelacionCategoria.Count())
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.CantidadRegistrosLimiteRelaciones);
                }
                else
                {
                    ResultadoConsulta.objetoRespuesta = clsDatos.ActualizarDatosDetalle(detalleRelacion);
                    ResultadoConsulta.CantidadRegistros = ResultadoConsulta.objetoRespuesta.Count();
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

        public RespuestaConsulta<List<RelacionCategoria>> ObtenerDatos(RelacionCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<RelacionCategoria>> ValidarDatos(RelacionCategoria objeto)
        {
            throw new NotImplementedException();
        }
    }
}
