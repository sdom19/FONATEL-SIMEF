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
    public class TipoReglaValidacionDAL : BitacoraDAL
    {
        private SIMEFContext db;

       /// <summary>
       /// Listado de años 
       /// </summary>
       /// <returns></returns>
        public List<TipoReglaValidacion> ObtenerDatos()
        {
            List<TipoReglaValidacion> listaTipoReglaValidacion = new List<TipoReglaValidacion>();

            using (db = new SIMEFContext())
            {
                
                listaTipoReglaValidacion = db.TipoReglaValidacion.Where(x => x.Estado == true).ToList();
            }

            return listaTipoReglaValidacion;
        }

  
    }
}
