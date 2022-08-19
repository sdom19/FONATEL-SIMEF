using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL
{
    public class FuentesRegistroBL : IMetodos<FuentesRegistro>
    {
        private readonly FuentesRegistroDAL clsDatos;
        private readonly CategoriasDesagregacionDAL clsDatosTexto;

        private RespuestaConsulta<List<FuentesRegistro>> ResultadoConsulta;
        string modulo = EtiquetasViewFuentesRegistro.FuentesRegistro;

        public FuentesRegistroBL()
        {
            this.clsDatos = new FuentesRegistroDAL();
            this.clsDatosTexto = new CategoriasDesagregacionDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<FuentesRegistro>>();
        }

        public RespuestaConsulta<List<FuentesRegistro>> ActualizarElemento(FuentesRegistro objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FuentesRegistro>> CambioEstado(FuentesRegistro objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FuentesRegistro>> ClonarDatos(FuentesRegistro objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FuentesRegistro>> EliminarElemento(FuentesRegistro objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FuentesRegistro>> InsertarDatos(FuentesRegistro objeto)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Fecha 10/08/2022
        /// Michael Hernández
        /// Metodo para obtener la lista de fuentes
        /// </summary>
        public RespuestaConsulta<List<FuentesRegistro>> ObtenerDatos(FuentesRegistro objFuentesRegistro)
        {
            try
            {
                if (!string.IsNullOrEmpty(objFuentesRegistro.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objFuentesRegistro.id), out temp);
                    objFuentesRegistro.idFuente = temp;
                }
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;
                var resul = clsDatos.ObtenerDatos(objFuentesRegistro);
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



        public RespuestaConsulta<List<FuentesRegistro>> ValidarDatos(FuentesRegistro objeto)
        {
            throw new NotImplementedException();
        }
    }
}
