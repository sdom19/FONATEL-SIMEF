using GB.SUTEL.BL.Mantenimientos;
using GB.SUTEL.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.UI.Helpers;
using System.Text;
using GB.SUTEL.UI.Recursos.Utilidades;
using GB.SUTEL.Entities.CustomModels.ReglaEstadistica;
using Microsoft.AspNet.Identity;

namespace GB.SUTEL.UI.Controllers
{
    public class ConstructorController : BaseController
    {
        #region atributos
        private ConstructorBL refConstructorBL;
        private FrecuenciaBL refFrecuenciaBL;
        private DireccionBL refDireccionBL;
        private IndicadorBL refIndicadorBL;
        private CriterioBL refCriterioBL;
        private OperadorBL refOperadorBL;
        private DetalleAgrupacionBL refDetalleAgrupacionBL;
        private TipoValorBL refTipoValorBL;
        private TipoNivelDetalleBL refTipoNivelDetalleBL;
        private Genero refTipoNivelDetalleGenero;
        Funcion func = new Funcion();


        #endregion



        #region constructorController
        public ConstructorController()
        {
            refConstructorBL = new ConstructorBL(AppContext);
            refFrecuenciaBL = new FrecuenciaBL(AppContext);
            refDireccionBL = new DireccionBL();
            refIndicadorBL = new IndicadorBL(AppContext);
            refCriterioBL = new CriterioBL(AppContext);
            refOperadorBL = new OperadorBL(AppContext);
            refDetalleAgrupacionBL = new DetalleAgrupacionBL(AppContext);
            refTipoValorBL = new TipoValorBL(AppContext);
            refTipoNivelDetalleBL = new TipoNivelDetalleBL(AppContext);
   //         refTipoNivelDetalleGenero = new Genero(AppContext);
        }
        #endregion

        #region constructor
        //
        // GET: /Constructor/
        [AuthorizeUserAttribute]
        public ActionResult Index()
        {
            

            ConstructorViewModel vm = new ConstructorViewModel();
            vm.listaConstructores = new List<Constructor>();
            vm.constructor = new Constructor();
            vm.constructor.Indicador = new Indicador();
            Session["idDireccion"] = null;
            Session["idIndicador"] = null;
            string user;
            user = User.Identity.GetUserId();
            try
            {
                func._index(user, "Constructor", "Constructor");
            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }
            return View(vm);

           
        }
        #region listado constructor
        /// <summary>
        /// Listado de la pantalla de constructor
        /// </summary>
        /// <returns></returns>
         [AuthorizeUserAttribute]
        public ActionResult _table()
        {
            Respuesta<List<Constructor>> respuesta = new Respuesta<List<Constructor>>();
            Constructor constructor = new Constructor();
            try
            {


                respuesta = refConstructorBL.gObtenerConstructores();
                constructor.Indicador = new Indicador();
                constructor.Frecuencia1 = new Frecuencia();
                constructor.Frecuencia = new Frecuencia();
                ViewBag.searchTerm = constructor;
                Session["idDireccion"] = null;
                Session["idIndicador"] = null;
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
       [AuthorizeUserAttribute]
        [HttpPost]
        public ActionResult _table(Constructor searchTerm)
        {
            Respuesta<List<Constructor>> respuesta = new Respuesta<List<Constructor>>();
            Constructor constructor = new Constructor();
            try
            {
                Session["idDireccion"] = null;
                Session["idIndicador"] = null;
                respuesta = refConstructorBL.gObtenerConstructoresPorFiltros((searchTerm.Indicador == null || searchTerm.Indicador.IdIndicador == null ? "" : searchTerm.Indicador.IdIndicador), (searchTerm.Indicador == null || searchTerm.Indicador.NombreIndicador == null ? "" : searchTerm.Indicador.NombreIndicador), (searchTerm.Frecuencia == null || searchTerm.Frecuencia.NombreFrecuencia == null ? "" : searchTerm.Frecuencia.NombreFrecuencia), (searchTerm.Frecuencia1 == null || searchTerm.Frecuencia1.NombreFrecuencia == null ? "" : searchTerm.Frecuencia1.NombreFrecuencia));
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

        #endregion

        #region crear
        //
        // GET: /Constructor/Create
        /// <summary>
        /// Llama y prepara la vista para crear
        /// </summary>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        public ActionResult Crear()
        {
            Constructor nuevoConstructor = new Constructor();
            try
            {
                ConstructorViewModel mvnuevoContructor = new ConstructorViewModel();
                Respuesta<List<Frecuencia>> frecuencias = new Respuesta<List<Frecuencia>>();
                Respuesta<List<Frecuencia>> desgloses = new Respuesta<List<Frecuencia>>();
                Respuesta<List<Direccion>> direcciones = new Respuesta<List<Direccion>>();
                Respuesta<List<Criterio>> criterios = new Respuesta<List<Criterio>>();
                Respuesta<List<Operador>> operadores = new Respuesta<List<Operador>>();

                criterios = refCriterioBL.gListarCriteriosPorDireccion(0,"");
                frecuencias = refFrecuenciaBL.gListar();
                direcciones = refDireccionBL.gListar();
                operadores = refOperadorBL.ConsultarTodos();


                mvnuevoContructor.listaFrecuencia = new List<Frecuencia>(frecuencias.objObjeto.OrderBy(x => x.NombreFrecuencia));
                mvnuevoContructor.listaDesglose = new List<Frecuencia>(frecuencias.objObjeto.OrderBy(x => x.NombreFrecuencia));
                mvnuevoContructor.listaDireccion = new List<Direccion>(direcciones.objObjeto.OrderBy(x => x.Nombre));

                mvnuevoContructor.listaOperadores = new List<Operador>(operadores.objObjeto.OrderBy(x => x.NombreOperador));
                nuevoConstructor.IdDesglose = 0;
                nuevoConstructor.IdFrecuencia = 0;
                mvnuevoContructor.criterio = new ConstructorCriterio();
                mvnuevoContructor.constructor = nuevoConstructor;
                ViewBag.searchIndicador = new Indicador();
                Session["constructorCriterio"] = null;
                Session["detallesAgrupacionPorOperador"] = null;
                Session["idDireccion"] = null;
                Session["idIndicador"] = null;
                return View(mvnuevoContructor);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return View();
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return View();
            }

        }

        //
        // POST: /Constructor/Create
        /// <summary>
        /// Crea el constructor
        /// </summary>
        /// <param name="nuevoConstructor"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeUserAttribute]
        public String CrearOriginal(ConstructorViewModel nuevoConstructor)
        {
            Respuesta<Constructor> respuesta = new Respuesta<Constructor>();
            Respuesta<Constructor> constructorAnterior = new Respuesta<Constructor>();
            JSONResult<Constructor> jsonRespuesta = new JSONResult<Constructor>();
            List<ConstructorCriterio> constructorCriterio = new List<ConstructorCriterio>();
            Constructor nConstructor = new Constructor();
            jsonRespuesta.ok = false;
            bool esNuevo = true;
            try
            {
                if (Session["constructorCriterio"] != null)
                {
                    constructorCriterio = (List<ConstructorCriterio>)Session["constructorCriterio"];
                }
                if (nuevoConstructor.constructor.IdConstructor == null || nuevoConstructor.constructor.IdConstructor.Equals(new Guid("00000000-0000-0000-0000-000000000000")))
                {
                    nConstructor = nuevoConstructor.constructor;
                    nConstructor.ConstructorCriterio = constructorCriterio;
                    respuesta = refConstructorBL.gAgregarConstructor(nConstructor);
                   
                }
                else {
                    esNuevo = false;
                    nConstructor = nuevoConstructor.constructor;
                    nConstructor.ConstructorCriterio = constructorCriterio;
                    constructorAnterior = refConstructorBL.gObtenerConstructor(nuevoConstructor.constructor.IdConstructor.ToString());
                    refConstructorBL = new ConstructorBL(AppContext);
                    respuesta = refConstructorBL.gEditarConstructor(nConstructor);
                }
                if (respuesta.blnIndicadorTransaccion)
                {

                    //Inserción Bitácora
                    if (esNuevo)
                    {
                        func.constructorbit(ActionsBinnacle.Crear, respuesta.objObjeto, null);
                        jsonRespuesta.data = respuesta.objObjeto;
                        jsonRespuesta.ok = true;
                        jsonRespuesta.state = (int)ActionsBinnacle.Crear;
                        return jsonRespuesta.toJSONLoopHandlingIgnore();
                    }
                    else {
                        func.constructorbit(ActionsBinnacle.Editar, respuesta.objObjeto, constructorAnterior.objObjeto);
                        jsonRespuesta.data = respuesta.objObjeto;
                        jsonRespuesta.ok = true;
                        jsonRespuesta.state = (int)ActionsBinnacle.Editar;
                        return jsonRespuesta.toJSON();
                    }
                }
                else
                {
                    jsonRespuesta.ok = false;
                    jsonRespuesta.strMensaje = respuesta.strMensaje;
                    jsonRespuesta.data = respuesta.objObjeto;
                    return jsonRespuesta.toJSON();
                }
            }
            catch (CustomException ce)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = ce.Message;
                jsonRespuesta.data = new Constructor();
                return jsonRespuesta.toJSON();
            }
            catch (Exception ex)
            {

                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                AppContext.ExceptionBuilder.BuildException(msj, ex);
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = msj;
                jsonRespuesta.data = new Constructor();
                return jsonRespuesta.toJSON();
               
            }
        }
        /// <summary>
        ///<method>NewMethod</method>
        ///El crear con Post
        /// </summary>
        /// <param name="nuevoConstructor"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeUserAttribute]
        public ActionResult Crear(ConstructorViewModel nuevoConstructor)
        {
            Respuesta<Constructor> respuesta = new Respuesta<Constructor>();
            Respuesta<Constructor> constructorAnterior = new Respuesta<Constructor>();
            List<ConstructorCriterio> constructorCriterio = new List<ConstructorCriterio>();
            Constructor nConstructor = new Constructor();
  
            try
            {
                if (nuevoConstructor.constructor.IdConstructor == null || nuevoConstructor.constructor.IdConstructor.Equals(new Guid("00000000-0000-0000-0000-000000000000")))
                {
                    nConstructor = nuevoConstructor.constructor;
                    nConstructor.ConstructorCriterio = constructorCriterio;
                    respuesta = refConstructorBL.gAgregarConstructor(nConstructor);
                }
                if (respuesta.blnIndicadorTransaccion)
                {
                    //Inserción Bitácora             
                        func.constructorbit(ActionsBinnacle.Crear, respuesta.objObjeto, null);
                        string id = respuesta.objObjeto.IdConstructor.ToString();
                        TempData["Success"] = "Constructor creado exitosamente";
                        return Redirect("EditarCriterio?id=" + id);
                }
                else
                {
                    TempData["Error"] = respuesta.strMensaje;
                    return Redirect("Crear");
                }
            
            }
            catch (CustomException ce)
            {
                ViewBag.Error = respuesta.strMensaje;
                return Redirect("Crear");
            }
            catch (Exception ex)
            {

                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = respuesta.strMensaje;
                return Redirect("Crear");

            }

        }

        #endregion

        #region editar

        // GET: /Constructor/Edit/5
        /// <summary>
        /// Obtiene el constructor a editar por el id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        public ActionResult EditarOriginal(String id)
        {
            ConstructorViewModel mvnuevoContructor = new ConstructorViewModel();
            Respuesta<List<Frecuencia>> frecuencias = new Respuesta<List<Frecuencia>>();
            Respuesta<List<Frecuencia>> desgloses = new Respuesta<List<Frecuencia>>();

            Respuesta<List<Criterio>> criterios = new Respuesta<List<Criterio>>();
            Respuesta<List<Operador>> operadores = new Respuesta<List<Operador>>();
            Respuesta<Constructor> nConstructor = new Respuesta<Constructor>();
            List<ConstructorCriterio> constructorCriterios = new List<ConstructorCriterio>();
            List<string> idOperadores = new List<string>();
            List<string> idCriterios = new List<string>();
            try
            {
                ViewBag.Error = null;
                nConstructor = refConstructorBL.gObtenerConstructor(id);//obtengo el constructor 
                if (nConstructor.objObjeto.ConstructorCriterio != null && nConstructor.objObjeto.ConstructorCriterio.Count > 0)//pregunta si tiene criterios va en otro metodo 
                {
                    constructorCriterios = lTrasformarConstructorCriterio(nConstructor.objObjeto.ConstructorCriterio.ToList());//trae la lista de Criterios DE N CRITERIOS CON N DETALLES DE AGRUPACION DENTRO
                    idCriterios = constructorCriterios.Select(x => x.IdCriterio).Distinct().ToList();//TRAE LOS ID CRITERIOS PARA BUSCARLOS EN SIGUIENTE METODO Y ELIMINAR LOS QUE HAN SIDO UTILIZADOS

                }

                //esto lo ocupo
                criterios = refCriterioBL.gListarCriteriosPorDireccion((int)nConstructor.objObjeto.IdDireccion,nConstructor.objObjeto.IdIndicador);

                //Elimina de la Lista el criterio ya usado 
                foreach (String itemCriterio in idCriterios)
                {
                    int indice = criterios.objObjeto.FindIndex(x => x.IdCriterio.Equals(itemCriterio));
                    if (indice > -1)
                    {
                        criterios.objObjeto.RemoveAt(indice);// ELIMINA EL CRITERIO UTILIZADO 
                    }
                }


                frecuencias = refFrecuenciaBL.gListar();//LISTAR FRECUENCIAS la ocupo en la primera pantalla

             
                // se Necesita Para la primera Pantalla 
                mvnuevoContructor.listaFrecuencia = new List<Frecuencia>(frecuencias.objObjeto.OrderBy(x => x.NombreFrecuencia));
                mvnuevoContructor.listaDesglose = new List<Frecuencia>(frecuencias.objObjeto.OrderBy(x => x.NombreFrecuencia));


                operadores = refOperadorBL.ConsultarTodos();//OPERADORES SE OCUPA EN LA TERCERA PANTALLA

                mvnuevoContructor.idOperador = "";


                //lista de criterios segunda Pantalla 
                mvnuevoContructor.listaCriterios = new List<Criterio>(criterios.objObjeto.OrderBy(x => x.DescCriterio));
                  
                //tercera Pantalla 
                mvnuevoContructor.listaOperadores = new List<Operador>(operadores.objObjeto.OrderBy(x => x.NombreOperador));
                mvnuevoContructor.constructor = nConstructor.objObjeto;
                Session["detallesAgrupacionPorOperador"] = null;
                Session["constructorCriterio"] = constructorCriterios;
                Session["idDireccion"] = nConstructor.objObjeto.IdDireccion;
                Session["idIndicador"] = nConstructor.objObjeto.IdIndicador;
                if (nConstructor.objObjeto.SolicitudConstructor != null && nConstructor.objObjeto.SolicitudConstructor.Count>0) {
                    ViewBag.Error = GB.SUTEL.Shared.ErrorTemplate.Constructor_EnSolicitudEditar;
                }
                return View(mvnuevoContructor);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return View();
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return View();
            }

        }

  
        // GET: /Constructor/Edit/5
        /// <summary>
        /// <method>NewMethod</method>
        /// Obtiene el constructor a editar por el id
        /// Para el view de editar ademas de poder
        /// cargar los combox 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        public ActionResult Editar(String id)
        {
            ConstructorViewModel mvnuevoContructor = new ConstructorViewModel();
            Respuesta<List<Frecuencia>> frecuencias = new Respuesta<List<Frecuencia>>();
            Respuesta<List<Frecuencia>> desgloses = new Respuesta<List<Frecuencia>>();
            Respuesta<Constructor> nConstructor = new Respuesta<Constructor>();
            try
            {
                ViewBag.Error = null;

                nConstructor = refConstructorBL.gObtenerConstructor(id);//obtengo el constructor 
                frecuencias = refFrecuenciaBL.gListar();//LISTAR FRECUENCIAS la ocupo en la primera pantalla

                // se Necesita Para la primera Pantalla 
                mvnuevoContructor.listaFrecuencia = new List<Frecuencia>(frecuencias.objObjeto.OrderBy(x => x.NombreFrecuencia));
                mvnuevoContructor.listaDesglose = new List<Frecuencia>(frecuencias.objObjeto.OrderBy(x => x.NombreFrecuencia));
                mvnuevoContructor.constructor = nConstructor.objObjeto;
                if (nConstructor.objObjeto.SolicitudConstructor != null && nConstructor.objObjeto.SolicitudConstructor.Count > 0)
                {
                    ViewBag.Error = GB.SUTEL.Shared.ErrorTemplate.Constructor_EnSolicitudEditar;
                }
                return View(mvnuevoContructor);
            }
            catch (Exception e)
            {
                ViewBag.Error = e;
                return View();
            }
          
        }

        // POST: /Constructor/Edit/5
        /// <summary>
        ///<method>NewMethod</method>
        /// Edita el constructor seleccionado solamenta la frecuencia 
        /// </summary>
        /// <param name="nuevoConstructor"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeUserAttribute]
        public ActionResult Editar(ConstructorViewModel nuevoConstructor)
        {
            Respuesta<Constructor> respuesta = new Respuesta<Constructor>();
            Respuesta<Constructor> constructorAnterior = new Respuesta<Constructor>();          
            List<ConstructorCriterio> constructorCriterio = new List<ConstructorCriterio>();
            Constructor nConstructor = new Constructor();
          
           
            try
            {
                string url2 = Request.Url.AbsolutePath.ToString();
                string[] tokens = url2.Split('/');
                string url3 = tokens[1];

                nConstructor = nuevoConstructor.constructor;               
                constructorAnterior = refConstructorBL.gObtenerConstructor(nuevoConstructor.constructor.IdConstructor.ToString());
                refConstructorBL = new ConstructorBL(AppContext);
                respuesta = refConstructorBL.gEditarConstructorfrecuenciayDesglose(nConstructor);// Metodo que hace la insercion y la validacion 
                if (respuesta.blnIndicadorTransaccion)
                {
                    //Inserción Bitácora
                    func.constructorbit(ActionsBinnacle.Editar, respuesta.objObjeto, constructorAnterior.objObjeto);
                    string id = nConstructor.IdConstructor.ToString();
                    TempData["Success"] = "Actualizado exitosamente";
                    return Redirect("EditarCriterio?id=" + id);
                }
                else
                {
                    TempData["Error"] = respuesta.strMensaje;
                    string id = nConstructor.IdConstructor.ToString();
                    return Redirect("Editar?id=" + id);
                }
            }
            catch (CustomException ce)
            {
                TempData["Error"] = respuesta.strMensaje;
                string id = nConstructor.IdConstructor.ToString();
                return Redirect("Editar?id=" + id);
            }
            catch (Exception ex)
            {

                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                AppContext.ExceptionBuilder.BuildException(msj, ex);
                TempData["Error"] = respuesta.strMensaje;
                string id = nConstructor.IdConstructor.ToString();
                return Redirect("Editar?id=" + id);
            }
        }

        /// <summary>
        /// <method>NewMethod</method>
        /// obtiene el id del url 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditarCriterio(String id)
        {
            ConstructorViewModel mvnuevoContructor = new ConstructorViewModel();           
            Respuesta<Constructor> nConstructor = new Respuesta<Constructor>();
            List<ConstructorCriterio> constructorCriterios = new List<ConstructorCriterio>();
            Respuesta<List<Criterio>> criterios = new Respuesta<List<Criterio>>();

            List<Criterio> listaCriterios = new List<Criterio>();
            List<String> idCriterios = new List<String>();
            nConstructor = refConstructorBL.gObtenerConstructor(id);

            try
            {

                if (nConstructor != null)
                {
                    constructorCriterios = refCriterioBL.gobtenerConstructorCriterioporId(id).objObjeto;
                }

                criterios = refCriterioBL.gListarCriteriosPorDireccion((int)nConstructor.objObjeto.IdDireccion, nConstructor.objObjeto.IdIndicador);//criterios

                if (constructorCriterios.Count > 0)
                {
                    idCriterios = constructorCriterios.Select(x => x.IdCriterio).Distinct().ToList();
                    foreach (String itemCriterio in idCriterios)
                    {
                        int indice = criterios.objObjeto.FindIndex(x => x.IdCriterio.Equals(itemCriterio));
                        if (indice > -1)
                        {
                            criterios.objObjeto.RemoveAt(indice);// ELIMINA EL CRITERIO UTILIZADO 
                        }
                    }
                }

                //lista de criterios segunda Pantalla 
                mvnuevoContructor.listaCriterios = new List<Criterio>(criterios.objObjeto.OrderBy(x => x.DescCriterio));
                mvnuevoContructor.constructor = nConstructor.objObjeto;
                // si tiene solicitud
                if (nConstructor.objObjeto.SolicitudConstructor != null && nConstructor.objObjeto.SolicitudConstructor.Count > 0)
                {
                    ViewBag.Error = GB.SUTEL.Shared.ErrorTemplate.Constructor_EnSolicitudEditar;
                }
                return View(mvnuevoContructor);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return View(mvnuevoContructor);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                                                          
                return View(mvnuevoContructor);
            }

        }

        // POST: /Constructor/Edit/5
        /// <summary>
        /// <method>NewMethod</method>
        /// Edita el constructor Para Agregar 
        /// un nuevo Criterio
        /// </summary>
        /// <param name="nuevoConstructor"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeUserAttribute]
        public ActionResult EditarCriterio(FormCollection form)
        {


            ConstructorViewModel mvnuevoContructor = new ConstructorViewModel();
            Respuesta<Constructor> nConstructor = new Respuesta<Constructor>();
            ConstructorCriterio ConstructorCriterio = new ConstructorCriterio();
            Respuesta<bool> respuesta = new Respuesta<bool>();

            string idCriterio = ""; 
            string TextAyuda = ""; 
            string idconstructor = "";
            //asignando los datos del formulario 
            idCriterio = Convert.ToString(form["nombreCriterio"]);//obtengo los valores del form
            TextAyuda = Convert.ToString(form["criterio.Ayuda"]);//obtengo los valores del form
            idconstructor = Convert.ToString(form["constructor.IdConstructor"]);//obtengo los valores del form
            nConstructor = refConstructorBL.gObtenerConstructor(idconstructor);
            if (idCriterio == null || idCriterio == "")
            {
                TempData["Error"] = "Por favor seleccione un criterio";
                string id = nConstructor.objObjeto.IdConstructor.ToString();
                return Redirect("EditarCriterio?id=" + id);
            }

            if (idCriterio == null || idCriterio == null)
            {
                TempData["Error"] = "Por favor digite la ayuda";
                string id = nConstructor.objObjeto.IdConstructor.ToString();
                return Redirect("EditarCriterio?id=" + id);
            }          
             try
            {              
                //transformo el string a GUID
                Guid IDConstructor = new Guid(idconstructor);
                 //asigno los valores al obj
                ConstructorCriterio.IdConstructor = IDConstructor;
                ConstructorCriterio.Ayuda = TextAyuda;
                ConstructorCriterio.IdCriterio = idCriterio;
                respuesta=refConstructorBL.gAgregarConstructorCriterio(ConstructorCriterio);
                string user;
                user = User.Identity.GetUserId();
                //Bitacora
                try
                {
                    func.constructor(user, "Creacion de Mantenimiento Constructor/Ayuda", 2,"Id Constructor: "+IDConstructor.ToString()+ " Id Criterio: " + idCriterio.ToString()+" Texto de ayuda: " + TextAyuda.ToString(),"");
                }
                catch
                {

                }
                if (respuesta.objObjeto)
                {
                     TempData["Error"] = respuesta.strMensaje;
                     return Redirect("EditarOperador?id=" + idconstructor + "&idcriterio=" + idCriterio);
                   // return Redirect("EditarCriterio?id=" + idconstructor);
                }
                else
                {
                         
                    TempData["Error"] = respuesta.strMensaje;
                    string id = nConstructor.objObjeto.IdConstructor.ToString();
                    return Redirect("EditarCriterio?id=" + id);
                   

                }
                //si todo sale bien  
               
            }
             catch (CustomException cEx)
             {
                 ViewBag.Error = cEx;             
                 string id = nConstructor.objObjeto.IdConstructor.ToString();
                 return Redirect("EditarCriterio?id=" + id);
             }
             catch (Exception ex)
             {
                 string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                 var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                 ViewBag.Error = ((CustomException)newEx);      
                 string id = nConstructor.objObjeto.IdConstructor.ToString();



            
                return Redirect("EditarCriterio?id=" + id);
             }


        }

        
        /// <summary>
        /// <method>NewMethod</method>
        /// Edita el constructor Para Agregar 
        /// un nuevo Criterio
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idcriterio"></param>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        public ActionResult EditarOperador(String id, String idcriterio)
        {
            ConstructorViewModel mvnuevoContructor = new ConstructorViewModel();
            Respuesta<Constructor> nConstructor = new Respuesta<Constructor>();   
      
            nConstructor = refConstructorBL.gObtenerConstructor(id);
            try
            {       
                



                mvnuevoContructor.constructor = nConstructor.objObjeto;


                //Bitacora
                string user;
                user = User.Identity.GetUserId();
                try
                {
                    func._index(user, "Constructor", "Mantenimiento de Constructor");
                }
                catch
                {

                }
                if (nConstructor.objObjeto.SolicitudConstructor != null && nConstructor.objObjeto.SolicitudConstructor.Count > 0)
                {
                    ViewBag.Error = GB.SUTEL.Shared.ErrorTemplate.Constructor_EnSolicitudEditar;
                }
                return View(mvnuevoContructor);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return View(mvnuevoContructor);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);

          


                return View(mvnuevoContructor);
            }
        }

        /// <summary>
        ///  <method>NewMethod</method>
        ///  Se edita el arbol 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idcriterio"></param>
        /// <param name="Operador"></param>
        /// <returns></returns>
        public ActionResult EditarArbol(String id, String idcriterio, String idoperador)
        {
            ConstructorViewModel mvnuevoContructor = new ConstructorViewModel();
            Respuesta<Constructor> nConstructor = new Respuesta<Constructor>();
            nConstructor = refConstructorBL.gObtenerConstructor(id);
            mvnuevoContructor.constructor = nConstructor.objObjeto;
            mvnuevoContructor.idCriterio = idcriterio;
            if (nConstructor.objObjeto.SolicitudConstructor != null && nConstructor.objObjeto.SolicitudConstructor.Count > 0)
            {
                ViewBag.Error = GB.SUTEL.Shared.ErrorTemplate.Constructor_EnSolicitudEditar;
            }
            //Bitacora
            string user;
            user = User.Identity.GetUserId();
            try
            {
                func.constructor(user, "Proceso de edición en Mantenimiento Constructor/Arbol", 3, "ID Constructor: " + id.ToString() + "ID Criterio: " + idcriterio.ToString()+"Operador: "+ idoperador.ToString(), "ID Constructor: " + nConstructor.objObjeto.ToString() + "ID Criterio: " + idcriterio.ToString());
                
            }
            catch
            {

            }

            return View(mvnuevoContructor);
        }
        /// <summary>
        /// se crea el arbol 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idcriterio"></param>
        /// <returns></returns>
        public ActionResult CrearArbol(String id, String idcriterio)
        {
            ConstructorViewModel mvnuevoContructor = new ConstructorViewModel();
            Respuesta<Constructor> nConstructor = new Respuesta<Constructor>();
            nConstructor = refConstructorBL.gObtenerConstructor(id);
            
            mvnuevoContructor.constructor = nConstructor.objObjeto;
            mvnuevoContructor.idCriterio = idcriterio;
            //Bitacora
            string user;
            user = User.Identity.GetUserId();
            try
            {
                func.constructor(user, "Proceso de consulta en: Mantenimiento Constructor/Arbol",5, "ID Criterio: "+idcriterio.ToString()," ");
            }
            catch
            {

            }

            return View(mvnuevoContructor);
        }
        // POST: /Constructor/Edit/5
        /// <summary>
        /// Edita el constructor seleccionado
        /// </summary>
        /// <param name="nuevoConstructor"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeUserAttribute]
        public String EditarOriginal(ConstructorViewModel nuevoConstructor)
        {
            Respuesta<Constructor> respuesta = new Respuesta<Constructor>();
            Respuesta<Constructor> constructorAnterior = new Respuesta<Constructor>();
            JSONResult<Constructor> jsonRespuesta = new JSONResult<Constructor>();
            List<ConstructorCriterio> constructorCriterio = new List<ConstructorCriterio>();
            Constructor nConstructor = new Constructor();
            jsonRespuesta.ok = false;
            try
            {
                if (Session["constructorCriterio"] != null)
                {
                    constructorCriterio = (List<ConstructorCriterio>)Session["constructorCriterio"];
                }
                nConstructor = nuevoConstructor.constructor;
                nConstructor.ConstructorCriterio = constructorCriterio;
                constructorAnterior = refConstructorBL.gObtenerConstructor(nuevoConstructor.constructor.IdConstructor.ToString());
                refConstructorBL = new ConstructorBL(AppContext);
                respuesta = refConstructorBL.gEditarConstructor(nConstructor);// Metodo que hace la insercion y la validacion 
                if (respuesta.blnIndicadorTransaccion)
                {
                    Session.Remove("detallesAgrupacionPorOperador");
                    Session.Remove("constructorCriterio");
                    Session.Remove("idDireccion");
                    Session.Remove("idIndicador");
                    //Inserción Bitácora
                    func.constructorbit(ActionsBinnacle.Editar, respuesta.objObjeto, constructorAnterior.objObjeto);


                    jsonRespuesta.data = respuesta.objObjeto;
                    jsonRespuesta.ok = true;
                    return jsonRespuesta.toJSONLoopHandlingIgnore();
                }
                else
                {
                    jsonRespuesta.ok = false;
                    jsonRespuesta.strMensaje = respuesta.strMensaje;
                    jsonRespuesta.data = respuesta.objObjeto;
                    return jsonRespuesta.toJSON();
                }
            }
            catch (CustomException ce)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = ce.Message;
                jsonRespuesta.data = new Constructor();
                return jsonRespuesta.toJSON();
            }
            catch (Exception ex)
            {

                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                AppContext.ExceptionBuilder.BuildException(msj, ex);
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = msj;
                jsonRespuesta.data = new Constructor();



                return jsonRespuesta.toJSON();

            }
        }

        #endregion

        #region eliminar
        //
        // GET: /Constructor/Delete/5
        /// <summary>
        /// Obtiene el constructor a eliminar
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
         [AuthorizeUserAttribute]
        public String Eliminar(int id)
        {
            JSONResult<Constructor> jsonRespuesta = new JSONResult<Constructor>();
            try
            {
                jsonRespuesta.ok = true;

                jsonRespuesta.data = new Constructor();
                return jsonRespuesta.toJSON();
            }
            catch (CustomException ce)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = ce.Message;
                jsonRespuesta.data = new Constructor();
                return jsonRespuesta.toJSON();
            }
            catch (Exception ex)
            {

                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                AppContext.ExceptionBuilder.BuildException(msj, ex);
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = msj;
                jsonRespuesta.data = new Constructor();
                return jsonRespuesta.toJSON();

            }
        }

