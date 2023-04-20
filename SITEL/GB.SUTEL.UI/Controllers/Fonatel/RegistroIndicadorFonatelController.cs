using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
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
using Color = System.Drawing.Color;

namespace GB.SUTEL.UI.Controllers.Fonatel
{
    [AuthorizeUserAttribute]
    public class RegistroIndicadorFonatelController  : Controller
    {
        // GET: RegistroIndicadorFonatel


       private readonly RegistroIndicadorFonatelBL registroIndicadorBL;
        private readonly DetalleRegistroIndicadorFonatelBL detalleRegistroIndicadorBL;
        private readonly DetalleRegistroIndicadorCategoriaValorFonatelBL detalleRegistroIndicadorCategoriaValorFonatelBL;

        public RegistroIndicadorFonatelController()
        {

            registroIndicadorBL = new RegistroIndicadorFonatelBL(EtiquetasViewRegistroIndicadorFonatel.RegistroIndicador, System.Web.HttpContext.Current.User.Identity.GetUserId());
            detalleRegistroIndicadorBL = new DetalleRegistroIndicadorFonatelBL(EtiquetasViewRegistroIndicadorFonatel.RegistroIndicador, System.Web.HttpContext.Current.User.Identity.GetUserId());
            detalleRegistroIndicadorCategoriaValorFonatelBL = new DetalleRegistroIndicadorCategoriaValorFonatelBL(EtiquetasViewRegistroIndicadorFonatel.RegistroIndicador, System.Web.HttpContext.Current.User.Identity.GetUserId());
        }

