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

        #endregion


        public CategoriasDesagregacionController()
        {
            categoriaBL = new CategoriasDesagregacionBL(EtiquetasViewCategorias.Categorias, System.Web.HttpContext.Current.User.Identity.GetUserId());
            categoriaDetalleBL = new DetalleCategoriasTextoBL(EtiquetasViewCategorias.Categorias, System.Web.HttpContext.Current.User.Identity.GetUserId());
            TipoCategoriaBL = new TipoCategoriaBL(EtiquetasViewCategorias.Categorias, System.Web.HttpContext.Current.User.Identity.GetUserId());
            TipoDetalleCategoriaBL = new TipoDetalleCategoriaBL(EtiquetasViewCategorias.Categorias, System.Web.HttpContext.Current.User.Identity.GetUserId());

        }

        #region Eventos de la Página
        [AuthorizeUserAttribute]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        // GET: CategoriasDesagregacion/Details/5
        [AuthorizeUserAttribute]
        [HttpGet]
        [ConsultasFonatelFilter]
        public ActionResult Detalle(string idCategoria)
        {
            if (string.IsNullOrEmpty(idCategoria))
            {
                return View("Index");
            } else
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
        [AuthorizeUserAttribute]
        [ConsultasFonatelFilter]
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
                if (modo == (int)Constantes.Accion.Clonar)
                {
                    ViewBag.titulo = EtiquetasViewCategorias.Clonar;
                    objCategoria.Codigo = string.Empty;
                    objCategoria.id = string.Empty;
                    objCategoria.NombreCategoria = string.Empty;
                }
                else
                {
                    ViewBag.titulo = EtiquetasViewCategorias.EditarCategoria;
                }

            }
            else
            {
                ViewBag.titulo = EtiquetasViewCategorias.CrearCategoria;
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



        [AuthorizeUserAttribute]

        [HttpGet]
        public ActionResult DescargarExcel(string id)
        {
            var categoria = categoriaBL
                    .ObtenerDatos(new CategoriasDesagregacion() { id = id }).objetoRespuesta.Single();

            categoria.DetalleCategoriaTexto = categoriaDetalleBL.ObtenerDatos
                (new DetalleCategoriaTexto() { idCategoria = categoria.idCategoria }).objetoRespuesta;
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
        [ConsultasFonatelFilter]

        public async Task<string> CambiarEstadoCategoria(CategoriasDesagregacion categoria)
        {
            RespuestaConsulta<List<CategoriasDesagregacion>> result = null;
            await Task.Run(() =>
            {
                int nuevoestado = categoria.idEstado;
                categoria = categoriaBL.ObtenerDatos(
                    new CategoriasDesagregacion() {id=categoria.id }).objetoRespuesta.Single();
                categoria.idEstado = nuevoestado;
                return categoria;

            }).ContinueWith(data =>
            {

                result = categoriaBL.CambioEstado(data.Result);
            }
            );
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
        [ConsultasFonatelFilter]
        public async Task<string> InsertarCategoria(CategoriasDesagregacion categoria)
        {

            RespuestaConsulta<List<CategoriasDesagregacion>> result = null;
            await Task.Run(() =>
            {
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
        [ConsultasFonatelFilter]
        public async Task<string> EditarCategoria(CategoriasDesagregacion categoria)
        {
            RespuestaConsulta<List<CategoriasDesagregacion>> result = null;
            await Task.Run(() =>
            {
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
        [ConsultasFonatelFilter]
        public async Task<string> ClonarCategoria(CategoriasDesagregacion categoria)
        {
            RespuestaConsulta<List<CategoriasDesagregacion>> result = null;
            await Task.Run(() =>
            {
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
        [ConsultasFonatelFilter]
        public async Task<String> CargaExcel()
        {
            RespuestaConsulta<List<DetalleCategoriaTexto>> resultado = null;
            await Task.Run(() =>
            {
                
                string ruta = Utilidades.RutaCarpeta(ConfigurationManager.AppSettings["rutaCarpetaSimef"], EtiquetasViewCategorias.Categorias);
                if (Request.Files.Count > 0)
                {
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    string fileName = file.FileName;
                    Directory.CreateDirectory(ruta);
                    string path = Path.Combine(ruta, fileName);
                    resultado = categoriaDetalleBL.CargarExcel(file);
                    file.SaveAs(path);
                }
            });


            return JsonConvert.SerializeObject(resultado);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns></returns>
        [HttpPost]
        [ConsultasFonatelFilter]
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
        [ConsultasFonatelFilter]
        public async Task<string> InsertarCategoriasDetalle(DetalleCategoriaTexto DetalleCategoria)
        {
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
        [ConsultasFonatelFilter]
        public async Task<string> ModificaCategoriasDetalle(DetalleCategoriaTexto detalleCategoria)
        {
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
        [ConsultasFonatelFilter]
        public async Task<string> EliminarCategoriasDetalle( DetalleCategoriaTexto DetalleCategoriaTexto)
        {
            RespuestaConsulta<List<DetalleCategoriaTexto>> result = null;
            await Task.Run(() =>
            {
                DetalleCategoriaTexto.usuario = User.Identity.GetUserId();
                result = categoriaDetalleBL.EliminarElemento(DetalleCategoriaTexto);

            }).ContinueWith(data =>
            {
                var categoria = categoriaBL.ObtenerDatos(new CategoriasDesagregacion() {id= DetalleCategoriaTexto.categoriaid }).objetoRespuesta.Single();
                categoria.idEstado = (int)Constantes.EstadosRegistro.EnProceso;
                return categoria;

            }).ContinueWith(data =>
            {

               categoriaBL.CambioEstado(data.Result);
            }
            );
            return JsonConvert.SerializeObject(result);




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
        [HttpPost]
        public async Task<string> ObtenerCategoriasDetalle(DetalleCategoriaTexto DetalleCategoriaTexto)
        {
            RespuestaConsulta<List<DetalleCategoriaTexto>> result = null;
            await Task.Run(() =>
            {
                result = categoriaDetalleBL.ObtenerDatos(DetalleCategoriaTexto);

            });
            return JsonConvert.SerializeObject(result);
        }



        /// <summary>
        /// Cambiar estado a Finalizado Categorías
        /// </summary>
        /// <param name="indicador"></param>
        /// <returns></returns>
        [HttpPost]
        [ConsultasFonatelFilter]
        public async Task<string> CambiarEstadoFinalizado(CategoriasDesagregacion categoria)
        {
            RespuestaConsulta<List<CategoriasDesagregacion>> result = null;
            await Task.Run(() =>
            {
                categoria=  categoriaBL.ObtenerDatos(categoria).objetoRespuesta.Single();
                categoria.idEstado = (int)Constantes.EstadosRegistro.Activo;
                return categoria;

            }).ContinueWith(data =>
            {

                result = categoriaBL.CambioEstado(data.Result);
            }
            );
            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Fecha 10-11-2022
        /// Georgi Mesen Cerdas
        /// Obtiene datos de categorias para relaciones categoria
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<string> ListaCategoriasParaRelacion()
        {
            RespuestaConsulta<List<CategoriasDesagregacion>> result = null;
            await Task.Run(() =>
            {
                result = categoriaBL.ListaCategoriasParaRelacion(new CategoriasDesagregacion());
            });

            return JsonConvert.SerializeObject(result);


        }

        /// <summary>
        /// Fecha 5-01-2023
        /// Jeaustin Chaves Sánchez
        /// Obtiene datos de categoria buscando por ID
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string ObtenerCategoria(string pid)
        {
            
            RespuestaConsulta<List<CategoriasDesagregacion>> result = null;
            
            result = categoriaBL.ObtenerDatos(new CategoriasDesagregacion() { id = pid });
              

            return JsonConvert.SerializeObject(result);


        }

        #endregion

    }
}
