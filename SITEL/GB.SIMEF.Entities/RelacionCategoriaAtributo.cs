//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GB.SIMEF.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RelacionCategoriaAtributo")]
    public partial class RelacionCategoriaAtributo
    {
        [Key, Column(Order = 0)]
        public int idRelacionCategoriaId { get; set; }
        [Key, Column(Order = 1)]
        public string idCategoriaDesagregacion { get; set; }

        [Key, Column(Order = 2)]
        public int idCategoriaDesagregacionAtributo { get; set; }
        [Key, Column(Order = 3)]
        public int idDetalleCategoriaTextoAtributo { get; set; }


        [NotMapped]
        public string Etiqueta { get; set; }
    }
}