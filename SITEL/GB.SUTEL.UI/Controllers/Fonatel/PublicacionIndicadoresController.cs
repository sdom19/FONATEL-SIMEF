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

namespace GB.SUTEL.UI.Controllers.Fonatel
{
    public class PublicacionIndicadoresController : Controller
    {

        private readonly IndicadorFonatelBL indicadorfonatelBL;

        // GET: CategoriasDesagregacion


        public PublicacionIndicadoresController()
        {
            indicadorfonatelBL = new IndicadorFonatelBL(EtiquetasViewPublicaciones.PublicacionIndicadores, System.Web.HttpContext.Current.User.Identity.GetUserId());
        }


        [HttpGet]
        public ActionResult Index()
        {
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
                result = indicadorfonatelBL.ObtenerDatos(new Indicador() { idEstado = (int)Constantes.EstadosRegistro.Activo });

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
        public async Task<string> CambiarEstadoSigitel(Indicador indicador)
        {
            RespuestaConsulta<List<Indicador>> result = null;
            await Task.Run(() =>
            {
                int temp = 0;
                int.TryParse(Utilidades.Desencriptar(indicador.id), out temp);
                indicador.idIndicador = temp;
                return indicadorfonatelBL.ObtenerDatos(indicador);

            }).ContinueWith(data =>
             {
                Indicador objetoActualizar = data.Result.objetoRespuesta.Single();
                objetoActualizar.VisualizaSigitel = indicador.VisualizaSigitel;
                result = indicadorfonatelBL.PublicacionSigitel(objetoActualizar);
             }
            );
            return JsonConvert.SerializeObject(result);
        }

        #endregion

    }
}
