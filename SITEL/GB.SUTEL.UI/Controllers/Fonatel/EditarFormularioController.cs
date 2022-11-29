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
    public class EditarFormularioController : Controller
    {


        #region Variables Públicas del controller
        private readonly EditarRegistroIndicadorFonatelBL EditarRegistroIndicadorBL;


        private string modulo = string.Empty;
        private string user = string.Empty;

        #endregion

        public EditarFormularioController()
        {
            modulo = EtiquetasViewReglasValidacion.ReglasValidacion;
            user = System.Web.HttpContext.Current.User.Identity.GetUserId();
            EditarRegistroIndicadorBL = new EditarRegistroIndicadorFonatelBL(modulo, user);

        }


        #region Metodos de Vista

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Edit(string id)
        {
            return View();
        }

        #endregion



        #region Metodos de controlador


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
                ExcelWorksheet worksheetInicio = package.Workbook.Worksheets.Add("Prueba Excel");

                worksheetInicio.Cells["A1"].Value = "Categoría Atributo";
                worksheetInicio.Cells["B1"].Value = "Detalle Relación Atributo";

                Response.BinaryWrite(package.GetAsByteArray());
                Response.ContentType = "application/vnd.ms-excel.sheet.macroEnabled.12";
                Response.AddHeader("content-disposition", "attachment;  filename=" + nombre + ".xlsx");
            }

            return new EmptyResult();

        }

        /// <summary>
        /// Francisco Vindas
        /// 29-11-2022
        /// Metodo para obtener la lista de Registros de Indicador a Editar
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public async Task<string> ObtenerListaRegistroIndicador()
        {
            RespuestaConsulta<List<RegistroIndicadorFonatel>> result = null;
            await Task.Run(() =>
            {
                result = EditarRegistroIndicadorBL.ObtenerDatos(new RegistroIndicadorFonatel());

            });
            return JsonConvert.SerializeObject(result);
        }

        #endregion




    }

}
