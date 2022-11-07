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
    public partial class DetalleRegistroIndicadorFonatel
    {

        //public DetalleRegistroIndicadorFonatel()
        //{
        //    this.DetalleRegistroIndicadorCategoriaFonatel = new List<DetalleRegistroIndicadorCategoriaFonatel>();
        //    this.DetalleRegistroIndicadorVariableFonatel = new List<DetalleRegistroIndicadorVariableFonatel>();
        //}


        [Key, Column(Order = 0)]
        public int IdSolicitud { get; set; }
        [Key, Column(Order = 1)]
        public int IdFormulario { get; set; }
        [Key, Column(Order = 2)]
        public int IdIndicador { get; set; }
        public int IdDetalleRegistroIndicador { get; set; }
  
        public string TituloHojas { get; set; }
        public string NotasEncargado { get; set; }
        public string NotasInformante { get; set; }
        public string CodigoIndicador { get; set; }
        public string NombreIndicador { get; set; }
        public int CantidadFilas { get; set; }

        [NotMapped]
        public string IdSolicitudString { get; set; }
        [NotMapped]
        public string IdFormularioString { get; set; }
        [NotMapped]
        public string IdIndicadorString { get; set; }

        //[NotMapped]
        //List<DetalleRegistroIndicadorCategoriaFonatel> DetalleRegistroIndicadorCategoriaFonatel { get; set; }
        //[NotMapped]
        //List<DetalleRegistroIndicadorVariableFonatel> DetalleRegistroIndicadorVariableFonatel { get; set; }


    }
}

