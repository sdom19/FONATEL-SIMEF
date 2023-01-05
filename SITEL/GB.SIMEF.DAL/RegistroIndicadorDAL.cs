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
    public class RegistroIndicadorFonatelDAL: BitacoraDAL
    {
        private SITELContext db;

        private SIMEFContext SIMEFdb;

        private DetalleRegistroIndicadorFonatelDAL DetalleRegistroIndicadorFonatelDAL;

        public RegistroIndicadorFonatelDAL()
        {
            DetalleRegistroIndicadorFonatelDAL = new DetalleRegistroIndicadorFonatelDAL();
           
        }

        /// Autor: Francisco Vindas
        /// Fecha: 03/01/2023
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
                Mes = x.Mes,
                Estado = x.Estado,
                IdEstado = x.IdEstado,
                FechaModificacion = x.FechaModificacion,
                UsuarioModificacion = x.UsuarioModificacion,
                IdAnno = x.IdAnno,
                Anno = x.Anno,
                IdMes = x.IdMes,
                IdFuente = x.IdFuente,
                Fuente = ObtenerFuente(x.IdFuente),
                Solicitud = ObtenerSolicitud(x.IdSolicitud),
                Solicitudid = Utilidades.Encriptar(x.IdSolicitud.ToString()),
                FormularioId = Utilidades.Encriptar(x.IdFormulario.ToString()),
                DetalleRegistroIndcadorFonatel = DetalleRegistroIndicadorFonatelDAL.ObtenerDatoDetalleRegistroIndicador(new DetalleRegistroIndicadorFonatel() { IdSolicitud = x.IdSolicitud, IdFormulario = x.IdFormulario })

            }).ToList();
        }

        #region Metodos Consulta Base de Datos
        /// <summary>
        /// Metodo que carga los registros de RegistroIndicadorFonatel según parametros
        /// fecha 03-08-2022
        /// Michael Hernandez
        /// </summary>
        /// <returns>Lista</returns>
        public List<RegistroIndicadorFonatel> ObtenerDatos(RegistroIndicadorFonatel objRegistroIndicadorFonatel)
        {
            List<RegistroIndicadorFonatel> ListaRegistroIndicadorFonatel = new List<RegistroIndicadorFonatel>();
            using (db = new SITELContext())
            {

                ListaRegistroIndicadorFonatel = db.Database.SqlQuery<RegistroIndicadorFonatel>
                    ("execute Fonatel.pa_obtenerRegistroIndicadorFonatel @IdSolicitud,@IdFormulario,@idFuente, @idEstado, @RangoFecha",
                     new SqlParameter("@IdSolicitud", objRegistroIndicadorFonatel.IdSolicitud),
                     new SqlParameter("@IdFormulario", objRegistroIndicadorFonatel.IdFormulario),
                     new SqlParameter("@IdFuente", objRegistroIndicadorFonatel.IdFuente),
                     new SqlParameter("@IdEstado", objRegistroIndicadorFonatel.IdEstado),
                     new SqlParameter("@RangoFecha", objRegistroIndicadorFonatel.RangoFecha)
                    ).ToList();

                ListaRegistroIndicadorFonatel = CrearListado(ListaRegistroIndicadorFonatel);
            }
            
            return ListaRegistroIndicadorFonatel;
        }

        public FuentesRegistro ObtenerFuente(int id)
        {
            using (SIMEFdb = new SIMEFContext()) { 
                FuentesRegistro fuente = SIMEFdb.FuentesRegistro.Where(i => i.idFuente == id).Single();
                fuente.DetalleFuentesRegistro = SIMEFdb.DetalleFuentesRegistro.Where(i => i.idFuente == id).ToList();
                return fuente;
            }
        }

        public Solicitud ObtenerSolicitud(int id)
        {
            using (SIMEFdb = new SIMEFContext())
            {
                Solicitud solicitud = SIMEFdb.Solicitud.Where(i => i.idSolicitud == id).Single();
                solicitud.SolicitudFormulario = SIMEFdb.DetalleSolicitudFormulario.Where(i => i.IdSolicitud == id).ToList();
                return solicitud;
            }
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


        #endregion

    }
}
