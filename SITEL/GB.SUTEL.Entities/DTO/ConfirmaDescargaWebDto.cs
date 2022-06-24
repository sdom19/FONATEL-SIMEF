using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SUTEL.Entities.DTO
{
    public class ConfirmaDescargaWebDto
    {
        public SolicitudIndicador solicitudIndicador { get; set; }
        public string direccion { get; set; }
        public int IdDireccion { get; set; }
        public string servicio { get; set; }
        public string frecuencia { get; set; }
        public string desglose { get; set; }
        public List<SolicitudConstructorDto> solicitudConstructor { get; set; }
        public List<DetalleAgrupacionesPorOperadorDto> detalleAgrupacionesPorOperador { get; set; }
        public List<TEMPDetalleRegistroIndicador> listaDetalleRegistroIndicador { get; set; }
        public int nivelMax { get; set; }
        public int nivelMin { get; set; }
        public List<string> listaOperador { get; set; }
        public List<string> listaSolicitudConstructor { get; set; }
        public List<string> listaIdOperador { get; set; }
		public List<string> listaAyudaConstructor { get; set; }
        public List<int?> listaIdSemaforo { get; set; }
        public List<System.Guid> listaIdsSolicitudConstructor { get; set; }
        public List<Canton> cantones { get; set; }
        public List<Provincia> provincias { get; set; }
        public List<Distrito> distritos { get; set; }
        public bool NivelDetalle { get; set; }
        public string observacion { get; set; }
        public int Mesinicial { get; set; }
    }
}
