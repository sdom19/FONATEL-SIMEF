using GB.SIMEF.BL;
using GB.SIMEF.Entities;
using GB.SUTEL.UI.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SUTEL.UI.Controllers.Fonatel
{
    [AuthorizeUserAttribute]
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
            var lista = BitacoraBL.ObtenerDatos( new Bitacora()).objetoRespuesta;

            var listaUsuario = lista.Select(x => x.Usuario).Distinct();
            var listaPantalla = lista.Select(x => x.Pantalla).Distinct();

            var ListaAcciones = lista.Select(x => x.Accion).Distinct();

            ViewBag.Pantalla = listaPantalla.Select(x => new SelectListItem() { Selected = false, Value = x, Text = x }).ToList();
            ViewBag.Usuario = listaUsuario.Select(x => new SelectListItem() { Selected = false, Value = x, Text = x }).ToList();

            ViewBag.Accion = ListaAcciones.Select(x => new SelectListItem() { Selected = false, Value = x.ToString(), Text = Enum.GetName(typeof(Accion), x) }).ToList();

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
