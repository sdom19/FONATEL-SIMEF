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
    public class DetalleFormularioWebBL : IMetodos<DetalleFormularioWeb>
    {
        private readonly DetalleFormularioWebDAL clsDatos;
        private RespuestaConsulta<List<DetalleFormularioWeb>> ResultadoConsulta;
        string modulo = Etiquetas.Formulario;
        string user = string.Empty;


        public DetalleFormularioWebBL(string modulo, string user) 
        {
            this.modulo = modulo;
            this.user = user;
            this.clsDatos = new DetalleFormularioWebDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<DetalleFormularioWeb>>();
        }

        public RespuestaConsulta<List<DetalleFormularioWeb>> ActualizarElemento(DetalleFormularioWeb objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleFormularioWeb>> CambioEstado(DetalleFormularioWeb objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleFormularioWeb>> ClonarDatos(DetalleFormularioWeb objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleFormularioWeb>> EliminarElemento(DetalleFormularioWeb objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleFormularioWeb>> InsertarDatos(DetalleFormularioWeb objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleFormularioWeb>> ObtenerDatos(DetalleFormularioWeb objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
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


        public RespuestaConsulta<List<DetalleFormularioWeb>> ValidarDatos(DetalleFormularioWeb objeto)
        {
            throw new NotImplementedException();
        }
    }
}
