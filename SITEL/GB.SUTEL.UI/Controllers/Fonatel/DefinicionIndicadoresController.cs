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
    [AuthorizeUserAttribute]
    public class DefinicionIndicadoresController : Controller
    {


        private readonly DefinicionIndicadorBL definicionBL;

        private readonly IndicadorFonatelBL indicadorBl;

        public DefinicionIndicadoresController()
        {
            indicadorBl = new IndicadorFonatelBL(EtiquetasViewDefinicionIndicadores.TituloIndex, System.Web.HttpContext.Current.User.Identity.GetUserId());
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
            int.TryParse(Utilidades.Desencriptar(id), out temp);
            
            var model = definicionBL.VisualizarElemento(new DefinicionIndicador() { idIndicador = temp }).objetoRespuesta;
           
            return View(model);
        }



        [HttpGet]
        public ActionResult Clonar(string id, int? modo)
        {

            ViewBag.ListaIndicadores =
                definicionBL.ObtenerDatos(new DefinicionIndicador()).objetoRespuesta.Where( x=>x.idEstado!=(int)Constantes.EstadosRegistro.Activo)
                            .Select(x => new SelectListItem()  {  Value = x.Indicador.id, Text = Utilidades.ConcatenadoCombos(x.Indicador.Codigo, x.Indicador.Nombre) }).ToList();
                

            int temp;
            int.TryParse( Utilidades.Desencriptar( id), out temp);
            var model = definicionBL.ObtenerDatos(new DefinicionIndicador() { idIndicador = temp }).objetoRespuesta.Single();
            model.Indicador.Codigo = string.Empty;
            model.Indicador.Nombre = string.Empty;
            model.Indicador.id = string.Empty;
            model.Indicador.TipoIndicadores.Nombre = string.Empty;
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
                return definicionBL.ObtenerDatos(new DefinicionIndicador()
                        {id=definicion.id });
            }).ContinueWith((obj)=> {
                DefinicionIndicador pdefinicion = null;
                if (obj.Result.CantidadRegistros == 1)
                {
                    pdefinicion = obj.Result.objetoRespuesta.Single();
                }
                result = definicionBL.EliminarElemento(pdefinicion);
            });
            return JsonConvert.SerializeObject(result);
        }



        [HttpPost]
        public async Task<string> ActualizarDefinicion(DefinicionIndicador objDefinicion)
        {

            RespuestaConsulta<List<DefinicionIndicador>> result = null;
            await Task.Run(() =>
            {
                return definicionBL.ObtenerDatos(new DefinicionIndicador()
                { id = objDefinicion.id });
            }).ContinueWith((obj) => {
                DefinicionIndicador pdefinicion = null;
                if (obj.Result.CantidadRegistros ==1)
                {
                    pdefinicion = obj.Result.objetoRespuesta.Single();
                    pdefinicion.json = definicionBL.SerializarObjetoBitacora(pdefinicion);
                    pdefinicion.Notas = objDefinicion.Notas.Trim();
                    pdefinicion.Definicion = objDefinicion.Definicion.Trim();
                    pdefinicion.Fuente = objDefinicion.Fuente.Trim();
                    
                }                    
                result = definicionBL.ActualizarElemento(pdefinicion);
            });
            return JsonConvert.SerializeObject(result);
        }



        [HttpPost]
        public async Task<string> ClonarDefinicion(DefinicionIndicador objDefinicion)
        {

            RespuestaConsulta<List<DefinicionIndicador>> result = null;
            await Task.Run(() =>
            {
                return definicionBL.ObtenerDatos(new DefinicionIndicador()
                { id = objDefinicion.id });
            }).ContinueWith((obj) => {
                var definicionSeleccionada = obj.Result.objetoRespuesta.Single();
                objDefinicion.json = definicionBL.SerializarObjetoBitacora(definicionSeleccionada);
                if (obj.Result.CantidadRegistros==0)
                {
                    objDefinicion = null;
                }
                result = definicionBL.ClonarDatos(objDefinicion);
            });
            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public async Task<string> AgregarDefinicion(DefinicionIndicador objDefinicion)
        {

            RespuestaConsulta<List<DefinicionIndicador>> result = null;
            await Task.Run(() =>
            {
                return definicionBL.ObtenerDatos(new DefinicionIndicador()
                { id = objDefinicion .id });
            }).ContinueWith((obj) => {
                var definicionSeleccionada = obj.Result.objetoRespuesta.Single();
                if (!string.IsNullOrEmpty(definicionSeleccionada.Definicion))
                {
                    objDefinicion = null;
                }
                result = definicionBL.InsertarDatos(objDefinicion);
            });
            return JsonConvert.SerializeObject(result);
        }



        [HttpPost]
        public async Task<string> SeleccionarIndicador(Indicador indicador)
        {
            RespuestaConsulta<List<Indicador>> result = null;
            await Task.Run(() =>
            {

                int temp = 0;
                int.TryParse(Utilidades.Desencriptar(indicador.id), out temp);
                indicador.idIndicador = temp;
                result = indicadorBl.ObtenerDatos(indicador);
            });
            return JsonConvert.SerializeObject(result);
        }



        #endregion


    }
}
