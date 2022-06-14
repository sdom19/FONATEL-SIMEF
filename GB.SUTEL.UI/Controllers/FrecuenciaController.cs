using GB.SUTEL.BL.Mantenimientos;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GB.SUTEL.Entities;
using GB.SUTEL.BL;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.UI.Helpers;
using System.Text;
using GB.SUTEL.UI.Recursos.Utilidades;
using Microsoft.AspNet.Identity;

namespace GB.SUTEL.UI.Controllers
{
    public class FrecuenciaController : BaseController
    {
        #region atributos
        FrecuenciaBL frecuenciaBL;
        Funcion func = new Funcion();
        #endregion

        #region Constructores
        public FrecuenciaController()
        {
            frecuenciaBL = new FrecuenciaBL(AppContext);
        }

        #endregion

        [AuthorizeUserAttribute]
        public ActionResult Index()
        {
            //se restaura los terminos de busqueda
            ViewBag.TerminosBusqueda = string.Empty;
            Respuesta<List<Frecuencia>> respuesta = new Respuesta<List<Frecuencia>>();
            respuesta.objObjeto = frecuenciaBL.gListar().objObjeto;
            Frecuencia fre = new Frecuencia();
            if (respuesta.blnIndicadorTransaccion)
            {
                string user;
                user = User.Identity.GetUserId();
                try
                {
                    func._index(user, "Frecuencia", "Frecuencia Mantenimiento");
                }
                
                catch (Exception e)
                {
                    Console.WriteLine("{0} Exception caught.", e);
                }
                return View(new Tuple<List<Frecuencia>, Frecuencia>(respuesta.objObjeto, fre));
            }

            return View();
        }

       [AuthorizeUserAttribute]
        [HttpGet]
        public ActionResult _table()
        {

            //se restaura los terminos de busqueda
            ViewBag.TerminosBusqueda = string.Empty;

            Respuesta<List<Frecuencia>> respuesta = new Respuesta<List<Frecuencia>>();
            respuesta.objObjeto = frecuenciaBL.gListar().objObjeto;
            if (respuesta.blnIndicadorTransaccion)
            {                
                return PartialView(new Tuple<List<Frecuencia>>(respuesta.objObjeto));

            }

            return PartialView(new Tuple<List<Frecuencia>>(respuesta.objObjeto));
        }

        [AuthorizeUserAttribute]
        [HttpPost]
        public ActionResult _table(string NombreFrecuencia)
        {
            //se restaura los terminos de busqueda
            ViewBag.TerminosBusqueda = NombreFrecuencia;

            Respuesta<List<Frecuencia>> respuesta = new Respuesta<List<Frecuencia>>();              

            //se consulta todo el listado 
            if (string.IsNullOrEmpty(NombreFrecuencia))
            {
                respuesta.objObjeto = frecuenciaBL.gListar().objObjeto;
            }
            else
            {

                var x = frecuenciaBL.gConsultarPorNombreListado(NombreFrecuencia).objObjeto;

                respuesta.objObjeto = x;
                
            }

            if (respuesta.blnIndicadorTransaccion)
            {
                return PartialView(new Tuple<List<Frecuencia>>(respuesta.objObjeto));

            }

            return PartialView(new Tuple<List<Frecuencia>>(respuesta.objObjeto));
        }

        [AuthorizeUserAttribute]
        public ActionResult Listar()
        {
            return View();
        }



        // POST: Nivel/Create
        [AuthorizeUserAttribute]
        [HttpPost]
        public string Crear(Frecuencia poFrecuencia)
        {

            Respuesta<Frecuencia> respuesta = new Respuesta<Frecuencia>();
            JSONResult<Frecuencia> jsonRespuesta = new JSONResult<Frecuencia>();

            if (ModelState.IsValid)
            {
                return jsonRespuesta.toJSON();
            }

            try
            {

                poFrecuencia.NombreFrecuencia = poFrecuencia.NombreFrecuencia.Trim();
                respuesta = frecuenciaBL.gAgregar(poFrecuencia);
          
                if (respuesta.blnIndicadorTransaccion)
                {
                    func.frecuenciabit(ActionsBinnacle.Crear, respuesta.objObjeto, null);
                    jsonRespuesta.data = null;
                    jsonRespuesta.ok = true;
                }
                else
                {
                    jsonRespuesta.ok = false;
                }

                jsonRespuesta.strMensaje = respuesta.strMensaje;
                return jsonRespuesta.toJSON();
            }
            catch
            {
                jsonRespuesta.ok = false;
                return jsonRespuesta.toJSON();
            }
        }



        /* string us;
         us = User.Identity.GetUserId();
         string nuevo = respuesta.objObjeto.NombreFrecuencia.ToString();
         func.frecuencia("user","Proceso de creacion en: Frecuencia", 2, nuevo, null);
*/




        [AuthorizeUserAttribute]
        [HttpPost]
        public string Editar(string  IdFrecuencia,string NombreFrecuencia, string CantidadMeses)
        {
            Respuesta<Frecuencia> respuesta = new Respuesta<Frecuencia>();
            JSONResult<Frecuencia> jsonRespuesta = new JSONResult<Frecuencia>();
            FrecuenciaBL auxBL = new FrecuenciaBL(this.AppContext);
            Frecuencia frecuenciaAux = null;

            Frecuencia obj = new Frecuencia();
            obj.CantidadMeses = Convert.ToInt32(CantidadMeses);
            obj.IdFrecuencia = Convert.ToInt32(IdFrecuencia);
            obj.NombreFrecuencia = NombreFrecuencia.Trim();

            if (!ModelState.IsValid)
            {
                return jsonRespuesta.toJSON();
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    return jsonRespuesta.toJSON();
                }
                frecuenciaAux = auxBL.gConsultarPorId(obj.IdFrecuencia).objObjeto;
                respuesta = frecuenciaBL.gModificar(obj);
                if (respuesta.blnIndicadorTransaccion)
                {
                    func.frecuenciabit(ActionsBinnacle.Editar, respuesta.objObjeto,frecuenciaAux);

                    jsonRespuesta.data = null;
                    
                }
                else
                {
                    jsonRespuesta.ok = false;
                    
                 
                }
                jsonRespuesta.strMensaje = respuesta.strMensaje;
                return jsonRespuesta.toJSON();
            }
            catch(Exception ex)
            {
                jsonRespuesta.ok = false;
                return jsonRespuesta.toJSON();
            }
        }


        [AuthorizeUserAttribute]
        [HttpPost]
        public string Eliminar(int ItemEliminar)
        {
            Respuesta<Frecuencia> respuesta = new Respuesta<Frecuencia>();
            JSONResult<Frecuencia> jsonRespuesta = new JSONResult<Frecuencia>();
            try
            {


                respuesta = frecuenciaBL.gEliminar(ItemEliminar);
                if (respuesta.blnIndicadorTransaccion)
                {
                    func.frecuenciabit(ActionsBinnacle.Borrar, null, respuesta.objObjeto); 
                    jsonRespuesta.data = respuesta.objObjeto;
                 
                }
                else
                {
                    jsonRespuesta.ok = false;
                    
                  
                }

                jsonRespuesta.strMensaje = respuesta.strMensaje;
                return jsonRespuesta.toJSON();

            }
            catch
            {
                jsonRespuesta.ok = false;

                return jsonRespuesta.toJSON();
            }
        }


      
    }
}