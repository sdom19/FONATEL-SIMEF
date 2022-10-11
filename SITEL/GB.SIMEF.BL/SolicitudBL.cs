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
    public class SolicitudBL : IMetodos<Solicitud>
    {
        private readonly SolicitudDAL clsDatos;


        private RespuestaConsulta<List<Solicitud>> ResultadoConsulta;
        string modulo = EtiquetasViewSolicitudes.Solicitudes;
        string user = string.Empty;

        public SolicitudBL()
        {
            this.clsDatos = new SolicitudDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<Solicitud>>();
        }

        public RespuestaConsulta<List<Solicitud>> ObtenerDatos(Solicitud objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;

                if (!String.IsNullOrEmpty(objeto.id))
                {
                    objeto.id = Utilidades.Desencriptar(objeto.id);
                    int temp;
                    if (int.TryParse(objeto.id, out temp))
                    {
                        objeto.idSolicitud = temp;
                    }
                }

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

        public RespuestaConsulta<List<string>> ValidarExistenciaSolicitudEliminar(Solicitud objeto)
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
                        objeto.idSolicitud = temp;
                    }
                }
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;
                listaExistencias.objetoRespuesta = clsDatos.ValidarSolicitud(objeto);

            }
            catch (Exception ex)
            {
                listaExistencias.HayError = (int)Constantes.Error.ErrorSistema;
                listaExistencias.MensajeError = ex.Message;
            }
            return listaExistencias;
        }

        public RespuestaConsulta<List<Solicitud>> ActualizarElemento(Solicitud objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Editar;
                ResultadoConsulta.Usuario = objeto.UsuarioCreacion;
                objeto.UsuarioModificacion = objeto.UsuarioCreacion;

                List<Solicitud> BuscarRegistros = clsDatos.ObtenerDatos(new Solicitud());

                if (!string.IsNullOrEmpty(objeto.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.id), out temp);
                    objeto.idSolicitud = temp;
                }

                var result = BuscarRegistros.Where(x => x.idSolicitud == objeto.idSolicitud).Single();

                if (BuscarRegistros.Where(x => x.idSolicitud == objeto.idSolicitud).Count() == 0)
                {
                    throw new Exception(Errores.NoRegistrosActualizar);
                }
                else if (result.CantidadFormularios > objeto.CantidadFormularios)
                {
                    //FALTA EL CONTADOR DE DETALLE
                    throw new Exception(Errores.CantidadRegistrosLimite);
                }
                else if (BuscarRegistros.Where(X => X.Nombre.ToUpper() == objeto.Nombre.ToUpper() && !X.idSolicitud.Equals(objeto.idSolicitud)).ToList().Count() >= 1)
                {
                    throw new Exception(Errores.NombreRegistrado);
                }
                //VALIDAR QUE NO EXCEDA EL LIMITE DE REGISTROS
                else if (result.FormularioWeb.Count() > objeto.CantidadFormularios)
                {
                    //FALTA EL CONTADOR DE DETALLE
                    throw new Exception(Errores.CantidadRegistrosLimite);
                }
                else if (objeto.FechaFin < objeto.FechaInicio)
                {
                    throw new Exception(Errores.ValorFecha);
                }
                else
                {
                    result = clsDatos.ActualizarDatos(objeto)
                    .Where(x => x.Codigo.ToUpper() == objeto.Codigo.ToUpper()).FirstOrDefault();
                }

                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                        ResultadoConsulta.Usuario,
                            ResultadoConsulta.Clase, objeto.Codigo);
            }
            catch (Exception ex)
            {

                if (ex.Message == Errores.NoRegistrosActualizar ||
                    ex.Message == Errores.NombreRegistrado || ex.Message == Errores.CantidadRegistrosLimite
                    || ex.Message == Errores.ValorMinimo || ex.Message == Errores.ValorFecha)
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

        public RespuestaConsulta<List<Solicitud>> CambioEstado(Solicitud objeto)
        {
            try
            {
                if (!String.IsNullOrEmpty(objeto.id))
                {
                    objeto.id = Utilidades.Desencriptar(objeto.id);
                    int temp;
                    if (int.TryParse(objeto.id, out temp))
                    {
                        objeto.idSolicitud = temp;
                    }
                }

                ResultadoConsulta.Clase = modulo;
                int nuevoEstado = objeto.IdEstado;
                objeto.IdEstado = 0;
                ResultadoConsulta.Usuario = user;


                var resul = clsDatos.ObtenerDatos(objeto);
                objeto = resul.Single();
                objeto.IdEstado = nuevoEstado;

                ResultadoConsulta.Accion = (int)EstadosRegistro.Activo == objeto.IdEstado ? (int)Accion.Activar : (int)Accion.Inactiva;
                resul = clsDatos.ActualizarDatos(objeto);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();



                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                       ResultadoConsulta.Usuario,
                       ResultadoConsulta.Clase, objeto.Codigo);

            }
            catch (Exception ex)
            {
                ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<Solicitud>> ClonarDatos(Solicitud objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Clonar;
                ResultadoConsulta.Usuario = objeto.UsuarioCreacion;

                List<Solicitud> BuscarRegistros = clsDatos.ObtenerDatos(new Solicitud());

                if (BuscarRegistros.Where(X => X.Codigo.ToUpper() == objeto.Codigo.ToUpper() && !X.idSolicitud.Equals(objeto.idSolicitud)).ToList().Count() > 0)
                {
                    throw new Exception(Errores.CodigoRegistrado);
                }

                else if (BuscarRegistros.Where(X => X.Nombre.ToUpper() == objeto.Nombre.ToUpper() && !X.idSolicitud.Equals(objeto.idSolicitud)).ToList().Count() > 0)
                {
                    throw new Exception(Errores.NombreRegistrado);
                }
                else if (objeto.FechaFin < objeto.FechaInicio)
                {
                    throw new Exception(Errores.ValorFecha);
                }
                else
                {
                    var resul = clsDatos.ActualizarDatos(objeto);
                    ResultadoConsulta.objetoRespuesta = resul;
                }


                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                        ResultadoConsulta.Usuario,
                            ResultadoConsulta.Clase, objeto.Codigo);
            }
            catch (Exception ex)
            {
                if (ex.Message == Errores.CantidadRegistros || ex.Message == Errores.CodigoRegistrado || ex.Message == Errores.NombreRegistrado
                    || ex.Message == Errores.ValorMinimo || ex.Message == Errores.ValorFecha)
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

        public RespuestaConsulta<List<Solicitud>> EliminarElemento(Solicitud objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Eliminar;
                ResultadoConsulta.Usuario = objeto.UsuarioModificacion;
                Solicitud registroActualizar;

                if (!string.IsNullOrEmpty(objeto.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.id), out temp);
                    objeto.idSolicitud = temp;
                }

                var resul = clsDatos.ObtenerDatos(objeto);
                if (resul.Count() == 0)
                {
                    throw new Exception(Errores.NoRegistrosActualizar);

                }
                else
                {
                    registroActualizar = resul.SingleOrDefault();
                    registroActualizar.IdEstado = 4;
                    resul = clsDatos.ActualizarDatos(registroActualizar);
                }

                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();

                //REGISTRAMOS EN BITACORA 
                //clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                //       ResultadoConsulta.Usuario,
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<Solicitud>> InsertarDatos(Solicitud objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Insertar;
                ResultadoConsulta.Usuario = objeto.UsuarioCreacion;

                List<Solicitud> BuscarRegistros = clsDatos.ObtenerDatos(new Solicitud());

                if (BuscarRegistros.Where(X => X.Codigo.ToUpper() == objeto.Codigo.ToUpper() && !X.idSolicitud.Equals(objeto.idSolicitud)).ToList().Count() > 0)
                {
                    throw new Exception(Errores.CodigoRegistrado);
                }

                if (BuscarRegistros.Where(X => X.Nombre.ToUpper() == objeto.Nombre.ToUpper() && !X.idSolicitud.Equals(objeto.idSolicitud)).ToList().Count() > 0)
                {
                    throw new Exception(Errores.NombreRegistrado);
                }
                else if (objeto.FechaFin < objeto.FechaInicio)
                {
                    throw new Exception(Errores.ValorFecha);
                }
                else
                {
                    var resul = clsDatos.ActualizarDatos(objeto);
                    ResultadoConsulta.objetoRespuesta = resul;
                }

                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                        ResultadoConsulta.Usuario,
                            ResultadoConsulta.Clase, objeto.Codigo);
            }
            catch (Exception ex)
            {
                if (ex.Message == Errores.CantidadRegistros || ex.Message == Errores.CodigoRegistrado || ex.Message == Errores.NombreRegistrado
                    || ex.Message == Errores.ValorMinimo || ex.Message == Errores.ValorFecha)
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

        public RespuestaConsulta<List<Solicitud>> ValidarDatos(Solicitud objeto)
        {
            throw new NotImplementedException();
        }
    }
}