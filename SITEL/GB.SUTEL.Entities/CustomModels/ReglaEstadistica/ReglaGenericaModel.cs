using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GB.SUTEL.Entities.CustomModels.ReglaEstadistica
{
    public class ReglaGenericaModel
    {

        public int IdNivelDetalle { get; set; }

        public double ValorInferior { get; set; }

        public double ValorSuperior { get; set; }

        //IdGenerico puede ser IdProvincia, IdGenero, IdCanton o ninguno de los tres. Esto se determina con el IdDetalleNivel
        public int IdGenerico { get; set; }

        public ReglaGenericaModel()
        { 

        ValorSuperior  = new double();

        ValorInferior = new double();

        IdGenerico = new int();

        IdNivelDetalle = new int();
        
        }

    }
}