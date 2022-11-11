using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
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
    public class RegistroIndicadorFonatelController  : Controller
    {
        // GET: RegistroIndicadorFonatel


       private readonly RegistroIndicadorFonatelBL registroIndicadorBL;
        private readonly DetalleRegistroIndicadorFonatelBL detalleRegistroIndicadorBL;

        public RegistroIndicadorFonatelController()
        {

            registroIndicadorBL = new RegistroIndicadorFonatelBL(EtiquetasViewRegistroIndicadorFonatel.RegistroIndicador, System.Web.HttpContext.Current.User.Identity.GetUserId());
            detalleRegistroIndicadorBL = new DetalleRegistroIndicadorFonatelBL(EtiquetasViewRegistroIndicadorFonatel.RegistroIndicador, System.Web.HttpContext.Current.User.Identity.GetUserId());
        }

        #region Metodos de las vistas
        [HttpGet]
        public ActionResult Index()
        {
            RespuestaConsulta<List<RegistroIndicadorFonatel>> model = registroIndicadorBL.ObtenerDatos(new RegistroIndicadorFonatel()
            {
                RangoFecha = true
            });
            return View(model.objetoRespuesta);
        }




        [HttpGet]
        public ActionResult Create(string idSolicitud, string idFormulario)
        {
            RespuestaConsulta<List<RegistroIndicadorFonatel>> model = registroIndicadorBL.ObtenerDatos(new RegistroIndicadorFonatel()
            {
                IdFormularioString=idFormulario,
                IdSolicitudString=idSolicitud,
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
        public ActionResult DescargarExcel()
        {
            string nombre = "PruebaExcel";

            MemoryStream stream = new MemoryStream();

            using (ExcelPackage package = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheetInicio = package.Workbook.Worksheets.Add("REGISTRO INDICADOR");

                worksheetInicio.Cells["A1"].Value = "Categoría Atributo";
                worksheetInicio.Cells["B1"].Value = "Detalle Relación Atributo";

                Response.BinaryWrite(package.GetAsByteArray());
                Response.ContentType = "application/vnd.ms-excel.sheet.macroEnabled.12";
                Response.AddHeader("content-disposition", "attachment;  filename=" + nombre + ".xlsx");
            }

            return new EmptyResult();

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



    }
}
