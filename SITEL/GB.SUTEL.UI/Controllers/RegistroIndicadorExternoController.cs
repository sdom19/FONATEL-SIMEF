using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;

using GB.SUTEL.Entities;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.BL.FuenteExternas;
using GB.SUTEL.BL.Mantenimientos;
using GB.SUTEL.UI.Helpers;
using GB.SUTEL.UI.Recursos.Utilidades;
using GB.SUTEL.Entities.Utilidades;
using System.Net;

using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Drawing.Chart;
using System.Drawing;
using System.Security.Claims;
using System.IO;
using System.Globalization;
using NPOI.HSSF.UserModel;
using System.Data;
using DocumentFormat.OpenXml.Spreadsheet;
using NPOI.SS.UserModel;
using GB.SUTEL.BL.FuenteExternas;
using Microsoft.AspNet.Identity;

namespace GB.SUTEL.UI.Controllers
{
    [AuthorizeUserAttribute]
    public class RegistroIndicadorExternoController : BaseController
    {
        RegistroIndicadorExternoBL refRegistroIndicadorExternoBL;
        BitacoraWriter bitacora;
        Funcion func = new Funcion();
        public RegistroIndicadorExternoController()
        {
            refRegistroIndicadorExternoBL = new RegistroIndicadorExternoBL(AppContext);
            bitacora = new BitacoraWriter(AppContext);            
        }
        //
        // GET: /User/
        public ActionResult Index()
        {

            string user;
            user = User.Identity.GetUserId();
            try
            {
                func._index(user, "Registro Indicador Externo", "Registro Indicador Externo Procesos");
            }
           
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }
            return View();
        }
        public ActionResult _table()
        {
            try
            {
                Respuesta<List<RegistroIndicadorExterno>> objRespuesta = new Respuesta<List<RegistroIndicadorExterno>>();
                objRespuesta = refRegistroIndicadorExternoBL.ConsultarTodos();
                objRespuesta.objObjeto.Clear();
                ViewBag.searchTerm = new string[6];
                return PartialView(objRespuesta.objObjeto);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = String.Format(cEx.Message, cEx.Id);
                return PartialView();
            }
            catch (Exception ex)
            {
                var newEx = AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                ViewBag.Error = String.Format(((CustomException)newEx).Message,((CustomException)newEx).Id);
                return PartialView();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _table(string fuente, string indicador, string valor,
            string anno, string zona, string region)
        {
            try
            {//valor==null||valor==""? 0: int.Parse(valor)
                valor = valor.Replace(",", CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator);
                valor = valor.Replace(".", CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator);
                double intValor = 0;
                double.TryParse(valor, out intValor);
                Respuesta<List<RegistroIndicadorExterno>> objRespuesta = new Respuesta<List<RegistroIndicadorExterno>>();
                objRespuesta = refRegistroIndicadorExternoBL.gFiltrarRegistroIndicadorExterno(fuente==null?"":fuente,
                    indicador == null ? "" : indicador, intValor, anno == null ? "" : anno, zona == null ? "" : zona, region == null ? "" : region);
                valor = valor.Replace(",",".");
                ViewBag.searchTerm = new string[6] { fuente, indicador, valor.ToString(), anno, zona, region };
                return PartialView(objRespuesta.objObjeto);
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
        public ActionResult Crear()
        {
            try
            {
                FuenteExternaBL objFuenteExternaBL = new FuenteExternaBL(AppContext);
                PeriodicidadBL objPeriodicidadBL = new PeriodicidadBL(AppContext);
                ZonaIndicadorExternoBL objZonaBL = new ZonaIndicadorExternoBL(AppContext);
                RegionIndicadorExternoBL objRegionBL = new RegionIndicadorExternoBL(AppContext);

                List<FuenteExterna> FuenteExterna = objFuenteExternaBL.ConsultarTodos().objObjeto;
                List<IndicadorExterno> IndicadorExterno = FuenteExterna.SelectMany(x => x.IndicadorExterno).Where(z => z.Borrado == 0).Distinct().ToList();
                //FuenteExterna = IndicadorExterno.Select(x => x.FuenteExterna).Distinct().ToList();
                List<Periodicidad> Periodicidades = objPeriodicidadBL.ConsultarTodos().objObjeto;
                List<ZonaIndicadorExterno> Zona = objZonaBL.ConsultarTodos().objObjeto;
                List<RegionIndicadorExterno> Region = objRegionBL.ConsultarTodos().objObjeto;
                List<int> Anio = new List<int>();
                for (int i = DateTime.Today.Year; i > (DateTime.Today.Year - 100); i--)
                {
                    Anio.Add(i);
                }
                List<Trimestre> Trimestre = refRegistroIndicadorExternoBL.ConsultarTrimestres().objObjeto;
                List<Canton> Canton = refRegistroIndicadorExternoBL.ConsultarCantones().objObjeto.OrderBy(x=>x.IdCanton).ToList();
                List<Genero> Genero = refRegistroIndicadorExternoBL.ConsultarGeneros().objObjeto;

                ViewBag.FuenteExterna = FuenteExterna;
                ViewBag.IndicadorExterno = IndicadorExterno;
                ViewBag.Periodicidades = Periodicidades;
                ViewBag.Zona = Zona;
                ViewBag.Region = Region;
                ViewBag.Anio = Anio;
                ViewBag.Trimestre = Trimestre;
                ViewBag.Canton = Canton;
                ViewBag.Genero = Genero;
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
        public ActionResult Crear([Bind(Include = @"IdPeridiocidad,IdRegionIndicadorExterno,IdZonaIndicadorExterno,IdIndicadorExterno,
            IdGenero,ValorIndicador,Anno,IdCanton,IDTRIMESTRE")]RegistroIndicadorExterno RegistroIndicadorExterno, string ValorIndicador)
        {
            try
            {
                Respuesta<RegistroIndicadorExterno> respuesta = new Respuesta<RegistroIndicadorExterno>();
                JSONResult<RegistroIndicadorExterno> jsonRespuesta = new JSONResult<RegistroIndicadorExterno>();
                respuesta = refRegistroIndicadorExternoBL.Agregar(RegistroIndicadorExterno, ValorIndicador);
                if (respuesta.blnIndicadorState == 200)
                {
                    jsonRespuesta.strMensaje = respuesta.strMensaje;
                    bitacora.gRegistrar<RegistroIndicadorExterno>(HttpContext, 2, respuesta.objObjeto, null,"mensaje");
                }
                else
                    return new HttpStatusCodeResult(HttpStatusCode.HttpVersionNotSupported, respuesta.strMensaje);
                return Content(jsonRespuesta.toJSON());
            }
            catch (Exception ex)
            {
                if (!(ex is CustomException))
                    AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, String.Format(((CustomException)ex).Message, ((CustomException)ex).Id));
            }
        }
        //
        // GET: RegistroIndicadorExterno/Editar/5
        public ActionResult Editar(Guid? id)
        {
            try
            {
                if (id == null)
                    return HttpNotFound();
                RegistroIndicadorExterno objRegistroIndicadorExterno = refRegistroIndicadorExternoBL.ConsultarPorExpresion(x => x.IdRegistroIndicadorExterno == id).objObjeto;
                if (objRegistroIndicadorExterno == null)
                    return HttpNotFound();
                FuenteExternaBL objFuenteExternaBL = new FuenteExternaBL(AppContext);
                PeriodicidadBL objPeriodicidadBL = new PeriodicidadBL(AppContext);
                ZonaIndicadorExternoBL objZonaBL = new ZonaIndicadorExternoBL(AppContext);
                RegionIndicadorExternoBL objRegionBL = new RegionIndicadorExternoBL(AppContext);

                List<FuenteExterna> FuenteExterna = objFuenteExternaBL.ConsultarTodos(objRegistroIndicadorExterno).objObjeto;
                List<IndicadorExterno> IndicadorExterno = FuenteExterna.SelectMany(x => x.IndicadorExterno).Where(z => (z.Borrado==0
                    || objRegistroIndicadorExterno.IdIndicadorExterno==z.IdIndicadorExterno)).Distinct().ToList();
                //FuenteExterna = IndicadorExterno.Select(x => x.FuenteExterna).Distinct().ToList();
                List<Periodicidad> Periodicidades = objPeriodicidadBL.ConsultarTodos(objRegistroIndicadorExterno).objObjeto;
                List<ZonaIndicadorExterno> Zona = objZonaBL.ConsultarTodos(objRegistroIndicadorExterno).objObjeto;
                List<RegionIndicadorExterno> Region = objRegionBL.ConsultarTodos(objRegistroIndicadorExterno).objObjeto;
                List<int> Anio = new List<int>();
                for (int i = DateTime.Today.Year; i > (DateTime.Today.Year - 100); i--)
                {
                    Anio.Add(i);
                }
                List<Trimestre> Trimestre = refRegistroIndicadorExternoBL.ConsultarTrimestres().objObjeto;
                List<Canton> Canton = refRegistroIndicadorExternoBL.ConsultarCantones().objObjeto.OrderBy(x => x.IdCanton).ToList();
                List<Genero> Genero = refRegistroIndicadorExternoBL.ConsultarGeneros().objObjeto;

                ViewBag.FuenteExterna = FuenteExterna;
                ViewBag.IndicadorExterno = IndicadorExterno;
                ViewBag.Periodicidades = Periodicidades;
                ViewBag.Zona = Zona;
                ViewBag.Region = Region;
                ViewBag.Anio = Anio;
                ViewBag.Trimestre = Trimestre;
                ViewBag.Canton = Canton;
                ViewBag.Genero = Genero;
                return View(objRegistroIndicadorExterno);
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

        // POST: RegistroIndicadorExterno/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = @"IdRegistroIndicadorExterno,IdPeridiocidad,IdRegionIndicadorExterno,IdZonaIndicadorExterno,IdIndicadorExterno,
            IdGenero,ValorIndicador,Anno,IdCanton,IDTRIMESTRE")] RegistroIndicadorExterno objRegistroIndicadorExterno, string ValorIndicador)
        {
            Respuesta<RegistroIndicadorExterno[]> respuesta = new Respuesta<RegistroIndicadorExterno[]>();
            JSONResult<RegistroIndicadorExterno> jsonRespuesta = new JSONResult<RegistroIndicadorExterno>();
            try
            {
                respuesta = refRegistroIndicadorExternoBL.Editar(objRegistroIndicadorExterno, ValorIndicador);
                if (respuesta.blnIndicadorState != 200)
                    return new HttpStatusCodeResult(HttpStatusCode.HttpVersionNotSupported, respuesta.strMensaje);
                else
                {
                    jsonRespuesta.strMensaje = "{ \"url\":\"" +
                        this.HttpContext.Request.RequestContext.RouteData.GetRequiredString("controller") + "\", \"msg\":\""+respuesta.strMensaje+"\"}";
                    bitacora.gRegistrar<RegistroIndicadorExterno>(HttpContext, 3, respuesta.objObjeto[0], respuesta.objObjeto[1], "mensaje");
                }
            }
            catch (Exception ex)
            {   
                if (!(ex is CustomException))
                    AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, String.Format(((CustomException)ex).Message, ((CustomException)ex).Id));
            }
            return Content(jsonRespuesta.toJSON());            
        }

        // POST: Pantalla/Delete/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(Guid id)
        {
            try
            {
                Respuesta<RegistroIndicadorExterno> respuesta = new Respuesta<RegistroIndicadorExterno>();
                JSONResult<RegistroIndicadorExterno> jsonRespuesta = new JSONResult<RegistroIndicadorExterno>();
                respuesta = refRegistroIndicadorExternoBL.Eliminar(id);
                jsonRespuesta.strMensaje = respuesta.strMensaje;
                bitacora.gRegistrar<RegistroIndicadorExterno>(HttpContext, 4, null, respuesta.objObjeto, "mensaje");
                return Content(jsonRespuesta.toJSON());
            }
            catch (Exception ex)
            {
                if (!(ex is CustomException))
                    AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, String.Format(((CustomException)ex).Message, ((CustomException)ex).Id));
            }
        }

        public ActionResult GenerarPlantilla()
        {
            try
            {
                FuenteExternaBL objFuenteExternaBL = new FuenteExternaBL(AppContext);
                PeriodicidadBL objPeriodicidadBL = new PeriodicidadBL(AppContext);
                ZonaIndicadorExternoBL objZonaBL = new ZonaIndicadorExternoBL(AppContext);
                RegionIndicadorExternoBL objRegionBL = new RegionIndicadorExternoBL(AppContext);

                List<FuenteExterna> FuenteExterna = objFuenteExternaBL.ConsultarTodos().objObjeto;
                List<IndicadorExterno> IndicadorExterno = FuenteExterna.SelectMany(x => x.IndicadorExterno).Where(z => z.Borrado == 0).Distinct().ToList();

                ViewBag.FuenteExterna = FuenteExterna;
                ViewBag.IndicadorExterno = IndicadorExterno;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _tableIndicadoresExternos(string Nombre, int? searchid)
        {
            try
            {
                string[] searchTerm = new string[2] { Nombre, searchid == null ? null: searchid.ToString() };
                ViewBag.searchTerm = searchTerm;
                IndicadorExternoBL refIndicadorExternoBL = new IndicadorExternoBL(AppContext);
                return PartialView(refIndicadorExternoBL.gFiltrarIndicadorExterno("",Nombre == null ? "":Nombre, searchid,"").objObjeto);
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

        #region download
        [HttpGet]        
        public ActionResult Download(string INDICADORES)
        {            
            try
            {
                var startRow = 8;

                #region listas logicas
                FuenteExternaBL objFuenteExternaBL = new FuenteExternaBL(AppContext);
                PeriodicidadBL objPeriodicidadBL = new PeriodicidadBL(AppContext);
                ZonaIndicadorExternoBL objZonaBL = new ZonaIndicadorExternoBL(AppContext);
                RegionIndicadorExternoBL objRegionBL = new RegionIndicadorExternoBL(AppContext);

                List<FuenteExterna> FuentesExternas = objFuenteExternaBL.ConsultarTodos().objObjeto;
                List<IndicadorExterno> IndicadorExterno = FuentesExternas.SelectMany(x => x.IndicadorExterno).Where(z => z.Borrado == 0).Distinct().ToList();
                List<string> Indicadores = INDICADORES.Split(',').ToList();
                IndicadorExterno = IndicadorExterno.Where(x => Indicadores.Contains(x.IdIndicadorExterno.ToString())).ToList();

                List<Periodicidad> Periodicidades = objPeriodicidadBL.ConsultarTodos().objObjeto.Where(x => x.DescPeriodicidad == "Anual").ToList();
                List<ZonaIndicadorExterno> Zona = objZonaBL.ConsultarTodos().objObjeto;
                List<RegionIndicadorExterno> Region = objRegionBL.ConsultarTodos().objObjeto;
                List<int> Anio = new List<int>();
                for (int i = DateTime.Today.Year; i > (DateTime.Today.Year-100); i--)
                {
                    Anio.Add(i);
                }
                List<Trimestre> Trimestre = refRegistroIndicadorExternoBL.ConsultarTrimestres().objObjeto;
                List<Canton> Canton = refRegistroIndicadorExternoBL.ConsultarCantones().objObjeto.OrderBy(x => x.IdCanton).ToList();
                List<Genero> Genero = refRegistroIndicadorExternoBL.ConsultarGeneros().objObjeto;
                FuenteExterna FuenteExterna;
                if (IndicadorExterno.Count() > 0)
                    FuenteExterna = IndicadorExterno.FirstOrDefault().FuenteExterna;
                else FuenteExterna = null;

                #endregion
                
                #region creacion del excel
                var package = new ExcelPackage();
                package.Workbook.Protection.LockRevision = true;
                package.Workbook.Protection.LockStructure = true;

                package.Workbook.Worksheets.Add("Hoja1");
                package.Workbook.Worksheets.Add("Inmodificable");
                ExcelWorksheet ws = package.Workbook.Worksheets[1];
                ExcelWorksheet ws2 = package.Workbook.Worksheets[2];
                ws.Name = "Hoja1"; 
                ws2.Name = "Inmodificable"; 
                ws2.Hidden = eWorkSheetHidden.Hidden;
                
                System.Drawing.Color headColorFromHex = System.Drawing.ColorTranslator.FromHtml("#2f75b5");
                System.Drawing.Color fontColorFromHex = System.Drawing.ColorTranslator.FromHtml("#fff");
                System.Drawing.Color grayColorFromHex = System.Drawing.ColorTranslator.FromHtml("#e7e6e6");
                System.Drawing.Color greenColorFromHex = System.Drawing.ColorTranslator.FromHtml("#e2efda");
                /*Header*/
                ws.Cells["A1:N6"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells["A1:N4"].Style.Fill.BackgroundColor.SetColor(grayColorFromHex);
                ws.Cells["A5:N5"].Style.Fill.BackgroundColor.SetColor(headColorFromHex);
                ws.Cells["A6:N6"].Style.Fill.BackgroundColor.SetColor(greenColorFromHex);
                ws.Row(5).Height = 6;
                ws.Cells["I2"].Value = "Usuario:";
                ws.Cells["I2"].Style.Font.Bold = true;
                var user = ((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault();
                ws.Cells["J2"].Value =user==null?"":user.Value;
                Image logo = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Content\\Images\\logos\\logo-Sutel_11.png");
                logo = (Image)(new Bitmap(logo, new Size(313,90)));
                var picture = ws.Drawings.AddPicture("SUTEL", logo);
                picture.SetPosition(0,0,0,0);

                ws.Cells[7, 1, 7, 14].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[7, 1, 7, 14].Style.Fill.BackgroundColor.SetColor(headColorFromHex);
                ws.Cells[7, 1, 7, 10].Style.Font.Color.SetColor(fontColorFromHex);    
                ws.Cells[7, 1].Value = "Fuente";        ws.Cells[7, 6].Value = "Año";
                ws.Cells[7, 2].Value = "Indicador";     ws.Cells[7, 7].Value = "Trimestre";
                ws.Cells[7, 3].Value = "Periodicidad";  ws.Cells[7, 8].Value = "Cantón";
                ws.Cells[7, 4].Value = "Zona";          ws.Cells[7, 9].Value = "Género";
                ws.Cells[7, 5].Value = "Región";        ws.Cells[7, 10].Value = "Valor";
                ws.Cells[7, 1, 7, 10].AutoFitColumns();

                ws2.Cells[1, 1].Value = "Fuente";       ws2.Cells[1, 11].Value = "Año";
                ws2.Cells[1, 3].Value = "Indicador";    ws2.Cells[1, 13].Value = "Trimestre";
                ws2.Cells[1, 5].Value = "Periodicidad"; ws2.Cells[1, 15].Value = "Cantón";
                ws2.Cells[1, 7].Value = "Zona";         ws2.Cells[1, 17].Value = "Género";
                ws2.Cells[1, 9].Value = "Región"; 
                ws2.Cells[1, 1, 1, 18].AutoFitColumns();

                ws2.Cells[1, 1, 1, 2].Merge = true;
                ws2.Cells[1, 3, 1, 4].Merge = true;
                ws2.Cells[1, 5, 1, 6].Merge = true;
                ws2.Cells[1, 7, 1, 8].Merge = true;
                ws2.Cells[1, 9, 1, 10].Merge = true;
                ws2.Cells[1, 11, 1, 12].Merge = true;
                ws2.Cells[1, 13, 1, 14].Merge = true;
                ws2.Cells[1, 15, 1, 16].Merge = true;
                ws2.Cells[1, 17, 1, 18].Merge = true;
                #endregion

                #region llenado de listas ocultas
                var cellFuenteNombre = ws2.Cells[2, 1];
                var cellFuenteId = ws2.Cells[2, 2];
                if(FuenteExterna!=null){
                    cellFuenteNombre.Value = FuenteExterna.NombreFuenteExterna;
                    cellFuenteId.Value = FuenteExterna.IdFuenteExterna;
                }                
                var y = 2;
                foreach(var item in IndicadorExterno){
                    //var cell1 = ws2.Cells[y, 3];
                    //var cell2 = ws2.Cells[y, 4];
                    //cell1.Value = item.Nombre;
                    //cell2.Value = item.IdIndicadorExterno;
                    ws2.Cells[y, 3].Value = item.Nombre; 
                    ws2.Cells[y, 4].Value = item.IdIndicadorExterno;
                    y++;
                }
                y = 2;
                foreach (var item in Periodicidades)
                {
                    var cell1 = ws2.Cells[y, 5];
                    var cell2 = ws2.Cells[y, 6];
                    cell1.Value = item.DescPeriodicidad;
                    cell2.Value = item.IdPeridiocidad;
                    y++;
                }
                y = 2;
                foreach (var item in Zona)
                {
                    var cell1 = ws2.Cells[y, 7];
                    var cell2 = ws2.Cells[y, 8];
                    cell1.Value = item.DescZonaIndicadorExterno;
                    cell2.Value = item.IdZonaIndicadorExterno;
                    y++;
                }
                y = 2;
                foreach (var item in Region)
                {
                    var cell1 = ws2.Cells[y, 9];
                    var cell2 = ws2.Cells[y, 10];
                    cell1.Value = item.DescRegionIndicadorExterno;
                    cell2.Value = item.IdRegionIndicadorExterno;
                    y++;
                }
                y = 2;
                foreach (var item in Anio)
                {
                    var cell1 = ws2.Cells[y, 11];
                    cell1.Value = item;
                    y++;
                }
                y = 2;
                foreach (var item in Trimestre)
                {
                    var cell1 = ws2.Cells[y, 13];
                    var cell2 = ws2.Cells[y, 14];
                    cell1.Value = item.Nombre;
                    cell2.Value = item.IdTrimestre;
                    y++;
                }
                y = 2;
                foreach (var item in Canton)
                {
                    var cell1 = ws2.Cells[y, 15];
                    var cell2 = ws2.Cells[y, 16];
                    cell1.Value = item.Nombre;
                    cell2.Value = item.IdCanton;
                    y++;
                }
                y = 2;
                foreach (var item in Genero)
                {
                    var cell1 = ws2.Cells[y, 17];
                    var cell2 = ws2.Cells[y, 18];
                    cell1.Value = item.Descripcion;
                    cell2.Value = item.IdGenero;
                    y++;
                }
                #endregion

                #region aplicación de reglas para listas
                ws.Names.Add("FuenteExterna", ws2.Cells["A2"]);
                ws.Names.Add("IndicadorExterno", ws2.Cells["C2:C" + (IndicadorExterno.Count() == 0 ? 2 : (1) + IndicadorExterno.Count())]);
                ws.Names.Add("Periodicidades", ws2.Cells["E2:E" + (Periodicidades.Count() == 0 ? 2 : (1) + Periodicidades.Count())]);
                ws.Names.Add("Zona", ws2.Cells["G2:G" + (Zona.Count() == 0 ? 2 : (1) + Zona.Count())]);
                ws.Names.Add("Region", ws2.Cells["I2:I" + (Region.Count() == 0 ? 2 : (1) + Region.Count())]);
                ws.Names.Add("Anio", ws2.Cells["K2:K" + (Anio.Count() == 0 ? 2 : (1) + Anio.Count())]);
                ws.Names.Add("Trimestre", ws2.Cells["M2:M" + (Trimestre.Count() == 0 ? 2 : (1) + Trimestre.Count())]);
                ws.Names.Add("Canton", ws2.Cells["O2:O" + (Canton.Count() == 0 ? 2 : (1) + Canton.Count())]);
                ws.Names.Add("Genero", ws2.Cells["Q2:Q" + (Genero.Count() == 0 ? 2 : (1) + Genero.Count())]);
                //var validation = ws.DataValidations.AddListValidation("A2");
                //ws.DataValidations.AddListValidation("A2").Formula.ExcelFormula = "=FuenteExterna";
                ws.Cells[startRow, 1, 200, 1].DataValidation.AddListDataValidation().Formula.ExcelFormula = "=FuenteExterna";
                ws.Cells[startRow, 2, 200, 2].DataValidation.AddListDataValidation().Formula.ExcelFormula = "=IndicadorExterno";
                ws.Cells[startRow, 3, 200, 3].DataValidation.AddListDataValidation().Formula.ExcelFormula = "=Periodicidades";
                ws.Cells[startRow, 4, 200, 4].DataValidation.AddListDataValidation().Formula.ExcelFormula = "=Zona";
                ws.Cells[startRow, 5, 200, 5].DataValidation.AddListDataValidation().Formula.ExcelFormula = "=Region";
                ws.Cells[startRow, 6, 200, 6].DataValidation.AddListDataValidation().Formula.ExcelFormula = "=Anio";
                ws.Cells[startRow, 7, 200, 7].DataValidation.AddListDataValidation().Formula.ExcelFormula = "=Trimestre";
                ws.Cells[startRow, 8, 200, 8].DataValidation.AddListDataValidation().Formula.ExcelFormula = "=Canton";
                ws.Cells[startRow, 9, 200, 9].DataValidation.AddListDataValidation().Formula.ExcelFormula = "=Genero";
                #endregion

                ws2.Protection.SetPassword("Password123!");

                var memoryStream = package.GetAsByteArray();
                var fileName = string.Format("PlantillaIndicadorExterno{0:yyyyMMdd-HH-mm-ss}.xlsx", DateTime.Now);
                // mimetype from http://stackoverflow.com/questions/4212861/what-is-a-correct-mime-type-for-docx-pptx-etc
                return base.File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                if (!(ex is CustomException))
                    AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, String.Format(GB.SUTEL.Shared.ErrorTemplate.InternalError,"No se pude generar el archivo .xlsx"));
            }
        }
        public ActionResult CargaAutomatica()
        {
            return View();
        }
        #endregion
        [HttpPost]
        public ActionResult _preview(HttpPostedFileBase file)
        {
            try
            {
                List<string[]> objToReturn = new List<string[]>();
                string[] strArray;
                string archivo = file.FileName;
                if (archivo.ToUpper().Contains("XLSX"))
                {
                    var startRow = 8;
                    var package = new ExcelPackage(file.InputStream);
                    package.Workbook.Protection.LockRevision = true;
                    package.Workbook.Protection.LockStructure = true;

                    var ws = package.Workbook.Worksheets.FirstOrDefault();
                    var ws2 = package.Workbook.Worksheets[2];
                    if (ws != null)
                    { 
                        int lastRow = ws.Dimension.End.Row;
                        int lastRow2 = ws2.Dimension.End.Column;
                        int cantErrores = 0;
                        int cantErroresLenght = 0;
                        int valueIsInt = 0;
                        for (int i = startRow; i <= lastRow; i++)
                        {
                            if (ws.Cells[i, 1].Value != null && ws.Cells[i, 2].Value != null && ws.Cells[i, 3].Value != null &&
                                ws.Cells[i, 4].Value != null && ws.Cells[i, 5].Value != null && ws.Cells[i, 6].Value != null &&
                                ws.Cells[i, 7].Value != null && ws.Cells[i, 8].Value != null && ws.Cells[i, 9].Value != null &&
                                ws.Cells[i, 10].Value != null)
                            {
                                strArray = new string[10];
                                strArray[0] = ws.Cells[i, 1].Value.ToString();
                                strArray[1] = ws.Cells[i, 2].Value.ToString();
                                strArray[2] = ws.Cells[i, 3].Value.ToString();
                                strArray[3] = ws.Cells[i, 4].Value.ToString();
                                strArray[4] = ws.Cells[i, 5].Value.ToString();
                                strArray[5] = ws.Cells[i, 6].Value.ToString();
                                strArray[6] = ws.Cells[i, 7].Value.ToString();
                                strArray[7] = ws.Cells[i, 8].Value.ToString();
                                strArray[8] = ws.Cells[i, 9].Value.ToString();
                                strArray[9] = ws.Cells[i, 10].Value.ToString();

                                try
                                {
                                    float.Parse(ws.Cells[i, 10].Value.ToString());
                                    if (ws.Cells[i, 10].Value.ToString().Length > 11)
                                    {
                                        cantErroresLenght++;
                                    }

                                }catch(Exception ex){
                                    cantErroresLenght++;
                                }

                                
                                objToReturn.Add(strArray);
                            }

                            if (ws.Cells[i, 1].Value == null || ws.Cells[i, 2].Value == null || ws.Cells[i, 3].Value == null ||
                                ws.Cells[i, 4].Value == null || ws.Cells[i, 5].Value == null || ws.Cells[i, 6].Value == null ||
                                ws.Cells[i, 7].Value == null || ws.Cells[i, 8].Value == null || ws.Cells[i, 9].Value == null ||
                                ws.Cells[i, 10].Value == null)
                            {
                                cantErrores++;
                            }
                        }

                        if (cantErroresLenght > 0)
                        {
                            ViewBag.Error = "Se encontró " + cantErroresLenght + " dato(s) incorrecto(s) en la columna 'Valor' del archivo excel, la cantidad máxima de caracteres son 10​, los decimales separados por una coma. Por favor revisar el archivo e intente nuevamente.";
                            return PartialView();
                        }

                        if (cantErrores > 0)
                        {
                            ViewBag.Error = "Se encontró " + cantErrores + " espacio(s) en blanco en el archivo excel que debe completar. Por favor revisar el archivo e intente nuevamente.";
                            return PartialView();                            
                        }
                        else
                        {
                            return PartialView(objToReturn);
                        }
                     }
                    else
                    {
                        ViewBag.Error = "El formato del archivo no es válido.";
                        return PartialView();
                    }
                     
                 }
                 else 
                 {
                     int start = 7;
                     HSSFWorkbook hssfwb = new HSSFWorkbook(file.InputStream);
                     ISheet sheet = hssfwb.GetSheet("Hoja1");
                     if (sheet != null)
                     {
                         int last = sheet.LastRowNum;
                         for (int i = start; i <= last; i++)
                         {
                             if (sheet.GetRow(i).GetCell(0) != null && sheet.GetRow(i).GetCell(1) != null && sheet.GetRow(i).GetCell(2) != null &&
                                 sheet.GetRow(i).GetCell(3) != null && sheet.GetRow(i).GetCell(4) != null && sheet.GetRow(i).GetCell(5) != null &&
                                 sheet.GetRow(i).GetCell(6) != null && sheet.GetRow(i).GetCell(7) != null && sheet.GetRow(i).GetCell(8) != null &&
                                 sheet.GetRow(i).GetCell(9) != null)
                             {
                                 strArray = new string[10];
                                 strArray[0] = sheet.GetRow(i).GetCell(0).ToString();
                                 strArray[1] = sheet.GetRow(i).GetCell(1).ToString();
                                 strArray[2] = sheet.GetRow(i).GetCell(2).ToString();
                                 strArray[3] = sheet.GetRow(i).GetCell(3).ToString();
                                 strArray[4] = sheet.GetRow(i).GetCell(4).ToString();
                                 strArray[5] = sheet.GetRow(i).GetCell(5).ToString();
                                 strArray[6] = sheet.GetRow(i).GetCell(6).ToString();
                                 strArray[7] = sheet.GetRow(i).GetCell(7).ToString();
                                 strArray[8] = sheet.GetRow(i).GetCell(8).ToString();
                                 strArray[9] = sheet.GetRow(i).GetCell(9).ToString();
                                 objToReturn.Add(strArray);
                             }
                         }
                         return PartialView(objToReturn);
                     }
                     else
                     {
                         ViewBag.Error = "El formato del archivo no es válido.";
                         return PartialView();
                     }
                 }
            }
            catch (Exception ex)
            {
                if (!(ex is CustomException))
                    AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                ViewBag.Error = "El formato del archivo no es válido.";
                return PartialView();
            }
        }
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            Respuesta<RegistroIndicadorExterno> respuesta = new Respuesta<RegistroIndicadorExterno>();
            Respuesta<List<RegistroIndicadorExterno>> respuestaBitacora = new Respuesta<List<RegistroIndicadorExterno>>();
            JSONResult<RegistroIndicadorExterno> jsonRespuesta = new JSONResult<RegistroIndicadorExterno>();
            try
            {
                var startRow = 8;
                string archivo = file.FileName;
                if (archivo.ToUpper().Contains("XLSX"))
                {
                    var package = new ExcelPackage(file.InputStream);
                    package.Workbook.Protection.LockRevision = true;
                    package.Workbook.Protection.LockStructure = true;
                    ExcelWorksheet ws = package.Workbook.Worksheets.FirstOrDefault();
                    ExcelWorksheet ws2 = package.Workbook.Worksheets[2];
                    
                    if (ws != null) 
                    {
                        respuestaBitacora = refRegistroIndicadorExternoBL.leerXlsx(ws, ws2, startRow);                        
                    }
                    else
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, String.Format(GB.SUTEL.Shared.ErrorTemplate.InternalError, "El formato del archivo no es válido."));
                    }
                
                }
                else 
                {

                    HSSFWorkbook hssfwb = new HSSFWorkbook(file.InputStream);
                    ISheet sheet = hssfwb.GetSheet("Hoja1");
                    ISheet sheet2 = hssfwb.GetSheet("Inmodificable");

                    #region listas logicas
                    FuenteExternaBL objFuenteExternaBL = new FuenteExternaBL(AppContext);
                    PeriodicidadBL objPeriodicidadBL = new PeriodicidadBL(AppContext);
                    ZonaIndicadorExternoBL objZonaBL = new ZonaIndicadorExternoBL(AppContext);
                    RegionIndicadorExternoBL objRegionBL = new RegionIndicadorExternoBL(AppContext);

                    List<FuenteExterna> FuentesExternas = objFuenteExternaBL.ConsultarTodos().objObjeto;
                    List<IndicadorExterno> IndicadorExterno = FuentesExternas.SelectMany(x => x.IndicadorExterno).Where(z => z.Borrado == 0).Distinct().ToList();
                    List<Periodicidad> Periodicidades = objPeriodicidadBL.ConsultarTodos().objObjeto;
                    List<ZonaIndicadorExterno> Zona = objZonaBL.ConsultarTodos().objObjeto;
                    List<RegionIndicadorExterno> Region = objRegionBL.ConsultarTodos().objObjeto;
                    List<Trimestre> Trimestre = refRegistroIndicadorExternoBL.ConsultarTrimestres().objObjeto;
                    List<Canton> Canton = refRegistroIndicadorExternoBL.ConsultarCantones().objObjeto;
                    List<Genero> Genero = refRegistroIndicadorExternoBL.ConsultarGeneros().objObjeto;
                    #endregion
                    if (sheet != null)
                    {
                        respuesta = refRegistroIndicadorExternoBL.leerXls(sheet, sheet2, startRow, IndicadorExterno, Periodicidades, Zona, Region, Trimestre, Canton, Genero);    
                    }
                    else
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, String.Format(GB.SUTEL.Shared.ErrorTemplate.InternalError, "El formato del archivo no es válido."));
                    }
                }
                if (respuesta.blnIndicadorState == 200)
                {
                    jsonRespuesta.strMensaje = respuesta.strMensaje;
                    foreach (var item in respuestaBitacora.objObjeto)
                    {
                        bitacora.gRegistrar<RegistroIndicadorExterno>(HttpContext, 2, item, new RegistroIndicadorExterno(), "mensaje");
                    }                    
                }
                else 
                {
                    jsonRespuesta.strMensaje = respuesta.strMensaje;
                    jsonRespuesta.ok = respuesta.blnIndicadorTransaccion;
                }
                return Content(jsonRespuesta.toJSON());
            }
            catch (Exception ex)
            {
                if (!(ex is CustomException))
                {
                    AppContext.ExceptionBuilder.BuildUIException(GB.SUTEL.Shared.ErrorTemplate.InternalError, ex);
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al cargar.");
                }
                else
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, String.Format(GB.SUTEL.Shared.ErrorTemplate.InternalError, GB.SUTEL.Shared.ErrorTemplate.CouldntSaveAll));                
            }        
        }
    }
}