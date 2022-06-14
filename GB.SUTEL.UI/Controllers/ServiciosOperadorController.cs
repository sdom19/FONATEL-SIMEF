using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GB.SUTEL.BL.Mantenimientos;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.Entities.Metadata;
using GB.SUTEL.Entities;
using GB.SUTEL.BL;
using GB.SUTEL.Resources;
using GB.SUTEL.UI.Helpers;
using System.Text;
using GB.SUTEL.UI.Recursos.Utilidades;
using Microsoft.AspNet.Identity;

namespace GB.SUTEL.UI.Controllers
{
    public class ServiciosOperadorController : BaseController
    {
        #region atributos
        OperadorBL operadorBL;
        ServicioBL servicioBL;
        Funcion func = new Funcion();
        #endregion

        public ServiciosOperadorController()
        {
            operadorBL = new OperadorBL(AppContext);
            servicioBL = new ServicioBL(AppContext);
        }

        // GET: ServiciosOperador
        [HttpGet]
        [AuthorizeUserAttribute]
        public ActionResult Index()
        {
            ViewBag.IdOperadorSeleccionado = string.Empty;
            Respuesta<List<Operador>> listadoOperadores = operadorBL.ConsultarTodos();
            Respuesta<List<Servicio>> listadoServicios = servicioBL.ConsultarTodos();

            string user;
            user = User.Identity.GetUserId();
            try
            {
                func._index(user, "Servicios Operador", "Servicios Operador Mantenimiento");
            }
            
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }
            return View(new Tuple<List<Operador>, List<Servicio>>(listadoOperadores.objObjeto, listadoServicios.objObjeto));

        }

        [HttpPost]
        [AuthorizeUserAttribute]
        public ActionResult Index(Tuple<List<Operador>, List<Servicio>> tuple)
        {
            ViewBag.IdOperadorSeleccionado = string.Empty;
            //Respuesta<List<Operador>> listadoOperadores = operadorBL.ConsultarTodos();
            //Respuesta<List<Servicio>> listadoServicios = servicioBL.ConsultarTodos();
            return View(tuple);

        }

        [HttpGet]
        [AuthorizeUserAttribute]
        public ActionResult _busqueda()
        {

            ViewBag.searchTerm_NOMBREOPERADOR = string.Empty;


            return PartialView();

        }

        [HttpPost]
        [AuthorizeUserAttribute]
        public ActionResult _busqueda(string NombreOperador)
        {


            string nombreOperador = Request.Params["data[NombreOperador]"];
            JSONResult<List<Operador>> respuesta = new JSONResult<List<Operador>>();

            ViewBag.searchTerm_NOMBREOPERADOR = nombreOperador;


            Respuesta<List<Operador>> listadoOperadores = new Respuesta<List<Operador>>();
            if (string.IsNullOrEmpty(nombreOperador))
            {
                listadoOperadores = operadorBL.ConsultarTodos();
            }
            else
            {
                listadoOperadores = operadorBL.gFiltrarOperadores(null, nombreOperador);
            }

            respuesta.data = listadoOperadores.objObjeto;
            Respuesta<List<Servicio>> listadoServicios = servicioBL.ConsultarTodos();
            //return View("Index",new Tuple<List<Operador>, List<Servicio>>(listadoOperadores.objObjeto, listadoServicios.objObjeto));
            return PartialView(new Tuple<List<Operador>, List<Servicio>>(listadoOperadores.objObjeto, listadoServicios.objObjeto));
            //RedirectToAction("Index", new Tuple<List<Operador>, List<Servicio>>(listadoOperadores.objObjeto, listadoServicios.objObjeto));

            // return respuesta.toJSONLoopHandlingIgnore();
        }

