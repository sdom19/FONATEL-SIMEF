using GB.SUTEL.BL.Mantenimientos;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GB.SUTEL.Entities;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.UI.Helpers;
using System.Text;
using GB.SUTEL.UI.Recursos.Utilidades;
using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using System.IO;
using System.Web.Configuration;
using GB.SUTEL.Resources;

namespace GB.SUTEL.UI.Controllers
{
    public class DetalleAgrupacionController : BaseController
    {
        #region atributos
        DetalleAgrupacionBL detalleBL;
        AgrupacionBL agrupacionBL;
        OperadorBL operadorBL;
        Funcion func = new Funcion();
        #endregion

        #region Constructor
        public DetalleAgrupacionController()
        {
            detalleBL = new DetalleAgrupacionBL(AppContext);
            agrupacionBL = new AgrupacionBL(AppContext);
            operadorBL = new OperadorBL(AppContext);
        }


        #endregion

        #region Vistas


        // GET: Nivel
        [AuthorizeUserAttribute]
        public ActionResult Index()
        {


            Respuesta<DetalleAgrupacionViewModels> respuesta = new Respuesta<DetalleAgrupacionViewModels>();
            respuesta.objObjeto = new DetalleAgrupacionViewModels();
            respuesta.objObjeto.listadoDetalleAgrupacion = detalleBL.gListar().objObjeto;
            if (respuesta.blnIndicadorTransaccion)
            {

                //carga de ViewBag
                cargarAgrupacionesEnBag();
                cargarOperadoresEnBag();
                string user;
                user = User.Identity.GetUserId();
                try
                {
                    func._index(user, "Detalle de Agrupación", "Detalle Agrupación Mantenimiento");
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Exception caught.", e);
                }
                return View(respuesta.objObjeto);


            }


            return View();
        }
        [AuthorizeUserAttribute]
        [HttpGet]
        public ActionResult _table()
        {
            try
            {
                ViewBag.TerminosBusqueda_AGRUPACION = string.Empty;
                ViewBag.TerminosBusqueda_OPERADOR = string.Empty;
                ViewBag.TerminosBusqueda_DESCRIPCION = string.Empty;

                Respuesta<DetalleAgrupacionViewModels> respuesta = new Respuesta<DetalleAgrupacionViewModels>();
                respuesta.objObjeto = new DetalleAgrupacionViewModels();
                respuesta.objObjeto.listadoDetalleAgrupacion = detalleBL.gListar().objObjeto;
                if (respuesta.blnIndicadorTransaccion)
                {
                    return PartialView(respuesta.objObjeto);

                }
                return PartialView();
            }
            catch (CustomException)
            {
                return PartialView();
            }
            catch (Exception)
            {
                return PartialView();
            }
        }

        [AuthorizeUserAttribute]
        [HttpPost]
        public ActionResult _table(string Operador, string Agrupacion, string DESCDETALLE)
        {
            try
            {


                ViewBag.TerminosBusqueda_AGRUPACION = Agrupacion;
                ViewBag.TerminosBusqueda_OPERADOR = Operador;
                ViewBag.TerminosBusqueda_DESCRIPCION = DESCDETALLE;

                Respuesta<DetalleAgrupacionViewModels> respuesta = new Respuesta<DetalleAgrupacionViewModels>();
                respuesta.objObjeto = new DetalleAgrupacionViewModels();

                if (!(string.IsNullOrEmpty(Operador)) || !(string.IsNullOrEmpty(Agrupacion)) || !(string.IsNullOrEmpty(DESCDETALLE)))
                {
                    respuesta.objObjeto.listadoDetalleAgrupacion = detalleBL.gListarPorFiltros(Operador, Agrupacion, DESCDETALLE).objObjeto;
                }
                else
                {
                    respuesta.objObjeto.listadoDetalleAgrupacion = detalleBL.gListar().objObjeto;
                }
                if (respuesta.blnIndicadorTransaccion)
                {
                    return PartialView(respuesta.objObjeto);

                }
                return PartialView();
            }
            catch (CustomException)
            {
                return PartialView();
            }
            catch (Exception)
            {
                return PartialView();
            }
        }


