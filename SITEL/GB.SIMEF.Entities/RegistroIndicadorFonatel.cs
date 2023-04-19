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
    using System.Text;

    [Table("RegistroIndicadorFonatel")]

    public partial class RegistroIndicadorFonatel
    {
        [Key, Column(Order =0)]
        public int IdSolicitud { get; set; }
        [Key, Column(Order = 1)]
        public int idFormularioWeb { get; set; }
        public string CodigoFormulario { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public int IdMes { get; set; }
        public string Mes { get; set; }
        public int IdAnno { get; set; }
        public string Anno { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime FechaInicio { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime FechaFin { get; set; }
        public int IdFuente { get; set; }
        public string Mensaje { get; set; }
        public string Formulario { get; set; }
        public int IdEstado { get; set; }
        public string Estado { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }


        [NotMapped]
        public bool RangoFecha { get; set; }
        [NotMapped]
        public List<DetalleRegistroIndicadorFonatel> DetalleRegistroIndcadorFonatel { get; set; }

        [NotMapped]
        public string Solicitudid { get; set; }

        [NotMapped]
        public string FormularioId { get; set; }

        [NotMapped]
        public string IndicadorId { get; set; }

        [NotMapped]
        public EstadoRegistro EstadoRegistro { get; set; }

        [NotMapped]

        public FuenteRegistro Fuente { get; set; }

        [NotMapped]
        public Solicitud Solicitud { get; set; }

        public override string ToString()
        {
            StringBuilder json = new StringBuilder();
            json.Append("{\"IdSolicitud\":\"").Append(this.IdSolicitud).Append("\",");
            json.Append("\"idFormularioWeb\":\"").Append(this.idFormularioWeb).Append("\",");
            json.Append("\"Codigo\":\"").Append(this.Codigo).Append("\",");
            json.Append("\"Nombre\":\"").Append(this.Nombre).Append("\",");
            json.Append("\"IdMes\":\"").Append(this.IdMes).Append("\",");
            json.Append("\"Mes\":\"").Append(this.Mes).Append("\",");
            json.Append("\"IdAnno\":\"").Append(this.IdAnno).Append("\",");
            json.Append("\"Anno\":\"").Append(this.Anno).Append("\",");          
            json.Append("\"FechaInicio\":\"").Append(this.FechaInicio).Append("\",");
            json.Append("\"FechaFin\":\"").Append(this.FechaFin).Append("\",");
            json.Append("\"IdFuente\":\"").Append(this.IdFuente).Append("\",");
            json.Append("\"Mensaje\":\"").Append(this.Mensaje).Append("\",");            
            json.Append("\"Formulario\":\"").Append(this.Formulario).Append("\",");
            json.Append("\"IdEstado\":\"").Append(this.IdEstado).Append("\",");
            json.Append("\"Estado\":\"").Append(this.Estado).Append("\",");
            json.Append("\"UsuarioModificacion\":\"").Append(this.UsuarioModificacion);

            string resultado = json.ToString();

            return resultado;
        }

    }
}
