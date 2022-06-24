
using GB.SUTEL.UI.Recursos.Utilidades;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GB.SUTEL.DAL;
using GB.SUTEL.Entities;

namespace GB.SUTEL.Controllers
{

    public class SolicitudGeneralsController : Controller
    {
        Funcion func = new Funcion();
        //Conexión a la base de datos
        private SUTEL_IndicadoresEntities db = new SUTEL_IndicadoresEntities();

        /// <summary>
        /// Vista Index
        /// </summary>
        /// <returns>Vista</returns>
        public ActionResult Index()
        {
            //Consultar a la BD las solicitudes
            IQueryable<SolicitudGeneral> solicitudGeneral = db.SolicitudGeneral.Include(s => s.Usuario).Where(u => u.Borrado == false);
            //Retornar vista con lista}
            string user;
            user = User.Identity.GetUserId();
            try
            {
                func._index(user, "Solicitud General Index", "Solicitud General Procesos");
            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }

            return View(solicitudGeneral.ToList());
        }


        /// <summary>
        /// Vista Create
        /// </summary>
        /// <returns>Vista</returns>
        [HttpGet]
        public ActionResult Create()
        {
            //Retorna vista
            return View();
        }

        /// <summary>
        /// Vista Edit
        /// </summary>
        /// <param name="id">Id solicitud</param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Consultar a la BD la solicitud con ese id
            SolicitudGeneral solicitudGeneral = db.SolicitudGeneral.Find(id);
            if (solicitudGeneral == null)
            {
                return HttpNotFound();
            }
            //Consultar a la BD los usuarios
            ViewBag.IdUsuario = new SelectList(db.Usuario, "IdUsuario", "IdOperador", solicitudGeneral.IdUsuario);
            //Retornar vista con solicitud
            return View(solicitudGeneral);
        }

