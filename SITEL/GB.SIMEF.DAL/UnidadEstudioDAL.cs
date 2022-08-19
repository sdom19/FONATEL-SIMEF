using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.DAL
{
    public class UnidadEstudioDAL : BitacoraDAL
    {
        private SIMEFContext db;

        /// <summary>
        /// 18/08/2022
        /// José Navarro Acuña
        /// Función que retorna todos los tipos indicadores registrados en estado activo
        /// </summary>
        /// <returns></returns>
        public List<UnidadEstudio> ObtenerDatos()
        {
            List<UnidadEstudio> listaUnidadEstudio = new List<UnidadEstudio>();

            using (db = new SIMEFContext())
            {
                listaUnidadEstudio = db.UnidadEstudio.Where(x => x.Estado == true).ToList();
            }

            listaUnidadEstudio = listaUnidadEstudio.Select(x => new UnidadEstudio()
            {
                id = Utilidades.Encriptar(x.idUnidad.ToString()),
                Nombre = x.Nombre,
                Estado = x.Estado
            }).ToList();

            return listaUnidadEstudio;
        }
    }
}
