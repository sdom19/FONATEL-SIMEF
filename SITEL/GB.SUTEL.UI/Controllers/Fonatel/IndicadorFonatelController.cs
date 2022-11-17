using GB.SIMEF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using GB.SIMEF.BL;
using Microsoft.AspNet.Identity;
using GB.SIMEF.Resources;
using static GB.SIMEF.Resources.Constantes;
using GB.SUTEL.UI.Helpers;

namespace GB.SUTEL.UI.Controllers.Fonatel
{
    [AuthorizeUserAttribute]
    public class IndicadorFonatelController : Controller
    {
        private readonly IndicadorFonatelBL indicadorBL;
        private readonly TipoIndicadorBL tipoIndicadorBL;
        private readonly GrupoIndicadorBL grupoIndicadorBL;
        private readonly UnidadEstudioBL unidadEstudioBL;
        private readonly FrecuenciaEnvioBL frecuenciaEnvioBL;
        private readonly ClasificacionIndicadorBL clasificacionIndicadorBL;
        private readonly TipoMedidaBL tipoMedidaBL;
        private readonly DetalleIndicadorVariablesBL detalleIndicadorVariablesBL;
        private readonly DetalleIndicadorCategoriaBL detalleIndicadorCategoriaBL;
        private readonly CategoriasDesagregacionBL categoriasDesagregacionBL;
        private readonly DetalleCategoriasTextoBL detalleCategoriasTextoBL;
        private readonly string defaultDropDownValue;
        private readonly string usuario = string.Empty;
        private readonly string view = string.Empty;


        public IndicadorFonatelController()
        {
            usuario = System.Web.HttpContext.Current.User.Identity.GetUserId();
            view = EtiquetasViewIndicadorFonatel.TituloIndex;
            indicadorBL = new IndicadorFonatelBL(view, usuario);
            tipoIndicadorBL = new TipoIndicadorBL(view, usuario);
            grupoIndicadorBL = new GrupoIndicadorBL(view, usuario);
            unidadEstudioBL = new UnidadEstudioBL(view, usuario);
            frecuenciaEnvioBL = new FrecuenciaEnvioBL(view, usuario);
            clasificacionIndicadorBL = new ClasificacionIndicadorBL(view, usuario);
            tipoMedidaBL = new TipoMedidaBL(view, usuario);
            detalleIndicadorVariablesBL = new DetalleIndicadorVariablesBL(view, usuario);
            detalleIndicadorCategoriaBL = new DetalleIndicadorCategoriaBL(view, usuario);
            categoriasDesagregacionBL = new CategoriasDesagregacionBL(view, usuario);
            detalleCategoriasTextoBL = new DetalleCategoriasTextoBL(view, usuario);

            defaultDropDownValue = Utilidades.GetDefaultDropDownValue();
        }

        #region Eventos de la página

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Detalle(int id)
        {
            return View();
        }

        [HttpGet]
        public ActionResult DetalleVariables(int id)
        {
            return View();
        }


        public ActionResult DetalleCategoria(int id)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            CargarDatosEnVistas();
            ViewBag.ModoFormulario = ((int) Accion.Insertar).ToString();
            ViewBag.TituloVista = EtiquetasViewIndicadorFonatel.TituloCrearIndicador;

            return View();
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            CargarDatosEnVistas();
            ViewBag.ModoFormulario = ((int) Accion.Editar).ToString();
            ViewBag.TituloVista = EtiquetasViewIndicadorFonatel.TituloEditarIndicador;

            if (string.IsNullOrEmpty(id))
                return View("Index");

            Indicador objIndicador = null;
            try
            {
                objIndicador = indicadorBL.ObtenerDatos(new Indicador() { id = id }).objetoRespuesta.FirstOrDefault();
            }
            catch (Exception) { };

            if (objIndicador == null)
                return View("Index");

            return View("Create", objIndicador);
        }

        [HttpGet]
        public ActionResult Clone(string id)
        {
            CargarDatosEnVistas();
            ViewBag.ModoFormulario = ((int) Accion.Clonar).ToString();
            ViewBag.TituloVista = EtiquetasViewIndicadorFonatel.TituloClonarIndicador;

            if (string.IsNullOrEmpty(id))
                return View("Index");

            Indicador objIndicador = null;
            try
            {
                objIndicador = indicadorBL.ObtenerDatos(new Indicador() { id = id }).objetoRespuesta.FirstOrDefault();
            }
            catch (Exception) { };

            if (objIndicador == null)
                return View("Index");

            objIndicador.Codigo = string.Empty;
            objIndicador.Nombre = string.Empty;

            return View("Create", objIndicador);
        }

        #endregion

        #region Métodos de async

