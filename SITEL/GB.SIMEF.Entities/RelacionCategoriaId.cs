using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RelacionCategoriaId")]
    public partial class RelacionCategoriaId
    {
        [Key, Column(Order = 0)]
        public int idRelacion { get; set; }

        [Key, Column(Order = 1)]
        public string idCategoriaId { get; set; }

        public int idEstado { get; set; }

        #region Variables que no estan en la entiendad
        [NotMapped]
        public bool OpcionEliminar { get; set; }

        [NotMapped]
        public List<RelacionCategoriaAtributo> listaCategoriaAtributo { get; set; }


        [NotMapped]
        public string RelacionId { get; set; }

        [NotMapped]
        public virtual EstadoRegistro EstadoRegistro { get; set; }
        #endregion
    }
}
