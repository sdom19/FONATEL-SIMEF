using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.DTO;

namespace GB.SUTEL.UI.Models
{
    public class SolicitudViewModels
    {
        
        private string _filtroInicioPeriodo = DateTime.Today.AddMonths(-Properties.Settings.Default.MostrarMesesSolicitudOmision).ToString("dd/MM/yyyy");
        public List<SolicitudIndicador> listadoSolicitudes { get; set; }

        public List<pa_getListaIndicadoresSolicitud_Result> listadoIndicadores { get; set; }
        public List<SolicitudConstructorDto> solicitudConstructorDto { get; set; }

        public List<pa_getListaIndicadoresXSolicitud_Result> listadoIndicadoresXOperador { get; set; }

        public SolicitudIndicador itemSolicitudIndicador { get; set; }

        public string FiltroFormulario { get; set; }
        public string FiltroInicioPeriodo { get { return _filtroInicioPeriodo; } set { _filtroInicioPeriodo = value; } }
        public string FiltroFinPeriodo { get; set; }

        public List<Servicio> listadoServicios { get; set; }
        public List<int> ServicioSeleccionados { get; set; }
        
    }
}

