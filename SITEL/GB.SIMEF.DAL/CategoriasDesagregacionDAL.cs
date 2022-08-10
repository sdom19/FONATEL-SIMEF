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
    public class CategoriasDesagregacionDAL:BitacoraDAL
    {
        private  SIMEFContext db;
        #region Metodos Consulta Base de Datos
        /// <summary>
        /// Metodo que carga los registros de categorias de desagregación según parametros
        /// fecha 03-08-2022
        /// Michael Hernandez
        /// </summary>
        /// <returns>Lista</returns>
        public List<CategoriasDesagregacion> ObtenerDatos(CategoriasDesagregacion objCategoria)
        {
            List<CategoriasDesagregacion> ListaCategoria = new List<CategoriasDesagregacion>();
            using (db = new SIMEFContext())
            {

                ListaCategoria = db.Database.SqlQuery<CategoriasDesagregacion>
                    ("execute spObtenerCategoriasDesagregacion @idCategoria,@codigo,@idEstado,@idTipoCategoria ",
                     new SqlParameter("@idCategoria",objCategoria.idCategoria),
                     new SqlParameter("@codigo", string.IsNullOrEmpty(objCategoria.Codigo) ? DBNull.Value.ToString() : objCategoria.Codigo),
                     new SqlParameter("@idEstado", objCategoria.idEstado),
                     new SqlParameter("@idTipoCategoria", objCategoria.IdTipoCategoria)
                    ).ToList();

                ListaCategoria = ListaCategoria.Select(x => new CategoriasDesagregacion()
                {
                    id = Utilidades.Encriptar(x.idCategoria.ToString()),
                    idCategoria = x.idCategoria,
                    Codigo = x.Codigo,
                    NombreCategoria = x.NombreCategoria,
                    idEstado = x.idEstado,
                    IdTipoCategoria = x.IdTipoCategoria,
                    CantidadDetalleDesagregacion = x.CantidadDetalleDesagregacion,
                    idTipoDetalle = x.idTipoDetalle,
                    FechaCreacion = x.FechaCreacion,
                    FechaModificacion = x.FechaModificacion,
                    UsuarioCreacion = x.UsuarioCreacion,
                    UsuarioModificacion = x.UsuarioModificacion,
                    DetalleCategoriaTexto = ListaDetalleCategoriaTexto(x.idCategoria),
                    EstadoRegistro = db.EstadoRegistro.Where(i => i.idEstado == x.idEstado).Single(),
                    TieneDetalle = ValidarTieneDetalle(x.idTipoDetalle),
                    DetalleCategoriaFecha = ObtenerDetalleCategoriaFecha(x.idCategoria),
                    DetalleCategoriaNumerico = ObtenerDetalleCategoriaNumerico(x.idCategoria)

                }).ToList();
            }
            return ListaCategoria;
        }


        public List<CategoriasDesagregacion> ActualizarDatos(CategoriasDesagregacion objCategoria)
        {
            List<CategoriasDesagregacion> ListaCategoria = new List<CategoriasDesagregacion>();
            using (db = new SIMEFContext())
            {
                ListaCategoria = db.Database.SqlQuery<CategoriasDesagregacion>
                ("execute spActualizarCategoriasDesagregacion @idCategoria ,@Codigo,@NombreCategoria ,@CantidadDetalleDesagregacion ,@idTipoDetalle ,@IdTipoCategoria ,@UsuarioCreacion,@UsuarioModificacion,@idEstado ",
                     new SqlParameter("@idCategoria", objCategoria.idCategoria),
                     new SqlParameter("@codigo", string.IsNullOrEmpty(objCategoria.Codigo) ? DBNull.Value.ToString() : objCategoria.Codigo),
                     new SqlParameter("@NombreCategoria", string.IsNullOrEmpty(objCategoria.NombreCategoria) ? DBNull.Value.ToString() : objCategoria.NombreCategoria),
                     new SqlParameter("@CantidadDetalleDesagregacion", objCategoria.CantidadDetalleDesagregacion),
                     new SqlParameter("@idTipoDetalle", objCategoria.idTipoDetalle),
                     new SqlParameter("@IdTipoCategoria", objCategoria.IdTipoCategoria),
                     new SqlParameter("@UsuarioCreacion", objCategoria.UsuarioCreacion),
                     new SqlParameter("@UsuarioModificacion", objCategoria.UsuarioModificacion),
                     new SqlParameter("@idEstado", objCategoria.idEstado)
                    ).ToList();

                ListaCategoria = ListaCategoria.Select(x => new CategoriasDesagregacion()
                {
                    idCategoria = x.idCategoria,
                    Codigo = x.Codigo,
                    NombreCategoria = x.NombreCategoria,
                    idEstado = x.idEstado,
                    IdTipoCategoria = x.IdTipoCategoria,
                    CantidadDetalleDesagregacion = x.CantidadDetalleDesagregacion,
                    idTipoDetalle = x.idTipoDetalle,
                    FechaCreacion = x.FechaCreacion,
                    FechaModificacion = x.FechaModificacion,
                    UsuarioCreacion = x.UsuarioCreacion,
                    UsuarioModificacion = x.UsuarioModificacion,
                    DetalleCategoriaTexto = db.DetalleCategoriaTexto.Where(i => i.idCategoria == x.idCategoria).ToList(),
                    EstadoRegistro = db.EstadoRegistro.Where(i => i.idEstado == x.idEstado).Single(),
                    TieneDetalle = ValidarTieneDetalle(x.idTipoDetalle)

                }).ToList();
            }
            return ListaCategoria;
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

        private DetalleCategoriaNumerico ObtenerDetalleCategoriaNumerico(int id)
        {
            return 
            db.DetalleCategoriaNumerico
                             .Where(x => x.idCategoria == id && x.Estado == true).FirstOrDefault();
        }
        private DetalleCategoriaFecha ObtenerDetalleCategoriaFecha(int id)
        {
            return db.DetalleCategoriaFecha
                             .Where(x => x.idCategoria == id && x.Estado == true).FirstOrDefault();
        }

        private List<DetalleCategoriaTexto> ListaDetalleCategoriaTexto(int id)
        {
            return db.DetalleCategoriaTexto
                             .Where(x => x.idCategoria == id && x.Estado == true).ToList();
        }
    }
}
