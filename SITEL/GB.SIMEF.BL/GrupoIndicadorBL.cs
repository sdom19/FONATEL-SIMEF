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
    public class GrupoIndicadorBL : IMetodos<GrupoIndicadores>
    {
        readonly string modulo = "";
        readonly string user = "";
        private readonly GrupoIndicadorDAL grupoIndicadorDAL;

        public GrupoIndicadorBL(string pModulo, string pUser)
        {
            modulo = pModulo;
            user = pUser;
            grupoIndicadorDAL = new GrupoIndicadorDAL();
        }

        public RespuestaConsulta<List<GrupoIndicadores>> ActualizarElemento(GrupoIndicadores objeto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 18/08/2022
        /// José Navarro Acuña
        /// Función que permite cambiar el estado de un grupo indicador, activo o desactivado
        /// </summary>
        /// <param name="pGrupoIndicador"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<GrupoIndicadores>> CambioEstado(GrupoIndicadores pGrupoIndicador)
        {
            RespuestaConsulta<List<GrupoIndicadores>> resultado = new RespuestaConsulta<List<GrupoIndicadores>>();
            bool errorControlado = false, nuevoEstado = pGrupoIndicador.nuevoEstado;

            try
            {
                int.TryParse(Utilidades.Desencriptar(pGrupoIndicador.id), out int idDecencriptado);
                pGrupoIndicador.idGrupo = idDecencriptado;

                if (pGrupoIndicador.idGrupo == 0) // ¿ID descencriptado con éxito?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                pGrupoIndicador = grupoIndicadorDAL.ObtenerDatos(pGrupoIndicador).Single();

                // actualizar el estado del indicador
                pGrupoIndicador.idGrupo = idDecencriptado;
                pGrupoIndicador.Estado = nuevoEstado;
                var grupoIndicadorActualizado = grupoIndicadorDAL.ActualizarDatos(pGrupoIndicador);

                if (grupoIndicadorActualizado.Count() <= 0) // ¿actualizó correctamente?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                // construir respuesta
                resultado.Accion = nuevoEstado ? (int)Accion.Activar : (int)Accion.Eliminar;
                resultado.Clase = modulo;
                resultado.Usuario = user;
                resultado.CantidadRegistros = grupoIndicadorActualizado.Count();

                grupoIndicadorDAL.RegistrarBitacora(resultado.Accion,
                        resultado.Usuario,
                        resultado.Clase, pGrupoIndicador.Nombre);
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

        public RespuestaConsulta<List<GrupoIndicadores>> ClonarDatos(GrupoIndicadores objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<GrupoIndicadores>> EliminarElemento(GrupoIndicadores objeto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 22/08/2022
        /// José Navarro Acuña
        /// Función que inserta un nuevo registro grupo indicador
        /// </summary>
        /// <param name="pGrupoIndicador"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<GrupoIndicadores>> InsertarDatos(GrupoIndicadores pGrupoIndicador)
        {
            RespuestaConsulta<List<GrupoIndicadores>> resultado = new RespuestaConsulta<List<GrupoIndicadores>>();
            bool errorControlado = false;

            try
            {
                List<GrupoIndicadores> grupos = grupoIndicadorDAL.ObtenerDatos(new GrupoIndicadores());
                GrupoIndicadores grupoExiste = grupos.FirstOrDefault(x => x.Nombre.ToUpper().Equals(pGrupoIndicador.Nombre.ToUpper()));

                if (grupoExiste != null)
                {
                    errorControlado = true;
                    throw new Exception(string.Format(Errores.CampoYaExiste, pGrupoIndicador.Nombre));
                }

                List<GrupoIndicadores> grupoInsertado = grupoIndicadorDAL.InsertarGrupoIndicador(pGrupoIndicador);
                resultado.objetoRespuesta = grupoInsertado;
                resultado.CantidadRegistros = grupoInsertado.Count();
                resultado.Accion = (int)Accion.Insertar;
                resultado.Clase = modulo;
                resultado.Usuario = user;

                grupoIndicadorDAL.RegistrarBitacora(resultado.Accion,
                        resultado.Usuario,
                        resultado.Clase, pGrupoIndicador.Nombre);
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
        /// Función que retorna todos los grupos de indicadores registrados en estado activo
        /// Se puede aplicar un filtro para obtener un único elemento a traves del ID.
        /// </summary>
        /// <returns></returns>
        public RespuestaConsulta<List<GrupoIndicadores>> ObtenerDatos(GrupoIndicadores pGrupoIndicadores)
        {
            RespuestaConsulta<List<GrupoIndicadores>> resultado = new RespuestaConsulta<List<GrupoIndicadores>>();

            try
            {
                resultado.Accion = (int)Accion.Consultar;
                var result = grupoIndicadorDAL.ObtenerDatos(pGrupoIndicadores);
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
        /// 08/12/2022
        /// José Navarro
        /// Función que retorna los grupos de indicadores de mercados
        /// </summary>
        /// <returns></returns>
        public RespuestaConsulta<List<GrupoIndicadores>> ObtenerDatosMercado()
        {
            RespuestaConsulta<List<GrupoIndicadores>> resultado = new RespuestaConsulta<List<GrupoIndicadores>>();

            try
            {
                resultado.Accion = (int)Accion.Consultar;
                var result = grupoIndicadorDAL.ObtenerDatosMercado();
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
        /// 09/12/2022
        /// José Navarro Acuña
        /// Función que retorna los grupos de indicadores de calidad
        /// </summary>
        /// <returns></returns>
        public RespuestaConsulta<List<GrupoIndicadores>> ObtenerDatosCalidad()
        {
            RespuestaConsulta<List<GrupoIndicadores>> resultado = new RespuestaConsulta<List<GrupoIndicadores>>();

            try
            {
                resultado.Accion = (int)Accion.Consultar;
                var result = grupoIndicadorDAL.ObtenerDatosCalidad();
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
        /// Función que retorna los grupos de indicadores UIT
        /// </summary>
        /// <returns></returns>
        public RespuestaConsulta<List<GrupoIndicadores>> ObtenerDatosUIT()
        {
            RespuestaConsulta<List<GrupoIndicadores>> resultado = new RespuestaConsulta<List<GrupoIndicadores>>();

            try
            {
                resultado.Accion = (int)Accion.Consultar;
                var result = grupoIndicadorDAL.ObtenerDatosUIT();
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
        /// Función que retorna los grupos de indicadores cruzados
        /// </summary>
        /// <returns></returns>
        public RespuestaConsulta<List<GrupoIndicadores>> ObtenerDatosCruzado()
        {
            RespuestaConsulta<List<GrupoIndicadores>> resultado = new RespuestaConsulta<List<GrupoIndicadores>>();

            try
            {
                resultado.Accion = (int)Accion.Consultar;
                var result = grupoIndicadorDAL.ObtenerDatosCruzado();
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

        public RespuestaConsulta<List<GrupoIndicadores>> ValidarDatos(GrupoIndicadores objeto)
        {
            throw new NotImplementedException();
        }
    }
}
