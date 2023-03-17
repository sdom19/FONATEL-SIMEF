using GB.SIMEF.DAL;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.BL
{
    public class TipoIndicadorBL : IMetodos<TipoIndicador>
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

        public RespuestaConsulta<List<TipoIndicador>> ActualizarElemento(TipoIndicador objeto)
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
        public RespuestaConsulta<List<TipoIndicador>> CambioEstado(TipoIndicador pTipoIndicadores)
        {
            RespuestaConsulta<List<TipoIndicador>> resultado = new RespuestaConsulta<List<TipoIndicador>>();
            bool errorControlado = false, nuevoEstado = pTipoIndicadores.nuevoEstado;

            try
            {
                int.TryParse(Utilidades.Desencriptar(pTipoIndicadores.id), out int idDecencriptado);
                pTipoIndicadores.IdTipoIndicador = idDecencriptado;

                if (pTipoIndicadores.IdTipoIndicador == 0) // ¿ID descencriptado con éxito?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                pTipoIndicadores = tipoIndicadorDAL.ObtenerDatos(pTipoIndicadores).Single();

                // actualizar el estado del indicador
                pTipoIndicadores.IdTipoIndicador = idDecencriptado;
                pTipoIndicadores.Estado = nuevoEstado;
                List<TipoIndicador> tipoIndicadorActualizado = tipoIndicadorDAL.ActualizarDatos(pTipoIndicadores);

                if (tipoIndicadorActualizado.Count() <= 0) // ¿actualizó correctamente?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                // construir respuesta
                resultado.Accion = nuevoEstado ? (int)Accion.Activar : (int)Accion.Eliminar;
                resultado.Clase = modulo;
                resultado.Usuario = user;
                resultado.CantidadRegistros = tipoIndicadorActualizado.Count();

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

        public RespuestaConsulta<List<TipoIndicador>> ClonarDatos(TipoIndicador objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<TipoIndicador>> EliminarElemento(TipoIndicador objeto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 22/08/2022
        /// José Navarro Acuña
        /// Función que inserta un nuevo registro tipo indicador
        /// </summary>
        /// <param name="pTipoIndicadores"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<TipoIndicador>> InsertarDatos(TipoIndicador pTipoIndicadores)
        {
            RespuestaConsulta<List<TipoIndicador>> resultado = new RespuestaConsulta<List<TipoIndicador>>();
            bool errorControlado = false;

            try
            {
                List<TipoIndicador> tipos = tipoIndicadorDAL.ObtenerDatos(new TipoIndicador());
                TipoIndicador indicadorExiste = tipos.FirstOrDefault(x => x.Nombre.ToUpper().Equals(pTipoIndicadores.Nombre.ToUpper()));

                if (indicadorExiste != null)
                {
                    errorControlado = true;
                    throw new Exception(string.Format(Errores.CampoYaExiste, pTipoIndicadores.Nombre));
                }

                List<TipoIndicador> indicadorInsertado = tipoIndicadorDAL.InsertarTipoIndicador(pTipoIndicadores);
                resultado.objetoRespuesta = indicadorInsertado;
                resultado.CantidadRegistros = indicadorInsertado.Count();
                resultado.Accion = (int)Accion.Insertar;
                resultado.Clase = modulo;
                resultado.Usuario = user;

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

        /// <summary>
        /// 18/08/2022
        /// José Navarro Acuña
        /// Función que retorna todos los tipos indicadores registrados en estado activo
        /// Se puede aplicar un filtro para obtener un único elemento a traves del ID.
        /// </summary>
        /// <returns></returns>
        public RespuestaConsulta<List<TipoIndicador>> ObtenerDatos(TipoIndicador pTipoIndicadores)
        {
            RespuestaConsulta<List<TipoIndicador>> resultado = new RespuestaConsulta<List<TipoIndicador>>();

            try
            {
                resultado.Accion = (int)Accion.Consultar;
                List<TipoIndicador> result = tipoIndicadorDAL.ObtenerDatos(pTipoIndicadores);
                resultado.objetoRespuesta = result;
                resultado.CantidadRegistros = result.Count();
            }
            catch (Exception ex)
            {
                resultado.HayError = (int)Error.ErrorSistema;
                resultado.MensajeError = ex.Message;
            }
            return resultado;
        }

        /// <summary>
        /// 24/11/2022
        /// José Navarro Acuña
        /// Función que retorna todos los tipos indicadores registrados Mercado
        /// </summary>
        /// <returns></returns>
        public RespuestaConsulta<List<TipoIndicador>> ObtenerDatosMercado()
        {
            RespuestaConsulta<List<TipoIndicador>> resultado = new RespuestaConsulta<List<TipoIndicador>>();

            try
            {
                List<TipoIndicador> result = tipoIndicadorDAL.ObtenerDatosMercado();
                resultado.objetoRespuesta = result;
                resultado.CantidadRegistros = result.Count();
            }
            catch (Exception ex)
            {
                resultado.HayError = (int)Error.ErrorSistema;
                resultado.MensajeError = ex.Message;
            }
            return resultado;
        }

        /// <summary>
        /// 24/11/2022
        /// José Navarro Acuña
        /// Función que retorna todos los tipos indicadores registrados Mercado
        /// </summary>
        /// <returns></returns>
        public RespuestaConsulta<List<TipoIndicador>> ObtenerDatosCalidad()
        {
            RespuestaConsulta<List<TipoIndicador>> resultado = new RespuestaConsulta<List<TipoIndicador>>();

            try
            {
                List<TipoIndicador> result = tipoIndicadorDAL.ObtenerDatosCalidad();
                resultado.objetoRespuesta = result;
                resultado.CantidadRegistros = result.Count();
            }
            catch (Exception ex)
            {
                resultado.HayError = (int)Error.ErrorSistema;
                resultado.MensajeError = ex.Message;
            }
            return resultado;
        }

        /// <summary>
        /// 12/12/2022
        /// José Navarro Acuña
        /// Función que retorna todos los tipos indicadores registrado en UIT
        /// </summary>
        /// <returns></returns>
        public RespuestaConsulta<List<TipoIndicador>> ObtenerDatosUIT()
        {
            RespuestaConsulta<List<TipoIndicador>> resultado = new RespuestaConsulta<List<TipoIndicador>>();

            try
            {
                List<TipoIndicador> result = tipoIndicadorDAL.ObtenerDatosUIT();
                resultado.objetoRespuesta = result;
                resultado.CantidadRegistros = result.Count();
            }
            catch (Exception ex)
            {
                resultado.HayError = (int)Error.ErrorSistema;
                resultado.MensajeError = ex.Message;
            }
            return resultado;
        }

        /// <summary>
        /// 12/12/2022
        /// José Navarro Acuña
        /// Función que retorna todos los tipos indicadores registrado en UIT
        /// </summary>
        /// <returns></returns>
        public RespuestaConsulta<List<TipoIndicador>> ObtenerDatosCruzado()
        {
            RespuestaConsulta<List<TipoIndicador>> resultado = new RespuestaConsulta<List<TipoIndicador>>();

            try
            {
                List<TipoIndicador> result = tipoIndicadorDAL.ObtenerDatosCruzado();
                resultado.objetoRespuesta = result;
                resultado.CantidadRegistros = result.Count();
            }
            catch (Exception ex)
            {
                resultado.HayError = (int)Error.ErrorSistema;
                resultado.MensajeError = ex.Message;
            }
            return resultado;
        }


        public RespuestaConsulta<List<TipoIndicador>> ValidarDatos(TipoIndicador objeto)
        {
            throw new NotImplementedException();
        }
    }
}
