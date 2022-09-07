using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GB.SIMEF.BL;
using GB.SIMEF.Entities;

using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using GB.SIMEF.Resources;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GB.SUTEL.UI.Controllers.Fonatel
{
    public class HistoricoFonatelController : Controller
    {

        private readonly DatosHistoricosBL historicoBl;

        public HistoricoFonatelController()
        {
            historicoBl = new DatosHistoricosBL("Historico", System.Web.HttpContext.Current.User.Identity.GetUserId());
        }



        // GET: Solicitud
        public ActionResult Index()
        {


            ViewBag.ListadoDatosHistoricos =
                historicoBl.ObtenerDatos(new DatoHistorico()).objetoRespuesta
                .Select(x => new SelectListItem() { Selected = false, Value = x.id, Text = Utilidades.ConcatenadoCombos(x.Codigo, x.NombrePrograma)}).ToList();

            return View();
        }

        [HttpGet]
        public ActionResult Detalle(string  id)
        {
       
            return View();
        }

        // GET: Solicitud/Create
        public ActionResult Create()
        {
            return View();
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
            return View();
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



        /// <summary>
        /// Fecha 04-08-2022
        /// Michael Hernández Cordero
        /// Obtiene datos para la table de categorías INDEX
        /// </summary>
        /// <returns></returns>

        [HttpPost ]
        public async Task<string> ObtenerListaHistorica(DatoHistorico datoHistorico)
        {

            RespuestaConsulta<List<DatoHistorico>> result = null;
            await Task.Run(() =>
            {
                result = historicoBl.ObtenerDatos(datoHistorico);
            });

            return JsonConvert.SerializeObject(result);
        }




    }
}
