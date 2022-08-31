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
    public class UnidadEstudioBL : IMetodos<UnidadEstudio>
    {
        readonly string modulo = "";
        readonly string user = "";
        private readonly UnidadEstudioDAL unidadEstudioDAL;

        public UnidadEstudioBL(string pModulo, string pUser)
        {
            modulo = pModulo;
            user = pUser;
            unidadEstudioDAL = new UnidadEstudioDAL();
        }

        public RespuestaConsulta<List<UnidadEstudio>> ActualizarElemento(UnidadEstudio objeto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 19/08/2022
        /// José Navarro Acuña
        /// Función que permite cambiar el estado de una unidad de estudio. True: activo, False: desactivado
        /// </summary>
        /// <param name="pUnidadEstudio"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<UnidadEstudio>> CambioEstado(UnidadEstudio pUnidadEstudio)
        {
            RespuestaConsulta<List<UnidadEstudio>> resultado = new RespuestaConsulta<List<UnidadEstudio>>();
            bool errorControlado = false, nuevoEstado = pUnidadEstudio.nuevoEstado;

            try
            {
                int.TryParse(Utilidades.Desencriptar(pUnidadEstudio.id), out int idDecencriptado);
                pUnidadEstudio.idUnidad = idDecencriptado;

                if (pUnidadEstudio.idUnidad == 0) // ¿ID descencriptado con éxito?
                {
                    errorControlado = true;
                    throw new Exception(Errores.NoRegistrosActualizar);
                }

                pUnidadEstudio = unidadEstudioDAL.ObtenerDatos(pUnidadEstudio).Single();

                // actualizar el estado del indicador
                pUnidadEstudio.idUnidad = idDecencriptado;
                pUnidadEstudio.Estado = nuevoEstado;
                var grupoIndicadorActualizado = unidadEstudioDAL.ActualizarDatos(pUnidadEstudio);

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

                unidadEstudioDAL.RegistrarBitacora(resultado.Accion,
                        resultado.Usuario,
                        resultado.Clase, pUnidadEstudio.Nombre);
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

        public RespuestaConsulta<List<UnidadEstudio>> ClonarDatos(UnidadEstudio objeto)
        {
            throw new NotImplementedException();
        }

        public RespuestaConsulta<List<UnidadEstudio>> EliminarElemento(UnidadEstudio objeto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 22/08/2022
        /// José Navarro Acuña
        /// Función que inserta un nuevo registro unidad de estudio
        /// </summary>
        /// <param name="pUnidadEstudio"></param>
        /// <returns></returns>
        public RespuestaConsulta<List<UnidadEstudio>> InsertarDatos(UnidadEstudio pUnidadEstudio)
        {
            RespuestaConsulta<List<UnidadEstudio>> resultado = new RespuestaConsulta<List<UnidadEstudio>>();
            bool errorControlado = false;

            try
            {
                List<UnidadEstudio> unidades = unidadEstudioDAL.ObtenerDatos(new UnidadEstudio());
                UnidadEstudio unidadExiste = unidades.FirstOrDefault(x => x.Nombre.ToUpper().Equals(pUnidadEstudio.Nombre.ToUpper()));

                if (unidadExiste != null)
                {
                    errorControlado = true;
                    throw new Exception(string.Format(Errores.CampoYaExiste, pUnidadEstudio.Nombre));
                }

                List<UnidadEstudio> unidadInsertada = unidadEstudioDAL.InsertarUnidadEstudio(pUnidadEstudio);
                resultado.objetoRespuesta = unidadInsertada;
                resultado.CantidadRegistros = unidadInsertada.Count();
                resultado.Accion = (int)Accion.Insertar;
                resultado.Clase = modulo;
                resultado.Usuario = user;

                unidadEstudioDAL.RegistrarBitacora(resultado.Accion,
                        resultado.Usuario,
                        resultado.Clase, pUnidadEstudio.Nombre);
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
        /// 19/08/2022
        /// José Navarro Acuña
        /// Función que retorna todos las unidades de estudio registradas en estado activo
        /// Se puede aplicar un filtro para obtener un único elemento a traves del ID.
        /// </summary>
        /// <returns></returns>
        public RespuestaConsulta<List<UnidadEstudio>> ObtenerDatos(UnidadEstudio pUnidadEstudio)
        {
            RespuestaConsulta<List<UnidadEstudio>> resultado = new RespuestaConsulta<List<UnidadEstudio>>();

            try
            {
                resultado.Clase = modulo;
                resultado.Accion = (int)Accion.Consultar;
                var result = unidadEstudioDAL.ObtenerDatos(pUnidadEstudio);
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

        public RespuestaConsulta<List<UnidadEstudio>> ValidarDatos(UnidadEstudio objeto)
        {
            throw new NotImplementedException();
        }
    }
}
