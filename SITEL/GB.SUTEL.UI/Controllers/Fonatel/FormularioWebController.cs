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
        private readonly DetalleFormularioWebBL detalleFormularioWebBL;

        #endregion

        public FormularioWebController()
        {
            this.detalleFormularioWebBL = new DetalleFormularioWebBL(EtiquetasViewFormulario.Formulario, System.Web.HttpContext.Current.User.Identity.GetUserId());
            this.formularioWebBL = new FormularioWebBL(EtiquetasViewFormulario.Formulario, System.Web.HttpContext.Current.User.Identity.GetUserId());
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
        public async Task<string> ObtenerIndicadoresFormulario(string idFormulario)
        {
            FormularioWeb objFormularioWeb = new FormularioWeb();
            objFormularioWeb.id = idFormulario;
            RespuestaConsulta<List<Indicador>> result = null;
            await Task.Run(() =>
            {
                result = formularioWebBL.ObtenerIndicadoresFormulario(objFormularioWeb);
            });
            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public async Task<string> EliminarFormulario(FormularioWeb objFormulario)
        {

            RespuestaConsulta<List<FormularioWeb>> result = null;
            await Task.Run(() =>
            {
                return formularioWebBL.ObtenerDatos(objFormulario);

            }).ContinueWith(data =>
            {
                FormularioWeb objetoValidar = data.Result.objetoRespuesta.Single();
                objetoValidar.idEstado = (int)Constantes.EstadosRegistro.Eliminado;
                result = formularioWebBL.EliminarElemento(objetoValidar);
            }
            );
            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public async Task<string> DesactivarFormulario(FormularioWeb objFormulario)
        {

            RespuestaConsulta<List<FormularioWeb>> result = null;
            await Task.Run(() =>
            {
                return formularioWebBL.ObtenerDatos(objFormulario);

            }).ContinueWith(data =>
            {
                FormularioWeb objetoValidar = data.Result.objetoRespuesta.Single();
                objetoValidar.idEstado = (int)Constantes.EstadosRegistro.Desactivado;
                result = formularioWebBL.CambioEstado(objetoValidar);
            }
            );
            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public async Task<string> ActivarFormulario(FormularioWeb objFormulario)
        {

            RespuestaConsulta<List<FormularioWeb>> result = null;
            await Task.Run(() =>
            {
                    return formularioWebBL.ObtenerDatos(objFormulario);

            }).ContinueWith(data =>
            {
                FormularioWeb objetoValidar = data.Result.objetoRespuesta.Single();
                objetoValidar.idEstado = (int)Constantes.EstadosRegistro.Activo;
                result = formularioWebBL.CambioEstado(objetoValidar);
            }
            );
            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public async Task<string> GuardadoCompleto(FormularioWeb objFormulario)
        {

            RespuestaConsulta<List<FormularioWeb>> result = null;
            await Task.Run(() =>
            {
                return formularioWebBL.ObtenerDatos(objFormulario);

            }).ContinueWith(data =>
            {
                FormularioWeb objetoValidar = data.Result.objetoRespuesta.Single();
                objetoValidar.idEstado = (int)Constantes.EstadosRegistro.Activo;
                result = formularioWebBL.CambioEstado(objetoValidar);
            }
            );
            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public async Task<string> ClonarFormulario(FormularioWeb objFormulario)
        {
            RespuestaConsulta<List<FormularioWeb>> result = null;
            await Task.Run(() =>
            {
                result = formularioWebBL.ClonarDatos(objFormulario);
            });

            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public async Task<string> EditarFormularioWeb(FormularioWeb objFormulario)
        {
            RespuestaConsulta<List<FormularioWeb>> result = null;
            await Task.Run(() =>
            {
                result = formularioWebBL.ActualizarElemento(objFormulario);
            });

            return JsonConvert.SerializeObject(result);
        }
        
        /// <summary>
        /// Validar si el formulario está en una solicitud 
        /// Michael Hernandez Cordero
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public async Task<string> ValidarFormulario( FormularioWeb objFormulario)
        {
            RespuestaConsulta<List<string>> result = null;
            await Task.Run(() =>
            {
                return formularioWebBL.ObtenerDatos(objFormulario);

            }).ContinueWith(data =>
            {
                FormularioWeb objetoValidar = data.Result.objetoRespuesta.Single();
                result = formularioWebBL.ValidarDatos(objetoValidar);
            }
            );
            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public async Task<string> InsertarFormularioWeb(FormularioWeb formulario)
        {
            RespuestaConsulta<List<FormularioWeb>> result = null;
            await Task.Run(() =>
            {
                result = formularioWebBL.InsertarDatos(formulario);
                ViewBag.CantidadMax = result.objetoRespuesta[0].CantidadIndicadores;
            });
            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public async Task<string> InsertarIndicadoresFormulario(DetalleFormularioWeb detalleformulario)
        {
            RespuestaConsulta<List<DetalleFormularioWeb>> result = null;
            await Task.Run(() =>
            {
                result = detalleFormularioWebBL.InsertarDatos(detalleformulario);
            });
            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public async Task<string> EliminarIndicadoresFormulario(DetalleFormularioWeb detalleformulario) 
        {
            int temp = 0;
            int.TryParse(Utilidades.Desencriptar(detalleformulario.formularioweb.id), out temp);
            detalleformulario.idFormulario = temp;
            RespuestaConsulta<List<DetalleFormularioWeb>> result = null;
            await Task.Run(() =>
            {
                result = detalleFormularioWebBL.EliminarElemento(detalleformulario);
            });
            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public async Task<string> EditarIndicadoresFormulario(DetalleFormularioWeb detalleformulario)
        {
            int temp = 0;
            int.TryParse(Utilidades.Desencriptar(detalleformulario.formularioweb.id), out temp);
            detalleformulario.idFormulario = temp;
            RespuestaConsulta<List<DetalleFormularioWeb>> result = null;
            await Task.Run(() =>
            {
                result = detalleFormularioWebBL.ActualizarElemento(detalleformulario);
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
            var listaValores = indicadores.Select(x => new SelectListItem() { Selected = false, Value = x.idIndicador.ToString(), Text = Utilidades.ConcatenadoCombos(x.Codigo, x.Nombre) }).ToList();
            ViewBag.Indicador = listaValores;
            DetalleFormularioWeb objDetalleFormularioWeb = new DetalleFormularioWeb();
            ViewBag.Modo = modo.ToString();
            if (id != null)
            {
                FormularioWeb objFormularioWeb = new FormularioWeb();
                objFormularioWeb.id = id;
                objFormularioWeb = formularioWebBL.ObtenerDatos(objFormularioWeb).objetoRespuesta.SingleOrDefault();
                objFormularioWeb.ListaIndicadoresObj = formularioWebBL.ObtenerIndicadoresFormulario(objFormularioWeb).objetoRespuesta.ToList();
                
                ViewBag.CantidadMax = objFormularioWeb.CantidadIndicadores;
                int idFormulario = 0;
                int.TryParse(Utilidades.Desencriptar(id), out idFormulario);
                if (modo == (int)Constantes.Accion.Clonar)
                {
                    ViewBag.ModoTitulo = EtiquetasViewFormulario.ClonarFormulario;
                    objFormularioWeb.Codigo = string.Empty;
                    objFormularioWeb.Nombre = string.Empty;
                }

                if (modo == (int)Constantes.Accion.Editar)
                {
                    ViewBag.ModoTitulo = EtiquetasViewFormulario.EditarFormularioWeb;
                }
                objDetalleFormularioWeb.formularioweb = objFormularioWeb;
            }
            else
            {
                ViewBag.CantidadMax = 0;
                ViewBag.ModoTitulo = EtiquetasViewFormulario.CrearFormulario;
                objDetalleFormularioWeb.formularioweb = new FormularioWeb();
            }
            return View(objDetalleFormularioWeb);
        }

        [HttpGet]
        public ActionResult Visualizar(int? id, int? modo)
        {
            return View();
        }

        [HttpGet]
        public async Task<string> ObtenerDetalleFormularioWeb(int idIndicador, string idFormulario)
        {
            DetalleFormularioWeb objDetalleFormularioWeb = new DetalleFormularioWeb();
            int temp = 0;
            int.TryParse(Utilidades.Desencriptar( idFormulario), out temp );
            objDetalleFormularioWeb.idFormulario = temp;
            objDetalleFormularioWeb.idIndicador = idIndicador;
            await Task.Run(() =>
            {
                objDetalleFormularioWeb = detalleFormularioWebBL.ObtenerDatos(objDetalleFormularioWeb).objetoRespuesta.FirstOrDefault();
            });
            return JsonConvert.SerializeObject(objDetalleFormularioWeb);
        }

        [HttpGet]
        public ActionResult _CrearIndicador(int idIndicador, int idFormulario)
        {
            var indicadores = indicadorBL.ObtenerDatos(new Indicador() { })
                .objetoRespuesta;
            var listaValores = indicadores.Select(x => new SelectListItem() { Selected = false, Value = x.idIndicador.ToString(), Text = Utilidades.ConcatenadoCombos(x.Codigo, x.Nombre) }).ToList();
            ViewBag.Indicador = listaValores;
            DetalleFormularioWeb objDetalleFormularioWeb = new DetalleFormularioWeb();
            objDetalleFormularioWeb.TituloHojas = "prueba lo que sea";
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
