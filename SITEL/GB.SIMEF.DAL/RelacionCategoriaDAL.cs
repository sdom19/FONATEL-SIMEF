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

        private CategoriasDesagregacionDAL categoriasDesagregacionDAL;
        
        private List<DetalleRelacionCategoria> listaDetalleRelacionCategoria;

        private List<CategoriasDesagregacion> listaCategorias;

        public  RelacionCategoriaDAL()
        {
            categoriasDesagregacionDAL = new CategoriasDesagregacionDAL();
            listaCategorias = categoriasDesagregacionDAL.ObtenerDatos(new CategoriasDesagregacion());
        }

        #region Metodos de Consulta a Base Datos Relacion Categoria
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
                new SqlParameter("@idRelacionCategoria", objRelacionCategoria.IdRelacionCategoria),
                new SqlParameter("@codigo", string.IsNullOrEmpty(objRelacionCategoria.Codigo) ? DBNull.Value.ToString() : objRelacionCategoria.Codigo),
                new SqlParameter("@idCategoria", objRelacionCategoria.idCategoria),
                new SqlParameter("@idEstado", objRelacionCategoria.idEstado)
                ).ToList();
            }
            ListaRelacionCategoria = ListaRelacionCategoria.Select(X => new RelacionCategoria
            {
                IdRelacionCategoria = X.IdRelacionCategoria,
                Codigo = X.Codigo,
                Nombre = X.Nombre,
                CantidadCategoria = X.CantidadCategoria,
                idCategoria = X.idCategoria,
                FechaCreacion = X.FechaCreacion,
                FechaModificacion = X.FechaModificacion,
                UsuarioCreacion = X.UsuarioCreacion,
                UsuarioModificacion = X.UsuarioModificacion,
                idEstado = X.idEstado,
                CantidadFilas = X.CantidadFilas,
                id = Utilidades.Encriptar(X.IdRelacionCategoria.ToString()),
                CategoriasDesagregacionid = ObtenerCategoria(X.idCategoria),
                DetalleRelacionCategoria = ObtenerDatosDetalleRelacionCategoria(new DetalleRelacionCategoria() { IdRelacionCategoria = X.IdRelacionCategoria }),
                //RelacionCategoriaId = db.RelacionCategoriaId.Where(p => p.idRelacion == X.IdRelacionCategoria).FirstOrDefault(),
                EstadoRegistro = ObtenerEstadoRegistro(X.idEstado)
            }).ToList();

            return ListaRelacionCategoria;
        }


        private EstadoRegistro ObtenerEstadoRegistro(int idEstado)
        {
            using (db = new SIMEFContext())
            {
                return db.EstadoRegistro.Where(p => p.idEstado == idEstado).FirstOrDefault();
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
                ("execute spActualizarRelacionCategoria @idRelacionCategoria ,@Codigo ,@Nombre ,@CantidadCategoria ,@idCategoria  ,@UsuarioCreacion ,@UsuarioModificacion ,@idEstado ,@CantidadFilas ",
                     new SqlParameter("@idRelacionCategoria", objeto.IdRelacionCategoria),
                     new SqlParameter("@Codigo", string.IsNullOrEmpty(objeto.Codigo) ? DBNull.Value.ToString() : objeto.Codigo),
                     new SqlParameter("@Nombre", string.IsNullOrEmpty(objeto.Nombre) ? DBNull.Value.ToString() : objeto.Nombre),
                     new SqlParameter("@CantidadCategoria", objeto.CantidadCategoria),
                     new SqlParameter("@idCategoria", objeto.idCategoria),
                     new SqlParameter("@UsuarioCreacion", string.IsNullOrEmpty(objeto.UsuarioCreacion) ? DBNull.Value.ToString() : objeto.UsuarioCreacion),
                     new SqlParameter("@UsuarioModificacion", string.IsNullOrEmpty(objeto.UsuarioModificacion) ? DBNull.Value.ToString() : objeto.UsuarioModificacion),
                     new SqlParameter("@idEstado", objeto.idEstado),
                     new SqlParameter("@CantidadFilas", objeto.CantidadFilas)
                    ).ToList();
            }
            ListaRelacionCategoria = ListaRelacionCategoria.Select(X => new RelacionCategoria
            {
                IdRelacionCategoria = X.IdRelacionCategoria,
                Codigo = X.Codigo,
                Nombre = X.Nombre,
                CantidadCategoria = X.CantidadCategoria,
                idCategoria = X.idCategoria,
                FechaCreacion = X.FechaCreacion,
                FechaModificacion = X.FechaModificacion,
                UsuarioCreacion = X.UsuarioCreacion,
                UsuarioModificacion = X.UsuarioModificacion,
                idEstado = X.idEstado,
                CantidadFilas = X.CantidadFilas,
                id = Utilidades.Encriptar(X.IdRelacionCategoria.ToString()),
                DetalleRelacionCategoria = ObtenerDatosDetalleRelacionCategoria(new DetalleRelacionCategoria() { IdRelacionCategoria = X.IdRelacionCategoria }),
                CategoriasDesagregacionid = ObtenerCategoria(X.idCategoria),
                //RelacionCategoriaId = db.RelacionCategoriaId.Where(p => p.idRelacion == X.IdRelacionCategoria).FirstOrDefault(),
                EstadoRegistro = ObtenerEstadoRegistro(X.idEstado)
            }).ToList();

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
                       new SqlParameter("@idRelacionCategoria", objeto.IdRelacionCategoria)
                    ).ToList();

            }

            return listaValicion;
        }



        #endregion




        #region Metodos de Consulta a Base Datos Relacion Categoria Detalle
        public List<RelacionCategoria> ActualizarDatosDetalle(DetalleRelacionCategoria objDetalle)
        {
            List<RelacionCategoria> ListaRelacionCategoria = new List<RelacionCategoria>();
            using (db = new SIMEFContext())
            {
                ListaRelacionCategoria = db.Database.SqlQuery<RelacionCategoria>
                    ("execute spActualizarDetalleRelacionCategoria @idDetalleRelacionCategoria, @IdRelacionCategoria, @idCategoriaAtributo, @Estado",
                      new SqlParameter("@idDetalleRelacionCategoria", objDetalle.idDetalleRelacionCategoria),
                      new SqlParameter("@IdRelacionCategoria", objDetalle.IdRelacionCategoria),
                      new SqlParameter("@idCategoriaAtributo", objDetalle.idCategoriaAtributo),
                      new SqlParameter("@Estado", objDetalle.Estado)
                    ).ToList();
            }
            ListaRelacionCategoria = ListaRelacionCategoria.Select(X => new RelacionCategoria
            {
                IdRelacionCategoria = X.IdRelacionCategoria,
                Codigo = X.Codigo,
                Nombre = X.Nombre,
                CantidadCategoria = X.CantidadCategoria,
                idCategoria = X.idCategoria,
                FechaCreacion = X.FechaCreacion,
                FechaModificacion = X.FechaModificacion,
                UsuarioCreacion = X.UsuarioCreacion,
                UsuarioModificacion = X.UsuarioModificacion,
                idEstado = X.idEstado,
                CantidadFilas = X.CantidadFilas,
                id = Utilidades.Encriptar(X.IdRelacionCategoria.ToString()),
                DetalleRelacionCategoria = ObtenerDatosDetalleRelacionCategoria(new DetalleRelacionCategoria() { IdRelacionCategoria = X.IdRelacionCategoria }),
                CategoriasDesagregacionid = ObtenerCategoria(X.idCategoria),
                    //RelacionCategoriaId = db.RelacionCategoriaId.Where(p => p.idRelacion == X.IdRelacionCategoria).FirstOrDefault(),
                    EstadoRegistro = ObtenerEstadoRegistro(X.idEstado)
            }).ToList();
                return ListaRelacionCategoria;
            
        }

        private CategoriasDesagregacion ObtenerCategoria(int idCategoria )
        {
            if (idCategoria == 0)
            {
                return new CategoriasDesagregacion();
            }
            else
            {

                return listaCategorias.Where(p => p.idCategoria == idCategoria).SingleOrDefault();
            }
           
          
        }

        public List<DetalleRelacionCategoria> ObtenerDatosDetalleRelacionCategoria(DetalleRelacionCategoria objDetalle)
        {
            List<DetalleRelacionCategoria> ListaDetalle = new List<DetalleRelacionCategoria>();



            using (db = new SIMEFContext())
            {
                ListaDetalle = db.Database.SqlQuery<DetalleRelacionCategoria>
                    ("execute spObtenerDetalleRelacionCategoria @idDetalleRelacionCategoria, @idRelacionCategoria, @idCategoriaAtributo ",
                      new SqlParameter("@idDetalleRelacionCategoria", objDetalle.idDetalleRelacionCategoria),
                      new SqlParameter("@idRelacionCategoria", objDetalle.IdRelacionCategoria),
                      new SqlParameter("@idCategoriaAtributo", objDetalle.idCategoriaAtributo)
                    ).ToList();
            }

            ListaDetalle = ListaDetalle.Select(x => new DetalleRelacionCategoria()
            {
                idDetalleRelacionCategoria = x.idDetalleRelacionCategoria,
                Estado = x.Estado,
                IdRelacionCategoria = x.IdRelacionCategoria,
                idCategoriaAtributo = x.idCategoriaAtributo,
                CategoriaAtributo = ObtenerCategoria(x.idCategoriaAtributo),
                id = Utilidades.Encriptar(x.idDetalleRelacionCategoria.ToString()),
            }).ToList();
            return ListaDetalle;
        }


        #endregion
    }

}
