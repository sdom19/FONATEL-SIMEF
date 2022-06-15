using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GB.SUTEL.BL.Mantenimientos;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.UI.Helpers;
using GB.SUTEL.UI.Recursos.Utilidades;
using System.Text;
using System.Security.Claims;
using System.IO;
using System.Data.OleDb;
using ClosedXML.Excel;
using System.Data;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using GB.SUTEL.BL.Proceso;
using GB.SUTEL.UI.Models;
using System.Configuration;
using Newtonsoft.Json;
using System.Web.Configuration;
using Microsoft.AspNet.Identity;

namespace GB.SUTEL.UI.Controllers
{
    [AuthorizeUserAttribute]
    public class RegistroIndicadorController : BaseController
    {
        #region Atributos
        SolicitudIndicadorBL solicitudBL;
        RegistroIndicadorBL refRegistroIndicadorBL;
        SolicitudIndicadorBL refSolicitudIndicadorBL;
        RegistroIndicadorInternoBL refRegistroIndicadorInternoBL;
        ServicioBL servicioBL;
        Funcion func = new Funcion();
        #endregion

        #region Constructor
        public RegistroIndicadorController()
        {
            solicitudBL = new SolicitudIndicadorBL(AppContext);
            refRegistroIndicadorBL = new RegistroIndicadorBL(AppContext);
            refSolicitudIndicadorBL = new SolicitudIndicadorBL(AppContext);
            servicioBL = new ServicioBL(AppContext);
            refRegistroIndicadorInternoBL = new RegistroIndicadorInternoBL(AppContext);
        }
        #endregion

