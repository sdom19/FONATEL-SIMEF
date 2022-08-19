using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace GB.SIMEF.DAL
{
    public class TipoIndicadorDAL : BitacoraDAL
    {
        private SIMEFContext db;

        /// <summary>
        /// 18/08/2022
        /// José Navarro Acuña
        /// Función que retorna todos los tipos indicadores registrados en estado activo.
        /// Se puede filtrar por el ID del objecto
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

        /// <summary>
        /// 18/08/2022
        /// José Navarro Acuña
        /// Función que actualiza los datos de un tipo de indicador.
        /// </summary>
        /// <param name="pTipoIndicadores"></param>
        /// <returns></returns>
        public List<TipoIndicadores> ActualizarDatos(TipoIndicadores pTipoIndicadores)
        {
            List<TipoIndicadores> listaTipoIndicadores = new List<TipoIndicadores>();

            using (db = new SIMEFContext())
            {
                db.Entry(pTipoIndicadores).State = EntityState.Modified;
                db.SaveChanges();
            }

            pTipoIndicadores.IdTipoIdicador = 0;
            listaTipoIndicadores.Add(pTipoIndicadores);

            return listaTipoIndicadores;
        }
    }
}
