using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GB.SUTEL.Entities;

namespace GB.SUTEL.UI.Models
{
    public class ServicioViewModels
    {
        public List<TipoIndicador> listadoTipoIndicador { get; set; }

        public Servicio itemServicio { get; set; }

        public List<TipoIndicadorServicio> listadoTipoIndicadorServicio { get; set; }
    }
}