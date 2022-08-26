using GB.SIMEF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GB.SIMEF.BL;
using GB.SUTEL.UI.Helpers;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using GB.SIMEF.Resources;

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

            categoriasDesagregacionBl = new CategoriasDesagregacionBL();

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
            ViewBag.ListaCatergorias= categoriasDesagregacionBl.ObtenerDatos(new CategoriasDesagregacion()
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
                ViewBag.ListaCatergoriaValor = new List<SelectListItem>();
                return View();
            }
            else
            {
                RelacionCategoria model = RelacionCategoriaBL.ObtenerDatos(new RelacionCategoria() {id=id })
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
            relacion.idEstado = (int)Constantes.EstadosRegistro.Desactivado;

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
            relacion.idEstado = (int)Constantes.EstadosRegistro.Desactivado;

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

            RespuestaConsulta < List < string >> result = new RespuestaConsulta<List<string>>();

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


        #endregion

    }
}
