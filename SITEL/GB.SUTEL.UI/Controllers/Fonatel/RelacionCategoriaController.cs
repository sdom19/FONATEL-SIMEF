﻿using GB.SIMEF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GB.SIMEF.BL;
using GB.SIMEF.Entities;
using GB.SUTEL.UI.Helpers;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using GB.SIMEF.Resources;

namespace GB.SUTEL.UI.Controllers.Fonatel
{
    public class RelacionCategoriaController : Controller
    {
        private readonly string user;

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
        public ActionResult Create()
        {
            ViewBag.ListaCatergoriaIdUnico = categoriasDesagregacionBl.ObtenerDatos(new CategoriasDesagregacion()
            {
                //IdTipoCategoria = (int)Constantes.TipoCategoriaEnum.IdUnico

            }).objetoRespuesta;
            return View();
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
            }
            );
            return JsonConvert.SerializeObject(result);
        }


        [HttpPost]
        public async Task<string> InsertarRelacionCategoria(RelacionCategoria relacion)
        {
            RespuestaConsulta<List<RelacionCategoria>> result = null;
            await Task.Run(() =>
            {
                //relacion.UsuarioModificacion = user;
                //result = RelacionCategoriaBL.CambioEstado(relacion);
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
        public async Task<string> ObtenerListaCategorias(int select)
        {
            RespuestaConsulta <List<CategoriasDesagregacion>> result = null;

            await Task.Run(() =>
            {
                result = categoriasDesagregacionBl.ObtenerDatos(new CategoriasDesagregacion() { idCategoria = select });

            });
            return JsonConvert.SerializeObject(result);
        }


        #endregion

    }
}
