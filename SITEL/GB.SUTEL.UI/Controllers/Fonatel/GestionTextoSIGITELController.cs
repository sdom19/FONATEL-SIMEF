using GB.SIMEF.BL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using GB.SUTEL.UI.Filters;
using GB.SUTEL.UI.Helpers;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GB.SUTEL.UI.Controllers.Fonatel
{
    [AuthorizeUserAttribute]
    public class GestionTextoSIGITELController : Controller
    {
        GestionTextoSIGITELBL gestionTextoSIGITELBL;

        public GestionTextoSIGITELController()
        {
            gestionTextoSIGITELBL = new GestionTextoSIGITELBL(EtiquetasViewGestionTextoSIGITEL.GestionTextoSIGITEL, System.Web.HttpContext.Current.User.Identity.GetUserId());
        }
        // GET: GestionTextoSIGITEL
        public ActionResult Index()
        {
            var data = gestionTextoSIGITELBL.ObtenerPantallasSIGITEL();
            ViewBag.datos = data;
            return View();
        }

        [ConsultasFonatelFilter]
        public ActionResult Create()
        {
            var tipoContenido = gestionTextoSIGITELBL.ObtenerTipoContenido();
            ViewBag.tipoContenido = tipoContenido.objetoRespuesta.Select(i => new SelectListItem() { Selected = false, Text = i.NombreTipoContenido, Value = i.IdTipoContenidoTextoSIGITEL.ToString() });
            return View();
        }

        /// <summary>
        /// Fecha 19/04/2022
        /// Adolfo Cunquero
        /// Guardar detalle de contenido para pantalla SIGITEL
        /// </summary>
        [HttpPost]
        [ConsultasFonatelFilter]
        public async Task<string> GuardarDetalle(Object datos)
        {
            JObject objData = JObject.Parse(((string[])datos)[0]);
            ContenidoPantallaSIGITEL obj = JsonConvert.DeserializeObject<ContenidoPantallaSIGITEL>(objData.GetValue("datos").ToString());

            if (Request.Files.Count > 0)
            {
                if (obj.IdContenidoPantallaSIGITEL != 0)
                {
                    EliminarImagen(obj.RutaImagen);
                }

                string extension = Path.GetExtension(Request.Files[0].FileName);
                obj.RutaImagen = ConfigurationManager.AppSettings["RutaImagenesSIGITEL"] + ObtenerNombreArchivo(extension);
                Request.Files[0].SaveAs(Server.MapPath(obj.RutaImagen));
            }

            if (!obj.Estado)
            {
                EliminarImagen(obj.RutaImagen);
            }    

            var respuesta = new RespuestaConsulta<List<ContenidoPantallaSIGITEL>>();

            await Task.Run(() =>
            {
                respuesta = gestionTextoSIGITELBL.ActualizarDatos(obj);
            });

            return JsonConvert.SerializeObject(respuesta);
        }


        /// <summary>
        /// Fecha 19/04/2022
        /// Adolfo Cunquero
        /// Obtiene detalle de contenido para pantalla SIGITEL
        /// </summary>
        [HttpPost]
        public async Task<string> ObtenerDetallePantalla(ContenidoPantallaSIGITEL obj)
        {
            var respuesta = new RespuestaConsulta<List<ContenidoPantallaSIGITEL>>();
            await Task.Run(() =>
            {
                respuesta = gestionTextoSIGITELBL.ObtenerDatos(obj);
            });
            return JsonConvert.SerializeObject(respuesta);
        }

        /// <summary>
        /// Fecha 20/04/2022
        /// Adolfo Cunquero
        /// Genera un nombre unico para cada imagen para no reemplazar una existente
        /// </summary>
        private string ObtenerNombreArchivo(string ext)
        {
            long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            return string.Format("img-{0}{1}", milliseconds.ToString(), ext);
        }

        /// <summary>
        /// Fecha 20/04/2022
        /// Adolfo Cunquero
        /// Si una imagen existe la borra
        /// </summary>
        private void EliminarImagen(string filePath)
        {
            filePath = Server.MapPath(filePath);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }
}