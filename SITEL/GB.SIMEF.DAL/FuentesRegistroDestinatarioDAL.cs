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

        public List<DetalleFuentesRegistro> ObtenerDatos(DetalleFuentesRegistro objDetalleFuentesRegistro)
        {
            List<DetalleFuentesRegistro> ListaDetalleFuentesRegistro = new List<DetalleFuentesRegistro>();

            using (db = new SIMEFContext())
            {
             

                ListaDetalleFuentesRegistro = db.Database.SqlQuery<DetalleFuentesRegistro>
                ("execute spObtenerFuentesRegistrosDestinatarios @idDetalleFuente, @idFuentesRegistro",
                    new SqlParameter("@idDetalleFuente", objDetalleFuentesRegistro.idDetalleFuente),
                    new SqlParameter("@idFuentesRegistro", objDetalleFuentesRegistro.idFuente)
                ).ToList();
                ListaDetalleFuentesRegistro = ListaDetalleFuentesRegistro.Select(x => new DetalleFuentesRegistro()
                {
                    idDetalleFuente = x.idDetalleFuente,
                    idFuente = x.idFuente,
                    NombreDestinatario = x.NombreDestinatario,
                    CorreoElectronico = x.CorreoElectronico,
                    Estado = x.Estado,
                    idUsuario = x.idUsuario,
                    NombreFuente = db.FuentesRegistro.Where(i => i.idFuente == x.idFuente).Single().Fuente,
                    CantidadDisponible = ListaDetalleFuentesRegistro.Count() - (int)db.FuentesRegistro.Where(i => i.idFuente == x.idFuente).Single().CantidadDestinatario,
                    CorreoEnviado = x.CorreoEnviado
                }).ToList();

                return ListaDetalleFuentesRegistro;
            }

        }



        public List<DetalleFuentesRegistro> ActualizarDatos(DetalleFuentesRegistro objDetalleFuentesRegistro)
        {
            List<DetalleFuentesRegistro> ListaDetalleFuentesRegistro = new List<DetalleFuentesRegistro>();

            using (db = new SIMEFContext())
            {
                ListaDetalleFuentesRegistro = db.Database.SqlQuery<DetalleFuentesRegistro>
                ("execute spActualizarFuentesRegistroDetalle @idDetalleFuente, @idFuente, @Nombre, @CorreoElectronico, @Estado, @IdUsuario",
                    new SqlParameter("@idDetalleFuente", objDetalleFuentesRegistro.idDetalleFuente),
                    new SqlParameter("@idFuente", objDetalleFuentesRegistro.idFuente),
                    new SqlParameter("@Nombre", string.IsNullOrEmpty(objDetalleFuentesRegistro.NombreDestinatario)?DBNull.Value.ToString(): objDetalleFuentesRegistro.NombreDestinatario),
                    new SqlParameter("@CorreoElectronico", string.IsNullOrEmpty(objDetalleFuentesRegistro.CorreoElectronico) ? DBNull.Value.ToString() : objDetalleFuentesRegistro.CorreoElectronico),
                    new SqlParameter("@Estado", objDetalleFuentesRegistro.Estado),
                     new SqlParameter("@IdUsuario", objDetalleFuentesRegistro.idUsuario)
                ).ToList();

                ListaDetalleFuentesRegistro = ListaDetalleFuentesRegistro.Select(x => new DetalleFuentesRegistro()
                {
                    idDetalleFuente=x.idDetalleFuente,
                    idFuente=x.idFuente,
                    NombreDestinatario=x.NombreDestinatario,
                    CorreoElectronico=x.CorreoElectronico,
                    Estado=x.Estado,
                    idUsuario=x.idUsuario,
                    NombreFuente = db.FuentesRegistro.Where(i => i.idFuente == x.idFuente).Single().Fuente,
                    CantidadDisponible = ListaDetalleFuentesRegistro.Count() - (int)db.FuentesRegistro.Where(i => i.idFuente == x.idFuente).Single().CantidadDestinatario,
                    CorreoEnviado = x.CorreoEnviado
                }).ToList();

                return ListaDetalleFuentesRegistro;
            }

        }



  





        #endregion

    }
}
