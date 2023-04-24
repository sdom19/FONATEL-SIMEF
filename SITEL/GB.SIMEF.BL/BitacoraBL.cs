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
    public class BitacoraBL:IMetodos<Bitacora>
    {
        #region Variables Globales de la clase
            private readonly BitacoraDAL clsDatos;

            string modulo = "";

            string user = "";

            private RespuestaConsulta<List<Bitacora>> ResultadoConsulta;
        #endregion


        public BitacoraBL(string modulo="", string user="")
        {
            this.clsDatos = new BitacoraDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<Bitacora>>();
            this.user = user;
            this.modulo = modulo;
        }

        #region Metodos sin Usar
        public RespuestaConsulta<List<Bitacora>> ValidarDatos(Bitacora objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<Bitacora>> ActualizarElemento(Bitacora objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<Bitacora>> CambioEstado(Bitacora objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<Bitacora>> ClonarDatos(Bitacora objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<Bitacora>> EliminarElemento(Bitacora objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<Bitacora>> InsertarDatos(Bitacora objeto)
        {
            throw new NotImplementedException();
        }
        #endregion


        /// <summary>
        /// 05/08/2022
        /// Obtiene la lista con base a un parametro de fecha desde y fecha hasta
        /// Michael Hernández Cordero
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<Bitacora>> ObtenerDatos(Bitacora objeto)
        {
            try
            {
                    ValidarBitacora(objeto);
                    ResultadoConsulta.Clase = "Bitacora";
                    ResultadoConsulta.Accion = (int)Accion.Consultar;
                    var resul = clsDatos.ObtenerDatos(objeto);
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

        /// <summary>
        /// Fecha 30-03-2023
        /// Georgi Mesen Cerdas
        /// Valida el objecto de bitacora
        /// </summary>
        /// <returns></returns>

        public Boolean ValidarBitacora(Bitacora objeto)
        {
            Boolean ind = false;

            if (objeto.FechaDesde != null && objeto.FechaHasta != null)
            {
                if (Convert.ToDateTime(objeto.FechaDesde) > Convert.ToDateTime(objeto.FechaHasta))
                {
                    ResultadoConsulta.HayError = (int)Constantes.Error.ErrorControlado;
                    throw new Exception(Errores.ValorFecha);
                }
                else
                {
                    ind = true;
                }
            }
            else
            {
                ind = true;
            }

            return ind;
        }


        public RespuestaConsulta<List<Bitacora>> InsertarDatos(int accion, string codigo, string valorActual = "", string ValorAnterior = "", string ValorInicial = "")
        {
            ResultadoConsulta.objetoRespuesta = new List<Bitacora>();
            try
            {
                var resultado = clsDatos.RegistrarBitacora(accion, user, modulo, codigo, valorActual, ValorAnterior, ValorInicial);
                ResultadoConsulta.objetoRespuesta.Add(resultado);
            }
            catch (Exception ex)
            {
                ResultadoConsulta.HayError = (int)Constantes.Error.ErrorSistema;
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }

    }
}
