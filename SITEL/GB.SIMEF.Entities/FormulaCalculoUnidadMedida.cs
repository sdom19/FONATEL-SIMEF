using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.Entities
{
    [Table("FormulaCalculoUnidadMedida")]
    public class FormulaCalculoUnidadMedida
    {
        [Key]
        public int IdFormulaCalculoUnidadMedida { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }

        [NotMapped]
        public string id { get; set; }
    }
}
