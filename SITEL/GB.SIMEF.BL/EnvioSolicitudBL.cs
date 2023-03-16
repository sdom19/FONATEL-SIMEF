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
    public class EnvioSolicitudBL : IMetodos<EnvioSolicitud>
    {
        readonly string modulo = "";
        readonly string user = "";
        private readonly EnvioSolicitudDAL EnvioSolicitudDAL;
        RespuestaConsulta<List<EnvioSolicitud>> resultado;
        public EnvioSolicitudBL(string modulo, string user)
        {
            this.user = user;
            this.modulo = modulo;
            EnvioSolicitudDAL = new EnvioSolicitudDAL(); 
            resultado = new RespuestaConsulta<List<EnvioSolicitud>>();
        }

        public RespuestaConsulta<List<EnvioSolicitud>> ActualizarElemento(EnvioSolicitud objeto)
        {
            try
            {
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;

                DesencriptarObjeto(objeto);

                resultado.objetoRespuesta = EnvioSolicitudDAL.ActualizarEnvioSolicitud(objeto);
                resultado.CantidadRegistros = resultado.objetoRespuesta.Count();
            }
            catch (Exception ex)
            {
                resultado.HayError = (int)Constantes.Error.ErrorSistema;
                resultado.MensajeError = ex.Message;
            }
            return resultado;
        }

        public RespuestaConsulta<List<EnvioSolicitud>> CambioEstado(EnvioSolicitud objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<EnvioSolicitud>> ClonarDatos(EnvioSolicitud objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<EnvioSolicitud>> EliminarElemento(EnvioSolicitud objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<EnvioSolicitud>> InsertarDatos(EnvioSolicitud objeto)
        {
            throw new NotImplementedException();
        }


        public RespuestaConsulta<List<EnvioSolicitud>> ObtenerDatos(EnvioSolicitud objeto)
        {
           

            try
            {
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                resultado.objetoRespuesta = EnvioSolicitudDAL.ObtenerDatos();
                resultado.CantidadRegistros = resultado.objetoRespuesta.Count();
            }
            catch (Exception ex)
            {
                resultado.HayError = (int)Constantes.Error.ErrorSistema;
                resultado.MensajeError = ex.Message;
            }
            return resultado;
        }

        public RespuestaConsulta<List<EnvioSolicitud>> ValidarDatos(EnvioSolicitud objeto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Autor: Francisco Vindas Ruiz
        /// Fecha: 05/01/2023
        /// Metodo que ayuda a desencriptar los datos a utilizar proveniente del objeto EnvioSolicitudes
        /// </summary>
        /// <param name="objeto"></param>
        private void DesencriptarObjeto(EnvioSolicitud objeto)
        {

            if (!string.IsNullOrEmpty(objeto.IdSolicitudString))
            {
                objeto.IdSolicitudString = Utilidades.Desencriptar(objeto.IdSolicitudString);

                int temp;
                if (int.TryParse(objeto.IdSolicitudString, out temp))
                {
                    objeto.IdSolicitud = temp;
                }
            }
        }
    }
}
