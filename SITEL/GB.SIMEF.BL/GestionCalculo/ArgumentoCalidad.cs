using GB.SIMEF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public string ConstruirPredicadoSQL(ArgumentoFormula pArgumentoFormula)
        {
            FormulasVariableDatoCriterio argumentoVariable = (FormulasVariableDatoCriterio)pArgumentoFormula;
            string predicadoSQL = string.Empty;

            if (argumentoVariable.IdCriterio == (int)TipoPorcentajeIndicadorCalculoEnum.indicador)
            {
                predicadoSQL = "SELECT SUM(PorcentInd) FROM [CalidadIndicadorCalculo].[dbo].[FactRigurosidadFac] where IdIndicador = '{0}'";
            }
            else if (argumentoVariable.IdCriterio == (int)TipoPorcentajeIndicadorCalculoEnum.cumplimiento)
            {
                predicadoSQL = "SELECT SUM(PorcCumpl) FROM [CalidadIndicadorCalculo].[dbo].[FactRigurosidadFac] where IdIndicador = '{0}'";
            }
            return predicadoSQL != string.Empty ? string.Format(predicadoSQL, argumentoVariable.IdIndicador) : string.Empty;
        }
    }
}
