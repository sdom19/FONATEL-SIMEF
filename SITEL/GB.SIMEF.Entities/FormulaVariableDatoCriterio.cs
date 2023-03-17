using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GB.SIMEF.Entities
{
    [Table("FormulaVariableDatoCriterio")]
    public class FormulaVariableDatoCriterio : ArgumentoFormula
    {
        [Key]
        public int IdFormulaVariableDatoCriterio { set; get; }
        public int IdFuenteIndicador { get; set; }
        public string IdIndicador { get; set; }
        public int? IdDetalleIndicadorVariable { get; set; }
        public string IdCriterio { get; set; }
        public int? IdCategoriaDesagregacion { get; set; }
        public int? IdDetalleCategoriaTexto { get; set; }
        public int? IdAcumulacionFormula { get; set; }
        public bool EsValorTotal { get; set; }

        [NotMapped]
        public string IdFormulasVariablesDatoCriterioString { set; get; }
        [NotMapped]
        public string IdFuenteIndicadorString { get; set; }
        [NotMapped]
        public string IdIndicadorString { get; set; }
        [NotMapped]
        public string IdVariableDatoString { get; set; }
        [NotMapped]
        public string IdCriterioString { get; set; }
        [NotMapped]
        public string IdCategoriaString { get; set; }
        [NotMapped]
        public string IdDetalleCategoriaString { get; set; }
        [NotMapped]
        public string IdAcumulacionString { get; set; }
    }
}
