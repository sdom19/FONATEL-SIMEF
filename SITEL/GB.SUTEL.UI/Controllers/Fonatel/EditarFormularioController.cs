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
using GB.SUTEL.UI.Filters;
using GB.SUTEL.UI.Helpers;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Color = System.Drawing.Color;
using System.Drawing;

namespace GB.SUTEL.UI.Controllers.Fonatel
{
    [AuthorizeUserAttribute]
    public class EditarFormularioController : Controller
    {


        #region Variables Públicas del controller
        private readonly RegistroIndicadorFonatelBL EditarRegistroIndicadorBL;
        private readonly DetalleRegistroIndicadorFonatelBL DetalleRegistroIndicadorBL;
        private readonly DetalleRegistroIndicadorCategoriaValorFonatelBL DetalleRegistroIndicadorCategoriaValorFonatelBL;

        #endregion

        public EditarFormularioController()
        {
            EditarRegistroIndicadorBL = new RegistroIndicadorFonatelBL(EtiquetasViewRegistroIndicadorFonatel.DescargaPantalla, System.Web.HttpContext.Current.User.Identity.GetUserId());
            DetalleRegistroIndicadorBL = new DetalleRegistroIndicadorFonatelBL(EtiquetasViewRegistroIndicadorFonatel.DescargaPantalla, System.Web.HttpContext.Current.User.Identity.GetUserId());
            DetalleRegistroIndicadorCategoriaValorFonatelBL = new DetalleRegistroIndicadorCategoriaValorFonatelBL(EtiquetasViewRegistroIndicadorFonatel.DescargaPantalla, System.Web.HttpContext.Current.User.Identity.GetUserId());
        }


        #region Metodos de Vista

