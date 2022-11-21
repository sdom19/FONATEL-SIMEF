using System;
using System.Collections.Generic;
using System.Configuration;
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
using static GB.SIMEF.Resources.Constantes;

namespace GB.SUTEL.UI.Controllers.Fonatel
{
    public class RelacionCategoriaController : Controller
    {
       

        // : RELACION ENTRE CATEGORIAS
        private readonly RelacionCategoriaBL RelacionCategoriaBL;
        private readonly DetalleRelacionCategoriaBL detalleRelacionCategoriaBL;


        // : CATEGORIAS DESAGREGACION
        private readonly CategoriasDesagregacionBL categoriasDesagregacionBl;
        private readonly DetalleCategoriasTextoBL DetalleCategoriasTextoBL;

        public RelacionCategoriaController()
        {

            categoriasDesagregacionBl = new CategoriasDesagregacionBL(EtiquetasViewRelacionCategoria.RelacionCategoria, System.Web.HttpContext.Current.User.Identity.GetUserId());

            DetalleCategoriasTextoBL = new DetalleCategoriasTextoBL(EtiquetasViewRelacionCategoria.RelacionCategoria, System.Web.HttpContext.Current.User.Identity.GetUserId());
            RelacionCategoriaBL = new RelacionCategoriaBL(EtiquetasViewRelacionCategoria.RelacionCategoria, System.Web.HttpContext.Current.User.Identity.GetUserId());
            detalleRelacionCategoriaBL = new DetalleRelacionCategoriaBL(EtiquetasViewRelacionCategoria.RelacionCategoria, System.Web.HttpContext.Current.User.Identity.GetUserId());
          

        }

        #region Eventos de la pagina 

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }




        [HttpGet]
        public ActionResult Create(string id)
        {
            List<CategoriasDesagregacion> listaCategorias = categoriasDesagregacionBl.ObtenerDatos(new CategoriasDesagregacion()
            {
                idEstado = (int)Constantes.EstadosRegistro.Activo
            }).objetoRespuesta;


            ViewBag.ListaCatergoriaIdUnico = listaCategorias.Where( p=>p.IdTipoCategoria == (int)Constantes.TipoCategoriaEnum.IdUnico)
                .Select(x => new SelectListItem() { Selected = false, Value = x.idCategoria.ToString(), Text = Utilidades.ConcatenadoCombos(x.Codigo, x.NombreCategoria) }).ToList();

            ViewBag.ListaCategoriaAtributo =listaCategorias.Where(p=>p.IdTipoCategoria==(int)Constantes.TipoCategoriaEnum.Atributo && p.CantidadDetalleDesagregacion>0)
                .Select(x => new SelectListItem() { Selected = false, Value = x.idCategoria.ToString(), Text = Utilidades.ConcatenadoCombos(x.Codigo, x.NombreCategoria) }).ToList();

            if (string.IsNullOrEmpty(id))
            {

                ViewBag.titulo = EtiquetasViewRelacionCategoria.CrearRelacion;
                return View(new RelacionCategoria());
            }
            else
            {
                ViewBag.titulo = EtiquetasViewRelacionCategoria.EditarRelacion;

                RelacionCategoria model = RelacionCategoriaBL
                    .ObtenerDatos(new RelacionCategoria() { id = id }).objetoRespuesta.Single();
                return View(model);
            }
        }




        [HttpGet]
        public ActionResult Detalle(string idRelacionCategoria)
        {
            RelacionCategoria model = RelacionCategoriaBL
                   .ObtenerDatos(new RelacionCategoria() { id = idRelacionCategoria }).objetoRespuesta.Single();
            return View(model);

        }


            #endregion

            #region Metodos de ASYNC Relacion Categoria



            /// Fecha 16/09/2022
            /// Francisco Vindas Ruiz
            /// Validar existencia en Indicadores
            /// </summary>
            /// <param name="categoria"></param>
            /// <returns></returns>
            [HttpPost]
        public async Task<string> ValidarRelacion(RelacionCategoria relacion)
        {
            RespuestaConsulta<List<string>> result = null;

            await Task.Run(() =>
            {
                result = RelacionCategoriaBL.ValidarExistencia(relacion);
            });

            return JsonConvert.SerializeObject(result);
        }



        /// <summary>
        /// Fecha 10/08/2022
        /// Francisco Vindas Ruiz
        /// Metodo para obtener la lista Relacion Categorias en el Index 
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<string> ObtenerListaRelacionCategoria()
        {

                RespuestaConsulta<List<RelacionCategoria>> result = null;

                await Task.Run(() =>
                {
                    result = RelacionCategoriaBL.ObtenerDatos(new RelacionCategoria());
                });
                return JsonConvert.SerializeObject(result);
        }



