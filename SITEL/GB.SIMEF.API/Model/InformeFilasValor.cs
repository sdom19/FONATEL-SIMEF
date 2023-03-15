using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GB.SIMEF.API.Model
{
    [Table("InformeFilasValor")]
    public class InformeFilasValor
    {
        public InformeFilasValor()
        {

        }

        [Key]
        public int IdInformeNombreTabla { get; set; }
        public int IdInformeEncabezadoTabla { get; set; }
        public int IdFilaValor { get; set; }
        public string Valor { get; set; }
    }
}
