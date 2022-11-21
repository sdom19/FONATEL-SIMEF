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
        private readonly DetalleIndicadorVariablesBL detalleIndicadorVariablesBL;
        private readonly CategoriasDesagregacionBL categoriasDesagregacionBL;
        private readonly FuenteIndicadorBL fuenteIndicadorBL;
        private readonly GrupoIndicadorBL grupoIndicadorBL;
        private readonly TipoIndicadorBL tipoIndicadorBL;
        private readonly ClasificacionIndicadorBL clasificacionIndicadorBL;
        private readonly ServicioSitelBL servicioSitelBL;
        private readonly AcumulacionFormulaBL acumulacionFormulaBL;

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
            categoriasDesagregacionBL = new CategoriasDesagregacionBL(nombreVista, usuario);
            fuenteIndicadorBL = new FuenteIndicadorBL(nombreVista, usuario);
            grupoIndicadorBL = new GrupoIndicadorBL(nombreVista, usuario);
            tipoIndicadorBL = new TipoIndicadorBL(nombreVista, usuario);
            clasificacionIndicadorBL = new ClasificacionIndicadorBL(nombreVista, usuario);
            servicioSitelBL = new ServicioSitelBL();
            acumulacionFormulaBL = new AcumulacionFormulaBL(nombreVista, usuario);
        }

        #region Eventos de la página

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
        public ActionResult Create()
        {
            CargarDatosEnVistas();
            ViewBag.ModoFormulario = ((int)Accion.Insertar).ToString();
            ViewBag.TituloVista = EtiquetasViewFormulasCalculo.TituloCrear;

            return View(new FormulasCalculo());
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return View("Index");

            FormulasCalculo objFormulaCalculo = null;

            try
            {
                objFormulaCalculo = formulaBL.ObtenerDatos(new FormulasCalculo() { id = id }).objetoRespuesta.Single();
            }
            catch (Exception) { }

            if (objFormulaCalculo == null)
                return View("Index");

            CargarDatosEnVistas(objFormulaCalculo.IdIndicadorSalidaString, objFormulaCalculo.NivelCalculoTotal);

            ViewBag.ModoFormulario = ((int)Accion.Editar).ToString();
            ViewBag.TituloVista = EtiquetasViewFormulasCalculo.TituloEditar;

            return View("Create", objFormulaCalculo);
        }

        // GET: Solicitud/Clone/5

        [HttpGet]
        public ActionResult Clone(string id)
        {
            if (string.IsNullOrEmpty(id))
                return View("Index");

            FormulasCalculo objFormulaCalculo = null;

            try
            {
                objFormulaCalculo = formulaBL.ObtenerDatos(new FormulasCalculo() { id = id }).objetoRespuesta.Single();
            }
            catch (Exception) { }

            if (objFormulaCalculo == null)
                return View("Index");

            objFormulaCalculo.Nombre = string.Empty;
            objFormulaCalculo.Codigo = string.Empty;

            CargarDatosEnVistas(objFormulaCalculo.IdIndicadorSalidaString, objFormulaCalculo.NivelCalculoTotal);

            ViewBag.ModoFormulario = ((int)Accion.Clonar).ToString();
            ViewBag.TituloVista = EtiquetasViewFormulasCalculo.TituloClonar;

            return View("Create", objFormulaCalculo);
        }

        [HttpGet]
        public ActionResult Visualize(string id)
        {
            if (string.IsNullOrEmpty(id))
                return View("Index");

            FormulasCalculo objFormulaCalculo = null;

            try
            {
                objFormulaCalculo = formulaBL.ObtenerDatos(new FormulasCalculo() { id = id }).objetoRespuesta.Single();
            }
            catch (Exception) { }

            if (objFormulaCalculo == null)
                return View("Index");

            CargarDatosEnVistas(objFormulaCalculo.IdIndicadorSalidaString, objFormulaCalculo.NivelCalculoTotal);

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
            RespuestaConsulta<List<FormulasCalculo>> result = null;
            await Task.Run(() =>
            {
                result = formulaBL.ObtenerDatos(new FormulasCalculo());
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
        public async Task<string> EliminarFormula(FormulasCalculo pFormulaCalculo)
        {
            RespuestaConsulta<List<FormulasCalculo>> resultado = new RespuestaConsulta<List<FormulasCalculo>>();

            if (string.IsNullOrEmpty(pFormulaCalculo.id))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            await Task.Run(() =>
            {
                resultado = formulaBL.CambioEstado(new FormulasCalculo() { 
                    id = pFormulaCalculo.id, IdEstado = (int)EstadosRegistro.Eliminado
                });
            });
            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// Permite activar una fórmula
        /// </summary>
        /// <param name="formulaCalculo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ActivarFormula(FormulasCalculo pFormulaCalculo)
        {
            RespuestaConsulta<List<FormulasCalculo>> resultado = new RespuestaConsulta<List<FormulasCalculo>>();

            if (string.IsNullOrEmpty(pFormulaCalculo.id))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            await Task.Run(() =>
            {
                resultado = formulaBL.CambioEstado(new FormulasCalculo()
                {
                    id = pFormulaCalculo.id,
                    IdEstado = (int)EstadosRegistro.Activo
                });
            });
            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// Permite desactivar una fórmula
        /// </summary>
        /// <param name="formulaCalculo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> DesactivarFormula(FormulasCalculo pFormulaCalculo)
        {
            RespuestaConsulta<List<FormulasCalculo>> resultado = new RespuestaConsulta<List<FormulasCalculo>>();

            if (string.IsNullOrEmpty(pFormulaCalculo.id))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            await Task.Run(() =>
            {
                resultado = formulaBL.CambioEstado(new FormulasCalculo()
                {
                    id = pFormulaCalculo.id,
                    IdEstado = (int)EstadosRegistro.Desactivado
                });
            });
            return JsonConvert.SerializeObject(resultado);
        }

        #endregion

        #region Funciones async - Create

        /// <summary>
        /// 18/10/20122
        /// José Navarro Acuña
        /// Obtiene un listado de las variables dato de un indicador
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerVariablesDatoDeIndicador(string pIdIndicador)
        {
            RespuestaConsulta<List<DetalleIndicadorVariables>> resultado = new RespuestaConsulta<List<DetalleIndicadorVariables>>();

            if (string.IsNullOrEmpty(pIdIndicador))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            await Task.Run(() =>
            {
                resultado = detalleIndicadorVariablesBL.ObtenerDatos(new DetalleIndicadorVariables()
                {
                    idIndicadorString = pIdIndicador
                });
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
            RespuestaConsulta<List<CategoriasDesagregacion>> resultado = new RespuestaConsulta<List<CategoriasDesagregacion>>();

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
        /// 17/11/2022
        /// José Navarro Acuña
        /// Obtiene un listado de las categorias de desagregación relacionadas a una formula respecto al nivel de calculo
        /// </summary>
        /// <param name="pIdFormula"></param>
        /// <param name="pIdIndicador"></param>
        /// <returns></returns>
        public async Task<string> ObtenerCategoriasDeFormulaNivelCalculo(string pIdFormula, string pIdIndicador)
        {
            RespuestaConsulta<List<CategoriasDesagregacion>> resultado = new RespuestaConsulta<List<CategoriasDesagregacion>>();

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
        /// <param name="pFormulasCalculo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CrearFormulaCalculo(FormulasCalculo pFormulaCalculo)
        {
            modoFormulario = (string)Session[keyModoFormulario];

            if (modoFormulario.Equals(((int)Accion.Visualizar).ToString()))
            {
                return JsonConvert.SerializeObject(new RespuestaConsulta<FormulasCalculo>() { HayError = (int)Error.ErrorSistema });
            }

            string mensajesValidacion = ValidarObjectoCrearFormulaCalculo(pFormulaCalculo);

            if (!string.IsNullOrEmpty(mensajesValidacion))
            {
                return JsonConvert.SerializeObject(
                    new RespuestaConsulta<FormulasCalculo>() { HayError = (int)Error.ErrorControlado, MensajeError = mensajesValidacion });
            }

            pFormulaCalculo.IdEstado = (int)EstadosRegistro.EnProceso;
            pFormulaCalculo.UsuarioCreacion = usuario;
            pFormulaCalculo.IdFormula = 0;
            pFormulaCalculo.IdFrecuencia = 0;
            pFormulaCalculo.IdIndicador = 0;
            pFormulaCalculo.IdIndicadorVariable = 0;

            RespuestaConsulta<List<FormulasCalculo>> resultado = new RespuestaConsulta<List<FormulasCalculo>>();

            await Task.Run(() =>
            {
                resultado = formulaBL.InsertarDatos(pFormulaCalculo);
            });
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
        public async Task<string> EditarFormulaCalculo(FormulasCalculo pFormulaCalculo)
        {
            modoFormulario = (string)Session[keyModoFormulario];

            if (modoFormulario.Equals(((int)Accion.Visualizar).ToString()))
            {
                return JsonConvert.SerializeObject(new RespuestaConsulta<FormulasCalculo>() { HayError = (int)Error.ErrorSistema });
            }

            if (string.IsNullOrEmpty(pFormulaCalculo.id))
            {
                return JsonConvert.SerializeObject(
                    new RespuestaConsulta<List<FormulasCalculo>>() { HayError = (int)Error.ErrorControlado, MensajeError = Errores.NoRegistrosActualizar });
            }

            // tanto crear, clonar y editar pueden actualizar la fórmula, asi que se debe diferenciar la acción
            if (modoFormulario.Equals(((int)Accion.Editar).ToString()))
            {
                try
                {
                    FormulasCalculo objFormular = formulaBL.ObtenerDatos(new FormulasCalculo() { id = pFormulaCalculo.id }).objetoRespuesta.FirstOrDefault();
                    pFormulaCalculo.Codigo = objFormular.Codigo;
                }
                catch (Exception)
                {
                    return JsonConvert.SerializeObject(
                        new RespuestaConsulta<List<Indicador>>() { HayError = (int)Error.ErrorControlado, MensajeError = Errores.NoRegistrosActualizar });
                };
            }

            pFormulaCalculo.UsuarioModificacion = usuario;
            return await CrearFormulaCalculo(pFormulaCalculo);
        }

        /// <summary>
        /// 21/11/2022
        /// José Navarro Acuña
        /// Función que permite clonar una fórmula de cálculo
        /// </summary>
        /// <param name="pFormulaCalculo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ClonarFormulaCalculo(FormulasCalculo pFormulaCalculo)
        {
            modoFormulario = (string)Session[keyModoFormulario];

            if (modoFormulario.Equals(((int)Accion.Visualizar).ToString()))
            {
                return JsonConvert.SerializeObject(new RespuestaConsulta<FormulasCalculo>() { HayError = (int)Error.ErrorSistema });
            }

            if (string.IsNullOrEmpty(pFormulaCalculo.id)) // id indicador requerido
            {
                return JsonConvert.SerializeObject(
                    new RespuestaConsulta<List<Indicador>>() { HayError = (int)Error.ErrorControlado, MensajeError = Errores.NoRegistrosActualizar });
            }

            string idFormulaAClonar = pFormulaCalculo.id; // id de la formula seleccionada para clonar
            pFormulaCalculo.id = string.Empty;
            pFormulaCalculo.IdFormula = 0;

            string creacionFormula = await CrearFormulaCalculo(pFormulaCalculo);
            RespuestaConsulta<List<FormulasCalculo>> formulaDeserializado = JsonConvert.DeserializeObject<RespuestaConsulta<List<FormulasCalculo>>>(creacionFormula);

            if (formulaDeserializado.HayError != (int)Error.NoError) // se creó el indicador correctamente?
            {
                return creacionFormula;
            }

            RespuestaConsulta<FormulasCalculo> resultado = new RespuestaConsulta<FormulasCalculo>();

            await Task.Run(() =>
            {
                // clonar los detalles del paso 2 del formulario
            });

            return JsonConvert.SerializeObject(resultado);
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
        public async Task<string> ObtenerGrupoIndicador(bool pEsFuenteIndicadorFonatel)
        {
            RespuestaConsulta<List<GrupoIndicadores>> resultado = new RespuestaConsulta<List<GrupoIndicadores>>();

            await Task.Run(() =>
            {
                if (pEsFuenteIndicadorFonatel)
                {
                    resultado = grupoIndicadorBL.ObtenerDatos(new GrupoIndicadores());
                }
                else // SITEL
                {

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
        public async Task<string> ObtenerTipoIndicador(bool pEsFuenteIndicadorFonatel)
        {
            RespuestaConsulta<List<TipoIndicadores>> resultado = new RespuestaConsulta<List<TipoIndicadores>>();

            await Task.Run(() =>
            {
                if (pEsFuenteIndicadorFonatel)
                {
                    resultado = tipoIndicadorBL.ObtenerDatos(new TipoIndicadores());
                }
                else // SITEL
                {
                    resultado = tipoIndicadorBL.ObtenerDatosSitel(new TipoIndicadores());
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
            RespuestaConsulta<List<ClasificacionIndicadores>> resultado = new RespuestaConsulta<List<ClasificacionIndicadores>>();

            await Task.Run(() =>
            {
                 resultado = clasificacionIndicadorBL.ObtenerDatos(new ClasificacionIndicadores());
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
        public async Task<string> ObtenerServiciosSitel()
        {
            RespuestaConsulta<List<ServicioSitel>> resultado = new RespuestaConsulta<List<ServicioSitel>>();

            await Task.Run(() =>
            {
                resultado = servicioSitelBL.ObtenerDatos(new ServicioSitel());
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
        public async Task<string> ObtenerIndicadores(Indicador pIndicador, bool pEsFuenteIndicadorFonatel, ServicioSitel pServicioSitel = null)
        {
            RespuestaConsulta<List<Indicador>> resultado = new RespuestaConsulta<List<Indicador>>();

            if (
                (string.IsNullOrEmpty(pIndicador.GrupoIndicadores?.id) || string.IsNullOrEmpty(pIndicador.TipoIndicadores?.id)) 
                ||
                (pEsFuenteIndicadorFonatel && string.IsNullOrEmpty(pIndicador.ClasificacionIndicadores?.id))
                ||
                (!pEsFuenteIndicadorFonatel && string.IsNullOrEmpty(pServicioSitel?.id))
            )
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.CamposIncompletos;
                return JsonConvert.SerializeObject(resultado);
            }

            await Task.Run(() =>
            {
                if (pEsFuenteIndicadorFonatel)
                {
                    resultado = indicadorFonatelBL.ObtenerDatos(pIndicador);
                }
                else
                {
                    resultado = indicadorFonatelBL.ObtenerDatosSitel(pIndicador, pServicioSitel);
                }
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
        private void CargarDatosEnVistas(string pIdIndicador = null, bool pEsNivelCalculoTotal = true)
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

            ViewBag.FrecuenciaEnvio = frecuenciaEnvioBL.ObtenerDatos(new FrecuenciaEnvio() { }).objetoRespuesta;
            ViewBag.IndicadorSalida = indicadorFonatelBL.ObtenerDatos(new Indicador() { }).objetoRespuesta
                .Where(y => y.IdClasificacion == (int)ClasificacionIndicadorEnum.Salida || y.IdClasificacion == (int)ClasificacionIndicadorEnum.EntradaSalida)
                .Select(x => new Indicador()
                {
                    id = x.id,
                    Nombre = Utilidades.ConcatenadoCombos(x.Codigo, x.Nombre)
                }).ToList();

            if (!string.IsNullOrEmpty(pIdIndicador))
            {
                List<DetalleIndicadorVariables> detalles = detalleIndicadorVariablesBL.ObtenerDatos(new DetalleIndicadorVariables()
                {
                    idIndicadorString = pIdIndicador
                }).objetoRespuesta;

                if (detalles != null)
                {
                    ViewBag.VariablesDato = detalles;
                }
            }

            if (!pEsNivelCalculoTotal)
            {
                List<CategoriasDesagregacion> categorias = categoriasDesagregacionBL.ObtenerCategoriasDesagregacionDeIndicador(pIdIndicador, true).objetoRespuesta;

                if (categorias != null)
                {
                    ViewBag.CategoriasDeIndicador = categorias;
                }
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
        public string ValidarObjectoCrearFormulaCalculo(FormulasCalculo pFormulaCalculo)
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
            else if (!Utilidades.rx_soloTexto.Match(pFormulaCalculo.Nombre.Trim()).Success // validar formato
                || pFormulaCalculo.Nombre.Trim().Length > 300)
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
