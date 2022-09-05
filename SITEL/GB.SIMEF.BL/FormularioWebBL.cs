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
    public class FormularioWebBL : IMetodos<FormularioWeb>
    {
        private readonly FormularioWebDAL clsDatos;
        private readonly DetalleFormularioWebDAL clsDatos2;

        private RespuestaConsulta<List<FormularioWeb>> ResultadoConsulta;
        string modulo = Etiquetas.Formulario;

        public FormularioWebBL()
        {
            this.clsDatos = new FormularioWebDAL();
            this.clsDatos2 = new DetalleFormularioWebDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<FormularioWeb>>();
        }

        public RespuestaConsulta<List<FormularioWeb>> ActualizarElemento(FormularioWeb objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FormularioWeb>> CambioEstado(FormularioWeb objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FormularioWeb>> ClonarDatos(FormularioWeb objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FormularioWeb>> EliminarElemento(FormularioWeb objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FormularioWeb>> InsertarDatos(FormularioWeb objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<FormularioWeb>> ObtenerDatos(FormularioWeb objeto)
        {
            try
            {
                if (!String.IsNullOrEmpty(objeto.id))
                {
                    objeto.id = Utilidades.Desencriptar(objeto.id);
                    int temp;
                    if (int.TryParse(objeto.id, out temp))
                    {
                        objeto.idFormulario = temp;
                    }
                }
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

        public RespuestaConsulta<List<FormularioWeb>> ValidarDatos(FormularioWeb objeto)
        {
            throw new NotImplementedException();
        }
    }
}
