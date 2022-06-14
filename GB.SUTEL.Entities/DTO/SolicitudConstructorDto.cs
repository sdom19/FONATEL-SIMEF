using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SUTEL.Entities.DTO
{
    public class SolicitudConstructorDto
    {
        public System.Guid IdSolicitudContructor { get; set; }
        public System.Guid IdSolicitudIndicador { get; set; }
        //public System.Guid IdConstructor { get; set; }
        public int IdEstado { get; set; }
        public string IdOperador { get; set; }
        public byte Borrado { get; set; }
        //public byte FormularioWeb { get; set; }
        public Nullable<int> OrdenIndicadorEnExcel { get; set; }
        public Nullable<bool> Cargado { get; set; }
        public Nullable<int> CantidadCarga { get; set; }
        public Nullable<int> IdSemaforo { get; set; }
        public string IdIndicador { get; set; }
        public string NombreIndicador { get; set; }
        public Operador listaOperador { get; set; }
    }
}
