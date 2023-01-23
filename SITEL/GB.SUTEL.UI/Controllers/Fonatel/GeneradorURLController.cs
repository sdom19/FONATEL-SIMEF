using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace GB.SUTEL.UI.Controllers.Fonatel
{
    public class GeneradorURLController : Controller
    {
        // GET: GeneradorURL
        public ActionResult Index()
        {
            ViewBag.RutaAPISIGITEL = WebConfigurationManager.AppSettings["rutaAPISIGITEL"].ToString();
            return View();
        }
    }
}