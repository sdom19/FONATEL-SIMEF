using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace GB.SUTEL.Entities
{
    [MetadataType(typeof(TipoNivelDetalle_metadata))]
    public partial class TipoNivelDetalle
    { }
    public class TipoNivelDetalle_metadata
    {
        [DisplayName("Código")]
        public int IdNivelDetalle { get; set; }
        [DisplayName("Nombre")]
        public string Nombre { get; set; }
        [DisplayName("Tabla")]
        public string Tabla { get; set; }
    }
}
