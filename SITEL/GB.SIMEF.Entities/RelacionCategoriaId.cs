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

        public RelacionCategoriaId()
        {

        }
        [Key]
        public int idRelacion { get; set; }

        public int idCategoriaId { get; set; }
    }
}
