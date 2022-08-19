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
    public class UnidadEstudioDAL : BitacoraDAL
    {
        private SIMEFContext db;

        /// <summary>
        /// 18/08/2022
        /// José Navarro Acuña
        /// Función que retorna todos los tipos indicadores registrados en estado activo
        /// Se puede filtrar por el ID del objecto
        /// </summary>
        /// <returns></returns>
        public List<UnidadEstudio> ObtenerDatos(UnidadEstudio pUnidadEstudio)
        {
            List<UnidadEstudio> listaUnidadEstudio = new List<UnidadEstudio>();

            using (db = new SIMEFContext())
            {
                if (pUnidadEstudio.idUnidad != 0)
                {
                    listaUnidadEstudio = db.UnidadEstudio.Where(x => x.idUnidad == pUnidadEstudio.idUnidad && x.Estado == true).ToList();
                }
                else
                {
                    listaUnidadEstudio = db.UnidadEstudio.Where(x => x.Estado == true).ToList();
                }
            }

            listaUnidadEstudio = listaUnidadEstudio.Select(x => new UnidadEstudio()
            {
                id = Utilidades.Encriptar(x.idUnidad.ToString()),
                Nombre = x.Nombre,
                Estado = x.Estado
            }).ToList();

            return listaUnidadEstudio;
        }

        /// <summary>
        /// 18/08/2022
        /// José Navarro Acuña
        /// Función que actualiza los datos de una unidad de estudio
        /// </summary>
        /// <param name="pTipoIndicadores"></param>
        /// <returns></returns>
        public List<UnidadEstudio> ActualizarDatos(UnidadEstudio pUnidadEstudio)
        {
            List<UnidadEstudio> listaUnidadEstudio = new List<UnidadEstudio>();

            using (db = new SIMEFContext())
            {
                db.Entry(pUnidadEstudio).State = EntityState.Modified;
                db.SaveChanges();
            }

            pUnidadEstudio.idUnidad = 0;
            listaUnidadEstudio.Add(pUnidadEstudio);

            return listaUnidadEstudio;
        }
    }
}
