using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.Entities
{
    [Table("CatalogoPantallaSIGITEL")]
    public partial class CatalogoPantallaSIGITEL
    {
        [Key]
        public int IdCatalogoPantallaSIGITEL { get; set; }
        public string NombrePantalla { get; set; }
        public bool Estado { get; set; }

        [NotMapped]
        public string id { get; set; }
    }
}
