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
        public string ConstruirPredicadoSQL(ArgumentoFormula pArgumentoFormula, FormulasCalculo pFormulasCalculo)
        {
            FormulasDefinicionFecha argumentoFecha = (FormulasDefinicionFecha)pArgumentoFormula;

            return string.Format(
                PredicadosSQLFormulasCalculo.fonatel_definicionFechas,
                argumentoFecha.IdUnidadMedida,
                
                argumentoFecha.IdTipoFechaInicio,
                argumentoFecha.FechaInicio != null ? argumentoFecha.FechaInicio.ToString() : DateTime.MinValue.ToString(),
                argumentoFecha.IdCategoriaInicio != null ? argumentoFecha.IdCategoriaInicio : 0,
                
                argumentoFecha.IdTipoFechaFinal,
                argumentoFecha.FechaFinal != null ? argumentoFecha.FechaFinal.ToString() : DateTime.MinValue.ToString(),
                argumentoFecha.IdCategoriaFinal != null ? argumentoFecha.IdCategoriaFinal : 0
                );
        }
    }
}
