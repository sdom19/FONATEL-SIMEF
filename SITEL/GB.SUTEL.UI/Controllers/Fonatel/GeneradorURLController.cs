using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

            var consultasFonatel = false;
            var roles = ((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault().Value.Split(',');
            if (roles.Contains(Constantes.RolConsultasFonatel))
            {
                consultasFonatel = true;
            }
            ViewBag.ConsultasFonatel = consultasFonatel;


            return View();
        }
    }
}