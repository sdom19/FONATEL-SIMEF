using System.ComponentModel.DataAnnotations;
using GB.SUTEL.Entities;
using System;


namespace GB.SUTEL.Entities
{
    [MetadataType(typeof(FRECUENCIA_metadata))]
    public partial class Frecuencia
    {
    }

    public class FRECUENCIA_metadata
    {
        const int tamanoNombre = 20;

        [Display(Name = "Código")]
        [Range(1,10)]
        public int IdFrecuencia { get; set; }

        [Required(ErrorMessage = "La descripción es requerida.")]
        [Display(Name = "Nombre")]
        [MaxLength(20, ErrorMessage = "El texto digitado no puede contener más de 20 caracteres.")]
        [RegularExpression("^[^<>'\"/;`%]*$", ErrorMessage = "Caracteres inválidos.")]
        public string NombreFrecuencia { get; set; }

        [Required(ErrorMessage = "La cantidad requerido (Solo números).")]
        [Display(Name = "Cantidad de meses")]
        //[MaxLength(2, ErrorMessage = "El texto digitado no puede contener más de 2 números.")]        
        [RegularExpression(@"^[0-9]{1,50}$", ErrorMessage = "Solamente números.")]
        public int CantidadMeses { get; set; }
        public byte Borrado { get; set; }
    }
}
