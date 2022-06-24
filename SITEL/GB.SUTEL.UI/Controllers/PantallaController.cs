using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GB.SUTEL.BL.Seguridad;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.ExceptionHandler;

namespace GB.SUTEL.UI.Controllers
{
    public class PantallaController : BaseController
    {
        #region atributos
        PantallaBL refPantallaBL;
        #endregion

        public PantallaController()
        {
            refPantallaBL = new PantallaBL(AppContext);            
        }

        public string ConsultarPorMenu()
        {
            Respuesta<List<PANTALLAMENU>> respuesta = new Respuesta<List<PANTALLAMENU>>();
            JSONResult<List<PANTALLAMENU>> jsonRespuesta = new JSONResult<List<PANTALLAMENU>>();
            try
            {
                respuesta = refPantallaBL.ConsultarTodosConPadre();
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
            catch
            {
                jsonRespuesta.ok = false;
                return jsonRespuesta.toJSON();
            }
        }
        // GET: Pantalla
        public ActionResult Index()
        {
            return View(refPantallaBL.ConsultarTodos().objObjeto);
        }

        // GET: Pantalla/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pantalla pANTALLA = refPantallaBL.ConsultarPorExpresion(id ?? default(int)).objObjeto;
            if (pANTALLA == null)
            {
                return HttpNotFound();
            }
            return View(pANTALLA);
        }

        // GET: Pantalla/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pantalla/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPantalla,Nombre,Descripcion")] Pantalla pANTALLA)
        {
            if (ModelState.IsValid)
            {
                refPantallaBL.Agregar(pANTALLA);
                return RedirectToAction("Index");
            }

            return View(pANTALLA);
        }

        // GET: Pantalla/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pantalla pANTALLA = refPantallaBL.ConsultarPorExpresion(id ?? default(int)).objObjeto;
            if (pANTALLA == null)
            {
                return HttpNotFound();
            }
            return View(pANTALLA);
        }

        // POST: Pantalla/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPantalla,Nombre,Descripcion")] Pantalla pANTALLA)
        {
            if (ModelState.IsValid)
            {
                refPantallaBL.Editar(pANTALLA);                
                return RedirectToAction("Index");
            }
            return View(pANTALLA);
        }

        // GET: Pantalla/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pantalla pANTALLA = refPantallaBL.ConsultarPorExpresion(id ?? default(int)).objObjeto;
            if (pANTALLA == null)
            {
                return HttpNotFound();
            }
            return View(pANTALLA);
        }

        // POST: Pantalla/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pantalla pANTALLA = refPantallaBL.ConsultarPorExpresion(id).objObjeto;
            refPantallaBL.Eliminar(pANTALLA);            
            return RedirectToAction("Index");
        }
    }
}
