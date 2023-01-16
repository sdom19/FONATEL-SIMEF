using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.Entities
{
    [Table("FormulasCalculoTipoFecha")]
    public class FormulasCalculoTipoFecha
    {
        [Key]
        public int IdTipoFecha { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }

        [NotMapped]
        public string id { get; set; }
    }
}