        [HttpGet]
        public ActionResult Index()
        {
            var roles = ((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault().Value.Split(',');
            ViewBag.ConsultasFonatel = roles.Contains(Constantes.RolConsultasFonatel).ToString().ToLower();

            if (((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault() != null)
            {
                string nombreUsuario = ((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault().Value;
                RespuestaConsulta<List<RegistroIndicadorFonatel>> model = EditarRegistroIndicadorBL.ObtenerEditarRegistroIndicador(new RegistroIndicadorFonatel()
                {
                    RangoFecha = true
                }, nombreUsuario);
                model.objetoRespuesta = model.objetoRespuesta == null ?
                    new List<RegistroIndicadorFonatel>():
                    
                    model.objetoRespuesta.Where(x => x.FechaFin < System.DateTime.Today).ToList();
                return View(model.objetoRespuesta);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpGet]
        [ConsultasFonatelFilter]
        public ActionResult Edit(string idSolicitud, string idFormularioWeb)
        {
            RespuestaConsulta<List<RegistroIndicadorFonatel>> model = EditarRegistroIndicadorBL.ObtenerDatos(new RegistroIndicadorFonatel()
            {
                FormularioId = idFormularioWeb,
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

        /// <summary>
        /// Autor: Francisco Vindas Ruiz
        /// Fecha: 17-02-2023
        /// Metodo que permite Descargar el Excel Multiple de las tablas principal segun los datos, categorias, variables dato de un Indicador en especifico
        /// </summary>
        /// <param name="idSolicitud"></param>
        /// <param name="idFormularioWeb"></param>
        /// <returns></returns>
        [HttpGet]
        [ConsultasFonatelFilter]
        public ActionResult DescargarExcel(string idSolicitud, string idFormularioWeb)
        {
            MemoryStream stream = new MemoryStream();

            DetalleRegistroIndicadorFonatel objDetalleRegistroIndicador=null;


            using (ExcelPackage package = new ExcelPackage(stream))
            {
                package.Workbook.Protection.LockRevision = true;
                package.Workbook.Protection.LockStructure = true;
                package.Workbook.Protection.SetPassword(idSolicitud);

                var Formulario = EditarRegistroIndicadorBL.ObtenerDatos(new RegistroIndicadorFonatel() { Solicitudid = idSolicitud, FormularioId = idFormularioWeb }).objetoRespuesta.Single();

                var NombreExcel = Formulario.Formulario.Trim();



                for (int ws = 0; ws < Formulario.DetalleRegistroIndcadorFonatel.Count(); ws++)
                {

                    var maxFilas = Formulario.DetalleRegistroIndcadorFonatel[ws].CantidadFila;
                    var cantVariables = Formulario.DetalleRegistroIndcadorFonatel[ws].DetalleRegistroIndicadorVariableFonatel.Count();
                    var cantCategorias = Formulario.DetalleRegistroIndcadorFonatel[ws].DetalleRegistroIndicadorCategoriaFonatel.Count();
                    var maxColumnas = cantVariables + cantCategorias;
                    var indicador = Formulario.DetalleRegistroIndcadorFonatel[ws].IdIndicadorString;

                    int fila = Constantes.Excel.Columna;
                    int columna = 0;

                    objDetalleRegistroIndicador = DetalleRegistroIndicadorBL.ObtenerDatos(new DetalleRegistroIndicadorFonatel() { IdSolicitudString = idSolicitud, idFormularioWebString = idFormularioWeb, IdIndicadorString = indicador}).objetoRespuesta.Single();

                    ExcelWorksheet worksheetInicio = package.Workbook.Worksheets.Add(Formulario.DetalleRegistroIndcadorFonatel[ws].TituloHoja);

                    worksheetInicio.Cells["A1:E8"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

                    worksheetInicio.Cells["A1:E6"].Style.Fill.BackgroundColor.SetColor(Constantes.fontColorFromHex);
                    worksheetInicio.Cells["A7:E7"].Style.Fill.BackgroundColor.SetColor(Constantes.headColorFromHex);
                    worksheetInicio.Cells["A8:E8"].Style.Fill.BackgroundColor.SetColor(Constantes.headColorFromHex);
                    worksheetInicio.Row(7).Height = 6;
                    worksheetInicio.Row(8).Height = 4;
                    // carga el logo
                    Image logo = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constantes.Rutalogo);
                    logo = (Image)(new Bitmap(logo, new Size(313, 90)));
                    var picture = worksheetInicio.Drawings.AddPicture(Constantes.Nombrelogo, logo);
                    picture.SetPosition(1, 0, 0, 0);

                    worksheetInicio.Protection.IsProtected = true;
                    worksheetInicio.Protection.SetPassword(idSolicitud);

                    
                    //for (int i = 0; i < cantVariables; i++)
                    //{
                    foreach(var variables in Formulario.DetalleRegistroIndcadorFonatel[ws].DetalleRegistroIndicadorVariableFonatel)
                    {
                         var ValoresVariables = objDetalleRegistroIndicador.DetalleRegistroIndicadorVariableValorFonatel.Where(x => x.IdVariable == variables.idVariable).ToList();
                        //var Valores = Detalle.DetalleRegistroIndicadorVariableValorFonatel.Where(x => x.IdSolicitud == Detalle.IdSolicitud && x.idFormularioWeb == Detalle.idFormularioWeb).ToList();

                        worksheetInicio.Cells[fila, columna + 1].Value = variables.NombreVariable;
                        worksheetInicio.Cells[fila, columna + 1].Style.Font.Bold = true;
                        worksheetInicio.Cells[fila, columna + 1].Style.Font.Size = 12;
                        worksheetInicio.Cells[fila, columna + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheetInicio.Cells[fila, columna + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(64, 152, 166));
                        worksheetInicio.Cells[fila, columna + 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        worksheetInicio.Cells[fila, columna + 1].AutoFitColumns();

                        
                        foreach (var item in ValoresVariables)
                        {
                            var FilaVariableDato = item.NumeroFila;

                            worksheetInicio.Cells[fila + FilaVariableDato, columna + 1].Value = item.Valor;
                            worksheetInicio.Cells[fila + FilaVariableDato, columna + 1].Style.Font.Bold = true;
                            worksheetInicio.Cells[fila + FilaVariableDato, columna + 1].Style.Font.Size = 12;
                            worksheetInicio.Cells[fila + FilaVariableDato, columna + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            worksheetInicio.Cells[fila + FilaVariableDato, columna + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(64, 152, 166));
                            worksheetInicio.Cells[fila + FilaVariableDato, columna + 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                            worksheetInicio.Cells[fila + FilaVariableDato, columna + 1].AutoFitColumns();
                            worksheetInicio.Cells[fila + FilaVariableDato, columna + 1].Style.Locked = false;
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
                            worksheetInicio.Cells[fila + item.NumeroFila, columna + 1].Style.Locked = false;
                        }
                        columna++;
                    }

                }
                //89482 Llamado de metodo para bitacora al descargar
                if (!string.IsNullOrEmpty(idSolicitud))
                {
                    int.TryParse(Utilidades.Desencriptar(idSolicitud), out int temp);
                    idSolicitud = temp.ToString();
                }
                EditarRegistroIndicadorBL.BitacoraDescargar(objDetalleRegistroIndicador.RegistroIndicadorFonatel);

                Response.BinaryWrite(package.GetAsByteArray());
                Response.ContentType = "application/vnd.ms-excel.sheet.macroEnabled.12";
                Response.AddHeader("content-disposition", "attachment;  filename=" + NombreExcel + ".xlsx");
            }

            return new EmptyResult();
        }

        /// <summary>
        /// Autor: Francisco Vindas Ruiz
        /// Fecha: 17-02-2023
        /// Metodo que permite Descargar el Excel Unitario de la tabla principal segun los datos, categorias, variables dato de un Indicador en especifico
        /// </summary>
        /// <param name="idSolicitud"></param>
        /// <param name="idFormularioWeb"></param>
        /// <param name="idIndicador"></param>
        /// <returns></returns>
        [HttpGet]
        [ConsultasFonatelFilter]
        public ActionResult DescargarExcelUnitario(string idSolicitud, string idFormularioWeb, string idIndicador)
        {
            var Formulario = EditarRegistroIndicadorBL.ObtenerDatos(new RegistroIndicadorFonatel() { Solicitudid = idSolicitud, FormularioId = idFormularioWeb, IndicadorId = idIndicador }).objetoRespuesta.Single();
            DetalleRegistroIndicadorFonatel objDetalleRegistroIndicador = DetalleRegistroIndicadorBL.ObtenerDatos(new DetalleRegistroIndicadorFonatel() { IdSolicitudString = idSolicitud, idFormularioWebString = idFormularioWeb, IdIndicadorString = idIndicador }).objetoRespuesta.Single();

            var NombreExcel = Formulario.Formulario.Trim();

            var maxFilas = objDetalleRegistroIndicador.CantidadFila;
            var cantVariables = objDetalleRegistroIndicador.DetalleRegistroIndicadorVariableFonatel.Count();
            var cantCategorias = objDetalleRegistroIndicador.DetalleRegistroIndicadorCategoriaFonatel.Count();
            var maxColumnas = cantVariables + cantCategorias;

            int fila = 9;
            int columna = 0;

            MemoryStream stream = new MemoryStream();

            using (ExcelPackage package = new ExcelPackage(stream))
            {
                package.Workbook.Protection.LockRevision = true;
                package.Workbook.Protection.LockStructure = true;
                package.Workbook.Protection.SetPassword(idSolicitud);

               

                ExcelWorksheet worksheetInicio = package.Workbook.Worksheets.Add(objDetalleRegistroIndicador.TituloHoja);
                worksheetInicio.Cells["A1:E8"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

                worksheetInicio.Cells["A1:E6"].Style.Fill.BackgroundColor.SetColor(Constantes.fontColorFromHex);
                worksheetInicio.Cells["A7:E7"].Style.Fill.BackgroundColor.SetColor(Constantes.headColorFromHex);
                worksheetInicio.Cells["A8:E8"].Style.Fill.BackgroundColor.SetColor(Constantes.headColorFromHex);
                worksheetInicio.Row(7).Height = 6;
                worksheetInicio.Row(8).Height = 4;
                // carga el logo
                Image logo = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constantes.Rutalogo);
                logo = (Image)(new Bitmap(logo, new Size(313, 90)));
                var picture = worksheetInicio.Drawings.AddPicture(Constantes.Nombrelogo , logo);
                picture.SetPosition(1, 0, 0, 0);

                worksheetInicio.Protection.IsProtected = true;
                worksheetInicio.Protection.SetPassword(idSolicitud);

                //    for (int i = 0; i < cantVariables; i++)
                //   {
                foreach (var variables in objDetalleRegistroIndicador.DetalleRegistroIndicadorVariableFonatel)
                {
                    var ValoresVariables = objDetalleRegistroIndicador.DetalleRegistroIndicadorVariableValorFonatel.Where(x => x.IdVariable == variables.idVariable).ToList();

                    //var Valores = Detalle.DetalleRegistroIndicadorVariableValorFonatel.Where(x => x.IdSolicitud == Detalle.IdSolicitud && x.idFormularioWeb == Detalle.idFormularioWeb && x.IdIndicador == Detalle.IdIndicador).ToList();
                    worksheetInicio.Cells[fila, columna + 1].Value = variables.NombreVariable;
                    worksheetInicio.Cells[fila, columna + 1].Style.Font.Bold = true;
                    worksheetInicio.Cells[fila, columna + 1].Style.Font.Size = 12;
                    worksheetInicio.Cells[fila, columna + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheetInicio.Cells[fila, columna + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(64, 152, 166));
                    worksheetInicio.Cells[fila, columna + 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                    worksheetInicio.Cells[fila, columna + 1].AutoFitColumns();

                    foreach (var item in ValoresVariables)
                    {
                        var FilaVariableDato = item.NumeroFila;

                        worksheetInicio.Cells[fila + FilaVariableDato, columna + 1].Value = item.Valor;
                        worksheetInicio.Cells[fila + FilaVariableDato, columna + 1].Style.Font.Bold = true;
                        worksheetInicio.Cells[fila + FilaVariableDato, columna + 1].Style.Font.Size = 12;
                        worksheetInicio.Cells[fila + FilaVariableDato, columna + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheetInicio.Cells[fila + FilaVariableDato, columna + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(64, 152, 166));
                        worksheetInicio.Cells[fila + FilaVariableDato, columna + 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        worksheetInicio.Cells[fila + FilaVariableDato, columna + 1].AutoFitColumns();
                        worksheetInicio.Cells[fila + FilaVariableDato, columna + 1].Style.Locked = false;
                    }
                    
                    columna++;
                }

                for (int i = 0; i < cantCategorias; i++)
                {
                    var Categoria = objDetalleRegistroIndicador.DetalleRegistroIndicadorCategoriaFonatel[i];
                    var Valores = objDetalleRegistroIndicador.DetalleRegistroIndicadorCategoriaValorFonatel.Where(x => x.idCategoria == Categoria.idCategoria).ToList();

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
                        worksheetInicio.Cells[fila + item.NumeroFila, columna + 1].Style.Locked = false;
                    }

                    columna++;
                }

                //89482 Llamado de metodo para bitacora al descargar
                if (!string.IsNullOrEmpty(idSolicitud))
                {
                    int.TryParse(Utilidades.Desencriptar(idSolicitud), out int temp);
                    idSolicitud = temp.ToString();
                }
                EditarRegistroIndicadorBL.BitacoraDescargar(objDetalleRegistroIndicador.RegistroIndicadorFonatel);

                Response.BinaryWrite(package.GetAsByteArray());
                Response.ContentType = "application/vnd.ms-excel.sheet.macroEnabled.12";
                Response.AddHeader("content-disposition", "attachment;  filename=" + NombreExcel + ".xlsx");
            }

            return new EmptyResult();

        }


        /// <summary>
        /// Autor: Francisco Vindas Ruiz
        /// Fecha: 17-02-2023
        /// Metodo que permite subir los datos de una Excel a la tabla principal segun los datos, categorias, variables dato de un Indicador en especifico
        /// </summary>
        /// <param name="datos"></param>
        /// <param name="cantidadFilas"></param>
        /// <returns></returns>
        [HttpPost]
        [ConsultasFonatelFilter]
        public async Task<string> CargarExcel(Object datos, int cantidadFila)
        {
            //retonar un detalle registro indicador con result / resultVariable

            RespuestaConsulta<List<DetalleRegistroIndicadorCategoriaValorFonatel>> result = null;

            RespuestaConsulta<List<DetalleRegistroIndicadorVariableValorFonatel>> resultVariable = null;

            RespuestaConsulta<DetalleRegistroIndicadorFonatel> resultDetalle = new RespuestaConsulta<DetalleRegistroIndicadorFonatel>();

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
                    result = DetalleRegistroIndicadorCategoriaValorFonatelBL.CargarExcel(file, obj, cantidadFila);
                    resultDetalle.objetoRespuesta = new DetalleRegistroIndicadorFonatel();
                    if (result.HayError == 0)
                    {
                        resultDetalle.objetoRespuesta.DetalleRegistroIndicadorCategoriaValorFonatel = result.objetoRespuesta.ToList();
                    }
                    else
                    {
                        resultDetalle.HayError = result.HayError;
                        resultDetalle.MensajeError = result.MensajeError;
                        
                    }

                }).ContinueWith(data =>
                {
                    if (resultDetalle.HayError == 0)
                    {
                        resultVariable = DetalleRegistroIndicadorCategoriaValorFonatelBL.CargarExcelVariable(file, obj, cantidadFila);
                        if (resultVariable.HayError == 0)
                        {
                            resultDetalle.objetoRespuesta.DetalleRegistroIndicadorVariableValorFonatel = resultVariable.objetoRespuesta.ToList();
                        }
                        else
                        {
                            resultDetalle.HayError = resultVariable.HayError;
                            resultDetalle.MensajeError = resultVariable.MensajeError;
                        }
                    }

                });

                file.SaveAs(path);
            }

            return JsonConvert.SerializeObject(resultDetalle);
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

        /// <summary>
        /// Fecha 01/02/2023
        /// Francisco Vindas 
        /// Metodo para obtener la lista de DetalleRegistroIndicadorCategoriaValorFonatel y DetalleRegistroIndicadorVariableValorFonatel
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public async Task<string> ObtenerListaDetalleRegistroIndicadorValoresFonatel(DetalleRegistroIndicadorFonatel detalle)
        {
            RespuestaConsulta<DetalleRegistroIndicadorFonatel> result = null;

            await Task.Run(() =>
            {
                result = DetalleRegistroIndicadorBL.ObtenerListaDetalleRegistroIndicadorValoresFonatel(detalle);
            });
            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        [ConsultasFonatelFilter]
        public async Task<string> ActualizarDetalleRegistroIndicador(List<DetalleRegistroIndicadorFonatel> lista)
        {
            RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>> result = null;

            await Task.Run(() =>
            {
                return DetalleRegistroIndicadorBL.ActualizarDetalleRegistroIndicadorFonatelMultiple(lista);
            }).ContinueWith((resultado) =>
            {
                result =resultado.Result;      
            });

            //Retornamos un Json con el resultado
            return JsonConvert.SerializeObject(result);

        }

        [HttpPost]
        [ConsultasFonatelFilter]
        public async Task<string> InsertarRegistroIndicadorVariable(DetalleRegistroIndicadorFonatel ListaDetalleIndicadorValor)
        {

            RespuestaConsulta<List<DetalleRegistroIndicadorCategoriaValorFonatel>> result = null;

            await Task.Run(() =>
            {
                result = DetalleRegistroIndicadorCategoriaValorFonatelBL.InsertarDatos(ListaDetalleIndicadorValor,true);

            });

            return JsonConvert.SerializeObject(result);

        }

        /// <summary>
        /// Autor: Francisco Vindas RUiz
        /// Fecha: 27/01/2023
        /// Metodo: El metodo sirve para realizar la carga total de la informacion de Registro Indicador a la Base de Datos de SITELP
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        [HttpPost]
        [ConsultasFonatelFilter]
        public async Task<string> CargaTotalRegistroIndicador(List<DetalleRegistroIndicadorFonatel> lista)
        {

            RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>> result = null;

            RespuestaConsulta<bool> envioCorreo = new RespuestaConsulta<bool>();

            RegistroIndicadorFonatel registroIndicador = new RegistroIndicadorFonatel();

            await Task.Run(() =>
            {
                result = DetalleRegistroIndicadorBL.CargaTotalRegistroIndicador(lista);

            })
            .ContinueWith(data =>
            {
                if (result.objetoRespuesta.Count() > 0)
                {
                    var respuestaConsulta = result.objetoRespuesta.FirstOrDefault();

                    registroIndicador.IdSolicitud = respuestaConsulta.IdSolicitud;

                    registroIndicador.idFormularioWeb = respuestaConsulta.idFormularioWeb;

                    envioCorreo = EditarRegistroIndicadorBL.EnvioCorreoInformante(registroIndicador);

                    envioCorreo = EditarRegistroIndicadorBL.EnvioCorreoEncargado(registroIndicador);

                }
            });
            return JsonConvert.SerializeObject(result);

        }



        #endregion
    }
}
