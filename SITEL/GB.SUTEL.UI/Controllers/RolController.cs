using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GB.SUTEL.BL.Seguridad;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.UI.Helpers;
using GB.SUTEL.UI.Recursos.Utilidades;
using System.Text;
using Microsoft.AspNet.Identity;

namespace GB.SUTEL.UI.Controllers.Seguridad
{
    public class RolController : BaseController
    {
        #region Atributos
        RolBL refRolBL;
        AccesoPermisosBL refAccesoPermisosBL;
        private static Rol bitacoraRol = new Rol();
        Funcion func = new Funcion();
        #endregion

        public RolController()
        {
            refRolBL = new RolBL(AppContext);
            refAccesoPermisosBL = new AccesoPermisosBL(AppContext);
        }
        //
        // GET: /Rol/
        [AuthorizeUserAttribute]
        public ActionResult Index()
        {
            try
            {
                Respuesta<List<Rol>> objRespuesta = new Respuesta<List<Rol>>();
                Respuesta<List<Accion>> objRespuestaAccion = new Respuesta<List<Accion>>();
                Respuesta<List<PANTALLAMENU>> objRespuestaPantalla = new Respuesta<List<PANTALLAMENU>>();
                Rol oRol = new Rol();
                objRespuesta = refRolBL.ConsultarTodos();
                objRespuestaAccion = refAccesoPermisosBL.ConsultarAcciones();
                objRespuestaPantalla = refAccesoPermisosBL.ConsultarPantallas();

                string user;
                user = User.Identity.GetUserId();
                try
                {
                    func._index(user, "Roles", "Roles Seguridad");
                
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Exception caught.", e);
                }
                return View(new Tuple<List<Rol>, Rol, List<PANTALLAMENU>, List<Accion>>(objRespuesta.objObjeto, oRol, objRespuestaPantalla.objObjeto, objRespuestaAccion.objObjeto));
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return View(new Tuple<List<Rol>, Rol,
                    List<PANTALLAMENU>, List<Accion>>(new List<Rol>(), null, new List<PANTALLAMENU>(), new List<Accion>()));
            }
            catch (Exception ex)
            {
                var newEx = AppContext.ExceptionBuilder.BuildException("Ha ocurrido un error al guardar", ex);
                ViewBag.Error = ((CustomException)newEx);
                return View(new Tuple<List<Rol>, Rol,
                    List<PANTALLAMENU>, List<Accion>>(new List<Rol>(), null, new List<PANTALLAMENU>(), new List<Accion>()));
            } 
        }