        //
        // POST: /Constructor/Delete/5
        /// <summary>
        /// Elimina el constructor si no se presenta algún problema
        /// </summary>
        /// <param name="constructor"></param>
        /// <returns></returns>
         [AuthorizeUserAttribute]
        [HttpPost]
        public string Eliminar(Constructor constructor)
        {
            Respuesta<Constructor> respuesta = new Respuesta<Constructor>();
            JSONResult<Constructor> jsonRespuesta = new JSONResult<Constructor>();
            try
            {
                respuesta = refConstructorBL.gEliminarConstructor(constructor);

                if (respuesta.blnIndicadorTransaccion)
                {
                   func.constructorbit(ActionsBinnacle.Borrar, null, respuesta.objObjeto);
                    jsonRespuesta.data = new Constructor();
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
            catch (CustomException ce)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = ce.Message;
                jsonRespuesta.data = new Constructor();
                return jsonRespuesta.toJSON();
            }
            catch (Exception ex)
            {

                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                AppContext.ExceptionBuilder.BuildException(msj, ex);
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = msj;
                jsonRespuesta.data = new Constructor();
               
                return jsonRespuesta.toJSON();

            }
        }
        #endregion
        #endregion

        #region indicador
        /// <summary>
        /// tabla de indicadores 
        /// </summary>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        [HttpGet]
        public ActionResult _tablaIndicador()
        {
            Respuesta<List<Indicador>> respuesta = new Respuesta<List<Indicador>>();
            Direccion direccion = new Direccion();
            try
            {


                respuesta = refIndicadorBL.gObtenerIndicadorPorDireccion(direccion == null ? 0 : direccion.IdDireccion);
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
        /// tabla de indicadores por dirección
        /// </summary>
        /// <param name="direccion"></param>
        /// <returns></returns>
         [AuthorizeUserAttribute]
        [HttpPost]
        public ActionResult _tablaIndicador(Direccion direccion)
        {
            Respuesta<List<Indicador>> respuesta = new Respuesta<List<Indicador>>();

            try
            {


                respuesta = refIndicadorBL.gObtenerIndicadorPorDireccion(direccion == null ? 0 : direccion.IdDireccion);
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
        /// Filtro de indicadores
        /// </summary>
        /// <param name="poFiltro"></param>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        [HttpPost]
        public ActionResult _filtroIndicador(FiltroIndicadorViewModel poFiltro)
        {
            Respuesta<List<Indicador>> respuesta = new Respuesta<List<Indicador>>();

            try
            {


                respuesta = refIndicadorBL.gFiltroIndicador(poFiltro.idDireccion, (poFiltro.codigoIndicador == null ? "" : poFiltro.codigoIndicador),
                    (poFiltro.nombreTipoIndicador == null ? "" : poFiltro.nombreTipoIndicador), (poFiltro.nombreIndicador == null ? "" : poFiltro.nombreIndicador));
                return PartialView("_tablaIndicador", respuesta.objObjeto);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView("_tablaIndicador", respuesta.objObjeto);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView("_tablaIndicador", respuesta.objObjeto);
            }
        }






        #endregion

        #region operador

        /// <summary>
        /// lista de operadores
        /// </summary>
        /// <returns></returns>
         [AuthorizeUserAttribute]
        public ActionResult _tablaOperador()
        {
            Respuesta<List<Operador>> respuesta = new Respuesta<List<Operador>>();

            try
            {
                respuesta = refOperadorBL.ConsultarTodos();
                return PartialView("_tablaOperador", respuesta.objObjeto);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView("_tablaOperador", respuesta.objObjeto);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView("_tablaOperador", respuesta.objObjeto);
            }
        }

         /// <summary>
         /// lista de operadores
         /// </summary>
         /// <returns></returns>
         [AuthorizeUserAttribute]
         public ActionResult _tablaOperadorEditar(String id, String idcriterio)
         {
             Respuesta<List<Operador>> detallesAgrupacionPorOperador = new Respuesta<List<Operador>>();
             List<Operador> detalles = new List<Operador>();
             Guid IdConstructor;
             IdConstructor = new Guid(id);

             Respuesta<List<Operador>> respuesta = new Respuesta<List<Operador>>();
             List<Operador> operadores = new List<Operador>();
             Operador operador = new Operador();
             int indiceOperador = 0;
             List<String> idOperadores = new List<String>();
             try
             {
                 detallesAgrupacionPorOperador = refConstructorBL.gObtenerOperadoresDetalleAgrupacion(IdConstructor, idcriterio);
                 if (detallesAgrupacionPorOperador != null && detallesAgrupacionPorOperador.objObjeto.Count != 0)
                 {
                     detalles = detallesAgrupacionPorOperador.objObjeto;
                 }
                 if (detalles.Count > 0)
                 {
                     idOperadores = detalles.Select(x => x.IdOperador).Distinct().ToList();
                 }
                 respuesta = refOperadorBL.ConsultarTodos();
                 foreach (Operador item in respuesta.objObjeto)
                 {
                     indiceOperador = idOperadores.FindIndex(y => y.Equals(item.IdOperador));
                     if (indiceOperador < 0)
                     {
                         operador = new Operador();
                         operador.IdOperador = item.IdOperador;
                         operador.NombreOperador = item.NombreOperador;
                         operadores.Add(operador);
                     }
                 }

                 return PartialView("_tablaOperadorEditar", operadores);
             }
             catch (CustomException cEx)
             {
                 ViewBag.Error = cEx;
                 return PartialView("_tablaOperadorEditar", respuesta.objObjeto);
             }
             catch (Exception ex)
             {
                 string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                 var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                 ViewBag.Error = ((CustomException)newEx);

                         return PartialView("_tablaOperadorEditar", respuesta.objObjeto);
             }
         }

        /// <summary>
        /// tabla de operadores en criterio por sus detalles agrupación
        /// </summary>
        /// <returns></returns>
         [AuthorizeUserAttribute]
        public ActionResult _tablaOperadorCriterio()
        {
            List<ConstructorCriterioDetalleAgrupacion> detalles = new List<ConstructorCriterioDetalleAgrupacion>();
            Respuesta<List<Operador>> respuesta = new Respuesta<List<Operador>>();
            List<Operador> operadores = new List<Operador>();
            Operador operador = new Operador();
            int indiceOperador = 0;
            List<String> idOperadores = new List<String>();
            try
            {
                if (Session["detallesAgrupacionPorOperador"] != null)
                {
                    detalles = (List<ConstructorCriterioDetalleAgrupacion>)Session["detallesAgrupacionPorOperador"];
                }
                if (detalles.Count > 0)
                {
                    idOperadores = detalles.Select(x => x.IdOperador).Distinct().ToList();
                }
                respuesta = refOperadorBL.ConsultarTodos();
                foreach (Operador item in respuesta.objObjeto)
                {
                    indiceOperador = idOperadores.FindIndex(y => y.Equals(item.IdOperador));
                    if (indiceOperador < 0)
                    {
                        operador = new Operador();
                        operador.IdOperador = item.IdOperador;
                        operador.NombreOperador = item.NombreOperador;
                        operadores.Add(operador);
                    }
                }

                return PartialView("_tablaOperador", operadores);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView("_tablaOperador", respuesta.objObjeto);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView("_tablaOperador", respuesta.objObjeto);
            }
        }

                 [AuthorizeUserAttribute]
         public ActionResult _tablaOperadorCriterioEditar(String id, String idcriterio)
        {
            Respuesta<List<Operador>> detallesAgrupacionPorOperador = new Respuesta<List<Operador>>();
            List<Operador> detalles = new List<Operador>();
            Guid IdConstructor;
            IdConstructor = new Guid(id);

            Respuesta<List<Operador>> respuesta = new Respuesta<List<Operador>>();
            List<Operador> operadores = new List<Operador>();
            Operador operador = new Operador();
            int indiceOperador = 0;
            List<String> idOperadores = new List<String>();
            try
            {
                detallesAgrupacionPorOperador = refConstructorBL.gObtenerOperadoresDetalleAgrupacion(IdConstructor, idcriterio);
                if (detallesAgrupacionPorOperador != null && detallesAgrupacionPorOperador.objObjeto.Count != 0)
                {
                    detalles = detallesAgrupacionPorOperador.objObjeto;
                }
                if (detalles.Count > 0)
                {
                    idOperadores = detalles.Select(x => x.IdOperador).Distinct().ToList();
                }
                respuesta = refOperadorBL.ConsultarTodos();
                foreach (Operador item in respuesta.objObjeto)
                {
                    indiceOperador = idOperadores.FindIndex(y => y.Equals(item.IdOperador));
                    if (indiceOperador < 0)
                    {
                        operador = new Operador();
                        operador.IdOperador = item.IdOperador;
                        operador.NombreOperador = item.NombreOperador;
                        operadores.Add(operador);
                    }
                }

                return PartialView("_tablaOperadorEditar", operadores);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView("_tablaOperadorEditar", respuesta.objObjeto);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView("_tablaOperadorEditar", respuesta.objObjeto);
            }
        }


         [AuthorizeUserAttribute]
        public ActionResult _filtroOperador(Operador filtroOperador)
        {
            List<ConstructorCriterioDetalleAgrupacion> detalles = new List<ConstructorCriterioDetalleAgrupacion>();
            Respuesta<List<Operador>> respuesta = new Respuesta<List<Operador>>();
            List<Operador> operadores = new List<Operador>();
            Operador operador = new Operador();
            int indiceOperador = 0;
            List<String> idOperadores = new List<String>();
            try
            {
                if (Session["detallesAgrupacionPorOperador"] != null)
                {
                    detalles = (List<ConstructorCriterioDetalleAgrupacion>)Session["detallesAgrupacionPorOperador"];
                }
                if (detalles.Count > 0)
                {
                    idOperadores = detalles.Select(x => x.IdOperador).Distinct().ToList();
                }
                respuesta = refOperadorBL.gFiltrarOperadores((filtroOperador.IdOperador == null ? "" : filtroOperador.IdOperador), (filtroOperador.NombreOperador == null ? "" : filtroOperador.NombreOperador));
                foreach (Operador item in respuesta.objObjeto)
                {
                    indiceOperador = idOperadores.FindIndex(y => y.Equals(item.IdOperador));
                    if (indiceOperador < 0)
                    {
                        operador = new Operador();
                        operador.IdOperador = item.IdOperador;
                        operador.NombreOperador = item.NombreOperador;
                        operadores.Add(operador);
                    }
                }

                return PartialView("_tablaOperador", operadores);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView("_tablaOperador", respuesta.objObjeto);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView("_tablaOperador", respuesta.objObjeto);
            }
        }

        /// <summary>
        /// <METHOD>NEWM</METHOD>
        /// filtro de operadores 
        /// </summary>
        /// <param name="filtroOperador"></param>
        /// <returns></returns>
         [AuthorizeUserAttribute]
         public ActionResult _filtroOperadorEditar(DetalleAgrupacionyurl filtroOperador)
         {
             
             List<Operador> operadores = new List<Operador>();      
             List<String> idOperadores = new List<String>();
             int indiceOperador = 0;
             Operador operador = new Operador();
             Respuesta<List<Operador>> respuesta = new Respuesta<List<Operador>>();
             List<Operador> detallesAgrupacionPorOperador = new List<Operador>();
             Respuesta<List<Operador>> detalles = new Respuesta<List<Operador>>();
             string IDConstructor = filtroOperador.idConstructor;         
             string IDCriterio = filtroOperador.idCriterio;
             Guid IdConstructor;
             IdConstructor = new Guid(IDConstructor);

             try
             {
                 detalles = refConstructorBL.gObtenerOperadoresDetalleAgrupacion(IdConstructor, IDCriterio);
                 respuesta = refOperadorBL.ConsultarTodos();
                 if (detalles.objObjeto != null && detalles.objObjeto.Count > 0)
                 {
                     if (detalles.objObjeto.Count > 0)
                     {
                         idOperadores = detalles.objObjeto.Select(x => x.IdOperador).Distinct().ToList();
                     }

                    
                     foreach (Operador item in respuesta.objObjeto)
                     {
                         indiceOperador = idOperadores.FindIndex(y => y.Equals(item.IdOperador));
                         if (indiceOperador < 0)
                         {
                             operador = new Operador();
                             operador.IdOperador = item.IdOperador;
                             operador.NombreOperador = item.NombreOperador;
                             operadores.Add(operador);
                         }
                     }
                 }
                 else
                 {
                     operadores = respuesta.objObjeto;
                 }


                 if (operadores.Count > 0)
                 {
                               
                     if (filtroOperador.idOperador != null && filtroOperador.NombreOperador != null)
                     {
                         detallesAgrupacionPorOperador = operadores.Where(x => x.IdOperador.Contains(filtroOperador.idOperador)).ToList();
                         detallesAgrupacionPorOperador = detallesAgrupacionPorOperador.Where(x => x.NombreOperador.ToUpper().Contains(filtroOperador.NombreOperador.ToUpper())).ToList();
                     }
                     else
                     {
                         if (filtroOperador.idOperador != null)
                         {
                             detallesAgrupacionPorOperador = operadores.Where(x => x.IdOperador.Contains(filtroOperador.idOperador)).ToList();                    
                         }

                         if (filtroOperador.NombreOperador != null)
                         {
                             detallesAgrupacionPorOperador = operadores.Where(x => x.NombreOperador.ToUpper().Contains(filtroOperador.NombreOperador.ToUpper())).ToList();
                         }
                     }

                     if (filtroOperador.idOperador == null && filtroOperador.NombreOperador == null)
                     {
                         detallesAgrupacionPorOperador = operadores;                       
                     }
                 }


                 return PartialView("_tablaOperadorEditar", detallesAgrupacionPorOperador);
             }
             catch (CustomException cEx)
             {
                 ViewBag.Error = cEx;
                 return PartialView("_tablaOperadorEditar", detalles.objObjeto);
             }
             catch (Exception ex)
             {
                 string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                 var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                 ViewBag.Error = ((CustomException)newEx);
                 return PartialView("_tablaOperador", detalles.objObjeto);
             }
         }

        #endregion

        #region criterio

         /// <summary>
         /// tabla de criterios
         /// </summary>
         /// <returns></returns>
         [AuthorizeUserAttribute]
         [HttpGet]
         public ActionResult _tablaCriterio()
         {
             Respuesta<List<Criterio>> respuesta = new Respuesta<List<Criterio>>();
             List<ConstructorCriterio> constructorCriterios = new List<ConstructorCriterio>();
             List<Criterio> listaCriterios = new List<Criterio>();

             List<String> idCriterios = new List<String>();
             Direccion direccion = new Direccion();
             int idDireccion = 0;
             String idIndicador = "";
             int indiceCriterio = 0;
             try
             {
                 if (Session["idDireccion"] != null)
                 {
                     idDireccion = (int)Session["idDireccion"];
                 }
                 if (Session["idIndicador"] != null)
                 {
                     idIndicador = (string)Session["idIndicador"];
                 }
                 if (Session["constructorCriterio"] != null)
                 {
                     constructorCriterios = (List<ConstructorCriterio>)Session["constructorCriterio"];
                 }
                 respuesta = refCriterioBL.gListarCriteriosPorDireccion(idDireccion, idIndicador);
                 if (constructorCriterios.Count > 0)
                 {
                     idCriterios = constructorCriterios.Select(x => x.IdCriterio).Distinct().ToList();
                     foreach (Criterio item in respuesta.objObjeto)
                     {
                         indiceCriterio = idCriterios.FindIndex(y => y.Equals(item.IdCriterio));
                         if (indiceCriterio < 0)
                         {
                             listaCriterios.Add(item);
                         }
                     }
                 }
                 else
                 {
                     listaCriterios.AddRange(respuesta.objObjeto);
                 }



                 return PartialView(listaCriterios);
             }
             catch (CustomException cEx)
             {
                 ViewBag.Error = cEx;
                 return PartialView(listaCriterios);
             }
             catch (Exception ex)
             {
                 string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                 var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                 ViewBag.Error = ((CustomException)newEx);
                 return PartialView(listaCriterios);
             }
         }

        /// <summary>
        /// <Method>NewM</Method>
        /// tabla de criterios
        /// Metodo Modifacado 
        /// </summary>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        [HttpGet]
        public ActionResult _tablaCriterioEditar()
        {
            string url = Request.Url.ToString();           
            string[] tokens = url.Split('=');
            string id = tokens[1];
        
            Respuesta<List<Criterio>> respuesta = new Respuesta<List<Criterio>>();
            List<ConstructorCriterio> constructorCriterios = new List<ConstructorCriterio>();
            Respuesta<Constructor> nConstructor = new Respuesta<Constructor>();
         
            List<Criterio> listaCriterios = new List<Criterio>();
            List<String> idCriterios = new List<String>();

            int idDireccion = 0;
            String idIndicador = "";
            int indiceCriterio = 0;          
            nConstructor = refConstructorBL.gObtenerConstructor(id);
            try
            {
                if (nConstructor != null)
                {
                    idDireccion =(int) nConstructor.objObjeto.IdDireccion;
                }
                if (nConstructor != null)
                {
                    idIndicador = (String)nConstructor.objObjeto.IdIndicador;
                }
                if (nConstructor != null)
                {
                    constructorCriterios = refCriterioBL.gobtenerConstructorCriterioporId(id).objObjeto;
                }

              
                respuesta = refCriterioBL.gListarCriteriosPorDireccion(idDireccion, idIndicador);

                if (constructorCriterios.Count > 0)
                {
                    idCriterios = constructorCriterios.Select(x => x.IdCriterio).Distinct().ToList();
                     foreach (Criterio item in respuesta.objObjeto)
                                    {
                                        indiceCriterio = idCriterios.FindIndex(y => y.Equals(item.IdCriterio));
                                        if (indiceCriterio < 0)
                                        {
                                            listaCriterios.Add(item);
                                        }
                       }
                }
                else
                {
                    listaCriterios.AddRange(respuesta.objObjeto);
                }
             
                return PartialView(listaCriterios);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView(listaCriterios);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView(listaCriterios);
            }
        }

        /// <summary>
        /// tabla de criterios por dirección
        /// </summary>
        /// <param name="direccion"></param>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        [HttpPost]
        public ActionResult _tablaCriterio(FiltroCriterio poFiltroCriterio)
        {
            Respuesta<List<Criterio>> respuesta = new Respuesta<List<Criterio>>();

            try
            {


                respuesta = refCriterioBL.gListarCriteriosPorDireccion(poFiltroCriterio.idDireccion == null ? 0 : poFiltroCriterio.idDireccion
                                                                        , poFiltroCriterio.idIndicador == null ?"" : poFiltroCriterio.idIndicador);
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
        /// Obtiene aquellos  criterios que hay no sido seleccionados
        /// </summary>
        /// <param name="poDireccion"></param>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        public ActionResult gMostrarCriterioNoSeleccionados(FiltroCriterio poCriterio)
        {
            Respuesta<List<Criterio>> respuesta = new Respuesta<List<Criterio>>();
            List<ConstructorCriterio> constructorCriterios = new List<ConstructorCriterio>();
            List<Criterio> listaCriterios = new List<Criterio>();

            List<String> idCriterios = new List<String>();
            Direccion direccion = new Direccion();
            int idDireccion = poCriterio.idDireccion == null ? 0 : poCriterio.idDireccion;
            int indiceCriterio = 0;
            try
            {

                if (Session["constructorCriterio"] != null)
                {
                    constructorCriterios = (List<ConstructorCriterio>)Session["constructorCriterio"];
                }
                respuesta = refCriterioBL.gListarCriteriosPorDireccion(idDireccion, (poCriterio.idIndicador == null ? "" : poCriterio.idIndicador));
                if (constructorCriterios.Count > 0)
                {
                    idCriterios = constructorCriterios.Select(x => x.IdCriterio).Distinct().ToList();
                }
                else
                {
                    listaCriterios.AddRange(respuesta.objObjeto);
                }


                foreach (Criterio item in respuesta.objObjeto)
                {
                    indiceCriterio = idCriterios.FindIndex(y => y.Equals(item.IdCriterio));
                    if (indiceCriterio < 0)
                    {
                        listaCriterios.Add(item);
                    }
                }
                return PartialView("_tablaCriterio", listaCriterios);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView("_tablaCriterio", listaCriterios);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView("_tablaCriterio", listaCriterios);
            }
        }

        /// <summary>
        /// agrega el criterio al constructor temporalmente
        /// </summary>
        /// <param name="criterio"></param>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        public ActionResult _agregarCriterio(Criterio criterio)
        {
   
        List<ConstructorCriterioDetalleAgrupacion> detallesAgrupacionPorOperador = new List<ConstructorCriterioDetalleAgrupacion>();
            List<ConstructorCriterio> constructorCriterio = new List<ConstructorCriterio>();
            ConstructorCriterio nuevoConstructorCriterio = new ConstructorCriterio();
            string[] descripciones = criterio.DescCriterio.Split('|');
            if (Session["constructorCriterio"] != null)
            {
                constructorCriterio = (List<ConstructorCriterio>)Session["constructorCriterio"];
            }
            if (Session["detallesAgrupacionPorOperador"] != null)
            {
                detallesAgrupacionPorOperador = (List<ConstructorCriterioDetalleAgrupacion>)Session["detallesAgrupacionPorOperador"];
            }
            nuevoConstructorCriterio.ConstructorCriterioDetalleAgrupacion = new List<ConstructorCriterioDetalleAgrupacion>();
            nuevoConstructorCriterio.ConstructorCriterioDetalleAgrupacion = detallesAgrupacionPorOperador;
            nuevoConstructorCriterio.Criterio = new Criterio();
            nuevoConstructorCriterio.Criterio.DescCriterio = descripciones[0];
            nuevoConstructorCriterio.Criterio.IdCriterio = criterio.IdCriterio;
            nuevoConstructorCriterio.IdCriterio = criterio.IdCriterio;
            nuevoConstructorCriterio.Ayuda = descripciones[1];
            constructorCriterio.Add(nuevoConstructorCriterio);
            Session["detallesAgrupacionPorOperador"] = null;
            Session["constructorCriterio"] = constructorCriterio;



  



            return PartialView("_tablaCriterioConstructor", constructorCriterio);
        }

        /// <summary>
        /// Elimina los detalles agrupación por operador
        /// </summary>
        /// <param name="operador"></param>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        public ActionResult _criterioEliminar(Criterio criterio)
        {
            List<ConstructorCriterio> constructorCriterio = new List<ConstructorCriterio>();
            try
            {

                if (Session["constructorCriterio"] != null)
                {
                    constructorCriterio = (List<ConstructorCriterio>)Session["constructorCriterio"];
                }
                foreach (ConstructorCriterio item in constructorCriterio)
                {
                    if (item.IdCriterio.Equals(criterio.IdCriterio))
                    {
                        constructorCriterio.Remove(item);
                        break;
                    }
                }
                Session["constructorCriterio"] = constructorCriterio;
                return PartialView("_tablaCriterioConstructor", constructorCriterio);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView("_tablaCriterioConstructor", constructorCriterio);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);


           


                return PartialView("_tablaCriterioConstructor", constructorCriterio);
            }
        }

        /// <summary>
        /// <Method>newM</Method>
        /// Elimina los detalles agrupación por operador
        /// </summary>
        /// <param name="operador"></param>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        public ActionResult _criterioEliminarEditar(ConstructorCriterio criterio)
        {
            List<ConstructorCriterio> criterioConstructor = new List<ConstructorCriterio>();
            Respuesta<bool> respuesta = new Respuesta<bool>();
            string id = criterio.IdConstructor.ToString();
            //criterioConstructor = refCriterioBL.gobtenerConstructorCriterioporId(id).objObjeto;
           
         
            try
            {
                  
                   respuesta = refConstructorBL.gEliminarConstructorCriterio(criterio);
                   if (respuesta.objObjeto == true)
                   {
                       return PartialView("_tablaCriterioConstructorEditar", new Tuple<string, List<ConstructorCriterio>>(id, criterioConstructor));
                   }
                   else
                   {
                       return PartialView("_tablaCriterioConstructorEditar", new Tuple<string, List<ConstructorCriterio>>(id, criterioConstructor));
                   }
               
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView("_tablaCriterioConstructorEditar", new Tuple<string, List<ConstructorCriterio>>(id, criterioConstructor));
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);




 



                return PartialView("_tablaCriterioConstructorEditar", new Tuple<string, List<ConstructorCriterio>>(id, criterioConstructor));
            }
        }

        /// <summary>
        /// Edita el criterio
        /// </summary>
        /// <param name="criterio"></param>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        public ActionResult _criterioEditar(Criterio criterio)
        {
            List<ConstructorCriterio> constructorCriterio = new List<ConstructorCriterio>();
            List<ConstructorCriterioDetalleAgrupacion> detalles = new List<ConstructorCriterioDetalleAgrupacion>();
            ConstructorCriterio criterioSeleccionado = new ConstructorCriterio();
            try
            {

                if (Session["constructorCriterio"] != null)
                {
                    constructorCriterio = (List<ConstructorCriterio>)Session["constructorCriterio"];
                }
                foreach (ConstructorCriterio item in constructorCriterio)
                {
                    if (item.IdCriterio.Equals(criterio.IdCriterio))
                    {
                        criterioSeleccionado = item;
                        break;
                    }
                }
                detalles = criterioSeleccionado.ConstructorCriterioDetalleAgrupacion.ToList();
                Session["detallesAgrupacionPorOperador"] = detalles;
                return PartialView("_tablaDetalleAgrupacionOperador", detalles);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView("_tablaDetalleAgrupacionOperador", detalles);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                
                
         

                return PartialView("_tablaDetalleAgrupacionOperador", detalles);
            }
        }
        /// <summary>
        /// Guarda el criterio en edición en el constructor
        /// </summary>
        /// <param name="criterio"></param>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        public ActionResult _criterioGuardarEditar(Criterio criterio)
        {
            List<ConstructorCriterioDetalleAgrupacion> detallesAgrupacionPorOperador = new List<ConstructorCriterioDetalleAgrupacion>();
            List<ConstructorCriterio> constructorCriterio = new List<ConstructorCriterio>();
            ConstructorCriterio nuevoConstructorCriterio = new ConstructorCriterio();
            string[] descripciones = criterio.DescCriterio.Split('|');
            if (Session["constructorCriterio"] != null)
            {
                constructorCriterio = (List<ConstructorCriterio>)Session["constructorCriterio"];
            }
            if (Session["detallesAgrupacionPorOperador"] != null)
            {
                detallesAgrupacionPorOperador = (List<ConstructorCriterioDetalleAgrupacion>)Session["detallesAgrupacionPorOperador"];
            }
            foreach (ConstructorCriterio item in constructorCriterio)
            {
                if (item.IdCriterio.Equals(criterio.IdCriterio))
                {
                    constructorCriterio.Remove(item);
                    break;
                }
            }

            nuevoConstructorCriterio.ConstructorCriterioDetalleAgrupacion = new List<ConstructorCriterioDetalleAgrupacion>();
            nuevoConstructorCriterio.ConstructorCriterioDetalleAgrupacion = detallesAgrupacionPorOperador;
            nuevoConstructorCriterio.Criterio = new Criterio();
            nuevoConstructorCriterio.Criterio.DescCriterio = descripciones[0];
            nuevoConstructorCriterio.Criterio.IdCriterio = criterio.IdCriterio;
            nuevoConstructorCriterio.IdCriterio = criterio.IdCriterio;
            nuevoConstructorCriterio.Ayuda = descripciones[1];
            constructorCriterio.Add(nuevoConstructorCriterio);
            Session["detallesAgrupacionPorOperador"] = new List<ConstructorCriterioDetalleAgrupacion>();
            Session["constructorCriterio"] = constructorCriterio;
   

            return PartialView("_tablaCriterioConstructor", constructorCriterio);
        }
        /// <summary>
        /// limpieza del criterio
        /// </summary>
        /// <param name="criterio"></param>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        public ActionResult _limpiarCriterio(Criterio criterio)
        {

            List<ConstructorCriterio> constructorCriterio = new List<ConstructorCriterio>();
            Session["constructorCriterio"] = constructorCriterio;
            return PartialView("_tablaCriterioConstructor", constructorCriterio);
        }

        /// <summary>
        /// tabla de criterios del constructor
        /// Metodo 
        /// </summary>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        public ActionResult _tablaCriterioConstructor()
        {
            Respuesta<Constructor> nConstructor = new Respuesta<Constructor>();
            List<ConstructorCriterio> criterioConstructor = new List<ConstructorCriterio>();
            List<Criterio> listaCriterios = new List<Criterio>();
            String id = "";

            string url = Request.Url.ToString();
            string[] tokens = url.Split('=');
            if(tokens.Length>1){
                 id = tokens[1];
            }
            else
            {
                return PartialView(criterioConstructor);
            }
           
            nConstructor = refConstructorBL.gObtenerConstructor(id);
            String idIndicador="";
            int direccion = 0;
            try
            {
                idIndicador = (String)nConstructor.objObjeto.IdIndicador;
                direccion=(int) nConstructor.objObjeto.IdDireccion;
                listaCriterios = refCriterioBL.gListarCriteriosPorDireccion(direccion, idIndicador).objObjeto;
                if (nConstructor != null)
                {
                    criterioConstructor = refCriterioBL.gobtenerConstructorCriterioporId(id).objObjeto;
                }
                for (int i = 0; i < criterioConstructor.Count; i++ )
                {
                    for (int j = 0; j < listaCriterios.Count; j++)
                    if (criterioConstructor[i].IdCriterio == listaCriterios[j].IdCriterio)
                    {
                        criterioConstructor[i].Criterio = listaCriterios[j];
                    }
                }


                    return PartialView(criterioConstructor);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView(criterioConstructor);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView(criterioConstructor);
            }
        }

        /// <summary>
        /// <method>newM</method>
        /// tabla de criterios del constructor
        /// Metodo 
        /// </summary>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        public ActionResult _tablaCriterioConstructorEditar()
        {
            Respuesta<Constructor> nConstructor = new Respuesta<Constructor>();
            List<ConstructorCriterio> criterioConstructor = new List<ConstructorCriterio>();
            List<Criterio> listaCriterios = new List<Criterio>();
            String id = "";

            string url = Request.Url.ToString();
            string[] tokens = url.Split('=');
            if (tokens.Length > 1)
            {     id = tokens[1]; }
            else
            { return PartialView(criterioConstructor);  }

            nConstructor = refConstructorBL.gObtenerConstructor(id);
            String idIndicador = "";
            int direccion = 0;
            try
            {
                idIndicador = (String)nConstructor.objObjeto.IdIndicador;
                direccion = (int)nConstructor.objObjeto.IdDireccion;
                listaCriterios = refCriterioBL.gListarCriteriosPorDireccion(direccion, idIndicador).objObjeto;
                if (nConstructor != null)
                {
                    criterioConstructor = refCriterioBL.gobtenerConstructorCriterioporId(id).objObjeto;
                }
                for (int i = 0; i < criterioConstructor.Count; i++)
                {
                    for (int j = 0; j < listaCriterios.Count; j++)
                        if (criterioConstructor[i].IdCriterio == listaCriterios[j].IdCriterio)
                        {
                            criterioConstructor[i].Criterio = listaCriterios[j];
                        }
                }


                return PartialView(new Tuple<string, List<ConstructorCriterio>>(id, criterioConstructor));
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView(criterioConstructor);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView(criterioConstructor);
            }
        }
        /// <summary>
        /// Filtro los criterios
        /// </summary>
        /// <param name="pFiltroCriterio"></param>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        [HttpPost]
        public ActionResult _filtrarCriterio(FiltroCriterio pFiltroCriterio)
        {
            Respuesta<List<Criterio>> respuesta = new Respuesta<List<Criterio>>();
            List<ConstructorCriterio> constructorCriterios = new List<ConstructorCriterio>();
            int indiceCriterio = 0;
            try
            {

                if (Session["constructorCriterio"] != null)
                {
                    constructorCriterios = (List<ConstructorCriterio>)Session["constructorCriterio"];
                }
                respuesta = refCriterioBL.gFiltrarCriterioConDireccion((pFiltroCriterio.idDireccion == null ? 0 : pFiltroCriterio.idDireccion)
                    , (pFiltroCriterio.codigoCriterio == null ? "" : pFiltroCriterio.codigoCriterio)
                    , (pFiltroCriterio.nombreCriterio == null ? "" : pFiltroCriterio.nombreCriterio)
                    , (pFiltroCriterio.idIndicador == null ? "" : pFiltroCriterio.idIndicador));

                if (respuesta.objObjeto.Count > 0 && constructorCriterios.Count() > 0) {
                    foreach (ConstructorCriterio item in constructorCriterios)
                    {
                        indiceCriterio = respuesta.objObjeto.FindIndex(y => y.IdCriterio.Equals(item.IdCriterio));
                        if (indiceCriterio >= 0)
                        {
                            respuesta.objObjeto.RemoveAt(indiceCriterio);
                           
                        }
                    }
                }
                return PartialView("_tablaCriterio", respuesta.objObjeto);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView("_tablaCriterio", respuesta.objObjeto);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView("_tablaCriterio", respuesta.objObjeto);
            }
        }
        /// <summary>
        /// <Method>NewM</Method>
        /// </summary>
        /// <param name="pFiltroCriterio"></param>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        [HttpPost]
        public ActionResult _filtrarCriterioEditar(FiltroCriterio pFiltroCriterio)
        {
            Respuesta<List<Criterio>> respuesta = new Respuesta<List<Criterio>>();
            List<ConstructorCriterio> constructorCriterios1 = new List<ConstructorCriterio>();
            List<ConstructorCriterio> constructorCriterios = new List<ConstructorCriterio>();
            Respuesta<Constructor> nConstructor = new Respuesta<Constructor>();
       
            string idConstructor = pFiltroCriterio.idConstructor;
            int indiceCriterio = 0;
            int idDireccion = 0;
            String idIndicador = "";
            nConstructor = refConstructorBL.gObtenerConstructor(idConstructor);
            try
            {
                if (nConstructor != null)
                {
                    idDireccion = (int)nConstructor.objObjeto.IdDireccion;
                }
                if (nConstructor != null)
                {
                    idIndicador = (String)nConstructor.objObjeto.IdIndicador;
                }
                if (nConstructor != null)
                {
                    constructorCriterios1 = refCriterioBL.gobtenerConstructorCriterioporId(idConstructor).objObjeto;
                }

                if (constructorCriterios1 != null && constructorCriterios1.Count() > 0)
                {
                    constructorCriterios = constructorCriterios1;
                }

                respuesta = refCriterioBL.gFiltrarCriterioConDireccion((idDireccion == null ? 0 : idDireccion)
                    , (pFiltroCriterio.codigoCriterio == null ? "" : pFiltroCriterio.codigoCriterio)
                    , (pFiltroCriterio.nombreCriterio == null ? "" : pFiltroCriterio.nombreCriterio)
                    , (idIndicador == null ? "" : idIndicador));

                if (respuesta.objObjeto.Count > 0 && constructorCriterios.Count() > 0)
                {
                    foreach (ConstructorCriterio item in constructorCriterios)
                    {
                        indiceCriterio = respuesta.objObjeto.FindIndex(y => y.IdCriterio.Equals(item.IdCriterio));
                        if (indiceCriterio >= 0)
                        {
                            respuesta.objObjeto.RemoveAt(indiceCriterio);

                        }
                    }
                }
                return PartialView("_tablaCriterioEditar", respuesta.objObjeto);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView("_tablaCriterioEditar", respuesta.objObjeto);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView("_tablaCriterioEditar", respuesta.objObjeto);
            }
        }
        /// <summary>
        /// <method>NewM<method>
        /// filtra los criterios del constructor
        /// </summary>
        /// <param name="psfiltroCriterio"></param>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        public ActionResult _filtrarCriterioConstructorEditar(FiltroCriterio psfiltroCriterio)
        {
            List<ConstructorCriterio> criterioConstructor = new List<ConstructorCriterio>();
            string psCodigoCriterio = (psfiltroCriterio.codigoCriterio == null ? "" : psfiltroCriterio.codigoCriterio);
            string psNombreCriterio = (psfiltroCriterio.nombreCriterio == null ? "" : psfiltroCriterio.nombreCriterio);
            string psAyuda = (psfiltroCriterio.ayuda == null ? "" : psfiltroCriterio.ayuda);
            string idConstructor = psfiltroCriterio.idConstructor;
            Respuesta<Constructor> nConstructor = new Respuesta<Constructor>();          
            List<Criterio> listaCriterios = new List<Criterio>();

            nConstructor = refConstructorBL.gObtenerConstructor(idConstructor);
            String idIndicador = "";
            int direccion = 0;
            try
            {
                idIndicador = (String)nConstructor.objObjeto.IdIndicador;
                direccion = (int)nConstructor.objObjeto.IdDireccion;
                listaCriterios = refCriterioBL.gListarCriteriosPorDireccion(direccion, idIndicador).objObjeto;
                if (nConstructor != null)
                {
                    criterioConstructor = refCriterioBL.gobtenerConstructorCriterioporId(idConstructor).objObjeto;
                }
                for (int i = 0; i < criterioConstructor.Count; i++)
                {
                    for (int j = 0; j < listaCriterios.Count; j++)
                        if (criterioConstructor[i].IdCriterio == listaCriterios[j].IdCriterio)
                        {
                            criterioConstructor[i].Criterio = listaCriterios[j];
                        }
                }//termina 

                if (criterioConstructor.Count > 0 && criterioConstructor != null)
                {
                    criterioConstructor = criterioConstructor.Where(x => (psCodigoCriterio.Equals("") || x.Criterio.IdCriterio.ToUpper().Contains(psCodigoCriterio.ToUpper()))
                                                                      && (psNombreCriterio.Equals("") || x.Criterio.DescCriterio.ToUpper().Contains(psNombreCriterio.ToUpper()))
                                                                      && (psAyuda.Equals("") || x.Ayuda.ToUpper().Contains(psAyuda.ToUpper()))).ToList();
                }

                return PartialView("_tablaCriterioConstructorEditar", new Tuple<string, List<ConstructorCriterio>>(idConstructor, criterioConstructor));
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView("_tablaCriterioConstructorEditar", criterioConstructor);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView("_tablaCriterioConstructorEditar", criterioConstructor);
            }
        }

        /// <summary>
        /// 
        /// filtra los criterios del constructor
        /// </summary>
        /// <param name="psfiltroCriterio"></param>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        public ActionResult _filtrarCriterioConstructor(FiltroCriterio psfiltroCriterio)
        {
            List<ConstructorCriterio> criterioConstructor = new List<ConstructorCriterio>();
            string psCodigoCriterio = (psfiltroCriterio.codigoCriterio == null ? "" : psfiltroCriterio.codigoCriterio);
            string psNombreCriterio = (psfiltroCriterio.nombreCriterio == null ? "" : psfiltroCriterio.nombreCriterio);
            string psAyuda = (psfiltroCriterio.ayuda == null ? "" : psfiltroCriterio.ayuda);

            try
            {

                //obtengo la lista
                if (Session["constructorCriterio"] != null)// pregunto si esta llena
                {
                    criterioConstructor = (List<ConstructorCriterio>)Session["constructorCriterio"];// la igualo 
                }

                if (criterioConstructor.Count > 0)
                {
                    criterioConstructor = criterioConstructor.Where(x => (psCodigoCriterio.Equals("") || x.Criterio.IdCriterio.ToUpper().Contains(psCodigoCriterio.ToUpper()))
                                                                      && (psNombreCriterio.Equals("") || x.Criterio.DescCriterio.ToUpper().Contains(psNombreCriterio.ToUpper()))
                                                                      && (psAyuda.Equals("") || x.Ayuda.ToUpper().Contains(psAyuda.ToUpper()))).ToList();
                }// realizo el filtro

                return PartialView("_tablaCriterioConstructor", criterioConstructor);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView("_tablaCriterioConstructor", criterioConstructor);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView("_tablaCriterioConstructor", criterioConstructor);
            }
        }
        #endregion

        #region detalleAgrupacion

        /// <summary>
        /// Crear la vista parcial de detalles agrupación por operador
        /// </summary>
        /// <returns></returns>
         [AuthorizeUserAttribute]
        public ActionResult _agregarDetalleAgrupacion(List<ConstructorDetalleAgrupacionViewModels> jsTreeDetalleAgrupacion)
        {

            List<ConstructorCriterioDetalleAgrupacion> detallesAgrupacionPorOperador = new List<ConstructorCriterioDetalleAgrupacion>();
            ConstructorCriterioDetalleAgrupacion nodoPadre = new ConstructorCriterioDetalleAgrupacion();
            bool agregado = false;
            try
            {
                if (Session["detallesAgrupacionPorOperador"] != null)
                {
                    detallesAgrupacionPorOperador = (List<ConstructorCriterioDetalleAgrupacion>)Session["detallesAgrupacionPorOperador"];
                }
                nodoPadre = lTrasformacionVMObjetoDetalle(jsTreeDetalleAgrupacion);
                foreach (ConstructorCriterioDetalleAgrupacion item in detallesAgrupacionPorOperador)
                {
                    if (item.IdOperador == nodoPadre.IdOperador) {
                        agregado = true;
                        break;
                    }
                }
                if (agregado == false) { 
                    detallesAgrupacionPorOperador.Add(nodoPadre);
                }
                Session["detallesAgrupacionPorOperador"] = detallesAgrupacionPorOperador;
                return PartialView("_tablaDetalleAgrupacionOperador", detallesAgrupacionPorOperador);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView("_tablaDetalleAgrupacionOperador", detallesAgrupacionPorOperador);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);


              
                return PartialView("_tablaDetalleAgrupacionOperador", detallesAgrupacionPorOperador);
            }
        }
        /// <summary>
        /// <method>NewM</method>
        /// </summary>
        /// <param name="jsTreeDetalleAgrupacion"></param>
        /// <returns></returns>
         public ActionResult _agregarDetalleAgrupacionEditar(DetalleAgrupacionyurl pObject)
         {

             string url = pObject.url;
             string[] tokens = url.Split('=');
             string[] variable = tokens[1].Split('&');
             string Idconstructor = variable[0];
             string idCriterio = tokens[2];           
           
             List<ConstructorDetalleAgrupacionViewModels> jsTreeDetalleAgrupacion = pObject.Listadetalles;

             string[] ids = jsTreeDetalleAgrupacion[0].id.Split('|');
             List<ConstructorCriterioDetalleAgrupacion> detallesAgrupacionPorOperador = new List<ConstructorCriterioDetalleAgrupacion>();
             ConstructorCriterioDetalleAgrupacion nodoPadre = new ConstructorCriterioDetalleAgrupacion();

             Guid idConstructor = new Guid(Idconstructor);
             Respuesta<bool> respuesta = new Respuesta<bool>();
             Respuesta<bool> respuestaCrearArbol = new Respuesta<bool>();

             try
             {
                     nodoPadre = lTrasformacionVMObjetoDetalle(jsTreeDetalleAgrupacion);
                     respuestaCrearArbol = refConstructorBL.gCrearNuevoDetalleAgrupacionporOperador(nodoPadre, idConstructor, idCriterio);
                //Bitacora
                string user;
                user = User.Identity.GetUserId();
                try
                {
                    func.constructor(user,"Proceso de creacion, Arbol", 2,"Registro de arbol con: Id Constructor  "+idConstructor.ToString()+", Id Criterio: "+idCriterio.ToString()," ");
                }
                catch
                {

                }
                return PartialView("_tablaDetalleAgrupacionOperador", detallesAgrupacionPorOperador);
             }
             catch (CustomException cEx)
             {
                 ViewBag.Error = cEx;
                 return PartialView("_tablaDetalleAgrupacionOperador", detallesAgrupacionPorOperador);
             }
             catch (Exception ex)
             {
                 string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                 var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                 ViewBag.Error = ((CustomException)newEx);
 

                return PartialView("_tablaDetalleAgrupacionOperador", detallesAgrupacionPorOperador);
             }
         }

         /// <summary>
         /// Crear la vista parcial de detalles agrupación por operador
         /// </summary>
         /// <returns></returns>
         [AuthorizeUserAttribute]
         public String _esPosibleAplicarRegla(int idTipoValor, string idOperador, int idDescDetalleAgrupacion, int idNivelDetalle)
         {

             JSONResult<List<NivelDetalleReglaEstadistica>> jsonRespuesta = new JSONResult<List<NivelDetalleReglaEstadistica>>();

             jsonRespuesta.data = new List<NivelDetalleReglaEstadistica>();

             Respuesta<List<NivelDetalleReglaEstadistica>> respuesta = new Respuesta<List<NivelDetalleReglaEstadistica>>();

             try
             {
                
                 if (idNivelDetalle == -1) // No se seleccionó ningún detalle de último nivel
                     respuesta = refConstructorBL.ObtenerReglaSinDetalleNivel(idOperador, idTipoValor, idDescDetalleAgrupacion);

                 if (idNivelDetalle == 1) // Se seleccionó el detalle de último nivel Provincia 
                     respuesta = refConstructorBL.ObtenerListaValoresRegla_Provincia(idOperador, idTipoValor, idDescDetalleAgrupacion);
             

                 if (idNivelDetalle == 2) // Se seleccionó el detalle de último nivel Canton
                     respuesta = refConstructorBL.ObtenerListaValoresRegla_Canton(idOperador, idTipoValor, idDescDetalleAgrupacion);

                 if (idNivelDetalle == 3) // Se seleccionó el detalle de último nivel Género
                     respuesta = refConstructorBL.ObtenerListaValoresRegla_Genero(idOperador, idTipoValor, idDescDetalleAgrupacion);

             }

            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;

                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = cEx.Message;
                jsonRespuesta.data = null;
                return jsonRespuesta.toJSON();
            }
            catch (Exception ex)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = ex.Message;
                jsonRespuesta.data = null;
                return jsonRespuesta.toJSON();
            }

             if (respuesta.objObjeto.Count == 0)
                 jsonRespuesta.data = null;
             else
             jsonRespuesta.data = respuesta.objObjeto;

             jsonRespuesta.strMensaje = respuesta.strMensaje;
             jsonRespuesta.ok = true;
             return jsonRespuesta.toJSON();
            
         }         


        /// <summary>
        /// Edita la vista parcial de detalles agrupación
        /// </summary>
        /// <returns></returns>
         [AuthorizeUserAttribute]
        public ActionResult _guardarEdicionDetalleAgrupacion(List<ConstructorDetalleAgrupacionViewModels> jsTreeDetalleAgrupacion)
        {

            string[] ids = jsTreeDetalleAgrupacion[0].id.Split('|');
            List<ConstructorCriterioDetalleAgrupacion> detallesAgrupacionPorOperador = new List<ConstructorCriterioDetalleAgrupacion>();
            ConstructorCriterioDetalleAgrupacion nodoPadre = new ConstructorCriterioDetalleAgrupacion();

            try
            {
                if (Session["detallesAgrupacionPorOperador"] != null)
                {
                    detallesAgrupacionPorOperador = (List<ConstructorCriterioDetalleAgrupacion>)Session["detallesAgrupacionPorOperador"];
                }
                foreach (ConstructorCriterioDetalleAgrupacion item in detallesAgrupacionPorOperador)
                {
                    if (item.IdOperador.Equals(ids[2]))
                    {
                        detallesAgrupacionPorOperador.Remove(item);
                        break;
                    }
                }

                nodoPadre = lTrasformacionVMObjetoDetalle(jsTreeDetalleAgrupacion);


                detallesAgrupacionPorOperador.Add(nodoPadre);
                Session["detallesAgrupacionPorOperador"] = detallesAgrupacionPorOperador;
                return PartialView("_tablaDetalleAgrupacionOperador", detallesAgrupacionPorOperador);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView("_tablaDetalleAgrupacionOperador", detallesAgrupacionPorOperador);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
               

                return PartialView("_tablaDetalleAgrupacionOperador", detallesAgrupacionPorOperador);
            }
        }

         /// <summary>
         /// <method>NewM</method>
         /// Edita la vista parcial de detalles agrupación
         /// </summary>
         /// <returns></returns>
         [AuthorizeUserAttribute]
         public ActionResult _guardarEditarDetalleAgrupacion(DetalleAgrupacionyurl pObject)
         {
             string url = pObject.url;
             string[] tokens = url.Split('=');
             string[] variable = tokens[1].Split('&');
             string Idconstructor = variable[0];
             string[] variable2 = tokens[2].Split('&');
             string idCriterio = variable2[0];
             string idOperador = tokens[3];
             List<ConstructorDetalleAgrupacionViewModels> jsTreeDetalleAgrupacion = pObject.Listadetalles;

             string[] ids = jsTreeDetalleAgrupacion[0].id.Split('|');
             List<ConstructorCriterioDetalleAgrupacion> detallesAgrupacionPorOperador = new List<ConstructorCriterioDetalleAgrupacion>();
             ConstructorCriterioDetalleAgrupacion nodoPadre = new ConstructorCriterioDetalleAgrupacion();

             Guid idConstructor = new Guid(Idconstructor);
             Respuesta<bool> respuesta = new Respuesta<bool>();
             Respuesta<bool> respuestaCrearArbol = new Respuesta<bool>();
 
             try
             {

                 if (idOperador.Equals(ids[2]))
                 {
                 respuesta = refConstructorBL.gEliminarDetalledeAgrupacionporOperador(idConstructor,idCriterio, idOperador);
                 }

                 if (respuesta.objObjeto==true)
                 {
                     nodoPadre = lTrasformacionVMObjetoDetalle(jsTreeDetalleAgrupacion);
                     respuestaCrearArbol = refConstructorBL.gCrearNuevoDetalleAgrupacionporOperador(nodoPadre, idConstructor, idCriterio);

                 }// llamar al metodo que inserta
                 return PartialView("_tablaDetalleAgrupacionOperador", detallesAgrupacionPorOperador);
             }
             catch (CustomException cEx)
             {
                 ViewBag.Error = cEx;
                 return PartialView("_tablaDetalleAgrupacionOperador", detallesAgrupacionPorOperador);
             }
             catch (Exception ex)
             {
                 string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                 var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                 ViewBag.Error = ((CustomException)newEx);
                 return PartialView("_tablaDetalleAgrupacionOperador", detallesAgrupacionPorOperador);
             }
         }

        /// <summary>
        /// Edita los detalles agrupación por operador
        /// </summary>
        /// <param name="operador"></param>
        /// <returns></returns>
         [AuthorizeUserAttribute]
        public String _detalleAgrupacionEditar(Operador operador)
        {
            List<NivelDetalleReglaEstadistica> nivelDetalleReglaEstadistica = new List<NivelDetalleReglaEstadistica>();
            List<ConstructorCriterioDetalleAgrupacion> detallesAgrupacionPorOperador = new List<ConstructorCriterioDetalleAgrupacion>();
            List<ConstructorCriterioDetalleAgrupacion> detalles = new List<ConstructorCriterioDetalleAgrupacion>();
            List<ConstructorDetalleAgrupacionViewModels> jsTreeDetalleAgrupacion = new List<ConstructorDetalleAgrupacionViewModels>();//contiene el Arbol
            ConstructorDetalleAgrupacionViewModels detalle = new ConstructorDetalleAgrupacionViewModels();
            ConstructorCriterioDetalleAgrupacion raiz = new ConstructorCriterioDetalleAgrupacion();
            JSONResult<List<ConstructorDetalleAgrupacionViewModels>> jsonRespuesta = new JSONResult<List<ConstructorDetalleAgrupacionViewModels>>();
            try
            {

                if (Session["detallesAgrupacionPorOperador"] != null)
                {
                    detallesAgrupacionPorOperador = (List<ConstructorCriterioDetalleAgrupacion>)Session["detallesAgrupacionPorOperador"];
                }

                foreach (ConstructorCriterioDetalleAgrupacion item in detallesAgrupacionPorOperador)
                {
                    if (item.IdOperador.Equals(operador.IdOperador))
                    {
                        raiz = item;
                        detalles = item.ConstructorCriterioDetalleAgrupacion1.ToList();
                        break;
                    }
                }



                    jsTreeDetalleAgrupacion.Add(lRetornarNodo(raiz));
                for (int i = 0; i < detalles.Count; i++)
                {
                    jsTreeDetalleAgrupacion.Add(lRetornarNodo(detalles[i]));
                }

                jsonRespuesta.data = jsTreeDetalleAgrupacion;
                jsonRespuesta.ok = true;
                return jsonRespuesta.toJSON();
            }
            catch (CustomException cEx)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = cEx.Message;
                jsonRespuesta.data = new List<ConstructorDetalleAgrupacionViewModels>();
                return jsonRespuesta.toJSON();
            }
            catch (Exception ex)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = ex.Message;
                jsonRespuesta.data = new List<ConstructorDetalleAgrupacionViewModels>();

                return jsonRespuesta.toJSON();
            }
        }

