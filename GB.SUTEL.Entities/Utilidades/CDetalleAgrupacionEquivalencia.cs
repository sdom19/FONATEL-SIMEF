using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SUTEL.Entities.Utilidades
{
    public class CDetalleAgrupacionEquivalencia
    {
        public DetalleAgrupacion detalleAgrupacionAnterior { get; set; }
        public DetalleAgrupacion detalleAgrupacionEquivalente { get; set; }
    }
}
