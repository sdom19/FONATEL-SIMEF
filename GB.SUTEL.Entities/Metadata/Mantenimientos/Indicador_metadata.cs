using System.ComponentModel.DataAnnotations;
using GB.SUTEL.Entities;


namespace GB.SUTEL.Entities
{
    [MetadataType(typeof(Indicador_metadata))]
    public partial class Indicador
    { }
    public class Indicador_metadata
    {        
        [Display(Name = "Id")]
        [Required(ErrorMessage = "El id es requerido")]
        [MaxLength(50, ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "indicadorIdTamanno")]
        [RegularExpression(@"^[A-Za-z0-9 .,áéíóúÁÉÍÓÚñÑ]*$", ErrorMessage = "Los caracteres que digitó son inválidos.")]
        public int IdIndicador { get; set; }       

        [Display(Name = "Tipo Indicador")]
        [Required(ErrorMessage = "El tipo indicador es requerido")]
        public int IdTipoInd { get; set; }

        [Display(Name = "Nombre")]
        [MaxLength(250, ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "indicadorNombreTamanno")]
        [RegularExpression(@"^[A-Za-z0-9 .,()áéíóúÁÉÍÓÚñÑ]*$", ErrorMessage = "Los caracteres que digitó son inválidos.")]
        [Required(ErrorMessage = "El nombre es requerido")]
        public string NombreIndicador { get; set; }

        [Display(Name = "Descripción")]
        [MaxLength(2000, ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "indicadorDescripcionTamanno")]
        [Required(ErrorMessage = "La descripción es requerida")]
        public string DescIndicador { get; set; }

        [Display(Name = "Borrado")]        
        public byte Borrado { get; set; }
    }
}
