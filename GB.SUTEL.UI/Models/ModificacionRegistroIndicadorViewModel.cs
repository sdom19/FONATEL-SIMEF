using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GB.SUTEL.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace GB.SUTEL.UI.Models
{
    public class ModificacionRegistroIndicadorViewModel
    {

        public List<Servicio> ListaServicios { get; set; }
        public List<Direccion> listaDirecciones { get; set; }
        public List<Frecuencia> listaFrecuencias { get; set; }
        public List<Frecuencia> listaDesglose { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "valorRequerido")]
        public string idOperador { get; set; }
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "valorRequerido")]
        public int idDireccion { get; set; } 
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "valorRequerido")]
        public string idIndicador { get; set; }
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "valorRequerido")]
        public int idFrecuencia { get; set; }
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "valorRequerido")]
        public int idDesglose { get; set; }
        [DataType(DataType.Date, ErrorMessage = "Fecha en formato incorrecto")]
        [Required(ErrorMessage = "La Fecha Inicial es requerida")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime fechaInicial { get; set; }
        [DataType(DataType.Date, ErrorMessage = "Fecha en formato incorrecto")]
        [Required(ErrorMessage = "La Fecha Final es requerida")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime fechaFinal { get; set; }
        [Display(Name = "Operador")]
        public Operador operador { get; set; }
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "valorRequerido")]
        public int idServicio { get; set; }
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "valorRequerido")]
        public string idCriterio { get; set; }
        public Guid idSolicitudIndicador { get; set; }

        public int idServicioModificacionMasiva { get; set; }
       } 
}