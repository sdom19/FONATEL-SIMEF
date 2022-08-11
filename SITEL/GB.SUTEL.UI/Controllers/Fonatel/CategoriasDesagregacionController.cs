using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GB.SIMEF.BL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using GB.SUTEL.UI.Helpers;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace GB.SUTEL.UI.Controllers.Fonatel
{
    //[AuthorizeUserAttribute]
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
        /// <param name="categoria"></param>
        /// <returns></returns>
        [HttpPost]

        public async Task<string> CambiarEstadoCategoria(CategoriasDesagregacion categoria)
        {
            user = User.Identity.GetUserId();
            RespuestaConsulta<List<CategoriasDesagregacion>> result = null;
            await Task.Run(() =>
            {
                categoria.UsuarioModificacion = user;
                result = categoriaBL.CambioEstado(categoria); 
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
        /// <param name="detalleCategoria"></param>
        /// <returns></returns>

        [HttpPost]

        public async Task<string> InsertarCategoriasDetalle(DetalleCategoriaTexto detalleCategoria)
        {
            user = User.Identity.GetUserId();
            detalleCategoria.usuario = user;
            RespuestaConsulta<List<DetalleCategoriaTexto>> result = null;
            await Task.Run(() =>
            {
                result = categoriaDetalleBL.InsertarDatos(detalleCategoria);
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
        public async Task<string> EliminarCategoriasDetalle(string idDetalleCategoria)
        {
            user = User.Identity.GetUserId();
            RespuestaConsulta<List<DetalleCategoriaTexto>> result = null;
            await Task.Run(() =>
            {
                result = categoriaDetalleBL.EliminarElemento(new DetalleCategoriaTexto()
                {
                    id = idDetalleCategoria,
                    usuario = user
                });

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
