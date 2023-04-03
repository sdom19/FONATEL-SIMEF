using GB.SIMEF.Entities;
using System;
using System.Globalization;

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

            DateTime fechaInicio = (DateTime)(argumentoFecha.FechaInicio != null ? argumentoFecha.FechaInicio : DateTime.MinValue);
            DateTime fechaFinal = (DateTime)(argumentoFecha.FechaFinal != null ? argumentoFecha.FechaFinal : DateTime.MinValue);

            return string.Format(
                "EXEC pa_ConstruirArgumentoSalidaFecha {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}",
                argumentoFecha.IdUnidadMedida,
                
                argumentoFecha.IdTipoFechaInicio,
                ConvertirFechaAFormatoSQL(fechaInicio),
                argumentoFecha.IdCategoriaDesagregacionInicio != null ? argumentoFecha.IdCategoriaDesagregacionInicio : 0,
                
                argumentoFecha.IdTipoFechaFinal,
                ConvertirFechaAFormatoSQL(fechaFinal),
                argumentoFecha.IdCategoriaDesagregacionFinal != null ? argumentoFecha.IdCategoriaDesagregacionFinal : 0,

                argumentoFecha.IdIndicador,

                pFormulasCalculo.IdIndicador
            );
        }

        /// <summary>
        /// 31/03/2023
        /// José Navarro Acuña
        /// Función que permite convertir una fecha a un compatible con SQL
        /// </summary>
        /// <returns></returns>
        private string ConvertirFechaAFormatoSQL(DateTime pFecha)
        {
            return "'" + pFecha.ToString("MM/dd/yyyy") + "'";
        }
    }
}
