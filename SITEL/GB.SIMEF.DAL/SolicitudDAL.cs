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
                    EnvioProgramado = db.SolicitudEnvioProgramado.Where(i => i.IdSolicitud == x.idSolicitud && i.Estado == true).SingleOrDefault(),
                    SolicitudFormulario = db.DetalleSolicitudFormulario.Where(i => i.IdSolicitud == x.idSolicitud).ToList(),
                    FormulariosString= ObtenerListaFormularioString(x.idSolicitud),
                    FormularioWeb= ObtenerListaFormulario(x.idSolicitud),

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
        public List<Solicitud> ActualizarDatos(Solicitud objeto)
        {
            List<Solicitud> ListaSolicitud = new List<Solicitud>();

            using (db = new SIMEFContext())
            {
                ListaSolicitud = db.Database.SqlQuery<Solicitud>

                ("execute spActualizarSolicitud @idSolicitud ,@Codigo, @Nombre ,@FechaInicio ,@FechaFin ,@idMes ,@idAnno ,@CantidadFormularios ,@idFuente ,@Mensaje ,@UsuarioCreacion ,@UsuarioModificacion ,@idEstado ",
                     new SqlParameter("@idSolicitud", objeto.idSolicitud),
                     new SqlParameter("@Codigo", string.IsNullOrEmpty(objeto.Codigo) ? DBNull.Value.ToString() : objeto.Codigo),
                     new SqlParameter("@Nombre", string.IsNullOrEmpty(objeto.Nombre) ? DBNull.Value.ToString() : objeto.Nombre),
                     objeto.FechaInicio == null ?
                        new SqlParameter("@FechaInicio", DBNull.Value)
                        :
                        new SqlParameter("@FechaInicio", objeto.FechaInicio),
                     objeto.FechaFin == null ?
                        new SqlParameter("@FechaFin", DBNull.Value)
                        :
                        new SqlParameter("@FechaFin", objeto.FechaFin),
                     new SqlParameter("@idMes", objeto.idMes),
                     new SqlParameter("@idAnno", objeto.idAnno),
                     new SqlParameter("@CantidadFormularios", objeto.CantidadFormularios),
                     new SqlParameter("@idFuente", objeto.idFuente),
                     new SqlParameter("@Mensaje", string.IsNullOrEmpty(objeto.Mensaje) ? DBNull.Value.ToString() : objeto.Mensaje),
                     new SqlParameter("@UsuarioCreacion", objeto.UsuarioCreacion),
                     new SqlParameter("@UsuarioModificacion", string.IsNullOrEmpty(objeto.UsuarioModificacion) ? DBNull.Value.ToString() : objeto.UsuarioModificacion),
                     new SqlParameter("@idEstado", objeto.IdEstado)
                    ).ToList();

                ListaSolicitud = ListaSolicitud.Select(x => new Solicitud()
                {
                    id = Utilidades.Encriptar(x.idSolicitud.ToString()),
                    idSolicitud = x.idSolicitud,
                    Codigo = x.Codigo,
                    Nombre = x.Nombre,
                    FechaInicio = x.FechaInicio,
                    FechaFin = x.FechaFin,
                    idMes = x.idMes,
                    idAnno = x.idAnno,
                    CantidadFormularios = x.CantidadFormularios,
                    idFuente = x.idFuente,
                    Mensaje = x.Mensaje,
                    FechaCreacion = x.FechaCreacion,
                    FechaModificacion = x.FechaModificacion,
                    UsuarioCreacion = x.UsuarioCreacion,
                    UsuarioModificacion = x.UsuarioModificacion,
                    Estado = db.EstadoRegistro.Where(i => i.idEstado == x.IdEstado).Single(),
                }).ToList();

            }

            return ListaSolicitud;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="objSolicitud"></param>
        /// <returns></returns>
        public List<FormularioWeb> ObtenerListaFormulario(int objSolicitud)
        {
            List<FormularioWeb> resultado = db.Database.SqlQuery<FormularioWeb>
                  ("execute spObtenerFormularioXSolicitudLista @idSolicitud",
                  new SqlParameter("@idSolicitud", objSolicitud)
                    ).ToList();

            if (resultado==null)
            {
                resultado = new List<FormularioWeb>();
            }
            return resultado;
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
                resultado = "No Definido";
            }
            return resultado;
        }

        /// <summary>
        /// Valida el existencia de la solicitud si tiene envío o programación automatica
        /// </summary>
        /// <param name="objSolicitud"></param>
        /// <returns></returns>
        public List<string> ValidarSolicitud(Solicitud objSolicitud)
        {
            List<string> resultado = new List<string>();
            using (db = new SIMEFContext())
            {
                resultado = db.Database.SqlQuery<string>("execute spValidarSolicitud @idSolicitud"
                , new SqlParameter("@idSolicitud", objSolicitud.idSolicitud)).ToList();
            }
            return resultado;
        }

        /// <summary>
        /// 14/10/2022
        /// Francisco Vindas Ruiz
        /// Función que clona los detalles de las solicitudes
        /// </summary>
        /// <param name="pIdSolicitudAClonar"></param>
        /// <param name="pIdSolicitudDestino"></param>
        /// <returns></returns>
        public bool ClonarDetallesDeSolicitudes(int pIdSolicitudAClonar, int pIdSolicitudDestino)
        {
            using (db = new SIMEFContext())
            {
                db.Database.SqlQuery<object>
                    ("execute spClonarDetallesDeSolicitudes @pIdSolicitudAClonar, @pIdSolicitudDestino",
                     new SqlParameter("@pIdSolicitudAClonar", pIdSolicitudAClonar),
                     new SqlParameter("@pIdSolicitudDestino", pIdSolicitudDestino)
                    ).ToList();
            }

            return true;
        }


        #endregion

    }
}
