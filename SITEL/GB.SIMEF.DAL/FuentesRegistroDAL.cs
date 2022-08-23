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
    public class FuentesRegistroDAL:BitacoraDAL
    {
        private SIMEFContext db;

        #region Metodos de Consulta a Base Datos

        public List<FuentesRegistro> ObtenerDatos(FuentesRegistro objFuentesRegistro)
        {
            List<FuentesRegistro> ListaFuentesRegistro = new List<FuentesRegistro>();

            using (db = new SIMEFContext())
            {
                ListaFuentesRegistro = db.Database.SqlQuery<FuentesRegistro>
                ("execute spObtenerFuentesRegistros @idFuentesRegistro,@idEstado",
                new SqlParameter("@idFuentesRegistro", objFuentesRegistro.idFuente),
                new SqlParameter("@idEstado", objFuentesRegistro.idEstado)
                ).ToList();


                ListaFuentesRegistro = ListaFuentesRegistro.Select(X => new FuentesRegistro
                {
                    id = Utilidades.Encriptar(X.idFuente.ToString()),
                    idFuente = X.idFuente,
                    Fuente = X.Fuente,
                    idEstado=X.idEstado,
                    CantidadDestinatario = X.CantidadDestinatario,
                    FechaCreacion = X.FechaCreacion,
                    UsuarioCreacion = X.UsuarioCreacion,
                    UsuarioModificacion = X.UsuarioModificacion,
                    EstadoRegistro = db.EstadoRegistro.Where(i => i.idEstado == X.idEstado).Single(),
                    DetalleFuentesRegistro = ObtenerDetalleFuentesRegistro(X.idFuente)

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

        private List<DetalleFuentesRegistro> ObtenerDetalleFuentesRegistro(int id)
        {
            return db.DetalleFuentesRegistro
                .Where(x => x.idFuente == id & x.Estado==true).ToList();
        }



        public List<FuentesRegistro> ActualizarDatos(FuentesRegistro objFuentesRegistro)
        {
            List<FuentesRegistro> ListaFuentesRegistro = new List<FuentesRegistro>();

            using (db = new SIMEFContext())
            {
                ListaFuentesRegistro = db.Database.SqlQuery<FuentesRegistro>
                ("execute spActualizarFuentesRegistro @idFuente,@Fuente,@CantidadDestinatario ,@UsuarioCreacion ,@UsuarioModificacion ,@Estado",
                    new SqlParameter("@idFuente", objFuentesRegistro.idFuente),
                    new SqlParameter("@Fuente", objFuentesRegistro.Fuente),
                    new SqlParameter("@CantidadDestinatario", objFuentesRegistro.CantidadDestinatario),
                    new SqlParameter("@UsuarioCreacion",    objFuentesRegistro.UsuarioCreacion),
                    new SqlParameter("@UsuarioModificacion", string.IsNullOrEmpty( objFuentesRegistro.UsuarioModificacion)?DBNull.Value.ToString(): objFuentesRegistro.UsuarioModificacion),
                new SqlParameter("@Estado", objFuentesRegistro.idEstado)
                ).ToList();


                ListaFuentesRegistro = ListaFuentesRegistro.Select(X => new FuentesRegistro
                {
                    id = Utilidades.Encriptar(X.idFuente.ToString()),
                    idFuente = X.idFuente,
                    Fuente = X.Fuente,
                    CantidadDestinatario = X.CantidadDestinatario,
                    FechaCreacion = X.FechaCreacion,
                    UsuarioCreacion = X.UsuarioCreacion,
                    UsuarioModificacion = X.UsuarioModificacion,
                    EstadoRegistro = db.EstadoRegistro.Where(i => i.idEstado == X.idEstado).Single(),
                    DetalleFuentesRegistro = ObtenerDetalleFuentesRegistro(X.idFuente)

                }).ToList();

                return ListaFuentesRegistro;
            }

        }


        #endregion

    }
}
