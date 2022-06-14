using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace GB.SUTEL.Entities
{
    [MetadataType(typeof(TipoValor_metadata))]
    public partial class TipoValor
    {

    }

    public class TipoValor_metadata
    {
        [DisplayName("Código")]
        public int IdTipoValor { get; set; }
        [DisplayName("Tipo de Valor")]
        public string Descripcion { get; set; }
        [DisplayName("Formato")]
        public string Formato { get; set; }
    }
}
