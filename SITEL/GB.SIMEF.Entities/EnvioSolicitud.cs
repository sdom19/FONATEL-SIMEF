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

    [Table("EnvioSolicitud")]
    public partial class EnvioSolicitud
    {
        [Key]
        public int idEnvioSolicitud { get; set; }
        public System.DateTime Fecha { get; set; }
        public bool Enviado { get; set; }
        public bool EnvioProgramado { get; set; }
        public string MensajeError { get; set; }

        public int IdSolicitud { get; set; }

        [NotMapped]
        public bool EjecutaJob { get; set; }

        [NotMapped]
        public string IdSolicitudString { get; set; }
    }
}
