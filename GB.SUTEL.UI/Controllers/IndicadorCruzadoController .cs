using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

using GB.SUTEL.Entities;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.BL.Mantenimientos;
using GB.SUTEL.UI.Helpers;
using GB.SUTEL.UI.Recursos.Utilidades;
using GB.SUTEL.Entities.Utilidades;
using System.Net;
using GB.SUTEL.BL.FuenteExternas;
using Microsoft.AspNet.Identity;

namespace GB.SUTEL.UI.Controllers
{
    [AuthorizeUserAttribute]
    public class IndicadorCruzadoController : BaseController
    {
        private IndicadorCruzadoBL refIndicadorCruzadoBL;
        private BitacoraWriter bitacora;
        Funcion func = new Funcion();
        public IndicadorCruzadoController()
        {
            
            refIndicadorCruzadoBL = new IndicadorCruzadoBL(AppContext);
            bitacora = new BitacoraWriter(AppContext);
        }
        //
        // GET: /User/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _table()
        {
            try
            {
                Respuesta<List<IndicadorCruzado>> objRespuesta = new Respuesta<List<IndicadorCruzado>>();
                objRespuesta = refIndicadorCruzadoBL.ConsultarTodos();
                ViewBag.searchTerm = new IndicadorCruzado();
                string user;
                user = User.Identity.GetUserId();
                try
                {
                    func._index(user, "Indicador Cruzado", "Indicador Cruzado Mantenimiento");
                }
         
                catch (Exception e)
                {
                    Console.WriteLine("{0} Exception caught.", e);
                }
                return PartialView(objRespuesta.objObjeto);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = String.Format(cEx.Message, cEx.Id);
                return View();
            }
            catch (Exception ex)
            {
                var newEx = AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                ViewBag.Error = String.Format(((CustomException)newEx).Message,((CustomException)newEx).Id);
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _table(IndicadorCruzado IndicadorCruzado)
        {
            try
            {
                Respuesta<List<IndicadorCruzado>> objRespuesta = new Respuesta<List<IndicadorCruzado>>();
                objRespuesta = refIndicadorCruzadoBL.gFiltrarIndicadorCruzado(IndicadorCruzado.Nombre==null?"":IndicadorCruzado.Nombre,IndicadorCruzado.Descripcion==null?"":IndicadorCruzado.Descripcion);
                ViewBag.searchTerm = IndicadorCruzado;
               
                return PartialView(objRespuesta.objObjeto);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = String.Format(cEx.Message, cEx.Id);
                return View();
            }
            catch (Exception ex)
            {
                var newEx = AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                ViewBag.Error = String.Format(((CustomException)newEx).Message, ((CustomException)newEx).Id);
                return View();
            }

        }
        public ActionResult Crear()
        {
            try
            {
                FuenteExternaBL objFuenteExternaBL = new FuenteExternaBL(AppContext);
                IndicadorBL objIndicadorBL = new IndicadorBL(AppContext);
                IndicadorExternoBL objIndicadorExternoBL = new IndicadorExternoBL(AppContext);
                ViewBag.FuenteExterna = objFuenteExternaBL.ConsultarTodos().objObjeto;
                ViewBag.Direccion = refIndicadorCruzadoBL.ConsultarTodosDirecciones().objObjeto;
                ViewBag.IndicadorInterno = objIndicadorBL.ConsultarTodos().objObjeto;
                ViewBag.IndicadorExterno = objIndicadorExternoBL.ConsultarTodos().objObjeto;
                return View();
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = String.Format(cEx.Message, cEx.Id);
                return View();
            }
            catch (Exception ex)
            {
                var newEx = AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                ViewBag.Error = String.Format(((CustomException)newEx).Message, ((CustomException)newEx).Id);
                return View();
            }
        }
        //
        // POST: /User/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear([Bind(Include="IdIndicadorCruzado,Nombre,Descripcion")]IndicadorCruzado IndicadorCruzado, string currentId,
            string[] myIndicadorInternoList,
            string[] IndicadorInternoList, string[] IndicadorExternoList)
        {
            try            
            {
                if (myIndicadorInternoList.Length == 0)
                    return new HttpStatusCodeResult(HttpStatusCode.Conflict,GB.SUTEL.Shared.ErrorTemplate.IndicadoreCruzados3);
                Respuesta<IndicadorCruzado> respuesta = new Respuesta<IndicadorCruzado>();
                JSONResult<IndicadorCruzado> jsonRespuesta = new JSONResult<IndicadorCruzado>();
                respuesta = refIndicadorCruzadoBL.Agregar(IndicadorCruzado, myIndicadorInternoList, IndicadorInternoList, IndicadorExternoList);
                if (respuesta.blnIndicadorState == 200)
                    bitacora.gRegistrarBitacora<IndicadorCruzado>(HttpContext, 2, respuesta.objObjeto, null, "mensaje");   
                else
                    return new HttpStatusCodeResult(HttpStatusCode.Conflict, respuesta.strMensaje);
                return Content("{ \"msg\":\""+respuesta.strMensaje+"\"}");
            }
            catch (Exception ex)
            {
                if (!(ex is CustomException))
                    AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, String.Format(((CustomException)ex).Message, ((CustomException)ex).Id));
            }
        }
        //
        // GET: IndicadorCruzado/Editar/5
        public ActionResult Editar(string id)
        {
            try
            {
                if (id == null)
                    return HttpNotFound();
                IndicadorCruzado objIndicadorCruzado = refIndicadorCruzadoBL.ConsultarPorExpresion(x => x.IdIndicadorCruzado == id).objObjeto;
                if (objIndicadorCruzado == null)
                    return HttpNotFound();
                FuenteExternaBL objFuenteExternaBL = new FuenteExternaBL(AppContext);
                IndicadorBL objIndicadorBL = new IndicadorBL(AppContext);
                IndicadorExternoBL objIndicadorExternoBL = new IndicadorExternoBL(AppContext);
                /*todas*/
                var direcciones = refIndicadorCruzadoBL.ConsultarTodosDirecciones().objObjeto;
                //Indicadores internos asociados (lado izquierdo)
                var myIndicadores = objIndicadorCruzado.DetalleIndicadorCruzado.Select(x => x.IdIndicador).Distinct().ToArray();
                var myIndicadoresJson = JsonConvert.SerializeObject(myIndicadores);
                //Indicadores internos asociados (lado derecho)
                var IndicadoresInternos = objIndicadorCruzado.DetalleIndicadorCruzado.Where(p => p.IdIndicadorExterno == null).Select(x => x.IdIndicadorInterno).Distinct().ToArray();
                var IndicadoresInternosJson = JsonConvert.SerializeObject(IndicadoresInternos);
                ViewBag.FuenteExterna = objFuenteExternaBL.ConsultarTodos().objObjeto;
                ViewBag.Direccion = direcciones;
                ViewBag.IndicadorInterno = objIndicadorBL.ConsultarTodos().objObjeto;
                ViewBag.IndicadorExterno = objIndicadorExternoBL.ConsultarTodos().objObjeto;
                
                
                ViewBag.Indicadores = myIndicadoresJson;
                ViewBag.IndicadoresInternos = IndicadoresInternosJson;
                ViewBag.IndicadoresExternos = JsonConvert.SerializeObject(objIndicadorCruzado.DetalleIndicadorCruzado.Where(p => p.IdIndicadorInterno == null).Select(x => x.IdIndicadorExterno).Distinct().ToList());

                var indicador = objIndicadorCruzado.DetalleIndicadorCruzado;
                var indicadorInterno = objIndicadorCruzado.DetalleIndicadorCruzado.Where(x => x.IdIndicadorExterno == null);
                var indicadorExterno = objIndicadorCruzado.DetalleIndicadorCruzado.Where(x => x.IdIndicadorInterno == null).FirstOrDefault();
                                
                if (indicador.Count()>0)
                {
                    var maxim = indicador.Where(a => myIndicadores.Contains(a.IdIndicador));
                    var maxi = maxim.Select(b => b.Indicador).Distinct().
                        SelectMany(c => c.Direccion).
                        GroupBy(b => b.IdDireccion);
                    var max = maxi.Max(x => x.Count());
                    var uniqueDir1 = indicador.Where(a => myIndicadores.Contains(a.IdIndicador)).Select(b => b.Indicador).
                        Distinct().SelectMany(c => c.Direccion).
                        GroupBy(b => b.IdDireccion).Where(x => x.Count() == max).FirstOrDefault().FirstOrDefault();
                    ViewBag.IdDireccion = uniqueDir1.IdDireccion;
                }
                else ViewBag.IdDireccion = default(int);

                if (indicadorInterno.Count() > 0)
                {
                    var maxim = indicadorInterno.Where(a => IndicadoresInternos.Contains(a.IdIndicadorInterno));
                    var maxi = maxim.Select(b => b.Indicador1).Distinct().
                        SelectMany(c => c.Direccion).
                        GroupBy(b => b.IdDireccion);
                    var max = maxi.Max(x => x.Count());
                    var uniqueDir1 = indicadorInterno.Where(a => IndicadoresInternos.Contains(a.IdIndicadorInterno)).Select(b => b.Indicador1).
                        Distinct().SelectMany(c => c.Direccion).
                        GroupBy(b => b.IdDireccion).Where(x => x.Count() == max).FirstOrDefault().FirstOrDefault();
                    ViewBag.IdDireccionInterno = uniqueDir1.IdDireccion;
                }
                else ViewBag.IdDireccionInterno = default(int);
                
                ViewBag.IdFuenteExterna = indicadorExterno != null ? indicadorExterno.IndicadorExterno.IdFuenteExterna : default(int);
                return View(objIndicadorCruzado);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = String.Format(cEx.Message, cEx.Id);
                return View();
            }
            catch (Exception ex)
            {
                var newEx = AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                ViewBag.Error = String.Format(GB.SUTEL.Shared.ErrorTemplate.InternalErrorUI,"Indicador Cruzado");
                return View();
            }
        }

