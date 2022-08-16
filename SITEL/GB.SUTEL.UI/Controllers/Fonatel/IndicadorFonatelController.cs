using GB.SIMEF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using GB.SIMEF.BL;
using Microsoft.AspNet.Identity;
using GB.SIMEF.Resources;

namespace GB.SUTEL.UI.Controllers.Fonatel
{
    public class IndicadorFonatelController : Controller
    {
        private readonly IndicadorFonatelBL indicadorBL;

        public IndicadorFonatelController()
        {
            indicadorBL = new IndicadorFonatelBL(
                EtiquetasViewIndicadorFonatel.TituloIndex, 
                System.Web.HttpContext.Current.User.Identity.GetUserId()
                );
        }

        #region Eventos de la página

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Detalle(int id)
        {
            return View();
        }

        [HttpGet]
        public ActionResult DetalleVariables(int id)
        {
            return View();
        }


        public ActionResult DetalleCategoria(int id)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region Métodos de async

        /// <summary>
        /// 10/08/2022
        /// José Navarro Acuña
        /// Método que retorna todos los indicadores registrados en el sistema
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerListaIndicadores()
        {
            RespuestaConsulta<List<Indicador>> result = null;
            await Task.Run(() =>
            {
                result = indicadorBL.ObtenerDatos(new Indicador());
            });
            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// 10/08/2022
        /// José Navarro Acuña
        /// Método que elimina un indicador
        /// </summary>
        /// <param name="pIdIndicador"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> EliminarIndicador(string pIdIndicador)
        {
            RespuestaConsulta<List<Indicador>> result = null;
            await Task.Run(() =>
            {
                result = indicadorBL.EliminarElemento(new Indicador()
                {
                    id = pIdIndicador
                });

            });
            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// 16/08/2022
        /// José Navarro Acuña
        /// Método que verifica si el indicador se encuentra en algún formulario web
        /// </summary>
        /// <param name="pIdIndicador"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> VerificarIndicadorEnFormularioWeb(string pIdIndicador)
        {
            RespuestaConsulta<List<FormularioWeb>> result = null;
            await Task.Run(() =>
            {
                result = indicadorBL.ObtenerFormulariosWebSegunIndicador(new Indicador()
                {
                    id = pIdIndicador
                });

            });

            result.objetoRespuesta = null; // datos innecesarios, de momento

            return JsonConvert.SerializeObject(result);
        }
        #endregion
    }
}
