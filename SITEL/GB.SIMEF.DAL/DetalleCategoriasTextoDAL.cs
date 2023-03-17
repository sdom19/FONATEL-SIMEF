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
                List<CategoriaDesagregacion> categoriasDesagregacion = ObtenerCategoria();

                ListaCategoriaDetalle = db.Database.SqlQuery<DetalleCategoriaTexto>
                    ("execute pa_ObtenerDetalleCategoriaTexto @idCategoriaDetalle, @idCategoria,@codigo, @Etiqueta",
                      new SqlParameter("@idCategoriaDetalle", objCategoria.idDetalleCategoriaTexto),
                      new SqlParameter("@idCategoria", objCategoria.idCategoriaDesagregacion),
                      new SqlParameter("@codigo", objCategoria.Codigo),
                      new SqlParameter("@Etiqueta", string.IsNullOrEmpty( objCategoria.Etiqueta)?DBNull.Value.ToString():objCategoria.Etiqueta)
                    ).ToList();

                ListaCategoriaDetalle = ListaCategoriaDetalle.Select(x => new DetalleCategoriaTexto()
                {
                    idDetalleCategoriaTexto = x.idDetalleCategoriaTexto,
                    idCategoriaDesagregacion = x.idCategoriaDesagregacion,
                    Codigo = x.Codigo,
                    Estado = x.Estado,
                    Etiqueta = x.Etiqueta,
                    Completo = categoriasDesagregacion.Where(i=>i.idCategoriaDesagregacion==x.idCategoriaDesagregacion)
                                        .Single().CantidadDetalleDesagregacion== ListaCategoriaDetalle.Count() ? true : false,
                    CodigoCategoria= categoriasDesagregacion.Where(i => i.idCategoriaDesagregacion == x.idCategoriaDesagregacion)
                                        .Single().Codigo,
                    id = Utilidades.Encriptar(x.idDetalleCategoriaTexto.ToString()),
                    categoriaid = Utilidades.Encriptar(x.idCategoriaDesagregacion.ToString())
                }).ToList();
            }
            return ListaCategoriaDetalle;
        }


        private List<CategoriaDesagregacion> ObtenerCategoria(int id=0)
        {
            if(id==0)
            {
                return db.CategoriasDesagregacion.ToList();
            }
            else
            {
                return db.CategoriasDesagregacion.Where(x=>x.idCategoriaDesagregacion==id).ToList();
            }
        }


        public List<DetalleCategoriaTexto> ActualizarDatos(DetalleCategoriaTexto objCategoria)
        {
            List<DetalleCategoriaTexto> ListaCategoriaDetalle = new List<DetalleCategoriaTexto>();
            using (db = new SIMEFContext())
            {
                CategoriaDesagregacion categoriasDesagregacion = ObtenerCategoria(objCategoria.idCategoriaDesagregacion).Single();
                ListaCategoriaDetalle = db.Database.SqlQuery<DetalleCategoriaTexto>
                    ("execute pa_ActualizarDetalleCategoriaTexto @idCategoriaDetalle, @idCategoria,@codigo,@Etiqueta,@Estado",
                      new SqlParameter("@idCategoriaDetalle", objCategoria.idDetalleCategoriaTexto),
                      new SqlParameter("@idCategoria", objCategoria.idCategoriaDesagregacion),
                      new SqlParameter("@codigo", objCategoria.Codigo),
                      new SqlParameter("@Etiqueta", objCategoria.Etiqueta),
                      new SqlParameter("@Estado", objCategoria.Estado)
                    ).ToList();
                ListaCategoriaDetalle = ListaCategoriaDetalle.Select(x => new DetalleCategoriaTexto()
                {
                    idDetalleCategoriaTexto = x.idDetalleCategoriaTexto,
                    idCategoriaDesagregacion = x.idCategoriaDesagregacion,
                    Codigo = x.Codigo,
                    Estado = x.Estado,
                    Etiqueta = x.Etiqueta,
                    Completo = categoriasDesagregacion.CantidadDetalleDesagregacion == ListaCategoriaDetalle.Count() ? true : false,
                    CodigoCategoria = categoriasDesagregacion.Codigo,
                    id = Utilidades.Encriptar(x.idDetalleCategoriaTexto.ToString()),
                    categoriaid = Utilidades.Encriptar(x.idCategoriaDesagregacion.ToString())
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
                      new SqlParameter("@idCategoria", objCategoria.idCategoriaDesagregacion)
                    );
            } 
        }



        #endregion
    }
}