        // POST: IndicadorCruzado/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Editar([Bind(Include = "IdIndicadorCruzado,Nombre,Descripcion")]IndicadorCruzado IndicadorCruzado, string currentId,
        public ActionResult Editar([Bind(Include = "Nombre,Descripcion")]IndicadorCruzado IndicadorCruzado, string currentId,
            string[] myIndicadorInternoList,
            string[] IndicadorInternoList, string[] IndicadorExternoList)
        {
            try
            {
                Respuesta<IndicadorCruzado[]> respuesta = new Respuesta<IndicadorCruzado[]>();
                JSONResult<IndicadorCruzado> jsonRespuesta = new JSONResult<IndicadorCruzado>();
                respuesta = refIndicadorCruzadoBL.Editar(IndicadorCruzado, currentId, myIndicadorInternoList, IndicadorInternoList, IndicadorExternoList);
                if (respuesta.blnIndicadorState == 200)
                {
                    jsonRespuesta.strMensaje = respuesta.strMensaje;
                    bitacora.gRegistrarBitacora<IndicadorCruzado>(HttpContext, 3, respuesta.objObjeto[0], respuesta.objObjeto[1], "mensaje");
                }
                else
                    return new HttpStatusCodeResult(HttpStatusCode.Conflict, respuesta.strMensaje);
                return Content("{ \"url\":\"" +this.HttpContext.Request.RequestContext.RouteData.GetRequiredString("controller") + "\", \"msg\":\""+respuesta.strMensaje+"\"}");
            }
            catch (Exception ex)
            {
                if (!(ex is CustomException))
                    AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, String.Format(((CustomException)ex).Message, ((CustomException)ex).Id));
            }
        }

