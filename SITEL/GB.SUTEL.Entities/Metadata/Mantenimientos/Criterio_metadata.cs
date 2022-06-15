using System.ComponentModel.DataAnnotations;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Metadata;


namespace GB.SUTEL.Entities
{
    [MetadataType(typeof(Criterio_metadata))]
    public partial class Criterio
    {
    }

    public class Criterio_metadata
    {
        const int tamanoCodigo = 10;
        const int tamanoDescripcion = 2000;

      

         [Range(1, 1000,
        ErrorMessage = "Seleccione una Dirección")]
        [Display(Name = "Dirección")]
        public int IdDireccion { get; set; }

        [Required(ErrorMessage = "El código es requerido")]
        [Display(Name = "Código")]
        [MaxLength(tamanoCodigo, ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "tamanoString")]
        [RegularExpression("^[^<>'\"/;`%]*$", ErrorMessage = "Caracteres inválidos")]
         public string IdCriterio { get; set; }

        [Required(ErrorMessage = "La descripción es requerida")]
        [Display(Name = "Descripción")]
        [MaxLength(tamanoDescripcion, ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "tamanoString")]
        public string DescCriterio { get; set; }

        [Required(ErrorMessage = "El indicador es requerido")]
        [Display(Name = "Indicador")]
        public string IdIndicador { get; set; }
        public bool Borrado { get; set; }

    }
}
