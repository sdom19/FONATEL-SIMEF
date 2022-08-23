using GB.SIMEF.BL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GB.SUTEL.UI.Controllers.Fonatel
{
    public class ReglasValidacionController : Controller
    {


        #region Variables Públicas del controller
        private readonly ReglaValidacionBL reglaBL;
        private readonly IndicadorFonatelBL indicadorfonatelBL;
        string user;

        #endregion

        public ReglasValidacionController()
        {
            reglaBL = new ReglaValidacionBL();
            indicadorfonatelBL = new IndicadorFonatelBL("","");
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Detalle(string idregla)
        {
            if (string.IsNullOrEmpty(idregla))
            {
                return View("Index");
            }
            else
            {
                ReglaValidacion objregla = new ReglaValidacion();
                if (!string.IsNullOrEmpty(idregla))
                {
                    objregla.id = idregla;
                    objregla = reglaBL.ObtenerDatos(objregla).objetoRespuesta.SingleOrDefault();
                }
                return View(objregla);
            }

        }


        [HttpGet]
        public ActionResult Create(string id, int? modo)
        {
            ViewBag.indicador = indicadorfonatelBL.ObtenerDatos(new Indicador() { })
                .objetoRespuesta;
            
            ViewBag.Modo = modo.ToString();

            ReglaValidacion objregla = new ReglaValidacion();
            if (!string.IsNullOrEmpty(id))
            {
                objregla.id = id;
                objregla = reglaBL.ObtenerDatos(objregla).objetoRespuesta.SingleOrDefault();
                if (modo == (int)Constantes.Accion.Clonar)
                {
                    objregla.Codigo = string.Empty;
                    objregla.id = string.Empty;
                }

            }
            return View(objregla);

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



        #region Metodos Async


        /// <summary>
        /// Fecha 17-08-2022
        /// Michael Hernández Cordero
        /// Obtiene datos para la table reglas de validación
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerListaReglasValidacion()
        {
            RespuestaConsulta<List<ReglaValidacion>> result = null;
            await Task.Run(() =>
            {
                result = reglaBL.ObtenerDatos(new ReglaValidacion());

            });
            return JsonConvert.SerializeObject(result);
        }
        #endregion

    }
}
