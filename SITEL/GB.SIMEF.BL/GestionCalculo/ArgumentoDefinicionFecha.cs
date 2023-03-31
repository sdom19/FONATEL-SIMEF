using GB.SIMEF.Entities;
using System;

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
                "EXEC pa_ConstruirArgumentoSalidaFecha {0}, {1}, {2}, {3}, {4}, {5}, {6}",
                argumentoFecha.IdUnidadMedida,
                
                argumentoFecha.IdTipoFechaInicio,
                argumentoFecha.FechaInicio != null ? argumentoFecha.FechaInicio.ToString() : DateTime.MinValue.ToString(),
                argumentoFecha.IdCategoriaDesagregacionInicio != null ? argumentoFecha.IdCategoriaDesagregacionInicio : 0,
                
                argumentoFecha.IdTipoFechaFinal,
                argumentoFecha.FechaFinal != null ? argumentoFecha.FechaFinal.ToString() : DateTime.MinValue.ToString(),
                argumentoFecha.IdCategoriaDesagregacionFinal != null ? argumentoFecha.IdCategoriaDesagregacionFinal : 0,

                argumentoFecha.IdIndicador
            );
        }
    }
}
