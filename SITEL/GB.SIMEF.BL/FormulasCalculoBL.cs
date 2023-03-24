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
    public class FormulasCalculoBL : IMetodos<FormulaCalculo>
    {
        readonly string modulo = string.Empty;
        readonly string user = string.Empty;

        private readonly FormulasCalculoDAL formulasCalculoDAL;
        private readonly IndicadorFonatelDAL indicadorFonatelDAL;
        private readonly FrecuenciaEnvioDAL frecuenciaEnvioDAL;
        private readonly DetalleIndicadorVariablesDAL detalleIndicadorVariablesDAL;
        private readonly FormulaNivelCalculoCategoriaDAL formulaNivelCalculoCategoriaDAL;
        private readonly FormulasVariableDatoCriterioDAL formulasVariableDatoCriterioDAL;
        private readonly FormulasDefinicionFechaDAL formulasDefinicionFechaDAL;
        private readonly ArgumentoFormulaDAL argumentoFormulaDAL;

        public FormulasCalculoBL(string modulo, string user)
        {
            this.modulo = modulo;
            this.user = user;

            formulasCalculoDAL = new FormulasCalculoDAL();
            indicadorFonatelDAL = new IndicadorFonatelDAL();
            frecuenciaEnvioDAL = new FrecuenciaEnvioDAL();
            detalleIndicadorVariablesDAL = new DetalleIndicadorVariablesDAL();
            formulaNivelCalculoCategoriaDAL = new FormulaNivelCalculoCategoriaDAL();
            formulasVariableDatoCriterioDAL = new FormulasVariableDatoCriterioDAL();
            formulasDefinicionFechaDAL = new FormulasDefinicionFechaDAL();
            argumentoFormulaDAL = new ArgumentoFormulaDAL();
        }

        /// <summary>
        /// 09/02/2023
        /// José Navarro Acuña
        /// Función que edita un registro en la entidad Fórmula de Cálculo
        /// </summary>
        /// <param name="pFormulasCalculo"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<FormulaCalculo>> ActualizarElemento(FormulaCalculo pFormulasCalculo)
        {
            RespuestaConsulta<List<FormulaCalculo>> resultado = new RespuestaConsulta<List<FormulaCalculo>>();
            bool errorControlado = false;

            try
            {
                PrepararObjetoFormulaCalculo(pFormulasCalculo);
                resultado = ValidarDatos(pFormulasCalculo);

                if (resultado.HayError != 0)
                {
                    return resultado;
                }

                if (pFormulasCalculo.IdFormulaCalculo == 0)
                {
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                // bitacora
                FormulaCalculo formulaAntesDelCambio = formulasCalculoDAL.ObtenerDatos(pFormulasCalculo).Single();
                // ---------

                List<FormulaCalculo> formulaCalculo = formulasCalculoDAL.ActualizarDatos(pFormulasCalculo);

                // en este punto tenemos la fórmula creada/actualizada
                // eliminar las categorias del nivel de cálculo
                formulaNivelCalculoCategoriaDAL.EliminarFormulaNivelCalculoCategoriaPorIDFormula(formulaCalculo[0].IdFormulaCalculo);

                // el indicador fue proporcionado, marcada la opcion de categorias y se incluye el listado de categorias?
                if (pFormulasCalculo.IdIndicador != 0 && pFormulasCalculo.ListaCategoriasNivelesCalculo.Count > 0 && !pFormulasCalculo.NivelCalculoTotal)
                {
                    formulaNivelCalculoCategoriaDAL.InsertarFormulaNivelCalculoCategoria(formulaCalculo[0].IdFormulaCalculo, pFormulasCalculo.ListaCategoriasNivelesCalculo);
                }

                formulaCalculo[0].IdFormulaCalculo = 0;
                resultado.objetoRespuesta = formulaCalculo;
                resultado.Usuario = user;
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Editar;

                indicadorFonatelDAL.RegistrarBitacora(resultado.Accion, resultado.Usuario, resultado.Clase, 
                    formulaCalculo[0].Codigo, formulaCalculo[0].ToString(), formulaAntesDelCambio.ToString(), "");
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
        /// 20/01/2023
        /// José Navarro Acuña
        /// Función que permite actualizar la etiqueta formula del objeto formula de calculo
        /// </summary>
        /// <param name="pFormulasCalculo"></param>
        /// <returns></returns>
        public RespuestaConsulta<FormulaCalculo> ActualizarEtiquetaFormula(FormulaCalculo pFormulasCalculo, FormulaCalculo pFormulaCalculoAntesDeActualizar = null)
        {
            RespuestaConsulta<FormulaCalculo> resultado = new RespuestaConsulta<FormulaCalculo>();
            bool errorControlado = false;

            try
            {
                PrepararObjetoFormulaCalculo(pFormulasCalculo);
                
                if (pFormulasCalculo.IdFormulaCalculo == 0)
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                // bitacora
                FormulaCalculo formulaAntesDelCambio = pFormulaCalculoAntesDeActualizar == null ? formulasCalculoDAL.ObtenerDatos(pFormulasCalculo).Single() : pFormulaCalculoAntesDeActualizar;
                // ---------

                pFormulasCalculo.UsuarioModificacion = user;
                FormulaCalculo formulaActualizada = formulasCalculoDAL.ActualizarEtiquetaFormula(pFormulasCalculo);

                resultado.Usuario = user;
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Editar;
                resultado.objetoRespuesta = formulaActualizada;

                formulasCalculoDAL.RegistrarBitacora(resultado.Accion, resultado.Usuario, resultado.Clase,
                    formulaAntesDelCambio.Codigo, formulaActualizada.ToString(), formulaAntesDelCambio.ToString(), "");
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
        public RespuestaConsulta<List<FormulaCalculo>> CambioEstado(FormulaCalculo pFormulasCalculo)
        {
            RespuestaConsulta<List<FormulaCalculo>> resultadoConsulta = new RespuestaConsulta<List<FormulaCalculo>>();

            try
            {
                PrepararObjetoFormulaCalculo(pFormulasCalculo);

                if (pFormulasCalculo.IdFormulaCalculo == 0) // ¿ID descencriptado?
                {
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                FormulaCalculo formulaExistente = formulasCalculoDAL.ObtenerDatos(pFormulasCalculo).FirstOrDefault();

                if (formulaExistente == null) // ¿fórmula registrada?
                {
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                PrepararObjetoFormulaCalculo(formulaExistente);

                // bitacora
                FormulaCalculo formulaAntesDelCambio = formulaExistente.Shallowcopy();
                //----------

                formulaExistente.UsuarioModificacion = user;
                formulaExistente.IdEstadoRegistro = pFormulasCalculo.IdEstadoRegistro;
                List<FormulaCalculo> resultadoActualizar = formulasCalculoDAL.ActualizarDatos(formulaExistente);

                // bitacora
                FormulaCalculo formulaDespuesDelCambio = formulaExistente;
                //----------

                resultadoConsulta.Clase = modulo;
                resultadoConsulta.Accion = pFormulasCalculo.IdEstadoRegistro;
                resultadoConsulta.Usuario = user;
                resultadoConsulta.objetoRespuesta = resultadoActualizar;
                resultadoConsulta.CantidadRegistros = resultadoActualizar.Count();

                formulasCalculoDAL.RegistrarBitacora(resultadoConsulta.Accion, resultadoConsulta.Usuario,
                      resultadoConsulta.Clase, formulaDespuesDelCambio.Codigo, formulaDespuesDelCambio.ToString(), formulaAntesDelCambio.ToString(), "");
            }
            catch (Exception ex)
            {
                resultadoConsulta.HayError = (int)Error.ErrorSistema;
                resultadoConsulta.MensajeError = ex.Message;
            }
            return resultadoConsulta;
        }

        public RespuestaConsulta<List<FormulaCalculo>> ClonarDatos(FormulaCalculo objeto)
        {
            // la clonación se creó a travez de la creación y la carga inicial de la pantalla
            throw new NotImplementedException();
        }

        /// <summary>
        /// 07/02/2023
        /// José Navarro Acuña
        /// Función que permite clonar los argumentos de una fórmula de cálculo
        /// </summary>
        /// <param name="pFormulasCalculoClonada"></param>
        /// <param name="pIdFormulaArgumentosAClonar"></param>
        /// <returns></returns>
        public RespuestaConsulta<FormulaCalculo> ClonarArgumentosDeFormula(FormulaCalculo pFormulasCalculoClonada, string pIdFormulaArgumentosAClonar)
        {
            RespuestaConsulta<FormulaCalculo> resultado = new RespuestaConsulta<FormulaCalculo>();
            int idFormulaArgumentosAClonar;
            bool errorControlado = false;

            try
            {
                PrepararObjetoFormulaCalculo(pFormulasCalculoClonada); // descriptar los IDs de la formula creada en el paso 1

                if (pFormulasCalculoClonada.IdFormulaCalculo == 0) // ¿ID descencriptado con éxito?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                int.TryParse(Utilidades.Desencriptar(pIdFormulaArgumentosAClonar), out int number);
                idFormulaArgumentosAClonar = number;

                if (idFormulaArgumentosAClonar == 0) // ¿ID descencriptado con éxito?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                FormulaPredicado formulaPredicado = new FormulaPredicado();
                formulaPredicado.SetFormulasCalculo(pFormulasCalculoClonada);

                ClonarFormulasVariableDatoCriterio(formulaPredicado, pFormulasCalculoClonada, idFormulaArgumentosAClonar);
                ClonarFormulasDefinicionFechas(formulaPredicado, pFormulasCalculoClonada, idFormulaArgumentosAClonar);

                // se debe obtener la etiqueta que representa la fórmula matemática. No se maneja desde js para evitar SQL Inyection
                List<FormulaCalculo> formulaBasadaParaClonar = formulasCalculoDAL.ObtenerDatos(new FormulaCalculo() { IdFormulaCalculo = idFormulaArgumentosAClonar });

                if (formulaBasadaParaClonar.Count <= 0)
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                pFormulasCalculoClonada.Formula = formulaBasadaParaClonar[0].Formula;
                resultado = ActualizarEtiquetaFormula(pFormulasCalculoClonada);
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

        public RespuestaConsulta<List<FormulaCalculo>> EliminarElemento(FormulaCalculo pFormulasCalculo)
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
        public RespuestaConsulta<List<FormulaCalculo>> InsertarDatos(FormulaCalculo pFormulasCalculo)
        {
            RespuestaConsulta<List<FormulaCalculo>> resultado = new RespuestaConsulta<List<FormulaCalculo>>();
            bool errorControlado = false;

            try
            {
                PrepararObjetoFormulaCalculo(pFormulasCalculo);
                resultado = ValidarDatos(pFormulasCalculo);

                if (resultado.HayError != 0)
                {
                    return resultado;
                }

                List<FormulaCalculo> formulaCalculo = formulasCalculoDAL.ActualizarDatos(pFormulasCalculo);

                // en este punto tenemos la fórmula creada/actualizada
                // eliminar las categorias del nivel de cálculo
                formulaNivelCalculoCategoriaDAL.EliminarFormulaNivelCalculoCategoriaPorIDFormula(formulaCalculo[0].IdFormulaCalculo);

                // el indicador fue proporcionado, marcada la opcion de categorias y se incluye el listado de categorias?
                if (pFormulasCalculo.IdIndicador != 0 && pFormulasCalculo.ListaCategoriasNivelesCalculo.Count > 0 && !pFormulasCalculo.NivelCalculoTotal)
                {
                    formulaNivelCalculoCategoriaDAL.InsertarFormulaNivelCalculoCategoria(formulaCalculo[0].IdFormulaCalculo, pFormulasCalculo.ListaCategoriasNivelesCalculo);
                }

                formulaCalculo[0].IdFormulaCalculo = 0;
                resultado.objetoRespuesta = formulaCalculo;
                resultado.Usuario = user;
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Insertar;

                indicadorFonatelDAL.RegistrarBitacora(resultado.Accion,
                        resultado.Usuario, resultado.Clase, formulaCalculo[0].Codigo, "", "", formulaCalculo[0].ToString());
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
        /// 13/02/2023
        /// José Navarro Acuña
        /// Función que permite realizar un guardado definitivo de una fórmula de cálculo
        /// </summary>
        /// <param name="pFormulasCalculo"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<FormulaCalculo>> GuardadoDefinitivoFormulaCalculo(FormulaCalculo pFormulasCalculo)
        {
            RespuestaConsulta<List<FormulaCalculo>> resultado = new RespuestaConsulta<List<FormulaCalculo>>();
            bool errorControlado = false;

            try
            {
                int.TryParse(Utilidades.Desencriptar(pFormulasCalculo.id), out int number);
                pFormulasCalculo.IdFormulaCalculo = number;

                if (pFormulasCalculo.IdFormulaCalculo == 0) // ¿ID descencriptado con éxito?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                FormulaCalculo formulaRegistrada = formulasCalculoDAL.VerificarExistenciaFormulaPorID(pFormulasCalculo.IdFormulaCalculo);

                if (formulaRegistrada == null) // la fórmula existe?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                // validar que la fórmula tenga sus datos completos y exista al menos un argumento registrado
                string msgFormulaCompleto = VerificarDatosCompletosFormulaCalculo(formulaRegistrada);

                if (!string.IsNullOrEmpty(msgFormulaCompleto))
                {
                    errorControlado = true;
                    throw new Exception(msgFormulaCompleto);
                }

                pFormulasCalculo.IdEstadoRegistro = (int)EstadosRegistro.Activo; // estado que representa "Finalizado"
                return CambioEstado(pFormulasCalculo); // reutilizar la función de cambio de estado
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
        public RespuestaConsulta<List<FormulaCalculo>> ObtenerDatos(FormulaCalculo pFormulasCalculo)
        {
            RespuestaConsulta<List<FormulaCalculo>> resultadoConsulta = new RespuestaConsulta<List<FormulaCalculo>>();

            try
            {
                if (!string.IsNullOrEmpty(pFormulasCalculo.id))
                {
                    if (int.TryParse(Utilidades.Desencriptar(pFormulasCalculo.id), out int temp))
                    {
                        pFormulasCalculo.IdFormulaCalculo = temp;
                    }
                }

                resultadoConsulta.Clase = modulo;
                resultadoConsulta.Accion = (int)Accion.Consultar;
                List<FormulaCalculo> result = formulasCalculoDAL.ObtenerDatos(pFormulasCalculo);
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
        public RespuestaConsulta<List<FormulaCalculo>> ValidarDatos(FormulaCalculo pFormulasCalculo)
        {
            RespuestaConsulta<List<FormulaCalculo>> resultado = new RespuestaConsulta<List<FormulaCalculo>>();
            resultado.HayError = (int)Error.NoError;
            bool errorControlado = false;

            try
            {
                // validar la existencia de la fórmula por medio del nombre y/o código
                FormulaCalculo formulasExistente = formulasCalculoDAL.VerificarExistenciaFormulaPorCodigoNombre(pFormulasCalculo);
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

                if (pFormulasCalculo.IdDetalleIndicadorVariable != 0) // variable dato
                {
                    if (detalleIndicadorVariablesDAL.ObtenerDatos(new DetalleIndicadorVariable() { IdDetalleIndicadorVariable = (int)pFormulasCalculo.IdDetalleIndicadorVariable }).Count <= 0)
                    {
                        errorControlado = true;
                        throw new Exception(string.Format(Errores.CampoConValorInvalido, EtiquetasViewFormulasCalculo.CrearFormula_LabelVariableDato));
                    }
                }

                if (pFormulasCalculo.IdFrecuenciaEnvio != 0)
                {
                    if (frecuenciaEnvioDAL.ObtenerDatos(new FrecuenciaEnvio() { IdFrecuenciaEnvio = (int)pFormulasCalculo.IdFrecuenciaEnvio }).Count <= 0)
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
        public RespuestaConsulta<FormulaCalculo> ValidarDatosArgumentoEnFormula(ArgumentoFormula pArgumento)
        {
            RespuestaConsulta<FormulaCalculo> resultado = new RespuestaConsulta<FormulaCalculo>
            {
                HayError = (int)Error.NoError
            };

            try
            {
                if (pArgumento.IdFormulaTipoArgumento == (int)FormulasTipoArgumentoEnum.VariableDatoCriterio)
                {
                    FormulaVariableDatoCriterio argVariableDato = (FormulaVariableDatoCriterio)pArgumento; // casteo explícito

                    if (string.IsNullOrEmpty(argVariableDato.IdIndicador))
                    {
                        throw new Exception(Errores.CamposIncompletos);
                    }

                    if ((argVariableDato.IdDetalleIndicadorVariable == null || argVariableDato.IdDetalleIndicadorVariable <= 0) && string.IsNullOrEmpty(argVariableDato.IdCriterio))
                    {
                        throw new Exception(Errores.CamposIncompletos);
                    }

                    if (argVariableDato.IdFuenteIndicador == (int)FuenteIndicadorEnum.IndicadorDGF)
                    {
                        if (argVariableDato.IdAcumulacionFormula == null || argVariableDato.IdAcumulacionFormula <= 0)
                        {
                            throw new Exception(Errores.CamposIncompletos);
                        }
                    }

                    if (argVariableDato.IdFuenteIndicador == (int)FuenteIndicadorEnum.IndicadorDGF)
                    {
                        if (!argVariableDato.EsValorTotal)
                        {
                            if (argVariableDato.IdCategoriaDesagregacion == null || argVariableDato.IdCategoriaDesagregacion <= 0 || argVariableDato.IdDetalleCategoriaTexto == null || argVariableDato.IdDetalleCategoriaTexto <= 0)
                            {
                                throw new Exception(Errores.CamposIncompletos);
                            }
                        }
                    }

                    if (argVariableDato.IdFuenteIndicador == (int)FuenteIndicadorEnum.IndicadorDGM)
                    {
                        if (!argVariableDato.EsValorTotal)
                        {
                            if (argVariableDato.IdDetalleCategoriaTexto == null || argVariableDato.IdDetalleCategoriaTexto <= 0)
                            {
                                throw new Exception(Errores.CamposIncompletos);
                            }
                        }
                    }

                    if (argVariableDato.IdFuenteIndicador == (int)FuenteIndicadorEnum.IndicadorDGC)
                    {
                        if (!argVariableDato.IdCriterio.Equals(((int)TipoPorcentajeIndicadorCalculoEnum.cumplimiento).ToString())
                            && !argVariableDato.IdCriterio.Equals(((int)TipoPorcentajeIndicadorCalculoEnum.indicador).ToString()))
                        {
                            throw new Exception(Errores.CamposIncompletos);
                        }
                    }
                }
                else if (pArgumento.IdFormulaTipoArgumento == (int)FormulasTipoArgumentoEnum.DefinicionFecha)
                {
                    FormulaDefinicionFecha argDefinicionFecha = (FormulaDefinicionFecha)pArgumento; // casteo explícito

                    if (argDefinicionFecha.IdTipoFechaInicio < (int)UnidadMedidaDefinicionFechasFormulasEnum.dias
                        && argDefinicionFecha.IdTipoFechaInicio > (int)UnidadMedidaDefinicionFechasFormulasEnum.anios)
                    {
                        throw new Exception(Errores.CamposIncompletos);
                    }

                    if (argDefinicionFecha.IdTipoFechaInicio == (int)TipoFechaDeficionFechasFormulasEnum.categoriaDesagregacion)
                    {
                        if (argDefinicionFecha.IdCategoriaDesagregacionInicio == null || argDefinicionFecha.IdCategoriaDesagregacionInicio <= 0)
                        {
                            throw new Exception(Errores.CamposIncompletos);
                        }
                    }

                    if (argDefinicionFecha.IdTipoFechaFinal == (int)TipoFechaDeficionFechasFormulasEnum.categoriaDesagregacion)
                    {
                        if (argDefinicionFecha.IdCategoriaDesagregacionFinal == null || argDefinicionFecha.IdCategoriaDesagregacionFinal <= 0)
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
        /// 13/02/2023
        /// José Navarro Acuña
        /// Función que permite verificar si una fórmula ha sido ejecutada
        /// </summary>
        /// <param name="pIdFormula"></param>
        /// <returns></returns>
        public RespuestaConsulta<string> VerificarSiFormulaEjecuto(string pIdFormula)
        {
            RespuestaConsulta<string> resultado = new RespuestaConsulta<string>
            {
                HayError = (int)Error.NoError
            };

            try
            {
                int idFormula = 0;

                if (!string.IsNullOrEmpty(pIdFormula))
                {
                    int.TryParse(Utilidades.Desencriptar(pIdFormula), out int number);
                    idFormula = number;
                }

                if (idFormula == 0)
                {
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                FormulaCalculoDTO formulaDTO = formulasCalculoDAL.VerificarSiFormulaEjecuto(idFormula);
                
                if (formulaDTO != null)
                {
                    resultado.objetoRespuesta = string.Format(
                        EtiquetasViewFormulasCalculo.MensajeAdvertenciaFormulaEjecutada,
                        formulaDTO.NombreIndicador,
                        formulaDTO.NombreVariableDato,
                        formulaDTO.FechaEjecucion.ToShortDateString() // dd-mm-yyyy
                    );
                }
            }
            catch (Exception ex)
            {
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
        public RespuestaConsulta<FormulaCalculo> InsertarDetallesFormulaCalculo(FormulaCalculo pFormulasCalculo, List<ArgumentoConstruidoDTO> pListaArgumentosDTO)
        {
            RespuestaConsulta<FormulaCalculo> resultado = new RespuestaConsulta<FormulaCalculo>();
            resultado.HayError = (int)Error.NoError;
            bool errorControlado = false;

            try
            {
                // consultar el indicador de salida almacenado previamente
                PrepararObjetoFormulaCalculo(pFormulasCalculo);
                FormulaCalculo formulaAlmacenada = formulasCalculoDAL.ObtenerDatos(pFormulasCalculo)[0];

                if (formulaAlmacenada == null || string.IsNullOrEmpty(formulaAlmacenada.IdIndicadorSalidaString))
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                pFormulasCalculo.IdIndicadorSalidaString = formulaAlmacenada.IdIndicadorSalidaString;
                PrepararObjetoFormulaCalculo(pFormulasCalculo);

                if (pFormulasCalculo.IdFormulaCalculo == 0 || pFormulasCalculo.IdIndicador == 0)
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                // procesar los objetos del DTO para obtener los datos basados en los modelos
                RespuestaConsulta<List<ArgumentoFormula>> respuestaArgumentosValidados = ProcesarArgumentosEnDTO(pListaArgumentosDTO, pFormulasCalculo.IdFormulaCalculo);

                if (respuestaArgumentosValidados.HayError != (int) Error.NoError)
                {
                    throw new Exception(respuestaArgumentosValidados.MensajeError);
                }

                // remover los argumentos anteriores, ya sea por que cambiaron o es un guardado parcial
                bool elimino = argumentoFormulaDAL.EliminarArgumentos(new ArgumentoFormula() { IdFormulaCalculo = pFormulasCalculo.IdFormulaCalculo });

                if (!elimino)
                {
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                string etiquetaFormula = InsertarArgumentosEnFormula(pFormulasCalculo, respuestaArgumentosValidados.objetoRespuesta);

                pFormulasCalculo.Formula = etiquetaFormula.ToString();
                resultado = ActualizarEtiquetaFormula(pFormulasCalculo, formulaAlmacenada);
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

        #region Funciones del motor de cálculo

        /// <summary>
        /// 01/03/2023
        /// José Navarro Acuña
        /// Función que permite crear un job en el motor de cálculo para correr una fórmula
        /// </summary>
        /// <param name="pFormulasCalculo"></param>
        /// <returns></returns>
        public async Task<RespuestaConsulta<List<FormulaCalculo>>> CrearJobEnMotor(FormulaCalculo pFormulasCalculo)
        {
            RespuestaConsulta<List<FormulaCalculo>> resultado = new RespuestaConsulta<List<FormulaCalculo>>();
            resultado.objetoRespuesta = new List<FormulaCalculo>();
            resultado.HayError = (int)Error.NoError;
            bool errorControlado = false;

            try
            {
                PrepararObjetoFormulaCalculo(pFormulasCalculo);
                FormulaCalculo formulaAlmacenda = formulasCalculoDAL.ObtenerDatos(pFormulasCalculo)[0];
                PrepararObjetoFormulaCalculo(formulaAlmacenda);


                if (formulaAlmacenda.IdJob != null) // existe un job para esta formula?
                {
                    JobMotorFormulaDTO jobMotorDTO = await formulasCalculoDAL.ObtenerJobMotor(formulaAlmacenda);
                    if (jobMotorDTO != null)
                    {
                        JobMotorFormulaDTO jobDTO_frecuencia = await formulasCalculoDAL.ActualizarCalendarizacionJob(formulaAlmacenda);
                        JobMotorFormulaDTO jobDTO_estado = await formulasCalculoDAL.CambiarEstadoJob(formulaAlmacenda);
                    }
                }
                else
                {
                    JobMotorFormulaDTO jobDTO = await formulasCalculoDAL.CrearJobEnMotorAsync(formulaAlmacenda, false);
                    formulaAlmacenda.IdJob = jobDTO.idJob;
                    FormulaCalculo formula = formulasCalculoDAL.RegistrarJobEnFormula(formulaAlmacenda);
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
        /// 01/03/2023
        /// José Navarro Acuña
        /// Función que permite cambiar el estado de la formula relacionada a un job en el motor de cálculo
        /// </summary>
        /// <param name="pFormulasCalculo"></param>
        /// <returns></returns>
        public async Task<RespuestaConsulta<List<FormulaCalculo>>> CambiarEstadoJob(FormulaCalculo pFormulasCalculo)
        {
            RespuestaConsulta<List<FormulaCalculo>> resultado = new RespuestaConsulta<List<FormulaCalculo>>();
            resultado.HayError = (int)Error.NoError;
            bool errorControlado = false;

            try
            {
                PrepararObjetoFormulaCalculo(pFormulasCalculo);
                FormulaCalculo formulaAlmacenda = formulasCalculoDAL.ObtenerDatos(pFormulasCalculo)[0];
                PrepararObjetoFormulaCalculo(formulaAlmacenda);

                if (formulaAlmacenda.IdJob != null)
                {
                    JobMotorFormulaDTO jobMotorDTO = await formulasCalculoDAL.ObtenerJobMotor(formulaAlmacenda);
                    if (jobMotorDTO != null)
                    {
                        JobMotorFormulaDTO jobDTO = await formulasCalculoDAL.CambiarEstadoJob(formulaAlmacenda);
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
        /// 01/03/2023
        /// José Navarro Acuña
        /// Función que permite ejecutar de manera manual una fórmula de cálculo en el motor de cálculo
        /// </summary>
        /// <param name="pFormulasCalculo"></param>
        /// <returns></returns>
        public async Task<RespuestaConsulta<List<FormulaCalculo>>> EjecutarJobFormulaManualmente(FormulaCalculo pFormulasCalculo)
        {
            RespuestaConsulta<List<FormulaCalculo>> resultado = new RespuestaConsulta<List<FormulaCalculo>>();
            bool errorControlado = false;

            try
            {
                int.TryParse(Utilidades.Desencriptar(pFormulasCalculo.id), out int number);
                pFormulasCalculo.IdFormulaCalculo = number;

                if (pFormulasCalculo.IdFormulaCalculo == 0) // ¿ID descencriptado con éxito?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                FormulaCalculo formulaRegistrada = formulasCalculoDAL.VerificarExistenciaFormulaPorID(pFormulasCalculo.IdFormulaCalculo);

                if (formulaRegistrada == null) // la fórmula existe?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                // validar que la fórmula tenga sus datos completos y exista al menos un argumento registrado
                string msgFormulaCompleto = VerificarDatosCompletosFormulaCalculo(formulaRegistrada);

                if (!string.IsNullOrEmpty(msgFormulaCompleto))
                {
                    errorControlado = true;
                    throw new Exception(msgFormulaCompleto);
                }

                PrepararObjetoFormulaCalculo(formulaRegistrada);

                if (formulaRegistrada.IdJob != null) // en caso de que el job ya haya sido creado anteriormente, mandarlo a correr segun su ID
                {
                    JobMotorFormulaDTO jobMotorDTO = await formulasCalculoDAL.ObtenerJobMotor(formulaRegistrada);
                    if (jobMotorDTO != null)
                    { 
                        _ = await formulasCalculoDAL.EjecutarJobExistente(formulaRegistrada);
                    }
                }
                else 
                {
                    // el job aun no existe por tanto, mandarlo a crear y correr de una vez
                    JobMotorFormulaDTO jobDTO = await formulasCalculoDAL.CrearJobEnMotorAsync(formulaRegistrada, true);
                    formulaRegistrada.IdJob = jobDTO.idJob;
                    FormulaCalculo formula = formulasCalculoDAL.RegistrarJobEnFormula(formulaRegistrada);
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

        #endregion

        #region Funciones privadas

        /// <summary>
        /// 14/02/2023
        /// José Navarro
        /// Función que inserta un listado de argumentos a una fórmula de cálculo.
        /// Retorna una etiqueta parametrizada que representa la fórmula matemática construida, por ej.: ({0} + 2) / {1} - 25
        /// </summary>
        /// <param name="pFormulasCalculo"></param>
        /// <param name="pArgumentosValidados"></param>
        /// <returns></returns>
        private string InsertarArgumentosEnFormula(FormulaCalculo pFormulasCalculo, List<ArgumentoFormula> pArgumentosValidados)
        {
            FormulaPredicado formulaPredicado = new FormulaPredicado();
            formulaPredicado.SetFormulasCalculo(pFormulasCalculo); // datos almacenados del paso 1 del formulario

            StringBuilder etiquetaFormula = new StringBuilder();
            int ordenEnFormula = 0;

            for (int i = 0; i < pArgumentosValidados.Count; i++)
            {
                if (!pArgumentosValidados[i].EsOperadorMatematico)
                {
                    etiquetaFormula.Append("{" + string.Format("{0}", ordenEnFormula) + "}");

                    formulaPredicado.SetArgumentoFormula(pArgumentosValidados[i]); // establecer cada argumento a utilizar para computar el predicado SQL

                    if (pArgumentosValidados[i].IdFormulaTipoArgumento == (int)FormulasTipoArgumentoEnum.VariableDatoCriterio) // determinar el tipo de argumento a evaluar
                    {
                        FormulaVariableDatoCriterio argVariableDato = (FormulaVariableDatoCriterio)pArgumentosValidados[i]; // casteo explícito

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
                        InsertarArgumentoVariableDatoCriterio(predicadoSQL, pFormulasCalculo, argVariableDato, ordenEnFormula);
                        ordenEnFormula++;

                    }
                    else if (pArgumentosValidados[i].IdFormulaTipoArgumento == (int)FormulasTipoArgumentoEnum.DefinicionFecha)
                    {
                        formulaPredicado.SetFuenteArgumento(new ArgumentoDefinicionFecha());
                        string predicadoSQL = formulaPredicado.GetArgumentoComoPredicadoSQL(); // Paso 4, construir el predicado SQL
                        InsertarArgumentoDefinicionDeFecha(predicadoSQL, pFormulasCalculo, (FormulaDefinicionFecha)pArgumentosValidados[i], ordenEnFormula);
                        ordenEnFormula++;
                    }
                }
                else
                {
                    etiquetaFormula.Append(pArgumentosValidados[i].Etiqueta);
                }
            }
            return etiquetaFormula.ToString();
        }

        /// <summary>
        /// 08/02/2023
        /// José Navarro Acuña
        /// Función que permite clonar los argumentos de tipo variables dato / criterios de una fórmula a otra
        /// El objeto pFormulaPredicado debe tener establecida la fórmula y el objeto pFormulasCalculo tiene que tener todos los valores 
        /// </summary>
        /// <param name="pFormulaPredicado"></param>
        /// <param name="pFormulasCalculo"></param>
        /// <param name="pIdFormulaArgumentosAClonar"></param>
        private void ClonarFormulasVariableDatoCriterio(FormulaPredicado pFormulaPredicado, FormulaCalculo pFormulasCalculo, int pIdFormulaArgumentosAClonar)
        {
            List<FormulaVariableDatoCriterio> listaArgumentosVariables = formulasVariableDatoCriterioDAL.ObtenerDatos(new FormulaVariableDatoCriterio() { IdFormulaCalculo = pIdFormulaArgumentosAClonar });

            for (int i = 0; i < listaArgumentosVariables.Count; i++)
            {
                listaArgumentosVariables[i].IdFormulaVariableDatoCriterio = 0; // evitar actualizar el registro existente
                pFormulaPredicado.SetArgumentoFormula(listaArgumentosVariables[i]);

                switch (listaArgumentosVariables[i].IdFuenteIndicador)
                {
                    case (int)FuenteIndicadorEnum.IndicadorDGF:
                        pFormulaPredicado.SetFuenteArgumento(new ArgumentoFonatel());
                        break;
                    case (int)FuenteIndicadorEnum.IndicadorDGM:
                        pFormulaPredicado.SetFuenteArgumento(new ArgumentoMercados());
                        break;
                    case (int)FuenteIndicadorEnum.IndicadorDGC:
                        pFormulaPredicado.SetFuenteArgumento(new ArgumentoCalidad());
                        break;
                    default:
                        break;
                }

                string predicadoSQL = pFormulaPredicado.GetArgumentoComoPredicadoSQL(); // construir el predicado SQL
                InsertarArgumentoVariableDatoCriterio(predicadoSQL, pFormulasCalculo, listaArgumentosVariables[i], listaArgumentosVariables[i].OrdenEnFormula);
            }
        }

        /// <summary>
        /// 08/02/2023
        /// José Navarro Acuña
        /// Función que permite clonar los argumentos de tipo variables dato / criterios de una fórmula a otra.
        /// El objeto pFormulaPredicado debe tener establecida la fórmula y el objeto pFormulasCalculo tiene que tener todos los valores 
        /// </summary>
        /// <param name="pFormulaPredicado"></param>
        /// <param name="pFormulasCalculo"></param>
        /// <param name="pIdFormulaArgumentosAClonar"></param>
        private void ClonarFormulasDefinicionFechas(FormulaPredicado pFormulaPredicado, FormulaCalculo pFormulasCalculo, int pIdFormulaArgumentosAClonar)
        {
            List<FormulaDefinicionFecha> listaArgumentosDefinicionFecha = formulasDefinicionFechaDAL.ObtenerDatos(new FormulaDefinicionFecha() { IdFormulaCalculo = pIdFormulaArgumentosAClonar });

            for (int i = 0; i < listaArgumentosDefinicionFecha.Count; i++)
            {
                listaArgumentosDefinicionFecha[i].IdFormulaDefinicionFecha = 0; // evitar actualizar el registro existente
                pFormulaPredicado.SetArgumentoFormula(listaArgumentosDefinicionFecha[i]);
                pFormulaPredicado.SetFuenteArgumento(new ArgumentoDefinicionFecha());

                string predicadoSQL = pFormulaPredicado.GetArgumentoComoPredicadoSQL(); // construir el predicado SQL
                InsertarArgumentoDefinicionDeFecha(predicadoSQL, pFormulasCalculo, listaArgumentosDefinicionFecha[i], listaArgumentosDefinicionFecha[i].OrdenEnFormula);
            }
        }

        /// <summary>
        /// 06/02/2023
        /// José Navarro Acuña
        /// Función que procesa objectos argumentos y los traspasa al modelo de datos. Desencripta IDs y valida el contenido de cada argumento.
        /// </summary>
        /// <param name="pListaArgumentosDTO"></param>
        /// <param name="pIdFormula"></param>
        /// <returns></returns>
        private RespuestaConsulta<List<ArgumentoFormula>> ProcesarArgumentosEnDTO(List<ArgumentoConstruidoDTO> pListaArgumentosDTO, int pIdFormula)
        {
            List<ArgumentoFormula> argumentosValidados = new List<ArgumentoFormula>();
            RespuestaConsulta<List<ArgumentoFormula>> respuesta = new RespuestaConsulta<List<ArgumentoFormula>>();

            for (int i = 0; i < pListaArgumentosDTO.Count; i++)
            {
                ArgumentoFormula argumento = PrepararObjetoArgumentoFormula(pListaArgumentosDTO[i]);

                if (argumento != null) // obviar operadores de suma, resta, división, etc
                {
                    argumento.Etiqueta = pListaArgumentosDTO[i].Etiqueta;
                    argumento.IdFormulaCalculo = pIdFormula;
                    argumentosValidados.Add(argumento);

                    // se valida la información proporcionada para cada argumento
                    RespuestaConsulta<FormulaCalculo> validacionArgumento = ValidarDatosArgumentoEnFormula(argumento);

                    if (validacionArgumento.HayError != (int)Error.NoError)
                    {
                        respuesta.HayError = (int)Error.ErrorControlado;
                        respuesta.MensajeError = validacionArgumento.MensajeError;
                        return respuesta;
                    }
                }
                else // +, -, *, /, (, ), etc..
                {
                    argumentosValidados.Add(new ArgumentoFormula() {
                        EsOperadorMatematico = true,
                        Etiqueta = pListaArgumentosDTO[i].Etiqueta
                    });
                }
            }
            respuesta.objetoRespuesta = argumentosValidados;
            return respuesta;
        }

        /// <summary>
        /// 30/01/2023
        /// José Navarro Acuña
        /// Función que permite registrar un argumento variable dato/criterio en una fórmula dada
        /// </summary>
        /// <param name="pPredicadoSQL"></param>
        /// <param name="pFormulasCalculo"></param>
        /// <param name="pFormulasVariableDatoCriterio"></param>
        /// <param name="pOrden"></param>
        private void InsertarArgumentoVariableDatoCriterio(string pPredicadoSQL, FormulaCalculo pFormulasCalculo, FormulaVariableDatoCriterio pFormulasVariableDatoCriterio, int pOrden)
        {
            FormulaVariableDatoCriterio argumento = formulasVariableDatoCriterioDAL.ActualizarDatos(pFormulasVariableDatoCriterio);

            if (argumento != null)
            {
                argumentoFormulaDAL.ActualizarDatos(new ArgumentoFormula() {
                    IdFormulaVariableDatoCriterio = argumento.IdFormulaVariableDatoCriterio,
                    IdFormulaTipoArgumento = (int)FormulasTipoArgumentoEnum.VariableDatoCriterio,
                    IdFormulaCalculo = pFormulasCalculo.IdFormulaCalculo,
                    PredicadoSQL = pPredicadoSQL,
                    OrdenEnFormula = pOrden,
                    Etiqueta = pFormulasVariableDatoCriterio.Etiqueta
                });
            }
        }

        /// <summary>
        /// 30/01/2023
        /// José Navarro Acuña
        /// Función que permite registrar un argumento definición de fecha en una fórmula dada
        /// </summary>
        /// <param name="pPredicadoSQL"></param>
        /// <param name="pFormulasCalculo"></param>
        /// <param name="pFormulasDefinicionFecha"></param>
        /// <param name="pOrden"></param>
        private void InsertarArgumentoDefinicionDeFecha(string pPredicadoSQL, FormulaCalculo pFormulasCalculo, FormulaDefinicionFecha pFormulasDefinicionFecha, int pOrden)
        {
            FormulaDefinicionFecha argumento = formulasDefinicionFechaDAL.ActualizarDatos(pFormulasDefinicionFecha);

            if (argumento != null)
            {
                argumentoFormulaDAL.ActualizarDatos(new ArgumentoFormula() {
                    IdFormulaDefinicionFecha = argumento.IdFormulaDefinicionFecha,
                    IdFormulaTipoArgumento = (int)FormulasTipoArgumentoEnum.DefinicionFecha,
                    IdFormulaCalculo = pFormulasCalculo.IdFormulaCalculo,
                    PredicadoSQL = pPredicadoSQL,
                    OrdenEnFormula = pOrden,
                    Etiqueta = pFormulasDefinicionFecha.Etiqueta
                });
            }
        }

        /// <summary>
        /// 24/10/2022
        /// José Navarro Acuña
        /// Prepara un objeto de fórmula para ser enviado al servicio DAL.
        /// Se preparan los id's de las tablas relacionadas para poder efectuar consultas debido a la encriptación.
        /// </summary>
        /// <param name="pIndicador"></param>
        private void PrepararObjetoFormulaCalculo(FormulaCalculo pFormulasCalculo)
        {
            if (!string.IsNullOrEmpty(pFormulasCalculo.id))
            {
                int.TryParse(Utilidades.Desencriptar(pFormulasCalculo.id), out int number);
                pFormulasCalculo.IdFormulaCalculo = number;
            }

            if (!string.IsNullOrEmpty(pFormulasCalculo.IdFrecuenciaString))
            {
                int.TryParse(Utilidades.Desencriptar(pFormulasCalculo.IdFrecuenciaString), out int number);
                pFormulasCalculo.IdFrecuenciaEnvio = number;
            }

            if (!string.IsNullOrEmpty(pFormulasCalculo.IdIndicadorSalidaString))
            {
                int.TryParse(Utilidades.Desencriptar(pFormulasCalculo.IdIndicadorSalidaString), out int number);
                pFormulasCalculo.IdIndicador = number;
            }

            if (!string.IsNullOrEmpty(pFormulasCalculo.IdVariableDatoString))
            {
                int.TryParse(Utilidades.Desencriptar(pFormulasCalculo.IdVariableDatoString), out int number);
                pFormulasCalculo.IdDetalleIndicadorVariable = number;
            }

            if (!pFormulasCalculo.NivelCalculoTotal && pFormulasCalculo.ListaCategoriasNivelesCalculo.Count > 0)
            {
                for (int i = 0; i < pFormulasCalculo.ListaCategoriasNivelesCalculo.Count; i++)
                {
                    int.TryParse(Utilidades.Desencriptar(pFormulasCalculo.ListaCategoriasNivelesCalculo[i].IdCategoriaString), out int number);
                    pFormulasCalculo.ListaCategoriasNivelesCalculo[i].IdCategoriaDesagregacion = number;
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
                argumento.IdFormulaTipoArgumento = (int)FormulasTipoArgumentoEnum.VariableDatoCriterio;
                FormulaVariableDatoCriterio argVariableDato = (FormulaVariableDatoCriterio)argumento; // casteo explicito

                if (!string.IsNullOrEmpty(argVariableDato.IdFuenteIndicadorString))
                {
                    int.TryParse(Utilidades.Desencriptar(argVariableDato.IdFuenteIndicadorString), out int number);
                    argVariableDato.IdFuenteIndicador = number;
                }

                if (argVariableDato.IdFuenteIndicador == (int)FuenteIndicadorEnum.IndicadorDGF)
                {
                    if (!string.IsNullOrEmpty(argVariableDato.IdVariableDatoString))
                    {
                        int.TryParse(Utilidades.Desencriptar(argVariableDato.IdVariableDatoString), out int number);
                        argVariableDato.IdDetalleIndicadorVariable = number;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(argVariableDato.IdCriterioString))
                    {
                        argVariableDato.IdCriterio = Utilidades.Desencriptar(argVariableDato.IdCriterioString);
                    }
                }

                if (!string.IsNullOrEmpty(argVariableDato.IdCategoriaString))
                {
                    int.TryParse(Utilidades.Desencriptar(argVariableDato.IdCategoriaString), out int number);
                    argVariableDato.IdCategoriaDesagregacion = number;
                }

                if (!string.IsNullOrEmpty(argVariableDato.IdDetalleCategoriaString))
                {
                    int.TryParse(Utilidades.Desencriptar(argVariableDato.IdDetalleCategoriaString), out int number);
                    argVariableDato.IdDetalleCategoriaTexto = number;
                }

                if (!string.IsNullOrEmpty(argVariableDato.IdAcumulacionString))
                {
                    int.TryParse(Utilidades.Desencriptar(argVariableDato.IdAcumulacionString), out int number);
                    argVariableDato.IdAcumulacionFormula = number;
                }

                if (!string.IsNullOrEmpty(argVariableDato.IdIndicadorString))
                {
                    argVariableDato.IdIndicador = Utilidades.Desencriptar(argVariableDato.IdIndicadorString);
                }
            }
            else if (pArgumentoDTO.TipoArgumento == FormulasTipoArgumentoEnum.DefinicionFecha) // FormulasTipoArgumentoEnum.DefinicionFecha
            {
                argumento = pArgumentoDTO.ConvertToFormulasDefinicionFecha();
                argumento.IdFormulaTipoArgumento = (int)FormulasTipoArgumentoEnum.DefinicionFecha;
                FormulaDefinicionFecha argDefinicionFecha = (FormulaDefinicionFecha)argumento;

                if (!string.IsNullOrEmpty(argDefinicionFecha.IdTipoFechaInicioString))
                {
                    int.TryParse(Utilidades.Desencriptar(argDefinicionFecha.IdTipoFechaInicioString), out int number);
                    argDefinicionFecha.IdTipoFechaInicio = number;
                }

                if (!string.IsNullOrEmpty(argDefinicionFecha.IdTipoFechaFinalString))
                {
                    int.TryParse(Utilidades.Desencriptar(argDefinicionFecha.IdTipoFechaFinalString), out int number);
                    argDefinicionFecha.IdTipoFechaFinal = number;
                }

                if (!string.IsNullOrEmpty(argDefinicionFecha.IdCategoriaInicioString))
                {
                    int.TryParse(Utilidades.Desencriptar(argDefinicionFecha.IdCategoriaInicioString), out int number);
                    argDefinicionFecha.IdCategoriaDesagregacionInicio = number;
                }

                if (!string.IsNullOrEmpty(argDefinicionFecha.IdCategoriaFinalString))
                {
                    int.TryParse(Utilidades.Desencriptar(argDefinicionFecha.IdCategoriaFinalString), out int number);
                    argDefinicionFecha.IdCategoriaDesagregacionFinal = number;
                }

                if (!string.IsNullOrEmpty(argDefinicionFecha.IdIndicadorString))
                {
                    int.TryParse(Utilidades.Desencriptar(argDefinicionFecha.IdIndicadorString), out int number);
                    argDefinicionFecha.IdIndicador = number;
                }
            }
            return argumento;
        }

        /// <summary>
        /// 13/02/2023
        /// José Navarro Acuña
        /// Función que verifica si todos los campos de una fórmula estan completos, e incluidos al menos 1 argumento en la fórmula
        /// </summary>
        /// <returns></returns>
        private string VerificarDatosCompletosFormulaCalculo(FormulaCalculo pFormulasCalculo)
        {
            if (
                pFormulasCalculo.Codigo == null || string.IsNullOrEmpty(pFormulasCalculo.Codigo.Trim()) ||
                pFormulasCalculo.Nombre == null || string.IsNullOrEmpty(pFormulasCalculo.Nombre.Trim()) ||
                pFormulasCalculo.IdIndicador == null || pFormulasCalculo.IdIndicador == 0 ||
                pFormulasCalculo.IdDetalleIndicadorVariable == null || pFormulasCalculo.IdDetalleIndicadorVariable == 0 ||
                pFormulasCalculo.IdFrecuenciaEnvio == null || pFormulasCalculo.IdFrecuenciaEnvio == 0 ||
                pFormulasCalculo.Descripcion == null || string.IsNullOrEmpty(pFormulasCalculo.Descripcion.Trim()) ||
                pFormulasCalculo.FechaCalculo == null || pFormulasCalculo.FechaCalculo <= DateTime.MinValue
                )
            {
                return Errores.CamposIncompletos;
            }

            List<FormulaVariableDatoCriterio> listadoVariables = formulasVariableDatoCriterioDAL.ObtenerDatos(new FormulaVariableDatoCriterio() { IdFormulaCalculo = pFormulasCalculo.IdFormulaCalculo });
            List<FormulaDefinicionFecha> listadoDefinicionFechas = formulasDefinicionFechaDAL.ObtenerDatos(new FormulaDefinicionFecha() { IdFormulaCalculo = pFormulasCalculo.IdFormulaCalculo });

            if (listadoVariables.Count <= 0 && listadoDefinicionFechas.Count <= 0) // existen argumentos?
            {
                return Errores.CamposIncompletos;
            }

            if (!pFormulasCalculo.NivelCalculoTotal) // en caso de que el nivel de cálculo sea por categorias, existen categorias relacionadas?
            {
                List<FormulaNivelCalculoCategoria> listaCategorias = formulaNivelCalculoCategoriaDAL.ObtenerDatos(pFormulasCalculo.IdFormulaCalculo);

                if (listaCategorias.Count <= 0)
                {
                    return Errores.CamposIncompletos;
                }
            }

            return string.Empty;
        }

        #endregion
    }
}
