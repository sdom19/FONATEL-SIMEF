using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GB.SIMEF.API.Model
{
    [Table("InformeNombreTabla")]
    public class InformeNombreTabla
    {
        public InformeNombreTabla()
        {

        }

        [Key]
        public int IdInformeNombreTabla { get; set; }
        public string NombreTabla { get; set; }
        public Boolean Estado { get; set; }

        #region Varibles que no forman parte del contexto
        [NotMapped]
        public List<InformeEncabezadoTabla> InformeEncabezadoTablas { get; set; }
        [NotMapped]
        public List<InformeFilasValor> InformeFilasValor { get; set; }
        #endregion
    }
}
