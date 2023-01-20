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
    /// Contexto para dar uso al patrón funcional Strategy
    /// </summary>
    public class FormulaPredicado
    {
        private IArgumento argumento; // mantener una referencia al tipo de argumento a construir 

        /// <summary>
        /// Establecer el tipo de argumento matemetico.
        /// </summary>
        /// <param name="argumento"></param>
        public void SetArgumento(IArgumento argumento)
        {
            this.argumento = argumento; // permite intercambiar la funcionalidad de construir el predicado en tiempo de ejecución
        }

        /// <summary>
        /// Ejecutar una llamada que construye el predicado SQL de acuerdo al tipo argumento
        /// </summary>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public string GetArgumentoComoPredicadoSQL()
        {
            return argumento.ConstruirPredicadoSQL(); // nótese que no nos importa cual argumento es, por tanto aqui se delega comportamiento
        }
    }
}
