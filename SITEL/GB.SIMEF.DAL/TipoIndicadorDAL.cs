using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
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
                if (pTipoIndicadores.IdTipoIndicador != 0)
                {
                    listaTipoIndicadores = db.TipoIndicadores.Where(x => x.IdTipoIndicador == pTipoIndicadores.IdTipoIndicador && x.Estado == true).ToList();
                }
                else
                {
                    listaTipoIndicadores = db.TipoIndicadores.Where(x => x.Estado == true).ToList();
                }
            }

            listaTipoIndicadores = listaTipoIndicadores.Select(x => new TipoIndicadores()
            {
                id = Utilidades.Encriptar(x.IdTipoIndicador.ToString()),
                Nombre = x.Nombre,
                Estado = x.Estado
            }).ToList();
            
            return listaTipoIndicadores;
        }

        /// <summary>
        /// 09/12/2022
        /// José Andrés Navarro
        /// Función que retorna los tipos de indicadores de mercado
        /// </summary>
        /// <returns></returns>
        public List<TipoIndicadores> ObtenerDatosMercado()
        {
            List<TipoIndicadores> listaTipoIndicadores = new List<TipoIndicadores>();

            using (SIGITELContext db = new SIGITELContext())
            {
                listaTipoIndicadores = db.Database.SqlQuery<TipoIndicadores>(
                    "select distinct " +
                    "IdTipoIndicador as IdTipoIdicador, " +
                    "TipoIndicador as Nombre, " +
                    "cast(1 as bit) as Estado " +
                    "from [FONATEL].[viewIndicadorDGM]").ToList();
            }

            return listaTipoIndicadores;
        }

        /// <summary>
        /// 09/12/2022
        /// José Andrés Navarro
        /// Función que retorna los tipos de indicadores de mercado
        /// </summary>
        /// <returns></returns>
        public List<TipoIndicadores> ObtenerDatosCalidad()
        {
            List<TipoIndicadores> listaTipoIndicadores = new List<TipoIndicadores>();

            using (CALIDADContext db = new CALIDADContext())
            {
                listaTipoIndicadores = db.Database.SqlQuery<TipoIndicadores>(
                    "select distinct " +
                    "IdTipoIndicador as IdTipoIdicador, " +
                    "TipoIndicador as Nombre, " +
                    "cast(1 as bit) as Estado " +
                    "from [FONATEL].[viewIndicadorDGC]"
                    ).ToList();
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
        /// 12/12/2022
        /// José Navarro Acuña
        /// Función que retorna los tipos de indicadores de UIT
        /// </summary>
        /// <returns></returns>
        public List<TipoIndicadores> ObtenerDatosUIT()
        {
            List<TipoIndicadores> listaTipoIndicadores = new List<TipoIndicadores>();

            using (SITELContext db = new SITELContext())
            {
                listaTipoIndicadores = db.Database.SqlQuery<TipoIndicadores>(
                    "select distinct " +
                    "IdTipoIndicador as IdTipoIdicador, " +
                    "TipoIndicador as Nombre, " +
                    "cast(1 as bit) as Estado " +
                    "from [FONATEL].[viewIndicadorUIT]"
                    ).ToList();
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
        /// 12/12/2022
        /// José Navarro Acuña
        /// Función que retorna los tipos de indicadores cruzados
        /// </summary>
        /// <returns></returns>
        public List<TipoIndicadores> ObtenerDatosCruzado()
        {
            List<TipoIndicadores> listaTipoIndicadores = new List<TipoIndicadores>();

            using (SITELContext db = new SITELContext())
            {
                listaTipoIndicadores = db.Database.SqlQuery<TipoIndicadores>(
                    "select distinct " +
                    "IdTipoIndicador as IdTipoIdicador, " +
                    "TipoIndicador as Nombre, " +
                    "cast(1 as bit) as Estado " +
                    "from [FONATEL].[viewIndicadorCruzado]"
                    ).ToList();
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

            pTipoIndicadores.IdTipoIndicador = 0;
            listaTipoIndicadores.Add(pTipoIndicadores);

            return listaTipoIndicadores;
        }

        /// <summary>
        /// 22/08/2022
        /// José Navarro Acuña
        /// Función que crea un nuevo registro tipo indicador.
        /// </summary>
        /// <param name="detalleFecha"></param>
        public List<TipoIndicadores> InsertarTipoIndicador(TipoIndicadores pTipoIndicadores)
        {
            List<TipoIndicadores> listaTipoIndicadores = new List<TipoIndicadores>();
            
            using (db = new SIMEFContext())
            {
                db.TipoIndicadores.Add(pTipoIndicadores);
                db.SaveChanges();

                // EF establecerá el objecto cuando sea guardado
                pTipoIndicadores.id = Utilidades.Encriptar(pTipoIndicadores.IdTipoIndicador.ToString());
                pTipoIndicadores.IdTipoIndicador = 0;
                listaTipoIndicadores.Add(pTipoIndicadores);
            }
            return listaTipoIndicadores;
        }
    }
}