        // POST: Pantalla/Delete/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(string id)
        {
            try
            {
                Respuesta<IndicadorCruzado> respuesta = new Respuesta<IndicadorCruzado>();
                JSONResult<IndicadorCruzado> jsonRespuesta = new JSONResult<IndicadorCruzado>();
                respuesta = refIndicadorCruzadoBL.Eliminar(id);
                jsonRespuesta.strMensaje = respuesta.strMensaje;
                bitacora.gRegistrarBitacora<IndicadorCruzado>(HttpContext, 4, null, respuesta.objObjeto, "mensaje");
                return Content(jsonRespuesta.toJSON());
            }
            catch (Exception ex)
            {
                if (!(ex is CustomException))
                    AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, String.Format(((CustomException)ex).Message, ((CustomException)ex).Id));
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _tableIndicadores(string Nombre, int? searchid)
        {
            try
            {
                ViewBag.searchTerm = new string[2]{Nombre,searchid.ToString()};                
                return PartialView(refIndicadorCruzadoBL.gFiltrarIndicadorInterno(Nombre == null ? "" : Nombre, searchid ?? 0));
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = String.Format(cEx.Message, cEx.Id);
                return PartialView();
            }
            catch (Exception ex)
            {
                var newEx = AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                ViewBag.Error = String.Format(((CustomException)newEx).Message, ((CustomException)newEx).Id);
                return PartialView();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _tableIndicadoresInternos(string Nombre, int? searchid)
        {
            try
            {
                ViewBag.searchTerm = new string[2] { Nombre, searchid.ToString() };
                return PartialView(refIndicadorCruzadoBL.gFiltrarIndicadorInterno(Nombre == null ? "" : Nombre, searchid ?? 0));
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = String.Format(cEx.Message, cEx.Id);
                return PartialView();
            }
            catch (Exception ex)
            {
                var newEx = AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                ViewBag.Error = String.Format(((CustomException)newEx).Message, ((CustomException)newEx).Id);
                return PartialView();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _tableIndicadoresExternos(string Nombre, int? searchid)
        {
            try
            {
                ViewBag.searchTerm = new string[2] { Nombre, searchid.ToString() };
                IndicadorExternoBL refIndicadorExternoBL = new IndicadorExternoBL(AppContext);
                FuenteExternaBL refFuenteExternaBL = new FuenteExternaBL(AppContext);
                FuenteExterna FuenteExterna = refFuenteExternaBL.ConsultarPorExpresion(x => x.IdFuenteExterna == (searchid ?? 0)).objObjeto;
                return PartialView(refIndicadorExternoBL.gFiltrarIndicadorExterno(searchid == null ? "" : FuenteExterna.NombreFuenteExterna,
                    Nombre == null ? "" : Nombre,null,"").objObjeto);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = String.Format(cEx.Message, cEx.Id);
                return PartialView();
            }
            catch (Exception ex)
            {
                var newEx = AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                ViewBag.Error = String.Format(((CustomException)newEx).Message, ((CustomException)newEx).Id);
                return PartialView();
            }
        }
    }
}