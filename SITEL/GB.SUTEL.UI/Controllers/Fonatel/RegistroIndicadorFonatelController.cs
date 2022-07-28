using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GB.SUTEL.UI.Controllers.Fonatel
{
    public class RegistroIndicadorFonatelController  : Controller
    {
        // GET: RegistroIndicadorFonatel

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        // GET: RegistroIndicadorFonatel/Details/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create(int? id)
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
    }
}
