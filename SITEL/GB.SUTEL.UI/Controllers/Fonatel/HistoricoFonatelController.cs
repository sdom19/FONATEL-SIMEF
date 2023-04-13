using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GB.SIMEF.BL;
using GB.SIMEF.Entities;

using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using GB.SIMEF.Resources;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using OfficeOpenXml;
using GB.SUTEL.UI.Helpers;
using GB.SUTEL.UI.Filters;
using System.Security.Claims;

namespace GB.SUTEL.UI.Controllers.Fonatel
{
    [AuthorizeUserAttribute]
    public class HistoricoFonatelController : Controller
    {

        private readonly DatosHistoricosBL historicoBl;

        public HistoricoFonatelController()
        {
            historicoBl = new DatosHistoricosBL("Historico", System.Web.HttpContext.Current.User.Identity.GetUserId());
        }



        // GET: Solicitud
        public ActionResult Index()
        {


            ViewBag.ListadoDatosHistoricos =
                historicoBl.ObtenerDatos(new DatoHistorico()).objetoRespuesta
                .Select(x => new SelectListItem() { Selected = false, Value = x.id, Text = Utilidades.ConcatenadoCombos(x.Codigo, x.NombrePrograma)}).ToList();
            
            var roles = ((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault().Value.Split(',');
            ViewBag.ConsultasFonatel = roles.Contains(Constantes.RolConsultasFonatel).ToString().ToLower();

            return View();
        }

        [HttpGet]
        public ActionResult Detalle(string  id)
        {
       
            return View();
        }

        /// <summary>
        /// Fecha 04-08-2022
        /// Michael Hernández Cordero
        /// Obtiene datos para la table de categorías INDEX
        /// </summary>
        /// <returns></returns>

        [HttpPost ]
        public async Task<string> ObtenerListaHistorica(DatoHistorico datoHistorico)
        {

            RespuestaConsulta<List<DatoHistorico>> result = null;
            await Task.Run(() =>
            {
                result = historicoBl.ObtenerDatos(datoHistorico);
            });

            return JsonConvert.SerializeObject(result);
        }


        [HttpGet]
        [ConsultasFonatelFilter]
        public async Task<ActionResult> DescargarExcel(string id)
        {
            await Task.Run(() =>
            {
                return historicoBl.ObtenerDatos(new DatoHistorico() { id = id });
            }).ContinueWith(data => {
                MemoryStream stream = new MemoryStream();
                using (ExcelPackage package = new ExcelPackage(stream))
                {
                    package.Workbook.Protection.LockRevision = true;
                    package.Workbook.Protection.LockStructure = true;
                    foreach (var worksheet in data.Result.objetoRespuesta)
                    {
                        ExcelWorksheet worksheetInicio = package.Workbook.Worksheets.Add(worksheet.NombrePrograma);
                       
                        foreach (var columnas in worksheet.DetalleDatoHistoricoColumna)
                        {
                            int cell = columnas.NumeroColumna+1 ;
                            worksheetInicio.Cells[1, cell].Value = columnas.Nombre;
                            worksheetInicio.Cells[1, cell].Style.Font.Bold = true;
                            worksheetInicio.Cells[1, cell].Style.Font.Size = 12;
                            worksheetInicio.Cells[1, cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            worksheetInicio.Cells[1, cell].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(6, 113, 174));
                            worksheetInicio.Cells[1, cell].Style.Font.Color.SetColor(System.Drawing.Color.White);
                            worksheetInicio.Cells[1, cell].Style.Font.Bold = true;
                            worksheetInicio.Cells[1, cell].Style.Font.Size = 12;
                            worksheetInicio.Cells[1, cell].AutoFitColumns();
                            List<DetalleDatoHistoricoFila> listaFila = worksheet.DetalleDatoHistoricoFila.Where(x => x.IdDetalleDatoHistoricoColumna == columnas.IdDetalleDatoHistoricoColumna).ToList();
                            foreach (var fila in listaFila)
                            {
                                int cell2 = fila.NumeroFila + 1;
                                worksheetInicio.Cells[cell2, cell].Value = fila.Atributo;
                            }
                        }
                        //Bug 89482 se llama metodo para registrar bitacora cuando se descarga
                        historicoBl.BitacoraDescargar(worksheet);
                    }
                    Response.BinaryWrite(package.GetAsByteArray());
                    Response.ContentType = "application/vnd.ms-excel.sheet.macroEnabled.12";
                    Response.AddHeader("content-disposition", "attachment;  filename=ReporteHistorico.xlsx");
                }
            });
            return null;
        }
    }
}
