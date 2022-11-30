using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.Entities
{
    [Table("AcumulacionFormula")]
    public class AcumulacionFormula
    {
        [Key]
        public int IdAcumulacionFormula { get; set; }
    
        [MaxLength(25)]
        public string Acumulacion { get; set; }

        public bool Estado { set; get; }

        [NotMapped]
        public string id { get; set; }
    }

}