        [AuthorizeUserAttribute]
        [HttpPost]
        public string Crear(DetalleAgrupacionViewModels poDetalle)
        {
            Respuesta<DetalleAgrupacion> respuesta = new Respuesta<DetalleAgrupacion>();
            JSONResult<DetalleAgrupacion> jsonRespuesta = new JSONResult<DetalleAgrupacion>();

            if (!ModelState.IsValid)
            {
                return jsonRespuesta.toJSON();
            }

            try
            {
                respuesta = detalleBL.gAgregar(poDetalle.itemDetalleAgrupacion);
                if (respuesta.blnIndicadorTransaccion)
                {
                    func.detalleagrupacionbit(ActionsBinnacle.Crear, respuesta.objObjeto, null);
                    jsonRespuesta.data = respuesta.objObjeto;

                }
                else
                {
                    jsonRespuesta.ok = false;
                }

                jsonRespuesta.strMensaje = respuesta.strMensaje;
                return jsonRespuesta.toJSON();
            }
            catch
            {
                jsonRespuesta.ok = false;
                return jsonRespuesta.toJSON();
            }
        }



        [AuthorizeUserAttribute]
        [HttpPost]
        public string Editar(DetalleAgrupacionViewModels poDetalle)
        {
            Respuesta<DetalleAgrupacion> respuesta = new Respuesta<DetalleAgrupacion>();
            JSONResult<DetalleAgrupacion> jsonRespuesta = new JSONResult<DetalleAgrupacion>();
            DetalleAgrupacionBL detalleAuxBL = new DetalleAgrupacionBL(this.AppContext);
            try
            {
                DetalleAgrupacion detalleAnterior = detalleAuxBL.gConsultar(poDetalle.itemDetalleAgrupacion.IdDetalleAgrupacion).objObjeto;

                respuesta = detalleBL.gEditar(poDetalle.itemDetalleAgrupacion);

                if (respuesta.blnIndicadorTransaccion)
                {
                    jsonRespuesta.data = respuesta.objObjeto;
                    func.detalleagrupacionbit(ActionsBinnacle.Editar, respuesta.objObjeto, detalleAnterior);
                }
                else
                {
                    jsonRespuesta.ok = false;
                }

                jsonRespuesta.strMensaje = respuesta.strMensaje;
                return jsonRespuesta.toJSON();
            }
            catch
            {
                jsonRespuesta.ok = false;
                return jsonRespuesta.toJSON();
            }
        }


        [AuthorizeUserAttribute]
        [HttpPost]
        public string Eliminar(int ItemEliminarIdDetalleAgrupacion)
        {
            Respuesta<DetalleAgrupacion> respuesta = new Respuesta<DetalleAgrupacion>();
            JSONResult<DetalleAgrupacion> jsonRespuesta = new JSONResult<DetalleAgrupacion>();
            try
            {


                respuesta = detalleBL.gEliminar(ItemEliminarIdDetalleAgrupacion);
                if (respuesta.blnIndicadorTransaccion)
                {
                    jsonRespuesta.data = respuesta.objObjeto;
                    func.detalleagrupacionbit(ActionsBinnacle.Borrar, null, respuesta.objObjeto);
                }
                else
                {
                    jsonRespuesta.ok = false;

                }

                jsonRespuesta.strMensaje = respuesta.strMensaje;
                return jsonRespuesta.toJSON();
            }
            catch (Exception ex)
            {
                jsonRespuesta.ok = false;

                return jsonRespuesta.toJSON();
            }
        }


