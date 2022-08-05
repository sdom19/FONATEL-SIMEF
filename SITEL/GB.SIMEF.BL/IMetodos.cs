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

        RespuestaConsulta<T> InsertarDatos(T objeto);

        RespuestaConsulta<T> ClonarDatos(T objeto);

        RespuestaConsulta<T> CambioEstado(T objeto);

        RespuestaConsulta<List<T>> EliminarElemento(T objeto);

        RespuestaConsulta<T> ActualizarElemento(T objeto);

    }
}
