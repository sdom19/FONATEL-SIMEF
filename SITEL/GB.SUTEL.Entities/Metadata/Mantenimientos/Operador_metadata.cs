using System.ComponentModel.DataAnnotations;
using GB.SUTEL.Entities;


namespace GB.SUTEL.Entities
{
    [MetadataType(typeof(Operador_metadata))]
    public partial class Operador
    {
    }

    public class Operador_metadata
    {
        [Display(Name = "Código")]
        public int IdOperador { get; set; }

       
        [Display(Name = "Operador")]
        public string NombreOperador { get; set; }
        public bool Estado { get; set; }
    }
}
