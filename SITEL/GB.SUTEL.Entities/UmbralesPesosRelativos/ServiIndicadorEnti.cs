using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SUTEL.Entities.UmbralesPesosRelativos
{
    public class ServiIndicadorEnti
    {
        public string IdIndicador { get; set; }
        public string NombreIndicador { get; set; }
        public int IdServicio { get; set; }
        public decimal Peso { get; set; }
        public decimal Umbral { get; set; }
        public string Usuario { get; set; }
        public string FechaUltimaModificacion { get; set; }
    }
}
