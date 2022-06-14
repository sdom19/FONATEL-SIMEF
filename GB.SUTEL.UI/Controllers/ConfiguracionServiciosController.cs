using GB.SUTEL.BL.Mantenimientos;
using GB.SUTEL.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.Entities.Metadata;
using GB.SUTEL.Entities;
using GB.SUTEL.BL;
using GB.SUTEL.Resources;
using System.Text;
using GB.SUTEL.UI.Recursos.Utilidades;


namespace GB.SUTEL.UI.Controllers
{
    [AuthorizeUserAttribute]
    public class ConfiguracionServiciosController : BaseController
    {

           #region atributos
        OperadorBL operadorBL;
        ServicioBL servicioBL;
        BitacoraWriter bitacora;
        private static List<int> lstServicios = new List<int>();
        private static string IdOperador;
        #endregion

        public ConfiguracionServiciosController()
        {
            operadorBL = new OperadorBL(AppContext);
            servicioBL = new ServicioBL(AppContext);
            bitacora = new BitacoraWriter(AppContext);
        }

        // GET: ConfiguracionServicios
        [HttpGet]
        [AuthorizeUserAttribute]
        public ActionResult Index()
        
        {
            if (((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault() != null)
            {
             string nombreUsuario = ((ClaimsIdentity)this.HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault().Value;
                BL.Seguridad.UsersBL refUsuario = new BL.Seguridad.UsersBL(AppContext);
                IdOperador = refUsuario.ConsultarPorExpresion(x => x.AccesoUsuario == nombreUsuario & x.Borrado == 0).objObjeto.IdOperador;
            }
            List<Operador> resultadoOperador = new List<Operador>();
            List<Servicio> resultadoServicio = new List<Servicio>();
            List<ServicioOperador> verificados = new List<ServicioOperador>();
            Operador consultar = new Operador();
            lstServicios = new List<int>();
            consultar.IdOperador = IdOperador;
            resultadoOperador = operadorBL.gFiltrarOperadores(IdOperador, "").objObjeto;
            resultadoServicio = servicioBL.ConsultarPorOperador(consultar).objObjeto;
            verificados = servicioBL.ConsultarServicioOperador(IdOperador).objObjeto;
         
            return View(new Tuple<List<Operador>, List<Servicio>,List<ServicioOperador>>(resultadoOperador, resultadoServicio,verificados));
        }

        public ActionResult _Servicios() 
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult _Servicios(string NombreOperador)
        {
            Respuesta<List<Servicio>> respuesta = new Respuesta<List<Servicio>>();
            List<Operador> resultadoOperador = new List<Operador>();
            List<ServicioOperador> verificados = new List<ServicioOperador>();
            Operador consultar = new Operador();

            consultar.IdOperador = IdOperador;
            resultadoOperador = operadorBL.gFiltrarOperadores(IdOperador, "").objObjeto;
            verificados = servicioBL.ConsultarServicioOperador(IdOperador).objObjeto;
            try
            {
                respuesta = servicioBL.ConsultarServicioVerificado(IdOperador, NombreOperador);
                ViewBag.searchTerm = NombreOperador;
                return View(new Tuple<List<Operador>, List<Servicio>,List<ServicioOperador>>(resultadoOperador, respuesta.objObjeto,verificados));
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                return View(new Tuple<List<Operador>, List<Servicio>, List<ServicioOperador>>(resultadoOperador, respuesta.objObjeto, verificados));
            }
        }

        public ActionResult Verificar(string id) 
        {
            int servicio = Convert.ToInt32(id);
            bool resultado = (from i in lstServicios where i == servicio select i).Any();
            if (resultado)
            {
                lstServicios.Remove(servicio);
            }
            else 
            {
                lstServicios.Add(servicio);
            }
            return new JsonResult { Data = new { success = true } };
        }
        [HttpPost]
        [AuthorizeUserAttribute]
        public string Guardar() 
        {
            JSONResult<string> jsonRespuesta = new JSONResult<string>();
            Respuesta<ServicioOperador> resultado = new Respuesta<ServicioOperador>();
            if (lstServicios.Count > 0)
            {
                //llamado al proceso de guardar
                resultado = operadorBL.Verificar(IdOperador, lstServicios);

                //Envio del correo
                string renderedEmail = "";
                List<Servicio> verificar = new List<Servicio>();
                List<Operador> operador = new List<Operador>();
                List<ServicioOperador> noVerificar = new List<ServicioOperador>();
                Operador consultar = new Operador();
                consultar.IdOperador = IdOperador;

                operador = operadorBL.gFiltrarOperadores(IdOperador, "").objObjeto;
                verificar = servicioBL.ConsultarPorOperador(consultar).objObjeto;
                noVerificar = servicioBL.ConsultarServicioOperador(IdOperador).objObjeto;
                if (verificar.Count > 0)
                {
                    renderedEmail = RenderView.RenderViewToString("Emails", "VerificarServicios", new Tuple<List<Servicio>, List<Operador>, List<ServicioOperador>>(verificar, operador,noVerificar));
                    bool respuesta = servicioBL.SendMail(renderedEmail);
                }
                 
            if (resultado.blnIndicadorTransaccion & resultado.blnIndicadorState == 200)
            {
                jsonRespuesta.ok = true;
                jsonRespuesta.strMensaje = resultado.strMensaje;
                foreach (var item in noVerificar)
                {
                    foreach (int item2 in lstServicios)
                    {
                        if(item.IdeServicio==item2){
                            bitacora.gRegistrarBitacora<ServicioOperador>(HttpContext, 1, item, null, "mensaje");
                        }
                    }                    
                }                
            }
            else 
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = "Error al verificar";
                jsonRespuesta.state = resultado.blnIndicadorState;
            }
            }
            else
            {
                 jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = "Debe seleccionar un servicio para verificar";
                jsonRespuesta.state = resultado.blnIndicadorState;
            }
            return jsonRespuesta.toJSON();
                
        }

        
    }
}