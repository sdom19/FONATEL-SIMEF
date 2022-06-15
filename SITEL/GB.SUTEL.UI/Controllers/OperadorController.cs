using GB.SUTEL.BL.Mantenimientos;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.UI.Helpers;
using GB.SUTEL.UI.Recursos.Utilidades;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GB.SUTEL.UI.Controllers
{
    public class OperadorController : BaseController
    {
        Funcion func = new Funcion();
        #region atributos
        OperadorBL refOperadorBL;

        #endregion

        public OperadorController()
        {
            refOperadorBL = new OperadorBL(AppContext);
        }

        //
        // GET: /Operador/
        [AuthorizeUserAttribute]
        public ActionResult Index()
        {
            string user;
            user = User.Identity.GetUserId();
            try
            {
                func._index(user, "Operador", "Consultar Operadores Mantenimiento");
            }
            
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }
            return View(new List<Operador>());
        }

        /// <summary>
        /// Listado de la pantalla
        /// </summary>
        /// <returns></returns>
        public ActionResult _table()
        {
            Respuesta<List<Operador>> respuesta = new Respuesta<List<Operador>>();
            Operador Operador = new Operador();
            try
            {


                respuesta = refOperadorBL.ConsultarTodos();
                ViewBag.searchTerm = new Operador();
                return PartialView(respuesta.objObjeto);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView(respuesta.objObjeto);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView(respuesta.objObjeto);
            }
        }
        /// <summary>
        /// Listado de la pantalla con filtros
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _table(Operador searchTerm)
        {
            Respuesta<List<Operador>> respuesta = new Respuesta<List<Operador>>();

            try
            {
                respuesta = refOperadorBL.gFiltrarOperadores((searchTerm.IdOperador == null ? "" : searchTerm.IdOperador), searchTerm.NombreOperador != null ? searchTerm.NombreOperador : "");
                ViewBag.searchTerm = searchTerm;
                return PartialView(respuesta.objObjeto);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView(respuesta.objObjeto);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView(respuesta.objObjeto);
            }
        }

	}
}