         /// <summary>
         /// <Method>newM</Method>
         /// Edita los detalles agrupación por operador
         /// </summary>
         /// <param name="operador"></param>
         /// <returns></returns>
         [AuthorizeUserAttribute]
         public String _newdetalleAgrupacionEditar(ConstructorOperadorClonarViewModel poOperadorClonar)
         {
             String idOperador = poOperadorClonar.idOperadorSeleccionado;
             String idClonar = poOperadorClonar.idOperadoreClonar;
             String idConstructor = poOperadorClonar.idConstructor;
             String idCriterio = poOperadorClonar.idCriterio;

             Respuesta<List<ConstructorCriterioDetalleAgrupacion>> DetalleAgrupacion = new Respuesta<List<ConstructorCriterioDetalleAgrupacion>>();//obtengo los detalles de agrupacion 
             List<ConstructorCriterioDetalleAgrupacion> detalles = new List<ConstructorCriterioDetalleAgrupacion>();// para asignar los Detalles de agrupacion 
             ConstructorCriterioDetalleAgrupacion detalleListo = new ConstructorCriterioDetalleAgrupacion();// se transforma el detalle  

             List<NivelDetalleReglaEstadistica> nivelDetalleReglaEstadistica = new List<NivelDetalleReglaEstadistica>();// se ocupa para las reglas
             List<ConstructorCriterioDetalleAgrupacion> detallesAgrupacionPorOperador = new List<ConstructorCriterioDetalleAgrupacion>();
             List<ConstructorDetalleAgrupacionViewModels> jsTreeDetalleAgrupacion = new List<ConstructorDetalleAgrupacionViewModels>();//contiene el Arbol
             ConstructorDetalleAgrupacionViewModels detalle = new ConstructorDetalleAgrupacionViewModels();
             ConstructorCriterioDetalleAgrupacion raiz = new ConstructorCriterioDetalleAgrupacion();
             JSONResult<List<ConstructorDetalleAgrupacionViewModels>> jsonRespuesta = new JSONResult<List<ConstructorDetalleAgrupacionViewModels>>();
             try
             {
                 DetalleAgrupacion = refConstructorBL.gObtenerDetallesAgrupacionporOperador(idConstructor, idCriterio, idOperador);// obtengo los detalles de Agrupacion  

                 if (DetalleAgrupacion.objObjeto != null && DetalleAgrupacion.objObjeto.Count != 0)
                 {
                     detallesAgrupacionPorOperador = DetalleAgrupacion.objObjeto;
                 }

                 detalleListo = lTransformarConstructorDetalleAgrupacion(detallesAgrupacionPorOperador, idOperador);// transformo el detalle de Agrupacion para ser leido en el javascript


                 if (detalleListo.IdOperador.Equals(idOperador))
                 {
                     raiz = detalleListo;
                     detalles = detalleListo.ConstructorCriterioDetalleAgrupacion1.ToList();                 
                 }

                 jsTreeDetalleAgrupacion.Add(lRetornarNodo(raiz));
                 for (int i = 0; i < detalles.Count; i++)
                 {
                     jsTreeDetalleAgrupacion.Add(lRetornarNodo(detalles[i]));
                 }

                 jsonRespuesta.data = jsTreeDetalleAgrupacion;
                 jsonRespuesta.ok = true;
                 return jsonRespuesta.toJSON();
             }
             catch (CustomException cEx)
             {
                 jsonRespuesta.ok = false;
                 jsonRespuesta.strMensaje = cEx.Message;
                 jsonRespuesta.data = new List<ConstructorDetalleAgrupacionViewModels>();
                 return jsonRespuesta.toJSON();
             }
             catch (Exception ex)
             {
                 jsonRespuesta.ok = false;
                 jsonRespuesta.strMensaje = ex.Message;
                 jsonRespuesta.data = new List<ConstructorDetalleAgrupacionViewModels>();
                

                return jsonRespuesta.toJSON();
             }
         }

