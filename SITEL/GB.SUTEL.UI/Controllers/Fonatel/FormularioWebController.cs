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
    public class FormularioWebController : Controller
    {
        #region Variables Públicas del controller
        private readonly FormularioWebBL formularioWebBL;
        private readonly FrecuenciaEnvioBL frecuenciaEnvioBL;
        private readonly IndicadorFonatelBL indicadorBL;

        #endregion

        public FormularioWebController()
        {
            formularioWebBL = new FormularioWebBL(EtiquetasViewFormulario.Formulario, System.Web.HttpContext.Current.User.Identity.GetUserId());
            this.frecuenciaEnvioBL = new FrecuenciaEnvioBL(EtiquetasViewFormulario.Formulario, System.Web.HttpContext.Current.User.Identity.GetUserId());
            this.indicadorBL = new IndicadorFonatelBL(EtiquetasViewFormulario.Formulario, System.Web.HttpContext.Current.User.Identity.GetUserId());
        }

        #region Eventos de la Página
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
    #endregion

        /// <summary>
        /// Fecha 24-08-2022
        /// Anderson Alvarado Aguero
        /// Obtiene datos para la tabla de formularios web INDEX
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerFormulariosWeb()
        {
            RespuestaConsulta<List<FormularioWeb>> result = null;
            await Task.Run(() =>
            {
                result = formularioWebBL.ObtenerDatos(new FormularioWeb());
            });
            return JsonConvert.SerializeObject(result);
        }

        [HttpGet]
        public ActionResult Create(string id, int? modo)
        {
            ViewBag.FrecuanciaEnvio = frecuenciaEnvioBL.ObtenerDatos(new FrecuenciaEnvio() { })
                .objetoRespuesta;
            var indicadores = indicadorBL.ObtenerDatos(new Indicador() { })
                .objetoRespuesta;
            var listaValores= indicadores.Select(x => new SelectListItem() { Selected = false, Value = x.idIndicador.ToString(), Text = Utilidades.ConcatenadoCombos(x.Codigo, x.Nombre) }).ToList();
            ViewBag.Indicador = listaValores;
            FormularioWeb objFormularioWeb = new FormularioWeb();
            DetalleFormularioWeb objDetalleFormularioWeb = new DetalleFormularioWeb();
            ViewBag.Modo = modo.ToString();
            if (id != null) {
                objFormularioWeb.id = id;
                objFormularioWeb = formularioWebBL.ObtenerDatos(objFormularioWeb).objetoRespuesta.SingleOrDefault();
                objDetalleFormularioWeb.formularioweb = objFormularioWeb;
                if (modo==(int)Constantes.Accion.Clonar)
                    ViewBag.ModoTitulo = EtiquetasViewFormulario.ClonarFormulario;
                objDetalleFormularioWeb.formularioweb.Codigo = string.Empty;
                objDetalleFormularioWeb.formularioweb.Nombre = string.Empty;
                if (modo==(int)Constantes.Accion.Editar)
                    ViewBag.ModoTitulo = EtiquetasViewFormulario.EditarFormularioWeb;
            }
            else
                ViewBag.ModoTitulo = EtiquetasViewFormulario.CrearFormulario;
            return View(objDetalleFormularioWeb);
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
    }
}
