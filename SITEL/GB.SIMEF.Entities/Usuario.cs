using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.Entities
{

        [Table("Usuario")]
        public partial class Usuario
        {

            [Key]
            public int IdUsuario { get; set; }
            public string IdOperador { get; set; }
            public string AccesoUsuario { get; set; }
            public string NombreUsuario { get; set; }
            public string Contrasena { get; set; }
            public string CorreoUsuario { get; set; }
            public byte UsuarioInterno { get; set; }
            public byte Activo { get; set; }
            public byte Borrado { get; set; }
            public Nullable<bool> Mercado { get; set; }
            public Nullable<bool> Calidad { get; set; }
            public Nullable<bool> FONATEL { get; set; }
            public Nullable<System.DateTime> FechaRegistro { get; set; }
            [NotMapped]
            public string ContrasenaSinEncriptar { get; set; }
             [NotMapped]
             public bool EnviaCorreo { get; set; }
    }
}