        /// <summary>
        /// Elimina los detalles agrupación por operador
        /// </summary>
        /// <param name="operador"></param>
        /// <returns></returns>
         [AuthorizeUserAttribute]
        public ActionResult _detalleAgrupacionEliminar(Operador operador)
        {
            List<ConstructorCriterioDetalleAgrupacion> detallesAgrupacionPorOperador = new List<ConstructorCriterioDetalleAgrupacion>();
            try
            {

                if (Session["detallesAgrupacionPorOperador"] != null)
                {
                    detallesAgrupacionPorOperador = (List<ConstructorCriterioDetalleAgrupacion>)Session["detallesAgrupacionPorOperador"];
                }
                foreach (ConstructorCriterioDetalleAgrupacion item in detallesAgrupacionPorOperador)
                {
                    if (item.IdOperador.Equals(operador.IdOperador))
                    {
                        detallesAgrupacionPorOperador.Remove(item);
                        break;
                    }
                }
                Session["detallesAgrupacionPorOperador"] = detallesAgrupacionPorOperador;
                return PartialView("_tablaDetalleAgrupacionOperador", detallesAgrupacionPorOperador);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView("_tablaDetalleAgrupacionOperador", detallesAgrupacionPorOperador);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
          

                return PartialView("_tablaDetalleAgrupacionOperador", detallesAgrupacionPorOperador);
            }
        }
        /// <summary>
        /// <Method>newM</Method>
        /// Este metedo eliminara por medio de 
        /// de SP eliminara un Arbol (Detalle de Agrupacion)
        /// </summary>
        /// <param name="operador"></param>
        /// <returns></returns>
         public ActionResult _detalleAgrupacionEliminarEditar(ConstructorCriterioDetalleAgrupacion arbol)
         {
             List<ConstructorCriterioDetalleAgrupacion> detallesAgrupacionPorOperador = new List<ConstructorCriterioDetalleAgrupacion>();
             string idOperador = arbol.IdOperador;
             string idCriterio = arbol.IdCriterio;
             Guid idConstructor = arbol.IdConstructor;
             Respuesta<bool> respuesta = new Respuesta<bool>();
             try
             {
                 respuesta = refConstructorBL.gEliminarDetalledeAgrupacionporOperador(idConstructor,idCriterio, idOperador);
                 if (respuesta.objObjeto == true)
                 {
                     TempData["Error"] = "La información se ha eliminado con éxito.";
                     return PartialView("_tablaDetalleAgrupacionOperador", detallesAgrupacionPorOperador);
                 }
                 else {
                     TempData["Fail"] = respuesta.strMensaje;
                     return PartialView("_tablaDetalleAgrupacionOperador", detallesAgrupacionPorOperador);
                 }
 
                
             }
             catch (CustomException cEx)
             {
                 ViewBag.Error = cEx;
                 return PartialView("_tablaDetalleAgrupacionOperador", detallesAgrupacionPorOperador);
             }
             catch (Exception ex)
             {
                 string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                 var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                 ViewBag.Error = ((CustomException)newEx);
               


                return PartialView("_tablaDetalleAgrupacionOperador", detallesAgrupacionPorOperador);
             }
         }

