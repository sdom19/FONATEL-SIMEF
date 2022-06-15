using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SUTEL.Entities.FormCumplimientoPorcenEnti
{
    public class FacReglasReport
    {
        public int IdServicio { get; set; }
        public string Servicio { get; set; }
        public int IdTipoInd { get; set; }
        public string TipoIndicador { get; set; }
        public string IdIndicador { get; set; }
        public string NombreIndicador { get; set; }
        public string IdCriterio { get; set; }
        public string NombreCriterio { get; set; }
        public string Agrupacion { get; set; }
        public string DetalleAgrupacion { get; set; }
        public string Frecuencia { get; set; }
        public string Desglose { get; set; }
        public Int64 ValMes1 { get; set; }
        public Int64 ValMes2 { get; set; }
        public Int64 ValMes3 { get; set; }
        public int IdParamFormulas { get; set; }
        public decimal ValFormulaCalcPorc { get; set; }
        public decimal Umbral { get; set; }
        public decimal FactorRigurosidad { get; set; }
        public decimal ValFormulaCalcCump { get; set; }
        public decimal Peso { get; set; }
        public decimal ResultCumplPeso { get; set; }
        public decimal FacServSinDesgTec { get; set; }
        public decimal FacServicio2G { get; set; }
        public decimal FacServicio3G { get; set; }
        public decimal FacServicio4G { get; set; }
        public string IdOperador { get; set; }
        public string NombreOperador { get; set; }
        public int AnioProcesado { get; set; }
        public short PeriodoEjec { get; set; }
    }
}
