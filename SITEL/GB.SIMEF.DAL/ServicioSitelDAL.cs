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
                    ("select distinct IdServicio, Servicio Nombre from [FONATEL].[vw_IndicadorDGM]").ToList();
            }

            listaServicioSitel = listaServicioSitel.Select(x => new ServicioSitel()
            {
                id = Utilidades.Encriptar(x.IdServicio.ToString()),
                Nombre = x.Nombre,
                Estado = true
            }).ToList();

            return listaServicioSitel;
        }

        /// <summary>
        /// 09/12/2022
        /// José Navarro Acuña
        /// Función que retorna los tipos de servicios de Calidad
        /// </summary>
        /// <returns></returns>
        public List<ServicioSitel> ObtenerDatosCalidad()
        {
            List<ServicioSitel> listaServicioSitel = new List<ServicioSitel>();

            using (CALIDADContext db = new CALIDADContext())
            {
                listaServicioSitel = db.Database.SqlQuery<ServicioSitel>
                    ("select distinct IdServicio, Servicio Nombre from [FONATEL].[vw_IndicadorDGC]").ToList();
            }

            listaServicioSitel = listaServicioSitel.Select(x => new ServicioSitel()
            {
                id = Utilidades.Encriptar(x.IdServicio.ToString()),
                Nombre = x.Nombre,
                Estado = true
            }).ToList();

            return listaServicioSitel;
        }

        /// <summary>
        /// 12/12/2022
        /// José Navarro Acuña
        /// Función que retorna los tipos de servicios de UIT
        /// </summary>
        /// <returns></returns>
        public List<ServicioSitel> ObtenerDatosUIT()
        {
            List<ServicioSitel> listaServicioSitel = new List<ServicioSitel>();

            using (SITELContext db = new SITELContext())
            {
                listaServicioSitel = db.Database.SqlQuery<ServicioSitel>
                    ("select distinct IdServicio, Servicio Nombre from [FONATEL].[vw_IndicadorUIT]").ToList();
            }

            listaServicioSitel = listaServicioSitel.Select(x => new ServicioSitel()
            {
                id = Utilidades.Encriptar(x.IdServicio.ToString()),
                Nombre = x.Nombre,
                Estado = true
            }).ToList();

            return listaServicioSitel;
        }

        /// <summary>
        /// 12/12/2022
        /// José Navarro Acuña
        /// Función que retorna los tipos de servicios de indicadores cruzados
        /// </summary>
        /// <returns></returns>
        public List<ServicioSitel> ObtenerDatosCruzados()
        {
            List<ServicioSitel> listaServicioSitel = new List<ServicioSitel>();

            using (SITELContext db = new SITELContext())
            {
                listaServicioSitel = db.Database.SqlQuery<ServicioSitel>
                    ("select distinct IdServicio, Servicio Nombre from [FONATEL].[vw_IndicadorCruzado]").ToList();
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
