using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

using SIMEF.API.Controllers;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using SIMEF.API.Models;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using GB.SIMEF.API.Model;
using Microsoft.Data.SqlClient;
using Dapper;

namespace GB.SIMEF.API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class DescargaIndicadoresController : ControllerBase
    {

        //private SIMEFContext db;
        private DWHSIMEFContext db = new DWHSIMEFContext();


        /// <summary>
        /// Obtiene los indicadores filtrados por servicio y tipo
        /// </summary>
        /// <param name="programa">Servicio index</param>
        /// <param name="tipo">Nombre Tipo</param>
        /// <returns>Lista de indicadores</returns>
        [HttpGet]
        [Route("/api/DescargaIndicadores/GetGrupo")]
        [ProducesResponseType(typeof(List<Combos>), 200)]
        public ActionResult<IEnumerable<Combos>> GetGrupo()
        {
            var SqlQuery = "execute [FONATEL].[spObtenerGrupos]";

            List<Combos> lista = null;

            using (var connection = new SqlConnection(Connection.SIGITELDatabase))
            {
                lista = connection.Query<Combos>(SqlQuery, new { }).ToList();
            }
            return lista;
        }
        /// <summary>
        /// Obtiene los indicadores filtrados por servicio y tipo
        /// </summary>
        /// <param name="programa">Servicio index</param>
        /// <param name="tipo">Nombre Tipo</param>
        /// <returns>Lista de indicadores</returns>
        [HttpGet]
        [Route("/api/DescargaIndicadores/GetTipo")]
        [ProducesResponseType(typeof(List<Combos>), 200)]
        public ActionResult<IEnumerable<Combos>> GetTipo()
        {
            var SqlQuery = "execute [FONATEL].[spObtenerTipos]";

            List<Combos> lista = null;

            using (var connection = new SqlConnection(Connection.SIGITELDatabase))
            {
                lista = connection.Query<Combos>(SqlQuery, new { }).ToList();
            }
            return lista;
        }
        /// <summary>
        /// Obtiene un indicador por id
        /// </summary>
        /// <param name="Grupo">Id del Indicador</param>
        /// /// <param name="Tipo">Id del Indicador</param>
        /// <returns>Indicador</returns>
        [HttpGet]
        [Route("/api/DescargaIndicadores/GetIndicadores")]
        [ProducesResponseType(typeof(List<Combos>), 200)]
        public ActionResult<IEnumerable<Combos>> GetIndicadores(int Grupo, int Tipo)
        {
            var SqlQuery = "execute [FONATEL].[spObtenerIndicadores] @idtipoIndicador,@idGrupo";

            List<Combos> lista = null;

            using (var connection = new SqlConnection(Connection.SIGITELDatabase))
            {
                lista = connection.Query<Combos>(SqlQuery, new { idtipoIndicador = Tipo , idGrupo = Grupo }).ToList();
            }
            return lista;
        }
        /// <summary>
        /// Obtiene un indicador por id
        /// </summary>
        /// <param name="Grupo">Id del Indicador</param>
        /// /// <param name="Tipo">Id del Indicador</param>
        /// <returns>Indicador</returns>
        [HttpGet]
        [Route("/api/DescargaIndicadores/GetVariableDato")]
        [ProducesResponseType(typeof(List<Combos>), 200)]
        public ActionResult<IEnumerable<Combos>> GetVariableDato(int IdIndicador)
        {
            var SqlQuery = "execute [FONATEL].[spObtenerVariables] @idIndicador";

            List<Combos> lista = null;

            using (var connection = new SqlConnection(Connection.SIGITELDatabase))
            {
                lista = connection.Query<Combos>(SqlQuery, new { idIndicador = IdIndicador }).ToList();
            }
            return lista;
        }
        /// <summary>
        /// Obtiene las categorias por indicador
        /// </summary>
        /// <param name="IdIndicador">Id del Indicador</param>
        /// <returns>Indicador</returns>
        [HttpGet]
        [Route("/api/DescargaIndicadores/GetCategoria")]
        [ProducesResponseType(typeof(List<Combos>), 200)]
        public ActionResult<IEnumerable<Combos>> GetCategoria(int IdIndicador)
        {
            var SqlQuery = "execute [FONATEL].[spObtenerCategorias] @idIndicador";

            List<Combos> lista = null;

            using (var connection = new SqlConnection(Connection.SIGITELDatabase))
            {
                lista = connection.Query<Combos>(SqlQuery, new { idIndicador = IdIndicador }).ToList();
            }
            return lista;
        }

        /// <summary>
        /// Obtiene las categorias por indicador
        /// </summary>
        /// <param name="IdIndicador">Id del Indicador</param>
        /// <returns>Indicador</returns>
        [HttpGet]
        [Route("/api/DescargaIndicadores/GetAnno")]
        [ProducesResponseType(typeof(List<Combos>), 200)]
        public ActionResult<IEnumerable<Combos>> GetAnno(int IdIndicador)
        {
            var SqlQuery = "execute [FONATEL].[spObtenerAnno] @idIndicador";

            List<Combos> lista = null;

            using (var connection = new SqlConnection(Connection.SIGITELDatabase))
            {
                lista = connection.Query<Combos>(SqlQuery, new { idIndicador = IdIndicador }).ToList();
            }
            return lista;
        }

        /// <summary>
        /// Obtiene las categorias por indicador
        /// </summary>
        /// <param name="IdIndicador">Id del Indicador</param>
        /// <returns>Indicador</returns>
        [HttpGet]
        [Route("/api/DescargaIndicadores/GetMes")]
        [ProducesResponseType(typeof(List<Combos>), 200)]
        public ActionResult<IEnumerable<Combos>> GetMes(int IdIndicador)
        {
            var SqlQuery = "execute [FONATEL].[spObtenerMes] @idIndicador";

            List<Combos> lista = null;

            using (var connection = new SqlConnection(Connection.SIGITELDatabase))
            {
                lista = connection.Query<Combos>(SqlQuery, new { idIndicador = IdIndicador }).ToList();
            }
            return lista;
        }


        /// <summary>
        /// Método para filtrar indicadores por servicio
        /// </summary>
        /// <param name="servicio">Servicio index</param>
        /// <returns>Lista Indicadores</returns>
        internal List<DimDefinicionIndicador> FiltrarPrograma(string Programa)
        {
            var indicadores = new List<DimDefinicionIndicador>();
            int IdGrupo = 0, Tipo = 0;
            switch (Programa)
            {
                case "P1":
                    //Programa 1
                    IdGrupo = 1;
                    //Tipo = 1;
                    goto default;
                case "S2":
                    //Telefonía móvil
                    IdGrupo = 4;
                    Tipo = 2;
                    goto default;
                case "S3":
                    //Telefonía fija
                    IdGrupo = 3;
                    Tipo = 3;
                    goto default;
                case "S4":
                    //Televisión por suscripción
                    IdGrupo = 6;
                    Tipo = 4;
                    goto default;
                case "S5":
                    //Internet
                    //indicadores = db.Indicador
                    // .Where(x => ((x.ServicioIndicador.Any(y => y.IdServicio == 5) && x.IndicadorDireccion.Any(y => y.IdDireccion == 1) && x.ServicioDefinicion.Tipo != 6) || x.ServicioDefinicion.Tipo == 5) && x.ParametroIndicador.Any(y => y.Visualiza))
                    // .Select(x => new Indicador { IdIndicador = x.IdIndicador, NombreIndicador = x.NombreIndicador, DefinicionIndicador = x.DefinicionIndicador, IndicadorDireccion = x.IndicadorDireccion, IdTipoInd = x.IdTipoInd })
                    // .ToList();
                    break;
                case "S6":
                    //Líneas dedicadas
                    //indicadores = db.Indicador
                    // .Where(x => (x.ServicioIndicador.Any(y => y.IdServicio == 5) && x.IndicadorDireccion.Any(y => y.IdDireccion == 1) && x.ServicioDefinicion.Tipo == 6) && x.ParametroIndicador.Any(y => y.Visualiza))
                    // .Select(x => new Indicador { IdIndicador = x.IdIndicador, NombreIndicador = x.NombreIndicador, DefinicionIndicador = x.DefinicionIndicador, IndicadorDireccion = x.IndicadorDireccion, IdTipoInd = x.IdTipoInd })
                    // .ToList();
                    break;
                default:
                    indicadores = db.DimDefinicionIndicador
                    .Where(x => (x.Idgrupo == IdGrupo))
                    .Select(x => new DimDefinicionIndicador { IdIndicador = x.IdIndicador, Nombre = x.Nombre, Definicion = x.Definicion, IdTipoindicador = x.IdTipoindicador })
                    .ToList();
                    break;
            }
            //Retorna lista de indicadores
            return indicadores;
        }

        /// <summary>
        /// Método para filtrar indicadores por tipo indicador 
        /// </summary>
        /// <param name="indicadores">Lista de Indicadores</param>
        /// <param name="tipo">Tipo</param>
        /// <returns>Lista de indicadores</returns>
        internal List<IndicadorViewModel> FiltrarTipo(List<DimDefinicionIndicador> indicadores, string tipo)
        {
            var indicadoresViewModel = new List<IndicadorViewModel>();
            switch (tipo)
            {

                case "Gestion":
                    //Tipo Suscripción
                    indicadoresViewModel = indicadores.Where(x => x.IdTipoindicador == 2)
                        .Select(x => new IndicadorViewModel { IdIndicador = x.IdIndicador.ToString(), NombreIndicador = x.Nombre, DefinicionIndicador = x.Definicion })
                        .ToList();

                    break;
                case "Tráfico":
                case "Trafico":
                    //Tipo Tráfico
                    //indicadoresViewModel = indicadores.Where(x => x.IndicadorDireccion.Any(y => y.IdDireccion == 1) && (x.IdTipoInd == 7 || (x.IdTipoInd >= 11 && x.IdTipoInd <= 14)))
                    // .Select(x => new IndicadorViewModel { IdIndicador = x.IdIndicador, NombreIndicador = x.NombreIndicador, DefinicionIndicador = x.DefinicionIndicador })
                    // .ToList();
                    break;
                case "Ingreso":
                    //Tipo Ingreso
                    //indicadoresViewModel = indicadores.Where(x => x.IndicadorDireccion.Any(y => y.IdDireccion == 1) && x.IdTipoInd == 5)
                    // .Select(x => new IndicadorViewModel { IdIndicador = x.IdIndicador, NombreIndicador = x.NombreIndicador, DefinicionIndicador = x.DefinicionIndicador })
                    // .ToList();
                    break;
                case "General":
                    //Tipo General
                    // List<int> rechazados = new List<int>() { 5, 7, 10, 11, 12, 13, 14 };
                    // indicadoresViewModel = indicadores.Where(x => x.IndicadorDireccion.Any(y => y.IdDireccion == 1) && (!rechazados.Contains(x.IdTipoInd)))
                    //.Select(x => new IndicadorViewModel { IdIndicador = x.IdIndicador, NombreIndicador = x.NombreIndicador, DefinicionIndicador = x.DefinicionIndicador })
                    //  .ToList();
                    break;
            }
            //Retorna lista de indicadores
            return indicadoresViewModel;
        }

        /// <summary>
        /// Obtiene los resultados
        /// </summary>
        /// <param name="IdIndicador">Id del indicador</param>
        /// <param name="idVariable">Id del variable</param>
        /// <param name="AnnoInicio">Año de inicio</param>
        /// <param name="MesInicio">Mes Inicio</param>
        /// <param name="AnnoFin">Año fin</param>
        /// <param name="MesFin">Mes fin</param>
        /// <returns>Indicador</returns>
        [HttpGet]
        [Route("/api/DescargaIndicadores/GetResultado")]
        [ProducesResponseType(typeof(List<DimResultadoIndicador>), 200)]
        public ActionResult<IEnumerable<DimResultadoIndicador>> GetResultado(int IdIndicador, int idVariable, string desde, string hasta, int idCategoria)
        {
            var SqlQuery = "execute [FONATEL].[spObtenerDimResultadoIndicador] @idIndicador,@desde,@hasta,@idCategoria";

            List<DimResultadoIndicador> lista = null;

            using (var connection = new SqlConnection(Connection.SIGITELDatabase))
            {
                lista = connection.Query<DimResultadoIndicador>(SqlQuery, new { IdIndicador = IdIndicador, idVariable=idVariable, desde= desde, hasta = hasta, idCategoria = idCategoria }).ToList();
            }
            return lista;
        }

        /// <summary>
        /// Obtiene los detalles de categoria
        /// </summary>
        /// <param name="IdIndicador">Id del Indicador</param>
        /// <returns>Indicador</returns>
        [HttpGet]
        [Route("/api/DescargaIndicadores/GetDetalleCategoria")]
        [ProducesResponseType(typeof(List<IndicadorResultado>), 200)]
        public ActionResult<IEnumerable<IndicadorResultado>> GetDetalleCategoria(int IdIndicador)
        {
            var lista = db.IndicadorResultado.Where(x => x.IdIndicador == IdIndicador && x.VariableDato == false).Select(x => new IndicadorResultado
            {
                idResultado = x.idResultado,
                IdIndicador = x.IdIndicador,
                NombreIndicador = x.NombreIndicador,
                EstadoIndicador = x.EstadoIndicador,
                CodigoIndicador = x.CodigoIndicador,
                IdSolicitud = x.IdSolicitud,
                NombreSolicitud = x.NombreSolicitud,
                CodigoSolicitud = x.CodigoSolicitud,
                NombreFuente = x.NombreFuente,
                IdFormulario = x.IdFormulario,
                CodigoFormulario = x.CodigoFormulario,
                NombreFormulario = x.NombreFormulario,
                idMes = x.idMes,
                Mes = x.Mes,
                idGrupo = x.idGrupo,
                NombreGrupo = x.NombreGrupo,
                IdClasificacion = x.IdClasificacion,
                NombreClasificacion = x.NombreClasificacion,
                IdAnno = x.IdAnno,
                Anno = x.Anno,
                NumeroFila = x.NumeroFila,
                IdColumna = x.IdColumna,
                NombreColumna = x.NombreColumna,
                ValorColumna = x.ValorColumna,
                VariableDato = x.VariableDato,
                AnnoMes = x.AnnoMes,
            }).ToList();
            return lista;
        }

    }
}