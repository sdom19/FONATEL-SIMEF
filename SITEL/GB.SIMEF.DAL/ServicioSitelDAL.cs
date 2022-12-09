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
    public class ServicioSitelDAL
    {
        private SIMEFContext db;

        public ServicioSitelDAL()
        {

        }

        /// <summary>
        /// 24/11/2022
        /// José Navarro Acuña
        /// Función que retorna todos los tipos de servicio registrados en estado activo de la BD de SITEL.
        /// Se puede filtrar por el ID del objecto
        /// </summary>
        /// <param name="pServicioSitel"></param>
        /// <returns></returns>
        public List<ServicioSitel> ObtenerDatos(ServicioSitel pServicioSitel)
        {
            List<ServicioSitel> listaServicioSitel = new List<ServicioSitel>();
            using (db = new SIMEFContext())
            {
                List<SUTEL.Entities.Servicio> listaTiposIndicadoresSitel =
                    db.Database.SqlQuery<SUTEL.Entities.Servicio> // Notar el uso de la clase "Servicio" el cual pertenece al namespace Sitel
                        ("execute spObtenerServiciosSitel @pIdServicio ",
                        new SqlParameter("@pIdServicio", pServicioSitel.IdServicio)
                    ).ToList();

                listaServicioSitel = listaTiposIndicadoresSitel.Select(x => new ServicioSitel()
                { // hacer el "traspaso" de datos hacia la identidad del namespace actual
                    id = Utilidades.Encriptar(x.IdServicio.ToString()),
                    Nombre = x.DesServicio
                }).ToList();
            }
            return listaServicioSitel;
        }

        /// <summary>
        /// 09/12/2022
        /// José Navarro Acuña
        /// Función que retorna los tipos de servicios de Mercado
        /// </summary>
        /// <returns></returns>
        public List<ServicioSitel> ObtenerDatosMercado()
        {
            List<ServicioSitel> listaServicioSitel = new List<ServicioSitel>();

            using (SIGITELContext db = new SIGITELContext())
            {
                listaServicioSitel = db.Database.SqlQuery<ServicioSitel>
                    ("select distinct IdServicio, Servicio Nombre from [FONATEL].[viewIndicadorDGM]").ToList();
            }

            listaServicioSitel = listaServicioSitel.Select(x => new ServicioSitel()
            {
                id = Utilidades.Encriptar(x.IdServicio.ToString()),
                Nombre = x.Nombre,
                Estado = true
            }).ToList();

            return listaServicioSitel;
        }
    }
}
