using GB.SIMEF.Entities;
using System;
using GB.SIMEF.Resources;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL.GestionCalculo
{
    public class ArgumentoCalidad : IArgumento
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

            if (argumentoVariable.IdCriterio.Equals(((int)TipoPorcentajeIndicadorCalculoEnum.indicador).ToString()))
            {
                predicadoSQL = PredicadosSQLFormulasCalculo.calidadPorcentajeIndicador;
            }
            else if (argumentoVariable.IdCriterio.Equals(((int)TipoPorcentajeIndicadorCalculoEnum.cumplimiento).ToString()))
            {
                predicadoSQL = PredicadosSQLFormulasCalculo.calidadPorcentajeCumplimiento;
            }
            return predicadoSQL != string.Empty ? string.Format(predicadoSQL, argumentoVariable.IdIndicador) : string.Empty;
        }
    }
}
