using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SUTEL.Entities.DTO
{
    public class Zona
    {
        public int? idProvincia { get; set; }
        public int? idCanton { get; set; }
        public int? idDistrito { get; set; }
        public System.Guid? idConstructorCriterio { get; set; }

    }
}
