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
    using GB.SIMEF.Resources;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    [Table("DetalleFuenteRegistro")]
    public partial class DetalleFuenteRegistro
    {

        public DetalleFuenteRegistro()
        {
          
        }

        [Key]
        public int idDetalleFuenteRegistro { get; set; }
        public int idFuenteRegistro { get; set; }
        public string NombreDestinatario { get; set; }
        public string CorreoElectronico { get; set; }
        public bool Estado { get; set; }

        public int idUsuario { get; set; }

        public bool CorreoEnviado { get; set; }


        #region Atributos no forman parte del contexto

        [NotMapped]
        public string FuenteId { get; set; }

        [JsonIgnore]
        [NotMapped]
        public string Json { get; set; }
        [NotMapped]
        public string Contrasena { get; set; }

        [NotMapped]
        public string NombreFuente { get; set; }

        [NotMapped]
        public int CantidadDisponible { get; set; }

        public override string ToString()
        {
            StringBuilder json = new StringBuilder();
            json.Append("{\"Nombre destinatario\":\"").Append(this.NombreDestinatario).Append("\",");
            json.Append("\"Correo electrónico\":\"").Append(this.CorreoElectronico).Append("\",");
            json.Append("\"Activo\":\"").Append(this.Estado ? "Si" : "No").Append("\"}");

            return json.ToString();
        }


        #endregion

    }
}
