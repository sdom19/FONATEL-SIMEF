using GB.SUTEL.BL.Proceso;
using GB.SUTEL.Entities.Espectro;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.UI.Helpers;
using GB.SUTEL.UI.Recursos.Utilidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Web.Configuration;

using Microsoft.AspNet.Identity;
using GB.SUTEL.DAL;
using GB.SUTEL.Entities;
using GB.SUTEL.Shared;

namespace GB.SUTEL.UI.Controllers
{

    /// <summary>
    /// Este controlador maneja las acciones de carga y reemplazo de archivos
    /// </summary>
    /// <createddate>28/02/2016</createddate>
    /// <creator>Kevin Solís</creator>
    /// <lastmodificationdate>28/03/2016</lastmodificationdate



    [AuthorizeUserAttribute]
    public class EspectroController : BaseController
    {
        //Conexión a la base de datos
        private readonly SUTEL_IndicadoresEntities db = new SUTEL_IndicadoresEntities();
        Funcion func = new Funcion();

        #region atributos
        EspectroBL espectroBL;

        public List<Respuesta<ArchivoCsvModel>> ListaArchivosCsv = new List<Respuesta<ArchivoCsvModel>>();
        public List<Respuesta<ArchivoMifModel>> ListaArchivosMif = new List<Respuesta<ArchivoMifModel>>();
        public List<Respuesta<ArchivoTelevisionDigitalCsv>> ListaArchivosTvDigitalCsv = new List<Respuesta<ArchivoTelevisionDigitalCsv>>();
        public List<Respuesta<ArchivoIMTFijasCsv>> ListaArchivosIMTFijasCsv = new List<Respuesta<ArchivoIMTFijasCsv>>();
        public List<Respuesta<ArchivoBandaAngostaCsv>> ListaArchivosBandaACsv = new List<Respuesta<ArchivoBandaAngostaCsv>>();
        List<string> NombresErroneos = new List<string>();
        List<string> NombresConExtensionesErroneas = new List<string>();
        List<string> NombresErroneosPRUEBA = new List<string>();
        List<HttpPostedFileBase> fileGlobalList;
        #endregion

        #region constructor
        public EspectroController()
        {

            espectroBL = new EspectroBL(AppContext);
        }
        #endregion   detalleArchivo.Frecuencia = Int64.Parse(arrayValores[1]);
        //
        // GET: /Espectro/
        public ActionResult Espectro()
        {
            string user;
            user = User.Identity.GetUserId();
            try
            {
                func._index(user, "Espectro", "Cargar archivo espectro");
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }
            return View();
        }

        //
        // GET: /Espectro/Details/5
        public ActionResult Details()
        {
            return View("Espectro");
        }

        //
        // GET: /Espectro/Create
        public ActionResult Create()
        {
            return View("Espectro");

        }



