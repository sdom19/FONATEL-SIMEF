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
    using System.ComponentModel.DataAnnotations;

    public partial class ClasificacionIndicadores
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ClasificacionIndicadores()
        {
            this.Indicador = new HashSet<Indicador>();
        }
        [Key]
        public int idClasificacion { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
    

        public virtual ICollection<Indicador> Indicador { get; set; }
    }
}