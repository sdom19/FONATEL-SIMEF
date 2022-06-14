using GB.SUTEL.BL.UmbralesPesosRelativos;
using GB.SUTEL.Entities.UmbralesPesosRelativos;
using GB.SUTEL.UI.Helpers;
using GB.SUTEL.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using GB.SUTEL.UI.Recursos.Utilidades;

namespace GB.SUTEL.UI.Controllers
{

    [Authorize]
    public class IndicadorUmbralController : BaseController
    {
        Funcion func = new Funcion();
        // GET: IndicadorUmbarl
        //[Authorize(Roles = "Administrador")]
        public ActionResult Index()
        {
            var LisModel = new UmbralesPesosRelativosViewModel();
            // se obtiene le listad de Direcciones
            var LisDireccion = LisModel.LstDireccion();
            ViewBag.LisDireccion = new SelectList(LisDireccion, "Value", "Text");
            // se obtiene el listado de servicios
            var LisServicios = LisModel.LstServicios();
            ViewBag.LisServicios = new SelectList(LisServicios, "Value", "Text");

            var LisUsuarios = LisModel.LstUsuarios();
            ViewBag.LisUsuarios = new SelectList(LisUsuarios, "Value", "Text");
            var Model = new UmbralesPesosRelativosViewModel();
            string user;
            user = User.Identity.GetUserId();
            try
            {
                func._index(user, "Calidad Umbrales", "Umbrales y Pesos Relativos Calidad");
            }
           
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }
            return View(Model);
        }

        //[Authorize(Roles = "Administrador")]
        [HttpPost]
        public ActionResult GetDatosIndicadorXServicio(int IdServicios, int IdDireccion)
        {
            var Result = new UmbralesPesosRelativosBL().GetLisIndicadorXservicio(IdServicios, IdDireccion);
            return Json(Result);
        }

        //[Authorize(Roles = "Administrador")]
        [HttpPost]
        public ActionResult GetDatosIndicadorXServicioFechas(int IdServicios, DateTime FechaInic, DateTime FechaFin, int IdDireccion)
        {
            //string formarinic = FechaInic.ToString("yyyy-dd-MM");
            //string formarFinc = FechaFin.ToString("yyyy-dd-MM");

            var Result = new UmbralesPesosRelativosBL().GetLisIndicadorXservicio(IdServicios, FechaInic, FechaFin, IdDireccion);
            return Json(Result);

        }


        //[Authorize(Roles = "Administrador")]
        [HttpPost]
        public ActionResult GetDatosUsuarios()
        {
            var Result = new UmbralesPesosRelativosBL().GetUser();
            return Json(Result);

        }

        //[Authorize(Roles = "Administrador")]
        [HttpPost]
        public ActionResult GetDatosIndicadorXServicioUsuarios(int IdServicios, string Usuario, int IdDireccion)
        {
            var Result = new UmbralesPesosRelativosBL().GetLisIndicadorXservicio(IdServicios, Usuario, IdDireccion);
            return Json(Result);
        }


        //[Authorize(Roles = "Administrador")]
        [HttpPost]
        public ActionResult CrearImbralesIndicadores(UmbralesPesosRelativosViewModel Mimodel)
        {

          
            ///Obtenemos el nombre del usuario que inicio sessión
            string Username = User.Identity.Name == null || User.Identity.Name == "" ? "SitelAdm" : User.Identity.Name;
           
            var ServerData = new ServiIndicadorEnti
            {
                IdIndicador = Mimodel.IdIndicador,
                Peso = Mimodel.Peso,
                Umbral = Mimodel.Umbral,
                Usuario = Username               
        };

            string peso= Mimodel.Peso.ToString();
            string umb= Mimodel.Umbral.ToString();
            string[] umbral = new string[10];
            umbral[0]= "Id Indicador: " + Mimodel.IdIndicador.ToString();
            umbral[1]= ", Peso: " + peso;
            umbral[2] = ", Umbral: " + umb;

            string um = umbral[0] + umbral[1] + umbral[2];
                 
            var TransPesoUmbral = new UmbralesPesosRelativosBL().CrearIndicadorUmbral(ServerData);
            string _ConfMensaje = "";
          
            switch (TransPesoUmbral)
            {
                case 1:
                    _ConfMensaje = "de indicador actualizada correctamente";
                    break;
                case 2:
                    _ConfMensaje = "EL proceso se ha realizado de manera sastifactoria";
                    break;
                case 3:
                    _ConfMensaje = "No ha sido posible realizar el proceso,intente nuevamene";
                    break;
                default:
                    break;
            }
            List<string> Mensaje = new List<string>(new string[] { _ConfMensaje, TransPesoUmbral.ToString() });
   
        
           //try
           //{
           //    SUTEL_IndicadoresEntities db = new SUTEL_IndicadoresEntities();
           //         db.Bitacora.Add(new Bitacora()
           // {
           //        Usuario = User.Identity.GetUserId(),
           //        FechaBitacora = DateTime.Now,
           //        Pantalla = "Umbrales y Pesos Relativos",
           //        Descripcion = "Proceso de insercion de Umbrales y Pesos Relativos Calidad",
           //       Accion = 1,
           //        RegistroAnterior = " ",
           //        RegistroNuevo = um
           //    }); ;
           //    db.SaveChanges();
           //}
           //catch (Exception e)
           //{
           //    Console.WriteLine("{0} Exception caught.", e);
           //}
            
            return Json(Mensaje);

        }
        

    }
}