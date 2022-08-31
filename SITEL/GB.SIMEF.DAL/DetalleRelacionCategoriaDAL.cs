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
                    ("execute spObtenerDetalleRelacionCategoria @idDetalleRelacionCategoria, @idRelacionCategoria, @idCategoriaAtributo , @CategoriaAtributoValor",
                      new SqlParameter("@idDetalleRelacionCategoria", objDetalle.idDetalleRelacionCategoria),
                      new SqlParameter("@idRelacionCategoria", objDetalle.IdRelacionCategoria),
                      new SqlParameter("@idCategoriaAtributo", objDetalle.idCategoriaAtributo),
                      new SqlParameter("@CategoriaAtributoValor", string.IsNullOrEmpty(objDetalle.CategoriaAtributoValor) ? DBNull.Value.ToString() : objDetalle.CategoriaAtributoValor)
                    ).ToList();

                ListaDetalle = ListaDetalle.Select(x => new DetalleRelacionCategoria()
                {
                    idDetalleRelacionCategoria = x.idDetalleRelacionCategoria,
                    IdRelacionCategoria = x.IdRelacionCategoria,
                    CategoriaDesagracion = ObtenerCategoriaDesagracion(x.idCategoriaAtributo),
                    CategoriaAtributoValor = x.CategoriaAtributoValor,
                    Estado = x.Estado,
                    RelacionCategoria = db.RelacionCategoria.Where(i => i.idRelacionCategoria == x.IdRelacionCategoria).Single(),
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
                    ("execute spActualizarDetalleRelacionCategoria @idDetalleRelacionCategoria, @IdRelacionCategoria, @idCategoriaAtributo, @CategoriaAtributoValor, @Estado",
                      new SqlParameter("@idDetalleRelacionCategoria", objDetalle.idDetalleRelacionCategoria),
                      new SqlParameter("@IdRelacionCategoria", objDetalle.IdRelacionCategoria),
                      new SqlParameter("@idCategoriaAtributo", objDetalle.idCategoriaAtributo),
                      new SqlParameter("@CategoriaAtributoValor", objDetalle.CategoriaAtributoValor),
                      new SqlParameter("@Estado", objDetalle.Estado)
                    ).ToList();
            }

            return ListaDetalle;
        }


        private CategoriasDesagregacion ObtenerCategoriaDesagracion(int id)
        {
            return
            db.CategoriasDesagregacion.Where(x => x.idCategoria == id && x.idEstado == 2).FirstOrDefault();
        }

    }

    #endregion

}

