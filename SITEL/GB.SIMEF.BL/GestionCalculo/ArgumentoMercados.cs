using GB.SIMEF.Entities;

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

            return string.Format(
                "EXEC FONATEL.pa_ConstruirArgumentoSalidaVariable {0}, {1}",
                argumentoCriterio.IdCriterio,
                argumentoCriterio.EsValorTotal ? "null" : argumentoCriterio.IdDetalleCategoriaTexto?.ToString()
            );
        }
    }
}
