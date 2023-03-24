using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using GB.SUTEL.DAL;

namespace GB.SIMEF.DAL
{
    public class FuentesRegistroDAL:BitacoraDAL
    {
        private SIMEFContext db;



        #region Metodos de Consulta a Base Datos

        public List<FuenteRegistro> ObtenerDatos(FuenteRegistro objFuentesRegistro)
        {
            List<FuenteRegistro> ListaFuentesRegistro = new List<FuenteRegistro>();

            using (db = new SIMEFContext())
            {
                ListaFuentesRegistro = db.Database.SqlQuery<FuenteRegistro>
                ("execute pa_ObtenerFuenteRegistro @IdFuenteRegistro, @IdEstadoRegistro",
                new SqlParameter("@IdFuenteRegistro", objFuentesRegistro.IdFuenteRegistro),
                new SqlParameter("@IdEstadoRegistro", objFuentesRegistro.IdEstadoRegistro)
                ).ToList();


                ListaFuentesRegistro = ListaFuentesRegistro.Select(X => new FuenteRegistro
                {
                    id = Utilidades.Encriptar(X.IdFuenteRegistro.ToString()),
                    IdFuenteRegistro = X.IdFuenteRegistro,
                    Fuente = X.Fuente,
                    IdEstadoRegistro=X.IdEstadoRegistro,
                    CantidadDestinatario = X.CantidadDestinatario,
                    FechaCreacion = X.FechaCreacion,
                    UsuarioCreacion = X.UsuarioCreacion,
                    UsuarioModificacion = X.UsuarioModificacion,
                    EstadoRegistro = db.EstadoRegistro.Where(i => i.IdEstadoRegistro == X.IdEstadoRegistro).Single(),
                    DetalleFuenteRegistro = ObtenerDetalleFuentesRegistro(X.IdFuenteRegistro)

                }).ToList();

                return ListaFuentesRegistro;
            }

        }
        /// <summary>
        /// Michael Hernández Cordero
        /// 19/08/2022
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        private List<DetalleFuenteRegistro> ObtenerDetalleFuentesRegistro(int id)
        {
            return db.DetalleFuentesRegistro
                .Where(x => x.idFuenteRegistro == id & x.Estado==true).ToList();
        }

        /// <summary>
        /// Creación y modificación
        /// </summary>
        /// <param name="objFuentesRegistro"></param>
        /// <returns></returns>

        public List<FuenteRegistro> ActualizarDatos(FuenteRegistro objFuentesRegistro)
        {
            List<FuenteRegistro> ListaFuentesRegistro = new List<FuenteRegistro>();

            using (db = new SIMEFContext())
            {
                ListaFuentesRegistro = db.Database.SqlQuery<FuenteRegistro>
                ("execute pa_ActualizarFuenteRegistro @IdFuenteRegistro,@Fuente,@CantidadDestinatario ,@UsuarioCreacion ,@UsuarioModificacion ,@Estado",
                    new SqlParameter("@IdFuenteRegistro", objFuentesRegistro.IdFuenteRegistro),
                    new SqlParameter("@Fuente", objFuentesRegistro.Fuente),
                    new SqlParameter("@CantidadDestinatario", objFuentesRegistro.CantidadDestinatario),
                    new SqlParameter("@UsuarioCreacion",    objFuentesRegistro.UsuarioCreacion),
                    new SqlParameter("@UsuarioModificacion", string.IsNullOrEmpty( objFuentesRegistro.UsuarioModificacion)?DBNull.Value.ToString(): objFuentesRegistro.UsuarioModificacion),
                new SqlParameter("@Estado", objFuentesRegistro.IdEstadoRegistro)
                ).ToList();


                ListaFuentesRegistro = ListaFuentesRegistro.Select(X => new FuenteRegistro
                {
                    id = Utilidades.Encriptar(X.IdFuenteRegistro.ToString()),
                    IdFuenteRegistro = X.IdFuenteRegistro,
                    Fuente = X.Fuente,
                    IdEstadoRegistro = X.IdEstadoRegistro,
                    CantidadDestinatario = X.CantidadDestinatario,
                    FechaCreacion = X.FechaCreacion,
                    UsuarioCreacion = X.UsuarioCreacion,
                    UsuarioModificacion = X.UsuarioModificacion,
                    EstadoRegistro = db.EstadoRegistro.Where(i => i.IdEstadoRegistro == X.IdEstadoRegistro).Single(),
                    DetalleFuenteRegistro = ObtenerDetalleFuentesRegistro(X.IdFuenteRegistro)

                }).ToList();

                return ListaFuentesRegistro;
            }

        }

        /// Michael Hernández Cordero
        /// Valida dependencias con otras tablas
        /// 18/08/2022
        /// </summary>
        /// <param name="fuente"></param>
        /// <returns></returns>

        public List<string> ValidarFuente(FuenteRegistro fuente)
        {
            List<string> listaValicion = new List<string>();
            using (db = new SIMEFContext())
            {
                listaValicion = db.Database.SqlQuery<string>
                    ("exec pa_ValidarFuente @IdFuenteRegistro",
                       new SqlParameter("@IdFuenteRegistro", fuente.IdFuenteRegistro)
                    ).ToList();
            }

            return listaValicion;
        }

        #endregion

    }
}
