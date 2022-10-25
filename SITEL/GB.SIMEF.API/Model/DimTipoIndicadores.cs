

namespace SIMEF.API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("DimTipoIndicadores", Schema = "SIMEF")]
    public partial class DimTipoIndicadores
    {
        public DimTipoIndicadores()
        {
        }

        [Key]
        public int IdTipoIdicador { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }

        #region Variables que no forman parte del contexto
        [NotMapped]
        public string id { get; set; }
        [NotMapped]
        public bool nuevoEstado { get; set; }
        #endregion
    }
}
