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

        // GET: CategoriasDesagregacion

        private readonly RelacionCategoriaBL RelacionCategoriaBL;
        private readonly CategoriasDesagregacionBL categoriasDesagregacionBl;
        

        public RelacionCategoriaController()
        {
            //user = User.Identity.GetUserId(); 
            categoriasDesagregacionBl = new CategoriasDesagregacionBL();
            RelacionCategoriaBL = new RelacionCategoriaBL();


        }

        #region Eventos de la pagina 

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        // GET: CategoriasDesagregacion/Details/5
        [HttpGet]
        public ActionResult Detalle(int id)
        { 
            return View();
        }


        [HttpGet]
        public ActionResult Create(string id)
        {

            ViewBag.ListaCatergoriaIdUnico = categoriasDesagregacionBl.ObtenerDatos(new CategoriasDesagregacion()
            {
                IdTipoCategoria = (int)Constantes.TipoCategoriaEnum.IdUnico,
                idEstado=(int)Constantes.EstadosRegistro.Activo

            }).objetoRespuesta;

           


            if (string.IsNullOrEmpty(id))
            {
                return View();
            }
            else
            {
                RelacionCategoria model = RelacionCategoriaBL.ObtenerDatos(new RelacionCategoria() {id=id })
                    .objetoRespuesta.Single() ;

              
                return View(model);
            }           
        }


        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        #endregion

        #region Metodos de ASYNC

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
        /// Fecha 17-08-2022
        /// Francisco Vindas
        /// Obtiene datos para la table de categorías Detalle Detalle
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<string> ObtenerDetalleDesagregacionId(int select)
        {
            List<string> result = new List<string>() ;

            await Task.Run(() =>
            {
                var categoria = categoriasDesagregacionBl.ObtenerDatos(new CategoriasDesagregacion()
                {
                    idCategoria = select
                }).objetoRespuesta.Single();

                result = RelacionCategoriaBL.ObtenerListaCategoria(categoria);
            });
            return JsonConvert.SerializeObject(result);
        }

        #endregion

    }
}
