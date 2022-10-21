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
                List<CategoriasDesagregacion> categoriasDesagregacion = ObtenerCategoria();

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
                    Completo = categoriasDesagregacion.Where(i=>i.idCategoria==x.idCategoria)
                                        .Single().CantidadDetalleDesagregacion== ListaCategoriaDetalle.Count() ? true : false,
                    CodigoCategoria= categoriasDesagregacion.Where(i => i.idCategoria == x.idCategoria)
                                        .Single().Codigo,
                    id = Utilidades.Encriptar(x.idCategoriaDetalle.ToString()),
                    categoriaid = Utilidades.Encriptar(x.idCategoria.ToString())
                }).ToList();
            }
            return ListaCategoriaDetalle;
        }


        private List<CategoriasDesagregacion> ObtenerCategoria(int id=0)
        {
            if(id==0)
            {
                return db.CategoriasDesagregacion.ToList();
            }
            else
            {
                return db.CategoriasDesagregacion.Where(x=>x.idCategoria==id).ToList();
            }
        }


        public List<DetalleCategoriaTexto> ActualizarDatos(DetalleCategoriaTexto objCategoria)
        {
            List<DetalleCategoriaTexto> ListaCategoriaDetalle = new List<DetalleCategoriaTexto>();
            using (db = new SIMEFContext())
            {
                CategoriasDesagregacion categoriasDesagregacion = ObtenerCategoria(objCategoria.idCategoria).Single();
                ListaCategoriaDetalle = db.Database.SqlQuery<DetalleCategoriaTexto>
                    ("execute spActualizarDetalleCategoriaTexto @idCategoriaDetalle, @idCategoria,@codigo,@Etiqueta,@Estado",
                      new SqlParameter("@idCategoriaDetalle", objCategoria.idCategoriaDetalle),
                      new SqlParameter("@idCategoria", objCategoria.idCategoria),
                      new SqlParameter("@codigo", objCategoria.Codigo),
                      new SqlParameter("@Etiqueta", objCategoria.Etiqueta),
                      new SqlParameter("@Estado", objCategoria.Estado)
                    ).ToList();
                ListaCategoriaDetalle = ListaCategoriaDetalle.Select(x => new DetalleCategoriaTexto()
                {
                    idCategoriaDetalle = x.idCategoriaDetalle,
                    idCategoria = x.idCategoria,
                    Codigo = x.Codigo,
                    Estado = x.Estado,
                    Etiqueta = x.Etiqueta,
                    Completo = categoriasDesagregacion.CantidadDetalleDesagregacion == ListaCategoriaDetalle.Count() ? true : false,
                    CodigoCategoria = categoriasDesagregacion.Codigo,
                    id = Utilidades.Encriptar(x.idCategoriaDetalle.ToString()),
                    categoriaid = Utilidades.Encriptar(x.idCategoria.ToString())
                }).ToList();
            }

            return ListaCategoriaDetalle;
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
