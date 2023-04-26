using GB.SIMEF.BL;
using GB.SIMEF.Entities;
using GB.SIMEF.Entities.DTO;
using GB.SIMEF.Resources;
using GB.SUTEL.UI.Filters;
using GB.SUTEL.UI.Helpers;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SUTEL.UI.Controllers.Fonatel
{
    [AuthorizeUserAttribute]
    public class FormulaCalculoController : Controller
    {
        #region Variables públicas del controller

        private readonly FormulasCalculoBL formulaBL;
        private readonly FrecuenciaEnvioBL frecuenciaEnvioBL;
        private readonly IndicadorFonatelBL indicadorFonatelBL;
        private readonly DetalleIndicadorVariablesBL detalleIndicadorVariablesBL;
        private readonly DetalleIndicadorCategoriaBL detalleIndicadorCategoriaBL;
        private readonly CategoriasDesagregacionBL categoriasDesagregacionBL;
        private readonly FuenteIndicadorBL fuenteIndicadorBL;
        private readonly GrupoIndicadorBL grupoIndicadorBL;
        private readonly TipoIndicadorBL tipoIndicadorBL;
        private readonly ClasificacionIndicadorBL clasificacionIndicadorBL;
        private readonly ServicioSitelBL servicioSitelBL;
        private readonly AcumulacionFormulaBL acumulacionFormulaBL;
        private readonly DetalleIndicadorCriteriosSitelBL detalleIndicadorCriteriosSitelBL;
        private readonly FormulasCalculoTipoFechaBL formulasCalculoTipoFechaBL;
        private readonly ArgumentoFormulaBL argumentoFormulaBL;

        private readonly string usuario = string.Empty;
        private readonly string nombreVista = string.Empty;
        private string modoFormulario = string.Empty;

        #endregion

        public FormulaCalculoController()
        {
            usuario = System.Web.HttpContext.Current.User.Identity.GetUserId();
            nombreVista = EtiquetasViewFormulasCalculo.TituloIndex;
            frecuenciaEnvioBL = new FrecuenciaEnvioBL(nombreVista, usuario);
            formulaBL = new FormulasCalculoBL(nombreVista, usuario);
            indicadorFonatelBL = new IndicadorFonatelBL(nombreVista, usuario);
            detalleIndicadorVariablesBL = new DetalleIndicadorVariablesBL(nombreVista, usuario);
            detalleIndicadorCategoriaBL = new DetalleIndicadorCategoriaBL(nombreVista, usuario);
            categoriasDesagregacionBL = new CategoriasDesagregacionBL(nombreVista, usuario);
            fuenteIndicadorBL = new FuenteIndicadorBL(nombreVista, usuario);
            grupoIndicadorBL = new GrupoIndicadorBL(nombreVista, usuario);
            tipoIndicadorBL = new TipoIndicadorBL(nombreVista, usuario);
            clasificacionIndicadorBL = new ClasificacionIndicadorBL(nombreVista, usuario);
            servicioSitelBL = new ServicioSitelBL();
            acumulacionFormulaBL = new AcumulacionFormulaBL(nombreVista, usuario);
            detalleIndicadorCriteriosSitelBL = new DetalleIndicadorCriteriosSitelBL();
            formulasCalculoTipoFechaBL = new FormulasCalculoTipoFechaBL();
            argumentoFormulaBL = new ArgumentoFormulaBL();
        }

        #region Eventos de página

        // GET: Solicitud
        public ActionResult Index()
        {
            var roles = ((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault().Value.Split(',');
            ViewBag.ConsultasFonatel = roles.Contains(RolConsultasFonatel).ToString().ToLower();
            return View();
        }

        // GET: Solicitud/Details/5
        [ConsultasFonatelFilter]
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpGet]
        [ConsultasFonatelFilter]
        public ActionResult Create()
        {
            CargarDatosEnVistas(Accion.Crear, new FormulaCalculo());
            ViewBag.ModoFormulario = ((int)Accion.Crear).ToString();
            ViewBag.TituloVista = EtiquetasViewFormulasCalculo.TituloCrear;

            return View(new FormulaCalculo());
        }

        [HttpGet]
        [ConsultasFonatelFilter]
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return View("Index");

            FormulaCalculo objFormulaCalculo = null;

            try
            {
                objFormulaCalculo = formulaBL.ObtenerDatos(new FormulaCalculo() { id = id }).objetoRespuesta.Single();
            }
            catch (Exception) { }

            if (objFormulaCalculo == null)
                return View("Index");

            CargarDatosEnVistas(Accion.Editar, objFormulaCalculo);

            ViewBag.ModoFormulario = ((int)Accion.Editar).ToString();
            ViewBag.TituloVista = EtiquetasViewFormulasCalculo.TituloEditar;

            return View("Create", objFormulaCalculo);
        }

        // GET: Solicitud/Clone/5

        [HttpGet]
        [ConsultasFonatelFilter]
        public ActionResult Clone(string id)
        {
            if (string.IsNullOrEmpty(id))
                return View("Index");

            FormulaCalculo objFormulaCalculo = null;

            try
            {
                objFormulaCalculo = formulaBL.ObtenerDatos(new FormulaCalculo() { id = id }).objetoRespuesta.Single();
            }
            catch (Exception) { }

            // formula no encontrada?
            if (objFormulaCalculo == null)
                return View("Index");

            // la formula no tiene que estar en proceso
            if (objFormulaCalculo.IdEstadoRegistro == (int)EstadosRegistro.EnProceso)
                return View("Index");

            objFormulaCalculo.Nombre = string.Empty;
            objFormulaCalculo.Codigo = string.Empty;
            objFormulaCalculo.IdVariableDatoString = string.Empty;

            CargarDatosEnVistas(Accion.Clonar, objFormulaCalculo);

            ViewBag.ModoFormulario = ((int)Accion.Clonar).ToString();
            ViewBag.TituloVista = EtiquetasViewFormulasCalculo.TituloClonar;

            return View("Create", objFormulaCalculo);
        }

        [HttpGet]
        public ActionResult Visualize(string id)
        {
            if (string.IsNullOrEmpty(id))
                return View("Index");

            FormulaCalculo objFormulaCalculo = null;

            try
            {
                objFormulaCalculo = formulaBL.ObtenerDatos(new FormulaCalculo() { id = id }).objetoRespuesta.Single();
            }
            catch (Exception) { }

            // formula no encontrada?
            if (objFormulaCalculo == null)
                return View("Index");

            // la formula no tiene que estar en proceso
            if (objFormulaCalculo.IdEstadoRegistro == (int)EstadosRegistro.EnProceso)
                return View("Index");

            CargarDatosEnVistas(Accion.Visualizar, objFormulaCalculo);

            ViewBag.ModoFormulario = ((int)Accion.Visualizar).ToString();
            ViewBag.TituloVista = EtiquetasViewFormulasCalculo.TituloVisualizar;

            return View("Create", objFormulaCalculo);
        }

        #endregion

        #region Funciones async - Index

        /// <summary>
        /// Obtiene el listado de fórmulas registras
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerListaFormulas()
        {
            RespuestaConsulta<List<FormulaCalculo>> result = null;
            await Task.Run(() =>
            {
                result = formulaBL.ObtenerDatos(new FormulaCalculo());
            });

            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// 21/10/2022
        /// José Navarro Acuña
        /// Permite eliminar una fórmula
        /// </summary>
        /// <param name="formulaCalculo"></param>
        /// <returns></returns>
        [HttpPost]
        [ConsultasFonatelFilter]
        public async Task<string> EliminarFormula(FormulaCalculo pFormulaCalculo)
        {
            RespuestaConsulta<List<FormulaCalculo>> resultado = new RespuestaConsulta<List<FormulaCalculo>>();

            if (string.IsNullOrEmpty(pFormulaCalculo.id))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            FormulaCalculo formulaAEnviar = new FormulaCalculo()
            {
                id = pFormulaCalculo.id,
                IdEstadoRegistro = (int)EstadosRegistro.Eliminado
            };

            resultado = await formulaBL.CambiarEstadoJob(formulaAEnviar);

            if (resultado.HayError == (int)Error.NoError)
            {
                await Task.Run(() =>
                {
                    resultado = formulaBL.CambioEstado(formulaAEnviar);
                });
            }

            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// Permite activar una fórmula
        /// </summary>
        /// <param name="pFormulaCalculo"></param>
        /// <returns></returns>
        [HttpPost]
        [ConsultasFonatelFilter]
        public async Task<string> ActivarFormula(FormulaCalculo pFormulaCalculo)
        {
            RespuestaConsulta<List<FormulaCalculo>> resultado = new RespuestaConsulta<List<FormulaCalculo>>();

            if (string.IsNullOrEmpty(pFormulaCalculo.id))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            FormulaCalculo formulaAEnviar = new FormulaCalculo();

            await Task.Run(() => 
            { 
                formulaAEnviar = formulaBL.ObtenerDatos(new FormulaCalculo() { id = pFormulaCalculo.id }).objetoRespuesta.FirstOrDefault();
            });

            // la formula no tiene que estar en proceso
            if (formulaAEnviar == null || formulaAEnviar.IdEstadoRegistro == (int)EstadosRegistro.EnProceso)
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            formulaAEnviar = new FormulaCalculo()
            {
                id = pFormulaCalculo.id,
                IdEstadoRegistro = (int)EstadosRegistro.Activo
            };

            await Task.Run(() =>
            {
                resultado = formulaBL.CambioEstado(formulaAEnviar);
            });

            if (resultado.HayError == (int)Error.NoError)
            {
                resultado = await formulaBL.CambiarEstadoJob(formulaAEnviar);
            }

            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// Permite desactivar una fórmula
        /// </summary>
        /// <param name="formulaCalculo"></param>
        /// <returns></returns>
        [HttpPost]
        [ConsultasFonatelFilter]
        public async Task<string> DesactivarFormula(FormulaCalculo pFormulaCalculo)
        {
            RespuestaConsulta<List<FormulaCalculo>> resultado = new RespuestaConsulta<List<FormulaCalculo>>();

            if (string.IsNullOrEmpty(pFormulaCalculo.id))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            FormulaCalculo formulaAEnviar = new FormulaCalculo();

            await Task.Run(() =>
            {
                formulaAEnviar = formulaBL.ObtenerDatos(new FormulaCalculo() { id = pFormulaCalculo.id }).objetoRespuesta.FirstOrDefault();
            });

            // la formula no tiene que estar en proceso
            if (formulaAEnviar == null || formulaAEnviar.IdEstadoRegistro == (int)EstadosRegistro.EnProceso)
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            formulaAEnviar = new FormulaCalculo()
            {
                id = pFormulaCalculo.id,
                IdEstadoRegistro = (int)EstadosRegistro.Desactivado
            };

            await Task.Run(() =>
            {
                resultado = formulaBL.CambioEstado(formulaAEnviar);
            });

            if (resultado.HayError == (int)Error.NoError)
            {
                resultado = await formulaBL.CambiarEstadoJob(formulaAEnviar);
            }

            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 13/02/2023
        /// José Navarro Acuña
        /// Función que permite verificar si una fórmula ha sido ejecutada
        /// </summary>
        /// <param name="pIdFormula"></param>
        /// <returns></returns>
        [HttpGet]
        [ConsultasFonatelFilter]
        public async Task<string> VerificarSiFormulaEjecuto(string pIdFormula)
        {
            RespuestaConsulta<string> resultado = new RespuestaConsulta<string>();

            if (string.IsNullOrEmpty(pIdFormula))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            await Task.Run(() =>
            {
                resultado = formulaBL.VerificarSiFormulaEjecuto(pIdFormula);
            });
            return JsonConvert.SerializeObject(resultado);
        }

        #endregion

        #region Funciones async - Create

        /// <summary>
        /// 14/02/2023
        /// José Navarro Acuña
        /// Obtiene un listado de las variables dato de un indicador que no esta siendo utilizadas en las demás fórmulas de cálculo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerVariablesSinUsoEnFormula(string pIdFormula, string pIdIndicador)
        {
            RespuestaConsulta<List<DetalleIndicadorVariable>> resultado = new RespuestaConsulta<List<DetalleIndicadorVariable>>();

            if (string.IsNullOrEmpty(pIdIndicador))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            await Task.Run(() =>
            {
                resultado = detalleIndicadorVariablesBL.ObtenerVariablesSinUsoEnFormula(new DetalleIndicadorVariable()
                {
                    idIndicadorString = pIdIndicador
                }, 
                pIdFormula
                );
            });

            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 18/10/2022
        /// José Navarro Acuña
        /// Obtiene un listado de las categorias de desagregación relacionadas a un indicador
        /// </summary>
        /// <param name="pIdIndicador"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerCategoriasDesagregacionDeIndicador(string pIdIndicador)
        {
            RespuestaConsulta<List<CategoriaDesagregacion>> resultado = new RespuestaConsulta<List<CategoriaDesagregacion>>();

            if (string.IsNullOrEmpty(pIdIndicador))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            await Task.Run(() =>
            {
                resultado = categoriasDesagregacionBL.ObtenerCategoriasDesagregacionDeIndicador(pIdIndicador);
            });

            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 10/01/2023
        /// José Navarro Acuña
        /// Obtiene un listado de las categorias de desagrecion de tipo fecha de un indicador
        /// </summary>
        /// <param name="pIdIndicador"></param>
        /// <returns></returns>
        public async Task<string> ObtenerCategoriasDesagregacionTipoFechaDeIndicador(string pIdIndicador)
        {
            RespuestaConsulta<List<CategoriaDesagregacion>> resultado = new RespuestaConsulta<List<CategoriaDesagregacion>>();

            if (string.IsNullOrEmpty(pIdIndicador))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            await Task.Run(() =>
            {
                resultado = categoriasDesagregacionBL.ObtenerCategoriasDesagregacionTipoFechaDeIndicador(
                    pIdIndicador,
                    Utilidades.Encriptar(((int)TipoDetalleCategoriaEnum.Fecha).ToString()
                    ));
            });

            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 02/01/2022
        /// José Navarro Acuña
        /// Obtiene los detalles relacionados (vista indicador) con una categoria de desagregación, y a su vez con un indicador
        /// </summary>
        /// <param name="pIdIndicador"></param>
        /// <param name="pIdCategoria"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerListaDetallesDeCategoria(string pIdIndicador, string pIdCategoria)
        {
            RespuestaConsulta<List<DetalleIndicadorCategoria>> resultado = new RespuestaConsulta<List<DetalleIndicadorCategoria>>();

            if (string.IsNullOrEmpty(pIdIndicador))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            if (string.IsNullOrEmpty(pIdCategoria))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            await Task.Run(() =>
            {
                resultado = detalleIndicadorCategoriaBL.ObtenerDetallesDeCategoria(new DetalleIndicadorCategoria()
                {
                    idIndicadorString = pIdIndicador,
                    idCategoriaString = pIdCategoria
                });
            });
            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 14/02/2022
        /// José Navarro Acuña
        /// Obtiene los detalles relacionados con un criterio de mercados, y a su vez con un indicador
        /// </summary>
        /// <param name="pIdIndicador"></param>
        /// <param name="pIdCriterio"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerListaDetallesDeCriterioMercados(string pIdIndicador, string pIdCriterio)
        {
            RespuestaConsulta<List<DetalleIndicadorCategoria>> resultado = new RespuestaConsulta<List<DetalleIndicadorCategoria>>();

            if (string.IsNullOrEmpty(pIdIndicador))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            if (string.IsNullOrEmpty(pIdCriterio))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            await Task.Run(() =>
            {
                resultado = detalleIndicadorCriteriosSitelBL.ObtenerDetallesDeCriterioMercado(
                    new DetalleIndicadorCategoria()
                    {
                        idIndicadorString = pIdIndicador,
                        idCriterioString = pIdCriterio
                    });
            });
            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 17/11/2022
        /// José Navarro Acuña
        /// Obtiene un listado de las categorias de desagregación relacionadas a una formula respecto al nivel de calculo
        /// </summary>
        /// <param name="pIdFormula"></param>
        /// <param name="pIdIndicador"></param>
        /// <returns></returns>
        public async Task<string> ObtenerCategoriasDeFormulaNivelCalculo(string pIdFormula, string pIdIndicador)
        {
            RespuestaConsulta<List<CategoriaDesagregacion>> resultado = new RespuestaConsulta<List<CategoriaDesagregacion>>();

            if (string.IsNullOrEmpty(pIdFormula) || string.IsNullOrEmpty(pIdIndicador))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            await Task.Run(() =>
            {
                resultado = categoriasDesagregacionBL.ObtenerCategoriasDeFormulaNivelCalculo(pIdFormula, pIdIndicador);
            });

            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 21/10/2022
        /// José Navarro Acuña
        /// Función que permite crear una fórmula de cálculo
        /// </summary>
        /// <param name="pFormulaCalculo"></param>
        /// <returns></returns>
        [HttpPost]
        [ConsultasFonatelFilter]
        public async Task<string> CrearFormulaCalculo(FormulaCalculo pFormulaCalculo)
        {
            modoFormulario = (string)Session[keyModoFormulario];

            if (modoFormulario.Equals(((int)Accion.Visualizar).ToString()))
            {
                return JsonConvert.SerializeObject(new RespuestaConsulta<FormulaCalculo>() { HayError = (int)Error.ErrorSistema });
            }

            string mensajesValidacion = ValidarObjectoCrearFormulaCalculo(pFormulaCalculo);

            if (!string.IsNullOrEmpty(mensajesValidacion))
            {
                return JsonConvert.SerializeObject(
                    new RespuestaConsulta<FormulaCalculo>() { HayError = (int)Error.ErrorControlado, MensajeError = mensajesValidacion });
            }

            pFormulaCalculo.IdEstadoRegistro = (int)EstadosRegistro.EnProceso;
            pFormulaCalculo.UsuarioCreacion = usuario;
            pFormulaCalculo.IdFormulaCalculo = 0;
            pFormulaCalculo.IdFrecuenciaEnvio = 0;
            pFormulaCalculo.IdIndicador = 0;
            pFormulaCalculo.IdDetalleIndicadorVariable = 0;

            RespuestaConsulta<List<FormulaCalculo>> resultado = new RespuestaConsulta<List<FormulaCalculo>>();

            await Task.Run(() =>
            {
                resultado = formulaBL.InsertarDatos(pFormulaCalculo);
            });

            RespuestaConsulta<List<FormulaCalculo>> resultadoJob = new RespuestaConsulta<List<FormulaCalculo>>();

            if (resultado.HayError == (int)Error.NoError)
            {
                pFormulaCalculo = resultado.objetoRespuesta[0];
                resultadoJob = await formulaBL.CambiarEstadoJob(pFormulaCalculo);
            }

            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 15/11/2022
        /// José Navarro Acuña
        /// Función que permite editar una fórmula de cálculo
        /// </summary>
        /// <param name="pFormulasCalculo"></param>
        /// <returns></returns>
        [HttpPost]
        [ConsultasFonatelFilter]
        public async Task<string> EditarFormulaCalculo(FormulaCalculo pFormulaCalculo)
        {
            modoFormulario = (string)Session[keyModoFormulario];

            if (modoFormulario.Equals(((int)Accion.Visualizar).ToString()))
            {
                return JsonConvert.SerializeObject(new RespuestaConsulta<FormulaCalculo>() { HayError = (int)Error.ErrorSistema });
            }

            if (string.IsNullOrEmpty(pFormulaCalculo.id))
            {
                return JsonConvert.SerializeObject(
                    new RespuestaConsulta<List<FormulaCalculo>>() { HayError = (int)Error.ErrorControlado, MensajeError = Errores.NoRegistrosActualizar });
            }

            // tanto crear, clonar y editar pueden actualizar la fórmula, asi que se debe diferenciar la acción
            if (modoFormulario.Equals(((int)Accion.Editar).ToString()))
            {
                try
                {
                    FormulaCalculo objFormular = formulaBL.ObtenerDatos(new FormulaCalculo() { id = pFormulaCalculo.id }).objetoRespuesta.FirstOrDefault();
                    pFormulaCalculo.Codigo = objFormular.Codigo;
                }
                catch (Exception)
                {
                    return JsonConvert.SerializeObject(
                        new RespuestaConsulta<List<Indicador>>() { HayError = (int)Error.ErrorControlado, MensajeError = Errores.NoRegistrosActualizar });
                };
            }

            string mensajesValidacion = ValidarObjectoCrearFormulaCalculo(pFormulaCalculo);

            if (!string.IsNullOrEmpty(mensajesValidacion))
            {
                return JsonConvert.SerializeObject(
                    new RespuestaConsulta<FormulaCalculo>() { HayError = (int)Error.ErrorControlado, MensajeError = mensajesValidacion });
            }

            pFormulaCalculo.IdEstadoRegistro = (int)EstadosRegistro.EnProceso;
            pFormulaCalculo.IdFormulaCalculo = 0;
            pFormulaCalculo.IdFrecuenciaEnvio = 0;
            pFormulaCalculo.IdIndicador = 0;
            pFormulaCalculo.IdDetalleIndicadorVariable = 0;
            pFormulaCalculo.UsuarioModificacion = usuario;

            RespuestaConsulta<List<FormulaCalculo>> resultado = new RespuestaConsulta<List<FormulaCalculo>>();

            await Task.Run(() =>
            {
                resultado = formulaBL.ActualizarElemento(pFormulaCalculo);
            });

            if (resultado.HayError == (int)Error.NoError)
            {
                resultado = await formulaBL.CambiarEstadoJob(pFormulaCalculo);
            }

            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 21/11/2022
        /// José Navarro Acuña
        /// Función que permite clonar una fórmula de cálculo
        /// </summary>
        /// <param name="pFormulaCalculo"></param>
        /// <returns></returns>
        [HttpPost]
        [ConsultasFonatelFilter]
        public async Task<string> ClonarFormulaCalculo(FormulaCalculo pFormulaCalculo)
        {
            modoFormulario = (string)Session[keyModoFormulario];

            if (modoFormulario.Equals(((int)Accion.Visualizar).ToString()))
            {
                return JsonConvert.SerializeObject(new RespuestaConsulta<FormulaCalculo>() { HayError = (int)Error.ErrorSistema });
            }

            if (string.IsNullOrEmpty(pFormulaCalculo.id)) // id indicador requerido
            {
                return JsonConvert.SerializeObject(
                    new RespuestaConsulta<List<Indicador>>() { HayError = (int)Error.ErrorControlado, MensajeError = Errores.NoRegistrosActualizar });
            }

            string idFormulaAClonar = pFormulaCalculo.id; // id de la formula seleccionada para clonar
            pFormulaCalculo.id = string.Empty;
            pFormulaCalculo.IdFormulaCalculo = 0;

            string creacionFormula = await CrearFormulaCalculo(pFormulaCalculo);
            RespuestaConsulta<List<FormulaCalculo>> formulaDeserializado = JsonConvert.DeserializeObject<RespuestaConsulta<List<FormulaCalculo>>>(creacionFormula);

            if (formulaDeserializado.HayError != (int)Error.NoError) // se creó la formula (paso 1) correctamente?
            {
                return creacionFormula;
            }

            RespuestaConsulta<FormulaCalculo> resultado = new RespuestaConsulta<FormulaCalculo>();

            await Task.Run(() =>
            {
                // clonar los detalles del paso 2 del formulario, se envia la fórmula clonada (paso 1) y el ID de la fórmula de donde se va a obtener los argumentos para duplicar
                resultado = formulaBL.ClonarArgumentosDeFormula(formulaDeserializado.objetoRespuesta[0], idFormulaAClonar);
            });

            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 13/02/2023
        /// José Navarro Acuña
        /// Función que permite realizar un guardado definitivo de una fórmula de cálculo. Se establece el estado 'Activo'
        /// </summary>
        /// <param name="pIdFormulaCalculo"></param>
        /// <returns></returns>
        [HttpPost]
        [ConsultasFonatelFilter]
        public async Task<string> GuardadoDefinitivoFormulaCalculo(string pIdFormulaCalculo)
        {
            modoFormulario = (string)Session[keyModoFormulario];

            if (modoFormulario.Equals(((int)Accion.Visualizar).ToString()))
            {
                return JsonConvert.SerializeObject(new RespuestaConsulta<FormulaCalculo>() { HayError = (int)Error.ErrorSistema });
            }

            if (string.IsNullOrEmpty(pIdFormulaCalculo)) // id indicador requerido
            {
                return JsonConvert.SerializeObject(
                    new RespuestaConsulta<List<Indicador>>() { HayError = (int)Error.ErrorControlado, MensajeError = Errores.NoRegistrosActualizar });
            }

            RespuestaConsulta<List<FormulaCalculo>> resultado = new RespuestaConsulta<List<FormulaCalculo>>();

            await Task.Run(() =>
            {
                resultado = formulaBL.GuardadoDefinitivoFormulaCalculo(new FormulaCalculo() { id = pIdFormulaCalculo });
            });

            if (resultado.HayError == (int)Error.NoError)
            {
                resultado = await formulaBL.CrearJobEnMotor(new FormulaCalculo() { id = pIdFormulaCalculo, UsuarioCreacion = usuario });
            }

            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 01/03/2023
        /// José Navarro Acuña
        /// Función que permite ejecutar una formula de cálculo de manera manual en el motor
        /// </summary>
        /// <param name="pIdFormulaCalculo"></param>
        /// <returns></returns>
        [HttpPost]
        [ConsultasFonatelFilter]
        public async Task<string> EjecutarFormula(string pIdFormulaCalculo)
        {
            if (string.IsNullOrEmpty(pIdFormulaCalculo)) // id indicador requerido
            {
                return JsonConvert.SerializeObject(
                    new RespuestaConsulta<List<Indicador>>() { HayError = (int)Error.ErrorControlado, MensajeError = Errores.NoRegistrosActualizar });
            }

            RespuestaConsulta<List<FormulaCalculo>> resultado = await formulaBL.EjecutarJobFormulaManualmente(new FormulaCalculo() { id = pIdFormulaCalculo });

            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 24/04/2023
        /// José Navarro Acuña
        /// Función que permite ejecutar todas las fórmulas de cálculo en estado activo
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ConsultasFonatelFilter]
        public async Task<string> EjecutarFormulasEnEstadoActivo()
        {
            return JsonConvert.SerializeObject(await formulaBL.EjecutarFormulasEnEstadoActivo(new FormulaCalculo() { UsuarioModificacion = usuario }));
        }

        /// <summary>
        /// 22/11/2022
        /// José Navarro Acuña
        /// Función que permite obtener las fuentes de un los indicadores
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerFuentesIndicador()
        {
            RespuestaConsulta<List<FuenteIndicador>> resultado = new RespuestaConsulta<List<FuenteIndicador>>();

            await Task.Run(() =>
            {
                resultado = fuenteIndicadorBL.ObtenerDatos(new FuenteIndicador());
            });

            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 23/11/2022
        /// José Navarro Acuña
        /// Función que retorna los grupo de indicador. Puede consultar los de SITEL
        /// </summary>
        /// <param name="pEsFuenteIndicadorFonatel"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerGrupoIndicador(FuenteIndicadorEnum pFuenteIndicador)
        {
            RespuestaConsulta<List<GrupoIndicador>> resultado = new RespuestaConsulta<List<GrupoIndicador>>();

            await Task.Run(() =>
            {
                switch (pFuenteIndicador)
                {
                    case FuenteIndicadorEnum.IndicadorDGF:
                        resultado = grupoIndicadorBL.ObtenerDatos(new GrupoIndicador());
                        break;
                    case FuenteIndicadorEnum.IndicadorDGM:
                        resultado = grupoIndicadorBL.ObtenerDatosMercado();
                        break;
                    case FuenteIndicadorEnum.IndicadorDGC:
                        resultado = grupoIndicadorBL.ObtenerDatosCalidad();
                        break;
                    // se conservan debido a futuros cambios
                    //case FuenteIndicadorEnum.IndicadorUIT:
                    //    resultado = grupoIndicadorBL.ObtenerDatosCruzado();
                    //    break;
                    //case FuenteIndicadorEnum.IndicadorCruzado:
                    //    resultado = grupoIndicadorBL.ObtenerDatosCruzado();
                    //    break;
                    //case FuenteIndicadorEnum.IndicadorFuenteExterna:
                    //    break;
                    default:
                        resultado.HayError = (int)Error.ErrorSistema;
                        break;
                }
            });

            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 23/11/2022
        /// José Navarro Acuña
        /// Función que retorna los tipos de indicador. Puede consultar los de SITEL
        /// </summary>
        /// <param name="pEsFuenteIndicadorFonatel"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerTipoIndicador(FuenteIndicadorEnum pFuenteIndicador)
        {
            RespuestaConsulta<List<TipoIndicador>> resultado = new RespuestaConsulta<List<TipoIndicador>>();

            await Task.Run(() =>
            {
                switch (pFuenteIndicador)
                {
                    case FuenteIndicadorEnum.IndicadorDGF:
                        resultado = tipoIndicadorBL.ObtenerDatos(new TipoIndicador());
                        break;
                    case FuenteIndicadorEnum.IndicadorDGM:
                        resultado = tipoIndicadorBL.ObtenerDatosMercado();
                        break;
                    case FuenteIndicadorEnum.IndicadorDGC:
                        resultado = tipoIndicadorBL.ObtenerDatosCalidad();
                        break;
                    // se conservan debido a futuros cambios
                    //case FuenteIndicadorEnum.IndicadorUIT:
                    //    resultado = tipoIndicadorBL.ObtenerDatosUIT();
                    //    break;
                    //case FuenteIndicadorEnum.IndicadorCruzado:
                    //    resultado = tipoIndicadorBL.ObtenerDatosCruzado();
                    //    break;
                    //case FuenteIndicadorEnum.IndicadorFuenteExterna:
                    //    break;
                    default:
                        resultado.HayError = (int)Error.ErrorSistema;
                        break;
                }
            });

            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 23/11/2022
        /// José Navarro Acuña
        /// Función que retorna las clasificaciones de indicador
        /// </summary>
        /// <param name="pEsFuenteIndicadorFonatel"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerClasificacionIndicador()
        {
            RespuestaConsulta<List<ClasificacionIndicador>> resultado = new RespuestaConsulta<List<ClasificacionIndicador>>();

            await Task.Run(() =>
            {
                resultado = clasificacionIndicadorBL.ObtenerDatos(new ClasificacionIndicador());
            });

            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 28/11/2022
        /// José Navarro Acuña
        /// Función que retorna las diferentes acumulaciones de formulas Fonatel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerAcumulacionesFonatel()
        {
            RespuestaConsulta<List<AcumulacionFormula>> resultado = new RespuestaConsulta<List<AcumulacionFormula>>();

            await Task.Run(() =>
            {
                resultado = acumulacionFormulaBL.ObtenerDatos(new AcumulacionFormula());
            });

            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 24/11/2022
        /// José Navarro Acuña
        /// Función que retorna los servicios de la base de datos de SITEL
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerServicios(FuenteIndicadorEnum pFuenteIndicador)
        {
            RespuestaConsulta<List<ServicioSitel>> resultado = new RespuestaConsulta<List<ServicioSitel>>();

            await Task.Run(() =>
            {
                switch (pFuenteIndicador)
                {
                    case FuenteIndicadorEnum.IndicadorDGM:
                        resultado = servicioSitelBL.ObtenerDatosMercado();
                        break;
                    case FuenteIndicadorEnum.IndicadorDGC:
                        resultado = servicioSitelBL.ObtenerDatosCalidad();
                        break;
                    // se conservan debido a futuros cambios
                    //case FuenteIndicadorEnum.IndicadorUIT:
                    //    resultado = servicioSitelBL.ObtenerDatosUIT();
                    //    break;
                    //case FuenteIndicadorEnum.IndicadorCruzado:
                    //    resultado = servicioSitelBL.ObtenerDatosCruzado();
                    //    break;
                    default:
                        resultado.HayError = (int)Error.ErrorSistema;
                        break;
                }
            });

            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 24/11/2022
        /// José Navarro Acuña
        /// Función que retorna indicadores, ya sea de Fonatel o Sitel
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <param name="pEsFuenteIndicadorFonatel"></param>
        /// <param name="pServicioSitel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ObtenerIndicadores(Indicador pIndicador, FuenteIndicadorEnum pFuenteIndicador, ServicioSitel pServicio)
        {
            RespuestaConsulta<List<Indicador>> resultado = new RespuestaConsulta<List<Indicador>>();

            if (pFuenteIndicador != FuenteIndicadorEnum.IndicadorFuenteExterna)
            {
                if (
                    (string.IsNullOrEmpty(pIndicador.GrupoIndicadores?.id) || string.IsNullOrEmpty(pIndicador.TipoIndicadores?.id))
                    ||
                    (pFuenteIndicador == FuenteIndicadorEnum.IndicadorDGF && string.IsNullOrEmpty(pIndicador.ClasificacionIndicadores?.id))
                    ||
                    (pFuenteIndicador != FuenteIndicadorEnum.IndicadorDGF && string.IsNullOrEmpty(pServicio?.id))
                )
                {
                    resultado.HayError = (int)Error.ErrorControlado;
                    resultado.MensajeError = Errores.CamposIncompletos;
                    return JsonConvert.SerializeObject(resultado);
                }
            }

            await Task.Run(() =>
            {
                switch (pFuenteIndicador)
                {
                    case FuenteIndicadorEnum.IndicadorDGF:
                        resultado = indicadorFonatelBL.ObtenerDatos(pIndicador);
                        break;
                    case FuenteIndicadorEnum.IndicadorDGM:
                        resultado = indicadorFonatelBL.ObtenerDatosMercado(pIndicador, pServicio);
                        break;
                    case FuenteIndicadorEnum.IndicadorDGC:
                        resultado = indicadorFonatelBL.ObtenerDatosCalidad(pIndicador, pServicio);
                        break;
                    //case FuenteIndicadorEnum.IndicadorUIT:
                    //    resultado = indicadorFonatelBL.ObtenerDatosUIT(pIndicador, pServicio);
                    //    break;
                    //case FuenteIndicadorEnum.IndicadorCruzado:
                    //    resultado = indicadorFonatelBL.ObtenerDatosCruzado(pIndicador, pServicio);
                    //    break;
                    //case FuenteIndicadorEnum.IndicadorFuenteExterna:
                    //    resultado = indicadorFonatelBL.ObtenerDatosFuenteExterna();
                    //    break;
                    default:
                        resultado.HayError = (int)Error.ErrorSistema;
                        break;
                }
            });

            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 22/12/2022
        /// José Navarro Acuña
        /// Función que retorna los detalles de un indicador provenientes de una fuente, ya sea, fonatel, mercados, calidad, uit, cruzados o fuente externa
        /// </summary>
        /// <param name="pIdIndicador"></param>
        /// <param name="pFuenteIndicadorEnum"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerVariablesDatoCriteriosIndicador(string pIdIndicador, FuenteIndicadorEnum pFuenteIndicador)
        {
            RespuestaConsulta<List<DetalleIndicadorVariable>> resultado = new RespuestaConsulta<List<DetalleIndicadorVariable>>();

            if (string.IsNullOrEmpty(pIdIndicador))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            DetalleIndicadorVariable detalleIndicador = new DetalleIndicadorVariable()
            {
                idIndicadorString = pIdIndicador
            };

            await Task.Run(() =>
            {
                switch (pFuenteIndicador)
                {
                    case FuenteIndicadorEnum.IndicadorDGF:
                        resultado = detalleIndicadorVariablesBL.ObtenerDatos(detalleIndicador);
                        break;
                    case FuenteIndicadorEnum.IndicadorDGM:
                        resultado = detalleIndicadorCriteriosSitelBL.ObtenerDatosMercado(detalleIndicador);
                        break;
                    case FuenteIndicadorEnum.IndicadorDGC:
                        resultado = detalleIndicadorCriteriosSitelBL.ObtenerDatosCalidad(detalleIndicador);
                        break;
                    //case FuenteIndicadorEnum.IndicadorUIT:
                    //    resultado = detalleIndicadorCriteriosSitelBL.ObtenerDatosUIT(detalleIndicador);
                    //    break;
                    //case FuenteIndicadorEnum.IndicadorCruzado:
                    //    resultado = detalleIndicadorCriteriosSitelBL.ObtenerDatosCruzado(detalleIndicador);
                    //    break;
                    //case FuenteIndicadorEnum.IndicadorFuenteExterna:
                    //    resultado = detalleIndicadorCriteriosSitelBL.ObtenerDatosExterno(detalleIndicador);
                    //    break;
                    default:
                        resultado.HayError = (int)Error.ErrorSistema;
                        break;
                }
            });
            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// Función que permite crear los detalles matematicos de la fórmula de cálculo.
        /// Se debe ingresar la fórmula asociada a los argumentos, e ingresar el listado de dichos argumentos
        /// </summary>
        /// <param name="pFormulaCalculo"></param>
        /// <param name="pListaArgumentos"></param>
        /// <returns></returns
        [HttpPost]
        public async Task<string> CrearDetallesFormulaCalculo(FormulaCalculo pFormulaCalculo, List<ArgumentoConstruidoDTO> pListaArgumentos)
        {
            modoFormulario = (string)Session[keyModoFormulario];

            if (modoFormulario.Equals(((int)Accion.Visualizar).ToString()))
            {
                return JsonConvert.SerializeObject(new RespuestaConsulta<FormulaCalculo>() { HayError = (int)Error.ErrorSistema });

            }
            RespuestaConsulta<FormulaCalculo> resultado = new RespuestaConsulta<FormulaCalculo>();

            if (string.IsNullOrEmpty(pFormulaCalculo.id))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.CamposIncompletos;
                return JsonConvert.SerializeObject(resultado);
            }

            if (pListaArgumentos == null) pListaArgumentos = new List<ArgumentoConstruidoDTO>();

            pFormulaCalculo.UsuarioCreacion = usuario;

            await Task.Run(() =>
            {
                resultado = formulaBL.InsertarDetallesFormulaCalculo(pFormulaCalculo, pListaArgumentos);
            });

            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 09/01/2023
        /// José Navarro Acuña
        /// Función que permite cargar los tipos de fechas para la defición de fechas de un argumento
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerTiposFechasDefinicion()
        {
            RespuestaConsulta<List<FormulaCalculoTipoFecha>> resultado = new RespuestaConsulta<List<FormulaCalculoTipoFecha>>();

            await Task.Run(() =>
            {
                resultado = formulasCalculoTipoFechaBL.ObtenerDatos(new FormulaCalculoTipoFecha());
            });
            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 10/01/2023
        /// José Navarro Acuña
        /// Función que consulta los argumentos de una fórmula de cálculo
        /// </summary>
        /// <param name="pIdFormula"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ConsultarArgumentosDeFormula(string pIdFormula) 
        {
            RespuestaConsulta<List<ArgumentoConstruidoDTO>> resultado = new RespuestaConsulta<List<ArgumentoConstruidoDTO>>();

            if (string.IsNullOrEmpty(pIdFormula))
            {
                return JsonConvert.SerializeObject(new RespuestaConsulta<FormulaCalculo>() { HayError = (int)Error.ErrorSistema });
            }

            await Task.Run(() =>
            {
                resultado = argumentoFormulaBL.ObtenerArgumentosCompletosDeFormula(pIdFormula);
            });

            return JsonConvert.SerializeObject(resultado);
        }

        #endregion

        #region Funciones privadas

        /// <summary>
        /// 13/10/2022
        /// José Navarro Acuña
        /// Método que permite cargar los datos necesarios del formulario de Fórmulas de Cálculo
        /// </summary>
        private void CargarDatosEnVistas(Accion pAccionPantalla, FormulaCalculo pFormulasDeCalculo)
        {
            ViewBag.VariablesDato = Enumerable.Empty<SelectListItem>();
            ViewBag.CategoriasDeIndicador = Enumerable.Empty<SelectListItem>();
            ViewBag.FuentesIndicador = Enumerable.Empty<SelectListItem>();
            ViewBag.GruposFonatel = Enumerable.Empty<SelectListItem>();
            ViewBag.ClasificacionesFonatel = Enumerable.Empty<SelectListItem>();
            ViewBag.Servicios = Enumerable.Empty<SelectListItem>();
            ViewBag.TiposFonatel = Enumerable.Empty<SelectListItem>();
            ViewBag.Indicadores = Enumerable.Empty<SelectListItem>();
            ViewBag.Acumulaciones = Enumerable.Empty<SelectListItem>();
            ViewBag.IndicadorSalida = Enumerable.Empty<SelectListItem>();
            ViewBag.TiposFechaInicioModalFecha = Enumerable.Empty<SelectListItem>();
            ViewBag.CategoriasTipoFechaInicioModalFecha = Enumerable.Empty<SelectListItem>();
            ViewBag.TiposFechaFinalModalFecha = Enumerable.Empty<SelectListItem>();
            ViewBag.CategoriasTipoFechaFinalModalFecha = Enumerable.Empty<SelectListItem>();
            ViewBag.FrecuenciaEnvio = frecuenciaEnvioBL.ObtenerDatos(new FrecuenciaEnvio() { }).objetoRespuesta;

            // Modal detalle de agregación/agrupación
            ViewBag.CategoriasModalDetalle = Enumerable.Empty<SelectListItem>();
            ViewBag.CriteriosModalDetalle = Enumerable.Empty<SelectListItem>();
            ViewBag.DetallesModalDetalle = Enumerable.Empty<SelectListItem>();

            // Bandera para determinar si la fórmula ejecutó
            ViewBag.LaFormulaHaSidoEjecutada = false;

            string idFormula = pAccionPantalla == Accion.Editar || pAccionPantalla == Accion.Visualizar ? pFormulasDeCalculo.id : string.Empty; // en caso de editar/visualizar la opción debe estar disponible

            // Indicador de salida
            List<Indicador> indicadoresDeSalida = indicadorFonatelBL.ObtenerIndicadoresSalidaParaFormulasCalculo(idFormula).objetoRespuesta;

            if (indicadoresDeSalida != null && indicadoresDeSalida.Count > 0)
            {
                ViewBag.IndicadorSalida = indicadoresDeSalida;
            }

            if (!string.IsNullOrEmpty(pFormulasDeCalculo.IdIndicadorSalidaString))
            {
                // Variables dato de salida
                List<DetalleIndicadorVariable> detalles = detalleIndicadorVariablesBL.ObtenerVariablesSinUsoEnFormula(new DetalleIndicadorVariable()
                {
                    idIndicadorString = pFormulasDeCalculo.IdIndicadorSalidaString
                }, 
                idFormula).objetoRespuesta;

                if (detalles != null)
                {
                    ViewBag.VariablesDato = detalles;
                }
            }

            // Nivel de cálculo
            if (!pFormulasDeCalculo.NivelCalculoTotal)
            {
                List<CategoriaDesagregacion> categorias = categoriasDesagregacionBL.ObtenerCategoriasDesagregacionDeIndicador(pFormulasDeCalculo.IdIndicadorSalidaString, true).objetoRespuesta;

                if (categorias != null)
                {
                    ViewBag.CategoriasDeIndicador = categorias;
                }
            }

            if (!string.IsNullOrEmpty(pFormulasDeCalculo.id))
            {
                ViewBag.LaFormulaHaSidoEjecutada = !string.IsNullOrEmpty(formulaBL.VerificarSiFormulaEjecuto(pFormulasDeCalculo.id).objetoRespuesta);
            }
        }

        /// <summary>
        /// 21/10/2022
        /// José Navarro Acuña
        /// Permite verificar los datos requeridos y el formato de los campos de una formula de cálcula al momento de crear
        /// </summary>
        /// <param name="pFormulaCalculo"></param>
        /// <param name="pEsGuardadoParcial"></param>
        /// <returns></returns>
        private string ValidarObjectoCrearFormulaCalculo(FormulaCalculo pFormulaCalculo)
        {
            if (!pFormulaCalculo.EsGuardadoParcial)
            {
                if (
                    string.IsNullOrEmpty(pFormulaCalculo.IdFrecuenciaString) ||
                    pFormulaCalculo.FechaCalculo == null ||
                    pFormulaCalculo.Descripcion == null || string.IsNullOrEmpty(pFormulaCalculo.Descripcion.Trim()) ||
                    string.IsNullOrEmpty(pFormulaCalculo.IdIndicadorSalidaString) ||
                    string.IsNullOrEmpty(pFormulaCalculo.IdVariableDatoString) ||
                    (!pFormulaCalculo.NivelCalculoTotal && pFormulaCalculo.ListaCategoriasNivelesCalculo.Count < 1)
                )
                {
                    return Errores.CamposIncompletos;
                }
            }

            if (pFormulaCalculo.Codigo == null || string.IsNullOrEmpty(pFormulaCalculo.Codigo.Trim())) // campo requerido
            {
                return string.Format(Errores.CampoRequeridoV2, EtiquetasViewFormulasCalculo.CrearFormula_LabelCodigo);
            }
            else if (!Utilidades.rx_alfanumerico.Match(pFormulaCalculo.Codigo.Trim()).Success // validar formato
                || pFormulaCalculo.Codigo.Trim().Length > 30)
            {
                return string.Format(Errores.CampoConFormatoInvalido, EtiquetasViewFormulasCalculo.CrearFormula_LabelCodigo);
            }

            if (pFormulaCalculo.Nombre == null || string.IsNullOrEmpty(pFormulaCalculo.Nombre.Trim())) // campo requerido
            {
                return string.Format(Errores.CampoRequeridoV2, EtiquetasViewFormulasCalculo.CrearFormula_LabelNombre);
            }
            else if (!Utilidades.rx_alfanumerico.Match(pFormulaCalculo.Nombre.Trim()).Success // validar formato
                || pFormulaCalculo.Nombre.Trim().Length > 500)
            {
                return string.Format(Errores.CampoConFormatoInvalido, EtiquetasViewFormulasCalculo.CrearFormula_LabelNombre);
            }

            if (pFormulaCalculo.FechaCalculo <= DateTime.MinValue || pFormulaCalculo.FechaCalculo >= DateTime.MaxValue)
            {
                return string.Format(Errores.CampoRequeridoV2, EtiquetasViewFormulasCalculo.CrearFormula_LabelFechaCalculo);
            }

            if (!string.IsNullOrEmpty(pFormulaCalculo.Descripcion?.Trim())) // ¿se ingresó el dato?
            {
                if (!Utilidades.rx_soloTexto.Match(pFormulaCalculo.Descripcion).Success               // la descripción solo debe contener texto como valor
                    || pFormulaCalculo.Descripcion.Trim().Length > 1500)                               // validar la cantidad de caracteres
                {
                    return string.Format(Errores.CampoConFormatoInvalido, EtiquetasViewFormulasCalculo.CrearFormula_LabelDescripcion);
                }
            }

            return null;
        }

        #endregion
    }
}
