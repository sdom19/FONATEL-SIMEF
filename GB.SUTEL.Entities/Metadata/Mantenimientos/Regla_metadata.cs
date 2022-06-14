using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace GB.SUTEL.Entities
{
    [MetadataType(typeof(Regla_metadata))]
    public partial class Regla
    { }
    public class Regla_metadata
    {
        [DisplayName("Código")]
        public System.Guid IdConstructorCriterio { get; set; }
        [DisplayName("Valor Inferior")]
        public string ValorLimiteInferior { get; set; }
        [DisplayName("Valor Superior")]
        public string ValorLimiteSuperior { get; set; }
        [DisplayName("Fecha de Creación")]
        public System.DateTime FechaCreacionRegla { get; set; }
        public byte Borrado { get; set; }
    }
}
