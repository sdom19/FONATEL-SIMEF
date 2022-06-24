using System.ComponentModel.DataAnnotations;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Metadata;


namespace GB.SUTEL.Entities
{
    [MetadataType(typeof(DetalleAgrupacion_metadata))]
    public partial class DetalleAgrupacion
    {
    }

    public class DetalleAgrupacion_metadata
    {

        public int IdDetalleAgrupacion { get; set; }
        
        [Range(1, 90000,
        ErrorMessage = "Seleccione una Agrupación")]
        [Display(Name = "Agrupación")]
        public int IdAgrupacion { get; set; }

        [Required(ErrorMessage = "Seleccione un operador")]
        [Display(Name = "Operador")]
         public string IdOperador { get; set; }

        [Required(ErrorMessage = "La descripción es requerida")]
        [Display(Name = "Descripción")]
        [MaxLength(250, ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "tamanoString")]
        [RegularExpression("^[^<>'\"/;`%]*$", ErrorMessage = "Caracteres inválidos")]
        public string DescDetalleAgrupacion { get; set; }
        public bool Borrado { get; set; }

    }
}
