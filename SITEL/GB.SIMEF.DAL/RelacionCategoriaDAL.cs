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
    public class RelacionCategoriaDAL : BitacoraDAL
    {
        private SIMEFContext db;

        #region Metodos de Consulta a Base Datos
        /// <summary>
        /// Autor: Francisco Vindas Ruiz
        /// Fecha: 10/08/2022
        /// Ejecutar procedimiento almacenado para obtener datos de relacion categoria
        /// </summary>
        /// <param name="objRelacionCategoria"></param>
        /// <returns></returns>
        public List<RelacionCategoria> ObtenerDatos(RelacionCategoria objRelacionCategoria)
        {
            List<RelacionCategoria> ListaRelacionCategoria = new List<RelacionCategoria>();

            using (db = new SIMEFContext())
            {
                ListaRelacionCategoria = db.Database.SqlQuery<RelacionCategoria>
                ("execute spObtenerRelacionCategorias @idRelacionCategoria,@codigo,@idCategoria,@idEstado",
                new SqlParameter("@idRelacionCategoria", objRelacionCategoria.idRelacionCategoria),
                new SqlParameter("@codigo", string.IsNullOrEmpty(objRelacionCategoria.Codigo) ? DBNull.Value.ToString() : objRelacionCategoria.Codigo),
                new SqlParameter("@idCategoria", objRelacionCategoria.idCategoria),
                new SqlParameter("@idEstado", objRelacionCategoria.idEstado)
                ).ToList();


                ListaRelacionCategoria = ListaRelacionCategoria.Select(X => new RelacionCategoria
                {
                    idRelacionCategoria = X.idRelacionCategoria,
                    Codigo = X.Codigo,
                    Nombre = X.Nombre,
                    CantidadCategoria = X.CantidadCategoria,
                    idCategoria = X.idCategoria,
                    idCategoriaValor = X.idCategoriaValor,
                    FechaCreacion = X.FechaCreacion,
                    FechaModificacion = X.FechaModificacion,
                    UsuarioCreacion = X.UsuarioCreacion,
                    UsuarioModificacion = X.UsuarioModificacion,
                    idEstado = X.idEstado,
                    id = Utilidades.Encriptar(X.idRelacionCategoria.ToString()),
                    DetalleRelacionCategoria = db.DetalleRelacionCategoria.Where(p => p.IdRelacionCategoria == X.idRelacionCategoria & p.Estado == true).ToList(),
                    EstadoRegistro = db.EstadoRegistro.Where(p => p.idEstado == X.idEstado).FirstOrDefault()
                }).ToList();

                return ListaRelacionCategoria;
            }

        }

        /// <summary>
        /// Autor: Francisco Vindas Ruiz
        /// Fecha: 19/08/2022
        /// Ejecutar procedimiento almacenado para actualizar datos de relacion categoria
        /// </summary>
        /// <param name="objCategoria"></param>
        /// <returns></returns>
        public List<RelacionCategoria> ActualizarDatos(RelacionCategoria objeto)
        {
            List<RelacionCategoria> ListaRelacionCategoria = new List<RelacionCategoria>();
            using (db = new SIMEFContext())
            {
                ListaRelacionCategoria = db.Database.SqlQuery<RelacionCategoria>
                ("execute spActualizarRelacionCategoria @idRelacionCategoria ,@Codigo ,@Nombre ,@CantidadCategoria ,@idCategoria  ,@idCategoriaValor ,@UsuarioCreacion ,@UsuarioModificacion ,@idEstado ",
                     new SqlParameter("@idRelacionCategoria", objeto.idRelacionCategoria),
                     new SqlParameter("@Codigo", string.IsNullOrEmpty(objeto.Codigo) ? DBNull.Value.ToString() : objeto.Codigo),
                     new SqlParameter("@Nombre", string.IsNullOrEmpty(objeto.Nombre) ? DBNull.Value.ToString() : objeto.Nombre),
                     new SqlParameter("@CantidadCategoria", objeto.CantidadCategoria),
                     new SqlParameter("@idCategoria", objeto.idCategoria),
                     new SqlParameter("@idCategoriaValor", objeto.idCategoriaValor),
                     new SqlParameter("@UsuarioCreacion", objeto.UsuarioCreacion),
                     new SqlParameter("@UsuarioModificacion", string.IsNullOrEmpty(objeto.UsuarioModificacion) ? DBNull.Value.ToString() : objeto.UsuarioModificacion),
                     new SqlParameter("@idEstado", objeto.idEstado)
                    ).ToList();

                ListaRelacionCategoria = ListaRelacionCategoria.Select(x => new RelacionCategoria()
                {
                    idRelacionCategoria = x.idRelacionCategoria,
                    Codigo = x.Codigo,
                    Nombre = x.Nombre,
                    idEstado = x.idEstado,
                    CantidadCategoria = x.CantidadCategoria,
                    idCategoria = x.idCategoria,
                    idCategoriaValor = x.idCategoriaValor,
                    FechaCreacion = x.FechaCreacion,
                    FechaModificacion = x.FechaModificacion,
                    UsuarioCreacion = x.UsuarioCreacion,
                    UsuarioModificacion = x.UsuarioModificacion,
                    EstadoRegistro = db.EstadoRegistro.Where(i => i.idEstado == x.idEstado).Single(),
                }).ToList();
            }
            return ListaRelacionCategoria;
        }

        /// <summary>
        /// Fecha 16/09/2022
        /// Francisco Vindas Ruiz
        /// Validar existencia en Indicadores
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns></returns>
        public List<string> ValidarRelacion(RelacionCategoria objeto)
        {
            List<string> listaValicion = new List<string>();

            using (db = new SIMEFContext())
            {

                listaValicion = db.Database.SqlQuery<string>

                    ("exec spValidarRelacionCategoria @idRelacionCategoria",
                       new SqlParameter("@idRelacionCategoria", objeto.idRelacionCategoria)
                    ).ToList();

            }

            return listaValicion;
        }

        #endregion

    }
}
