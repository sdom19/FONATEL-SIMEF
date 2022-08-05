using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;

namespace GB.SIMEF.DAL
{
    public class DetalleCategoriaTextoDAL
    {
        private  SIMEFContext db;
        #region Metodos Consulta Base de Datos
        /// <summary>
        /// Metodo que carga los registros de categorias de desagregación tipo texto
        /// fecha 03-08-2022
        /// Michael Hernandez
        /// </summary>
        /// <returns>Lista</returns>
        /// 
        
        public List<DetalleCategoriaTexto> ObtenerDatos(DetalleCategoriaTexto objCategoria)
        {
            List<DetalleCategoriaTexto> ListaCategoria = new List<DetalleCategoriaTexto>();
            using (db = new SIMEFContext())
            {
                ListaCategoria = db.Database.SqlQuery<DetalleCategoriaTexto>
                    ("execute spObtenerDetalleCategoriaTexto @idCategoria,@codigo",
                     new SqlParameter("@idCategoria", objCategoria.idCategoria),
                     new SqlParameter("@codigo", objCategoria.Codigo)
                    ).ToList();
            }
            return ListaCategoria;
        }
        #endregion
    }
}
