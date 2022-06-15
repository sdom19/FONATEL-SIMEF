using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;



namespace GB.SUTEL.Entities
{
     [MetadataType(typeof(Constructor_metadata))]

    public partial class Constructor
    {
         

        
     }

    public class Constructor_metadata
    {
        public System.Guid IdConstructor { get; set; }
        [DisplayName("Indicador")]
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "valorRequerido")]
        public string IdIndicador { get; set; }
        [DisplayName("Frecuencia")]
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "valorRequerido")]
        public int IdFrecuencia { get; set; }
        [DisplayName("Desglose")]
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "valorRequerido")]
        public Nullable<int> IdDesglose { get; set; }
        [DisplayName("Dirección")]
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "valorRequerido")]
        public Nullable<int> IdDireccion { get; set; }
        public System.DateTime FechaCreacionConstructor { get; set; }
        public byte Borrado { get; set; }
    }
}
