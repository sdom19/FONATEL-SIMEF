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
    public class SolicitudDAL: BitacoraDAL
    {
        private  SIMEFContext db;
        #region Metodos Consulta Base de Datos
        /// <summary>
        /// Metodo que carga los registros de Solicitudes según parametros
        /// fecha 03-08-2022
        /// Michael Hernandez
        /// </summary>
        /// <returns>Lista</returns>
        public List<Solicitud> ObtenerDatos(Solicitud objSolicitud)
        {
            List<Solicitud> ListaSolicitud = new List<Solicitud>();
            using (db = new SIMEFContext())
            {

                ListaSolicitud = db.Database.SqlQuery<Solicitud>
                    ("execute spObtenerSolicitudes @idSolicitud ,@codigo,@idEstado",
                     new SqlParameter("@idSolicitud", objSolicitud.idSolicitud),
                     new SqlParameter("@codigo", string.IsNullOrEmpty(objSolicitud.Codigo) ? DBNull.Value.ToString() : objSolicitud.Codigo),
                    new SqlParameter("@idEstado", objSolicitud.IdEstado)
                    ).ToList();

                ListaSolicitud = ListaSolicitud.Select(x => new Solicitud()
                {
                    id = Utilidades.Encriptar(x.idSolicitud.ToString()),
                    idSolicitud = x.idSolicitud,
                    Nombre = x.Nombre,
                    CantidadFormularios = x.CantidadFormularios,
                    Codigo = x.Codigo,
                    FechaInicio = x.FechaInicio,
                    FechaFin = x.FechaFin,
                    IdEstado = x.IdEstado,
                    Mensaje = x.Mensaje,
                    idAnno = x.idAnno,
                    idMes = x.idMes,
                    idFuente = x.idFuente,
                    FechaCreacion = x.FechaCreacion,
                    UsuarioCreacion = x.UsuarioCreacion,
                    FechaModificacion = x.FechaModificacion,
                    UsuarioModificacion = x.UsuarioModificacion,
                    Estado = db.EstadoRegistro.Where(i => i.idEstado == x.IdEstado).Single(),
                    Fuente = db.FuentesRegistro.Where(i => i.idFuente == x.idFuente).Single(),
                    EnvioProgramado = db.SolicitudEnvioProgramado.Where(i => i.IdSolicitud == x.idSolicitud).SingleOrDefault(),
                    SolicitudFormulario = db.DetalleSolicitudFormulario.Where(i => i.IdSolicitud == x.idSolicitud).ToList(),
                    FormulariosString= ObtenerListaFormularioString(x.idSolicitud)


                }).ToList();
            }
            return ListaSolicitud;
        }

        /// <summary>
        /// Actualiza los datos e inserta por medio de merge
        /// 17/08/2022
        /// michael Hernández C
        /// </summary>
        /// <param name="objSolicitud"></param>
        /// <returns></returns>
        public List<Solicitud> ActualizarDatos(Solicitud objSolicitud)
        {
            List<Solicitud> ListaSolicitud = new List<Solicitud>();
            using (db = new SIMEFContext())
            {
                ListaSolicitud = db.Database.SqlQuery<Solicitud>
                ("execute spActualizarSolicitud @idCategoria ,@Codigo,@NombreCategoria ,@CantidadDetalleDesagregacion ,@idTipoDetalle ,@IdTipoCategoria ,@UsuarioCreacion,@UsuarioModificacion,@idEstado "
                    
                    ).ToList();

            }
            return ListaSolicitud;
        }

        /// <summary>
        /// Obtener el listado de formularios en una fila string
        /// </summary>
        /// <param name="objSolicitud"></param>
        /// <returns></returns>
        public string ObtenerListaFormularioString(int objSolicitud)
        {
            string resultado = string.Empty;
            resultado=  db.Database.SqlQuery<string>
                  ("execute spObtenerFormularioXSolicitud @idSolicitud",
                  new SqlParameter("@idSolicitud", objSolicitud)
                    ).SingleOrDefault();
            if (string.IsNullOrEmpty(resultado))
            {
                resultado = "N/A";
            }
            return resultado;
        }
        #endregion



    }
}
