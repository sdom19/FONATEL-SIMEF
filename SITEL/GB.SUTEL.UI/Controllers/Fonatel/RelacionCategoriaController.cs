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
    public class RelacionCategoriaController : Controller
    {
        string user;

        // : RELACION ENTRE CATEGORIAS

        private readonly RelacionCategoriaBL RelacionCategoriaBL;
        private readonly DetalleRelacionCategoriaBL DetalleRelacionCategoriaBL;

        // : CATEGORIAS DESAGREGACION

        private readonly CategoriasDesagregacionBL categoriasDesagregacionBl;
        private readonly DetalleCategoriasTextoBL DetalleCategoriasTextoBL;

        public RelacionCategoriaController()
        {

            categoriasDesagregacionBl = new CategoriasDesagregacionBL(EtiquetasViewReglasValidacion.ReglasValidacion, System.Web.HttpContext.Current.User.Identity.GetUserId());

            DetalleCategoriasTextoBL = new DetalleCategoriasTextoBL();

            RelacionCategoriaBL = new RelacionCategoriaBL();

            DetalleRelacionCategoriaBL = new DetalleRelacionCategoriaBL();

        }

        #region Eventos de la pagina 

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// Francisco Vindas Ruiz
        /// 24-08-2022
        /// Obtener la lista de detalles
        /// </summary>
        /// <param name="idRelacionCategoria"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detalle(string idRelacionCategoria)
        {

            //CATEGORIAS DE TIPO ATRIBUTO
            ViewBag.ListaCatergorias = categoriasDesagregacionBl.ObtenerDatos(new CategoriasDesagregacion()
            {
                IdTipoCategoria = (int)Constantes.TipoCategoriaEnum.Atributo,
                idEstado = (int)Constantes.EstadosRegistro.Activo

            }).objetoRespuesta;


            if (string.IsNullOrEmpty(idRelacionCategoria))
            {
                ViewBag.ListaCatergoriaValor = new List<SelectListItem>();
                return View();
            }
            else
            {
                RelacionCategoria model = RelacionCategoriaBL.ObtenerDatos(new RelacionCategoria() { id = idRelacionCategoria })
                    .objetoRespuesta.Single();

                return View(model);
            }


        }

        [HttpGet]
        public ActionResult Create(string id, int? modo)
        {
            ViewBag.Modo = modo.ToString();

            ViewBag.ListaCatergoriaIdUnico = categoriasDesagregacionBl.ObtenerDatos(new CategoriasDesagregacion()
            {
                IdTipoCategoria = (int)Constantes.TipoCategoriaEnum.IdUnico,
                idEstado = (int)Constantes.EstadosRegistro.Activo

            }).objetoRespuesta;

            if (string.IsNullOrEmpty(id))
            {

                ViewBag.titulo = EtiquetasViewRelacionCategoria.CrearRelacion;

                ViewBag.ListaCatergoriaValor = new List<SelectListItem>();

                return View();
            }
            else
            {
                ViewBag.titulo = EtiquetasViewRelacionCategoria.EditarRelacion;

                RelacionCategoria model = RelacionCategoriaBL.ObtenerDatos(new RelacionCategoria() { id = id })
                    .objetoRespuesta.Single();

                var categoria = categoriasDesagregacionBl.ObtenerDatos(new CategoriasDesagregacion()
                {
                    idCategoria = model.idCategoria
                }).objetoRespuesta.Single();

                var listavalores = RelacionCategoriaBL.ObtenerListaCategoria(categoria).objetoRespuesta
                    .Select(x => new SelectListItem() { Selected = false, Value = x, Text = x }).ToList();
                listavalores.Add(new SelectListItem() { Value = model.idCategoriaValor, Text = model.idCategoriaValor, Selected = true });
                ViewBag.ListaCatergoriaValor = listavalores;
                return View(model);
            }
        }




        #endregion

        #region Metodos de ASYNC Relacion Categoria

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


        /// <summary>
        /// Fecha 19-08-2022
        /// Francisco Vindas
        /// Metodo para insertar los relacion categorias
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> InsertarRelacionCategoria(RelacionCategoria relacion)
        {

            //Identificamos el id del usuario
            user = User.Identity.GetUserId();
            relacion.idEstado = (int)Constantes.EstadosRegistro.EnProceso;

            //Creamos una variable resultado de tipo lista relacion categoria
            RespuestaConsulta<List<RelacionCategoria>> result = null;

            await Task.Run(() =>
            {
                //Obtenemos el usuario de creacion en la variable user
                relacion.UsuarioCreacion = user;

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
            user = User.Identity.GetUserId();
            relacion.idEstado = (int)Constantes.EstadosRegistro.EnProceso;

            //Creamos una variable resultado de tipo lista relacion categoria
            RespuestaConsulta<List<RelacionCategoria>> result = null;

            await Task.Run(() =>
            {
                //Obtenemos el usuario de creacion en la variable user
                relacion.UsuarioCreacion = user;

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
            user = User.Identity.GetUserId();

            RespuestaConsulta<List<RelacionCategoria>> result = null;

            await Task.Run(() =>
            {
                result = RelacionCategoriaBL.EliminarElemento(new RelacionCategoria()
                {

                    id = idRelacionCategoria,
                    UsuarioModificacion = user

                });

            });
            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Fecha 17-08-2022
        /// Francisco Vindas
        /// Obtiene datos para la table de categorías Detalle Detalle
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<string> ObtenerDetalleDesagregacionId(int selected)
        {
            //ACA TAMBIEN PUEDO AGREGAR VALIDACIONES DE ERRORES CONTROLADOS

            RespuestaConsulta<List<string>> result = new RespuestaConsulta<List<string>>();

            await Task.Run(() =>
            {
                var categoria = categoriasDesagregacionBl.ObtenerDatos(new CategoriasDesagregacion()
                {
                    idCategoria = selected
                }).objetoRespuesta.Single();

                result = RelacionCategoriaBL.ObtenerListaCategoria(categoria);
            });
            return JsonConvert.SerializeObject(result);
        }

        #endregion

        #region Metodos ASYNC Detalle Relacion Categoria

        /// <summary>
        /// Fecha 24-08-2022
        /// Francisco Vindas Ruiz
        /// Obtiene los datos en la vista principal en el Detalle Relacion Categoria 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ObtenerListaCategoriasDetalle(string IdRelacionCategoria)
        {
            RespuestaConsulta<List<DetalleRelacionCategoria>> result = null;

            await Task.Run(() =>
            {
                result = DetalleRelacionCategoriaBL.ObtenerDatos(new DetalleRelacionCategoria()
                { relacionid = IdRelacionCategoria });

            });
            return JsonConvert.SerializeObject(result);
        }


        /// <summary>
        /// Fecha 17-08-2022
        /// Francisco Vindas
        /// Carga el combo box categoria atributo
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<string> CargarListaDetalleDesagregacion(int selected)
        {
            RespuestaConsulta<List<string>> result = new RespuestaConsulta<List<string>>();

            await Task.Run(() =>
            {
                var categoria = categoriasDesagregacionBl.ObtenerDatos(new CategoriasDesagregacion()
                {
                    idCategoria = selected
                }).objetoRespuesta.Single();

                result = RelacionCategoriaBL.ObtenerListaDetalleCategoria(categoria);
            });
            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Inserta un detalle relacion entre categorias
        /// 25/08/2022
        /// Francisco Vindas Ruiz
        /// </summary>
        /// <param name="detalleRelacion"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<string> InsertarDetalleRelacion(DetalleRelacionCategoria relacion)
        {
            user = User.Identity.GetUserId();
            relacion.usuario = user;
            RespuestaConsulta<List<DetalleRelacionCategoria>> result = null;
            await Task.Run(() =>
            {
                result = DetalleRelacionCategoriaBL.InsertarDatos(relacion);
            });

            return JsonConvert.SerializeObject(result);
        }


        /// <summary>
        /// Inserta un detalle relacion entre categorias
        /// 29/08/2022
        /// Francisco Vindas Ruiz
        /// </summary>
        /// <param name="detalleRelacion"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<string> ModificarDetalleRelacion(DetalleRelacionCategoria relacion)
        {
            user = User.Identity.GetUserId();
            relacion.usuario = user;
            RespuestaConsulta<List<DetalleRelacionCategoria>> result = null;
            await Task.Run(() =>
            {
                result = DetalleRelacionCategoriaBL.ActualizarElemento(relacion);
            });

            return JsonConvert.SerializeObject(result);
        }


        /// <summary>
        /// Francisco Vindas Ruiz 
        /// 29/08/2022
        /// Pase el estado a falso para el eliminado del detalle 
        /// </summary>
        /// <param name="idDetalleRelacion"></param>
        /// <returns>JSON</returns>
        [HttpPost]
        public async Task<string> EliminarDetalleRelacion(string idDetalleRelacionCategoria)
        {
            user = User.Identity.GetUserId();
            RespuestaConsulta<List<DetalleRelacionCategoria>> result = null;
            await Task.Run(() =>
            {
                result = DetalleRelacionCategoriaBL.EliminarElemento(new DetalleRelacionCategoria()
                {
                    id = idDetalleRelacionCategoria,
                    usuario = user
                });

            });

            return JsonConvert.SerializeObject(result);
        }



        #endregion

        #region Metodo para descargar y cargar excel

        /// <summary>
        /// Francisco Vindas
        /// 02/09/2022
        /// Metodo para descargar en un Excel los detalles Relacion Categoria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet]
        public ActionResult DescargarExcel(string id)
        {
            user = User.Identity.GetUserId();

            var relacion = RelacionCategoriaBL.ObtenerDatos(new RelacionCategoria() { id = id }).objetoRespuesta.Single();

            relacion.DetalleRelacionCategoria = DetalleRelacionCategoriaBL.ObtenerDatos(new DetalleRelacionCategoria()
            {
                IdRelacionCategoria = relacion.idRelacionCategoria
            }).objetoRespuesta;

            MemoryStream stream = new MemoryStream();

            using (ExcelPackage package = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheetInicio = package.Workbook.Worksheets.Add(relacion.Codigo);

                worksheetInicio.Cells["A1"].LoadFromCollection(relacion.DetalleRelacionCategoria

                    .Select(i => new { i.CategoriaDesagracion.NombreCategoria, i.CategoriaAtributoValor }), true);

                worksheetInicio.Cells["A1"].Value = "Categoría Atributo";
                worksheetInicio.Cells["B1"].Value = "Detalle Relación Atributo";

                worksheetInicio.Cells["A1:B1"].Style.Font.Bold = true;
                worksheetInicio.Cells["A1:B1"].Style.Font.Size = 12;
                worksheetInicio.Cells["A1:B1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheetInicio.Cells["A1:B1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(6, 113, 174));
                worksheetInicio.Cells["A1:B1"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                worksheetInicio.Cells["A1:B1"].Style.Font.Bold = true;
                worksheetInicio.Cells["A1:B1"].Style.Font.Size = 12;
                worksheetInicio.Cells["A1:B1"].AutoFitColumns();
                for (int i = 0; i < relacion.CantidadCategoria; i++)
                {
                    string celdas = string.Format("A{0}:B{0}", i + 2);


                    worksheetInicio.Cells[celdas].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheetInicio.Cells[celdas].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    worksheetInicio.Cells[celdas].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    worksheetInicio.Cells[celdas].AutoFitColumns();
                }
                Response.BinaryWrite(package.GetAsByteArray());
                Response.ContentType = "application/vnd.ms-excel.sheet.macroEnabled.12";
                Response.AddHeader("content-disposition", "attachment;  filename=" + relacion.Nombre + ".xlsx");
            }

            return new EmptyResult();

        }

        /// <summary>
        /// Fecha 02/09/2022
        /// Francisco Vindas Ruiz
        /// Cargar de los detalles desde un Excel
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

                DetalleRelacionCategoriaBL.CargarExcel(file);
                file.SaveAs(path);
            }
        }

        #endregion
    }
}