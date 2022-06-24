using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SUTEL.Entities.Espectro
{
    public class ArchivoCsvModel
    {

        public DateTime FechaCapturaArchivo { set; get; }

        public string CodigoSitio { set; get; }

        public string CodigoConsecutivo { set; get; }

        public List<DetalleArchivoCsv> listaDetalle { set; get; }

        //public List<DateTime> tiempo  { set; get; }

        //public List<int> frecuencia  { set; get; }

        //public List<float> nivel { set; get; }

        public string informacionArchivo { set; get; }

        public string nombreArchivo { set; get; }  

        public ArchivoCsvModel() 
        {
         FechaCapturaArchivo = new DateTime();

         CodigoSitio = string.Empty;

         CodigoConsecutivo = string.Empty;

         listaDetalle = new List<DetalleArchivoCsv>();

         //tiempo  =  new List<DateTime>();

         //frecuencia  = new List<int>();

         //nivel = new List<float>();

         informacionArchivo = string.Empty;

         nombreArchivo = string.Empty;
        
        
        }


    }
}
