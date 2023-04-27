using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.Entities
{
    [Table("ContenidoPantallaSIGITEL")]
    public class ContenidoPantallaSIGITEL
    {
        [Key]
        public int IdContenidoPantallaSIGITEL { get; set; }
        public int IdCatalogoPantallaSIGITEL { get; set; }
        public int IdTipoContenidoTextoSIGITEL { get; set; }
        public int Orden { get; set; }
        public bool Estado { get; set; }

        [NotMapped]
        public string IdCatalogoPantallaSIGITELString { get; set; }
        [NotMapped]
        public string RutaImagen { get; set; }
        [NotMapped]
        public string Texto { get; set; }
        [NotMapped]
        public CatalogoPantallaSIGITEL Pantalla { get; set; }
        [NotMapped]
        public TipoContenidoTextoSIGITEL TipoContenidoTextoSIGITEL { get; set; }

        public override string ToString()
        {
            StringBuilder json = new StringBuilder();

            json.Append("{\"Pantalla\":\"").Append(this.Pantalla.NombrePantalla).Append("\",");
            json.Append("\"Tipo detalle\":\"").Append(this.TipoContenidoTextoSIGITEL.NombreTipoContenido).Append("\",");

            if (this.IdTipoContenidoTextoSIGITEL == 4)
            {
                json.Append("\"Ruta imagen\":\"").Append(this.RutaImagen).Append("\",");
            }
            else 
            {
                json.Append("\"Descripción\":\"").Append(this.Texto).Append("\",");
            }

            json.Append("\"Estado\":\"").Append(this.Estado ? "Activo" : "Inactivo").Append("\",");
            json.Append("\"Orden\":").Append(this.Orden).Append("}");

            return json.ToString();
        }
    }
}
