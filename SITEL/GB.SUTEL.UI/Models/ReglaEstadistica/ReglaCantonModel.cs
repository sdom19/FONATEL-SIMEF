using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GB.SUTEL.UI.Models.ReglaEstadistica
{
    public class ReglaCantonModel
    {

        public double ValorInferior { get; set; }

        public double ValorSuperior { get; set; }

        public int IdCanton { get; set; }

        public ReglaCantonModel()
        { 

        ValorSuperior  = new double();

        ValorInferior = new double();

        IdCanton = new int();
        
        }
    }
}