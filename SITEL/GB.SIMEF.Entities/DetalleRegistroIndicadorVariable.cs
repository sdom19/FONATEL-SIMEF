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
    
    public partial class DetalleRegistroIndicadorVariable
    {
        public int IdDetalleRegistroindicador { get; set; }
        public int IdRegistroIndicador { get; set; }
        public int Valor { get; set; }
    
        public virtual DetalleIndicadorVariables DetalleIndicadorVariables { get; set; }
    }
}
