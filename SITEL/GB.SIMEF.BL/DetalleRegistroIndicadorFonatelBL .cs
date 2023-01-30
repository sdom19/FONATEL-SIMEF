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
    public class DetalleRegistroIndicadorFonatelBL : IMetodos<DetalleRegistroIndicadorFonatel>
    {
        readonly string modulo = "";
        readonly string user = "";
        private readonly DetalleRegistroIndicadorFonatelDAL DetalleRegistroIndicadorFonatelDAL;
        RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>> ResultadoConsulta;

        public DetalleRegistroIndicadorFonatelBL(string modulo, string user)
        {
            this.modulo = modulo;
            this.user = user;
            this.DetalleRegistroIndicadorFonatelDAL = new DetalleRegistroIndicadorFonatelDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>>();
        }

        public RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>> ActualizarElemento(DetalleRegistroIndicadorFonatel objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;

                DesencriptarRegistroIndicador(objeto);

                DetalleRegistroIndicadorFonatel detalle = DetalleRegistroIndicadorFonatelDAL.ObtenerDatoDetalleRegistroIndicador(objeto).FirstOrDefault();
                detalle.CantidadFilas = objeto.CantidadFilas;
                if (!string.IsNullOrEmpty(objeto.NotasInformante))
                {
                    detalle.NotasInformante = objeto.NotasInformante;
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

                DesencriptarRegistroIndicador(objeto);

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

        public RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>> ActualizarDetalleRegistroIndicadorFonatelMultiple(List<DetalleRegistroIndicadorFonatel> lista)
        {
            try
            {
                foreach (var objeto in lista)
                {
                    ResultadoConsulta.Clase = modulo;
                    ResultadoConsulta.Accion = (int)Accion.Consultar;

                    DesencriptarRegistroIndicador(objeto);

                    DetalleRegistroIndicadorFonatel detalle = DetalleRegistroIndicadorFonatelDAL.ObtenerDatoDetalleRegistroIndicador(objeto).FirstOrDefault();

                    detalle.CantidadFilas = objeto.CantidadFilas;

                    if (!string.IsNullOrEmpty(objeto.NotasInformante))
                    {
                        detalle.NotasInformante = objeto.NotasInformante;
                    }

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

        /// <summary>
        /// Autor:Francisco Vindas Ruiz
        /// Fecha:26/01/2022
        /// Metodo: Funciona para realizar la carga total del registro indicador el cual incluirá el envio de correo a los usuarios
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<DetalleRegistroIndicadorFonatel>> CargaTotalRegistroIndicador(List<DetalleRegistroIndicadorFonatel> lista)
        {
            try
            {
                foreach (var objeto in lista)
                {
                    ResultadoConsulta.Clase = modulo;
                    ResultadoConsulta.Accion = (int)Accion.Consultar;

                    DesencriptarRegistroIndicador(objeto);

                    DetalleRegistroIndicadorFonatel detalle = DetalleRegistroIndicadorFonatelDAL.ObtenerDatoDetalleRegistroIndicador(objeto).FirstOrDefault();

                    detalle.CantidadFilas = objeto.CantidadFilas;

                    if (!string.IsNullOrEmpty(objeto.NotasInformante))
                    {
                        detalle.NotasInformante = objeto.NotasInformante;
                    }

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

        /// <summary>
        /// Autor:Francisco Vindas Ruiz
        /// Fecha:26/01/2022
        /// Metodo: Funciona para desencriptar los Ids necesarios para los metodos que lo necesiten
        /// </summary>
        /// <param name="objeto"></param>
        private void DesencriptarRegistroIndicador(DetalleRegistroIndicadorFonatel objeto)
        {
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
        }
    }
}
