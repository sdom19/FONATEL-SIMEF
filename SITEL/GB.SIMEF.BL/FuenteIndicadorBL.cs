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
    public class FuenteIndicadorBL : IMetodos<FuenteIndicador>
    {
        string modulo = string.Empty;
        string user = string.Empty;
        private readonly FuenteIndicadorDAL fuenteIndicadorDAL;

        public FuenteIndicadorBL(string modulo, string user)
        {
            this.modulo = modulo;
            this.user = user;
            fuenteIndicadorDAL = new FuenteIndicadorDAL();
        }

        public RespuestaConsulta<List<FuenteIndicador>> ActualizarElemento(FuenteIndicador objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FuenteIndicador>> CambioEstado(FuenteIndicador objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FuenteIndicador>> ClonarDatos(FuenteIndicador objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FuenteIndicador>> EliminarElemento(FuenteIndicador objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FuenteIndicador>> InsertarDatos(FuenteIndicador objeto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 22/11/2022
        /// José Navarro Acuña
        /// Función que permite obtener las fuentes de indicador. Es posible obtener un único valor a través de la llave primaria
        /// </summary>
        /// <param name="pFuenteIndicador"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<FuenteIndicador>> ObtenerDatos(FuenteIndicador pFuenteIndicador)
        {
            RespuestaConsulta<List<FuenteIndicador>> resultado = new RespuestaConsulta<List<FuenteIndicador>>();

            try
            {
                if (!string.IsNullOrEmpty(pFuenteIndicador.id))
                {
                    int.TryParse(Utilidades.Desencriptar(pFuenteIndicador.id), out int idDesencriptado);
                    pFuenteIndicador.IdFuenteIndicador = idDesencriptado;
                }

                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                var result = fuenteIndicadorDAL.ObtenerDatos(pFuenteIndicador);
                resultado.objetoRespuesta = result;
                resultado.CantidadRegistros = result.Count();
            }
            catch (Exception ex)
            {
                resultado.HayError = (int)Error.ErrorSistema;
                resultado.MensajeError = ex.Message;
            }
            return resultado;
        }

        public RespuestaConsulta<List<FuenteIndicador>> ValidarDatos(FuenteIndicador objeto)
        {
            throw new NotImplementedException();
        }
    }
}
