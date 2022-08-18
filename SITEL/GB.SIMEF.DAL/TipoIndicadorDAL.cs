using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.DAL
{
    public class TipoIndicadorDAL : BitacoraDAL
    {
        private SIMEFContext db;

        /// <summary>
        /// 18/08/2022
        /// José Navarro Acuña
        /// Función que retorna todos los tipos indicadores registrados en estado activo
        /// </summary>
        /// <returns></returns>
        public List<TipoIndicadores> ObtenerDatos()
        {
            List<TipoIndicadores> listaTipoIndicadores = new List<TipoIndicadores>();

            using (db = new SIMEFContext())
            {
                listaTipoIndicadores = db.TipoIndicadores.Where(x => x.Estado == true).ToList();
            }

            listaTipoIndicadores = listaTipoIndicadores.Select(x => new TipoIndicadores()
            {
                id = Utilidades.Encriptar(x.IdTipoIdicador.ToString()),
                Nombre = x.Nombre,
                Estado = x.Estado
            }).ToList();

            return listaTipoIndicadores;
        }
    }
}
