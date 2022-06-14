using System.ComponentModel.DataAnnotations;
using GB.SUTEL.Entities.Metadata;
using System;

namespace GB.SUTEL.Entities
{
    [MetadataType(typeof(INDICADORCRUZADO_metadata))]
    partial class IndicadorCruzado { }
    class INDICADORCRUZADO_metadata
    {
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "Requerido")]
        [Display(Name = "Codigo", ResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityDisplays))]
        [RegularExpression("[a-zA-Z0-9-_]+", ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "InvalidCharacters")]
        [MaxLength(500, ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "StringLength50")]
        public string IdIndicadorCruzado { get; set; }
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "Requerido")]
        [Display(Name = "Nombre", ResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityDisplays))]
        [MaxLength(500, ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "StringLength500")]
        [RegularExpression("[a-zA-Z0-9-_ .,:;()$&@!+|^á-úÁ-Ú\t*]+", ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "InvalidCharacters")]        
        public string Nombre { get; set; }
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "Requerido")]
        [Display(Name = "Descripcion", ResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityDisplays))]
        [MaxLength(3000, ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "StringLength3000")]
        [RegularExpression("[a-zA-Z0-9-_ .,:;()$&@!+|^á-úÁ-Ú\t*]+", ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "InvalidCharacters")]        
        public string Descripcion { get; set; }

    }
}