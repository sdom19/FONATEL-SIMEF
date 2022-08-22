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
    public class GrupoIndicadorDAL : BitacoraDAL
    {
        private SIMEFContext db;

        /// <summary>
        /// 18/08/2022
        /// José Navarro Acuña
        /// Función que retorna todos los grupos de indicadores registrados en estado activo.
        /// Se puede filtrar por el ID del objecto
        /// </summary>
        /// <returns></returns>
        public List<GrupoIndicadores> ObtenerDatos(GrupoIndicadores pGrupoIndicadores)
        {
            List<GrupoIndicadores> listaGrupoIndicadores = new List<GrupoIndicadores>();

            using (db = new SIMEFContext())
            {
                if (pGrupoIndicadores.idGrupo != 0)
                {
                    listaGrupoIndicadores = db.GrupoIndicadores.Where(x => x.idGrupo == pGrupoIndicadores.idGrupo && x.Estado == true).ToList();
                }
                else
                {
                    listaGrupoIndicadores = db.GrupoIndicadores.Where(x => x.Estado == true).ToList();
                }
            }

            listaGrupoIndicadores = listaGrupoIndicadores.Select(x => new GrupoIndicadores()
            {
                id = Utilidades.Encriptar(x.idGrupo.ToString()),
                Nombre = x.Nombre,
                Estado = x.Estado
            }).ToList();

            return listaGrupoIndicadores;
        }

        /// <summary>
        /// 18/08/2022
        /// José Navarro Acuña
        /// Función que actualiza los datos de un grupo indicador.
        /// </summary>
        /// <param name="pTipoIndicadores"></param>
        /// <returns></returns>
        public List<GrupoIndicadores> ActualizarDatos(GrupoIndicadores pGrupoIndicadores)
        {
            List<GrupoIndicadores> listaGrupoIndicadores = new List<GrupoIndicadores>();

            using (db = new SIMEFContext())
            {
                db.Entry(pGrupoIndicadores).State = EntityState.Modified;
                db.SaveChanges();
            }

            pGrupoIndicadores.idGrupo = 0;
            listaGrupoIndicadores.Add(pGrupoIndicadores);

            return listaGrupoIndicadores;
        }

        /// <summary>
        /// 22/08/2022
        /// José Navarro Acuña
        /// Función que crea un nuevo registro grupo indicador.
        /// </summary>
        /// <param name="detalleFecha"></param>
        public List<GrupoIndicadores> InsertarGrupoIndicador(GrupoIndicadores pGrupoIndicadores)
        {
            List<GrupoIndicadores> listaGrupos = new List<GrupoIndicadores>();

            using (db = new SIMEFContext())
            {
                db.GrupoIndicadores.Add(pGrupoIndicadores);
                db.SaveChanges();

                // EF establecerá el objecto cuando sea guardado
                pGrupoIndicadores.id = Utilidades.Encriptar(pGrupoIndicadores.idGrupo.ToString());
                pGrupoIndicadores.idGrupo = 0;
                listaGrupos.Add(pGrupoIndicadores);
            }
            return listaGrupos;
        }
    }
}
