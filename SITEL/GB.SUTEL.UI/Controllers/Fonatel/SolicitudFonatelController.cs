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
    public class SolicitudFonatelController : Controller
    {

        #region Variables publicas


        private readonly SolicitudBL SolicitudesBL;
        private readonly AnnoBL AnnoBL;
        private readonly MesBL MesBL;
        private readonly FuentesRegistroBL fuenteBl;
        private readonly FormularioWebBL formularioWebBL;
        private readonly DetalleSolicitudesBL detalleSolicitudesBL;



        string user;

        #endregion


        public SolicitudFonatelController()
        {
            SolicitudesBL = new SolicitudBL();
            AnnoBL = new AnnoBL();
            MesBL = new MesBL();
            fuenteBl = new FuentesRegistroBL(EtiquetasViewSolicitudes.Solicitudes, System.Web.HttpContext.Current.User.Identity.GetUserId());
            formularioWebBL = new FormularioWebBL(EtiquetasViewSolicitudes.Solicitudes, System.Web.HttpContext.Current.User.Identity.GetUserId());
            detalleSolicitudesBL = new DetalleSolicitudesBL();
        }

        // GET: Solicitud
        public ActionResult Index()
        {
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
            ViewBag.ListaFormularioWeb = formularioWebBL.ObtenerDatos(new FormularioWeb())
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

            //VALIDACIONES DE NOMBRES CODIGOS ETC

            user = User.Identity.GetUserId();
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
        /// Fecha: 04
        /// Francisco Vindas
        /// Metodo para editar solicitudes de informacion
        /// </summary>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> EditarSolicitud(Solicitud solicitud)
        {
            
            user = User.Identity.GetUserId();
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

            user = User.Identity.GetUserId();
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
            user = User.Identity.GetUserId();

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
        public async Task<string> EliminarDetallesSolicitud(DetalleSolicitudFormulario Solicitud)
        {
            user = User.Identity.GetUserId();

            RespuestaConsulta<List<DetalleSolicitudFormulario>> result = null;

            Solicitud.Estado = false;

            await Task.Run(() =>
            {
                result = detalleSolicitudesBL.CambioEstado(Solicitud);
                
            });
            return JsonConvert.SerializeObject(result);
        }

        #endregion

    }
}
