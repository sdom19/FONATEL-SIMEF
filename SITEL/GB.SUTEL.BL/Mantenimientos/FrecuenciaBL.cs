using GB.SUTEL.DAL.Mantenimientos;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SUTEL.Resources;
using GB.SUTEL.Shared;
using GB.SUTEL.ExceptionHandler;


namespace GB.SUTEL.BL.Mantenimientos
{
    public class FrecuenciaBL : LocalContextualizer
    {
        #region atributos
        FrecuenciaAD frecuenciaAD;
        #endregion

        #region constructores
        public FrecuenciaBL(ApplicationContext appContext)
            : base(appContext)
        {
            frecuenciaAD = new FrecuenciaAD(appContext);
        }

        #endregion

        #region metodos

        public Respuesta<Frecuencia> gAgregar(Frecuencia poFrecuencia)
        {

            Respuesta<Frecuencia> respuesta = new Respuesta<Frecuencia>();
            try
            {
                if (frecuenciaAD.ConsultarPorNombre(poFrecuencia.NombreFrecuencia, poFrecuencia.IdFrecuencia).objObjeto == null)
                {
                    if (frecuenciaAD.ConsultarPorCantidadMeses(Convert.ToInt32(poFrecuencia.CantidadMeses),poFrecuencia.IdFrecuencia).objObjeto == null)
                    {
                        respuesta = frecuenciaAD.Agregar(poFrecuencia);
                        respuesta.strMensaje = Mensajes.ExitoInsertar;
                    }
                    else
                    {
                        respuesta.blnIndicadorTransaccion = false;
                        respuesta.strMensaje = "La cantidad de meses ya existe, y no se permite duplicar. Por favor intente con un número diferente.";
                    }
                }
                else
                {
                    respuesta.blnIndicadorTransaccion = false;
                    respuesta.strMensaje = Mensajes.RegistroDuplicado;
                }
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }

        /// <summary>
        /// Consulta una frecuencia por Nombre
        /// </summary>
        /// <param name="poNombre"></param>
        /// <returns></returns>
        public Respuesta<Frecuencia> gConsultarPorNombre(string poNombre, int idFrecuencia)
        {

            Respuesta<Frecuencia> respuesta = new Respuesta<Frecuencia>();
            try
            {
                respuesta.objObjeto = frecuenciaAD.ConsultarPorNombre(poNombre, idFrecuencia).objObjeto;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        public Respuesta<Frecuencia> gConsultarPorId(int poIdFrecuencia)
        {

            Respuesta<Frecuencia> respuesta = new Respuesta<Frecuencia>();
            try
            {
                respuesta.objObjeto = frecuenciaAD.ConsultarPorId(poIdFrecuencia).objObjeto;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        /// <summary>
        /// Listado de Frecuencias
        /// </summary>
        /// <returns></returns>
        public Respuesta<List<Frecuencia>> gListar()
        {
            Respuesta<List<Frecuencia>> respuesta = new Respuesta<List<Frecuencia>>();

            try
            {
                respuesta = frecuenciaAD.Listar();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        /// <summary>
        /// Elimina Logicamente un registro de Frecuencia
        /// </summary>
        /// <param name="Nivel"></param>
        /// <returns></returns>
        public Respuesta<Frecuencia> gEliminar(int poIdFrecuencia)
        {
            Respuesta<Frecuencia> respuesta = new Respuesta<Frecuencia>();

            try
            {
                respuesta = frecuenciaAD.Eliminar(poIdFrecuencia);

                if (respuesta.blnIndicadorState == 300)
                {
                    respuesta.strMensaje = Mensajes.RegistroNoSePuedeEliminar;

                }
                else
                {
                    respuesta.strMensaje = Mensajes.ExitoEliminar;
                }
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        public Respuesta<Frecuencia> gModificar(Frecuencia poFrecuencia)
        {
            Respuesta<Frecuencia> respuesta = new Respuesta<Frecuencia>();

            try
            {
                if (frecuenciaAD.ConsultarPorNombre(poFrecuencia.NombreFrecuencia, poFrecuencia.IdFrecuencia).objObjeto == null)
                {
                    if (frecuenciaAD.ConsultarPorCantidadMeses(Convert.ToInt32(poFrecuencia.CantidadMeses), poFrecuencia.IdFrecuencia).objObjeto == null)
                    {
                        respuesta = frecuenciaAD.Modificar(poFrecuencia);
                        respuesta.strMensaje = Mensajes.ExitoEditar;
                    }
                    else
                    {
                        respuesta.blnIndicadorTransaccion = false;
                        respuesta.strMensaje = "La cantidad de meses ya existe, y no se permite duplicar. Por favor intente con un número diferente.";
                    }                   
                }
                else
                {
                    respuesta.blnIndicadorTransaccion = false;
                    respuesta.strMensaje = Mensajes.RegistroDuplicado;
                }
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        public Respuesta<List<Frecuencia>> gConsultarPorNombreListado(string poNombre)
        {

            Respuesta<List<Frecuencia>> respuesta = new Respuesta<List<Frecuencia>>();
            try
            {
                respuesta.objObjeto = frecuenciaAD.ConsultarPorNombreListado(poNombre).objObjeto;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        #endregion
    }
}
