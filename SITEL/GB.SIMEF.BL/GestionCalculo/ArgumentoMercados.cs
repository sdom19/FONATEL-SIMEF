using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.BL.GestionCalculo
{
    public class ArgumentoMercados : IArgumento
    {
        /// <summary>
        /// 21/01/2023
        /// José Navarro Acuña
        /// Función que permite construir el predicado SQL del argumento
        /// </summary>
        /// <returns></returns>
        public string ConstruirPredicadoSQL(ArgumentoFormula pArgumentoFormula, FormulaCalculo pFormulasCalculo)
        {
            FormulaVariableDatoCriterio argumentoCriterio = (FormulaVariableDatoCriterio)pArgumentoFormula;
            string predicadoSQL = string.Empty;

            if (argumentoCriterio.EsValorTotal)
            {
                predicadoSQL = string.Format(
                    PredicadosSQLFormulasCalculo.mercados,
                    argumentoCriterio.IdCriterio,
                    "null"
                );
            }
            else
            {
                predicadoSQL = string.Format(
                    PredicadosSQLFormulasCalculo.mercados,
                    argumentoCriterio.IdCriterio,
                    argumentoCriterio.IdDetalleCategoriaTexto
                );
            }
            return predicadoSQL;
        }
    }
}
