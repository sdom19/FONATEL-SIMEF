using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;


using SIMEF.API.Controllers;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using SIMEF.API.Models;

namespace GB.SIMEF.API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class CatalogoIndicadoresController : ControllerBase
    {
        //private SIMEFContext db;
        private readonly SIGITELContext db;
        
        public CatalogoIndicadoresController(SIGITELContext _contexto)
        {
            db = _contexto;
        }
        

        

        /// <summary>
        /// Obtiene los indicadores filtrados por servicio y tipo
        /// </summary>
        /// <param name="programa">Servicio index</param>
        /// <param name="tipo">Nombre Tipo</param>
        /// <returns>Lista de indicadores</returns>
        [HttpGet]
        [Route("/api/CatalogoIndicadores/GetDefinicionXprograma")]
        [ProducesResponseType(typeof(List<IndicadorViewModel>), 200)]
        public ActionResult<IEnumerable<IndicadorViewModel>> GetDefinicionXprograma(string programa, int tipo)
        {
            //Filtro por servicio
            var indicadores = ObtenerDefinicionXGrupo(Convert.ToInt32(programa));
            //Filtro por tipo
            var indicadoresView = FiltrarDefinicionXTipo(indicadores,tipo);
            //Devuelve lista
            return indicadoresView;
        }
        /// <summary>
        /// Obtiene un indicador por id
        /// </summary>
        /// <param name="indicador">Id del Indicador</param>
        /// <returns>Indicador</returns>
        [HttpGet]
        [Route("/api/CatalogoIndicadores/GetIndicador")]
        [ProducesResponseType(typeof(IndicadorViewModel), 200)]
        public ActionResult<IndicadorViewModel> GetIndicador(int Indicador)
        {
            IndicadorViewModel indicadorViewModel = null;
            if (Indicador.ToString() != null)
            {
                //Busca en BD el indicador
                indicadorViewModel = db.DefinicionIndicador
                    .Where(x => x.IdIndicador == Indicador)
                    .Select(x => new IndicadorViewModel { IdIndicador = x.IdIndicador.ToString(), NombreIndicador = x.Nombre, DefinicionIndicador = x.Fuente })
                    .FirstOrDefault();
                //Añade definición alterna
                if (indicadorViewModel != null)
                    if (indicadorViewModel.DefinicionIndicador == null)
                        indicadorViewModel.DefinicionIndicador = "Actualmente no tiene definición";
            }
            //Retorna indicador
            return indicadorViewModel;
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
                     .Select(x => new DefinicionIndicador { IdIndicador = x.IdIndicador, Nombre = x.Nombre, Definicion = x.Definicion,IdTipoIndicador = x.IdTipoIndicador })
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
        /// Método para filtrar indicadores por grupo
        /// </summary>
        /// <param name="IdGrupo">Código de Grupo</param>
        /// <returns>Lista Indicadores</returns>
        internal List<DefinicionIndicador> ObtenerDefinicionXGrupo(int IdGrupo)
        {
            var indicadores = new List<DefinicionIndicador>();

                    indicadores = db.DefinicionIndicador
                    .Where(x => (x.IdGrupoIndicador == IdGrupo))
                    .Select(x => new DefinicionIndicador { IdIndicador = x.IdIndicador, Nombre = x.Nombre, Definicion = x.Definicion, IdTipoIndicador = x.IdTipoIndicador })
                    .ToList();
                    
            //Retorna lista de indicadores
            return indicadores;
        }

        /// <summary>
        /// Método para filtrar Definiciones por tipo
        /// </summary>
        /// <param name="indicadores">Lista de Indicadores</param>
        /// <param name="tipo">Tipo</param>
        /// <returns>Lista de indicadores</returns>
        internal List<IndicadorViewModel> FiltrarDefinicionXTipo(List<DefinicionIndicador> indicadores, int tipo)
        {
            var indicadoresViewModel = new List<IndicadorViewModel>();

            indicadoresViewModel = indicadores.Where(x => x.IdTipoIndicador == tipo)
                       .Select(x => new IndicadorViewModel { IdIndicador = x.IdIndicador.ToString(), NombreIndicador = x.Nombre, DefinicionIndicador = x.Definicion })
                       .ToList();

            //Retorna lista de indicadores
            return indicadoresViewModel;
        }

        /// <summary>
        /// Obtiene los grupos
        /// </summary>
        [HttpGet]
        [Route("/api/CatalogoIndicadores/GetGrupo")]
        [ProducesResponseType(typeof(List<GrupoIndicador>), 200)]
        public ActionResult<IEnumerable<GrupoIndicador>> GetGrupo()
        {
            var lista = db.GrupoIndicador.Where(x => x.Estado == true).Select(x => new GrupoIndicador
            {
                id = x.id,
                Nombre = x.Nombre,
                idGrupoIndicador = x.idGrupoIndicador,
                Estado = x.Estado,
                DetalleHtml = x.DetalleHtml
            }).ToList();

            return lista;
        }

        /// <summary>
        /// Obtiene los tipos
        /// </summary>
        [HttpGet]
        [Route("/api/CatalogoIndicadores/GetTipo")]
        [ProducesResponseType(typeof(List<TipoIndicador>), 200)]
        public ActionResult<IEnumerable<TipoIndicador>> GetTipo()
        {
            var lista = db.TipoIndicador.Where(x => x.Estado == true).Select(x => new TipoIndicador
            {
                id = x.id,
                Nombre = x.Nombre,
                IdTipoIndicador = x.IdTipoIndicador,
                Estado = x.Estado
            }).ToList();
            return lista;
        }
    }
}