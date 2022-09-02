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
    public class MesDAL : BitacoraDAL
    {
        private SIMEFContext db;

       /// <summary>
       /// Listado de años 
       /// </summary>
       /// <returns></returns>
        public List<Mes> ObtenerDatos()
        {
            List<Mes> listaMes = new List<Mes>();

            using (db = new SIMEFContext())
            {
                
                listaMes = db.Mes.Where(x => x.Estado == true).ToList();
            }

            return listaMes;
        }

  
    }
}
