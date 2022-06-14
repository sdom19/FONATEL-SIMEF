using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SUTEL.Entities.Espectro
{
    public class DetalleArchivoTelevisionDigitalCsv
    {
        public int idDetalle { get; set; }
        public int IdArchivoCsv { get; set; }
        public DateTime Tiempo { get; set; }
        public int Frecuencia { get; set; }
        public string NombreSistema { get; set; }
        public decimal ISDBT { get; set; }
        public int Acimut { get; set; }
        public string Logitud { get; set; }
        public string Latitude { get; set; }
        public string ArchInfo { get; set; }

    }
}