        [HttpPost]
        [AuthorizeUserAttribute]
        //public ActionResult ConsultarServiciosPorOperador(FormCollection data)
        public string ConsultarServiciosPorOperador(string IdOperadorBuscar)
        {
            JSONResult<List<Servicio>> jsonRespuesta = new JSONResult<List<Servicio>>();
            Operador poOperador = new Operador();
            Respuesta<List<Servicio>> listadoServicios;
            //asignación de datos
            poOperador.IdOperador = IdOperadorBuscar;
            string ServiciosVector = string.Empty;
            List<Servicio> lstServicios = new List<Servicio>();
            if (!(string.IsNullOrEmpty(IdOperadorBuscar)))
            {
                ViewBag.IdOperadorSeleccionado = IdOperadorBuscar;
            }


            try
            {
                listadoServicios = servicioBL.ConsultarPorOperador(poOperador);

                //if (listadoServicios.blnIndicadorTransaccion)
                //{

                //    foreach (var item in listadoServicios.objObjeto)
                //    {
                //        ServiciosVector += item.IdServicio + "|";
                //    }

                //    jsonRespuesta.data = listadoServicios.objObjeto;
                //    return jsonRespuesta.toJSON();
                //}
                //else
                //{
                //    jsonRespuesta.ok = false;
                //    return jsonRespuesta.toJSON();
                //}

                if (listadoServicios.blnIndicadorTransaccion)
                {
                    foreach (var item in listadoServicios.objObjeto)
                    {
                        Servicio resultado = new Servicio();
                        bool? verificado = (from x in item.ServicioOperador where x.IdOperador == IdOperadorBuscar select x.Verificar).FirstOrDefault();
                        if (verificado == true)
                        {
                            resultado.Borrado = 1;
                            resultado.IdServicio = item.IdServicio;
                            lstServicios.Add(resultado);
                        }
                        else 
                        {
                            resultado.Borrado = 0;
                            resultado.IdServicio = item.IdServicio;
                            lstServicios.Add(resultado);
                        }
                        
                    }
                    jsonRespuesta.data = lstServicios;
                    return jsonRespuesta.toJSON();
                }
                else
                {
                    jsonRespuesta.ok = false;
                    return jsonRespuesta.toJSON();
                }
               
            }
            catch (Exception ex)
            {
                jsonRespuesta.ok = false;
                return jsonRespuesta.toJSON();
            }
        }

        /// <summary>
        /// Guardar
        /// </summary>
        /// <param name="IdOperadorGuardar">Codigo del operador</param>
        /// <param name="ServiciosEliminar">Identificadores de servicios separados por coma(,) que van a ser eliminados</param>
        /// <param name="form">FormCollection</param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeUserAttribute]
        public string Guardar(string IdOperadorGuardar, string ServiciosEliminar, FormCollection form)
        {
            JSONResult<string> jsonRespuesta = new JSONResult<string>();

            if (!(string.IsNullOrEmpty(IdOperadorGuardar)))
            {

                //vector de servicios a insertar
                int[] servicios = new int[form.AllKeys.Count() - 2];

                for (int i = 2; i < form.AllKeys.Count(); i++)
                {
                    var item = form.AllKeys[i];
                    servicios[i - 2] = int.Parse(item.ToString());
                }

                //vector de servicios a eliminar
                string[] serviciosEliminarAux = ServiciosEliminar.Split(',');
                int[] serviciosEliminar = new int[serviciosEliminarAux.Count()];

                int index = 0;

                if (!string.IsNullOrEmpty(ServiciosEliminar))
                {
                    foreach (string item in serviciosEliminarAux)
                    {
                        serviciosEliminar[index] = int.Parse(item);
                        ++index;
                    }
                }

                //Ejecucion 

                //llamado al proceso de guardar
                Respuesta<ServicioOperador> resultado = new Respuesta<ServicioOperador>();
                if (servicios.Length > 0)
                {         //bitacora de insercion
                    func.serviciosopbit(ActionsBinnacle.Crear, servicios);
                    resultado = operadorBL.AgregarServicio(IdOperadorGuardar, servicios);
                    if (resultado.blnIndicadorTransaccion && !(string.IsNullOrEmpty(ServiciosEliminar)))
                    {
               
                        //llamado al proceso de eliminar
                        resultado = operadorBL.EliminarServicio(IdOperadorGuardar, serviciosEliminar);
                        //bitacora eliminacion
                        if (resultado.blnIndicadorTransaccion)
                        {
                            func.serviciosopbit(ActionsBinnacle.Borrar, serviciosEliminar);
                        }
                    }
                    jsonRespuesta.ok = true;
                    return jsonRespuesta.toJSON();

                }
                else 
                {
                    //jsonRespuesta.ok = false;
                    //jsonRespuesta.strMensaje = Mensajes.RegistroOperadorServicio;
                    //return jsonRespuesta.toJSON();
                    resultado = operadorBL.EliminarServicio(IdOperadorGuardar, serviciosEliminar);
                    //bitacora eliminacion
                    if (resultado.blnIndicadorTransaccion)
                    {
                        func.serviciosopbit(ActionsBinnacle.Borrar, serviciosEliminar);
                    }
                    jsonRespuesta.ok = true;
                    return jsonRespuesta.toJSON();
                }

            }
            else
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = Mensajes.MensajeSeleccionesOperador_serviciosOperador;
                return jsonRespuesta.toJSON();
            }
        }

       
    }
}