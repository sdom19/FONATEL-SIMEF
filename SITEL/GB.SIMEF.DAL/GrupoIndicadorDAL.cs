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
        public List<GrupoIndicador> ObtenerDatos(GrupoIndicador pGrupoIndicadores)
        {
            List<GrupoIndicador> listaGrupoIndicadores = new List<GrupoIndicador>();

            using (db = new SIMEFContext())
            {
                if (pGrupoIndicadores.IdGrupoIndicador != 0)
                {
                    listaGrupoIndicadores = db.GrupoIndicadores.Where(x => x.IdGrupoIndicador == pGrupoIndicadores.IdGrupoIndicador && x.Estado == true).ToList();
                }
                else
                {
                    listaGrupoIndicadores = db.GrupoIndicadores.Where(x => x.Estado == true).ToList();
                }
            }

            listaGrupoIndicadores = listaGrupoIndicadores.Select(x => new GrupoIndicador()
            {
                id = Utilidades.Encriptar(x.IdGrupoIndicador.ToString()),
                Nombre = x.Nombre,
                Estado = x.Estado
            }).ToList();

            return listaGrupoIndicadores;
        }

        /// <summary>
        /// 09/12/2022
        /// José Navarro Acuña
        /// Retorna todos los grupos de indicadores registrados de Mercados
        /// </summary>
        /// <returns></returns>
        public List<GrupoIndicador> ObtenerDatosMercado()
        {
            List<GrupoIndicador> listaGrupoIndicadores = new List<GrupoIndicador>();

            using (SIGITELContext db = new SIGITELContext())
            {
                listaGrupoIndicadores = db.Database.SqlQuery<GrupoIndicador>(
                    "select distinct " +
                    "0 as idGrupo, " +
                    "Agrupacion as Nombre, " +
                    "cast(1 as bit) as Estado " +
                    "from [FONATEL].[viewIndicadorDGM]"
                    ).ToList();
            }

            listaGrupoIndicadores = listaGrupoIndicadores.Select(x => new GrupoIndicador()
            {
                id = x.Nombre,
                Nombre = x.Nombre,
                Estado = x.Estado
            }).ToList();

            return listaGrupoIndicadores;
        }

        /// <summary>
        /// 09/12/2022
        /// José Navarro Acuña
        /// Retorna todos los grupos de indicadores registrados de Calidad
        /// </summary>
        /// <returns></returns>
        public List<GrupoIndicador> ObtenerDatosCalidad()
        {
            List<GrupoIndicador> listaGrupoIndicadores = new List<GrupoIndicador>();

            using (CALIDADContext db = new CALIDADContext())
            {
                listaGrupoIndicadores = db.Database.SqlQuery<GrupoIndicador>(
                    "select distinct " +
                    "0 as idGrupo, " +
                    "Agrupacion as Nombre, " +
                    "cast(1 as bit) as Estado " +
                    "from [FONATEL].[viewIndicadorDGC]"
                    ).ToList();
            }

            listaGrupoIndicadores = listaGrupoIndicadores.Select(x => new GrupoIndicador()
            {
                id = x.Nombre,
                Nombre = x.Nombre,
                Estado = x.Estado
            }).ToList();

            return listaGrupoIndicadores;
        }

        /// <summary>
        /// 12/12/2022
        /// José Navarro Acuña
        /// Retorna todos los grupos de indicadores registrados de UIT
        /// </summary>
        /// <returns></returns>
        public List<GrupoIndicador> ObtenerDatosUIT()
        {
            List<GrupoIndicador> listaGrupoIndicadores = new List<GrupoIndicador>();

            using (SITELContext db = new SITELContext())
            {
                listaGrupoIndicadores = db.Database.SqlQuery<GrupoIndicador>(
                    "select distinct " +
                    "0 as idGrupo, " +
                    "Agrupacion as Nombre, " +
                    "cast(1 as bit) as Estado " +
                    "from [FONATEL].[viewIndicadorUIT]"
                    ).ToList();
            }

            listaGrupoIndicadores = listaGrupoIndicadores.Select(x => new GrupoIndicador()
            {
                id = x.Nombre,
                Nombre = x.Nombre,
                Estado = x.Estado
            }).ToList();

            return listaGrupoIndicadores;
        }

        /// <summary>
        /// 12/12/2022
        /// José Navarro Acuña
        /// Retorna todos los grupos de indicadores cruzados registrado 
        /// </summary>
        /// <returns></returns>
        public List<GrupoIndicador> ObtenerDatosCruzado()
        {
            List<GrupoIndicador> listaGrupoIndicadores = new List<GrupoIndicador>();

            using (SITELContext db = new SITELContext())
            {
                listaGrupoIndicadores = db.Database.SqlQuery<GrupoIndicador>(
                    "select distinct " +
                    "0 as idGrupo, " +
                    "Agrupacion as Nombre, " +
                    "cast(1 as bit) as Estado " +
                    "from [FONATEL].[viewIndicadorCruzado]"
                    ).ToList();
            }

            listaGrupoIndicadores = listaGrupoIndicadores.Select(x => new GrupoIndicador()
            {
                id = x.Nombre,
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
        public List<GrupoIndicador> ActualizarDatos(GrupoIndicador pGrupoIndicadores)
        {
            List<GrupoIndicador> listaGrupoIndicadores = new List<GrupoIndicador>();

            using (db = new SIMEFContext())
            {
                db.Entry(pGrupoIndicadores).State = EntityState.Modified;
                db.SaveChanges();
            }

            pGrupoIndicadores.IdGrupoIndicador = 0;
            listaGrupoIndicadores.Add(pGrupoIndicadores);

            return listaGrupoIndicadores;
        }

        /// <summary>
        /// 22/08/2022
        /// José Navarro Acuña
        /// Función que crea un nuevo registro grupo indicador.
        /// </summary>
        /// <param name="detalleFecha"></param>
        public List<GrupoIndicador> InsertarGrupoIndicador(GrupoIndicador pGrupoIndicadores)
        {
            List<GrupoIndicador> listaGrupos = new List<GrupoIndicador>();

            using (db = new SIMEFContext())
            {
                db.GrupoIndicadores.Add(pGrupoIndicadores);
                db.SaveChanges();

                // EF establecerá el objecto cuando sea guardado
                pGrupoIndicadores.id = Utilidades.Encriptar(pGrupoIndicadores.IdGrupoIndicador.ToString());
                pGrupoIndicadores.IdGrupoIndicador = 0;
                listaGrupos.Add(pGrupoIndicadores);
            }
            return listaGrupos;
        }
    }
}
