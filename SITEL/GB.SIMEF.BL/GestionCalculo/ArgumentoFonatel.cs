using GB.SIMEF.Entities;

namespace GB.SIMEF.BL.GestionCalculo
{
    public class ArgumentoFonatel : IArgumento
    {
        /// <summary>
        /// 20/01/2023
        /// José Navarro Acuña
        /// Función que permite construir el predicado SQL del argumento
        /// </summary>
        /// <returns></returns>
        public string ConstruirPredicadoSQL(ArgumentoFormula pArgumentoFormula, FormulaCalculo pFormulasCalculo)
        {
            FormulaVariableDatoCriterio argumentoVariable = (FormulaVariableDatoCriterio)pArgumentoFormula;

            return string.Format(
                "EXEC pa_ConstruirArgumentoSalidaVariable {0}, {1}, {2}, {3}, {4}, {5}",
                pFormulasCalculo.IdIndicador,
                argumentoVariable.IdAcumulacionFormula,
                argumentoVariable.IdDetalleIndicadorVariable,
                argumentoVariable.EsValorTotal ? "null" : argumentoVariable.IdCategoriaDesagregacion?.ToString(),
                argumentoVariable.EsValorTotal ? "null" : argumentoVariable.IdDetalleCategoriaTexto?.ToString(),
                argumentoVariable.IdIndicador
            );
        }
    }
}
