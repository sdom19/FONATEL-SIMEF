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
    
    public partial class BitacoraParametrizacionIndicador
    {
        public int IdBitacoraParametrizacionIndicador { get; set; }
        public string IdIndicador { get; set; }
        public int IdServicio { get; set; }
        public bool Visualiza { get; set; }
        public int AnnoPorOperador { get; set; }
        public int MesPorOperador { get; set; }
        public int AnnoPorTotal { get; set; }
        public int MesPorTotal { get; set; }
        public System.DateTime FechaPublicacion { get; set; }
        public System.TimeSpan HoraPublicacion { get; set; }
        public string UsuarioPublicador { get; set; }
        public Nullable<int> AnnoDesde { get; set; }
        public Nullable<int> MesDesde { get; set; }
    
        public virtual Indicador Indicador { get; set; }
        public virtual Servicio Servicio { get; set; }
    }
}
