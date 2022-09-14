using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.DAL
{
    public class ClasificacionIndicadorDAL : BitacoraDAL
    {
        private SIMEFContext db;

        /// <summary>
        /// 23/08/2022
        /// José Navarro Acuña
        /// Función que retorna todas las clasificaciones de indicadores registradas en estado activo.
        /// Se puede filtrar por el ID del objecto
        /// </summary>
        /// <returns></returns>
        public List<ClasificacionIndicadores> ObtenerDatos(ClasificacionIndicadores pClasificacionIndicadores)
        {
            List<ClasificacionIndicadores> lista = new List<ClasificacionIndicadores>();

            using (db = new SIMEFContext())
            {
                if (pClasificacionIndicadores.idClasificacion != 0)
                {
                    lista = db.ClasificacionIndicadores.Where(x => x.idClasificacion == pClasificacionIndicadores.idClasificacion && x.Estado == true).ToList();
                }
                else
                {
                    lista = db.ClasificacionIndicadores.Where(x => x.Estado == true).ToList();
                }
            }

            lista = lista.Select(x => new ClasificacionIndicadores()
            {
                id = Utilidades.Encriptar(x.idClasificacion.ToString()),
                Nombre = x.Nombre,
                Estado = x.Estado
            }).ToList();

            return lista;
        }
    }
}
