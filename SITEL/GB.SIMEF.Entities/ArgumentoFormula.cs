using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.Entities
{
    [Table("ArgumentoFormula")]
    public class ArgumentoFormula
    {
        [Key]
        public int IdArgumentoFormula { get; set; }
        public int IdFormula { get; set; }
        public int IdFormulasTipoArgumento { get; set; }
        public int IdFormulasDefinicionFecha { get; set; }
        public int IdFormulasVariableDatoCriterio { get; set; }
        public string PredicadoSQL { get; set; }
        public int OrdenEnFormula { get; set; }
    }
}