        /// <summary>
        /// Generar plantilla de excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeUserAttribute]
        public ActionResult GenerarExcel(string detalleAgrupacionNombreExcel)
        {
            JSONResult<string> jsonRespuesta = new JSONResult<string>();
            try
            {
                MemoryStream stream = new MemoryStream();

                using (ExcelPackage package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheetInicio = package.Workbook.Worksheets.Add("Página inicial");
                    ExcelWorksheet worksheet1 = package.Workbook.Worksheets.Add("Agrupación");
                    ExcelWorksheet worksheet2 = package.Workbook.Worksheets.Add("Operador");

                    worksheetInicio.Cells["A1"].Value = "Agrupación";
                    worksheetInicio.Cells["B1"].Value = "Operador";
                    worksheetInicio.Cells["C1"].Value = "Descripción";
                    worksheetInicio.Cells["A1:C1"].Style.Font.Bold = true;
                    worksheetInicio.Cells["A1:C1"].Style.Font.Size = 12;
                    worksheetInicio.Cells["A1:C1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheetInicio.Cells["A1:C1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(6,113,174));
                    worksheetInicio.Cells["A1:C1"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                    worksheetInicio.Cells["A1:C1"].AutoFitColumns();

                    var listAgrupacion = agrupacionBL.gObtenerAgrupaciones().objObjeto.Select(e => new { Código = e.IdAgrupacion, Descripción = e.DescAgrupacion }).OrderBy(o => o.Código).ToList();
                    var listOperador = operadorBL.ConsultarTodosParaDetalleAgrupacion().objObjeto.Select(e => new { Código = e.IdOperador, Descripción = e.NombreOperador }).OrderBy(o => o.Código).ToList();
                   
                    worksheet1.Cells["A1"].LoadFromCollection(listAgrupacion, true);
                    worksheet1.Cells["A1:B1"].Style.Font.Bold = true;
                    worksheet1.Cells["A1:B1"].Style.Font.Size = 12;
                    worksheet1.Cells["A1:B1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet1.Cells["A1:B1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(6, 113, 174));
                    worksheet1.Cells["A1:B1"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                    worksheet1.Cells["A1:B1"].AutoFitColumns();

                    worksheet2.Cells["A1"].LoadFromCollection(listOperador, true);
                    worksheet2.Cells["A1:B1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet2.Cells["A1:B1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(6, 113, 174));
                    worksheet2.Cells["A1:B1"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                    worksheet2.Cells["A1:B1"].Style.Font.Bold = true;
                    worksheet2.Cells["A1:B1"].Style.Font.Size = 12;
                    worksheet2.Cells["A1:B1"].AutoFitColumns();

                    Response.BinaryWrite(package.GetAsByteArray());
                    Response.ContentType = "application/vnd.ms-excel.sheet.macroEnabled.12";
                    Response.AddHeader("content-disposition", "attachment;  filename=" + detalleAgrupacionNombreExcel + ".xlsx");

                    return new EmptyResult();
                }
            }

            catch (Exception ex)
            {
                return RedirectToAction("Error", "DetalleAgrupacion");
            }
        }

        /// <summary>
        /// Cargar detalle agrupación desde Excel
        /// </summary>
        [HttpPost]
        public string CargarExcel(HttpPostedFileBase selectedFile)
        {
            JSONResult<string> jsonRespuesta = new JSONResult<string>();
            try
            {
                var fileName = Path.GetFileName(selectedFile.FileName);
                string pantalla = CBitacora.GetValueAsString(CBitacora.BitacoraOpciones.detalleAgrupacion);

                Respuesta<List<DetalleAgrupacion>> respuesta = new Respuesta<List<DetalleAgrupacion>>();

                respuesta = detalleBL.gCrearDetalleDesdeExcel(fileName, selectedFile.InputStream);

                if (!respuesta.blnIndicadorTransaccion)
                {
                    jsonRespuesta.strMensaje = respuesta.strMensaje;
                }

                if (respuesta.objObjeto != null && respuesta.objObjeto.Count >= 1)
                {
                    CBitacora.gRegistrarBitacora(AppContext, Convert.ToInt32(ActionsBinnacle.Crear.GetHashCode()), pantalla,
                                                Mensajes.Crear + pantalla, string.Format(Mensajes.RegistrosInsertados, respuesta.objObjeto.Count), "");
                    jsonRespuesta.data = respuesta.objObjeto.Count().ToString();
                }
            }
            catch (Exception ex)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = Mensajes.ErrorCarga;
            }

            return jsonRespuesta.toJSON();
        }

        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }

        #endregion

        #region Metodos Privados

        private void cargarAgrupacionesEnBag()
        {

            List<Agrupacion> listaAgrupacion = new List<Agrupacion>();

            listaAgrupacion = agrupacionBL.gObtenerAgrupaciones().objObjeto;

            listaAgrupacion.Insert(0, new Agrupacion() { IdAgrupacion = 0, DescAgrupacion = "<Seleccione>" });

            ViewBag.listaAgrupaciones = new SelectList(listaAgrupacion, "IdAgrupacion", "DescAgrupacion").ToList();
        }

        private void cargarOperadoresEnBag()
        {

            List<Operador> listaOperadores = new List<Operador>();

            listaOperadores = operadorBL.ConsultarTodosParaDetalleAgrupacion().objObjeto;

            listaOperadores.Insert(0, new Operador() { IdOperador = "", NombreOperador = "<Seleccione>" });

            ViewBag.listaOperadores = new SelectList(listaOperadores, "IdOperador", "NombreOperador").ToList();
        }

        #endregion


    }
}
