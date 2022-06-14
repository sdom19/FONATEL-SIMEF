using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GB.SUTEL.UI.Controllers.Fonatel
{
    public class CategoriasDesagregacionController : Controller
    {
        // GET: CategoriasDesagregacion

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

        // GET: CategoriasDesagregacion/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Deatlle(int id)
        {
            return View();
        }






        // POST: CategoriasDesagregacion/Create
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

        // GET: CategoriasDesagregacion/Edit/5
        
        // POST: CategoriasDesagregacion/Edit/5
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

        // GET: CategoriasDesagregacion/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CategoriasDesagregacion/Delete/5
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
    }
}
