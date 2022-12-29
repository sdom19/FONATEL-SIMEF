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
    public class DetalleSolicitudesBL : IMetodos<DetalleSolicitudFormulario>
    {
        private readonly DetalleSolicitudesDAL clsDatos;

        private readonly SolicitudDAL clsDatosSolicitud;

        string modulo = EtiquetasViewSolicitudes.Solicitudes;
        string user = string.Empty;

        private RespuestaConsulta<List<DetalleSolicitudFormulario>> ResultadoConsulta;

        public DetalleSolicitudesBL(string modulo, string user)
        {
            clsDatos = new DetalleSolicitudesDAL();
            clsDatosSolicitud = new SolicitudDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<DetalleSolicitudFormulario>>();
            this.user = user;
            this.modulo = modulo;
        }

        public RespuestaConsulta<List<DetalleSolicitudFormulario>> ActualizarElemento(DetalleSolicitudFormulario objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleSolicitudFormulario>> CambioEstado(DetalleSolicitudFormulario objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleSolicitudFormulario>> ClonarDatos(DetalleSolicitudFormulario objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleSolicitudFormulario>> EliminarElemento(DetalleSolicitudFormulario objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Eliminar;
                ResultadoConsulta.Usuario = user;

                DetalleSolicitudFormulario registroActualizar;

                DesencriptarDetalle(objeto);

                var resul = clsDatos.ObtenerDatos(objeto);

                registroActualizar = resul.Single(x => x.IdSolicitud == objeto.IdSolicitud && x.IdFormulario == objeto.IdFormulario);

                registroActualizar.Estado = false;
                resul = clsDatos.ActualizarDatos(registroActualizar);

                ResultadoConsulta.objetoRespuesta = resul;

                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                            ResultadoConsulta.Usuario,
                                ResultadoConsulta.Clase, objeto.Formularioid, "", "", "");

            }
            catch (Exception ex)
            {
                if (ex.Message == Errores.NoRegistrosActualizar)
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

        public RespuestaConsulta<List<DetalleSolicitudFormulario>> InsertarDatos(DetalleSolicitudFormulario objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Insertar;
                ResultadoConsulta.Usuario = user;

                DesencriptarDetalle(objeto);

                var consultarDatos = clsDatos.ObtenerListaFormularios(objeto);

                if (consultarDatos.Where(x => x.idFormulario == objeto.IdFormulario).Count() > 0)
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                    throw new Exception(Errores.FormularioIngresado);
                }
             
                var resul = clsDatos.ActualizarDatos(objeto);

                ResultadoConsulta.objetoRespuesta = resul;

                consultarDatos = clsDatos.ObtenerListaFormularios(objeto);
                var objetoInicial = resul.FirstOrDefault();
                objetoInicial.Formularioid = consultarDatos.Where(x => x.idFormulario == objeto.IdFormulario).FirstOrDefault().Nombre;

                string jsonInicial = objetoInicial.ToString();
                clsDatos.RegistrarBitacora(ResultadoConsulta.Accion,
                            ResultadoConsulta.Usuario,
                                ResultadoConsulta.Clase, objeto.Formularioid, "", "", jsonInicial);
            }
            catch (Exception ex)
            {
                ResultadoConsulta.MensajeError = ex.Message;

                if (ResultadoConsulta.HayError!= (int)Error.ErrorControlado)
                {
                    ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                }

            }

            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<FormularioWeb>> ObtenerListaFormularios(DetalleSolicitudFormulario objeto)
        {

            RespuestaConsulta<List<FormularioWeb>> ResulFormularios = new RespuestaConsulta<List<FormularioWeb>>();

            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;

                DesencriptarDetalle(objeto);

                var resul = clsDatos.ObtenerListaFormularios(objeto);

                ResulFormularios.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();

            }
            catch (Exception ex)
            {
                ResulFormularios.HayError = (int)Constantes.Error.ErrorSistema;
                ResulFormularios.MensajeError = ex.Message;
            }

            return ResulFormularios;
        }

        public RespuestaConsulta<List<DetalleSolicitudFormulario>> ValidarDatos(DetalleSolicitudFormulario objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleSolicitudFormulario>> ObtenerDatos(DetalleSolicitudFormulario objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Eliminar;

                DesencriptarDetalle(objeto);

                var resul = clsDatos.ObtenerDatos(objeto);

                ResultadoConsulta.objetoRespuesta = resul;

            }
            catch (Exception ex)
            {
                if (ex.Message == Errores.NoRegistrosActualizar)
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

        private void DesencriptarDetalle(DetalleSolicitudFormulario objeto)
        {
            if (!String.IsNullOrEmpty(objeto.id))
            {
                objeto.id = Utilidades.Desencriptar(objeto.id);
                int temp;
                if (int.TryParse(objeto.id, out temp))
                {
                    objeto.IdSolicitud = temp;
                }
            }

            if (!String.IsNullOrEmpty(objeto.Formularioid))
            {
                objeto.Formularioid = Utilidades.Desencriptar(objeto.Formularioid);
                int temp;
                if (int.TryParse(objeto.Formularioid, out temp))
                {
                    objeto.IdFormulario = temp;
                }
            }

        }

    }
}
