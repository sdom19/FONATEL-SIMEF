using OfficeOpenXml;
using OfficeOpenXml.DataValidation;
using OfficeOpenXml.Drawing;
using SutelSolution.Models; // to use CommandType
using System;
using System.Collections.Generic;
using System.Configuration;
//SQL 
using System.Data.SqlClient;
// Epplus
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
/// para usar metodos
/// 
//using GB.SUTEL.Entities;

//using GB.SUTEL.BL.FuenteExternas;
//using GB.SUTEL.BL.Mantenimientos;
//using GB.SUTEL.UI.Helpers;

//using GB.SUTEL.Entities.Utilidades;

namespace SutelSolution
{
    /// <summary>
    /// Diego Navarrete Alvarez
    /// se incrementó el valor de  whenInitIndexNextIndicador
    /// </summary>
    ///  
    public class BLArchivoExcel
    {

        #region Atributos


        string uRLSaveFormatXLS = "C:\\Sutel\\Repositorio"; //quitar
        string to = ConfigurationManager.AppSettings["to"];
        string profileName = ConfigurationManager.AppSettings["profileName"];
        private static string rutaGuardarExcel = "F:\\";
        private static string nombreExcel = "Plantilla " + String.Format("{0:MM-dd-yyyy}", DateTime.Now);
        private static string extensionExcel = ".xlsx";
        private string nombreUsuario = string.Empty;
        private string IdOperador = string.Empty;
        int whenInitIndexNextIndicador = 15;//era 14
        int whenInitIndexNextCoordenadaLectura = 1;
        static EventWaitHandle _waitHandle = new AutoResetEvent(false);
        string columnaFinalMergeCeldaObservacion = string.Empty;
        string IdSolicitudActual = string.Empty;
        string IdOperadorActual = string.Empty;

        //Agregados para el desarrollo de regla estadística {   
        string ConnString = string.Empty;
        bool trabajandoConCantones = false;
        bool trabajandoConProvincias = false;
        bool trabajandoConGeneros = false;
        bool SegregacionGenero = false;
        string SegregacionGeneroDetalle = string.Empty;
        string SegregacionGeneroDetalleUltimoNivel = string.Empty;
        List<ValoresReglaSP> valoresReglaProvincias = new List<ValoresReglaSP>();
        List<ValoresReglaSP> valoresReglaGeneros = new List<ValoresReglaSP>();
        List<ValoresReglaSP> valoresReglaCantones = new List<ValoresReglaSP>();
        List<string> nombresCantones = new List<string>();
        List<string> nombresDistritos = new List<string>();
        List<string> nombresProvincias = new List<string>();
        List<string> nombresGeneros = new List<string>();

        // RegistroIndicadorExternoBL refRegistroIndicadorExternoBL;
        //  }



        #endregion

