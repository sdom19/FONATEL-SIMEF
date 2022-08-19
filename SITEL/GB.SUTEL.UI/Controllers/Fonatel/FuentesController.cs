using GB.SIMEF.BL;
using GB.SIMEF.Entities;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GB.SUTEL.UI.Controllers.Fonatel
{
    public class FuentesController : Controller
    {


        FuentesRegistroBL FuenteBL;
        string user;

        public FuentesController()
        {
            FuenteBL = new FuentesRegistroBL();
        }


        // GET: FuentesRegistro

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        // GET: FuentesRegistro/Details/5
        [HttpGet]
        public ActionResult Detalle(int id)
        {
            return View();
        }

        // GET: FuentesRegistro/Create
        [HttpGet]
        public ActionResult Create(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View();
            }else
            {
                FuentesRegistro fuente = 
                    FuenteBL.ObtenerDatos(new FuentesRegistro() { id = id }).objetoRespuesta.Single();
                return View(fuente);
            }
            
        }

        public ActionResult Deatlle(int id)
        {
            return View();
        }







        #region Métodos de ASYNC Fuentes


        /// <summary>
        /// Fecha 04-08-2022
        /// Michael Hernández Cordero
        /// Obtiene datos para la table de categorías INDEX
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<string> ObtenerListaFuentes()
        {
            RespuestaConsulta<List<FuentesRegistro>> result = null;
            await Task.Run(() =>
            {
                result = FuenteBL.ObtenerDatos(new FuentesRegistro());
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

        public async Task<string> CambiarEstadoFuentes(FuentesRegistro fuente)
        {
            user = User.Identity.GetUserId();
            RespuestaConsulta<List<FuentesRegistro>> result = null;
            await Task.Run(() =>
            {
                fuente.UsuarioModificacion = user;
                result = FuenteBL.CambioEstado(fuente);
            });

            return JsonConvert.SerializeObject(result);
        }

        #endregion




    }
}
