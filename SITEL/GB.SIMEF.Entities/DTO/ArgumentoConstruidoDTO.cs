using System;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.Entities.DTO
{
    public partial class ArgumentoConstruidoDTO
    {
        public ArgumentoConstruidoDTO() { }

        public int TipoObjecto { get; set; }
        public string Etiqueta { get; set; }
        public FormulasTipoArgumentoEnum TipoArgumento { get; set; }
        public ArgumentoDTO Argumento { get; set; }

        /// <summary>
        /// 21/01/2023
        /// José Navarro Acuña
        /// Función que ejecuta el mapeo hacia el modelo FormulasVariableDatoCriterio
        /// </summary>
        /// <returns></returns>
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
                IdIndicadorString = Argumento.indicador,
                EsValorTotal = Argumento.valorTotal
            };
        }

        /// <summary>
        /// 21/01/2023
        /// José Navarro Acuña
        /// Función que ejecuta el mapeo hacia el modelo FormulasDefinicionFecha
        /// </summary>
        /// <returns></returns>
        public FormulasDefinicionFecha ConvertToFormulasDefinicionFecha()
        {
            return new FormulasDefinicionFecha()
            {
                IdUnidadMedida = Argumento.unidadMedida,
                IdTipoFechaInicioString = Argumento.tipoFechaInicio,
                IdTipoFechaFinalString = Argumento.tipoFechaFinal,
                IdCategoriaInicioString = Argumento.categoriaInicio,
                IdCategoriaFinalString = Argumento.categoriaFinal,
                FechaInicio = Argumento.fechaInicio,
                FechaFinal = Argumento.fechaFinal,
                IdIndicadorString = Argumento.indicador
            };
        }
    }
}
