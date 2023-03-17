using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Entities.DTO;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL
{
    public class ArgumentoFormulaBL
    {
        private readonly FormulasVariableDatoCriterioDAL formulasVariableDatoCriterioDAL;
        private readonly FormulasDefinicionFechaDAL formulasDefinicionFechaDAL;
        private readonly FormulasCalculoDAL formulasCalculoDAL;

        public ArgumentoFormulaBL()
        {
            formulasVariableDatoCriterioDAL = new FormulasVariableDatoCriterioDAL();
            formulasDefinicionFechaDAL = new FormulasDefinicionFechaDAL();
            formulasCalculoDAL = new FormulasCalculoDAL();
        }

        /// <summary>
        /// 10/02/2023
        /// José Navarro Acuña
        /// Función que permite obtener los argumentos de una fórmula. 
        /// Se incluyen operadores matemáticos y números.
        /// </summary>
        /// <param name="pIdFormula"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<ArgumentoConstruidoDTO>> ObtenerArgumentosCompletosDeFormula(string pIdFormula)
        {
            RespuestaConsulta<List<ArgumentoConstruidoDTO>> resultado = new RespuestaConsulta<List<ArgumentoConstruidoDTO>>();

            try
            {
                int idFormula = 0;
                if (!string.IsNullOrEmpty(pIdFormula))
                {
                    int.TryParse(Utilidades.Desencriptar(pIdFormula), out int number);
                    idFormula = number;
                }
                else
                {
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                if (idFormula == 0)
                {
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                FormulaCalculo formula = null;

                formula = formulasCalculoDAL.ObtenerDatos(new FormulaCalculo() { IdFormula = idFormula }).Single();
                List<FormulaVariableDatoCriterio> listadoVariables = formulasVariableDatoCriterioDAL.ObtenerDatos(new FormulaVariableDatoCriterio() { IdFormula = idFormula });
                List<FormulaDefinicionFecha> listadoDefinicionFechas = formulasDefinicionFechaDAL.ObtenerDatos(new FormulaDefinicionFecha() { IdFormula = idFormula });
                
                List<ArgumentoFormula> listaArgumentos = new List<ArgumentoFormula>();
                listaArgumentos.AddRange(listadoVariables);
                listaArgumentos.AddRange(listadoDefinicionFechas);

                if (string.IsNullOrEmpty(formula.Formula) || listaArgumentos.Count <= 0) // sin argumentos registrados
                {
                    resultado.objetoRespuesta = new List<ArgumentoConstruidoDTO>();
                    return resultado;
                }

                resultado.objetoRespuesta = ConstruirListadoArgumentosDTO(listaArgumentos, formula.Formula);
            }
            catch (Exception ex)
            {
                resultado.HayError = (int)Error.ErrorSistema;
            }
            return resultado;
        }

        /// <summary>
        /// 13/02/2023
        /// José Navarro Acuña
        /// Función que permite construir, a partir de un listado de argumentos basados en el modelo de datos, su respectiva representación en el modelo DTO
        /// para ser consumido en las vista. 
        /// Como ejemplo, esta función construye la misma estructura que envia el front cuando guarda los detalles/argumentos de una fórmula
        /// </summary>
        /// <param name="pListaArgumentos"></param>
        /// <param name="pEtiquetaFormula"></param>
        /// <returns></returns>
        private List<ArgumentoConstruidoDTO> ConstruirListadoArgumentosDTO(List<ArgumentoFormula> pListaArgumentos, string pEtiquetaFormula)
        {
            List<ArgumentoConstruidoDTO> argumentosConstruidos = new List<ArgumentoConstruidoDTO>();
            int cantidadArgumentos = 0, cantidadOperadores = 0, cantidadNumeros = 0;

            MatchCollection matchesArgumentos = Regex.Matches(pEtiquetaFormula, @"\{[0-9]+\}", RegexOptions.Compiled);
            MatchCollection matchesOperadores = Regex.Matches(pEtiquetaFormula, @"[+\-*\/\<\>=\(\)]", RegexOptions.Compiled);
            MatchCollection matchesNumeros = Regex.Matches(pEtiquetaFormula, @"[0-9]+", RegexOptions.Compiled);

            for (int i = 0; i < pEtiquetaFormula.Length; i++)
            {
                string caracter = pEtiquetaFormula[i].ToString();

                if (matchesArgumentos.Count > cantidadArgumentos && matchesArgumentos[cantidadArgumentos].Captures[0].Index == i)
                {
                    ArgumentoFormula argumentoAInsertar = pListaArgumentos.Find(x => x.OrdenEnFormula == cantidadArgumentos);
                    argumentosConstruidos.Add(new ArgumentoConstruidoDTO()
                    {
                        TipoObjeto = (int)FormulasTipoObjetoEnum.Variable,
                        Etiqueta = argumentoAInsertar.Etiqueta,
                        TipoArgumento = (FormulasTipoArgumentoEnum)argumentoAInsertar.IdFormulasTipoArgumento,
                        Argumento = EncriptarObjetoArgumento(argumentoAInsertar)
                    });

                    i += matchesArgumentos[cantidadArgumentos].Captures[0].Value.Length - 1; // calcular la cantidad de caracteres a obviar
                    cantidadArgumentos++;
                    cantidadNumeros++; // el regex de numeros tambien contempla argumentos, por tanto, avanzar en el indice para obviarlo
                }
                else if (matchesOperadores.Count > cantidadOperadores && matchesOperadores[cantidadOperadores].Captures[0].Index == i)
                {
                    argumentosConstruidos.Add(new ArgumentoConstruidoDTO()
                    {
                        TipoObjeto = (int)FormulasTipoObjetoEnum.Operador,
                        Etiqueta = caracter
                    });

                    i += matchesOperadores[cantidadOperadores].Captures[0].Value.Length - 1;
                    cantidadOperadores++;
                }
                else if (matchesNumeros.Count > cantidadNumeros && matchesNumeros[cantidadNumeros].Captures[0].Index == i)
                {
                    string numeroParseado = matchesNumeros[cantidadNumeros].Captures[0].Value;

                    argumentosConstruidos.Add(new ArgumentoConstruidoDTO()
                    {
                        TipoObjeto = (int)FormulasTipoObjetoEnum.Numero,
                        Etiqueta = numeroParseado
                    });

                    i += numeroParseado.Length - 1;
                    cantidadNumeros++;
                }
            }
            return argumentosConstruidos;
        }

        /// <summary>
        /// 10/02/2023
        /// José Navarro Acuña
        /// Función que encripta los datos del modelo ArgumentoFormula y devuelve su respectivo DTO.
        /// </summary>
        /// <param name="pArgumentoFormula"></param>
        /// <returns></returns>
        private ArgumentoDTO EncriptarObjetoArgumento(ArgumentoFormula pArgumentoFormula)
        {
            if (pArgumentoFormula.IdFormulasTipoArgumento == (int) FormulasTipoArgumentoEnum.VariableDatoCriterio)
            {
                FormulaVariableDatoCriterio variableDatoCriterio = (FormulaVariableDatoCriterio)pArgumentoFormula;
                return new ArgumentoDTO() 
                { 
                    fuente = Utilidades.Encriptar(variableDatoCriterio.IdFuenteIndicador.ToString()),
                    indicador = Utilidades.Encriptar(variableDatoCriterio.IdIndicador.ToString()),
                    //codigoIndicador = variableDatoCriterio
                    variableDatoCriterio = variableDatoCriterio.IdDetalleIndicadorVariable != null ? Utilidades.Encriptar(variableDatoCriterio.IdDetalleIndicadorVariable.ToString()) : Utilidades.Encriptar(variableDatoCriterio.IdCriterio.ToString()),
                    //nombreVariable = variableDatoCriterio
                    categoria = variableDatoCriterio.IdCategoriaDesagregacion != null ? Utilidades.Encriptar(variableDatoCriterio.IdCategoriaDesagregacion.ToString()) : null,
                    detalle = variableDatoCriterio.IdDetalleCategoriaTexto != null ? Utilidades.Encriptar(variableDatoCriterio.IdDetalleCategoriaTexto.ToString()) : null,
                    acumulacion = variableDatoCriterio.IdAcumulacionFormula != null ? Utilidades.Encriptar(variableDatoCriterio.IdAcumulacionFormula.ToString()) : null,
                    valorTotal = variableDatoCriterio.EsValorTotal
                };
            }
            else if (pArgumentoFormula.IdFormulasTipoArgumento == (int)FormulasTipoArgumentoEnum.DefinicionFecha)
            {
                FormulaDefinicionFecha definicionFecha = (FormulaDefinicionFecha)pArgumentoFormula;
                return new ArgumentoDTO()
                {
                    indicador = Utilidades.Encriptar(definicionFecha.IdIndicador.ToString()),
                    unidadMedida = definicionFecha.IdUnidadMedida,
                    tipoFechaInicio = Utilidades.Encriptar(definicionFecha.IdTipoFechaInicio.ToString()),
                    fechaInicio = definicionFecha.FechaInicio != null ? (DateTime)definicionFecha.FechaInicio : DateTime.MinValue,
                    categoriaInicio = definicionFecha.IdCategoriaDesagregacionInicio != null ? Utilidades.Encriptar(definicionFecha.IdCategoriaDesagregacionInicio?.ToString()) : null,
                    //nombreCategoriaInicio 
                    tipoFechaFinal = Utilidades.Encriptar(definicionFecha.IdTipoFechaFinal.ToString()),
                    fechaFinal = definicionFecha.FechaFinal != null ? (DateTime)definicionFecha.FechaFinal : DateTime.MinValue,
                    categoriaFinal = definicionFecha.IdCategoriaDesagregacionFinal != null ? Utilidades.Encriptar(definicionFecha.IdCategoriaDesagregacionFinal?.ToString()) : null
                    //nombreCategoriaFinal
                };
            }
            return new ArgumentoDTO();
        }
    }
}
