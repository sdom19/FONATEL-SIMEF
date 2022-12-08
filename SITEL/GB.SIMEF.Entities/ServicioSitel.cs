using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.Entities
{
    // Entidad espejo que proviene del namespace SITEL
    public class ServicioSitel
    {
        public ServicioSitel()
        {

        }

        public int IdServicio { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }

        [NotMapped]
        public string id { get; set; }
    }
}
