using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  GB.SUTEL.Entities;

namespace GB.SUTEL.Entities.Espectro
{
    public class ArchivoMifModel
    {

        public string nombreArchivoMif { get; set; }

        //public List<string> latitudes { get; set; }

        //public List<string> longitudes { get; set; }


        public List<CoordenadasArchivoMif> listaCoordenadas { get; set; }

        public int region { get; set; }

        public bool leidoCorrectamente { set; get; }

        public ArchivoMifModel()
        {

            listaCoordenadas = new List<CoordenadasArchivoMif>();

            //latitudes = new List<string>();

            //longitudes = new List<string>();

            region = new int();

            leidoCorrectamente = true;

        }
    }
}
