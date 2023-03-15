namespace SIMEF.API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("DefinicionIndicador")]
    public class DefinicionIndicador
    {
        public DefinicionIndicador()
        {
           
        }
        [Key]
        public string IdDefinicionIndicador { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public int IdGrupoIndicador { get; set; }
        public string Grupo { get; set; }
        public int IdTipoIndicador { get; set; }
        public string TipoIndicador { get; set; }
        public string Fuente { get; set; }
        public string Nota { get; set; }
        public int IdIndicador { get; set; }
        public int IdEstado { get; set; }
        public string Definicion { get; set; }
        
    }
}