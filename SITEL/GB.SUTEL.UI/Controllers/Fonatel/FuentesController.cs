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
                ViewBag.titulo = EtiquetasViewFuentesRegistro.CrearFuente;
                return View(new FuentesRegistro());
            } else
            {
                ViewBag.titulo = EtiquetasViewFuentesRegistro.Editar;
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
                result = FuenteBL.EliminarElemento(fuente);
            });

            return JsonConvert.SerializeObject(result);
        }




        [HttpPost]
        public async Task<string> AgregarFuente(FuentesRegistro objetoFuente)
        {
            RespuestaConsulta<List<FuentesRegistro>> result = null;
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

        [HttpPost]
        public async Task<string> ActivarFuente(FuentesRegistro fuente)
        {
            user = User.Identity.GetUserId();
            RespuestaConsulta<List<FuentesRegistro>> result = null;
            await Task.Run(() =>
            {
                result = FuenteBL.CambioEstado(fuente);

                return result;
            }).ContinueWith(objFuente=> {
                FuentesRegistro fuente = result.objetoRespuesta.Single(); 
                foreach (var item in fuente.DetalleFuentesRegistro)
                {
                    if (!item.CorreoEnviado)
                    {
                        usuarioFonatelBL.CambioEstado(new UsuarioFonatel()
                        { IdUsuario = item.idUsuario });
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
            RespuestaConsulta<List<DetalleFuentesRegistro>> result = null;
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

        [HttpPost]
        public async Task<string> AgregarDestinatario(DetalleFuentesRegistro destinatario)
        {
            RespuestaConsulta<List<DetalleFuentesRegistro>> result = null;
            UsuarioFonatel usuario = new UsuarioFonatel()
            {
                NombreUsuario = destinatario.NombreDestinatario,
                CorreoUsuario = destinatario.CorreoElectronico
            };
            await Task.Run(() =>
            {
                if (destinatario.idDetalleFuente == 0)
                {
                    return usuarioFonatelBL.InsertarDatos(usuario);
                }
                else
                {
                    result= FuenteDestinatariosBL
                            .ObtenerDatos(new DetalleFuentesRegistro()
                            { idDetalleFuente = destinatario.idDetalleFuente });
                    if (result.CantidadRegistros==1)
                    {
                        usuario.IdUsuario = result.objetoRespuesta.Single().idUsuario; 

                    }
                    return usuarioFonatelBL.ActualizarElemento(usuario);
                }
            }).ContinueWith(resultado=> {
                
                if (destinatario.idDetalleFuente == 0)
                {
                    if (resultado.Result.HayError==0)
                    {
                        destinatario.idUsuario = resultado.Result.objetoRespuesta.Single().IdUsuario;
                        result = FuenteDestinatariosBL.InsertarDatos(destinatario);
                    }
                    else
                    {
                        result = new RespuestaConsulta<List<DetalleFuentesRegistro>>();
                        result.HayError = resultado.Result.HayError;
                        result.MensajeError = resultado.Result.MensajeError;
                    }
                   
                }
                else
                {

                    if (resultado.Result.HayError == 0)
                    {
                        result = FuenteDestinatariosBL.ActualizarElemento(destinatario);
                    }
                    else
                    {
                        result = new RespuestaConsulta<List<DetalleFuentesRegistro>>();
                        result.HayError = resultado.Result.HayError;
                        result.MensajeError = resultado.Result.MensajeError;
                    }             
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
            RespuestaConsulta<List<DetalleFuentesRegistro>> result = null;
            await Task.Run(() =>
            {

                result = FuenteDestinatariosBL.EliminarElemento(destinatario);
                return result;
            }).ContinueWith(resultado => {

                usuarioFonatelBL.EliminarElemento(new UsuarioFonatel()
                {
                    IdUsuario = resultado.Result.objetoRespuesta.Single().idUsuario
                });
            });

            return JsonConvert.SerializeObject(result);
        }


        #endregion
    }
}
