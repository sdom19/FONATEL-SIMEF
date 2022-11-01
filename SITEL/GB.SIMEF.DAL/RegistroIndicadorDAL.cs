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
        private  SIMEFContext db;
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
            using (db = new SIMEFContext())
            {

                ListaRegistroIndicadorFonatel = db.Database.SqlQuery<RegistroIndicadorFonatel>
                    ("execute sitel.spObtenerRegistroIndicadorFonatel @idFuente, @idEstado",
                     new SqlParameter("@idFuente", objRegistroIndicadorFonatel.IdFuente),
                     new SqlParameter("@idEstado", objRegistroIndicadorFonatel.IdEstado)
                    ).ToList();
            }
            return ListaRegistroIndicadorFonatel;
        }
        private FuentesRegistro ObtenerFuente(int id)
        {
            FuentesRegistro fuente = db.FuentesRegistro.Where(i => i.idFuente == id).Single();
            fuente.DetalleFuentesRegistro = db.DetalleFuentesRegistro.Where(i => i.idFuente == id).ToList();
            return fuente;
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
