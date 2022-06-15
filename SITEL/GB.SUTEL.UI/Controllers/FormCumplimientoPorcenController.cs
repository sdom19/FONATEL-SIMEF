using GB.SUTEL.BL.FormCumplimientoPorcenBL;
using GB.SUTEL.BL.UmbralesPesosRelativos;
using GB.SUTEL.DAL;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.FormCumplimientoPorcenEnti;
using GB.SUTEL.UI.Helpers;
using GB.SUTEL.UI.Models;
using GB.SUTEL.UI.Recursos.Utilidades;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace GB.SUTEL.UI.Controllers
{
    [Authorize]
    public class FormCumplimientoPorcenController : Controller
    {
        Funcion func = new Funcion();

        //[Authorize(Roles = "Administrador")]
        public ActionResult EjecutarFormulas() {
            string user;
            user = User.Identity.GetUserId();
            try
            {
                func._index(user, "Calidad Formulas", "Programación de ejecución (Fórmulas)");
            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }

            return View(); }


        // GET: FormCumplimeintoPorc
        //[Authorize(Roles = "Administrador")]
        public ActionResult Index()
        {
            var LisModel = new FormCumplimeintoPorcenControViewMdel();
            // se obtiene le listad de Direcciones
            var LisDireccion = LisModel.LstDireccion();
            ViewBag.LisDireccion = new SelectList(LisDireccion, "Value", "Text");
            // se obtiene el listado de servicios
            var LisServicios = LisModel.LstServicios();
            ViewBag.LisServicios = new SelectList(LisServicios, "Value", "Text");
            var Model = new FormCumplimeintoPorcenControViewMdel();
            string user;
            user = User.Identity.GetUserId();
            try
            {
                func._index(user, "Calidad Formulas", "Fórmulas de Porcentaje y Cumplimiento Calidad");
            }
            
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }
            return View(Model);
        }


        [HttpPost]
        //[Authorize(Roles = "Administrador")]
        public ActionResult GetDatosIndicadorXServicio(int IdServicios, int IdDireccion)
        {
            var Result = new UmbralesPesosRelativosBL().GetLisIndicadorXservicio(IdServicios, IdDireccion);
            return Json(Result);
        }


        [HttpPost]
        //[Authorize(Roles = "Administrador")]
        public ActionResult GetCriteriosXIndicador(string IdIndicador)
        {
            var ResultConsult = new FormCumplimientoPorcenBL().LisCreteriosXIndicador(IdIndicador);
            return Json(ResultConsult);
        }

        [HttpPost]
        //[Authorize(Roles = "Administrador")]
        public ActionResult CrearParamFormulas(FormCumplimeintoPorcenControViewMdel Mimodel)
        {
            ///Obtenemos el nombre del usuario que inicio sessión
            string Username = User.Identity.Name == null || User.Identity.Name == "" ? "SitelAdm" : User.Identity.Name;

            var ServerData = new FormCumplimientoPorcenEnti
            {
                IdIndicador = Mimodel.IdIndicador,
                Usuario = Username,
                FormulaCumplimiento = Mimodel.FormulaCumplimiento,
                FormulaPorcentaje = Mimodel.FormulaPorcentaje,
                Criterios = Mimodel.Criterios,
                IdServicio = Mimodel.IdServicio,
                FromArray = Mimodel.FromArray,
                ArrayIF = Mimodel.ArrayIF,
                ArrayVerdadero = Mimodel.ArrayVerdadero,
                ArrayFalso = Mimodel.ArrayFalso
            };

           

            var TransParamFormula = new FormCumplimientoPorcenBL().CrearParamFormula(ServerData);
            string[] form = new string[9];
            form[0] = "Id Indicador: " + Mimodel.IdIndicador.ToString();
            form[1] = ", Formula de Cumplimiento: " + Mimodel.FormulaCumplimiento.ToString();
            form[2] = ", Formula de Porcentaje: " + Mimodel.FormulaPorcentaje.ToString();
            form[3] = ", Criterios: " + Mimodel.Criterios.ToString();
            form[4] = ", Id Servicio: " + Mimodel.IdServicio.ToString();
            form[5] = ", FromArray: " + Mimodel.FromArray.ToString();
            form[6] = ", Array IF: " + Mimodel.ArrayIF.ToString();
            form[7] = ", Array Verdadero: " + Mimodel.ArrayVerdadero.ToString();
            form[8] = ", Array Falso: " + Mimodel.ArrayFalso.ToString();
            string f = form[0] + form[1] + form[2] + form[3] + form[4] + form[5] + form[6] + form[7] + form[8];
            string _ConfMensaje = "";
            switch (TransParamFormula)
            {
                case 1:
                    _ConfMensaje = "Se ha actualizado el indicador de manera correcta";
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
            List<string> Mensaje = new List<string>(new string[] { _ConfMensaje, TransParamFormula.ToString() });
            
            try
            {
               SUTEL_IndicadoresEntities db = new SUTEL_IndicadoresEntities();

                db.Bitacora.Add(new Bitacora()
                {
                    Usuario = User.Identity.GetUserId(),
                    FechaBitacora = DateTime.Now,
                    Pantalla = "Calidad Formulas",
                    Descripcion = "Proceso de edición de Formulas de Procentajes y Cumplimiento",
                    Accion = 3,
                    RegistroAnterior = "",
                    RegistroNuevo = f
                }); ;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }




            return Json(Mensaje);

        }

        [HttpPost]
        //[Authorize(Roles = "Administrador")]
        public ActionResult PeriodosEjecutados()
        {
            var ResultConsult = new FormCumplimientoPorcenBL().PeridosEjecutados(DateTime.Now.Year);
            return Json(ResultConsult);
        }

        [HttpPost]
        //[Authorize(Roles = "Administrador")]
        public ActionResult ParamEjecucion(int Periodo, int anio, DateTime Fecha)
        {
            string Username = User.Identity.Name == null || User.Identity.Name == "" ? "SitelAdm" : User.Identity.Name;
            var ResultConsult = new FormCumplimientoPorcenBL().ParametroEjecucion(Periodo, Username, anio, Fecha);
            string _ConfMensaje = "";
            switch (ResultConsult)
            {
                case 1:
                    _ConfMensaje = " Programación de ejecución exitosa";
                    break;
                case 2:
                    _ConfMensaje = "No ha sido posible realizar el proceso,intente nuevamene";
                    break;
                default:
                    break;
            }
            List<string> Mensaje = new List<string>(new string[] { _ConfMensaje, ResultConsult.ToString() });


            try
            {
                SUTEL_IndicadoresEntities dba = new SUTEL_IndicadoresEntities();

                dba.Bitacora.Add(new Bitacora()

                {


                    Usuario = User.Identity.GetUserId(),
                    FechaBitacora = DateTime.Now,
                    Pantalla = "Calidad Formulas",
                    Descripcion = "Proceso de creación en: Programación de ejecución (Fórmulas)",
                    Accion = 2,
                    RegistroAnterior = "",
                    RegistroNuevo = "Periodo: " + Periodo + ",  Año: " + anio + ",  Fecha: " + Fecha



                }); ; ; ; ;
                dba.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);


            }





            return Json(Mensaje);
        }

        [HttpGet]
        //[Authorize(Roles = "Administrador")]
        public ActionResult GetLiprocesarMotor()
        {
            var ResultConsult = new FormCumplimientoPorcenBL().LisProcesarEjecuciones();
            return Json(ResultConsult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        //[Authorize(Roles = "Administrador")]
        public ActionResult GetLiprocesadasMotor()
        {
            var ResultConsult = new FormCumplimientoPorcenBL().LisProcesosEjecutados();
            return Json(ResultConsult, JsonRequestBehavior.AllowGet);
        }
     
   
        public ActionResult AnularEjecucion(int IdEjecucion)
        {
            var ResultConsult = new FormCumplimientoPorcenBL().AnularEjecucion(IdEjecucion);
           EjecucionMotorEnti db = new EjecucionMotorEnti();
            int id;
            id = System.Convert.ToInt32(IdEjecucion);
       
                try
                {
                    SUTEL_IndicadoresEntities dba = new SUTEL_IndicadoresEntities();
                var entidad = dba.EjecucionMotor.Find(IdEjecucion);
                var anio = entidad.anioEjecucion.ToString();
                var perdiodo = entidad.periodoEjecucion.ToString();
                var fecha = entidad.FechaRegistro.ToString();
                var ejecucion = entidad.Ejecutado.ToString();
                String old = "Id: " + id + "Periodo: " + perdiodo + " Año: " + anio + " Fecha de ejecucion: " + fecha + " Ejecucion: " + ejecucion;

                dba.Bitacora.Add(new Bitacora()

                {


                    Usuario = User.Identity.GetUserId(),
                    FechaBitacora = DateTime.Now,
                    Pantalla = "Calidad Formulas",
                    Descripcion = "Proceso de eliminacion en: Programación de ejecución (Fórmulas)",
                    Accion = 4,
                    RegistroAnterior = old,
                    RegistroNuevo = " "
                   


                    }); ; ; ;
                    dba.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Exception caught.", e);
            

            }

            return Json(ResultConsult, JsonRequestBehavior.AllowGet);
        }
    }
}