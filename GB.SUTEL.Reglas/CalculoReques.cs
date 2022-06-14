using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SUTEL.Reglas
{
    public class CalculoRequest
    {

        public List<VariablesProceso> Variables { get; set; }
        public List<Reglas> Reglas { get; set; }
        public List<Formulas> Formulas { get; set; }
    }
}
