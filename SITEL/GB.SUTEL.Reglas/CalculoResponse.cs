using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SUTEL.Reglas
{
    public class CalculoResponse
    {
        /// <summary>
        /// Almacena el error generado
        /// </summary>
        public string Error { get; set; }
        /// <summary>
        /// Representa el código que se está resolviendo
        /// </summary>
        public string CodProcesar { get; set; }
        public string CodC { get; set; }
        public List<ReglasResult> ReglasResult { get; set; }
        public List<FormulasResult> FormulasResult { get; set; }
    }
}