        /// <summary>
        /// Método para editar solicitud
        /// </summary>
        /// <param name="IdSolicitud">Id de Solicitud</param>
        /// <param name="Descripcion">Descripción de solicitud</param>
        /// <param name="Activo">Activo de solicitud</param>
        /// <param name="file">Archivo de solicitud</param>
        /// <returns>Resultado de la operación</returns>
        public JsonResult Editar(int IdSolicitud, string Descripcion, bool Activo, HttpPostedFileBase file)
        {
            try
            {
                //Consultar a la BD la solicitud
                SolicitudGeneral solicitudGeneral = db.SolicitudGeneral.Find(IdSolicitud);
                //Modificar solicitud
                solicitudGeneral.Descripcion = Descripcion;
                solicitudGeneral.Activo = Activo;
                if (file != null)
                {
                    //Actualizar archivo de solicitud
                    var files = Request.Files;
                    var path = $"/Archivos/{solicitudGeneral.IdSolicitud}-{solicitudGeneral.FechaInicio.ToString("ddMMyyyy")}-{solicitudGeneral.FechaFinal.ToString("ddMMyyyy")}/ ";
                    System.IO.File.SetAttributes(solicitudGeneral.Path, FileAttributes.Normal);
                    System.IO.File.Delete(solicitudGeneral.Path);
                    string filePath = HttpContext.Server.MapPath(path) + file.FileName.Replace(" ", string.Empty);
                    file.SaveAs(filePath);
                    solicitudGeneral.Path = filePath;
                }
                //Gaurdar cambios
                db.Entry(solicitudGeneral).State = EntityState.Modified;
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
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
        /// Método para guardar solicitud
        /// </summary>
        /// <param name="Nombre">Nombre solicitud</param>
        /// <param name="Descripcion">Descripcion solicitud</param>
        /// <param name="FechaInicio">Fecha Inicio solicitud</param>
        /// <param name="FechaFinal">Fecha Final solicitud</param>
        /// <param name="Mercados">Mercados solicitud</param>
        /// <param name="Calidad">Calidad solicitud</param>
        /// <param name="FONATEL"> FONATELsolicitud</param>
        /// <param name="file">Archivos solicitud</param>
        /// <returns>Resultado de la operación con id</returns>
        public JsonResult GuardarIndicador(string Nombre, string Descripcion, string FechaInicio, string FechaFinal, bool Mercados, bool Calidad, bool FONATEL, HttpPostedFileBase file)
        {
            //Consultar a la BD el usuario
            Usuario usuario = db.Usuario.FirstOrDefault(x => x.NombreUsuario == Nombre);
            SolicitudGeneral solicitudGeneral = new SolicitudGeneral()
            {
                IdUsuario = usuario.IdUsuario,

                Descripcion = Descripcion,
                FechaInicio = DateTime.Parse(FechaInicio),
                FechaFinal = DateTime.Parse(FechaFinal),
                Activo = true,
                Path = ""
            };
            //Guardar solicitud
            SolicitudGeneral solicitudNueva = db.SolicitudGeneral.Add(solicitudGeneral);
            db.SaveChanges();
            //Guardar archivo 
            var path = $"/Archivos/{solicitudNueva.IdSolicitud}-{solicitudNueva.FechaInicio.ToString("ddMMyyyy")}-{solicitudNueva.FechaFinal.ToString("ddMMyyyy")}/ ";
            var files = Request.Files;
            try
            {
                if (!Directory.Exists(HttpContext.Server.MapPath(path)))
                {
                    Directory.CreateDirectory(HttpContext.Server.MapPath(path));
                }
                string filePath = HttpContext.Server.MapPath(path) + file.FileName.Replace(" ", string.Empty);
                if (!System.IO.File.Exists(filePath))
                {
                    file.SaveAs(filePath);
                    solicitudNueva.Path = filePath;
                    db.Entry(solicitudNueva).State = EntityState.Modified;
                    db.SaveChanges();

                }
                else
                {
                    throw new Exception("Ya existe un archivo con el mismo nombre");
                }
            }
            catch (Exception ex)
            {
                var a = ex;
                throw ex;
            }
            //Retorna id de la solicitud
            return Json(solicitudNueva.IdSolicitud, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Método para obtener oepradores 
        /// </summary>
        /// <param name="Mercados">Mercador</param>
        /// <param name="Calidad">Calidad</param>
        /// <param name="FONATEL">FONATEL</param>
        /// <returns>Lista de operadores</returns>
        public JsonResult Operadores(bool Mercados, bool Calidad, bool FONATEL)
        {
            int MercadosInt = Mercados ? 1 : 0;
            int CalidadInt = Calidad ? 1 : 0;
            int FONATELInt = FONATEL ? 1 : 0;
            //Consultar a la BD los oepradores filtrados
            var operadoresId = db.Usuario.Where(u => (u.Mercado.HasValue && u.Mercado == true && u.Mercado == Mercados) || (u.Calidad.HasValue && u.Calidad == true && u.Calidad == Calidad) || (u.FONATEL.HasValue && u.FONATEL == true && u.FONATEL == FONATEL)).Select(u => u.IdOperador).Distinct();
            List<Operador> operadores = new List<Operador>();
            foreach (var IdOperador in operadoresId)
            {
                //Consultar a la BD el operador
                Operador operador = db.Operador.Find(IdOperador);
                //Agregar operadores a lista
                operadores.Add(new Operador()
                {
                    IdOperador = operador.IdOperador,
                    NombreOperador = operador.NombreOperador
                });
            }
            //Retornar operadores en formato JSON
            return Json(operadores, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Método para guardar operador
        /// </summary>
        /// <param name="IdSolicitud">Id de Solicitud</param>
        /// <param name="IdOperador">Id de Operador</param>
        /// <param name="Mercados">Mercados</param>
        /// <param name="Calidad">Calidad</param>
        /// <param name="FONATEL">FONATEL</param>
        /// <returns>Retorna resultado de la operación</returns>
        public JsonResult GuardarOperador(int IdSolicitud, string IdOperador, bool Mercados, bool Calidad, bool FONATEL)
        {
            //Consultar a la BD el usuario, solicitud y operador
            var usuarios = db.Usuario.Where(u => ((u.Mercado.HasValue && u.Mercado == true && u.Mercado == Mercados) || (u.Calidad.HasValue && u.Calidad == true && u.Calidad == Calidad) || (u.FONATEL.HasValue && u.FONATEL == true && u.FONATEL == FONATEL)) && u.IdOperador == IdOperador).Select(u => u.IdUsuario);
            SolicitudGeneral solicitudGeneral = db.SolicitudGeneral.Find(IdSolicitud);
            Operador operador = db.Operador.Find(IdOperador);
            //Path archivo
            var Path = $"/Archivos/{IdSolicitud}-{solicitudGeneral.FechaInicio.ToString("ddMMyyyy")}-{solicitudGeneral.FechaFinal.ToString("ddMMyyyy")}/{IdOperador}/";
            try
            {
                foreach (var usuario in usuarios)
                {
                    //Crear solicitud operador para cada usuario
                    SolicitudOperador solicitudOperador = new SolicitudOperador()
                    {
                        IdSolicitud = IdSolicitud,
                        IdUsuario = usuario,
                        Path = ""
                    };
                    //Agregar solicitud

                    db.SolicitudOperador.Add(solicitudOperador);
                    string user;
                    user = User.Identity.GetUserId();
                    func.solicitud(user, "Guardar Operador en : Solicitud General", 2, "id solicitud: " + IdSolicitud + ",  id operador: " + IdOperador, " ");
                }
                db.SaveChanges();
                //Guardar cambios
                /*
                
               
               
     */
                //Crear carpeta
                if (!Directory.Exists(HttpContext.Server.MapPath(Path)))
                    Directory.CreateDirectory(HttpContext.Server.MapPath(Path));


                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Método para eliminar solicitud
        /// </summary>
        /// <param name="IdSolicitud">Id de Solicitud</param>
        /// <returns>Resultado de la operación</returns>
        public JsonResult Eliminar(int IdSolicitud)
        {
            try
            {
                //Consultar a la BD la solicitud
                var solicitud = db.SolicitudGeneral.Find(IdSolicitud);
                //Activar borrado
                solicitud.Borrado = true;
                //Guardar cambios
                db.Entry(solicitud).State = EntityState.Modified;
                string info = "Id: " + solicitud.IdSolicitud + "  ,descripcion: " + solicitud.Descripcion + ",  fecha de inicio: " + solicitud.FechaInicio + "  ,fecha final: " + solicitud.FechaFinal;

                string user;
                user = User.Identity.GetUserId();
                func.solicitud(user, "Proceso de eliminacion de:  Solicitud General", 4, info, "");
                db.SaveChanges();


                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Método para ver operadores
        /// </summary>
        /// <param name="IdSolicitud">Id de Solicitud</param>
        /// <returns>Lista de operadores</returns>
        public JsonResult verOperadores(int IdSolicitud) => Json(db.Operador.Where(o => o.Usuario.Any(u => u.SolicitudOperador.Any(s => s.IdSolicitud == IdSolicitud))).Select(o => o.NombreOperador).ToList(), JsonRequestBehavior.AllowGet);

        /// <summary>
        /// Método para enviar notificaciones
        /// </summary>
        /// <param name="IdSolicitud">d de Solicitud</param>
        /// <returns>Resultado de la operación</returns>
        public JsonResult EnviarNotificaciones(int IdSolicitud)
        {
            try
            {
                //Enviar notificaciones
                db.Database.ExecuteSqlCommand("EXEC pa_enviarNotificacionesGeneral @IdSolicitud = " + IdSolicitud);
                string user;
                user = User.Identity.GetUserId();
                func.solicitud(user, "Se ha enviado una solicitud general", 2, "Id solicitud: " + IdSolicitud, "");
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //Corregir error
                SolicitudGeneral entity = db.SolicitudGeneral.Find(IdSolicitud);
                entity.NotificacionEnviada = false;
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Método para retornar achivo
        /// </summary>
        /// <param name="IdSolicitud">Id de Solicitud</param>
        /// <returns>Archivo</returns>
        public ActionResult ReturnFile(int IdSolicitud)
        {
            //Consultar a la BD la solicitud
            SolicitudGeneral solicitudGeneral = db.SolicitudGeneral.Find((object)IdSolicitud);
            //Retornar archivo
            return File(System.IO.File.ReadAllBytes(solicitudGeneral.Path), "application/octet-stream", solicitudGeneral.Path.Split('\\').Last());
        }
    }
}
