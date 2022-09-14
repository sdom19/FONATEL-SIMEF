using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.DAL
{
    public class FrecuenciaEnvioDAL : BitacoraDAL
    {
        private SIMEFContext db;

        /// <summary>
        /// 23/08/2022
        /// José Navarro Acuña
        /// Función que retorna todos las frecuencias de envio registradas en estado activo.
        /// Se puede filtrar por el ID del objecto
        /// </summary>
        /// <returns></returns>
        public List<FrecuenciaEnvio> ObtenerDatos(FrecuenciaEnvio pFrecuenciaEnvio)
        {
            List<FrecuenciaEnvio> lista = new List<FrecuenciaEnvio>();

            using (db = new SIMEFContext())
            {
                if (pFrecuenciaEnvio.idFrecuencia != 0)
                {
                    lista = db.FrecuenciaEnvio.Where(x => x.idFrecuencia == pFrecuenciaEnvio.idFrecuencia && x.Estado == true).ToList();
                }
                else
                {
                    lista = db.FrecuenciaEnvio.Where(x => x.Estado == true).ToList();
                }
            }

            lista = lista.Select(x => new FrecuenciaEnvio()
            {
                id = Utilidades.Encriptar(x.idFrecuencia.ToString()),
                idFrecuencia=x.idFrecuencia,
                Nombre = x.Nombre,
                CantidadDias = x.CantidadDias,
                Estado = x.Estado
            }).ToList();

            return lista;
        }
    }
}
