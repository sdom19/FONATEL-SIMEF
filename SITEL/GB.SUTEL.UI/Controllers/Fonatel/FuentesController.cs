using GB.SIMEF.BL;
using GB.SIMEF.Entities;
using GB.SUTEL.UI.Helpers;
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
    [AuthorizeUserAttribute]
    public class FuentesController : Controller
    {


        FuentesRegistroBL FuenteBL;
        FuentesRegistroDestinatariosBL FuenteDestinatariosBL;
        string user;

        public FuentesController()
        {
            FuenteBL = new FuentesRegistroBL();
            FuenteDestinatariosBL = new FuentesRegistroDestinatariosBL();
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
                return View(new FuentesRegistro());
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

        public async Task<string> EliminarFuente(FuentesRegistro fuente)
        {
            user = User.Identity.GetUserId();
            RespuestaConsulta<List<FuentesRegistro>> result = null;
            await Task.Run(() =>
            {
          
                fuente.UsuarioModificacion = user;
                result = FuenteBL.EliminarElemento(fuente);
            });

            return JsonConvert.SerializeObject(result);
        }




        [HttpPost]
        public async Task<string> AgregarFuente(FuentesRegistro fuente)
        {
            user = User.Identity.GetUserId();
            RespuestaConsulta<List<FuentesRegistro>> result = null;
            await Task.Run(() =>
            {
                if (String.IsNullOrEmpty(fuente.id))
                {
                    fuente.UsuarioCreacion = user;
                    result = FuenteBL.InsertarDatos(fuente);
                }
                else
                {
                    fuente.UsuarioModificacion = user;
                    result = FuenteBL.ActualizarElemento(fuente);
                }
           
            });

            return JsonConvert.SerializeObject(result);
        }


        /// <summary>
        /// Activar Fuente Proceso de finalizar 
        /// </summary>
        /// <param name="fuente"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<string> ActivarFuente(FuentesRegistro fuente)
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



        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ValidarFuente(FuentesRegistro fuente)
        {
            RespuestaConsulta<List<string>> result = null;
            await Task.Run(() =>
            {
                result = FuenteBL.ValidarExistencia(fuente);
            });

            return JsonConvert.SerializeObject(result);
        }


        #endregion



        #region Destinatarios



        [HttpPost]
        public async Task<string> ConsultarDestinatarios(DetalleFuentesRegistro destinatario)
        {
            user = User.Identity.GetUserId();
            RespuestaConsulta<List<DetalleFuentesRegistro>> result = null;
            await Task.Run(() =>
            {

                destinatario.Usuario = user;
                result = FuenteDestinatariosBL.ObtenerDatos(destinatario);
            });

            return JsonConvert.SerializeObject(result);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="fuente"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<string> AgregarDestinatario(DetalleFuentesRegistro destinatario)
        {
            user = User.Identity.GetUserId();
            RespuestaConsulta<List<DetalleFuentesRegistro>> result = null;
            await Task.Run(() =>
            {
                destinatario.Usuario = user;
                if (destinatario.idDetalleFuente==0)
                {
                    result = FuenteDestinatariosBL.InsertarDatos(destinatario);
                }
                else
                {

                    result = FuenteDestinatariosBL.ActualizarElemento(destinatario);
                }
              
            });

            return JsonConvert.SerializeObject(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fuente"></param>
        /// <returns></returns>
        /// 

        public async Task<string> EliminarDestinatario(DetalleFuentesRegistro destinatario)
        {
            user = User.Identity.GetUserId();
            RespuestaConsulta<List<DetalleFuentesRegistro>> result = null;
            await Task.Run(() =>
            {

                destinatario.Usuario = user;

                result = FuenteDestinatariosBL.EliminarElemento(destinatario);
            });

            return JsonConvert.SerializeObject(result);
        }


        #endregion
    }
}
