using System.ComponentModel.DataAnnotations;
using GB.SUTEL.Entities;
using System;


namespace GB.SUTEL.Entities
{
    [MetadataType(typeof(SolicitudIndicador_metadata))]
    public partial class SolicitudIndicador
    {
    }

    public class SolicitudIndicador_metadata
    {
        public int IdSolicitudIndicador { get; set; }

        [Display(Name = "Frecuencia")]
        [Required(ErrorMessage = "La frecuencia es requerida ")]
        public int IdFrecuencia { get; set; }
        
        [Display(Name = "Servicio")]
        [Required(ErrorMessage = "El servicio es requerido ")]
        public int IdServicio { get; set; }
        
        [Display(Name = "Dirección")]
        [Required(ErrorMessage = "La dirección es requerida ")]
        public int IdDireccion { get; set; }
        
        [Display(Name = "Descripción del Formulario")]
        [MaxLength(500, ErrorMessage = "El texto digitado no puede contener más de 500 caracteres")]
        [RegularExpression("^[^<>'\"/;`%]*$", ErrorMessage = "Caracteres inválidos")]
        [Required(ErrorMessage = "La descripción del formulario es requerido ")]
        public string DescFormulario { get; set; }

        [DataType(DataType.Date,ErrorMessage="Fecha en formato incorrecto")]
        [Display(Name = "Fecha Inicio")]
        [Required(ErrorMessage = "La Fecha Inicial es requerida")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaInicio { get; set; }

         [DataType(DataType.Date,ErrorMessage="Fecha en formato incorrecto")]
        [Display(Name = "Fecha Final")]
        [Required(ErrorMessage = "La Fecha Final es requerida")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaFin { get; set; }
        
        [Display(Name = "Activo")]
        public bool Activo { get; set; }

        public bool Borrado { get; set; }

       

    }
}
