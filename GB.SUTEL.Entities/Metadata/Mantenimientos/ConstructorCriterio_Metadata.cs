using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GB.SUTEL.Entities
{
    [MetadataType(typeof(ConstructorCriterio_Metadata))]
    public partial class ConstructorCriterio
    { }
    public class ConstructorCriterio_Metadata
    {
        public System.Guid IdConstructor { get; set; }
        [DisplayName("Critero")]
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "valorRequerido")]
        public string IdCriterio { get; set; }
        [DisplayName("Ayuda")]
        [MaxLength(2550,ErrorMessage= "La cantidad máxima para la ayuda es de 2500 caracteres.")]
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "valorRequerido")]
        public string Ayuda { get; set; }
    }
}
