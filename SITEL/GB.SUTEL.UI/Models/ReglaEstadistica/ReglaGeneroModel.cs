using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GB.SUTEL.UI.Models.ReglaEstadistica
{
    public class ReglaGeneroModel
    {


        public double ValorInferior { get; set; }

        public double ValorSuperior { get; set; }

        public int IdGenero { get; set; }

        public ReglaGeneroModel() { 

        ValorSuperior  = new double();

        ValorInferior = new double();

        IdGenero = new int();
        
        }
    }
}