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
    public class TipoDetalleCategoriaBL : IMetodos<TipoDetalleCategoria>
    {
        private readonly TipoDetalleCategoriaDAL clsDatos;



        private RespuestaConsulta<List<TipoDetalleCategoria>> ResultadoConsulta;
        string modulo = Etiquetas.Categorias;

        public TipoDetalleCategoriaBL()
        {
            this.clsDatos = new TipoDetalleCategoriaDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<TipoDetalleCategoria>>();
        }

        public RespuestaConsulta<List<TipoDetalleCategoria>> ActualizarElemento(TipoDetalleCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<TipoDetalleCategoria>> CambioEstado(TipoDetalleCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<TipoDetalleCategoria>> ClonarDatos(TipoDetalleCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<TipoDetalleCategoria>> EliminarElemento(TipoDetalleCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<TipoDetalleCategoria>> InsertarDatos(TipoDetalleCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<TipoDetalleCategoria>> ObtenerDatos(TipoDetalleCategoria objCategoria)
        {
            try
            {
              
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;
                var resul = clsDatos.ObtenerDatos(objCategoria);
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

        public RespuestaConsulta<List<TipoDetalleCategoria>> ValidarDatos(TipoDetalleCategoria objeto)
        {
            throw new NotImplementedException();
        }
    }
}
