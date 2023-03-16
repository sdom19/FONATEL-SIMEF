using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace GB.SIMEF.Entities
{
    public partial class DetalleRegistroIndicadorVariableValorFonatel
    {
        public int IdSolicitud { get; set; }
        public int idFormularioWeb { get; set; }
        public int IdIndicador { get; set; }
        public int IdVariable { get; set; }
        public int NumeroFila { get; set; }
        public decimal Valor { get; set; }



        [NotMapped]
        public string Solicitudid { get; set; }
        [NotMapped]
        public string FormularioId { get; set; }
        [NotMapped]
        public string IndicadorId { get; set; }
    }
}
