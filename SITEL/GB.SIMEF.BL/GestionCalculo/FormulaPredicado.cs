using GB.SIMEF.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.BL.GestionCalculo
{
    /// <summary>
    /// 20/01/2023
    /// José Navarro Acuña
    /// Clase contexto para dar uso al patrón de comportamiento Strategy
    /// </summary>
    public class FormulaPredicado
    {
        private IArgumento argumento; // mantener una referencia al tipo de argumento a construir 
        private ArgumentoFormula argumentoFormula;

        /// <summary>
        /// 21/01/2023
        /// José Navarro Acuña
        /// Establecer el tipo de argumento matemetico.
        /// </summary>
        /// <param name="argumento"></param>
        public void SetFuenteArgumento(IArgumento pIArgumento)
        {
            argumento = pIArgumento; // permite intercambiar la funcionalidad de construir el predicado en tiempo de ejecución
        }

        /// <summary>
        /// 21/01/2023
        /// José Navarro Acuña
        /// Establecer el objeto ArgumentoFormula
        /// </summary>
        /// <param name="pFormulasVariableDatoCriterio"></param>
        public void SetArgumentoFormula(ArgumentoFormula pArgumentoFormula)
        {
            argumentoFormula = pArgumentoFormula;
        }

        /// <summary>
        /// 21/01/2023
        /// José Navarro Acuña
        /// Ejecutar una llamada que construye el predicado SQL de acuerdo al tipo argumento
        /// </summary>
        /// <returns></returns>
        public string GetArgumentoComoPredicadoSQL()
        {
            return argumento.ConstruirPredicadoSQL(argumentoFormula); // nótese que no nos importa cúal argumento es, por tanto se delega comportamiento
        }
    }
}
