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
    public class TipoCategoriaDAL : BitacoraDAL
    {
        private  SIMEFContext db;
        #region Metodos Consulta Base de Datos
        /// <summary>
        /// Metodo que carga los registros de categorias de desagregación según parametros
        /// fecha 03-08-2022
        /// Michael Hernandez
        /// </summary>
        /// <returns>Lista</returns>
        public List<TipoCategoria> ObtenerDatos(TipoCategoria objTipoCategoria)
        {
            List<TipoCategoria> ListaCategoria = new List<TipoCategoria>();
            using (db = new SIMEFContext())
            {
                ListaCategoria = db.Database.SqlQuery<TipoCategoria>
                    ("execute pa_ObtenerTipoCategoria @idTipoCategoria",
                     new SqlParameter("@idTipoCategoria", objTipoCategoria.idTipoCategoria)
                    ).ToList();
            }
            return ListaCategoria;
        }
        #endregion
    }
}
