using GB.SUTEL.DAL.Mantenimientos;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SUTEL.Resources;
using System.IO;
using ClosedXML.Excel;
using System.Data;
using GB.SUTEL.BL.Seguridad;

namespace GB.SUTEL.BL.Mantenimientos
{
    public class DetalleAgrupacionBL : LocalContextualizer
    {

        #region atributos
        DetalleAgrupacionAD detalleAgrupacionAD;
        OperadorDA refOperadorDA;
        List<DetalleAgrupacion> listaDetalleAgrupacion;
        DetalleAgrupacion detalleAgru;
        AgrupacionBL agrupacionBL;
        OperadorBL operadorBL;
        #endregion

        #region Constructor
        public DetalleAgrupacionBL(ApplicationContext appContext)
            : base(appContext)
        {

            detalleAgrupacionAD = new DetalleAgrupacionAD(appContext);
            refOperadorDA = new OperadorDA(appContext);
            agrupacionBL = new AgrupacionBL(appContext);
            operadorBL = new OperadorBL(appContext);
        }
        #endregion

        #region metodos 
        /// <summary>
        /// Agregar detalle agrupación
        /// </summary>
        /// <param name="poAgrupacion"></param>
        /// <returns></returns>
        public Respuesta<DetalleAgrupacion> gAgregar(DetalleAgrupacion poDetalleAgrupacion)
        {
            Respuesta<DetalleAgrupacion> respuesta = new Respuesta<DetalleAgrupacion>();
            //se limpian espacios al inicio y final
            object aux = (object)poDetalleAgrupacion;
            Utilidades.LimpiarEspacios(ref aux);

            try
            {
                //se verifica si el registro existe
                if (detalleAgrupacionAD.gVerificarExistencia(poDetalleAgrupacion).objObjeto == null)// detenerlo aqui 
                {
                    respuesta = detalleAgrupacionAD.gAgregar(poDetalleAgrupacion);//
                    if (respuesta.blnIndicadorTransaccion)
                    {
                        respuesta.strMensaje = Mensajes.ExitoInsertar;
                    }
                }
                else
                {
                    respuesta.blnIndicadorTransaccion = false;
                    respuesta.strMensaje = Mensajes.RegistroDuplicado;
                }
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }

        /// <summary>
        /// Edita un detalle  agrupación
        /// </summary>
        /// <param name="poAgrupacion"></param>
        /// <returns></returns>
        public Respuesta<DetalleAgrupacion> gEditar(DetalleAgrupacion poDetalleAgrupacion)
        {
            Respuesta<DetalleAgrupacion> respuesta = new Respuesta<DetalleAgrupacion>();
            try
            {
                //se limpian espacios al inicio y final
                object aux = (object)poDetalleAgrupacion;
                Utilidades.LimpiarEspacios(ref aux);

                //se verifica si el registro existe
                if (detalleAgrupacionAD.gVerificarExistencia(poDetalleAgrupacion) != null)
                {
                    respuesta = detalleAgrupacionAD.gModificar(poDetalleAgrupacion);
                    if (respuesta.blnIndicadorTransaccion)
                    {
                        respuesta.strMensaje = Mensajes.ExitoEditar;
                    }
                }
                else
                {
                    respuesta.blnIndicadorTransaccion = false;
                    respuesta.strMensaje = Mensajes.RegistroDuplicado;
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, poDetalleAgrupacion);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

        }

        /// <summary>
        /// Elimina logicamente la DetalleAgrupacion
        /// </summary>
        /// <param name="poAgrupacion"></param>
        /// <returns></returns>
        public Respuesta<DetalleAgrupacion> gEliminar(int poIdDetalleAgrupacion)
        {
            Respuesta<DetalleAgrupacion> respuesta = new Respuesta<DetalleAgrupacion>();

            try
            {

                respuesta = detalleAgrupacionAD.gEliminar(poIdDetalleAgrupacion);
                if (respuesta.blnIndicadorTransaccion)
                {
                    respuesta.strMensaje = Mensajes.ExitoEliminar;
                }
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, respuesta.objObjeto);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }

        /// <summary>
        /// Obtiene los detalles agrupaciones
        /// </summary>
        /// <returns></returns>
        public Respuesta<List<DetalleAgrupacion>> gListar()
        {

            Respuesta<List<DetalleAgrupacion>> respuesta = new Respuesta<List<DetalleAgrupacion>>();
            try
            {
                respuesta = detalleAgrupacionAD.gListar();
                //foreach (DetalleAgrupacion item in respuesta.objObjeto)
                //{
                //    item.ConstructorCriterioDetalleAgrupacion.Clear();
                //    item.Operador = null;
                //    item.Agrupacion = null;

                //}

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }

        public Respuesta<DetalleAgrupacion> gConsultar(int poIdDetalleAgrupacion)
        {

            Respuesta<DetalleAgrupacion> respuesta = new Respuesta<DetalleAgrupacion>();
            try
            {
                respuesta = detalleAgrupacionAD.gConsultar(poIdDetalleAgrupacion);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }




        #endregion


        public Respuesta<List<DetalleAgrupacion>> gListarPorFiltros(string poOperador, string poAgrupacion, string poDescDetalle)
        {
            Respuesta<List<DetalleAgrupacion>> respuesta = new Respuesta<List<DetalleAgrupacion>>();
            try
            {
                OperadorBL operadorBL = new OperadorBL(this.AppContext);


                respuesta = detalleAgrupacionAD.gListarPorFiltros(poOperador, poAgrupacion, poDescDetalle);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }

        /// <summary>
        /// Obtiene los detalles agrupaciones
        /// </summary>
        /// <returns></returns>
        public Respuesta<List<DetalleAgrupacion>> gObtenerDetallesAgrupacionesPorOperador(String psIDOperador)
        {
            Respuesta<List<DetalleAgrupacion>> respuesta = new Respuesta<List<DetalleAgrupacion>>();
            try
            {
                respuesta = detalleAgrupacionAD.gObtenerDetallesAgrupacionesPorOperador(psIDOperador);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }

        /// <summary>
        /// Filtro de detalle Agrupacion
        /// </summary>
        /// <param name="psIDOperador"></param>
        /// <param name="psNombreAgrupacion"></param>
        /// <param name="psNombreDetalleAgrupacion"></param>
        /// <returns></returns>
        public Respuesta<List<DetalleAgrupacion>> gFiltrarDetalleAgrupacion(String psIDOperador, String psNombreAgrupacion, String psNombreDetalleAgrupacion)
        {
            Respuesta<List<DetalleAgrupacion>> respuesta = new Respuesta<List<DetalleAgrupacion>>();
            try
            {
                respuesta = detalleAgrupacionAD.gFiltrarDetalleAgrupacion(psIDOperador, psNombreAgrupacion, psNombreDetalleAgrupacion);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }

        /// <summary>
        /// Crea el detalle de agrupación que viene desde excel
        /// </summary>
        /// <param name="nombreArchivo">nombre del archivo</param>
        /// <param name="archivo">archivo a guardar</param>
        /// <returns>Lista de detalle agrupación</returns>
        public Respuesta<List<DetalleAgrupacion>> gCrearDetalleDesdeExcel(String nombreArchivo, Stream archivo)
        {

            Respuesta<List<DetalleAgrupacion>> objRespuesta = new Respuesta<List<DetalleAgrupacion>>();
            Respuesta<DetalleAgrupacion> respuestaDetalle = new Respuesta<DetalleAgrupacion>();
            Respuesta<string> respuestaLista = new Respuesta<string>();

            try
            {
                if (nombreArchivo.ToUpper().Contains("XLSX"))
                {
                    var book = new XLWorkbook(archivo);
                    var sheet = book.Worksheet(1);
                    var dtData = new DataTable();
                    dtData.TableName = "Datos";
                    #region Columnas
                    dtData.Columns.Add("IdAgrupacion"); //1
                    dtData.Columns.Add("IdOperador"); //2 
                    dtData.Columns.Add("Descripcion"); //3
                    #endregion
                    var range = sheet.RangeUsed();
                    var colCount = 3;
                    var contador = 0;

                    if (range.ColumnCount() == colCount)
                    {
                        foreach (var row in range.RowsUsed())
                        {
                            if (contador > 0)
                            {
                                object[] rowData = new object[colCount];
                                Int32 i = 0;
                                row.Cells().ForEach(c => rowData[i++] = c.Value);

                                dtData.Rows.Add(rowData);
                            }
                            contador++;
                        }

                        respuestaLista = listaDataTable(dtData);

                        if (!respuestaLista.blnIndicadorTransaccion)
                        {
                            objRespuesta.blnIndicadorTransaccion = false;
                            objRespuesta.strMensaje = respuestaLista.strMensaje;
                        }

                        if (listaDetalleAgrupacion.Count >= 1)
                        {
                            //ENVIAR A GUARDAR TODA LA LISTA
                            respuestaDetalle = detalleAgrupacionAD.gListaAgregar(listaDetalleAgrupacion);
                            if (respuestaDetalle.blnIndicadorTransaccion)
                            {
                                objRespuesta.objObjeto = listaDetalleAgrupacion;
                            }
                        }

                    }
                    else
                    {
                        objRespuesta.blnIndicadorTransaccion = false;
                        objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.DetalleAgrupacionExcelColumnas;
                    }
                }
                else
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.DetalleAgrupacionExcelExtension;
                }

            }
            catch (Exception ex)
            {
                objRespuesta.blnIndicadorTransaccion = false;
                objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.DetalleAgrupacionExcelVacio;
            }

            return objRespuesta;

        }

        /// <summary>
        /// Pasa de datatable a lista, y realiza las validaciones correspondientes
        /// </summary>
        /// <param name="dtData">datatable con datos</param>
        /// <returns></returns>
        private Respuesta<string> listaDataTable(DataTable dtData)
        {
            int agrupacionResult;
            Respuesta<string> objRespuesta = new Respuesta<string>();
            string mensaje = "";
            string msgDuplicado = "";
            string msgAgrupacion = "";
            string msgOperador = "";
            string duplicado = "";
            string agrupacionExiste = "";
            string operadorExiste = "";
            string agrupacionFormato = "";


            try
            {
                var listAgrupacion = agrupacionBL.gObtenerAgrupaciones().objObjeto;
                var listOperador = operadorBL.ConsultarTodosParaDetalleAgrupacion().objObjeto;
                listaDetalleAgrupacion = new List<DetalleAgrupacion>();

                foreach (DataRow row in dtData.Rows)
                {
                    string operador = "";
                    detalleAgru = new DetalleAgrupacion();

                    if (row["IdAgrupacion"] != null && !String.IsNullOrEmpty(row["IdAgrupacion"].ToString().Trim()) &&
                        row["IdOperador"] != null && !String.IsNullOrEmpty(row["IdOperador"].ToString().Trim()) &&
                        row["Descripcion"] != null && !String.IsNullOrEmpty(row["Descripcion"].ToString().Trim()))
                    {
                        bool agrupacionValidar = int.TryParse(row["IdAgrupacion"].ToString(), out agrupacionResult);
                        if (agrupacionValidar)
                        {
                            bool existeAgru = listAgrupacion.Any(x => x.IdAgrupacion == agrupacionResult);

                            if (existeAgru)
                            {
                                operador = row["IdOperador"].ToString().Length > 20 ? row["IdOperador"].ToString().Substring(0, 20) : row["IdOperador"].ToString();
                                bool existeOpe = listOperador.Any(x => x.IdOperador == operador);

                                if (existeOpe)
                                {
                                    detalleAgru.IdAgrupacion = agrupacionResult;

                                    detalleAgru.IdOperador = operador;

                                    detalleAgru.DescDetalleAgrupacion = row["Descripcion"].ToString().Length > 250 ? row["Descripcion"].ToString().Substring(0, 250).Trim() : row["Descripcion"].ToString().Trim();

                                    //se verifica si el registro existe
                                    if (detalleAgrupacionAD.gVerificarExistencia(detalleAgru).objObjeto == null)
                                    {
                                        List<DetalleAgrupacion> existeDuplicadosLista = listaDetalleAgrupacion.Where(c => c.IdAgrupacion == detalleAgru.IdAgrupacion &&
                                                                                                                          c.IdOperador == detalleAgru.IdOperador &&
                                                                                                                          c.DescDetalleAgrupacion == detalleAgru.DescDetalleAgrupacion).ToList();
                                        
                                        if(existeDuplicadosLista.Count() == 0)
                                        {
                                            listaDetalleAgrupacion.Add(detalleAgru);
                                        }
                                    }
                                    else
                                    {
                                        objRespuesta.blnIndicadorTransaccion = false;
                                        duplicado += detalleAgru.IdAgrupacion + " " + detalleAgru.IdOperador + " " + detalleAgru.DescDetalleAgrupacion + "<br/>";
                                    }
                                }
                                else
                                {
                                    objRespuesta.blnIndicadorTransaccion = false;
                                    operadorExiste += ((operadorExiste != "") ? ", " : "") + operador ;
                                }

                            }
                            else
                            {
                                objRespuesta.blnIndicadorTransaccion = false;
                                agrupacionExiste += ((agrupacionExiste != "") ? ", " : "") + agrupacionResult ;
                            }
                        }
                        else
                        {
                            objRespuesta.blnIndicadorTransaccion = false;
                            agrupacionFormato += ((agrupacionFormato != "") ? ", " : "") + row["IdAgrupacion"].ToString();
                        }
                    }
                }

                if (agrupacionFormato != "")
                {
                    mensaje = string.Format("{0}{1}<br/>", ErrorTemplate.DetalleAgrupacionFormatoAgrupacion, agrupacionFormato);
                }

                if (agrupacionExiste != "")
                {
                    msgAgrupacion = string.Format("{0}{1}<br/>", ErrorTemplate.DetalleAgrupacionAgru,agrupacionExiste);
                }

                if (operadorExiste != "")
                {
                    msgOperador = string.Format("{0}{1}<br/>", ErrorTemplate.DetalleAgrupacionOpe, operadorExiste);
                }

                if (duplicado != "")
                {
                    msgDuplicado = string.Format("{0}<br/>{1}", ErrorTemplate.DetalleAgrupacionDuplicado, duplicado);
                }

                objRespuesta.strMensaje = string.Format("<p>{0}{1}{2}{3}</p>", mensaje, msgAgrupacion, msgOperador, msgDuplicado);
            }
            catch (Exception ex)
            {
                objRespuesta.blnIndicadorTransaccion = false;
                objRespuesta.strMensaje = ex.Message;
            }

            return objRespuesta;
        }

       
        #region clonarDetalleAgrupacion
        /// <summary>
        /// <method>NewMethod</method>
        /// Clonar de Agrupacion por operador 
        /// con el sp de Arbol, en este metdo se
        /// valida que los detalles existan para cada operador
        /// solomente si cuentas con todas las ramas del arbol 
        /// original sera enviado al sp para clonar
        /// </summary>
        /// <param name="poDetalleOperadorClonar"></param>
        /// <param name="poListaOperador"></param>
        /// <param name="IdoperadorO"></param>
        /// <param name="IdConstructor"></param>
        /// <param name="IdCriterio"></param>
        /// <returns></returns>
        public Respuesta<List<ConstructorCriterioDetalleAgrupacion>> gClonarDetalleAgrupacionOperadores(List<ConstructorCriterioDetalleAgrupacion> poDetalleOperadorClonar, List<String> poListaOperador, String IdoperadorO, String IdConstructor, String IdCriterio)
        {
            Respuesta<List<ConstructorCriterioDetalleAgrupacion>> resultado = new Respuesta<List<ConstructorCriterioDetalleAgrupacion>>();
            Respuesta<Operador> operador = new Respuesta<Operador>();//lo ocupo
            Respuesta<DetalleAgrupacion> detalleHijo = new Respuesta<DetalleAgrupacion>();//lo ocupo
            List<String> listaOperadoresNoConcide = new List<string>();
            List<String> listaDetallesNoConciden = new List<string>();
            List<String> listaOperadoresClonados = new List<string>();
            bool noCoincideDetalleAgrupacion = false;// lo ocupo
            bool respuesta = false;
            List<CDetalleAgrupacionEquivalencia> detallesEquivalencias = new List<CDetalleAgrupacionEquivalencia>();
            CDetalleAgrupacionEquivalencia equivalencia = new CDetalleAgrupacionEquivalencia();
            resultado.objObjeto = new List<ConstructorCriterioDetalleAgrupacion>();
            int ContadorDetallesdeAgrupacionCorrectos = 0;

            try
            {
                foreach (String itemIdOperador in poListaOperador)
                {
                    noCoincideDetalleAgrupacion = false;// lo ocupo


                    detallesEquivalencias = new List<CDetalleAgrupacionEquivalencia>();
                    string Detalle = "";
                    foreach (ConstructorCriterioDetalleAgrupacion itemDetalle in poDetalleOperadorClonar)
                    {


                        detalleHijo = new Respuesta<DetalleAgrupacion>();// lo ocupo


                        string[] agrupaciondetalle = itemDetalle.DetalleAgrupacion.DescDetalleAgrupacion.Split('/');//lo ocupo
                        Detalle = itemDetalle.DetalleAgrupacion.DescDetalleAgrupacion;

                        detalleHijo = detalleAgrupacionAD.gConsultarAgrupacionClonar(agrupaciondetalle[0], agrupaciondetalle[1], itemIdOperador);//lo unico que ocupo                       
                        bool DescHexa = detalleAgrupacionAD.verificarDescHexadecimal(itemIdOperador, IdoperadorO, agrupaciondetalle[0], agrupaciondetalle[1]);
                        if (detalleHijo.objObjeto != null && DescHexa == true)
                        {
                            ContadorDetallesdeAgrupacionCorrectos = ContadorDetallesdeAgrupacionCorrectos + 1;
                        }
                        else
                        {
                            noCoincideDetalleAgrupacion = true;// lo ocupo
                            listaDetallesNoConciden.Add("idOperador: " + itemIdOperador + "|Detalle: " + Detalle);
                            break;
                        }
                    }
                    if (noCoincideDetalleAgrupacion)
                    {
                        listaOperadoresNoConcide.Add(itemIdOperador);
                        ContadorDetallesdeAgrupacionCorrectos = 0;
                    }

                    if (ContadorDetallesdeAgrupacionCorrectos == poDetalleOperadorClonar.Count)
                    {
                        respuesta = detalleAgrupacionAD.gClonarDetalleAgrupacionConstructor(IdConstructor, IdCriterio, IdoperadorO, itemIdOperador);
                        if (respuesta == true)
                        {
                            listaOperadoresClonados.Add(itemIdOperador);
                        }
                        else
                        {
                            listaDetallesNoConciden.Add("Falló la clonación: " + itemIdOperador);
                        }

                        ContadorDetallesdeAgrupacionCorrectos = 0;
                    }
                }
                //ocupo todo este gran if para lo mensajes de error cuales conciden y cuales no 
                if (listaOperadoresNoConcide.Count > 0)
                {
                    resultado.blnIndicadorTransaccion = false;
                    //Mensaje cuando ningún operador coincide
                    if (listaOperadoresNoConcide.Count == poListaOperador.Count)
                    {
                        resultado.strMensaje = GB.SUTEL.Shared.ErrorTemplate.Constructor_ClonarErrorTodos;
                    }
                    else
                    {
                        //mensaje cuando algunos operadores no coincidieron
                        resultado.strMensaje = GB.SUTEL.Shared.ErrorTemplate.Constructor_ClonarErrorAlgunos;
                        foreach (String itemOperador in listaOperadoresNoConcide)
                        {
                            operador = new Respuesta<Operador>();
                            operador = refOperadorDA.gObtenerOperador(itemOperador);
                            resultado.strMensaje += " " + operador.objObjeto.NombreOperador + ",";

                        }
                        resultado.strMensaje = resultado.strMensaje.Remove(resultado.strMensaje.Length - 1);
                        resultado.strMensaje += "\n";
                    }
                }

                if (listaDetallesNoConciden.Count > 0)
                {
                    resultado.strMensaje += "</br><h5>Detalles No ligados</h5><ul>";
                    foreach (String itemOperador in listaDetallesNoConciden)
                    {
                        resultado.strMensaje += "<li>" + itemOperador + "</li>";

                    }
                    resultado.strMensaje += "</ul>";
                }

                if (listaOperadoresClonados.Count > 0)
                {
                    resultado.strMensaje += "</br><h5>Operadores Clonados</h5><ul>";
                    foreach (String itemOperador in listaOperadoresClonados)
                    {
                        operador = new Respuesta<Operador>();
                        operador = refOperadorDA.gObtenerOperador(itemOperador);
                        resultado.strMensaje += "<li>" + operador.objObjeto.NombreOperador + "</li>";
                    }
                    resultado.strMensaje += "</ul>";
                }

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return resultado;

        }

        /// <summary>
        /// Proceso de clonar los detalles de agrupación
        /// </summary>
        /// <param name="poDetalleOperadorClonar"></param>
        /// <param name="poListaOperador"></param>
        /// <returns></returns>
        public Respuesta<List<ConstructorCriterioDetalleAgrupacion>> gClonarDetalleAgrupacion(List<ConstructorCriterioDetalleAgrupacion> poDetalleOperadorClonar, List<String> poListaOperador)
        {
            Respuesta<List<ConstructorCriterioDetalleAgrupacion>> resultado = new Respuesta<List<ConstructorCriterioDetalleAgrupacion>>();
            Respuesta<Operador> operador = new Respuesta<Operador>();
            DetalleAgrupacion detallePadre = new DetalleAgrupacion();//lo ocupo
            Respuesta<DetalleAgrupacion> detalleHijo = new Respuesta<DetalleAgrupacion>();//lo ocupo
            List<String> listaOperadoresNoCoincide = new List<string>();
            bool noCoincideDetalleAgrupacion = false;// lo ocupo
            bool nodoPadreCreado = false;
            ConstructorCriterioDetalleAgrupacion nodoPadre = new ConstructorCriterioDetalleAgrupacion();
            ConstructorCriterioDetalleAgrupacion nodoHijo = new ConstructorCriterioDetalleAgrupacion();
            List<CDetalleAgrupacionEquivalencia> detallesEquivalencias = new List<CDetalleAgrupacionEquivalencia>();
            CDetalleAgrupacionEquivalencia equivalencia = new CDetalleAgrupacionEquivalencia();
            resultado.objObjeto = new List<ConstructorCriterioDetalleAgrupacion>();
            try
            {
                foreach (String itemIdOperador in poListaOperador)
                {
                    noCoincideDetalleAgrupacion = false;// lo ocupo
                    nodoPadreCreado = false;
                    nodoPadre = new ConstructorCriterioDetalleAgrupacion();
                    detallesEquivalencias = new List<CDetalleAgrupacionEquivalencia>();
                    foreach (ConstructorCriterioDetalleAgrupacion itemDetalle in poDetalleOperadorClonar)
                    {

                        detallePadre = new DetalleAgrupacion();// ocupo
                        detalleHijo = new Respuesta<DetalleAgrupacion>();// lo ocupo


                        string[] agrupaciondetalle = itemDetalle.DetalleAgrupacion.DescDetalleAgrupacion.Split('/');//lo ocupo
                        detalleHijo = detalleAgrupacionAD.gConsultarAgrupacionClonar(agrupaciondetalle[0], agrupaciondetalle[1], itemIdOperador);//lo unico que ocupo

                        if (detalleHijo.objObjeto != null)
                        {
                            if (nodoPadreCreado == false)
                            {
                                nodoPadreCreado = true;
                                nodoPadre.DetalleAgrupacion = new DetalleAgrupacion();
                                nodoPadre.DetalleAgrupacion.Operador = new Operador();
                                equivalencia = new CDetalleAgrupacionEquivalencia();
                                equivalencia.detalleAgrupacionAnterior = new DetalleAgrupacion();
                                equivalencia.detalleAgrupacionEquivalente = new DetalleAgrupacion();
                                nodoPadre.IdOperador = detalleHijo.objObjeto.Operador.IdOperador;
                                nodoPadre.DetalleAgrupacion.IdOperador = "0|0|" + detalleHijo.objObjeto.Operador.IdOperador;
                                nodoPadre.DetalleAgrupacion.Operador.NombreOperador = detalleHijo.objObjeto.Operador.NombreOperador;
                                nodoPadre.ConstructorCriterioDetalleAgrupacion1 = new List<ConstructorCriterioDetalleAgrupacion>();
                                equivalencia.detalleAgrupacionAnterior.IdAgrupacion = 0;
                                equivalencia.detalleAgrupacionAnterior.IdDetalleAgrupacion = 0;
                                equivalencia.detalleAgrupacionAnterior.IdOperador = itemDetalle.IdOperador;
                                equivalencia.detalleAgrupacionEquivalente.IdAgrupacion = 0;
                                equivalencia.detalleAgrupacionEquivalente.IdDetalleAgrupacion = 0;
                                equivalencia.detalleAgrupacionEquivalente.IdOperador = itemIdOperador;
                                detallesEquivalencias.Add(equivalencia);
                            }
                            equivalencia = new CDetalleAgrupacionEquivalencia();
                            equivalencia.detalleAgrupacionAnterior = new DetalleAgrupacion();
                            equivalencia.detalleAgrupacionEquivalente = new DetalleAgrupacion();
                            equivalencia.detalleAgrupacionAnterior.IdAgrupacion = itemDetalle.IdAgrupacion;
                            equivalencia.detalleAgrupacionAnterior.IdDetalleAgrupacion = itemDetalle.IdDetalleAgrupacion;
                            equivalencia.detalleAgrupacionAnterior.IdOperador = itemDetalle.IdOperador;
                            equivalencia.detalleAgrupacionEquivalente = detalleHijo.objObjeto;
                            detallesEquivalencias.Add(equivalencia);
                            nodoHijo = new ConstructorCriterioDetalleAgrupacion();
                            nodoHijo.DetalleAgrupacion = new DetalleAgrupacion();
                            nodoHijo.ConstructorCriterioDetalleAgrupacion2 = new ConstructorCriterioDetalleAgrupacion();
                            nodoHijo.ConstructorCriterioDetalleAgrupacion2.DetalleAgrupacion = new DetalleAgrupacion();
                            //nodo padre del nodo hijo
                            detallePadre = lObtenerDetalleAgrupacionEquivalente(itemDetalle.ConstructorCriterioDetalleAgrupacion2.IdDetalleAgrupacion, itemDetalle.ConstructorCriterioDetalleAgrupacion2.IdAgrupacion, detallesEquivalencias);
                            nodoHijo.ConstructorCriterioDetalleAgrupacion2.IdDetalleAgrupacion = detallePadre.IdDetalleAgrupacion;
                            nodoHijo.ConstructorCriterioDetalleAgrupacion2.IdAgrupacion = detallePadre.IdAgrupacion;
                            nodoHijo.ConstructorCriterioDetalleAgrupacion2.IdOperador = itemIdOperador;
                            nodoHijo.ConstructorCriterioDetalleAgrupacion2.DetalleAgrupacion.IdOperador = lObtenerEquivalenciaIDDetalleAgrupacion(itemDetalle.ConstructorCriterioDetalleAgrupacion2.DetalleAgrupacion.IdOperador, detallesEquivalencias);
                            nodoHijo.IdDetalleAgrupacion = detalleHijo.objObjeto.IdDetalleAgrupacion;
                            nodoHijo.IdAgrupacion = detalleHijo.objObjeto.IdAgrupacion;
                            nodoHijo.IdOperador = detalleHijo.objObjeto.IdOperador;
                            nodoHijo.UltimoNivel = itemDetalle.UltimoNivel;
                            nodoHijo.IdNivel = itemDetalle.IdNivel;
                            nodoHijo.Orden = itemDetalle.Orden;
                            nodoHijo.DetalleAgrupacion.IdOperador = lObtenerEquivalenciaIDDetalleAgrupacion(itemDetalle.DetalleAgrupacion.IdOperador, detallesEquivalencias);
                            nodoHijo.DetalleAgrupacion.DescDetalleAgrupacion = itemDetalle.DetalleAgrupacion.DescDetalleAgrupacion;
                            if (nodoHijo.UltimoNivel.Equals((byte)1))
                            {
                                if (itemDetalle.UsaReglaEstadisticaConNivelDetalle == (byte)1)
                                {

                                    nodoHijo.UsaReglaEstadisticaConNivelDetalle = itemDetalle.UsaReglaEstadisticaConNivelDetalle;
                                    nodoHijo.NivelDetalleReglaEstadistica = itemDetalle.NivelDetalleReglaEstadistica;


                                }
                                else
                                {
                                    nodoHijo.UsaReglaEstadisticaConNivelDetalle = 0;
                                    nodoHijo.NivelDetalleReglaEstadistica = null;

                                }

                                nodoHijo.Regla = new Regla();
                                nodoHijo.Regla.FechaCreacionRegla = DateTime.Now;
                                nodoHijo.Regla.ValorLimiteInferior = itemDetalle.Regla.ValorLimiteInferior;
                                nodoHijo.Regla.ValorLimiteSuperior = itemDetalle.Regla.ValorLimiteSuperior;
                                nodoHijo.IdTipoValor = itemDetalle.IdTipoValor;
                                nodoHijo.IdNivelDetalle = itemDetalle.IdNivelDetalle;

                                nodoHijo.UsaReglaEstadistica = itemDetalle.UsaReglaEstadistica;

                            }
                            nodoPadre.ConstructorCriterioDetalleAgrupacion1.Add(nodoHijo);
                        }
                        else
                        {
                            noCoincideDetalleAgrupacion = true;// lo ocupo
                            break;
                        }
                    }
                    if (noCoincideDetalleAgrupacion)
                    {
                        listaOperadoresNoCoincide.Add(itemIdOperador);
                    }
                    else
                    {
                        resultado.objObjeto.Add(nodoPadre);
                    }
                }
                //ocupo todo este gran if para lo mensajes de error cuales conciden y cuales no 
                if (listaOperadoresNoCoincide.Count > 0)
                {
                    resultado.blnIndicadorTransaccion = false;
                    //Mensaje cuando ningún operador coincide
                    if (listaOperadoresNoCoincide.Count == poListaOperador.Count)
                    {
                        resultado.strMensaje = GB.SUTEL.Shared.ErrorTemplate.Constructor_ClonarErrorTodos;
                    }
                    else
                    {
                        //mensaje cuando algunos operadores no coincidieron
                        resultado.strMensaje = GB.SUTEL.Shared.ErrorTemplate.Constructor_ClonarErrorAlgunos;
                        foreach (String itemOperador in listaOperadoresNoCoincide)
                        {
                            operador = new Respuesta<Operador>();
                            operador = refOperadorDA.gObtenerOperador(itemOperador);
                            resultado.strMensaje += " " + operador.objObjeto.NombreOperador + ",";

                        }
                        resultado.strMensaje = resultado.strMensaje.Remove(resultado.strMensaje.Length - 1);
                    }

                }
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return resultado;

        }

        /// <summary>
        /// Obtiene el detalle Agrupación equivalente
        /// </summary>
        /// <param name="psIdAgrupacion"></param>
        /// <param name="psIdDetalleAgrupacion"></param>
        /// <param name="equivalencias"></param>
        /// <returns></returns>
        private DetalleAgrupacion lObtenerDetalleAgrupacionEquivalente(int psIdDetalleAgrupacion, int psIdAgrupacion, List<CDetalleAgrupacionEquivalencia> equivalencias)
        {
            return equivalencias.Where(x => x.detalleAgrupacionAnterior.IdAgrupacion.Equals(psIdAgrupacion) && x.detalleAgrupacionAnterior.IdDetalleAgrupacion.Equals(psIdDetalleAgrupacion)).FirstOrDefault().detalleAgrupacionEquivalente;
        }
        /// <summary>
        /// Obtiene el id equivalente del otro operador
        /// </summary>
        /// <param name="psIdAnterior"></param>
        /// <param name="equivalencias"></param>
        /// <returns></returns>
        private String lObtenerEquivalenciaIDDetalleAgrupacion(String psIdAnterior, List<CDetalleAgrupacionEquivalencia> equivalencias)
        {
            String resultado = "";
            string[] ids = psIdAnterior.Split('|');
            DetalleAgrupacion detalleEquivalente = new DetalleAgrupacion();
            for (int i = 0; i < ids.Length; i = i + 3)
            {
                detalleEquivalente = lObtenerDetalleAgrupacionEquivalente(int.Parse(ids[i]), int.Parse(ids[i + 1]), equivalencias);
                resultado += detalleEquivalente.IdDetalleAgrupacion + "|" + detalleEquivalente.IdAgrupacion + "|" + detalleEquivalente.IdOperador + "|";
            }
            if (!resultado.Equals(""))
            {
                resultado = resultado.Remove(resultado.Length - 1);
            }
            return resultado;
        }

        #endregion
    }



}
