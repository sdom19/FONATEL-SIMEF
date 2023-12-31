//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GB.SUTEL.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Servicio
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Servicio()
        {
            this.BitacoraParametrizacionIndicador = new HashSet<BitacoraParametrizacionIndicador>();
            this.ParamFormulas = new HashSet<ParamFormulas>();
            this.ServicioIndicador = new HashSet<ServicioIndicador>();
            this.ServicioOperador = new HashSet<ServicioOperador>();
            this.SolicitudIndicador = new HashSet<SolicitudIndicador>();
            this.TipoIndicadorServicio = new HashSet<TipoIndicadorServicio>();
            this.Indicador = new HashSet<Indicador>();
        }
    
        public int IdServicio { get; set; }
        public string DesServicio { get; set; }
        public byte Borrado { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BitacoraParametrizacionIndicador> BitacoraParametrizacionIndicador { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParamFormulas> ParamFormulas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServicioIndicador> ServicioIndicador { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServicioOperador> ServicioOperador { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SolicitudIndicador> SolicitudIndicador { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TipoIndicadorServicio> TipoIndicadorServicio { get; set; }
        public virtual ICollection<Indicador> Indicador { get; set; }
    }
}
