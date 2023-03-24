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
    public class FuentesRegistroDestinatarioDAL : BitacoraDAL
    {
        private SIMEFContext db;

        #region Metodos de Consulta a Base Datos

        public List<DetalleFuenteRegistro> ObtenerDatos(DetalleFuenteRegistro objDetalleFuentesRegistro)
        {
            List<DetalleFuenteRegistro> ListaDetalleFuentesRegistro = new List<DetalleFuenteRegistro>();

            using (db = new SIMEFContext())
            {
                ListaDetalleFuentesRegistro = db.Database.SqlQuery<DetalleFuenteRegistro>
                ("execute pa_ObtenerFuenteRegistroDestinatario @idDetalleFuente, @idFuentesRegistro",
                    new SqlParameter("@idDetalleFuente", objDetalleFuentesRegistro.idDetalleFuenteRegistro),
                    new SqlParameter("@idFuentesRegistro", objDetalleFuentesRegistro.idFuenteRegistro)
                ).ToList();
                ListaDetalleFuentesRegistro = ListaDetalleFuentesRegistro.Select(x => new DetalleFuenteRegistro()
                {
                    idDetalleFuenteRegistro = x.idDetalleFuenteRegistro,
                    idFuenteRegistro = x.idFuenteRegistro,
                    NombreDestinatario = x.NombreDestinatario,
                    CorreoElectronico = x.CorreoElectronico,
                    Estado = x.Estado,
                    idUsuario = x.idUsuario,
                    NombreFuente = db.FuentesRegistro.Where(i => i.IdFuenteRegistro == x.idFuenteRegistro).Single().Fuente,
                    CantidadDisponible = ListaDetalleFuentesRegistro.Count() - (int)db.FuentesRegistro.Where(i => i.IdFuenteRegistro == x.idFuenteRegistro).Single().CantidadDestinatario,
                    CorreoEnviado = x.CorreoEnviado
                }).ToList();

                return ListaDetalleFuentesRegistro;
            }

        }

        public List<DetalleFuenteRegistro> ActualizarDatos(DetalleFuenteRegistro objDetalleFuentesRegistro)
        {
            List<DetalleFuenteRegistro> ListaDetalleFuentesRegistro = new List<DetalleFuenteRegistro>();

            using (db = new SIMEFContext())
            {
                ListaDetalleFuentesRegistro = db.Database.SqlQuery<DetalleFuenteRegistro>
                ("execute pa_ActualizarFuenteRegistroDetalle @IdDetalleFuenteRegistro, @IdFuenteRegistro, @Nombre, @CorreoElectronico, @Estado, @IdUsuario",
                    new SqlParameter("@IdDetalleFuenteRegistro", objDetalleFuentesRegistro.idDetalleFuenteRegistro),
                    new SqlParameter("@IdFuenteRegistro", objDetalleFuentesRegistro.idFuenteRegistro),
                    new SqlParameter("@Nombre", string.IsNullOrEmpty(objDetalleFuentesRegistro.NombreDestinatario)?DBNull.Value.ToString(): objDetalleFuentesRegistro.NombreDestinatario),
                    new SqlParameter("@CorreoElectronico", string.IsNullOrEmpty(objDetalleFuentesRegistro.CorreoElectronico) ? DBNull.Value.ToString() : objDetalleFuentesRegistro.CorreoElectronico),
                    new SqlParameter("@Estado", objDetalleFuentesRegistro.Estado),
                     new SqlParameter("@IdUsuario", objDetalleFuentesRegistro.idUsuario)
                ).ToList();

                ListaDetalleFuentesRegistro = ListaDetalleFuentesRegistro.Select(x => new DetalleFuenteRegistro()
                {
                    idDetalleFuenteRegistro=x.idDetalleFuenteRegistro,
                    idFuenteRegistro=x.idFuenteRegistro,
                    NombreDestinatario=x.NombreDestinatario,
                    CorreoElectronico=x.CorreoElectronico,
                    Estado=x.Estado,
                    idUsuario=x.idUsuario,
                    NombreFuente = db.FuentesRegistro.Where(i => i.IdFuenteRegistro == x.idFuenteRegistro).Single().Fuente,
                    CantidadDisponible = ListaDetalleFuentesRegistro.Count() - (int)db.FuentesRegistro.Where(i => i.IdFuenteRegistro == x.idFuenteRegistro).Single().CantidadDestinatario,
                    CorreoEnviado = x.CorreoEnviado
                }).ToList();

                return ListaDetalleFuentesRegistro;
            }
        }
        #endregion
    }
}
