using System.ComponentModel.DataAnnotations;
using GB.SUTEL.Entities.Metadata;
using System;

namespace GB.SUTEL.Entities
{
    [MetadataType(typeof(REGISTROINDICADOREXTERNO_metadata))]
    partial class RegistroIndicadorExterno { }
    class REGISTROINDICADOREXTERNO_metadata
    {
        public System.Guid IdRegistroIndicadorExterno { get; set; }
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "Requerido")]
        [Display(Name = "Periodicidad", ResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityDisplays))]        
        public Nullable<int> IdPeridiocidad { get; set; }
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "Requerido")]
        [Display(Name = "Region", ResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityDisplays))]      
        public Nullable<int> IdRegionIndicadorExterno { get; set; }
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "Requerido")]
        [Display(Name = "Zona", ResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityDisplays))]      
        public Nullable<int> IdZonaIndicadorExterno { get; set; }
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "Requerido")]
        [Display(Name = "Indicador", ResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityDisplays))]      
        public System.Guid IdIndicadorExterno { get; set; }
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "Requerido")]
        [Display(Name = "Genero", ResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityDisplays))]      
        public Nullable<int> IdGenero { get; set; }
       
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "Requerido")]
        [Display(Name = "Valor", ResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityDisplays))]
        
        public Nullable<double> ValorIndicador { get; set; }
        public bool Borrado { get; set; }
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "Requerido")]
        [Display(Name = "Anio", ResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityDisplays))]      
        public int Anno { get; set; }
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "Requerido")]
        [Display(Name = "Canton", ResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityDisplays))]      
        public int IdCanton { get; set; }
        [Required(ErrorMessageResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityErrors), ErrorMessageResourceName = "Requerido")]
        [Display(Name = "Trimestre", ResourceType = typeof(GB.SUTEL.Entities.Metadata.EntityDisplays))]      
        public Nullable<int> IdTrimestre { get; set; }
    }
}