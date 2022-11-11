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
    public class DetalleRelacionCategoriaDAL : BitacoraDAL
    {
        private SIMEFContext db;

        private CategoriasDesagregacionDAL categoriaDAL;

        private List<CategoriasDesagregacion> listaCategoria;

        

        public DetalleRelacionCategoriaDAL()
        {
            categoriaDAL = new CategoriasDesagregacionDAL();
            listaCategoria = ObtenerCategoriaDesagregacion();
        }

        #region Metodos de Consulta a Base Datos
        /// <summary>
        /// Autor: Francisco Vindas Ruiz
        /// Fecha: 24/08/2022
        /// Ejecutar procedimiento almacenado para obtener datos del detalle relacion categoria
        /// </summary>
        /// <param name="objRelacionCategoria"></param>
        /// <returns></returns>
        public List<DetalleRelacionCategoria> ObtenerDatos(DetalleRelacionCategoria objDetalle)
        {
            List<DetalleRelacionCategoria> ListaDetalle = new List<DetalleRelacionCategoria>();



            using (db = new SIMEFContext())
            {
                ListaDetalle = db.Database.SqlQuery<DetalleRelacionCategoria>
                    ("execute spObtenerDetalleRelacionCategoria @idDetalleRelacionCategoria, @idRelacionCategoria, @idCategoriaAtributo , @idCategoriaDetalle ",
                      new SqlParameter("@idDetalleRelacionCategoria", objDetalle.idDetalleRelacionCategoria),
                      new SqlParameter("@idRelacionCategoria", objDetalle.IdRelacionCategoria),
                      new SqlParameter("@idCategoriaAtributo", objDetalle.idCategoriaAtributo),
                      new SqlParameter("@idCategoriaDetalle ", objDetalle.idCategoriaDetalle)
                    ).ToList();

                ListaDetalle = ListaDetalle.Select(x => new DetalleRelacionCategoria()
                {
                    idDetalleRelacionCategoria = x.idDetalleRelacionCategoria,
                    IdRelacionCategoria = x.IdRelacionCategoria,
                    idCategoriaAtributo = x.idCategoriaAtributo,
                    NombreCategoria = listaCategoria.Where(i => i.idCategoria == x.idCategoriaAtributo).Single().NombreCategoria,
                    DetalleCategoriaTexto = listaCategoria.Where(i => i.idCategoria == x.idCategoriaAtributo).Single()
                                             .DetalleCategoriaTexto.Where(i => i.idCategoriaDetalle == x.idCategoriaDetalle).Single(),
                    idCategoriaDetalle = x.idCategoriaDetalle,
                    Estado = x.Estado,
                    Completo = db.RelacionCategoria.Where(i => i.idRelacionCategoria == x.IdRelacionCategoria).Single().CantidadCategoria == ListaDetalle.Count() ? true : false,
                    id = Utilidades.Encriptar(x.idDetalleRelacionCategoria.ToString()),
                }).ToList();


            }




            return ListaDetalle;
        }

        /// <summary>
        /// Autor: Francisco Vindas Ruiz
        /// Fecha: 24/08/2022
        /// Ejecutar procedimiento almacenado para insertar y editar datos detalle relacion categoria
        /// </summary>
        /// <param name="objRelacionCategoria"></param>
        /// <returns></returns>
        public List<DetalleRelacionCategoria> ActualizarDatos(DetalleRelacionCategoria objDetalle)
        {
            List<DetalleRelacionCategoria> ListaDetalle = new List<DetalleRelacionCategoria>();
            using (db = new SIMEFContext())
            {
                ListaDetalle = db.Database.SqlQuery<DetalleRelacionCategoria>
                    ("execute spActualizarDetalleRelacionCategoria @idDetalleRelacionCategoria, @IdRelacionCategoria, @idCategoriaAtributo, @idCategoriaDetalle , @Estado",
                      new SqlParameter("@idDetalleRelacionCategoria", objDetalle.idDetalleRelacionCategoria),
                      new SqlParameter("@IdRelacionCategoria", objDetalle.IdRelacionCategoria),
                      new SqlParameter("@idCategoriaAtributo", objDetalle.idCategoriaAtributo),
                      new SqlParameter("@idCategoriaDetalle ", objDetalle.idCategoriaDetalle),
                      new SqlParameter("@Estado", objDetalle.Estado)
                    ).ToList();
                ListaDetalle = ListaDetalle.Select(x => new DetalleRelacionCategoria()
                {
                    idDetalleRelacionCategoria = x.idDetalleRelacionCategoria,
                    IdRelacionCategoria = x.IdRelacionCategoria,
                    idCategoriaAtributo = x.idCategoriaAtributo,
                    NombreCategoria = listaCategoria.Where(i => i.idCategoria == x.idCategoriaAtributo).Single().NombreCategoria,
                    DetalleCategoriaTexto = listaCategoria.Where(i => i.idCategoria == x.idCategoriaAtributo).Single()
                             .DetalleCategoriaTexto.Where(i => i.idCategoriaDetalle == x.idCategoriaDetalle).Single(),
                    idCategoriaDetalle = x.idCategoriaDetalle,
                    Estado = x.Estado,
                    Completo = db.RelacionCategoria.Where(i => i.idRelacionCategoria == x.IdRelacionCategoria).Single().CantidadCategoria == ListaDetalle.Count() ? true : false,
                    id = Utilidades.Encriptar(x.idDetalleRelacionCategoria.ToString()),

                }).ToList();
            }



            

            return ListaDetalle;
        }


        private List<CategoriasDesagregacion> ObtenerCategoriaDesagregacion()
        {
            return categoriaDAL.ObtenerDatos(new CategoriasDesagregacion());
        }






    }

    #endregion

}

