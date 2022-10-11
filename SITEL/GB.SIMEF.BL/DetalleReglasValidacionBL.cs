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
    public class DetalleReglaValidacionBL : IMetodos<DetalleReglaValidacion>
    {
        private readonly DetalleReglasValicionDAL clsDatos;
        private readonly DetalleIndicadorVariablesDAL clsDatosIndicadorVariable;
        private RespuestaConsulta<List<DetalleReglaValidacion>> ResultadoConsulta;
        string modulo = Etiquetas.ReglasValidacion;
        string user;

        public DetalleReglaValidacionBL(string modulo, string user)
        {
            this.modulo = modulo;
            this.user = user;
            clsDatos = new DetalleReglasValicionDAL();
            clsDatosIndicadorVariable = new DetalleIndicadorVariablesDAL();
            ResultadoConsulta = new RespuestaConsulta<List<DetalleReglaValidacion>>();
        }

        private string SerializarObjetoBitacora(DetalleReglaValidacion objRegla)
        {
            return JsonConvert.SerializeObject(objRegla, new JsonSerializerSettings
            { ContractResolver = new JsonIgnoreResolver(objRegla.NoSerialize) });
        }

        public RespuestaConsulta<List<DetalleReglaValidacion>> ActualizarElemento(DetalleReglaValidacion objeto)
        {
            try
            {
                if (!string.IsNullOrEmpty(objeto.id))
                {
                    DesencriptarObjReglasValidacion(objeto);

                    ResultadoConsulta.Clase = modulo;
                    ResultadoConsulta.Accion = (int)Constantes.Accion.Editar;
                    ResultadoConsulta.Usuario = user;

                    int IdReglasValidacionTipo = objeto.idReglasValidacionTipo;
                    int IdRegla = objeto.idRegla;
                    int IdOperador = objeto.IdOperador;
                    int Indicador = objeto.IdIndicador;
                    int idindicadorVariable = objeto.idIndicadorVariable;

                    var resul = clsDatos.ObtenerDatos(new DetalleReglaValidacion());
                    //string valorAnterior = SerializarObjetoBitacora(resul.Where(x => x.idReglasValidacionTipo == objeto.idReglasValidacionTipo).Single());
                    objeto = resul.Where(x => x.idRegla == objeto.idRegla).Single();


                    objeto.idIndicadorVariable = idindicadorVariable;
                    objeto.IdIndicador = Indicador;
                    objeto.IdOperador = IdOperador;

                    clsDatos.ActualizarDatos(objeto);

                    var nuevoValor = clsDatos.ObtenerDatos(objeto).Single();

                    string jsonNuevoValor = SerializarObjetoBitacora(nuevoValor);

                    ResultadoConsulta.objetoRespuesta = resul;
                    ResultadoConsulta.CantidadRegistros = resul.Count();
                    //clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                    //       ResultadoConsulta.Usuario,
                    //       ResultadoConsulta.Clase, nuevovalor.Codigo, jsonNuevoValor, valorAnterior);

                }
            }
            catch (Exception ex)
            {

                if (ex.Message == Errores.NoRegistrosActualizar || ex.Message == Errores.ReglaRegistrada)
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

        public RespuestaConsulta<List<DetalleReglaValidacion>> CambioEstado(DetalleReglaValidacion objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleReglaValidacion>> ClonarDatos(DetalleReglaValidacion objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleReglaValidacion>> EliminarElemento(DetalleReglaValidacion objeto)
        {
            try
            {
                if (!String.IsNullOrEmpty(objeto.id) || !String.IsNullOrEmpty(objeto.idReglasValidacionTipoString))
                {
                    DesencriptarObjReglasValidacion(objeto);
                }
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Usuario = user;
                var resul = clsDatos.ObtenerDatos(objeto).ToList();

                if (resul.Count() == 0)
                {
                    throw new Exception(Errores.NoRegistrosActualizar);
                }
                else
                {
                    objeto = resul.Single();
                    objeto.Estado = false;
                    ResultadoConsulta.Accion = (int)Constantes.Accion.Eliminar;
                    resul = clsDatos.ActualizarDatos(objeto);
                    ResultadoConsulta.objetoRespuesta = resul;
                    ResultadoConsulta.CantidadRegistros = resul.Count();

                    //clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                    //       ResultadoConsulta.Usuario,
                    //       ResultadoConsulta.Clase, objeto.);
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

        public RespuestaConsulta<List<DetalleReglaValidacion>> InsertarDatos(DetalleReglaValidacion objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Constantes.Accion.Insertar;
                ResultadoConsulta.Usuario = user;

                DesencriptarObjReglasValidacion(objeto);

                objeto.idIndicadorVariable = objeto.idIndicadorVariable;
                objeto.IdOperador = objeto.IdOperador;
                objeto.idRegla = objeto.idRegla;
                objeto.Estado = true;
                List<DetalleIndicadorVariables> listaDetallesIndicadorVariable = clsDatosIndicadorVariable.ObtenerDatos(new DetalleIndicadorVariables(){idDetalleIndicador = objeto.idIndicadorVariable});
                if (listaDetallesIndicadorVariable.Count < 1)
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                    throw new Exception();
                }
                objeto.idIndicadorString = listaDetallesIndicadorVariable[0].idIndicadorString;
                DesencriptarObjReglasValidacion(objeto);
                var resul = clsDatos.ActualizarDatos(objeto);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();
                //string JsonNuevoValor = SerializarObjetoBitacora(resul.Where(x => x.Codigo == objeto.Codigo.ToUpper()).Single());
                //clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                //     ResultadoConsulta.Usuario,
                //     ResultadoConsulta.Clase, objeto.Codigo, "", "", JsonNuevoValor);
            }
            catch (Exception ex)
            {
                if (ex.Message == Errores.ReglaRegistrada)
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

        public RespuestaConsulta<List<DetalleReglaValidacion>> ObtenerDatos(DetalleReglaValidacion objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;

                DesencriptarObjReglasValidacion(objeto);

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

        public RespuestaConsulta<List<string>> ValidarDatos(DetalleReglaValidacion objeto)
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

        RespuestaConsulta<List<DetalleReglaValidacion>> IMetodos<DetalleReglaValidacion>.ValidarDatos(DetalleReglaValidacion objeto)
        {
            throw new NotImplementedException();
        }

        private void DesencriptarObjReglasValidacion(DetalleReglaValidacion detalleReglaValidacion)
        {
            if (!string.IsNullOrEmpty(detalleReglaValidacion.id))
            {
                int.TryParse(Utilidades.Desencriptar(detalleReglaValidacion.id), out int temp);
                detalleReglaValidacion.idRegla = temp;
            }
            if (!string.IsNullOrEmpty(detalleReglaValidacion.idIndicadorVariableString))
            {
                int.TryParse(Utilidades.Desencriptar(detalleReglaValidacion.idIndicadorVariableString), out int temp);
                detalleReglaValidacion.idIndicadorVariable = temp;
            }
            if (!string.IsNullOrEmpty(detalleReglaValidacion.idIndicadorString))
            {
                int.TryParse(Utilidades.Desencriptar(detalleReglaValidacion.idIndicadorString), out int temp);
                detalleReglaValidacion.IdIndicador = temp;
            }
            if (!string.IsNullOrEmpty(detalleReglaValidacion.idReglasValidacionTipoString))
            {
                int.TryParse(Utilidades.Desencriptar(detalleReglaValidacion.idReglasValidacionTipoString), out int temp);
                detalleReglaValidacion.idReglasValidacionTipo = temp;
            }

        }
    }
}
