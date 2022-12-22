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

namespace GB.SIMEF.API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class DescargaIndicadoresController : ControllerBase
    {
              
        //private SIMEFContext db;
        private  DWHSIMEFContext db = new DWHSIMEFContext();
        

        /// <summary>
        /// Obtiene los indicadores filtrados por servicio y tipo
        /// </summary>
        /// <param name="programa">Servicio index</param>
        /// <param name="tipo">Nombre Tipo</param>
        /// <returns>Lista de indicadores</returns>
        [HttpGet]
        [Route("/api/DescargaIndicadores/GetGrupo")]
        [ProducesResponseType(typeof(List<DimGrupoIndicadores>), 200)]
        public ActionResult<IEnumerable<DimGrupoIndicadores>> GetGrupo()
        {
            var lista = db.DimGrupoIndicador.Where(x => x.Estado ==true).Select(x => new DimGrupoIndicadores
            {
                id = x.id,
                Nombre = x.Nombre,
                idGrupo = x.idGrupo,
                Estado=x.Estado,
                DetalleHtml = x.DetalleHtml
            }).ToList();

            //// para cada servicio ver si hay datos en  el cubo
            //foreach (var s in lista)
            //{
            //    var li = (from u in DB.JerarquiaIndicadorMercados
            //              join k in DB.Indicador on u.IdIndicador equals k.IdIndicador
            //              where u.IdServicio == s.IdServicio
            //              select k.CodIndicador).Distinct().ToList();
            //    int cnt = 0;
            //    // para cada servicio, ver cuantos indicadores tenemos visibles
            //    foreach (var lii in li)
            //    {
            //        cnt += pi.Where(x => x.IdIndicador == lii).Count();

            //    }
            //    // la cantidad corresponde al # de indicadores visibles.
            //    s.Cantidad = cnt;
            //}
            //// vemos si hay que aplicar algun cambio en codigos o nombres
            //var servicios_convertir = ConsultasMapeo.UnCampo_iis("select * from mapeoservicio");
            //foreach (var sc in servicios_convertir)
            //{
            //    // obtenemos las tematicas que hay. 
            //    var l = GetTematicas(sc.i1, "") as JsonResult;
            //    var l2 = l.Value as List<ModelTematica>;

            //    lista.RemoveAll(x => x.IdServicio == sc.i0);

            //    // si la cantidad de tematicas que tiene este servicio es 0, no lo incluimos
            //    if (l2.Count > 0)
            //    {
            //        lista.Add(new _IDC0
            //        {
            //            IdServicio = sc.i1,
            //            DesServicio = sc.s2,
            //            Cantidad = l2.Count,
            //        });
            //    }
            //}
            // filtrar lista de servicios segun si tiene indicador parametrizado.
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
        [ProducesResponseType(typeof(List<DimTipoIndicadores>), 200)]
        public ActionResult<IEnumerable<DimTipoIndicadores>> GetTipo()
        {
            var lista = db.DimTipoIndicadores.Where(x => x.Estado == true).Select(x => new DimTipoIndicadores
            {
                id = x.id,
                Nombre = x.Nombre,
                IdTipoIdicador = x.IdTipoIdicador,
                Estado = x.Estado
            }).ToList();
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
        [ProducesResponseType(typeof(List<ModelIndicador>), 200)]
        public ActionResult<IEnumerable<ModelIndicador>> GetIndicadores(int Grupo, int Tipo)
        {
            var lista = db.DimTablaIndicadores.Where(x => x.idGrupo == Grupo && x.IdTipoIndicador == Tipo).Select(x => new ModelIndicador
            {
                Id = x.idIndicador,
                IdIndicador = x.Codigo,
                DesIndicador = (x.Codigo.ToString() +' ' + x.Nombre)


            }).Distinct().ToList();
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
        [ProducesResponseType(typeof(List<DimDetalleIndicadorVariables>), 200)]
        public ActionResult<IEnumerable<DimDetalleIndicadorVariables>> GetVariableDato(int IdIndicador)
        {
            var lista = db.DimTablaIndicadores.Where(x => x.idIndicador == IdIndicador).Select(x => new DimDetalleIndicadorVariables
            {
                idIndicador = x.idIndicador,
                idDetalleIndicador = 0,
                NombreVariable =  x.NombreVariable


            }).ToList();
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
                     .Select(x => new DimDefinicionIndicador { IdIndicador = x.IdIndicador, Nombre = x.Nombre, Definicion = x.Definicion,IdTipoindicador = x.IdTipoindicador })
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
    }
}