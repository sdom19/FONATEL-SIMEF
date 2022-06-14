using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SutelSolution
{
    public class DetalleAgrupacionSP
    {


        public int Id_Detalle_Agrupacion { get; set; }
        public string Nombre_Detalle_Agrupacion { get; set; }
        public string Nombre_Agrupacion { get; set; }
        public string Nombre_Operador { get; set; }
        public string Nombre_Criterio { get; set; }
        public int ID_Frecuencia { get; set; }
        public string Nombre_Frecuencia { get; set; }
        public int Cantidad_Meses_Frecuencia { get; set; }
        public int ID_Desglose { get; set; }
        public string Nombre_Desglose { get; set; }
        public int Cantidad_Meses_Desglose { get; set; }
        public string ID_Indicador { get; set; }
        public string Nombre_Indicador { get; set; }
        public string Nivel { get; set; }
        public string Valor_Inferior { get; set; }
        public string Valor_Superior { get; set; }
        public Nullable<int> Id_Tipo_Valor { get; set; }
        public string Tipo_Valor { get; set; }
        public string Valor_Formato { get; set; }
        public Nullable<Guid> Id_ConstructorCriterio { get; set; }
        public Nullable<Guid> Id_Padre_ConstructorCriterio { get; set; }
        public Nullable<int> Id_Padre_Detalle_Agrupacion { get; set; }
        public string Nombre_Padre_Detalle_Agrupacion { get; set; }
        public DateTime Fecha_Inicio { get; set; }
        public DateTime Fecha_Fin { get; set; }
        public DateTime FechaBaseParaCrearExcel { get; set; }
        public string Constructor_Criterio_Ayuda { get; set; }
        public string Descripcion_Criterio { get; set; }
        public string Nombre_Direccion { get; set; }
        public string Nombre_Servicio { get; set; }
        public Guid Id_Solicitud_Indicador { get; set; }
        public Guid Id_Solicitud_Constructor { get; set; }
        public string Tipo_Nivel_Detalle { get; set; }
        public string Id_Tipo_Nivel_Detalle { get; set; }
        public string Tabla_Tipo_Nivel_Detalle { get; set; }
        public Nullable<int> Id_Genero { get; set; }
        public Nullable<int> Id_Provincia { get; set; }
        public Nullable<int> Id_Canton { get; set; }
        public Nullable<int> Id_Distrito { get; set; }
        public byte UsaReglaEstadisticaConNivelDetalle { get; set; }
        public Nullable<int> Id_Tipo_Nivel_Detalle_Genero { get; set; }
    }
}
