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
        private readonly IndicadorFonatelDAL clsIndicadorDAL;
        private readonly CategoriasDesagregacionDAL clsCategoriaDal;
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
            this.clsIndicadorDAL = new IndicadorFonatelDAL();
            this.clsCategoriaDal = new CategoriasDesagregacionDAL();
            this.clsDatosIndicadorVariableDAL = new DetalleIndicadorVariablesDAL();
            ResultadoConsulta = new RespuestaConsulta<List<DetalleReglaValidacion>>();
        }

       /// <summary>
       /// Obtiene la Categoria o indicador según el tipo;
       /// Michael Hernández Cordero
       /// </summary>
       /// <param name="objeto"></param>
       /// <returns></returns>
        private DetalleReglaValidacion ObtenerCategoriaIndicadorTipo(DetalleReglaValidacion objDetalleRegla)
        {
            switch (objDetalleRegla.idTipoReglaValidacion)
            {
                case (int)Constantes.TipoReglasDetalle.FormulaActualizacionSecuencial:
                    objDetalleRegla.CategoriaDesagregacion = clsCategoriaDal
                        .ObtenerDatos(new CategoriaDesagregacion()
                        {
                            idCategoriaDesagregacion =
                        objDetalleRegla.reglaSecuencial.idCategoriaDesagregacion
                        }).FirstOrDefault();
                    break;
                case (int)Constantes.TipoReglasDetalle.FormulaContraAtributosValidos:
                    string[] listaAtributosId = objDetalleRegla.reglaAtributoValido.idAtributoString.Split(',');
                    List<string> categoriaAtributo = new List<string>();

                    objDetalleRegla.CategoriaDesagregacion = clsCategoriaDal.ObtenerDatos(new CategoriaDesagregacion()
                    { idCategoriaDesagregacion = objDetalleRegla.reglaAtributoValido.idCategoriaDesagregacion }
                    ).FirstOrDefault();
                    foreach (var item in listaAtributosId)
                    {
                        string NombreCategoria = clsCategoriaDal.ObtenerDatos(
                            new CategoriaDesagregacion() { idCategoriaDesagregacion = Convert.ToInt32(item) })
                            .Select(X => X.NombreCategoria).FirstOrDefault();
                        categoriaAtributo.Add(NombreCategoria);
                    }

                    objDetalleRegla.AtributosValidos = string.Join(",", categoriaAtributo);

                    break;
                case (int)Constantes.TipoReglasDetalle.FormulaContraOtroIndicadorEntrada:

                    objDetalleRegla.Indicador =
                        clsIndicadorDAL
                        .ObtenerDatos(new Indicador() { IdIndicador= objDetalleRegla.reglaIndicadorEntrada.idIndicador }).FirstOrDefault();

                    objDetalleRegla.IndicadorVariable = clsDatosIndicadorVariableDAL.
                        ObtenerDatos(new DetalleIndicadorVariable() { IdIndicador = objDetalleRegla.reglaIndicadorEntrada.idIndicador, IdDetalleIndicadorVariable = objDetalleRegla.reglaIndicadorEntrada.idDetalleIndicadorVariable })
                    .FirstOrDefault();

                    break;
                case (int)Constantes.TipoReglasDetalle.FormulaContraOtroIndicadorEntradaSalida:

                    objDetalleRegla.Indicador =
                        clsIndicadorDAL
                        .ObtenerDatos(new Indicador() { IdIndicador = objDetalleRegla.reglaIndicadorEntradaSalida.idIndicador }).FirstOrDefault();
                    objDetalleRegla.IndicadorVariable = clsDatosIndicadorVariableDAL.
                        ObtenerDatos(new DetalleIndicadorVariable() { IdIndicador = objDetalleRegla.reglaIndicadorEntradaSalida.idIndicador, IdDetalleIndicadorVariable= objDetalleRegla.reglaIndicadorEntradaSalida.idDetalleIndicadorVariable })
                        .FirstOrDefault();

                    break;
                case (int)Constantes.TipoReglasDetalle.FormulaContraOtroIndicadorSalida:

                    objDetalleRegla.Indicador =
                        clsIndicadorDAL
                        .ObtenerDatos(new Indicador() { IdIndicador = objDetalleRegla.reglaIndicadorSalida.idIndicador }).FirstOrDefault();

                    objDetalleRegla.IndicadorVariable = clsDatosIndicadorVariableDAL.
                        ObtenerDatos(new DetalleIndicadorVariable() { IdIndicador = objDetalleRegla.reglaIndicadorSalida.idIndicador, IdDetalleIndicadorVariable = objDetalleRegla.reglaIndicadorSalida.idDetalleIndicadorVariable })
                        .FirstOrDefault();

                    break;
            }
            return objDetalleRegla;
        }


        public RespuestaConsulta<List<DetalleReglaValidacion>> ActualizarElemento(DetalleReglaValidacion objDetalleReglaValidacion)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Constantes.Accion.Editar;
                ResultadoConsulta.Usuario = user;

                objDetalleReglaValidacion.idOperadorAritmetico = objDetalleReglaValidacion.idOperadorAritmetico;
                objDetalleReglaValidacion.idReglaValidacion = objDetalleReglaValidacion.idReglaValidacion;
                objDetalleReglaValidacion.Estado = true;

                DesencriptarObjReglasValidacion(objDetalleReglaValidacion);

                var BuscarDatos = clsDatos.ObtenerDatos(new DetalleReglaValidacion() { idReglaValidacion = objDetalleReglaValidacion.idReglaValidacion,
                    idTipoReglaValidacion=objDetalleReglaValidacion.idTipoReglaValidacion });

                if (BuscarDatos.Where(x => x.idReglaValidacion == objDetalleReglaValidacion.idReglaValidacion 
                && x.idDetalleReglaValidacion != objDetalleReglaValidacion.idDetalleReglaValidacion  && x.idTipoReglaValidacion != (int)Constantes.TipoReglasDetalle.FormulaContraAtributosValidos && x.idTipoReglaValidacion != (int)Constantes.TipoReglasDetalle.FormulaActualizacionSecuencial && x.idDetalleIndicadorVariable == objDetalleReglaValidacion.idDetalleIndicadorVariable && x.Estado == true).Count() > 0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.ReglasVariableIngresada);
                }

                if (objDetalleReglaValidacion.reglaAtributoValido!= null)
                {
                    if (BuscarDatos.Where(x=>x.idDetalleReglaValidacion != objDetalleReglaValidacion.idDetalleReglaValidacion 
                    && x.reglaAtributoValido.idCategoriaDesagregacion == objDetalleReglaValidacion.reglaAtributoValido.idCategoriaDesagregacion ).Count() > 0)
                    {
                        ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                        throw new Exception(Errores.ReglasCategoriaIngresada);
                    }
                }

                if (objDetalleReglaValidacion.reglaSecuencial != null)
                {
                    if (BuscarDatos.Where(x => x.idReglaValidacion == objDetalleReglaValidacion.idReglaValidacion && x.idDetalleReglaValidacion != objDetalleReglaValidacion.idDetalleReglaValidacion && x.idTipoReglaValidacion == objDetalleReglaValidacion.idTipoReglaValidacion && x.reglaSecuencial.idCategoriaDesagregacion == objDetalleReglaValidacion.reglaSecuencial.idCategoriaDesagregacion && x.Estado == true).Count() > 0)
                    {
                        ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                        throw new Exception(Errores.ReglasCategoriaIngresada);
                    }
                }

                DetalleReglaValidacion objDetalleReglaValidacionAnterior =
                    BuscarDatos.Where(x => x.idDetalleReglaValidacion == objDetalleReglaValidacion.idDetalleReglaValidacion).SingleOrDefault();

                objDetalleReglaValidacionAnterior = ObtenerCategoriaIndicadorTipo(objDetalleReglaValidacionAnterior);


               var resul = clsDatos.ActualizarDatos(objDetalleReglaValidacion);
                objDetalleReglaValidacion.idDetalleReglaValidacion = resul.Single().idDetalleReglaValidacion;
                AgregarTipoDetalleReglaValidacion(objDetalleReglaValidacion);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();

                objDetalleReglaValidacion = clsDatos.ObtenerDatos(resul.Single()).Single();
                objDetalleReglaValidacion = ObtenerCategoriaIndicadorTipo(objDetalleReglaValidacion);

                    
                string JsonActual = objDetalleReglaValidacion.ToString();
                string JsonAnterior = objDetalleReglaValidacionAnterior.ToString();

                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                        ResultadoConsulta.Usuario,
                            ResultadoConsulta.Clase, objDetalleReglaValidacion.idDetalleReglaValidacion.ToString()
                            , JsonActual, JsonAnterior, "");
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
                ResultadoConsulta.Accion = (int)Constantes.Accion.Eliminar;

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

        public RespuestaConsulta<List<DetalleReglaValidacion>> InsertarDatos(DetalleReglaValidacion objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Constantes.Accion.Crear;
                ResultadoConsulta.Usuario = user;
                objeto.Estado = true;

                DesencriptarObjReglasValidacion(objeto);

                if (objeto.reglaSecuencial != null && !string.IsNullOrEmpty(objeto.reglaSecuencial.idCategoriaString))
                {
                    int.TryParse(Utilidades.Desencriptar(objeto.reglaSecuencial.idCategoriaString), out int temp);
                    objeto.reglaSecuencial.idCategoriaDesagregacion = temp;
                }

                var BuscarDatos = clsDatos.ObtenerDatos(new DetalleReglaValidacion { idReglaValidacion = objeto.idReglaValidacion, Estado = true });

                if (BuscarDatos.Where(x => x.idReglaValidacion == objeto.idReglaValidacion && x.idTipoReglaValidacion == objeto.idTipoReglaValidacion && x.idTipoReglaValidacion != (int)Constantes.TipoReglasDetalle.FormulaContraAtributosValidos && x.idTipoReglaValidacion != (int)Constantes.TipoReglasDetalle.FormulaActualizacionSecuencial && x.idDetalleIndicadorVariable == objeto.idDetalleIndicadorVariable && x.Estado == true).Count() > 0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.ReglasVariableIngresada);
                }

                if (objeto.reglaAtributoValido != null)
                {
                    if (BuscarDatos.Where(x => x.idReglaValidacion == objeto.idReglaValidacion && x.idTipoReglaValidacion == objeto.idTipoReglaValidacion && x.reglaAtributoValido.idCategoriaDesagregacion == objeto.reglaAtributoValido.idCategoriaDesagregacion && x.Estado == true).Count() > 0)
                    {
                        ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                        throw new Exception(Errores.ReglasCategoriaIngresada);
                    }
                }

                if (objeto.reglaSecuencial != null)
                {
                    if (BuscarDatos.Where(x => x.idReglaValidacion == objeto.idReglaValidacion && x.idTipoReglaValidacion == objeto.idTipoReglaValidacion && x.reglaSecuencial.idCategoriaDesagregacion == objeto.reglaSecuencial.idCategoriaDesagregacion && x.Estado == true).Count() > 0)
                    {
                        ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                        throw new Exception(Errores.ReglasCategoriaIngresada);
                    }
                }

                var resul = clsDatos.ActualizarDatos(objeto);
                objeto.idDetalleReglaValidacion = resul.Single().idDetalleReglaValidacion;
                AgregarTipoDetalleReglaValidacion(objeto);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();

                DetalleReglaValidacion objetoDetalle = clsDatos.ObtenerDatos(resul.Single()).Single();
                objetoDetalle = ObtenerCategoriaIndicadorTipo(objetoDetalle);


                string jsonValorInicial = objetoDetalle.ToString();

                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                       ResultadoConsulta.Usuario,
                           ResultadoConsulta.Clase, objetoDetalle.idDetalleReglaValidacion.ToString(), "", "", jsonValorInicial);

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
            switch (objeto.idTipoReglaValidacion)
            {
                case (int)Constantes.TipoReglasDetalle.FormulaContraOtroIndicadorEntrada:
                    DesencriptarReglaComparacionEntrada(objeto);
                    objeto.reglaIndicadorEntrada.idDetalleReglaValidacion = objeto.idDetalleReglaValidacion;
                    clsReglaIndicadorEntradaDAL.ActualizarDatos(objeto.reglaIndicadorEntrada);
                    break;

                case (int)Constantes.TipoReglasDetalle.FormulaContraOtroIndicadorEntradaSalida:
                    DesencriptarReglaComparacionEntradaSalida(objeto);
                    objeto.reglaIndicadorEntradaSalida.idDetalleReglaValidacion = objeto.idDetalleReglaValidacion;
                    clsReglaIndicadorEntradaSalidaDAL.ActualizarDatos(objeto.reglaIndicadorEntradaSalida);
                    break;

                case (int)Constantes.TipoReglasDetalle.FormulaContraOtroIndicadorSalida:
                    DesencriptarReglaComparacionSalida(objeto);
                    objeto.reglaIndicadorSalida.idDetalleReglaValidacion = objeto.idDetalleReglaValidacion;
                    clsReglaIndicadorSalidaDAL.ActualizarDatos(objeto.reglaIndicadorSalida);
                    break;

                case (int)Constantes.TipoReglasDetalle.FormulaContraConstante:
                    objeto.reglaComparacionConstante.idDetalleReglaValidacion = objeto.idDetalleReglaValidacion;
                    clsReglaComparacionConstanteDAL.ActualizarDatos(objeto.reglaComparacionConstante);
                    break;

                case (int)Constantes.TipoReglasDetalle.FormulaContraAtributosValidos:
                    
                    
                    
                    List<string> listaAtributos = objeto.reglaAtributoValido.idAtributoString.Split(',').ToList();

                    objeto.reglaAtributoValido.OpcionEliminar = true;

                    foreach (var item in listaAtributos)
                    {
                        if (item!= "all")
                        {
                            objeto.reglaAtributoValido.idAtributoString = item;                            
                            
                            objeto.reglaAtributoValido.IdDetalleReglaValidacion = objeto.idDetalleReglaValidacion;

                            objeto.reglaAtributoValido.idDetalleCategoriaTexto = Convert.ToInt32(item);

                            clsReglaValidacionAtributosValidosDAL.ActualizarDatos(objeto.reglaAtributoValido);
                            objeto.reglaAtributoValido.OpcionEliminar = false;
                        }
            
                    }

                    break;

                case (int)Constantes.TipoReglasDetalle.FormulaActualizacionSecuencial:
                    objeto.reglaSecuencial.idDetalleReglaValidacion = objeto.idDetalleReglaValidacion;
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
                detalleReglaValidacion.idReglaValidacion = temp;
            }
            if (!string.IsNullOrEmpty(detalleReglaValidacion.idDetalleReglaString))
            {
                int.TryParse(Utilidades.Desencriptar(detalleReglaValidacion.idDetalleReglaString), out int temp);
                detalleReglaValidacion.idDetalleReglaValidacion = temp;
            }
            if (!string.IsNullOrEmpty(detalleReglaValidacion.idIndicadorVariableString))
            {
                int.TryParse(Utilidades.Desencriptar(detalleReglaValidacion.idIndicadorVariableString), out int temp);
                detalleReglaValidacion.idDetalleIndicadorVariable = temp;
            }

            if (!string.IsNullOrEmpty(detalleReglaValidacion.idIndicadorString))
            {
                int.TryParse(Utilidades.Desencriptar(detalleReglaValidacion.idIndicadorString), out int temp);
                detalleReglaValidacion.idIndicador = temp;
            }

            
        }

        private void DesencriptarReglaComparacionEntrada(DetalleReglaValidacion objeto)
        {
            if (!string.IsNullOrEmpty(objeto.reglaIndicadorEntrada.idIndicadorComparaString))
            {
                int.TryParse(Utilidades.Desencriptar(objeto.reglaIndicadorEntrada.idIndicadorComparaString), out int temp);
                objeto.reglaIndicadorEntrada.idIndicador = temp;
            }

            if (!string.IsNullOrEmpty(objeto.reglaIndicadorEntrada.idVariableComparaString))
            {
                int.TryParse(Utilidades.Desencriptar(objeto.reglaIndicadorEntrada.idVariableComparaString), out int temp);
                objeto.reglaIndicadorEntrada.idDetalleIndicadorVariable = temp;
            }

        }

        private void DesencriptarReglaComparacionEntradaSalida(DetalleReglaValidacion objeto)
        {
            if (!string.IsNullOrEmpty(objeto.reglaIndicadorEntradaSalida.idIndicadorComparaString))
            {
                int.TryParse(Utilidades.Desencriptar(objeto.reglaIndicadorEntradaSalida.idIndicadorComparaString), out int temp);
                objeto.reglaIndicadorEntradaSalida.idIndicador = temp;
            }

            if (!string.IsNullOrEmpty(objeto.reglaIndicadorEntradaSalida.idVariableComparaString))
            {
                int.TryParse(Utilidades.Desencriptar(objeto.reglaIndicadorEntradaSalida.idVariableComparaString), out int temp);
                objeto.reglaIndicadorEntradaSalida.idDetalleIndicadorVariable = temp;
            }

        }

        private void DesencriptarReglaComparacionSalida(DetalleReglaValidacion objeto)
        {
            if (!string.IsNullOrEmpty(objeto.reglaIndicadorSalida.idIndicadorComparaString))
            {
                int.TryParse(Utilidades.Desencriptar(objeto.reglaIndicadorSalida.idIndicadorComparaString), out int temp);
                objeto.reglaIndicadorSalida.idIndicador = temp;
            }

            if (!string.IsNullOrEmpty(objeto.reglaIndicadorSalida.idVariableComparaString))
            {
                int.TryParse(Utilidades.Desencriptar(objeto.reglaIndicadorSalida.idVariableComparaString), out int temp);
                objeto.reglaIndicadorSalida.idDetalleIndicadorVariable = temp;
            }

        }
    }
}
