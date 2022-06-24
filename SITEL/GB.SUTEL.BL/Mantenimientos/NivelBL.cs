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
    public class NivelBL : LocalContextualizer
    {
        #region atributos
        NivelAD nivelAD;
        #endregion

        #region metodos

        #region Constructores
        public NivelBL(ApplicationContext appContext)
            : base(appContext)
        {

            nivelAD = new NivelAD(appContext);
        }
        #endregion

        public Respuesta<Nivel> gAgregar(Nivel poNivel)
        {

            Respuesta<Nivel> respuesta = new Respuesta<Nivel>();
            try
            {
                //se limpian espacios al inicio y final
                object aux = (object)poNivel;
                Utilidades.LimpiarEspacios(ref aux);

                if (nivelAD.ConsultarPorDescripcion(poNivel.DescNivel).objObjeto == null)
                {
                    respuesta = nivelAD.Agregar(poNivel);
                    if (respuesta.blnIndicadorTransaccion)
                    {
                        respuesta.strMensaje = Mensajes.ExitoInsertar;
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
                respuesta.toError(msj, poNivel);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }

        /// <summary>
        /// Listado de Niveles
        /// </summary>
        /// <returns></returns>
        public Respuesta<List<Nivel>> gListar()
        {
            Respuesta<List<Nivel>> respuesta = new Respuesta<List<Nivel>>();

            try
            {
                respuesta = nivelAD.Listar();
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

        public Respuesta<List<Nivel>> gConsutarPorIdDescripcion(int poIdNivel, string poDescNivel)
        {
            Respuesta<List<Nivel>> respuesta = new Respuesta<List<Nivel>>();

            try
            {
                respuesta = nivelAD.ConsultarPorIdDescripcion(poIdNivel, poDescNivel);
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

        public Respuesta<Nivel> gConsultar(int poIdNivel)
        {
            Respuesta<Nivel> respuesta = new Respuesta<Nivel>();

            try
            {
                respuesta = nivelAD.Consultar(poIdNivel);
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
        /// Elimina Logicamente un registro de Nivel
        /// </summary>
        /// <param name="Nivel"></param>
        /// <returns></returns>
        public Respuesta<Nivel> gEliminar(Nivel Nivel)
        {
            Respuesta<Nivel> respuesta = new Respuesta<Nivel>();

            try
            {
                respuesta = nivelAD.Eliminar(Nivel);
                if (respuesta.blnIndicadorTransaccion)
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

        public Respuesta<Nivel> gModificar(Nivel Nivel)
        {
            Respuesta<Nivel> respuesta = new Respuesta<Nivel>();

            try
            {

                //se limpian espacios al inicio y final
                object aux = (object)Nivel;
                Utilidades.LimpiarEspacios(ref aux);

                if (nivelAD.ConsultarPorDescripcion(Nivel.DescNivel).objObjeto == null)
                {
                    respuesta = nivelAD.Modificar(Nivel);
                    if (respuesta.blnIndicadorTransaccion)
                    {
                        respuesta.strMensaje = Mensajes.ExitoEditar;
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



        #endregion
    }
}
