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
    public class AnnoDAL : BitacoraDAL
    {
        private SIMEFContext db;

       /// <summary>
       /// Listado de años 
       /// </summary>
       /// <returns></returns>
        public List<Anno> ObtenerDatos()
        {
            List<Anno> listaAnno = new List<Anno>();

            using (db = new SIMEFContext())
            {
                
                listaAnno = db.Anno.Where(x => x.Estado == true).ToList();
            }

            return listaAnno;
        }

  
    }
}
