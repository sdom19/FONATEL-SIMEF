using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GB.SUTEL.Entities.CustomModels.ReglaEstadistica;
using GB.SUTEL.Entities;

namespace GB.SUTEL.UI.Models
{
    public class ConstructorDetalleAgrupacionViewModels
    {
        public string id { get; set; }
        public string text { get; set; }
        public string parent { get; set; }
        public int ultimoNivel { get; set; }
        public int idTipoValor { get; set; }
        public string valorInferior { get; set; }
        public string valorSuperior { get; set; }
        public int idTipoNivelDetalle { get; set; }
        public int idNivel { get; set; }
        public Guid idConstructorCriterioDetalleAgrupacion { get; set; }
        public int UsaReglaEstadistica { get; set; }
        public int idTipoNivelDetalleGenero { get; set; }
        public List<NivelDetalleReglaEstadistica> nivelDetalleReglaEstadistica { get; set; }
    }
}