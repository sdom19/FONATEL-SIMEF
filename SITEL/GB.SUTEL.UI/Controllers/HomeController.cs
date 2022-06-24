using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GB.SUTEL.UI.Helpers;
using GB.SUTEL.BL.Mantenimientos;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.UI.Models;
using GB.SUTEL.ExceptionHandler;
using Microsoft.AspNet.Identity;
using GB.SUTEL.UI.Recursos.Utilidades;

namespace GB.SUTEL.UI.Controllers
{    
    public class HomeController : BaseController
    {
        Funcion func = new Funcion();
        //NivelBL nivelBL = new NivelBL();        
        [AuthorizeUserAttribute]
        public ActionResult Index()
        {
            // this is for you to test the logger!!!
            //try
            //{
            //    int zero = 0;
            //    var result = 1000 / zero;
            //}catch(Exception ex)
            //{
            //    AppContext.ExceptionBuilder.BuildException(ex.Message, ex);
            //}


            string user;
            user = User.Identity.GetUserId();
            try
            {
                func._index(user, "Inicio de sesion", "Inicio de sesion");
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }
            return View();
        }
        [HttpGet]
        public ActionResult IndexPost()
        {
            NivelBL nivelBL = new NivelBL(AppContext);
            return PartialView("_Datos", nivelBL.gListar().objObjeto);
            //return View();
        }
        [HttpPost]
        public ActionResult IndexPost(string searchTerm)
        {
            //System.Threading.Thread.Sleep(4000);
            /// EL FILTRO SE HACE EN DAL
            /// 
            NivelBL nivelBL = new NivelBL(AppContext);
            return PartialView("_Datos", nivelBL.gListar().objObjeto.Where(x => x.DescNivel.ToLower() == (searchTerm.ToLower() == "" ? x.DescNivel.ToLower() : searchTerm.ToLower())));
            //return "TODO OK";
        }
        [HttpPost]

        public ActionResult SetCulture(string culture)
        {
            // Validate input
            culture = CultureHelper.GetImplementedCulture(culture);
            // Save culture in a cookie
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = culture;   // update cookie value
            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = culture;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return RedirectToAction("Index");
        }
        [HttpPost]

        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
        public ActionResult LogIn()
        {
            Session.Clear();
            
            return RedirectToAction("Index","Login");
        }
    }
}
