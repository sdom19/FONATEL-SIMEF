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
    public class BitacoraFonatelController : Controller
    {
        private readonly BitacoraBL BitacoraBL;

        public BitacoraFonatelController()
        {
            BitacoraBL = new BitacoraBL();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }




        #region Métodos de ASYNC
        /// <summary>
        /// Fecha 04-08-2022
        /// Michael Hernández Cordero
        /// Obtiene datos para la table de Bitacora INDEX
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public async Task<string> ObtenerListaBitacora(Bitacora bitacora)
        {
            RespuestaConsulta<List<Bitacora>> result = null;
            await Task.Run(() =>
            {
                result = BitacoraBL.ObtenerDatos(bitacora);
            });
            return JsonConvert.SerializeObject(result);
        }

        #endregion

    }
}
