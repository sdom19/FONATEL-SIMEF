using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using Newtonsoft.Json;
using OfficeOpenXml;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL
{
    public class DetalleCategoriasTextoBL : IMetodos<DetalleCategoriaTexto>
    {
        private readonly DetalleCategoriaTextoDAL clsDatos;

        private readonly CategoriasDesagregacionDAL clsDatosCategoria;

        private RespuestaConsulta<List<DetalleCategoriaTexto>> ResultadoConsulta;
        string modulo = Etiquetas.DetalleCategorias;

        string user = string.Empty;

        public DetalleCategoriasTextoBL(string modulo, string user)
        {
            this.user = user;
            this.modulo = modulo;
            clsDatos = new DetalleCategoriaTextoDAL();
            clsDatosCategoria = new CategoriasDesagregacionDAL();
            ResultadoConsulta = new RespuestaConsulta<List<DetalleCategoriaTexto>>();
        }
        private string SerializarObjetoBitacora(DetalleCategoriaTexto objCategoria)
        {
            return JsonConvert.SerializeObject(objCategoria, new JsonSerializerSettings
            { ContractResolver = new JsonIgnoreResolver(objCategoria.NoSerialize) });
        }
        public RespuestaConsulta<List<DetalleCategoriaTexto>> ActualizarElemento(DetalleCategoriaTexto objeto)
        {
            try
            {
                objeto.Estado = true;
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Editar;
                ResultadoConsulta.Usuario = user;
                objeto.usuario=user;
                if (!string.IsNullOrEmpty(objeto.categoriaid))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.categoriaid), out temp);
                    objeto.idCategoria = temp;
                }
                var auxTemp = clsDatos.ObtenerDatos(new DetalleCategoriaTexto()
                { idCategoria = objeto.idCategoria }).ToList();

                string jsonAnterior = SerializarObjetoBitacora(auxTemp.Where(x=>x.Codigo==objeto.Codigo).Single());

                auxTemp = auxTemp.Where(x => x.Codigo != objeto.Codigo).ToList();


                objeto.CategoriasDesagregacion =
                       clsDatosCategoria.ObtenerDatos(new CategoriasDesagregacion() { idCategoria = objeto.idCategoria }).Single();


                if (auxTemp.Where(x => x.Etiqueta.ToUpper()==objeto.Etiqueta.ToUpper()).Count()>0)
                {
                    throw new Exception(Errores.EtiquetaRegistrada);
                }
                else if (!Utilidades.rx_soloTexto.Match(objeto.Etiqueta.Trim()).Success && objeto.CategoriasDesagregacion.idTipoDetalle == (int)Constantes.TipoDetalleCategoriaEnum.Texto)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(string.Format(Errores.CampoConFormatoInvalido, "Etiqueta"));
                }
                else if (!Utilidades.rx_alfanumerico.Match(objeto.Etiqueta.Trim()).Success && objeto.CategoriasDesagregacion.idTipoDetalle == (int)Constantes.TipoDetalleCategoriaEnum.Alfanumerico)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(string.Format(Errores.CampoConFormatoInvalido, "Etiqueta"));
                }
                else
                {
                    var categoria = clsDatosCategoria
                      .ObtenerDatos(new CategoriasDesagregacion() { idCategoria = objeto.idCategoria }).Single();
                    ResultadoConsulta.objetoRespuesta= clsDatos.ActualizarDatos(objeto);
                    ResultadoConsulta.CantidadRegistros = ResultadoConsulta.objetoRespuesta.Count();

                    string jsonActual = SerializarObjetoBitacora(objeto);



                    clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                     ResultadoConsulta.Usuario,
                     ResultadoConsulta.Clase, string.Format("{0}/{1}",
                     categoria.Codigo, objeto.Codigo),jsonActual,jsonAnterior);
                }
            }
            catch (Exception ex)
            {

                if (ResultadoConsulta.HayError != (int)Error.ErrorControlado)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                }
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<DetalleCategoriaTexto>> CambioEstado(DetalleCategoriaTexto objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleCategoriaTexto>> ClonarDatos(DetalleCategoriaTexto objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleCategoriaTexto>> EliminarElemento(DetalleCategoriaTexto objeto)
        {
            DetalleCategoriaTexto objCategoria = (DetalleCategoriaTexto)objeto;
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Eliminar;
                ResultadoConsulta.Usuario = user;
                objeto.usuario = user;
                DetalleCategoriaTexto registroActializar;
                if (!String.IsNullOrEmpty(objeto.id))
                {
                    objCategoria.id = Utilidades.Desencriptar(objCategoria.id);
                    int temp;
                    if (int.TryParse(objeto.id, out temp))
                    {
                        objCategoria.idCategoriaDetalle = temp;
                    }
                }
                var resul = clsDatos.ObtenerDatos(objCategoria);
                if (resul.Count() == 0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.NoRegistrosActualizar);

                }
                else
                {
                    registroActializar = resul.SingleOrDefault();
                    registroActializar.Estado = false;
                    resul = clsDatos.ActualizarDatos(registroActializar);
                }
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();
                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion, ResultadoConsulta.Usuario,
                    ResultadoConsulta.Clase,
                   string.Format("{0}/{1}",
                   registroActializar.CategoriasDesagregacion.Codigo,
                   registroActializar.Codigo)
                 );
            }
            catch (Exception ex)
            {
                ResultadoConsulta.MensajeError = ex.Message;
                if (ResultadoConsulta.HayError != (int)Error.ErrorControlado)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                }
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<DetalleCategoriaTexto>> InsertarDatos(DetalleCategoriaTexto objeto)
        {
            try
            {
                if (!string.IsNullOrEmpty(objeto.categoriaid))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.categoriaid), out temp);
                    objeto.idCategoria = temp;
                    objeto.Estado = true;
                }
                objeto.CategoriasDesagregacion =
                        clsDatosCategoria.ObtenerDatos(new CategoriasDesagregacion() { idCategoria = objeto.idCategoria }).Single();
                int cantidadDisponible = (int)objeto.CategoriasDesagregacion.CantidadDetalleDesagregacion
                                            - objeto.CategoriasDesagregacion.DetalleCategoriaTexto.Count();
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Insertar;
                ResultadoConsulta.Usuario = user;
                objeto.usuario = user;
                List< DetalleCategoriaTexto> detalleCategoria = clsDatos.ObtenerDatos(new DetalleCategoriaTexto() { idCategoria = objeto.idCategoria });

                if (cantidadDisponible <= 0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.CantidadRegistros);
                }
                else if (detalleCategoria.Where(x=>x.Codigo == objeto.Codigo).Count() > 0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.CodigoRegistrado);
                }
                else if (detalleCategoria.Where(x=>x.Etiqueta == objeto.Etiqueta.ToUpper()).Count() > 0)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(Errores.EtiquetaRegistrada);
                }
                else if (!Utilidades.rx_soloTexto.Match(objeto.Etiqueta.Trim()).Success && objeto.CategoriasDesagregacion.idTipoDetalle == (int)Constantes.TipoDetalleCategoriaEnum.Texto)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(string.Format( Errores.CampoConFormatoInvalido,"Etiqueta"));
                }
                else if (!Utilidades.rx_alfanumerico.Match(objeto.Etiqueta.Trim()).Success && objeto.CategoriasDesagregacion.idTipoDetalle == (int)Constantes.TipoDetalleCategoriaEnum.Alfanumerico)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                    throw new Exception(string.Format(Errores.CampoConFormatoInvalido, "Etiqueta"));
                }
                else
                {
                    ResultadoConsulta.objetoRespuesta = clsDatos.ActualizarDatos(objeto);
                    ResultadoConsulta.CantidadRegistros = ResultadoConsulta.objetoRespuesta.Count();
                    string jsonInicial = SerializarObjetoBitacora(objeto);
                      clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                       ResultadoConsulta.Usuario,
                       ResultadoConsulta.Clase, string.Format("{0}/{1}",
                       objeto.CategoriasDesagregacion.Codigo, objeto.Codigo),"","",jsonInicial);
                    
                }
            }
            catch (Exception ex)
            {

                if (ResultadoConsulta.HayError != (int)Error.ErrorControlado)
                { 
                    ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                }
                ResultadoConsulta.MensajeError = ex.Message;
            }

            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<DetalleCategoriaTexto>> ObtenerDatos(DetalleCategoriaTexto objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;
                if (!string.IsNullOrEmpty(objeto.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.id), out temp);
                    objeto.idCategoriaDetalle = temp;
                }
                if (!string.IsNullOrEmpty(objeto.categoriaid))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.categoriaid), out temp);
                    objeto.idCategoria = temp;
                }


                var resul = clsDatos.ObtenerDatos(objeto);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();
            }
            catch (Exception ex)
            {
                ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                ResultadoConsulta.MensajeError = ex.Message;

            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<DetalleCategoriaTexto>> ValidarDatos(DetalleCategoriaTexto objeto)
        {
            throw new NotImplementedException();
        }


        public bool CargarExcel(HttpPostedFileBase file)
        {
            bool resultado = true;
            using (var package = new ExcelPackage(file.InputStream))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                string Codigo = worksheet.Name;

                CategoriasDesagregacion categoria =
                                     clsDatosCategoria.ObtenerDatos(new CategoriasDesagregacion() { Codigo = Codigo })
                                    .SingleOrDefault();
                categoria.DetalleCategoriaTexto = new List<DetalleCategoriaTexto>();

                for (int i = 0; i < categoria.CantidadDetalleDesagregacion; i++)
                {
                    int fila = i + 2;
                    if (worksheet.Cells[fila, 1].Value != null || worksheet.Cells[fila, 2].Value != null)
                    {
                        int codigo = 0;
                        string Etiqueta = string.Empty;
                        int.TryParse(worksheet.Cells[fila, 1].Value.ToString().Trim(), out codigo);
                        Etiqueta = worksheet.Cells[fila, 2].Value.ToString().Trim();

                        var detallecategoria = new DetalleCategoriaTexto()
                        {
                            idCategoria = categoria.idCategoria,
                            Codigo = codigo,
                            Etiqueta = Etiqueta,
                            Estado = true
                        };

                        DetalleCategoriaTexto consultarCategoria = ObtenerDatos(new DetalleCategoriaTexto() 
                                { Codigo = detallecategoria.Codigo, idCategoria = detallecategoria.idCategoria }  ).objetoRespuesta.SingleOrDefault();

                        if (consultarCategoria==null)
                        {
                            if (InsertarDatos(detallecategoria).HayError != 0)
                            {
                                resultado = false;
                                break;
                            }
                        }
                        else
                        {
                            if (ActualizarElemento(detallecategoria).HayError != 0)
                            {
                                resultado = false;
                                break;
                            }
                        }

                       
                    }
                }
            }

            return resultado;
        }
    }
}
