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
    public class DetalleRelacionCategoriaBL : IMetodos<DetalleRelacionCategoria>
    {
        private readonly DetalleRelacionCategoriaDAL clsDatos;

        private readonly RelacionCategoriaDAL clsDatosRelacionCategoria;

        private RespuestaConsulta<List<DetalleRelacionCategoria>> ResultadoConsulta;

        string modulo = EtiquetasViewRelacionCategoria.DetalleRelacion;

        public DetalleRelacionCategoriaBL()
        {
            clsDatos = new DetalleRelacionCategoriaDAL();
            clsDatosRelacionCategoria = new RelacionCategoriaDAL();
            ResultadoConsulta = new RespuestaConsulta<List<DetalleRelacionCategoria>>();
        }

        public RespuestaConsulta<List<DetalleRelacionCategoria>> ActualizarElemento(DetalleRelacionCategoria objeto)
        {
            try
            {
                if (!string.IsNullOrEmpty(objeto.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.id), out temp);
                    objeto.IdRelacionCategoria = temp;
                    objeto.Estado = true;
                }

                objeto.RelacionCategoria =
                    clsDatosRelacionCategoria.ObtenerDatos(new RelacionCategoria() { idRelacionCategoria = objeto.IdRelacionCategoria }).Single();

                List<DetalleRelacionCategoria> ObtenerListaParaComparar = clsDatos.ObtenerDatos(
                    new DetalleRelacionCategoria() { idCategoriaAtributo = objeto.idCategoriaAtributo }
                    );

                int cantidadDisponible = (int)objeto.RelacionCategoria.CantidadCategoria
                            - objeto.RelacionCategoria.DetalleRelacionCategoria.Count();
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Insertar;
                ResultadoConsulta.Usuario = objeto.usuario;

                if (cantidadDisponible <= 0)
                {
                    throw new Exception(Errores.CantidadRegistros);
                }
                else if (clsDatos.ObtenerDatos(new DetalleRelacionCategoria() { CategoriaAtributoValor = objeto.CategoriaAtributoValor, IdRelacionCategoria = objeto.IdRelacionCategoria }).Count() > 0)
                {
                    throw new Exception(Errores.NombreRegistrado);
                }
                else
                {
                    ResultadoConsulta.objetoRespuesta = clsDatos.ActualizarDatos(objeto);
                    ResultadoConsulta.CantidadRegistros = ResultadoConsulta.objetoRespuesta.Count();

                    if (cantidadDisponible == 1)
                    {
                        objeto.RelacionCategoria.idEstado = (int)Constantes.EstadosRegistro.Activo;
                        objeto.RelacionCategoria.UsuarioModificacion = objeto.usuario;
                        clsDatosRelacionCategoria.ActualizarDatos(objeto.RelacionCategoria);
                    }

                    clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                     ResultadoConsulta.Usuario,
                     ResultadoConsulta.Clase, string.Format("{0}/{1}",
                     objeto.RelacionCategoria.idCategoriaValor, objeto.idCategoriaAtributo));

                }

            }
            catch (Exception ex)
            {
                if (ex.Message == Errores.CantidadRegistros || ex.Message == Errores.CodigoRegistrado || ex.Message == Errores.NombreRegistrado)
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

        public RespuestaConsulta<List<DetalleRelacionCategoria>> CambioEstado(DetalleRelacionCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleRelacionCategoria>> ClonarDatos(DetalleRelacionCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleRelacionCategoria>> EliminarElemento(DetalleRelacionCategoria objeto)
        {
            DetalleRelacionCategoria objrelacion = (DetalleRelacionCategoria)objeto;

            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Eliminar;
                ResultadoConsulta.Usuario = objeto.usuario;
                DetalleRelacionCategoria registroActualizar;

                //DESENCRIPTAR EL ID
                if (!string.IsNullOrEmpty(objrelacion.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objrelacion.id), out temp);
                    objrelacion.idDetalleRelacionCategoria = temp;
                }

                var resul = clsDatos.ObtenerDatos(objrelacion);

                if (resul.Count() == 0)
                {
                    throw new Exception(Errores.NoRegistrosActualizar);

                }
                else
                {
                    registroActualizar = resul.SingleOrDefault();
                    registroActualizar.Estado = false;
                    resul = clsDatos.ActualizarDatos(registroActualizar);
                }

                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();

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

        public RespuestaConsulta<List<DetalleRelacionCategoria>> InsertarDatos(DetalleRelacionCategoria objeto)
        {
            try
            {

                if (!string.IsNullOrEmpty(objeto.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.id), out temp);
                    objeto.IdRelacionCategoria = temp;
                    objeto.Estado = true;
                }

                objeto.RelacionCategoria = 
                    clsDatosRelacionCategoria.ObtenerDatos(new RelacionCategoria() { idRelacionCategoria = objeto.IdRelacionCategoria}).Single();

                List<DetalleRelacionCategoria> ObtenerListaParaComparar = clsDatos.ObtenerDatos(
                    new DetalleRelacionCategoria() { idCategoriaAtributo = objeto.idCategoriaAtributo }
                    );

                int cantidadDisponible = (int)objeto.RelacionCategoria.CantidadCategoria
                            - objeto.RelacionCategoria.DetalleRelacionCategoria.Count();
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Insertar;
                ResultadoConsulta.Usuario = objeto.usuario;


                if (cantidadDisponible <= 0)
                {
                    throw new Exception(Errores.CantidadRegistros);
                }
                else if (clsDatos.ObtenerDatos(new DetalleRelacionCategoria() { CategoriaAtributoValor = objeto.CategoriaAtributoValor, IdRelacionCategoria = objeto.IdRelacionCategoria }).Count() > 0)
                {
                    throw new Exception(Errores.DetalleRegistrado);
                }
                else
                {
                   
                    ResultadoConsulta.objetoRespuesta = clsDatos.ActualizarDatos(objeto);
                    ResultadoConsulta.CantidadRegistros = ResultadoConsulta.objetoRespuesta.Count();
                    

                    if (cantidadDisponible == 1)
                    {
                        objeto.RelacionCategoria.idEstado = (int)Constantes.EstadosRegistro.Activo;
                        objeto.RelacionCategoria.UsuarioModificacion = objeto.usuario;
                        clsDatosRelacionCategoria.ActualizarDatos(objeto.RelacionCategoria);
                    }

                    clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                     ResultadoConsulta.Usuario,
                     ResultadoConsulta.Clase, string.Format("{0}/{1}",
                     objeto.RelacionCategoria.idCategoriaValor, objeto.idCategoriaAtributo));

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

        public RespuestaConsulta<List<DetalleRelacionCategoria>> ObtenerDatos(DetalleRelacionCategoria objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;

                if (!string.IsNullOrEmpty(objeto.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.id), out temp);
                    objeto.IdRelacionCategoria = temp;
                }
                if (!string.IsNullOrEmpty(objeto.relacionid))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.relacionid), out temp);
                    objeto.IdRelacionCategoria = temp;
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

        public RespuestaConsulta<List<DetalleRelacionCategoria>> ValidarDatos(DetalleRelacionCategoria objeto)
        {
            throw new NotImplementedException();
        }

    }
  
}
