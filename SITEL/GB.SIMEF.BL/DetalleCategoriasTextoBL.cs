using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL
{
    public class DetalleCategoriasTextoBL:IMetodos<DetalleCategoriaTexto>
    {
        private readonly DetalleCategoriaTextoDAL clsDatos;

        private readonly CategoriasDesagregacionDAL  clsDatosCategoria;

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
            throw new NotImplementedException();
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
                if (cantidadDisponible<=0)
                {
                    throw new Exception(Errores.CantidadRegistros);
                }
                else if (ObtenerListaParaComparar.Where(x=>x.Codigo==objeto.Codigo).Count()>0)
                {
                    throw new Exception(Errores.CodigoRegistrado);
                }
                else if (ObtenerListaParaComparar.Where(x => x.Etiqueta.ToUpper() == objeto.Etiqueta.ToUpper()).Count() > 0)
                {
                    throw new Exception(Errores.EtiquetaRegistrada);
                }
                else
                {
                    ResultadoConsulta.objetoRespuesta = clsDatos.ActualizarDatos(objeto);
                    ResultadoConsulta.CantidadRegistros = ResultadoConsulta.objetoRespuesta.Count();
                    clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                        ResultadoConsulta.Usuario,
                        ResultadoConsulta.Clase, string.Format("{0}/{1}",
                        objeto.CategoriasDesagregacion.Codigo, objeto.Codigo));
                    
                }
            }
            catch (Exception ex)
            {
               
                if (ex.Message==Errores.CantidadRegistros || ex.Message==Errores.CodigoRegistrado || ex.Message==Errores.EtiquetaRegistrada)
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
    }
}
