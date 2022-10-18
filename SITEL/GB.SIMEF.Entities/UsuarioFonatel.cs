using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.Entities
{
    [Table("UsuarioFonatel")]
    public partial class UsuarioFonatel
    {
      [Key]
      public int IdUsuario { get; set; }
      public string AccesoUsuario { get; set; } 
      public string NombreUsuario { get; set; }  
      public string Contrasena { get; set; }  
      public string CorreoUsuario { get; set; }
      public bool Activo { get; set; }  
      public bool borrado { get; set; }
      public bool FONATEL { get; set; }
      public DateTime  FechaRegistro { get; set; }

      [NotMapped]
      public string ContrasenaSinEncriptar { get; set; }

      [NotMapped]
      public bool EnviaCorreo { get; set; }
    }
}
