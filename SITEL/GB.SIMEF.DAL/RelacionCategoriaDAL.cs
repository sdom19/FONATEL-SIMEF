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
            ListaRelacionCategoria = CrearListadoRelacion(ListaRelacionCategoria);

            return ListaRelacionCategoria;
        }


        private List<RelacionCategoriaId> ObtenerCategoriaid(int idRelacion)
        {
            List<RelacionCategoriaId> lista = new List<RelacionCategoriaId>();

            using (db=new SIMEFContext())
            {
                lista= db.RelacionCategoriaId.Where(x => x.idRelacion == idRelacion).ToList();

            }
            lista = lista.Select(x => new RelacionCategoriaId()
            {
               idRelacion = x.idRelacion,
               idCategoriaId = x.idCategoriaId,
               listaCategoriaAtributo = ObtenerCategoriaAtributo( x.idRelacion, x.idCategoriaId).ToList()

            }).ToList();

            return lista;
            
        }


        private List<RelacionCategoriaAtributo> ObtenerCategoriaAtributo(int idRelacion, string IdCategoriaId)
        {
            List<RelacionCategoriaAtributo> lista = new List<RelacionCategoriaAtributo>();

            using (db = new SIMEFContext())
            {
                lista  = db.Database.SqlQuery<RelacionCategoriaAtributo>
              ("execute spObtenerRelacionCategoriaAtributo @idRelacion,@idCategoriaId ",
              new SqlParameter("@idRelacion", idRelacion),
              new SqlParameter("@idCategoriaId", IdCategoriaId)
              ).ToList();
                List<RelacionCategoriaAtributo> lista2 = new List<RelacionCategoriaAtributo>(lista);              
                foreach (var item in lista)
                {
                    foreach (var item2 in lista)
                    {
                        if (item2.IdcategoriaAtributo == item.IdcategoriaAtributo && item2.IdcategoriaAtributoDetalle != item.IdcategoriaAtributoDetalle)
                        {
                            if (item2.IdcategoriaAtributoDetalle == 0)
                            {
                                lista2.Remove(item2);
                            }
                            else
                            {
                                lista2.Remove(item);
                            }                            
                        }
                    }
                }
                if (lista2.Count() != 0)
                {
                    lista = lista2.Select(x => new RelacionCategoriaAtributo()
                    {
                        idRelacion = x.idRelacion,
                        IdcategoriaAtributo=x.IdcategoriaAtributo,
                        IdcategoriaAtributoDetalle=x.IdcategoriaAtributoDetalle,
                        IdCategoriaId=x.IdCategoriaId,
                        Etiqueta=db.DetalleCategoriaTexto
                            .Where(p=>p.idCategoria==x.IdcategoriaAtributo && p.idCategoriaDetalle==x.IdcategoriaAtributoDetalle).FirstOrDefault().Etiqueta
                    }).ToList();
                }
                
            }
            return lista;

        }




        private List<RelacionCategoria> CrearListadoRelacion(List<RelacionCategoria> ListaRelacionCategoria)
        {
            return ListaRelacionCategoria.Select(X => new RelacionCategoria
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
                RelacionCategoriaId = ObtenerCategoriaid(X.IdRelacionCategoria),
                EstadoRegistro = ObtenerEstadoRegistro(X.idEstado)
            }).ToList();
        }



        public List<RelacionCategoria> InsertarRelacionCategoriaId(RelacionCategoria objRelacionCategoria)
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
            ListaRelacionCategoria = CrearListadoRelacion(ListaRelacionCategoria);

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
            ListaRelacionCategoria = CrearListadoRelacion(ListaRelacionCategoria);

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

        public List<RelacionCategoriaAtributo> ObtenerRelacionCategoriaAtributoXIdCategoriaDetalle(int idCategoriaDetalle)
        {
            List<RelacionCategoriaAtributo> ListaRelacionCategoriaDetalle = new List<RelacionCategoriaAtributo>();
            using (db = new SIMEFContext())
            {
                ListaRelacionCategoriaDetalle = db.Database.SqlQuery<RelacionCategoriaAtributo>
                ("execute spObtenerRelacionCategoriaAtributoXIdCategoriaDetalle @IdCategoriaDetalle   ",
                     new SqlParameter("@IdCategoriaDetalle", idCategoriaDetalle)
                    ).ToList();
            }
            

            return ListaRelacionCategoriaDetalle;
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
            ListaRelacionCategoria = CrearListadoRelacion(ListaRelacionCategoria);
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




        #region Metodos relaión categoría id

        public List<RelacionCategoria> ActualizarRelacionCategoriaid(RelacionCategoriaId objeto)
        {
            List<RelacionCategoria> ListaRelacionCategoria = new List<RelacionCategoria>();
            using (db = new SIMEFContext())
            {
                ListaRelacionCategoria = db.Database.SqlQuery<RelacionCategoria>
                ("execute spActualizarRelacionCategoriaId @IdRelacion,@IdCategoriaId,@OpcionEliminar   ",
                     new SqlParameter("@IdRelacion", objeto.idRelacion),
                     new SqlParameter("@IdCategoriaId", objeto.idCategoriaId),
                      new SqlParameter("@OpcionEliminar", objeto.OpcionEliminar==true?1:0)
                    ).ToList();
            }
            ListaRelacionCategoria = CrearListadoRelacion(ListaRelacionCategoria);

            return ListaRelacionCategoria;
        }


       


        public List<RelacionCategoria> ActualizarRelacionAtributo(RelacionCategoriaAtributo objeto)
        {
            List<RelacionCategoria> ListaRelacionCategoria = new List<RelacionCategoria>();
            using (db = new SIMEFContext())
            {
                ListaRelacionCategoria = db.Database.SqlQuery<RelacionCategoria>
                ("execute spActualizarRelacionCategoriaAtributo  @idRelacion,@IdCategoriaId,@IdcategoriaAtributo,@IdcategoriaAtributoDetalle",
                     new SqlParameter("@idRelacion", objeto.idRelacion),
                     new SqlParameter("@IdCategoriaId", objeto.IdCategoriaId),
                      new SqlParameter("@IdcategoriaAtributo", objeto.IdcategoriaAtributo),
                     new SqlParameter("@IdcategoriaAtributoDetalle", objeto.IdcategoriaAtributoDetalle)

                    ).ToList();
            }
            ListaRelacionCategoria = CrearListadoRelacion(ListaRelacionCategoria);

            return ListaRelacionCategoria;
        }

        public List<RelacionCategoria> EliminarDetalleRelacionCategoria(RelacionCategoriaAtributo objeto)
        {
            List<RelacionCategoria> ListaRelacionCategoria = new List<RelacionCategoria>();
            using (db = new SIMEFContext())
            {
                ListaRelacionCategoria = db.Database.SqlQuery<RelacionCategoria>
                ("execute spEliminarRelacionCategoriaAtributoDetalle @IdRelacion,@IdCategoriaId   ",
                     new SqlParameter("@IdRelacion", objeto.idRelacion),
                     new SqlParameter("@IdCategoriaId", objeto.IdCategoriaId)
                    ).ToList();
            }
            return ListaRelacionCategoria;
        }

        #endregion
    }

}