        /// <summary>
        /// 10/08/2022
        /// José Navarro Acuña
        /// Función que retorna todos los indicadores registrados en el sistema
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerListaIndicadores()
        {
            RespuestaConsulta<List<Indicador>> resultado = new RespuestaConsulta<List<Indicador>>();
            await Task.Run(() =>
            {
                resultado = indicadorBL.ObtenerDatos(new Indicador());
            });
            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 10/08/2022
        /// José Navarro Acuña
        /// Función que permite realizar un eliminado lógico de un indicador
        /// </summary>
        /// <param name="pIdIndicador"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> EliminarIndicador(string pIdIndicador)
        {
            RespuestaConsulta<List<Indicador>> resultado = new RespuestaConsulta<List<Indicador>>();

            if (string.IsNullOrEmpty(pIdIndicador))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            await Task.Run(() =>
            {
                resultado = indicadorBL.CambioEstado(new Indicador()
                {
                    id = pIdIndicador,
                    nuevoEstado = (int)EstadosRegistro.Eliminado // nuevo estado
                });

            });
            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 17/08/2022
        /// José Navarro Acuña
        /// Función que permite desactivar un indicador
        /// </summary>
        /// <param name="pIdIndicador"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> DesactivarIndicador(string pIdIndicador)
        {
            RespuestaConsulta<List<Indicador>> resultado = new RespuestaConsulta<List<Indicador>>();

            if (string.IsNullOrEmpty(pIdIndicador))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            await Task.Run(() =>
            {
                resultado = indicadorBL.CambioEstado(new Indicador()
                {
                    id = pIdIndicador,
                    nuevoEstado = (int)EstadosRegistro.Desactivado // nuevo estado
                });

            });
            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 10/08/2022
        /// José Navarro Acuña
        /// Función que permite activar un indicador
        /// </summary>
        /// <param name="pIdIndicador"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ActivarIndicador(string pIdIndicador)
        {
            RespuestaConsulta<List<Indicador>> resultado = new RespuestaConsulta<List<Indicador>>();

            if (string.IsNullOrEmpty(pIdIndicador))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            await Task.Run(() =>
            {
                resultado = indicadorBL.CambioEstado(new Indicador()
                {
                    id = pIdIndicador,
                    nuevoEstado = (int)EstadosRegistro.EnProceso // nuevo estado
                });

            });
            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 08/09/2022
        /// José Navarro Acuña
        /// Función que verifica si el indicador se encuentra en algún formulario web o una formula de calculo
        /// </summary>
        /// <param name="pIdIndicador"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> VerificarUsoIndicador(string pIdIndicador)
        {
            RespuestaConsulta<List<string>> resultado = new RespuestaConsulta<List<string>>();
            
            if (string.IsNullOrEmpty(pIdIndicador))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            await Task.Run(() =>
            {
                resultado = indicadorBL.VerificarUsoIndicador(new Indicador()
                {
                    id = pIdIndicador
                });

            });
            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 18/08/2022
        /// José Navarro Acuña
        /// Función que obtiene un listado de los tipos de indicadores registrados en el sistema
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerListaTipoIndicador()
        {
            RespuestaConsulta<List<TipoIndicadores>> resultado = new RespuestaConsulta<List<TipoIndicadores>>();
            await Task.Run(() =>
            {
                resultado = tipoIndicadorBL.ObtenerDatos(new TipoIndicadores());
            });

            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 18/08/2022
        /// José Navarro Acuña
        /// Función que obtiene un listado de los tipos de indicadores registrados en el sistema
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerListaGrupoIndicador()
        {
            RespuestaConsulta<List<GrupoIndicadores>> resultado = new RespuestaConsulta<List<GrupoIndicadores>>();
            await Task.Run(() =>
            {
                resultado = grupoIndicadorBL.ObtenerDatos(new GrupoIndicadores());
            });

            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 19/08/2022
        /// José Navarro Acuña
        /// Función que obtiene un listado de las unidades de estudio registradas en el sistema
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerListaUnidadEstudio()
        {
            RespuestaConsulta<List<UnidadEstudio>> resultado = new RespuestaConsulta<List<UnidadEstudio>>();
            await Task.Run(() =>
            {
                resultado = unidadEstudioBL.ObtenerDatos(new UnidadEstudio());
            });

            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 19/08/2022
        /// José Navarro Acuña
        /// Función que realiza un eliminado lógico de un tipo de indicador
        /// </summary>
        /// <param name="pIdTipoIndicador"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> EliminarTipoIndicador(string pIdTipoIndicador)
        {
            RespuestaConsulta<List<TipoIndicadores>> resultado = new RespuestaConsulta<List<TipoIndicadores>>();

            if (string.IsNullOrEmpty(pIdTipoIndicador))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            await Task.Run(() =>
            {
                resultado = tipoIndicadorBL.CambioEstado(new TipoIndicadores()
                {
                    id = pIdTipoIndicador,
                    nuevoEstado = false // eliminar
                });

            });
            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 19/08/2022
        /// José Navarro Acuña
        /// Función que realiza un eliminado lógico de un grupo de indicador
        /// </summary>
        /// <param name="pIdTipoIndicador"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> EliminarGrupoIndicador(string pIdGrupoIndicador)
        {
            RespuestaConsulta<List<GrupoIndicadores>> resultado = new RespuestaConsulta<List<GrupoIndicadores>>();

            if (string.IsNullOrEmpty(pIdGrupoIndicador))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            await Task.Run(() =>
            {
                resultado = grupoIndicadorBL.CambioEstado(new GrupoIndicadores()
                {
                    id = pIdGrupoIndicador,
                    nuevoEstado = false // eliminar
                });

            });
            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 19/08/2022
        /// José Navarro Acuña
        /// Función que realiza un eliminado lógico de una unidad de estudio
        /// </summary>
        /// <param name="pIdTipoIndicador"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> EliminarUnidadEstudio(string pIdUnidadEstudio)
        {
            RespuestaConsulta<List<UnidadEstudio>> resultado = new RespuestaConsulta<List<UnidadEstudio>>();

            if (string.IsNullOrEmpty(pIdUnidadEstudio))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            await Task.Run(() =>
            {
                resultado = unidadEstudioBL.CambioEstado(new UnidadEstudio()
                {
                    id = pIdUnidadEstudio,
                    nuevoEstado = false // eliminado
                });

            });
            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 22/08/2022
        /// José Navarro Acuña
        /// Función que permite crear un tipo de indicador. Por defecto el estado es activo
        /// </summary>
        /// <param name="pNombre"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CrearTipoIndicador(string pNombre)
        {
            RespuestaConsulta<List<TipoIndicadores>> resultado = new RespuestaConsulta<List<TipoIndicadores>>();

            if (string.IsNullOrEmpty(pNombre))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.CamposIncompletos;
                return JsonConvert.SerializeObject(resultado);
            }

            if (!Utilidades.rx_alfanumerico.Match(pNombre.Trim()).Success) // validar si el nombre tiene el formato correcto
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = string.Format(Errores.CampoConFormatoInvalido, EtiquetasViewIndicadorFonatel.CrearIndicador_LabelTipo);
                return JsonConvert.SerializeObject(resultado);
            }

            await Task.Run(() =>
            {
                resultado = tipoIndicadorBL.InsertarDatos(new TipoIndicadores()
                {
                    id = "",
                    IdTipoIdicador = 0,
                    Nombre = pNombre.Trim(),
                    Estado = true // activo
                });

            });
            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 22/08/2022
        /// José Navarro Acuña
        /// Función que permite crear un grupo indicador. Por defecto el estado es activo
        /// </summary>
        /// <param name="pNombre"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CrearGrupoIndicador(string pNombre)
        {
            RespuestaConsulta<List<GrupoIndicadores>> resultado = new RespuestaConsulta<List<GrupoIndicadores>>();

            if (string.IsNullOrEmpty(pNombre))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.CamposIncompletos;
                return JsonConvert.SerializeObject(resultado);
            }

            if (!Utilidades.rx_alfanumerico.Match(pNombre.Trim()).Success) // validar si el nombre tiene el formato correcto
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = string.Format(Errores.CampoConFormatoInvalido, EtiquetasViewIndicadorFonatel.CrearIndicador_LabelGrupo);
                return JsonConvert.SerializeObject(resultado);
            }

            await Task.Run(() =>
            {
                resultado = grupoIndicadorBL.InsertarDatos(new GrupoIndicadores()
                {
                    id = "",
                    idGrupo = 0,
                    Nombre = pNombre.Trim(),
                    Estado = true // activo
                });

            });
            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 22/08/2022
        /// José Navarro Acuña
        /// Función que permite crear una unidad de estudio. Por defecto el estado es activo
        /// </summary>
        /// <param name="pNombre"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CrearUnidadEstudio(string pNombre)
        {
            RespuestaConsulta<List<UnidadEstudio>> resultado = new RespuestaConsulta<List<UnidadEstudio>>();

            if (string.IsNullOrEmpty(pNombre))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.CamposIncompletos;
                return JsonConvert.SerializeObject(resultado);
            }

            if (!Utilidades.rx_alfanumerico.Match(pNombre.Trim()).Success) // validar si el nombre tiene el formato correcto
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = string.Format(Errores.CampoConFormatoInvalido, EtiquetasViewIndicadorFonatel.CrearIndicador_LabelUnidadEstudio);
                return JsonConvert.SerializeObject(resultado);
            }

            await Task.Run(() =>
            {
                resultado = unidadEstudioBL.InsertarDatos(new UnidadEstudio()
                {
                    id = "",
                    idUnidad = 0,
                    Nombre = pNombre.Trim(),
                    Estado = true // activo
                });

            });
            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 24/08/2022
        /// José Navarro Acuña
        /// Función que permite crear un indicador.
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CrearIndicador(Indicador pIndicador)
        {
            string mensajeValidacionIndicador = ValidarObjetoIndicador(pIndicador, pIndicador.esGuardadoParcial);

            if (!string.IsNullOrEmpty(mensajeValidacionIndicador))
            {
                return JsonConvert.SerializeObject(
                    new RespuestaConsulta<List<Indicador>>() { HayError = (int)Error.ErrorControlado, MensajeError = mensajeValidacionIndicador });
            }

            if (pIndicador.esGuardadoParcial)
            {
                PrepararObjetoIndicadorGuardadoParcial(pIndicador);
            }

            pIndicador.idEstado = (int)EstadosRegistro.EnProceso;
            pIndicador.UsuarioCreacion = usuario;

            // evitar datos indeseados en los ids
            pIndicador.idIndicador = 0;
            pIndicador.IdTipoIndicador = 0;
            pIndicador.IdFrecuencia = 0;
            pIndicador.IdClasificacion = 0;
            pIndicador.IdTipoIndicador = 0;
            pIndicador.idGrupo = 0;
            pIndicador.IdUnidadEstudio = 0;

            RespuestaConsulta<List<Indicador>> resultado = new RespuestaConsulta<List<Indicador>>();

            await Task.Run(() =>
            {
                resultado = indicadorBL.InsertarDatos(pIndicador);
            });
            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 24/08/2022
        /// José Navarro Acuña
        /// Función que permite editar un indicador.
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> EditarIndicador(Indicador pIndicador)
        {
            if (string.IsNullOrEmpty(pIndicador.id)) // id indicador requerido
            {
                return JsonConvert.SerializeObject(
                    new RespuestaConsulta<List<Indicador>>() { HayError = (int)Error.ErrorControlado, MensajeError = Errores.NoRegistrosActualizar });
            }

            // debido a que la función puede ser llamada por crear o clonar (en esos modos tambien se puede actualizar),
            // se debe identificar cuando se esta editando un indicador existente producto de la seleccion de la tabla de la vista de inicio,
            // y de esta manera se realiza la validación respecto al código, ya que no puede ser modificado
            string modoFormulario = TempData[keyModoFormulario].ToString(); // valor proveniente de la vista Create
            TempData.Keep(keyModoFormulario);

            if (modoFormulario.Equals(((int)Accion.Editar).ToString()))
            {
                try
                {
                    Indicador objIndicador = indicadorBL.ObtenerDatos(new Indicador() { id = pIndicador.id }).objetoRespuesta.FirstOrDefault();
                    pIndicador.Codigo = objIndicador.Codigo;
                }
                catch (Exception)
                {
                    return JsonConvert.SerializeObject(
                        new RespuestaConsulta<List<Indicador>>() { HayError = (int)Error.ErrorControlado, MensajeError = Errores.NoRegistrosActualizar });
                };
            }

            pIndicador.UsuarioModificacion = usuario;
            return await CrearIndicador(pIndicador); // reutilizar la función de crear
        }

        /// <summary>
        /// 30/08/2022
        /// José Navarro Acuña
        /// Función que permite clonar un indicador.
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ClonarIndicador(Indicador pIndicador)
        {
            if (string.IsNullOrEmpty(pIndicador.id)) // id indicador requerido
            {
                return JsonConvert.SerializeObject(
                    new RespuestaConsulta<List<Indicador>>() { HayError = (int)Error.ErrorControlado, MensajeError = Errores.NoRegistrosActualizar });
            }

            string idIndicadorAClonar = pIndicador.id;
            pIndicador.id = string.Empty;
            pIndicador.idIndicador = 0;

            string creacionIndicador = await CrearIndicador(pIndicador); // reutilizar la función de crear para registrar el nuevo indicador
            RespuestaConsulta<List<Indicador>> indicadorDeserializado = JsonConvert.DeserializeObject<RespuestaConsulta<List<Indicador>>>(creacionIndicador);

            if (indicadorDeserializado.HayError != (int)Error.NoError) // se creó el indicador correctamente?
            {
                return creacionIndicador;
            }

            RespuestaConsulta<Indicador> resultado = new RespuestaConsulta<Indicador>();

            await Task.Run(() =>
            {
                // se envia el id del indicador a clonar y el id del indicador creado anteriormente
                resultado = indicadorBL.ClonarDetallesDeIndicador(idIndicadorAClonar, indicadorDeserializado.objetoRespuesta[0].id);
            });

            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 03/10/2022
        /// José Navarro Acuña
        /// Función que permite realizar un guardado definitivo de un indicador.
        /// </summary>
        /// <param name="pIdIndicador"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GuardadoDefinitivoIndicador(string pIdIndicador)
        {
            if (string.IsNullOrEmpty(pIdIndicador)) // id indicador requerido
            {
                return JsonConvert.SerializeObject(
                    new RespuestaConsulta<List<Indicador>>() { HayError = (int)Error.ErrorControlado, MensajeError = Errores.NoRegistrosActualizar });
            }

            RespuestaConsulta<List<Indicador>> resultado = new RespuestaConsulta<List<Indicador>>();

            await Task.Run(() =>
            {
                resultado = indicadorBL.GuardadoDefinitivoIndicador(new Indicador() { id = pIdIndicador });
            });

            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 01/09/2022
        /// José Navarro Acuña
        /// Función que permite obtener los detalles variables dato de un indicador
        /// </summary>
        /// <param name="pIdIndicador"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerListaDetallesVariable(string pIdIndicador)
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
        /// 02/09/2022
        /// José Navarro Acuña
        /// Función que permite obtener los detalles categoria de un indicador de manera agrupada
        /// </summary>
        /// <param name="pIdIndicador"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerListaDetallesCategoria(string pIdIndicador)
        {
            RespuestaConsulta<List<DetalleIndicadorCategoria>> resultado = new RespuestaConsulta<List<DetalleIndicadorCategoria>>();

            if (string.IsNullOrEmpty(pIdIndicador))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            await Task.Run(() =>
            {
                resultado = detalleIndicadorCategoriaBL.ObtenerDatosPorIndicador(new DetalleIndicadorCategoria()
                {
                    DetallesAgrupados = true,
                    idIndicadorString = pIdIndicador,
                    Estado = true
                });
                
            });
            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 05/09/2022
        /// José Navarro Acuña
        /// Función que permite obtener las categorías de desagregación
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerCategoriasDesagregacion()
        {
            RespuestaConsulta<List<CategoriasDesagregacion>> resultado = new RespuestaConsulta<List<CategoriasDesagregacion>>();

            await Task.Run(() =>
            {
                resultado = categoriasDesagregacionBL.ObtenerDatos(new CategoriasDesagregacion() { idEstado = (int)EstadosRegistro.Activo });
            });
            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 06/09/2022
        /// José Navarro Acuña
        /// Función que permite obtener los detalles de una categorías de desagregación.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerDetallesTipoTextoDeCategoriaDesagregacion(string pIdCategoria)
        {
            RespuestaConsulta<List<DetalleCategoriaTexto>> resultado = new RespuestaConsulta<List<DetalleCategoriaTexto>>();

            if (string.IsNullOrEmpty(pIdCategoria))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }
            await Task.Run(() =>
            {
                resultado = detalleCategoriasTextoBL.ObtenerDatos(new DetalleCategoriaTexto() { categoriaid = pIdCategoria });
            });
            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 07/09/2022
        /// José Navarro Acuña
        /// Función que permite obtener los detalles de una categorías de desagregación a manera de listado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerListaDetallesDeCategoriaGuardada(string pIdIndicador, string pIdCategoria)
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
                resultado = detalleIndicadorCategoriaBL.ObtenerDatosPorIndicadorYCategoria(new DetalleIndicadorCategoria()
                {
                    DetallesAgrupados = false,
                    idIndicadorString = pIdIndicador,
                    idCategoriaString = pIdCategoria
                });
            });
            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 12/09/2022
        /// Michael Hernandez 
        /// Función que permite crear un nuevo detalle de categoria para un indicador
        /// Última modificación: 09/11/2022 - José Navarro Acuña
        /// </summary>
        /// <param name="pDetalleIndicadorCategoria"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CrearDetalleCategoriaDesagregacion(DetalleIndicadorCategoria pDetalleIndicadorCategoria)
        {
            RespuestaConsulta<List<DetalleIndicadorCategoria>> resultado = new RespuestaConsulta<List<DetalleIndicadorCategoria>>();

            string mensajeValidacion = ValidarObjetoDetalleCategoria(pDetalleIndicadorCategoria);

            if (mensajeValidacion != null)
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = mensajeValidacion;
                return JsonConvert.SerializeObject(resultado);
            }

            await Task.Run(() =>
            {
                resultado = detalleIndicadorCategoriaBL.InsertarDatos(pDetalleIndicadorCategoria);
            });

            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 12/09/2022
        /// José Navarro Acuña
        /// Función que permite editar un detalla de categoria existe de un indicador
        /// </summary>
        /// <param name="pDetalleIndicadorCategoria"></param>
        /// <returns></returns>
        public async Task<string> EditarDetalleCategoriaDesagreagacion(DetalleIndicadorCategoria pDetalleIndicadorCategoria)
        {
            RespuestaConsulta<List<DetalleIndicadorCategoria>> resultado = new RespuestaConsulta<List<DetalleIndicadorCategoria>>();

            string mensajeValidacion = ValidarObjetoDetalleCategoria(pDetalleIndicadorCategoria);

            if (mensajeValidacion != null)
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = mensajeValidacion;
                return JsonConvert.SerializeObject(resultado);
            }

            await Task.Run(() =>
            {
                resultado = detalleIndicadorCategoriaBL.ActualizarElemento(pDetalleIndicadorCategoria);
            });

            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 09/11/2022
        /// José Navarro Acuña
        /// Función que permite eliminar un detalle de categoria de un indicador
        /// </summary>
        /// <param name="pDetalleIndicadorCategoria"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> EliminarDetalleCategoria(DetalleIndicadorCategoria pDetalleIndicadorCategoria)
        {
            RespuestaConsulta<List<DetalleIndicadorCategoria>> resultado = new RespuestaConsulta<List<DetalleIndicadorCategoria>>();

            if (string.IsNullOrEmpty(pDetalleIndicadorCategoria.idIndicadorString) ||
                string.IsNullOrEmpty(pDetalleIndicadorCategoria.idCategoriaString)
                )
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            await Task.Run(() =>
            {
                resultado = detalleIndicadorCategoriaBL.EliminarElemento(pDetalleIndicadorCategoria);
            });
            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 12/09/2022
        /// José Navarro Acuña
        /// Función que permite crear un nuevo detalle de variable dato para un indicador
        /// </summary>
        /// <param name="pDetalleIndicadorVariables"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CrearDetalleVariableDato(DetalleIndicadorVariables pDetalleIndicadorVariables)
        {
            RespuestaConsulta<List<DetalleIndicadorVariables>> resultado = new RespuestaConsulta<List<DetalleIndicadorVariables>>();

            string mensajeValidacion = ValidarObjetoDetalleVariable(pDetalleIndicadorVariables);

            if (mensajeValidacion != null)
            {
                resultado.HayError = (int) Error.ErrorControlado;
                resultado.MensajeError = mensajeValidacion;
                return JsonConvert.SerializeObject(resultado);
            }
            await Task.Run(() =>
            {
                resultado = detalleIndicadorVariablesBL.InsertarDatos(pDetalleIndicadorVariables);
            });
            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 13/09/2022
        /// José Navarro Acuña
        /// Función que permite actualizar un detalle de variable dato existente de un indicador
        /// </summary>
        /// <param name="pDetalleIndicadorVariables"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> EditarDetalleVariableDato(DetalleIndicadorVariables pDetalleIndicadorVariables)
        {
            RespuestaConsulta<List<DetalleIndicadorVariables>> resultado = new RespuestaConsulta<List<DetalleIndicadorVariables>>();

            string mensajeValidacion = ValidarObjetoDetalleVariable(pDetalleIndicadorVariables);

            if (mensajeValidacion != null)
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = mensajeValidacion;
                return JsonConvert.SerializeObject(resultado);
            }

            if (string.IsNullOrEmpty(pDetalleIndicadorVariables.id))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            await Task.Run(() =>
            {
                resultado = detalleIndicadorVariablesBL.ActualizarElemento(pDetalleIndicadorVariables);
            });
            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 15/09/2022
        /// José Navarro Acuña
        /// Función que permite eliminar un detalle de variable dato de un indicador
        /// </summary>
        /// <param name="pDetalleIndicadorVariables"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> EliminarDetalleVariableDato(DetalleIndicadorVariables pDetalleIndicadorVariables)
        {
            RespuestaConsulta<List<DetalleIndicadorVariables>> resultado = new RespuestaConsulta<List<DetalleIndicadorVariables>>();

            if (string.IsNullOrEmpty(pDetalleIndicadorVariables.idIndicadorString) ||
                string.IsNullOrEmpty(pDetalleIndicadorVariables.id)
                )
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            pDetalleIndicadorVariables.Estado = false;

            await Task.Run(() =>
            {
                resultado = detalleIndicadorVariablesBL.CambioEstado(pDetalleIndicadorVariables);
            });
            return JsonConvert.SerializeObject(resultado);
        }

        #endregion

        #region Funciones privadas

        /// <summary>
        /// 30/08/2022
        /// José Navarro Acuña
        /// Función que permite verificar los datos requeridos y el formato de los mismos de un indicador al momento de crear o actualizar
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <returns></returns>
        public string ValidarObjetoIndicador(Indicador pIndicador, bool esGuardadoParcial)
        {
            if (!esGuardadoParcial)
            {
                if ( // el nombre y código siempre son obligatorios
                    pIndicador.TipoIndicadores == null          || string.IsNullOrEmpty(pIndicador.TipoIndicadores.id) ||
                    pIndicador.FrecuenciaEnvio == null          || string.IsNullOrEmpty(pIndicador.FrecuenciaEnvio.id) ||
                    pIndicador.Descripcion == null              || string.IsNullOrEmpty(pIndicador.Descripcion.Trim()) ||
                    pIndicador.ClasificacionIndicadores == null || string.IsNullOrEmpty(pIndicador.ClasificacionIndicadores.id) ||
                    pIndicador.TipoMedida == null               || string.IsNullOrEmpty(pIndicador.TipoMedida.id) ||
                    pIndicador.GrupoIndicadores == null         || string.IsNullOrEmpty(pIndicador.GrupoIndicadores.id) ||
                    pIndicador.Interno == null ||
                    pIndicador.Notas == null                    || string.IsNullOrEmpty(pIndicador.Notas.Trim()) ||
                    pIndicador.CantidadVariableDato == null ||
                    pIndicador.CantidadCategoriasDesagregacion == null ||
                    pIndicador.UnidadEstudio == null            || string.IsNullOrEmpty(pIndicador.UnidadEstudio.id) ||
                    pIndicador.Solicitud == null ||
                    pIndicador.Fuente == null                   || string.IsNullOrEmpty(pIndicador.Fuente.Trim())
                )
                {
                    return Errores.CamposIncompletos;
                }
            }

            if (pIndicador.Codigo == null || string.IsNullOrEmpty(pIndicador.Codigo.Trim())) // campo requerido (obligatorio siempre)
            {
                return string.Format(Errores.CampoRequeridoV2, EtiquetasViewIndicadorFonatel.CrearIndicador_LabelCodigo);
            }
            else if (!Utilidades.rx_alfanumerico.Match(pIndicador.Codigo.Trim()).Success    // validar el formato correcto
                || pIndicador.Codigo.Trim().Length > 30)                                    // validar la cantidad de caracteres
            {
                return string.Format(Errores.CampoConFormatoInvalido, EtiquetasViewIndicadorFonatel.CrearIndicador_LabelCodigo);
            }

            if (pIndicador.Nombre == null || string.IsNullOrEmpty(pIndicador.Nombre.Trim())) // campo requerido (obligatorio siempre)
            {
                return string.Format(Errores.CampoRequeridoV2, EtiquetasViewIndicadorFonatel.CrearIndicador_LabelNombre);
            }
            else if (!Utilidades.rx_soloTexto.Match(pIndicador.Nombre.Trim()).Success       // validar el formato correcto
                || pIndicador.Nombre.Trim().Length > 300)                                   // validar la cantidad de caracteres
            {
                return string.Format(Errores.CampoConFormatoInvalido, EtiquetasViewIndicadorFonatel.CrearIndicador_LabelNombre);
            }

            // verificar formato de los campos opcionales

            if (!string.IsNullOrEmpty(pIndicador.Descripcion?.Trim())) // ¿se ingresó el dato?
            {
                if (!Utilidades.rx_soloTexto.Match(pIndicador.Descripcion).Success          // la descripción solo debe contener texto como valor
                    || pIndicador.Descripcion.Trim().Length > 3000)                         // validar la cantidad de caracteres
                {
                    return string.Format(Errores.CampoConFormatoInvalido, EtiquetasViewIndicadorFonatel.CrearIndicador_LabelDescripcion);
                }
            }

            if (!string.IsNullOrEmpty(pIndicador.Notas?.Trim())) // ¿se ingresó el dato?
            {
                if (!Utilidades.rx_soloTexto.Match(pIndicador.Notas).Success                // las notas solo deben contener texto como valor
                    || pIndicador.Notas.Trim().Length > 3000)                               // validar la cantidad de caracteres
                {
                    return string.Format(Errores.CampoConFormatoInvalido, EtiquetasViewIndicadorFonatel.CrearIndicador_LabelNotas);
                }
            }

            if (!string.IsNullOrEmpty(pIndicador.Fuente?.Trim())) // ¿se ingresó el dato?
            {
                if (!Utilidades.rx_soloTexto.Match(pIndicador.Fuente).Success               // la fuente solo debe contener texto como valor
                    || pIndicador.Fuente.Trim().Length > 300)                               // validar la cantidad de caracteres
                {
                    return string.Format(Errores.CampoConFormatoInvalido, EtiquetasViewIndicadorFonatel.CrearIndicador_LabelFuenteIndicador);
                }
            }

            if (pIndicador.CantidadVariableDato != null)
            {
                if (pIndicador.CantidadVariableDato < 1) // ¿menor o igual 0?
                {
                    return string.Format(Errores.CampoRequeridoV2, EtiquetasViewIndicadorFonatel.CrearIndicador_LabelCantidadVariableDatosIndicador);
                }
            }
            
            if (pIndicador.CantidadCategoriasDesagregacion != null)
            {
                if (pIndicador.CantidadCategoriasDesagregacion < 1) // ¿menor o igual 0?
                {
                    return string.Format(Errores.CampoRequeridoV2, EtiquetasViewIndicadorFonatel.CrearIndicador_LabelCantidadCategoriaIndicador);
                }
            }

            return null;
        }

        /// <summary>
        /// 12/09/2022
        /// José Navarro Acuña
        /// Función que permite verificar los datos de un objeto detalle de Indicador de tipo variable
        /// </summary>
        /// <param name="pDetalleIndicadorVariables"></param>
        /// <returns></returns>
        private string ValidarObjetoDetalleVariable(DetalleIndicadorVariables pDetalleIndicadorVariables)
        {
            if (string.IsNullOrEmpty(pDetalleIndicadorVariables.idIndicadorString))
            {
                return Errores.NoRegistrosActualizar;
            }

            if (pDetalleIndicadorVariables.NombreVariable == null || string.IsNullOrEmpty(pDetalleIndicadorVariables.NombreVariable.Trim()))
            {
                return string.Format(Errores.CampoRequeridoV2, EtiquetasViewIndicadorFonatel.CrearVariable_LabelNombreVariable);
            }

            if (pDetalleIndicadorVariables.Descripcion == null || string.IsNullOrEmpty(pDetalleIndicadorVariables.Descripcion.Trim())) 
            {
                return string.Format(Errores.CampoRequeridoV2, EtiquetasViewIndicadorFonatel.CrearVariable_LabelDescripcionVariable);
            }

            if (!Utilidades.rx_soloTexto.Match(pDetalleIndicadorVariables.NombreVariable).Success       // validar el formato correcto
                || pDetalleIndicadorVariables.NombreVariable.Trim().Length > 350)                       // validar la cantidad de caracteres
            {
                return string.Format(Errores.CampoConFormatoInvalido, EtiquetasViewIndicadorFonatel.CrearVariable_LabelNombreVariable);
            }

            if (!Utilidades.rx_soloTexto.Match(pDetalleIndicadorVariables.Descripcion).Success      // validar el formato correcto
                || pDetalleIndicadorVariables.Descripcion.Trim().Length > 2000)                     // validar la cantidad de caracteres
            {
                return string.Format(Errores.CampoConFormatoInvalido, EtiquetasViewIndicadorFonatel.CrearVariable_LabelDescripcionVariable);
            }

            return null;
        }

        /// <summary>
        /// 07/11/2022
        /// José Navarro Acuña
        /// Función que permite verificar los datos de un objecto detalle de Indicador de tipo categorias
        /// </summary>
        /// <param name="pDetalleIndicadorCategoria"></param>
        /// <returns></returns>
        private string ValidarObjetoDetalleCategoria(DetalleIndicadorCategoria pDetalleIndicadorCategoria)
        {
            if (string.IsNullOrEmpty(pDetalleIndicadorCategoria.idIndicadorString))
            {
                return Errores.NoRegistrosActualizar;
            }

            if (string.IsNullOrEmpty(pDetalleIndicadorCategoria.idCategoriaString))
            {
                return Errores.NoRegistrosActualizar;
            }

            return null;
        }

        /// <summary>
        /// 30/08/2022
        /// José Navarro Acuña
        /// Función que prepara los atributos no establecidos en un indicador para realizar un guardado parcial
        /// </summary>
        /// <param name="pIndicador"></param>
        private void PrepararObjetoIndicadorGuardadoParcial(Indicador pIndicador)
        {
            if (string.IsNullOrEmpty(pIndicador.TipoIndicadores.id))
                pIndicador.TipoIndicadores.id = defaultDropDownValue;
            
            if (string.IsNullOrEmpty(pIndicador.FrecuenciaEnvio.id))
                pIndicador.FrecuenciaEnvio.id = defaultDropDownValue;
            
            if (pIndicador.Descripcion == null || string.IsNullOrEmpty(pIndicador.Descripcion.Trim()))
                pIndicador.Descripcion = defaultInputTextValue;
            
            if (string.IsNullOrEmpty(pIndicador.ClasificacionIndicadores.id))
                pIndicador.ClasificacionIndicadores.id = defaultDropDownValue;
            
            if (string.IsNullOrEmpty(pIndicador.TipoMedida.id))
                pIndicador.TipoMedida.id = defaultDropDownValue;
            
            if (string.IsNullOrEmpty(pIndicador.GrupoIndicadores.id))
                pIndicador.GrupoIndicadores.id = defaultDropDownValue;
            
            if (pIndicador.Interno == null) // Uso
                pIndicador.Interno = false;

            if (pIndicador.Notas == null || string.IsNullOrEmpty(pIndicador.Notas.Trim()))
                pIndicador.Notas = defaultInputTextValue;
            
            //if (pIndicador.CantidadVariableDato == null)
            //    pIndicador.CantidadVariableDato = defaultInputNumberValue;
            
            //if (pIndicador.CantidadCategoriasDesagregacion == null)
            //    pIndicador.CantidadCategoriasDesagregacion = defaultInputNumberValue;
            
            if (string.IsNullOrEmpty(pIndicador.UnidadEstudio.id))
                pIndicador.UnidadEstudio.id = defaultDropDownValue;
            
            if (pIndicador.Solicitud == null)
                pIndicador.Solicitud = false;
            
            if (pIndicador.Fuente == null || string.IsNullOrEmpty(pIndicador.Fuente.Trim()))
                pIndicador.Fuente = defaultInputTextValue;
        }

        /// <summary>
        /// 07/09/2022
        /// José Navarro Acuña
        /// Método que permite cargar los datos necesarios del formulario de indicadores Fonatel
        /// </summary>
        private void CargarDatosEnVistas()
        {
            ViewBag.TiposIndicadores = tipoIndicadorBL.ObtenerDatos(new TipoIndicadores()).objetoRespuesta;
            ViewBag.FrecuenciasEnvio = frecuenciaEnvioBL.ObtenerDatos(new FrecuenciaEnvio()).objetoRespuesta;
            ViewBag.Clasificaciones = clasificacionIndicadorBL.ObtenerDatos(new ClasificacionIndicadores()).objetoRespuesta;
            ViewBag.TipoMedidas = tipoMedidaBL.ObtenerDatos(new TipoMedida()).objetoRespuesta;
            ViewBag.Grupos = grupoIndicadorBL.ObtenerDatos(new GrupoIndicadores()).objetoRespuesta;
            ViewBag.UsosIndicador = indicadorBL.ObtenerListaUsosIndicador().objetoRespuesta;
            ViewBag.UsosSolicitud = indicadorBL.ObtenerListaMostrarIndicadorEnSolicitud().objetoRespuesta;
            ViewBag.UnidadesEstudio = unidadEstudioBL.ObtenerDatos(new UnidadEstudio()).objetoRespuesta;
            ViewBag.UsosSolicitud = indicadorBL.ObtenerListaMostrarIndicadorEnSolicitud().objetoRespuesta;
        }
        #endregion
    }
}