        /// <summary>
        /// limpia la tabla de detalles de agrupación por operador
        /// </summary>
        /// <returns></returns>
         [AuthorizeUserAttribute]
        public ActionResult _limpiarDetalleAgrupacionConstructor()
        {

            List<ConstructorCriterioDetalleAgrupacion> detallesAgrupacionPorOperador = new List<ConstructorCriterioDetalleAgrupacion>();


            try
            {
                Session["detallesAgrupacionPorOperador"] = detallesAgrupacionPorOperador;
                return PartialView("_tablaDetalleAgrupacionOperador", detallesAgrupacionPorOperador);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView("_tablaDetalleAgrupacionOperador", detallesAgrupacionPorOperador);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView("_tablaDetalleAgrupacionOperador", detallesAgrupacionPorOperador);
            }
        }


        /// <summary>
        /// Crear la vista parcial de detalles agrupación
        /// </summary>
        /// <returns></returns>
         [AuthorizeUserAttribute]
        public ActionResult _detalleAgrupacionCrear()
        {
            ConstructorViewModel mvnuevoContructor = new ConstructorViewModel();
            Respuesta<List<TipoValor>> tipoValor = new Respuesta<List<TipoValor>>();
            Respuesta<List<TipoNivelDetalle>> tipoNivelDetalle = new Respuesta<List<TipoNivelDetalle>>();
            Regla nRegla = new Regla();
            DetalleAgrupacion nDetalleAgrupacion = new DetalleAgrupacion();
            try
            {
                tipoValor = refTipoValorBL.gObtenerTiposValor();
                tipoNivelDetalle = refTipoNivelDetalleBL.gObtenerTiposNivelDetalle();

                nRegla.ValorLimiteInferior = "";
                nRegla.ValorLimiteSuperior = "";
                nDetalleAgrupacion.Agrupacion = new Agrupacion();
                mvnuevoContructor.detalleAgrupacion = nDetalleAgrupacion;
                mvnuevoContructor.regla = nRegla;
                mvnuevoContructor.listaTipoValor = new List<TipoValor>(tipoValor.objObjeto.OrderBy(x => x.Descripcion));
                mvnuevoContructor.listaTipoNivelDetalle = new List<TipoNivelDetalle>(tipoNivelDetalle.objObjeto.OrderBy(x => x.IdNivelDetalle));
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView(mvnuevoContructor);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView(mvnuevoContructor);
            }
           
            return PartialView(mvnuevoContructor);
        }

