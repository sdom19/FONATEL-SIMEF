

namespace SIMEF.API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("DimGrupoIndicadores", Schema = "FONATEL")]
    public partial class DimGrupoIndicadores
    {
        public DimGrupoIndicadores()
        {
        }

        [Key]
        public int idGrupo { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
        public string DetalleHtml { get; set; }

        #region Variables que no forman parte del contexto
        [NotMapped]
        public string id { get; set; }
        [NotMapped]
        public bool nuevoEstado { get; set; }
        #endregion
    }
}
