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
            var SqlQuery = "execute pa_ObtenerGrupo";

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
            var SqlQuery = "execute pa_ObtenerTipo";

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
            var SqlQuery = "execute pa_ObtenerIndicador @idtipoIndicador,@idGrupo";

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
            var SqlQuery = "execute pa_ObtenerVariable @idIndicador";

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
            var SqlQuery = "execute pa_ObtenerCategoria @idIndicador";

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
            var SqlQuery = "execute pa_ObtenerAnno @idIndicador";

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
            var SqlQuery = "execute pa_ObtenerMes @idIndicador";

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
        internal List<DefinicionIndicador> FiltrarPrograma(string Programa)
        {
            var indicadores = new List<DefinicionIndicador>();
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
                    indicadores = db.DefinicionIndicador
                    .Where(x => (x.IdGrupoIndicador == IdGrupo))
                    .Select(x => new DefinicionIndicador { IdIndicador = x.IdIndicador, Nombre = x.Nombre, Definicion = x.Definicion, IdTipoIndicador = x.IdTipoIndicador })
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
        internal List<IndicadorViewModel> FiltrarTipo(List<DefinicionIndicador> indicadores, string tipo)
        {
            var indicadoresViewModel = new List<IndicadorViewModel>();
            switch (tipo)
            {

                case "Gestion":
                    //Tipo Suscripción
                    indicadoresViewModel = indicadores.Where(x => x.IdTipoIndicador == 2)
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
        /// <param name="variable">Id del variable</param>
        /// <param name="desde">Fecha desde</param>
        /// <param name="hasta">Fecha hasta</param>
        /// <param name="idCategoria">categoria</param>
        /// <returns>Indicador</returns>
        [HttpGet]
        [Route("/api/DescargaIndicadores/GetResultado")]
        [ProducesResponseType(typeof(InformeResultadoIndicadorSalida), 200)]
        public ActionResult<InformeResultadoIndicadorSalida> GetResultado(int IdIndicador, string variable, string desde, string hasta, int idCategoria)
        {
            var SqlQuery = "execute pa_ObtenerResultadoIndicador @pi_Desde, @pi_Hasta, @pi_Variable, @pi_IdCategoria, @pi_IdIndicador";

            List<InformeResultadoIndicador> lista = null;

            using (var connection = new SqlConnection(Connection.SIGITELDatabase))
            {
                lista = connection.Query<InformeResultadoIndicador>(SqlQuery, new { pi_Desde = desde, pi_Hasta = hasta, pi_Variable = variable, pi_IdCategoria = idCategoria, pi_IdIndicador = IdIndicador }).ToList();
            }
            
            InformeResultadoIndicadorSalida salida = new InformeResultadoIndicadorSalida();
                
            if (lista != null) 
            {
                salida.Encabezados = lista.GroupBy(g => g.AnnoMes).Select(encabezado => encabezado.Key).ToList();
                var elementos = lista.Where(c => c.Categoria != null && c.Categoria != "" ).GroupBy(g => g.Categoria).Select(categoria => categoria.Key).ToList();
                salida.Datos = new List<InformeResultadoDatos>();

                if (elementos.Count > 0)
                {
                    foreach (var categoria in elementos)
                    {
                        InformeResultadoDatos informeResultadoDatos = new InformeResultadoDatos();
                        informeResultadoDatos.Categoria = categoria;
                        informeResultadoDatos.Variable = lista.Where(v => v.Categoria == categoria).Select(v => v.Variable).First();
                        informeResultadoDatos.Valores = new Dictionary<string, double>();

                        var query = lista.Where(v => v.Categoria == categoria).Select(v => new { v.AnnoMes, v.Total});

                        foreach (var item in query)
                        {
                            informeResultadoDatos.Valores.Add(item.AnnoMes,item.Total);
                        }

                        salida.Datos.Add(informeResultadoDatos);
                    }
                }
                else
                {
                    elementos = lista.GroupBy(g => g.Variable).Select(variable => variable.Key).ToList();

                    foreach (var variableCategoria in elementos)
                    {
                        InformeResultadoDatos informeResultadoDatos = new InformeResultadoDatos();
                        informeResultadoDatos.Categoria = null;
                        informeResultadoDatos.Variable = variableCategoria;
                        informeResultadoDatos.Valores = new Dictionary<string, double>();

                        var query = lista.Where(v => v.Variable == variableCategoria).Select(v => new { v.AnnoMes, v.Total });

                        foreach (var item in query)
                        {
                            informeResultadoDatos.Valores.Add(item.AnnoMes, item.Total);
                        }

                        salida.Datos.Add(informeResultadoDatos);
                    }
                }
                

                salida.Totales = new Dictionary<string, double>();
                foreach (var encabezado in salida.Encabezados)
                {
                    double total = lista.Where(e => e.AnnoMes == encabezado).Sum(e => e.Total);
                    salida.Totales.Add(encabezado, total);
                }
            }

            return salida;
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
                idIndicadorResultado = x.idIndicadorResultado,
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
                idGrupoIndicador = x.idGrupoIndicador,
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

        /// <summary>
        /// Obtiene la definicion del indicador seleccionada
        /// </summary>
        [HttpGet]
        [Route("/api/DescargaIndicadores/GetDefinicion")]
        [ProducesResponseType(typeof(DefinicionIndicador), 200)]
        public ActionResult<DefinicionIndicador> GetDefinicion(int tipo,int grupo,int indicador)
        {
            DefinicionIndicador definicion = db.DefinicionIndicador.Where(x => x.IdTipoIndicador == tipo &&
            x.IdGrupoIndicador == grupo && x.IdIndicador == indicador).Select(x => new DefinicionIndicador
            {
              
                Nombre = x.Nombre,
                Fuente=x.Fuente,
                Nota=x.Nota,
            }).FirstOrDefault();
            return definicion;
        }

    }
}