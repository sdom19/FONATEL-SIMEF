using System.ComponentModel.DataAnnotations;
using GB.SUTEL.Entities;


namespace GB.SUTEL.Entities
{
    #region Accion
    [MetadataType(typeof(ACCION_metadata))]
    public partial class Accion
    { }
    public class ACCION_metadata
    {
        [Display(Name = "Identificador")]
        [Required]
        public int IdAccion { get; set; }

        [Display(Name = "Nombre de Acción")]
        [Required]
        public string Nombre { get; set; }
    }
    #endregion

    #region Rol
    [MetadataType(typeof(ROL_metadata))]
    public partial class Rol
    { }
    public class ROL_metadata
    {
        [Display(Name = "Identificador")]
        [Required(ErrorMessage = "El id es requerido")]
        public int IdRol { get; set; }

        [Display(Name = "Nombre")]
        [RegularExpression(@"^[A-Za-z0-9 .,áéíóúÁÉÍÓÚñÑ]*$", ErrorMessage = "Los caracteres que digitó son inválidos.")]
        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(250, ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "rolDescripcionTamanno")]
        public string NombreRol { get; set; }
    }
    #endregion

    #region Pantalla
    [MetadataType(typeof(PANTALLA_metadata))]
    public partial class Pantalla
    { }
    public class PANTALLA_metadata
    {
        [Display(Name = "Identificador")]
        [Required]
        public int IdPantalla { get; set; }

        [Display(Name = "Nombre de Pantalla")]
        [Required]
        public string Nombre { get; set; }

        [Display(Name = "Descripción")]       
        public string Descripcion { get; set; }

        //[Display(Name = "Borrado")]        
        //public string Borrado { get; set; }
    }
    #endregion
}
