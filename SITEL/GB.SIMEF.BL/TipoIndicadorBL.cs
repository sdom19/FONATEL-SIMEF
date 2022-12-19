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
            bool errorControlado = false, nuevoEstado = pTipoIndicadores.nuevoEstado;

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

                // actualizar el estado del indicador
                pTipoIndicadores.IdTipoIdicador = idDecencriptado;
                pTipoIndicadores.Estado = nuevoEstado;
                List<TipoIndicadores> tipoIndicadorActualizado = tipoIndicadorDAL.ActualizarDatos(pTipoIndicadores);

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

        public RespuestaConsulta<List<TipoIndicadores>> ClonarDatos(TipoIndicadores objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<TipoIndicadores>> EliminarElemento(TipoIndicadores objeto)
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
        public RespuestaConsulta<List<TipoIndicadores>> InsertarDatos(TipoIndicadores pTipoIndicadores)
        {
            RespuestaConsulta<List<TipoIndicadores>> resultado = new RespuestaConsulta<List<TipoIndicadores>>();
            bool errorControlado = false;

            try
            {
                List<TipoIndicadores> tipos = tipoIndicadorDAL.ObtenerDatos(new TipoIndicadores());
                TipoIndicadores indicadorExiste = tipos.FirstOrDefault(x => x.Nombre.ToUpper().Equals(pTipoIndicadores.Nombre.ToUpper()));

                if (indicadorExiste != null)
                {
                    errorControlado = true;
                    throw new Exception(string.Format(Errores.CampoYaExiste, pTipoIndicadores.Nombre));
                }

                List<TipoIndicadores> indicadorInsertado = tipoIndicadorDAL.InsertarTipoIndicador(pTipoIndicadores);
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
        public RespuestaConsulta<List<TipoIndicadores>> ObtenerDatos(TipoIndicadores pTipoIndicadores)
        {
            RespuestaConsulta<List<TipoIndicadores>> resultado = new RespuestaConsulta<List<TipoIndicadores>>();

            try
            {
                resultado.Accion = (int)Accion.Consultar;
                List<TipoIndicadores> result = tipoIndicadorDAL.ObtenerDatos(pTipoIndicadores);
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
        public RespuestaConsulta<List<TipoIndicadores>> ObtenerDatosMercado()
        {
            RespuestaConsulta<List<TipoIndicadores>> resultado = new RespuestaConsulta<List<TipoIndicadores>>();

            try
            {
                List<TipoIndicadores> result = tipoIndicadorDAL.ObtenerDatosMercado();
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
        public RespuestaConsulta<List<TipoIndicadores>> ObtenerDatosCalidad()
        {
            RespuestaConsulta<List<TipoIndicadores>> resultado = new RespuestaConsulta<List<TipoIndicadores>>();

            try
            {
                List<TipoIndicadores> result = tipoIndicadorDAL.ObtenerDatosCalidad();
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
        public RespuestaConsulta<List<TipoIndicadores>> ObtenerDatosUIT()
        {
            RespuestaConsulta<List<TipoIndicadores>> resultado = new RespuestaConsulta<List<TipoIndicadores>>();

            try
            {
                List<TipoIndicadores> result = tipoIndicadorDAL.ObtenerDatosUIT();
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
        public RespuestaConsulta<List<TipoIndicadores>> ObtenerDatosCruzado()
        {
            RespuestaConsulta<List<TipoIndicadores>> resultado = new RespuestaConsulta<List<TipoIndicadores>>();

            try
            {
                List<TipoIndicadores> result = tipoIndicadorDAL.ObtenerDatosCruzado();
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

        public RespuestaConsulta<List<TipoIndicadores>> ValidarDatos(TipoIndicadores objeto)
        {
            throw new NotImplementedException();
        }
    }
}
