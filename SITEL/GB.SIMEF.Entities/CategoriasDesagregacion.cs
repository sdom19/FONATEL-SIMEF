//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GB.SIMEF.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class CategoriasDesagregacion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CategoriasDesagregacion()
        {
            this.DetalleCategoriaNumerico = new HashSet<DetalleCategoriaNumerico>();
            this.DetalleCategoriaFecha = new HashSet<DetalleCategoriaFecha>();
            this.DetalleCategoriaTexto = new HashSet<DetalleCategoriaTexto>();
            this.DetalleIndicadorVariables = new HashSet<DetalleIndicadorVariables>();
            this.RelacionCategoria = new HashSet<RelacionCategoria>();
        }
    
        public int idCategoria { get; set; }
        public string Codigo { get; set; }
        public string NombreCategoria { get; set; }
        public Nullable<int> CantidadDetalleDesagregacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
    
        public virtual EstadoRegistro EstadoRegistro { get; set; }
        public virtual TipoCategoria TipoCategoria { get; set; }
        public virtual TiposDetalleCategoria TiposDetalleCategoria { get; set; }

        public virtual ICollection<DetalleCategoriaNumerico> DetalleCategoriaNumerico { get; set; }

        public virtual ICollection<DetalleCategoriaFecha> DetalleCategoriaFecha { get; set; }

        public virtual ICollection<DetalleCategoriaTexto> DetalleCategoriaTexto { get; set; }

        public virtual ICollection<DetalleIndicadorVariables> DetalleIndicadorVariables { get; set; }

        public virtual ICollection<RelacionCategoria> RelacionCategoria { get; set; }
    }
}
