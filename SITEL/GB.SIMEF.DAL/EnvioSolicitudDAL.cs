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
    public class EnvioSolicitudDAL : BitacoraDAL
    {
        private SIMEFContext db;


        public List<EnvioSolicitud> ActualizarEnvioSolicitud(EnvioSolicitud solicitud)
        {
            List<EnvioSolicitud> envioSolicitudes = new List<EnvioSolicitud>();

            using (db = new SIMEFContext())
            {

                envioSolicitudes  = db.Database.SqlQuery<EnvioSolicitud>
                ("execute  pa_ActualizarEnvioSolicitudTabla @IdEnvio, @IdSolicitud, @Enviado,@EnvioProgramado, @MensajeError, @EjecutarJob",
                    new SqlParameter("@IdEnvio", solicitud.idEnvioSolicitud),
                    new SqlParameter("@IdSolicitud", solicitud.IdSolicitud),
                    new SqlParameter("@Enviado", solicitud.Enviado),
                    new SqlParameter("@EnvioProgramado", solicitud.EnvioProgramado),
                    new SqlParameter("@MensajeError", string.IsNullOrEmpty( solicitud.MensajeError)? DBNull.Value.ToString(): solicitud.MensajeError),
                     new SqlParameter("@EjecutarJob", solicitud.EjecutaJob==true?1:0)
                ).ToList();

            }

            return envioSolicitudes;
        }



        /// <summary>
        /// Autor: Francisco Vindas RuiZ
        /// Fecha: 14-02-2023
        /// Metodo para obtner todos los envios de solicitudes realizados a SITELP
        /// </summary>
        /// <returns></returns>
        public List<EnvioSolicitud> ObtenerEnviosCorrectos()
        {
            List<EnvioSolicitud> envioSolicitudes = new List<EnvioSolicitud>();

            using (db = new SIMEFContext())
            {

                envioSolicitudes = db.EnvioSolicitud.Where(x => x.Enviado == true).ToList();
            }

            return envioSolicitudes;
        }


        /// <summary>
        /// Listado de envíos de solicitud 
        /// </summary>
        /// <returns></returns>
        public List<EnvioSolicitud> ObtenerDatos()
        {
            List<EnvioSolicitud> envioSolicitudes = new List<EnvioSolicitud>();

            using (db = new SIMEFContext())
            {
                
                envioSolicitudes = db.EnvioSolicitud.Where(x => x.Enviado == false).ToList();
            }

            return envioSolicitudes;
        }

  
    }
}
