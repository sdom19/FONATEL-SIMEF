using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GB.SIMEF.Entities
{
    [Table("FormulaDefinicionFecha")]
    public class FormulaDefinicionFecha : ArgumentoFormula
    {
        //[Key]
        //public int IdFormulaDefinicionFecha { set; get; }
        public DateTime? FechaInicio { set; get; }
        public DateTime? FechaFinal { set; get; }
        public int IdUnidadMedida { set; get; }
        public int IdTipoFechaInicio { set; get; }
        public int IdTipoFechaFinal { set; get; }
        public int? IdCategoriaDesagregacionInicio { set; get; }
        public int? IdCategoriaDesagregacionFinal { set; get; }
        public int IdIndicador { set; get; }

        [NotMapped]
        public string IdFormulasDefinicionFechaString { set; get; }
        //[NotMapped]
        //public string IdUnidadMedidaString { set; get; }
        [NotMapped]
        public string IdTipoFechaInicioString { set; get; }
        [NotMapped]
        public string IdTipoFechaFinalString { set; get; }
        [NotMapped]
        public string IdCategoriaInicioString { set; get; }
        [NotMapped]
        public string IdCategoriaFinalString { set; get; }
        [NotMapped]
        public string IdIndicadorString { set; get; }
    }
}
