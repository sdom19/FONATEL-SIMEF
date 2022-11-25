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
    public class SolicitudFonatelController : Controller
    {

        #region Variables publicas


        private readonly SolicitudBL SolicitudesBL;
        private readonly AnnoBL AnnoBL;
        private readonly MesBL MesBL;
        private readonly FuentesRegistroBL fuenteBl;
        private readonly FormularioWebBL formularioWebBL;
        private readonly DetalleSolicitudesBL detalleSolicitudesBL;
        private readonly FrecuenciaEnvioBL frecuenciaEnvioBL;
        private readonly SolicitudEnvioProgramadoBL EnvioProgramadoBL;



        string user;

        #endregion


        public SolicitudFonatelController()
        {
            SolicitudesBL = new SolicitudBL(EtiquetasViewSolicitudes.Solicitudes, System.Web.HttpContext.Current.User.Identity.GetUserId());
            AnnoBL = new AnnoBL();
            MesBL = new MesBL();
            fuenteBl = new FuentesRegistroBL(EtiquetasViewSolicitudes.Solicitudes, System.Web.HttpContext.Current.User.Identity.GetUserId());
            formularioWebBL = new FormularioWebBL(EtiquetasViewSolicitudes.Solicitudes, System.Web.HttpContext.Current.User.Identity.GetUserId());
            detalleSolicitudesBL = new DetalleSolicitudesBL();
            EnvioProgramadoBL = new SolicitudEnvioProgramadoBL();
            frecuenciaEnvioBL = new FrecuenciaEnvioBL(EtiquetasViewSolicitudes.Solicitudes, System.Web.HttpContext.Current.User.Identity.GetUserId());
        }

        // GET: Solicitud
        public ActionResult Index()
        {
            ViewBag.ListaFrecuencia = frecuenciaEnvioBL.ObtenerDatos(new FrecuenciaEnvio()).objetoRespuesta;

            return View();
        }


        #region METODOS DE PAGINA

        // GET: Solicitud/Create
        public ActionResult Create(string id, int? modo)
        {
            Solicitud model = new Solicitud();
            ViewBag.ListaAnno = AnnoBL.ObtenerDatos(new Anno() ).objetoRespuesta;
            ViewBag.Modo = modo.ToString();
            ViewBag.ListaMes= MesBL.ObtenerDatos(new Mes()).objetoRespuesta;
            ViewBag.ListaFuentes = fuenteBl.ObtenerDatos(new FuentesRegistro()).objetoRespuesta;
            ViewBag.ListaFormularioWeb = formularioWebBL.ObtenerDatos(new FormularioWeb() {idEstado=(int)Constantes.EstadosRegistro.Activo })
                                        .objetoRespuesta.Select(x=>new SelectListItem() { Selected=false, Value=x.id, 
                                            Text=Utilidades.ConcatenadoCombos(x.Codigo,x.Nombre) }).ToList();
            if (!string.IsNullOrEmpty(id))
            {
                
                model = SolicitudesBL.ObtenerDatos(new Solicitud() {id=id })
                    .objetoRespuesta.Single();
                if (modo== (int)Constantes.Accion.Clonar)
                {
                    ViewBag.titulo = EtiquetasViewSolicitudes.Clonar;
                    model.Nombre = string.Empty;
                    model.Codigo = string.Empty;
                }
                else
                {
                    ViewBag.titulo = EtiquetasViewSolicitudes.Editar;
                  
                }
            }
            else
            {
                ViewBag.titulo = EtiquetasViewSolicitudes.Crear;
                model.FormularioWeb = new List<FormularioWeb>();              
            }
            return View(model);
        }

        #endregion

        #region METODOS DE SOLICITUDES

        [HttpGet]
        public async Task<string> ObtenerListaSolicitudes()
        {
            RespuestaConsulta<List<Solicitud>> result = null;
            await Task.Run(() =>
            {
                result = SolicitudesBL.ObtenerDatos(new Solicitud());
            });

            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Fecha: 03/10/2022
        /// Francisco Vindas
        /// Metodo para insertar solicitudes de informacion
        /// </summary>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> InsertarSolicitud(Solicitud solicitud)
        {

          
            solicitud.IdEstado = (int)Constantes.EstadosRegistro.EnProceso;

            RespuestaConsulta<List<Solicitud>> result = null;

            await Task.Run(() =>
            {
                solicitud.UsuarioCreacion = user;

                result = SolicitudesBL.InsertarDatos(solicitud);

            });

            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Fecha: 24/11/2022
        /// Francisco Vindas
        /// Metodo para clonarsolicitudes de informacion
        /// </summary>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Clonar(Solicitud solicitud)
        {
            solicitud.IdEstado = (int)Constantes.EstadosRegistro.EnProceso;

            RespuestaConsulta<List<Solicitud>> result = null;

            await Task.Run(() =>
            {
                solicitud.UsuarioCreacion = user;

                result = SolicitudesBL.ClonarDatos(solicitud);

            });

            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Fecha: 04
        /// Francisco Vindas
        /// Metodo para editar solicitudes de informacion
        /// </summary>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> EditarSolicitud(Solicitud solicitud)
        {
            
            solicitud.IdEstado = (int)Constantes.EstadosRegistro.EnProceso;
            RespuestaConsulta<List<Solicitud>> result = null;

            await Task.Run(() =>
            {
                solicitud.UsuarioCreacion = user;

                result = SolicitudesBL.ActualizarElemento(solicitud);

            });

            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Fecha: 04/10/2022
        /// Francisco Vindas
        /// Metodo para insertar solicitudes de informacion
        /// </summary>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ClonarSolicitud(Solicitud solicitud)
        {
            solicitud.IdEstado = (int)Constantes.EstadosRegistro.EnProceso;

            if (string.IsNullOrEmpty(solicitud.id))
            {
                return JsonConvert.SerializeObject(
                    new RespuestaConsulta<List<Indicador>>() { HayError = (int)Error.ErrorControlado, MensajeError = Errores.NoRegistrosActualizar });
            }

            string idSolicitudAClonar = solicitud.id;

            string creacionSolicitud = await Clonar(solicitud); 
            RespuestaConsulta<List<Solicitud>> SolicitudDeserializado = JsonConvert.DeserializeObject<RespuestaConsulta<List<Solicitud>>>(creacionSolicitud);

            if (SolicitudDeserializado.HayError != (int)Error.NoError) // se creó la solicitud correctamente?
            {
                return creacionSolicitud;
            }

            RespuestaConsulta<Solicitud> resultado = new RespuestaConsulta<Solicitud>();

            await Task.Run(() =>
            {
                // se envia el id del indicador a clonar y el id de la Solicitud creado anteriormente
                resultado = SolicitudesBL.ClonarDetallesDeSolicitudes(idSolicitudAClonar, SolicitudDeserializado.objetoRespuesta[0].id);
            });


            return JsonConvert.SerializeObject(SolicitudDeserializado);
        }

        /// <summary>
        /// Fecha 05/10/2022
        /// Francisco Vindas Ruiz
        /// Cambio el estado de registro a desactivado y activado 
        /// </summary>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CambiarEstadoEliminado(Solicitud solicitud)
        {
            RespuestaConsulta<List<Solicitud>> result = null;

            solicitud.IdEstado = (int)Constantes.EstadosRegistro.Eliminado;

            await Task.Run(() =>
            {
                result = SolicitudesBL.CambioEstado(solicitud);
            });

            return JsonConvert.SerializeObject(result);
        }

        /// <summary> 
        /// 05/10/2022
        /// Francisco Vindas Ruiz
        /// Metodo para eliminar solicitud
        /// </summary>
        /// <param name="idSolicitud></param>
        /// <returns>JSON</returns>
        [HttpPost]
        public async Task<string> EliminarSolicitud(string idSolicitud)
        {
            RespuestaConsulta<List<Solicitud>> result = null;

            await Task.Run(() =>
            {
                result = SolicitudesBL.EliminarElemento(new Solicitud()
                {

                    id = idSolicitud,
                    UsuarioModificacion = user

                });

            });
            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Fecha 05/10/2022
        /// Francisco Vindas Ruiz
        /// Cambio el estado de registro a desactivado y activado 
        /// </summary>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CambiarEstadoActivado(Solicitud solicitud)
        {
            RespuestaConsulta<List<Solicitud>> result = null;

            solicitud.IdEstado = (int)Constantes.EstadosRegistro.Activo;

            await Task.Run(() =>
            {
                result = SolicitudesBL.CambioEstado(solicitud);
            });

            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Fecha 05/10/2022
        /// Francisco Vindas Ruiz
        /// Cambio el estado de registro a desactivado y activado 
        /// </summary>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CambiarEstadoDesactivado(Solicitud solicitud)
        {
            RespuestaConsulta<List<Solicitud>> result = null;

            solicitud.IdEstado = (int)Constantes.EstadosRegistro.Desactivado;

            await Task.Run(() =>
            {
                result = SolicitudesBL.CambioEstado(solicitud);
            });

            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public async Task<string> ValidarExistenciaSolicitud(Solicitud solicitud )
        {
            RespuestaConsulta<List<string>> result = null;
            await Task.Run(() =>
            {
                result = SolicitudesBL.ValidarExistenciaSolicitudEliminar(solicitud);
            });

            return JsonConvert.SerializeObject(result);

        }

        #endregion

        #region METODOS DE DETALLES DE SOLICITUDES

        /// <summary>
        /// Fecha: 13/10/2022
        /// Autor: Francisco Vindas
        /// Metodo para obtener los formularios asociados a solicitudes de informacion
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerListaFormulario(DetalleSolicitudFormulario Solicitud)
        {
            RespuestaConsulta<List<FormularioWeb>> result = null;

            await Task.Run(() =>
            {
                result = detalleSolicitudesBL.ObtenerListaFormularios(Solicitud);
            });

            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Fecha: 10/10/2022
        /// Francisco Vindas
        /// Metodo para insertar detalles solicitudes de informacion
        /// </summary>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> InsertarDetalleSolicitud(DetalleSolicitudFormulario Solicitud)
        {

            RespuestaConsulta<List<DetalleSolicitudFormulario>> result = null;

            Solicitud.Estado = true;

            await Task.Run(() =>
            {
                result = detalleSolicitudesBL.InsertarDatos(Solicitud);
            });

            return JsonConvert.SerializeObject(result);
        }

        /// <summary> 
        /// 05/10/2022
        /// Francisco Vindas Ruiz
        /// Metodo para eliminar solicitud
        /// </summary>
        /// <param name="idDetalleSolicitud></param>
        /// <returns>JSON</returns>
        [HttpPost]
        public async Task<string> EliminarDetalleSolicitud(string idSolicitud, string idFormulario)
        {
            RespuestaConsulta<List<DetalleSolicitudFormulario>> result = null;

            await Task.Run(() =>
            {
                result = detalleSolicitudesBL.EliminarElemento(new DetalleSolicitudFormulario()
                {

                    id = idSolicitud,
                    Formularioid = idFormulario

                });

            });
            return JsonConvert.SerializeObject(result);
        }

        #endregion

        #region METODO DE ENVIO DE CORREOS



        /// <summary>
        /// Fecha: 18/10/2022
        /// Francisco Vindas
        /// Metodo para insertar solicitudes de informacion
        /// </summary>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> EnvioCorreo(Solicitud objeto)
        {
            RespuestaConsulta<bool> result = null;
            await Task.Run(() =>
            {
                result = SolicitudesBL.EnvioCorreo(objeto);
            });
            return JsonConvert.SerializeObject(result);
        }
        /// <summary>
        /// Fecha: 18/10/2022
        /// Francisco Vindas
        /// Metodo para insertar solicitudes de informacion
        /// </summary>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> InsertarEnvioProgramado(SolicitudEnvioProgramado objeto)
        {



            RespuestaConsulta<List<SolicitudEnvioProgramado>> result = null;

            await Task.Run(() =>
            {

                result = EnvioProgramadoBL.InsertarDatos(objeto);

            });

            return JsonConvert.SerializeObject(result);
        }

        /// <summary> 
        /// 19/10/2022
        /// Francisco Vindas Ruiz
        /// Metodo para eliminar envio programado
        /// </summary>
        /// <param name="idSolicitud></param>
        /// <returns>JSON</returns>
        [HttpPost]
        public async Task<string> EliminarEnvioProgramado(SolicitudEnvioProgramado objeto)
        {

            RespuestaConsulta<List<SolicitudEnvioProgramado>> result = null;

            await Task.Run(() =>
            {
                result = EnvioProgramadoBL.EliminarElemento(new SolicitudEnvioProgramado()
                {
                    id = objeto.id
                });

            });
            return JsonConvert.SerializeObject(result);
        }

        #endregion

    }
}
