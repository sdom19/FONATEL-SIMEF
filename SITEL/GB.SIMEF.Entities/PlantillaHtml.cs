using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.Entities
{
    [Table("PlantillaHtml")]
    public partial class PlantillaHtml
    {
        [Key]
        public int IdPlantillaHTML { get; set; }

        public string Html { get; set; }

        public string Descripcion { get; set; }
    }
}
