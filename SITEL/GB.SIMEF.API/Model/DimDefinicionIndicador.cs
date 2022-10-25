namespace SIMEF.API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("DimDefinicionIndicador", Schema = "SIGITEL")]
    public class DimDefinicionIndicador
    {
        public DimDefinicionIndicador()
        {
           
        }
        [Key]
        public string IdDefinicion { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public int Idgrupo { get; set; }
        public string Grupo { get; set; }
        public int IdTipoidicador { get; set; }
        public string TipoIndicador { get; set; }
        public string Fuente { get; set; }
        public string Notas { get; set; }
        public int IdIndicador { get; set; }
        public int IdEstado { get; set; }
        public string Definicion { get; set; }
        
    }
}