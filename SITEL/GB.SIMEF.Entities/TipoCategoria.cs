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
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("TipoCategoria")]
    public partial class TipoCategoria
    {
      
        public TipoCategoria()
        {
            //this.CategoriasDesagregacion = new HashSet<CategoriasDesagregacion>();
        }
        [Key]
      
        public int idTipoCategoria { get; set; }
        public string Nombre { get; set; }
      
        public bool Estado { get; set; }
    

        //public virtual ICollection<CategoriasDesagregacion> CategoriasDesagregacion { get; set; }
    }
}