        //
        // GET: /Registro Indicador/
        [AuthorizeUserAttribute]
        public ActionResult Index()
        {
            ViewBag.idSolicidud = "";
            string nombreUsuario = "";
            Operador objOperador = new Operador();

            if (((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault() != null)
            {
                nombreUsuario = ((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault().Value;
                BL.Seguridad.UsersBL refUsuario = new BL.Seguridad.UsersBL(AppContext);
                objOperador.IdOperador = refUsuario.ConsultarPorExpresion(x => x.AccesoUsuario == nombreUsuario && x.Borrado == 0 && x.Activo == 1).objObjeto.IdOperador;
            }


            if (objOperador.IdOperador == "" || objOperador.IdOperador == null)
            {
                return RedirectToAction("Index", "Login");
            }

            return View(objOperador);
        }

        public List<SelectListItem> ServicioList()
        {
            List<Servicio> myCollection = servicioBL.ConsultarTodos().objObjeto;
            return myCollection.Select(x => new SelectListItem { Value = x.IdServicio.ToString(), Text = x.DesServicio }).ToList();
        }

        /// <summary>
        /// editado por Diego
        /// </summary>
        /// <param name="objOperador"></param>
        /// <returns></returns>
        /// 

        [AuthorizeUserAttribute]
        public ActionResult _tableMercado(Operador objOperador, int idServicio = 0)
        {
            try
            {
                //ViewBag.ServicioList = ServicioList();

                ViewBag.searchTerm = new SolicitudIndicador();
                Respuesta<List<SolicitudIndicador>> objRespuesta = new Respuesta<List<SolicitudIndicador>>();
                Respuesta<List<ArchivoExcel>> objexcel = new Respuesta<List<ArchivoExcel>>();
                objexcel = refSolicitudIndicadorBL.ListarDescargaSolicitud(objOperador);
                objRespuesta = refSolicitudIndicadorBL.ListarSolicitudesParaOperadores(objOperador);
                objRespuesta.objObjeto = objRespuesta.objObjeto.Where(b => b.Direccion.IdDireccion == 1 && ( b.FormularioWeb == 0 || b.FormularioWeb == 3)).OrderBy(y => y.IdServicio).ToList();
                //ViewBag.ServicioList = ServicioList();

                List<Servicio> myCollection = servicioBL.ConsultarPorOperador(objOperador).objObjeto;
                List<Servicio> Servicios = new List<Servicio>();
                if (objRespuesta != null)
                {
                    foreach (var item in objRespuesta.objObjeto)
                    {
                        foreach (var item2 in myCollection)
                        {
                            if (item.IdServicio == item2.IdServicio)
                            {
                                bool alreadyExist = Servicios.Contains(item2);
                                if (alreadyExist)
                                {

                                }
                                else {
                                    Servicios.Add(item2);
                                }

                            }

                        }

                    }
                }


                ViewBag.ServicioList = Servicios.Select(x => new SelectListItem { Value = x.IdServicio.ToString(), Text = x.DesServicio }).ToList(); ;
                objexcel.objObjeto.Clear();
                objRespuesta.objObjeto.Clear();

                if (idServicio > 0)
                {
                    objexcel = refSolicitudIndicadorBL.ListarDescargaSolicitud(objOperador);
                    objRespuesta = refSolicitudIndicadorBL.ListarSolicitudesParaOperadores(objOperador);
                    objRespuesta.objObjeto = objRespuesta.objObjeto.Where(b => b.Direccion.IdDireccion == 1 && b.IdServicio == idServicio && (b.FormularioWeb == 0 || b.FormularioWeb == 3)).ToList();
                }

                gBitacora(ActionsBinnacle.Consultar, null, null);

                string user;
                user = User.Identity.GetUserId();
                try
                {
                    func._index(user, "Registro Indicadores", "Registro Indicadores Procesos");
                }

                catch (Exception e)
                {
                    Console.WriteLine("{0} Exception caught.", e);
                }

                return PartialView(new Tuple<List<SolicitudIndicador>, Operador, List<ArchivoExcel>>(objRespuesta.objObjeto, objOperador, objexcel.objObjeto));
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView(new Tuple<List<SolicitudIndicador>>(new List<SolicitudIndicador>()));
            }
            catch (Exception ex)
            {
                var newEx = AppContext.ExceptionBuilder.BuildException("Ha ocurrido un error al guardar", ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView(new Tuple<List<SolicitudIndicador>>(new List<SolicitudIndicador>()));
            }
        }


        [AuthorizeUserAttribute]
        public ActionResult _tableMercadoWeb(Operador objOperador, int idServicio = 0)
        {
            try
            {
                               
                ViewBag.searchTerm = new SolicitudIndicador();
                Respuesta<List<SolicitudIndicador>> objRespuesta = new Respuesta<List<SolicitudIndicador>>();
                Respuesta<List<SolicitudConstructor>> objexcel = new Respuesta<List<SolicitudConstructor>>();
                objexcel = refSolicitudIndicadorBL.ListarDescargaSolicitudWeb(objOperador);
                objRespuesta = refSolicitudIndicadorBL.ListarSolicitudesParaOperadoresWeb(objOperador);
                objRespuesta.objObjeto = objRespuesta.objObjeto.Where(b => b.Direccion.IdDireccion == 1 && (b.FormularioWeb == 1 || b.FormularioWeb == 3)).OrderBy(y => y.IdServicio).ToList();
                //ViewBag.ServicioList = ServicioList();

                List<Servicio> myCollection = servicioBL.ConsultarPorOperador(objOperador).objObjeto;
                List<Servicio> Servicios = new List<Servicio>();
                if (objRespuesta != null)
                {
                    foreach (var item in objRespuesta.objObjeto)
                    {
                        foreach (var item2 in myCollection)
                        {
                            if(item.IdServicio == item2.IdServicio)
                            {
                                bool alreadyExist = Servicios.Contains(item2);
                                if (alreadyExist)
                                {

                                }
                                else
                                {
                                    Servicios.Add(item2);
                                }
                            }

                        }

                    }
                }

                
                ViewBag.ServicioList = Servicios.Select(x => new SelectListItem { Value = x.IdServicio.ToString(), Text = x.DesServicio }).ToList(); ;
                objexcel.objObjeto.Clear();
                objRespuesta.objObjeto.Clear();

                if (idServicio > 0)
                {
                    objexcel = refSolicitudIndicadorBL.ListarDescargaSolicitudWeb(objOperador);
                    objRespuesta = refSolicitudIndicadorBL.ListarSolicitudesParaOperadoresWeb(objOperador);
                    objRespuesta.objObjeto = objRespuesta.objObjeto.Where(b => b.Direccion.IdDireccion == 1 && b.IdServicio == idServicio && (b.FormularioWeb == 1 || b.FormularioWeb == 3)).ToList();
                }

                gBitacora(ActionsBinnacle.Consultar, null, null);

                string user;
                user = User.Identity.GetUserId();
                try
                {
                    func._index(user, "Registro Indicadores", "Registro Indicadores Procesos");
                }

                catch (Exception e)
                {
                    Console.WriteLine("{0} Exception caught.", e);
                }

                return PartialView(new Tuple<List<SolicitudIndicador>, Operador, List<SolicitudConstructor>>(objRespuesta.objObjeto, objOperador, objexcel.objObjeto));
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView(new Tuple<List<SolicitudIndicador>>(new List<SolicitudIndicador>()));
            }
            catch (Exception ex)
            {
                var newEx = AppContext.ExceptionBuilder.BuildException("Ha ocurrido un error al guardar", ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView(new Tuple<List<SolicitudIndicador>>(new List<SolicitudIndicador>()));
            }
        }

        [AuthorizeUserAttribute]
        public ActionResult _tableCalidad(Operador objOperador, int idServicioCalidad = 0)
        {
            try
            {
                //ViewBag.ServicioList = ServicioList();

                ViewBag.searchTerm = new SolicitudIndicador();
                Respuesta<List<SolicitudIndicador>> objRespuesta = new Respuesta<List<SolicitudIndicador>>();
                Respuesta<List<ArchivoExcel>> objexcel = new Respuesta<List<ArchivoExcel>>();
                objexcel = refSolicitudIndicadorBL.ListarDescargaSolicitud(objOperador);
                objRespuesta = refSolicitudIndicadorBL.ListarSolicitudesParaOperadores(objOperador);
                objRespuesta.objObjeto = objRespuesta.objObjeto.Where(b => b.Direccion.IdDireccion == 2 && (b.FormularioWeb == 0 || b.FormularioWeb == 3)).OrderBy(y => y.IdServicio).ToList();
                //ViewBag.ServicioList = ServicioList();

                List<Servicio> myCollection = servicioBL.ConsultarPorOperador(objOperador).objObjeto;
                List<Servicio> Servicios = new List<Servicio>();
                if (objRespuesta != null)
                {
                    foreach (var item in objRespuesta.objObjeto)
                    {
                        foreach (var item2 in myCollection)
                        {
                            if (item.IdServicio == item2.IdServicio)
                            {
                                bool alreadyExist = Servicios.Contains(item2);
                                if (alreadyExist)
                                {

                                }
                                else
                                {
                                    Servicios.Add(item2);
                                }

                            }

                        }

                    }
                }


                ViewBag.ServicioList = Servicios.Select(x => new SelectListItem { Value = x.IdServicio.ToString(), Text = x.DesServicio }).ToList(); ;
                objexcel.objObjeto.Clear();
                objRespuesta.objObjeto.Clear();

                if (idServicioCalidad > 0)
                {
                    objexcel = refSolicitudIndicadorBL.ListarDescargaSolicitud(objOperador);
                    objRespuesta = refSolicitudIndicadorBL.ListarSolicitudesParaOperadores(objOperador);
                    objRespuesta.objObjeto = objRespuesta.objObjeto.Where(b => b.Direccion.IdDireccion == 2 && b.IdServicio == idServicioCalidad && (b.FormularioWeb == 0 || b.FormularioWeb == 3)).ToList();
                }

                gBitacora(ActionsBinnacle.Consultar, null, null);

                return PartialView(new Tuple<List<SolicitudIndicador>, Operador, List<ArchivoExcel>>(objRespuesta.objObjeto, objOperador, objexcel.objObjeto));
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView(new Tuple<List<SolicitudIndicador>>(new List<SolicitudIndicador>()));
            }
            catch (Exception ex)
            {
                var newEx = AppContext.ExceptionBuilder.BuildException("Ha ocurrido un error al guardar", ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView(new Tuple<List<SolicitudIndicador>>(new List<SolicitudIndicador>()));
            }
        }

        [AuthorizeUserAttribute]
        public ActionResult _tableCalidadWeb(Operador objOperador, int idServicioCalidad = 0)
        {
            try
            {
                //ViewBag.ServicioList = ServicioList();

                ViewBag.searchTerm = new SolicitudIndicador();
                Respuesta<List<SolicitudIndicador>> objRespuesta = new Respuesta<List<SolicitudIndicador>>();
                Respuesta<List<SolicitudConstructor>> objexcel = new Respuesta<List<SolicitudConstructor>>();
                objexcel = refSolicitudIndicadorBL.ListarDescargaSolicitudWeb(objOperador);
                objRespuesta = refSolicitudIndicadorBL.ListarSolicitudesParaOperadoresWeb(objOperador);
                objRespuesta.objObjeto = objRespuesta.objObjeto.Where(b => b.Direccion.IdDireccion == 2 && (b.FormularioWeb == 1 || b.FormularioWeb == 3)).OrderBy(y => y.IdServicio).ToList();
                

                List<Servicio> myCollection = servicioBL.ConsultarPorOperador(objOperador).objObjeto;
                List<Servicio> Servicios = new List<Servicio>();
                if (objRespuesta != null)
                {
                    foreach (var item in objRespuesta.objObjeto)
                    {
                        foreach (var item2 in myCollection)
                        {
                            if (item.IdServicio == item2.IdServicio)
                            {
                                bool alreadyExist = Servicios.Contains(item2);
                                if (alreadyExist)
                                {

                                }
                                else
                                {
                                    Servicios.Add(item2);
                                }
                            }

                        }

                    }
                }


                ViewBag.ServicioList = Servicios.Select(x => new SelectListItem { Value = x.IdServicio.ToString(), Text = x.DesServicio }).ToList(); ;
                objexcel.objObjeto.Clear();
                objRespuesta.objObjeto.Clear();


                if (idServicioCalidad > 0)
                {
                    objexcel = refSolicitudIndicadorBL.ListarDescargaSolicitudWeb(objOperador);
                    objRespuesta = refSolicitudIndicadorBL.ListarSolicitudesParaOperadores(objOperador);
                    objRespuesta.objObjeto = objRespuesta.objObjeto.Where(b => b.Direccion.IdDireccion == 2 && b.IdServicio == idServicioCalidad && (b.FormularioWeb == 1 || b.FormularioWeb == 3)).ToList();
                }

                gBitacora(ActionsBinnacle.Consultar, null, null);

                return PartialView(new Tuple<List<SolicitudIndicador>, Operador, List<SolicitudConstructor>>(objRespuesta.objObjeto, objOperador, objexcel.objObjeto));
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView(new Tuple<List<SolicitudIndicador>>(new List<SolicitudIndicador>()));
            }
            catch (Exception ex)
            {
                var newEx = AppContext.ExceptionBuilder.BuildException("Ha ocurrido un error al guardar", ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView(new Tuple<List<SolicitudIndicador>>(new List<SolicitudIndicador>()));
            }
        }


        [HttpPost]
        [AuthorizeUserAttribute]
        public ActionResult _table(SolicitudIndicador solicitudIndicador)
        {
            ViewBag.searchTerm = new SolicitudIndicador();

            Respuesta<List<SolicitudIndicador>> objRespuesta = new Respuesta<List<SolicitudIndicador>>();
            //objRespuesta = refRolBL.ConsultarTodos();
            Operador objOperador = new Operador();
            gBitacora(ActionsBinnacle.Consultar, null, null);

            return PartialView(new Tuple<List<SolicitudIndicador>, Operador>(objRespuesta.objObjeto, objOperador));
        }

        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ErrorData()
        {
            return View();
        }

        //
        //Descarga de Excel
        [HttpGet]
        [AuthorizeUserAttribute]
        public ActionResult DescargarExcel(string IdSolicitud, string idOperador, string registroIndicadoresNombreExcel, string registroIndicadoresExtension)
        {
            Respuesta<Byte[]> respuesta = new Respuesta<Byte[]>();
            try
            {
                string nombreUsuario = "";
                //string IdOperador = "";
                string URLSaveFormatXLS = Server.MapPath("~/Recursos/ConvertToOldExcel/");

                if (((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault() != null)
                {
                    nombreUsuario = ((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault().Value;
                    BL.Seguridad.UsersBL refUsuario = new BL.Seguridad.UsersBL(AppContext);
                    //IdOperador = refUsuario.ConsultarPorExpresion(x => x.AccesoUsuario == nombreUsuario && x.Borrado == 0 && x.Activo == 1).objObjeto.IdOperador;
                }

                if (idOperador != null)
                {
                    var resultado = refRegistroIndicadorBL.actualizarEstadoDescargaExcels(IdSolicitud, idOperador);
                }

                if (registroIndicadoresExtension == "xls")
                {
                    registroIndicadoresExtension = "." + registroIndicadoresExtension;
                }
                else
                {
                    registroIndicadoresExtension = ".xlsm";
                }

                if (idOperador != "" && idOperador != null && IdSolicitud != "" && IdSolicitud != null)
                {
                    respuesta = refRegistroIndicadorBL.obtenerArchivoExcel(idOperador, IdSolicitud, nombreUsuario, registroIndicadoresNombreExcel, registroIndicadoresExtension);
                    //respuesta = refRegistroIndicadorBL.CrearIndicadoresExcel(IdOperador, IdSolicitud, nombreUsuario, registroIndicadoresNombreExcel, registroIndicadoresExtension, URLSaveFormatXLS);
                    respuesta.strMensaje = respuesta.strMensaje;
                    gBitacora(ActionsBinnacle.Imprimir, null, null);
                }
                else
                {
                    return RedirectToAction("Error", "RegistroIndicador");
                }

                if (respuesta.blnIndicadorTransaccion)
                {
                    if (respuesta.objObjeto != null)
                    {

                        Response.BinaryWrite(respuesta.objObjeto);
                        Response.ContentType = "application/vnd.ms-excel.sheet.macroEnabled.12";
                        Response.AddHeader("content-disposition", "attachment;  filename=" + registroIndicadoresNombreExcel + registroIndicadoresExtension);

                        return new EmptyResult();

                        //FileContentResult file = new FileContentResult(respuesta.objObjeto, "application/vnd.ms-excel.sheet.macroEnabled.12");                        
                        //file.FileDownloadName = registroIndicadoresNombreExcel + registroIndicadoresExtension                        
                    }
                    else
                    {
                        return RedirectToAction("ErrorData", "RegistroIndicador");
                    }
                }
                else
                {
                    return RedirectToAction("Error", "RegistroIndicador");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "RegistroIndicador");
            }
        }

        #region cargaExcel
        /// <summary>
        /// Vista para importar excel
        /// </summary>
        /// <returns></returns>
        public ActionResult Importar()
        {
            return View();
        }
        /// <summary>
        /// Importar excel
        /// </summary>
        /// <param name="file"></param>
        /// <param name="IDSolicitudIndicadorImportar"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeUserAttribute]
        public String ImportarExcel(HttpPostedFileBase file, Guid IDSolicitudIndicadorImportar)
        {
            //var resultado = refRegistroIndicadorBL.obtieneEstadoCargaExcel(idSolicitud, idOperador);

            Respuesta<List<RegistroIndicador>> respuesta = new Respuesta<List<RegistroIndicador>>();
            JSONResult<List<RegistroIndicador>> jsonRespuesta = new JSONResult<List<RegistroIndicador>>();

            string nombreUsuario = "";
            string correo = "";
            string path = string.Empty;
            Usuario usuario = new Usuario();
            string mensaje = string.Empty;
            string asunto = string.Empty;
            int direccion = 0;

            try
            {

                var fileName = Path.GetFileName(file.FileName);

                if (((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault() != null)
                {
                    var user = ((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").FirstOrDefault();
                    correo = user.Value;

                    nombreUsuario = ((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault().Value;
                    BL.Seguridad.UsersBL refUsuario = new BL.Seguridad.UsersBL(AppContext);

                    usuario = refUsuario.ConsultarPorExpresion(x => x.AccesoUsuario == nombreUsuario && x.Borrado == 0 && x.Activo == 1).objObjeto;
                }

                //////
                //// Rutina Para guardar las plantilas de forma Fisica en el Directorio que este configurado
                /// En el archivo de configuracion Web.config en la seccion appSettings
                //////
                if (fileName.ToUpper().Contains("XLSM"))
                {
                    path = WebConfigurationManager.AppSettings["rutaCarpetaPlantillas"];//Path.Combine(Server.MapPath("~/App_Data/"), Guid.NewGuid() + fileName);
                    path += "\\" + IDSolicitudIndicadorImportar + "_" + usuario.IdOperador + "_" + fileName;

                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                        file.SaveAs(path);
                    }
                    else
                    {
                        file.SaveAs(path);
                    }
                }

                respuesta = refRegistroIndicadorInternoBL.gRegistroIndicadorInterno(IDSolicitudIndicadorImportar, usuario.IdOperador, usuario.IdUsuario, fileName, file.InputStream, path, ref direccion);

                if (respuesta.blnIndicadorTransaccion)
                {
                    //Inserción Bitácora
                    gBitacoraRegistroIndicador(ActionsBinnacle.Crear, respuesta.objObjeto, null);


                    ObtenerMensajeNotificarOperador(ref mensaje, ref asunto, IDSolicitudIndicadorImportar);

                    var result = (solicitudBL.gConfirmacion(correo, mensaje, asunto, direccion));

                    jsonRespuesta.data = respuesta.objObjeto;
                    jsonRespuesta.ok = true;
                    foreach (var item in jsonRespuesta.data) { item.SolicitudConstructor = null; }
                    return jsonRespuesta.toJSON();
                }
                else
                {
                    jsonRespuesta.ok = false;
                    jsonRespuesta.strMensaje = respuesta.strMensaje;
                    jsonRespuesta.data = new List<RegistroIndicador>();
                    return jsonRespuesta.toJSON();

                }

            }
            catch (DataAccessException de)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = de.Message;
                jsonRespuesta.data = new List<RegistroIndicador>();
                return jsonRespuesta.toJSON();
            }
            catch (BusinessException be)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = be.Message;
                jsonRespuesta.data = new List<RegistroIndicador>();
                return jsonRespuesta.toJSON();
            }
            catch (CustomException ce)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = ce.Message;
                jsonRespuesta.data = new List<RegistroIndicador>();
                return jsonRespuesta.toJSON();
            }

            catch (Exception ex)
            {

                string msj = GB.SUTEL.Shared.ErrorTemplate.Formatoexcel;
                AppContext.ExceptionBuilder.BuildException(msj, ex);
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = msj;
                jsonRespuesta.data = new List<RegistroIndicador>();
                return jsonRespuesta.toJSON();

            }


        }


        private void ObtenerMensajeNotificarOperador(ref string poHtml, ref string poAsunto, Guid IDSolicitudIndicadorImportar)
        {
            SolicitudIndicador objSolicitudIndicador = new SolicitudIndicador();
            objSolicitudIndicador = solicitudBL.gConsultarPorIdentificador(IDSolicitudIndicadorImportar).objObjeto;

            string host = Request.Url.Host;
            String tipoSitio = "https";

            MailingModel message = new MailingModel()
            {
                ImagenSutel = tipoSitio + "://" + host + "/Content/Images/logos/logo-Sutel_11_3.png",
                RutaSistema = tipoSitio + "://" + host + "/"
            };

            string renderedEmail = RenderView.RenderViewToString("Emails", "notificacionCargarArchivo", new Tuple<SolicitudIndicador, MailingModel>(objSolicitudIndicador, message));

            //poHtml = GetHTMLParsedAsString(Server.MapPath(ConfigurationManager.AppSettings["URLPlantillaNotificacion"].ToString()));
            poHtml = renderedEmail;
            poAsunto = ConfigurationManager.AppSettings["AsuntoNotificacion"].ToString();
        }


        #endregion

        #region Bitacora
        /// <summary>
        /// Bitacora de registro de indicador
        /// </summary>
        /// <param name="accion"></param>
        /// <param name="nuevosRegistros"></param>
        /// <param name="anterioresRegistros"></param>
        private void gBitacoraRegistroIndicador(ActionsBinnacle accion, List<RegistroIndicador> nuevosRegistros, List<RegistroIndicador> anterioresRegistros)
        {
            StringBuilder newData = new StringBuilder();
            StringBuilder oldData = new StringBuilder();
            String pantalla = "";
            String descripcion = "";
            pantalla = CBitacora.GetValueAsString(CBitacora.BitacoraOpciones.registroIndicadorInterno);
            if (nuevosRegistros != null)
            {
                foreach (RegistroIndicador item in nuevosRegistros)
                {
                    newData.Append("Registro de registro indicador con Id de Registro: ");
                    newData.Append(item.IdRegistroIndicador);
                    newData.Append("; ID Solicitud Constructor: ");
                    newData.Append(item.IdSolicitudConstructor);
                    newData.Append("; ID usuario: ");
                    newData.Append(item.IdUsuario);
                    newData.Append("; Fecha de registro: ");
                    newData.Append(item.FechaRegistroIndicador);
                    newData.Append("; Comentaro: ");
                    newData.Append(item.Comentario);
                    newData.Append("; Justificación: ");
                    newData.Append(item.Justificado);
                    newData.Append("; Bloqueado: ");
                    newData.Append(item.Bloqueado);
                    newData.Append("; Bandera de borrado de la agrupación: ");
                    newData.Append(item.Borrado);
                }

            }
            if (anterioresRegistros != null)
            {
                foreach (RegistroIndicador item in anterioresRegistros)
                {
                    oldData.Append("Registro de registro indicador con Id de Registro: ");
                    oldData.Append(item.IdRegistroIndicador);
                    oldData.Append("; ID Solicitud Constructor: ");
                    oldData.Append(item.IdSolicitudConstructor);
                    oldData.Append("; ID usuario: ");
                    oldData.Append(item.IdUsuario);
                    oldData.Append("; Fecha de registro: ");
                    oldData.Append(item.FechaRegistroIndicador);
                    oldData.Append("; Comentaro: ");
                    oldData.Append(item.Comentario);
                    oldData.Append("; Justificación: ");
                    oldData.Append(item.Justificado);
                    oldData.Append("; Bloqueado: ");
                    oldData.Append(item.Bloqueado);
                    oldData.Append("; Bandera de borrado de la agrupación: ");
                    oldData.Append(item.Borrado);
                }
            }
            if (accion.Equals(ActionsBinnacle.Crear))
            {
                descripcion = "Proceso de creación de " + pantalla;
            }
            else if (accion.Equals(ActionsBinnacle.Editar))
            {
                descripcion = "Proceso de edición de " + pantalla;
            }
            else if (accion.Equals(ActionsBinnacle.Borrar))
            {
                descripcion = "Proceso de eliminación de " + pantalla;
            }
            else
            {
                descripcion = "Proceso en " + pantalla;
            }


            try
            {

                CBitacora.gRegistrarBitacora(AppContext, Convert.ToInt32(accion.GetHashCode()), pantalla,
                                                 descripcion, newData.ToString(), oldData.ToString());
            }
            catch (Exception)
            {



            }
        }


        private void gBitacora(ActionsBinnacle accion, Rol newRol, Rol oldRol)
        {
            try
            {
                StringBuilder newData = new StringBuilder();
                StringBuilder oldData = new StringBuilder();
                String pantalla = "";
                String descripcion = "";
                pantalla = CBitacora.GetValueAsString(CBitacora.BitacoraOpciones.rol);
                if (newRol != null)
                {
                    newData.Append("Registro de un rol con Id de rol: ");
                    newData.Append(newRol.IdRol);
                    newData.Append(" Descripción del rol: ");
                    newData.Append(newRol.NombreRol);
                }
                if (oldRol != null)
                {
                    oldData.Append("Registro de un rol con Id de rol: ");
                    oldData.Append(oldRol.IdRol);
                    oldData.Append(" Descripción del rol: ");
                    oldData.Append(oldRol.NombreRol);
                }
                if (accion.Equals(ActionsBinnacle.Crear))
                {
                    descripcion = "Proceso de creación de " + pantalla;
                }
                else if (accion.Equals(ActionsBinnacle.Editar))
                {
                    descripcion = "Proceso de edición de " + pantalla;
                }
                else if (accion.Equals(ActionsBinnacle.Borrar))
                {
                    descripcion = "Proceso de eliminación de " + pantalla;
                }
                else if (accion.Equals(ActionsBinnacle.Imprimir))
                {
                    descripcion = "Descarga de Plantilla";
                    pantalla = "Registro Indicadores";
                    newData.Append("Descarga exitosa de plantilla");
                    oldData.Append("Proceso de descarga de plantilla");
                }
                else
                {
                    descripcion = "Proceso en " + pantalla;
                }

                CBitacora.gRegistrarBitacora(AppContext, Convert.ToInt32(accion.GetHashCode()), pantalla,
                                                 descripcion, newData.ToString(), oldData.ToString());
            }
            catch (Exception)
            { }
        }

        #endregion



        //
        //Descarga de Brochure
        [HttpGet]
        [AuthorizeUserAttribute]
        public ActionResult DescargarBrochure()
        {
            Respuesta<Byte[]> respuesta = new Respuesta<Byte[]>();
            try
            {

                var path = WebConfigurationManager.AppSettings["rutaCarpetaBrochure"];

                respuesta.objObjeto = refRegistroIndicadorInternoBL.getBrochure(path);

                if (respuesta.objObjeto != null)
                {
                    string nombreArchivo = refRegistroIndicadorInternoBL.getNombreArchivoBrochure(path);

                    Response.BinaryWrite(respuesta.objObjeto);
                    Response.ContentType = "application/vnd.ms-excel.sheet.macroEnabled.12";
                    Response.AddHeader("content-disposition", "attachment;  filename=" + nombreArchivo);

                    return new EmptyResult();
                }
                else
                {
                    return RedirectToAction("ErrorDescargaBrochure", "RegistroIndicador");
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorDescargaBrochure", "RegistroIndicador");
            }
        }


        [HttpGet]
        public ActionResult ErrorDescargaBrochure()
        {
            return View();
        }

    }
}
