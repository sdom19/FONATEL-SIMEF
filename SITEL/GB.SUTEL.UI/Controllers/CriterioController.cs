using GB.SUTEL.BL.Mantenimientos;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GB.SUTEL.Entities;
using GB.SUTEL.UI.Helpers;
using System.Text;
using GB.SUTEL.UI.Recursos.Utilidades;
using GB.SUTEL.ExceptionHandler;
using Microsoft.AspNet.Identity;

namespace GB.SUTEL.UI.Controllers
{
    public class CriterioController : BaseController
    {
        #region atributos
        CriterioBL criterioBL;
        DireccionBL direccionBL;
        IndicadorBL refIndicadorBL;
        Funcion func = new Funcion();
        #endregion

        #region Constructor

        public CriterioController()
        {

            criterioBL = new CriterioBL(AppContext);
            direccionBL = new DireccionBL();
            refIndicadorBL = new IndicadorBL(AppContext);
        }



        #endregion

        #region Vistas

        [AuthorizeUserAttribute]
        public ActionResult Index()
        {

            Respuesta<List<Criterio>> respuesta = new Respuesta<List<Criterio>>();
            Direccion direccion = new Direccion();
            Criterio oCriterio = new Criterio();
            respuesta.objObjeto = criterioBL.gListar().objObjeto;
            cargarDireccionesEnBag();
            cargarIndicadoresEnBag(direccion);
            if (respuesta.blnIndicadorTransaccion)
            {

                string user;
                user = User.Identity.GetUserId();
                try
                {
                    func._index(user, "Criterios", "Criterios Mantenimiento");
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Exception caught.", e);
                }
                return View(new Tuple<List<Criterio>, Criterio>(respuesta.objObjeto, oCriterio));

            }

            return View(new Tuple<List<Criterio>, Criterio>(new List<Criterio>(), oCriterio));
        }

        [HttpGet]
        [AuthorizeUserAttribute]
        public ActionResult _table()
        {
            Criterio nuevoCriterio = new Criterio();
            nuevoCriterio.Direccion = new Direccion();
            nuevoCriterio.Indicador = new Indicador();
            //se restaura los terminos de busqueda
            ViewBag.TerminosBusqueda=nuevoCriterio;
            

            Respuesta<List<Criterio>> respuesta = new Respuesta<List<Criterio>>();
            Criterio oCriterio = new Criterio();
            respuesta.objObjeto = criterioBL.gListar().objObjeto;
            cargarDireccionesEnBag();
            if (respuesta.blnIndicadorTransaccion)
            {
                return PartialView(new Tuple<List<Criterio>, Criterio>(respuesta.objObjeto, oCriterio));

            }

            return PartialView();
        }

        [HttpPost]
        [AuthorizeUserAttribute]
        public ActionResult _table(Criterio poCriterio)
        {
            ViewBag.TerminosBusqueda = new Criterio();
            //se restaura los terminos de busqueda
            ViewBag.TerminosBusqueda= poCriterio;
           

            Respuesta<List<Criterio>> respuesta = new Respuesta<List<Criterio>>();
            Criterio oCriterio = new Criterio();

            respuesta.objObjeto = criterioBL.gFiltrarCriterio(poCriterio.IdCriterio == null ? "" : poCriterio.IdCriterio,
                poCriterio.DescCriterio == null ? "" : poCriterio.DescCriterio,
                poCriterio.Direccion == null || poCriterio.Direccion.Nombre == null ? "" : poCriterio.Direccion.Nombre,
                poCriterio.Indicador == null || poCriterio.Indicador.NombreIndicador == null ? "" : poCriterio.Indicador.NombreIndicador).objObjeto;
           
               
            cargarDireccionesEnBag();
            if (respuesta.blnIndicadorTransaccion)
            {
                return PartialView(new Tuple<List<Criterio>, Criterio>(respuesta.objObjeto, oCriterio));

            }

            return PartialView();
        }

