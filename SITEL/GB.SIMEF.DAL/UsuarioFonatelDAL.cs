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
        private SIMEFContext db;
        
        /// <summary>
        /// 23/08/2022
        /// Michael
        /// Función que retorna todos los usuarios activos en la bd sitel .
        /// </summary>
        /// <returns></returns>
        public List<UsuarioFonatel> ObtenerDatos()
        {
            using (db=new SIMEFContext())
            {
                return db.UsuarioFonatel.ToList();
            }
        
        }



        public List<UsuarioFonatel> ActualizarUsuarioSitel(UsuarioFonatel objUser)
        {
            using (db = new SIMEFContext())
            {
                return db.Database.SqlQuery<UsuarioFonatel>("exec sitel.spCrearUsuarioSitel @IdUsuario ,@AccesoUsuario, @NombreUsuario,@Contrasena,@CorreoUsuario,@Activo,@borrado,@FONATEL",
                new SqlParameter("@IdUsuario", objUser.IdUsuario),
                new SqlParameter("@AccesoUsuario", objUser.AccesoUsuario),
                new SqlParameter("@NombreUsuario", objUser.NombreUsuario),
                new SqlParameter("@Contrasena", objUser.Contrasena),
                new SqlParameter("@CorreoUsuario", objUser.CorreoUsuario),
                new SqlParameter("@Activo", objUser.Activo),
                new SqlParameter("@borrado", objUser.borrado),
                new SqlParameter("@FONATEL", objUser.FONATEL)
                ).ToList();
            }
        }


    }
}
