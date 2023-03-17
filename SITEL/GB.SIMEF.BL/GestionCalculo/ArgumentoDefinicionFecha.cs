using GB.SIMEF.Entities;
using System;
using GB.SIMEF.Resources;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL.GestionCalculo
{
    public class ArgumentoDefinicionFecha : IArgumento
    {
        /// <summary>
        /// 20/01/2023
        /// José Navarro Acuña
        /// Función que permite construir el predicado SQL del argumento
        /// </summary>
        /// <returns></returns>
        public string ConstruirPredicadoSQL(ArgumentoFormula pArgumentoFormula, FormulaCalculo pFormulasCalculo)
        {
            FormulaDefinicionFecha argumentoFecha = (FormulaDefinicionFecha)pArgumentoFormula;

            return string.Format(
                PredicadosSQLFormulasCalculo.fonatel_definicionFechas,
                argumentoFecha.IdUnidadMedida,
                
                argumentoFecha.IdTipoFechaInicio,
                argumentoFecha.FechaInicio != null ? argumentoFecha.FechaInicio.ToString() : DateTime.MinValue.ToString(),
                argumentoFecha.IdCategoriaDesagregacionInicio != null ? argumentoFecha.IdCategoriaDesagregacionInicio : 0,
                
                argumentoFecha.IdTipoFechaFinal,
                argumentoFecha.FechaFinal != null ? argumentoFecha.FechaFinal.ToString() : DateTime.MinValue.ToString(),
                argumentoFecha.IdCategoriaDesagregacionFinal != null ? argumentoFecha.IdCategoriaDesagregacionFinal : 0
                );
        }
    }
}
