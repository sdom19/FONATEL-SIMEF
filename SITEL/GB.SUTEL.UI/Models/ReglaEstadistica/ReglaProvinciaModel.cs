using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GB.SUTEL.UI.Models.ReglaEstadistica
{
    public class ReglaProvinciaModel
    {


         public double ValorInferior { get; set; }

        public double ValorSuperior { get; set; }

        public int IdProvincia { get; set; }

        public ReglaProvinciaModel()
        { 

        ValorSuperior  = new double();

        ValorInferior = new double();

        IdProvincia = new int();
        
        }

    }
}