﻿//------------------------------------------------------------------------------
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

    [Table("ReglaIndicadorSalida")]
    public partial class ReglaIndicadorSalida
    {
        [Key]
        public int IdCompara { get; set; }
        public int IdDetalleReglaValidacion { get; set; }
        public int IdIndicador { get; set; }

        [NotMapped]
        public string idIndicadorComparaString { get; set; }

    }
}