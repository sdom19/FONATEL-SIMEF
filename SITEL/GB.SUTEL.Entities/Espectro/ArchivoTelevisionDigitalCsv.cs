using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SUTEL.Entities.Espectro
{
    public class ArchivoTelevisionDigitalCsv
    {
        public int IdArchivoCsv { get; set; }
        public string CodigoSitio { get; set; }
        public DateTime FechaCapturaArchivo { get; set; }
        public string CodigoConsecutivo { get; set; }

        //public string nombreArchivo { get; set; }
    }
}
