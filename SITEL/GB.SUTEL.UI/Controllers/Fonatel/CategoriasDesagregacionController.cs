using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GB.SIMEF.BL;
using GB.SIMEF.Entities;
using GB.SUTEL.UI.Helpers;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace GB.SUTEL.UI.Controllers.Fonatel
{
    //[AuthorizeUserAttribute]
    public class CategoriasDesagregacionController : Controller
    {

        #region Variables Públicas del controller
        private readonly CategoriasDesagregacionBL categoriaBL;

        private readonly DetalleCategoriasTextoBL categoriaDetalleBL;

        private readonly TipoCategoriaBL TipoCategoriaBL;

        private readonly TipoDetalleCategoriaBL TipoDetalleCategoriaBL;

        string user;

        #endregion


        public CategoriasDesagregacionController()
        {
            categoriaBL = new CategoriasDesagregacionBL();
            categoriaDetalleBL = new DetalleCategoriasTextoBL();
            TipoCategoriaBL = new TipoCategoriaBL();
            TipoDetalleCategoriaBL = new TipoDetalleCategoriaBL();

        }

        #region Eventos de la Página
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        // GET: CategoriasDesagregacion/Details/5
        [HttpGet]
        public ActionResult Detalle()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create(string id)
        {
            ViewBag.TipoCategoria = TipoCategoriaBL.ObtenerDatos(new TipoCategoria() { })
                .objetoRespuesta;
            ViewBag.TipoDetalleCategoria = TipoDetalleCategoriaBL.ObtenerDatos(new TipoDetalleCategoria() { })
               .objetoRespuesta;
            CategoriasDesagregacion objCategoria = new CategoriasDesagregacion();
            if (!string.IsNullOrEmpty(id))
            {
                objCategoria.id = id;
                objCategoria = categoriaBL.ObtenerDatos(objCategoria).objetoRespuesta.SingleOrDefault();
            }
            return View(objCategoria);
           
        }

        [HttpPost]
        public ActionResult Create(CategoriasDesagregacion obCategoria)
        {
           
            return View();

        }



        #endregion

        #region Métodos de ASYNC
        /// <summary>
        /// Fecha 04-08-2022
        /// Michael Hernández Cordero
        /// Obtiene datos para la table de categorías INDEX
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<string> ObtenerListaCategorias()
        {
            RespuestaConsulta<List<CategoriasDesagregacion>> result = null;
            await Task.Run(() =>
            {
               result = categoriaBL.ObtenerDatos(new CategoriasDesagregacion());
            });
         
            return JsonConvert.SerializeObject(result);
        }
        /// <summary>
        /// Fecha 04-08-2022
        /// Michael Hernández Cordero
        /// Obtiene datos para la table de categorías Detalle Detalle
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerListaCategoriasDetalle(int idCategoria)
        {
            RespuestaConsulta<List<DetalleCategoriaTexto>> result = null;
            await Task.Run(() =>
            {
                result = categoriaDetalleBL.ObtenerDatos(new DetalleCategoriaTexto() { idCategoria = idCategoria });

            });
            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public async Task<string> EliminarCategoriasDetalle(string idDetalleCategoria)
        {
            user = User.Identity.GetUserId();
            RespuestaConsulta<List<DetalleCategoriaTexto>> result = null;
            await Task.Run(() =>
            {
                result = categoriaDetalleBL.EliminarElemento(new DetalleCategoriaTexto() { 
                    id=idDetalleCategoria,
                    usuario = user
                });

            });
            return JsonConvert.SerializeObject(result);
        }


        [HttpGet]
        public async Task<string> ObtenerCategoriasDetalle(int idCategoriaDetalle)
        {
            RespuestaConsulta<List<DetalleCategoriaTexto>> result = null;
            await Task.Run(() =>
            {
                result = categoriaDetalleBL.ObtenerDatos(new DetalleCategoriaTexto() 
                { idCategoriaDetalle=idCategoriaDetalle});

            });
            return JsonConvert.SerializeObject(result);
        }




        #endregion

    }
}
