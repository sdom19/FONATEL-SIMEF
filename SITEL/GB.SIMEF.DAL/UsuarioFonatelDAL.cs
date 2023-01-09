using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.DAL
{
    public class UsuarioFonatelDAL 
    {
        private SITELContext db;

        /// <summary>
        /// 23/08/2022
        /// Michael
        /// Función que retorna todos los usuarios activos en la bd sitel .
        /// </summary>
        /// <returns></returns>

        public List<Usuario> ObtenerDatos(byte activo = 1)
        {
            using (db = new SITELContext())
            {
                return db.Usuario.Where(x=>x.Borrado==0 && x.Activo==activo && x.FONATEL==true).ToList();
            }
        }


        public List<Usuario> ActualizarUsuarioSitel(Usuario objUser)
        {
            using (db = new SITELContext())
            {
                return db.Database.SqlQuery<Usuario>("exec FONATEL.pa_crearUsuarioSitelFonatel @IdUsuario ,@AccesoUsuario, @NombreUsuario,@Contrasena,@CorreoUsuario,@Activo,@borrado,@FONATEL",
                new SqlParameter("@IdUsuario", objUser.IdUsuario),
                new SqlParameter("@AccesoUsuario", objUser.AccesoUsuario),
                new SqlParameter("@NombreUsuario", objUser.NombreUsuario),
                new SqlParameter("@Contrasena", objUser.Contrasena),
                new SqlParameter("@CorreoUsuario", objUser.CorreoUsuario),
                new SqlParameter("@Activo", objUser.Activo),
                new SqlParameter("@borrado", objUser.Borrado),
                new SqlParameter("@FONATEL", objUser.FONATEL)
                ).ToList();
            }
        }


    }
}
