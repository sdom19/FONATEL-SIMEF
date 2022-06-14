using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GB.SUTEL.Entities;

namespace GB.SUTEL.UI.Models
{
    public class DetalleAgrupacionViewModels
    {
        public List<DetalleAgrupacion> listadoDetalleAgrupacion { get; set; }

        public DetalleAgrupacion itemDetalleAgrupacion { get; set; }
    }
}