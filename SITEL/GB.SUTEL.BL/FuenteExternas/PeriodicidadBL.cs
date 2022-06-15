using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.Shared;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.DAL.FuenteExternas;


namespace GB.SUTEL.BL.FuenteExternas
{
    public class PeriodicidadBL : LocalContextualizer
    {
        private Respuesta<Periodicidad> objRespuesta;
        private PeriodicidadDA objPeriodicidadDA;
        public PeriodicidadBL(ApplicationContext appContext)
            : base(appContext)
        {
            objRespuesta = new Respuesta<Periodicidad>();
            objPeriodicidadDA = new PeriodicidadDA(appContext);
        }

        #region Agregar
        public Respuesta<Periodicidad> Agregar(Periodicidad objPeriodicidad)
        {
            try
            {
                if (objPeriodicidadDA.Single(x => x.DescPeriodicidad == objPeriodicidad.DescPeriodicidad && x.Borrado == 0).objObjeto == null)
                {
                    objPeriodicidad.DescPeriodicidad = objPeriodicidad.DescPeriodicidad.Trim();
                    objRespuesta = objPeriodicidadDA.Agregar(objPeriodicidad);
                    objRespuesta.strMensaje = GB.SUTEL.Resources.Resources.OperacionExitosa;
                }
                else
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.NombreDuplicado;
                    objRespuesta.blnIndicadorState = 300;
                }
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
            return objRespuesta;
        }
        #endregion

        #region Editar
        public Respuesta<Periodicidad[]> Editar(Periodicidad objPeriodicidad)
        {
            Respuesta<Periodicidad[]> objRespuesta = new Respuesta<Periodicidad[]>();
            try
            {
                var Periodicidad = objPeriodicidadDA.Single(x => x.DescPeriodicidad == objPeriodicidad.DescPeriodicidad && x.Borrado == 0).objObjeto;
                if (Periodicidad == null || (Periodicidad.DescPeriodicidad == objPeriodicidad.DescPeriodicidad
                    && Periodicidad.IdPeridiocidad == objPeriodicidad.IdPeridiocidad))
                {
                    if (Periodicidad == null)
                    {
                        objPeriodicidad.DescPeriodicidad = objPeriodicidad.DescPeriodicidad.Trim();
                        objRespuesta = objPeriodicidadDA.Editar(objPeriodicidad);
                    }
                    else
                    {
                        Periodicidad.DescPeriodicidad = objPeriodicidad.DescPeriodicidad;
                        objRespuesta = objPeriodicidadDA.Editar(Periodicidad);
                    }
                    objRespuesta.strMensaje = GB.SUTEL.Resources.Resources.OperacionExitosa;
                }
                else
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.NombreDuplicado;
                    objRespuesta.blnIndicadorState = 300;
                }
                return objRespuesta;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
        }
        #endregion

        #region ConsultarTodos
        public Respuesta<List<Periodicidad>> ConsultarTodos()
        {
            Respuesta<List<Periodicidad>> objRespuesta = new Respuesta<List<Periodicidad>>();
            try
            {
                objRespuesta = objPeriodicidadDA.ConsultarTodos();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
            return objRespuesta;
        }
        public Respuesta<List<Periodicidad>> ConsultarTodos(RegistroIndicadorExterno RegistroIndicadorExterno)
        {
            Respuesta<List<Periodicidad>> objRespuesta = new Respuesta<List<Periodicidad>>();
            try
            {
                objRespuesta = objPeriodicidadDA.ConsultarTodos(RegistroIndicadorExterno);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
            return objRespuesta;
        }
        #endregion

        #region Filtro
        public Respuesta<List<Periodicidad>> gFiltrarPeriodicidad(int id, string nombre)
        {
            Respuesta<List<Periodicidad>> objRespuesta = new Respuesta<List<Periodicidad>>();
            try
            {
                objRespuesta = objPeriodicidadDA.gFiltrarPeriodicidad(id, nombre);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
            return objRespuesta;
        }
        #endregion

        #region ConsultarPorExpresion
        public Respuesta<Periodicidad> ConsultarPorExpresion(System.Linq.Expressions.Expression<Func<Periodicidad, bool>> expression)
        {
            try
            {
                objRespuesta = objPeriodicidadDA.Single(expression);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
            return objRespuesta;
        }
        #endregion

        #region Eliminar
        public Respuesta<Periodicidad> Eliminar(int id)
        {
            Respuesta<Periodicidad> objRespuesta = new Respuesta<Periodicidad>();
            try
            {
                objRespuesta = objPeriodicidadDA.Eliminar(id);
                objRespuesta.strMensaje = GB.SUTEL.Resources.Resources.OperacionExitosa;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
            return objRespuesta;
        }
        #endregion
    }
}
