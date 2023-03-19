using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GB.SIMEF.BL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using GB.SUTEL.UI.Helpers;
using GB.SUTEL.UI.Filters;
using System.Security.Claims;

namespace GB.SUTEL.UI.Controllers.Fonatel
{
    [AuthorizeUserAttribute]
    public class PublicacionIndicadoresController : Controller
    {

        private readonly IndicadorFonatelBL indicadorfonatelBL;
        private readonly DefinicionIndicadorBL definicionBL;

        // GET: CategoriasDesagregacion


        public PublicacionIndicadoresController()
        {
            indicadorfonatelBL = new IndicadorFonatelBL(EtiquetasViewPublicaciones.PublicacionIndicadores, System.Web.HttpContext.Current.User.Identity.GetUserId());
            definicionBL = new DefinicionIndicadorBL(EtiquetasViewDefinicionIndicadores.TituloIndex, System.Web.HttpContext.Current.User.Identity.GetUserId());
        }


        [HttpGet]
        public ActionResult Index()
        {
            var roles = ((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault().Value.Split(',');
            ViewBag.ConsultasFonatel = roles.Contains(Constantes.RolConsultasFonatel).ToString().ToLower();
            return View();

        }




        #region Metodos Async


        /// <summary>
        /// Fecha 17-08-2022
        /// Michael Hernández Cordero
        /// Obtiene datos para la table reglas de Publicados
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerListaIndicadores()
        {
            RespuestaConsulta<List<Indicador>> result = null;
            await Task.Run(() =>
            {
                var def = definicionBL.ObtenerDatos(new DefinicionIndicador() { IdEstadoRegistro = (int)Constantes.EstadosRegistro.Activo }).objetoRespuesta;
                result = indicadorfonatelBL.ObtenerDatos(new Indicador() { IdEstadoRegistro = (int)Constantes.EstadosRegistro.Activo });
                result.objetoRespuesta = result
                    .objetoRespuesta.Where(x => x.IdClasificacionIndicador != (int)Constantes.ClasificacionIndicadorEnum.Entrada && x.Interno==false).ToList();
                for (var i = 0; i < result.objetoRespuesta.Count(); i++)
                {
                    result.objetoRespuesta[i].tieneDefinicion = (def.Where(d => d.idDefinicionIndicador == result.objetoRespuesta[i].IdIndicador).Count() > 0);
                }
            });
            return JsonConvert.SerializeObject(result);
        }




        /// <summary>
        /// Fecha 17-08-2022
        /// Michael Hernández Cordero
        /// Cambiar Estado de Publicado
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ConsultasFonatelFilter]
        public async Task<string> CambiarEstadoSigitel(Indicador indicador)
        {
            RespuestaConsulta<List<Indicador>> result = null;
            await Task.Run(() =>
            {
                int temp = 0;
                int.TryParse(Utilidades.Desencriptar(indicador.id), out temp);
                indicador.IdIndicador = temp;
                return indicadorfonatelBL.ObtenerDatos(indicador);

            }).ContinueWith(data =>
            {
                Indicador objetoActualizar = data.Result.objetoRespuesta.Single();
                objetoActualizar.VisualizaSigitel = indicador.VisualizaSigitel;
                result = indicadorfonatelBL.PublicacionSigitel(objetoActualizar);
            });
            return JsonConvert.SerializeObject(result);
        }

        #endregion

    }
}
