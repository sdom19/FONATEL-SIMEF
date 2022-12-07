using System;
using System.Collections.Generic;
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
    public class EditarFormularioController : Controller
    {


        #region Variables Públicas del controller
        private readonly EditarRegistroIndicadorFonatelBL EditarRegistroIndicadorBL;
        private readonly EditarDetalleRegistroIndicadorFonatelBL DetalleRegistroIndicadorBL;
        private readonly EditarDetalleRegistroIndicadorCategoriaValorFonatelBL DetalleRegistroIndicadorCategoriaValorFonatelBL;

        #endregion

        public EditarFormularioController()
        {
            EditarRegistroIndicadorBL = new EditarRegistroIndicadorFonatelBL(EtiquetasViewRegistroIndicadorFonatel.RegistroIndicador, System.Web.HttpContext.Current.User.Identity.GetUserId());
            DetalleRegistroIndicadorBL = new EditarDetalleRegistroIndicadorFonatelBL(EtiquetasViewRegistroIndicadorFonatel.RegistroIndicador, System.Web.HttpContext.Current.User.Identity.GetUserId());
            DetalleRegistroIndicadorCategoriaValorFonatelBL = new EditarDetalleRegistroIndicadorCategoriaValorFonatelBL(EtiquetasViewRegistroIndicadorFonatel.RegistroIndicador, System.Web.HttpContext.Current.User.Identity.GetUserId());

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
        public async Task<string> ObtenerListaDetalleRegistroIndicadorCategoriaValorFonatel(DetalleRegistroIndicadorFonatel detalle)
        {
            RespuestaConsulta<List<DetalleRegistroIndicadorCategoriaValorFonatel>> result = null;

            await Task.Run(() =>
            {
                result = DetalleRegistroIndicadorCategoriaValorFonatelBL.ObtenerDatosCategoriaValor(detalle);
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
