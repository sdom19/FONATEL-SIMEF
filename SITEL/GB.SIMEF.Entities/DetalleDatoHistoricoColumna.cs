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

    [Table("DetalleDatoHistoricoColumna")]
    public partial class DetalleDatoHistoricoColumna
    {
        [Key]
        public int IdDetalleDatoHistoricoColumna { get; set; }

        public int IdDatoHistorico { get; set; }
        public int NumeroColumna { get; set; }
        public string Nombre { get; set; }
      
    
    }
}