using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SUTEL.Entities.DTO
{
    public class ListaIDDto
    {
        public Guid Id_ConstructorCriterio { get; set; }
        public int Id_Tipo_Valor { get; set; }
        public int Numero_Desglose { get; set; }
        public string Valor { get; set; }
        public int Anno { get; set; }
        public int idZona { get; set; }
        public string  zona { get; set; }
        public int? idProvincia { get; set; }
        public int? idCanton { get; set; }
        public int? idDistrito { get; set; }
        
    }
}