        /// <summary>
        /// lista los detalles de agrupación deacuerdo al operador
        /// </summary>
        /// <param name="operador"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeUserAttribute]
        public ActionResult _tablaDetalleAgrupacion()
        {
            Respuesta<List<DetalleAgrupacion>> respuesta = new Respuesta<List<DetalleAgrupacion>>();
            Operador operador = new Operador();
            try
            {


                respuesta = refDetalleAgrupacionBL.gObtenerDetallesAgrupacionesPorOperador(operador == null && operador.IdOperador == null ? "" : operador.IdOperador);
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
        /// lista los detalles de agrupación deacuerdo al operador
        /// </summary>
        /// <param name="operador"></param>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        [HttpPost]
        
        public ActionResult _tablaDetalleAgrupacion(Operador operador)
        {
            Respuesta<List<DetalleAgrupacion>> respuesta = new Respuesta<List<DetalleAgrupacion>>();

            try
            {


                respuesta = refDetalleAgrupacionBL.gObtenerDetallesAgrupacionesPorOperador(operador == null && operador.IdOperador == null ? "" : operador.IdOperador);
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
        /// tabla con los detalles agrupación del operador
        /// Carga los detalles agrupacion solamente id de operador
        /// y el nombre 
        /// </summary>
        /// Diego Navarrete 
        /// 21/07/2016
        /// <Method>NewM</Method>
        /// <param name="id"></param>
        /// <param name="idcriterio"></param>
        /// <returns></returns>
         [AuthorizeUserAttribute]
        public ActionResult _tablaDetalleAgrupacionOperadorEditar(String id, String idcriterio)
        {
            Respuesta<List<Operador>> detalles = new Respuesta<List<Operador>>();
            List<Operador> detallesAgrupacionPorOperador = new List<Operador>();
            ConstructorCriterioDetalleAgrupacion ccdetallea = new ConstructorCriterioDetalleAgrupacion();
            Guid IdConstructor;
            IdConstructor = new Guid(id);
                    
            try
            {
                ccdetallea.IdConstructor = IdConstructor;
                ccdetallea.IdCriterio = idcriterio;
                detalles = refConstructorBL.gObtenerOperadoresDetalleAgrupacion(IdConstructor, idcriterio);
                if (detalles.objObjeto != null && detalles.objObjeto.Count != 0)
                {
                    detallesAgrupacionPorOperador = detalles.objObjeto;
                }
                //el error mas probable
                return PartialView("_tablaDetalleAgrupacionOperadorEditar", new Tuple<ConstructorCriterioDetalleAgrupacion, List<Operador>>(ccdetallea, detallesAgrupacionPorOperador));
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView("_tablaDetalleAgrupacionOperadorEditar", new Tuple<ConstructorCriterioDetalleAgrupacion, List<Operador>>(ccdetallea, detallesAgrupacionPorOperador));
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView(detallesAgrupacionPorOperador);
            }
        }
         /// <summary>
         /// Metodo Original
         /// tabla con los detalles agrupación del operador
         /// </summary>
         /// <param name="operador"></param>
         /// <returns></returns>
         [AuthorizeUserAttribute]
         public ActionResult _tablaDetalleAgrupacionOperador(Operador operador)
         {
             List<ConstructorCriterioDetalleAgrupacion> detallesAgrupacionPorOperador = new List<ConstructorCriterioDetalleAgrupacion>();

             try
             {
                 if (Session["detallesAgrupacionPorOperador"] != null)
                 {
                     detallesAgrupacionPorOperador = (List<ConstructorCriterioDetalleAgrupacion>)Session["detallesAgrupacionPorOperador"];
                 }



                 return PartialView("_tablaDetalleAgrupacionOperador", detallesAgrupacionPorOperador);
             }
             catch (CustomException cEx)
             {
                 ViewBag.Error = cEx;
                 return PartialView("_tablaDetalleAgrupacionOperador", detallesAgrupacionPorOperador);
             }
             catch (Exception ex)
             {
                 string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                 var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                 ViewBag.Error = ((CustomException)newEx);
                 return PartialView(detallesAgrupacionPorOperador);
             }
         }
        /// <summary>
        /// filtra los detalles de un operador en especifico
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        [HttpPost]
        public ActionResult _filtrarDetalleAgrupacion(FiltroDetalleAgrupacion filtro)
        {
            Respuesta<List<DetalleAgrupacion>> respuesta = new Respuesta<List<DetalleAgrupacion>>();

            try
            {


                respuesta = refDetalleAgrupacionBL.gFiltrarDetalleAgrupacion(filtro.idOperador, (filtro.nombreAgrupacion == null ? "" : filtro.nombreAgrupacion), (filtro.nombreDetalleAgrupacion == null ? "" : filtro.nombreDetalleAgrupacion));
                return PartialView("_tablaDetalleAgrupacion", respuesta.objObjeto);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView("_tablaDetalleAgrupacion", respuesta.objObjeto);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView("_tablaDetalleAgrupacion", respuesta.objObjeto);
            }
        }
     
        /// <summary>
        /// <method>newM</method>
        /// </summary>
        /// <param name="operador"></param>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        [HttpPost]
        public ActionResult _filtroDetalleAgrupacionOperadorEditar(DetalleAgrupacionyurl operador)
        {
            List<Operador> detallesAgrupacionPorOperador = new List<Operador>();
            Respuesta<List<Operador>> detalles = new Respuesta<List<Operador>>();
            ConstructorCriterioDetalleAgrupacion ccdetallea = new ConstructorCriterioDetalleAgrupacion();
            string IDConstructor = operador.idConstructor;
            string NombreOperador = operador.NombreOperador;
            string IDCriterio = operador.idCriterio;

            Guid IdConstructor;
            IdConstructor = new Guid(IDConstructor);

            try
            {
                ccdetallea.IdConstructor = IdConstructor;
                ccdetallea.IdCriterio = IDCriterio;
                detalles = refConstructorBL.gObtenerOperadoresDetalleAgrupacion(IdConstructor, IDCriterio);

                if (detalles.objObjeto != null && detalles.objObjeto.Count != 0)
                {

                    if (operador.NombreOperador != null)
                    {
                        detallesAgrupacionPorOperador = detalles.objObjeto.Where(x => x.NombreOperador.ToUpper().Contains(operador.NombreOperador.ToUpper())).ToList();
                    }
                    else
                    {
                        detallesAgrupacionPorOperador = detalles.objObjeto;
                    }

                }

                return PartialView("_tablaDetalleAgrupacionOperadorEditar", new Tuple<ConstructorCriterioDetalleAgrupacion, List<Operador>>(ccdetallea, detallesAgrupacionPorOperador));
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView("_tablaDetalleAgrupacionOperadorEditar", new Tuple<ConstructorCriterioDetalleAgrupacion, List<Operador>>(ccdetallea, detallesAgrupacionPorOperador));
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView("_tablaDetalleAgrupacionOperadorEditar", new Tuple<ConstructorCriterioDetalleAgrupacion, List<Operador>>(ccdetallea, detallesAgrupacionPorOperador));
            }
        }
        /// <summary>
        /// filtra los detalles operador que estan registrados por operador
        /// </summary>
        /// <param name="operador"></param>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        [HttpPost]
        public ActionResult _filtroDetalleAgrupacionOperador(Operador operador)
        {
            List<ConstructorCriterioDetalleAgrupacion> detallesAgrupacionPorOperador = new List<ConstructorCriterioDetalleAgrupacion>();

            try
            {
                if (Session["detallesAgrupacionPorOperador"] != null)
                {
                    detallesAgrupacionPorOperador = (List<ConstructorCriterioDetalleAgrupacion>)Session["detallesAgrupacionPorOperador"];
                }
                if (operador.NombreOperador != null)
                {
                    detallesAgrupacionPorOperador = detallesAgrupacionPorOperador.Where(x => x.DetalleAgrupacion.Operador.NombreOperador.ToUpper().Contains(operador.NombreOperador.ToUpper())).ToList();
                }

                return PartialView("_tablaDetalleAgrupacionOperador", detallesAgrupacionPorOperador);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView("_tablaDetalleAgrupacionOperador", detallesAgrupacionPorOperador);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView(detallesAgrupacionPorOperador);
            }
        }
        #endregion

        #region clonarDetalleAgrupacion
        /// <summary>
        /// tabla con los operadores disponibles a clonar
        /// </summary>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        public ActionResult _tablaOperadorClonarEditar(String id, String idcriterio)
        {
            Respuesta<List<Operador>> detallesAgrupacionPorOperador = new Respuesta<List<Operador>>();
            List<Operador> detalles = new List<Operador>();
            Guid IdConstructor;
            IdConstructor = new Guid(id);
            
            Respuesta<List<Operador>> respuesta = new Respuesta<List<Operador>>();
            List<Operador> operadores = new List<Operador>();
            Operador operador = new Operador();
            int indiceOperador = 0;
            List<String> idOperadores = new List<String>();
            try
            {
                detallesAgrupacionPorOperador = refConstructorBL.gObtenerOperadoresDetalleAgrupacion(IdConstructor, idcriterio);
                if (detallesAgrupacionPorOperador != null && detallesAgrupacionPorOperador.objObjeto.Count != 0)
                {
                    detalles = detallesAgrupacionPorOperador.objObjeto;
                }
                if (detalles.Count > 0)
                {
                    idOperadores = detalles.Select(x => x.IdOperador).Distinct().ToList();
                }
                respuesta = refOperadorBL.ConsultarTodos();
                foreach (Operador item in respuesta.objObjeto)
                {
                    indiceOperador = idOperadores.FindIndex(y => y.Equals(item.IdOperador));
                    if (indiceOperador < 0)
                    {
                        operador = new Operador();
                        operador.IdOperador = item.IdOperador;
                        operador.NombreOperador = item.NombreOperador;
                        operadores.Add(operador);
                    }
                }

                return PartialView("_tablaOperadorClonarEditar", operadores);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView("_tablaOperadorClonarEditar", respuesta.objObjeto);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);

               
                return PartialView("_tablaOperadorClonarEditar", respuesta.objObjeto);
            }
        }

        /// <summary>
        /// tabla con los operadores disponibles a clonar
        /// </summary>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        public ActionResult _tablaOperadorClonar()
        {
            List<ConstructorCriterioDetalleAgrupacion> detalles = new List<ConstructorCriterioDetalleAgrupacion>();
            Respuesta<List<Operador>> respuesta = new Respuesta<List<Operador>>();
            List<Operador> operadores = new List<Operador>();
            Operador operador = new Operador();
            int indiceOperador = 0;
            List<String> idOperadores = new List<String>();
            try
            {
                if (Session["detallesAgrupacionPorOperador"] != null)
                {
                    detalles = (List<ConstructorCriterioDetalleAgrupacion>)Session["detallesAgrupacionPorOperador"];
                }
                if (detalles.Count > 0)
                {
                    idOperadores = detalles.Select(x => x.IdOperador).Distinct().ToList();
                }
                respuesta = refOperadorBL.ConsultarTodos();
                foreach (Operador item in respuesta.objObjeto)
                {
                    indiceOperador = idOperadores.FindIndex(y => y.Equals(item.IdOperador));
                    if (indiceOperador < 0)
                    {
                        operador = new Operador();
                        operador.IdOperador = item.IdOperador;
                        operador.NombreOperador = item.NombreOperador;
                        operadores.Add(operador);
                    }
                }

                return PartialView("_tablaOperadorClonar", operadores);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView("_tablaOperadorClonar",respuesta.objObjeto);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView("_tablaOperadorClonar",respuesta.objObjeto);
            }
        }


        /// <summary>
        /// Filtra los operadores que se pueden clonar
        /// </summary>
        /// <param name="poOperador"></param>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        public ActionResult _filtarTablaOperadorClonarEditar(DetalleAgrupacionyurl filtroOperador)
        {

            List<Operador> operadores = new List<Operador>();
            List<String> idOperadores = new List<String>();
            int indiceOperador = 0;
            Operador operador = new Operador();
            Respuesta<List<Operador>> respuesta = new Respuesta<List<Operador>>();
            List<Operador> detallesAgrupacionPorOperador = new List<Operador>();
            Respuesta<List<Operador>> detalles = new Respuesta<List<Operador>>();
            string IDConstructor = filtroOperador.idConstructor;
            string IDCriterio = filtroOperador.idCriterio;
            Guid IdConstructor;
            IdConstructor = new Guid(IDConstructor);

            try
            {
                detalles = refConstructorBL.gObtenerOperadoresDetalleAgrupacion(IdConstructor, IDCriterio);
                respuesta = refOperadorBL.ConsultarTodos();
                if (detalles.objObjeto != null && detalles.objObjeto.Count > 0)
                {
                    if (detalles.objObjeto.Count > 0)
                    {
                        idOperadores = detalles.objObjeto.Select(x => x.IdOperador).Distinct().ToList();
                    }


                    foreach (Operador item in respuesta.objObjeto)
                    {
                        indiceOperador = idOperadores.FindIndex(y => y.Equals(item.IdOperador));
                        if (indiceOperador < 0)
                        {
                            operador = new Operador();
                            operador.IdOperador = item.IdOperador;
                            operador.NombreOperador = item.NombreOperador;
                            operadores.Add(operador);
                        }
                    }
                }
                else
                {
                    operadores = respuesta.objObjeto;
                }


                if (operadores.Count > 0)
                {

                    if (filtroOperador.idOperador != null && filtroOperador.NombreOperador != null)
                    {
                        detallesAgrupacionPorOperador = operadores.Where(x => x.IdOperador.Contains(filtroOperador.idOperador)).ToList();
                        detallesAgrupacionPorOperador = detallesAgrupacionPorOperador.Where(x => x.NombreOperador.ToUpper().Contains(filtroOperador.NombreOperador.ToUpper())).ToList();
                    }
                    else
                    {
                        if (filtroOperador.idOperador != null)
                        {
                            detallesAgrupacionPorOperador = operadores.Where(x => x.IdOperador.Contains(filtroOperador.idOperador)).ToList();
                        }

                        if (filtroOperador.NombreOperador != null)
                        {
                            detallesAgrupacionPorOperador = operadores.Where(x => x.NombreOperador.ToUpper().Contains(filtroOperador.NombreOperador.ToUpper())).ToList();
                        }
                    }

                    if (filtroOperador.idOperador == null && filtroOperador.NombreOperador == null)
                    {
                        detallesAgrupacionPorOperador = operadores;
                    }
                }

                return PartialView("_tablaOperadorClonarEditar", detallesAgrupacionPorOperador);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView("_tablaOperadorClonarEditar", detallesAgrupacionPorOperador);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView("_tablaOperadorClonarEditar", detallesAgrupacionPorOperador);
            }
        }

        /// <summary>
        /// Filtra los operadores que se pueden clonar
        /// </summary>
        /// <param name="poOperador"></param>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        public ActionResult _filtarTablaOperadorClonar(Operador poOperador)
        {
            List<ConstructorCriterioDetalleAgrupacion> detalles = new List<ConstructorCriterioDetalleAgrupacion>();
            Respuesta<List<Operador>> respuesta = new Respuesta<List<Operador>>();
            List<Operador> operadores = new List<Operador>();
            Operador operador = new Operador();
            int indiceOperador = 0;
            List<String> idOperadores = new List<String>();
            try
            {
                if (Session["detallesAgrupacionPorOperador"] != null)
                {
                    detalles = (List<ConstructorCriterioDetalleAgrupacion>)Session["detallesAgrupacionPorOperador"];
                }
                if (detalles.Count > 0)
                {
                    idOperadores = detalles.Select(x => x.IdOperador).Distinct().ToList();
                }
                respuesta = refOperadorBL.gFiltrarOperadores(poOperador.IdOperador == null ? "" : poOperador.IdOperador.ToString(), poOperador.NombreOperador == null ? "" : poOperador.NombreOperador);
                foreach (Operador item in respuesta.objObjeto)
                {
                    indiceOperador = idOperadores.FindIndex(y => y.Equals(item.IdOperador));
                    if (indiceOperador < 0)
                    {
                        operador = new Operador();
                        operador.IdOperador = item.IdOperador;
                        operador.NombreOperador = item.NombreOperador;
                        operadores.Add(operador);
                    }
                }
                
                return PartialView("_tablaOperadorClonar", operadores);
            }
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView("_tablaOperadorClonar", respuesta.objObjeto);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
                return PartialView("_tablaOperadorClonar", respuesta.objObjeto);
            }
        }

      
        /// <summary>
        /// Controlador para clonar detalles agrupación
        /// </summary>
        /// <param name="poOperadorClonar"></param>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        public ActionResult _clonarDetalleAgrupacionEditar(ConstructorOperadorClonarViewModel poOperadorClonar)
        {
         
            String idOperador = poOperadorClonar.idOperadorSeleccionado;
            String idClonar = poOperadorClonar.idOperadoreClonar;
            String idConstructor = poOperadorClonar.idConstructor;
            String idCriterio = poOperadorClonar.idCriterio;

            List<String> idOperadoresClonar = new List<string>();
            Respuesta<List<ConstructorCriterioDetalleAgrupacion>> DetalleAgrupacion = new Respuesta<List<ConstructorCriterioDetalleAgrupacion>>();
            List<ConstructorCriterioDetalleAgrupacion> detalles = new List<ConstructorCriterioDetalleAgrupacion>();// lo ocupo 
            ConstructorCriterioDetalleAgrupacion detalleClonar = new ConstructorCriterioDetalleAgrupacion();// Detalle a clonar 
            Respuesta<List<ConstructorCriterioDetalleAgrupacion>> resultado = new Respuesta<List<ConstructorCriterioDetalleAgrupacion>>();// lo ocupo 
         
            try {
                DetalleAgrupacion = refConstructorBL.gObtenerDetallesAgrupacionporOperador(idConstructor, idCriterio, idOperador);// obtengo los detalles de Agrupacion  
                if (DetalleAgrupacion.objObjeto != null && DetalleAgrupacion.objObjeto.Count != 0)
                {
                    detalles = DetalleAgrupacion.objObjeto;                  
                }
                if (idClonar != null)
                {
                    idOperadoresClonar =new List<string>( (Array.ConvertAll(idClonar.Split(','), x => x)));//separo en una lista los idoperadores a Clonar 
                    detalleClonar = lTransformarConstructorDetalleAgrupacion(detalles, idOperador);// transformo el detalle de Agrupacion para ser leido en el javascript
          
                    resultado = refDetalleAgrupacionBL.gClonarDetalleAgrupacionOperadores(detalleClonar.ConstructorCriterioDetalleAgrupacion1.ToList(),idOperadoresClonar,idOperador, idConstructor, idCriterio);//el metodo que clona
                }
                    if (resultado.strMensaje != null && !resultado.strMensaje.Equals(""))
                    {
                        Session["mensajeErrorClonar"] = resultado.strMensaje;
                    }
                detalles = new List<ConstructorCriterioDetalleAgrupacion>();
                return PartialView("_tablaDetalleAgrupacionOperador", detalles);
            }
            
            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView("_tablaDetalleAgrupacionOperador", detalles);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);

              

                return PartialView("_tablaDetalleAgrupacionOperador", detalles);
            }
        
        }

       

