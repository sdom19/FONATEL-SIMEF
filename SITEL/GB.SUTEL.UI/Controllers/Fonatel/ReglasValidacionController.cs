using GB.SIMEF.BL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GB.SUTEL.UI.Controllers.Fonatel
{
    public class ReglasValidacionController : Controller
    {


        #region Variables Públicas del controller
        private readonly ReglaValidacionBL reglaBL;

        private readonly CategoriasDesagregacionBL categoriasDesagregacionBL;
        private readonly IndicadorFonatelBL indicadorfonatelBL;
        private readonly TipoReglaValidacionBL TipoReglasBL;
        private readonly OperadorArismeticoBL OperadoresBL;


        string user;

        #endregion

        public ReglasValidacionController()
        {
            reglaBL = new ReglaValidacionBL();
            indicadorfonatelBL = new IndicadorFonatelBL(EtiquetasViewPublicaciones.PublicacionIndicadores, System.Web.HttpContext.Current.User.Identity.GetUserId());
            categoriasDesagregacionBL = new CategoriasDesagregacionBL(EtiquetasViewReglasValidacion.ReglasValidacion, System.Web.HttpContext.Current.User.Identity.GetUserId());
            TipoReglasBL = new TipoReglaValidacionBL();
            OperadoresBL = new OperadorArismeticoBL();
    
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Detalle(string idregla)
        {
            if (string.IsNullOrEmpty(idregla))
            {
                return View("Index");
            }
            else
            {
                ReglaValidacion objregla = new ReglaValidacion();
                if (!string.IsNullOrEmpty(idregla))
                {
                    objregla.id = idregla;
                    objregla = reglaBL.ObtenerDatos(objregla).objetoRespuesta.SingleOrDefault();
                }
                return View(objregla);
            }

        }


        [HttpGet]
        public ActionResult Create(string id, int? modo)
        {
            var ListadoIndicador = indicadorfonatelBL
                .ObtenerDatos(new Indicador() { idEstado = (int)Constantes.EstadosRegistro.Activo }).objetoRespuesta;

            var listadoCategoria = categoriasDesagregacionBL
               .ObtenerDatos(new CategoriasDesagregacion() {idEstado=(int)Constantes.EstadosRegistro.Activo }).objetoRespuesta;
                

            ViewBag.ListaCategoria=listadoCategoria
                .Where(x => x.IdTipoCategoria != (int)Constantes.TipoCategoriaEnum.Actualizable)
                .Select(x => new SelectListItem() { Selected = false, Value = x.idCategoria.ToString(), Text = Utilidades.ConcatenadoCombos(x.Codigo, x.NombreCategoria) }).ToList();

            ViewBag.ListaCategoriaActualizable = listadoCategoria
                .Where(x => x.IdTipoCategoria == (int)Constantes.TipoCategoriaEnum.Actualizable)
                .Select(x => new SelectListItem() { Selected = false, Value = x.idCategoria.ToString(), Text = Utilidades.ConcatenadoCombos(x.Codigo, x.NombreCategoria) }).ToList();

            ViewBag.ListaTipoReglas =
                TipoReglasBL.ObtenerDatos(new TipoReglaValidacion()).objetoRespuesta.Select(x => new SelectListItem() { Selected=false, Value=x.IdTipo.ToString(), Text=x.Nombre }).ToList();

            ViewBag.ListaOperadores = 
                OperadoresBL.ObtenerDatos(new OperadorArismetico()).objetoRespuesta.Select(x => new SelectListItem() { Selected = false, Value = x.IdOperador.ToString(), Text = x.Nombre }).ToList();


            ViewBag.ListaIndicadores=
                        ListadoIndicador. Select(x => new SelectListItem() { Selected = false, Value = x.idIndicador.ToString(), Text = Utilidades.ConcatenadoCombos(x.Codigo, x.Nombre) }).ToList();


            ViewBag.ListaIndicadoresSalida =
                       ListadoIndicador.Where(x=>x.IdClasificacion==(int)Constantes.ClasificacionIndicadorEnum.Salida).Select(x => new SelectListItem() { Selected = false, Value = x.idIndicador.ToString(), Text = Utilidades.ConcatenadoCombos(x.Codigo, x.Nombre) }).ToList();

            ViewBag.Modo = modo.ToString();

            ReglaValidacion objregla = new ReglaValidacion();
            if (!string.IsNullOrEmpty(id))
            {
                objregla.id = id;
                objregla = reglaBL.ObtenerDatos(objregla).objetoRespuesta.SingleOrDefault();
                if (modo == (int)Constantes.Accion.Clonar)
                {
                    ViewBag.titulo = EtiquetasViewReglasValidacion.Clonar;
                    objregla.Codigo = string.Empty;
                    objregla.id = string.Empty;
                }
                else
                {
                    ViewBag.titulo = EtiquetasViewReglasValidacion.Editar;
                }

            }
            else
            {
                ViewBag.titulo = EtiquetasViewReglasValidacion.Crear;
            }
            return View(objregla);

        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }



        #region Metodos Async


        /// <summary>
        /// Fecha 17-08-2022
        /// Michael Hernández Cordero
        /// Obtiene datos para la table reglas de validación
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerListaReglasValidacion()
        {
            RespuestaConsulta<List<ReglaValidacion>> result = null;
            await Task.Run(() =>
            {
                result = reglaBL.ObtenerDatos(new ReglaValidacion());

            });
            return JsonConvert.SerializeObject(result);
        }
        #endregion

    }
}
