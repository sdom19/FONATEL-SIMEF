//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GB.SIMEF.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Anno
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Anno()
        {
            this.Solicitud = new HashSet<Solicitud>();
        }
    
        public int idAnno { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
    

        public virtual ICollection<Solicitud> Solicitud { get; set; }
    }
}
