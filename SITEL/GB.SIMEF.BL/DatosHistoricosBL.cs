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
    public class DatosHistoricosBL : IMetodos<DatoHistorico>
    {
        readonly string modulo = "";
        readonly string user = "";
        private readonly DatosHistoricosDAL DatosHistoricosDAL;

        private readonly BitacoraDAL BitacoraDAL;

        public DatosHistoricosBL(string modulo, string user )
        {
            this.modulo = modulo;
            this.user = user;
            DatosHistoricosDAL = new DatosHistoricosDAL();
            BitacoraDAL = new BitacoraDAL();
        }

        public RespuestaConsulta<List<DatoHistorico>> ActualizarElemento(DatoHistorico objeto)
        {
            RespuestaConsulta<List<DatoHistorico>> resultado = new RespuestaConsulta<List<DatoHistorico>>();

            try
            {


                objeto.Codigo = string.Format("HistCod-{0}", DatosHistoricosDAL.ObtenerDatos(new DatoHistorico()).Max().IdDatoHistorico+1);
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
                resultado.Accion = pDatosHistoricos.Accion;
                resultado.Usuario = user;
                if (!string.IsNullOrEmpty(pDatosHistoricos.id))
                {
                    pDatosHistoricos.id = Utilidades.DesencriptarArray(pDatosHistoricos.id);
                    int temp;
                    if (int.TryParse(pDatosHistoricos.id, out temp))
                    {
                        pDatosHistoricos.IdDatoHistorico = temp;
                        pDatosHistoricos.id = string.Empty;
                    }
                }
                resultado.Clase = modulo;
            
                var result = DatosHistoricosDAL.ObtenerDatos(pDatosHistoricos);
                resultado.objetoRespuesta = result;

                resultado.CantidadRegistros = result.Count();
                if (resultado.Accion==(int)Constantes.Accion.Descargar || resultado.Accion==(int)Constantes.Accion.Consultar)
                {
                    string codigo = string.Join(", ", result.Select(x => string.Format("{0}/{1}", x.Codigo, x.NombrePrograma)).ToList());
                    BitacoraDAL.RegistrarBitacora(resultado.Accion,
                    resultado.Usuario,
                        resultado.Clase, codigo);

                }
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

        /// <summary>
        /// Fecha 31-03-2023
        /// Georgi Mesen Cerdas
        /// Metodo para registrar la bitacora cuando se descarga los datos historicos
        /// </summary>
        /// <returns></returns>
        public void BitacoraDescargar(DatoHistorico objeto)
        {
            RespuestaConsulta<List<DatoHistorico>> resultado = new RespuestaConsulta<List<DatoHistorico>>();
            resultado.Clase = modulo;
            resultado.Accion = (int)Accion.Descargar;
            resultado.Usuario = user;
            objeto.id = Utilidades.DesencriptarArray(objeto.id);

            DatosHistoricosDAL.RegistrarBitacora(resultado.Accion,
                        resultado.Usuario,
                            resultado.Clase, objeto.id, "", "", "");
        }
    }
}