        /// <summary>
        /// Metodo que permite subir un nuevo documento CSV o MIF
        /// </summary>
        /// <param name="fileList">Lista de archivos a subir</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(List<HttpPostedFileBase> fileList)
        {
            fileGlobalList = fileList;
            //Se eliminan todos los archivos de la carpeta insertar
            //var rutaDirArchivos = WebConfigurationManager.AppSettings["rutaCarpetaInsertar"];       
            //espectroBL.eliminarArchivos(rutaDirArchivos);


            string extension;
            string nombreArchivo = string.Empty;
            string[] separadorRuta = new string[1];
            bool formatoCsvNoCorrecto = false;
            bool formatoMifNoCorrecto = false;
            bool formatoColumnasNoCorrecto = false;
            bool formatoTipoDatosNoCorrecto = false;
            bool tituloArchivoNoCorrecto = false;
            var rutaReemplazar = WebConfigurationManager.AppSettings["rutaCarpetaReemplazar"];
            string rutaInsertar = WebConfigurationManager.AppSettings["rutaCarpetaInsertar"];
            bool existeNulos = false;
            bool existeTitulos = true;
            int contadorArchivos = 0;
            int numeroEspaciosEnBlanco = 0;
            int numeroEspaciosEnBlanco02 = 0;
            int contador = 0;

            var user = ((ClaimsIdentity)HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault();
            var usuario = user == null ? "" : user.Value;

            if (fileList != null && fileList.Count > 0 && fileList[0] != null)
            {

                //Se recorre la lista de archivos a subir
                foreach (var file in fileList)
                {
                    var fileDuplicado = file;
                    //Se obtiene la extensión del archivo
                    extension = file.FileName.Substring((file.FileName.Length - 3), 3);

                    nombreArchivo = ExtraerNombre(file);

                    //Se valida la extensión del archivo
                    if (ValidarExtensionArchivo(extension))
                    {
                        // MIF
                        if (extension.ToLower().Equals("mif"))
                        {

                            StreamReader reader = new StreamReader(file.InputStream);
                            var line = string.Empty;


                            while ((line = reader.ReadLine()) != null)
                            {

                                if (!espectroBL.LeerValidarContenidoArchivoMif(line))
                                    formatoMifNoCorrecto = true;
                            }

                            if (formatoMifNoCorrecto)
                            {

                                ListaArchivosMif.Add(new Respuesta<ArchivoMifModel>() { objObjeto = new ArchivoMifModel() { nombreArchivoMif = nombreArchivo }, blnIndicadorTransaccion = false, blnIndicadorState = -4 });


                            }
                            else
                            {

                                if (!espectroBL.archivoMifYaCargado(nombreArchivo).objObjeto)
                                    espectroBL.AgregarMifALista(nombreArchivo);

                                else
                                    GuardarArchivoEnCarpetaReemplazar(file);
                            }
                        }
                        // CSV  
                        else
                        {
                            //Se valida el contenido del archivo
                            StreamReader reader = new StreamReader(file.InputStream);
                            var line = string.Empty;


                            if (espectroBL.ValidarNombreArchivoCsv(nombreArchivo))
                            {
                                //Se elimina la extensión del nombre, y delimita separarlo por '_' y asi verificar si es ETL o no
                                string[] nombreArchivoUltimo = nombreArchivo.Substring(0, (nombreArchivo.Length - 4)).Split(new Char[] { '_' });

                                //Se valida si el archivo es ETL
                                if (System.Text.RegularExpressions.Regex.IsMatch(nombreArchivoUltimo[2], "ETL"))
                                {
                                    while ((line = reader.ReadLine()) != null)
                                    {
                                        //Se valida si la línea que se está leyendo está vacía o si es la primer línea del archivo (Encabezado), que no interesa.
                                        if (!string.IsNullOrEmpty(line) && !line.Substring(0, 2).Contains("T"))
                                        {
                                            if (espectroBL.LeerValidarColumnas(line))
                                            {
                                                if (!espectroBL.LeerValidarContenidoArchivoCsvETL())
                                                {
                                                    formatoTipoDatosNoCorrecto = true;

                                                    //-6 Error, tipo de datos
                                                    ListaArchivosTvDigitalCsv.Add(new Respuesta<ArchivoTelevisionDigitalCsv>() { objObjeto = new ArchivoTelevisionDigitalCsv() { nombreArchivo = nombreArchivo }, blnIndicadorTransaccion = false, blnIndicadorState = -6 });
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                formatoColumnasNoCorrecto = true;

                                                //-5 Error, columnas
                                                ListaArchivosTvDigitalCsv.Add(new Respuesta<ArchivoTelevisionDigitalCsv>() { objObjeto = new ArchivoTelevisionDigitalCsv() { nombreArchivo = nombreArchivo }, blnIndicadorTransaccion = false, blnIndicadorState = -5 });
                                                break;
                                            }
                                        }
                                    }

                                    if (!formatoTipoDatosNoCorrecto && !formatoColumnasNoCorrecto)
                                    {
                                        //Se agrega el true, para indicar que es archivo ETL
                                        if (!espectroBL.archivoCsvYaCargado(nombreArchivo, true).objObjeto)
                                            espectroBL.AgregarCsvALista(nombreArchivo, true);

                                        else
                                            GuardarArchivoEnCarpetaReemplazar(file, true);
                                    }

                                }
                                else
                                {
                                    //Se valida si el archivo es IMT
                                    if (System.Text.RegularExpressions.Regex.IsMatch(nombreArchivoUltimo[0], "IMT"))
                                    {




                                        while ((line = reader.ReadLine()) != null)
                                        {

                                            //funciona para saber si el archivo viene totalmente en blanco
                                            if (line.Trim() == "")
                                            {
                                                numeroEspaciosEnBlanco++;
                                            }
                                            contador++;


                                            //funciona para saber si el archivo viene con algunas lineas en blanco expetuando
                                            //las ultimas lineas del archivo
                                            if (line.Trim() == "")
                                            {
                                                numeroEspaciosEnBlanco02++;
                                            }
                                            else
                                            {
                                                if (numeroEspaciosEnBlanco02 > 0)
                                                {
                                                    existeNulos = true;
                                                    break;
                                                }
                                            }
                                            if (contadorArchivos == 0)
                                            {
                                                //Valida si los titulos de las columnas estan correctas con ayuda del contador que verifica si es la primer vez que ingresa 
                                                if (!espectroBL.validaTitulosColumnasArchivoBA_IMT(line))
                                                {
                                                    existeTitulos = false;
                                                    break;
                                                }
                                                //Valida si el titulo del archivo lleva el formato correcto 
                                                if (!espectroBL.validarTituloArchivoIMT_BA(nombreArchivo))
                                                {
                                                    //-8 Error: Tipos de datos incorrectos
                                                    ListaArchivosIMTFijasCsv.Add(new Respuesta<ArchivoIMTFijasCsv>() { objObjeto = new ArchivoIMTFijasCsv() { nombreArchivo = nombreArchivo }, blnIndicadorTransaccion = false, blnIndicadorState = -6 });
                                                    tituloArchivoNoCorrecto = true;
                                                    break;

                                                }
                                            }

                                            //Se valida si la línea que se está leyendo está vacía o si es la primer línea del archivo (Encabezado), que no interesa.
                                            if (!string.IsNullOrEmpty(line) && !line.Substring(0, 2).Contains("T"))
                                            {
 
                                                if (!espectroBL.validarDatosArchivoIMT_BA(line))
                                                {
                                                    //-8 Error: Tipos de datos incorrectos
                                                    ListaArchivosIMTFijasCsv.Add(new Respuesta<ArchivoIMTFijasCsv>() { objObjeto = new ArchivoIMTFijasCsv() { nombreArchivo = nombreArchivo }, blnIndicadorTransaccion = false, blnIndicadorState = -8 });
                                                    formatoTipoDatosNoCorrecto = true;
                                                    break;

                                                }
                                                if (espectroBL.LeerValidarColumnas(line, true))
                                                {

                                                    if (!espectroBL.LeerValidarArchivoCsvIMTBandaA())
                                                    {
                                                        formatoTipoDatosNoCorrecto = true;

                                                        //-6 Error, tipo de datos
                                                        ListaArchivosIMTFijasCsv.Add(new Respuesta<ArchivoIMTFijasCsv>() { objObjeto = new ArchivoIMTFijasCsv() { nombreArchivo = nombreArchivo }, blnIndicadorTransaccion = false, blnIndicadorState = -6 });
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    formatoColumnasNoCorrecto = true;

                                                    //-5 Error, columnas
                                                    ListaArchivosIMTFijasCsv.Add(new Respuesta<ArchivoIMTFijasCsv>() { objObjeto = new ArchivoIMTFijasCsv() { nombreArchivo = nombreArchivo }, blnIndicadorTransaccion = false, blnIndicadorState = -5 });
                                                    break;
                                                }
                                            }
                                            contadorArchivos++;
                                        }
                                        if (contador == numeroEspaciosEnBlanco || existeNulos || contador == 0)
                                        {
                                            //-7 Error, No trae datos el archivo CSV
                                            ListaArchivosBandaACsv.Add(new Respuesta<ArchivoBandaAngostaCsv>() { objObjeto = new ArchivoBandaAngostaCsv() { nombreArchivo = nombreArchivo }, blnIndicadorTransaccion = false, blnIndicadorState = -7 });

                                        }
                                        else if (!existeTitulos)
                                        {
                                            ListaArchivosBandaACsv.Add(new Respuesta<ArchivoBandaAngostaCsv>() { objObjeto = new ArchivoBandaAngostaCsv() { nombreArchivo = nombreArchivo }, blnIndicadorTransaccion = false, blnIndicadorState = -5 });

                                        }else if (!formatoTipoDatosNoCorrecto && !formatoColumnasNoCorrecto && !tituloArchivoNoCorrecto)
                                        {
                                            //Se agrega el true, para indicar que es archivo IMT
                                            if (!espectroBL.archivoCsvYaCargado(nombreArchivo, false, true).objObjeto)
                                            {
                                                espectroBL.AgregarCsvALista(nombreArchivo, false, true);
                                                GuardarArchivoEnCarpetaInsertar(file);
                                            }else {
                                                GuardarArchivoEnCarpetaReemplazar(file, false, true);
                                            }
                                                
                                        }
                                    }
                                    else
                                    {
                                        //Se valida si el archivo es Banda Angosta
                                        if (System.Text.RegularExpressions.Regex.IsMatch(nombreArchivoUltimo[0], "BA"))
                                        {
                                            while ((line = reader.ReadLine()) != null)
                                            {
                                                //funciona para saber si el archivo viene totalmente en blanco
                                                if (line.Trim() == "")
                                                {
                                                    numeroEspaciosEnBlanco++;
                                                }
                                                contador++;


                                                //funciona para saber si el archivo viene con algunas lineas en blanco expetuando
                                                //las ultimas lineas del archivo
                                                if (line.Trim() == "")
                                                {
                                                    numeroEspaciosEnBlanco02++;
                                                }
                                                else
                                                {
                                                    if (numeroEspaciosEnBlanco02 > 0)
                                                    {
                                                        existeNulos = true;
                                                        break;
                                                    }
                                                }

                                                
                                                if (contadorArchivos == 0) {
                                                    //Valida si los titulos de las columnas estan correctas con ayuda del contador que verifica si es la primer vez que ingresa 
                                                    if (!espectroBL.validaTitulosColumnasArchivoBA_IMT(line))
                                                    {
                                                        existeTitulos = false;
                                                        break;
                                                    }
                                                    //Valida si el titulo del archivo lleva el formato correcto 
                                                    if (!espectroBL.validarTituloArchivoIMT_BA(nombreArchivo))
                                                    {
                                                        //-8 Error: Tipos de datos incorrectos
                                                        ListaArchivosIMTFijasCsv.Add(new Respuesta<ArchivoIMTFijasCsv>() { objObjeto = new ArchivoIMTFijasCsv() { nombreArchivo = nombreArchivo }, blnIndicadorTransaccion = false, blnIndicadorState = -6 });
                                                        tituloArchivoNoCorrecto = true;
                                                        break;

                                                    }

                                                }
                                                //Se valida si la línea que se está leyendo está vacía o si es la primer línea del archivo (Encabezado), que no interesa.
                                                if (!string.IsNullOrEmpty(line) && !line.Substring(0, 2).Contains("T"))
                                                {

                                                    if (!espectroBL.validarDatosArchivoIMT_BA(line))
                                                    {
                                                        //-8 Error: Tipos de datos incorrectos
                                                        ListaArchivosIMTFijasCsv.Add(new Respuesta<ArchivoIMTFijasCsv>() { objObjeto = new ArchivoIMTFijasCsv() { nombreArchivo = nombreArchivo }, blnIndicadorTransaccion = false, blnIndicadorState = -8 });
                                                        formatoTipoDatosNoCorrecto = true;
                                                        break;

                                                    }

                                                    if (espectroBL.LeerValidarColumnas(line, false, true))
                                                    {
                                                        if (espectroBL.validarDatosArchivoIMT_BA(line)) {
                                                            if (!espectroBL.LeerValidarArchivoCsvIMTBandaA(false))
                                                            {
                                                                formatoTipoDatosNoCorrecto = true;

                                                                //-6 Error, tipo de datos
                                                                ListaArchivosBandaACsv.Add(new Respuesta<ArchivoBandaAngostaCsv>() { objObjeto = new ArchivoBandaAngostaCsv() { nombreArchivo = nombreArchivo }, blnIndicadorTransaccion = false, blnIndicadorState = -6 });
                                                                break;
                                                            }

                                                        }
     
                                                    }
                                                    else
                                                    {
                                                        formatoColumnasNoCorrecto = true;

                                                        //-5 Error, columnas
                                                        ListaArchivosBandaACsv.Add(new Respuesta<ArchivoBandaAngostaCsv>() { objObjeto = new ArchivoBandaAngostaCsv() { nombreArchivo = nombreArchivo }, blnIndicadorTransaccion = false, blnIndicadorState = -5 });
                                                        break;
                                                    }
                                                }
                                                contadorArchivos++;
                                            }

                                            // valida si el archivo viene totalmente en blanco o si viene algunas filas en blanco
                                            // con los resultados de las valiadaciones anteriores
                                            if (contador == numeroEspaciosEnBlanco || existeNulos || contador == 0)
                                            {
                                                //-7 Error, Trae filas en blanco
                                                ListaArchivosBandaACsv.Add(new Respuesta<ArchivoBandaAngostaCsv>() { objObjeto = new ArchivoBandaAngostaCsv() { nombreArchivo = nombreArchivo }, blnIndicadorTransaccion = false, blnIndicadorState = -7 });

                                            }
                                            else if (!existeTitulos) {
                                                ListaArchivosBandaACsv.Add(new Respuesta<ArchivoBandaAngostaCsv>() { objObjeto = new ArchivoBandaAngostaCsv() { nombreArchivo = nombreArchivo }, blnIndicadorTransaccion = false, blnIndicadorState = -5 });

                                            }
                                            else if (!formatoTipoDatosNoCorrecto && !formatoColumnasNoCorrecto && !tituloArchivoNoCorrecto)
                                            {
                                                //Se agrega el true, para indicar que es archivo Banda Angosta
                                                if (!espectroBL.archivoCsvYaCargado(nombreArchivo, false, false, true).objObjeto)
                                                {
                                                    espectroBL.AgregarCsvALista(nombreArchivo, false, false, true);
                                                    GuardarArchivoEnCarpetaInsertar(file);
                                                }
                                                else
                                                {
                                                    //ListaArchivosBandaACsv.Add(new Respuesta<ArchivoBandaAngostaCsv>() { objObjeto = new ArchivoBandaAngostaCsv() { nombreArchivo = nombreArchivo }, blnIndicadorTransaccion = false, blnIndicadorState = -1 });
                                                    GuardarArchivoEnCarpetaReemplazar(file, false, false, true);
                                                }
                                                        
                                            }

                                           
                                        }
                                        else
                                        {
                                            while ((line = reader.ReadLine()) != null)
                                            {

                                                if (!espectroBL.LeerValidarContenidoArchivoCsv(line))
                                                    formatoCsvNoCorrecto = true;
                                            }

                                            if (formatoCsvNoCorrecto)
                                            {
                                                ListaArchivosCsv.Add(new Respuesta<ArchivoCsvModel>() { objObjeto = new ArchivoCsvModel() { nombreArchivo = nombreArchivo }, blnIndicadorTransaccion = false, blnIndicadorState = -4 });

                                            }
                                            else
                                            {

                                                if (!espectroBL.archivoCsvYaCargado(nombreArchivo).objObjeto)
                                                    espectroBL.AgregarCsvALista(nombreArchivo);

                                                else
                                                    GuardarArchivoEnCarpetaReemplazar(file);
                                            }
                                        }
                                    }
                                }
                            }
                            else { NombresErroneos.Add(nombreArchivo); }
                        }

                    } // valida Extension END
                    else
                    { NombresConExtensionesErroneas.Add(nombreArchivo); }

                }//fin foreach

                //Se guardan los archivos en BD 
                espectroBL.InsertarArchivos(AppContext.MachineName, usuario);

                //Se recuperan los resultados de transacciones

                foreach (var item in espectroBL.listaRespuestasCsv)
                {

                    ListaArchivosCsv.Add(item);
                }

                foreach (var item in espectroBL.listaRespuestasMif)
                {

                    ListaArchivosMif.Add(item);
                }

                foreach (var item in espectroBL.listaRespuestasCsvETL)
                {
                    ListaArchivosTvDigitalCsv.Add(item);
                }

                foreach (var item in espectroBL.listaRespuestasCsvIMTFijas)
                {
                    ListaArchivosIMTFijasCsv.Add(item);
                }

                foreach (var item in espectroBL.listaRespuestasCsvBandaA)
                {
                    ListaArchivosBandaACsv.Add(item);
                }

            }
            else //ESTE ES LA PRIMER PANTALLA QUE APARECE DE ESPECTRO
            {     //No hay archivos seleccionados

                ListaArchivosCsv = null;
                ListaArchivosMif = null;
                ListaArchivosTvDigitalCsv = null;
                ListaArchivosIMTFijasCsv = null;
                ListaArchivosBandaACsv = null;
                NombresErroneos = null;
                NombresConExtensionesErroneas = null;
                return View("Espectro");
            }

            //AQUI ES PARA LOS MENSAJES QUE SE MUESTRAN EN PANTALLA
            //Se envia de la siguiente forma debido a que Tuple, solo permite 4 parametros.. cuando ya se envia más de eso la vista no lo encuentra.
            ViewBag.ListaTuple = new Tuple<List<Respuesta<ArchivoCsvModel>>,
                                              List<Respuesta<ArchivoMifModel>>,
                                              List<string>,
                                              List<string>,
                                              List<Respuesta<ArchivoTelevisionDigitalCsv>>,
                                              List<Respuesta<ArchivoIMTFijasCsv>>,
                                              List<Respuesta<ArchivoBandaAngostaCsv>>>(ListaArchivosCsv, ListaArchivosMif, NombresErroneos, 
                                              NombresConExtensionesErroneas, ListaArchivosTvDigitalCsv, ListaArchivosIMTFijasCsv, ListaArchivosBandaACsv);
            return View("Details");

        }


        [HttpPost]
        public ActionResult Reemplazar(FormCollection collection)
        {
            if (collection.Count > 0 && collection != null)
            {
                List<string> listaNombres = new List<string>();

                var user = ((ClaimsIdentity)HttpContext.GetOwinContext().Authentication.User.Identity).Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault();
                var usuario = user == null ? "" : user.Value;

                foreach (var key in collection.AllKeys)
                {
                    listaNombres.Add(collection[key]);
                }

                //Se mueven los archivos de carpeta reemplazar a insertar

                var rutaReemplazar = WebConfigurationManager.AppSettings["rutaCarpetaReemplazar"];
                string rutaCarpetaInsertar = WebConfigurationManager.AppSettings["rutaCarpetaInsertar"];


                

                //Se eliminan registros relacionados a los archivos ya registrados en la BD

                var todosEliminadosCorrectamente = true;

                foreach (var item in listaNombres)
                {
                    var boolRespuesta = false;
                    if (item.Substring(item.Length - 3, 3).ToLower().Equals("mif"))
                    {
                        boolRespuesta = espectroBL.EliminarArchivosMif(item).objObjeto;
                        ListaArchivosMif.Add(new Respuesta<ArchivoMifModel>() { objObjeto = new ArchivoMifModel() { nombreArchivoMif = item }, blnIndicadorTransaccion = true, blnIndicadorState = -2 });

                    }
                    else
                    {
                        //Se obtiene el nombre del archivo
                        var nombreArchivo = item.Substring(0, item.Length - 4);

                        //Si el archivo es ETL
                        if (nombreArchivo.Substring(nombreArchivo.Length - 3, 3).Equals("ETL"))
                        {
                            boolRespuesta = espectroBL.EliminarArchivosCsv(item, true).objObjeto;
                            ListaArchivosTvDigitalCsv.Add(new Respuesta<ArchivoTelevisionDigitalCsv>() { objObjeto = new ArchivoTelevisionDigitalCsv() { nombreArchivo = item }, blnIndicadorTransaccion = true, blnIndicadorState = -2 });

                        }
                        else
                        {
                            if (nombreArchivo.Substring(0, 3).Equals("IMT"))
                            {
                                boolRespuesta = espectroBL.EliminarArchivosCsv(item, false, true).objObjeto;
                                ListaArchivosIMTFijasCsv.Add(new Respuesta<ArchivoIMTFijasCsv>() { objObjeto = new ArchivoIMTFijasCsv() { nombreArchivo = item }, blnIndicadorTransaccion = true, blnIndicadorState = -2 });

                            }
                            else
                            {
                                if(nombreArchivo.Substring(0, 2).Equals("BA"))
                                {
                                    boolRespuesta = espectroBL.EliminarArchivosCsv(item, false, false, true).objObjeto;
                                    ListaArchivosBandaACsv.Add(new Respuesta<ArchivoBandaAngostaCsv>() { objObjeto = new ArchivoBandaAngostaCsv() { nombreArchivo = item }, blnIndicadorTransaccion = true, blnIndicadorState = -2 });

                                }
                                else
                                {
                                    boolRespuesta = espectroBL.EliminarArchivosCsv(item).objObjeto;
                                    ListaArchivosCsv.Add(new Respuesta<ArchivoCsvModel>() { objObjeto = new ArchivoCsvModel() { nombreArchivo = item }, blnIndicadorTransaccion = true, blnIndicadorState = -2 });

                                }
                            }
                        }

                    }

                    if (!boolRespuesta)
                    {
                        todosEliminadosCorrectamente = boolRespuesta;
                    }
                }

                espectroBL.LeerArchivosAReemplazar(listaNombres, rutaReemplazar, usuario, AppContext.MachineName);

                foreach (var nombre in listaNombres)
                {
                    System.IO.File.Move(rutaReemplazar + "\\" + nombre, rutaCarpetaInsertar + "\\" + nombre);
                }

                //Se eliminan todos de carpeta Reemplazar               
                espectroBL.eliminarArchivos(rutaReemplazar);

                //Se insertan los nuevos archivos en BD

                if (todosEliminadosCorrectamente)
                {
                    //Se envia de la siguiente forma debido a que Tuple, solo permite 4 parametros.. cuando ya se envia más de eso la vista no lo encuentra.
                    ViewBag.ListaTuple = new Tuple<List<Respuesta<ArchivoCsvModel>>,
                                                      List<Respuesta<ArchivoMifModel>>,
                                                      List<string>,
                                                      List<string>,
                                                      List<Respuesta<ArchivoTelevisionDigitalCsv>>,
                                                      List<Respuesta<ArchivoIMTFijasCsv>>,
                                                      List<Respuesta<ArchivoBandaAngostaCsv>>>(ListaArchivosCsv, ListaArchivosMif, NombresErroneos, 
                                                      NombresConExtensionesErroneas, ListaArchivosTvDigitalCsv, ListaArchivosIMTFijasCsv, ListaArchivosBandaACsv);
                    return View("Details");

                }

            }

            ListaArchivosCsv = null;
            ListaArchivosMif = null;
            ListaArchivosTvDigitalCsv = null;
            ListaArchivosIMTFijasCsv = null;
            ListaArchivosBandaACsv = null;
            NombresErroneos = null;
            NombresConExtensionesErroneas = null;


            //Se envia de la siguiente forma debido a que Tuple, solo permite 4 parametros.. cuando ya se envia más de eso la vista no lo encuentra.
            ViewBag.ListaTuple = new Tuple<List<Respuesta<ArchivoCsvModel>>,
                                              List<Respuesta<ArchivoMifModel>>,
                                              List<string>,
                                              List<string>,
                                              List<Respuesta<ArchivoTelevisionDigitalCsv>>,
                                              List<Respuesta<ArchivoIMTFijasCsv>>,
                                              List<Respuesta<ArchivoBandaAngostaCsv>>>(ListaArchivosCsv, ListaArchivosMif, NombresErroneos,
                                              NombresConExtensionesErroneas, ListaArchivosTvDigitalCsv, ListaArchivosIMTFijasCsv, ListaArchivosBandaACsv);
            return View("Details");

        }


        /// <summary>
        /// verifica la existencia del Archivo
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        [AuthorizeUserAttribute]
        public string ExistenciaDelArchivo()
        {
            string nombredelArchivo = Request.Params["data[NombredelArchivo]"];
            string rutaCarpetaInsertar = WebConfigurationManager.AppSettings["rutaCarpetaInsertar"];
            string rutaCarpetaReemplazar = WebConfigurationManager.AppSettings["rutaCarpetaReemplazar"];
            JSONResult<List<string>> jsonRespuesta = new JSONResult<List<string>>();
            string extension;

            try
            {
                extension = nombredelArchivo.Substring((nombredelArchivo.Length - 3), 3);

                if (ValidarExtensionArchivo(extension))
                {
                    if (nombredelArchivo.Substring(nombredelArchivo.Length - 3, 3).ToLower().Equals("mif")||
                        nombredelArchivo.Substring(nombredelArchivo.Length - 3, 3).ToLower().Equals("csv"))
                    {
                        if (espectroBL.VerificaArchivoCsv(nombredelArchivo, rutaCarpetaInsertar, rutaCarpetaReemplazar).objObjeto)
                        {
                            jsonRespuesta.ok = true;
                            jsonRespuesta.strMensaje = string.Format(Logica.MensajeConfirmacionEliminaArchivo, nombredelArchivo);
                        }
                        else
                        {
                            jsonRespuesta.ok = false;
                            jsonRespuesta.strMensaje = string.Format(Logica.MensajeErrorArchivoNoCargador, nombredelArchivo);
                        }
                    }

                    if (nombredelArchivo.Substring(nombredelArchivo.Length - 3, 3).ToLower().Equals("csv"))
                    {
                        //if (formato(nombredelArchivo))
                        //{
                        //    if (espectroBL.archivoCsvYaCargado(nombredelArchivo, nombredelArchivo.Contains(Logica.FormatoArchivoTVdigital),
                        //        nombredelArchivo.Contains(Logica.FormatoArchivoIMT),
                        //        nombredelArchivo.Contains(Logica.FormatoArchivoBandaAngosta)).objObjeto)
                        //    {
                        //        jsonRespuesta.ok = true;
                        //        jsonRespuesta.strMensaje = string.Format(Logica.MensajeConfirmacionEliminaArchivo, nombredelArchivo);
                        //    }
                        //    else
                        //    {
                        //        jsonRespuesta.ok = false;
                        //        jsonRespuesta.strMensaje = string.Format(Logica.MensajeErrorArchivoNoCargador, nombredelArchivo);
                        //    }
                        //}
                        //else
                        //{
                        //    jsonRespuesta.ok = false;
                        //    jsonRespuesta.strMensaje = string.Format(Logica.MensajeErrorArchivoNoCumpleFormato, nombredelArchivo); ;
                        //}
                    }

                }
                else
                {
                    jsonRespuesta.ok = false;
                    jsonRespuesta.strMensaje = string.Format(Logica.MensajeErrorArchivoNoCumpleFormatoCSVMIF, nombredelArchivo);
                }
            }
            catch (Exception ex)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = string.Format(Logica.ErrorNoControlado, ex.Message);
                return jsonRespuesta.toJSON();
            }
            return jsonRespuesta.toJSON();
        }

        public bool formato(string nombre)
        {
            bool format = false;
            try
            {
                string[] array = nombre.Split('_');
                if (array.Length == 3 || array.Length == 4)
                {
                    format = true;
                }
                else
                {
                    format = false;
                }

            }
            catch
            {
                format = false;
            }
            return format;
        }

        /// <summary>
        /// Este metdo recive el nombre del Archivo a Eliminar ya sea .mif o .csv 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeUserAttribute]
        public string EliminarArchivo()
        {
            string rutaCarpetaInsertar = WebConfigurationManager.AppSettings["rutaCarpetaInsertar"];
            string rutaCarpetaReemplazar = WebConfigurationManager.AppSettings["rutaCarpetaReemplazar"];

            JSONResult<List<string>> jsonRespuesta = new JSONResult<List<string>>();
            string nombredelArchivo = Request.Params["data[NombredelArchivo]"];
            try
            {
                if (nombredelArchivo.Substring(nombredelArchivo.Length - 3, 3).ToLower().Equals("mif"))
                {
                    if (espectroBL.EliminarArchivosMif(nombredelArchivo).objObjeto)
                    {
                        jsonRespuesta.ok = true;
                        jsonRespuesta.strMensaje = string.Format(Logica.MensajeArchivoEliminadoCorectamente, nombredelArchivo);
                    }
                    else
                    {
                        jsonRespuesta.ok = false;
                        jsonRespuesta.strMensaje = string.Format(Logica.MensajeArchivoNoFueEliminado, nombredelArchivo);
                    }
                    //  ListaArchivosMif.Add(new Respuesta<ArchivoMifModel>() { objObjeto = new ArchivoMifModel() { nombreArchivoMif = nombredelArchivo }, blnIndicadorTransaccion = true, blnIndicadorState = -2 });
                }

                if (nombredelArchivo.Substring(nombredelArchivo.Length - 3, 3).ToLower().Equals("csv"))
                {

                    if (espectroBL.EliminarArchivosCsv(nombredelArchivo, nombredelArchivo.Contains(Logica.FormatoArchivoTVdigital), nombredelArchivo.Contains(Logica.FormatoArchivoIMT), nombredelArchivo.Contains(Logica.FormatoArchivoBandaAngosta), rutaCarpetaInsertar, rutaCarpetaReemplazar).objObjeto)
                    {

                        jsonRespuesta.ok = true;
                        jsonRespuesta.strMensaje = string.Format(Logica.MensajeArchivoEliminadoCorectamente, nombredelArchivo);
                    }
                    else
                    {
                        jsonRespuesta.ok = false;
                        jsonRespuesta.strMensaje = string.Format(Logica.MensajeArchivoNoFueEliminado, nombredelArchivo);
                    }
                }
            }
            catch (Exception ex)
            {
                jsonRespuesta.ok = false;
                jsonRespuesta.strMensaje = string.Format(Logica.ErrorNoControlado, ex.Message);
                return jsonRespuesta.toJSON();
            }

            return jsonRespuesta.toJSON();
        }



        /// <summary>
        /// La función valida que que la extensión del archivo sea Mif o Csv.
        /// </summary>
        /// <param name="extension">extensión se refiere a la extension del archivo que se encuentra en lectura</param>
        /// <returns>Si la extensión que se está leyendo NO es Mif ni Csv retornará false, de lo contrario, true.</returns>
        /// 
        public bool ValidarExtensionArchivo(string extension)
        {
            if (extension.ToLower().Equals("mif") || extension.ToLower().Equals("csv"))
                return true;
            else
                return false;
        }


        public bool EsCsv(string nombreArchivo)
        {
            var extension = nombreArchivo.Substring(nombreArchivo.Length - 3, 3);

            if (extension.ToLower().Equals("csv"))
                return true;
            else
                return false;
        }

        public string ExtraerNombre(HttpPostedFileBase file)
        {
            string[] separadorRuta;

            //el nombre varía dependiendo del navegador
            separadorRuta = file.FileName.Split(new char[] { '\\' });

            if (separadorRuta.Length == 1) //chrome
                return file.FileName;
            else
                return separadorRuta[(separadorRuta.Length - 1)];
        }


        private void GuardarArchivoEnCarpetaInsertar(HttpPostedFileBase file)
        {

            var nombreArchivo = ExtraerNombre(file);
            var fileName = Path.GetFileName(nombreArchivo);
            // store the file inside ~/App_Data/uploads folder

            //var path = WebConfigurationManager.AppSettings["rutaCarpetaInsertar"];
            //file.SaveAs(path);

            var pathInsertar = WebConfigurationManager.AppSettings["rutaCarpetaInsertar"];
            var pathRemplazar = WebConfigurationManager.AppSettings["rutaCarpetaReemplazar"];

            System.IO.File.Delete(pathInsertar + "\\" + nombreArchivo);
            System.IO.File.Delete(pathRemplazar + "\\" + nombreArchivo);

            file.SaveAs(pathInsertar + "\\" + fileName);

            //if (EsCsv(nombreArchivo))
            //    ListaArchivosCsv.Add(new Respuesta<ArchivoCsvModel>() { objObjeto = new ArchivoCsvModel() { nombreArchivo = nombreArchivo }, blnIndicadorTransaccion = true, blnIndicadorState = 1 });
            //else
            //    ListaArchivosMif.Add(new Respuesta<ArchivoMifModel>() { objObjeto = new ArchivoMifModel() { nombreArchivoMif = nombreArchivo }, blnIndicadorTransaccion = true, blnIndicadorState = 1 });

        }


        private void GuardarArchivoEnCarpetaReemplazar(HttpPostedFileBase file, bool archivoETL = false, bool archivoIMTFija = false, bool archivoBandaAn = false)
        {
            var nombreArchivo = ExtraerNombre(file);
            var fileName = Path.GetFileName(nombreArchivo);
            // store the file inside ~/App_Data/uploads folder
            var path = WebConfigurationManager.AppSettings["rutaCarpetaReemplazar"];
            //path += "\\" + fileName;
            //file.SaveAs(path);

            var pathInsertar = WebConfigurationManager.AppSettings["rutaCarpetaInsertar"];
            var pathRemplazar = WebConfigurationManager.AppSettings["rutaCarpetaReemplazar"];

            System.IO.File.Delete(pathInsertar + "\\" + fileName);
            System.IO.File.Delete(pathRemplazar + "\\" + fileName);

            file.SaveAs(pathRemplazar + "\\" + fileName);

            if (EsCsv(nombreArchivo))
                if (archivoETL)
                {
                    ListaArchivosTvDigitalCsv.Add(new Respuesta<ArchivoTelevisionDigitalCsv>() { objObjeto = new ArchivoTelevisionDigitalCsv() { nombreArchivo = nombreArchivo }, blnIndicadorTransaccion = false, blnIndicadorState = -1 });
                }
                else
                {
                    if (archivoIMTFija)
                    {
                        ListaArchivosIMTFijasCsv.Add(new Respuesta<ArchivoIMTFijasCsv>() { objObjeto = new ArchivoIMTFijasCsv() { nombreArchivo = nombreArchivo }, blnIndicadorTransaccion = false, blnIndicadorState = -1 });
                    }
                    else
                    {
                        if (archivoBandaAn)
                        {
                            ListaArchivosBandaACsv.Add(new Respuesta<ArchivoBandaAngostaCsv>() { objObjeto = new ArchivoBandaAngostaCsv() { nombreArchivo = nombreArchivo }, blnIndicadorTransaccion = false, blnIndicadorState = -1 });
                        }
                        else
                        {
                            ListaArchivosCsv.Add(new Respuesta<ArchivoCsvModel>() { objObjeto = new ArchivoCsvModel() { nombreArchivo = nombreArchivo }, blnIndicadorTransaccion = false, blnIndicadorState = -1 });
                        }
                    }
                }
            else
                ListaArchivosMif.Add(new Respuesta<ArchivoMifModel>() { objObjeto = new ArchivoMifModel() { nombreArchivoMif = nombreArchivo }, blnIndicadorTransaccion = false, blnIndicadorState = -1 });

        }

    }

}
