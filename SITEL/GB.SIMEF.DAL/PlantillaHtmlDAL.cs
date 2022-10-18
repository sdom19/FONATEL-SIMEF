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
    public class PlantillaHtmlDAL : BitacoraDAL
    {
        private SIMEFContext db;

       /// <summary>
       /// obtener Platillas para envío
       /// </summary>
       /// <returns></returns>
        public PlantillaHtml ObtenerDatos(int idPlantilla)
        {
            PlantillaHtml listaPlantillaHtml = new PlantillaHtml();
            using (db = new SIMEFContext())
            { 
                listaPlantillaHtml = db.PlantillaHtml.Where(x=>x.IdPlantilla==idPlantilla).Single();
            }
            return listaPlantillaHtml;
        }

  
    }
}
