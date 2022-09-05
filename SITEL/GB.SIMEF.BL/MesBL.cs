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
    public class MesBL : IMetodos<Mes>
    {
        readonly string modulo = "";
        readonly string user = "";
        private readonly MesDAL MesDAL;

        public MesBL()
        {
            MesDAL = new MesDAL();
        }

        public RespuestaConsulta<List<Mes>> ActualizarElemento(Mes objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<Mes>> CambioEstado(Mes objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<Mes>> ClonarDatos(Mes objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<Mes>> EliminarElemento(Mes objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<Mes>> InsertarDatos(Mes objeto)
        {
            throw new NotImplementedException();
        }


        public RespuestaConsulta<List<Mes>> ObtenerDatos(Mes pMes)
        {
            RespuestaConsulta<List<Mes>> resultado = new RespuestaConsulta<List<Mes>>();

            try
            {
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                var result = MesDAL.ObtenerDatos();
                resultado.objetoRespuesta = result;
                resultado.CantidadRegistros = result.Count();
            }
            catch (Exception ex)
            {
                resultado.HayError = (int)Constantes.Error.ErrorSistema;
                resultado.MensajeError = ex.Message;
            }
            return resultado;
        }

        public RespuestaConsulta<List<Mes>> ValidarDatos(Mes objeto)
        {
            throw new NotImplementedException();
        }
    }
}
