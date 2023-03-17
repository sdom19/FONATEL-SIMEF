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
    public class TipoDetalleCategoriaDAL : BitacoraDAL
    {
        private  SIMEFContext db;
        #region Metodos Consulta Base de Datos
        /// <summary>
        /// Metodo que carga los registros de categorias de desagregación según parametros
        /// fecha 03-08-2022
        /// Michael Hernandez
        /// </summary>
        /// <returns>Lista</returns>
        public List<TipoDetalleCategoria> ObtenerDatos(TipoDetalleCategoria objTipoDetalleCategoria)
        {
            List<TipoDetalleCategoria> ListaCategoria = new List<TipoDetalleCategoria>();
            using (db = new SIMEFContext())
            {
                ListaCategoria = db.Database.SqlQuery<TipoDetalleCategoria>
                    ("execute pa_ObtenerTipoDetalleCategoria @idTipoCategoria",
                     new SqlParameter("@idTipoCategoria", objTipoDetalleCategoria.IdTipoDetalleCategoria)
                    ).ToList();
            }
            return ListaCategoria;
        }
        #endregion
    }
}
