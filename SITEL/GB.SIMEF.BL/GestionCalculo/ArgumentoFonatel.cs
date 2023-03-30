using GB.SIMEF.Entities;
using System;
using GB.SIMEF.Resources;

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
            string predicadoSQL = string.Empty;

            if (argumentoVariable.EsValorTotal)
            {
                predicadoSQL = string.Format(
                    PredicadosSQLFormulasCalculo.fonatel_variablesDatoCriterio,
                    pFormulasCalculo.IdIndicador,
                    argumentoVariable.IdAcumulacionFormula,
                    argumentoVariable.IdDetalleIndicadorVariable,
                    "null",
                    "null"
                );
            }
            else // detalle de desagregación
            {
                predicadoSQL = string.Format(
                    PredicadosSQLFormulasCalculo.fonatel_variablesDatoCriterio,
                    pFormulasCalculo.IdIndicador,
                    argumentoVariable.IdAcumulacionFormula,
                    argumentoVariable.IdDetalleIndicadorVariable,
                    argumentoVariable.IdCategoriaDesagregacion,
                    argumentoVariable.IdDetalleCategoriaTexto
                );
            }
            return predicadoSQL;
        }
    }
}
