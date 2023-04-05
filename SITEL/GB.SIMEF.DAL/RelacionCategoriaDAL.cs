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

        private List<CategoriaDesagregacion> listaCategorias;

        public  RelacionCategoriaDAL()
        {
            categoriasDesagregacionDAL = new CategoriasDesagregacionDAL();
            listaCategorias = categoriasDesagregacionDAL.ObtenerTodosDatos(new CategoriaDesagregacion());
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
                ("execute pa_ObtenerRelacionCategoria @idRelacionCategoria,@codigo,@idCategoria,@idEstado",
                new SqlParameter("@idRelacionCategoria", objRelacionCategoria.IdRelacionCategoria),
                new SqlParameter("@codigo", string.IsNullOrEmpty(objRelacionCategoria.Codigo) ? DBNull.Value.ToString() : objRelacionCategoria.Codigo),
                new SqlParameter("@idCategoria", objRelacionCategoria.idCategoriaDesagregacion),
                new SqlParameter("@idEstado", objRelacionCategoria.IdEstadoRegistro)
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
                lista= db.RelacionCategoriaId.Where(x => x.idRelacionCategoriaId == idRelacion && x.idEstadoRegistro != (int)Constantes.EstadosRegistro.Eliminado).ToList();

            }
            lista = lista.Select(x => new RelacionCategoriaId()
            {
               idRelacionCategoriaId = x.idRelacionCategoriaId,
               idCategoriaDesagregacion = x.idCategoriaDesagregacion,
               idEstadoRegistro = x.idEstadoRegistro,
               listaCategoriaAtributo = ObtenerCategoriaAtributo( x.idRelacionCategoriaId, x.idCategoriaDesagregacion).ToList(),
               EstadoRegistro = ObtenerEstadoRegistro(x.idEstadoRegistro)
            }).ToList();

            return lista;
            
        }


        private List<RelacionCategoriaAtributo> ObtenerCategoriaAtributo(int idRelacion, string IdCategoriaId)
        {
            List<RelacionCategoriaAtributo> lista = new List<RelacionCategoriaAtributo>();

            using (db = new SIMEFContext())
            {
                lista = db.Database.SqlQuery<RelacionCategoriaAtributo>
              ("execute pa_ObtenerRelacionCategoriaAtributo @idRelacion,@idCategoriaId ",
              new SqlParameter("@idRelacion", idRelacion),
              new SqlParameter("@idCategoriaId", IdCategoriaId)
              ).ToList();

                lista = lista.Select(x => new RelacionCategoriaAtributo()
                {
                    idCategoriaDesagregacion = x.idCategoriaDesagregacion,
                    idRelacionCategoriaId = x.idRelacionCategoriaId,
                    idCategoriaDesagregacionAtributo = x.idCategoriaDesagregacionAtributo,
                    idDetalleCategoriaTextoAtributo = x.idDetalleCategoriaTextoAtributo,
                    Etiqueta = x.idDetalleCategoriaTextoAtributo==0?"N/A": db.DetalleCategoriaTexto.Where(i => i.idCategoriaDesagregacion == x.idCategoriaDesagregacionAtributo &&
                      i.idDetalleCategoriaTexto == x.idDetalleCategoriaTextoAtributo).Single().Etiqueta

                }).ToList();
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
                idCategoriaDesagregacion = X.idCategoriaDesagregacion,
                FechaCreacion = X.FechaCreacion,
                FechaModificacion = X.FechaModificacion,
                UsuarioCreacion = X.UsuarioCreacion,
                UsuarioModificacion = X.UsuarioModificacion,
                IdEstadoRegistro = X.IdEstadoRegistro,
                CantidadFila = X.CantidadFila,
                id = Utilidades.Encriptar(X.IdRelacionCategoria.ToString()),
                CategoriasDesagregacionid = ObtenerCategoria(X.idCategoriaDesagregacion),
                DetalleRelacionCategoria = ObtenerDatosDetalleRelacionCategoria(new DetalleRelacionCategoria() { idRelacionCategoria = X.IdRelacionCategoria }),
                RelacionCategoriaId = ObtenerCategoriaid(X.IdRelacionCategoria),
                EstadoRegistro = ObtenerEstadoRegistro(X.IdEstadoRegistro)
            }).ToList();
        }



        public List<RelacionCategoria> InsertarRelacionCategoriaId(RelacionCategoria objRelacionCategoria)
        {
            List<RelacionCategoria> ListaRelacionCategoria = new List<RelacionCategoria>();

            using (db = new SIMEFContext())
            {
                ListaRelacionCategoria = db.Database.SqlQuery<RelacionCategoria>
                ("execute pa_ObtenerRelacionCategoria @idRelacionCategoria,@codigo,@idCategoria,@idEstado",
                new SqlParameter("@idRelacionCategoria", objRelacionCategoria.IdRelacionCategoria),
                new SqlParameter("@codigo", string.IsNullOrEmpty(objRelacionCategoria.Codigo) ? DBNull.Value.ToString() : objRelacionCategoria.Codigo),
                new SqlParameter("@idCategoria", objRelacionCategoria.idCategoriaDesagregacion),
                new SqlParameter("@idEstado", objRelacionCategoria.IdEstadoRegistro)
                ).ToList();
            }
            ListaRelacionCategoria = CrearListadoRelacion(ListaRelacionCategoria);

            return ListaRelacionCategoria;
        }









        private EstadoRegistro ObtenerEstadoRegistro(int idEstado)
        {
            using (db = new SIMEFContext())
            {
                return db.EstadoRegistro.Where(p => p.IdEstadoRegistro == idEstado).FirstOrDefault();
            }
        }



        /// <summary>
        /// Autor: Francisco Vindas Ruiz
        /// Fecha: 19/08/2022
        /// Ejecutar procedimiento almacenado para actualizar datos de relacion categoria
        /// </summary>
        /// <param name="objCategoria"></param>
        /// <returns></returns>
        public List<RelacionCategoria> ActualizarDatos(RelacionCategoria objeto, Constantes.Accion accion = Constantes.Accion.Consultar)
        {
            List<RelacionCategoria> ListaRelacionCategoria = new List<RelacionCategoria>();
            using (db = new SIMEFContext())
            {
                ListaRelacionCategoria = db.Database.SqlQuery<RelacionCategoria>
                ("execute pa_ActualizarRelacionCategoria @idRelacionCategoria ,@Codigo ,@Nombre ,@CantidadCategoria ,@idCategoria  ,@UsuarioCreacion ,@UsuarioModificacion ,@IdEstadoRegistro ,@CantidadFila ",
                     new SqlParameter("@idRelacionCategoria", objeto.IdRelacionCategoria),
                     new SqlParameter("@Codigo", string.IsNullOrEmpty(objeto.Codigo) ? DBNull.Value.ToString() : objeto.Codigo),
                     new SqlParameter("@Nombre", string.IsNullOrEmpty(objeto.Nombre) ? DBNull.Value.ToString() : objeto.Nombre),
                     new SqlParameter("@CantidadCategoria", objeto.CantidadCategoria),
                     new SqlParameter("@idCategoria", objeto.idCategoriaDesagregacion),
                     new SqlParameter("@UsuarioCreacion", string.IsNullOrEmpty(objeto.UsuarioCreacion) ? DBNull.Value.ToString() : objeto.UsuarioCreacion),
                     new SqlParameter("@UsuarioModificacion", string.IsNullOrEmpty(objeto.UsuarioModificacion) ? DBNull.Value.ToString() : objeto.UsuarioModificacion),
                     new SqlParameter("@idEstadoRegistro", objeto.IdEstadoRegistro),
                     new SqlParameter("@CantidadFila", objeto.CantidadFila)
                    ).ToList();
            }
            if (accion != Constantes.Accion.Crear)
            {
                ListaRelacionCategoria = CrearListadoRelacion(ListaRelacionCategoria);

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

                    ("exec pa_ValidarRelacionCategoria @idRelacionCategoria",
                       new SqlParameter("@idRelacionCategoria", objeto.IdRelacionCategoria)
                    ).ToList();

            }

            return listaValicion;
        }

        /// <summary>
        /// Fecha 12/01/2023
        /// Jeaustin Chaves
        /// Se obtienen las relaciones categorias atributo por el id categoria detalle
        /// </summary>
        /// <param name="idCategoriaDetalle"></param>
        /// <returns></returns>
        public List<RelacionCategoriaAtributo> ObtenerRelacionCategoriaAtributoXIdCategoriaDetalle(int idCategoriaDetalle)
        {
            List<RelacionCategoriaAtributo> ListaRelacionCategoriaDetalle = new List<RelacionCategoriaAtributo>();
            using (db = new SIMEFContext())
            {
                ListaRelacionCategoriaDetalle = db.Database.SqlQuery<RelacionCategoriaAtributo>
                ("execute pa_ObtenerRelacionCategoriaAtributoXIdCategoriaDetalle @IdCategoriaDetalle   ",
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
                    ("execute pa_ActualizarDetalleRelacionCategoria @idDetalleRelacionCategoria, @IdRelacionCategoria, @idCategoriaAtributo, @Estado",
                      new SqlParameter("@idDetalleRelacionCategoria", objDetalle.idDetalleRelacionCategoria),
                      new SqlParameter("@IdRelacionCategoria", objDetalle.idRelacionCategoria),
                      new SqlParameter("@idCategoriaAtributo", objDetalle.idCategoriaDesagregacion),
                      new SqlParameter("@Estado", objDetalle.Estado)
                    ).ToList();
            }
            ListaRelacionCategoria = CrearListadoRelacion(ListaRelacionCategoria);
            return ListaRelacionCategoria;
            
        }

        private CategoriaDesagregacion ObtenerCategoria(int idCategoria )
        {
            if (idCategoria == 0)
            {
                return new CategoriaDesagregacion();
            }
            else
            {

                return listaCategorias.Where(p => p.idCategoriaDesagregacion == idCategoria).SingleOrDefault();
            }     
        }

        public List<DetalleRelacionCategoria> ObtenerDatosDetalleRelacionCategoria(DetalleRelacionCategoria objDetalle)
        {
            List<DetalleRelacionCategoria> ListaDetalle = new List<DetalleRelacionCategoria>();



            using (db = new SIMEFContext())
            {
                ListaDetalle = db.Database.SqlQuery<DetalleRelacionCategoria>
                    ("execute pa_ObtenerDetalleRelacionCategoria @idDetalleRelacionCategoria, @idRelacionCategoria, @idCategoriaAtributo ",
                      new SqlParameter("@idDetalleRelacionCategoria", objDetalle.idDetalleRelacionCategoria),
                      new SqlParameter("@idRelacionCategoria", objDetalle.idRelacionCategoria),
                      new SqlParameter("@idCategoriaAtributo", objDetalle.idCategoriaDesagregacion)
                    ).ToList();
            }

            ListaDetalle = ListaDetalle.Select(x => new DetalleRelacionCategoria()
            {
                idDetalleRelacionCategoria = x.idDetalleRelacionCategoria,
                Estado = x.Estado,
                idRelacionCategoria = x.idRelacionCategoria,
                idCategoriaDesagregacion = x.idCategoriaDesagregacion,
                CategoriaAtributo = ObtenerCategoria(x.idCategoriaDesagregacion),
                id = Utilidades.Encriptar(x.idDetalleRelacionCategoria.ToString()),
            }).ToList();
            return ListaDetalle;
        }


        #endregion




        #region Metodos relaión categoría id

        public List<RelacionCategoria> ActualizarRelacionCategoriaid(RelacionCategoriaId objeto, Constantes.Accion accion = Constantes.Accion.Consultar)
        {
            List<RelacionCategoria> ListaRelacionCategoria = new List<RelacionCategoria>();
            using (db = new SIMEFContext())
            {
                ListaRelacionCategoria = db.Database.SqlQuery<RelacionCategoria>
                ("execute pa_ActualizarRelacionCategoriaId @IdRelacion,@IdCategoriaId,@IdEstadoRegistro,@OpcionEliminar   ",
                     new SqlParameter("@IdRelacion", objeto.idRelacionCategoriaId),
                     new SqlParameter("@IdCategoriaId", objeto.idCategoriaDesagregacion),
                      new SqlParameter("@IdEstadoRegistro", objeto.idEstadoRegistro),
                      new SqlParameter("@OpcionEliminar", objeto.OpcionEliminar==true?1:0)
                    ).ToList();
            }
            if (accion != Constantes.Accion.Crear)
            {
                ListaRelacionCategoria = CrearListadoRelacion(ListaRelacionCategoria);

            }

            return ListaRelacionCategoria;
        }

        /// <summary>
        /// Fecha 17/01/2023
        /// Georgi Mesen Cerdas
        /// Se actualiza las relaciones categoria id
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public List<RelacionCategoria> ActualizarRelacionCategoriaidSinReturn(RelacionCategoriaId objeto)
        {
            List<RelacionCategoria> ListaRelacionCategoria = new List<RelacionCategoria>();
            using (db = new SIMEFContext())
            {
                ListaRelacionCategoria = db.Database.SqlQuery<RelacionCategoria>
                ("execute pa_ActualizarRelacionCategoriaId @IdRelacion,@IdCategoriaId,@idEstado,@OpcionEliminar   ",
                     new SqlParameter("@IdRelacion", objeto.idRelacionCategoriaId),
                     new SqlParameter("@IdCategoriaId", objeto.idCategoriaDesagregacion),
                      new SqlParameter("@idEstado", objeto.idEstadoRegistro),
                      new SqlParameter("@OpcionEliminar", objeto.OpcionEliminar == true ? 1 : 0)
                    ).ToList();
            }

            return ListaRelacionCategoria;
        }



        public List<RelacionCategoria> ActualizarRelacionAtributo(RelacionCategoriaAtributo objeto, Constantes.Accion accion = Constantes.Accion.Consultar)
        {
            List<RelacionCategoria> ListaRelacionCategoria = new List<RelacionCategoria>();
            using (db = new SIMEFContext())
            {
                ListaRelacionCategoria = db.Database.SqlQuery<RelacionCategoria>
                ("execute pa_ActualizarRelacionCategoriaAtributo  @idRelacion,@IdCategoriaId,@IdcategoriaAtributo,@IdcategoriaAtributoDetalle",
                     new SqlParameter("@idRelacion", objeto.idRelacionCategoriaId),
                     new SqlParameter("@IdCategoriaId", objeto.idCategoriaDesagregacion),
                      new SqlParameter("@IdcategoriaAtributo", objeto.idCategoriaDesagregacionAtributo),
                     new SqlParameter("@IdcategoriaAtributoDetalle", objeto.idDetalleCategoriaTextoAtributo)

                    ).ToList();
            }
            if (accion != Constantes.Accion.Crear)
            {
                ListaRelacionCategoria = CrearListadoRelacion(ListaRelacionCategoria);

            }

            return ListaRelacionCategoria;
        }

        /// <summary>
        /// Fecha 19/01/2023
        /// Georgi Mesen Cerdas
        /// Metodo para obtener eliminar un detalle relacion categoria
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public List<RelacionCategoria> EliminarDetalleRelacionCategoria(RelacionCategoriaAtributo objeto)
        {
            List<RelacionCategoria> ListaRelacionCategoria = new List<RelacionCategoria>();
            using (db = new SIMEFContext())
            {
                ListaRelacionCategoria = db.Database.SqlQuery<RelacionCategoria>
                ("execute pa_EliminarRelacionCategoriaAtributoDetalle @IdRelacion,@IdCategoriaId   ",
                     new SqlParameter("@IdRelacion", objeto.idRelacionCategoriaId),
                     new SqlParameter("@IdCategoriaId", objeto.idCategoriaDesagregacion)
                    ).ToList();
            }
            return ListaRelacionCategoria;
        }

        #endregion
    }

}
