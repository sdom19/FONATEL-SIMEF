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

    public partial class ReglaSecuencial
    {
        [Key]
        public int idCompara { get; set; }
        public int idCategoriaId { get; set; }
        public Nullable<int> idvariable { get; set; }
        public Nullable<bool> Estado { get; set; }
    

        public virtual ReglaValidacionTipo ReglaValidacionTipo { get; set; }
    }
}
