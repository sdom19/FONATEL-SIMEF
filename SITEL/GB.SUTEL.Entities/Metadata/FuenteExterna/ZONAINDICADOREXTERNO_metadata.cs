using System.ComponentModel.DataAnnotations;
using GB.SUTEL.Entities.Metadata;
using System;

namespace GB.SUTEL.Entities
{
    [MetadataType(typeof(ZONAINDICADOREXTERNO_metadata))]
    partial class ZonaIndicadorExterno { }
    class ZONAINDICADOREXTERNO_metadata
    {
        public int IdZonaIndicadorExterno { get; set; }
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "Requerido")]
        [Display(Name = "Descripcion", ResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityDisplays))]
        [MaxLength(50, ErrorMessageResourceType=typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName="StringLength50")]
        [RegularExpression("[a-zA-Z0-9-_ .,:;()$&@!+|^á-úÁ-Ú\t*]+", ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "InvalidCharacters")]        
        public string DescZonaIndicadorExterno { get; set; }
        public byte Borrado { get; set; }
    }
}