
using GB.SUTEL.UI.Recursos.Utilidades;
using Microsoft.AspNet.Identity;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GB.SUTEL.DAL;
using GB.SUTEL.Entities;

namespace GB.SUTEL.UI.Controllers
{
    public class SolicitudOperadorsController : Controller
    {
        private SUTEL_IndicadoresEntities db = new SUTEL_IndicadoresEntities();

        // GET: SolicitudOperadors
        /// <summary>
        /// Vista Index
        /// </summary>
        /// <param name="Nombre">Nombre del usuario actual</param>
        /// <returns>Vista</returns>
        public ActionResult Index(string Nombre)
        {
            Funcion func = new Funcion();
            //Consultar a la BD el usuario actual
            Usuario usuario = db.Usuario.FirstOrDefault(x => x.NombreUsuario == Nombre);
            DateTime now = DateTime.Now.Date;
            //Consultar a la BD las solicitadas filtradas
            var solicitudOperador = db.SolicitudOperador.Include(s => s.SolicitudGeneral).Include(s => s.Usuario).Where(s => s.IdUsuario == usuario.IdUsuario && s.Respuesta == false && DateTime.Compare(s.SolicitudGeneral.FechaFinal, now) >= 0).ToList();
            //retorna vista

            string user;
            user = User.Identity.GetUserId();
            try
            {
                func._index(user, "Solicitud", "Solicitud Operadores Procesos");
            }
           
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }

            return View(solicitudOperador);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Método para guardar los archivos de la solicitud
        /// </summary>
        /// <param name="IdSolicitud"></param>
        /// <param name="IdUsuario"></param>
        /// <param name="file"></param>
        /// <returns>Resultado de la operación</returns>
        public JsonResult Upload(int IdSolicitud, int IdUsuario, HttpPostedFileBase file)
        {
            if (file != null)
            {
                //Consultar a la BD las solicitudes
                SolicitudGeneral solicitudGeneral = db.SolicitudGeneral.Find(IdSolicitud);
                SolicitudOperador solicitudOperador = db.SolicitudOperador.Where(x => x.IdSolicitud == IdSolicitud && x.IdUsuario == IdUsuario).FirstOrDefault();
                //Path de carpeta
                var Path = $"/Archivos/{IdSolicitud}-{solicitudGeneral.FechaInicio.ToString("ddMMyyyy")}-{solicitudGeneral.FechaFinal.ToString("ddMMyyyy")}/{solicitudOperador.Usuario.IdOperador}/";
                var files = Request.Files;
                try
                {
                    //Path archivo
                    string filePath = HttpContext.Server.MapPath(Path) + file.FileName.Replace(" ", string.Empty);
                    if (!System.IO.File.Exists(filePath))
                    {
                        //Guardar archivo
                        file.SaveAs(filePath);
                        //Consultar a la BD las solicitudes operador
                        foreach (SolicitudOperador solicitud in db.SolicitudOperador.Where(x => x.Usuario.IdOperador == solicitudOperador.Usuario.IdOperador && x.SolicitudGeneral.IdSolicitud == IdSolicitud).ToList())
                        {
                            //Actualizar path en solicitudes
                            solicitud.Path = filePath;
                            solicitud.Respuesta = true;
                            db.Entry(solicitud).State = EntityState.Modified;
                        }
                        //Guardar cambios
                        db.SaveChanges();
                        //Enviar notificaciones
                        db.Database.ExecuteSqlCommand("EXEC pa_EnviarRespuestaGeneral @IdSolicitud = " + IdSolicitud + ", @IdUsuario = " + IdUsuario);
                    }
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            else
                return Json(false, JsonRequestBehavior.AllowGet);
        }
    }
}
