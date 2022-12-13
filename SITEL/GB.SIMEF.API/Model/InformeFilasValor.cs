using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GB.SIMEF.API.Model
{
    [Table("InformeFilasValor", Schema = "FONATEL")]
    public class InformeFilasValor
    {
        public InformeFilasValor()
        {

        }

        [Key]
        public int IdTabla { get; set; }
        public int IdEncabezado { get; set; }
        public int IdFilaValor { get; set; }
        public string Valor { get; set; }
    }
}
