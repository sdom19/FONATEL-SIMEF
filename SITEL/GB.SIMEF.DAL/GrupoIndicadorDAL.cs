using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.DAL
{
    public class GrupoIndicadorDAL
    {
        private SIMEFContext db;

        /// <summary>
        /// 18/08/2022
        /// José Navarro Acuña
        /// Función que retorna todos los grupos de indicadores registrados en estado activo
        /// </summary>
        /// <returns></returns>
        public List<GrupoIndicadores> ObtenerDatos()
        {
            List<GrupoIndicadores> listaGrupoIndicadores = new List<GrupoIndicadores>();

            using (db = new SIMEFContext())
            {
                listaGrupoIndicadores = db.GrupoIndicadores.Where(x => x.Estado == true).ToList();
            }

            listaGrupoIndicadores = listaGrupoIndicadores.Select(x => new GrupoIndicadores()
            {
                id = Utilidades.Encriptar(x.idGrupo.ToString()),
                Nombre = x.Nombre,
                Estado = x.Estado
            }).ToList();

            return listaGrupoIndicadores;
        }
    }
}
