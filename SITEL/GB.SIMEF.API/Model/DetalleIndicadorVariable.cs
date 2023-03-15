

namespace SIMEF.API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("DetalleIndicadorVariable")]
    public partial class DetalleIndicadorVariable
    {
        
        public DetalleIndicadorVariable()
        {
            
        }
        [Key]
        public int IdDetalleIndicadorVariable { get; set; }
        public int idIndicador { get; set; }
        public string NombreVariable { get; set; }
    
       // public virtual CategoriasDesagregacion CategoriasDesagregacion { get; set; }
        //public virtual Indicador Indicador { get; set; }

        //public virtual ICollection<DetalleRegistroIndicadorVariable> DetalleRegistroIndicadorVariable { get; set; }

        //public virtual ICollection<FormulasCalculo> FormulasCalculo { get; set; }
    }
}
