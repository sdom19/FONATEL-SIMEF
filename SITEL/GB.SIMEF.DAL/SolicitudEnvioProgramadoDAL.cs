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
    public class SolicitudEnvioProgramadoDAL : BitacoraDAL
    {
        private SIMEFContext db;

        /// <summary>
        /// Autor: Francisco Vindas Ruiz
        /// Fecha: 18/10/2022
        /// Ejecutar procedimiento almacenado para insertar envios programados
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public List<SolicitudEnvioProgramado> ActualizarDatos(SolicitudEnvioProgramado objeto)
        {
            List<SolicitudEnvioProgramado> ListaDetalle = new List<SolicitudEnvioProgramado>();

            using (db = new SIMEFContext())
            {
                ListaDetalle = db.Database.SqlQuery<SolicitudEnvioProgramado>

                    ("execute spActualizarEnvioProgramado @pIdEnvioProgramado ,@pIdSolicitud ,@pIdFrecuencia ,@pCantidadRepeticiones ,@pFechaCiclo, @pEstado",
                      new SqlParameter("@pIdEnvioProgramado", objeto.IdEnvioProgramado),
                      new SqlParameter("@pIdSolicitud", objeto.IdSolicitud),
                      new SqlParameter("@pIdFrecuencia", objeto.IdFrecuencia),
                      new SqlParameter("@pCantidadRepeticiones", objeto.CantidadRepiticiones),
                      new SqlParameter("@pFechaCiclo", objeto.FechaCiclo),
                      new SqlParameter("@pEstado", objeto.Estado)
                    ).ToList();
            }

            return ListaDetalle;
        }
    }
}
