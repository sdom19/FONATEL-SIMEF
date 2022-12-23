using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL
{
    public class EditarDetalleRegistroIndicadorFonatelBL : IMetodos<DetalleRegistroIndicadorFonatel>
    {
        readonly string modulo = "";
        readonly string user = "";
        private readonly EditarDetalleRegistroIndicadorFonatelDAL DetalleRegistroIndicadorFonatelDAL;
        RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>> ResultadoConsulta;

        public EditarDetalleRegistroIndicadorFonatelBL(string modulo, string user)
        {
            this.modulo = modulo;
            this.user = user;
            this.DetalleRegistroIndicadorFonatelDAL = new EditarDetalleRegistroIndicadorFonatelDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>>();
        }

        public RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>> ActualizarMultiplesElementos(List<DetalleRegistroIndicadorFonatel> list)
        {
            try
            {
                foreach (var objeto in list)
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

                    DetalleRegistroIndicadorFonatel detalle = DetalleRegistroIndicadorFonatelDAL.ObtenerDatoDetalleRegistroIndicador(objeto).FirstOrDefault();

                    if (!string.IsNullOrEmpty(objeto.NotasEncargado))
                    {
                        detalle.NotasEncargado = objeto.NotasEncargado;
                    }

                    var result = DetalleRegistroIndicadorFonatelDAL.ActualizarDetalleRegistroIndicadorFonatel(detalle);

                    ResultadoConsulta.objetoRespuesta = result;
                    ResultadoConsulta.CantidadRegistros = result.Count();
                }

            }
            catch (Exception ex)
            {
                ResultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>> ActualizarElemento(DetalleRegistroIndicadorFonatel objeto)
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

                DetalleRegistroIndicadorFonatel detalle = DetalleRegistroIndicadorFonatelDAL.ObtenerDatoDetalleRegistroIndicador(objeto).FirstOrDefault();

                if (!string.IsNullOrEmpty(objeto.NotasEncargado))
                {
                    detalle.NotasEncargado = objeto.NotasEncargado;
                }

                var result = DetalleRegistroIndicadorFonatelDAL.ActualizarDetalleRegistroIndicadorFonatel(detalle);

                ResultadoConsulta.objetoRespuesta = result;
                ResultadoConsulta.CantidadRegistros = result.Count();

            }
            catch (Exception ex)
            {
                ResultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>> CambioEstado(DetalleRegistroIndicadorFonatel objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>> ClonarDatos(DetalleRegistroIndicadorFonatel objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>> EliminarElemento(DetalleRegistroIndicadorFonatel objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>> InsertarDatos(DetalleRegistroIndicadorFonatel objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>> ObtenerDatos(DetalleRegistroIndicadorFonatel objeto)
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
                var result = DetalleRegistroIndicadorFonatelDAL.ObtenerDatoDetalleRegistroIndicador(objeto);
                ResultadoConsulta.objetoRespuesta = result;
                ResultadoConsulta.CantidadRegistros = result.Count();
            }
            catch (Exception ex)
            {
                ResultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                ResultadoConsulta.MensajeError = ex.Message;
            }

            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>> ValidarDatos(DetalleRegistroIndicadorFonatel objeto)
        {
            throw new NotImplementedException();
        }
    }
}
