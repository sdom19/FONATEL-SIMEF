using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GB.SUTEL.UI.Models
{
    public class FiltroCriterio
    {
        public int idDireccion { get; set; }
        public string idIndicador { get;set; }
        public string codigoCriterio { get; set; }
        public string nombreCriterio { get; set; }
        public string ayuda { get; set; }
        public string idConstructor { get; set; }
    }
}