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
        /// 24/11/2022
        /// José Navarro Acuña
        /// Función que retorna todos los tipos indicadores registrados en estado activo de la BD de SITEL.
        /// Se puede filtrar por el ID del objecto
        /// </summary>
        /// <param name="pTipoIndicadores"></param>
        /// <returns></returns>
        public List<TipoIndicadores> ObtenerDatosSitel(TipoIndicadores pTipoIndicadores)
        {
            List<TipoIndicadores> listaTiposIndicadores = new List<TipoIndicadores>();
            using (db = new SIMEFContext())
            {
                List<SUTEL.Entities.TipoIndicador> listaTiposIndicadoresSitel = 
                    db.Database.SqlQuery<SUTEL.Entities.TipoIndicador> // Notar el uso de la clase "TipoIndicador" el cual pertenece al namespace Sitel
                        ("execute spObtenerTipoIndicadoresSitel @pIdTipoIndicador ",
                        new SqlParameter("@pIdTipoIndicador", pTipoIndicadores.IdTipoIdicador)
                    ).ToList();

                listaTiposIndicadores = listaTiposIndicadoresSitel.Select(x => new TipoIndicadores()
                { // hacer el "traspaso" de datos hacia la identidad del namespace actual
                    id = Utilidades.Encriptar(x.IdTipoInd.ToString()),
                    Nombre = x.DesTipoInd
                }).ToList();
            }
            return listaTiposIndicadores;
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
                pTipoIndicadores.id = Utilidades.Encriptar(pTipoIndicadores.IdTipoIdicador.ToString());
                pTipoIndicadores.IdTipoIdicador = 0;
                listaTipoIndicadores.Add(pTipoIndicadores);
            }
            return listaTipoIndicadores;
        }
    }
}
