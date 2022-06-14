using System.ComponentModel.DataAnnotations;
using GB.SUTEL.Entities;


namespace GB.SUTEL.Entities
{
    [MetadataType(typeof(Servicio_metadata))]
    public class Servicio_metadata
    {
        [Display(Name = "Código")]
        [Required(ErrorMessage = "El id es requerido")]
        public int IdServicio { get; set; }

        [RegularExpression(@"^[A-Za-z0-9 .,áéíóúÁÉÍÓÚñÑ]*$", ErrorMessage = "Los caracteres que digitó son inválidos.")]
        [Display(Name = "Servicio")]
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "valorRequerido")]
        [MaxLength(250, ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "servicioDescripcionTamanno")]
        public string DesServicio { get; set; }
        public bool Borrado { get; set; }
    }
}
