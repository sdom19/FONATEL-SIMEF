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
        private readonly DetalleReglaValidacionBL detalleReglaBL;
        private readonly CategoriasDesagregacionBL categoriasDesagregacionBL;
        private readonly IndicadorFonatelBL indicadorfonatelBL;
        private readonly TipoReglaValidacionBL TipoReglasBL;
        private readonly OperadorArismeticoBL OperadoresBL;
        private readonly DetalleIndicadorVariablesBL DetalleIndicadorVariablesBL;

        private string modulo = string.Empty;
        private string user = string.Empty;

        #endregion

        public ReglasValidacionController()
        {
            modulo = EtiquetasViewReglasValidacion.ReglasValidacion;
            user = System.Web.HttpContext.Current.User.Identity.GetUserId();
            reglaBL = new ReglaValidacionBL(modulo, user);
            indicadorfonatelBL = new IndicadorFonatelBL(modulo, user);
            categoriasDesagregacionBL = new CategoriasDesagregacionBL(modulo, user);
            detalleReglaBL = new DetalleReglaValidacionBL(modulo, user);
            TipoReglasBL = new TipoReglaValidacionBL();
            OperadoresBL = new OperadorArismeticoBL();
            DetalleIndicadorVariablesBL = new DetalleIndicadorVariablesBL(modulo, user);

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
               .ObtenerDatos(new CategoriasDesagregacion() { idEstado = (int)Constantes.EstadosRegistro.Activo }).objetoRespuesta;

            ViewBag.ListaCategoria = listadoCategoria
                .Where(x => x.IdTipoCategoria != (int)Constantes.TipoCategoriaEnum.Actualizable)
                .Select(x => new SelectListItem() { Selected = false, Value = x.idCategoria.ToString(), Text = Utilidades.ConcatenadoCombos(x.Codigo, x.NombreCategoria) }).ToList();

            ViewBag.ListaCategoriaActualizable = listadoCategoria
                .Where(x => x.IdTipoCategoria == (int)Constantes.TipoCategoriaEnum.Actualizable)
                .Select(x => new SelectListItem() { Selected = false, Value = x.idCategoria.ToString(), Text = Utilidades.ConcatenadoCombos(x.Codigo, x.NombreCategoria) }).ToList();

            ViewBag.ListaTipoReglas =
                TipoReglasBL.ObtenerDatos(new TipoReglaValidacion()).objetoRespuesta.Select(x => new SelectListItem() { Selected = false, Value = x.IdTipo.ToString(), Text = x.Nombre }).ToList();

            ViewBag.ListaOperadores =
                OperadoresBL.ObtenerDatos(new OperadorArismetico()).objetoRespuesta.Select(x => new SelectListItem() { Selected = false, Value = x.IdOperador.ToString(), Text = x.Nombre }).ToList();

            ViewBag.ListaIndicadores =
                        ListadoIndicador.Select(x => new SelectListItem() { Selected = false, Value = x.id, Text = Utilidades.ConcatenadoCombos(x.Codigo, x.Nombre) }).ToList();
            
            ViewBag.ListaIndicadoresSalida =
                       ListadoIndicador.Where(x => int.Parse(Utilidades.Desencriptar(x.ClasificacionIndicadores.id)) == (int)Constantes.ClasificacionIndicadorEnum.Salida).Select(x => new SelectListItem() { Selected = false, Value = Utilidades.Desencriptar(x.id), Text = Utilidades.ConcatenadoCombos(x.Codigo, x.Nombre) }).ToList();

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
                    objregla.Nombre = string.Empty;
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

        [HttpGet]
        public ActionResult Edit(string id)
        {
            ViewBag.ModoFormulario = ((int)Constantes.Accion.Editar).ToString();
            ViewBag.TituloVista = EtiquetasViewIndicadorFonatel.TituloEditarIndicador;

            if (string.IsNullOrEmpty(id))
                return View("Index");

            DetalleReglaValidacion objDetalle = null;
            try
            {
                objDetalle = detalleReglaBL.ObtenerDatos(new DetalleReglaValidacion() { id = id }).objetoRespuesta.FirstOrDefault();
            }
            catch (Exception) { };

            if (objDetalle == null)
                return View("Index");

            return View("Create", objDetalle);
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

        /// <summary>
        /// Obtiene datos para la table detalles reglas de validación
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerListaDetalleReglasValidacion(string idReglasValidacionTipo)
        {
            RespuestaConsulta<List<DetalleReglaValidacion>> result = null;

            await Task.Run(() =>
            {
                result = detalleReglaBL.ObtenerDatos(new DetalleReglaValidacion()
                { id = idReglasValidacionTipo });

            });
            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Autor: Francisco Vindas
        /// Fecha: 27/10/2022
        /// Metodo para obtener las variables dato de cada Indicador
        /// </summary>
        /// <param name="idIndicadorString"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerListaVariablesDato(string idIndicadorString)
        {
            RespuestaConsulta<List<DetalleIndicadorVariables>> result = null;

            await Task.Run(() =>
            {
                result = DetalleIndicadorVariablesBL.ObtenerDatos(new DetalleIndicadorVariables()
                {
                    idIndicadorString = idIndicadorString 
                });
            });

            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Validar si el regla está en un indicador
        /// Michael Hernandez Cordero
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public async Task<string> ValidarRegla(ReglaValidacion objRegla)
        {
            RespuestaConsulta<List<string>> result = null;
            await Task.Run(() =>
            {
                return reglaBL.ObtenerDatos(objRegla);

            }).ContinueWith(data =>
            {
                ReglaValidacion objetoValidar = data.Result.objetoRespuesta.Single();
                result = reglaBL.ValidarDatos(objetoValidar);
            }
            );
            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Autor:Francisco Vindas Ruiz
        /// Fecha: 27/10/2022
        /// Crea la Regla de validacion
        /// </summary>
        /// <param name="objetoRegla"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> InsertarReglaValidacion(ReglaValidacion objeto)
        {
            user = User.Identity.GetUserId();
            RespuestaConsulta<List<ReglaValidacion>> result = null;
            await Task.Run(() =>
            {
                    result = reglaBL.InsertarDatos(objeto);               
            });

            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Fecha: 27/10/2022
        /// Autor: Francisco Vindas
        /// Metodo para editar una regla de validacion 
        /// </summary>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> EditarReglaValidacion(ReglaValidacion objeto)
        {

            user = User.Identity.GetUserId();
            objeto.idEstado = (int)Constantes.EstadosRegistro.EnProceso;

            RespuestaConsulta<List<ReglaValidacion>> result = null;

            await Task.Run(() =>
            {
                objeto.UsuarioCreacion = user;

                result = reglaBL.ActualizarElemento(objeto);

            });

            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Agregar el detalle de la reglade validación
        /// </summary>
        /// <param name="objetoTipoRegla"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> AgregarDetalleRegla(DetalleReglaValidacion objetoTipoRegla)
        {
            user = User.Identity.GetUserId();

            RespuestaConsulta<List<DetalleReglaValidacion>> result = null;
            await Task.Run(() =>
            {
                if (objetoTipoRegla.IdDetalleReglaValidacion == 0)
                {
                    result = detalleReglaBL.InsertarDatos(objetoTipoRegla);
                }
                else
                {
                    //objetoTipoRegla.UsuarioModificacion = user;
                    result = detalleReglaBL.ActualizarElemento(objetoTipoRegla);
                }

            });

            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Cambio el estado de registro a desactivado y activado 
        /// </summary>
        /// <param name="regla"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> EliminarRegla(ReglaValidacion regla)
        {
            user = User.Identity.GetUserId();
            RespuestaConsulta<List<ReglaValidacion>> result = null;
            await Task.Run(() =>
            {
                result = reglaBL.EliminarElemento(regla);
            });

            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Cambio el estado de registro a desactivado y activado 
        /// </summary>
        /// <param name="regla"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> EliminarDetalleRegla(DetalleReglaValidacion detalleRegla)
        {
            user = User.Identity.GetUserId();
            RespuestaConsulta<List<DetalleReglaValidacion>> result = null;
            await Task.Run(() =>
            {
                result = detalleReglaBL.EliminarElemento(detalleRegla);
            });

            return JsonConvert.SerializeObject(result);
        }
        #endregion

    }
}
