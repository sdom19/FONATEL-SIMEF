using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GB.SIMEF.API.Model
{
    public class InformeResultadoIndicador
    {
        public InformeResultadoIndicador()
        {

        }

        public string Variable { get; set; }
        public string Categoria { get; set; }
        public string AnnoMes { get; set; }
        public double Total { get; set; }
    }
}
