using GB.SIMEF.BL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
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


        string user;

        #endregion


        public SolicitudFonatelController()
        {
            SolicitudesBL = new SolicitudBL();
            AnnoBL = new AnnoBL();
            MesBL = new MesBL();
            fuenteBl = new FuentesRegistroBL();
            formularioWebBL = new FormularioWebBL();

        }




        // GET: Solicitud
        public ActionResult Index()
        {
            return View();
        }

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
            }
            return View(model);
        }






        public async Task<string> ObtenerListaSolicitudes()
        {
            RespuestaConsulta<List<Solicitud>> result = null;
            await Task.Run(() =>
            {
                result = SolicitudesBL.ObtenerDatos(new Solicitud());
            });

            return JsonConvert.SerializeObject(result);


        }



    }
}
