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
    
    public partial class DefinicionIndicadores
    {
        public int idDefinición { get; set; }
        public string Fuente { get; set; }
        public string Notas { get; set; }
        public int idIndicador { get; set; }
        public int idEstado { get; set; }
    
        public virtual EstadoRegistro EstadoRegistro { get; set; }
        public virtual Indicador Indicador { get; set; }
    }
}
