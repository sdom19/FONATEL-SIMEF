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
    public class FormularioWebController : Controller
    {


      


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
        public ActionResult Create(int? id)
        {
            return View();
        }


    }
}
