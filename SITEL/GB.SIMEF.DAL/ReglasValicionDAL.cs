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
    public class ReglasValicionDAL : BitacoraDAL
    {
        private SIMEFContext db;

        #region Metodos Consulta Base de Datos
        /// <summary>
        /// Metodo que carga los registros de categorias de desagregación tipo texto
        /// fecha 03-08-2022
        /// Michael Hernandez
        /// </summary>
        /// <returns>Lista</returns>
        /// 

        public List<ReglaValidacion> ObtenerDatos(ReglaValidacion objReglas)
        {
            List<ReglaValidacion> ListaCategoriaDetalle = new List<ReglaValidacion>();
            using (db = new SIMEFContext())
            {
                ListaCategoriaDetalle = db.Database.SqlQuery<ReglaValidacion>
                    ("execute pa_ObtenerReglaValidacion @idRegla,@Codigo,@idIndicador",
                      new SqlParameter("@idRegla", objReglas.idReglaValidacion),
                      new SqlParameter("@Codigo", string.IsNullOrEmpty(objReglas.Codigo) ? DBNull.Value.ToString() : objReglas.Codigo),
                      new SqlParameter("@idIndicador", objReglas.idIndicador)
                    ).ToList();

                ListaCategoriaDetalle = ListaCategoriaDetalle.Select(x => new ReglaValidacion()
                {
                    id = Utilidades.Encriptar(x.idReglaValidacion.ToString()),
                    idIndicadorString = Utilidades.Encriptar(x.idIndicador.ToString()),
                    idReglaValidacion = x.idReglaValidacion,
                    Codigo = x.Codigo,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    idIndicador = x.idIndicador,
                    FechaCreacion = x.FechaCreacion,
                    FechaModificacion = x.FechaModificacion,
                    idEstadoRegistro = x.idEstadoRegistro,
                    UsuarioCreacion = x.UsuarioCreacion,
                    UsuarioModificacion = x.UsuarioModificacion,
                    DetalleReglaValidacion = new List<DetalleReglaValidacion>(),
                    EstadoRegistro = db.EstadoRegistro.Where(i => i.IdEstadoRegistro == x.idEstadoRegistro).Single(),
                    ListadoTipoReglas = ObtenerListadoTipoReglas(x.idReglaValidacion)
                }).ToList();
            }

            return ListaCategoriaDetalle;
        }
        /// <summary>
        /// Valida si existen Reglas de Validación en indicadores
        /// Michael Hernández Cordero
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public List<string> ValidarDatos(ReglaValidacion objeto)
        {
            List<string> listaValicion = new List<string>();
            using (db = new SIMEFContext())
            {
                listaValicion = db.Database.SqlQuery<string>
                    ("exec pa_ValidarRegla @idRegla",
                       new SqlParameter("@idRegla", objeto.idReglaValidacion)
                    ).ToList();
            }

            return listaValicion;
        }


        private string ObtenerListadoTipoReglas(int idRegla)
        {
            string tipoReglas= "";
                tipoReglas = db.Database.SqlQuery<string>
                    ("exec pa_ObtenerListadoTipoReglaXReglaValidacion @idRegla",
                       new SqlParameter("@idRegla", idRegla)
                    ).Single();

            return tipoReglas;
        }


        /// <summary>
        /// Creación y modificación
        /// </summary>
        /// <param name="objReglaValidacion"></param>
        /// <returns></returns>
        public List<ReglaValidacion> ActualizarDatos(ReglaValidacion objReglaValidacion)
        {
            List<ReglaValidacion> ListaReglaValidacion = new List<ReglaValidacion>();

            using (db = new SIMEFContext())
            {
                ListaReglaValidacion = db.Database.SqlQuery<ReglaValidacion>
                ("execute pa_ActualizarReglaValidacion @IdRegla,@Codigo,@Nombre,@Descripcion,@IdIndicador,@UsuarioCreacion,@UsuarioModificacion,@IdEstado",
                    new SqlParameter("@IdRegla", objReglaValidacion.idReglaValidacion),
                    new SqlParameter("@Codigo", objReglaValidacion.Codigo),
                    new SqlParameter("@Nombre", objReglaValidacion.Nombre),
                    new SqlParameter("@Descripcion", string.IsNullOrEmpty(objReglaValidacion.Descripcion) ? DBNull.Value.ToString() : objReglaValidacion.Descripcion),
                    new SqlParameter("@IdIndicador", objReglaValidacion.idIndicador),
                    new SqlParameter("@UsuarioCreacion", string.IsNullOrEmpty(objReglaValidacion.UsuarioCreacion) ? DBNull.Value.ToString() : objReglaValidacion.UsuarioCreacion),
                    new SqlParameter("@UsuarioModificacion", string.IsNullOrEmpty(objReglaValidacion.UsuarioModificacion) ? DBNull.Value.ToString() : objReglaValidacion.UsuarioModificacion),
                    new SqlParameter("@IdEstado", objReglaValidacion.idEstadoRegistro)
                ).ToList();


                ListaReglaValidacion = ListaReglaValidacion.Select(X => new ReglaValidacion
                {
                    id = Utilidades.Encriptar(X.idReglaValidacion.ToString()),
                    idReglaValidacion = X.idReglaValidacion,
                    Codigo = X.Codigo,
                    Nombre = X.Nombre,
                    Descripcion = X.Descripcion,
                    idIndicador = X.idIndicador,
                    UsuarioCreacion = X.UsuarioCreacion,
                    UsuarioModificacion = X.UsuarioModificacion,
                    EstadoRegistro = db.EstadoRegistro.Where(i => i.IdEstadoRegistro == X.idEstadoRegistro).Single()

                }).ToList();

                return ListaReglaValidacion;
            }
        }

        /// <summary>
        /// 07/11/2022
        /// Francisco Vindas Ruiz
        /// Función que clona los detalles de las reglas
        /// </summary>
        /// <param name="pIdReglaAClonar"></param>
        /// <param name="pIdReglaDestino"></param>
        /// <returns></returns>
        public bool ClonarDetallesReglas(int pIdReglaAClonar, int pIdReglaDestino)
        {
            using (db = new SIMEFContext())
            {
                db.Database.SqlQuery<object>
                    ("execute PA_ClonarDetalleRegla @pIdReglaAClonar, @pIdReglaDestino",
                     new SqlParameter("@pIdReglaAClonar", pIdReglaAClonar),
                     new SqlParameter("@pIdReglaDestino", pIdReglaDestino)
                    ).ToList();
            }

            return true;
        }

        #endregion
    }
}
