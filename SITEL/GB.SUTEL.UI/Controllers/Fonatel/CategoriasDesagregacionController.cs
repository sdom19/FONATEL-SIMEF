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
    [AuthorizeUserAttribute]
    public class CategoriasDesagregacionController : Controller
    {

        #region Variables Públicas del controller
        private readonly CategoriasDesagregacionBL categoriaBL;

        private readonly DetalleCategoriasTextoBL categoriaDetalleBL;

        private readonly TipoCategoriaBL TipoCategoriaBL;

        private readonly TipoDetalleCategoriaBL TipoDetalleCategoriaBL;

        string user;

        #endregion


        public CategoriasDesagregacionController()
        {
            categoriaBL = new CategoriasDesagregacionBL();
            categoriaDetalleBL = new DetalleCategoriasTextoBL();
            TipoCategoriaBL = new TipoCategoriaBL();
            TipoDetalleCategoriaBL = new TipoDetalleCategoriaBL();

        }

        #region Eventos de la Página
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        // GET: CategoriasDesagregacion/Details/5
        [HttpGet]
        public ActionResult Detalle(string idCategoria)
        {
            if (string.IsNullOrEmpty( idCategoria))
            {
                return View("Index");
            }else
            {
                CategoriasDesagregacion objCategoria = new CategoriasDesagregacion();
                if (!string.IsNullOrEmpty(idCategoria))
                {
                    objCategoria.id = idCategoria;
                    objCategoria = categoriaBL.ObtenerDatos(objCategoria).objetoRespuesta.SingleOrDefault();
                }
                return View(objCategoria);
            }
           
        }

        [HttpGet]
        public ActionResult Create(string id, int? modo)
        {
            ViewBag.TipoCategoria = TipoCategoriaBL.ObtenerDatos(new TipoCategoria() { })
                .objetoRespuesta;
            ViewBag.TipoDetalleCategoria = TipoDetalleCategoriaBL.ObtenerDatos(new TipoDetalleCategoria() { })
               .objetoRespuesta;
            ViewBag.Modo = modo.ToString();

            CategoriasDesagregacion objCategoria = new CategoriasDesagregacion();
            if (!string.IsNullOrEmpty(id))
            {
                objCategoria.id = id;
                objCategoria = categoriaBL.ObtenerDatos(objCategoria).objetoRespuesta.SingleOrDefault();
                if (modo==(int)Constantes.Accion.Clonar)
                {
                    objCategoria.Codigo = string.Empty;
                    objCategoria.id = string.Empty;
                }

            }
            return View(objCategoria);
           
        }



        /// <summary>
        /// Genera el detalle total de los atributos
        /// 11/08/2022
        /// Michael Hernández Cordero
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>




         [HttpGet]
        public ActionResult DescargarExcel(string id)
        {
            user = User.Identity.GetUserId();

            var categoria = categoriaBL
                    .ObtenerDatos(new CategoriasDesagregacion() { id = id }).objetoRespuesta.Single();

            categoria.DetalleCategoriaTexto = categoriaDetalleBL.ObtenerDatos
                (new DetalleCategoriaTexto() { idCategoria=categoria.idCategoria }).objetoRespuesta;
            MemoryStream stream = new MemoryStream();

            using (ExcelPackage package = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheetInicio = package.Workbook.Worksheets.Add(categoria.Codigo);
                

                worksheetInicio.Cells["A1"].LoadFromCollection(categoria.DetalleCategoriaTexto
                   
                    .Select(i => new { i.Codigo, i.Etiqueta }), true);
                worksheetInicio.Cells["A1"].Value = "Código";
                worksheetInicio.Cells["B1"].Value = "Etiqueta";

                worksheetInicio.Cells["A1:B1"].Style.Font.Bold = true;
                worksheetInicio.Cells["A1:B1"].Style.Font.Size = 12;
                worksheetInicio.Cells["A1:B1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheetInicio.Cells["A1:B1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(6, 113, 174));
                worksheetInicio.Cells["A1:B1"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                worksheetInicio.Cells["A1:B1"].Style.Font.Bold = true;
                worksheetInicio.Cells["A1:B1"].Style.Font.Size = 12;
                worksheetInicio.Cells["A1:B1"].AutoFitColumns();
                for (int i = 0; i < categoria.CantidadDetalleDesagregacion; i++)
                {
                    string celdas = string.Format("A{0}:B{0}", i + 2);


                    worksheetInicio.Cells[celdas].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheetInicio.Cells[celdas].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    worksheetInicio.Cells[celdas].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    worksheetInicio.Cells[celdas].AutoFitColumns();
                }
                Response.BinaryWrite(package.GetAsByteArray());
                Response.ContentType = "application/vnd.ms-excel.sheet.macroEnabled.12";
                Response.AddHeader("content-disposition", "attachment;  filename=" + categoria.NombreCategoria + ".xlsx");

                return new EmptyResult();

            }

        }




        #endregion

        #region Métodos de ASYNC Categoria


      /// <summary>
      /// Fecha 04-08-2022
      /// Michael Hernández Cordero
      /// Obtiene datos para la table de categorías INDEX
      /// </summary>
      /// <returns></returns>

      [HttpGet]
        public async Task<string> ObtenerListaCategorias()
        {
            RespuestaConsulta<List<CategoriasDesagregacion>> result = null;
            await Task.Run(() =>
            {
               result = categoriaBL.ObtenerDatos(new CategoriasDesagregacion());
            });

            return JsonConvert.SerializeObject(result);
          
 
        }

        /// <summary>
        /// Fecha 10/08/2022
        /// Michael Hernández Cordero
        /// Cambio el estado de registro a desactivado y activado 
        /// </summary>
        /// <param name="Categoria"></param>
        /// <returns></returns>
        [HttpPost]

        public async Task<string> CambiarEstadoCategoria(CategoriasDesagregacion Categoria)
        {
            user = User.Identity.GetUserId();
            RespuestaConsulta<List<CategoriasDesagregacion>> result = null;
            await Task.Run(() =>
            {
                Categoria.UsuarioModificacion = user;
                result = categoriaBL.CambioEstado(Categoria); 
            });

            return JsonConvert.SerializeObject(result);
        }
        /// <summary>
        /// Fecha 10/08/2022
        /// Michael Hernández Cordero
        /// Insertar Categoría  
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns></returns>

        [HttpPost]

        public async Task<string> InsertarCategoria(CategoriasDesagregacion categoria)
        {
            user = User.Identity.GetUserId();
            RespuestaConsulta<List<CategoriasDesagregacion>> result = null;
            await Task.Run(() =>
            {
                categoria.UsuarioCreacion = user;
                result = categoriaBL.InsertarDatos(categoria);
            });

            return JsonConvert.SerializeObject(result);
        }


        /// <summary>
        /// Fecha 10/08/2022
        /// Michael Hernández Cordero
        /// Editar Categoría  
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns></returns>

        [HttpPost]

        public async Task<string> EditarCategoria(CategoriasDesagregacion categoria)
        {
            user = User.Identity.GetUserId();
            RespuestaConsulta<List<CategoriasDesagregacion>> result = null;
            await Task.Run(() =>
            {
                categoria.UsuarioCreacion = user;
                result = categoriaBL.ActualizarElemento(categoria);
            });

            return JsonConvert.SerializeObject(result);
        }




        /// <summary>
        /// Fecha 10/08/2022
        /// Michael Hernández Cordero
        /// Clonar Categoría  
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns></returns>

        [HttpPost]

        public async Task<string> ClonarCategoria(CategoriasDesagregacion categoria)
        {
            user = User.Identity.GetUserId();
            RespuestaConsulta<List<CategoriasDesagregacion>> result = null;
            await Task.Run(() =>
            {
                categoria.UsuarioCreacion = user;
                result = categoriaBL.ClonarDatos(categoria);
            });

            return JsonConvert.SerializeObject(result);
        }


        /// <summary>
        /// Fecha 16-08-2022
        /// Michael Hernández Cordero
        /// Realiza la carga de información por medio de un excel
        /// </summary>

        [HttpPost]
        public void CargaExcel()
        {
            if (Request.Files.Count > 0)
            {       
               HttpFileCollectionBase files = Request.Files;
               HttpPostedFileBase file = files[0];
               string fileName = file.FileName;  
               Directory.CreateDirectory(Server.MapPath("~/Simef/"));
               string path = Path.Combine(Server.MapPath("~/Simef/"), fileName);

               categoriaDetalleBL.CargarExcel(file);
               file.SaveAs(path);             
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ValidarCategoria(CategoriasDesagregacion categoria)
        {
            RespuestaConsulta<List<string>> result = null;
            await Task.Run(() =>
            {
                result = categoriaBL.ValidarExistencia(categoria);
            });

            return JsonConvert.SerializeObject(result);
        }



        #endregion


        #region Metodos Async DetalleCateriaTexto






        /// <summary>
        /// Fecha 04-08-2022
        /// Michael Hernández Cordero
        /// Obtiene datos para la table de categorías Detalle Detalle
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerListaCategoriasDetalle(string idCategoria)
        {
            RespuestaConsulta<List<DetalleCategoriaTexto>> result = null;
            await Task.Run(() =>
            {
                result = categoriaDetalleBL.ObtenerDatos(new DetalleCategoriaTexto() { categoriaid = idCategoria });

            });
            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Inserta un detalle para los atributops tipo texto
        /// 09/08/2022
        /// Michael Hernández
        /// </summary>
        /// <param name="DetalleCategoria"></param>
        /// <returns></returns>

        [HttpPost]

        public async Task<string> InsertarCategoriasDetalle(DetalleCategoriaTexto DetalleCategoria)
        {
            user = User.Identity.GetUserId();
            DetalleCategoria.usuario = user;
            RespuestaConsulta<List<DetalleCategoriaTexto>> result = null;
            await Task.Run(() =>
            {
                result = categoriaDetalleBL.InsertarDatos(DetalleCategoria);
            });
          
            return JsonConvert.SerializeObject(result);
        }


        /// <summary>
        /// Modifica un detalle para los atributos tipo texto
        /// 09/08/2022
        /// Michael Hernández
        /// </summary>
        /// <param name="detalleCategoria"></param>
        /// <returns></returns>

        [HttpPost]

        public async Task<string> ModificaCategoriasDetalle(DetalleCategoriaTexto detalleCategoria)
        {
            user = User.Identity.GetUserId();
            detalleCategoria.usuario = user;
            RespuestaConsulta<List<DetalleCategoriaTexto>> result = null;
            await Task.Run(() =>
            {
                result = categoriaDetalleBL.ActualizarElemento(detalleCategoria);
            });

            return JsonConvert.SerializeObject(result);
        }



        /// <summary>
        /// Establece la variable estado en false, estado eliminado
        /// 09/08/2022
        /// Michael Hernández Codero
        /// </summary>
        /// <param name="idDetalleCategoria"></param>
        /// <returns>JSON</returns>
        [HttpPost]
        public async Task<string> EliminarCategoriasDetalle( DetalleCategoriaTexto DetalleCategoriaTexto)
        {
            RespuestaConsulta<List<DetalleCategoriaTexto>> result = null;
            await Task.Run(() =>
            {
                DetalleCategoriaTexto.usuario = User.Identity.GetUserId();
                result = categoriaDetalleBL.EliminarElemento(DetalleCategoriaTexto);

            });
            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Fecha 10/08/2022
        /// Michael Hernández Cordero 
        /// Obtiene la lista de elementos con base al parámetro encriptado
        /// , Js filtra el elemento a 1
        /// </summary>
        /// <param name="idCategoriaDetalle"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerCategoriasDetalle(string idCategoriaDetalle)
        {
            RespuestaConsulta<List<DetalleCategoriaTexto>> result = null;
            await Task.Run(() =>
            {
                result = categoriaDetalleBL.ObtenerDatos(new DetalleCategoriaTexto()
                { id = idCategoriaDetalle });

            });
            return JsonConvert.SerializeObject(result);
        }


        #endregion

    }
}
