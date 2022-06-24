using System.ComponentModel.DataAnnotations;
using GB.SUTEL.Entities.Metadata;
using System;

namespace GB.SUTEL.Entities
{
    [MetadataType(typeof(FUENTEEXTERNA_metadata))]
    partial class FuenteExterna { }
    class FUENTEEXTERNA_metadata
    {
        public int IdFuenteExterna { get; set; }
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "Requerido")]
        [Display(Name = "Nombre", ResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityDisplays))]
        [MaxLength(50, ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "StringLength50")]
        [RegularExpression("[a-zA-Z0-9-_ .,:;()$&@!+|^á-úÁ-Ú\t*]+", ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "InvalidCharacters")]        
        public string NombreFuenteExterna { get; set; }
        public byte Borrado { get; set; }
    }
}