using GB.SIMEF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.BL
{
    public interface IMetodos<T>
    {
        RespuestaConsulta<List<T>> ObtenerDatos(T objeto);

        RespuestaConsulta<List<T>> InsertarDatos(T objeto);

        RespuestaConsulta<List<T>> ClonarDatos(T objeto);

        RespuestaConsulta<List<T>> CambioEstado(T objeto);

        RespuestaConsulta<List<T>> EliminarElemento(T objeto);

        RespuestaConsulta<List<T>> ActualizarElemento(T objeto);

        RespuestaConsulta<List<T>> ValidarDatos(T objeto);

    }
}
