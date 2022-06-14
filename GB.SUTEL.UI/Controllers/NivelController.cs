using GB.SUTEL.BL.Mantenimientos;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GB.SUTEL.Entities;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.UI.Helpers;
using System.Text;
using GB.SUTEL.UI.Recursos.Utilidades;
using Microsoft.AspNet.Identity;
using GB.SUTEL.UI.Recursos.Utilidades;

namespace GB.SUTEL.UI.Controllers
{
    public class NivelController : BaseController
    {
        #region atributos
        NivelBL nivelBL;
        Funcion func = new Funcion();
        #endregion

        #region Constructor
        public NivelController()
        {
            nivelBL = new NivelBL(AppContext);
          
        }


        #endregion

        #region Vistas


        // GET: Nivel
        [AuthorizeUserAttribute]
        public ActionResult Index()
        {


            Respuesta<NivelViewModels> respuesta = new Respuesta<NivelViewModels>();
            respuesta.objObjeto = new NivelViewModels();
            respuesta.objObjeto.listadoNiveles = nivelBL.gListar().objObjeto;
            if (respuesta.blnIndicadorTransaccion)
            {

                string user;
                user = User.Identity.GetUserId();
                try
                {
                    func._index(user, "Nivel", "Nivel Mantenimiento");
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Exception caught.", e);
                }
                return View(respuesta.objObjeto);

            }
             return View();
        }

        [AuthorizeUserAttribute]
        [HttpGet]
        public ActionResult _table()
        {
            try
            {
                ViewBag.TerminosBusqueda = new Nivel();

                Respuesta<NivelViewModels> respuesta = new Respuesta<NivelViewModels>();
                respuesta.objObjeto = new NivelViewModels();
                respuesta.objObjeto.listadoNiveles = nivelBL.gListar().objObjeto;
                if (respuesta.blnIndicadorTransaccion)
                {
                    return PartialView(respuesta.objObjeto);

                }
                return PartialView();
            }
            catch (CustomException)
            {
                return PartialView();
            }
            catch (Exception)
            {
                return PartialView();
            }
        }

        [AuthorizeUserAttribute]
        [HttpPost]
        public ActionResult _table(int? IdNivel, string DescNivel)
        {
            try
            {

                Nivel nivel = new Nivel();
                nivel.IdNivel = IdNivel.HasValue ? IdNivel.Value : 0;
                nivel.DescNivel = DescNivel;
                ViewBag.TerminosBusqueda = nivel;

                Respuesta<NivelViewModels> respuesta = new Respuesta<NivelViewModels>();
                respuesta.objObjeto = new NivelViewModels();
                if (IdNivel.HasValue || !(string.IsNullOrEmpty(DescNivel)))
                {
                    respuesta.objObjeto.listadoNiveles = nivelBL.gConsutarPorIdDescripcion(IdNivel.HasValue ? IdNivel.Value : 0, DescNivel).objObjeto;
                }
                else
                {
                    respuesta.objObjeto.listadoNiveles = nivelBL.gListar().objObjeto;
                }
                if (respuesta.blnIndicadorTransaccion)
                {
                    return PartialView(respuesta.objObjeto);

                }
                return PartialView();
            }
            catch (CustomException)
            {
                return PartialView();
            }
            catch (Exception)
            {
                return PartialView();
            }
        }


        [AuthorizeUserAttribute]
        public ActionResult Listar()
        {
            return View();
        }

        [AuthorizeUserAttribute]
        [HttpPost]
        public string Crear(NivelViewModels poNivel)
        {
            Respuesta<Nivel> respuesta = new Respuesta<Nivel>();
            JSONResult<Nivel> jsonRespuesta = new JSONResult<Nivel>();
            try
            {


                respuesta = nivelBL.gAgregar(poNivel.itemNivel);
                if (respuesta.blnIndicadorTransaccion)
                {
                    jsonRespuesta.data = respuesta.objObjeto;
                    func.nivelbit(ActionsBinnacle.Crear, respuesta.objObjeto, null);
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



        [AuthorizeUserAttribute]
        [HttpPost]
        public string Editar(NivelViewModels poNivel)
        {
            Respuesta<Nivel> respuesta = new Respuesta<Nivel>();
            JSONResult<Nivel> jsonRespuesta = new JSONResult<Nivel>();
            Nivel objetoAnterior = null;
            NivelBL logicaAux = new NivelBL(this.AppContext);
            try
            {

                objetoAnterior = logicaAux.gConsultar(poNivel.itemNivel.IdNivel).objObjeto;

                respuesta = nivelBL.gModificar(poNivel.itemNivel);
                if (respuesta.blnIndicadorTransaccion)
                {
                    jsonRespuesta.data = respuesta.objObjeto;
                    func.nivelbit(ActionsBinnacle.Editar, respuesta.objObjeto, objetoAnterior);
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


        [AuthorizeUserAttribute]
        [HttpPost]
        public string Eliminar(int ItemEliminar)
        {
            Respuesta<Nivel> respuesta = new Respuesta<Nivel>();
            JSONResult<Nivel> jsonRespuesta = new JSONResult<Nivel>();
            try
            {
                Nivel Nivel = new Nivel();
                Nivel.IdNivel = ItemEliminar;

                respuesta = nivelBL.gEliminar(Nivel);
                if (respuesta.blnIndicadorTransaccion)
                {
                    jsonRespuesta.data = respuesta.objObjeto;
                    func.nivelbit(ActionsBinnacle.Borrar, null, respuesta.objObjeto);

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
        #endregion

        


    }
}
