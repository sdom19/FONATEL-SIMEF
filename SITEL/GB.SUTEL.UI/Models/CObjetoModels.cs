using GB.SUTEL.Entities.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GB.SUTEL.UI.Models
{
    public class CObjetoModels
    {
        public List<ConstructorDetalleAgrupacionViewModels> arbol { get; set; }
        public List<CMes> meses{get;set;}
        public int idNivelDetalle { get; set; }
        public int idItemSeleccionado { get; set; }
        public int idMes { get; set; }
        public int idAnno { get; set; }
    }
}