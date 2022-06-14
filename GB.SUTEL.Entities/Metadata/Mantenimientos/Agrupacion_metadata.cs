using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace GB.SUTEL.Entities
{
    [MetadataType(typeof(Agrupacion_metadata))]
    public partial class Agrupacion
    {
       
    }

    

    public class Agrupacion_metadata
    {
        [DisplayName("Código")]
        public int IdAgrupacion { get; set; }
        [DisplayName("Agrupación")]
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "valorRequerido")]
        [MaxLength(600, ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "agrupacionDescripcionTamaño")]
        public string DescAgrupacion { get; set; }
        public byte Borrado { get; set; }
    }
   
}
