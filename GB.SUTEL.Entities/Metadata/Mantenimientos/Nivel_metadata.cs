using System.ComponentModel.DataAnnotations;
using GB.SUTEL.Entities;


namespace GB.SUTEL.Entities
{
    [MetadataType(typeof(Nivel_metadata))]
    public partial class Nivel
    {
    }

    public class Nivel_metadata
    {
        [Display(Name = "Código")]
        public int IdNivel { get; set; }

        [Required(ErrorMessage = "La descripción es requerida")]
        [Display(Name = "Descripción")]
        [MaxLength(50,  ErrorMessage = "El texto digitado no puede contener más de 50 caracteres")]
        [RegularExpression("^[^<>'\"/;`%]*$", ErrorMessage = "Caracteres inválidos")]
        public string DescNivel { get; set; }
        public bool Borrado { get; set; }
    }
}
