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
        private string SerializarObjetoBitacora(ReglaValidacion objRegla)
        {
            return JsonConvert.SerializeObject(objRegla, new JsonSerializerSettings
            { ContractResolver = new JsonIgnoreResolver(objRegla.NoSerialize) });
        }

        public RespuestaConsulta<List<ReglaValidacion>> ActualizarElemento(ReglaValidacion objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Constantes.Accion.Editar;
                ResultadoConsulta.Usuario = user;
                objeto.UsuarioModificacion = user;

                DesencriptarReglasValidacion(objeto);

                var resul = clsDatos.ObtenerDatos(new ReglaValidacion());

                if (resul.Where(x => x.idRegla == objeto.idRegla).Count() == 0)
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }
                else if (resul.Where(x => x.idRegla != objeto.idRegla && x.Nombre.ToUpper() == objeto.Nombre.ToUpper()).Count() > 0)
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                    throw new Exception(Errores.NombreRegistrado);
                }
                else
                {
                    clsDatos.ActualizarDatos(objeto);
                    ResultadoConsulta.objetoRespuesta = resul;
                    ResultadoConsulta.CantidadRegistros = resul.Count();

                }

            }
            catch (Exception ex)
            {
                ResultadoConsulta.MensajeError = ex.Message;

                if (ex.Message == Errores.NoRegistrosActualizar || ex.Message == Errores.NombreRegistrado)
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
                        objeto.idRegla = temp;
                    }
                }

                var resul = clsDatos.ObtenerDatos(objeto).ToList();

                if (resul.Count() == 0)
                {
                    throw new Exception(Errores.NoRegistrosActualizar);
                }
                else
                {
                    objeto = resul.Single();
                    objeto.idEstado = nuevoEstado;
                    objeto.UsuarioModificacion = ResultadoConsulta.Usuario;
                    resul = clsDatos.ActualizarDatos(objeto);
                    ResultadoConsulta.objetoRespuesta = resul;
                    ResultadoConsulta.CantidadRegistros = resul.Count();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == Errores.NoRegistrosActualizar)
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                }
                else
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                }

                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<ReglaValidacion>> ClonarDatos(ReglaValidacion objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<ReglaValidacion> ClonarDetallesReglas(string pIdReglaAClonar, string pIdReglaDestino)
        {
            RespuestaConsulta<ReglaValidacion> resultado = new RespuestaConsulta<ReglaValidacion>();

            int IdReglaAClonar, IdReglaDestino;

            try
            {
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

                resultado.Usuario = user;
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Clonar;

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
                    objeto.idEstado = (int)Constantes.EstadosRegistro.Eliminado;
                    resul = clsDatos.ActualizarDatos(objeto);
                    ResultadoConsulta.objetoRespuesta = resul;
                    ResultadoConsulta.CantidadRegistros = resul.Count();

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

        public RespuestaConsulta<List<ReglaValidacion>> InsertarDatos(ReglaValidacion objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Constantes.Accion.Insertar;
                ResultadoConsulta.Usuario = user;
                objeto.UsuarioCreacion = user;
                objeto.idEstado = (int)EstadosRegistro.EnProceso;

                var BuscarDatos = clsDatos.ObtenerDatos(new ReglaValidacion());

                DesencriptarReglasValidacion(objeto);

                if (BuscarDatos.Where(x => x.idRegla != objeto.idRegla && x.Codigo.ToUpper() == objeto.Codigo.ToUpper() && x.idEstado != 4).Count() > 0){
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                    throw new Exception(Errores.CodigoRegistrado);
                }

                if(BuscarDatos.Where(x=> x.idRegla != objeto.idRegla && x.Nombre.ToUpper() == objeto.Nombre.ToUpper() && x.idEstado != 4).Count() > 0)
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                    throw new Exception(Errores.NombreRegistrado);
                }
                
                var resul = clsDatos.ActualizarDatos(objeto);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();
                

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
                    objeto.idRegla = temp;
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

        private void DesencriptarReglasValidacion(ReglaValidacion objeto)
        {
            if (!string.IsNullOrEmpty(objeto.id))
            {
                objeto.id = Utilidades.Desencriptar(objeto.id);
                int temp;
                if (int.TryParse(objeto.id, out temp))
                {
                    objeto.idRegla = temp;
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
    }
}