        ///<summary>
        ///Clonar detalle agrupacion con sp 
        ///metodo como soloción para el error de guardar 
        ///Constructor. 
        ///</summary>>
        ///
         [AuthorizeUserAttribute]
        public ActionResult _clonarDetalleAgrupacion(ConstructorOperadorClonarViewModel poOperadorClonar)
        {
          
            String idOperador = poOperadorClonar.idOperadorSeleccionado;
            String idClonar = poOperadorClonar.idOperadoreClonar;

            List<String> idOperadoresClonar = new List<string>();
            List<ConstructorCriterioDetalleAgrupacion> detalles = new List<ConstructorCriterioDetalleAgrupacion>();
            ConstructorCriterioDetalleAgrupacion detalleClonar = new ConstructorCriterioDetalleAgrupacion();
            Respuesta<List<ConstructorCriterioDetalleAgrupacion>> resultado = new Respuesta<List<ConstructorCriterioDetalleAgrupacion>>();
            try
            {
                if (Session["detallesAgrupacionPorOperador"] != null)
                {
                    detalles = (List<ConstructorCriterioDetalleAgrupacion>)Session["detallesAgrupacionPorOperador"];
                }
                if (idClonar != null)
                {
                    idOperadoresClonar = new List<string>((Array.ConvertAll(idClonar.Split(','), x => x)));
                    detalleClonar = detalles.Where(x => x.IdOperador.Equals(idOperador)).FirstOrDefault();

                    resultado = refDetalleAgrupacionBL.gClonarDetalleAgrupacion(detalleClonar.ConstructorCriterioDetalleAgrupacion1.ToList(), idOperadoresClonar);
                }
                if (resultado.strMensaje != null && !resultado.strMensaje.Equals(""))
                {
                    Session["mensajeErrorClonar"] = resultado.strMensaje;
                }

                if (resultado.blnIndicadorTransaccion == true)
                {

                    detalles.AddRange(resultado.objObjeto);
                }
                return PartialView("_tablaDetalleAgrupacionOperador", detalles);
            }

            catch (CustomException cEx)
            {
                ViewBag.Error = cEx;
                return PartialView("_tablaDetalleAgrupacionOperador", detalles);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                var newEx = AppContext.ExceptionBuilder.BuildException(msj, ex);
                ViewBag.Error = ((CustomException)newEx);
           

               
                return PartialView("_tablaDetalleAgrupacion", detalles);
            }
        }

        /// <summary>
        /// Obtiene el mensaje del proceso de clonar
        /// </summary>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        public String _clonarMensajeError() {
            JSONResult<String> jsonRespuesta = new JSONResult<String>();
             jsonRespuesta.ok = false;
            try
            {
                if (Session["mensajeErrorClonar"] != null)
                {
                    jsonRespuesta.strMensaje = (String)Session["mensajeErrorClonar"];
                    jsonRespuesta.ok = true;
                    jsonRespuesta.data = "";
                }
                else {
                    jsonRespuesta.strMensaje ="";
                    jsonRespuesta.ok = false;
                    jsonRespuesta.data = "";
                }
                Session["mensajeErrorClonar"] = null;
                return jsonRespuesta.toJSON();
            }
           
            catch (Exception ex)
            {

                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                AppContext.ExceptionBuilder.BuildException(msj, ex);
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = msj;
                jsonRespuesta.data = "";
                return jsonRespuesta.toJSON();

            }
        }


        #endregion

        #region transformacionDatos
        /// <summary>
        /// Obtiene los datos trasformados en el vm
        /// </summary>
        /// <returns></returns>
        [AuthorizeUserAttribute]
        private ConstructorDetalleAgrupacionViewModels lRetornarNodo(ConstructorCriterioDetalleAgrupacion detalleAgrupacion)
        {
            ConstructorDetalleAgrupacionViewModels nDetalle = new ConstructorDetalleAgrupacionViewModels();
            List<NivelDetalleReglaEstadistica> ListNivelDetalleEs = new List<NivelDetalleReglaEstadistica>();
            NivelDetalleReglaEstadistica ObjNivelDetalleEs = new NivelDetalleReglaEstadistica();
            nDetalle.id = detalleAgrupacion.DetalleAgrupacion.IdOperador;
            nDetalle.parent = detalleAgrupacion.ConstructorCriterioDetalleAgrupacion2 == null ? "#" : detalleAgrupacion.ConstructorCriterioDetalleAgrupacion2.DetalleAgrupacion.IdOperador;
            nDetalle.text = detalleAgrupacion.DetalleAgrupacion.Operador != null ? detalleAgrupacion.DetalleAgrupacion.Operador.NombreOperador : detalleAgrupacion.DetalleAgrupacion.DescDetalleAgrupacion;
            nDetalle.ultimoNivel = detalleAgrupacion.UltimoNivel == null ? 0 : (int)detalleAgrupacion.UltimoNivel;

            if (nDetalle.ultimoNivel == 0)
            {
                nDetalle.idTipoValor = 0;
                nDetalle.valorInferior = "";
                nDetalle.valorSuperior = "";
                nDetalle.UsaReglaEstadistica = 0; // editado!!! antes (UsaReglaEstadisticaConNivelDetalle)
                nDetalle.nivelDetalleReglaEstadistica = null;
                nDetalle.idNivel = 0;
            }
            else
            {
                nDetalle.idTipoValor = detalleAgrupacion.IdTipoValor == null ? 0 : (int)detalleAgrupacion.IdTipoValor;
                nDetalle.idTipoNivelDetalle = detalleAgrupacion.IdNivelDetalle == null ? 0 : (int)detalleAgrupacion.IdNivelDetalle;
                nDetalle.idNivel = detalleAgrupacion.IdNivelDetalle == null ? 0 : (int)detalleAgrupacion.IdNivelDetalle;
                nDetalle.idTipoNivelDetalleGenero = detalleAgrupacion.IdNivelDetalleGenero == null ? 0 : (int)detalleAgrupacion.IdNivelDetalleGenero;
                if (detalleAgrupacion.Regla != null)
                {
                    nDetalle.valorInferior = detalleAgrupacion.Regla.ValorLimiteInferior;
                    nDetalle.valorSuperior = detalleAgrupacion.Regla.ValorLimiteSuperior;
                }
                else
                {
                    nDetalle.valorInferior = "";
                    nDetalle.valorSuperior = "";

                }

                if (detalleAgrupacion.UsaReglaEstadistica == 1)
                {
                    nDetalle.UsaReglaEstadistica = (int)detalleAgrupacion.UsaReglaEstadistica;  // editado kev (UsaReglaEstadisticaConNivelDetalle)


                    if (detalleAgrupacion.NivelDetalleReglaEstadistica != null && detalleAgrupacion.NivelDetalleReglaEstadistica.Count > 0)
                    {

                        nDetalle.nivelDetalleReglaEstadistica = detalleAgrupacion.NivelDetalleReglaEstadistica.ToList();
                    
                        for (int i = 0; i < nDetalle.nivelDetalleReglaEstadistica.Count; i++)
                    {
                        ObjNivelDetalleEs = new NivelDetalleReglaEstadistica();
                        ObjNivelDetalleEs.Borrado = nDetalle.nivelDetalleReglaEstadistica[i].Borrado;
                        ObjNivelDetalleEs.IdConstructorCriterioDetalleAgrupacion = nDetalle.nivelDetalleReglaEstadistica[i].IdConstructorCriterioDetalleAgrupacion;
                        ObjNivelDetalleEs.IdGenerico = nDetalle.nivelDetalleReglaEstadistica[i].IdGenerico;
                        ObjNivelDetalleEs.IdNivelDetalle = nDetalle.nivelDetalleReglaEstadistica[i].IdNivelDetalle;
                        ObjNivelDetalleEs.ValorLimiteInferior = nDetalle.nivelDetalleReglaEstadistica[i].ValorLimiteInferior;
                        ObjNivelDetalleEs.ValorLimiteSuperior = nDetalle.nivelDetalleReglaEstadistica[i].ValorLimiteSuperior;
                        ObjNivelDetalleEs.IdNivelDetalleReglaEstadistica = nDetalle.nivelDetalleReglaEstadistica[i].IdNivelDetalleReglaEstadistica;
                        ListNivelDetalleEs.Add(ObjNivelDetalleEs);
                    }
                    nDetalle.nivelDetalleReglaEstadistica = ListNivelDetalleEs; 
                    
                    }
                      
                    else 
                        nDetalle.nivelDetalleReglaEstadistica = null;



                
                }

            }


            return nDetalle;

        }



        /// <summary>
        /// Transforma los vm a objeto
        /// </summary>
        /// <param name="jsTreeDetalleAgrupacion"></param>
        /// <returns></returns>
         [AuthorizeUserAttribute]
        private ConstructorCriterioDetalleAgrupacion lTrasformacionVMObjetoDetalle(List<ConstructorDetalleAgrupacionViewModels> jsTreeDetalleAgrupacion)
        {
            List<ConstructorCriterioDetalleAgrupacion> detallesAgrupacionPorOperador = new List<ConstructorCriterioDetalleAgrupacion>();
            ConstructorCriterioDetalleAgrupacion nodoPadre = new ConstructorCriterioDetalleAgrupacion();
            ConstructorCriterioDetalleAgrupacion nodoHijo = new ConstructorCriterioDetalleAgrupacion();
            string[] ids = jsTreeDetalleAgrupacion[0].id.Split('|');
            int orden = 0;
            try
            {
                if (Session["detallesAgrupacionPorOperador"] != null)
                {
                    detallesAgrupacionPorOperador = (List<ConstructorCriterioDetalleAgrupacion>)Session["detallesAgrupacionPorOperador"];
                }
                nodoPadre.DetalleAgrupacion = new DetalleAgrupacion();
                nodoPadre.DetalleAgrupacion.Operador = new Operador();
                nodoPadre.IdOperador = ids[2];
                nodoPadre.DetalleAgrupacion.IdOperador = jsTreeDetalleAgrupacion[0].id;
                nodoPadre.DetalleAgrupacion.Operador.NombreOperador = jsTreeDetalleAgrupacion[0].text;

                nodoPadre.ConstructorCriterioDetalleAgrupacion1 = new List<ConstructorCriterioDetalleAgrupacion>();

                for (int i = 1; i < jsTreeDetalleAgrupacion.Count(); i++)
                {
                    string[] idsPadre = jsTreeDetalleAgrupacion[i].parent.Split('|');
                    nodoHijo = new ConstructorCriterioDetalleAgrupacion();
                    nodoHijo.DetalleAgrupacion = new DetalleAgrupacion();
                    nodoHijo.ConstructorCriterioDetalleAgrupacion2 = new ConstructorCriterioDetalleAgrupacion();
                    nodoHijo.ConstructorCriterioDetalleAgrupacion2.DetalleAgrupacion = new DetalleAgrupacion();
                    //nodoHijo.NivelDetalleReglaEstadistica = new NivelDetalleReglaEstadistica(); //Added by kevin
                    ids = jsTreeDetalleAgrupacion[i].id.Split('|');
                    //nodo padre del hijo
                    nodoHijo.ConstructorCriterioDetalleAgrupacion2.IdDetalleAgrupacion = int.Parse(idsPadre[0]);
                    nodoHijo.ConstructorCriterioDetalleAgrupacion2.IdAgrupacion = int.Parse(idsPadre[1]);
                    nodoHijo.ConstructorCriterioDetalleAgrupacion2.IdOperador = idsPadre[2];
                    nodoHijo.ConstructorCriterioDetalleAgrupacion2.DetalleAgrupacion.IdOperador = jsTreeDetalleAgrupacion[i].parent;
                   
                    nodoHijo.IdDetalleAgrupacion = int.Parse(ids[0]);
                    nodoHijo.IdAgrupacion = int.Parse(ids[1]);
                    nodoHijo.IdOperador = ids[2];
                    nodoHijo.UltimoNivel = (byte)jsTreeDetalleAgrupacion[i].ultimoNivel;
                    nodoHijo.IdNivel =(jsTreeDetalleAgrupacion[i].idNivel==0?ids.Length/3: jsTreeDetalleAgrupacion[i].idNivel);
                    nodoHijo.DetalleAgrupacion.IdOperador = jsTreeDetalleAgrupacion[i].id;
                    nodoHijo.DetalleAgrupacion.DescDetalleAgrupacion = jsTreeDetalleAgrupacion[i].text;
                    nodoHijo.Orden = orden;
                    orden++;
                    if (jsTreeDetalleAgrupacion[i].ultimoNivel.Equals(1))
                    {
                        nodoHijo.Regla = new Regla();

                        nodoHijo.Regla.FechaCreacionRegla = DateTime.Now;

                        nodoHijo.NivelDetalleReglaEstadistica = new List<NivelDetalleReglaEstadistica>();

                        //Inicio de bloque de código agregado por implementar regla estadística                       

                            if (jsTreeDetalleAgrupacion[i].idTipoNivelDetalle != 0 && jsTreeDetalleAgrupacion[i].nivelDetalleReglaEstadistica.Count > 0) // Entonces seleccionaron algún NivelDetalle
                        {

                                  nodoHijo.NivelDetalleReglaEstadistica = jsTreeDetalleAgrupacion[i].nivelDetalleReglaEstadistica;
                                  nodoHijo.UsaReglaEstadisticaConNivelDetalle = 1; 
                                  nodoHijo.UsaReglaEstadistica = 1;
                                  nodoHijo.Regla.ValorLimiteInferior = string.Empty;
                                  nodoHijo.Regla.ValorLimiteSuperior = string.Empty;
                               
                        }
                        else {
                                //COD ORIGINAL START

                            if (jsTreeDetalleAgrupacion[i].UsaReglaEstadistica == 1)
                                nodoHijo.UsaReglaEstadistica = 1;
                            else
                                nodoHijo.UsaReglaEstadistica = 0;
                            nodoHijo.Regla.ValorLimiteInferior = jsTreeDetalleAgrupacion[i].valorInferior;
                            nodoHijo.Regla.ValorLimiteSuperior = jsTreeDetalleAgrupacion[i].valorSuperior;
                            nodoHijo.UsaReglaEstadisticaConNivelDetalle = 0;
                            nodoHijo.NivelDetalleReglaEstadistica = null;

                            //COD ORIGINAL END                                            
                        
                        }
                            //FIN de bloque de código agregado por implementar regla estadística
                      
                    

                        if (jsTreeDetalleAgrupacion[i].idTipoValor == 0)
                        {
                            nodoHijo.IdTipoValor = null;
                        }
                        else
                        {
                            nodoHijo.IdTipoValor = jsTreeDetalleAgrupacion[i].idTipoValor;
                        }
                           

                        nodoHijo.IdNivelDetalle = jsTreeDetalleAgrupacion[i].idTipoNivelDetalle;
                        nodoHijo.IdNivelDetalleGenero = jsTreeDetalleAgrupacion[i].idTipoNivelDetalleGenero;  
                    }

                    nodoPadre.ConstructorCriterioDetalleAgrupacion1.Add(nodoHijo);
                }

            }
            catch (Exception)
            {

            }
            return nodoPadre;
        }

        /// <summary>
        /// transforma los criterios que vienen de la base de datos
        /// </summary>
        /// <param name="criterios"></param>
        /// <returns></returns>
         [AuthorizeUserAttribute]
        private List<ConstructorCriterio> lTrasformarConstructorCriterio(List<ConstructorCriterio> criterios)
        {
            List<ConstructorCriterio> constructorCriterio = new List<ConstructorCriterio>();
            ConstructorCriterio nuevoConstructorCriterio = new ConstructorCriterio();
            foreach (ConstructorCriterio item in criterios)
            {
                nuevoConstructorCriterio = new ConstructorCriterio();
                nuevoConstructorCriterio.ConstructorCriterioDetalleAgrupacion = new List<ConstructorCriterioDetalleAgrupacion>();
                if (item.ConstructorCriterioDetalleAgrupacion != null && item.ConstructorCriterioDetalleAgrupacion.Count > 0)
                {
                    nuevoConstructorCriterio.ConstructorCriterioDetalleAgrupacion = lTransformarConstructorDetalle(item.ConstructorCriterioDetalleAgrupacion.OrderBy(x=>(x.Orden==null?x.IdNivel:x.Orden)).ToList());
                }
                nuevoConstructorCriterio.Criterio = new Criterio();
                nuevoConstructorCriterio.Criterio.DescCriterio = item.Criterio.DescCriterio;
                nuevoConstructorCriterio.Criterio.IdCriterio = item.IdCriterio;
                nuevoConstructorCriterio.IdCriterio = item.IdCriterio;
                nuevoConstructorCriterio.Ayuda = item.Ayuda;
                constructorCriterio.Add(nuevoConstructorCriterio);
            }
            return constructorCriterio;
        }
        
