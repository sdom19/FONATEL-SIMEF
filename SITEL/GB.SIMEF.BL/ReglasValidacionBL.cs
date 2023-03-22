using System;
using System.Collections.Generic;
using System.Linq;
using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using Newtonsoft.Json;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL
{
    public class ReglaValidacionBL : IMetodos<ReglaValidacion>
    {
        private readonly ReglasValicionDAL clsDatos;
        private RespuestaConsulta<List<ReglaValidacion>> ResultadoConsulta;
        string modulo = Etiquetas.ReglasValidacion;
        string user;
        public ReglaValidacionBL(string modulo, string user)
        {
            this.modulo = modulo;
            this.user = user;
            clsDatos = new ReglasValicionDAL();
            ResultadoConsulta = new RespuestaConsulta<List<ReglaValidacion>>();
        }

        public RespuestaConsulta<List<ReglaValidacion>> ActualizarElemento(ReglaValidacion objeto)
        {
            try
            {
                List<ReglaValidacion> listadoReglas = clsDatos.ObtenerDatos(new ReglaValidacion());

                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Constantes.Accion.Editar;
                ResultadoConsulta.Usuario = user;
                objeto.UsuarioModificacion = user;

                DesencriptarReglasValidacion(objeto);

                var objetoAnterior  = listadoReglas.Where(x => x.idReglaValidacion == objeto.idReglaValidacion).Single();

                ValidarObjetoRegla(objeto);

                objeto.idEstadoRegistro = objetoAnterior.idEstadoRegistro;

                if (objeto.Descripcion.Equals(objetoAnterior.Descripcion) && objeto.idIndicador.Equals(objetoAnterior.idIndicador) && objeto.idEstadoRegistro == (int)Constantes.EstadosRegistro.Activo)
                {
                    objeto.idEstadoRegistro = (int)EstadosRegistro.Activo;
                }
                else
                {
                    objeto.idEstadoRegistro = (int)EstadosRegistro.EnProceso;
                }

                ResultadoConsulta.objetoRespuesta = clsDatos.ActualizarDatos(objeto);
                ResultadoConsulta.CantidadRegistros = listadoReglas.Count();
                

                objeto = ResultadoConsulta.objetoRespuesta.Single();
                string JsonActual = objeto.ToString();
                string JsonAnterior = objetoAnterior.ToString();

                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                        ResultadoConsulta.Usuario,
                            ResultadoConsulta.Clase, objeto.Codigo
                            , JsonActual, JsonAnterior, "");

            }
            catch (Exception ex)
            {
                ResultadoConsulta.MensajeError = ex.Message;

                if (ResultadoConsulta.HayError!= (int)Error.ErrorControlado)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                }
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<ReglaValidacion>> CambioEstado(ReglaValidacion objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Constantes.Accion.Activar;
                int nuevoEstado = (int)Constantes.EstadosRegistro.Activo;
                ResultadoConsulta.Usuario = user;
                objeto.UsuarioModificacion = user;

                if (!String.IsNullOrEmpty(objeto.id))
                {
                    objeto.id = Utilidades.Desencriptar(objeto.id);
                    int temp;
                    if (int.TryParse(objeto.id, out temp))
                    {
                        objeto.idReglaValidacion = temp;
                    }
                }

                var resul = clsDatos.ObtenerDatos(objeto).ToList();

                if (resul.Count() == 0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }
                else
                {
                    objeto = resul.Single();
                    objeto.idEstadoRegistro = nuevoEstado;
                    objeto.UsuarioModificacion = ResultadoConsulta.Usuario;
                    resul = clsDatos.ActualizarDatos(objeto);
                    ResultadoConsulta.objetoRespuesta = resul;
                    ResultadoConsulta.CantidadRegistros = resul.Count();
                }
            }
            catch (Exception ex)
            {
                ResultadoConsulta.MensajeError = ex.Message;

                if (ResultadoConsulta.HayError != (int)Error.ErrorControlado)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                }
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<ReglaValidacion>> ClonarDatos(ReglaValidacion objeto)
        {
            try
            {
                List<ReglaValidacion> listadoReglas = clsDatos.ObtenerDatos(new ReglaValidacion());

                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Constantes.Accion.Clonar;
                ResultadoConsulta.Usuario = user;
                objeto.UsuarioCreacion = user;
                objeto.idEstadoRegistro = (int)EstadosRegistro.EnProceso;

                DesencriptarReglasValidacion(objeto);

                var BuscarDatos = clsDatos.ObtenerDatos(new ReglaValidacion());
                var objetoInicial = listadoReglas.Where(x => x.idReglaValidacion == objeto.idReglaValidacion).Single();

                objeto.id = string.Empty;
                objeto.idReglaValidacion = 0;


                if (BuscarDatos.Where(x => x.idReglaValidacion != objeto.idReglaValidacion && x.Codigo.ToUpper() == objeto.Codigo.ToUpper() && x.idEstadoRegistro != 4).Count() > 0)
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                    throw new Exception(Errores.CodigoRegistrado);
                }

                if (BuscarDatos.Where(x => x.idReglaValidacion != objeto.idReglaValidacion && x.Nombre.ToUpper() == objeto.Nombre.ToUpper() && x.idEstadoRegistro != 4).Count() > 0)
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                    throw new Exception(Errores.NombreRegistrado);
                }
                else
                {
                    var resul = clsDatos.ActualizarDatos(objeto);
                    ResultadoConsulta.objetoRespuesta = resul;
                    ResultadoConsulta.CantidadRegistros = resul.Count();
                }

                objeto = clsDatos.ObtenerDatos(objeto).Single();

                string jsonValorInicial = objetoInicial.ToString();
                string JsonNuevoValor = objeto.ToString();

                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                            ResultadoConsulta.Usuario,
                                ResultadoConsulta.Clase, objeto.Codigo, JsonNuevoValor, "", jsonValorInicial);

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

        public RespuestaConsulta<ReglaValidacion> ClonarDetallesReglas(string pIdReglaAClonar, string pIdReglaDestino)
        {
            RespuestaConsulta<ReglaValidacion> resultado = new RespuestaConsulta<ReglaValidacion>();

            int IdReglaAClonar, IdReglaDestino;

            try
            {
                resultado.Usuario = user;
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Clonar;

                int.TryParse(Utilidades.Desencriptar(pIdReglaAClonar), out int number);
                IdReglaAClonar = number;

                if (IdReglaAClonar == 0) // ¿ID descencriptado con éxito?
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                int.TryParse(Utilidades.Desencriptar(pIdReglaDestino), out number);
                IdReglaDestino = number;

                if (IdReglaDestino == 0) // ¿ID descencriptado con éxito?
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                clsDatos.ClonarDetallesReglas(IdReglaAClonar, IdReglaDestino);

                resultado.objetoRespuesta = new ReglaValidacion() { id = pIdReglaDestino };


            }
            catch (Exception ex)
            {
                if (ResultadoConsulta.HayError != (int)Error.ErrorControlado)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                }
                ResultadoConsulta.MensajeError = ex.Message;
            }

            return resultado;
        }

        public RespuestaConsulta<List<ReglaValidacion>> EliminarElemento(ReglaValidacion objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Usuario = user;
                objeto.UsuarioModificacion = user;
                ResultadoConsulta.Accion = (int)Constantes.Accion.Eliminar;

                DesencriptarReglasValidacion(objeto);

                var resul = clsDatos.ObtenerDatos(objeto).ToList();

                if (resul.Count() == 0)
                {
                    throw new Exception(Errores.NoRegistrosActualizar);
                }
                else
                {
                    objeto = resul.Single();
                    objeto.idEstadoRegistro = (int)Constantes.EstadosRegistro.Eliminado;
                    resul = clsDatos.ActualizarDatos(objeto);
                    ResultadoConsulta.objetoRespuesta = resul;
                    ResultadoConsulta.CantidadRegistros = resul.Count();

                }

                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                ResultadoConsulta.Usuario,
                ResultadoConsulta.Clase, objeto.Codigo, JsonConvert.SerializeObject(objeto), "", "");
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

        public RespuestaConsulta<List<ReglaValidacion>> InsertarDatos(ReglaValidacion objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Constantes.Accion.Insertar;
                ResultadoConsulta.Usuario = user;
                objeto.UsuarioCreacion = user;
                objeto.idEstadoRegistro = (int)EstadosRegistro.EnProceso;

                DesencriptarReglasValidacion(objeto);

                ValidarObjetoRegla(objeto);

                var resul = clsDatos.ActualizarDatos(objeto);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();
         
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

        public RespuestaConsulta<List<ReglaValidacion>> ObtenerDatos(ReglaValidacion objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;
                if (!string.IsNullOrEmpty(objeto.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.id), out temp);
                    objeto.idReglaValidacion = temp;
                }
                var resul = clsDatos.ObtenerDatos(objeto);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();
            }
            catch (Exception ex)
            {
                ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                ResultadoConsulta.MensajeError = ex.Message;

            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<string>> ValidarDatos(ReglaValidacion objeto)
        {
            RespuestaConsulta<List<string>> resultado = new RespuestaConsulta<List<string>>();
            try
            {
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                var resul = clsDatos.ValidarDatos(objeto);
                resultado.objetoRespuesta = resul;
                resultado.CantidadRegistros = resul.Count();
            }
            catch (Exception ex)
            {
                resultado.HayError = (int)Error.ErrorSistema;
                resultado.MensajeError = ex.Message;

            }
            return resultado;
        }

        RespuestaConsulta<List<ReglaValidacion>> IMetodos<ReglaValidacion>.ValidarDatos(ReglaValidacion objeto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Autor: Francisco Vindas Ruiz
        /// Fecha: 06/01/2023
        /// Metodo para desencriptar los datos del Objeto regla de validación 
        /// </summary>
        /// <param name="objeto"></param>
        private void DesencriptarReglasValidacion(ReglaValidacion objeto)
        {
            if (!string.IsNullOrEmpty(objeto.id))
            {
                objeto.id = Utilidades.Desencriptar(objeto.id);
                int temp;
                if (int.TryParse(objeto.id, out temp))
                {
                    objeto.idReglaValidacion = temp;
                }
            }

            if (!string.IsNullOrEmpty(objeto.idIndicadorString))
            {
                objeto.idIndicadorString = Utilidades.Desencriptar(objeto.idIndicadorString);
                int temp;
                if (int.TryParse(objeto.idIndicadorString, out temp))
                {
                    objeto.idIndicador = temp;
                }
            }
        }

        /// <summary>
        /// Fecha: 10-02-2023
        /// Autor: Francisco Vindas Ruiz
        /// Metodo que funciona para validar que el objeto regla cumpla con las validaciones correspondientes
        /// </summary>
        /// <param name="objeto"></param>
        private void ValidarObjetoRegla(ReglaValidacion objeto)
        {

            var BuscarDatos = clsDatos.ObtenerDatos(new ReglaValidacion());

            if (BuscarDatos.Where(x => x.idReglaValidacion != objeto.idReglaValidacion && x.Codigo.ToUpper() == objeto.Codigo.ToUpper() && x.idEstadoRegistro != 4).Count() > 0)
            {
                ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                throw new Exception(Errores.CodigoRegistrado);
            }

            if (BuscarDatos.Where(x => x.idReglaValidacion != objeto.idReglaValidacion && x.Nombre.ToUpper() == objeto.Nombre.ToUpper() && x.idEstadoRegistro != 4).Count() > 0)
            {
                ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                throw new Exception(Errores.NombreRegistrado);
            }

            if (objeto.Codigo == null || string.IsNullOrEmpty(objeto.Codigo.Trim()))
            {
                ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                throw new Exception(string.Format(Errores.CampoConValorInvalido, EtiquetasViewReglasValidacion.Codigo));
            }

            if (!Utilidades.rx_alfanumerico.Match(objeto.Codigo).Success || objeto.Codigo.Trim().Length > 30)
            {
                ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                throw new Exception(string.Format(Errores.CampoConFormatoInvalido, EtiquetasViewReglasValidacion.Codigo));
            }

            if (objeto.Nombre == null || string.IsNullOrEmpty(objeto.Nombre.Trim()))
            {
                ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                throw new Exception(string.Format(Errores.CampoConValorInvalido, EtiquetasViewReglasValidacion.Nombre));
            }

            if (!Utilidades.rx_alfanumerico.Match(objeto.Nombre).Success || objeto.Nombre.Trim().Length > 500)
            {
                ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                throw new Exception(string.Format(Errores.CampoConFormatoInvalido, EtiquetasViewReglasValidacion.Nombre));
            }

            if (!string.IsNullOrEmpty(objeto.Descripcion?.Trim())) // ¿se ingresó el dato?
            {
                if (!Utilidades.rx_alfanumerico.Match(objeto.Descripcion).Success          // la descripción solo debe contener texto como valor
                    || objeto.Descripcion.Trim().Length > 3000)                         // validar la cantidad de caracteres
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                    throw new Exception(string.Format(Errores.CampoConValorInvalido, EtiquetasViewReglasValidacion.Descripcion));
                }
            }

            if (objeto.Descripcion == null)
            {
                objeto.Descripcion = "";
            }



        }
    }
}
