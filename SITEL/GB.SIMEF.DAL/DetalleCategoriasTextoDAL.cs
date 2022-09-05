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
    public class DetalleCategoriaTextoDAL:BitacoraDAL
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
            List<DetalleCategoriaTexto> ListaCategoriaDetalle = new List<DetalleCategoriaTexto>();
            using (db = new SIMEFContext())
            {
                ListaCategoriaDetalle = db.Database.SqlQuery<DetalleCategoriaTexto>
                    ("execute spObtenerDetalleCategoriasTexto @idCategoriaDetalle, @idCategoria,@codigo, @Etiqueta",
                      new SqlParameter("@idCategoriaDetalle", objCategoria.idCategoriaDetalle),
                      new SqlParameter("@idCategoria", objCategoria.idCategoria),
                      new SqlParameter("@codigo", objCategoria.Codigo),
                      new SqlParameter("@Etiqueta", string.IsNullOrEmpty( objCategoria.Etiqueta)?DBNull.Value.ToString():objCategoria.Etiqueta)
                    ).ToList();

                ListaCategoriaDetalle = ListaCategoriaDetalle.Select(x => new DetalleCategoriaTexto()
                {
                    idCategoriaDetalle = x.idCategoriaDetalle,
                    idCategoria = x.idCategoria,
                    Codigo = x.Codigo,
                    Estado = x.Estado,
                    Etiqueta = x.Etiqueta,
                    Completo = db.CategoriasDesagregacion.Where(i => i.idCategoria == x.idCategoria).Single().CantidadDetalleDesagregacion
                    == ListaCategoriaDetalle.Count()?true:false,
                    id = Utilidades.Encriptar(x.idCategoriaDetalle.ToString()),
                }).ToList();
            }
            return ListaCategoriaDetalle;
        }


        public List<DetalleCategoriaTexto> ActualizarDatos(DetalleCategoriaTexto objCategoria)
        {
            List<DetalleCategoriaTexto> ListaCategoria = new List<DetalleCategoriaTexto>();
            using (db = new SIMEFContext())
            {
                ListaCategoria = db.Database.SqlQuery<DetalleCategoriaTexto>
                    ("execute spActualizarDetalleCategoriaTexto @idCategoriaDetalle, @idCategoria,@codigo,@Etiqueta,@Estado",
                      new SqlParameter("@idCategoriaDetalle", objCategoria.idCategoriaDetalle),
                      new SqlParameter("@idCategoria", objCategoria.idCategoria),
                      new SqlParameter("@codigo", objCategoria.Codigo),
                      new SqlParameter("@Etiqueta", objCategoria.Etiqueta),
                      new SqlParameter("@Estado", objCategoria.Estado)
                    ).ToList();
            }

            return ListaCategoria;
        }


        public void DeshabilitarDatos(DetalleCategoriaTexto objCategoria)
        {
            using (db = new SIMEFContext())
            {
                 db.Database.SqlQuery<DetalleCategoriaTexto>
                    ("execute spDeshabilitarDetalleCategoriaTexto @idCategoria",
                      new SqlParameter("@idCategoria", objCategoria.idCategoria)
                    );
            } 
        }



        #endregion
    }
}
