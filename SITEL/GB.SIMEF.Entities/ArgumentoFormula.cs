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
        public int IdFormulaTipoArgumento { get; set; }
        public int? IdFormulaDefinicionFecha { get; set; }
        public int? IdFormulaVariableDatoCriterio { get; set; }
        public int IdFormulaCalculo { get; set; }

        [MaxLength(8000)]
        public string PredicadoSQL { get; set; }

        public int OrdenEnFormula { get; set; }

        [MaxLength(1000)]
        public string Etiqueta { get; set; }

        [NotMapped]
        public bool EsOperadorMatematico { get; set; }
    }
}
