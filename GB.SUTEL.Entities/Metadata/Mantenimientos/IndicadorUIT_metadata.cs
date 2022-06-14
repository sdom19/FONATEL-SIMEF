using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel;

namespace GB.SUTEL.Entities
{
    [MetadataType(typeof(IndicadorUIT_metadata))]
    public partial class IndicadorUIT
    { }
    public class IndicadorUIT_metadata
    {
        [DisplayName("Código")]
        public int IdIndicadorUIT { get; set; }

        [DisplayName("Indicador UIT")]
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "valorRequerido")]
        [MaxLength(250, ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "indicadorUITDescripcionTamanno")]
        [RegularExpression("^[^<>'\"/;`%]*$", ErrorMessage = "Caracteres inválidos")]
        public string DescIndicadorUIT { get; set; }
        public byte Borrado { get; set; }
    }
}
