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
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("DetalleCategoriaNumerico")]
    public partial class DetalleCategoriaNumerico
    {
        [Key]
        public int idCategoriaDetalle { get; set; }
        public int idCategoria { get; set; }
        public int Minimo { get; set; }
        public int Maximo { get; set; }
        public bool Estado { get; set; }
    }
}
