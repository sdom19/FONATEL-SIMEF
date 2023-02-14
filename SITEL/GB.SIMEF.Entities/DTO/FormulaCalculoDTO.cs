using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.Entities.DTO
{
    public class FormulaCalculoDTO
    {
        public string NombreIndicador { get; set; }
        public string NombreVariableDato { get; set; }
        public DateTime FechaEjecucion { get; set; }
    }
}
