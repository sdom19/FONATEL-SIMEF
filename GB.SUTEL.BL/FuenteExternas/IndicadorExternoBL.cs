using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.Shared;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.UI.Helpers;
using GB.SUTEL.DAL.FuenteExternas;


namespace GB.SUTEL.BL.FuenteExternas
{
    public class IndicadorExternoBL : LocalContextualizer
    {
        private Respuesta<IndicadorExterno> objRespuesta;
        private IndicadorExternoDA objIndicadorExternoDA;
        public IndicadorExternoBL(ApplicationContext appContext)
            : base(appContext)
        {
            objRespuesta = new Respuesta<IndicadorExterno>();
            objIndicadorExternoDA = new IndicadorExternoDA(appContext);
        }

        #region Agregar
        public Respuesta<IndicadorExterno> Agregar(IndicadorExterno objIndicadorExterno)
        {
            try
            {
                objIndicadorExterno.IdTipoValor = (int)CCatalogo.TipoValor.CANTIDAD;
                if (objIndicadorExternoDA.Single(x => x.Nombre == objIndicadorExterno.Nombre && x.Borrado == 0).objObjeto == null)
                {
                    if (objIndicadorExternoDA.Single(x => x.IdIndicadorExterno == objIndicadorExterno.IdIndicadorExterno && x.Borrado == 0).objObjeto == null)
                    {
                        objIndicadorExterno.IdIndicadorExterno = objIndicadorExterno.IdIndicadorExterno.Trim();
                        objIndicadorExterno.Descripcion = objIndicadorExterno.Descripcion.Trim();
                        objIndicadorExterno.Nombre = objIndicadorExterno.Nombre.Trim();
                        objRespuesta = objIndicadorExternoDA.Agregar(objIndicadorExterno);
                        objRespuesta.strMensaje = GB.SUTEL.Resources.Resources.OperacionExitosa;
                    }
                    else
                    {
                        objRespuesta.blnIndicadorTransaccion = false;
                        objRespuesta.strMensaje = "El id ya se encuentra registrado";
                        objRespuesta.blnIndicadorState = 300;
                    }
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
        public Respuesta<IndicadorExterno[]> Editar(IndicadorExterno objIndicadorExterno)
        {
            Respuesta<IndicadorExterno[]> objRespuesta = new Respuesta<IndicadorExterno[]>();
            try
            {
                var IndicadorExterno = objIndicadorExternoDA.Single(x => x.Nombre == objIndicadorExterno.Nombre && x.Borrado == 0).objObjeto;
                if (IndicadorExterno == null || (IndicadorExterno.Nombre == objIndicadorExterno.Nombre
                    && IndicadorExterno.IdIndicadorExterno == objIndicadorExterno.IdIndicadorExterno))
                {
                    if (IndicadorExterno == null)
                    {
                        objIndicadorExterno.Nombre = objIndicadorExterno.Nombre.Trim();
                        objIndicadorExterno.Descripcion = objIndicadorExterno.Descripcion.Trim();
                        objIndicadorExterno.IdTipoValor = (int)CCatalogo.TipoValor.CANTIDAD;
                        objRespuesta = objIndicadorExternoDA.Editar(objIndicadorExterno);
                        objIndicadorExterno.IdTipoValor = (int)CCatalogo.TipoValor.CANTIDAD;
                    }
                    else
                    {
                        IndicadorExterno.Nombre = objIndicadorExterno.Nombre.Trim();
                        IndicadorExterno.IdFuenteExterna = objIndicadorExterno.IdFuenteExterna;
                        IndicadorExterno.Descripcion = objIndicadorExterno.Descripcion.Trim();
                        IndicadorExterno.IdTipoValor = objIndicadorExterno.IdTipoValor;
                        IndicadorExterno.IdTipoValor = (int)CCatalogo.TipoValor.CANTIDAD;
                        objRespuesta = objIndicadorExternoDA.Editar(IndicadorExterno);
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
        public Respuesta<List<IndicadorExterno>> ConsultarTodos()
        {
            Respuesta<List<IndicadorExterno>> objRespuesta = new Respuesta<List<IndicadorExterno>>();
            try
            {
                objRespuesta = objIndicadorExternoDA.ConsultarTodos();
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
        public Respuesta<List<IndicadorExterno>> gFiltrarIndicadorExterno(string nombreFuente, string nombreIndicador, int? idFuenteExterna, string IdIndicadorExterno)
        {
            Respuesta<List<IndicadorExterno>> objRespuesta = new Respuesta<List<IndicadorExterno>>();
            try
            {
                objRespuesta = objIndicadorExternoDA.gFiltrarIndicadorExterno(nombreFuente, nombreIndicador, idFuenteExterna ?? 0, IdIndicadorExterno);
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
        public Respuesta<IndicadorExterno> ConsultarPorExpresion(System.Linq.Expressions.Expression<Func<IndicadorExterno, bool>> expression)
        {
            try
            {
                objRespuesta = objIndicadorExternoDA.Single(expression);
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
        public Respuesta<IndicadorExterno> Eliminar(string id)
        {
            Respuesta<IndicadorExterno> objRespuesta = new Respuesta<IndicadorExterno>();
            try
            {
                objRespuesta = objIndicadorExternoDA.Eliminar(id);
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
