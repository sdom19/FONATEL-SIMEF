using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GB.SUTEL.UI.Models
{
    public class DetalleAgrupacionyurl
    {
        public List<ConstructorDetalleAgrupacionViewModels> Listadetalles { get; set; }
        public string url { get; set; }
        public string idOperador { get; set; }
        public string NombreOperador { get; set; }
        public string idConstructor { get; set; }
        public string idCriterio { get; set; }

    }
}