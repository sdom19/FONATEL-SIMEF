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
        private readonly ReglaValidacionAtributosValidosDAL clsReglaValidacionAtributosValidosDAL;

        private RespuestaConsulta<List<DetalleReglaValidacion>> ResultadoConsulta;
        string modulo = Etiquetas.ReglasValidacion;
        string user;

        public DetalleReglaValidacionBL(string modulo, string user)
        {
            this.modulo = modulo;
            this.user = user;
            this.clsDatos = new DetalleReglasValicionDAL();
            this.clsDatosIndicadorVariable = new DetalleIndicadorVariablesDAL();
            this.clsReglaValidacionAtributosValidosDAL = new ReglaValidacionAtributosValidosDAL();
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
                    int IdReglasValidacionTipo = objeto.IdReglasValidacionTipo;
                    int IdRegla = objeto.IdRegla;
                    int IdOperador = objeto.IdOperador;
                    var resul = clsDatos.ObtenerDatos(new DetalleReglaValidacion());
                    //string valorAnterior = SerializarObjetoBitacora(resul.Where(x => x.IdReglasValidacionTipo == objeto.IdReglasValidacionTipo).Single());
                    objeto = resul.Where(x => x.IdRegla == objeto.IdRegla).Single();
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

                if (ResultadoConsulta.HayError != (int)Constantes.Error.ErrorControlado)
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

        private void AgregarTipoDetalleReglaValidacion(DetalleReglaValidacion objeto)
        {
            switch (objeto.IdTipo)
            {
                case (int)Constantes.TipoReglasDetalle.FormulaContraAtributosValidos:
                    clsReglaValidacionAtributosValidosDAL.ActualizarDatos(objeto.ReglaAtributosValidos);
                    break;
                default:
                    break;
            }
        }


        public RespuestaConsulta<List<DetalleReglaValidacion>> InsertarDatos(DetalleReglaValidacion objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Constantes.Accion.Insertar;
                ResultadoConsulta.Usuario = user;
                objeto.IdOperador = objeto.IdOperador;
                objeto.IdRegla = objeto.IdRegla;
                objeto.Estado = true;
                //List<DetalleIndicadorVariables> listaDetallesIndicadorVariable = clsDatosIndicadorVariable.ObtenerDatos(new DetalleIndicadorVariables(){idDetalleIndicador = objeto.idIndicadorVariable});
                //bjeto.idIndicadorString = listaDetallesIndicadorVariable[0].idIndicadorString;
                DesencriptarObjReglasValidacion(objeto);
                var resul = clsDatos.ActualizarDatos(objeto);
                //AgregarTipoDetalleReglaValidacion(objeto);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();

            }
            catch (Exception ex)
            {
                if (ResultadoConsulta.HayError != (int)Constantes.Error.ErrorControlado)
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
                detalleReglaValidacion.IdRegla = temp;
            }
            if (!string.IsNullOrEmpty(detalleReglaValidacion.idReglasValidacionTipoString))
            {
                int.TryParse(Utilidades.Desencriptar(detalleReglaValidacion.idReglasValidacionTipoString), out int temp);
                detalleReglaValidacion.IdReglasValidacionTipo = temp;
            }

        }
    }
}
