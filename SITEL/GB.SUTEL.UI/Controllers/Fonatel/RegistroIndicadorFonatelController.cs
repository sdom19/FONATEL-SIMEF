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
                FormularioId=idFormulario,
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

            detalleRegistroIndicadorFonatel.IdFormularioString = lista[1];
            detalleRegistroIndicadorFonatel.IdIndicadorString = lista[2];
            detalleRegistroIndicadorFonatel.IdSolicitudString = lista[0];
            detalleRegistroIndicadorFonatel.CantidadFilas= Convert.ToInt32(lista[3]);

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
                ExcelWorksheet worksheetInicio = package.Workbook.Worksheets.Add(result.objetoRespuesta[0].TituloHojas);
                int celda = 1;
                foreach (var item in listaVariable)
                {
                    worksheetInicio.Cells[1, celda].Value = item.NombreVariable;
                    worksheetInicio.Cells[1, celda].Style.Font.Bold = true;
                    worksheetInicio.Cells[1, celda].Style.Font.Size = 12;
                    worksheetInicio.Cells[1, celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheetInicio.Cells[1, celda].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(64, 152, 166));
                    worksheetInicio.Cells[1, celda].Style.Font.Color.SetColor(System.Drawing.Color.White);
                    worksheetInicio.Cells[1, celda].Style.Font.Bold = true;
                    worksheetInicio.Cells[1, celda].Style.Font.Size = 12;
                    worksheetInicio.Cells[1, celda].AutoFitColumns();

                    worksheetInicio.Cells[2, celda, detalleRegistroIndicadorFonatel.CantidadFilas+1, celda].Value = "1";

                    celda++;
                }

                foreach (var item in listaCategoria)
                {
                    worksheetInicio.Cells[1, celda].Value = item.NombreCategoria;
                    worksheetInicio.Cells[1, celda].Style.Font.Bold = true;
                    worksheetInicio.Cells[1, celda].Style.Font.Size = 12;
                    worksheetInicio.Cells[1, celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheetInicio.Cells[1, celda].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(6, 113, 174));
                    worksheetInicio.Cells[1, celda].Style.Font.Color.SetColor(System.Drawing.Color.White);
                    worksheetInicio.Cells[1, celda].Style.Font.Bold = true;
                    worksheetInicio.Cells[1, celda].Style.Font.Size = 12;
                    worksheetInicio.Cells[1, celda].AutoFitColumns();

                    celda++;
                }


                worksheetInicio.Cells[2, 1, detalleRegistroIndicadorFonatel.CantidadFilas+1, celda-1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheetInicio.Cells[2, 1, detalleRegistroIndicadorFonatel.CantidadFilas+1, celda-1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                worksheetInicio.Cells[2, 1, detalleRegistroIndicadorFonatel.CantidadFilas+1, celda-1].Style.Font.Color.SetColor(System.Drawing.Color.Black);

                Response.BinaryWrite(package.GetAsByteArray());
                Response.ContentType = "application/vnd.ms-excel.sheet.macroEnabled.12";
                Response.AddHeader("content-disposition", "attachment;  filename=" + nombre + ".xlsx");
            }

            return new EmptyResult();

        }




        [HttpPost]
        public async Task<string> InsertarRegistroIndicadorVariable(List<DetalleRegistroIndicadorCategoriaValorFonatel> ListaDetalleIndicadorValor)
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
        public async Task<string> CargarExcel(Object datos,int cantidadFilas)
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
                    result = detalleRegistroIndicadorCategoriaValorFonatelBL.CargarExcel(file, obj, cantidadFilas);

                });
                
                file.SaveAs(path);
            }
            return JsonConvert.SerializeObject(result);
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
        /// Metodo para obtener la lista de DetalleRegistroIndicadorCategoriaValorFonatel
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public async Task<string> ObtenerListaDetalleRegistroIndicadorCategoriaValorFonatel(DetalleRegistroIndicadorCategoriaValorFonatel detalle)
        {
            RespuestaConsulta<List<DetalleRegistroIndicadorCategoriaValorFonatel>> result = null;

            await Task.Run(() =>
            {
                result = detalleRegistroIndicadorCategoriaValorFonatelBL.ObtenerDatos(detalle);
            });
            return JsonConvert.SerializeObject(result);
        }

    }
}