        #region CrearIndicadoresExcel
        /// <summary>
        /// Consultar Todos
        /// </summary>        
        /// <returns>Objeto de tipo Respuesta</returns>
        public void CrearIndicadoresExcel(string connString)
        {

            StoredProcedures sp = new StoredProcedures();

            Respuesta<List<SolicitudPendiente>> respuestaSolicitudesPendientes = new Respuesta<List<SolicitudPendiente>>();
            List<SolicitudPendiente> listaSolicitudesPendientes = new List<SolicitudPendiente>();

            ConnString = connString;

            respuestaSolicitudesPendientes = sp.ExtraerSolicitudesPendientes(connString);
            if(respuestaSolicitudesPendientes.strMensaje!=string.Empty)
            {
                throw new Exception(respuestaSolicitudesPendientes.strMensaje);
            }
            listaSolicitudesPendientes = respuestaSolicitudesPendientes.objObjeto;

            Respuesta<Byte[]> objRespuestRuta = new Respuesta<Byte[]>();
            List<Respuesta<Byte[]>> objRespuestList = new List<Respuesta<Byte[]>>();
            Respuesta<List<DetalleAgrupacionSP>> respuesta = new Respuesta<List<DetalleAgrupacionSP>>();



            if (listaSolicitudesPendientes != null)
            {
                for (int i = 0; i < listaSolicitudesPendientes.Count; i++)
                {
                    objRespuestRuta = new Respuesta<Byte[]>();
                    IdOperadorActual = listaSolicitudesPendientes[i].Id_Operador;
                    IdSolicitudActual = listaSolicitudesPendientes[i].Id_Solicitud_Indicador.ToString();


                    try
                    {
                        respuesta = sp.TodosDetalleAgrupacionDetallado(IdOperadorActual, IdSolicitudActual, connString);
                        if (respuesta.strMensaje != string.Empty)
                        {
                            throw new Exception(respuestaSolicitudesPendientes.strMensaje);
                        }
                        this.nombreUsuario = respuesta.objObjeto[0].Nombre_Operador;
                        if (respuesta.objObjeto.Count > 0 && respuesta.blnIndicadorTransaccion)
                        {
                            //Se crea el excel
                            objRespuestRuta.objObjeto = initCrearExcel(respuesta.objObjeto, nombreExcel, ".xlsm", IdSolicitudActual, this.uRLSaveFormatXLS);

                            //Se guarda la cadena de Bytes(Archivo Excel) en la base de datos
                            var idArchivoExcel = listaSolicitudesPendientes[i].Id_ArchivoExcel.ToString();
                            GuardarBytesArchivoExcel(objRespuestRuta.objObjeto, idArchivoExcel, connString);
                        }
                        else
                        {
                            if (respuesta.objObjeto.Count <= 0)
                            {
                                objRespuestRuta.blnIndicadorTransaccion = false;
                                objRespuestRuta.strMensaje = "No existen datos para generar el Excel." +
                                                             "\n Detalle del archivo" +
                                                             "\n IdSolicitudIndicador: " + IdSolicitudActual +
                                                             "\n IdOperador:" + IdOperadorActual +
                                                             "\n NombreOperador:" + this.nombreUsuario;

                                NotificarError(objRespuestRuta.strMensaje, connString);
                            }
                            else
                            {
                                objRespuestRuta.blnIndicadorTransaccion = false;
                                objRespuestRuta.strMensaje = respuesta.strMensaje;
                                NotificarError(objRespuestRuta.strMensaje, connString);

                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        objRespuestRuta.strMensaje = "El IdSolicitud: " + IdSolicitudActual + ", IpOperador: " + IdOperadorActual + "\nError: " + ex.Message;
                        objRespuestRuta.blnIndicadorTransaccion = false;
                        NotificarError(objRespuestRuta.strMensaje, connString);
                   
                    }

                    objRespuestList.Add(objRespuestRuta);
                    this.ReiniciarAtributos();
                }
            }
            else
            {
                if (!respuestaSolicitudesPendientes.blnIndicadorTransaccion)
                {
                    NotificarError(respuestaSolicitudesPendientes.strMensaje, connString);
                }
            }

        }

        #endregion

        #region initCrearExcel
        /// <summary>
        /// Crear Excel
        /// </summary>        
        /// <returns>Objeto de tipo Respuesta</returns>
        public Byte[] initCrearExcel(List<DetalleAgrupacionSP> objRespuesta, string nombreExcel, string extension, string IdSolicitud, string URLSaveFormatXLS)
        {
            MemoryStream stream = new MemoryStream();

            //xapp.AutomationSecurity = Microsoft.Office.Core.MsoAutomationSecurity.msoAutomationSecurityForceDisable;


            using (ExcelPackage package = new ExcelPackage(stream)
            {
            })
            {
                string passwordExcel = IdSolicitud;

                ExcelWorksheet worksheetInicio = package.Workbook.Worksheets.Add("Inicio");

                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Indicadores");


                this.createVBADisableCopyPaste(package.Workbook);

                ExcelWorksheet worksheetInfo = package.Workbook.Worksheets.Add("Información de Criterios");

                ExcelWorksheet worksheetICoordenadasLectura = package.Workbook.Worksheets.Add("Coordenadas_de_Lectura");
                this.setProtectionSheet(worksheetInicio, 40, passwordExcel);
                this.EncabezadoInicioEnableMacro(worksheetInicio);
                this.EncabezadoCoordenadasLectura(worksheetICoordenadasLectura);

                this.setProtectionSheet(worksheet, 25, passwordExcel);
                worksheet.Column(1).Width = 25;

                this.pintarEncabezado(worksheet, objRespuesta[0].Nombre_Operador, objRespuesta[0].Nombre_Direccion, objRespuesta[0].Nombre_Servicio);

                //var stackSize = 100000000;
                Int32 stackSize = 100000000;
                Thread t = new Thread(() => this.setCellsExcel(worksheet, objRespuesta, worksheetICoordenadasLectura), stackSize);
                t.Start();

                _waitHandle.WaitOne();

                //Información de indicadores
                this.setProtectionSheet(worksheetInfo, 40, passwordExcel);
                this.addIndicadoresCriteriosInfo(worksheetInfo, objRespuesta);

                worksheetInicio.Select();

                //               worksheet.Hidden = eWorkSheetHidden.Hidden;
                //      worksheetInfo.Hidden = eWorkSheetHidden.Hidden;
                worksheetICoordenadasLectura.Hidden = eWorkSheetHidden.Hidden;



                worksheet.Cells["$A1:$O100000"].AutoFilter = true;/// prueba de filtros a una hoja
                                                                  /// 
                if (extension == ".xls")
                {
                    return this.convertToOldFormat(package.GetAsByteArray(), URLSaveFormatXLS);
                }
                else
                {
                    return package.GetAsByteArray();
                }
            }
        }

        #endregion

        #region setProtectionSheet
        public void setProtectionSheet(ExcelWorksheet worksheet, int pDefaultColWidth, string password)
        {
            worksheet.DefaultColWidth = pDefaultColWidth;
            worksheet.Protection.IsProtected = true;
            worksheet.Cells.Style.Locked = true;
            worksheet.Protection.AllowEditObject = false;
            worksheet.Protection.AllowEditScenarios = false;
            worksheet.Protection.AllowAutoFilter = true;
            worksheet.Protection.AllowPivotTables = false;
            worksheet.Protection.AllowSort = false;
            worksheet.Protection.AllowFormatRows = false;
            worksheet.Protection.AllowFormatColumns = false;
            worksheet.Protection.AllowFormatCells = false;
            worksheet.Protection.AllowDeleteRows = false;
            worksheet.Protection.AllowDeleteColumns = false;
            worksheet.Protection.AllowInsertRows = false;
            worksheet.Protection.AllowInsertColumns = false;
            worksheet.Protection.AllowSelectLockedCells = true;


            worksheet.Protection.SetPassword(password);
        }
        #endregion

        #region setCellsExcel
        /// <summary>
        /// Diego Navarrete Alvarez
        /// </summary>
        /// <param name="nombreIndicador"></param>
        public void setCellsExcel(ExcelWorksheet worksheet, List<DetalleAgrupacionSP> objRespuesta, ExcelWorksheet worksheetICoordenadasLectura)
        {
            string[] AllLetras = this.getLetras();

            int indexLetra = 0;
            int indexObj = 0;
            int[] ends = new int[2];
            string idIndicador = "";
            Guid idConstructor = new Guid();
            string nombreIndicador = "";
            string nombresDireccion = "";
            string nombreFrecuencia = "";
            string nombreDesglose = "";
            int Cantidad_Meses_Frecuencia = 0;
            int Cantidad_Meses_Desglose = 0;
            DateTime fechaInicio = new DateTime();
            DateTime fechaFin = new DateTime();
            List<DetalleAgrupacionSP> objRespuestaFilterIndicador = new List<DetalleAgrupacionSP>();
            List<DetalleAgrupacionSP> objRespuestaFilterReglas = new List<DetalleAgrupacionSP>();

            for (int i = 0; i < objRespuesta.Count; i++)
            {
                if (i == 99)
                {

                }


                if (idConstructor != objRespuesta[i].Id_Solicitud_Constructor)
                {
                    objRespuestaFilterIndicador = objRespuesta.Where(x => x.Nombre_Indicador == objRespuesta[i].Nombre_Indicador && x.Id_Solicitud_Constructor == objRespuesta[i].Id_Solicitud_Constructor).ToList();
                    idConstructor = objRespuesta[i].Id_Solicitud_Constructor;
                    idIndicador = objRespuesta[i].ID_Indicador;
                    nombreIndicador = objRespuesta[i].Nombre_Indicador;
                    nombresDireccion = objRespuesta[i].Nombre_Direccion;
                    nombreFrecuencia = objRespuesta[i].Nombre_Frecuencia;
                    nombreDesglose = objRespuesta[i].Nombre_Desglose;
                    Cantidad_Meses_Frecuencia = objRespuesta[i].Cantidad_Meses_Frecuencia;
                    Cantidad_Meses_Desglose = objRespuesta[i].Cantidad_Meses_Desglose;
                    fechaInicio = objRespuesta[i].FechaBaseParaCrearExcel;
                    fechaFin = objRespuesta[i].Fecha_Fin;
                    i = i + objRespuestaFilterIndicador.Count - 1;
                    indexObj = whenInitIndexNextIndicador;

                    objRespuestaFilterIndicador = this.addPadresCriterios(objRespuestaFilterIndicador);
                    objRespuestaFilterIndicador = this.enableTipoNivelDetalle(objRespuestaFilterIndicador);// Detalle Provincia
                    objRespuestaFilterIndicador = this.enableTipoNivelDetalleCanton(objRespuestaFilterIndicador);//Detalle Canton
                    objRespuestaFilterIndicador = this.enableTipoNivelDetalleDistrito(objRespuestaFilterIndicador);//Detalle Distrito
                    objRespuestaFilterIndicador = this.enableTipoNivelDetalleGenero(objRespuestaFilterIndicador);//Segregacion Genero

                    var PADRE2MB = objRespuestaFilterIndicador.Where(x => x.Nombre_Padre_Detalle_Agrupacion == "2MB").ToList();

                    var PADRE1MB = objRespuestaFilterIndicador.Where(x => x.Nombre_Padre_Detalle_Agrupacion == "1MB" && x.Id_Tipo_Valor == 3).ToList();



                    objRespuestaFilterReglas.Clear();
                    objRespuestaFilterReglas.AddRange(objRespuestaFilterIndicador);
                    ends = this.FibonacciHijo(null, null, objRespuestaFilterIndicador, worksheet, indexObj, indexLetra, indexLetra);

                    this.enableCellsByDeglose(worksheet, ends, Cantidad_Meses_Desglose, Cantidad_Meses_Frecuencia, fechaInicio, fechaFin, objRespuestaFilterReglas, worksheetICoordenadasLectura);
                    this.pintarEncabezadoIndicador(worksheet, idIndicador, nombreIndicador, nombreDesglose, nombreFrecuencia, nombresDireccion);

                    whenInitIndexNextIndicador = ends[1] + 7;
                    this.pintarFooterIndicador(worksheet);
                }

                ReiniciarControlesReglaEstadistica();
            }
            _waitHandle.Set();
        }
        #endregion

        #region FibonacciHijo

        public int[] FibonacciHijo(int? idPadre, Nullable<Guid> IdConstructorCriterioPadre, List<DetalleAgrupacionSP> objRespuesta, ExcelWorksheet worksheet, int indexObj, int indexLetra, int indexLetraParaDesgloseYFrecuencia)
        {
            if (objRespuesta.Count == 0)
            {
                int[] ends = new int[2];
                ends[0] = indexLetraParaDesgloseYFrecuencia;
                ends[1] = indexObj;
                return ends;
            }

            DetalleAgrupacionSP myseft = new DetalleAgrupacionSP();
            int? lastPadre = null;
            bool inSeft = false;
            foreach (DetalleAgrupacionSP da in objRespuesta)
            {
                //QUITAR
                var nombre = da.Nombre_Detalle_Agrupacion;
                var nombrePadre = da.Nombre_Padre_Detalle_Agrupacion;

                if (idPadre == da.Id_Padre_Detalle_Agrupacion && IdConstructorCriterioPadre == da.Id_Padre_ConstructorCriterio)
                {
                    string cell = this.getLetras()[indexLetra++] + indexObj;
                    this.setValueAndStyle(worksheet, cell, da.Nombre_Detalle_Agrupacion.ToString(), true);// aqui se agraga el detalle de agrupacion
                    if (da.Id_Padre_ConstructorCriterio == null)
                    {
                        worksheet.Cells[cell].Style.Font.Bold = true;
                    }
                    //this.setCommentarioCellAyuda(worksheet, cell, da.Constructor_Criterio_Ayuda.ToString());
                    return FibonacciHijo(da.Id_Detalle_Agrupacion, da.Id_ConstructorCriterio, objRespuesta, worksheet, indexObj, indexLetra, indexLetraParaDesgloseYFrecuencia);
                }
                if (idPadre == da.Id_Detalle_Agrupacion && !inSeft)
                {
                    inSeft = true;
                    myseft = da;
                }
            }

            indexObj++;

            if (indexLetraParaDesgloseYFrecuencia < indexLetra)
            {
                indexLetraParaDesgloseYFrecuencia = indexLetra;
            }


            ///lastPadre = myseft.Id_Padre_Detalle_Agrupacion;
            //IdConstructorCriterioPadre = myseft.Id_Padre_ConstructorCriterio;
            /*objRespuesta.Remove(myseft);
            this.setCellsSobrantesLeft(worksheet, indexObj, indexLetra - 1);
            this.setCellsSobrantesRight(worksheet, indexObj, indexLetra - 2);
            return FibonacciHermano(lastPadre, IdConstructorCriterioPadre, objRespuesta, worksheet, indexObj, indexLetra, indexLetraParaDesgloseYFrecuencia);
            */
            //Modificacion de funcion 
            lastPadre = myseft.Id_Padre_Detalle_Agrupacion;
            IdConstructorCriterioPadre = myseft.Id_Padre_ConstructorCriterio;
            objRespuesta.Remove(myseft);
            this.setCellsSobrantesLeft(worksheet, indexObj, indexLetra - 1);
            this.setCellsSobrantesRight(worksheet, indexObj, indexLetra - 1);
            return FibonacciHermano(lastPadre, IdConstructorCriterioPadre, objRespuesta, worksheet, indexObj, indexLetra, indexLetraParaDesgloseYFrecuencia);
        }

        #endregion

        #region FibonacciHermano

        public int[] FibonacciHermano(int? idHermano, Nullable<Guid> IdConstructorCriterioPadre, List<DetalleAgrupacionSP> objRespuesta, ExcelWorksheet worksheet, int indexObj, int indexLetra, int indexLetraParaDesgloseYFrecuencia)
        {
            if (objRespuesta.Count == 0)
            {
                int[] ends = new int[2];
                ends[0] = indexLetraParaDesgloseYFrecuencia;
                ends[1] = indexObj;
                return ends;
            }

            DetalleAgrupacionSP myseft = new DetalleAgrupacionSP();
            int? lastPadre = null;
            indexLetra--;
            bool inSeft = false;
            foreach (DetalleAgrupacionSP da in objRespuesta)
            {
                if (idHermano == da.Id_Padre_Detalle_Agrupacion && IdConstructorCriterioPadre == da.Id_Padre_ConstructorCriterio)
                {
                    return FibonacciHijo(idHermano, IdConstructorCriterioPadre, objRespuesta, worksheet, indexObj, indexLetra, indexLetraParaDesgloseYFrecuencia);
                }
                if (idHermano == da.Id_Detalle_Agrupacion && !inSeft && IdConstructorCriterioPadre == da.Id_ConstructorCriterio)
                {
                    inSeft = true;
                    myseft = da;
                }
            }

            this.collasedCellsPadre(worksheet, indexObj, indexLetra);
            lastPadre = myseft.Id_Padre_Detalle_Agrupacion;
            IdConstructorCriterioPadre = myseft.Id_Padre_ConstructorCriterio;
            objRespuesta.Remove(myseft);
            return FibonacciHermano(lastPadre, IdConstructorCriterioPadre, objRespuesta, worksheet, indexObj, indexLetra, indexLetraParaDesgloseYFrecuencia);
        }

        #endregion

        #region collasedCellsPadre

        public void collasedCellsPadre(ExcelWorksheet worksheet, int indexObj, int indexLetra)
        {
            string cellColl = "";
            string cellAntColl = "";
            int cantCollased = 0;
            indexLetra = indexLetra - 1;

            if (indexLetra >= 0)
            {
                for (var i = indexObj - 1; i >= 1; i--)
                {
                    cellColl = this.getLetras()[indexLetra] + i;
                    if (worksheet.Cells[cellColl].Value == null)
                    {
                        cantCollased++;
                    }
                    else
                    {
                        break;
                    }
                }
                if (cantCollased < indexObj && cantCollased > 0)
                {
                    cellColl = this.getLetras()[indexLetra] + (indexObj - 1);
                    cellAntColl = this.getLetras()[indexLetra] + ((indexObj - 1) - cantCollased);
                    if (cellColl != cellAntColl)
                    {
                        for (int j = (indexObj) - cantCollased; j <= indexObj - 1; j++)
                        {
                            worksheet.Cells[this.getLetras()[indexLetra] + (j)].Value = worksheet.Cells[cellAntColl].Value;
                        }
                        worksheet.Select(cellAntColl + ":" + cellColl);
                        worksheet.SelectedRange.Merge = true;
                        this.pintarBorderNegro(worksheet, cellAntColl + ":" + cellColl);
                    }
                }
            }
        }

        #endregion

        #region setCellsSobrantesLeft

        public void setCellsSobrantesLeft(ExcelWorksheet worksheet, int indexObj, int indexLetra)
        {
            string cellColl = "";
            if (indexLetra >= 0 && indexObj > 2)
            {
                indexObj--;
                for (int i = indexObj; i >= 1; i--)
                {
                    cellColl = this.getLetras()[indexLetra] + (i - 1);
                    if (i < this.whenInitIndexNextIndicador + 1 || worksheet.Cells[cellColl].Value != null)
                    {
                        break;
                    }
                    for (int k = indexLetra; k >= 1; k--)
                    {
                        cellColl = this.getLetras()[k] + (i - 1);
                        if (worksheet.Cells[cellColl].Value == null)
                        {
                            this.setValueAndStyle(worksheet, cellColl, "X", true);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }

        #endregion

        #region setCellsSobrantesRight

        public void setCellsSobrantesRight(ExcelWorksheet worksheet, int indexObj, int indexLetra)
        {
            string cellColl = "";
            if (indexLetra >= 0 && indexObj > 2)
            {
                indexObj--;
                for (int i = indexObj; i >= 1; i--)
                {
                    cellColl = this.getLetras()[indexLetra + 1] + (i - 1);
                    if (i < this.whenInitIndexNextIndicador + 1 || worksheet.Cells[cellColl].Value == null)
                    {
                        break;
                    }

                    cellColl = this.getLetras()[indexLetra + 1] + (i - 1);
                    int k = indexLetra;
                    while (worksheet.Cells[cellColl].Value != null)
                    {
                        cellColl = this.getLetras()[k + 1] + (i);
                        if (worksheet.Cells[cellColl].Value == null)
                        {
                            this.setValueAndStyle(worksheet, cellColl, "X", true);
                        }
                        else
                        {
                            break;
                        }

                        k++;
                        cellColl = this.getLetras()[k + 1] + (i - 1);
                    }
                }
            }
        }

        #endregion

        #region addPadresCriterios

        public List<DetalleAgrupacionSP> addPadresCriterios(List<DetalleAgrupacionSP> objList)
        {
            int Id_Detalle_Agrupacion = 0;
            string Nombre_Detalle_Agrupacion = "";
            Guid Id_ConstructorCriterio = Guid.NewGuid();
            string IdIndicador = "";
            bool isInList = false;
            List<DetalleAgrupacionSP> objListPorCriterio = new List<DetalleAgrupacionSP>();
            List<DetalleAgrupacionSP> objListOrdenadosCriterio = new List<DetalleAgrupacionSP>();
            objListOrdenadosCriterio = objList.GroupBy(x => x.Nombre_Criterio).Select(g => g.First()).ToList();

            for (int i = 0; i < objListOrdenadosCriterio.Count; i++)
            {
                isInList = false;
                DetalleAgrupacionSP newObj = new DetalleAgrupacionSP();
                Random random = new Random();
                System.Threading.Thread.Sleep(20);
                Id_Detalle_Agrupacion = random.Next(1, 900000000);
                Nombre_Detalle_Agrupacion = objListOrdenadosCriterio[i].Nombre_Criterio.Trim();
                Id_ConstructorCriterio = Guid.NewGuid();
                IdIndicador = objListOrdenadosCriterio[i].ID_Indicador;
                Guid Id_Solicitud_Indicador = objListOrdenadosCriterio[i].Id_Solicitud_Indicador;
                Guid Id_Solicitud_Constructor = objListOrdenadosCriterio[i].Id_Solicitud_Constructor;
                //Set new obj
                newObj.Id_ConstructorCriterio = Id_ConstructorCriterio;
                newObj.Id_Detalle_Agrupacion = Id_Detalle_Agrupacion;
                newObj.Nombre_Detalle_Agrupacion = Nombre_Detalle_Agrupacion;
                newObj.ID_Indicador = IdIndicador;
                newObj.Id_Solicitud_Indicador = Id_Solicitud_Indicador;
                newObj.Id_Solicitud_Constructor = Id_Solicitud_Constructor;

                objListPorCriterio = objList;
                objListPorCriterio = objListPorCriterio.Where(x => x.Id_Padre_ConstructorCriterio == null && x.Nombre_Criterio == objListOrdenadosCriterio[i].Nombre_Criterio).ToList();

                foreach (DetalleAgrupacionSP obj in objListPorCriterio)
                {
                    isInList = true;
                    objList.Where(x => x.Id_Padre_ConstructorCriterio == null && x.Id_ConstructorCriterio == obj.Id_ConstructorCriterio).FirstOrDefault().Nombre_Padre_Detalle_Agrupacion = Nombre_Detalle_Agrupacion;
                    objList.Where(x => x.Id_Padre_ConstructorCriterio == null && x.Id_ConstructorCriterio == obj.Id_ConstructorCriterio).FirstOrDefault().Id_Padre_Detalle_Agrupacion = Id_Detalle_Agrupacion;
                    objList.Where(x => x.Id_Padre_ConstructorCriterio == null && x.Id_ConstructorCriterio == obj.Id_ConstructorCriterio).FirstOrDefault().Id_Padre_ConstructorCriterio = Id_ConstructorCriterio;
                }

                if (isInList)
                {
                    objList.Add(newObj);
                }
            }

            return objList;
        }

        #endregion

        #region enableTipoNivelDetalle

        public List<DetalleAgrupacionSP> enableTipoNivelDetalle(List<DetalleAgrupacionSP> objList)
        {
            int Id_Detalle_Agrupacion = 0;
            string Nombre_Detalle_Agrupacion = "";
            Nullable<Guid> Id_Padre_ConstructorCriterio = null;
            string IdIndicador = "";
            string Valor_Inferior = "";
            string Valor_Superior = "";
            string Tipo_Valor = "";
            int? Id_Tipo_Valor = null;
            int? Id_TipoNivelDetalleGenero = null;
            string Valor_Formato = "";
            int count = objList.Count;
            int keyIdTipoNivelDetalle = 0;
            Nullable<int> idProvincia = null;
            Nullable<int> idCanton = null;
            Nullable<int> idDistrito = null;
            Nullable<int> idGenero = null;
            string Tipodetalle = "";
            trabajandoConGeneros = false;
            trabajandoConCantones = false;
            trabajandoConProvincias = false;

            StoredProcedures sp = new StoredProcedures();
            Respuesta<List<ValoresReglaSP>> respuesta = new Respuesta<List<ValoresReglaSP>>();



            for (int i = 0; i < count; i++)
            {
                var padre = objList.Where(x => x.Id_Padre_ConstructorCriterio == objList[i].Id_ConstructorCriterio).FirstOrDefault();
                if (objList[i].Tipo_Nivel_Detalle != null && padre == null)
                {
                    string[] values = objList[i].Tipo_Nivel_Detalle.Split(',').Select(sValue => sValue.Trim()).ToArray();
                    string[] valuesIDTipoNivelDetalle = objList[i].Id_Tipo_Nivel_Detalle.Split(',').Select(sValue => sValue.Trim()).ToArray();

                    keyIdTipoNivelDetalle = 0;

                    foreach (string val in values)
                    {
                        DetalleAgrupacionSP newObj = new DetalleAgrupacionSP();

                        int indice = 0;

                        //Inicia bloque de código agregado para las reglas estadísticas
                        List<string> listaNombres = values.ToList();

                        if (objList[i].UsaReglaEstadisticaConNivelDetalle == (byte)1)
                        {

                            // Esto es para llamar al sp solamente en la primera iteración.
                            indice = listaNombres.IndexOf(val);

                            if (indice == 0)
                            {

                                if (values.Length == 7)
                                {
                                    respuesta = sp.pa_getValoresReglaEstadisticaConNivelDetalle(objList[i].Id_ConstructorCriterio.ToString(), 1, ConnString, IdOperadorActual, IdSolicitudActual);

                                    if (respuesta.blnIndicadorTransaccion)
                                    {

                                        valoresReglaProvincias = respuesta.objObjeto;

                                        nombresProvincias = values.ToList();
                                    }
                                    else
                                        valoresReglaProvincias = null;

                                    trabajandoConProvincias = true;
                                }

                                if (values.Length == 2)
                                {
                                    respuesta = sp.pa_getValoresReglaEstadisticaConNivelDetalle(objList[i].Id_ConstructorCriterio.ToString(), 3, ConnString, IdOperadorActual, IdSolicitudActual);

                                    if (respuesta.blnIndicadorTransaccion)
                                    {
                                        valoresReglaGeneros = respuesta.objObjeto;
                                        nombresGeneros = values.ToList();
                                    }
                                    else
                                        valoresReglaGeneros = null;

                                    trabajandoConGeneros = true;
                                }

                                if (values.Length == 81)
                                {
                                    respuesta = sp.pa_getValoresReglaEstadisticaConNivelDetalle(objList[i].Id_ConstructorCriterio.ToString(), 2, ConnString, IdOperadorActual, IdSolicitudActual);

                                    if (respuesta.blnIndicadorTransaccion)
                                    {
                                        valoresReglaCantones = respuesta.objObjeto;

                                        nombresCantones = values.ToList();
                                    }
                                    else
                                        valoresReglaCantones = null;
                                    trabajandoConCantones = true;
                                }


                            }


                        }



                        //Termina bloque de código agregado para las reglas estadísticas


                        Id_Detalle_Agrupacion = objList[i].Id_Detalle_Agrupacion;
                        Nombre_Detalle_Agrupacion = objList[i].Nombre_Detalle_Agrupacion;
                        Id_Padre_ConstructorCriterio = objList[i].Id_ConstructorCriterio;
                        Valor_Inferior = objList[i].Valor_Inferior;
                        Valor_Superior = objList[i].Valor_Superior;
                        Id_Tipo_Valor = objList[i].Id_Tipo_Valor;
                        Valor_Formato = objList[i].Valor_Formato;
                        Tipo_Valor = objList[i].Tipo_Valor;
                        IdIndicador = objList[i].ID_Indicador;
                        Guid Id_Solicitud_Indicador = objList[i].Id_Solicitud_Indicador;
                        Guid Id_Solicitud_Constructor = objList[i].Id_Solicitud_Constructor;
                        Tipodetalle = objList[i].Tabla_Tipo_Nivel_Detalle.Trim().ToUpper();
                        Id_TipoNivelDetalleGenero = objList[i].Id_Tipo_Nivel_Detalle_Genero;
                        if (objList[i].Tabla_Tipo_Nivel_Detalle.Trim().ToUpper() == "PROVINCIA")// Aqui es donde podemos meter el codigo
                        {

                            idProvincia = objList[i].Id_Provincia;
                        }
                        idGenero = null;
                        idCanton = null;
                        idProvincia = null;
                        switch (objList[i].Tabla_Tipo_Nivel_Detalle.Trim().ToUpper())
                        {
                            case "PROVINCIA":

                                idProvincia = int.Parse(valuesIDTipoNivelDetalle[keyIdTipoNivelDetalle]);
                                if (Id_TipoNivelDetalleGenero > 0)
                                {
                                    SegregacionGenero = true;
                                    SegregacionGeneroDetalle = "PROVINCIA";
                                    SegregacionGeneroDetalleUltimoNivel = "PROVINCIA";
                                }
                                break;
                            case "CANTON":
                                idProvincia = int.Parse(valuesIDTipoNivelDetalle[keyIdTipoNivelDetalle]);
                                if (Id_TipoNivelDetalleGenero > 0)
                                {
                                    SegregacionGenero = true;
                                    SegregacionGeneroDetalle = "CANTON";
                                    SegregacionGeneroDetalleUltimoNivel = "CANTON";
                                }
                                //string[] cantones = Returncanton(valuesIDTipoNivelDetalle);
                                //idCanton = int.Parse(cantones[keyIdTipoNivelDetalle]);
                                break;
                            case "DISTRITO":
                                idProvincia = int.Parse(valuesIDTipoNivelDetalle[keyIdTipoNivelDetalle]);
                                if (Id_TipoNivelDetalleGenero > 0)
                                {
                                    SegregacionGenero = true;
                                    SegregacionGeneroDetalle = "DISTRITO";
                                    SegregacionGeneroDetalleUltimoNivel = "DISTRITO";

                                }
                                //string[] Distritos = Returndistrito(valuesIDTipoNivelDetalle);
                                //idDistrito = int.Parse(Distritos[keyIdTipoNivelDetalle]);
                                break;
                            case "GENERO":
                                SegregacionGenero = true;
                                idGenero = int.Parse(valuesIDTipoNivelDetalle[keyIdTipoNivelDetalle]);
                                SegregacionGeneroDetalle = "GENERO";
                                SegregacionGeneroDetalleUltimoNivel = "GENERO";
                                break;
                        }

                        newObj.Id_Padre_Detalle_Agrupacion = Id_Detalle_Agrupacion;
                        newObj.Nombre_Padre_Detalle_Agrupacion = Nombre_Detalle_Agrupacion;
                        newObj.Id_Padre_ConstructorCriterio = Id_Padre_ConstructorCriterio;


                        var indiceEnListaReglaValores = listaNombres.IndexOf(val);

                        if (trabajandoConProvincias && valoresReglaProvincias != null)
                        {
                            newObj.Valor_Superior = valoresReglaProvincias[indiceEnListaReglaValores].ValorLimiteSuperior;
                            newObj.Valor_Inferior = valoresReglaProvincias[indiceEnListaReglaValores].ValorLimiteInferior;
                        }

                        if (trabajandoConGeneros && valoresReglaGeneros != null)
                        {
                            newObj.Valor_Superior = valoresReglaGeneros[indiceEnListaReglaValores].ValorLimiteSuperior;
                            newObj.Valor_Inferior = valoresReglaGeneros[indiceEnListaReglaValores].ValorLimiteInferior;
                        }

                        if (trabajandoConCantones && valoresReglaCantones != null)
                        {
                            newObj.Valor_Superior = valoresReglaCantones[indiceEnListaReglaValores].ValorLimiteSuperior;
                            newObj.Valor_Inferior = valoresReglaCantones[indiceEnListaReglaValores].ValorLimiteInferior;
                        }

                        if (!trabajandoConGeneros && !trabajandoConCantones && !trabajandoConProvincias)
                        {
                            newObj.Valor_Inferior = Valor_Inferior;
                            newObj.Valor_Superior = Valor_Superior;
                        }


                        newObj.Id_Tipo_Valor = Id_Tipo_Valor;
                        newObj.Valor_Formato = Valor_Formato;
                        newObj.Tipo_Valor = Tipo_Valor;
                        newObj.ID_Indicador = IdIndicador;
                        newObj.Id_Solicitud_Indicador = Id_Solicitud_Indicador;
                        newObj.Id_Solicitud_Constructor = Id_Solicitud_Constructor;
                        newObj.Id_Provincia = idProvincia;
                        newObj.Id_Canton = idCanton;
                        newObj.Id_Distrito = idDistrito;
                        newObj.Id_Genero = idGenero;
                        Random random = new Random();
                        newObj.Id_Detalle_Agrupacion = 0;
                        System.Threading.Thread.Sleep(20);
                        newObj.Id_Detalle_Agrupacion = random.Next(1, 900000000);
                        newObj.Nombre_Detalle_Agrupacion = val;
                        newObj.Tabla_Tipo_Nivel_Detalle = Tipodetalle;
                        //newObj.Id_ConstructorCriterio = objList[i].Id_Padre_ConstructorCriterio;
                        keyIdTipoNivelDetalle++;
                        objList.Add(newObj);


                    }
                }

                trabajandoConGeneros = false;
                trabajandoConCantones = false;
                trabajandoConProvincias = false;

            }//fin del for

            return objList;
        }
        /// <summary>
        /// Este metodo llena una lista de string con los codigos respectivos de cada 
        /// cantón 
        /// </summary>
        /// <param name="cantonES"></param>
        /// <returns>value2</returns>
        public List<DetalleAgrupacionSP> enableTipoNivelDetalleCanton(List<DetalleAgrupacionSP> objList)
        {
            int Id_Detalle_Agrupacion = 0;
            string Nombre_Detalle_Agrupacion = "";
            Nullable<Guid> Id_Padre_ConstructorCriterio = null;
            string IdIndicador = "";
            string Valor_Inferior = "";
            string Valor_Superior = "";
            string Tipo_Valor = "";
            int? Id_Tipo_Valor = null;
            string Valor_Formato = "";
            int count = objList.Count;
            int keyIdTipoNivelDetalle = 0;
            string Tipodetalle = "";
            Nullable<int> idProvincia = null;
            Nullable<int> idCanton = null;
            Nullable<int> idDistrito = null;
            Nullable<int> idGenero = null;
            //List<Canton> listaCantones = new List<Canton>();
            trabajandoConGeneros = false;
            trabajandoConCantones = false;
            trabajandoConProvincias = false;

            StoredProcedures sp = new StoredProcedures();
            Respuesta<List<ValoresReglaSP>> respuesta = new Respuesta<List<ValoresReglaSP>>();



            for (int i = 0; i < count; i++)
            {
                var padre = objList.Where(x => x.Id_Padre_ConstructorCriterio == objList[i].Id_ConstructorCriterio).FirstOrDefault();

                if (objList[i].Id_Provincia != null && objList[i].Tabla_Tipo_Nivel_Detalle.Trim().ToUpper() != "PROVINCIA") //&& objList[i].Tabla_Tipo_Nivel_Detalle.Trim().ToUpper() == "CANTON")
                {
                    keyIdTipoNivelDetalle = 0;
                    //   List<string> listaNombres = values.ToList();

                    Id_Detalle_Agrupacion = objList[i].Id_Detalle_Agrupacion;
                    Nombre_Detalle_Agrupacion = objList[i].Nombre_Detalle_Agrupacion;
                    Id_Padre_ConstructorCriterio = objList[i].Id_ConstructorCriterio;
                    Valor_Inferior = objList[i].Valor_Inferior;
                    Valor_Superior = objList[i].Valor_Superior;
                    Id_Tipo_Valor = objList[i].Id_Tipo_Valor;
                    Valor_Formato = objList[i].Valor_Formato;
                    Tipo_Valor = objList[i].Tipo_Valor;
                    IdIndicador = objList[i].ID_Indicador;
                    Guid Id_Solicitud_Indicador = objList[i].Id_Solicitud_Indicador;
                    Guid Id_Solicitud_Constructor = objList[i].Id_Solicitud_Constructor;
                    Tipodetalle = objList[i].Tabla_Tipo_Nivel_Detalle.Trim().ToUpper();
                    ////// Cambio para validar si se tienen que agregar los detalles de agrupcion de camtones o Distritos

                    StoredProcedures sp1 = new StoredProcedures();
                    Respuesta<List<Canton>> respuestaCantones = new Respuesta<List<Canton>>();
                    List<Canton> ListaCantones = new List<Canton>();
                    respuestaCantones = sp1.ExtraerCantonesXProvincia(ConnString, objList[i].Id_Provincia);
                    ListaCantones = respuestaCantones.objObjeto;

                    // for (int X = 0; X < ListaCantones.Count; X++)
                    foreach (Canton itemCanton in ListaCantones)
                    {
                        DetalleAgrupacionSP newObjCanton = new DetalleAgrupacionSP();
                        newObjCanton.Id_Padre_Detalle_Agrupacion = Id_Detalle_Agrupacion;
                        newObjCanton.Nombre_Padre_Detalle_Agrupacion = objList[i].Nombre_Detalle_Agrupacion; //Nombre_Detalle_Agrupacion;
                        newObjCanton.Id_Padre_ConstructorCriterio = Id_Padre_ConstructorCriterio;
                        newObjCanton.Valor_Inferior = Valor_Inferior;
                        newObjCanton.Valor_Superior = Valor_Superior;


                        newObjCanton.Id_Tipo_Valor = Id_Tipo_Valor;
                        newObjCanton.Valor_Formato = Valor_Formato;
                        newObjCanton.Tipo_Valor = Tipo_Valor;
                        newObjCanton.ID_Indicador = IdIndicador;
                        newObjCanton.Id_Solicitud_Indicador = Id_Solicitud_Indicador;
                        newObjCanton.Id_Solicitud_Constructor = Id_Solicitud_Constructor;
                        newObjCanton.Id_Provincia = objList[i].Id_Provincia;
                        newObjCanton.Id_Canton = itemCanton.IdCanton;
                        newObjCanton.Id_Distrito = idDistrito;
                        newObjCanton.Id_Genero = idGenero;
                        Random random1 = new Random();
                        newObjCanton.Id_Detalle_Agrupacion = 0;
                        System.Threading.Thread.Sleep(20);
                        newObjCanton.Id_Detalle_Agrupacion = random1.Next(1, 900000000);
                        newObjCanton.Nombre_Detalle_Agrupacion = itemCanton.Nombre;
                        newObjCanton.Tabla_Tipo_Nivel_Detalle = Tipodetalle;
                        newObjCanton.Id_ConstructorCriterio = objList[i].Id_Padre_ConstructorCriterio;
                        //keyIdTipoNivelDetalle++;
                        objList.Add(newObjCanton);/// cambio de canton

                    }// fin del for



                }//fIN DEL IF 

                trabajandoConGeneros = false;
                trabajandoConCantones = false;
                trabajandoConProvincias = false;

            }//fin del for inicial

            return objList;
        }
        /// <summary>
        /// Cambio de distrito
        /// </summary>
        /// <param name="Distritos"></param>
        /// <returns></returns>
        /// 
        public List<DetalleAgrupacionSP> enableTipoNivelDetalleDistrito(List<DetalleAgrupacionSP> objList)
        {
            int Id_Detalle_Agrupacion = 0;
            string Nombre_Detalle_Agrupacion = "";
            Nullable<Guid> Id_Padre_ConstructorCriterio = null;
            string IdIndicador = "";
            string Valor_Inferior = "";
            string Valor_Superior = "";
            string Tipo_Valor = "";
            int? Id_Tipo_Valor = null;
            string Valor_Formato = "";
            int count = objList.Count;
            string Tipodetalle = "";
            int keyIdTipoNivelDetalle = 0;
            Nullable<int> idProvincia = null;
            Nullable<int> idCanton = null;
            Nullable<int> idDistrito = null;
            Nullable<int> idGenero = null;
            //List<Canton> listaCantones = new List<Canton>();
            trabajandoConGeneros = false;
            trabajandoConCantones = false;
            trabajandoConProvincias = false;

            StoredProcedures sp2 = new StoredProcedures();
            Respuesta<List<ValoresReglaSP>> respuesta = new Respuesta<List<ValoresReglaSP>>();



            for (int i = 0; i < count; i++)
            {
                var padre = objList.Where(x => x.Id_Padre_ConstructorCriterio == objList[i].Id_ConstructorCriterio).FirstOrDefault();
                if (objList[i].Id_Canton != null && objList[i].Tabla_Tipo_Nivel_Detalle.Trim().ToUpper() == "DISTRITO") //&& objList[i].Tabla_Tipo_Nivel_Detalle.Trim().ToUpper() == "CANTON")
                {
                    keyIdTipoNivelDetalle = 0;
                    //   List<string> listaNombres = values.ToList();

                    Id_Detalle_Agrupacion = objList[i].Id_Detalle_Agrupacion;
                    Nombre_Detalle_Agrupacion = objList[i].Nombre_Detalle_Agrupacion;
                    Id_Padre_ConstructorCriterio = objList[i].Id_ConstructorCriterio;
                    Valor_Inferior = objList[i].Valor_Inferior;
                    Valor_Superior = objList[i].Valor_Superior;
                    Id_Tipo_Valor = objList[i].Id_Tipo_Valor;
                    Valor_Formato = objList[i].Valor_Formato;
                    Tipo_Valor = objList[i].Tipo_Valor;
                    IdIndicador = objList[i].ID_Indicador;
                    Guid Id_Solicitud_Indicador = objList[i].Id_Solicitud_Indicador;
                    Guid Id_Solicitud_Constructor = objList[i].Id_Solicitud_Constructor;
                    Tipodetalle = objList[i].Tabla_Tipo_Nivel_Detalle.Trim().ToUpper();
                    ////// Cambio para validar si se tienen que agregar los detalles de agrupcion de camtones o Distritos

                    StoredProcedures sp1 = new StoredProcedures();
                    Respuesta<List<Distrito>> respuestaCantones = new Respuesta<List<Distrito>>();
                    List<Distrito> ListaCantones = new List<Distrito>();
                    respuestaCantones = sp1.ExtraerDistritoXCanton(ConnString, objList[i].Id_Canton);
                    ListaCantones = respuestaCantones.objObjeto;

                    // for (int X = 0; X < ListaCantones.Count; X++)
                    foreach (Distrito itemCanton in ListaCantones)
                    {
                        DetalleAgrupacionSP newObjCanton = new DetalleAgrupacionSP();
                        newObjCanton.Id_Padre_Detalle_Agrupacion = Id_Detalle_Agrupacion;
                        newObjCanton.Nombre_Padre_Detalle_Agrupacion = objList[i].Nombre_Detalle_Agrupacion; //Nombre_Detalle_Agrupacion;
                        newObjCanton.Id_Padre_ConstructorCriterio = Id_Padre_ConstructorCriterio;
                        newObjCanton.Valor_Inferior = Valor_Inferior;
                        newObjCanton.Valor_Superior = Valor_Superior;


                        newObjCanton.Id_Tipo_Valor = Id_Tipo_Valor;
                        newObjCanton.Valor_Formato = Valor_Formato;
                        newObjCanton.Tipo_Valor = Tipo_Valor;
                        newObjCanton.ID_Indicador = IdIndicador;
                        newObjCanton.Id_Solicitud_Indicador = Id_Solicitud_Indicador;
                        newObjCanton.Id_Solicitud_Constructor = Id_Solicitud_Constructor;
                        newObjCanton.Id_Provincia = objList[i].Id_Provincia;
                        newObjCanton.Id_Canton = objList[i].Id_Canton;
                        newObjCanton.Id_Distrito = itemCanton.IdDistrito;
                        newObjCanton.Id_Genero = idGenero;
                        Random random1 = new Random();
                        newObjCanton.Id_Detalle_Agrupacion = 0;
                        System.Threading.Thread.Sleep(20);
                        newObjCanton.Id_Detalle_Agrupacion = random1.Next(1, 900000000);
                        newObjCanton.Nombre_Detalle_Agrupacion = itemCanton.Nombre;
                        //keyIdTipoNivelDetalle++;
                        newObjCanton.Tabla_Tipo_Nivel_Detalle = Tipodetalle;
                        newObjCanton.Id_ConstructorCriterio = objList[i].Id_Padre_ConstructorCriterio;
                        objList.Add(newObjCanton);/// cambio de canton

                    }// fin del for



                }//fIN DEL IF 

                trabajandoConGeneros = false;
                trabajandoConCantones = false;
                trabajandoConProvincias = false;

            }//fin del for inicial

            return objList;
        }
        /// <summary>
        /// Segregacion de Genero
        /// </summary>
        /// <param name="Segregacion de Generos"></param>
        /// <returns></returns>
        /// 
        public List<DetalleAgrupacionSP> enableTipoNivelDetalleGenero(List<DetalleAgrupacionSP> objList)
        {
            int Id_Detalle_Agrupacion = 0;
            string Nombre_Detalle_Agrupacion = "";
            Nullable<Guid> Id_Padre_ConstructorCriterio = null;
            string IdIndicador = "";
            string Valor_Inferior = "";
            string Valor_Superior = "";
            string Tipo_Valor = "";
            int? Id_Tipo_Valor = null;
            string Valor_Formato = "";
            int count = objList.Count;
            int keyIdTipoNivelDetalle = 0;
            Nullable<int> idProvincia = null;
            Nullable<int> idCanton = null;
            Nullable<int> idDistrito = null;
            Nullable<int> idGenero = null;
            //List<Canton> listaCantones = new List<Canton>();
            trabajandoConGeneros = false;
            trabajandoConCantones = false;
            trabajandoConProvincias = false;

            StoredProcedures sp2 = new StoredProcedures();
            Respuesta<List<ValoresReglaSP>> respuesta = new Respuesta<List<ValoresReglaSP>>();



            for (int i = 0; i < count; i++)
            {
                var padre = objList.Where(x => x.Id_Padre_ConstructorCriterio == objList[i].Id_ConstructorCriterio).FirstOrDefault();
                // Inicia if Provincia
                if (objList[i].Id_Provincia != null && objList[i].Tabla_Tipo_Nivel_Detalle.Trim().ToUpper() == SegregacionGeneroDetalle && SegregacionGenero && SegregacionGeneroDetalleUltimoNivel == "PROVINCIA") //&& objList[i].Tabla_Tipo_Nivel_Detalle.Trim().ToUpper() == "CANTON")
                {
                    keyIdTipoNivelDetalle = 0;
                    //   List<string> listaNombres = values.ToList();

                    Id_Detalle_Agrupacion = objList[i].Id_Detalle_Agrupacion;
                    Nombre_Detalle_Agrupacion = objList[i].Nombre_Detalle_Agrupacion;
                    Id_Padre_ConstructorCriterio = objList[i].Id_ConstructorCriterio;
                    Valor_Inferior = objList[i].Valor_Inferior;
                    Valor_Superior = objList[i].Valor_Superior;
                    Id_Tipo_Valor = objList[i].Id_Tipo_Valor;
                    Valor_Formato = objList[i].Valor_Formato;
                    Tipo_Valor = objList[i].Tipo_Valor;
                    IdIndicador = objList[i].ID_Indicador;
                    Guid Id_Solicitud_Indicador = objList[i].Id_Solicitud_Indicador;
                    Guid Id_Solicitud_Constructor = objList[i].Id_Solicitud_Constructor;

                    StoredProcedures sp1 = new StoredProcedures();
                    Respuesta<List<Genero>> respuestaCantones = new Respuesta<List<Genero>>();
                    List<Genero> ListaCantones = new List<Genero>();
                    respuestaCantones = sp1.ExtraerGenero(ConnString);
                    ListaCantones = respuestaCantones.objObjeto;

                    // for (int X = 0; X < ListaCantones.Count; X++)
                    foreach (Genero itemCanton in ListaCantones)
                    {
                        DetalleAgrupacionSP newObjCanton = new DetalleAgrupacionSP();
                        newObjCanton.Id_Padre_Detalle_Agrupacion = Id_Detalle_Agrupacion;
                        newObjCanton.Nombre_Padre_Detalle_Agrupacion = objList[i].Nombre_Detalle_Agrupacion; //Nombre_Detalle_Agrupacion;
                        newObjCanton.Id_Padre_ConstructorCriterio = Id_Padre_ConstructorCriterio;
                        newObjCanton.Valor_Inferior = Valor_Inferior;
                        newObjCanton.Valor_Superior = Valor_Superior;


                        newObjCanton.Id_Tipo_Valor = Id_Tipo_Valor;
                        newObjCanton.Valor_Formato = Valor_Formato;
                        newObjCanton.Tipo_Valor = Tipo_Valor;
                        newObjCanton.ID_Indicador = IdIndicador;
                        newObjCanton.Id_Solicitud_Indicador = Id_Solicitud_Indicador;
                        newObjCanton.Id_Solicitud_Constructor = Id_Solicitud_Constructor;
                        newObjCanton.Id_Provincia = objList[i].Id_Provincia;
                        newObjCanton.Id_Canton = objList[i].Id_Canton;
                        newObjCanton.Id_Distrito = objList[i].Id_Distrito;
                        newObjCanton.Id_Genero = itemCanton.IdGenero;
                        Random random1 = new Random();
                        newObjCanton.Id_Detalle_Agrupacion = 0;
                        System.Threading.Thread.Sleep(20);
                        newObjCanton.Id_Detalle_Agrupacion = random1.Next(1, 900000000);
                        newObjCanton.Nombre_Detalle_Agrupacion = itemCanton.Nombre;
                        newObjCanton.Id_ConstructorCriterio = objList[i].Id_Padre_ConstructorCriterio;
                        //keyIdTipoNivelDetalle++;

                        objList.Add(newObjCanton);/// cambio de canton

                    }// fin del for DE PROVINCIA

                }//fIN DEL IF PROVINCIA
                // Inicia if CANTON

                if (objList[i].Id_Canton != null && objList[i].Tabla_Tipo_Nivel_Detalle.Trim().ToUpper() == SegregacionGeneroDetalle && SegregacionGenero && SegregacionGeneroDetalleUltimoNivel == "CANTON") //&& objList[i].Tabla_Tipo_Nivel_Detalle.Trim().ToUpper() == "CANTON")
                {
                    keyIdTipoNivelDetalle = 0;
                    //   List<string> listaNombres = values.ToList();

                    Id_Detalle_Agrupacion = objList[i].Id_Detalle_Agrupacion;
                    Nombre_Detalle_Agrupacion = objList[i].Nombre_Detalle_Agrupacion;
                    Id_Padre_ConstructorCriterio = objList[i].Id_ConstructorCriterio;
                    Valor_Inferior = objList[i].Valor_Inferior;
                    Valor_Superior = objList[i].Valor_Superior;
                    Id_Tipo_Valor = objList[i].Id_Tipo_Valor;
                    Valor_Formato = objList[i].Valor_Formato;
                    Tipo_Valor = objList[i].Tipo_Valor;
                    IdIndicador = objList[i].ID_Indicador;
                    Guid Id_Solicitud_Indicador = objList[i].Id_Solicitud_Indicador;
                    Guid Id_Solicitud_Constructor = objList[i].Id_Solicitud_Constructor;

                    StoredProcedures sp1 = new StoredProcedures();
                    Respuesta<List<Genero>> respuestaCantones = new Respuesta<List<Genero>>();
                    List<Genero> ListaCantones = new List<Genero>();
                    respuestaCantones = sp1.ExtraerGenero(ConnString);
                    ListaCantones = respuestaCantones.objObjeto;

                    // for (int X = 0; X < ListaCantones.Count; X++)
                    foreach (Genero itemCanton in ListaCantones)
                    {
                        DetalleAgrupacionSP newObjCanton = new DetalleAgrupacionSP();
                        newObjCanton.Id_Padre_Detalle_Agrupacion = Id_Detalle_Agrupacion;
                        newObjCanton.Nombre_Padre_Detalle_Agrupacion = objList[i].Nombre_Detalle_Agrupacion; //Nombre_Detalle_Agrupacion;
                        newObjCanton.Id_Padre_ConstructorCriterio = Id_Padre_ConstructorCriterio;
                        newObjCanton.Valor_Inferior = Valor_Inferior;
                        newObjCanton.Valor_Superior = Valor_Superior;


                        newObjCanton.Id_Tipo_Valor = Id_Tipo_Valor;
                        newObjCanton.Valor_Formato = Valor_Formato;
                        newObjCanton.Tipo_Valor = Tipo_Valor;
                        newObjCanton.ID_Indicador = IdIndicador;
                        newObjCanton.Id_Solicitud_Indicador = Id_Solicitud_Indicador;
                        newObjCanton.Id_Solicitud_Constructor = Id_Solicitud_Constructor;
                        newObjCanton.Id_Provincia = objList[i].Id_Provincia;
                        newObjCanton.Id_Canton = objList[i].Id_Canton;
                        newObjCanton.Id_Distrito = objList[i].Id_Distrito;
                        newObjCanton.Id_Genero = itemCanton.IdGenero;
                        Random random1 = new Random();
                        newObjCanton.Id_Detalle_Agrupacion = 0;
                        System.Threading.Thread.Sleep(20);
                        newObjCanton.Id_Detalle_Agrupacion = random1.Next(1, 900000000);
                        newObjCanton.Nombre_Detalle_Agrupacion = itemCanton.Nombre;
                        //keyIdTipoNivelDetalle++;
                        newObjCanton.Id_ConstructorCriterio = objList[i].Id_Padre_ConstructorCriterio;
                        objList.Add(newObjCanton);/// cambio de canton

                    }// fin del for DE canton

                }//fIN DEL IF Canton

                // Inicia if Distrito

                if (objList[i].Id_Distrito != null && objList[i].Tabla_Tipo_Nivel_Detalle.Trim().ToUpper() == SegregacionGeneroDetalle && SegregacionGenero && SegregacionGeneroDetalleUltimoNivel == "DISTRITO")
                {
                    keyIdTipoNivelDetalle = 0;
                    //   List<string> listaNombres = values.ToList();

                    Id_Detalle_Agrupacion = objList[i].Id_Detalle_Agrupacion;
                    Nombre_Detalle_Agrupacion = objList[i].Nombre_Detalle_Agrupacion;
                    Id_Padre_ConstructorCriterio = objList[i].Id_ConstructorCriterio;
                    Valor_Inferior = objList[i].Valor_Inferior;
                    Valor_Superior = objList[i].Valor_Superior;
                    Id_Tipo_Valor = objList[i].Id_Tipo_Valor;
                    Valor_Formato = objList[i].Valor_Formato;
                    Tipo_Valor = objList[i].Tipo_Valor;
                    IdIndicador = objList[i].ID_Indicador;
                    Guid Id_Solicitud_Indicador = objList[i].Id_Solicitud_Indicador;
                    Guid Id_Solicitud_Constructor = objList[i].Id_Solicitud_Constructor;

                    StoredProcedures sp1 = new StoredProcedures();
                    Respuesta<List<Genero>> respuestaCantones = new Respuesta<List<Genero>>();
                    List<Genero> ListaCantones = new List<Genero>();
                    respuestaCantones = sp1.ExtraerGenero(ConnString);
                    ListaCantones = respuestaCantones.objObjeto;

                    // for (int X = 0; X < ListaCantones.Count; X++)
                    foreach (Genero itemCanton in ListaCantones)
                    {
                        DetalleAgrupacionSP newObjCanton = new DetalleAgrupacionSP();
                        newObjCanton.Id_Padre_Detalle_Agrupacion = Id_Detalle_Agrupacion;
                        newObjCanton.Nombre_Padre_Detalle_Agrupacion = objList[i].Nombre_Detalle_Agrupacion; //Nombre_Detalle_Agrupacion;
                        newObjCanton.Id_Padre_ConstructorCriterio = Id_Padre_ConstructorCriterio;
                        newObjCanton.Valor_Inferior = Valor_Inferior;
                        newObjCanton.Valor_Superior = Valor_Superior;


                        newObjCanton.Id_Tipo_Valor = Id_Tipo_Valor;
                        newObjCanton.Valor_Formato = Valor_Formato;
                        newObjCanton.Tipo_Valor = Tipo_Valor;
                        newObjCanton.ID_Indicador = IdIndicador;
                        newObjCanton.Id_Solicitud_Indicador = Id_Solicitud_Indicador;
                        newObjCanton.Id_Solicitud_Constructor = Id_Solicitud_Constructor;
                        newObjCanton.Id_Provincia = objList[i].Id_Provincia;
                        newObjCanton.Id_Canton = objList[i].Id_Canton;
                        newObjCanton.Id_Distrito = objList[i].Id_Distrito;
                        newObjCanton.Id_Genero = itemCanton.IdGenero;
                        Random random1 = new Random();
                        newObjCanton.Id_Detalle_Agrupacion = 0;
                        System.Threading.Thread.Sleep(20);
                        newObjCanton.Id_Detalle_Agrupacion = random1.Next(1, 900000000);
                        newObjCanton.Nombre_Detalle_Agrupacion = itemCanton.Nombre;
                        //keyIdTipoNivelDetalle++;
                        newObjCanton.Id_ConstructorCriterio = objList[i].Id_Padre_ConstructorCriterio;
                        objList.Add(newObjCanton);/// cambio de canton

                    }// fin del for DE Distrito

                }//fIN DEL IF Distrito


                /// cambio de Genero solo Genero
                /// 
                if (objList[i].Id_Tipo_Nivel_Detalle_Genero == 1 && objList[i].Id_Distrito == null && objList[i].Id_Canton == null && objList[i].Id_Provincia == null)
                {
                    keyIdTipoNivelDetalle = 0;
                    //   List<string> listaNombres = values.ToList();

                    Id_Detalle_Agrupacion = objList[i].Id_Detalle_Agrupacion;
                    Nombre_Detalle_Agrupacion = objList[i].Nombre_Detalle_Agrupacion;
                    Id_Padre_ConstructorCriterio = objList[i].Id_ConstructorCriterio;
                    Valor_Inferior = objList[i].Valor_Inferior;
                    Valor_Superior = objList[i].Valor_Superior;
                    Id_Tipo_Valor = objList[i].Id_Tipo_Valor;
                    Valor_Formato = objList[i].Valor_Formato;
                    Tipo_Valor = objList[i].Tipo_Valor;
                    IdIndicador = objList[i].ID_Indicador;
                    Guid Id_Solicitud_Indicador = objList[i].Id_Solicitud_Indicador;
                    Guid Id_Solicitud_Constructor = objList[i].Id_Solicitud_Constructor;

                    StoredProcedures sp1 = new StoredProcedures();
                    Respuesta<List<Genero>> respuestaCantones = new Respuesta<List<Genero>>();
                    List<Genero> ListaCantones = new List<Genero>();
                    respuestaCantones = sp1.ExtraerGenero(ConnString);
                    ListaCantones = respuestaCantones.objObjeto;

                    // for (int X = 0; X < ListaCantones.Count; X++)
                    foreach (Genero itemCanton in ListaCantones)
                    {
                        DetalleAgrupacionSP newObjCanton = new DetalleAgrupacionSP();
                        newObjCanton.Id_Padre_Detalle_Agrupacion = Id_Detalle_Agrupacion;
                        newObjCanton.Nombre_Padre_Detalle_Agrupacion = objList[i].Nombre_Detalle_Agrupacion; //Nombre_Detalle_Agrupacion;
                        newObjCanton.Id_Padre_ConstructorCriterio = Id_Padre_ConstructorCriterio;
                        newObjCanton.Valor_Inferior = Valor_Inferior;
                        newObjCanton.Valor_Superior = Valor_Superior;


                        newObjCanton.Id_Tipo_Valor = Id_Tipo_Valor;
                        newObjCanton.Valor_Formato = Valor_Formato;
                        newObjCanton.Tipo_Valor = Tipo_Valor;
                        newObjCanton.ID_Indicador = IdIndicador;
                        newObjCanton.Id_Solicitud_Indicador = Id_Solicitud_Indicador;
                        newObjCanton.Id_Solicitud_Constructor = Id_Solicitud_Constructor;
                        newObjCanton.Id_Provincia = objList[i].Id_Provincia;
                        newObjCanton.Id_Canton = objList[i].Id_Canton;
                        newObjCanton.Id_Distrito = objList[i].Id_Distrito;
                        newObjCanton.Id_Genero = itemCanton.IdGenero;
                        Random random1 = new Random();
                        newObjCanton.Id_Detalle_Agrupacion = 0;
                        System.Threading.Thread.Sleep(20);
                        newObjCanton.Id_Detalle_Agrupacion = random1.Next(1, 900000000);
                        newObjCanton.Nombre_Detalle_Agrupacion = itemCanton.Nombre;
                        //keyIdTipoNivelDetalle++;
                        newObjCanton.Id_ConstructorCriterio = objList[i].Id_Padre_ConstructorCriterio;
                        objList.Add(newObjCanton);/// cambio de canton

                    }// fin del for DE Distrito

                }
                //// Fin de la Prueba de Genero
            }//fin del for inicial

            return objList;
        }
        public string[] Returncanton(string[] cantonES)
        {
            string[] value2 = new string[90];/// Cambio para manejar distritos ojo revisar logica completa

            for (int i = 0; i < cantonES.Length; ++i)
            {
                int numeroCanton = i + 1;
                value2[i] = numeroCanton.ToString();
            }

            return value2;

        }
        public string[] Returndistrito(string[] cantonES)
        {
            string[] value2 = new string[468];

            for (int i = 0; i < cantonES.Length; ++i)
            {
                int numeroCanton = i + 1;
                value2[i] = numeroCanton.ToString();
            }

            return value2;

        }

        #endregion

        #region setValueAndStyle

        public void setValueAndStyle(ExcelWorksheet worksheet, string cell, string value, bool isLocked)
        {
            value = value.Trim();

            if (cell == "E156")
            {

            }
            worksheet.Cells[cell].Value = value;
            worksheet.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Justify;
            worksheet.Cells[cell].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            worksheet.Cells[cell].Style.WrapText = true;

            this.pintarBorderNegro(worksheet, cell);
            this.pintarColorCells(worksheet, cell);

            if (!isLocked)
            {
                worksheet.Cells[cell].Style.Locked = false;
            }

            if (value == "x" || value == "X")
            {
                Color colorTemp = System.Drawing.ColorTranslator.FromHtml("#9C0006");
                worksheet.Cells[cell].Style.Font.Color.SetColor(colorTemp);

                colorTemp = System.Drawing.ColorTranslator.FromHtml("#FFC7CE");
                worksheet.Cells[cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[cell].Style.Fill.BackgroundColor.SetColor(colorTemp);
            }

        }

        #endregion

        #region pintarBorderNegro

        public void pintarBorderNegro(ExcelWorksheet worksheet, string cell)
        {
            worksheet.Cells[cell].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[cell].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[cell].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[cell].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            Color colorBorder = System.Drawing.ColorTranslator.FromHtml("#000000");
            worksheet.Cells[cell].Style.Border.Bottom.Color.SetColor(colorBorder);
            worksheet.Cells[cell].Style.Border.Top.Color.SetColor(colorBorder);
            worksheet.Cells[cell].Style.Border.Left.Color.SetColor(colorBorder);
            worksheet.Cells[cell].Style.Border.Right.Color.SetColor(colorBorder);
        }

        #endregion

        #region pintarColorCells

        public void pintarColorCells(ExcelWorksheet worksheet, string cell)
        {
            Color headColorFromHex = System.Drawing.ColorTranslator.FromHtml("#e2efda");
            worksheet.Cells[cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[cell].Style.Fill.BackgroundColor.SetColor(headColorFromHex);
        }

        #endregion

        #region setCommentarioCellAyuda

        public void setCommentarioCellAyuda(ExcelWorksheet worksheet, string cell, string ayuda)
        {
            if (ayuda != "" && ayuda != null)
            {
                worksheet.Cells[cell].AddComment(ayuda.Trim(), "SUTEL S.A");
            }
        }

        #endregion

        #region enableCellsByDeglose

        public void enableCellsByDeglose(ExcelWorksheet worksheet, int[] ends, int cantidadMesesDesglose, int cantidadMesesFrecuencia, DateTime fechaInicio, DateTime fechaFin, List<DetalleAgrupacionSP> oListadoIndicadores, ExcelWorksheet worksheetICoordenadasLectura)
        {
            int i = 0;
            int letraEnd = ends[0] - 1;
            int indexEnd = ends[1] - 1;
            string cell = "";
            int firstIndex = 0;
            int frecuencia = this.getDesglosePorMeses(cantidadMesesDesglose, cantidadMesesFrecuencia);
            string nombreAplicarRegla = "";
            string nombreAplicarReglaPadre = "";
            DetalleAgrupacionSP objDetalleAgrupacionSP = new DetalleAgrupacionSP();
            List<DetalleAgrupacionSP> ListadoParcial = new List<DetalleAgrupacionSP>();
            ListadoParcial.AddRange(oListadoIndicadores);
            ListadoParcial.Reverse();
            DateTime fechaCalculo = new DateTime();
            fechaCalculo = fechaInicio;


            for (int k = frecuencia - 1; k >= 0; k--)
            {
                fechaCalculo = fechaCalculo.AddMonths(-cantidadMesesDesglose);
            }

            for (int k = 0; k < frecuencia; k++)
            {
                cell = this.getLetras()[(letraEnd + k)].ToString() + (indexEnd).ToString();

                while (worksheet.Cells[cell].Value != null && indexEnd >= i)
                {
                    cell = this.getLetras()[letraEnd + k + 1].ToString() + (indexEnd - i).ToString();
                    if (cell == "Q15")
                    {
                        //this.guardarCoordenadasLectura(worksheetICoordenadasLectura, objDetalleAgrupacionSP, cell, worksheet.Name, fechaCalculo.Month, fechaCalculo.Year);
                    }

                    this.setValueAndStyle(worksheet, cell, "", false);
                    nombreAplicarRegla = this.buscarEnExcelNombreParaAplicarRegla(worksheet, letraEnd + k + 1, indexEnd - i);
                    nombreAplicarReglaPadre = this.buscarEnExcelNombreParaAplicarReglaPadre(worksheet, letraEnd + k + 1, indexEnd - i, nombreAplicarRegla);
                    objDetalleAgrupacionSP = this.consultarReglaParaIndicador(ListadoParcial, nombreAplicarRegla, nombreAplicarReglaPadre);

                    this.guardarCoordenadasLectura(worksheetICoordenadasLectura, objDetalleAgrupacionSP, cell, worksheet.Name, fechaCalculo.Month, fechaCalculo.Year);
                    this.aplicarRegla(worksheet, cell, objDetalleAgrupacionSP);
                    ListadoParcial.Remove(objDetalleAgrupacionSP);
                    i++;
                    firstIndex++;
                    cell = this.getLetras()[letraEnd + k].ToString() + (indexEnd - i).ToString();

                }
                ListadoParcial.AddRange(oListadoIndicadores);
                ListadoParcial.Reverse();
                fechaCalculo = fechaCalculo.AddMonths(cantidadMesesDesglose);
                i = 0;
            }

            DateTime fechaRestar = new DateTime();
            fechaRestar = fechaInicio;
            int numeroMes = 0;
            int anno = 0;

            for (int k = frecuencia - 1; k >= 0; k--)
            {
                fechaRestar = fechaRestar.AddMonths(-cantidadMesesDesglose);
                numeroMes = fechaRestar.Month;
                anno = fechaRestar.Year;

                cell = this.getLetras()[letraEnd + k + 1].ToString() + (indexEnd - (firstIndex / frecuencia)).ToString();

                if (k == frecuencia - 1)
                    columnaFinalMergeCeldaObservacion = cell.Substring(0, 1);
                this.pintarMesesPorDesgloseYFrecuencia(worksheet, cell, numeroMes, anno);
                this.pintarBorderNegro(worksheet, cell);
            }
        }

        #endregion

        #region guardarCoordenadasLectura

        public void guardarCoordenadasLectura(ExcelWorksheet worksheetICoordenadasLectura, DetalleAgrupacionSP objDetalleAgrupacionSP, string cellCoordenada, string nameTab, int mes, int anno)
        {
            whenInitIndexNextCoordenadaLectura++;
            //IdSolicitud
            string cell = "A" + whenInitIndexNextCoordenadaLectura;
            string cellProvincia;
            worksheetICoordenadasLectura.Cells[cell].Value = objDetalleAgrupacionSP.Id_Solicitud_Indicador.ToString();

            //idConstructorCriterioDetalleAgrupacion
            cell = "B" + whenInitIndexNextCoordenadaLectura;
            if (objDetalleAgrupacionSP.Id_Provincia == null && objDetalleAgrupacionSP.Id_Canton == null && objDetalleAgrupacionSP.Id_Genero == null)
            {
                worksheetICoordenadasLectura.Cells[cell].Value = objDetalleAgrupacionSP.Id_ConstructorCriterio;
            }
            else
            {
                worksheetICoordenadasLectura.Cells[cell].Value = objDetalleAgrupacionSP.Id_Padre_ConstructorCriterio;
            }

            //IdIndicador
            cell = "C" + whenInitIndexNextCoordenadaLectura;
            if (objDetalleAgrupacionSP.ID_Indicador != null)
                worksheetICoordenadasLectura.Cells[cell].Value = objDetalleAgrupacionSP.ID_Indicador.ToString();

            //idTipoValor
            cell = "D" + whenInitIndexNextCoordenadaLectura;
            if (objDetalleAgrupacionSP.Id_Tipo_Valor != null)
            {
                worksheetICoordenadasLectura.Cells[cell].Value = objDetalleAgrupacionSP.Id_Tipo_Valor;
            }
            else
            {
                worksheetICoordenadasLectura.Cells[cell].Value = "";
            }

            //Valor
            cell = "E" + whenInitIndexNextCoordenadaLectura;
            if (objDetalleAgrupacionSP.Id_Tipo_Valor == 2)
            {
                worksheetICoordenadasLectura.Cells[cell].Style.Numberformat.Format = "dd/mm/yyy";
            }
            worksheetICoordenadasLectura.Cells[cell].Formula = "=" + nameTab + "!" + cellCoordenada;

            //Coordenada
            cell = "F" + whenInitIndexNextCoordenadaLectura;
            worksheetICoordenadasLectura.Cells[cell].Value = cellCoordenada;

            //Mes
            cell = "G" + whenInitIndexNextCoordenadaLectura;
            worksheetICoordenadasLectura.Cells[cell].Value = mes;

            //Anno
            cell = "H" + whenInitIndexNextCoordenadaLectura;
            worksheetICoordenadasLectura.Cells[cell].Value = anno;

            //Comentario
            cell = "I" + whenInitIndexNextCoordenadaLectura;
            worksheetICoordenadasLectura.Cells[cell].Value = "";

            //Provincia
            cell = "J" + whenInitIndexNextCoordenadaLectura;
            worksheetICoordenadasLectura.Cells[cell].Value = "";

            if (objDetalleAgrupacionSP.Id_Provincia != null)
            {
                worksheetICoordenadasLectura.Cells[cell].Value = objDetalleAgrupacionSP.Id_Provincia;
            }

            //Canton  

            cell = "K" + whenInitIndexNextCoordenadaLectura;
            cellProvincia = "J" + whenInitIndexNextCoordenadaLectura;
            worksheetICoordenadasLectura.Cells[cell].Value = "";
            if (objDetalleAgrupacionSP.Id_Canton != null)
            {
                worksheetICoordenadasLectura.Cells[cell].Value = objDetalleAgrupacionSP.Id_Canton;
            }
            else
            {
                switch (worksheetICoordenadasLectura.Cells[cellProvincia].Value)
                {
                    case 1:
                        worksheetICoordenadasLectura.Cells[cell].Value = 1;
                        break;
                    case 2:
                        worksheetICoordenadasLectura.Cells[cell].Value = 21;
                        break;
                    case 3:
                        worksheetICoordenadasLectura.Cells[cell].Value = 36;
                        break;
                    case 4:
                        worksheetICoordenadasLectura.Cells[cell].Value = 44;
                        break;
                    case 5:
                        worksheetICoordenadasLectura.Cells[cell].Value = 54;
                        break;
                    case 6:
                        worksheetICoordenadasLectura.Cells[cell].Value = 65;
                        break;
                    case 7:
                        worksheetICoordenadasLectura.Cells[cell].Value = 76;
                        break;

                    default:
                        // code block
                        break;
                }

            }



            //DISTRITO CAMBIO DE INCLUSION
            cell = "L" + whenInitIndexNextCoordenadaLectura;
            worksheetICoordenadasLectura.Cells[cell].Value = "";
            if (objDetalleAgrupacionSP.Id_Distrito != null)
            {
                worksheetICoordenadasLectura.Cells[cell].Value = objDetalleAgrupacionSP.Id_Distrito;
            }

            //Genero
            cell = "M" + whenInitIndexNextCoordenadaLectura;
            worksheetICoordenadasLectura.Cells[cell].Value = "";
            if (objDetalleAgrupacionSP.Id_Genero != null)
            {
                worksheetICoordenadasLectura.Cells[cell].Value = objDetalleAgrupacionSP.Id_Genero;
            }

            //Id Solicitud Constructor
            cell = "N" + whenInitIndexNextCoordenadaLectura;
            worksheetICoordenadasLectura.Cells[cell].Value = objDetalleAgrupacionSP.Id_Solicitud_Constructor;


            //Se agrega la coordenada de lectura del campo ObservacionOperador
            cell = "O" + whenInitIndexNextCoordenadaLectura;
            worksheetICoordenadasLectura.Cells[cell].Value = "B" + (whenInitIndexNextIndicador - 2);

            // CAMBIO PARA CORREGIR EL VALOR
            cell = "P" + whenInitIndexNextCoordenadaLectura;
            worksheetICoordenadasLectura.Cells[cell].Value = objDetalleAgrupacionSP.Id_ConstructorCriterio;

        }

        #endregion

        #region consultarReglaParaIndicador

        public DetalleAgrupacionSP consultarReglaParaIndicador(List<DetalleAgrupacionSP> objRespuesta, string nombre, string padre)
        {
            DetalleAgrupacionSP obj = new DetalleAgrupacionSP();

            try
            {
                //obj = objRespuesta.Where(x => x.Nombre_Detalle_Agrupacion.Trim() == nombre.Trim() && x.Id_Padre_ConstructorCriterio != null).FirstOrDefault();

                // obj = objRespuesta.Where(x => x.Nombre_Detalle_Agrupacion.Trim() == nombre.Trim() && x.Id_Padre_ConstructorCriterio != null && x.Id_Tipo_Valor != null ).FirstOrDefault();

                // Agregado por reglas estadísticas


                obj = objRespuesta.Where(x => x.Nombre_Detalle_Agrupacion.Trim() == nombre.Trim() && x.Nombre_Padre_Detalle_Agrupacion.Trim() == padre && x.Id_Padre_ConstructorCriterio != null && x.Id_Tipo_Valor != null && x.Valor_Superior != null && x.Valor_Inferior != null).FirstOrDefault();


                if (obj != null)
                {
                    return obj;
                }
                else
                {
                    //obj = objRespuesta.Where(x => x.Nombre_Detalle_Agrupacion.Trim() == nombre.Trim()).FirstOrDefault();
                    obj = objRespuesta.Where(x => x.Nombre_Detalle_Agrupacion.Trim() == nombre.Trim() && x.Id_Tipo_Valor != null).FirstOrDefault();
                    if (obj != null)
                    {
                        return obj;
                    }
                    else
                    {
                        obj = new DetalleAgrupacionSP();
                        return obj;
                    }
                }


            }
            catch (Exception ex)
            {
               obj = new DetalleAgrupacionSP();
               return obj;
            }
        }

        #endregion

        #region buscarEnExcelNombreParaAplicarRegla

        public string buscarEnExcelNombreParaAplicarRegla(ExcelWorksheet worksheet, int letra, int index)
        {
            string cell = "";
            string nombre = "";
            int j = 0;
            for (int i = letra; i > 0; i--)
            {
                j++;
                cell = this.getLetras()[letra - j] + (index).ToString();
                if (worksheet.Cells[cell].Value != null)
                {
                    if (worksheet.Cells[cell].Value != "" && worksheet.Cells[cell].Value.ToString() != "X")
                    {
                        nombre = worksheet.Cells[cell].Value.ToString();
                        break;
                    }
                }
            }



            return nombre;
        }
        /// <summary>
        /// /Funcion para obtener el padre y usarlo en la consulpara para que el mismo obtenga los valores correctos 
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="letra"></param>
        /// <param name="index"></param>
        /// <param name="detalle"></param>
        /// <returns></returns>
        public string buscarEnExcelNombreParaAplicarReglaPadre(ExcelWorksheet worksheet, int letra, int index, string detalle)
        {
            string cell = "";
            string nombre = null;
            int j = 0;
            for (int i = letra; i > 0; i--)
            {
                j++;
                cell = this.getLetras()[letra - j] + (index).ToString();
                if (worksheet.Cells[cell].Value != null)
                {
                    if (worksheet.Cells[cell].Value.ToString() != detalle.Trim() && worksheet.Cells[cell].Value.ToString() != "X" && worksheet.Cells[cell].Value != "")
                    {
                        nombre = worksheet.Cells[cell].Value.ToString();
                        break;
                    }
                }
            }



            return nombre;
        }

        #endregion

        /// <summary>
        /// Aplica la regla a las celdas de las Agrupaciones
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="cell"></param>
        /// <param name="objDetalleAgrupacionSP"></param>

        #region aplicarRegla

        public void aplicarRegla(ExcelWorksheet worksheet, string cell, DetalleAgrupacionSP objDetalleAgrupacionSP)
        {
            //kevin
            if (objDetalleAgrupacionSP.Valor_Inferior != null && objDetalleAgrupacionSP.Valor_Superior != null)
                if (objDetalleAgrupacionSP.Id_Tipo_Valor != null)
                {

                    switch (objDetalleAgrupacionSP.Id_Tipo_Valor)
                    {
                        //Texto
                        case 1:
                            this.aplicarReglaTexto(worksheet, cell);
                            break;

                        //Fecha
                        case 2:
                            try
                            {
                                this.aplicarReglaFecha(worksheet, cell, DateTime.ParseExact(objDetalleAgrupacionSP.Valor_Inferior, "dd/MM/yyyy", null), DateTime.ParseExact(objDetalleAgrupacionSP.Valor_Superior, "dd/MM/yyyy", null));
                            }
                            catch (Exception ex)
                            {
                                this.aplicarReglaFecha(worksheet, cell, new DateTime(2000, 01, 01), new DateTime(DateTime.Now.Year + 40, 12, 30));
                            }
                            break;

                        //Porcentaje 
                        case 3:


                            this.aplicarReglaDecimal(worksheet, cell, double.Parse(objDetalleAgrupacionSP.Valor_Inferior), double.Parse(objDetalleAgrupacionSP.Valor_Superior));
                            this.aplicarFormatoPorcentage(worksheet, cell);
                            break;

                        //Monto
                        case 4:
                            this.aplicarReglaDecimal(worksheet, cell, double.Parse(objDetalleAgrupacionSP.Valor_Inferior), double.Parse(objDetalleAgrupacionSP.Valor_Superior));
                            //  this.aplicarFormatoMoneda(worksheet, cell);
                            //this.aplicarReglaCellNumerico(worksheet, cell, int.Parse(objDetalleAgrupacionSP.Valor_Inferior), Convert.ToInt64(objDetalleAgrupacionSP.Valor_Superior));
                            break;


                        //Cantidad sin decimales
                        case 5:
                            this.aplicarReglaCellNumerico(worksheet, cell, int.Parse(objDetalleAgrupacionSP.Valor_Inferior), Convert.ToInt64(objDetalleAgrupacionSP.Valor_Superior));
                            //this.aplicarFormatoNumero(worksheet, cell);
                            //this.aplicarReglaCellNumerico(worksheet, cell, int.Parse(objDetalleAgrupacionSP.Valor_Inferior), Int64.Parse (objDetalleAgrupacionSP.Valor_Superior));
                            break;

                        //Minutos
                        case 7:
                            this.aplicarReglaCellNumerico(worksheet, cell, int.Parse(objDetalleAgrupacionSP.Valor_Inferior), Convert.ToInt64(objDetalleAgrupacionSP.Valor_Superior));
                            //this.aplicarReglaCellNumerico(worksheet, cell, int.Parse(objDetalleAgrupacionSP.Valor_Inferior), Int64.Parse(objDetalleAgrupacionSP.Valor_Superior));
                            break;

                        //Cantidad con decimales
                        case 6:
                            this.aplicarReglaDecimal(worksheet, cell, double.Parse(objDetalleAgrupacionSP.Valor_Inferior), double.Parse(objDetalleAgrupacionSP.Valor_Superior));
                            break;
                        default:
                            break;
                    }
                }
        }

        #endregion

        #region aplicarReglaEstadisticaConNivelDetalle

        public void aplicarReglaEstadisticaConNivelDetalle(ExcelWorksheet worksheet, string cell, DetalleAgrupacionSP objDetalleAgrupacionSP, int indiceValoresRegla)
        {

            double valorMinimo = new double();
            double valorMaximo = new double();

            if (objDetalleAgrupacionSP.Id_Tipo_Valor != null)
            {

                switch (objDetalleAgrupacionSP.Id_Tipo_Valor)
                {
                    //Porcentaje 
                    case 3:

                        if (trabajandoConCantones)
                        {

                            valorMinimo = double.Parse(valoresReglaCantones[indiceValoresRegla].ValorLimiteInferior);
                            //La siguiente línea es para trabajar con cuatro decimales 
                            valorMinimo = ((double)((int)(valorMinimo * 10000.0))) / 10000.0;

                            valorMaximo = double.Parse(valoresReglaCantones[indiceValoresRegla].ValorLimiteSuperior);

                            valorMaximo = ((double)((int)(valorMaximo * 10000.0))) / 10000.0;

                            this.aplicarReglaDecimal(worksheet, cell, valorMinimo, valorMaximo);

                        }

                        if (trabajandoConGeneros)
                        {

                            valorMinimo = double.Parse(valoresReglaGeneros[indiceValoresRegla].ValorLimiteInferior);
                            //La siguiente línea es para trabajar con cuatro decimales 
                            valorMinimo = ((double)((int)(valorMinimo * 10000.0))) / 10000.0;

                            valorMaximo = double.Parse(valoresReglaGeneros[indiceValoresRegla].ValorLimiteSuperior);

                            valorMaximo = ((double)((int)(valorMaximo * 10000.0))) / 10000.0;

                            this.aplicarReglaDecimal(worksheet, cell, valorMinimo, valorMaximo);

                        }


                        if (trabajandoConProvincias)
                        {

                            valorMinimo = double.Parse(valoresReglaProvincias[indiceValoresRegla].ValorLimiteInferior);
                            //La siguiente línea es para trabajar con cuatro decimales 
                            valorMinimo = ((double)((int)(valorMinimo * 10000.0))) / 10000.0;

                            valorMaximo = double.Parse(valoresReglaProvincias[indiceValoresRegla].ValorLimiteSuperior);

                            valorMaximo = ((double)((int)(valorMaximo * 10000.0))) / 10000.0;

                            this.aplicarReglaDecimal(worksheet, cell, valorMinimo, valorMaximo);

                        }

                        this.aplicarFormatoPorcentage(worksheet, cell);
                        break;

                    //Monto
                    case 4:


                        if (trabajandoConCantones)
                        {

                            valorMinimo = double.Parse(valoresReglaCantones[indiceValoresRegla].ValorLimiteInferior);
                            //La siguiente línea es para trabajar con cuatro decimales 
                            valorMinimo = ((double)((int)(valorMinimo * 10000.0))) / 10000.0;

                            valorMaximo = double.Parse(valoresReglaCantones[indiceValoresRegla].ValorLimiteSuperior);

                            valorMaximo = ((double)((int)(valorMaximo * 10000.0))) / 10000.0;

                            this.aplicarReglaDecimal(worksheet, cell, valorMinimo, valorMaximo);
                        }

                        if (trabajandoConGeneros)
                        {

                            valorMinimo = double.Parse(valoresReglaGeneros[indiceValoresRegla].ValorLimiteInferior);
                            //La siguiente línea es para trabajar con cuatro decimales 
                            valorMinimo = ((double)((int)(valorMinimo * 10000.0))) / 10000.0;

                            valorMaximo = double.Parse(valoresReglaGeneros[indiceValoresRegla].ValorLimiteSuperior);

                            valorMaximo = ((double)((int)(valorMaximo * 10000.0))) / 10000.0;

                            this.aplicarReglaDecimal(worksheet, cell, valorMinimo, valorMaximo);

                        }


                        if (trabajandoConProvincias)
                        {

                            valorMinimo = double.Parse(valoresReglaProvincias[indiceValoresRegla].ValorLimiteInferior);
                            //La siguiente línea es para trabajar con cuatro decimales 
                            valorMinimo = ((double)((int)(valorMinimo * 10000.0))) / 10000.0;

                            valorMaximo = double.Parse(valoresReglaProvincias[indiceValoresRegla].ValorLimiteSuperior);

                            valorMaximo = ((double)((int)(valorMaximo * 10000.0))) / 10000.0;

                            this.aplicarReglaDecimal(worksheet, cell, valorMinimo, valorMaximo);


                        }




                        this.aplicarFormatoMoneda(worksheet, cell);
                        break;

                    //Cantidad sin decimales
                    case 5:

                        if (trabajandoConCantones)
                        {
                            valorMinimo = double.Parse(valoresReglaCantones[indiceValoresRegla].ValorLimiteInferior);
                            //La siguiente línea es para trabajar con cuatro decimales 
                            valorMinimo = ((double)((int)(valorMinimo * 10000.0))) / 10000.0;

                            valorMaximo = double.Parse(valoresReglaCantones[indiceValoresRegla].ValorLimiteSuperior);

                            valorMaximo = ((double)((int)(valorMaximo * 10000.0))) / 10000.0;


                            this.aplicarReglaCellNumerico(worksheet, cell, Int32.Parse(valorMinimo.ToString()), Convert.ToInt64(valorMaximo.ToString()));
                            //this.aplicarReglaCellNumerico(worksheet, cell, Int32.Parse(valorMinimo.ToString()), int.Parse (valorMaximo.ToString()));
                        }

                        if (trabajandoConGeneros)
                        {

                            valorMinimo = double.Parse(valoresReglaGeneros[indiceValoresRegla].ValorLimiteInferior);
                            //La siguiente línea es para trabajar con cuatro decimales 
                            valorMinimo = ((double)((int)(valorMinimo * 10000.0))) / 10000.0;

                            valorMaximo = double.Parse(valoresReglaGeneros[indiceValoresRegla].ValorLimiteSuperior);

                            valorMaximo = ((double)((int)(valorMaximo * 10000.0))) / 10000.0;


                            this.aplicarReglaCellNumerico(worksheet, cell, Int32.Parse(valorMinimo.ToString()), Convert.ToInt64(valorMaximo.ToString()));
                            //this.aplicarReglaCellNumerico(worksheet, cell, Int32.Parse(valorMinimo.ToString()), int.Parse(valorMaximo.ToString()));

                        }

                        if (trabajandoConProvincias)
                        {
                            valorMinimo = double.Parse(valoresReglaProvincias[indiceValoresRegla].ValorLimiteInferior);
                            //La siguiente línea es para trabajar con cuatro decimales 
                            valorMinimo = ((double)((int)(valorMinimo * 10000.0))) / 10000.0;

                            valorMaximo = double.Parse(valoresReglaProvincias[indiceValoresRegla].ValorLimiteSuperior);

                            valorMaximo = ((double)((int)(valorMaximo * 10000.0))) / 10000.0;


                            this.aplicarReglaCellNumerico(worksheet, cell, Int32.Parse(valorMinimo.ToString()), Convert.ToInt64(valorMaximo.ToString()));
                            //this.aplicarReglaCellNumerico(worksheet, cell, Int32.Parse(valorMinimo.ToString()), int.Parse (valorMaximo.ToString()));
                        }
                        break;

                    //Minutos
                    case 7:

                        if (trabajandoConCantones)
                        {
                            valorMinimo = double.Parse(valoresReglaCantones[indiceValoresRegla].ValorLimiteInferior);
                            //La siguiente línea es para trabajar con cuatro decimales 
                            valorMinimo = ((double)((int)(valorMinimo * 10000.0))) / 10000.0;

                            valorMaximo = double.Parse(valoresReglaCantones[indiceValoresRegla].ValorLimiteSuperior);

                            valorMaximo = ((double)((int)(valorMaximo * 10000.0))) / 10000.0;


                            this.aplicarReglaCellNumerico(worksheet, cell, Int32.Parse(valorMinimo.ToString()), Convert.ToInt64(valorMaximo.ToString()));
                            //this.aplicarReglaCellNumerico(worksheet, cell, Int32.Parse(valorMinimo.ToString()), int.Parse (valorMaximo.ToString()));
                        }

                        if (trabajandoConGeneros)
                        {

                            valorMinimo = double.Parse(valoresReglaGeneros[indiceValoresRegla].ValorLimiteInferior);
                            //La siguiente línea es para trabajar con cuatro decimales 
                            valorMinimo = ((double)((int)(valorMinimo * 10000.0))) / 10000.0;

                            valorMaximo = double.Parse(valoresReglaGeneros[indiceValoresRegla].ValorLimiteSuperior);

                            valorMaximo = ((double)((int)(valorMaximo * 10000.0))) / 10000.0;


                            this.aplicarReglaCellNumerico(worksheet, cell, Int32.Parse(valorMinimo.ToString()), Convert.ToInt64(valorMaximo.ToString()));
                            //this.aplicarReglaCellNumerico(worksheet, cell, Int32.Parse(valorMinimo.ToString()), int.Parse (valorMaximo.ToString()));
                        }

                        if (trabajandoConProvincias)
                        {
                            valorMinimo = double.Parse(valoresReglaProvincias[indiceValoresRegla].ValorLimiteInferior);
                            //La siguiente línea es para trabajar con cuatro decimales 
                            valorMinimo = ((double)((int)(valorMinimo * 10000.0))) / 10000.0;

                            valorMaximo = double.Parse(valoresReglaProvincias[indiceValoresRegla].ValorLimiteSuperior);

                            valorMaximo = ((double)((int)(valorMaximo * 10000.0))) / 10000.0;


                            this.aplicarReglaCellNumerico(worksheet, cell, Int32.Parse(valorMinimo.ToString()), Convert.ToInt64(valorMaximo.ToString()));
                            //this.aplicarReglaCellNumerico(worksheet, cell, Int32.Parse(valorMinimo.ToString()), int.Parse (valorMaximo.ToString()));
                        }

                        break;

                    //Cantidad con decimales
                    case 6:

                        if (trabajandoConCantones)
                        {
                            valorMinimo = double.Parse(valoresReglaCantones[indiceValoresRegla].ValorLimiteInferior);
                            //La siguiente línea es para trabajar con cuatro decimales 
                            valorMinimo = ((double)((int)(valorMinimo * 10000.0))) / 10000.0;

                            valorMaximo = double.Parse(valoresReglaCantones[indiceValoresRegla].ValorLimiteSuperior);

                            valorMaximo = ((double)((int)(valorMaximo * 10000.0))) / 10000.0;


                            this.aplicarReglaDecimal(worksheet, cell, valorMinimo, valorMaximo);
                        }

                        if (trabajandoConGeneros)
                        {

                            valorMinimo = double.Parse(valoresReglaGeneros[indiceValoresRegla].ValorLimiteInferior);
                            //La siguiente línea es para trabajar con cuatro decimales 
                            valorMinimo = ((double)((int)(valorMinimo * 10000.0))) / 10000.0;

                            valorMaximo = double.Parse(valoresReglaGeneros[indiceValoresRegla].ValorLimiteSuperior);

                            valorMaximo = ((double)((int)(valorMaximo * 10000.0))) / 10000.0;


                            this.aplicarReglaDecimal(worksheet, cell, valorMinimo, valorMaximo);
                        }

                        if (trabajandoConProvincias)
                        {
                            valorMinimo = double.Parse(valoresReglaProvincias[indiceValoresRegla].ValorLimiteInferior);
                            //La siguiente línea es para trabajar con cuatro decimales 
                            valorMinimo = ((double)((int)(valorMinimo * 10000.0))) / 10000.0;

                            valorMaximo = double.Parse(valoresReglaProvincias[indiceValoresRegla].ValorLimiteSuperior);

                            valorMaximo = ((double)((int)(valorMaximo * 10000.0))) / 10000.0;

                            this.aplicarReglaDecimal(worksheet, cell, valorMinimo, valorMaximo);
                        }

                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        #region aplicarReglaCellNumerico

        public void aplicarReglaCellNumerico(ExcelWorksheet worksheet, string cell, int minValue, Int64 maxValue)
        {
            //worksheet.Cells[cell].Style.Numberformat.Format = "#";
            string minV = numeroEntero(minValue);
            string maxV = numeroEntero(maxValue);
            worksheet.Cells[cell].Style.Numberformat.Format = "#,###,###";

            //var validation = worksheet.DataValidations.AddIntegerValidation(cell);
            var validation = worksheet.DataValidations.AddIntegerValidation(cell);
            validation.ErrorStyle = ExcelDataValidationWarningStyle.stop;
            validation.PromptTitle = "Inserte un valor númerico.";
            validation.Prompt = "Solamente números. Entre " + minV + " a " + maxV;
            validation.ShowInputMessage = true;
            validation.ErrorTitle = "Error de dato";
            validation.Error = "Solamente números. Entre " + minV + " a " + maxV;
            validation.ShowErrorMessage = true;
            validation.Operator = ExcelDataValidationOperator.between;
            validation.Formula.Value = minValue;
            validation.Formula2.Value = maxValue;
            //Convert.ToInt32(maxValue);

        }

        public string numeroEntero(Int64 num)
        {
            string[] value2;
            string value = "";

            value = num.ToString("N4", new CultureInfo("is-IS"));
            value2 = value.Split(',');
            value = value2[0];

            return value;

        }


        #endregion

        #region aplicarReglaFecha

        public void aplicarReglaFecha(ExcelWorksheet worksheet, string cell, DateTime minDate, DateTime maxDate)
        {
            var validation = worksheet.DataValidations.AddDateTimeValidation(cell);
            validation.ErrorStyle = ExcelDataValidationWarningStyle.stop;
            validation.PromptTitle = "Inserte una fecha.";
            validation.Prompt = "Formato: dd/mm/yyyy \n\nEntre " + minDate.ToString("dd/MM/yyyy") + " a " + maxDate.ToString("dd/MM/yyyy");
            validation.ShowInputMessage = true;
            validation.ErrorTitle = "Error de dato";
            validation.Error = "Solamente se aceptan fechas. Entre " + minDate.ToString("dd/MM/yyyy") + " a " + maxDate.ToString("dd/MM/yyyy");
            validation.ShowErrorMessage = true;
            validation.Operator = ExcelDataValidationOperator.between;
            validation.Formula.Value = minDate;
            validation.Formula2.Value = maxDate;
        }

        #endregion

        #region aplicarReglaTexto

        public void aplicarReglaTexto(ExcelWorksheet worksheet, string cell)
        {
            var validation = worksheet.DataValidations.AddAnyValidation(cell);
            validation.PromptTitle = "";
            validation.Prompt = "Inserte un valor texto.";
            validation.ShowInputMessage = true;
        }

        #endregion

        #region aplicarReglaDecimal

        public void aplicarReglaDecimal(ExcelWorksheet worksheet, string cell, double minValue, double maxValue)
        {
            //worksheet.Cells[cell].Style.Numberformat.Format = "#";            

            //var validation = worksheet.DataValidations.AddDecimalValidation(cell);
            worksheet.Cells[cell].Style.Numberformat.Format = "#,###,###.0000";
            var validation = worksheet.DataValidations.AddDecimalValidation(cell);
            validation.ErrorStyle = ExcelDataValidationWarningStyle.stop;
            validation.PromptTitle = "Inserte un valor númerico decimal.";
            validation.Prompt = "Solamente números. Entre " + minValue.ToString("N4", new CultureInfo("is-IS")) + " a " + maxValue.ToString("N4", new CultureInfo("is-IS"));
            validation.ShowInputMessage = true;
            validation.ErrorTitle = "Error de dato";
            validation.Error = "Solamente números decimales. Entre " + minValue.ToString("N4", new CultureInfo("is-IS")) + " a " + maxValue.ToString("N4", new CultureInfo("is-IS"));
            validation.ShowErrorMessage = true;
            validation.Operator = ExcelDataValidationOperator.between;
            validation.Formula.Value = minValue;
            validation.Formula2.Value = maxValue;

        }

        #endregion

        #region aplicarFormatoMoneda

        public void aplicarFormatoMoneda(ExcelWorksheet worksheet, string cell)
        {
            worksheet.Cells[cell].Style.Numberformat.Format = " #,###,###.00";
        }

        public void aplicarFormatoNumero(ExcelWorksheet worksheet, string cell)
        {
            worksheet.Cells[cell].Style.Numberformat.Format = " #,###,###.00";
        }

        #endregion

        #region aplicarFormatoPorcentage

        public void aplicarFormatoPorcentage(ExcelWorksheet worksheet, string cell)
        {
            worksheet.Cells[cell].Style.Numberformat.Format = "#\\%";
        }

        #endregion

        #region pintarMesesPorDesgloseYFrecuencia

        public void pintarMesesPorDesgloseYFrecuencia(ExcelWorksheet worksheet, string cell, int numeroMes, int anno)
        {
            worksheet.Cells[cell].Value = this.getNombreMeses(numeroMes).ToString().Substring(0, 3) + "-" + anno.ToString().Substring(2, 2);
            worksheet.Cells[cell].Style.Font.Bold = true;
            worksheet.Cells[cell].Style.Font.Size = 9;
        }

        #endregion

        #region getNombreMeses

        public string getNombreMeses(int numero)
        {
            string[] meses = new string[13];
            meses[0] = ""; meses[1] = "Enero";
            meses[2] = "Febrero"; meses[3] = "Marzo";
            meses[4] = "Abril"; meses[5] = "Mayo";
            meses[6] = "Junio"; meses[7] = "Julio";
            meses[8] = "Agosto"; meses[9] = "Septiembre";
            meses[10] = "Octubre"; meses[11] = "Noviembre";
            meses[12] = "Diciembre";

            if (numero > 13)
            {
                numero = 0;
            }

            return meses[numero];
        }

        #endregion

        #region pintarEncabezado

        public void pintarEncabezado(ExcelWorksheet worksheet, string nombreOperador, string nombreDireccion, string nombreServicio)
        {
            Color headColorFromHex = System.Drawing.ColorTranslator.FromHtml("#2f75b5");
            Color fontColorFromHex = System.Drawing.ColorTranslator.FromHtml("#fff");
            Color grayColorFromHex = System.Drawing.ColorTranslator.FromHtml("#e7e6e6");
            Color greenColorFromHex = System.Drawing.ColorTranslator.FromHtml("#e2efda");



            worksheet.Cells["A1:N8"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

            worksheet.Cells["A1:N6"].Style.Fill.BackgroundColor.SetColor(grayColorFromHex);
            worksheet.Cells["A7:N7"].Style.Fill.BackgroundColor.SetColor(headColorFromHex);
            worksheet.Cells["A8:N8"].Style.Fill.BackgroundColor.SetColor(greenColorFromHex);
            worksheet.Row(7).Height = 6;
            worksheet.Cells["G2"].Value = "";
            worksheet.Cells["G2"].Style.Font.Bold = true;

            worksheet.Cells["G3"].Value = "Operador:";
            worksheet.Cells["G3"].Style.Font.Bold = true;
            worksheet.Cells["H3"].Value = nombreOperador.Trim();

            worksheet.Cells["G4"].Value = "Dirección:";
            worksheet.Cells["G4"].Style.Font.Bold = true;
            worksheet.Cells["H4"].Value = nombreDireccion.Trim();

            worksheet.Cells["G5"].Value = "Servicio:";
            worksheet.Cells["G5"].Style.Font.Bold = true;
            worksheet.Cells["H5"].Value = nombreServicio.Trim();


            string user = nombreUsuario.Trim();
            //worksheet.Cells["H2"].Value = user == null ? "" : user;
            Image logo = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Content\\Images\\logos\\logo-Sutel_11_3.png");
            logo = (Image)(new Bitmap(logo, new Size(313, 90)));
            var picture = worksheet.Drawings.AddPicture("SUTEL", logo);
            picture.SetPosition(1, 0, 0, 0);
        }

        #endregion

        #region pintarEncabezadoIndicador

        /// <summary>
        /// Diego Navarrete Alvarez
        /// En la ultima modifcación se agrego Id indicador y se 
        /// incremento el valor de los ciclos de 5 a 6 
        /// </summary>
        /// <param name="IdIndicador"></param>

        public void pintarEncabezadoIndicador(ExcelWorksheet worksheet, string IdIndicador, string nombreIndicador, string nombreDesglose, string nombreFrecuencia, string nombresDireccion)
        {
            Color grayColorFromHex = System.Drawing.ColorTranslator.FromHtml("#e2efda");

            string cell = string.Empty;

            cell = "A" + (this.whenInitIndexNextIndicador - 6).ToString();
            worksheet.Cells[cell].Value = "Id Indicador:";
            worksheet.Cells[cell].Style.Font.Bold = true;
            cell = "B" + (this.whenInitIndexNextIndicador - 6).ToString();
            worksheet.Cells[cell].Value = IdIndicador.Trim();

            cell = "A" + (this.whenInitIndexNextIndicador - 5).ToString();
            worksheet.Cells[cell].Value = "Indicador:";
            worksheet.Cells[cell].Style.Font.Bold = true;
            cell = "B" + (this.whenInitIndexNextIndicador - 5).ToString();
            worksheet.Cells[cell].Value = nombreIndicador.Trim();

            cell = "A" + (this.whenInitIndexNextIndicador - 4).ToString();
            worksheet.Cells[cell].Value = "Frecuencia:";
            worksheet.Cells[cell].Style.Font.Bold = true;
            cell = "B" + (this.whenInitIndexNextIndicador - 4).ToString();
            worksheet.Cells[cell].Value = nombreFrecuencia.Trim();

            cell = "A" + (this.whenInitIndexNextIndicador - 3).ToString();
            worksheet.Cells[cell].Value = "Desglose:";
            worksheet.Cells[cell].Style.Font.Bold = true;
            cell = "B" + (this.whenInitIndexNextIndicador - 3).ToString();
            worksheet.Cells[cell].Value = nombreDesglose.Trim();

            //Added by kevin

            cell = "A" + (this.whenInitIndexNextIndicador - 2).ToString();
            worksheet.Cells[cell].Value = "Observación:";
            worksheet.Cells[cell].Style.Font.Bold = true;
            cell = "B" + (this.whenInitIndexNextIndicador - 2).ToString();
            worksheet.Cells[cell].Value = "Este campo es opcional";
            worksheet.Cells[cell].Style.Locked = false;

            ////MERGE DE CELDAS PARA CAMPO OBSERVACIÓN START       

            //Se hace un merge de celdas para el campo observación
            var fila = (whenInitIndexNextIndicador - 2);
            string coordenada = "B" + fila + ":" + columnaFinalMergeCeldaObservacion + fila;
            worksheet.Cells[coordenada].Merge = true;

            //MERGE DE CELDAS PARA CAMPO OBSERVACIÓN END



            int indexLetra = 0;
            cell = this.getLetras()[indexLetra] + (this.whenInitIndexNextIndicador - 1).ToString();
            while (worksheet.Cells[cell].Value == null)
            {
                for (int i = 1; i <= 7; i++)
                {
                    worksheet.Cells[cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[cell].Style.Fill.BackgroundColor.SetColor(grayColorFromHex);
                    cell = this.getLetras()[indexLetra] + (this.whenInitIndexNextIndicador - i).ToString();
                }
                indexLetra++;
                cell = this.getLetras()[indexLetra] + (this.whenInitIndexNextIndicador - 1).ToString();
            }

            while (worksheet.Cells[cell].Value != null)
            {
                for (int i = 1; i <= 7; i++)
                {
                    worksheet.Cells[cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[cell].Style.Fill.BackgroundColor.SetColor(grayColorFromHex);
                    cell = this.getLetras()[indexLetra] + (this.whenInitIndexNextIndicador - i).ToString();
                }
                indexLetra++;
                cell = this.getLetras()[indexLetra] + (this.whenInitIndexNextIndicador - 1).ToString();
            }
        }

        #endregion

        #region pintarFooterIndicador
        //Se cambiaron los 7s por 8s y el 6 por un 7. 
        public void pintarFooterIndicador(ExcelWorksheet worksheet)
        {
            Color grayColorFromHex = System.Drawing.ColorTranslator.FromHtml("#e7e6e6");

            int indexLetra = 0;
            string cell = this.getLetras()[indexLetra] + (this.whenInitIndexNextIndicador - 8).ToString();
            while (worksheet.Cells[cell].Value != null)
            {
                for (int i = 5; i <= 7; i++)
                {
                    cell = this.getLetras()[indexLetra] + (this.whenInitIndexNextIndicador - i).ToString();
                    worksheet.Cells[cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[cell].Style.Fill.BackgroundColor.SetColor(grayColorFromHex);
                }
                indexLetra++;
                cell = this.getLetras()[indexLetra] + (this.whenInitIndexNextIndicador - 8).ToString();
            }

        }

        #endregion

        #region getDesglosePorMeses

        public int getDesglosePorMeses(int cantidadMesesDesglose, int cantidadMesesFrecuencia)
        {
            int cantidad = 0;
            if (cantidadMesesFrecuencia >= cantidadMesesDesglose)
            {
                cantidad = cantidadMesesFrecuencia / cantidadMesesDesglose;
            }

            if (cantidad <= 0)
            {
                cantidad = 1;
            }

            return cantidad;
        }

        #endregion

        #region getCantidadHijosPorPadre

        public int getCantidadHijosPorPadre(int? idPadre, List<DetalleAgrupacionSP> objRespuesta)
        {
            int cantidadHijos = 0;
            foreach (DetalleAgrupacionSP da in objRespuesta)
            {
                if (idPadre == da.Id_Padre_Detalle_Agrupacion)
                {
                    if (da.Id_Padre_Detalle_Agrupacion != null)
                    {
                        cantidadHijos++;
                    }
                }
            }

            return cantidadHijos;
        }

        #endregion

        #region getIdPadrePorHijo

        public string getIdPadrePorHijo(int idHijo, List<DetalleAgrupacionSP> objRespuesta)
        {
            string idPadre = "";
            foreach (DetalleAgrupacionSP da in objRespuesta)
            {
                if (idHijo == da.Id_Detalle_Agrupacion)
                {
                    if (da.Id_Padre_ConstructorCriterio.ToString() != "" && da.Id_Padre_ConstructorCriterio != null)
                    {
                        idPadre = da.Id_Padre_ConstructorCriterio.ToString();
                    }
                    break;
                }
            }

            return idPadre;
        }

        #endregion

        #region getLetras
        /// <summary>
        /// set celdas Excel
        /// </summary>
        /// <returns>Objeto de tipo Respuesta</returns>
        public string[] getLetras()
        {
            string[] letras = new string[104];
            letras[0] = "A"; letras[1] = "B";
            letras[2] = "C"; letras[3] = "D";
            letras[4] = "E"; letras[5] = "F";
            letras[6] = "G"; letras[7] = "H";
            letras[8] = "I"; letras[9] = "J";
            letras[10] = "K"; letras[11] = "L";
            letras[12] = "M"; letras[13] = "N";
            letras[14] = "O"; letras[15] = "P";
            letras[16] = "Q"; letras[17] = "R";
            letras[18] = "S"; letras[19] = "T";
            letras[20] = "U"; letras[21] = "V";
            letras[22] = "W"; letras[23] = "X";
            letras[24] = "Y"; letras[25] = "Z";

            letras[26] = "AA"; letras[27] = "AB";
            letras[28] = "AC"; letras[29] = "AD";
            letras[30] = "AE"; letras[31] = "AF";
            letras[32] = "AG"; letras[33] = "AH";
            letras[34] = "AI"; letras[35] = "AJ";
            letras[36] = "AK"; letras[37] = "AL";
            letras[38] = "AM"; letras[39] = "AN";
            letras[40] = "AO"; letras[41] = "AP";
            letras[42] = "AQ"; letras[43] = "AR";
            letras[44] = "AS"; letras[45] = "AT";
            letras[46] = "AU"; letras[47] = "AV";
            letras[48] = "AW"; letras[49] = "AX";
            letras[50] = "AY"; letras[51] = "AZ";

            letras[52] = "BA"; letras[53] = "BB";
            letras[54] = "BC"; letras[55] = "BD";
            letras[56] = "BE"; letras[57] = "BF";
            letras[58] = "BG"; letras[59] = "BH";
            letras[60] = "BI"; letras[61] = "BJ";
            letras[62] = "BK"; letras[63] = "BL";
            letras[64] = "BM"; letras[65] = "BN";
            letras[66] = "BO"; letras[67] = "BP";
            letras[68] = "BQ"; letras[69] = "BR";
            letras[70] = "BS"; letras[71] = "BT";
            letras[72] = "BU"; letras[73] = "BV";
            letras[74] = "BW"; letras[75] = "BX";
            letras[76] = "BY"; letras[77] = "BZ";

            letras[78] = "CA"; letras[79] = "CB";
            letras[80] = "CC"; letras[81] = "CD";
            letras[82] = "CE"; letras[83] = "CF";
            letras[84] = "CG"; letras[85] = "CH";
            letras[86] = "CI"; letras[87] = "CJ";
            letras[88] = "CK"; letras[89] = "CL";
            letras[90] = "CM"; letras[91] = "CN";
            letras[92] = "CO"; letras[93] = "CP";
            letras[94] = "CQ"; letras[95] = "CR";
            letras[96] = "CS"; letras[97] = "CT";
            letras[98] = "CU"; letras[99] = "CV";
            letras[100] = "CW"; letras[101] = "CX";
            letras[102] = "CY"; letras[103] = "CZ";

            return letras;
        }

        #endregion

        #region addIndicadoresCriteriosInfo
        /// <summary>
        /// set celdas Excel
        /// </summary>
        /// <returns>Objeto de tipo Respuesta</returns>
        public void addIndicadoresCriteriosInfo(ExcelWorksheet worksheet, List<DetalleAgrupacionSP> objRespuesta)
        {
            //this.EncabezadoCriterios(worksheet);
            //string nombreIndicador = "";
            //string cell = "";
            //int indexLetra = 0;
            //int indexRow = 3;
            //List<DetalleAgrupacionSP> objRespuestaFilterIndicador = new List<DetalleAgrupacionSP>();
            //for (int i = 0; i < objRespuesta.Count; i++)
            //{
            //    if (nombreIndicador != objRespuesta[i].Nombre_Indicador)
            //    {
            //        objRespuestaFilterIndicador = objRespuesta.Where(x => x.Nombre_Indicador == objRespuesta[i].Nombre_Indicador).ToList();
            //        nombreIndicador = objRespuesta[i].Nombre_Indicador;                    
            //        i = i + objRespuestaFilterIndicador.Count - 1;

            //        cell = getLetras()[indexLetra++].ToString() + indexRow.ToString();
            //        this.setValueAndStyle(worksheet, cell, objRespuesta[i].ID_Indicador.ToString(), true);

            //        cell = getLetras()[indexLetra++].ToString() + indexRow.ToString();
            //        this.setValueAndStyle(worksheet, cell, objRespuesta[i].Nombre_Indicador.ToString(), true);

            //        cell = getLetras()[indexLetra++].ToString() + indexRow.ToString();
            //        this.setValueAndStyle(worksheet, cell, objRespuesta[i].Constructor_Criterio_Ayuda.ToString(), true);

            //        cell = getLetras()[indexLetra++].ToString() + indexRow.ToString();
            //        this.setValueAndStyle(worksheet, cell, objRespuesta[i].Nombre_Criterio.ToString(), true);
            //        indexRow++;
            //        indexLetra = 0;
            //    }
            //}

            this.EncabezadoCriterios(worksheet);
            string cell = "";
            int indexLetra = 0;
            int indexRow = 3;
            List<DetalleAgrupacionSP> objRespuestaFilterIndicador = new List<DetalleAgrupacionSP>();

            objRespuestaFilterIndicador = objRespuesta.OrderBy(z => z.Nombre_Criterio).Distinct().ToList();
            objRespuestaFilterIndicador = objRespuestaFilterIndicador.GroupBy(x => x.Nombre_Criterio).Select(g => g.First()).ToList();

            for (int i = 0; i < objRespuestaFilterIndicador.Count; i++)
            {
                cell = getLetras()[indexLetra++].ToString() + indexRow.ToString();
                this.setValueAndStyle(worksheet, cell, objRespuestaFilterIndicador[i].ID_Indicador.ToString(), true);

                cell = getLetras()[indexLetra++].ToString() + indexRow.ToString();
                this.setValueAndStyle(worksheet, cell, objRespuestaFilterIndicador[i].Nombre_Indicador.ToString(), true);

                cell = getLetras()[indexLetra++].ToString() + indexRow.ToString();
                this.setValueAndStyle(worksheet, cell, objRespuestaFilterIndicador[i].Constructor_Criterio_Ayuda.ToString(), true);

                cell = getLetras()[indexLetra++].ToString() + indexRow.ToString();
                this.setValueAndStyle(worksheet, cell, objRespuestaFilterIndicador[i].Nombre_Criterio.ToString(), true);
                indexRow++;
                indexLetra = 0;
            }
        }

        #endregion

        #region EncabezadoCriterios
        /// <summary>
        /// set celdas Excel
        /// </summary>
        /// <returns>Objeto de tipo Respuesta</returns>
        public void EncabezadoCriterios(ExcelWorksheet worksheet)
        {
            Color headColorFromHex = System.Drawing.ColorTranslator.FromHtml("#2f75b5");
            Color fontColorFromHex = System.Drawing.ColorTranslator.FromHtml("#fff");
            Color grayColorFromHex = System.Drawing.ColorTranslator.FromHtml("#e7e6e6");
            Color greenColorFromHex = System.Drawing.ColorTranslator.FromHtml("#e2efda");

            string cell = "A1:D1";
            worksheet.Select(cell);
            worksheet.SelectedRange.Merge = true;

            worksheet.Cells[cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[cell].Style.Fill.BackgroundColor.SetColor(headColorFromHex);
            worksheet.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            worksheet.Cells[cell].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            worksheet.Cells[cell].Style.WrapText = true;

            worksheet.Row(1).Height = 30;
            worksheet.Column(1).Width = 15;
            worksheet.Column(3).Width = 200;
            worksheet.Cells[cell].Value = "Información de Criterios por Indicador";
            worksheet.Cells[cell].Style.Font.Bold = true;
            worksheet.Cells[cell].Style.Font.Size = 14;
            worksheet.Cells[cell].Style.Font.Color.SetColor(fontColorFromHex);

            string value = "";
            for (int i = 0; i < 4; i++)
            {
                cell = this.getLetras()[i].ToString() + "2";
                if (i == 0)
                {
                    value = "ID";
                }

                if (i == 1)
                {
                    value = "Indicador";
                }

                if (i == 2)
                {
                    value = "DEFINICIÓN GENERAL (Ayuda)";
                }

                if (i == 3)
                {
                    value = "Criterio";
                }

                worksheet.Cells[cell].Value = value;
                worksheet.Cells[cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[cell].Style.Fill.BackgroundColor.SetColor(greenColorFromHex);
                worksheet.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells[cell].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                worksheet.Cells[cell].Style.WrapText = true;
                worksheet.Cells[cell].Style.Font.Bold = true;

                worksheet.Cells[cell].Style.Font.Size = 12;
                worksheet.Cells[cell].Style.Font.Color.SetColor(headColorFromHex);
            }
        }

        #endregion

        #region EncabezadoInicioEnableMacro
        /// <summary>
        /// set celdas Excel
        /// </summary>
        /// <returns>Objeto de tipo Respuesta</returns>
        public void EncabezadoInicioEnableMacro(ExcelWorksheet worksheet)
        {
            Color headColorFromHex = System.Drawing.ColorTranslator.FromHtml("#FF0000");
            Color fontColorFromHex = System.Drawing.ColorTranslator.FromHtml("#fff");

            string cell = "A1:C1";
            worksheet.Select(cell);
            worksheet.SelectedRange.Merge = true;

            worksheet.Cells[cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[cell].Style.Fill.BackgroundColor.SetColor(headColorFromHex);
            worksheet.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            worksheet.Cells[cell].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            worksheet.Cells[cell].Style.WrapText = true;

            worksheet.Row(1).Height = 45;
            worksheet.Cells[cell].Value = "Debe habilitar las macros para continuar.";
            worksheet.Cells[cell].Style.Font.Bold = true;
            worksheet.Cells[cell].Style.Font.Size = 16;
            worksheet.Cells[cell].Style.Font.Color.SetColor(fontColorFromHex);

            /*cell = "A2:C2";
            worksheet.Select(cell);
            worksheet.SelectedRange.Merge = true;
            worksheet.Row(2).Height = 20;
            worksheet.Cells[cell].Value = "¿Cómo habilitar las macros?";
            worksheet.Cells[cell].Style.Font.Bold = true;
            worksheet.Cells[cell].Style.Font.Size = 16;
            //worksheet.Cells[cell].Style.Font.Color.SetColor(fontColorFromHex);   

            cell = "A3:C3";
            worksheet.Select(cell);
            worksheet.SelectedRange.Merge = true;
            worksheet.Cells[cell].Style.Font.Bold = true;
            worksheet.Cells[cell].Style.Font.Size = 16;
            worksheet.Cells[cell].Hyperlink = new ExcelHyperLink("https://www.youtube.com/watch?v=InpLE_QKQJo");
            worksheet.Cells[cell].StyleName = "Office 2013";
            worksheet.Cells[cell].Value = "Office 2013";
            //worksheet.Cells[cell].Style.Font.Color.SetColor(fontColorFromHex);   

            cell = "A4:C4";
            worksheet.Select(cell);
            worksheet.SelectedRange.Merge = true;            
            worksheet.Cells[cell].Style.Font.Bold = true;
            worksheet.Cells[cell].Style.Font.Size = 16;
            worksheet.Cells[cell].Hyperlink = new ExcelHyperLink("https://www.youtube.com/watch?v=InpLE_QKQJo");
            worksheet.Cells[cell].StyleName = "Office 2007";
            worksheet.Cells[cell].Value = "Office 2007";
            //worksheet.Cells[cell].Style.Font.Color.SetColor(fontColorFromHex);   

            cell = "A5:C5";
            worksheet.Select(cell);
            worksheet.SelectedRange.Merge = true;
            worksheet.Cells[cell].Style.Font.Bold = true;
            worksheet.Cells[cell].Style.Font.Size = 16;
            worksheet.Cells[cell].Hyperlink = new ExcelHyperLink("https://www.youtube.com/watch?v=InpLE_QKQJo");
            worksheet.Cells[cell].StyleName = "Office 2003";
            worksheet.Cells[cell].Value = "Office 2003";
            //worksheet.Cells[cell].Style.Font.Color.SetColor(fontColorFromHex); */
        }

        #endregion

        #region EncabezadoCoordenadasLectura
        /// <summary>
        /// set celdas Excel
        /// </summary>
        /// <returns>Objeto de tipo Respuesta</returns>
        public void EncabezadoCoordenadasLectura(ExcelWorksheet worksheet)
        {
            worksheet.DefaultColWidth = 30;
            string cell = "A1";
            this.setValueAndStyle(worksheet, cell, "IdSolicitud", true);
            worksheet.Cells[cell].Style.Font.Bold = true;

            cell = "B1";
            this.setValueAndStyle(worksheet, cell, "idConstructorCriterioDetalleAgrupacion", true);
            worksheet.Cells[cell].Style.Font.Bold = true;

            cell = "C1";
            this.setValueAndStyle(worksheet, cell, "IdIndicador", true);
            worksheet.Cells[cell].Style.Font.Bold = true;

            cell = "D1";
            this.setValueAndStyle(worksheet, cell, "idTipoValor", true);
            worksheet.Cells[cell].Style.Font.Bold = true;

            cell = "E1";
            this.setValueAndStyle(worksheet, cell, "Valor", true);
            worksheet.Cells[cell].Style.Font.Bold = true;

            cell = "F1";
            this.setValueAndStyle(worksheet, cell, "Coordenada", true);
            worksheet.Cells[cell].Style.Font.Bold = true;

            cell = "G1";
            this.setValueAndStyle(worksheet, cell, "Mes", true);
            worksheet.Cells[cell].Style.Font.Bold = true;

            cell = "H1";
            this.setValueAndStyle(worksheet, cell, "Anno", true);
            worksheet.Cells[cell].Style.Font.Bold = true;

            cell = "I1";
            this.setValueAndStyle(worksheet, cell, "Cometario", true);
            worksheet.Cells[cell].Style.Font.Bold = true;

            cell = "J1";
            this.setValueAndStyle(worksheet, cell, "IdProvincia", true);
            worksheet.Cells[cell].Style.Font.Bold = true;

            cell = "K1";
            this.setValueAndStyle(worksheet, cell, "IdCanton", true);
            worksheet.Cells[cell].Style.Font.Bold = true;

            cell = "L1";
            this.setValueAndStyle(worksheet, cell, "IdDistrito", true);
            worksheet.Cells[cell].Style.Font.Bold = true;

            cell = "M1";
            this.setValueAndStyle(worksheet, cell, "IdGenero", true);
            worksheet.Cells[cell].Style.Font.Bold = true;

            cell = "N1";
            this.setValueAndStyle(worksheet, cell, "IdSolicitudConstructor", true);
            worksheet.Cells[cell].Style.Font.Bold = true;

            // Se agrega una columna "ObservacionOperador"
            // Hecho el 31/03/2016 by Kevin Solís
            cell = "O1";
            this.setValueAndStyle(worksheet, cell, "Observacion", true);
            worksheet.Cells[cell].Style.Font.Bold = true;

            // Se agrega una columna "idcriteriopadre para corregir el overflow que crea el cambio de provincia y genero y distrito"
            //
            cell = "P1";
            this.setValueAndStyle(worksheet, cell, "IdCriterio", true);
            worksheet.Cells[cell].Style.Font.Bold = true;

        }

        #endregion

        #region createVBADisableCopyPaste
        public void createVBADisableCopyPaste(ExcelWorkbook workbook)
        {
            workbook.CreateVBAProject();
            StringBuilder function = new StringBuilder();

            function.Append("\r\n Option Explicit \r\n");

            function.Append("\r\nSub ToggleCutCopyAndPaste(Allow As Boolean)\r\n");
            function.Append("\r\n Application.CellDragAndDrop = False \r\n");
            function.Append("\r\n Application.OnKey \"^c\" , \"\" \r\n");
            function.Append("\r\n Application.OnKey \"^v\", \"\" \r\n");
            function.Append("\r\n Application.OnKey \"^x\" , \"\" \r\n");
            function.Append("\r\n Application.OnKey \"+{DEL}\" , \"\" \r\n");
            function.Append("\r\n Application.OnKey \"^{INSERT}\" , \"\" \r\n");
            function.Append("\r\n Application.CommandBars(\"Standard\").Enabled = False\r\n");
            function.Append("\r\n Application.CutCopyMode = False\r\n");
            //  function.Append("\r\n Application. = False\r\n");
            //function.Append("\r\n Application.ExecuteExcel4Macro \"SHOW.TOOLBAR(\"\"Ribbon\"\",False)\"\r\n");
            //function.Append("\r\n Application.CommandBars(\"standard\").Controls(\"&Copiar\").Enabled = False\r\n");

            function.Append("\r\nEnd Sub \r\n");

            function.Append("\r\nPrivate Sub Workbook_SheetBeforeRightClick (ByVal Sh As Object, ByVal Target As Range, Cancel As Boolean)\r\n");
            function.Append("\r\nCancel = True \r\n");
            function.Append("\r\n MsgBox \"En este documento no se permite la opción de clic derecho.\", vbExclamation,  \"Bloqueo\"\r\n");
            function.Append("End Sub \r\n");

            //function.Append("\r\nPrivate Sub Workbook_Open() \r\n");
            //function.Append("Call ToggleCutCopyAndPaste(False) \r\n");

            //function.Append("Dim ws As Worksheet \r\n");
            //function.Append("For Each ws In ThisWorkbook.Worksheets \r\n");
            //function.Append("ws.Visible = xlSheetVisible \r\n");
            //function.Append("Next ws \r\n");
            //function.Append("Sheets(\"Inicio\").Visible = xlVeryHidden \r\n");
            //function.Append("Sheets(\"Coordenadas_de_Lectura\").Visible = xlVeryHidden \r\n");
            //function.Append("Sheets(\"Indicadores\").Select \r\n");
            //function.Append("\r\nEnd Sub \r\n");

            // function.Append("\r\n Private Sub Workbook_BeforeClose(Cancel As Boolean)\r\n");
            // function.Append("\r\nApplication.CellDragAndDrop = True\r\n");
            // function.Append("\r\nApplication.OnKey \"^c\"\r\n");
            // function.Append("\r\nApplication.OnKey \"^v\"\r\n");
            // function.Append("\r\nApplication.OnKey \"^x\"\r\n");
            // function.Append("\r\nApplication.OnKey \"+{DEL}\"\r\n");
            // function.Append("\r\nApplication.OnKey \"^{INSERT}\"\r\n");
            // function.Append("\r\nApplication.DisplayFormulaBar = True\r\n");
            // function.Append("\r\nEnd Sub\r\n");

            // function.Append("\r\n Private Sub Workbook_BeforeSave(ByVal SaveAsUI As Boolean, Cancel As Boolean)\r\n");
            // function.Append("\r\nApplication.ExecuteExcel4Macro \"SHOW.TOOLBAR(\"\"Ribbon\"\",True)\"\r\n");
            // function.Append("\r\nApplication.DisplayFormulaBar = True\r\n");
            // function.Append("\r\nApplication.CellDragAndDrop = True\r\n");
            // function.Append("\r\nApplication.OnKey \"^c\"\r\n");
            // function.Append("\r\nApplication.OnKey \"^v\"\r\n");
            // function.Append("\r\nApplication.OnKey \"^x\"\r\n");
            // function.Append("\r\nApplication.OnKey \"+{DEL}\"\r\n");
            // function.Append("\r\nApplication.OnKey \"^{INSERT}\"\r\n");
            //function.Append("\r\nEnd Sub\r\n");

            // function.Append("\n Sub  Msg_Error()\n");
            // function.Append("\n   MsgBox  \"This is a message!\", vbExclamation, \"This is the Title\" \n");
            // function.Append("\r\n End Sub ");

            workbook.CodeModule.Code += function;
            workbook.CodeModule.ReadOnly = true;
        }

        #endregion

        #region convertToOldFormat
        public Byte[] convertToOldFormat(Byte[] bytes, string URLSaveFormatXLS)
        {
            int random = 0;
            Random numRan = new Random();
            random = numRan.Next(1, 50);

            string path = URLSaveFormatXLS + "convert" + random.ToString() + ".xlsm";
            File.WriteAllBytes(path, bytes);

            var app = new Microsoft.Office.Interop.Excel.Application();
            var wb = app.Workbooks.Open(path);

            //Get sheets.
            Microsoft.Office.Interop.Excel.Worksheet sheetInicio = (Microsoft.Office.Interop.Excel.Worksheet)app.ActiveWorkbook.Sheets["Inicio"];
            Microsoft.Office.Interop.Excel.Worksheet sheetIndicadores = (Microsoft.Office.Interop.Excel.Worksheet)app.ActiveWorkbook.Sheets["Indicadores"];
            Microsoft.Office.Interop.Excel.Worksheet sheetInformacionCriterios = (Microsoft.Office.Interop.Excel.Worksheet)app.ActiveWorkbook.Sheets["Información de Criterios"];
            Microsoft.Office.Interop.Excel.Worksheet sheetCoordenadas_de_Lectura = (Microsoft.Office.Interop.Excel.Worksheet)app.ActiveWorkbook.Sheets["Coordenadas_de_Lectura"];

            //Hide the worksheets.
            sheetInicio.Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetVisible;
            sheetIndicadores.Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
            sheetInformacionCriterios.Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
            sheetCoordenadas_de_Lectura.Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;

            app.DisplayAlerts = false;
            wb.SaveAs(Filename: path + "2" + ".xls", FileFormat: Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
            wb.Close();
            app.Quit();

            bytes = File.ReadAllBytes(path + "2" + ".xls");


            if (File.Exists(path))
            {
                File.Delete(path);
            }
            if (File.Exists(path + "2" + ".xls"))
            {
                File.Delete(path + "2" + ".xls");
            }

            return bytes;
        }
        #endregion

        #region ReiniciarAtributos
        public void ReiniciarAtributos()
        {

            to = ConfigurationManager.AppSettings["to"];
            profileName = ConfigurationManager.AppSettings["profileName"];
            rutaGuardarExcel = "F:\\";
            nombreExcel = "Plantilla " + String.Format("{0:MM-dd-yyyy}", DateTime.Now);
            extensionExcel = ".xlsx";
            nombreUsuario = "";
            IdOperador = "";
            whenInitIndexNextIndicador = 15;//era 14
            whenInitIndexNextCoordenadaLectura = 1;
            EventWaitHandle _waitHandle = new AutoResetEvent(false);
            columnaFinalMergeCeldaObservacion = string.Empty;
            IdSolicitudActual = "";
            IdOperadorActual = "";
            // Agregados por el requerimiento de regla estadistica {
            trabajandoConCantones = false;
            trabajandoConProvincias = false;
            trabajandoConGeneros = false;
            valoresReglaProvincias = new List<ValoresReglaSP>();
            valoresReglaGeneros = new List<ValoresReglaSP>();
            valoresReglaCantones = new List<ValoresReglaSP>();
            nombresCantones = new List<string>();
            nombresProvincias = new List<string>();
            nombresGeneros = new List<string>();

            //  }

        }
        #endregion

        #region GuardarBytesArchivoExcel
        public void GuardarBytesArchivoExcel(Byte[] bytesArchivoExcel, string idArchivoExcel, string connString)
        {

            StoredProcedures sp = new StoredProcedures();

            sp.InsertarArchivoExcel(bytesArchivoExcel, idArchivoExcel, connString);
        }

        #endregion

        #region NotificarErrores
        public void NotificarError(string msjError, string connString)
        {
            string MsjError = "Ha ocurrido un error al generar el archivo excel.\n El error fue el siguiente:\n" + msjError;
            string asunto = "Error al generar archivo Excel";
            StoredProcedures sp = new StoredProcedures();

            //MANDAR PARAMETRO EN 100
            sp.SendEmail(to, asunto, "<p>" + MsjError + "</p>", profileName, connString);
        }

        #endregion


        #region ReiniciarControlesReglaEstadistica
        public void ReiniciarControlesReglaEstadistica()
        {

            trabajandoConCantones = false;
            trabajandoConProvincias = false;
            trabajandoConGeneros = false;
            valoresReglaProvincias = new List<ValoresReglaSP>();
            valoresReglaGeneros = new List<ValoresReglaSP>();
            valoresReglaCantones = new List<ValoresReglaSP>();
            nombresCantones = new List<string>();
            nombresProvincias = new List<string>();
            nombresGeneros = new List<string>();
        }

        #endregion

    }
}
