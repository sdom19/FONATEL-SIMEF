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
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    [Table("FuenteRegistro")]
    public partial class FuenteRegistro
    {
        
        public FuenteRegistro()
        {
            DetalleFuenteRegistro = new List<DetalleFuenteRegistro>();
        }
        [Key]
        public int IdFuenteRegistro { get; set; }
        public string Fuente { get; set; }
        public int? CantidadDestinatario { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public int IdEstadoRegistro { get; set; }

        [NotMapped]
        public  EstadoRegistro EstadoRegistro { get; set; }

        [NotMapped]

        public string id { get; set; }

        [NotMapped]
        public List<DetalleFuenteRegistro> DetalleFuenteRegistro { get; set; }

        public override string ToString()
        {
            StringBuilder json = new StringBuilder();
            json.Append("{\"Fuente\":\"").Append(this.Fuente).Append("\",");
            json.Append("\"Cantidad de destinatarios\":").Append(this.CantidadDestinatario).Append(",");

            string estado = string.Empty;
            switch (this.EstadoRegistro.IdEstadoRegistro)
            {
                case (int)Constantes.EstadosRegistro.Desactivado:
                    estado = Enum.GetName(typeof(Constantes.EstadosRegistro), this.EstadoRegistro.IdEstadoRegistro);
                    break;
                case (int)Constantes.EstadosRegistro.Activo:
                    estado = Enum.GetName(typeof(Constantes.EstadosRegistro), this.EstadoRegistro.IdEstadoRegistro);
                    break;
                case (int)Constantes.EstadosRegistro.Eliminado:
                    estado = Enum.GetName(typeof(Constantes.EstadosRegistro), this.EstadoRegistro.IdEstadoRegistro);
                    break;
                case (int)Constantes.EstadosRegistro.EnProceso:
                    estado = "En Proceso";
                    break;
            }
            json.Append("\"Estado\":\"").Append(estado).Append("\"}");

            return json.ToString();
        }
    }
}