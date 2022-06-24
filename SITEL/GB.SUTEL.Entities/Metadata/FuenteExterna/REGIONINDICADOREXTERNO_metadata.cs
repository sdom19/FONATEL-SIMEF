using System.ComponentModel.DataAnnotations;
using GB.SUTEL.Entities.Metadata;
using System;

namespace GB.SUTEL.Entities
{
    [MetadataType(typeof(REGIONINDICADOREXTERNO_metadata))]
    partial class RegionIndicadorExterno { }
    class REGIONINDICADOREXTERNO_metadata
    {
        public int IdRegionIndicadorExterno { get; set; }
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "Requerido")]
        [Display(Name = "Descripcion", ResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityDisplays))]
        [MaxLength(50, ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "StringLength50")]
        [RegularExpression("[a-zA-Z0-9-_ .,:;()$&@!+|^á-úÁ-Ú\t*]+", ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "InvalidCharacters")]        
        public string DescRegionIndicadorExterno { get; set; }
        public byte Borrado { get; set; }
    }
}