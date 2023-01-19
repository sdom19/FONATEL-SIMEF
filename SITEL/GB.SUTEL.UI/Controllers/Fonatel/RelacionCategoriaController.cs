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
    [AuthorizeUserAttribute]
    public class RelacionCategoriaController : Controller
    {
       

        // : RELACION ENTRE CATEGORIAS
        private readonly RelacionCategoriaBL relacionCategoriaBL;
        private readonly DetalleRelacionCategoriaBL detalleRelacionCategoriaBL;


        // : CATEGORIAS DESAGREGACION
        private readonly CategoriasDesagregacionBL categoriasDesagregacionBl;

        private readonly RelacionCategoriaAtributoBL relacionCategoriaAtributoBL;

        public RelacionCategoriaController()
        {

            categoriasDesagregacionBl = new CategoriasDesagregacionBL(EtiquetasViewRelacionCategoria.RelacionCategoria, System.Web.HttpContext.Current.User.Identity.GetUserId());

            relacionCategoriaAtributoBL = new RelacionCategoriaAtributoBL(EtiquetasViewRelacionCategoria.RelacionCategoria, System.Web.HttpContext.Current.User.Identity.GetUserId());
            relacionCategoriaBL = new RelacionCategoriaBL(EtiquetasViewRelacionCategoria.RelacionCategoria, System.Web.HttpContext.Current.User.Identity.GetUserId());
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
            RelacionCategoria model = new RelacionCategoria();
            List <CategoriasDesagregacion> listaCategorias = categoriasDesagregacionBl.ObtenerDatos(new CategoriasDesagregacion()
            {
                idEstado = (int)Constantes.EstadosRegistro.Activo
            }).objetoRespuesta;

            List<RelacionCategoria> listaRelaciones = relacionCategoriaBL
                  .ObtenerDatos(new RelacionCategoria()).objetoRespuesta.ToList();


            ViewBag.ListaCategoriaAtributo =listaCategorias.Where(p=>p.IdTipoCategoria==(int)Constantes.TipoCategoriaEnum.Atributo && p.CantidadDetalleDesagregacion>0)
                .Select(x => new SelectListItem() { Selected = false, Value = x.idCategoria.ToString(), Text = Utilidades.ConcatenadoCombos(x.Codigo, x.NombreCategoria) }).ToList();

            listaCategorias= listaCategorias.Where(p => !listaRelaciones.Any(p2 => p2.CategoriasDesagregacionid.idCategoria==p.idCategoria)).ToList();
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.titulo = EtiquetasViewRelacionCategoria.CrearRelacion;
            }
            else
            {
                ViewBag.titulo = EtiquetasViewRelacionCategoria.EditarRelacion;
                model = listaRelaciones.Where(p=>p.IdRelacionCategoria == Convert.ToInt32(Utilidades.Desencriptar( id))).Single();
                listaCategorias.Add(model.CategoriasDesagregacionid);
            }

            ViewBag.ListaCatergoriaIdUnico = listaCategorias.Where(p => p.IdTipoCategoria == (int)Constantes.TipoCategoriaEnum.IdUnico)

                    .Select(x => new SelectListItem() { Selected = false, Value = x.idCategoria.ToString(), Text = Utilidades.ConcatenadoCombos(x.Codigo, x.NombreCategoria) }).ToList();

            return View(model);
        }




        [HttpGet]
        public ActionResult Detalle(string idRelacionCategoria)
        {
            RelacionCategoria model = relacionCategoriaBL
                   .ObtenerDatos(new RelacionCategoria() { id = idRelacionCategoria }).objetoRespuesta.Single();
            model.RelacionCategoriaId = model.RelacionCategoriaId.Where(x => x.idEstado != (int)Constantes.EstadosRegistro.Eliminado).ToList();
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
                result = relacionCategoriaBL.ValidarExistencia(relacion);
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
                    result = relacionCategoriaBL.ObtenerDatos(new RelacionCategoria());
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
                result = relacionCategoriaBL.InsertarDatos(relacion);

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
                result = relacionCategoriaBL.ActualizarElemento(relacion);

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
        public async Task<string> EliminarRelacionCategoria(RelacionCategoria relacionCategoria)
        {

            RespuestaConsulta<List<RelacionCategoria>> result = null;

            await Task.Run(() =>
            {
                result = relacionCategoriaBL.EliminarElemento(relacionCategoria);

            });
            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Fecha: 27-12-2022
        /// Autor:Francisco Vindas
        /// Metodo que activa la relacion 
        /// </summary>
        /// <param name="relacion"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CambiarEstadoActivado(RelacionCategoria relacion)
        {
            RespuestaConsulta<List<RelacionCategoria>> result = null;

            relacion.idEstado = (int)Constantes.EstadosRegistro.Activo;

            await Task.Run(() =>
            {
                result = relacionCategoriaBL.CambioEstado(relacion);
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

            var relacion = relacionCategoriaBL.ObtenerDatos(new RelacionCategoria() { id = id }).objetoRespuesta.Single();


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

                    int Columna = 1;
                var ArrayCategoriaId=relacion.RelacionCategoriaId.ToArray();
             
                foreach (var sub in relacion.DetalleRelacionCategoria)
                    {
                        Columna += 1;
                        for (int fila =2; fila < relacion.CantidadFilas+2; fila++)
                        {
                            if (Columna==2)
                            {
           
                                worksheetInicio.Cells[fila, 1].Value = ArrayCategoriaId.Length > (fila-2)?ArrayCategoriaId[fila-2].idCategoriaId:string.Empty;
                                worksheetInicio.Cells[fila, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheetInicio.Cells[fila, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                                worksheetInicio.Cells[fila, 1].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                                worksheetInicio.Cells[fila, 1].AutoFitColumns();
                            }


                        List<RelacionCategoriaAtributo> ArrayCategoriaAtributo = ArrayCategoriaId.Length > (fila - 2) ? ArrayCategoriaId[fila - 2].listaCategoriaAtributo : new List<RelacionCategoriaAtributo>();

                        worksheetInicio.Cells[fila, Columna].Value = ArrayCategoriaAtributo.Count > (Columna - 2) ? ArrayCategoriaAtributo.Where(x => x.IdcategoriaAtributo == sub.idCategoriaAtributo).FirstOrDefault().Etiqueta.Replace("N/A", string.Empty) : string.Empty;

                        worksheetInicio.Cells[fila, Columna].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            worksheetInicio.Cells[fila, Columna].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                            worksheetInicio.Cells[fila, Columna].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                            worksheetInicio.Cells[fila, Columna].AutoFitColumns();
                        }
                        
                        worksheetInicio.Cells[1, Columna].Value = sub.CategoriaAtributo.NombreCategoria;
                        worksheetInicio.Cells[1, Columna].Style.Font.Bold = true;
                        worksheetInicio.Cells[1, Columna].Style.Font.Size = 12;
                        worksheetInicio.Cells[1, Columna].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheetInicio.Cells[1, Columna].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(6, 113, 174));
                        worksheetInicio.Cells[1, Columna].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        worksheetInicio.Cells[1, Columna].Style.Font.Bold = true;
                        worksheetInicio.Cells[1, Columna].Style.Font.Size = 12;
                        worksheetInicio.Cells[1, Columna].AutoFitColumns();

                }
                
                Response.BinaryWrite(package.GetAsByteArray());
                Response.ContentType = "application/vnd.ms-excel.sheet.macroEnabled.12";
                Response.AddHeader("content-disposition", "attachment;  filename=" + relacion.Nombre + ".xlsx");
            }

            return new EmptyResult();

        }


        [HttpPost]
        public async Task<string> CargarExcel()
        {

            RespuestaConsulta<List<RelacionCategoria>> result = null;

            await Task.Run(() =>
            {

                if (Request.Files.Count > 0)
                {
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    string fileName = file.FileName;
                    Directory.CreateDirectory(Server.MapPath("~/Simef/"));
                    string path = Path.Combine(Server.MapPath("~/Simef/"), fileName);

                   result= relacionCategoriaAtributoBL.CargarExcel(file);
                    file.SaveAs(path);
                }
            });
            return JsonConvert.SerializeObject(result); 
        }



        #endregion


        #region ASYN DetalleRelacionID


        [HttpPost]
        public async Task<string> EliminarRegistroRelacionId(RelacionCategoriaId relacionCategoriaId)
        {

            RespuestaConsulta<List<RelacionCategoria>> result = null;

            await Task.Run(() =>
            {
                var result= relacionCategoriaBL.CambiarEstado(new RelacionCategoria() {id=relacionCategoriaId.RelacionId, idEstado=(int)EstadosRegistro.EnProceso });
                return result.objetoRespuesta.SingleOrDefault().RelacionCategoriaId.Where(x => x.idCategoriaId == relacionCategoriaId.idCategoriaId);
            }).ContinueWith(data =>{

                if (data.Result!=null)
                {
                    relacionCategoriaId = data.Result.SingleOrDefault();
                    result= relacionCategoriaAtributoBL.EliminarElemento(relacionCategoriaId);
                }
               
            });
            return JsonConvert.SerializeObject(result);
        }




        [HttpPost]
        public async Task<string> InsertarRelacionCategoriaId(RelacionCategoriaId relacionCategoriaId)
        {

            RespuestaConsulta<List<RelacionCategoria>> result = null;
            await Task.Run(() =>
            {
                 result = relacionCategoriaAtributoBL.InsertarDatos(relacionCategoriaId);
         
            });

            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public async Task<string> ObtenerRegistroRelacionId(RelacionCategoriaId relacionCategoriaId)
        {

            RespuestaConsulta<RelacionCategoria> result = null;

            await Task.Run(() =>
            {
                result = relacionCategoriaBL.ObtenerRegistroRelacionId(relacionCategoriaId);
            });
            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public async Task<string> ActualizarRelacionCategoriaId(RelacionCategoriaId relacionCategoriaId)
        {

            RespuestaConsulta<List<RelacionCategoria>> result = null;
            await Task.Run(() =>
            {
                result = relacionCategoriaAtributoBL.ActualizarRelacionCategoriaId(relacionCategoriaId);

            });

            return JsonConvert.SerializeObject(result);
        }

        #endregion

    }
}