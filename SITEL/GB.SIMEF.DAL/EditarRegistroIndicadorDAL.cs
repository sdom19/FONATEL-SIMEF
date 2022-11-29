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
    public class EditarRegistroIndicadorFonatelDAL: BitacoraDAL
    {
        private SIMEFContext db;
        private DetalleRegistroIndicadorFonatelDAL DetalleRegistroIndicadorFonatelDAL;

        public EditarRegistroIndicadorFonatelDAL()
        {
            DetalleRegistroIndicadorFonatelDAL = new DetalleRegistroIndicadorFonatelDAL();
           
        }

        /// <summary>
        /// Autor: Francisco Vindas
        /// Fecha: 25/11/2022
        /// El metodo crea una lista generica de la solicitud que puede ser utilizado en lo metodos que lo necesiten 
        /// </summary>
        /// <param name="ListaSolicitud"></param>
        /// <returns></returns>

        private List<RegistroIndicadorFonatel> CrearListado(List<RegistroIndicadorFonatel> ListaSolicitud)
        {
            return ListaSolicitud.Select(x => new RegistroIndicadorFonatel
            {
                Codigo = x.Codigo,
                IdSolicitud = x.IdSolicitud,
                Mensaje = x.Mensaje,
                Nombre = x.Nombre,
                FechaInicio = x.FechaInicio,
                FechaFin = x.FechaFin,
                Formulario = x.Formulario,
                IdFormulario = x.IdFormulario,
                IdEstado = x.IdEstado,
                FechaModificacion = x.FechaModificacion,
                UsuarioModificacion = x.UsuarioModificacion,
                IdMes = x.IdMes,
                Mes = x.Mes,
                IdAnno = x.IdAnno,
                Anno = x.Anno,
                IdFuente = x.IdFuente,
                Fuente = ObtenerFuente(x.IdFuente),
                EstadoRegistro = db.EstadoRegistro.Where(i => i.idEstado == x.IdEstado).Single(),
                Solicitud = ObtenerSolicitud(x.IdSolicitud),
                Solicitudid = Utilidades.Encriptar(x.IdSolicitud.ToString()),
                FormularioId = Utilidades.Encriptar(x.IdFormulario.ToString()),
                //DetalleRegistroIndcadorFonatel = DetalleRegistroIndicadorFonatelDAL.ObtenerDatoDetalleRegistroIndicador(new DetalleRegistroIndicadorFonatel() { IdSolicitud = x.IdSolicitud, IdFormulario = x.IdFormulario })
            }).ToList();
        }



        #region Metodos Consulta Base de Datos
        /// <summary>
        /// Metodo que carga los registros de RegistroIndicadorFonatel según parametros
        /// Fecha 29-11-2022
        /// Francisco Vindas
        /// </summary>
        /// <returns>Lista</returns>
        public List<RegistroIndicadorFonatel> ObtenerDatos(RegistroIndicadorFonatel objeto)
        {
            List<RegistroIndicadorFonatel> ListaRegistroIndicadorFonatel = new List<RegistroIndicadorFonatel>();
            using (db = new SIMEFContext())
            {
                //SE NECESITARAN LOS PARAMETROS @idFuente, @RangoFecha
                ListaRegistroIndicadorFonatel = db.Database.SqlQuery<RegistroIndicadorFonatel>
                    ("execute spObtenerRegistroIndicador @IdSolicitud, @IdFormulario, @Codigo, @IdEstado",
                     new SqlParameter("@IdSolicitud", objeto.IdSolicitud),
                     new SqlParameter("@IdFormulario", objeto.IdFormulario),
                     new SqlParameter("@Codigo", string.IsNullOrEmpty(objeto.Codigo) ? DBNull.Value.ToString() : objeto.Codigo),
                     new SqlParameter("@IdEstado", objeto.IdEstado)
                     //new SqlParameter("@IdFuente", objeto.IdFuente),
                     //new SqlParameter("@RangoFecha", objeto.RangoFecha)
                    ).ToList();
            
                ListaRegistroIndicadorFonatel = CrearListado(ListaRegistroIndicadorFonatel);
            }

            return ListaRegistroIndicadorFonatel;
        }



        ///// <summary>
        ///// Actualiza los datos e inserta por medio de merge
        ///// 17/08/2022
        ///// michael Hernández C
        ///// </summary>
        ///// <param name="objRegistroIndicadorFonatel"></param>
        ///// <returns></returns>
        //public List<RegistroIndicadorFonatel> ActualizarDatos(RegistroIndicadorFonatel objeto)
        //{
        //    List<RegistroIndicadorFonatel> ListaRegistroIndicadorFonatel = new List<RegistroIndicadorFonatel>();

        //    using (db = new SIMEFContext())
        //    {
        //        ListaRegistroIndicadorFonatel = db.Database.SqlQuery<RegistroIndicadorFonatel>

        //        ("execute spActualizarRegistroIndicadorFonatel @idRegistroIndicadorFonatel ,@Codigo, @Nombre ,@FechaInicio ,@FechaFin ,@idMes ,@idAnno ,@CantidadFormularios ,@idFuente ,@Mensaje ,@UsuarioCreacion ,@UsuarioModificacion ,@idEstado ",
        //             new SqlParameter("@idRegistroIndicadorFonatel", objeto.idRegistroIndicadorFonatel),
        //             new SqlParameter("@Codigo", string.IsNullOrEmpty(objeto.Codigo) ? DBNull.Value.ToString() : objeto.Codigo),
        //             new SqlParameter("@Nombre", string.IsNullOrEmpty(objeto.Nombre) ? DBNull.Value.ToString() : objeto.Nombre),
        //             objeto.FechaInicio == null ?
        //                new SqlParameter("@FechaInicio", DBNull.Value)
        //                :
        //                new SqlParameter("@FechaInicio", objeto.FechaInicio),
        //             objeto.FechaFin == null ?
        //                new SqlParameter("@FechaFin", DBNull.Value)
        //                :
        //                new SqlParameter("@FechaFin", objeto.FechaFin),
        //             new SqlParameter("@idMes", objeto.idMes),
        //             new SqlParameter("@idAnno", objeto.idAnno),
        //             new SqlParameter("@CantidadFormularios", objeto.CantidadFormularios),
        //             new SqlParameter("@idFuente", objeto.idFuente),
        //             new SqlParameter("@Mensaje", string.IsNullOrEmpty(objeto.Mensaje) ? DBNull.Value.ToString() : objeto.Mensaje),
        //             new SqlParameter("@UsuarioCreacion", objeto.UsuarioCreacion),
        //             new SqlParameter("@UsuarioModificacion", string.IsNullOrEmpty(objeto.UsuarioModificacion) ? DBNull.Value.ToString() : objeto.UsuarioModificacion),
        //             new SqlParameter("@idEstado", objeto.IdEstado)
        //            ).ToList();

        //        ListaRegistroIndicadorFonatel = ListaRegistroIndicadorFonatel.Select(x => new RegistroIndicadorFonatel()
        //        {
        //            id = Utilidades.Encriptar(x.idRegistroIndicadorFonatel.ToString()),
        //            idRegistroIndicadorFonatel = x.idRegistroIndicadorFonatel,
        //            Codigo = x.Codigo,
        //            Nombre = x.Nombre,
        //            FechaInicio = x.FechaInicio,
        //            FechaFin = x.FechaFin,
        //            idMes = x.idMes,
        //            idAnno = x.idAnno,
        //            CantidadFormularios = x.CantidadFormularios,
        //            idFuente = x.idFuente,
        //            Mensaje = x.Mensaje,
        //            FechaCreacion = x.FechaCreacion,
        //            FechaModificacion = x.FechaModificacion,
        //            UsuarioCreacion = x.UsuarioCreacion,
        //            UsuarioModificacion = x.UsuarioModificacion,
        //            Estado = db.EstadoRegistro.Where(i => i.idEstado == x.IdEstado).Single(),
        //        }).ToList();

        //    }

        //    return ListaRegistroIndicadorFonatel;
        //}

        private FuentesRegistro ObtenerFuente(int id)
        {
            FuentesRegistro fuente = db.FuentesRegistro.Where(i => i.idFuente == id).Single();
            fuente.DetalleFuentesRegistro = db.DetalleFuentesRegistro.Where(i => i.idFuente == id).ToList();
            return fuente;
        }

        private Solicitud ObtenerSolicitud(int id)
        {
            Solicitud solicitud = db.Solicitud.Where(i => i.idSolicitud == id).Single();
            solicitud.SolicitudFormulario = db.DetalleSolicitudFormulario.Where(i => i.IdSolicitud == id).ToList();
            return solicitud;
        }








        #endregion

    }
}
