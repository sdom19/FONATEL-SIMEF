using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GB.SIMEF.Resources.Constantes;

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
        public List<DetalleIndicadorVariable> ObtenerDatosMercado(DetalleIndicadorVariable pDetalleIndicadorVariables)
        {
            List<DetalleIndicadorVariable> listaDetalles = new List<DetalleIndicadorVariable>();
            List<CriterioIndicador> listaDetallesCriterio = new List<CriterioIndicador>();

            using (SIGITELContext db = new SIGITELContext())
            {
                listaDetallesCriterio = db.Database.SqlQuery<CriterioIndicador>
                    ("execute [FONATEL].pa_ObtenerCriteriosDeIndicador @pIndicador ",
                     new SqlParameter("@pIndicador", pDetalleIndicadorVariables.IdIndicador.ToString())
                    ).ToList();
            }

            listaDetalles = listaDetallesCriterio.Select(x => new DetalleIndicadorVariable()
            {
                id = Utilidades.Encriptar(x.IdCriterio.ToString()),
                NombreVariable = x.NombreCriterio
            }).ToList();

            return listaDetalles;
        }

        /// <summary>
        /// 14/02/2023
        /// José Navarro Acuña
        /// Función que retorna los detalles de un criterio proveniente de mercados
        /// </summary>
        /// <param name="pDetalleIndicadorVariables"></param>
        /// <returns></returns>
        public List<DetalleIndicadorCategoria> ObtenerDetallesDeCriterioMercado(DetalleIndicadorCategoria pDetalleIndicadorVariables)
        {
            List<DetalleIndicadorCategoria> listaDetalles = new List<DetalleIndicadorCategoria>();
            List<DetalleCriterioIndicador> listaDetallesCriterio = new List<DetalleCriterioIndicador>();

            using (SIGITELContext db = new SIGITELContext())
            {
                listaDetallesCriterio = db.Database.SqlQuery<DetalleCriterioIndicador>
                    ("execute [FONATEL].pa_ObtenerDetallesAgrupacionDeCriterio @pIdCriterio, @pIdDetalle, @pIncluirColumnaValor",
                     new SqlParameter("@pIdCriterio", pDetalleIndicadorVariables.idCriterioInt.ToString()),
                     new SqlParameter("@pIdDetalle", DBNull.Value), // retornar toda la lista
                     new SqlParameter("@pIncluirColumnaValor", false)
                    ).ToList();
            }

            listaDetalles = listaDetallesCriterio.Select(x => new DetalleIndicadorCategoria()
            {
                id = Utilidades.Encriptar(x.IdDetalle.ToString()),
                Etiquetas = x.Detalle
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
        public List<DetalleIndicadorVariable> ObtenerDatosCalidad(DetalleIndicadorVariable pDetalleIndicadorVariables)
        {
            List<DetalleIndicadorVariable> listaDetalles = new List<DetalleIndicadorVariable>();
            List<CriterioIndicador> listaDetallesCriterios = new List<CriterioIndicador>();

            using (CALIDADContext db = new CALIDADContext())
            {
                listaDetallesCriterios = db.Database.SqlQuery<CriterioIndicador>(
                    string.Format(
                        "select top 1 IdIndicador as IdCriterio from [dbo].[FactRigurosidadFac] " +
                        "where IdIndicador = '{0}' ", pDetalleIndicadorVariables.id)
                    ).ToList();
            }

            if (listaDetallesCriterios.Count > 0)
            {
                listaDetalles.Add( // al mostrar columnas como valores para seleccionar y siendo ajenas a la BD de Fonatel, se procede con constantes
                    new DetalleIndicadorVariable()
                    {
                        id = Utilidades.Encriptar(((int)TipoPorcentajeIndicadorCalculoEnum.indicador).ToString()),
                        NombreVariable = CriteriosIndicadoresCalidad.procentajeIndicador
                    });

                listaDetalles.Add(
                    new DetalleIndicadorVariable()
                    {
                        id = Utilidades.Encriptar(((int)TipoPorcentajeIndicadorCalculoEnum.indicador).ToString()),
                        NombreVariable = CriteriosIndicadoresCalidad.procentajeCumplimiento
                    });
            }

            return listaDetalles;
        }

        /// <summary>
        /// 23/12/2022
        /// José Navarro Acuña
        /// Función que retorna los criterios de indicadores de UIT
        /// </summary>
        /// <param name="pDetalleIndicadorVariables"></param>
        /// <returns></returns>
        public List<DetalleIndicadorVariable> ObtenerDatosIUT(DetalleIndicadorVariable pDetalleIndicadorVariables)
        {
            List<DetalleIndicadorVariable> listaDetalles = new List<DetalleIndicadorVariable>();
            List<CriterioIndicador> listaDetallesCriterio = new List<CriterioIndicador>();

            using (SITELContext db = new SITELContext())
            {
                listaDetallesCriterio = db.Database.SqlQuery<CriterioIndicador>(
                    string.Format(
                        "select distinct IdCriterio, NombreCriterio from [FONATEL].[vw_IndicadorUIT] " +
                        "where IdIndicador = '{0}' ", pDetalleIndicadorVariables.id)
                    ).ToList();
            }

            listaDetalles = listaDetallesCriterio.Select(x => new DetalleIndicadorVariable()
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
        public List<DetalleIndicadorVariable> ObtenerDatosCruzado(DetalleIndicadorVariable pDetalleIndicadorVariables)
        {
            List<DetalleIndicadorVariable> listaDetalles = new List<DetalleIndicadorVariable>();
            List<CriterioIndicador> listaDetallesCriterio = new List<CriterioIndicador>();

            using (SITELContext db = new SITELContext())
            {
                listaDetallesCriterio = db.Database.SqlQuery<CriterioIndicador>(
                    string.Format(
                        "select distinct IdCriterio, NombreCriterio from [FONATEL].[vw_IndicadorCruzado] " +
                        "where IdIndicador = '{0}' ", pDetalleIndicadorVariables.id)
                    ).ToList();
            }

            listaDetalles = listaDetallesCriterio.Select(x => new DetalleIndicadorVariable()
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
        public List<DetalleIndicadorVariable> ObtenerDatosExterno(DetalleIndicadorVariable pDetalleIndicadorVariables)
        {
            List<DetalleIndicadorVariable> listaDetalles = new List<DetalleIndicadorVariable>();
            List<CriterioIndicador> listaDetallesCriterio = new List<CriterioIndicador>();

            using (SITELContext db = new SITELContext())
            {
                //listaDetallesCriterio = db.Database.SqlQuery<CriterioIndicador>(
                //    string.Format(
                //        "select distinct IdCriterio, NombreCriterio from [FONATEL].[viewIndicadorDGC] " +
                //        "where IdIndicador = '{0}' ", pDetalleIndicadorVariables.id)
                //    ).ToList();
            }

            listaDetalles = listaDetallesCriterio.Select(x => new DetalleIndicadorVariable()
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
            public string CodigoCriterio { get; set; }
            public string NombreCriterio { get; set; }
        }

        /// <summary>
        /// 14/02/2022
        /// José Navarro Acuña
        /// Clase privada del modelo DAL para el consumo de la vista que consulta los criterios
        /// </summary>
        private class DetalleCriterioIndicador
        {
            public int IdIndicador { get; set; }
            public string CodidogIndicador { get; set; }
            public int IdCriterio { get; set; }
            public string CodigoCriterio { get; set; }
            public string Criterio { get; set; }
            public int IdDetalle { get; set; }
            public string Detalle { get; set; }
            public int IdFechaIndicador { get; set; }
        }
    }
}
