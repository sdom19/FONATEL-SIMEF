using GB.SIMEF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.BL
{
    public class DetalleIndicadorCategoriaBL : IMetodos<DetalleIndicadorCategoria>
    {
        private readonly string modulo = "";
        private readonly string user = "";

        public DetalleIndicadorCategoriaBL(string pView, string pUser)
        {
            modulo = pView;
            user = pUser;
        }

        public RespuestaConsulta<List<DetalleIndicadorCategoria>> ActualizarElemento(DetalleIndicadorCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleIndicadorCategoria>> CambioEstado(DetalleIndicadorCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleIndicadorCategoria>> ClonarDatos(DetalleIndicadorCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleIndicadorCategoria>> EliminarElemento(DetalleIndicadorCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleIndicadorCategoria>> InsertarDatos(DetalleIndicadorCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleIndicadorCategoria>> ObtenerDatos(DetalleIndicadorCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleIndicadorCategoria>> ObtenerDatosPorIndicador (DetalleIndicadorCategoria objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<DetalleIndicadorCategoria>> ValidarDatos(DetalleIndicadorCategoria objeto)
        {
            throw new NotImplementedException();
        }
    }
}
