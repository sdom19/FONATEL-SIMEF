using GB.SIMEF.BL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using GB.SUTEL.UI.Helpers;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace GB.SUTEL.UI.Controllers.Fonatel
{



    [AuthorizeUserAttribute]
    public class GeneradorURLController : Controller
    {
        #region Variables Globales del módulo

        private BitacoraBL clsDatos;
        #endregion


        public GeneradorURLController()
        {
            this.clsDatos = new BitacoraBL(EtiquetasViewIndicadorFonatel.TituloGeneradorUrl, System.Web.HttpContext.Current.User.Identity.GetUserId());
        }


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

        #region Métodos ASYN


        /// <summary>
        /// Fecha 21-04-2023
        /// Michael Hernández Cordero
        /// Registro Bitacora 
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public async Task<string> RegistarBitacora(List<string> oListaIndicador)
        {
            RespuestaConsulta<List<Bitacora>> result = null;
            await Task.Run(() =>
            {
                string indicadores =  Utilidades.GeneradorUrlJson(  string.Join(", ", oListaIndicador));
                result = clsDatos.InsertarDatos((int)Constantes.Accion.Crear, Constantes.ValorNoApilca, "", "", indicadores);
            });

            return JsonConvert.SerializeObject(result);
        }


        #endregion



    }
   
}