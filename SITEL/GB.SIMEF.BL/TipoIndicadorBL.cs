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
    public class TipoIndicadorBL : IMetodos<TipoIndicadores>
    {
        readonly string modulo = "";
        readonly string user = "";
        private readonly TipoIndicadorDAL tipoIndicadorDAL;

        public TipoIndicadorBL(string pView, string pUser)
        {
            modulo = pView;
            user = pUser;
            tipoIndicadorDAL = new TipoIndicadorDAL();
        }

        public RespuestaConsulta<List<TipoIndicadores>> ActualizarElemento(TipoIndicadores objeto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 18/08/2022
        /// José Navarro Acuña
        /// Función que permite cambiar el estado de un tipo indicador, activo o desactivado
        /// </summary>
        /// <param name="pTipoIndicadores"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<TipoIndicadores>> CambioEstado(TipoIndicadores pTipoIndicadores)
        {
            RespuestaConsulta<List<TipoIndicadores>> resultado = new RespuestaConsulta<List<TipoIndicadores>>();
            bool errorControlado = false;
            bool nuevoEstado = pTipoIndicadores.nuevoEstado;

            try
            {
                int.TryParse(Utilidades.Desencriptar(pTipoIndicadores.id), out int idDecencriptado);
                pTipoIndicadores.IdTipoIdicador = idDecencriptado;

                if (pTipoIndicadores.IdTipoIdicador == 0) // ¿ID descencriptado con éxito?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                pTipoIndicadores = tipoIndicadorDAL.ObtenerDatos(pTipoIndicadores).Single();

                if (pTipoIndicadores == null) // ¿el indicador existe en BD?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                // actualizar el estado del indicador
                pTipoIndicadores.IdTipoIdicador = idDecencriptado;
                pTipoIndicadores.Estado = nuevoEstado;
                var indicadorActualizado = tipoIndicadorDAL.ActualizarDatos(pTipoIndicadores);

                if (indicadorActualizado.Count() <= 0) // ¿actualizó correctamente?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                // construir respuesta
                resultado.Accion = nuevoEstado ? (int)Accion.Activar : (int)Accion.Eliminar;
                resultado.Clase = modulo;
                resultado.Usuario = user;
                resultado.CantidadRegistros = indicadorActualizado.Count();

                tipoIndicadorDAL.RegistrarBitacora(resultado.Accion,
                        resultado.Usuario,
                        resultado.Clase, pTipoIndicadores.Nombre);
            }
            catch (Exception ex)
            {
                resultado.MensajeError = ex.Message;

                if (errorControlado)
                    resultado.HayError = (int)Error.ErrorControlado;
                else
                    resultado.HayError = (int)Error.ErrorSistema;
            }
            return resultado;
        }

        public RespuestaConsulta<List<TipoIndicadores>> ClonarDatos(TipoIndicadores objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<TipoIndicadores>> EliminarElemento(TipoIndicadores objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<TipoIndicadores>> InsertarDatos(TipoIndicadores objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<TipoIndicadores>> ObtenerDatos(TipoIndicadores pTipoIndicadores)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 18/08/2022
        /// José Navarro Acuña
        /// Función que retorna todos los tipos indicadores registrados en estado activo
        /// </summary>
        /// <returns></returns>
        public RespuestaConsulta<List<TipoIndicadores>> ObtenerDatos()
        {
            RespuestaConsulta<List<TipoIndicadores>> resultado = new RespuestaConsulta<List<TipoIndicadores>>();

            try
            {
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                var result = tipoIndicadorDAL.ObtenerDatos(new TipoIndicadores());
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

        public RespuestaConsulta<List<TipoIndicadores>> ValidarDatos(TipoIndicadores objeto)
        {
            throw new NotImplementedException();
        }
    }
}