        public async Task<string> InsertarRelacionCategoria(RelacionCategoria relacion)
        {

            //Identificamos el id del usuario
      

            //Creamos una variable resultado de tipo lista relacion categoria
            RespuestaConsulta<List<RelacionCategoria>> result = null;

            await Task.Run(() =>
            {
                //Obtenemos el usuario de creacion en la variable user
               

                //Conectamos con el BL de relacion categoria para insertar y enviamos  la relacion
                result = RelacionCategoriaBL.InsertarDatos(relacion);

            });

            //Retornamos un Json con el resultado
            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Fecha 22-08-2022
        /// Francisco Vindas
        /// Metodo para editar los relacion categorias
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> EditarRelacionCategoria(RelacionCategoria relacion)
        {

            //Identificamos el id del usuario
        

            //Creamos una variable resultado de tipo lista relacion categoria
            RespuestaConsulta<List<RelacionCategoria>> result = null;

            await Task.Run(() =>
            {

                //Conectamos con el BL de relacion categoria para insertar y enviamos  la relacion
                result = RelacionCategoriaBL.ActualizarElemento(relacion);

            });

            //Retornamos un Json con el resultado
            return JsonConvert.SerializeObject(result);
        }

        /// <summary> 
        /// 23/08/2022
        /// Francisco Vindas Ruiz
        /// Metodo para eliminar relacion categoria
        /// </summary>
        /// <param name="idRelacionCategoria></param>
        /// <returns>JSON</returns>
        [HttpPost]
        public async Task<string> EliminarRelacionCategoria(string idRelacionCategoria)
        {

            RespuestaConsulta<List<RelacionCategoria>> result = null;

            await Task.Run(() =>
            {
                result = RelacionCategoriaBL.EliminarElemento(new RelacionCategoria()
                {

                    id = idRelacionCategoria,

                });

            });
            return JsonConvert.SerializeObject(result);
        }



        #endregion


        #region Metodos ASYN DetalleRelacion Categoria
        [HttpPost]
        public async Task<string> InsertarDetalleRelacion(DetalleRelacionCategoria DetalleRelacionCategoria)
        {
            RespuestaConsulta<List<RelacionCategoria>> result = null;

            await Task.Run(() =>
            {
                RelacionCategoria relacionCategoria = new RelacionCategoria();
                relacionCategoria.DetalleRelacionCategoria.Add(DetalleRelacionCategoria);
                result = detalleRelacionCategoriaBL.InsertarDatos(relacionCategoria);
            });
            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public async Task<string> EliminarDetalleRelacion(DetalleRelacionCategoria DetalleRelacionCategoria)
        {
            RespuestaConsulta<List<RelacionCategoria>> result = null;

            await Task.Run(() =>
            {
                RelacionCategoria relacionCategoria = new RelacionCategoria();
                relacionCategoria.DetalleRelacionCategoria.Add(DetalleRelacionCategoria);
                result = detalleRelacionCategoriaBL.EliminarElemento(relacionCategoria);
            });
            return JsonConvert.SerializeObject(result);
        }


        #endregion



        #region Metodos ASYN Excel

        [HttpGet]
        public ActionResult DescargarExcel(string id)
        {

            var relacion = RelacionCategoriaBL.ObtenerDatos(new RelacionCategoria() { id = id }).objetoRespuesta.Single();


            MemoryStream stream = new MemoryStream();

            using (ExcelPackage package = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheetInicio = package.Workbook.Worksheets.Add(relacion.Codigo);

                worksheetInicio.Cells["A1"].Value = relacion.CategoriasDesagregacionid.NombreCategoria;
                worksheetInicio.Cells["A1:A1"].Style.Font.Bold = true;
                worksheetInicio.Cells["A1:A1"].Style.Font.Size = 12;
                worksheetInicio.Cells["A1:A1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheetInicio.Cells["A1:A1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(6, 113, 174));
                worksheetInicio.Cells["A1:A1"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                worksheetInicio.Cells["A1:A1"].Style.Font.Bold = true;
                worksheetInicio.Cells["A1:A1"].Style.Font.Size = 12;
                worksheetInicio.Cells["A1:A1"].AutoFitColumns();

                    int celda = 1;

                    foreach (var sub in relacion.DetalleRelacionCategoria)
                    {
                    celda += 1;
                        for (int i =2; i < relacion.CantidadFilas+2; i++)
                        {
                            if (celda==2)
                            {
                            worksheetInicio.Cells[i, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            worksheetInicio.Cells[i, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                            worksheetInicio.Cells[i, 1].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                            worksheetInicio.Cells[i, 1].AutoFitColumns();
                            }
                            worksheetInicio.Cells[i, celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            worksheetInicio.Cells[i, celda].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                            worksheetInicio.Cells[i, celda].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                            worksheetInicio.Cells[i, celda].AutoFitColumns();
                        }
                        
                        worksheetInicio.Cells[1, celda].Value = sub.CategoriaAtributo.NombreCategoria;
                        worksheetInicio.Cells[1, celda].Style.Font.Bold = true;
                        worksheetInicio.Cells[1, celda].Style.Font.Size = 12;
                        worksheetInicio.Cells[1, celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheetInicio.Cells[1, celda].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(6, 113, 174));
                        worksheetInicio.Cells[1, celda].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        worksheetInicio.Cells[1, celda].Style.Font.Bold = true;
                        worksheetInicio.Cells[1, celda].Style.Font.Size = 12;
                        worksheetInicio.Cells[1, celda].AutoFitColumns();

                }
                
                Response.BinaryWrite(package.GetAsByteArray());
                Response.ContentType = "application/vnd.ms-excel.sheet.macroEnabled.12";
                Response.AddHeader("content-disposition", "attachment;  filename=" + relacion.Nombre + ".xlsx");
            }

            return new EmptyResult();

        }



        #endregion


    }
}