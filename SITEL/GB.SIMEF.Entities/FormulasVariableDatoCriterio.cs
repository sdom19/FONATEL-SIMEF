using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GB.SIMEF.Entities
{
    [Table("FormulasVariablesDatoCriterio")]
    public class FormulasVariableDatoCriterio : ArgumentoFormula
    {
        [Key]
        public new int IdFormulasVariableDatoCriterio { set; get; }
        public int IdFuenteIndicador { get; set; }
        public int IdIndicador { get; set; }
        public int IdVariableDato { get; set; }
        public int IdCriterio { get; set; }
        public int IdCategoria { get; set; }
        public int IdDetalleCategoria { get; set; }
        public int IdAcumulacion { get; set; }
        public bool EsValorTotal { get; set; }

        [NotMapped]
        public string IdFormulasVariablesDatoCriterioString { set; get; }
        [NotMapped]
        public string IdFuenteIndicadorString { get; set; }
        [NotMapped]
        public string IdIndicadorDesencriptado { get; set; } // debido a otras fuentes de indicador, existen IDs alfanumnericos
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
