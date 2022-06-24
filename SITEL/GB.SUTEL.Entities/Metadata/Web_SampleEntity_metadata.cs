using System.ComponentModel.DataAnnotations;
using fqn = GB.SUTEL.Entities.Metadata;

namespace GB.SUTEL.Entities
{
    /// <summary>
    /// Web Sample Entity, don't look at it too much, is just for explanation purpose
    /// </summary>
    /// <createdby>
    /// <name>Walter Montes, Marvin Zumbado</name>
    /// <contact>walter.montes@grupobabel.com; walmon93@hotmail.com, marvinzzz@gmail.com, marvin.zumbado@grupobabel.com</contact>
    /// </createdby>
    /// <created>January 13, 2014</created>
    /// 
    [MetadataType(typeof(Web_SampleEntity_metadata))]
    public partial class Web_SampleEntity
    {
    }

    public class Web_SampleEntity_metadata
    {
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "websamplenumberrequired")]
        [Display(Name = "websamplenumber", ResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityDisplays))]
        public int Number { get; set; }
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors),
    ErrorMessageResourceName = "websamplenamerequired")]
        [Display(Name = "websamplename", ResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityDisplays))]
        [MaxLength(10, ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "tamanoString")]
        public string Name { get; set; }
    }
}
