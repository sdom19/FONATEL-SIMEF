using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.BL
{
    public class AcumulacionFormulaBL : IMetodos<AcumulacionFormula>
    {
        private readonly AcumulacionFormulaDAL acumulacionFormulaDAL;

        string modulo = string.Empty;
        string user = string.Empty;

        public AcumulacionFormulaBL (string modulo, string user)
        {
            this.modulo = modulo;
            this.user = user;
            acumulacionFormulaDAL = new AcumulacionFormulaDAL();
        }

        public RespuestaConsulta<List<AcumulacionFormula>> ActualizarElemento(AcumulacionFormula objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<AcumulacionFormula>> CambioEstado(AcumulacionFormula objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<AcumulacionFormula>> ClonarDatos(AcumulacionFormula objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<AcumulacionFormula>> EliminarElemento(AcumulacionFormula objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<AcumulacionFormula>> InsertarDatos(AcumulacionFormula objeto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 28/11/2022
        /// José Navarro Acuña
        /// Función que retorna los tipos de acumulacion de fórmula de cálculo
        /// </summary>
        /// <param name="pAcumulacionFormula"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<AcumulacionFormula>> ObtenerDatos(AcumulacionFormula pAcumulacionFormula)
        {
            RespuestaConsulta<List<AcumulacionFormula>> resultado = new RespuestaConsulta<List<AcumulacionFormula>>();

            try
            {
                if (!string.IsNullOrEmpty(pAcumulacionFormula.id))
                {
                    int.TryParse(Utilidades.Desencriptar(pAcumulacionFormula.id), out int temp);
                    pAcumulacionFormula.IdAcumulacionFormula = temp;
                }

                resultado.Clase = modulo;
                resultado.Accion = (int)Constantes.Accion.Consultar;
                
                List<AcumulacionFormula> respuestaDal = acumulacionFormulaDAL.ObtenerDatos(pAcumulacionFormula);
                resultado.objetoRespuesta = respuestaDal;
                resultado.CantidadRegistros = respuestaDal.Count();
            }
            catch (Exception ex)
            {
                resultado.HayError = (int)Constantes.Error.ErrorSistema;
                resultado.MensajeError = ex.Message;
            }
            return resultado;

        }

        public RespuestaConsulta<List<AcumulacionFormula>> ValidarDatos(AcumulacionFormula objeto)
        {
            throw new NotImplementedException();
        }
    }
}
