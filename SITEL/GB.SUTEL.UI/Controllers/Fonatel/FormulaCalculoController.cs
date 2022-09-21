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
using static GB.SIMEF.Resources.Constantes;

namespace GB.SUTEL.UI.Controllers.Fonatel
{
    public class FormulaCalculoController : Controller
    {
        #region Variables Públicas del controller
        private readonly FormulasCalculoBL formulaBL;
        private readonly FrecuenciaEnvioBL frecuenciaEnvioBL;
        private readonly IndicadorFonatelBL indicadorFonatelBL;

        private readonly DetalleIndicadorVariablesBL indicadorVariableBL;
        #endregion
        public FormulaCalculoController()
        {
            this.frecuenciaEnvioBL = new FrecuenciaEnvioBL(EtiquetasViewFormulasCalculo.Pantalla, System.Web.HttpContext.Current.User.Identity.GetUserId());
            this.formulaBL = new FormulasCalculoBL(EtiquetasViewFormulasCalculo.Pantalla, System.Web.HttpContext.Current.User.Identity.GetUserId());
            this.indicadorFonatelBL = new IndicadorFonatelBL(EtiquetasViewFormulasCalculo.Pantalla, System.Web.HttpContext.Current.User.Identity.GetUserId());
            this.indicadorVariableBL = new DetalleIndicadorVariablesBL(EtiquetasViewFormulasCalculo.Pantalla, System.Web.HttpContext.Current.User.Identity.GetUserId());
        }

        // GET: Solicitud
        public ActionResult Index()
        {




            return View();
        }

        // GET: Solicitud/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        
        [HttpGet]
        public ActionResult Create(string id=null, int modo=0)
        {
            var modelo = new FormulasCalculo();
            ViewBag.FrecuanciaEnvio = frecuenciaEnvioBL.ObtenerDatos(new FrecuenciaEnvio() { })
                .objetoRespuesta.Select(x => new SelectListItem()
                {
                    Selected = false,
                    Value = x.idFrecuencia.ToString(),
                    Text =  x.Nombre
                }).ToList();

            ViewBag.IndicadorSalida =
                indicadorFonatelBL.ObtenerDatos(new Indicador() { IdClasificacion = (int)Constantes.ClasificacionIndicadorEnum.Salida }).objetoRespuesta
                .Select(x => new SelectListItem()
                {
                    Value = Utilidades.Desencriptar(x.id),
                    Text = Utilidades.ConcatenadoCombos(x.Codigo, x.Nombre),
                    Selected = false
                }).ToList();



            if (modo==(int)Constantes.Accion.Clonar && !string.IsNullOrEmpty(id))
            {
                ViewBag.ModoFormulario = ((int)Accion.Clonar).ToString();
                ViewBag.TituloVista = EtiquetasViewFormulasCalculo.TituloClonar;
                modelo = formulaBL.ObtenerDatos(new FormulasCalculo() { id = id }).objetoRespuesta.Single();
                modelo.Nombre = string.Empty;
                modelo.Codigo = string.Empty;
            }
            else if(modo == (int)Constantes.Accion.Editar && !string.IsNullOrEmpty(id))
            {
                ViewBag.ModoFormulario = ((int)Accion.Editar).ToString();
                ViewBag.TituloVista = EtiquetasViewFormulasCalculo.TituloEditar;
                modelo = formulaBL.ObtenerDatos(new FormulasCalculo() { id = id }).objetoRespuesta.Single();
            }
            else
            {
                ViewBag.ModoFormulario = ((int)Accion.Insertar).ToString();
                ViewBag.TituloVista = EtiquetasViewFormulasCalculo.TituloCrear;
            }


            ViewBag.VariableIndicador =
                    indicadorVariableBL.ObtenerDatos(new DetalleIndicadorVariables()  ).objetoRespuesta
                    .Select(x => new SelectListItem()
                    {
                        Value = Utilidades.Desencriptar(x.id),
                        Text = x.NombreVariable,
                        Selected = false
                    }).ToList();




            return View(modelo);
        }

        // POST: Solicitud/Create
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

        // GET: Solicitud/Edit/5
        public ActionResult Edit(string id)
        {
            ViewBag.ModoFormulario = ((int)Accion.Editar).ToString();
            ViewBag.TituloVista = EtiquetasViewFormulasCalculo.TituloEditar;
            return View("Create");
        }

        // POST: Solicitud/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Solicitud/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Solicitud/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Solicitud/Clone/5
        public ActionResult Clone(string id)
        {
            ViewBag.ModoFormulario = ((int)Accion.Clonar).ToString();
            ViewBag.TituloVista = EtiquetasViewFormulasCalculo.TituloClonar;
            return View("Create");
        }
        
        // GET: Solicitud/View/5
        public ActionResult View(string id)
        {
            ViewBag.ModoFormulario = ((int)Accion.Consultar).ToString();
            ViewBag.TituloVista = EtiquetasViewFormulasCalculo.TituloVisualizar;
            return View("Create");
        }




        /// <summary>
        /// Fecha 04-08-2022
        /// Michael Hernández Cordero
        /// Obtiene datos para la table de categorías INDEX
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<string> ObtenerListaFormulas()
        {
            RespuestaConsulta<List<FormulasCalculo>> result = null;
            await Task.Run(() =>
            {
                result = formulaBL.ObtenerDatos(new FormulasCalculo());
            });

            return JsonConvert.SerializeObject(result);


        }




        [HttpPost]
        public async Task<string> EliminarFormula(FormulasCalculo formulaCalculo)
        {
            RespuestaConsulta<List<FormulasCalculo>> result = null;
      
            await Task.Run(() =>
            {
                return formulaBL.ObtenerDatos(formulaCalculo);

            }).ContinueWith(data =>
            {
                var objetoValidar = data.Result.objetoRespuesta.Single();
                objetoValidar.IdEstado = (int)Constantes.EstadosRegistro.Eliminado;
                result = formulaBL.EliminarElemento(objetoValidar);
            }
            );
            return JsonConvert.SerializeObject(result);
        }


        [HttpPost]
        public async Task<string> ActivarFormula(FormulasCalculo formulaCalculo)
        {
            RespuestaConsulta<List<FormulasCalculo>> result = null;

            await Task.Run(() =>
            {
                return formulaBL.ObtenerDatos(formulaCalculo);

            }).ContinueWith(data =>
            {
                var objetoValidar = data.Result.objetoRespuesta.Single();
                objetoValidar.IdEstado = (int)Constantes.EstadosRegistro.Activo;
                result = formulaBL.CambioEstado(objetoValidar);
            }
            );
            return JsonConvert.SerializeObject(result);
        }


        [HttpPost]
        public async Task<string> DesactivarFormula(FormulasCalculo formulaCalculo)
        {
            RespuestaConsulta<List<FormulasCalculo>> result = null;

            await Task.Run(() =>
            {
                return formulaBL.ObtenerDatos(formulaCalculo);

            }).ContinueWith(data =>
            {
                var objetoValidar = data.Result.objetoRespuesta.Single();
                objetoValidar.IdEstado = (int)Constantes.EstadosRegistro.Desactivado;
                result = formulaBL.CambioEstado(objetoValidar);
            }
            );
            return JsonConvert.SerializeObject(result);
        }

    }
}
