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
    
    public partial class ReglaComparacionIndicador
    {
        public int idCompara { get; set; }
        public int IdIndicadorCompara { get; set; }
        public Nullable<int> idVariableCompara { get; set; }
        public Nullable<int> idvariable { get; set; }
        public bool Estado { get; set; }
    
        public virtual Operadores Operadores { get; set; }
        public virtual ReglaValidacionTipo ReglaValidacionTipo { get; set; }
    }
}
