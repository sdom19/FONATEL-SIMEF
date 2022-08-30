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
    public class SolicitudFonatelController : Controller
    {

        #region Variables publicas


        private readonly SolicitudBL SolicitudesBL;



        string user;

        #endregion


        public SolicitudFonatelController()
        {
            SolicitudesBL = new SolicitudBL();

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

        // GET: Solicitud/Create
        public ActionResult Create(string id, int? modo)
        {

            Solicitud model = new Solicitud();


            if (!string.IsNullOrEmpty(id))
            {
                model = SolicitudesBL.ObtenerDatos(new Solicitud() {id=id })
                    .objetoRespuesta.Single();
                if (modo== (int)Constantes.Accion.Clonar)
                {
                    ViewBag.titulo = EtiquetasViewSolicitudes.Clonar;
                    model.Nombre = string.Empty;
                    model.Codigo = string.Empty;
                }
                else
                {
                    ViewBag.titulo = EtiquetasViewSolicitudes.Editar;
                }
            }
            else
            {
                ViewBag.titulo = EtiquetasViewSolicitudes.Crear;
            }
            return View(model);
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



        public async Task<string> ObtenerListaSolicitudes()
        {
            RespuestaConsulta<List<Solicitud>> result = null;
            await Task.Run(() =>
            {
                result = SolicitudesBL.ObtenerDatos(new Solicitud());
            });

            return JsonConvert.SerializeObject(result);


        }



    }
}
