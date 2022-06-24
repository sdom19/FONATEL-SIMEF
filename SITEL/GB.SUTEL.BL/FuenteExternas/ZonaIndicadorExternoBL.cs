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
    public class ZonaIndicadorExternoBL : LocalContextualizer
    {
        private Respuesta<ZonaIndicadorExterno> objRespuesta;
        private ZonaIndicadorExternoDA objZonaIndicadorExternoDA;
        public ZonaIndicadorExternoBL(ApplicationContext appContext)
            : base(appContext)
        {
            objRespuesta = new Respuesta<ZonaIndicadorExterno>();
            objZonaIndicadorExternoDA = new ZonaIndicadorExternoDA(appContext);
        }

        #region Agregar
        public Respuesta<ZonaIndicadorExterno> Agregar(ZonaIndicadorExterno objZonaIndicadorExterno)
        {
            try
            {
                if (objZonaIndicadorExternoDA.Single(x => x.DescZonaIndicadorExterno == objZonaIndicadorExterno.DescZonaIndicadorExterno && x.Borrado == 0).objObjeto == null)
                {
                    objZonaIndicadorExterno.DescZonaIndicadorExterno = objZonaIndicadorExterno.DescZonaIndicadorExterno.Trim();
                    objRespuesta = objZonaIndicadorExternoDA.Agregar(objZonaIndicadorExterno);
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
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        #endregion

        #region Editar
        public Respuesta<ZonaIndicadorExterno[]> Editar(ZonaIndicadorExterno objZonaIndicadorExterno)
        {
            Respuesta<ZonaIndicadorExterno[]> objRespuesta = new Respuesta<ZonaIndicadorExterno[]>();
            try
            {
                var ZonaIndicadorExterno = objZonaIndicadorExternoDA.Single(x => x.DescZonaIndicadorExterno == objZonaIndicadorExterno.DescZonaIndicadorExterno && x.Borrado == 0).objObjeto;
                if (ZonaIndicadorExterno == null || (ZonaIndicadorExterno.DescZonaIndicadorExterno == objZonaIndicadorExterno.DescZonaIndicadorExterno
                    && ZonaIndicadorExterno.IdZonaIndicadorExterno == objZonaIndicadorExterno.IdZonaIndicadorExterno))
                {
                    if (ZonaIndicadorExterno == null)
                    {
                        objZonaIndicadorExterno.DescZonaIndicadorExterno = objZonaIndicadorExterno.DescZonaIndicadorExterno.Trim();
                        objRespuesta = objZonaIndicadorExternoDA.Editar(objZonaIndicadorExterno);
                    }
                    else
                    {
                        ZonaIndicadorExterno.DescZonaIndicadorExterno = objZonaIndicadorExterno.DescZonaIndicadorExterno.Trim();
                        objRespuesta = objZonaIndicadorExternoDA.Editar(ZonaIndicadorExterno);
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
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
        }
        #endregion

        #region ConsultarTodos
        public Respuesta<List<ZonaIndicadorExterno>> ConsultarTodos()
        {
            Respuesta<List<ZonaIndicadorExterno>> objRespuesta = new Respuesta<List<ZonaIndicadorExterno>>();
            try
            {
                objRespuesta = objZonaIndicadorExternoDA.ConsultarTodos();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        public Respuesta<List<ZonaIndicadorExterno>> ConsultarTodos(RegistroIndicadorExterno RegistroIndicadorExterno)
        {
            Respuesta<List<ZonaIndicadorExterno>> objRespuesta = new Respuesta<List<ZonaIndicadorExterno>>();
            try
            {
                objRespuesta = objZonaIndicadorExternoDA.ConsultarTodos(RegistroIndicadorExterno);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        #endregion

        #region Filtro
        public Respuesta<List<ZonaIndicadorExterno>> gFiltrarZonaIndicadorExterno(int id, string nombre)
        {
            Respuesta<List<ZonaIndicadorExterno>> objRespuesta = new Respuesta<List<ZonaIndicadorExterno>>();
            try
            {
                objRespuesta = objZonaIndicadorExternoDA.gFiltrarZonaIndicadorExterno(id, nombre);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        #endregion

        #region ConsultarPorExpresion
        public Respuesta<ZonaIndicadorExterno> ConsultarPorExpresion(System.Linq.Expressions.Expression<Func<ZonaIndicadorExterno, bool>> expression)
        {
            try
            {
                objRespuesta = objZonaIndicadorExternoDA.Single(expression);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        #endregion

        #region Eliminar
        public Respuesta<ZonaIndicadorExterno> Eliminar(int id)
        {
            Respuesta<ZonaIndicadorExterno> objRespuesta = new Respuesta<ZonaIndicadorExterno>();
            try
            {
                objRespuesta = objZonaIndicadorExternoDA.Eliminar(id);
                objRespuesta.strMensaje = GB.SUTEL.Resources.Resources.OperacionExitosa;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        #endregion
    }
}
