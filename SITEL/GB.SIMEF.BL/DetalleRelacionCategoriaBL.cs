using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using OfficeOpenXml;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL
{
    public class DetalleRelacionCategoriaBL : IMetodos<DetalleRelacionCategoria>
    {
        private readonly DetalleRelacionCategoriaDAL clsDatos;

        private readonly RelacionCategoriaDAL clsDatosRelacionCategoria;

        private RespuestaConsulta<List<DetalleRelacionCategoria>> ResultadoConsulta;

        string modulo = EtiquetasViewRelacionCategoria.DetalleRelacion;

        public DetalleRelacionCategoriaBL()
        {
            clsDatos = new DetalleRelacionCategoriaDAL();
            clsDatosRelacionCategoria = new RelacionCategoriaDAL();
            ResultadoConsulta = new RespuestaConsulta<List<DetalleRelacionCategoria>>();
        }

        public RespuestaConsulta<List<DetalleRelacionCategoria>> ActualizarElemento(DetalleRelacionCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleRelacionCategoria>> CambioEstado(DetalleRelacionCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleRelacionCategoria>> ClonarDatos(DetalleRelacionCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleRelacionCategoria>> EliminarElemento(DetalleRelacionCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleRelacionCategoria>> InsertarDatos(DetalleRelacionCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleRelacionCategoria>> ObtenerDatos(DetalleRelacionCategoria objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;

                if (!string.IsNullOrEmpty(objeto.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.id), out temp);
                    objeto.idDetalleRelacionCategoria = temp;
                }
                if (!string.IsNullOrEmpty(objeto.relacionid))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.relacionid), out temp);
                    objeto.IdRelacionCategoria = temp;
                }

                var resul = clsDatos.ObtenerDatos(objeto);
                ResultadoConsulta.objetoRespuesta = resul;
                ResultadoConsulta.CantidadRegistros = resul.Count();

            }
            catch (Exception ex) 
            {

                ResultadoConsulta.HayError = (int)Error.ErrorSistema;
                ResultadoConsulta.MensajeError = ex.Message;
            }

            return ResultadoConsulta;
        }

        public RespuestaConsulta<List<DetalleRelacionCategoria>> ValidarDatos(DetalleRelacionCategoria objeto)
        {
            throw new NotImplementedException();
        }

    }
  
}
