using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.DAL
{
    public class EnvioSolicitudDAL : BitacoraDAL
    {
        private SIMEFContext db;

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
