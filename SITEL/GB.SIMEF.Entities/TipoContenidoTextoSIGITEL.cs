using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.Entities
{
    [Table("TipoContenidoTextoSIGITEL")]
    public class TipoContenidoTextoSIGITEL
    {
        [Key]
        public int IdTipoContenidoTextoSIGITEL { get; set; }
        public string NombreTipoContenido { get; set; }
        public bool Estado { get; set; }
    }
}
