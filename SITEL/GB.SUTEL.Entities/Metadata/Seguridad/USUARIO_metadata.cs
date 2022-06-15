using System.ComponentModel.DataAnnotations;
using GB.SUTEL.Entities.Metadata;
using System;

namespace GB.SUTEL.Entities
{
    [MetadataType(typeof(USUARIO_metadata))]
    partial class Usuario { }
    class USUARIO_metadata
    {
        [Required]
        public int IdUsuario { get; set; }
        //[Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "Requerido")]
        [Display(Name = "USUARIO_operador", ResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityDisplays))]
        public string IdOperador { get; set; }
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "Requerido")]
        [Display(Name = "USUARIO_nombreusuario", ResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityDisplays))]
        [RegularExpression("[a-zA-Z0-9-_ .,:;()$&@!+|^á-úÁ-Ú\t*]+", ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "InvalidCharacters")]
        public string AccesoUsuario { get; set; }
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "Requerido")]
        [Display(Name = "USUARIO_nombre", ResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityDisplays))]
        [RegularExpression("[a-zA-Z0-9-_ .,:;()$&@!+|^á-úÁ-Ú\t*]+", ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "InvalidCharacters")]        
        public string NombreUsuario { get; set; }
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "Requerido")]
        [Display(Name = "USUARIO_contrasenia", ResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityDisplays))]
        [DataType(DataType.Password)]
        //[MinLength(6, ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "PassLength")]
        //[RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{5,15}$", ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors),ErrorMessageResourceName="RegExPassword")]
        public string Contrasena { get; set; }
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "Requerido")]
        [Display(Name = "USUARIO_correo", ResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityDisplays))]
        [DataType(DataType.EmailAddress, ErrorMessage = "Formato inválido.", ErrorMessageResourceType=typeof(GB.SUTEL.Entities.Metadata.EntityErrors),
            ErrorMessageResourceName="FormatoInvalido")]
        public string CorreoUsuario { get; set; }
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "Requerido")]
        [Display(Name = "USUARIO_interno", ResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityDisplays))]
        public byte UsuarioInterno { get; set; }
        //[Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "Requerido")]
        [Display(Name = "Activo", ResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityDisplays))]
        public bool Activo { get; set; }
        public byte Borrado { get; set; }
    }
}