        #region Metodos de las vistas
        [HttpGet]
        public ActionResult Index()
        {
            if (((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault() != null)
            {
                string nombreUsuario = ((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault().Value;
                RespuestaConsulta<List<RegistroIndicadorFonatel>> model = registroIndicadorBL.ObtenerRegistroIndicador(new RegistroIndicadorFonatel()
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
        public ActionResult Create(string idSolicitud, string idFormulario)
        {
            RespuestaConsulta<List<RegistroIndicadorFonatel>> model = registroIndicadorBL.ObtenerDatos(new RegistroIndicadorFonatel()
            {
                FormularioId= idFormulario,
                Solicitudid=idSolicitud,
            });
            if (model.CantidadRegistros==1)
            {
                return View(model.objetoRespuesta.Single());
            }
            else
            {
                return View("index");
            }
        
        }

        // GET: RegistroIndicadorFonatel/Details/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View();
        }


        #endregion

        /// <summary>
        /// Francisco Vindas
        /// 02/09/2022
        /// Metodo para descargar en un Excel los detalles Relacion Categoria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet]
        public ActionResult DescargarExcel(List<String> listaParametros)
        {
            string nombre = "PruebaExcel";

            DetalleRegistroIndicadorFonatel detalleRegistroIndicadorFonatel = new DetalleRegistroIndicadorFonatel();
            string hilera = listaParametros[0];
            string[] lista = hilera.Split(',');

            detalleRegistroIndicadorFonatel.idFormularioWebString = lista[1];
            detalleRegistroIndicadorFonatel.IdIndicadorString = lista[2];
            detalleRegistroIndicadorFonatel.IdSolicitudString = lista[0];
            detalleRegistroIndicadorFonatel.CantidadFila= Convert.ToInt32(lista[3]);

            RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>> result = null;
            result = detalleRegistroIndicadorBL.ObtenerDatos(detalleRegistroIndicadorFonatel);

            RegistroIndicadorFonatel registroIndicadorFonatel = new RegistroIndicadorFonatel();
            registroIndicadorFonatel.Solicitudid = lista[0];
            registroIndicadorFonatel.FormularioId = lista[1];
            RespuestaConsulta<List<RegistroIndicadorFonatel>> registro = null;
            registro = registroIndicadorBL.ObtenerDatos(registroIndicadorFonatel);
            nombre = registro.objetoRespuesta[0].Formulario.Trim();

            List<DetalleRegistroIndicadorCategoriaFonatel> listaCategoria = result.objetoRespuesta[0].DetalleRegistroIndicadorCategoriaFonatel;
            List<DetalleRegistroIndicadorVariableFonatel> listaVariable = result.objetoRespuesta[0].DetalleRegistroIndicadorVariableFonatel;

            MemoryStream stream = new MemoryStream();

            using (ExcelPackage package = new ExcelPackage(stream))
            {
                package.Workbook.Protection.LockRevision = true;
                package.Workbook.Protection.LockStructure = true;
                package.Workbook.Protection.SetPassword(nombre);

                ExcelWorksheet worksheetInicio = package.Workbook.Worksheets.Add(result.objetoRespuesta[0].TituloHoja);
               

                worksheetInicio.Cells["A1:E8"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

                worksheetInicio.Cells["A1:E6"].Style.Fill.BackgroundColor.SetColor(Constantes.fontColorFromHex);
                worksheetInicio.Cells["A7:E7"].Style.Fill.BackgroundColor.SetColor(Constantes.headColorFromHex);
                worksheetInicio.Cells["A8:E8"].Style.Fill.BackgroundColor.SetColor(Constantes.headColorFromHex);
                worksheetInicio.Row(7).Height = 6;
                worksheetInicio.Row(8).Height = 4;

                // carga el logo
                Image logo = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constantes.Rutalogo );
                logo = (Image)(new Bitmap(logo, new Size(313, 90)));
                var picture = worksheetInicio.Drawings.AddPicture(Constantes.Nombrelogo, logo);
                picture.SetPosition(1, 0, 0, 0);
                //fin del logo
                worksheetInicio.Protection.IsProtected = true;
                worksheetInicio.Protection.SetPassword(nombre);
                int celda = 1;
                foreach (var item in listaVariable)
                {
                    worksheetInicio.Cells[9, celda].Value = item.NombreVariable;
                    worksheetInicio.Cells[9, celda].Style.Font.Bold = true;
                    worksheetInicio.Cells[9, celda].Style.Font.Size = 12;
                    worksheetInicio.Cells[9, celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheetInicio.Cells[9, celda].Style.Fill.BackgroundColor.SetColor(Constantes.headColorFromHex);
                    worksheetInicio.Cells[9, celda].Style.Font.Color.SetColor(System.Drawing.Color.White);
                    worksheetInicio.Cells[9, celda].Style.Font.Bold = true;
                    worksheetInicio.Cells[9, celda].AutoFitColumns();
                    //worksheetInicio.Cells[9, celda].Style.Locked = false;
                    worksheetInicio.Column(celda).Style.Numberformat.Format = "@";
                    worksheetInicio.Cells[10, celda, detalleRegistroIndicadorFonatel.CantidadFila+10, celda].Value = "";

                    celda++;
                }

                foreach (var item in listaCategoria)
                {
                    worksheetInicio.Cells[9, celda].Value = item.NombreCategoria;
                    worksheetInicio.Cells[9, celda].Style.Font.Bold = true;
                    worksheetInicio.Cells[9, celda].Style.Font.Size = 12;
                    worksheetInicio.Cells[9, celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheetInicio.Cells[9, celda].Style.Fill.BackgroundColor.SetColor(Constantes.headColorFromHex);
                    worksheetInicio.Cells[9, celda].Style.Font.Color.SetColor(System.Drawing.Color.White);
                    worksheetInicio.Cells[9, celda].Style.Font.Bold = true;
                    worksheetInicio.Cells[9, celda].AutoFitColumns();
                   // worksheetInicio.Cells[9, celda].Style.Locked = false;
                    worksheetInicio.Column(celda).Style.Numberformat.Format = "@";

                    celda++;
                }


                worksheetInicio.Cells[10, 1, detalleRegistroIndicadorFonatel.CantidadFila+9, celda-1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheetInicio.Cells[10, 1, detalleRegistroIndicadorFonatel.CantidadFila+9, celda-1].Style.Fill.BackgroundColor.SetColor(Constantes.greenColorFromHex1);
                worksheetInicio.Cells[10, 1, detalleRegistroIndicadorFonatel.CantidadFila+9, celda-1].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                worksheetInicio.Cells[10, 1, detalleRegistroIndicadorFonatel.CantidadFila +9, celda - 1].AutoFitColumns();
                worksheetInicio.Cells[10, 1, detalleRegistroIndicadorFonatel.CantidadFila +9, celda - 1].Style.Locked = false;

                //89482 Llamado de metodo para bitacora al descargar 
                registroIndicadorBL.BitacoraDescargar(registro.objetoRespuesta[0]);

                Response.BinaryWrite(package.GetAsByteArray());
                Response.ContentType = "application/vnd.ms-excel.sheet.macroEnabled.12";
                Response.AddHeader("content-disposition", "attachment;  filename=" + nombre + ".xlsx");
            }

            return new EmptyResult();

        }

        [HttpPost]
        public async Task<string> InsertarRegistroIndicadorVariable(DetalleRegistroIndicadorFonatel ListaDetalleIndicadorValor)
        {

            //Creamos una variable resultado de tipo lista relacion categoria
            RespuestaConsulta<List<DetalleRegistroIndicadorCategoriaValorFonatel>> result = null;

            await Task.Run(() =>
            {
                //Obtenemos el usuario de creacion en la variable user


                //Conectamos con el BL de relacion categoria para insertar y enviamos  la relacion
                result = detalleRegistroIndicadorCategoriaValorFonatelBL.InsertarDatos(ListaDetalleIndicadorValor);

            });

            //Retornamos un Json con el resultado
            return JsonConvert.SerializeObject(result);

        }


        [HttpPost]
        public async Task<string> ConsultaRegistroIndicadorDetalle(DetalleRegistroIndicadorFonatel detalleIndicadorFonatel)
        {
            RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>> result = null;
            await Task.Run(() =>
            {
                result = detalleRegistroIndicadorBL.ObtenerDatos(detalleIndicadorFonatel);

            });
            return JsonConvert.SerializeObject(result);
        }


        /// <summary>
        /// Fecha 21/11/2022
        /// Georgi Mesen Cerdas
        /// Cargar de los datos desde un Excel
        /// </summary>
        [HttpPost]
        public async Task<string> CargarExcel(Object datos, int cantidadFilas)
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
                    result = detalleRegistroIndicadorCategoriaValorFonatelBL.CargarExcel(file, obj, cantidadFilas);
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
                        resultVariable = detalleRegistroIndicadorCategoriaValorFonatelBL.CargarExcelVariable(file, obj, cantidadFilas);
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

        [HttpPost]
        public async Task<string> ActualizarDetalleRegistroIndicadorFonatel(List<DetalleRegistroIndicadorFonatel> listaActDetalleRegistroIndicador)
        {

            //Creamos una variable resultado de tipo lista DetalleRegistroIndicadorFonatel
            RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>> result = null;

            await Task.Run(() =>
            {
                //Conectamos con el BL de relacion categoria para insertar y enviamos  la relacion
                result = detalleRegistroIndicadorBL.ActualizarDetalleRegistroIndicadorFonatelMultiple(listaActDetalleRegistroIndicador);
            });

            //Retornamos un Json con el resultado
            return JsonConvert.SerializeObject(result);

        }

        /// <summary>
        /// Fecha 28/11/2022
        /// Georgi Mesen Cerdas
        /// Metodo para obtener la lista de DetalleRegistroIndicadorCategoriaValorFonatel y DetalleRegistroIndicadorVariableValorFonatel
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public async Task<string> ObtenerListaDetalleRegistroIndicadorValoresFonatel(DetalleRegistroIndicadorFonatel detalle)
        {
            RespuestaConsulta<DetalleRegistroIndicadorFonatel> result = null;

            await Task.Run(() =>
            {
                result = detalleRegistroIndicadorBL.ObtenerListaDetalleRegistroIndicadorValoresFonatel(detalle);
            });
            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Fecha 28/11/2022
        /// Georgi Mesen Cerdas
        /// Metodo para obtener la Actualizar el estado de registro indicador
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public async Task<string> ActualizarRegistroIndicador(RegistroIndicadorFonatel objeto)
        {
            RespuestaConsulta<List<RegistroIndicadorFonatel>> result = null;

            await Task.Run(() =>
            {
                result = registroIndicadorBL.ActualizarElemento(objeto);
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
        public async Task<string> CargaTotalRegistroIndicador(List<DetalleRegistroIndicadorFonatel> lista)
        {

            RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>> result = null;

            RespuestaConsulta<bool> envioCorreo = new RespuestaConsulta<bool>();

            RegistroIndicadorFonatel registroIndicador = new RegistroIndicadorFonatel();

            await Task.Run(() =>
            {
                result = detalleRegistroIndicadorBL.CargaTotalRegistroIndicador(lista);

            })
            .ContinueWith(data =>
            {
                if (result.objetoRespuesta.Count() > 0)
                {
                    var respuestaConsulta = result.objetoRespuesta.FirstOrDefault();

                    registroIndicador.IdSolicitud = respuestaConsulta.IdSolicitud;

                    registroIndicador.idFormularioWeb = respuestaConsulta.idFormularioWeb;

                    envioCorreo = registroIndicadorBL.EnvioCorreoInformante(registroIndicador);

                    envioCorreo = registroIndicadorBL.EnvioCorreoEncargado(registroIndicador);

                }
            });
            return JsonConvert.SerializeObject(result);

        }

        /// <summary>
        /// Autor: Adolfo Cunquero
        /// Fecha: 21/02/2023
        /// Metodo: Metodo para llamar al API de validaciones
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public async Task<string> AplicarReglasValidacion(DetalleRegistroIndicadorFonatel objeto)
        {
            var resultado = await detalleRegistroIndicadorBL.AplicarReglasValidacion(objeto);
            return JsonConvert.SerializeObject(resultado);
        }
    }
}
