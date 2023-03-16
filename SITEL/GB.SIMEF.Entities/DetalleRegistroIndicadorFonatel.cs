using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.Entities
{

    [Table("DetalleRegistroIndicadorFonatel")]
    public partial class DetalleRegistroIndicadorFonatel
    {

        public DetalleRegistroIndicadorFonatel()
        {
            this.DetalleRegistroIndicadorCategoriaFonatel = new List<DetalleRegistroIndicadorCategoriaFonatel>();
            this.DetalleRegistroIndicadorVariableFonatel = new List<DetalleRegistroIndicadorVariableFonatel>();
            this.DetalleRegistroIndicadorCategoriaValorFonatel = new List<DetalleRegistroIndicadorCategoriaValorFonatel>();
        }


        [Key, Column(Order = 0)]
        public int IdSolicitud { get; set; }
        [Key, Column(Order = 1)]
        public int IdFormulario { get; set; }
        [Key, Column(Order = 2)]
        public int IdIndicador { get; set; }
        public int IdDetalleRegistroIndicador { get; set; }
  
        public string TituloHoja { get; set; }
        public string NotaEncargado { get; set; }
        public string NotaInformante { get; set; }
        public string CodigoIndicador { get; set; }
        public string NombreIndicador { get; set; }
        public int CantidadFila { get; set; }

        [NotMapped]
        public string IdSolicitudString { get; set; }
        [NotMapped]
        public string IdFormularioString { get; set; }
        [NotMapped]
        public string IdIndicadorString { get; set; }

        [NotMapped]
        public int idCategoria { get; set; }

        [NotMapped]
        public virtual List<DetalleRegistroIndicadorCategoriaFonatel> DetalleRegistroIndicadorCategoriaFonatel { get; set; }
        [NotMapped]
        public virtual List<DetalleRegistroIndicadorVariableFonatel> DetalleRegistroIndicadorVariableFonatel { get; set; }
        [NotMapped]
        public virtual List<DetalleRegistroIndicadorCategoriaValorFonatel> DetalleRegistroIndicadorCategoriaValorFonatel { get; set; }
        [NotMapped]
        public virtual List<DetalleRegistroIndicadorVariableValorFonatel> DetalleRegistroIndicadorVariableValorFonatel { get; set; }

    }
}

