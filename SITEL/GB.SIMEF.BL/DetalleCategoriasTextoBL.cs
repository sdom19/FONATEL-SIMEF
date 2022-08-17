using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
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

        public DetalleCategoriasTextoBL()
        {
            clsDatos = new DetalleCategoriaTextoDAL();
            clsDatosCategoria = new CategoriasDesagregacionDAL();
            ResultadoConsulta = new RespuestaConsulta<List<DetalleCategoriaTexto>>();
        }

        public RespuestaConsulta<List<DetalleCategoriaTexto>> ActualizarElemento(DetalleCategoriaTexto objeto)
        {
            try
            {
                objeto.Estado = true;
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Editar;
                ResultadoConsulta.Usuario = objeto.usuario;
                if (!string.IsNullOrEmpty(objeto.categoriaid))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.categoriaid), out temp);
                    objeto.idCategoria = temp;
                }
                var auxTemp = clsDatos.ObtenerDatos(new DetalleCategoriaTexto()
                { idCategoria = objeto.idCategoria }).ToList();
                auxTemp = auxTemp.Where(x => x.Codigo != objeto.Codigo).ToList();

              

                if (auxTemp.Where(x => x.Etiqueta.ToUpper()==objeto.Etiqueta.ToUpper()).Count()>0)
                {
                    throw new Exception(Errores.EtiquetaRegistrada);
                }
                else
                {
                    var categoria = clsDatosCategoria
                      .ObtenerDatos(new CategoriasDesagregacion() { idCategoria = objeto.idCategoria }).Single();
                    ResultadoConsulta.objetoRespuesta= clsDatos.ActualizarDatos(objeto);
                    ResultadoConsulta.CantidadRegistros = ResultadoConsulta.objetoRespuesta.Count();
                    clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                     ResultadoConsulta.Usuario,
                     ResultadoConsulta.Clase, string.Format("{0}/{1}",
                     categoria.Codigo, objeto.Codigo));
                }
            }
            catch (Exception ex)
            {

                if (ex.Message == Errores.EtiquetaRegistrada)
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
                ResultadoConsulta.Usuario = objeto.usuario;
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
                if (ex.Message == Errores.NoRegistrosActualizar)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorControlado;
                }
                else
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

                List<DetalleCategoriaTexto> ObtenerListaParaComparar = clsDatos.ObtenerDatos(
                    new DetalleCategoriaTexto() { idCategoria = objeto.idCategoria }
                    );

                int cantidadDisponible = (int)objeto.CategoriasDesagregacion.CantidadDetalleDesagregacion
                                            - objeto.CategoriasDesagregacion.DetalleCategoriaTexto.Count();
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Insertar;
                ResultadoConsulta.Usuario = objeto.usuario;
                if (cantidadDisponible <= 0)
                {
                    throw new Exception(Errores.CantidadRegistros);
                }
                else if (clsDatos.ObtenerDatos(new DetalleCategoriaTexto() { Codigo = objeto.Codigo, idCategoria = objeto.idCategoria }).Count() > 0)
                {
                    throw new Exception(Errores.CodigoRegistrado);
                }
                else if (clsDatos.ObtenerDatos(new DetalleCategoriaTexto() { Etiqueta = objeto.Etiqueta, idCategoria = objeto.idCategoria }).Count() > 0)
                {
                    throw new Exception(Errores.EtiquetaRegistrada);
                }
                else
                {
                    ResultadoConsulta.objetoRespuesta = clsDatos.ActualizarDatos(objeto);
                    ResultadoConsulta.CantidadRegistros = ResultadoConsulta.objetoRespuesta.Count();

                    if (cantidadDisponible == 1)
                    {
                        objeto.CategoriasDesagregacion.idEstado = (int)Constantes.EstadosRegistro.Activo;
                        objeto.CategoriasDesagregacion.UsuarioModificacion = objeto.usuario;
                        clsDatosCategoria.ActualizarDatos(objeto.CategoriasDesagregacion);
                    }

                      clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                       ResultadoConsulta.Usuario,
                       ResultadoConsulta.Clase, string.Format("{0}/{1}",
                       objeto.CategoriasDesagregacion.Codigo, objeto.Codigo));
                    
                }
            }
            catch (Exception ex)
            {

                if (ex.Message == Errores.CantidadRegistros || ex.Message == Errores.CodigoRegistrado || ex.Message == Errores.EtiquetaRegistrada)
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


        public void CargarExcel(HttpPostedFileBase file)
        {

            using (var package = new ExcelPackage(file.InputStream))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                string Codigo = worksheet.Name;

                CategoriasDesagregacion categoria =
                                     clsDatosCategoria.ObtenerDatos(new CategoriasDesagregacion() { Codigo = Codigo })
                                    .SingleOrDefault();
                categoria.DetalleCategoriaTexto = new List<DetalleCategoriaTexto>();

                clsDatos.DeshabilitarDatos(new DetalleCategoriaTexto() {idCategoria=categoria.idCategoria });
                for (int i = 0; i < categoria.CantidadDetalleDesagregacion; i++)
                {
                    int fila = i + 2;
                    if (worksheet.Cells[fila, 1].Value != null || worksheet.Cells[fila, 2].Value != null)
                    {
                        int codigo = 0;
                        string Etiqueta = string.Empty;
                        int.TryParse(worksheet.Cells[fila, 1].Value.ToString().Trim(), out codigo);
                        Etiqueta = worksheet.Cells[fila, 2].Value.ToString();

                        var detallecategoria = new DetalleCategoriaTexto()
                        {
                            idCategoria = categoria.idCategoria,
                            Codigo = codigo,
                            Etiqueta = Etiqueta,
                            Estado = true
                        };
                        InsertarDatos(detallecategoria);
                    }
                }
            }
        }
    }
}
