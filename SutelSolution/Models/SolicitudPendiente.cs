using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SutelSolution.Models
{
    public class SolicitudPendiente
    {

        public Guid Id_Solicitud_Indicador { get; set; }
        public string Id_Operador { get; set; }
        public int Id_ArchivoExcel { get; set; }
    }
}
