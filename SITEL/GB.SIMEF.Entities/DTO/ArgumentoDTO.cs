using System;

namespace GB.SIMEF.Entities.DTO
{
    public class ArgumentoDTO
    {
        public ArgumentoDTO() { }

        // FormulasVariableDatoCriterio
        public string fuente { set; get; }
        public string codigoIndicador { set; get; }
        public string nombreVariable { set; get; }
        public string variableDatoCriterio { set; get; }
        public string categoria { set; get; }
        public string detalle { set; get; }
        //public string criterio { set; get; }
        public string acumulacion { set; get; }
        public bool valorTotal { set; get; }

        // FormulasDefinicionFecha
        public string indicador { set; get; }
        public int unidadMedida { set; get; }
        //public int idTipoFechaInicio { set; get; } // obviar
        public string tipoFechaInicio { set; get; }
        public DateTime fechaInicio { set; get; }
        public string categoriaInicio { set; get; }
        public string nombreCategoriaInicio { set; get; }
        //public int idTipoFechaFinal { set; get; } // obviar
        public string tipoFechaFinal { set; get; }
        public DateTime fechaFinal { set; get; }
        public string categoriaFinal { set; get; }
        public string nombreCategoriaFinal { set; get; }
    }
}
