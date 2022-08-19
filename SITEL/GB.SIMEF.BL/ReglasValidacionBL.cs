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
    public class ReglaValidacionBL : IMetodos<ReglaValidacion>
    {
        private readonly ReglasValicionDAL clsDatos;

   

        private RespuestaConsulta<List<ReglaValidacion>> ResultadoConsulta;
        string modulo = Etiquetas.ReglasValidacion;

        public ReglaValidacionBL()
        {
            clsDatos = new ReglasValicionDAL();
            ResultadoConsulta = new RespuestaConsulta<List<ReglaValidacion>>();
        }

        public RespuestaConsulta<List<ReglaValidacion>> ActualizarElemento(ReglaValidacion objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<ReglaValidacion>> CambioEstado(ReglaValidacion objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<ReglaValidacion>> ClonarDatos(ReglaValidacion objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<ReglaValidacion>> EliminarElemento(ReglaValidacion objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<ReglaValidacion>> InsertarDatos(ReglaValidacion objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<ReglaValidacion>> ObtenerDatos(ReglaValidacion objeto)
        {
            try
            {
                ResultadoConsulta.Clase = modulo;
                ResultadoConsulta.Accion = (int)Accion.Consultar;
                if (!string.IsNullOrEmpty(objeto.id))
                {
                    int temp = 0;
                    int.TryParse(Utilidades.Desencriptar(objeto.id), out temp);
                    objeto.idRegla= temp;
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

        public RespuestaConsulta<List<ReglaValidacion>> ValidarDatos(ReglaValidacion objeto)
        {
            throw new NotImplementedException();
        }
    }
}