        [HttpPost]
        [AuthorizeUserAttribute]
        public string Crear(Criterio poCriterio)
        {
            Respuesta<Criterio> respuesta = new Respuesta<Criterio>();
            JSONResult<Criterio> jsonRespuesta = new JSONResult<Criterio>();
            poCriterio.Borrado = 0;
            try
            {

                if (!ModelState.IsValid)
                {
                    return jsonRespuesta.toJSON();
                }

                respuesta = criterioBL.gAgregar(poCriterio);
                if (respuesta.blnIndicadorTransaccion)
                {
                    jsonRespuesta.data = respuesta.objObjeto;
                    func.criteriosbit(ActionsBinnacle.Crear, respuesta.objObjeto, null);
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


        [HttpPost]
        [AuthorizeUserAttribute]
        public string Editar(Criterio poCriterio)
        {
            Respuesta<Criterio> respuesta = new Respuesta<Criterio>();
            JSONResult<Criterio> jsonRespuesta = new JSONResult<Criterio>();
            Criterio objetoAnterior = null;
            try
            {
                objetoAnterior = criterioBL.gConsutarPorIdDescripcion(poCriterio.IdCriterio,null).objObjeto.First();
                respuesta = criterioBL.gModificar(poCriterio);

                if (respuesta.blnIndicadorTransaccion)
                {
                    jsonRespuesta.ok = true;
                    func.criteriosbit(ActionsBinnacle.Editar, respuesta.objObjeto, objetoAnterior);
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

        [HttpPost]
        [AuthorizeUserAttribute]
        public string Eliminar(string ItemEliminar)
        {
            Respuesta<Criterio> respuesta = new Respuesta<Criterio>();
            JSONResult<Criterio> jsonRespuesta = new JSONResult<Criterio>();
            try
            {
                Criterio criterio = new Criterio();
                criterio.IdCriterio = ItemEliminar.ToString();

                respuesta = criterioBL.gEliminar(ItemEliminar);
                if (respuesta.blnIndicadorTransaccion)
                {
                    
                    func.criteriosbit(ActionsBinnacle.Borrar, null, respuesta.objObjeto);
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

        #region Metodos
        /// <summary>
        /// Sube al Bag los datos de las Direcciones para listar en combo
        /// </summary>
        private void cargarDireccionesEnBag()
        {

            List<Direccion> myCollection = direccionBL.gListar().objObjeto;
            myCollection.Insert(0, new Direccion() { IdDireccion = 0, Nombre = "<Seleccione>" });

            ViewBag.listaDirecciones = new SelectList(myCollection, "IdDireccion", "Nombre").ToList();
        }

         private void cargarIndicadoresEnBag(Direccion poDireccion)
        {

            List<Indicador> myCollection = new List<Indicador>();
             //Respuesta<List<Indicador>> indicadoresRespuesta = new Respuesta<List<Indicador>>();
             //indicadoresRespuesta=refIndicadorBL.gObtenerIndicadorPorDireccion(poDireccion.IdDireccion == null ? 0 : poDireccion.IdDireccion);
             //myCollection=indicadoresRespuesta.objObjeto;
            myCollection.Insert(0, new Indicador() { IdIndicador = "0", NombreIndicador = "<Seleccione>" });

            ViewBag.listaIndicadores = new SelectList(myCollection, "IdIndicador", "NombreIndicador").ToList();
        }
      
        public String _listarIndicadores(Direccion poDireccion) {
            Respuesta<List<Indicador>> indicadoresRespuesta = new Respuesta<List<Indicador>>();
            JSONResult<List<Indicador>> jsonRespuesta = new JSONResult<List<Indicador>>();
            List<Indicador> myCollection = new List<Indicador>();
            Indicador nuevoIndicador = new Indicador();
             try
              {

            indicadoresRespuesta=refIndicadorBL.gObtenerIndicadorPorDireccion(poDireccion.IdDireccion == null ? 0 : poDireccion.IdDireccion);
            foreach (Indicador item in indicadoresRespuesta.objObjeto)
            {
                nuevoIndicador = new Indicador();
                nuevoIndicador.IdIndicador = item.IdIndicador;
                nuevoIndicador.NombreIndicador = item.NombreIndicador;
                myCollection.Add(nuevoIndicador);
            }
           myCollection= myCollection.OrderBy(x => x.NombreIndicador).ToList();
          

            ViewBag.listaIndicadores = new SelectList(myCollection, "IdIndicador", "NombreIndicador").ToList();
            jsonRespuesta.data = myCollection;
                  jsonRespuesta.ok = true;
                  return jsonRespuesta.toJSON();
              }
              catch (CustomException cEx)
              {
                  jsonRespuesta.ok = false;
                  jsonRespuesta.strMensaje = cEx.Message;
                  jsonRespuesta.data = new List<Indicador>();
                  return jsonRespuesta.toJSON();
              }
              catch (Exception ex)
              {
                  jsonRespuesta.ok = false;
                  jsonRespuesta.strMensaje = ex.Message;
                  jsonRespuesta.data = new List<Indicador>();
                  return jsonRespuesta.toJSON();
              }
          }

        

        #endregion

    
    }
}
