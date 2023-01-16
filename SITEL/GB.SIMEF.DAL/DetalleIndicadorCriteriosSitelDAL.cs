using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.DAL
{
    public class DetalleIndicadorCriteriosSitelDAL
    {
        /// <summary>
        /// 23/12/2022
        /// José Navarro Acuña
        /// Función que retorna los criterios de indicadores de mercados
        /// </summary>
        /// <param name="pDetalleIndicadorVariables"></param>
        /// <returns></returns>
        public List<DetalleIndicadorVariables> ObtenerDatosMercado(DetalleIndicadorVariables pDetalleIndicadorVariables)
        {
            List<DetalleIndicadorVariables> listaDetalles = new List<DetalleIndicadorVariables>();
            List<CriterioIndicador> listaDetallesCriterio = new List<CriterioIndicador>();

            using (SIGITELContext db = new SIGITELContext())
            {
                listaDetallesCriterio = db.Database.SqlQuery<CriterioIndicador>(
                    string.Format(
                        "select distinct cast(IdCriterio as varchar) as IdCriterio, CodCriterio, DesCriterio as NombreCriterio from [DIM].[Criterio] " +
                        "where IdIndicador = {0} ", pDetalleIndicadorVariables.idIndicador.ToString())
                    ).ToList();
            }

            listaDetalles = listaDetallesCriterio.Select(x => new DetalleIndicadorVariables()
            {
                id = Utilidades.Encriptar(x.IdCriterio.ToString()),
                NombreVariable = x.NombreCriterio
            }).ToList();

            return listaDetalles;
        }

        /// <summary>
        /// 23/12/2022
        /// José Navarro Acuña
        /// Función que retorna los criterios de indicadores de calidad
        /// </summary>
        /// <param name="pDetalleIndicadorVariables"></param>
        /// <returns></returns>
        public List<DetalleIndicadorVariables> ObtenerDatosCalidad(DetalleIndicadorVariables pDetalleIndicadorVariables)
        {
            List<DetalleIndicadorVariables> listaDetalles = new List<DetalleIndicadorVariables>();
            List<CriterioIndicador> listaDetallesCriterio = new List<CriterioIndicador>();

            using (CALIDADContext db = new CALIDADContext())
            {
                listaDetallesCriterio = db.Database.SqlQuery<CriterioIndicador>(
                    string.Format(
                        "select distinct IdCriterio, NombreCriterio from [FONATEL].[viewIndicadorDGC] " +
                        "where IdIndicador = '{0}' ", pDetalleIndicadorVariables.id)
                    ).ToList();
            }

            listaDetalles = listaDetallesCriterio.Select(x => new DetalleIndicadorVariables()
            {
                id = Utilidades.Encriptar(x.IdCriterio.ToString()),
                NombreVariable = x.NombreCriterio
            }).ToList();

            return listaDetalles;
        }

        /// <summary>
        /// 23/12/2022
        /// José Navarro Acuña
        /// Función que retorna los criterios de indicadores de UIT
        /// </summary>
        /// <param name="pDetalleIndicadorVariables"></param>
        /// <returns></returns>
        public List<DetalleIndicadorVariables> ObtenerDatosIUT(DetalleIndicadorVariables pDetalleIndicadorVariables)
        {
            List<DetalleIndicadorVariables> listaDetalles = new List<DetalleIndicadorVariables>();
            List<CriterioIndicador> listaDetallesCriterio = new List<CriterioIndicador>();

            using (SITELContext db = new SITELContext())
            {
                listaDetallesCriterio = db.Database.SqlQuery<CriterioIndicador>(
                    string.Format(
                        "select distinct IdCriterio, NombreCriterio from [FONATEL].[viewIndicadorUIT] " +
                        "where IdIndicador = '{0}' ", pDetalleIndicadorVariables.id)
                    ).ToList();
            }

            listaDetalles = listaDetallesCriterio.Select(x => new DetalleIndicadorVariables()
            {
                id = Utilidades.Encriptar(x.IdCriterio.ToString()),
                NombreVariable = x.NombreCriterio
            }).ToList();

            return listaDetalles;
        }

        /// <summary>
        /// 23/12/2022
        /// José Navarro Acuña
        /// Función que retorna los criterios de indicadores cruzados
        /// </summary>
        /// <param name="pDetalleIndicadorVariables"></param>
        /// <returns></returns>
        public List<DetalleIndicadorVariables> ObtenerDatosCruzado(DetalleIndicadorVariables pDetalleIndicadorVariables)
        {
            List<DetalleIndicadorVariables> listaDetalles = new List<DetalleIndicadorVariables>();
            List<CriterioIndicador> listaDetallesCriterio = new List<CriterioIndicador>();

            using (SITELContext db = new SITELContext())
            {
                listaDetallesCriterio = db.Database.SqlQuery<CriterioIndicador>(
                    string.Format(
                        "select distinct IdCriterio, NombreCriterio from [FONATEL].[viewIndicadorCruzado] " +
                        "where IdIndicador = '{0}' ", pDetalleIndicadorVariables.id)
                    ).ToList();
            }

            listaDetalles = listaDetallesCriterio.Select(x => new DetalleIndicadorVariables()
            {
                id = Utilidades.Encriptar(x.IdCriterio.ToString()),
                NombreVariable = x.NombreCriterio
            }).ToList();

            return listaDetalles;
        }

        /// <summary>
        /// 23/12/2022
        /// José Navarro Acuña
        /// Función que retorna los criterios de indicadores externos
        /// </summary>
        /// <param name="pDetalleIndicadorVariables"></param>
        /// <returns></returns>
        public List<DetalleIndicadorVariables> ObtenerDatosExterno(DetalleIndicadorVariables pDetalleIndicadorVariables)
        {
            List<DetalleIndicadorVariables> listaDetalles = new List<DetalleIndicadorVariables>();
            List<CriterioIndicador> listaDetallesCriterio = new List<CriterioIndicador>();

            using (SITELContext db = new SITELContext())
            {
                //listaDetallesCriterio = db.Database.SqlQuery<CriterioIndicador>(
                //    string.Format(
                //        "select distinct IdCriterio, NombreCriterio from [FONATEL].[viewIndicadorDGC] " +
                //        "where IdIndicador = '{0}' ", pDetalleIndicadorVariables.id)
                //    ).ToList();
            }

            listaDetalles = listaDetallesCriterio.Select(x => new DetalleIndicadorVariables()
            {
                id = Utilidades.Encriptar(x.IdCriterio.ToString()),
                NombreVariable = x.NombreCriterio
            }).ToList();

            return listaDetalles;
        }

        /// <summary>
        /// 23/12/2022
        /// José Navarro Acuña
        /// Clase privada del modelo DAL para el consumo de la vista que consulta los criterios
        /// </summary>
        private class CriterioIndicador
        {
            public string IdCriterio { get; set; }
            public string NombreCriterio { get; set; }
        }
    }
}
