using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using GB.SIMEF.BL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using GB.SUTEL.UI.Helpers;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace GB.SUTEL.UI.Controllers.Fonatel
{
    public class EditarFormularioController : Controller
    {


        #region Variables Públicas del controller
        private readonly RegistroIndicadorFonatelBL EditarRegistroIndicadorBL;
        private readonly DetalleRegistroIndicadorFonatelBL DetalleRegistroIndicadorBL;
        private readonly DetalleRegistroIndicadorCategoriaValorFonatelBL DetalleRegistroIndicadorCategoriaValorFonatelBL;

        #endregion

        public EditarFormularioController()
        {
            EditarRegistroIndicadorBL = new RegistroIndicadorFonatelBL(EtiquetasViewRegistroIndicadorFonatel.RegistroIndicador, System.Web.HttpContext.Current.User.Identity.GetUserId());
            DetalleRegistroIndicadorBL = new DetalleRegistroIndicadorFonatelBL(EtiquetasViewRegistroIndicadorFonatel.RegistroIndicador, System.Web.HttpContext.Current.User.Identity.GetUserId());
            DetalleRegistroIndicadorCategoriaValorFonatelBL = new DetalleRegistroIndicadorCategoriaValorFonatelBL(EtiquetasViewRegistroIndicadorFonatel.RegistroIndicador, System.Web.HttpContext.Current.User.Identity.GetUserId());
        }


        #region Metodos de Vista

        [HttpGet]
        public ActionResult Index()
        {
            if (((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault() != null)
            {
                string nombreUsuario = ((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault().Value;
                RespuestaConsulta<List<RegistroIndicadorFonatel>> model = EditarRegistroIndicadorBL.ObtenerRegistroIndicador(new RegistroIndicadorFonatel()
                {
                    RangoFecha = true
                }, nombreUsuario);
                return View(model.objetoRespuesta);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpGet]
        public ActionResult Edit(string idSolicitud, string idFormulario)
        {
            RespuestaConsulta<List<RegistroIndicadorFonatel>> model = EditarRegistroIndicadorBL.ObtenerDatos(new RegistroIndicadorFonatel()
            {
                FormularioId = idFormulario,
                Solicitudid = idSolicitud,
            });

            if (model.CantidadRegistros == 1)
            {
                return View(model.objetoRespuesta.Single());
            }
            else
            {
                return View("index");
            }
        }

        #endregion

        #region Metodos de controlador


        [HttpGet]
        public ActionResult DescargarExcel(string idSolicitud, string idFormulario)
        {
            MemoryStream stream = new MemoryStream();

            using (ExcelPackage package = new ExcelPackage(stream))
            {
                var Formulario = EditarRegistroIndicadorBL.ObtenerDatos(new RegistroIndicadorFonatel() { Solicitudid = idSolicitud, FormularioId = idFormulario }).objetoRespuesta.Single();

                for (int ws = 0; ws < Formulario.DetalleRegistroIndcadorFonatel.Count(); ws++)
                {
                    var maxFilas = Formulario.DetalleRegistroIndcadorFonatel[ws].CantidadFilas;
                    var cantVariables = Formulario.DetalleRegistroIndcadorFonatel[ws].DetalleRegistroIndicadorVariableFonatel.Count();
                    var cantCategorias = Formulario.DetalleRegistroIndcadorFonatel[ws].DetalleRegistroIndicadorCategoriaFonatel.Count();
                    var maxColumnas = cantVariables + cantCategorias;

                    int fila = 1;
                    int columna = 0;


                    ExcelWorksheet worksheetInicio = package.Workbook.Worksheets.Add(Formulario.DetalleRegistroIndcadorFonatel[ws].TituloHojas);

                    for (int i = 0; i < cantVariables; i++)
                    {
                        worksheetInicio.Cells[fila, columna + 1].Value = Formulario.DetalleRegistroIndcadorFonatel[ws].DetalleRegistroIndicadorVariableFonatel[i].NombreVariable;
                        worksheetInicio.Cells[fila, columna + 1].Style.Font.Bold = true;
                        worksheetInicio.Cells[fila, columna + 1].Style.Font.Size = 12;
                        worksheetInicio.Cells[fila, columna + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheetInicio.Cells[fila, columna + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(64, 152, 166));
                        worksheetInicio.Cells[fila, columna + 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        worksheetInicio.Cells[fila, columna + 1].AutoFitColumns();

                        for (int x = 1; x <= maxFilas; x++)
                        {
                            worksheetInicio.Cells[fila + x, columna + 1].Style.Font.Bold = true;
                            worksheetInicio.Cells[fila + x, columna + 1].Style.Font.Size = 12;
                            worksheetInicio.Cells[fila + x, columna + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            worksheetInicio.Cells[fila + x, columna + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(64, 152, 166));
                            worksheetInicio.Cells[fila + x, columna + 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                            worksheetInicio.Cells[fila + x, columna + 1].AutoFitColumns();
                            worksheetInicio.Cells[fila + x, columna + 1].Value = "1";

                        }

                        columna++;
                    }

                    for (int i = 0; i < cantCategorias; i++)
                    {
                        var Categoria = Formulario.DetalleRegistroIndcadorFonatel[ws].DetalleRegistroIndicadorCategoriaFonatel[i];
                        var Valores = Formulario.DetalleRegistroIndcadorFonatel[ws].DetalleRegistroIndicadorCategoriaValorFonatel.Where(x => x.idCategoria == Categoria.idCategoria).ToList();

                        worksheetInicio.Cells[fila, columna + 1].Value = Categoria.NombreCategoria;
                        worksheetInicio.Cells[fila, columna + 1].Style.Font.Bold = true;
                        worksheetInicio.Cells[fila, columna + 1].Style.Font.Size = 12;
                        worksheetInicio.Cells[fila, columna + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheetInicio.Cells[fila, columna + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(6, 113, 174));
                        worksheetInicio.Cells[fila, columna + 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        worksheetInicio.Cells[fila, columna + 1].AutoFitColumns();

                        foreach (var item in Valores)
                        {
                            worksheetInicio.Cells[fila + item.NumeroFila, columna + 1].Value = item.Valor;
                            worksheetInicio.Cells[fila + item.NumeroFila, columna + 1].Style.Font.Size = 12;
                            worksheetInicio.Cells[fila + item.NumeroFila, columna + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            worksheetInicio.Cells[fila + item.NumeroFila, columna + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                            worksheetInicio.Cells[fila + item.NumeroFila, columna + 1].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                            worksheetInicio.Cells[fila + item.NumeroFila, columna + 1].AutoFitColumns();
                        }

                        columna++;
                    }

                }

                Response.BinaryWrite(package.GetAsByteArray());
                Response.ContentType = "application/vnd.ms-excel.sheet.macroEnabled.12";
                Response.AddHeader("content-disposition", "attachment;  filename=" + Formulario.Formulario + ".xlsx");
            }

            return new EmptyResult();
        }

        [HttpGet]
        public ActionResult DescargarExcelUnitario(string idSolicitud, string idFormulario, string idIndicador)
        {
            var Formulario = EditarRegistroIndicadorBL.ObtenerDatos(new RegistroIndicadorFonatel() { Solicitudid = idSolicitud, FormularioId = idFormulario, IndicadorId = idIndicador }).objetoRespuesta.Single();
            var Detalle = DetalleRegistroIndicadorBL.ObtenerDatos(new DetalleRegistroIndicadorFonatel() { IdSolicitudString = idSolicitud, IdFormularioString = idFormulario, IdIndicadorString = idIndicador }).objetoRespuesta.Single();

            var maxFilas = Detalle.CantidadFilas;
            var cantVariables = Detalle.DetalleRegistroIndicadorVariableFonatel.Count();
            var cantCategorias = Detalle.DetalleRegistroIndicadorCategoriaFonatel.Count();
            var maxColumnas = cantVariables + cantCategorias;

            int fila = 1;
            int columna = 0;

            MemoryStream stream = new MemoryStream();

            using (ExcelPackage package = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheetInicio = package.Workbook.Worksheets.Add(Detalle.TituloHojas);

                for (int i = 0; i < cantVariables; i++)
                {
                    worksheetInicio.Cells[fila, columna + 1].Value = Detalle.DetalleRegistroIndicadorVariableFonatel[i].NombreVariable;
                    worksheetInicio.Cells[fila, columna + 1].Style.Font.Bold = true;
                    worksheetInicio.Cells[fila, columna + 1].Style.Font.Size = 12;
                    worksheetInicio.Cells[fila, columna + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheetInicio.Cells[fila, columna + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(64, 152, 166));
                    worksheetInicio.Cells[fila, columna + 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                    worksheetInicio.Cells[fila, columna + 1].AutoFitColumns();

                    for (int x = 1; x <= maxFilas; x++)
                    {
                        worksheetInicio.Cells[fila + x, columna + 1].Value = "1";
                        worksheetInicio.Cells[fila + x, columna + 1].Style.Font.Bold = true;
                        worksheetInicio.Cells[fila + x, columna + 1].Style.Font.Size = 12;
                        worksheetInicio.Cells[fila + x, columna + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheetInicio.Cells[fila + x, columna + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(64, 152, 166));
                        worksheetInicio.Cells[fila + x, columna + 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        worksheetInicio.Cells[fila + x, columna + 1].AutoFitColumns();

                    }

                    columna++;
                }

                for (int i = 0; i < cantCategorias; i++)
                {
                    var Categoria = Detalle.DetalleRegistroIndicadorCategoriaFonatel[i];
                    var Valores = Detalle.DetalleRegistroIndicadorCategoriaValorFonatel.Where(x => x.idCategoria == Categoria.idCategoria).ToList();

                    worksheetInicio.Cells[fila, columna + 1].Value = Categoria.NombreCategoria;
                    worksheetInicio.Cells[fila, columna + 1].Style.Font.Bold = true;
                    worksheetInicio.Cells[fila, columna + 1].Style.Font.Size = 12;
                    worksheetInicio.Cells[fila, columna + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheetInicio.Cells[fila, columna + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(6, 113, 174));
                    worksheetInicio.Cells[fila, columna + 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                    worksheetInicio.Cells[fila, columna + 1].AutoFitColumns();

                    foreach (var item in Valores)
                    {
                        worksheetInicio.Cells[fila + item.NumeroFila, columna + 1].Value = item.Valor;
                        worksheetInicio.Cells[fila + item.NumeroFila, columna + 1].Style.Font.Size = 12;
                        worksheetInicio.Cells[fila + item.NumeroFila, columna + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheetInicio.Cells[fila + item.NumeroFila, columna + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                        worksheetInicio.Cells[fila + item.NumeroFila, columna + 1].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        worksheetInicio.Cells[fila + item.NumeroFila, columna + 1].AutoFitColumns();
                    }

                    columna++;
                }

                Response.BinaryWrite(package.GetAsByteArray());
                Response.ContentType = "application/vnd.ms-excel.sheet.macroEnabled.12";
                Response.AddHeader("content-disposition", "attachment;  filename=" + Formulario.Formulario + ".xlsx");
            }

            return new EmptyResult();

        }

        [HttpPost]
        public async Task<string> CargarExcel(Object datos, int cantidadFilas)
        {
            RespuestaConsulta<List<DetalleRegistroIndicadorCategoriaValorFonatel>> result = null;
            string ruta = Utilidades.RutaCarpeta(ConfigurationManager.AppSettings["rutaCarpetaSimef"]);
            string hilera = ((string[])datos)[0].Replace("{\"datos\":", "").Replace("}}", "}");
            DetalleRegistroIndicadorFonatel obj = JsonConvert.DeserializeObject<DetalleRegistroIndicadorFonatel>(hilera);

            if (Request.Files.Count > 0)
            {
                HttpFileCollectionBase files = Request.Files;
                //HttpRequestBase lista = Request.Form;
                HttpPostedFileBase file = files[0];
                string fileName = file.FileName;
                Directory.CreateDirectory(ruta);
                string path = Path.Combine(ruta, fileName);
                await Task.Run(() =>
                {
                    result = DetalleRegistroIndicadorCategoriaValorFonatelBL.CargarExcel(file, obj, cantidadFilas);

                });

                file.SaveAs(path);
            }

            return JsonConvert.SerializeObject(result);
        }


        /// <summary>
        /// Francisco Vindas
        /// 06-12-2022
        /// Metodo para obtener los detalles del registro Indicador
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 

        [HttpPost]
        public async Task<string> ConsultaRegistroIndicadorDetalle(DetalleRegistroIndicadorFonatel detalleIndicadorFonatel)
        {
            RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>> result = null;
            await Task.Run(() =>
            {
                result = DetalleRegistroIndicadorBL.ObtenerDatos(detalleIndicadorFonatel);

            });
            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Fecha 07/12/2022
        /// Francisco Vindas Ruiz
        /// Metodo para obtener la lista de DetalleRegistroIndicadorCategoriaValorFonatel
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public async Task<string> ObtenerListaDetalleRegistroIndicadorCategoriaValorFonatel(DetalleRegistroIndicadorCategoriaValorFonatel detalle)
        {
            RespuestaConsulta<List<DetalleRegistroIndicadorCategoriaValorFonatel>> result = null;

            await Task.Run(() =>
            {
                result = DetalleRegistroIndicadorCategoriaValorFonatelBL.ObtenerDatos(detalle);
            });

            return JsonConvert.SerializeObject(result);
        }


        [HttpPost]
        public async Task<string> ActualizarDetalleRegistroIndicador(List<DetalleRegistroIndicadorFonatel> lista)
        {

            //Creamos una variable resultado de tipo lista DetalleRegistroIndicadorFonatel
            RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>> result = null;

            await Task.Run(() =>
            {
                //Conectamos con el BL de relacion categoria para insertar y enviamos  la relacion
                result = DetalleRegistroIndicadorBL.ActualizarDetalleRegistroIndicadorFonatelMultiple(lista);
              
            });

            //Retornamos un Json con el resultado
            return JsonConvert.SerializeObject(result);

        }

        [HttpPost]
        public async Task<string> InsertarRegistroIndicadorVariable(List<DetalleRegistroIndicadorCategoriaValorFonatel> ListaDetalleIndicadorValor)
        {

            RespuestaConsulta<List<DetalleRegistroIndicadorCategoriaValorFonatel>> result = null;

            await Task.Run(() =>
            {

                result = DetalleRegistroIndicadorCategoriaValorFonatelBL.InsertarDatos(ListaDetalleIndicadorValor);

            });

            return JsonConvert.SerializeObject(result);

        }


        /// <summary>
        /// Francisco Vindas
        /// 29-11-2022
        /// Metodo para obtener la lista de Registros de Indicador a Editar
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        //[HttpGet]
        //public async Task<string> ObtenerListaRegistroIndicador()
        //{
        //    RespuestaConsulta<List<RegistroIndicadorFonatel>> result = null;

        //    await Task.Run(() =>
        //    {
        //        result = EditarRegistroIndicadorBL.ObtenerDatos(new RegistroIndicadorFonatel());

        //    });
        //    return JsonConvert.SerializeObject(result);
        //}


        #endregion




    }

}
