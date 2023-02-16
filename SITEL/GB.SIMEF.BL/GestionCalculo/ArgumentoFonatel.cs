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
        public string ConstruirPredicadoSQL(ArgumentoFormula pArgumentoFormula, FormulasCalculo pFormulasCalculo)
        {
            FormulasVariableDatoCriterio argumentoVariable = (FormulasVariableDatoCriterio)pArgumentoFormula;
            string predicadoSQL = string.Empty;

            if (argumentoVariable.EsValorTotal)
            {
                predicadoSQL = string.Format(
                    PredicadosSQLFormulasCalculo.fonatel_variablesDatoCriterio,
                    pFormulasCalculo.IdIndicador,
                    argumentoVariable.IdAcumulacion,
                    argumentoVariable.IdVariableDato,
                    argumentoVariable.IdIndicador,
                    "null",
                    "null",
                    pFormulasCalculo.IdFormula
                );
            }
            else // detalle de desagregación
            {
                predicadoSQL = string.Format(
                    PredicadosSQLFormulasCalculo.fonatel_variablesDatoCriterio,
                    pFormulasCalculo.IdIndicador,
                    argumentoVariable.IdAcumulacion,
                    argumentoVariable.IdVariableDato,
                    argumentoVariable.IdIndicador,
                    argumentoVariable.IdCategoria,
                    argumentoVariable.IdDetalleCategoria,
                    pFormulasCalculo.IdFormula
                );
            }
            return predicadoSQL;
        }
    }
}
