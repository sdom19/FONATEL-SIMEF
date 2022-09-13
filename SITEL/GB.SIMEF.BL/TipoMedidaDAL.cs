using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;


namespace GB.SIMEF.DAL
{
    public class TipoMedidaDAL : BitacoraDAL
    {
        private SIMEFContext db;

        /// <summary>
        /// 23/08/2022
        /// José Navarro Acuña
        /// Función que retorna todos los tipos de medidas registrados en estado activo.
        /// Se puede filtrar por el ID del objecto
        /// </summary>
        /// <returns></returns>
        public List<TipoMedida> ObtenerDatos(TipoMedida pTipoMedida)
        {
            List<TipoMedida> lista = new List<TipoMedida>();

            using (db = new SIMEFContext())
            {
                if (pTipoMedida.idMedida != 0)
                {
                    lista = db.TipoMedida.Where(x => x.idMedida == pTipoMedida.idMedida && x.Estado == true).ToList();
                }
                else
                {
                    lista = db.TipoMedida.Where(x => x.Estado == true).ToList();
                }
            }

            lista = lista.Select(x => new TipoMedida()
            {
                id = Utilidades.Encriptar(x.idMedida.ToString()),
                Nombre = x.Nombre,
                Estado = x.Estado
            }).ToList();

            return lista;
        }
    }
}
