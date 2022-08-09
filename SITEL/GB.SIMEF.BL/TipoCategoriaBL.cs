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
    public class TipoCategoriaBL : IMetodos<TipoCategoria>
    {
        private readonly TipoCategoriaDAL clsDatos;



        private RespuestaConsulta<List<TipoCategoria>> ResultadoConsulta;
        string modulo = Etiquetas.Categorias;

        public TipoCategoriaBL()
        {
            this.clsDatos = new TipoCategoriaDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<TipoCategoria>>();
        }

        public RespuestaConsulta<TipoCategoria> ActualizarElemento(TipoCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<TipoCategoria> CambioEstado(TipoCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<TipoCategoria> ClonarDatos(TipoCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<TipoCategoria>> EliminarElemento(TipoCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<TipoCategoria> InsertarDatos(TipoCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<TipoCategoria>> ObtenerDatos(TipoCategoria objCategoria)
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

        public RespuestaConsulta<TipoCategoria> ValidarDatos(TipoCategoria objeto)
        {
            throw new NotImplementedException();
        }
    }
}