        /// <summary>
        /// Transforma los detalles agrupación de la forma que se pueda leer 
        /// luego en el arbol de detalles agrupación
        /// SE TRANSFORMAN TODOS LOS ARBOLES DE TALLES DE AGRUPACION 
        /// UNO POR UNO PARA LUEGO SER CARGADOS 
        /// DEBE UTILIZARSE ESTE METODO UNO POR UNO DE LOS ARBOLES
        /// </summary>
        /// <param name="detalles"></param>
        /// <returns></returns>
        private List<ConstructorCriterioDetalleAgrupacion> lTransformarConstructorDetalle(List<ConstructorCriterioDetalleAgrupacion> detalles)
        {


            List<ConstructorCriterioDetalleAgrupacion> resultado = new List<ConstructorCriterioDetalleAgrupacion>();
            List<ConstructorCriterioDetalleAgrupacion> detallesXOperador = new List<ConstructorCriterioDetalleAgrupacion>();//ocupo este
            ConstructorCriterioDetalleAgrupacion nodoPadre = new ConstructorCriterioDetalleAgrupacion();//ocupo este
            ConstructorCriterioDetalleAgrupacion nodoHijo = new ConstructorCriterioDetalleAgrupacion();//ocupo este 
            ConstructorCriterioDetalleAgrupacion nodo = new ConstructorCriterioDetalleAgrupacion();// ocupo este 
            List<String> idOperadores = detalles.Select(x => x.IdOperador).Distinct().ToList();
           
            
            String id = "";
            String idPadre = "";
            foreach (String item in idOperadores)
            { 
                int orden = 0;
                detallesXOperador = detalles.Where(x => x.IdOperador.Equals(item)).OrderBy(x=>(x.Orden==null?x.IdNivel:x.Orden)).ToList();// voy tener los detalles de agrupacion especificos 
                nodoPadre = new ConstructorCriterioDetalleAgrupacion();
                nodoPadre.DetalleAgrupacion = new DetalleAgrupacion();
                nodoPadre.DetalleAgrupacion.Operador = new Operador();
                nodoPadre.IdOperador = detallesXOperador[0].IdOperador;
                nodoPadre.DetalleAgrupacion.IdOperador = "0|0|" + detallesXOperador[0].IdOperador;
                nodoPadre.DetalleAgrupacion.Operador.NombreOperador = detallesXOperador[0].DetalleAgrupacion.Operador.NombreOperador;
                nodoPadre.ConstructorCriterioDetalleAgrupacion1 = new List<ConstructorCriterioDetalleAgrupacion>();
                for (int i = 0; i < detallesXOperador.Count; i++)
                {
                    nodoHijo = new ConstructorCriterioDetalleAgrupacion();
                    nodoHijo.DetalleAgrupacion = new DetalleAgrupacion();
                    nodoHijo.IdConstructorCriterio = detallesXOperador[i].IdConstructorCriterio;
                    nodoHijo.ConstructorCriterioDetalleAgrupacion2 = new ConstructorCriterioDetalleAgrupacion();
                    nodoHijo.ConstructorCriterioDetalleAgrupacion2.DetalleAgrupacion = new DetalleAgrupacion();
                    nodoHijo.ConstructorCriterioDetalleAgrupacion2.IdDetalleAgrupacion = (detallesXOperador[i].ConstructorCriterioDetalleAgrupacion2 == null ? 0 : detallesXOperador[i].ConstructorCriterioDetalleAgrupacion2.IdDetalleAgrupacion);
                    nodoHijo.ConstructorCriterioDetalleAgrupacion2.IdAgrupacion = (detallesXOperador[i].ConstructorCriterioDetalleAgrupacion2 == null ? 0 : detallesXOperador[i].ConstructorCriterioDetalleAgrupacion2.IdAgrupacion);
                    nodoHijo.ConstructorCriterioDetalleAgrupacion2.IdOperador = (detallesXOperador[i].ConstructorCriterioDetalleAgrupacion2 == null ? item : detallesXOperador[i].ConstructorCriterioDetalleAgrupacion2.IdOperador);
                    if (detallesXOperador[i].IdConstructorDetallePadre == null)
                    {
                        nodoHijo.ConstructorCriterioDetalleAgrupacion2.DetalleAgrupacion.IdOperador = nodoPadre.DetalleAgrupacion.IdOperador;
                    }
                    else
                    {
                        nodoHijo.IdConstructorDetallePadre = detallesXOperador[i].IdConstructorDetallePadre;
                    }
                    nodoHijo.IdDetalleAgrupacion = detallesXOperador[i].IdDetalleAgrupacion;
                    nodoHijo.IdAgrupacion = detallesXOperador[i].IdAgrupacion;
                    nodoHijo.IdOperador = detallesXOperador[i].IdOperador;
                    nodoHijo.UltimoNivel = detallesXOperador[i].UltimoNivel;
                    nodoHijo.IdNivel = detallesXOperador[i].IdNivel;
                    nodoHijo.Orden = detallesXOperador[i].Orden==null?orden:detallesXOperador[i].Orden;
                   
                    orden++;
                    if (detallesXOperador[i].UltimoNivel.Equals((byte)1))
                    {
                        nodoHijo.Regla = new Regla();

                        if (detallesXOperador[i].Regla != null)
                        {
                            nodoHijo.Regla.ValorLimiteInferior= detallesXOperador[i].Regla.ValorLimiteInferior;

                            var f = detallesXOperador.Where(x=> x.UltimoNivel == 1).ToList();

                            nodoHijo.Regla.ValorLimiteSuperior = detallesXOperador[i].Regla.ValorLimiteSuperior;
                            nodoHijo.IdTipoValor = detallesXOperador[i].IdTipoValor;
                            nodoHijo.IdNivelDetalle = detallesXOperador[i].IdNivelDetalle;
                        }
                        nodoHijo.UsaReglaEstadisticaConNivelDetalle = detallesXOperador[i].UsaReglaEstadisticaConNivelDetalle;
                        if (nodoHijo.UsaReglaEstadisticaConNivelDetalle == 1 )
                        {
                            nodoHijo.IdTipoValor = detallesXOperador[i].IdTipoValor;
                            nodoHijo.NivelDetalleReglaEstadistica = detallesXOperador[i].NivelDetalleReglaEstadistica;
                            nodoHijo.IdNivelDetalle = detallesXOperador[i].IdNivelDetalle;
                        }

                        nodoHijo.UsaReglaEstadistica = detallesXOperador[i].UsaReglaEstadistica;
                    }
                    nodo = lObtenerPadreDetalle(nodoPadre.ConstructorCriterioDetalleAgrupacion1.ToList(), nodoHijo);
                    //id
                    idPadre = lIdPadre(detallesXOperador[i]) + nodoPadre.DetalleAgrupacion.IdOperador;
                    id = detallesXOperador[i].IdDetalleAgrupacion.ToString() + "|" + detallesXOperador[i].IdAgrupacion.ToString() + "|" + detallesXOperador[i].IdOperador;
                    nodoHijo.DetalleAgrupacion.IdOperador = id + (idPadre != "" ? "|" + idPadre : "");
                    string[] ids = nodoHijo.DetalleAgrupacion.IdOperador.Split('|'); ;
                    nodoHijo.IdNivel =  (ids.Length / 3)-1;
                    //texto
                    nodoHijo.DetalleAgrupacion.DescDetalleAgrupacion = detallesXOperador[i].DetalleAgrupacion.Agrupacion.DescAgrupacion + "/" + detallesXOperador[i].DetalleAgrupacion.DescDetalleAgrupacion;
                    //padre
                    nodoHijo.ConstructorCriterioDetalleAgrupacion2.DetalleAgrupacion.IdOperador = (idPadre != "" ? idPadre : nodoPadre.DetalleAgrupacion.IdOperador);
                    nodoPadre.ConstructorCriterioDetalleAgrupacion1.Add(nodoHijo);
                }// Aqui termino 
                resultado.Add(nodoPadre);// aqui retorno 
            }
             return resultado;
        }
        /// <summary>
        ///  <method>NewMethod</method>
        /// transformar el detalle Agrupacion 
        /// a la forma que pueda leer 
        /// </summary>
        /// <param name="detalles"></param>
        /// <returns></returns>
        private ConstructorCriterioDetalleAgrupacion lTransformarConstructorDetalleAgrupacion(List<ConstructorCriterioDetalleAgrupacion> detalles, String IdOperador)
        {


            //List<ConstructorCriterioDetalleAgrupacion> resultado = new List<ConstructorCriterioDetalleAgrupacion>();
            List<ConstructorCriterioDetalleAgrupacion> detallesXOperador = new List<ConstructorCriterioDetalleAgrupacion>();//ocupo este
            ConstructorCriterioDetalleAgrupacion nodoPadre = new ConstructorCriterioDetalleAgrupacion();//ocupo este
            ConstructorCriterioDetalleAgrupacion nodoHijo = new ConstructorCriterioDetalleAgrupacion();//ocupo este 
            ConstructorCriterioDetalleAgrupacion nodo = new ConstructorCriterioDetalleAgrupacion();// ocupo este 
            List<String> idOperadores = detalles.Select(x => x.IdOperador).Distinct().ToList();


            String id = "";
            String idPadre = "";
           // foreach (String item in idOperadores)
           // {
                int orden = 0;
                detallesXOperador = detalles.OrderBy(x => (x.Orden == null ? x.IdNivel : x.Orden)).ToList();// voy tener los detalles de agrupacion especificos 
                nodoPadre = new ConstructorCriterioDetalleAgrupacion();
                nodoPadre.DetalleAgrupacion = new DetalleAgrupacion();
                nodoPadre.DetalleAgrupacion.Operador = new Operador();
                nodoPadre.IdOperador = detallesXOperador[0].IdOperador;
                nodoPadre.DetalleAgrupacion.IdOperador = "0|0|" + detallesXOperador[0].IdOperador;
                nodoPadre.DetalleAgrupacion.Operador.NombreOperador = detallesXOperador[0].DetalleAgrupacion.Operador.NombreOperador;
                nodoPadre.ConstructorCriterioDetalleAgrupacion1 = new List<ConstructorCriterioDetalleAgrupacion>();
                for (int i = 0; i < detallesXOperador.Count; i++)
                {
                    nodoHijo = new ConstructorCriterioDetalleAgrupacion();
                    nodoHijo.DetalleAgrupacion = new DetalleAgrupacion();
                    nodoHijo.IdConstructorCriterio = detallesXOperador[i].IdConstructorCriterio;
                    nodoHijo.ConstructorCriterioDetalleAgrupacion2 = new ConstructorCriterioDetalleAgrupacion();
                    nodoHijo.ConstructorCriterioDetalleAgrupacion2.DetalleAgrupacion = new DetalleAgrupacion();
                    nodoHijo.ConstructorCriterioDetalleAgrupacion2.IdDetalleAgrupacion = (detallesXOperador[i].ConstructorCriterioDetalleAgrupacion2 == null ? 0 : detallesXOperador[i].ConstructorCriterioDetalleAgrupacion2.IdDetalleAgrupacion);
                    nodoHijo.ConstructorCriterioDetalleAgrupacion2.IdAgrupacion = (detallesXOperador[i].ConstructorCriterioDetalleAgrupacion2 == null ? 0 : detallesXOperador[i].ConstructorCriterioDetalleAgrupacion2.IdAgrupacion);
                    nodoHijo.ConstructorCriterioDetalleAgrupacion2.IdOperador = (detallesXOperador[i].ConstructorCriterioDetalleAgrupacion2 == null ? IdOperador : detallesXOperador[i].ConstructorCriterioDetalleAgrupacion2.IdOperador);
                    if (detallesXOperador[i].IdConstructorDetallePadre == null)
                    {
                        nodoHijo.ConstructorCriterioDetalleAgrupacion2.DetalleAgrupacion.IdOperador = nodoPadre.DetalleAgrupacion.IdOperador;
                    }
                    else
                    {
                        nodoHijo.IdConstructorDetallePadre = detallesXOperador[i].IdConstructorDetallePadre;
                    }
                    nodoHijo.IdDetalleAgrupacion = detallesXOperador[i].IdDetalleAgrupacion;
                    nodoHijo.IdAgrupacion = detallesXOperador[i].IdAgrupacion;
                    nodoHijo.IdOperador = detallesXOperador[i].IdOperador;
                    nodoHijo.UltimoNivel = detallesXOperador[i].UltimoNivel;
                    nodoHijo.IdNivel = detallesXOperador[i].IdNivel;
                    //nodoHijo.IdNivelDetalleGenero = detallesXOperador[i].IdNivelDetalleGenero;

                    nodoHijo.Orden = detallesXOperador[i].Orden == null ? orden : detallesXOperador[i].Orden;

                    orden++;
                    if (detallesXOperador[i].UltimoNivel.Equals((byte)1))
                    {
                        nodoHijo.Regla = new Regla();

                        if (detallesXOperador[i].Regla != null)
                        {
                            nodoHijo.Regla.ValorLimiteInferior = detallesXOperador[i].Regla.ValorLimiteInferior;

                            var f = detallesXOperador.Where(x => x.UltimoNivel == 1).ToList();

                            nodoHijo.Regla.ValorLimiteSuperior = detallesXOperador[i].Regla.ValorLimiteSuperior;
                            nodoHijo.IdTipoValor = detallesXOperador[i].IdTipoValor;
                            nodoHijo.IdNivelDetalle = detallesXOperador[i].IdNivelDetalle;
                            nodoHijo.IdNivelDetalleGenero = detallesXOperador[i].IdNivelDetalleGenero;
                            
                        }
                      //  nodoHijo.IdNivelDetalleGenero = detallesXOperador[i].IdNivelDetalleGenero;
                        nodoHijo.UsaReglaEstadisticaConNivelDetalle = detallesXOperador[i].UsaReglaEstadisticaConNivelDetalle;
                        if (nodoHijo.UsaReglaEstadisticaConNivelDetalle == 1)
                        {
                            nodoHijo.IdTipoValor = detallesXOperador[i].IdTipoValor;
                            nodoHijo.NivelDetalleReglaEstadistica = detallesXOperador[i].NivelDetalleReglaEstadistica;
                            nodoHijo.IdNivelDetalle = detallesXOperador[i].IdNivelDetalle;
                            nodoHijo.IdNivelDetalleGenero = detallesXOperador[i].IdNivelDetalleGenero;
                        }

                        nodoHijo.UsaReglaEstadistica = detallesXOperador[i].UsaReglaEstadistica;
                    }
                    nodo = lObtenerPadreDetalle(nodoPadre.ConstructorCriterioDetalleAgrupacion1.ToList(), nodoHijo);
                    //id
                    idPadre = lIdPadre(detallesXOperador[i]) + nodoPadre.DetalleAgrupacion.IdOperador;
                    id = detallesXOperador[i].IdDetalleAgrupacion.ToString() + "|" + detallesXOperador[i].IdAgrupacion.ToString() + "|" + detallesXOperador[i].IdOperador;
                    nodoHijo.DetalleAgrupacion.IdOperador = id + (idPadre != "" ? "|" + idPadre : "");
                    string[] ids = nodoHijo.DetalleAgrupacion.IdOperador.Split('|'); ;
                    nodoHijo.IdNivel = (ids.Length / 3) - 1;
                    //texto
                    nodoHijo.DetalleAgrupacion.DescDetalleAgrupacion = detallesXOperador[i].DetalleAgrupacion.Agrupacion.DescAgrupacion + "/" + detallesXOperador[i].DetalleAgrupacion.DescDetalleAgrupacion;
                    //padre
                    nodoHijo.ConstructorCriterioDetalleAgrupacion2.DetalleAgrupacion.IdOperador = (idPadre != "" ? idPadre : nodoPadre.DetalleAgrupacion.IdOperador);
                    nodoPadre.ConstructorCriterioDetalleAgrupacion1.Add(nodoHijo);
                }// Aqui termino el for
               return nodoPadre;// aqui retorno 
           // }
           // return resultado;
        }
         
        private String lIdPadre(ConstructorCriterioDetalleAgrupacion detalle)
        {
            String id = "";
            if (detalle.IdConstructorDetallePadre == null)
            {
                id = "";
            }
            else
            {
                id = detalle.ConstructorCriterioDetalleAgrupacion2.IdDetalleAgrupacion + "|" + detalle.ConstructorCriterioDetalleAgrupacion2.IdAgrupacion + "|" + detalle.ConstructorCriterioDetalleAgrupacion2.IdOperador + "|" + lIdPadre(detalle.ConstructorCriterioDetalleAgrupacion2);
            }
            return id;
        }

        /// <summary>
        /// Obtiene el detalle agrupación padre deacuerdo al IdConstructorDetallePadre
        /// </summary>
        /// <param name="detalles"></param>
        /// <param name="detalle"></param>
        /// <returns></returns>
        public ConstructorCriterioDetalleAgrupacion lObtenerPadreDetalle(List<ConstructorCriterioDetalleAgrupacion> detalles, ConstructorCriterioDetalleAgrupacion detalle)
        {
            foreach (ConstructorCriterioDetalleAgrupacion item in detalles)
            {
                if (item.IdConstructorCriterio.Equals(detalle.IdConstructorDetallePadre))
                {
                    return item;
                }
            }
            return null;
        }


        #endregion

        #region Clonar
        // GET: /Constructor/Clonar
        [AuthorizeUserAttribute]
        public String Clonar(String IdConstructor)
        {
            Guid IdConstructorParse = Guid.Parse(IdConstructor);

            JSONResult<Constructor> jsonRespuesta = new JSONResult<Constructor>();
            Respuesta<Constructor> respuesta = new Respuesta<Constructor>();

            respuesta = refConstructorBL.Clonar(IdConstructorParse);

            if (!respuesta.blnIndicadorTransaccion)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = respuesta.strMensaje;
            }
            else
            {
                jsonRespuesta.data = respuesta.objObjeto;
            }

            string user;
            user = User.Identity.GetUserId();
            try
            {
                func.constructor(user, "Clonar Constructor", 2," ","ID Constructor: "+IdConstructor);
            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }

            return jsonRespuesta.toJSON();
        }

         #endregion
    }
}
