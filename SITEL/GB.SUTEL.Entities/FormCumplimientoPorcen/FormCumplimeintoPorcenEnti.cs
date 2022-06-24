using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SUTEL.Entities.FormCumplimientoPorcenEnti
{
    public class FormCumplimientoPorcenEnti
    {
        public int IdParamFormulas { get; set; }
        public int IdServicio { get; set; }
        public string IdIndicador { get; set; }
        public string FormulaPorcentaje { get; set; }
        public string FormulaCumplimiento { get; set; }
        public string Criterios { get; set; }
        public string FechaUltimaActualizacion { get; set; }
        public string Usuario { get; set; }
        public string DescCriterio { get; set; }
        public int IdDireccion { get; set; }
        public string CriteriosCkech { get; set; }
        public string IdCriterio { get; set; }
        public List<string> FromArray { get; set; }
        public List<string> ArrayIF { get; set; }
        public List<string> ArrayVerdadero { get; set; }
        public List<string> ArrayFalso { get; set; }
    }
}
