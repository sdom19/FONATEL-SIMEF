using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL
{
    public class EditarDetalleRegistroIndicadorCategoriaValorFonatelBL : IMetodos<DetalleRegistroIndicadorCategoriaValorFonatel>
    {
        readonly string modulo = "";
        readonly string user = "";
        private readonly EditarDetalleRegistroIndicadorFonatelDAL detalleRegistroIndicadorFonatelDAL;
        private readonly CategoriasDesagregacionDAL categoriasDesagregacionDAL;
        RespuestaConsulta<List<DetalleRegistroIndicadorCategoriaValorFonatel>> ResultadoConsulta;

        public EditarDetalleRegistroIndicadorCategoriaValorFonatelBL(string modulo, string user)
        {
            this.modulo = modulo;
            this.user = user;
            this.detalleRegistroIndicadorFonatelDAL = new EditarDetalleRegistroIndicadorFonatelDAL();
            this.categoriasDesagregacionDAL = new CategoriasDesagregacionDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<DetalleRegistroIndicadorCategoriaValorFonatel>>();
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

        public RespuestaConsulta<List<DetalleRegistroIndicadorCategoriaValorFonatel>> InsertarDatos(List<DetalleRegistroIndicadorCategoriaValorFonatel> objeto)
        {
            try
            {//Se revisa si la lista contiene informacion
                if (objeto.Count > 0)
                {
                    //Se crea datatable con la informacion de la lista
                    var dt = new DataTable();
                    dt.Columns.Add("IdSolicitud", typeof(int));
                    dt.Columns.Add("IdFormulario", typeof(int));
                    dt.Columns.Add("IdIndicador", typeof(int));
                    dt.Columns.Add("idCategoria", typeof(int));
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
                            item.IdFormulario = temp;
                        }

                        if (!string.IsNullOrEmpty(item.IndicadorId))
                        {
                            int temp = 0;
                            int.TryParse(Utilidades.Desencriptar(item.IndicadorId), out temp);
                            item.IdIndicador = temp;
                        }

                        dt.Rows.Add(item.IdSolicitud, item.IdFormulario, item.IdIndicador, item.idCategoria, item.NumeroFila, item.Valor);
                    }

                    //Se elimina los detalles valores para insertar los nuevos
                    DetalleRegistroIndicadorCategoriaValorFonatel eliminar = objeto[0];
                    eliminar.idCategoria = 0;
                    detalleRegistroIndicadorFonatelDAL.EliminarDetalleRegistroIndicadorCategoriaValorFonatel(eliminar);

                    ResultadoConsulta.Clase = modulo;
                    ResultadoConsulta.Accion = (int)Accion.Insertar;
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

        public RespuestaConsulta<List<DetalleRegistroIndicadorCategoriaValorFonatel>> ObtenerDatosCategoriaValor(DetalleRegistroIndicadorFonatel objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;
                if (!string.IsNullOrEmpty(objeto.IdFormularioString))
                {
                    int.TryParse(Utilidades.Desencriptar(objeto.IdFormularioString), out int temp);
                    objeto.IdFormulario = temp;
                }
                if (!string.IsNullOrEmpty(objeto.IdSolicitudString))
                {
                    int.TryParse(Utilidades.Desencriptar(objeto.IdSolicitudString), out int temp);
                    objeto.IdSolicitud = temp;
                }
                if (!string.IsNullOrEmpty(objeto.IdIndicadorString))
                {
                    int.TryParse(Utilidades.Desencriptar(objeto.IdIndicadorString), out int temp);
                    objeto.IdIndicador = temp;
                }

                var resul = detalleRegistroIndicadorFonatelDAL.ObtenerDatoDetalleRegistroIndicadorCategoriaValor(objeto);
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

        public RespuestaConsulta<List<DetalleRegistroIndicadorCategoriaValorFonatel>> CargarExcel(HttpPostedFileBase file, DetalleRegistroIndicadorFonatel detalleRegistro,int cantidadFilas) //NOMBRE DEL ARCHIVO Y UN CODIGO - SI
        {
            try
            {
                Boolean ind = true;
                Boolean indFecha = true;
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

                    if (!string.IsNullOrEmpty(detalleRegistro.IdFormularioString))
                    {
                        int temp = 0;
                        int.TryParse(Utilidades.Desencriptar(detalleRegistro.IdFormularioString), out temp);
                        detalle.IdFormulario = temp;
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

                    for (int i = 1; i < cantColumnas + 1; i++)
                    {                      
                        if (worksheet.Cells[1, i].Value != null)
                        {
                            string desCategoria = worksheet.Cells[1, i].Value.ToString();
                            DetalleRegistroIndicadorCategoriaFonatel categoria = listaDetalle[0].DetalleRegistroIndicadorCategoriaFonatel.Where(x => x.NombreCategoria == desCategoria).FirstOrDefault();

                            if (categoria != null)
                            {
                                for (int j = 2; j < cantidadFilas + 2; j++)
                                {
                                    if (worksheet.Cells[j, i].Value != null)
                                    {
                                        String valor = "";
                                        Regex Val = null;
                                        switch (categoria.IdTipoCategoria)
                                        {
                                            case 0:
                                                CategoriasDesagregacion cd = new CategoriasDesagregacion();
                                                cd.idCategoria = categoria.idCategoria;
                                                List<CategoriasDesagregacion> listaCD = categoriasDesagregacionDAL.ObtenerDatos(cd);
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
                                                if (!Val.IsMatch(worksheet.Cells[j, i].Value.ToString())) {
                                                    ind = false;
                                                }
                                                else
                                                {
                                                    if ((Convert.ToDecimal(categoria.RangoMinimo) <= Convert.ToDecimal(worksheet.Cells[j, i].Value.ToString().Replace(",","."))) && (Convert.ToDecimal(worksheet.Cells[j, i].Value.ToString().Replace(",", ".")) <= Convert.ToDecimal(categoria.RangoMaximo)))
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
                                                bool isDate = DateTime.TryParse(worksheet.Cells[j, i].Value.ToString(), out outConvert);
                                                if (!isDate)
                                                {
                                                    ind = false;
                                                    indFecha = false;
                                                }
                                                else
                                                {

                                                    DateTime fechaValor = DateTime.Parse(worksheet.Cells[j, i].Value.ToString());
                                                    if ((DateTime.Parse(categoria.RangoMinimo) <= fechaValor) && (fechaValor <= DateTime.Parse(categoria.RangoMaximo)))
                                                    {
                                                        valor = fechaValor.ToString("yyyy-MM-dd");
                                                    }
                                                    else
                                                    {
                                                        ind = false;
                                                    }
                                                }
                                                
                                                break;
                                            default:
                                                valor = worksheet.Cells[j, i].Value.ToString();
                                                break;
                                        }
                                        if(ind){
                                            DetalleRegistroIndicadorCategoriaValorFonatel obj = new DetalleRegistroIndicadorCategoriaValorFonatel();
                                            obj.Valor = valor;
                                            obj.IdFormulario = detalle.IdFormulario;
                                            obj.IdIndicador = detalle.IdIndicador;
                                            obj.IdSolicitud = detalle.IdSolicitud;
                                            obj.idCategoria = categoria.idCategoria;
                                            obj.NumeroFila = j - 1;
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
                            ResultadoConsulta.MensajeError = "El formato de la fecha es incorrecto, por favor utilizar el formato año-mes-día";
                        }
                        ResultadoConsulta.HayError = (int)Error.ErrorSistema;
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

        public RespuestaConsulta<List<DetalleRegistroIndicadorCategoriaValorFonatel>> ObtenerDatos(DetalleRegistroIndicadorCategoriaValorFonatel objeto)
        {
            throw new NotImplementedException();
        }
    }
}
