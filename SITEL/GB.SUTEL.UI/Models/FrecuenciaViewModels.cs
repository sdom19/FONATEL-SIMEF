using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GB.SUTEL.Entities;

namespace GB.SUTEL.UI.Models
{
    public class FrecuenciaViewModels
    {
        public List<Frecuencia> listadoFrecuencias { get; set; }

        public Frecuencia itemFrecuencia { get; set; }
    }
}