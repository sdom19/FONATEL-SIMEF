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
                int.TryParse(Utilidades.Desencriptar(pIndicador.id), out int number);
                pIndicador.idIndicador = number;

                if (pIndicador.idIndicador == 0) // ¿ID descencriptado con éxito?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                pIndicador = indicadorFonatelDAL.ObtenerDatos(pIndicador).Single();

                if (pIndicador == null) // ¿el indicador existe en BD?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                // actualizar el estado del indicador
                pIndicador = PrepararObjetoIndicador(pIndicador);
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
                resultado.Usuario = user;
                resultado.Accion = (int)Accion.Eliminar;
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
                int.TryParse(Utilidades.Desencriptar(pIndicador.id), out int number);
                pIndicador.idIndicador = number;

                if (pIndicador.idIndicador == 0) // ¿ID descencriptado con éxito?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                pIndicador = indicadorFonatelDAL.ObtenerDatos(pIndicador).Single();

                if (pIndicador == null) // ¿ID válido?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                pIndicador = PrepararObjetoIndicador(pIndicador);
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

        /// <summary>
        /// Prepara un objeto indicador para ser enviado al servicio DAL.
        /// Se preparan los id's de las tablas relacionadas para poder efectuar consultas debido a la encriptación.
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <returns></returns>
        private Indicador PrepararObjetoIndicador(Indicador pIndicador)
        {
            if (!string.IsNullOrEmpty(pIndicador.id))
            {
                int.TryParse(Utilidades.Desencriptar(pIndicador.id), out int number);
                pIndicador.idIndicador = number;
            }

            if (pIndicador.TipoIndicadores != null && !string.IsNullOrEmpty(pIndicador.TipoIndicadores.id))
            {
                int.TryParse(Utilidades.Desencriptar(pIndicador.TipoIndicadores.id), out int number);
                pIndicador.IdTipoIndicador = number;
            }
            
            if (pIndicador.ClasificacionIndicadores != null && !string.IsNullOrEmpty(pIndicador.ClasificacionIndicadores.id))
            {
                int.TryParse(Utilidades.Desencriptar(pIndicador.ClasificacionIndicadores.id), out int number);
                pIndicador.IdClasificacion = number;
            }
            
            if (pIndicador.GrupoIndicadores != null && !string.IsNullOrEmpty(pIndicador.GrupoIndicadores.id))
            {
                int.TryParse(Utilidades.Desencriptar(pIndicador.GrupoIndicadores.id), out int number);
                pIndicador.idGrupo = number;
            }
            
            if (pIndicador.UnidadEstudio != null && !string.IsNullOrEmpty(pIndicador.UnidadEstudio.id))
            {
                int.TryParse(Utilidades.Desencriptar(pIndicador.UnidadEstudio.id), out int number);
                pIndicador.IdUnidadEstudio = number;
            }

            if (pIndicador.TipoMedida != null && !string.IsNullOrEmpty(pIndicador.TipoMedida.id))
            {
                int.TryParse(Utilidades.Desencriptar(pIndicador.TipoMedida.id), out int number);
                pIndicador.idTipoMedida = number;
            }

            if (pIndicador.FrecuenciaEnvio != null && !string.IsNullOrEmpty(pIndicador.FrecuenciaEnvio.id))
            {
                int.TryParse(Utilidades.Desencriptar(pIndicador.FrecuenciaEnvio.id), out int number);
                pIndicador.IdFrecuencia = number;
            }

            if (pIndicador.EstadoRegistro != null && !string.IsNullOrEmpty(pIndicador.EstadoRegistro.id))
            {
                int.TryParse(Utilidades.Desencriptar(pIndicador.EstadoRegistro.id), out int number);
                pIndicador.idEstado = number;
            }
            return pIndicador;
        }
    }
}
