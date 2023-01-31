using GB.SIMEF.BL.GestionCalculo;
using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Entities.DTO;
using GB.SIMEF.Resources;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL
{
    public class FormulasCalculoBL : IMetodos<FormulasCalculo>
    {
        readonly string modulo = "";
        readonly string user = "";
        private readonly FormulasCalculoDAL formulasCalculoDAL;
        private readonly IndicadorFonatelDAL indicadorFonatelDAL;
        private readonly FrecuenciaEnvioDAL frecuenciaEnvioDAL;
        private readonly DetalleIndicadorVariablesDAL detalleIndicadorVariablesDAL;
        private readonly FormulaNivelCalculoCategoriaDAL formulaNivelCalculoCategoriaDAL;
        private readonly FormulasVariableDatoCriterioDAL formulasVariableDatoCriterioDAL;
        private readonly FormulasDefinicionFechaDAL formulasDefinicionFechaDAL;

        public FormulasCalculoBL(string modulo, string user)
        {
            this.modulo = modulo;
            this.user = user;
            formulasCalculoDAL = new FormulasCalculoDAL();
            indicadorFonatelDAL = new IndicadorFonatelDAL();
            frecuenciaEnvioDAL = new FrecuenciaEnvioDAL();
            detalleIndicadorVariablesDAL = new DetalleIndicadorVariablesDAL();
            formulaNivelCalculoCategoriaDAL = new FormulaNivelCalculoCategoriaDAL();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pFormulasCalculo"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<FormulasCalculo>> ActualizarElemento(FormulasCalculo pFormulasCalculo)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 20/01/2023
        /// José Navarro Acuña
        /// Función que permite actualizar la etiqueta formula del objeto formula de calculo
        /// </summary>
        /// <param name="pFormulasCalculo"></param>
        /// <returns></returns>
        public RespuestaConsulta<FormulasCalculo> ActualizarEtiquetaFormula(FormulasCalculo pFormulasCalculo)
        {
            RespuestaConsulta<FormulasCalculo> resultado = new RespuestaConsulta<FormulasCalculo>();
            bool errorControlado = false;

            try
            {
                PrepararObjetoFormulaCalculo(pFormulasCalculo);
                
                if (pFormulasCalculo.IdFormula == 0)
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                formulasCalculoDAL.ActualizarEtiquetaFormula(pFormulasCalculo);

                resultado.Usuario = user;
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Editar;

                formulasCalculoDAL.RegistrarBitacora(resultado.Accion,
                        resultado.Usuario, resultado.Clase, pFormulasCalculo.Codigo);
            }
            catch (Exception ex)
            {
                resultado.MensajeError = ex.Message;

                if (errorControlado)
                    resultado.HayError = (int)Error.ErrorControlado;
                else
                    resultado.HayError = (int)Error.ErrorSistema;
            }

            return resultado;
        }

        /// <summary>
        /// 21/11/2022
        /// José Navarro Acuña
        /// Función que permite cambiar el estado de una fórmula cálculo
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<FormulasCalculo>> CambioEstado(FormulasCalculo pFormulasCalculo)
        {
            RespuestaConsulta<List<FormulasCalculo>> resultadoConsulta = new RespuestaConsulta<List<FormulasCalculo>>();

            try
            {
                PrepararObjetoFormulaCalculo(pFormulasCalculo);

                if (pFormulasCalculo.IdFormula == 0) // ¿ID descencriptado?
                {
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                FormulasCalculo formulaExistente = formulasCalculoDAL.ObtenerDatos(pFormulasCalculo).FirstOrDefault();

                if (formulaExistente == null) // ¿fórmula registrada?
                {
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                PrepararObjetoFormulaCalculo(formulaExistente);

                formulaExistente.UsuarioModificacion = user;
                formulaExistente.IdEstado = pFormulasCalculo.IdEstado;
                List<FormulasCalculo> resul = formulasCalculoDAL.ActualizarDatos(formulaExistente);

                resultadoConsulta.Clase = modulo;
                resultadoConsulta.Accion = pFormulasCalculo.IdEstado;
                resultadoConsulta.Usuario = user;
                resultadoConsulta.objetoRespuesta = resul;
                resultadoConsulta.CantidadRegistros = resul.Count();

                formulasCalculoDAL.RegistrarBitacora(resultadoConsulta.Accion,
                        resultadoConsulta.Usuario,
                            resultadoConsulta.Clase, formulaExistente.Codigo);
            }
            catch (Exception ex)
            {
                resultadoConsulta.HayError = (int)Error.ErrorSistema;
                resultadoConsulta.MensajeError = ex.Message;
            }
            return resultadoConsulta;
        }

        public RespuestaConsulta<List<FormulasCalculo>> ClonarDatos(FormulasCalculo objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FormulasCalculo>> EliminarElemento(FormulasCalculo pFormulasCalculo)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 21/10/2022
        /// José Navarro Acuña
        /// Función que crea un nuevo registro en la entidad Fórmula de Cálculo
        /// </summary>
        /// <param name="pFormulasCalculo"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<FormulasCalculo>> InsertarDatos(FormulasCalculo pFormulasCalculo)
        {
            RespuestaConsulta<List<FormulasCalculo>> resultado = new RespuestaConsulta<List<FormulasCalculo>>();
            bool errorControlado = false;

            try
            {
                PrepararObjetoFormulaCalculo(pFormulasCalculo);
                resultado = ValidarDatos(pFormulasCalculo);

                if (resultado.HayError != 0)
                {
                    return resultado;
                }

                List<FormulasCalculo> formulaCalculo = formulasCalculoDAL.ActualizarDatos(pFormulasCalculo);

                // en este punto tenemos la fórmula creada/actualizada
                // eliminar las categorias del nivel de cálculo
                formulaNivelCalculoCategoriaDAL.EliminarFormulaNivelCalculoCategoriaPorIDFormula(formulaCalculo[0].IdFormula);

                // el indicador fue proporcionado, marcada la opcion de categorias y se incluye el listado de categorias?
                if (pFormulasCalculo.IdIndicador != 0 && pFormulasCalculo.ListaCategoriasNivelesCalculo.Count > 0 && !pFormulasCalculo.NivelCalculoTotal)
                {
                    formulaNivelCalculoCategoriaDAL.InsertarFormulaNivelCalculoCategoria(formulaCalculo[0].IdFormula, pFormulasCalculo.ListaCategoriasNivelesCalculo);
                }

                formulaCalculo[0].IdFormula = 0;
                resultado.objetoRespuesta = formulaCalculo;
                resultado.Usuario = user;
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Insertar;

                //formulasCalculoDAL.RegistrarBitacora(resultado.Accion,
                        //resultado.Usuario, resultado.Clase, pFormulasCalculo.Codigo);
            }
            catch (Exception ex)
            {
                resultado.MensajeError = ex.Message;

                if (errorControlado)
                    resultado.HayError = (int)Error.ErrorControlado;
                else
                    resultado.HayError = (int)Error.ErrorSistema;
            }
            return resultado;
        }

        /// <summary>
        /// 21/10/2022
        /// José Navarro Acuña
        /// Función que permite obtener todos los datos de las fórmulas de cálculo
        /// </summary>
        /// <param name="pFormulasCalculo"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<FormulasCalculo>> ObtenerDatos(FormulasCalculo pFormulasCalculo)
        {
            RespuestaConsulta<List<FormulasCalculo>> resultadoConsulta = new RespuestaConsulta<List<FormulasCalculo>>();

            try
            {
                if (!string.IsNullOrEmpty(pFormulasCalculo.id))
                {
                    if (int.TryParse(Utilidades.Desencriptar(pFormulasCalculo.id), out int temp))
                    {
                        pFormulasCalculo.IdFormula = temp;
                    }
                }

                resultadoConsulta.Clase = modulo;
                resultadoConsulta.Accion = (int)Accion.Consultar;
                List<FormulasCalculo> result = formulasCalculoDAL.ObtenerDatos(pFormulasCalculo);
                resultadoConsulta.objetoRespuesta = result;
                resultadoConsulta.CantidadRegistros = result.Count();
            }
            catch (Exception ex)
            {
                resultadoConsulta.HayError = (int)Error.ErrorSistema;
                resultadoConsulta.MensajeError = ex.Message;
            }
            return resultadoConsulta;
        }

        /// <summary>
        /// 21/10/2022
        /// José Navarro Acuña
        /// Función que valida los datos de una fórmula de cálculo
        /// </summary>
        /// <param name="pFormulasCalculo"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<FormulasCalculo>> ValidarDatos(FormulasCalculo pFormulasCalculo)
        {
            RespuestaConsulta<List<FormulasCalculo>> resultado = new RespuestaConsulta<List<FormulasCalculo>>();
            resultado.HayError = (int)Error.NoError;
            bool errorControlado = false;

            try
            {
                // validar la existencia de la fórmula por medio del nombre y/o código
                FormulasCalculo formulasExistente = formulasCalculoDAL.VerificarExistenciaFormulaPorCodigoNombre(pFormulasCalculo);
                if (formulasExistente != null)
                {
                    if (formulasExistente.Codigo.Trim().ToUpper().Equals(pFormulasCalculo.Codigo.Trim().ToUpper()))
                    {
                        errorControlado = true;
                        throw new Exception(string.Format(Errores.CodigoRegistrado, EtiquetasViewFormulasCalculo.CrearFormula_LabelCodigo));
                    }
                    else
                    {
                        errorControlado = true;
                        throw new Exception(string.Format(Errores.NombreRegistrado, EtiquetasViewFormulasCalculo.CrearFormula_LabelNombre));
                    }
                }

                // validar si el valor selecionado en los comboboxes existe y se encuentra habilitado
                if (pFormulasCalculo.IdIndicador != 0)
                {
                    if (indicadorFonatelDAL.VerificarExistenciaIndicadorPorID((int)pFormulasCalculo.IdIndicador) == null)
                    {
                        errorControlado = true;
                        throw new Exception(string.Format(Errores.CampoConValorInvalido, EtiquetasViewFormulasCalculo.CrearFormula_LabelIndicadorSalida));
                    }
                }

                if (pFormulasCalculo.IdIndicadorVariable != 0) // variable dato
                {
                    if (detalleIndicadorVariablesDAL.ObtenerDatos(new DetalleIndicadorVariables() { idDetalleIndicador = (int)pFormulasCalculo.IdIndicadorVariable }).Count <= 0)
                    {
                        errorControlado = true;
                        throw new Exception(string.Format(Errores.CampoConValorInvalido, EtiquetasViewFormulasCalculo.CrearFormula_LabelVariableDato));
                    }
                }

                if (pFormulasCalculo.IdFrecuencia != 0)
                {
                    if (frecuenciaEnvioDAL.ObtenerDatos(new FrecuenciaEnvio() { idFrecuencia = (int)pFormulasCalculo.IdFrecuencia }).Count <= 0)
                    {
                        errorControlado = true;
                        throw new Exception(string.Format(Errores.CampoConValorInvalido, EtiquetasViewFormulasCalculo.CrearFormula_LabelFrecuencia));
                    }
                }
            }
            catch (Exception ex)
            {
                resultado.MensajeError = ex.Message;

                if (errorControlado)
                    resultado.HayError = (int)Error.ErrorControlado;
                else
                    resultado.HayError = (int)Error.ErrorSistema;
            }

            return resultado;
        }

        /// <summary>
        /// 30/01/2023
        /// José Navarro Acuña
        /// Función que valida los datos ingresados en cada argumento de la fórmula
        /// </summary>
        /// <param name="pListadoArgumento"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<ArgumentoFormula>> ValidarDatosArgumentoEnFormula(ArgumentoFormula pArgumento)
        {
            RespuestaConsulta<List<ArgumentoFormula>> resultado = new RespuestaConsulta<List<ArgumentoFormula>>
            {
                HayError = (int)Error.NoError
            };

            try
            {
                if (pArgumento.IdTipoArgumento == (int)FormulasTipoArgumentoEnum.VariableDatoCriterio)
                {
                    FormulasVariableDatoCriterio argVariableDato = (FormulasVariableDatoCriterio)pArgumento; // casteo explícito

                    if (argVariableDato.IdIndicador <= 0 && string.IsNullOrEmpty(argVariableDato.IdIndicadorDesencriptado))
                    {
                        throw new Exception(Errores.CamposIncompletos);
                    }

                    if (argVariableDato.IdVariableDato <= 0 && argVariableDato.IdCriterio <= 0)
                    {
                        throw new Exception(Errores.CamposIncompletos);
                    }

                    if (argVariableDato.IdFuenteIndicador == (int)FuenteIndicadorEnum.IndicadorDGF)
                    {
                        if (argVariableDato.IdAcumulacion <= 0)
                        {
                            throw new Exception(Errores.CamposIncompletos);
                        }
                    }

                    if (argVariableDato.IdFuenteIndicador == (int)FuenteIndicadorEnum.IndicadorDGF
                        || argVariableDato.IdFuenteIndicador == (int)FuenteIndicadorEnum.IndicadorDGM)
                    {
                        if (!argVariableDato.EsValorTotal)
                        {
                            if (argVariableDato.IdCategoria <= 0 || argVariableDato.IdDetalleCategoria <= 0)
                            {
                                throw new Exception(Errores.CamposIncompletos);
                            }
                        }
                    }

                    if (argVariableDato.IdFuenteIndicador == (int)FuenteIndicadorEnum.IndicadorDGC)
                    {
                        if (argVariableDato.IdCriterio != (int)TipoPorcentajeIndicadorCalculoEnum.cumplimiento
                            && argVariableDato.IdCriterio != (int)TipoPorcentajeIndicadorCalculoEnum.indicador)
                        {
                            throw new Exception(Errores.CamposIncompletos);
                        }
                    }
                }
                else if (pArgumento.IdTipoArgumento == (int)FormulasTipoArgumentoEnum.DefinicionFecha)
                {
                    FormulasDefinicionFecha argDefinicionFecha = (FormulasDefinicionFecha)pArgumento; // casteo explícito

                    if (argDefinicionFecha.IdTipoFechaInicio < (int)UnidadMedidaDefinicionFechasFormulasEnum.dias
                        && argDefinicionFecha.IdTipoFechaInicio > (int)UnidadMedidaDefinicionFechasFormulasEnum.anios)
                    {
                        throw new Exception(Errores.CamposIncompletos);
                    }

                    if (argDefinicionFecha.IdTipoFechaInicio == (int)TipoFechaDeficionFechasFormulasEnum.categoriaDesagregacion)
                    {
                        if (argDefinicionFecha.IdCategoriaInicio <= 0)
                        {
                            throw new Exception(Errores.CamposIncompletos);
                        }
                    }

                    if (argDefinicionFecha.IdTipoFechaFinal == (int)TipoFechaDeficionFechasFormulasEnum.categoriaDesagregacion)
                    {
                        if (argDefinicionFecha.IdCategoriaFinal <= 0)
                        {
                            throw new Exception(Errores.CamposIncompletos);
                        }
                    }

                    if (argDefinicionFecha.IdTipoFechaInicio == (int)TipoFechaDeficionFechasFormulasEnum.fecha)
                    {
                        if (argDefinicionFecha.FechaInicio == DateTime.MinValue || argDefinicionFecha.FechaInicio == null)
                        {
                            throw new Exception(Errores.CamposIncompletos);
                        }
                    }

                    if (argDefinicionFecha.IdTipoFechaFinal == (int)TipoFechaDeficionFechasFormulasEnum.fecha)
                    {
                        if (argDefinicionFecha.FechaFinal == DateTime.MinValue || argDefinicionFecha.FechaFinal == null)
                        {
                            throw new Exception(Errores.CamposIncompletos);
                        }
                    }
                }
                else
                {
                    throw new Exception(Errores.CamposIncompletos);
                }
            }
            catch (Exception ex)
            {
                resultado.MensajeError = ex.Message;
                resultado.HayError = (int)Error.ErrorControlado;
            }
            return resultado;
        }

        /// <summary>
        /// 20/01/2023
        /// José Navarro Acuña
        /// Función que permite registrar los detalles de la fórmula matematica del módulo gestión de cálculo
        /// </summary>
        /// <param name="pFormulasCalculo"></param>
        /// <param name="pListaArgumentosDTO"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<FormulasCalculo>> InsertarDetallesFormulaCalculo(FormulasCalculo pFormulasCalculo, List<ArgumentoConstruidoDTO> pListaArgumentosDTO)
        {
            RespuestaConsulta<List<FormulasCalculo>> resultado = new RespuestaConsulta<List<FormulasCalculo>>();
            resultado.HayError = (int)Error.NoError;
            bool errorControlado = false;

            try
            {
                PrepararObjetoFormulaCalculo(pFormulasCalculo);
                FormulaPredicado formulaPredicado = new FormulaPredicado();

                List<ArgumentoFormula> argumentoMapeados = new List<ArgumentoFormula>();

                if (pFormulasCalculo.IdFormula == 0)
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                // procesar los objetos del DTO para obtener los datos basados en los modelos
                for (int i = 0; i < pListaArgumentosDTO.Count; i++)
                {
                    ArgumentoFormula argumento = PrepararObjetoArgumentoFormula(pListaArgumentosDTO[i]);

                    // se valida la información proporcionada para cada argumento
                    RespuestaConsulta<List<ArgumentoFormula>> listaValidacion = ValidarDatosArgumentoEnFormula(argumento);

                    if (listaValidacion.HayError != 0)
                    {
                        return new RespuestaConsulta<List<FormulasCalculo>>() { MensajeError = listaValidacion.MensajeError };
                    }

                    if (argumento != null) // obviar operadores de suma, resta, división, etc
                    {
                        argumentoMapeados.Add(argumento);
                    }
                }

                for (int i = 0; i < argumentoMapeados.Count; i++)
                {
                    formulaPredicado.SetArgumentoFormula(argumentoMapeados[i]); // Paso 2, establecer el argumento a utilizar

                    if (argumentoMapeados[i].IdTipoArgumento == (int)FormulasTipoArgumentoEnum.VariableDatoCriterio) // Paso 3, determinar el tipo de argumento a evaluar
                    {
                        FormulasVariableDatoCriterio argVariableDato = (FormulasVariableDatoCriterio)argumentoMapeados[i]; // casteo explícito

                        switch (argVariableDato.IdFuenteIndicador)
                        {
                            case (int)FuenteIndicadorEnum.IndicadorDGF:
                                formulaPredicado.SetFuenteArgumento(new ArgumentoFonatel());
                                break;
                            case (int)FuenteIndicadorEnum.IndicadorDGM:
                                formulaPredicado.SetFuenteArgumento(new ArgumentoMercados());
                                break;
                            case (int)FuenteIndicadorEnum.IndicadorDGC:
                                formulaPredicado.SetFuenteArgumento(new ArgumentoCalidad());
                                break;
                            default:
                                break;
                        }

                        string predicadoSQL = formulaPredicado.GetArgumentoComoPredicadoSQL(); // Paso 4, construir el predicado SQL
                        InsertarArgumentoVariableDatoCriterio(predicadoSQL, pFormulasCalculo, argVariableDato, i);

                    }
                    else if (argumentoMapeados[i].IdTipoArgumento == (int)FormulasTipoArgumentoEnum.DefinicionFecha)
                    {
                        formulaPredicado.SetFuenteArgumento(new ArgumentoDefinicionFecha());
                        string predicadoSQL = formulaPredicado.GetArgumentoComoPredicadoSQL(); // Paso 4, construir el predicado SQL
                        InsertarArgumentoDefinicionDeFecha(predicadoSQL, pFormulasCalculo, (FormulasDefinicionFecha)argumentoMapeados[i], i);
                    }
                }

                ActualizarEtiquetaFormula(pFormulasCalculo);
                return null;
            }
            catch (Exception ex)
            {
                resultado.MensajeError = ex.Message;

                if (errorControlado)
                    resultado.HayError = (int)Error.ErrorControlado;
                else
                    resultado.HayError = (int)Error.ErrorSistema;
            }
            return resultado;
        }

        private void InsertarArgumentoNivelDeCalculo(string predicadoSQL)
        {

        }

        /// <summary>
        /// 30/01/2023
        /// José Navarro Acuña
        /// Función que permite registrar un argumento variable dato/criterio en una fórmula dada
        /// </summary>
        /// <param name="predicadoSQL"></param>
        /// <param name="pFormulasCalculo"></param>
        /// <param name="pFormulasVariableDatoCriterio"></param>
        /// <param name="orden"></param>
        private void InsertarArgumentoVariableDatoCriterio(string predicadoSQL, FormulasCalculo pFormulasCalculo, FormulasVariableDatoCriterio pFormulasVariableDatoCriterio, int pOrden)
        {
            formulasVariableDatoCriterioDAL.ActualizarDatos(pFormulasVariableDatoCriterio);
        }

        /// <summary>
        /// 30/01/2023
        /// José Navarro Acuña
        /// Función que permite registrar un argumento definición de fecha en una fórmula dada
        /// </summary>
        /// <param name="predicadoSQL"></param>
        /// <param name="pFormulasCalculo"></param>
        /// <param name="pFormulasDefinicionFecha"></param>
        /// <param name="orden"></param>
        private void InsertarArgumentoDefinicionDeFecha(string predicadoSQL, FormulasCalculo pFormulasCalculo, FormulasDefinicionFecha pFormulasDefinicionFecha, int pOrden)
        {
            formulasDefinicionFechaDAL.ActualizarDatos(pFormulasDefinicionFecha);
        }

        /// <summary>
        /// 24/10/2022
        /// José Navarro Acuña
        /// Prepara un objeto de fórmula para ser enviado al servicio DAL.
        /// Se preparan los id's de las tablas relacionadas para poder efectuar consultas debido a la encriptación.
        /// </summary>
        /// <param name="pIndicador"></param>
        private void PrepararObjetoFormulaCalculo(FormulasCalculo pFormulasCalculo)
        {
            if (!string.IsNullOrEmpty(pFormulasCalculo.id))
            {
                int.TryParse(Utilidades.Desencriptar(pFormulasCalculo.id), out int number);
                pFormulasCalculo.IdFormula = number;
            }

            if (!string.IsNullOrEmpty(pFormulasCalculo.IdFrecuenciaString))
            {
                int.TryParse(Utilidades.Desencriptar(pFormulasCalculo.IdFrecuenciaString), out int number);
                pFormulasCalculo.IdFrecuencia = number;
            }

            if (!string.IsNullOrEmpty(pFormulasCalculo.IdIndicadorSalidaString))
            {
                int.TryParse(Utilidades.Desencriptar(pFormulasCalculo.IdIndicadorSalidaString), out int number);
                pFormulasCalculo.IdIndicador = number;
            }

            if (!string.IsNullOrEmpty(pFormulasCalculo.IdVariableDatoString))
            {
                int.TryParse(Utilidades.Desencriptar(pFormulasCalculo.IdVariableDatoString), out int number);
                pFormulasCalculo.IdIndicadorVariable = number;
            }

            if (!pFormulasCalculo.NivelCalculoTotal && pFormulasCalculo.ListaCategoriasNivelesCalculo.Count > 0)
            {
                for (int i = 0; i < pFormulasCalculo.ListaCategoriasNivelesCalculo.Count; i++)
                {
                    int.TryParse(Utilidades.Desencriptar(pFormulasCalculo.ListaCategoriasNivelesCalculo[i].IdCategoriaString), out int number);
                    pFormulasCalculo.ListaCategoriasNivelesCalculo[i].IdCategoria = number;
                }
            }
        }

        /// <summary>
        /// 20/01/2023
        /// José Navarro Acuña
        /// Procesa un objeto argumento DTO para construir un argumento basado en el modelo,
        /// para luego ser utilizado en el proceso de la construicción de la fórmula matemática.
        /// Adicional al patrón de mapeo del DTO, se desencriptan IDs para el manejo del modelo en la capa DAL.
        /// </summary>
        /// <param name="pListaArgumentos"></param>
        private ArgumentoFormula PrepararObjetoArgumentoFormula(ArgumentoConstruidoDTO pArgumentoDTO)
        {
            ArgumentoFormula argumento = null;

            if (pArgumentoDTO.TipoArgumento == FormulasTipoArgumentoEnum.VariableDatoCriterio)
            {
                argumento = pArgumentoDTO.ConvertToVariableDatoCriterio();
                argumento.IdTipoArgumento = (int)FormulasTipoArgumentoEnum.VariableDatoCriterio;
                FormulasVariableDatoCriterio argVariableDato = (FormulasVariableDatoCriterio)argumento; // casteo explicito

                if (!string.IsNullOrEmpty(argVariableDato.IdFuenteIndicadorString))
                {
                    int.TryParse(Utilidades.Desencriptar(argVariableDato.IdFuenteIndicadorString), out int number);
                    argVariableDato.IdFuenteIndicador = number;
                }

                if (!string.IsNullOrEmpty(argVariableDato.IdVariableDatoString))
                {
                    int.TryParse(Utilidades.Desencriptar(argVariableDato.IdVariableDatoString), out int number);
                    argVariableDato.IdVariableDato = number;
                }

                if (!string.IsNullOrEmpty(argVariableDato.IdCriterioString))
                {
                    int.TryParse(Utilidades.Desencriptar(argVariableDato.IdCriterioString), out int number);
                    argVariableDato.IdCriterio = number;
                }

                if (!string.IsNullOrEmpty(argVariableDato.IdCategoriaString))
                {
                    int.TryParse(Utilidades.Desencriptar(argVariableDato.IdCategoriaString), out int number);
                    argVariableDato.IdCategoria = number;
                }

                if (!string.IsNullOrEmpty(argVariableDato.IdDetalleCategoriaString))
                {
                    int.TryParse(Utilidades.Desencriptar(argVariableDato.IdDetalleCategoriaString), out int number);
                    argVariableDato.IdDetalleCategoria = number;
                }

                if (!string.IsNullOrEmpty(argVariableDato.IdAcumulacionString))
                {
                    int.TryParse(Utilidades.Desencriptar(argVariableDato.IdAcumulacionString), out int number);
                    argVariableDato.IdAcumulacion = number;
                }

                if (!string.IsNullOrEmpty(argVariableDato.IdIndicadorString))
                {
                    int.TryParse(Utilidades.Desencriptar(argVariableDato.IdIndicadorString), out int number);
                    argVariableDato.IdIndicador = number;

                    // existen criterios donde el ID es alfanumerico, por tanto se procesa como string tambien
                    argVariableDato.IdIndicadorDesencriptado = Utilidades.Desencriptar(argVariableDato.IdIndicadorString);
                }
            }
            else if (pArgumentoDTO.TipoArgumento == FormulasTipoArgumentoEnum.DefinicionFecha) // FormulasTipoArgumentoEnum.DefinicionFecha
            {
                argumento = pArgumentoDTO.ConvertToFormulasDefinicionFecha();
                argumento.IdTipoArgumento = (int)FormulasTipoArgumentoEnum.DefinicionFecha;
                FormulasDefinicionFecha argDefinicionFecha = (FormulasDefinicionFecha)argumento;

                if (!string.IsNullOrEmpty(argDefinicionFecha.IdTipoFechaInicioString))
                {
                    int.TryParse(Utilidades.Desencriptar(argDefinicionFecha.IdTipoFechaInicioString), out int number);
                    argDefinicionFecha.IdTipoFechaInicio = number;
                }

                if (!string.IsNullOrEmpty(argDefinicionFecha.IdTipoFechaFinalString))
                {
                    int.TryParse(Utilidades.Desencriptar(argDefinicionFecha.IdTipoFechaFinalString), out int number);
                    argDefinicionFecha.IdTipoFechaInicio = number;
                }

                if (!string.IsNullOrEmpty(argDefinicionFecha.IdCategoriaInicioString))
                {
                    int.TryParse(Utilidades.Desencriptar(argDefinicionFecha.IdCategoriaInicioString), out int number);
                    argDefinicionFecha.IdCategoriaInicio = number;
                }

                if (!string.IsNullOrEmpty(argDefinicionFecha.IdCategoriaFinalString))
                {
                    int.TryParse(Utilidades.Desencriptar(argDefinicionFecha.IdCategoriaFinalString), out int number);
                    argDefinicionFecha.IdCategoriaInicio = number;
                }

                if (!string.IsNullOrEmpty(argDefinicionFecha.IdIndicadorString))
                {
                    int.TryParse(Utilidades.Desencriptar(argDefinicionFecha.IdIndicadorString), out int number);
                    argDefinicionFecha.IdIndicador = number;
                }
            }

            return argumento;
        }
    }
}
