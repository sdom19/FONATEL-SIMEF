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
        public string ConstruirPredicadoSQL(ArgumentoFormula pArgumentoFormula)
        {
            FormulasVariableDatoCriterio argumentoVariable = (FormulasVariableDatoCriterio)pArgumentoFormula;
            string predicadoSQL = string.Empty;

            if (argumentoVariable.EsValorTotal)
            {
                predicadoSQL = string.Format(
                    PredicadosSQLFormulasCalculo.fonatelValorTotal,
                    argumentoVariable.IdVariableDato,
                    argumentoVariable.IdIndicador,
                    argumentoVariable.IdFormula
                    );
            }
            else // detalle de desagregación
            {
                predicadoSQL = string.Format(
                    PredicadosSQLFormulasCalculo.fonatelDetalleDesagregacion,
                    argumentoVariable.IdVariableDato,
                    argumentoVariable.IdIndicador,
                    argumentoVariable.IdCategoria,
                    argumentoVariable.IdDetalleCategoria,
                    argumentoVariable.IdFormula
                    );
            }
            return predicadoSQL;
        }
    }
}
