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
        [NotMapped]
        public bool OpcionEliminar { get; set; }

        [NotMapped]
        public List<RelacionCategoriaAtributo> listaCategoriaAtributo { get; set; }

    }
}
