

namespace SIMEF.API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("DimDetalleIndicadorVariables", Schema = "FONTAEL")]
    public partial class DimDetalleIndicadorVariables
    {
        
        public DimDetalleIndicadorVariables()
        {
            
        }
        [Key]
        public int idDetalleIndicador { get; set; }
        public int idIndicador { get; set; }
        public string NombreVariable { get; set; }
    
       // public virtual CategoriasDesagregacion CategoriasDesagregacion { get; set; }
        //public virtual Indicador Indicador { get; set; }

        //public virtual ICollection<DetalleRegistroIndicadorVariable> DetalleRegistroIndicadorVariable { get; set; }

        //public virtual ICollection<FormulasCalculo> FormulasCalculo { get; set; }
    }
}
