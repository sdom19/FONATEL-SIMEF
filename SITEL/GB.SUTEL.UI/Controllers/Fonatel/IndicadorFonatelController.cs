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

namespace GB.SUTEL.UI.Controllers.Fonatel
{
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
        private readonly string defaultDropDownValue;


        public IndicadorFonatelController()
        {
            string usuario = System.Web.HttpContext.Current.User.Identity.GetUserId();
            indicadorBL = new IndicadorFonatelBL(EtiquetasViewIndicadorFonatel.TituloIndex, usuario);
            tipoIndicadorBL = new TipoIndicadorBL(EtiquetasViewIndicadorFonatel.TituloIndex, usuario);
            grupoIndicadorBL = new GrupoIndicadorBL(EtiquetasViewIndicadorFonatel.TituloIndex, usuario);
            unidadEstudioBL = new UnidadEstudioBL(EtiquetasViewIndicadorFonatel.TituloIndex, usuario);
            frecuenciaEnvioBL = new FrecuenciaEnvioBL(EtiquetasViewIndicadorFonatel.TituloIndex, usuario);
            clasificacionIndicadorBL = new ClasificacionIndicadorBL(EtiquetasViewIndicadorFonatel.TituloIndex, usuario);
            tipoMedidaBL = new TipoMedidaBL(EtiquetasViewIndicadorFonatel.TituloIndex, usuario);
            detalleIndicadorVariablesBL = new DetalleIndicadorVariablesBL(EtiquetasViewIndicadorFonatel.TituloIndex, usuario);
            detalleIndicadorCategoriaBL = new DetalleIndicadorCategoriaBL(EtiquetasViewIndicadorFonatel.TituloIndex, usuario);

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
            ViewBag.TiposIndicadores = tipoIndicadorBL.ObtenerDatos(new TipoIndicadores()).objetoRespuesta;
            ViewBag.FrecuenciasEnvio = frecuenciaEnvioBL.ObtenerDatos(new FrecuenciaEnvio()).objetoRespuesta;
            ViewBag.Clasificaciones = clasificacionIndicadorBL.ObtenerDatos(new ClasificacionIndicadores()).objetoRespuesta;
            ViewBag.TipoMedidas = tipoMedidaBL.ObtenerDatos(new TipoMedida()).objetoRespuesta;
            ViewBag.Grupos = grupoIndicadorBL.ObtenerDatos(new GrupoIndicadores()).objetoRespuesta;
            ViewBag.UsosIndicador = indicadorBL.ObtenerListaUsosIndicador().objetoRespuesta;
            ViewBag.UsosSolicitud = indicadorBL.ObtenerListaMostrarIndicadorEnSolicitud().objetoRespuesta;
            ViewBag.UnidadesEstudio = unidadEstudioBL.ObtenerDatos(new UnidadEstudio()).objetoRespuesta;
            return View();
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            ViewBag.TiposIndicadores = new List<TipoIndicadores>();
            ViewBag.FrecuenciasEnvio = new List<FrecuenciaEnvio>();
            ViewBag.Clasificaciones = new List<ClasificacionIndicadores>();
            ViewBag.TipoMedidas = new List<TipoMedida>();
            ViewBag.Grupos = new List<GrupoIndicadores>();
            ViewBag.UsosIndicador = new List<EstadoLogico>();
            ViewBag.UnidadesEstudio = new List<UnidadEstudio>();
            return View("Create");
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
                    nuevoEstado = (int)EstadosRegistro.Activo // nuevo estado
                });

            });
            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 16/08/2022
        /// José Navarro Acuña
        /// Función que verifica si el indicador se encuentra en algún formulario web
        /// </summary>
        /// <param name="pIdIndicador"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> VerificarIndicadorEnFormularioWeb(string pIdIndicador)
        {
            RespuestaConsulta<List<FormularioWeb>> resultado = new RespuestaConsulta<List<FormularioWeb>>();
            
            if (string.IsNullOrEmpty(pIdIndicador))
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = Errores.NoRegistrosActualizar;
                return JsonConvert.SerializeObject(resultado);
            }

            await Task.Run(() =>
            {
                resultado = indicadorBL.ObtenerFormulariosWebSegunIndicador(new Indicador()
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

            if (!Utilidades.rx_alfanumerico_v2.Match(pNombre.Trim()).Success) // validar si el nombre tiene el formato correcto
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

            if (!Utilidades.rx_alfanumerico_v2.Match(pNombre.Trim()).Success) // validar si el nombre tiene el formato correcto
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

            if (!Utilidades.rx_alfanumerico_v2.Match(pNombre.Trim()).Success) // validar si el nombre tiene el formato correcto
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
            RespuestaConsulta<List<Indicador>> resultado = new RespuestaConsulta<List<Indicador>>();

            string mensajeValidacionIndicador = ValidarObjetoIndicador(pIndicador, pIndicador.esGuardadoParcial);

            if (mensajeValidacionIndicador != null)
            {
                resultado.HayError = (int)Error.ErrorControlado;
                resultado.MensajeError = mensajeValidacionIndicador;
                return JsonConvert.SerializeObject(resultado);
            }

            if (pIndicador.esGuardadoParcial)
            {
                PrepararObjetoIndicadorGuardadoParcial(pIndicador);
            }

            pIndicador.idEstado = (int)Constantes.EstadosRegistro.EnProceso;
            // evitar datos indeseados en los ids
            pIndicador.IdTipoIndicador = 0;
            pIndicador.IdFrecuencia = 0;
            pIndicador.IdClasificacion = 0;
            pIndicador.IdTipoIndicador = 0;
            pIndicador.idGrupo = 0;
            pIndicador.IdUnidadEstudio = 0;

            await Task.Run(() =>
            {
                resultado = indicadorBL.InsertarDatos(pIndicador);
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
                resultado = detalleIndicadorVariablesBL.ObtenerDatosPorIndicador(new DetalleIndicadorVariables()
                {
                    idIndicadorString = pIdIndicador
                });

            });
            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 02/09/2022
        /// José Navarro Acuña
        /// Función que permite obtener los detalles categoria de un indicador
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
                    idIndicadorString = pIdIndicador
                });

            });
            return JsonConvert.SerializeObject(resultado);
        }

        #endregion

        #region Funciones privadas

        /// <summary>
        /// 30/08/2022
        /// José Navarro Acuña
        /// Función que permite verificar los datos de un objeto indicador.
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <returns></returns>
        private string ValidarObjetoIndicador(Indicador pIndicador, bool esGuardadoParcial)
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
            else if (!Utilidades.rx_alfanumerico_v2.Match(pIndicador.Codigo.Trim()).Success // validar el formato correcto
                || pIndicador.Codigo.Trim().Length > 30)                                    // validar la cantidad de caracteres
            {
                return string.Format(Errores.CampoConFormatoInvalido, EtiquetasViewIndicadorFonatel.CrearIndicador_LabelCodigo);
            }

            if (pIndicador.Nombre == null || string.IsNullOrEmpty(pIndicador.Nombre.Trim())) // campo requerido (obligatorio siempre)
            {
                return string.Format(Errores.CampoRequeridoV2, EtiquetasViewIndicadorFonatel.CrearIndicador_LabelNombre);
            }
            else if (!Utilidades.rx_alfanumerico_v2.Match(pIndicador.Nombre.Trim()).Success // validar el formato correcto
                || pIndicador.Nombre.Trim().Length > 300)                                   // validar la cantidad de caracteres
            {
                return string.Format(Errores.CampoConFormatoInvalido, EtiquetasViewIndicadorFonatel.CrearIndicador_LabelNombre);
            }

            if (pIndicador.Descripcion?.Trim().Length > 3000)                               // validar la cantidad de caracteres
            {
                return string.Format(Errores.CampoConFormatoInvalido, EtiquetasViewIndicadorFonatel.CrearIndicador_LabelDescripcion);
            }

            if (pIndicador.Notas?.Trim().Length > 3000)                                     // validar la cantidad de caracteres
            {
                return string.Format(Errores.CampoConFormatoInvalido, EtiquetasViewIndicadorFonatel.CrearIndicador_LabelNotas);
            }

            if (pIndicador.Fuente?.Trim().Length > 300)                                     // validar la cantidad de caracteres
            {
                return string.Format(Errores.CampoConFormatoInvalido, EtiquetasViewIndicadorFonatel.CrearIndicador_LabelFuenteIndicador);
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
            

            if (pIndicador.CantidadVariableDato == null)
                pIndicador.CantidadVariableDato = defaultInputNumberValue;
            

            if (pIndicador.CantidadCategoriasDesagregacion == null)
                pIndicador.CantidadCategoriasDesagregacion = defaultInputNumberValue;
            

            if (string.IsNullOrEmpty(pIndicador.UnidadEstudio.id))
                pIndicador.UnidadEstudio.id = defaultDropDownValue;
            

            if (pIndicador.Solicitud == null)
                pIndicador.Solicitud = false;
            

            if (pIndicador.Fuente == null || string.IsNullOrEmpty(pIndicador.Fuente.Trim()))
                pIndicador.Fuente = defaultInputTextValue;
            
        }

        #endregion
    }
}
