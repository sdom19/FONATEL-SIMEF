//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GB.SUTEL.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class ConstructorCriterioDetalleAgrupacion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ConstructorCriterioDetalleAgrupacion()
        {
            this.ConstructorCriterioDetalleAgrupacion1 = new HashSet<ConstructorCriterioDetalleAgrupacion>();
            this.DetalleRegistroIndicador = new HashSet<DetalleRegistroIndicador>();
            this.NivelDetalleReglaEstadistica = new HashSet<NivelDetalleReglaEstadistica>();
        }
    
        public System.Guid IdConstructorCriterio { get; set; }
        public System.Guid IdConstructor { get; set; }
        public string IdCriterio { get; set; }
        public string IdOperador { get; set; }
        public int IdAgrupacion { get; set; }
        public Nullable<System.Guid> IdConstructorDetallePadre { get; set; }
        public Nullable<int> IdTipoValor { get; set; }
        public Nullable<int> IdNivelDetalle { get; set; }
        public Nullable<int> IdNivel { get; set; }
        public Nullable<byte> UltimoNivel { get; set; }
        public Nullable<byte> Borrado { get; set; }
        public int IdDetalleAgrupacion { get; set; }
        public Nullable<int> Orden { get; set; }
        public Nullable<byte> UsaReglaEstadisticaConNivelDetalle { get; set; }
        public Nullable<byte> UsaReglaEstadistica { get; set; }
        public Nullable<int> OrdenCorregido { get; set; }
        public Nullable<int> IdNivelDetalleGenero { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConstructorCriterioDetalleAgrupacion> ConstructorCriterioDetalleAgrupacion1 { get; set; }
        public virtual ConstructorCriterioDetalleAgrupacion ConstructorCriterioDetalleAgrupacion2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetalleRegistroIndicador> DetalleRegistroIndicador { get; set; }
        public virtual Regla Regla { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NivelDetalleReglaEstadistica> NivelDetalleReglaEstadistica { get; set; }
        public virtual DetalleAgrupacion DetalleAgrupacion { get; set; }
    }
}
