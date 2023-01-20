using System;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.Entities.DTO
{
    public partial class ArgumentoConstruidoDTO
    {
        public ArgumentoConstruidoDTO() { }

        public int TipoObjecto { get; set; }
        public string Etiqueta { get; set; }
        public FormulasTipoArgumento TipoArgumento { get; set; }
        public ArgumentoDTO Argumento { get; set; }

        public FormulasVariableDatoCriterio ConvertToVariableDatoCriterio()
        {
            return new FormulasVariableDatoCriterio()
            {
                IdFuenteIndicadorString = Argumento.fuente,
                IdVariableDatoString = Argumento.variableDatoCriterio,
                IdCriterioString = Argumento.variableDatoCriterio,
                IdCategoriaString = Argumento.categoria,
                IdDetalleCategoriaString = Argumento.detalle,
                IdAcumulacionString = Argumento.acumulacion,
                EsValorTotal = Argumento.valorTotal
            };
        }

        public FormulasDefinicionFecha ConvertToVariableDatoCriterio()
        {
            return new FormulasDefinicionFecha()
            {

            };
        }
    }
}
