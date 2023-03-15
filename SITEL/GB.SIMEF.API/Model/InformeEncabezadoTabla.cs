using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GB.SIMEF.API.Model
{
    [Table("InformeEncabezadoTabla", Schema = "FONATEL")]
    public class InformeEncabezadoTabla
    {
        public InformeEncabezadoTabla()
        {

        }

        [Key]
        public int IdInformeNombreTabla { get; set; }
        public int IdInformeEncabezadoTabla { get; set; }
        public string NombreEncabezado { get; set; }
    }
}
