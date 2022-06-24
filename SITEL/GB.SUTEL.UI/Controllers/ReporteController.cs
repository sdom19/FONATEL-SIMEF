
using GB.SUTEL.DAL;
using GB.SUTEL.UI.Recursos.Utilidades;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GB.SUTEL.UI.Controllers
{
    public class ReporteController : Controller
    {
        Funcion func = new Funcion();
        //Conexión a la base de datos
        private readonly SUTEL_IndicadoresEntities db = new SUTEL_IndicadoresEntities();
        //
        // GET: /Reporte/
        LoginController Login = new LoginController();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _ReporteTipoIndicadorServicio()
        {
           
        
            string user = (string )Session["usuarioDominio"];
            if (Session["usuarioDominio"] == null || Session["usuarioDominio"] == "") {

                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;
                Session["usuarioDominio"] = null;
                authManager.SignOut("ApplicationCookie");
                return RedirectToAction("Index", "Home");

            }
            else
            {
                Session["idServicios"] = null;
                Session["idTipoIndicador"] = null;
                string us;
                us = User.Identity.GetUserId();
                try
                {
                    func._index(us, "Tipo de Indicador por Servicio", "Reportes Tipo de Indicador por Servicio");
                }
               
                catch (Exception e)
                {
                    Console.WriteLine("{0} Exception caught.", e);
                }
                return View();
            }
           
        }

        public ActionResult _ReporteIndicadorPorServicio()
        {
          
            string user = (string)Session["usuarioDominio"];
            if (Session["usuarioDominio"] == null || Session["usuarioDominio"] == "")
            {
                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;
                Session["usuarioDominio"] = null;
                authManager.SignOut("ApplicationCookie");
                return RedirectToAction("Index", "Home");

            }
            else
            {
                string us;
                us = User.Identity.GetUserId();
                try
                {
                    func._index(us, "Indicadores por Servicio", "Reportes Indicadores por Servicio");
                }
                
                catch (Exception e)
                {
                    Console.WriteLine("{0} Exception caught.", e);
                }
                return View();
            }
           
        }

        public ActionResult _ReporteIndicadorPorOperador()
        {
            string user = (string)Session["usuarioDominio"];
            if (Session["usuarioDominio"] == null || Session["usuarioDominio"] == "")
            {
                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;
                Session["usuarioDominio"] = null;
                authManager.SignOut("ApplicationCookie");
                return RedirectToAction("Index", "Home");

            }
            else
            {
                string us;
                us = User.Identity.GetUserId();
                try
                {
                    func._index(us, "Indicadores por Operador", "Reportes Indicadores por Operador");
                }
               
                catch (Exception e)
                {
                    Console.WriteLine("{0} Exception caught.", e);
                }
                return View();
            }
        }
        public ActionResult _ReporteDetalleAgrupacionPorAgrupacion()
        {
            string user = (string)Session["usuarioDominio"];
            if (Session["usuarioDominio"] == null || Session["usuarioDominio"] == "")
            {
                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;
                Session["usuarioDominio"] = null;
                authManager.SignOut("ApplicationCookie");
                return RedirectToAction("Index", "Home");

            }
            else
            {
                string us;
                us = User.Identity.GetUserId();
                try
                {
                    func._index(us, "Detalle Agrupación  por Agrupación", "Reporte Detalle Agrupación  por Agrupación");
                }
               
                catch (Exception e)
                {
                    Console.WriteLine("{0} Exception caught.", e);
                }
                return View();
            }
        }

        public ActionResult _ReporteBitacora()
        {
            string user = (string)Session["usuarioDominio"];
            if (Session["usuarioDominio"] == null || Session["usuarioDominio"] == "")
            {
                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;
                Session["usuarioDominio"] = null;
                authManager.SignOut("ApplicationCookie");
                return RedirectToAction("Index", "Home");
              
            }
            else
            {
                string us;
                us = User.Identity.GetUserId();
                try
                {
                    func._index(us, "Bitacora", "Reporte Bitácora");
                }
               
                catch (Exception e)
                {
                    Console.WriteLine("{0} Exception caught.", e);
                }
                return View();
            }
        }
	}
}