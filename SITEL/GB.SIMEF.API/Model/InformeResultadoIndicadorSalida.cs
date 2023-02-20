using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GB.SIMEF.API.Model
{
    public class InformeResultadoIndicadorSalida
    {
        public InformeResultadoIndicadorSalida()
        {

        }

        public List<string> Encabezados { get; set; }
        public List<InformeResultadoDatos> Datos { get; set; }
        public Dictionary<string, double> Totales { get; set; }
    }

    public class InformeResultadoDatos
    {
        public InformeResultadoDatos()
        {

        }

        public string Variable { get; set; }
        public string Categoria { get; set; }
        public Dictionary<string, double> Valores { get; set; }
    }

}
