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
    public class CategoriasDesagregacionDAL : BitacoraDAL
    {
        private SIMEFContext db;
        #region Metodos Consulta Base de Datos
        /// <summary>
        /// Metodo que carga los registros de categorias de desagregación según parametros
        /// fecha 03-08-2022
        /// Michael Hernandez
        /// </summary>
        /// <returns>Lista</returns>
        public List<CategoriaDesagregacion> ObtenerDatos(CategoriaDesagregacion objCategoria)
        {
            List<CategoriaDesagregacion> ListaCategoria = new List<CategoriaDesagregacion>();
            using (db = new SIMEFContext())
            {

                ListaCategoria = db.Database.SqlQuery<CategoriaDesagregacion>
                    ("execute pa_ObtenerCategoriaDesagregacion @idCategoria,@codigo,@idEstadoRegistro,@idTipoCategoria ",
                     new SqlParameter("@idCategoria", objCategoria.idCategoriaDesagregacion),
                     new SqlParameter("@codigo", string.IsNullOrEmpty(objCategoria.Codigo) ? DBNull.Value.ToString() : objCategoria.Codigo),
                     new SqlParameter("@idEstadoRegistro", objCategoria.idEstadoRegistro),
                     new SqlParameter("@idTipoCategoria", objCategoria.IdTipoCategoria)
                    ).Where(x=>x.idEstadoRegistro!=(int)Constantes.EstadosRegistro.Eliminado).ToList();

                ListaCategoria = ListaCategoria.Select(x => new CategoriaDesagregacion()
                {
                    id = Utilidades.Encriptar(x.idCategoriaDesagregacion.ToString()),
                    idCategoriaDesagregacion = x.idCategoriaDesagregacion,
                    Codigo = x.Codigo,
                    NombreCategoria = x.NombreCategoria,
                    idEstadoRegistro = x.idEstadoRegistro,
                    IdTipoCategoria = x.IdTipoCategoria,
                    CantidadDetalleDesagregacion = x.CantidadDetalleDesagregacion,
                    IdTipoDetalleCategoria = x.IdTipoDetalleCategoria,
                    FechaCreacion = x.FechaCreacion,
                    FechaModificacion = x.FechaModificacion,
                    UsuarioCreacion = x.UsuarioCreacion,
                    UsuarioModificacion = x.UsuarioModificacion,
                    DetalleCategoriaTexto = ListaDetalleCategoriaTexto(x.idCategoriaDesagregacion),
                    EstadoRegistro = db.EstadoRegistro.Where(i => i.IdEstadoRegistro == x.idEstadoRegistro).FirstOrDefault(),
                    TieneDetalle = ValidarTieneDetalle(x.IdTipoDetalleCategoria),
                    TipoCategoria = ObtenerTipoCategoria(x.IdTipoCategoria),
                    DetalleCategoriaFecha = ObtenerDetalleCategoriaFecha(x.idCategoriaDesagregacion),
                    DetalleCategoriaNumerico = ObtenerDetalleCategoriaNumerico(x.idCategoriaDesagregacion)

                }).ToList();
            }
            return ListaCategoria;
        }



        #region Metodos Consulta Base de Datos
        /// <summary>
        /// Metodo que carga los registros de categorias de desagregación según parametros hasta los eliminados
        /// fecha 03-08-2022
        /// Michael Hernandez
        /// </summary>
        /// <returns>Lista</returns>
        public List<CategoriaDesagregacion> ObtenerTodosDatos(CategoriaDesagregacion objCategoria)
        {
            List<CategoriaDesagregacion> ListaCategoria = new List<CategoriaDesagregacion>();
            using (db = new SIMEFContext())
            {

                ListaCategoria = db.Database.SqlQuery<CategoriaDesagregacion>
                    ("execute pa_ObtenerCategoriaDesagregacion @idCategoria,@codigo,@idEstadoRegistro,@idTipoCategoria ",
                     new SqlParameter("@idCategoria", objCategoria.idCategoriaDesagregacion),
                     new SqlParameter("@codigo", string.IsNullOrEmpty(objCategoria.Codigo) ? DBNull.Value.ToString() : objCategoria.Codigo),
                     new SqlParameter("@idEstadoRegistro", objCategoria.idEstadoRegistro),
                     new SqlParameter("@idTipoCategoria", objCategoria.IdTipoCategoria)
                    ).ToList();

                ListaCategoria = ListaCategoria.Select(x => new CategoriaDesagregacion()
                {
                    id = Utilidades.Encriptar(x.idCategoriaDesagregacion.ToString()),
                    idCategoriaDesagregacion = x.idCategoriaDesagregacion,
                    Codigo = x.Codigo,
                    NombreCategoria = x.NombreCategoria,
                    idEstadoRegistro = x.idEstadoRegistro,
                    IdTipoCategoria = x.IdTipoCategoria,
                    CantidadDetalleDesagregacion = x.CantidadDetalleDesagregacion,
                    IdTipoDetalleCategoria = x.IdTipoDetalleCategoria,
                    FechaCreacion = x.FechaCreacion,
                    FechaModificacion = x.FechaModificacion,
                    UsuarioCreacion = x.UsuarioCreacion,
                    UsuarioModificacion = x.UsuarioModificacion,
                    DetalleCategoriaTexto = ListaDetalleCategoriaTexto(x.idCategoriaDesagregacion),
                    EstadoRegistro = db.EstadoRegistro.Where(i => i.IdEstadoRegistro == x.idEstadoRegistro).FirstOrDefault(),
                    TieneDetalle = ValidarTieneDetalle(x.IdTipoDetalleCategoria),
                    TipoCategoria = ObtenerTipoCategoria(x.IdTipoCategoria),
                    DetalleCategoriaFecha = ObtenerDetalleCategoriaFecha(x.idCategoriaDesagregacion),
                    DetalleCategoriaNumerico = ObtenerDetalleCategoriaNumerico(x.idCategoriaDesagregacion)

                }).ToList();
            }
            return ListaCategoria;
        }
        /// <summary>
        /// Actualiza los datos e inserta por medio de merge
        /// 17/08/2022
        /// michael Hernández C
        /// </summary>
        /// <param name="objCategoria"></param>
        /// <returns></returns>
        public List<CategoriaDesagregacion> ActualizarDatos(CategoriaDesagregacion objCategoria)
        {
            List<CategoriaDesagregacion> ListaCategoria = new List<CategoriaDesagregacion>();
            using (db = new SIMEFContext())
            {
                ListaCategoria = db.Database.SqlQuery<CategoriaDesagregacion>
                ("execute pa_ActualizarCategoriaDesagregacion @idCategoria ,@Codigo,@NombreCategoria ,@CantidadDetalleDesagregacion ,@idTipoDetalle ,@IdTipoCategoria ,@UsuarioCreacion,@UsuarioModificacion,@idEstadoRegistro ",
                     new SqlParameter("@idCategoria", objCategoria.idCategoriaDesagregacion),
                     new SqlParameter("@codigo", string.IsNullOrEmpty(objCategoria.Codigo) ? DBNull.Value.ToString() : objCategoria.Codigo),
                     new SqlParameter("@NombreCategoria", string.IsNullOrEmpty(objCategoria.NombreCategoria) ? DBNull.Value.ToString() : objCategoria.NombreCategoria),
                     new SqlParameter("@CantidadDetalleDesagregacion", objCategoria.CantidadDetalleDesagregacion),
                     new SqlParameter("@idTipoDetalle", objCategoria.IdTipoDetalleCategoria),
                     new SqlParameter("@IdTipoCategoria", objCategoria.IdTipoCategoria),
                     new SqlParameter("@UsuarioCreacion", string.IsNullOrEmpty(objCategoria.UsuarioCreacion) ? DBNull.Value.ToString() : objCategoria.UsuarioCreacion),
                     new SqlParameter("@UsuarioModificacion", string.IsNullOrEmpty(objCategoria.UsuarioModificacion) ? DBNull.Value.ToString() : objCategoria.UsuarioModificacion),
                     new SqlParameter("@idEstadoRegistro", objCategoria.idEstadoRegistro)
                    ).ToList();

                ListaCategoria = ListaCategoria.Select(x => new CategoriaDesagregacion()
                {
                    idCategoriaDesagregacion = x.idCategoriaDesagregacion,
                    Codigo = x.Codigo,
                    NombreCategoria = x.NombreCategoria,
                    idEstadoRegistro = x.idEstadoRegistro,
                    IdTipoCategoria = x.IdTipoCategoria,
                    CantidadDetalleDesagregacion = x.CantidadDetalleDesagregacion,
                    IdTipoDetalleCategoria = x.IdTipoDetalleCategoria,
                    FechaCreacion = x.FechaCreacion,
                    FechaModificacion = x.FechaModificacion,
                    UsuarioCreacion = x.UsuarioCreacion,
                    UsuarioModificacion = x.UsuarioModificacion,
                    TipoCategoria = ObtenerTipoCategoria(x.IdTipoCategoria),
                    DetalleCategoriaTexto = db.DetalleCategoriaTexto.Where(i => i.idCategoriaDesagregacion == x.idCategoriaDesagregacion).ToList(),
                    EstadoRegistro = db.EstadoRegistro.Where(i => i.IdEstadoRegistro == x.idEstadoRegistro).Single(),
                    TieneDetalle = ValidarTieneDetalle(x.IdTipoDetalleCategoria)

                }).ToList();
            }
            return ListaCategoria;
        }

        /// <summary>
        /// José Navarro Acuña
        /// 20/10/2022
        /// Consulta las categorias de desagregación relacionadas con un indicador.
        /// Opcionalmente, se puede filtrar por el tipo detalle
        /// </summary>
        /// <param name="pIdIndicador"></param>
        /// <returns></returns>
        public List<CategoriaDesagregacion> ObtenerCategoriasDesagregacionDeIndicador(int pIdIndicador, int pIdTipoDetalleCategoria = 0)
        {
            List<CategoriaDesagregacion> listaCategorias = new List<CategoriaDesagregacion>();

            SqlParameter sqlParameter;

            if (pIdTipoDetalleCategoria == 0)
                sqlParameter = new SqlParameter("@pIdTipoDetalle", DBNull.Value);
            else
                sqlParameter = new SqlParameter("@pIdTipoDetalle", pIdTipoDetalleCategoria);

            using (db = new SIMEFContext())
            {
                listaCategorias = db.Database.SqlQuery<CategoriaDesagregacion>
                ("execute pa_ObtenerCategoriaDesagregacionDeIndicador @pIdIndicador, @pIdTipoDetalle",
                     new SqlParameter("@pIdIndicador", pIdIndicador.ToString()),
                     sqlParameter
                    ).ToList();

                listaCategorias = listaCategorias.Select(x => new CategoriaDesagregacion()
                {
                    id = Utilidades.Encriptar(x.idCategoriaDesagregacion.ToString()),
                    Codigo = x.Codigo,
                    NombreCategoria = x.NombreCategoria,
                    idEstadoRegistro = x.idEstadoRegistro,
                }).ToList();
            }
            return listaCategorias;
        }

        /// <summary>
        /// José Navarro Acuña
        /// 17/11/2022
        /// Consulta las categorias de desagregación relacionadas a una formula respecto al nivel de calculo        
        /// </summary>
        /// <param name="pIdFormula"></param>
        /// <param name="pIdIndicador"></param>
        /// <returns></returns>
        public List<CategoriaDesagregacion> ObtenerCategoriasDeFormulaNivelCalculo(int pIdFormula, int pIdIndicador)
        {
            List<CategoriaDesagregacion> listaCategorias = new List<CategoriaDesagregacion>();
            using (db = new SIMEFContext())
            {
                listaCategorias = db.Database.SqlQuery<CategoriaDesagregacion>
                ("execute pa_ObtenerCategoriaDeFormulaNivelCalculo @pIdFormula, @pIdIndicador ",
                     new SqlParameter("@pIdFormula", pIdFormula.ToString()),
                     new SqlParameter("@pIdIndicador", pIdIndicador.ToString())
                    ).ToList();

                listaCategorias = listaCategorias.Select(x => new CategoriaDesagregacion()
                {
                    id = Utilidades.Encriptar(x.idCategoriaDesagregacion.ToString()),
                    Codigo = x.Codigo,
                    NombreCategoria = x.NombreCategoria,
                    idEstadoRegistro = x.idEstadoRegistro,
                }).ToList();
            }
            return listaCategorias;
        }

        /// <summary>
        /// Valida si tiene detalle por el tipo de categoría 
        /// Fecha 03-08-2022
        /// Michael Hernández
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>

        private bool ValidarTieneDetalle(int i)
        {
            if (i == (int)Constantes.TipoDetalleCategoriaEnum.Texto || i == (int)Constantes.TipoDetalleCategoriaEnum.Alfanumerico)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// Michael Hernández C
        /// Actualiza e inserta
        /// 18/08/2022
        /// </summary>
        /// <param name="detalleFecha"></param>

        public void InsertarDetalleFecha(DetalleCategoriaFecha detalleFecha)
        {
            using (db = new SIMEFContext())
            {
                detalleFecha = db.Database.SqlQuery<DetalleCategoriaFecha>("exec pa_ActualizarCategoriaDesagregacionFecha  @idCategoria, @FechaMinima, @FechaMaxima, @Estado",
                     new SqlParameter("@idCategoria", detalleFecha.idCategoriaDesagregacion),
                     new SqlParameter("@FechaMinima", detalleFecha.FechaMinima),
                     new SqlParameter("@FechaMaxima", detalleFecha.FechaMaxima),
                     new SqlParameter("@Estado", detalleFecha.Estado)
                ).Single();
            }
        }
        /// <summary>
        /// Michael Hernández C
        /// Actualiza e inserta 
        /// </summary>
        /// <param name="detalleNumerico"></param>
        public void InsertarDetalleNumerico(DetalleCategoriaNumerico detalleNumerico)
        {
            using (db = new SIMEFContext())
            {
                detalleNumerico = db.Database.SqlQuery<DetalleCategoriaNumerico>("exec pa_ActualizarCategoriaDesagregacionNumerico  @idCategoria, @Minimo, @Maximo, @Estado",
                      new SqlParameter("@idCategoria", detalleNumerico.idCategoriaDesagregacion),
                      new SqlParameter("@Minimo", detalleNumerico.Minimo),
                      new SqlParameter("@Maximo", detalleNumerico.Maximo),
                      new SqlParameter("@Estado", detalleNumerico.Estado)
                 ).Single();
            }
        }

        /// <summary>
        /// Michael Hernández Cordero
        /// Valida dependencias con otras tablas
        /// 18/08/2022
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns></returns>

        public List<string> ValidarCategoria(CategoriaDesagregacion categoria)
        {
            List<string> listaValicion = new List<string>();
            using (db = new SIMEFContext())
            {
                listaValicion = db.Database.SqlQuery<string>
                    ("exec pa_ValidarCategoriaDesagregacion @IdCategoriaDesagregacion",
                       new SqlParameter("@IdCategoriaDesagregacion", categoria.idCategoriaDesagregacion)
                    ).ToList();
            }

            return listaValicion;
        }


        private DetalleCategoriaNumerico ObtenerDetalleCategoriaNumerico(int id)
        {
            return
            db.DetalleCategoriaNumerico
                             .Where(x => x.idCategoriaDesagregacion == id && x.Estado == true).FirstOrDefault();
        }


        private TipoCategoria ObtenerTipoCategoria(int id)
        {
            return
            db.TipoCategoria
                             .Where(x => x.idTipoCategoria == id).FirstOrDefault();
        }
        private DetalleCategoriaFecha ObtenerDetalleCategoriaFecha(int id)
        {
            return db.DetalleCategoriaFecha
                             .Where(x => x.idCategoriaDesagregacion == id && x.Estado == true).FirstOrDefault();
        }

        private List<DetalleCategoriaTexto> ListaDetalleCategoriaTexto(int id)
        {
            return db.DetalleCategoriaTexto
                             .Where(x => x.idCategoriaDesagregacion == id && x.Estado == true).ToList();
        }

        /// <summary>
        /// Georgi Mesen Cerdas
        /// Se obtiene las categorias para cargar la plantilla de excel de Relacion Categorias
        /// 18/08/2022
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns></returns>

        public List<DetalleCategoriaTexto> ObtenerCategoriasParaExcel(string NombreCategoria, string CategoriaTexto)
        {
            List<DetalleCategoriaTexto> listaValicion = new List<DetalleCategoriaTexto>();
            using (db = new SIMEFContext())
            {
                listaValicion = db.Database.SqlQuery<DetalleCategoriaTexto>
                    ("exec pa_ObtenerCategoriasParaExcel @NombreCategoria,@CategoriaTexto",
                       new SqlParameter("@NombreCategoria", NombreCategoria),
                       new SqlParameter("@CategoriaTexto", CategoriaTexto)
                    ).ToList();
            }

            return listaValicion;

        }
        #endregion
    }
}
