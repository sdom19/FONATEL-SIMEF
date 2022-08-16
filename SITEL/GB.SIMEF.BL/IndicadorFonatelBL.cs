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
    public class IndicadorFonatelBL : IMetodos<Indicador>
    {
        private readonly IndicadorFonatelDAL indicadorFonatelDAL;
        readonly string modulo = "";
        readonly string user = "";

        public IndicadorFonatelBL(string pView = "", string pUser = "")
        {
            modulo = pView;
            user = pUser;
            indicadorFonatelDAL = new IndicadorFonatelDAL();
        }

        public RespuestaConsulta<List<Indicador>> ActualizarElemento(Indicador pIndicador)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<Indicador>> CambioEstado(Indicador pIndicador)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<Indicador>> ClonarDatos(Indicador pIndicador)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 10/08/2022
        /// José Navarro Acuña
        /// Método que permite realizar un eliminado lógico de un indicador.
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<Indicador>> EliminarElemento(Indicador pIndicador)
        {
            RespuestaConsulta<List<Indicador>> resultado = new RespuestaConsulta<List<Indicador>>();
            bool errorControlado = false;

            try
            {
                if (string.IsNullOrEmpty(pIndicador.id))
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                // buscar el indicador
                int temp = -1;
                int.TryParse(Utilidades.Desencriptar(pIndicador.id), out temp);
                pIndicador.idIndicador = temp;

                pIndicador = indicadorFonatelDAL.ObtenerDatos(pIndicador).Single();

                if (pIndicador == null) // ¿ID válido?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                // actualizar el estado del indicador
                pIndicador.UsuarioModificacion = user;
                pIndicador.idEstado = (int)EstadosRegistro.Eliminado;
                var indicadorActualizado = indicadorFonatelDAL.ActualizarDatos(pIndicador);

                if (indicadorActualizado.Count() <= 0) // ¿actualizó correctamente?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                // construir respuesta
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Eliminar;
                resultado.objetoRespuesta = indicadorActualizado;
                resultado.CantidadRegistros = indicadorActualizado.Count();

                indicadorFonatelDAL.RegistrarBitacora(resultado.Accion,
                        resultado.Usuario,
                        resultado.Clase, pIndicador.Codigo);
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

        /// <summary>
        /// 16/08/2022
        /// José Navarro Acuña
        /// Método que retorna todos los formularios web relacionados a indicador.
        /// Se puede realizar un filtrado de acuerdo al objecto que se envia y obtener un compilado de varios indicadores.
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<FormularioWeb>> ObtenerFormulariosWebSegunIndicador(Indicador pIndicador)
        {
            RespuestaConsulta<List<FormularioWeb>> resultado = new RespuestaConsulta<List<FormularioWeb>>();
            bool errorControlado = false;

            try
            {
                if (string.IsNullOrEmpty(pIndicador.id))
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                int temp = -1;
                int.TryParse(Utilidades.Desencriptar(pIndicador.id), out temp);
                pIndicador.idIndicador = temp;

                pIndicador = indicadorFonatelDAL.ObtenerDatos(pIndicador).Single();

                if (pIndicador == null) // ¿ID válido?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                var result = indicadorFonatelDAL.ObtenerFormulariosWebSegunIndicador(pIndicador);
                resultado.objetoRespuesta = result;
                resultado.CantidadRegistros = result.Count();
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

        public RespuestaConsulta<List<Indicador>> InsertarDatos(Indicador pIndicador)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 10/08/2022
        /// José Navarro Acuña
        /// Método que retorna todos los indicadores registrados en el sistema.
        /// Se puede realizar un filtrado de acuerdo al objecto que se envia.
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<Indicador>> ObtenerDatos(Indicador pIndicador)
        {
            RespuestaConsulta<List<Indicador>> resultado = new RespuestaConsulta<List<Indicador>>();

            try
            {
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                var result = indicadorFonatelDAL.ObtenerDatos(pIndicador);
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

        public RespuestaConsulta<Indicador> ValidarDatos(Indicador pIndicador)
        {
            throw new NotImplementedException();
        }

        RespuestaConsulta<List<Indicador>> IMetodos<Indicador>.ValidarDatos(Indicador pIndicador)
        {
            throw new NotImplementedException();
        }
    }
}
