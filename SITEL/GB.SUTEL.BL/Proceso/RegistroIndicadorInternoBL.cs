using ClosedXML.Excel;
using GB.SUTEL.BL.Mantenimientos;
using GB.SUTEL.DAL.Proceso;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.Shared;
using GB.SUTEL.UI.Helpers;
using NPOI.HSSF.UserModel;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SUTEL.BL.Proceso
{
    public class RegistroIndicadorInternoBL : LocalContextualizer
    {
        #region atributos
        RegistroIndicadorInternoAD refRegistroIndicadorInternoAD;
        SolicitudIndicadorBL refSolicitudIndicadorBL;


        BitacoraIndicadorInternoBL refBitacoraBl;
  
        #endregion

        public RegistroIndicadorInternoBL(ApplicationContext appContext)
            : base(appContext)
        {
            refRegistroIndicadorInternoAD = new RegistroIndicadorInternoAD(appContext);
            refSolicitudIndicadorBL = new SolicitudIndicadorBL(appContext);
            refBitacoraBl = new BitacoraIndicadorInternoBL(appContext);
          
        }

        #region metodosAD


        /// <summary>
        /// Modifica el registro de indicador
        /// </summary>
        /// <param name="psIdDetalleRegistro"></param>
        /// <param name="valor"></param>
        public Respuesta<DetalleRegistroIndicador> gModificarRegistroIndicador(Guid psIdDetalleRegistro, string valor, string psJustificacion)
        {
            Respuesta<DetalleRegistroIndicador> objRespuesta = new Respuesta<DetalleRegistroIndicador>();
            try
            {
                 objRespuesta = lValidarModificacionRegistro(psIdDetalleRegistro, valor, psJustificacion);
                if (objRespuesta.blnIndicadorTransaccion == true)
                {
                    objRespuesta = refRegistroIndicadorInternoAD.gModificarRegistroIndicador(psIdDetalleRegistro, valor);
                }
                else
                {
                    return objRespuesta;
                }

            }
            catch (BusinessException ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(ex.Message, objRespuesta.objObjeto);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(ex.Message, objRespuesta.objObjeto);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
            return objRespuesta;

        }


        public Respuesta<DetalleRegistroIndicador> gModificarRegistroIndicadorMasivo(Guid psIdDetalleRegistro, string valor, BitacoraIndicador pbitacora)
        {
            Respuesta<DetalleRegistroIndicador> objRespuesta = new Respuesta<DetalleRegistroIndicador>();
            try
            {
                objRespuesta = refRegistroIndicadorInternoAD.gModificarRegistroIndicador(psIdDetalleRegistro, valor);
               
                refBitacoraBl.gInsertarBitacoraIndicadorExterno(pbitacora);

            }
            catch (BusinessException ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(ex.Message, objRespuesta.objObjeto);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(ex.Message, objRespuesta.objObjeto);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
            return objRespuesta;

        }






        /// <summary>
        /// Valida el valor a introducir
        /// </summary>
        /// <param name="psIdDetalleRegistro"></param>
        /// <param name="poValor"></param>
        private Respuesta<DetalleRegistroIndicador> lValidarModificacionRegistro(Guid psIdDetalleRegistro, String poValor, String psJustificacion) {
            Respuesta<DetalleRegistroIndicador> objRespuesta = new Respuesta<DetalleRegistroIndicador>();
            int tipoValor = 0;
            String valorSuperior = "";
            String valorInferior = "";

            try {
                if (psJustificacion.Trim().Equals(""))
                {
                    objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.ModificarIndicador_Justificacion;
                    objRespuesta.blnIndicadorTransaccion = false;
                    return objRespuesta;
                }
                if (poValor.Trim().Equals("")) {
                    objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.ModificarIndicador_Valor;
                    objRespuesta.blnIndicadorTransaccion = false;
                    return objRespuesta;
                }
                objRespuesta = refRegistroIndicadorInternoAD.gObtenerDetalleRegistroIndicadorPorID(psIdDetalleRegistro);
                tipoValor = (int)objRespuesta.objObjeto.ConstructorCriterioDetalleAgrupacion.IdTipoValor;
                valorInferior = objRespuesta.objObjeto.ConstructorCriterioDetalleAgrupacion.Regla.ValorLimiteInferior;
                valorSuperior = objRespuesta.objObjeto.ConstructorCriterioDetalleAgrupacion.Regla.ValorLimiteSuperior;
                if (tipoValor == 2) {
                    if (DateTime.Parse(valorInferior) > DateTime.Parse(poValor) || DateTime.Parse(valorSuperior) < DateTime.Parse(poValor)) {
                        objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.ModificarIndicador_AdventenciaValor + " " + valorInferior + " y " + valorSuperior;
                        objRespuesta.blnIndicadorTransaccion = false;
                        return objRespuesta;
                    }
                }
                else if (tipoValor == 3 || tipoValor == 5) {
                    poValor = poValor.Replace(" ", "");
                    if (Int64.Parse(valorInferior.Trim()) > Int64.Parse(poValor.Trim()) || Int64.Parse(valorSuperior.Trim()) < Int64.Parse(poValor.Trim()))
                    {
                        objRespuesta.blnIndicadorTransaccion = false;
                        objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.ModificarIndicador_AdventenciaValor + " " + valorInferior + " y " + valorSuperior;
                        return objRespuesta;
                    }
                }
                else if (tipoValor == 4 || tipoValor == 6)
                {
                    poValor = poValor.Replace(" ", "");
                    if (Double.Parse(valorInferior.Trim()) > Double.Parse(poValor.Trim()) || Double.Parse(valorSuperior.Trim()) < Double.Parse(poValor.Trim()))
                    {
                        objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.ModificarIndicador_AdventenciaValor + " " + valorInferior + " y " + valorSuperior;
                        objRespuesta.blnIndicadorTransaccion = false;
                        return objRespuesta;
                    }
                }
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(ex.Message, objRespuesta.objObjeto);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
            return objRespuesta;
        }

        /// <summary>
        /// Registro del indicador interno
        /// </summary>
        /// <param name="poIdSolicitud"></param>
        /// <param name="idOperador"></param>
        /// <param name="piUsuario"></param>
        /// <param name="nombreArchivo"></param>
        /// <param name="psDireccionArchivo"></param>
        /// <param name="archivo"></param>
        /// <returns></returns>
        public Respuesta<List<RegistroIndicador>> gRegistroIndicadorInterno(Guid poIdSolicitud, String idOperador, int piUsuario, String nombreArchivo, Stream archivo, String path, ref int direccion)
        {
                      
            Respuesta<List<RegistroIndicador>> objRespuesta = new Respuesta<List<RegistroIndicador>>();
            Respuesta<SolicitudIndicador> validacion = new Respuesta<SolicitudIndicador>();
            Respuesta<SolicitudIndicador> validacionfecha = new Respuesta<SolicitudIndicador>();
            List<Guid> listaRegistrosInsertados = new List<Guid>();
            List<Guid> listaRegistros = new List<Guid>();
            DataTable datos = new DataTable();
            List<RegistroIndicador> nuevoRegistro = new List<RegistroIndicador>();
            Respuesta<bool> respuestaSolicitud = new Respuesta<bool>();
            string msjNull = string.Empty;
            try
            {
                validacion = lValidacionSolicitud(poIdSolicitud, idOperador);
                ///validacionfecha = lValidacionSolicitud(poIdSolicitud, idOperador);
                ///

                if (validacion.blnIndicadorTransaccion == false)
                {
                    //------------metodo para eliminar todos los registros indicadores junto a su detalle----------/
                    refRegistroIndicadorInternoAD.eliminarRegistroIndicadorExistente(idOperador, poIdSolicitud);


                    //------------metodo para eliminar solo un registro indicador junto a su detalle---------------/
                    /*var listaRegistros2 = refRegistroIndicadorInternoAD.obtenerRegistroIndicador("A3E6E5C3-8CD2-4174-880A-2A97658F97CF");                   

                    listaRegistros = listaRegistros2.Select(x => x.IdRegistroIndicador).Distinct().ToList();

                    refRegistroIndicadorInternoAD.lEliminarRegistroIndicador(listaRegistros);

                    */
                  
                }

                if (nombreArchivo.ToUpper().Contains("XLSM"))
                {     
                    datos = lLeerExcelXLSX(archivo);

                }else
                {
                    datos = lLeerExcelXLS(archivo, path);
                }

                nuevoRegistro = lTrasformacionObjeto(datos, piUsuario);
                direccion = validacion.objObjeto.IdDireccion;
    
                //validar el excel
                validacion = lValidarExcel(nuevoRegistro, poIdSolicitud, validacion.objObjeto);
                    
                if (validacion.blnIndicadorTransaccion)
                {
                    respuestaSolicitud.blnIndicadorTransaccion = false;

                    objRespuesta = refRegistroIndicadorInternoAD.gAgregarRegistroIndicador(nuevoRegistro, direccion);

                    if (objRespuesta.blnIndicadorTransaccion == true) 
                    { 
                        respuestaSolicitud=lActualizarSolicitudRegistro(poIdSolicitud, idOperador, (int)CCatalogo.EstadosSolicitud.FINALIZADO);
                    }
                    
                    if (respuestaSolicitud.objObjeto == false || objRespuesta.blnIndicadorTransaccion == false)
                    {
                        listaRegistrosInsertados = objRespuesta.objObjeto.Select(x => x.IdRegistroIndicador).Distinct().ToList();
                        refRegistroIndicadorInternoAD.lEliminarRegistroIndicador(listaRegistrosInsertados);
                    }

                }else 
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.strMensaje = validacion.strMensaje;
                }
            }
                
           
            catch (Exception ex)
            {
                /* esto esta fallando
                 * if (respuestaSolicitud.blnIndicadorTransaccion == false && objRespuesta.blnIndicadorTransaccion==true)
                 {
                     listaRegistrosInsertados = objRespuesta.objObjeto.Select(x => x.IdRegistroIndicador).Distinct().ToList();
                     refRegistroIndicadorInternoAD.lEliminarRegistroIndicador(listaRegistrosInsertados);
                 }
                 string msj = "";
                 if (ex is CustomException) {
                     throw ex;
                 }

                 else {
                  msj = GB.SUTEL.Shared.ErrorTemplate.InternalError; 
                 }*/
                string msj = ex.Message;
                objRespuesta.toError(ex.Message, objRespuesta.objObjeto);
                objRespuesta.strMensaje = msjNull;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
                

            }
            return objRespuesta;
        }
        /// <summary>
        /// Actualización de la solitud
        /// </summary>
        /// <param name="poIdSolicitud"></param>
        /// <param name="psIdOperador"></param>
        /// <param name="piEstado"></param>
        private Respuesta<bool> lActualizarSolicitudRegistro(Guid poIdSolicitud, string psIdOperador, int piEstado)
        {
            Respuesta<bool> respuesta = new Respuesta<bool>();
            try {

               respuesta= refSolicitudIndicadorBL.gActualizarEstadoSolicitud(poIdSolicitud, psIdOperador, (int)CCatalogo.EstadosSolicitud.FINALIZADO);
            }
            
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.RegistroIndicador_ExcelSolicitudActualizada;
                respuesta.toError(ex.Message, false);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
           return respuesta;
        }

        #endregion

        #region validacion
        /// <summary>
        /// Validación de la solicitud al cargar el documento
        /// </summary>
        /// <param name="poIdSolicitud"></param>
        /// <param name="idOperador"></param>
        private Respuesta<SolicitudIndicador> lValidacionSolicitud(Guid poIdSolicitud, String idOperador)
        {
            Respuesta<SolicitudIndicador> objRespuesta = new Respuesta<SolicitudIndicador>();
            List<SolicitudConstructor> listaConstructores = new List<SolicitudConstructor>();
            List<Constructor> Item = new List<Constructor>();
            List<RegistroIndicador> ListaRegistro = new List<RegistroIndicador>(); 
             List<Guid> listaRegistrosInsertados = new List<Guid>();
            try
            {
                objRespuesta = refSolicitudIndicadorBL.gConsultarPorIdentificador(poIdSolicitud);
                if (objRespuesta.objObjeto == null)
                {
                    //la solicitud no existe
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.objObjeto = objRespuesta.objObjeto;
                    objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.RegistroIndicador_SolicitudInexistente;
                }
                else {
                    listaConstructores = objRespuesta.objObjeto.SolicitudConstructor.Where(x => x.IdOperador.Equals(idOperador)).ToList();
                    objRespuesta.objObjeto.SolicitudConstructor = new List<SolicitudConstructor>(listaConstructores);
                }

                if (objRespuesta.objObjeto.FechaInicio.Date > DateTime.Now.Date || objRespuesta.objObjeto.FechaFin.Date < DateTime.Now.Date)
                {
                    //la solicitud no es vigente
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.objObjeto = objRespuesta.objObjeto;
                    objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.RegistroDuplicado;
                }
                listaConstructores = new List<SolicitudConstructor>(objRespuesta.objObjeto.SolicitudConstructor.Where(x => x.IdEstado.Equals((int)CCatalogo.EstadosSolicitud.FINALIZADO) && x.IdOperador.Equals(idOperador)).ToList());
                if (listaConstructores == null || listaConstructores.Count > 0)
                {
                    /// Metodo para eliminar la informacion previamente Guardada
                    /// Elimino la informacion asociada 

                    //foreach (SolicitudConstructor item  in listaConstructores)
                    //{
                    //    var i = item.RegistroIndicador.Single();
                    //    listaRegistrosInsertados.Add(i.IdRegistroIndicador);
                    //    refRegistroIndicadorInternoAD.lEliminarRegistroIndicador(listaRegistrosInsertados);
                    //    listaRegistrosInsertados.Clear();
                    //}
                                           
                    // //solicitud finalizada
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.objObjeto = objRespuesta.objObjeto;
                    objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.RegistroIndicador_SolicitudEnviada;

                    // Cambio para que permita recargar la plantilla 
                    //objRespuesta.objObjeto.SolicitudConstructor = new List<SolicitudConstructor>(listaConstructores);
                }
                listaConstructores = new List<SolicitudConstructor>(objRespuesta.objObjeto.SolicitudConstructor.Where( x =>x.IdSolicitudIndicador.Equals(poIdSolicitud) && x.IdOperador.Equals(idOperador)).ToList());
                if (listaConstructores == null || listaConstructores.Count == 0)
                {
                    //la solicitud no corresponde al operador marcado
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.objObjeto = objRespuesta.objObjeto;
                    objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.RegistroIndicador_ExcelOperador;
                }
            }
            catch (DataAccessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(ex.Message, objRespuesta.objObjeto);
                throw  AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
            return objRespuesta;
        }
        /// <summary>
        /// Validación del excel
        /// </summary>
        /// <param name="registros"></param>
        private Respuesta<SolicitudIndicador> lValidarExcel(List<RegistroIndicador> registros, Guid poIDSolicitud, SolicitudIndicador poSolicitudIndicador)
        {
            List<RegistroIndicador> registrosValidacion = new List<RegistroIndicador>();
            
            Respuesta<SolicitudIndicador> objRespuesta = new Respuesta<SolicitudIndicador>();
            try
            {
                registrosValidacion = registros.Where(x => x.SolicitudConstructor.IdSolicitudIndicador.Equals(poIDSolicitud)).ToList();
                objRespuesta.objObjeto = new SolicitudIndicador();
                int cantidadConstructores = 0;
                if (!registros.Count().Equals(registrosValidacion.Count()))
                {
                    //excel no pertenece a la solicitud marcada
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.RegistroIndicador_ExcelNoSolicitud;
                    return objRespuesta;
                }
                foreach (SolicitudConstructor item in poSolicitudIndicador.SolicitudConstructor)
                {
                    int indice = registros.FindIndex(x => x.IdSolicitudConstructor.Equals(item.IdSolicitudContructor));
                    if (indice >= 0)
                    {
                        cantidadConstructores++;
                    }
                }
                if (cantidadConstructores == 0)
                {
                    //el excel no corresponde al operador
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.RegistroIndicador_ExcelNoPerteneceOperador;
                }
                else if (!registros.Count().Equals(cantidadConstructores))
                {
                    //la cantidad de indicadores solicitados es diferente a los cargados al excel
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.RegistroIndicador_ExcelImcompletoIndicadores;
                }


            }

            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(ex.Message, objRespuesta.objObjeto);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
            return objRespuesta;
        }

        #endregion

        #region transformacionExcel
        /// <summary>
        /// Transformación del datatable extraido del excel en objetos para persistir en bd
        /// </summary>
        /// <param name="poDatos"></param>
        /// <param name="piIdUsuario"></param>
        /// <returns></returns>
        private List<RegistroIndicador> lTrasformacionObjeto(DataTable poDatos, int piIdUsuario)
        {
            RegistroIndicador respuesta = new RegistroIndicador();
            DetalleRegistroIndicador detalle = new DetalleRegistroIndicador();
            List<RegistroIndicador> respuestas = new List<RegistroIndicador>();
            Guid idSolicitudConstructorAnterior = new Guid();
            Guid idSolicitudConstructorActual = new Guid();
            Guid test = new Guid();
           
            int contador = 0;
            int contadortest = 0;
            try
            {
                DataView dv = poDatos.DefaultView;
                dv.Sort = "IdSolicitudConstructor desc";
                poDatos = dv.ToTable();

                foreach (DataRow row in poDatos.Rows)
                {
                    idSolicitudConstructorActual = new Guid(row["IdSolicitudConstructor"].ToString().Trim());
                //    idSolicitudConstructorActual = new Guid(row[0].ToString().Trim());
                    contadortest++;
                    if(contadortest == 273){
                    
                    }
                    if (contador == 0)
                    {
                        idSolicitudConstructorAnterior = idSolicitudConstructorActual;
                        respuesta = new RegistroIndicador();
                        respuesta.IdSolicitudConstructor = new Guid(row["IdSolicitudConstructor"].ToString().Trim());
                        respuesta.SolicitudConstructor = new SolicitudConstructor();
                        respuesta.SolicitudConstructor.IdSolicitudIndicador = new Guid(row["IdSolicitud"].ToString().Trim());
                        respuesta.IdUsuario = piIdUsuario;
                        respuesta.Observacion = row["Observacion"].ToString().Trim();
                        contador++;
                    }
                    if (!idSolicitudConstructorAnterior.Equals(idSolicitudConstructorActual))
                    {
                        if (!idSolicitudConstructorActual.Equals(test))
                        {

                            respuestas.Add(respuesta);
                            respuesta = new RegistroIndicador();
                            respuesta.IdSolicitudConstructor = new Guid(row["IdSolicitudConstructor"].ToString().Trim());
                            respuesta.SolicitudConstructor = new SolicitudConstructor();
                            respuesta.SolicitudConstructor.IdSolicitudIndicador = new Guid(row["IdSolicitud"].ToString().Trim());
                            respuesta.IdUsuario = piIdUsuario;
                            respuesta.Observacion = row["Observacion"].ToString().Trim();
                            idSolicitudConstructorAnterior = new Guid(row["IdSolicitudConstructor"].ToString().Trim());
                            //contador++;
                        }
                    }
                    if (!idSolicitudConstructorActual.Equals(test)){
                
                    detalle = new DetalleRegistroIndicador();
                    if (row["idCriterio"].ToString().Trim() != "")
                    {
                       detalle.IdConstructorCriterio = new Guid(row["idCriterio"].ToString().Trim());
                        //detalle.IdConstructorCriterio = new Guid(row["idConstructorCriterioDetalleAgrupacion"].ToString().Trim());
                    }
                    else
                    {
                        detalle.IdConstructorCriterio = new Guid(row["idConstructorCriterioDetalleAgrupacion"].ToString().Trim());
                    }

                    detalle.IdTipoValor = int.Parse(row["idTipoValor"].ToString());
                    detalle.IdProvincia = (row["IdProvincia"] == null || row["IdProvincia"].ToString().Equals("") ? 0 : int.Parse(row["IdProvincia"].ToString()));
                    detalle.IdCanton = (row["IdCanton"] == null || row["IdCanton"].ToString().Equals("") ? 0 : int.Parse(row["IdCanton"].ToString()));
                    detalle.IdGenero = (row["IdGenero"] == null || row["IdGenero"].ToString().Equals("") ? 0 : int.Parse(row["IdGenero"].ToString()));
                    detalle.IdDistrito = (row["IdDistrito"] == null || row["IdDistrito"].ToString().Equals("") ? 0 : int.Parse(row["IdDistrito"].ToString()));
                    detalle.Anno = int.Parse(row["Anno"].ToString().Trim());
                    detalle.NumeroDesglose = int.Parse(row["Mes"].ToString().Trim());
                    detalle.Valor = (row["Valor"] == null ? "" : row["Valor"].ToString());
                    detalle.Comentario = (row["Cometario"] == null ? "" : row["Cometario"].ToString());

                    respuesta.DetalleRegistroIndicador.Add(detalle);
                    //contador++;
                    }
                }

                respuestas.Add(respuesta);
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.RegistroIndicador_IncopatibleExcel;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuestas;
        }

        /// <summary>
        /// Lectura del excel formato xlsx 
        /// </summary>
        /// <param name="xRutaExcel"></param>
        /// <returns></returns>
        private DataTable lLeerExcelXLSX(Stream file)
        {
            try
            {
                int contadortest = 0;
                var book = new XLWorkbook(file);
                var sheet = book.Worksheet("Coordenadas_de_Lectura");
                var dtData = new DataTable();
                dtData.TableName = "Datos";
                #region Columnas
                dtData.Columns.Add("IdSolicitud"); //1
                dtData.Columns.Add("idConstructorCriterioDetalleAgrupacion"); //2 
                dtData.Columns.Add("IdIndicador"); //3
                dtData.Columns.Add("idTipoValor"); //4
                dtData.Columns.Add("Valor"); //5
                dtData.Columns.Add("Coordenada"); //6
                dtData.Columns.Add("Mes"); //7
                dtData.Columns.Add("Anno"); //8
                dtData.Columns.Add("Cometario"); //9
                dtData.Columns.Add("IdProvincia"); //10
                dtData.Columns.Add("IdCanton"); //11
                dtData.Columns.Add("IdDistrito"); //12
                dtData.Columns.Add("IdGenero"); //13
                dtData.Columns.Add("IdSolicitudConstructor"); //14
                dtData.Columns.Add("Observacion"); //15
                dtData.Columns.Add("IdCriterio"); //11//Added by Kevin
                #endregion
                var range = sheet.RangeUsed();
                var colCount = range.ColumnCount();
                var contador = 0;
                foreach (var row in range.RowsUsed())
                {
                    if (contador == 273)
                    { }
                    if (contador > 0)
                    {
                        object[] rowData = new object[colCount];
                        Int32 i = 0;
                        row.Cells().ForEach(c => rowData[i++] = c.Value);

                        //2 líneas agregadas por kevin. Para obtener el valor de la observación digitada por el operador.
                        var sheetIndicadores = book.Worksheet("Indicadores");
                       
                        rowData[14] = sheetIndicadores.Cell(rowData[14].ToString()).Value;                      

                        dtData.Rows.Add(rowData);
                    }
                    contador++;
                }
                dtData.DefaultView.Sort = "IdSolicitudConstructor asc";
                return dtData;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.RegistroIndicador_IncopatibleExcel;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
        }
        /// <summary>
        /// Lectura de xls
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        /// 

        private DataTable lLeerExcelXLSXOLD(Stream file)
        {
            try
            {
                int contadortest = 0;
                var book = new XLWorkbook(file);
                var sheet = book.Worksheet("Coordenadas_de_Lectura");
                var dtData = new DataTable();
                dtData.TableName = "Datos";
                #region Columnas
                dtData.Columns.Add("IdSolicitud"); //1
                dtData.Columns.Add("idConstructorCriterioDetalleAgrupacion"); //2 
                dtData.Columns.Add("IdIndicador"); //3
                dtData.Columns.Add("idTipoValor"); //4
                dtData.Columns.Add("Valor"); //5
                dtData.Columns.Add("Coordenada"); //6
                dtData.Columns.Add("Mes"); //7
                dtData.Columns.Add("Anno"); //8
                dtData.Columns.Add("Cometario"); //9
                dtData.Columns.Add("IdProvincia"); //10
                dtData.Columns.Add("IdCanton"); //11
                dtData.Columns.Add("IdGenero"); //12
                dtData.Columns.Add("IdSolicitudConstructor"); //13
                dtData.Columns.Add("Observacion"); //14 //Added by Kevin
                dtData.Columns.Add("IdDistrito"); //14 //Added by Kevin
                #endregion
                var range = sheet.RangeUsed();
                var colCount = range.ColumnCount();
                var contador = 0;
                foreach (var row in range.RowsUsed())
                {
                    if (contador == 273)
                    { }
                    if (contador > 0)
                    {
                        object[] rowData = new object[colCount];
                        Int32 i = 0;
                        row.Cells().ForEach(c => rowData[i++] = c.Value);

                        //2 líneas agregadas por kevin. Para obtener el valor de la observación digitada por el operador.
                        var sheetIndicadores = book.Worksheet("Indicadores");
                        rowData[13] = sheetIndicadores.Cell(rowData[13].ToString()).Value;
                        rowData[13]="";
                        dtData.Rows.Add(rowData);
                    }
                    contador++;
                }
                dtData.DefaultView.Sort = "IdSolicitudConstructor asc";
                return dtData;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.RegistroIndicador_IncopatibleExcel;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
        }
        private DataTable lLeerExcelXLS(Stream file, String path)
        {

            try
            {
                //FileStream nFile=new FileStream(path, FileMode.Open, FileAccess.Read);
                HSSFWorkbook hssfwb = new HSSFWorkbook(file);
                ISheet sheet = hssfwb.GetSheet("Coordenadas_de_Lectura");
                
                var dtData = new DataTable();
                dtData.TableName = "Datos";
                #region Columnas
                dtData.Columns.Add("IdSolicitud"); //1
                dtData.Columns.Add("idConstructorCriterioDetalleAgrupacion"); //2 
                dtData.Columns.Add("IdIndicador"); //3
                dtData.Columns.Add("idTipoValor"); //4
                dtData.Columns.Add("Valor"); //5
                dtData.Columns.Add("Coordenada"); //6
                dtData.Columns.Add("Mes"); //7
                dtData.Columns.Add("Anno"); //8
                dtData.Columns.Add("Cometario"); //9
                dtData.Columns.Add("IdProvincia"); //10
                dtData.Columns.Add("IdCanton"); //11
                dtData.Columns.Add("IdGenero"); //12
                dtData.Columns.Add("IdSolicitudConstructor"); //13
                #endregion


                var colCount = 13;
                var contador = 0;

                for (int row = 0; row <= sheet.LastRowNum; row++)
                {
                    if (sheet.GetRow(row) != null) //null is when the row only contains empty cells 
                    {
                        if (contador > 0)
                        {
                            object[] rowData = new object[colCount];
                            Int32 i = 0;
                            
                            var a = sheet.GetRow(row).GetCell(0).StringCellValue;
                            var x = sheet.GetRow(row).Cells;
                            x.ForEach(c => rowData[i++] = c.ToString());
                            dtData.Rows.Add(rowData);
                        }
                        contador++;
                    }
                }
                dtData.DefaultView.Sort = "IdSolicitudConstructor asc";
                return dtData;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.RegistroIndicador_IncopatibleExcel;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
        }



        #endregion

        #region modificacionIndicador

         /// <summary>
        /// Obtiene el valor registrado
        /// </summary>
        /// <param name="poIdSolicitudConstructor"></param>
        /// <param name="poIdMes"></param>
        /// <param name="poIdNivelDetalle"></param>
        /// <param name="poValorNiveldDetalle"></param>
        /// <returns></returns>
        public Respuesta<DetalleRegistroIndicador> gObtenerDetalleRegistroIndicador(Guid poIdSolicitudConstructor,Guid poDetalleAgrupacion, int poIdMes, int poIdNivelDetalle, int poValorNiveldDetalle, int poIdAnno)
        {
            Respuesta<DetalleRegistroIndicador> objRespuesta = new Respuesta<DetalleRegistroIndicador>();
            try
            {
                objRespuesta = refRegistroIndicadorInternoAD.gObtenerDetalleRegistroIndicador(poIdSolicitudConstructor,poDetalleAgrupacion, poIdMes, poIdNivelDetalle, poValorNiveldDetalle, poIdAnno);

            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(ex.Message, objRespuesta.objObjeto);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
            return objRespuesta;
        
        }




        #endregion

        public Respuesta<List< pa_ListadoModificacionMasiva_Result>> gListadoModificacionMasiva(string annos, string operador, string servicio, string indicador)
        {
            annos = annos.Replace("'", "");
            annos = annos.Replace(",", "','");
            int estado = 4;
            return refRegistroIndicadorInternoAD.gListadoModificacionMasiva(annos,operador, servicio, estado,indicador);
        }

        public Respuesta<string> ModificarDetalleRegistroIndicadorMasivo(string nombreArchivo, Stream archivo, int idUsuario)
        {
            var objRespuesta = new Respuesta<string>();
            int numeroFilaRecorrido = 0;
            try
            {
                using (var package = new ExcelPackage(archivo))
                {
                    // get the first worksheet in the workbook
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    int idCol = 1;
                    int anterior = 17;
                    int valorCol = 18; 
                    for (int row = 2; worksheet.Cells[row, idCol].Value != null; row++)
                    {
                        numeroFilaRecorrido = row;
                        Guid idDetalleRegistro = Guid.Parse(worksheet.Cells[row, idCol].Value.ToString());
                        string valor = worksheet.Cells[row, valorCol].Value == null ? string.Empty : worksheet.Cells[row, valorCol].Value.ToString();
                        string valorAnte = worksheet.Cells[row, anterior].Value.ToString();
                        string justificacion = string.Format("{0} {1}", "Modificación másiva, plantilla", nombreArchivo);
                        BitacoraIndicador bitacora = new BitacoraIndicador();
                        bitacora.IdUsuario = idUsuario;
                        bitacora.IdDetalleRegistroIndicador = idDetalleRegistro;
                        bitacora.ValorNuevo = valor==string.Empty?"Vacio":valor;
                        bitacora.ValorAnterior = refRegistroIndicadorInternoAD.gObtenerDetalleRegistroIndicador(idDetalleRegistro).objObjeto.Valor;
                        bitacora.Justificacion = justificacion;
                        
                        
                        var resultado = gModificarRegistroIndicadorMasivo(idDetalleRegistro, valor, bitacora);
                        

                    }

                }
                objRespuesta.objObjeto = string.Format("<div class='alert alert-success' role='alert'>Se modificaron {0} registros</div>", numeroFilaRecorrido - 1);
            }
            catch (Exception ex)
            {

                objRespuesta.objObjeto = string.Format("<div class='alert alert-danger' role='alert'>se genero un error en la fila {0}, {1} </div>", numeroFilaRecorrido,ex.Message);
                objRespuesta.toError(ex.Message, objRespuesta.objObjeto);
              
            }

            return objRespuesta;
           
        }




        public Byte[] getBrochure(string path)
        {

            string[] filePaths = Directory.GetFiles(path);

            return File.ReadAllBytes(filePaths[0]);

        }

        public string getNombreArchivoBrochure(string path)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            string filePaths = Directory.GetFiles(path)[0];

            string[] nombreArchivoCsv = filePaths.Split(new char[] { '\\' });

            return nombreArchivoCsv[nombreArchivoCsv.Length - 1];

        }

        /// <summary>
        /// <autor>Michael Hernández Cordero</autor>
        /// <fecha>19/03/2019</fecha>
        /// <desc>Obtener la fechas por solicitud Indicador</desc>
        /// </summary>
        /// <param name="poSolicitudIndicador"></param>
        /// <returns></returns>


        public Respuesta<List<CMes>> lDetalleFechasSolicitudIndicador(Guid poSolicitudIndicador)
        {
            var objRespuesta = new Respuesta<List<CMes>>();
            try
            {
                objRespuesta = refRegistroIndicadorInternoAD.lDetalleFechasSolicitudIndicador(poSolicitudIndicador);
                objRespuesta.objObjeto.OrderBy(x => x.anno).ThenBy(y => y.idMes).ToList();
            }
            catch (Exception ex)
            {

                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(ex.Message, objRespuesta.objObjeto);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }



    }
}
