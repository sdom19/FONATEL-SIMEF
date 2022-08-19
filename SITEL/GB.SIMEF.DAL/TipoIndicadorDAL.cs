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
        /// Función que retorna todos los tipos indicadores registrados en estado activo.
        /// Se puede filtrar por el ID del tipo indicador
        /// </summary>
        /// <returns></returns>
        public List<TipoIndicadores> ObtenerDatos(TipoIndicadores pTipoIndicadores)
        {
            List<TipoIndicadores> listaTipoIndicadores = new List<TipoIndicadores>();

            using (db = new SIMEFContext())
            {
                if (pTipoIndicadores.IdTipoIdicador != 0)
                {
                    listaTipoIndicadores = db.TipoIndicadores.Where(x => x.IdTipoIdicador == pTipoIndicadores.IdTipoIdicador && x.Estado == true).ToList();
                }
                else
                {
                    listaTipoIndicadores = db.TipoIndicadores.Where(x => x.Estado == true).ToList();
                }
            }

            listaTipoIndicadores = listaTipoIndicadores.Select(x => new TipoIndicadores()
            {
                id = Utilidades.Encriptar(x.IdTipoIdicador.ToString()),
                Nombre = x.Nombre,
                Estado = x.Estado
            }).ToList();

            return listaTipoIndicadores;
        }

        public List<TipoIndicadores> ActualizarDatos(TipoIndicadores pTipoIndicadores)
        {
            List<TipoIndicadores> listaTipoIndicadores = new List<TipoIndicadores>();

            using (db = new SIMEFContext())
            {
                db.Entry(pTipoIndicadores).Property(i => i.Estado).IsModified = true;
                db.SaveChanges();
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
