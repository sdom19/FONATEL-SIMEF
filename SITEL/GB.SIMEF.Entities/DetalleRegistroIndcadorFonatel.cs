using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.Entities
{
    [Table("DetalleRegistroIndcadorFonatel")]
    public partial class DetalleRegistroIndcadorFonatel
    {
        [Key, Column(Order = 0)]
        public int IdSolicitud { get; set; }
        [Key, Column(Order = 1)]
        public int IdFormulario { get; set; }
        [Key, Column(Order = 2)]
        public int IdDetalleRegistroIndicador { get; set; }
        public int IdIndicador { get; set; }
        public string TituloHojas { get; set; }
        public string NotasEncargado { get; set; }
        public string NotasInformante { get; set; }
        public string CodigoIndicador { get; set; }
        public string NombreIndicador { get; set; }
        public int CantidadFilas { get; set; }
    }
}

