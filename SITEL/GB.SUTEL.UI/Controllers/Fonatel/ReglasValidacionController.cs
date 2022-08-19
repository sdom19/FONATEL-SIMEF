using GB.SIMEF.BL;
using GB.SIMEF.Entities;
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

        string user;

        #endregion

        public ReglasValidacionController()
        {
            reglaBL = new ReglaValidacionBL();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        // GET: CategoriasDesagregacion/Details/5
        [HttpGet]
        public ActionResult Detalle(int id)
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
