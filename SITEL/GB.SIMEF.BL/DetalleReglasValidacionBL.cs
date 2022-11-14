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
        private readonly DetalleIndicadorVariablesDAL clsDatosIndicadorVariableDAL;
        private readonly ReglaValidacionAtributosValidosDAL clsReglaValidacionAtributosValidosDAL;
        private readonly ReglaComparacionConstanteDAL clsReglaComparacionConstanteDAL;
        private readonly ReglaSecuencialDAL clsReglaSecuencialDAL;
        private readonly ReglaIndicadorSalidaDAL clsReglaIndicadorSalidaDAL;
        private readonly ReglaIndicadorEntradaDAL clsReglaIndicadorEntradaDAL;
        private readonly ReglaIndicadorEntradaSalidaDAL clsReglaIndicadorEntradaSalidaDAL;

        private RespuestaConsulta<List<DetalleReglaValidacion>> ResultadoConsulta;
        string modulo = Etiquetas.ReglasValidacion;
        string user;

        public DetalleReglaValidacionBL(string modulo, string user)
        {
            this.modulo = modulo;
            this.user = user;
            this.clsDatos = new DetalleReglasValicionDAL();
            this.clsDatosIndicadorVariableDAL = new DetalleIndicadorVariablesDAL();
            this.clsReglaValidacionAtributosValidosDAL = new ReglaValidacionAtributosValidosDAL();
            this.clsReglaComparacionConstanteDAL = new ReglaComparacionConstanteDAL();
            this.clsReglaSecuencialDAL = new ReglaSecuencialDAL();
            this.clsReglaIndicadorSalidaDAL = new ReglaIndicadorSalidaDAL();
            this.clsReglaIndicadorEntradaDAL = new ReglaIndicadorEntradaDAL();
            this.clsReglaIndicadorEntradaSalidaDAL = new ReglaIndicadorEntradaSalidaDAL();
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
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Constantes.Accion.Editar;
                ResultadoConsulta.Usuario = user;

                objeto.IdOperador = objeto.IdOperador;
                objeto.IdRegla = objeto.IdRegla;
                objeto.Estado = true;

                DesencriptarObjReglasValidacion(objeto);

                var BuscarDatos = clsDatos.ObtenerDatos(new DetalleReglaValidacion());

                if (BuscarDatos.Where(x => x.IdRegla == objeto.IdRegla && x.IdDetalleReglaValidacion != objeto.IdDetalleReglaValidacion && x.IdTipo == objeto.IdTipo && x.IdTipo != (int)Constantes.TipoReglasDetalle.FormulaContraAtributosValidos && x.IdDetalleIndicador == objeto.IdDetalleIndicador && x.Estado == true).Count() > 0)
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                    throw new Exception(Errores.ReglasVariableIngresada);
                }
                if (BuscarDatos.Where(x => x.IdRegla == objeto.IdRegla && x.IdDetalleReglaValidacion != objeto.IdDetalleReglaValidacion && x.IdTipo == objeto.IdTipo && x.IdTipo == (int)Constantes.TipoReglasDetalle.FormulaContraAtributosValidos && x.reglaAtributosValidos.IdCategoria == objeto.reglaAtributosValidos.IdCategoria && x.Estado == true).Count() > 0)
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;

                    throw new Exception(Errores.ReglasVariableIngresada);
                }
                else
                {

                    var resul = clsDatos.ActualizarDatos(objeto);
                    objeto.IdDetalleReglaValidacion = resul.Single().IdDetalleReglaValidacion;
                    AgregarTipoDetalleReglaValidacion(objeto);
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

                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Usuario = user;

                if (!String.IsNullOrEmpty(objeto.id) || !String.IsNullOrEmpty(objeto.idDetalleReglaString))
                {
                    DesencriptarObjReglasValidacion(objeto);
                }

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
                objeto.Estado = true;

                DesencriptarObjReglasValidacion(objeto);

                var BuscarDatos = clsDatos.ObtenerDatos(new DetalleReglaValidacion());

                if (BuscarDatos.Where(x => x.IdRegla == objeto.IdRegla && x.IdTipo == objeto.IdTipo && x.IdTipo != (int)Constantes.TipoReglasDetalle.FormulaContraAtributosValidos && x.IdDetalleIndicador == objeto.IdDetalleIndicador && x.Estado == true).Count() > 0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.ReglasVariableIngresada);
                }

                if (objeto.reglaAtributosValidos != null)
                {
                    if (BuscarDatos.Where(x => x.IdRegla == objeto.IdRegla && x.IdTipo == objeto.IdTipo && x.reglaAtributosValidos.IdCategoria == objeto.reglaAtributosValidos.IdCategoria && x.Estado == true).Count() > 0)
                    {
                        ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                        throw new Exception(Errores.ReglasCategoriaIngresada);
                    }
                }

                var resul = clsDatos.ActualizarDatos(objeto);
                objeto.IdDetalleReglaValidacion = resul.Single().IdDetalleReglaValidacion;
                AgregarTipoDetalleReglaValidacion(objeto);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();


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

        private void AgregarTipoDetalleReglaValidacion(DetalleReglaValidacion objeto)
        {
            switch (objeto.IdTipo)
            {
                case (int)Constantes.TipoReglasDetalle.FormulaContraOtroIndicadorEntrada:
                    DesencriptarReglaComparacionEntrada(objeto);
                    objeto.reglaIndicadorEntrada.IdDetalleReglaValidacion = objeto.IdDetalleReglaValidacion;
                    clsReglaIndicadorEntradaDAL.ActualizarDatos(objeto.reglaIndicadorEntrada);
                    break;

                case (int)Constantes.TipoReglasDetalle.FormulaContraOtroIndicadorEntradaSalida:
                    DesencriptarReglaComparacionEntradaSalida(objeto);
                    objeto.reglaIndicadorEntradaSalida.IdDetalleReglaValidacion = objeto.IdDetalleReglaValidacion;
                    clsReglaIndicadorEntradaSalidaDAL.ActualizarDatos(objeto.reglaIndicadorEntradaSalida);
                    break;

                case (int)Constantes.TipoReglasDetalle.FormulaContraOtroIndicadorSalida:
                    DesencriptarReglaComparacionSalida(objeto);
                    objeto.reglaIndicadorSalida.IdDetalleReglaValidacion = objeto.IdDetalleReglaValidacion;
                    clsReglaIndicadorSalidaDAL.ActualizarDatos(objeto.reglaIndicadorSalida);
                    break;

                case (int)Constantes.TipoReglasDetalle.FormulaContraConstante:
                    objeto.reglaComparacionConstante.IdDetalleReglaValidacion = objeto.IdDetalleReglaValidacion;
                    clsReglaComparacionConstanteDAL.ActualizarDatos(objeto.reglaComparacionConstante);
                    break;

                case (int)Constantes.TipoReglasDetalle.FormulaContraAtributosValidos:
                    List<string> listaAtributos = objeto.reglaAtributosValidos.idAtributoString.Split(',').ToList();

                    foreach (var item in listaAtributos)
                    {
                        objeto.reglaAtributosValidos.idAtributoString = item;
                        DesencriptarReglaAtributosValidos(objeto);
                        objeto.reglaAtributosValidos.IdDetalleReglaValidacion = objeto.IdDetalleReglaValidacion;
                        clsReglaValidacionAtributosValidosDAL.ActualizarDatos(objeto.reglaAtributosValidos);
                    }



                    break;

                case (int)Constantes.TipoReglasDetalle.FormulaActualizacionSecuencial:
                    objeto.reglaSecuencial.IdDetalleReglaValidacion = objeto.IdDetalleReglaValidacion;
                    clsReglaSecuencialDAL.ActualizarDatos(objeto.reglaSecuencial);
                    break;

                default:
                    break;
            }
        }

        private void DesencriptarObjReglasValidacion(DetalleReglaValidacion detalleReglaValidacion)
        {
            if (!string.IsNullOrEmpty(detalleReglaValidacion.id))
            {
                int.TryParse(Utilidades.Desencriptar(detalleReglaValidacion.id), out int temp);
                detalleReglaValidacion.IdRegla = temp;
            }
            if (!string.IsNullOrEmpty(detalleReglaValidacion.idDetalleReglaString))
            {
                int.TryParse(Utilidades.Desencriptar(detalleReglaValidacion.idDetalleReglaString), out int temp);
                detalleReglaValidacion.IdDetalleReglaValidacion = temp;
            }
            if (!string.IsNullOrEmpty(detalleReglaValidacion.idIndicadorVariableString))
            {
                int.TryParse(Utilidades.Desencriptar(detalleReglaValidacion.idIndicadorVariableString), out int temp);
                detalleReglaValidacion.IdDetalleIndicador = temp;
            }

            if (!string.IsNullOrEmpty(detalleReglaValidacion.idIndicadorString))
            {
                int.TryParse(Utilidades.Desencriptar(detalleReglaValidacion.idIndicadorString), out int temp);
                detalleReglaValidacion.IdIndicador = temp;
            }
        }

        private void DesencriptarReglaComparacionEntrada(DetalleReglaValidacion objeto)
        {
            if (!string.IsNullOrEmpty(objeto.reglaIndicadorEntrada.idIndicadorComparaString))
            {
                int.TryParse(Utilidades.Desencriptar(objeto.reglaIndicadorEntrada.idIndicadorComparaString), out int temp);
                objeto.reglaIndicadorEntrada.IdIndicador = temp;
            }

            if (!string.IsNullOrEmpty(objeto.reglaIndicadorEntrada.idVariableComparaString))
            {
                int.TryParse(Utilidades.Desencriptar(objeto.reglaIndicadorEntrada.idVariableComparaString), out int temp);
                objeto.reglaIndicadorEntrada.IdDetalleIndicador = temp;
            }

        }

        private void DesencriptarReglaComparacionEntradaSalida(DetalleReglaValidacion objeto)
        {
            if (!string.IsNullOrEmpty(objeto.reglaIndicadorEntradaSalida.idIndicadorComparaString))
            {
                int.TryParse(Utilidades.Desencriptar(objeto.reglaIndicadorEntradaSalida.idIndicadorComparaString), out int temp);
                objeto.reglaIndicadorEntradaSalida.IdIndicador = temp;
            }

            if (!string.IsNullOrEmpty(objeto.reglaIndicadorEntradaSalida.idVariableComparaString))
            {
                int.TryParse(Utilidades.Desencriptar(objeto.reglaIndicadorEntradaSalida.idVariableComparaString), out int temp);
                objeto.reglaIndicadorEntradaSalida.IdDetalleIndicador = temp;
            }

        }

        private void DesencriptarReglaComparacionSalida(DetalleReglaValidacion objeto)
        {
            if (!string.IsNullOrEmpty(objeto.reglaIndicadorSalida.idIndicadorComparaString))
            {
                int.TryParse(Utilidades.Desencriptar(objeto.reglaIndicadorSalida.idIndicadorComparaString), out int temp);
                objeto.reglaIndicadorSalida.IdIndicador = temp;
            }

        }

        private void DesencriptarReglaAtributosValidos(DetalleReglaValidacion objeto)
        {
            if (!string.IsNullOrEmpty(objeto.reglaAtributosValidos.idAtributoString))
            {
                int.TryParse(Utilidades.Desencriptar(objeto.reglaAtributosValidos.idAtributoString), out int temp);
                objeto.reglaAtributosValidos.IdCategoriaAtributo = temp;
            }

        }

    }
}
