using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GB.SUTEL.Entities;

namespace GB.SUTEL.UI.Models
{
    public class CDetalleModificacionRegistroModels
    {
        public List<CItemModels> items { get; set; }
        public DetalleRegistroIndicador detalle { get; set; }
    }
}