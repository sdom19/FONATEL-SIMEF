﻿using GB.SIMEF.DAL;
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
    public class DatosHistoricosBL : IMetodos<DatoHistorico>
    {
        readonly string modulo = "";
        readonly string user = "";
        private readonly DatosHistoricosDAL DatosHistoricosDAL;

        public DatosHistoricosBL(string modulo, string user )
        {
            this.modulo = modulo;
            this.user = user;
            DatosHistoricosDAL = new DatosHistoricosDAL();
        }

        public RespuestaConsulta<List<DatoHistorico>> ActualizarElemento(DatoHistorico objeto)
        {
            RespuestaConsulta<List<DatoHistorico>> resultado = new RespuestaConsulta<List<DatoHistorico>>();

            try
            {
                resultado.objetoRespuesta = new List<DatoHistorico>();

                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                objeto.FechaCarga= System.DateTime.Now;
                var result = DatosHistoricosDAL.AgregarDatos(objeto);
                resultado.objetoRespuesta.Add( result);
                resultado.CantidadRegistros = resultado.objetoRespuesta.Count();
                resultado.HayError = (int)Constantes.Error.NoError;
            }
            catch (Exception ex)
            {
                resultado.HayError = (int)Constantes.Error.ErrorSistema;
                resultado.MensajeError = ex.Message;
            }
            return resultado;
        }

        public RespuestaConsulta<List<DatoHistorico>> CambioEstado(DatoHistorico objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DatoHistorico>> ClonarDatos(DatoHistorico objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DatoHistorico>> EliminarElemento(DatoHistorico objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DatoHistorico>> InsertarDatos(DatoHistorico objeto)
        {
            throw new NotImplementedException();
        }


        public RespuestaConsulta<List<DatoHistorico>> ObtenerDatos(DatoHistorico pDatosHistoricos)
        {
            RespuestaConsulta<List<DatoHistorico>> resultado = new RespuestaConsulta<List<DatoHistorico>>();

            try
            {
                if (!string.IsNullOrEmpty(pDatosHistoricos.id))
                {

                   

                    pDatosHistoricos.id = Utilidades.DesencriptarArray(pDatosHistoricos.id);
                    int temp;
                    if (int.TryParse(pDatosHistoricos.id, out temp))
                    {
                        pDatosHistoricos.IdHistorico = temp;
                        pDatosHistoricos.id = string.Empty;
                    }
                }
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                var result = DatosHistoricosDAL.ObtenerDatos(pDatosHistoricos);
                resultado.objetoRespuesta = result;
                resultado.CantidadRegistros = result.Count();
            }
            catch (Exception ex)
            {
                resultado.HayError = (int)Constantes.Error.ErrorSistema;
                resultado.MensajeError = ex.Message;
            }
            return resultado;
        }

        public RespuestaConsulta<List<DatoHistorico>> ValidarDatos(DatoHistorico objeto)
        {
            throw new NotImplementedException();
        }
    }
}
