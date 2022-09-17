using GB.SIMEF.BL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SUTEL.UI.Controllers.Fonatel
{
    public class FormulaCalculoController : Controller
    {
        #region Variables Públicas del controller
        private readonly FormulasCalculoBL formulaBL;


        #endregion
        public FormulaCalculoController()
        {
            formulaBL = new FormulasCalculoBL(EtiquetasViewFormulasCalculo.Pantalla, System.Web.HttpContext.Current.User.Identity.GetUserId());
        }

        // GET: Solicitud
        public ActionResult Index()
        {




            return View();
        }

        // GET: Solicitud/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.ModoFormulario = ((int)Accion.Insertar).ToString();
            ViewBag.TituloVista = EtiquetasViewFormulasCalculo.TituloCrear;
            return View(new FormulasCalculo());
        }

        // POST: Solicitud/Create
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

        // GET: Solicitud/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.ModoFormulario = ((int)Accion.Editar).ToString();
            ViewBag.TituloVista = EtiquetasViewFormulasCalculo.TituloEditar;
            return View("Create");
        }

        // POST: Solicitud/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Solicitud/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Solicitud/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Solicitud/Clone/5
        public ActionResult Clone(int id)
        {
            ViewBag.ModoFormulario = ((int)Accion.Clonar).ToString();
            ViewBag.TituloVista = EtiquetasViewFormulasCalculo.TituloClonar;
            return View("Create");
        }
        
        // GET: Solicitud/View/5
        public ActionResult View(int id)
        {
            ViewBag.ModoFormulario = ((int)Accion.Consultar).ToString();
            ViewBag.TituloVista = EtiquetasViewFormulasCalculo.TituloVisualizar;
            return View("Create");
        }




        /// <summary>
        /// Fecha 04-08-2022
        /// Michael Hernández Cordero
        /// Obtiene datos para la table de categorías INDEX
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<string> ObtenerListaFormulas()
        {
            RespuestaConsulta<List<FormulasCalculo>> result = null;
            await Task.Run(() =>
            {
                result = formulaBL.ObtenerDatos(new FormulasCalculo());
            });

            return JsonConvert.SerializeObject(result);


        }
    }
}
