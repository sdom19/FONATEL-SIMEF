using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;

namespace GB.SIMEF.BL
{
    public class CategoriasDesagregacionBL:IMetodos<CategoriasDesagregacion>
    {
        private readonly CategoriasDesagregacionDAL clsDatos;

        private RespuestaConsulta<List<CategoriasDesagregacion>> ResultadoConsulta;
        string modulo = Etiquetas.Categorias;

        public CategoriasDesagregacionBL()
        {
            this.clsDatos = new CategoriasDesagregacionDAL();
            this.ResultadoConsulta = new RespuestaConsulta<List<CategoriasDesagregacion>>();
        }

        public RespuestaConsulta<CategoriasDesagregacion> ActualizarElemento(CategoriasDesagregacion objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<CategoriasDesagregacion> CambioEstado(CategoriasDesagregacion objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<CategoriasDesagregacion> ClonarDatos(CategoriasDesagregacion objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<CategoriasDesagregacion> EliminarElemento(CategoriasDesagregacion objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<CategoriasDesagregacion> InsertarDatos(CategoriasDesagregacion objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<CategoriasDesagregacion>> ObtenerDatos(CategoriasDesagregacion objCategoria)
        {
            try
            {
                
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = 1;
                var resul = clsDatos.ObtenerDatos(objCategoria);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();
            }
            catch (Exception ex)
            {
                ResultadoConsulta.HayError = true;
                ResultadoConsulta.MensajeError = ex.Message;
            }
            return ResultadoConsulta;
        }
    }
}
