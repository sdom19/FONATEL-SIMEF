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
using GB.SIMEF.Resources;
using System.Security.Claims;
using GB.SUTEL.UI.Filters;

namespace GB.SUTEL.UI.Controllers.Fonatel
{
    [AuthorizeUserAttribute]
    public class FuentesController : Controller
    {


        FuentesRegistroBL FuenteBL;
        FuentesRegistroDestinatariosBL FuenteDestinatariosBL;
        UsuarioFonatelBL usuarioFonatelBL;
        string user;

        public FuentesController()
        {
            FuenteBL = new FuentesRegistroBL(EtiquetasViewFuentesRegistro.FuentesRegistro, System.Web.HttpContext.Current.User.Identity.GetUserId());
            FuenteDestinatariosBL = new FuentesRegistroDestinatariosBL(EtiquetasViewFuentesRegistro.FuentesRegistro, System.Web.HttpContext.Current.User.Identity.GetUserId());
            usuarioFonatelBL = new UsuarioFonatelBL(EtiquetasViewFuentesRegistro.FuentesRegistro, System.Web.HttpContext.Current.User.Identity.GetUserId());
        }

        // GET: FuentesRegistro

        [HttpGet]
        public ActionResult Index()
        {
            var roles = ((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault().Value.Split(',');
            ViewBag.ConsultasFonatel = roles.Contains(Constantes.RolConsultasFonatel).ToString().ToLower();
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
        [ConsultasFonatelFilter]
        public ActionResult Create(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.titulo = EtiquetasViewFuentesRegistro.CrearFuente;
                return View(new FuenteRegistro());
            } else
            {
                ViewBag.titulo = EtiquetasViewFuentesRegistro.Editar;
                FuenteRegistro fuente =
                    FuenteBL.ObtenerDatos(new FuenteRegistro() { id = id }).objetoRespuesta.Single();
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
            RespuestaConsulta<List<FuenteRegistro>> result = null;
            await Task.Run(() =>
            {
                result = FuenteBL.ObtenerDatos(new FuenteRegistro());
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
        [ConsultasFonatelFilter]
        public async Task<string> EliminarFuente(FuenteRegistro fuente)
        {
            user = User.Identity.GetUserId();
            RespuestaConsulta<List<FuenteRegistro>> result = null;
            await Task.Run(() =>
            {
                result = FuenteBL.EliminarElemento(fuente);
            });

            return JsonConvert.SerializeObject(result);
        }



        [ConsultasFonatelFilter]
        [HttpPost]
        public async Task<string> AgregarFuente(FuenteRegistro objetoFuente)
        {
            RespuestaConsulta<List<FuenteRegistro>> result = null;
            await Task.Run(() =>
            {
                if (String.IsNullOrEmpty(objetoFuente.id))
                {
                    result = FuenteBL.InsertarDatos(objetoFuente);
                }
                else
                {
                    result = FuenteBL.ActualizarElemento(objetoFuente);
                }
           
            });

            return JsonConvert.SerializeObject(result);
        }


        /// <summary>
        /// Activar Fuente Proceso de finalizar 
        /// </summary>
        /// <param name="fuente"></param>
        /// <returns></returns>
        [ConsultasFonatelFilter]
        [HttpPost]
        public async Task<string> ActivarFuente(FuenteRegistro fuente)
        {
            user = User.Identity.GetUserId();
            RespuestaConsulta<List<FuenteRegistro>> result = null;
            await Task.Run(() =>
            {
                result = FuenteBL.CambioEstado(fuente);

                return result;
            }).ContinueWith(objFuente=> {
                FuenteRegistro fuente = result.objetoRespuesta.Single(); 
                foreach (var item in fuente.DetalleFuenteRegistro)
                {
                    if (!item.CorreoEnviado)
                    {
                        usuarioFonatelBL.CambioEstado(new Usuario()
                        { IdUsuario = item.IdUsuario });
                        item.CorreoEnviado = true;
                        FuenteDestinatariosBL.ActualizarElemento(item);
                    }        
                }
            
            });

            return JsonConvert.SerializeObject(result);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns></returns>
        [ConsultasFonatelFilter]
        [HttpPost]
        public async Task<string> ValidarFuente(FuenteRegistro fuente)
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
        public async Task<string> ConsultarDestinatarios(DetalleFuenteRegistro destinatario)
        {
            RespuestaConsulta<List<DetalleFuenteRegistro>> result = null;
            await Task.Run(() =>
            {
                result = FuenteDestinatariosBL.ObtenerDatos(destinatario);
            });

            return JsonConvert.SerializeObject(result);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="fuente"></param>
        /// <returns></returns>
        [ConsultasFonatelFilter]
        [HttpPost]
        public async Task<string> AgregarDestinatario(DetalleFuenteRegistro destinatario)
        {
            RespuestaConsulta<List<FuenteRegistro>> result = null;

            await Task.Run(() =>
            {
                Usuario usuario = new Usuario()
                {
                    NombreUsuario = destinatario.NombreDestinatario,
                    CorreoUsuario = destinatario.CorreoElectronico,
                    IdUsuario = destinatario.IdUsuario
            };
                if (destinatario.IdDetalleFuenteRegistro == 0)
                {
                    return usuarioFonatelBL.InsertarDatos(usuario);
                }
                else
                {
                    return usuarioFonatelBL.ActualizarElemento(usuario);
                }
            }).ContinueWith(resultado=> {

                if (destinatario.IdDetalleFuenteRegistro == 0)
                {
                    if (resultado.Result.HayError == 0)
                    {
                        destinatario.IdUsuario = resultado.Result.objetoRespuesta.Single().IdUsuario;
                        resultado.Result.HayError =FuenteDestinatariosBL.InsertarDatos(destinatario).HayError;
                    }
                }
                else
                {
                    if (resultado.Result.HayError == 0)
                    {
                        resultado.Result.HayError = FuenteDestinatariosBL.ActualizarElemento(destinatario).HayError;
                    }
                }
                result = FuenteBL.ObtenerDatos(new FuenteRegistro() { id=destinatario.FuenteId});
                result.HayError=resultado.Result.HayError;
                result.MensajeError= resultado.Result.MensajeError;
               
            });

            return JsonConvert.SerializeObject(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fuente"></param>
        /// <returns></returns>
        /// 
        [ConsultasFonatelFilter]
        public async Task<string> EliminarDestinatario(DetalleFuenteRegistro destinatario)
        {
            RespuestaConsulta<List<FuenteRegistro>> result = null;
            await Task.Run(() =>
            {
                return FuenteDestinatariosBL.EliminarElemento(destinatario);
            }).ContinueWith(resultado => {

               return usuarioFonatelBL.EliminarElemento(new Usuario()
                {
                    IdUsuario = resultado.Result.objetoRespuesta.Single().IdUsuario
                });
            }).ContinueWith(fuente=>
            {
                result= FuenteBL.ObtenerDatos(new FuenteRegistro() { IdFuenteRegistro=destinatario.IdFuenteRegistro });
            });

            return JsonConvert.SerializeObject(result);
        }


        #endregion
    }
}
