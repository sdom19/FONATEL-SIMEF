using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SUTEL.Entities.DTO
{
    public class DetalleAgrupacionesPorOperadorDto
    {
        public string Id_Detalle_Agrupacion { get; set; }
        public string Nombre_Detalle_Agrupacion { get; set; }
        public string Nombre_Agrupacion { get; set; }
        public string Nombre_Operador { get; set; }
        public string IdOperador { get; set; }
        public string Nombre_Criterio { get; set; }
        public string ID_Frecuencia { get; set; }
        public string Nombre_Frecuencia { get; set; }
        public string Cantidad_Meses_Frecuencia { get; set; }
        public string ID_Desglose { get; set; }
        public string Nombre_Desglose { get; set; }
        public string Cantidad_Meses_Desglose { get; set; }
        public string ID_Indicador { get; set; }
        public string Nombre_Indicador { get; set; }
        public string Nivel { get; set; }
        public int IdNivel { get; set; }
        public string Valor_Inferior { get; set; }
        public string Valor_Superior { get; set; }
        public string Id_Tipo_Valor { get; set; }
        public string Tipo_Valor { get; set; }
        public string Valor_Formato { get; set; }
        public Guid Id_ConstructorCriterio { get; set; }
        public Guid Id_Padre_ConstructorCriterio { get; set; }
        public bool Tiene_Hijos { get; set; }
        public string Id_Padre_Detalle_Agrupacion { get; set; }
        public string Nombre_Padre_Detalle_Agrupacion { get; set; }
        public string Fecha_Inicio { get; set; }
        public string Fecha_Fin { get; set; }
        public string FechaBaseParaCrearExcel { get; set; }
        public string UsaReglaEstadisticaConNivelDetalle { get; set; }
        public string Constructor_Criterio_Ayuda { get; set; }
        public string Descripcion_Criterio { get; set; }
        public string Id_Solicitud_Indicador { get; set; }
        public string Id_Solicitud_Constructor { get; set; }
        public string Nombre_Direccion { get; set; }
        public string Nombre_Servicio { get; set; }
        public int UltimoNivel { get; set; }
        public string Tipo_Nivel_Detalle { get; set; }
        public string Id_Tipo_Nivel_Detalle { get; set; }
        public string Tabla_Tipo_Nivel_Detalle { get; set; }
        public string IDNIVELDETALLEGENERO { get; set; }
        public string IdSemaforo { get; set; }
        public string IdSemaforohijo { get; set; }
        public int MesInicial { get; set; }
        public string ayuda { get; set; }
        public int NivelMaximo { get; set; }
        public int NivelMinimo { get; set; }
        public bool NivelDetalle { get; set; }
        public Operador listaOperador { get; set; }
        public List<DetalleAgrupacionesPorOperadorDto> Hijos { get; set; }
        public List<Zona> listaZonasTienenDatos { get; set; }
    }
}
