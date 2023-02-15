using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GB.SIMEF.API.Model
{
    public partial class DimDetalleIndicadorCategoria
    {
        public DimDetalleIndicadorCategoria()
        {

        }
        [Key]
        public int idDetalleIndicador { get; set; }
        public int idIndicador { get; set; }
        public int idCategoria { get; set; }
        public string NombreCategoria { get; set; }
    }
}
