using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.DAL
{
    public class EnvioSolicitudesDAL : BitacoraDAL
    {
        private SIMEFContext db;


        public List<EnvioSolicitudes> ActualizarDatos(EnvioSolicitudes solicitud)
        {
            List<EnvioSolicitudes> envioSolicitudes = new List<EnvioSolicitudes>();

            using (db = new SIMEFContext())
            {

                envioSolicitudes  = db.Database.SqlQuery<EnvioSolicitudes>
                ("execute  spActualizarEnvioSolicitudTabla @IdEnvio, @IdSolicitud, @Enviado,@EnvioProgramado, @MensajError",
                    new SqlParameter("@IdEnvio", solicitud.idEnvio),
                    new SqlParameter("@IdSolicitud", solicitud.IdSolicitud),
                    new SqlParameter("@Enviado", solicitud.Enviado),
                    new SqlParameter("@EnvioProgramado", solicitud.EnvioProgramado),
                    new SqlParameter("@MensajError", solicitud.MensajError)
                ).ToList();

            }

            return envioSolicitudes;
        }














        /// <summary>
        /// Listado de envíos de solicitud 
        /// </summary>
        /// <returns></returns>
        public List<EnvioSolicitudes> ObtenerDatos()
        {
            List<EnvioSolicitudes> envioSolicitudes = new List<EnvioSolicitudes>();

            using (db = new SIMEFContext())
            {
                
                envioSolicitudes = db.EnvioSolicitudes.Where(x => x.Enviado == false).ToList();
            }

            return envioSolicitudes;
        }

  
    }
}
