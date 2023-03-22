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
    public class OperadorArismeticoDAL : BitacoraDAL
    {
        private SIMEFContext db;

       /// <summary>
       /// Listado de años 
       /// </summary>
       /// <returns></returns>
        public List<OperadorAritmetico> ObtenerDatos()
        {
            List<OperadorAritmetico> listaOperadorArismetico = new List<OperadorAritmetico>();

            using (db = new SIMEFContext())
            {
                
                listaOperadorArismetico = db.OperadorArismetico.Where(x => x.Estado == true).ToList();
            }

            return listaOperadorArismetico;
        }

  
    }
}
