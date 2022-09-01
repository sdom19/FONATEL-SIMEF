using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using System.Threading.Tasks;
using GB.SIMEF.BL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using GB.SUTEL.UI.Helpers;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;



namespace GB.SUTEL.UI.Controllers.Fonatel
{
    public class DefinicionIndicadoresController : Controller
    {


        private readonly DefinicionIndicadorBL definicionBL;

        public DefinicionIndicadoresController()
        {
            definicionBL = new DefinicionIndicadorBL(EtiquetasViewDefinicionIndicadores.TituloIndex, System.Web.HttpContext.Current.User.Identity.GetUserId());
        }

        // GET: CategoriasDesagregacion

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        // GET: CategoriasDesagregacion/Details/5
        [HttpGet]
        public ActionResult Detalle(string id)
        {
            int temp;
            int.TryParse(id, out temp);
            var model = definicionBL.ObtenerDatos(new DefinicionIndicador() { idDefinicion = temp }).objetoRespuesta.Single();
            return View(model);
        }



        [HttpGet]
        public ActionResult Clonar(string id)
        {
            int temp;
            int.TryParse(Utilidades.Desencriptar(id), out temp);
            var model = definicionBL.ObtenerDatos(new DefinicionIndicador() { idIndicador = temp }).objetoRespuesta.Single();
            return View(model);
        }



        [HttpGet]
        public ActionResult Create(string id, int? modo)
        {
            if (modo==(int)Constantes.Accion.Editar)
            {
                ViewBag.Titulo = EtiquetasViewDefinicionIndicadores.Editar;
            }
            else
            {
                ViewBag.Titulo = EtiquetasViewDefinicionIndicadores.Crear;
            }
            int temp;
            int.TryParse(Utilidades.Desencriptar( id), out temp);
            ViewBag.Modo = modo.ToString();
            var model = definicionBL.ObtenerDatos(new DefinicionIndicador() {idIndicador=temp }).objetoRespuesta.Single();
            return View(model);
        }
   





        #region Métodos de ASYNC Categoria


        /// <summary>
        /// Fecha 04-08-2022
        /// Michael Hernández Cordero
        /// Obtiene datos para la table de categorías INDEX
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<string> ObtenerListaDefiniciones()
        {
            RespuestaConsulta<List<DefinicionIndicador>> result = null;
            await Task.Run(() =>
            {
                result = definicionBL.ObtenerDatos(new DefinicionIndicador() );
            });

            return JsonConvert.SerializeObject(result);


        }


        [HttpPost]
        public async Task<string> EliminarDefinicion(DefinicionIndicador definicion)
        {
            RespuestaConsulta<List<DefinicionIndicador>> result = null;
            await Task.Run(() =>
            {
                result = definicionBL.EliminarElemento(definicion);
            });

            return JsonConvert.SerializeObject(result);


        }


        
        #endregion


    }
}
