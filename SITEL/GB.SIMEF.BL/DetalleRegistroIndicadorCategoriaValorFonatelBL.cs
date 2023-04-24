using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL
{
    public class DetalleRegistroIndicadorCategoriaValorFonatelBL : IMetodos<DetalleRegistroIndicadorCategoriaValorFonatel>
    {
        readonly string modulo = "";
        readonly string user = "";
        private readonly DetalleRegistroIndicadorFonatelDAL detalleRegistroIndicadorFonatelDAL;
        private readonly CategoriasDesagregacionDAL categoriasDesagregacionDAL;
        RespuestaConsulta<List<DetalleRegistroIndicadorCategoriaValorFonatel>> ResultadoConsulta;
        RespuestaConsulta<List<DetalleRegistroIndicadorVariableValorFonatel>> ResultadoConsultaVariable;

        public DetalleRegistroIndicadorCategoriaValorFonatelBL(string modulo, string user)
        {
            this.modulo = modulo;
            this.user = user;
            this.detalleRegistroIndicadorFonatelDAL = new DetalleRegistroIndicadorFonatelDAL();
            this.categoriasDesagregacionDAL = new CategoriasDesagregacionDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<DetalleRegistroIndicadorCategoriaValorFonatel>>();
            this.ResultadoConsultaVariable = new RespuestaConsulta<List<DetalleRegistroIndicadorVariableValorFonatel>>();
        }

        public RespuestaConsulta<List<DetalleRegistroIndicadorCategoriaValorFonatel>> ActualizarElemento(DetalleRegistroIndicadorCategoriaValorFonatel objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleRegistroIndicadorCategoriaValorFonatel>> CambioEstado(DetalleRegistroIndicadorCategoriaValorFonatel objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleRegistroIndicadorCategoriaValorFonatel>> ClonarDatos(DetalleRegistroIndicadorCategoriaValorFonatel objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleRegistroIndicadorCategoriaValorFonatel>> EliminarElemento(DetalleRegistroIndicadorCategoriaValorFonatel objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleRegistroIndicadorCategoriaValorFonatel>> InsertarDatos(DetalleRegistroIndicadorFonatel objeto)
        {
            try
            {
                //Se revisa si la lista contiene informacion
                if (objeto.DetalleRegistroIndicadorVariableValorFonatel.Count > 0)
                {
                    //Se crea datatable con la informacion de la lista
                    var dt = new DataTable();
                    dt.Columns.Add("IdSolicitud", typeof(int));
                    dt.Columns.Add("idFormularioWeb", typeof(int));
                    dt.Columns.Add("IdIndicador", typeof(int));
                    dt.Columns.Add("idVariable", typeof(int));
                    dt.Columns.Add("NumeroFila", typeof(int));
                    dt.Columns.Add("Valor", typeof(string));

                    foreach (var item in objeto.DetalleRegistroIndicadorVariableValorFonatel)
                    {
                        if (!string.IsNullOrEmpty(item.Solicitudid))
                        {
                            int temp = 0;
                            int.TryParse(Utilidades.Desencriptar(item.Solicitudid), out temp);
                            item.IdSolicitud = temp;
                        }

                        if (!string.IsNullOrEmpty(item.FormularioId))
                        {
                            int temp = 0;
                            int.TryParse(Utilidades.Desencriptar(item.FormularioId), out temp);
                            item.idFormularioWeb = temp;
                        }

                        if (!string.IsNullOrEmpty(item.IndicadorId))
                        {
                            int temp = 0;
                            int.TryParse(Utilidades.Desencriptar(item.IndicadorId), out temp);
                            item.IdIndicador = temp;
                        }

                        dt.Rows.Add(item.IdSolicitud, item.idFormularioWeb, item.IdIndicador, item.IdVariable, item.NumeroFila, item.Valor);
                    }

                    //Se elimina los detalles valores para insertar los nuevos
                    DetalleRegistroIndicadorVariableValorFonatel eliminar = objeto.DetalleRegistroIndicadorVariableValorFonatel[0];
                    eliminar.IdVariable = 0;

                    //Se eliminan unicamente los indicadores actalizados
                    var indicadoresUnicos = objeto.DetalleRegistroIndicadorVariableValorFonatel.GroupBy(x => x.IdIndicador).Select(g=>g.First()).ToList();
                    foreach (var ind in indicadoresUnicos) {
                        eliminar.IdIndicador = ind.IdIndicador;
                        detalleRegistroIndicadorFonatelDAL.EliminarDetalleRegistroIndicadorVariableValorFonatel(eliminar);
                    }

                    detalleRegistroIndicadorFonatelDAL.InsertarDetalleRegistroIndicadorVariableValorFonatel(dt);
                }

                //Se revisa si la lista contiene informacion
                if (objeto.DetalleRegistroIndicadorCategoriaValorFonatel.Count > 0)
                {
                    //Se crea datatable con la informacion de la lista
                    var dt = new DataTable();
                    dt.Columns.Add("IdSolicitud", typeof(int));
                    dt.Columns.Add("idFormularioWeb", typeof(int));
                    dt.Columns.Add("IdIndicador", typeof(int));
                    dt.Columns.Add("idCategoria", typeof(int));
                    dt.Columns.Add("NumeroFila", typeof(int));
                    dt.Columns.Add("Valor", typeof(string));

                    foreach (var item in objeto.DetalleRegistroIndicadorCategoriaValorFonatel)
                    {
                        if (!string.IsNullOrEmpty(item.Solicitudid))
                        {
                            int temp = 0;
                            int.TryParse(Utilidades.Desencriptar(item.Solicitudid), out temp);
                            item.IdSolicitud = temp;
                        }

                        if (!string.IsNullOrEmpty(item.FormularioId))
                        {
                            int temp = 0;
                            int.TryParse(Utilidades.Desencriptar(item.FormularioId), out temp);
                            item.idFormularioWeb = temp;
                        }

                        if (!string.IsNullOrEmpty(item.IndicadorId))
                        {
                            int temp = 0;
                            int.TryParse(Utilidades.Desencriptar(item.IndicadorId), out temp);
                            item.IdIndicador = temp;
                        }

                        dt.Rows.Add(item.IdSolicitud, item.idFormularioWeb, item.IdIndicador, item.idCategoria, item.NumeroFila, item.Valor);
                    }

                    //Se elimina los detalles valores para insertar los nuevos
                    DetalleRegistroIndicadorCategoriaValorFonatel eliminar = objeto.DetalleRegistroIndicadorCategoriaValorFonatel[0];
                    eliminar.idCategoria = 0;

                    var indicadoresUnicos = objeto.DetalleRegistroIndicadorVariableValorFonatel.GroupBy(x => x.IdIndicador).Select(g => g.First()).ToList();
                    foreach (var ind in indicadoresUnicos)
                    {
                        eliminar.IdIndicador = ind.IdIndicador;
                        detalleRegistroIndicadorFonatelDAL.EliminarDetalleRegistroIndicadorCategoriaValorFonatel(eliminar);
                    }

                    ResultadoConsulta.Clase = modulo;
                    ResultadoConsulta.Accion = (int)Accion.Crear;
                    ResultadoConsulta.Usuario = user;

                    ResultadoConsulta.objetoRespuesta = detalleRegistroIndicadorFonatelDAL.InsertarDetalleRegistroIndicadorCategoriaValorFonatel(dt);
                    ResultadoConsulta.CantidadRegistros = ResultadoConsulta.objetoRespuesta.Count();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == Errores.CantidadRegistros || ex.Message == Errores.CodigoRegistrado || ex.Message == Errores.DetalleRegistrado)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                }
                else
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                }
                ResultadoConsulta.MensajeError = ex.Message;
            }

            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<DetalleRegistroIndicadorCategoriaValorFonatel>> InsertarDatos(DetalleRegistroIndicadorCategoriaValorFonatel objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleRegistroIndicadorCategoriaValorFonatel>> ObtenerDatos(DetalleRegistroIndicadorCategoriaValorFonatel objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;
                if (!string.IsNullOrEmpty(objeto.FormularioId))
                {
                    int.TryParse(Utilidades.Desencriptar(objeto.FormularioId), out int temp);
                    objeto.idFormularioWeb = temp;
                }
                if (!string.IsNullOrEmpty(objeto.Solicitudid))
                {
                    int.TryParse(Utilidades.Desencriptar(objeto.Solicitudid), out int temp);
                    objeto.IdSolicitud = temp;
                }
                if (!string.IsNullOrEmpty(objeto.IndicadorId))
                {
                    int.TryParse(Utilidades.Desencriptar(objeto.IndicadorId), out int temp);
                    objeto.IdIndicador = temp;
                }
                var resul = detalleRegistroIndicadorFonatelDAL.ObtenerDetalleRegistroIndicadorCategoriaValorFonatel(objeto);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();

            }
            catch (Exception ex)
            {
                ResultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<DetalleRegistroIndicadorCategoriaValorFonatel>> ValidarDatos(DetalleRegistroIndicadorCategoriaValorFonatel objeto)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Autor: Francisco Vindas Ruiz
        /// Fecha: 14-02-2023
        /// Metodo que permite subir el valor de las variables dato a la tabla principal del modulo por medio de un excel
        /// </summary>
        /// <param name="file"></param>
        /// <param name="detalleRegistro"></param>
        /// <param name="cantidadFilas"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<DetalleRegistroIndicadorVariableValorFonatel>> CargarExcelVariable(HttpPostedFileBase file, DetalleRegistroIndicadorFonatel detalleRegistro, int cantidadFilas) //NOMBRE DEL ARCHIVO Y UN CODIGO - SI
        {

            try
            {
                Boolean ind = true;

                using (var package = new ExcelPackage(file.InputStream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1]; //POSICION DEL CODIGO y NOMBRE
                    string Codigo = worksheet.Name; //POSICION DEL CODIGO

                    DetalleRegistroIndicadorFonatel detalle = new DetalleRegistroIndicadorFonatel();

                    if (!string.IsNullOrEmpty(detalleRegistro.IdSolicitudString))
                    {
                        int temp = 0;
                        int.TryParse(Utilidades.Desencriptar(detalleRegistro.IdSolicitudString), out temp);
                        detalle.IdSolicitud = temp;
                    }

                    if (!string.IsNullOrEmpty(detalleRegistro.idFormularioWebString))
                    {
                        int temp = 0;
                        int.TryParse(Utilidades.Desencriptar(detalleRegistro.idFormularioWebString), out temp);
                        detalle.idFormularioWeb = temp;
                    }

                    if (!string.IsNullOrEmpty(detalleRegistro.IdIndicadorString))
                    {
                        int temp = 0;
                        int.TryParse(Utilidades.Desencriptar(detalleRegistro.IdIndicadorString), out temp);
                        detalle.IdIndicador = temp;
                    }

                    List<DetalleRegistroIndicadorFonatel> listaDetalle = detalleRegistroIndicadorFonatelDAL.ObtenerDatoDetalleRegistroIndicador(detalle);

                    int cantColumnas = listaDetalle[0].DetalleRegistroIndicadorVariableFonatel.Count;

                    List<DetalleRegistroIndicadorVariableValorFonatel> listaValores = new List<DetalleRegistroIndicadorVariableValorFonatel>();

                    for (int i = 1; i <= cantColumnas; i++)
                    {
                        int numeroFila = 0;
                        for (int j = Constantes.Excel.Valores; j < cantidadFilas + Constantes.Excel.Valores; j++)
                        {
                            numeroFila++;
                            Regex Val = null;
                            var variableDato = worksheet.Cells[j, i].Value.ToString();

                            Val = new Regex(@"[0-9]{1,9}(\.[0-9]{0,2})?$");

                            if (!Val.IsMatch(worksheet.Cells[j, i].Value.ToString()))
                            {
                                ind = false;
                            }
                            else
                            {
                                DetalleRegistroIndicadorVariableValorFonatel obj = new DetalleRegistroIndicadorVariableValorFonatel();
                                obj.Valor = Decimal.Parse(variableDato.Replace(",","."));
                                obj.idFormularioWeb = detalle.idFormularioWeb;
                                obj.IdIndicador = detalle.IdIndicador;
                                obj.IdSolicitud = detalle.IdSolicitud;
                                obj.NumeroFila = numeroFila;
                                obj.IdVariable = listaDetalle[0].DetalleRegistroIndicadorVariableFonatel[i - 1].idVariable;
                                listaValores.Add(obj);
                            }

                        }

                    }

                    if (ind)
                    {
                        ResultadoConsultaVariable.objetoRespuesta = listaValores;
                        ResultadoConsultaVariable.CantidadRegistros = ResultadoConsulta.objetoRespuesta.Count();
                    }
                    else
                    {
                        ResultadoConsultaVariable.MensajeError = EtiquetasViewRegistroIndicadorFonatel.FormatoVariableIncorrecto;
                        ResultadoConsultaVariable.HayError = (int)Error.ErrorControlado;
                    }


                }

            }
            catch (Exception ex)
            {

                ResultadoConsultaVariable.MensajeError = ex.Message;
            }

            return ResultadoConsultaVariable;
        }

        public RespuestaConsulta<List<DetalleRegistroIndicadorCategoriaValorFonatel>> CargarExcel(HttpPostedFileBase file, DetalleRegistroIndicadorFonatel detalleRegistro, int cantidadFilas) //NOMBRE DEL ARCHIVO Y UN CODIGO - SI
        {
            try
            {
                Boolean ind = true;
                Boolean indFecha = true;
                Boolean indRango = true;

                using (var package = new ExcelPackage(file.InputStream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1]; //POSICION DEL CODIGO y NOMBRE
                    string Codigo = worksheet.Name; //POSICION DEL CODIGO

                    DetalleRegistroIndicadorFonatel detalle = new DetalleRegistroIndicadorFonatel();
                    if (!string.IsNullOrEmpty(detalleRegistro.IdSolicitudString))
                    {
                        int temp = 0;
                        int.TryParse(Utilidades.Desencriptar(detalleRegistro.IdSolicitudString), out temp);
                        detalle.IdSolicitud = temp;
                    }

                    if (!string.IsNullOrEmpty(detalleRegistro.idFormularioWebString))
                    {
                        int temp = 0;
                        int.TryParse(Utilidades.Desencriptar(detalleRegistro.idFormularioWebString), out temp);
                        detalle.idFormularioWeb = temp;
                    }

                    if (!string.IsNullOrEmpty(detalleRegistro.IdIndicadorString))
                    {
                        int temp = 0;
                        int.TryParse(Utilidades.Desencriptar(detalleRegistro.IdIndicadorString), out temp);
                        detalle.IdIndicador = temp;
                    }

                    List<DetalleRegistroIndicadorFonatel> listaDetalle = detalleRegistroIndicadorFonatelDAL.ObtenerDatoDetalleRegistroIndicador(detalle);

                    int cantColumnas = listaDetalle[0].DetalleRegistroIndicadorCategoriaFonatel.Count + listaDetalle[0].DetalleRegistroIndicadorVariableFonatel.Count;

                    List<DetalleRegistroIndicadorCategoriaValorFonatel> listaValores = new List<DetalleRegistroIndicadorCategoriaValorFonatel>();

      
                    for (int i = 1; i <= cantColumnas ; i++)
                    {
                        if (worksheet.Cells[Constantes.Excel.Columna, i].Value != null)
                        {
                            string desCategoria = worksheet.Cells[Constantes.Excel.Columna, i].Value.ToString();

                            DetalleRegistroIndicadorCategoriaFonatel categoria = listaDetalle[0].DetalleRegistroIndicadorCategoriaFonatel.Where(x => x.NombreCategoria == desCategoria).FirstOrDefault();

                            if (categoria != null)
                            {
                                int numFila = 0;
                                for (int j = Constantes.Excel.Valores; j <cantidadFilas + Constantes.Excel.Valores; j++)
                                {
                                    numFila++;
                                    if (worksheet.Cells[j, i].Value != null)
                                    {
                                        String valor = "";
                                        Regex Val = null;
                                        switch (categoria.IdTipoCategoria)
                                        {
                                            case 0:
                                                CategoriaDesagregacion cd = new CategoriaDesagregacion();
                                                cd.idCategoriaDesagregacion = categoria.idCategoria;
                                                List<CategoriaDesagregacion> listaCD = categoriasDesagregacionDAL.ObtenerDatos(cd);
                                                if (listaCD.Count > 0)
                                                {
                                                    int cont = listaCD[0].DetalleCategoriaTexto.Where(x => x.Etiqueta == worksheet.Cells[j, i].Value.ToString()).ToList().Count;
                                                    if (cont > 0)
                                                    {
                                                        valor = worksheet.Cells[j, i].Value.ToString();
                                                    }
                                                    else
                                                    {
                                                        ind = false;
                                                    }
                                                }
                                                else
                                                {
                                                    ind = false;
                                                }
                                                break;
                                            case 1:
                                                Val = new Regex(@"[0-9]{1,9}(\.[0-9]{0,2})?$");
                                                if (!Val.IsMatch(worksheet.Cells[j, i].Value.ToString()))
                                                {
                                                    ind = false;
                                                }
                                                else
                                                {
                                                    if ((Convert.ToDecimal(categoria.RangoMinimo) <= Convert.ToDecimal(worksheet.Cells[j, i].Value.ToString().Replace(",", "."))) && (Convert.ToDecimal(worksheet.Cells[j, i].Value.ToString().Replace(",", ".")) <= Convert.ToDecimal(categoria.RangoMaximo)))
                                                    {
                                                        valor = worksheet.Cells[j, i].Value.ToString().Replace(",", ".");
                                                    }
                                                    else
                                                    {
                                                        ind = false;
                                                    }
                                                }
                                                break;
                                            case 2:
                                                Val = Utilidades.rx_alfanumerico;
                                                if (!Val.IsMatch(worksheet.Cells[j, i].Value.ToString()))
                                                {
                                                    ind = false;
                                                }
                                                else
                                                {
                                                    valor = worksheet.Cells[j, i].Value.ToString();
                                                }
                                                break;
                                            case 3:
                                                Val = Utilidades.rx_soloTexto;
                                                if (!Val.IsMatch(worksheet.Cells[j, i].Value.ToString()))
                                                {
                                                    ind = false;
                                                }
                                                else
                                                {
                                                    valor = worksheet.Cells[j, i].Value.ToString();
                                                }
                                                break;
                                            case 4:
                                                DateTime outConvert;





                                                double.TryParse( worksheet.Cells[j, i].Value.ToString(),out  double num) ;




                                                bool isDate = DateTime.TryParse(DateTime.FromOADate(num).ToString("dd/MM/yyyy"), out outConvert);

                                                if (!isDate)
                                                {
                                                    ind = false;
                                                    indFecha = false;                                                   
                                                }
                                                else
                                                {
                                                   
                                                    DateTime fechaValor = DateTime.Parse(DateTime.FromOADate(num).ToString("dd/MM/yyyy"));
                                                    if ((DateTime.Parse(categoria.RangoMinimo) <= fechaValor) && (fechaValor <= DateTime.Parse(categoria.RangoMaximo)))
                                                    {
                                                        valor = fechaValor.ToString("yyyy-MM-dd");
                                                    }
                                                    else
                                                    {
                                                        ind = false;
                                                        indRango = false;
                                                    }
                                                }

                                                break;
                                            default:
                                                valor = worksheet.Cells[j, i].Value.ToString();
                                                break;
                                        }
                                        if (ind)
                                        {
                                            DetalleRegistroIndicadorCategoriaValorFonatel obj = new DetalleRegistroIndicadorCategoriaValorFonatel();
                                            obj.Valor = valor;
                                            obj.idFormularioWeb = detalle.idFormularioWeb;
                                            obj.IdIndicador = detalle.IdIndicador;
                                            obj.IdSolicitud = detalle.IdSolicitud;
                                            obj.idCategoria = categoria.idCategoria;
                                            obj.NumeroFila = numFila;
                                            listaValores.Add(obj);
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        ind = false;
                                        break;
                                    }
                                }
                                if (!ind)
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            ind = false;
                            break;
                        }
                    }

                    ResultadoConsulta.Clase = modulo;
                    ResultadoConsulta.Accion = (int)Accion.Consultar;
                    ResultadoConsulta.Usuario = user;

                    if (ind)
                    {
                        ResultadoConsulta.objetoRespuesta = listaValores;
                        ResultadoConsulta.CantidadRegistros = ResultadoConsulta.objetoRespuesta.Count();
                    }
                    else
                    {
                        if (!indFecha)
                        {
                            ResultadoConsulta.MensajeError = EtiquetasViewRegistroIndicadorFonatel.FormatoFechaIncorrecto;
                        }

                        if (!indRango)
                        {
                            ResultadoConsulta.MensajeError = EtiquetasViewRegistroIndicadorFonatel.RangoFechaIncorrecto;
                        }

                        ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == Errores.CantidadRegistros || ex.Message == Errores.CodigoRegistrado || ex.Message == Errores.DetalleRegistrado)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                }
                else
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                }
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        #region DetalleRegistroIndicadorVariableValorFonatel

        /// <summary>
        /// Autor: Francisco Vindas Ruiz
        /// Fecha: 14-02-2023
        /// Metodo que permite insertar el valor N de las variables dato a la tabla principal del modulo
        /// </summary>
        /// <param name="file"></param>
        /// <param name="detalleRegistro"></param>
        /// <param name="cantidadFilas"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<DetalleRegistroIndicadorVariableValorFonatel>> InsertarDetalleRegistroIndicadorVariableValorFonatel(List<DetalleRegistroIndicadorVariableValorFonatel> objeto)
        {
            try
            {//Se revisa si la lista contiene informacion
                if (objeto.Count > 0)
                {
                    //Se crea datatable con la informacion de la lista
                    var dt = new DataTable();
                    dt.Columns.Add("IdSolicitud", typeof(int));
                    dt.Columns.Add("idFormularioWeb", typeof(int));
                    dt.Columns.Add("IdIndicador", typeof(int));
                    dt.Columns.Add("idVariable", typeof(int));
                    dt.Columns.Add("NumeroFila", typeof(int));
                    dt.Columns.Add("Valor", typeof(string));

                    foreach (var item in objeto)
                    {
                        if (!string.IsNullOrEmpty(item.Solicitudid))
                        {
                            int temp = 0;
                            int.TryParse(Utilidades.Desencriptar(item.Solicitudid), out temp);
                            item.IdSolicitud = temp;
                        }

                        if (!string.IsNullOrEmpty(item.FormularioId))
                        {
                            int temp = 0;
                            int.TryParse(Utilidades.Desencriptar(item.FormularioId), out temp);
                            item.idFormularioWeb = temp;
                        }

                        if (!string.IsNullOrEmpty(item.IndicadorId))
                        {
                            int temp = 0;
                            int.TryParse(Utilidades.Desencriptar(item.IndicadorId), out temp);
                            item.IdIndicador = temp;
                        }

                        dt.Rows.Add(item.IdSolicitud, item.idFormularioWeb, item.IdIndicador, item.IdVariable, item.NumeroFila, item.Valor);
                    }

                    //Se elimina los detalles valores para insertar los nuevos
                    DetalleRegistroIndicadorVariableValorFonatel eliminar = objeto[0];
                    eliminar.IdVariable = 0;
                    eliminar.IdIndicador = 0;
                    detalleRegistroIndicadorFonatelDAL.EliminarDetalleRegistroIndicadorVariableValorFonatel(eliminar);

                    ResultadoConsultaVariable.Clase = modulo;
                    ResultadoConsultaVariable.Accion = (int)Accion.Crear;
                    ResultadoConsultaVariable.Usuario = user;

                    ResultadoConsultaVariable.objetoRespuesta = detalleRegistroIndicadorFonatelDAL.InsertarDetalleRegistroIndicadorVariableValorFonatel(dt);
                    ResultadoConsultaVariable.CantidadRegistros = ResultadoConsulta.objetoRespuesta.Count();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == Errores.CantidadRegistros || ex.Message == Errores.CodigoRegistrado || ex.Message == Errores.DetalleRegistrado)
                {
                    ResultadoConsultaVariable.HayError = (int)Error.ErrorControlado;
                }
                else
                {
                    ResultadoConsultaVariable.HayError = (int)Error.ErrorSistema;
                }
                ResultadoConsultaVariable.MensajeError = ex.Message;
            }

            return ResultadoConsultaVariable;
        }

        #endregion
    }
}
