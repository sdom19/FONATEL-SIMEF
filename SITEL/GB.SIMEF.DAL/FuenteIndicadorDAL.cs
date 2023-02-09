using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.DAL
{
    public class FuenteIndicadorDAL : BitacoraDAL
    {
        private SIMEFContext db;

        /// <summary>
        /// 22/11/2022
        /// José Navarro Acuña
        /// Función que retorna las fuentes de indicador.
        /// Permite obtener un único elemento a traves del ID.
        /// </summary>
        /// <param name="pFuenteIndicador"></param>
        /// <returns></returns>
        public List<FuenteIndicador> ObtenerDatos(FuenteIndicador pFuenteIndicador)
        {
            List<FuenteIndicador> listaFuentes = new List<FuenteIndicador>();

            using (db = new SIMEFContext())
            {
                listaFuentes = db.Database.SqlQuery<FuenteIndicador>
                ("execute spObtenerFuentesIndicadores @pIdFuenteIndicador",
                new SqlParameter("@pIdFuenteIndicador", pFuenteIndicador.IdFuenteIndicador)
                ).ToList();

                listaFuentes = listaFuentes.Select(x => new FuenteIndicador
                {
                    id = Utilidades.Encriptar(x.IdFuenteIndicador.ToString()),
                    IdFuenteIndicador = x.IdFuenteIndicador,
                    Fuente = x.Fuente,
                    Estado = x.Estado,
                }).ToList();

                return listaFuentes;
            }
        }
    }
}
