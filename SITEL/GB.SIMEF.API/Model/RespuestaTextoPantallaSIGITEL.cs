using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SIMEF.API.Models
{
    public class RespuestaTextoPantallaSIGITEL
    {
        public int IdContenidoPantallaSIGITEL { get; set; }
        public int IdCatalogoPantallaSIGITEL { get; set; }
        public int IdTipoContenidoTextoSIGITEL { get; set; }
        public string Texto { get; set; }
        public string RutaImagen { get; set; }
        public int Orden { get; set; }
        public bool Estado { get; set; }
    }
}