        public ActionResult _table()
        {
            try {                
                ViewBag.searchTerm = new Rol();

                Respuesta<List<Rol>> objRespuesta = new Respuesta<List<Rol>>();
                Respuesta<List<Accion>> objRespuestaAccion = new Respuesta<List<Accion>>();
                Respuesta<List<PANTALLAMENU>> objRespuestaPantalla = new Respuesta<List<PANTALLAMENU>>();
                Rol oRol = new Rol();
                objRespuesta = refRolBL.ConsultarTodos();

                // gBitacora(ActionsBinnacle.Consultar, null, null);

                objRespuestaAccion = refAccesoPermisosBL.ConsultarAcciones();
                objRespuestaPantalla = refAccesoPermisosBL.ConsultarPantallas();
                ViewBag.searchTerm = new Rol();
                return PartialView(new Tuple<List<Rol>, Rol, List<PANTALLAMENU>, List<Accion>>(objRespuesta.objObjeto, oRol, objRespuestaPantalla.objObjeto, objRespuestaAccion.objObjeto));
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView(new Tuple<List<Rol>, Rol,
                    List<PANTALLAMENU>, List<Accion>>(new List<Rol>(), null, new List<PANTALLAMENU>(), new List<Accion>()));
            }
            catch (Exception ex)
            {
                var newEx = AppContext.ExceptionBuilder.BuildException("Ha ocurrido un error al guardar", ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView(new Tuple<List<Rol>, Rol, 
                    List<PANTALLAMENU>, List<Accion>>(new List<Rol>(), null, new List<PANTALLAMENU>(), new List<Accion>()));
            }             
        }
        [HttpPost]
        public ActionResult _table(Rol rol)
        {
            Respuesta<List<Rol>> objRespuesta = new Respuesta<List<Rol>>();
            Respuesta<List<Accion>> objRespuestaAccion = new Respuesta<List<Accion>>();
            Respuesta<List<PANTALLAMENU>> objRespuestaPantalla = new Respuesta<List<PANTALLAMENU>>();
            Rol oRol = new Rol();
            objRespuesta = refRolBL.gFiltrarRoles(rol.IdRol,(rol.NombreRol==null?"":rol.NombreRol));
            objRespuestaAccion = refAccesoPermisosBL.ConsultarAcciones();
            objRespuestaPantalla = refAccesoPermisosBL.ConsultarPantallas();

            ViewBag.searchTerm = rol;
            return PartialView(new Tuple<List<Rol>, Rol, List<PANTALLAMENU>, List<Accion>>(objRespuesta.objObjeto, oRol, objRespuestaPantalla.objObjeto, objRespuestaAccion.objObjeto));
        }
        //
        // POST: /Rol/Crear
        [HttpPost]       
        public string Crear(Rol nuevoRol)
        {           
            Respuesta<Rol> respuesta = new Respuesta<Rol>();
            JSONResult<Rol> jsonRespuesta = new JSONResult<Rol>();
            try
            {
                ViewBag.mostarError = false;
                ViewBag.mensajeError = "";
                respuesta = refRolBL.Agregar(nuevoRol);

                func.rollbit(ActionsBinnacle.Crear, nuevoRol, null);
                // gBitacora(ActionsBinnacle.Insertar, nuevoRol, null);
                if (respuesta.blnIndicadorTransaccion)
                {
                    jsonRespuesta.data = respuesta.objObjeto;
                    return jsonRespuesta.toJSON();
                }
                else
                {
                    jsonRespuesta.ok = false;
                    jsonRespuesta.data = respuesta.objObjeto;
                    return jsonRespuesta.toJSON();
                }
           }
           catch (Exception ex)
            {
                jsonRespuesta.errorServer(ex);
                return jsonRespuesta.toJSON();
            }
        }

        //
        // POST: /Rol/Editar
        [HttpPost]
        public string Editar(Rol nuevoRol)
        {
            Respuesta<Rol> respuesta = new Respuesta<Rol>();
            JSONResult<Rol> jsonRespuesta = new JSONResult<Rol>();
            try
            {
                int id = nuevoRol.IdRol;
                List<Rol> consulta = refRolBL.gFiltrarRoles(id, string.Empty).objObjeto;
                bitacoraRol = new Rol();
                bitacoraRol.IdRol = consulta[0].IdRol;
                bitacoraRol.NombreRol = consulta[0].NombreRol;

                respuesta = refRolBL.Editar(nuevoRol);
               func.rollbit(ActionsBinnacle.Editar, nuevoRol, bitacoraRol);
                if (respuesta.blnIndicadorTransaccion)
                {
                    jsonRespuesta.data = respuesta.objObjeto;
                    return jsonRespuesta.toJSON();
                }
                else
                {
                    jsonRespuesta.ok = false;
                    jsonRespuesta.data = respuesta.objObjeto;
                    return jsonRespuesta.toJSON();
                }
            }
            catch (Exception ex)
            {
                jsonRespuesta.errorServer(ex);
                return jsonRespuesta.toJSON();
            }
        }

        //
        // POST: /Rol/Editar
        [HttpPost]
        public string Eliminar(Rol nuevoRol)
        {
            Respuesta<Rol> respuesta = new Respuesta<Rol>();
            JSONResult<Rol> jsonRespuesta = new JSONResult<Rol>();
            try
            {
                int id = nuevoRol.IdRol;
                List<Rol> consulta = refRolBL.gFiltrarRoles(id, string.Empty).objObjeto;
                bitacoraRol = new Rol();
                bitacoraRol.IdRol = consulta[0].IdRol;
                bitacoraRol.NombreRol = consulta[0].NombreRol;

                respuesta = refRolBL.Eliminar(nuevoRol);
                func.rollbit(ActionsBinnacle.Borrar, null, bitacoraRol);
                if (respuesta.blnIndicadorTransaccion)
                {
                    jsonRespuesta.data = respuesta.objObjeto;
                    return jsonRespuesta.toJSON();
                }
                else
                {
                    jsonRespuesta.ok = false;
                    jsonRespuesta.data = respuesta.objObjeto;
                    jsonRespuesta.strMensaje = respuesta.strMensaje;
                    return jsonRespuesta.toJSON();
                }
            }
            catch (Exception ex)
            {
                jsonRespuesta.errorServer(ex);
                return jsonRespuesta.toJSON();
            }
        }
       
        [HttpPost]
        public string Pantallas()
        {
            Respuesta<List<PANTALLAMENU>> objRespuesta = new Respuesta<List<PANTALLAMENU>>();
            JSONResult<List<PANTALLAMENU>> jsonRespuesta = new JSONResult<List<PANTALLAMENU>>();
            try
            {
                objRespuesta = refAccesoPermisosBL.ConsultarPantallas();
                if (objRespuesta.blnIndicadorTransaccion)
                {
                    jsonRespuesta.data = objRespuesta.objObjeto;                    
                }
                else
                {
                    jsonRespuesta.ok = false;
                    jsonRespuesta.data = null;
                }

                return jsonRespuesta.toJSON();
            }
            catch (Exception ex)
            {
                jsonRespuesta.errorServer(ex);
                return jsonRespuesta.toJSON();
            }
        }


        [HttpPost]
        public string AccionPorPantalla(int IdRol, int IdPantalla)
        {

            Respuesta<List<Accion>> objRespuesta = new Respuesta<List<Accion>>();
            JSONResult<List<Accion>> jsonRespuesta = new JSONResult<List<Accion>>();

            RolAccionPantalla objRolAccionPantalla = new RolAccionPantalla();
            objRolAccionPantalla.IdRol = IdRol;
            objRolAccionPantalla.IdPantalla = IdPantalla;

            try
            {
                objRespuesta = refAccesoPermisosBL.ConsultarAccionesPorRolYPantalla(objRolAccionPantalla);
                if (objRespuesta.blnIndicadorTransaccion)
                {
                    jsonRespuesta.data = objRespuesta.objObjeto;
                }
                else
                {
                    jsonRespuesta.ok = false;
                    jsonRespuesta.data = null;
                }

                return jsonRespuesta.toJSON();
            }
            catch (Exception ex)
            {
                jsonRespuesta.errorServer(ex);
                return jsonRespuesta.toJSON();
            }
        }

        [HttpPost]
        public string ConsultaPermisosPorRol(int idRol)
        {
            Respuesta<List<RolAccionPantalla>> objRespuesta = new Respuesta<List<RolAccionPantalla>>();
            JSONResult<List<RolAccionPantalla>> jsonRespuesta = new JSONResult<List<RolAccionPantalla>>();

            Rol objRol = new Rol();
            objRol.IdRol = idRol;            

            try
            {
                objRespuesta = refAccesoPermisosBL.ConsultarPermisosPorRol(objRol);
                if (objRespuesta.blnIndicadorTransaccion)
                {
                    jsonRespuesta.data = objRespuesta.objObjeto;
                }
                else
                {
                    jsonRespuesta.state = 300;
                    jsonRespuesta.data = null;
                }

                return jsonRespuesta.toJSON();
            }
            catch (Exception ex)
            {
                jsonRespuesta.errorServer(ex);
                return jsonRespuesta.toJSON();
            }
        }

        [HttpPost]
        public string ActualizarPermisos(int idRol, int count)
        {
            Respuesta<Rol> objRespuesta = new Respuesta<Rol>();
            JSONResult<Rol> jsonRespuesta = new JSONResult<Rol>();
            RolAccionPantalla objRolAccionPantalla;                      

            try
            {
                List<RolAccionPantalla> listadoPermisos = new List<RolAccionPantalla>();                  
                for (int i = 0; i < count; i++)
                {
                    objRolAccionPantalla = new RolAccionPantalla();
                    objRolAccionPantalla.IdRol = Convert.ToInt32(Request.Params["Permisos[" + i + "][IdRol]"]);
                    objRolAccionPantalla.IdAccion = Convert.ToInt32(Request.Params["Permisos[" + i + "][IdAccion]"]);
                    objRolAccionPantalla.IdPantalla = Convert.ToInt32(Request.Params["Permisos[" + i + "][IdPantalla]"]);
                    listadoPermisos.Add(objRolAccionPantalla);
                }
                Rol objRol = new Rol();
                objRol.IdRol = idRol;

                objRespuesta = refAccesoPermisosBL.ActualizaPermisos(objRol, listadoPermisos);
                if (objRespuesta.blnIndicadorTransaccion)
                {
                    jsonRespuesta.data = objRespuesta.objObjeto;
                }
                else
                {
                    jsonRespuesta.state = 300;
                    jsonRespuesta.data = null;
                }

                return jsonRespuesta.toJSON();
            }
            catch (Exception ex)
            {
                jsonRespuesta.errorServer(ex);
                return jsonRespuesta.toJSON();
            }
        }

    }
}
