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
    public class RegionIndicadorExternoBL : LocalContextualizer
    {
        private Respuesta<RegionIndicadorExterno> objRespuesta;
        private RegionIndicadorExternoDA objRegionIndicadorExternoDA;
        public RegionIndicadorExternoBL(ApplicationContext appContext)
            : base(appContext)
        {
            objRespuesta = new Respuesta<RegionIndicadorExterno>();
            objRegionIndicadorExternoDA = new RegionIndicadorExternoDA(appContext);
        }

        #region Agregar
        public Respuesta<RegionIndicadorExterno> Agregar(RegionIndicadorExterno objRegionIndicadorExterno)
        {
            try
            {   
                if (objRegionIndicadorExternoDA.Single(x => x.DescRegionIndicadorExterno == objRegionIndicadorExterno.DescRegionIndicadorExterno && x.Borrado == 0).objObjeto == null)
                {
                    objRegionIndicadorExterno.DescRegionIndicadorExterno = objRegionIndicadorExterno.DescRegionIndicadorExterno.Trim();
                    objRespuesta = objRegionIndicadorExternoDA.Agregar(objRegionIndicadorExterno);
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
        public Respuesta<RegionIndicadorExterno[]> Editar(RegionIndicadorExterno objRegionIndicadorExterno)
        {
            Respuesta<RegionIndicadorExterno[]> objRespuesta = new Respuesta<RegionIndicadorExterno[]>();
            try
            {
                var RegionIndicadorExterno = objRegionIndicadorExternoDA.Single(x => x.DescRegionIndicadorExterno == objRegionIndicadorExterno.DescRegionIndicadorExterno && x.Borrado == 0).objObjeto;
                if (RegionIndicadorExterno == null || (RegionIndicadorExterno.DescRegionIndicadorExterno == objRegionIndicadorExterno.DescRegionIndicadorExterno
                    && RegionIndicadorExterno.IdRegionIndicadorExterno == objRegionIndicadorExterno.IdRegionIndicadorExterno))
                {
                    if (RegionIndicadorExterno == null)
                    {
                        objRegionIndicadorExterno.DescRegionIndicadorExterno = objRegionIndicadorExterno.DescRegionIndicadorExterno.Trim();
                        objRespuesta = objRegionIndicadorExternoDA.Editar(objRegionIndicadorExterno);
                    }
                    else
                    {
                        RegionIndicadorExterno.DescRegionIndicadorExterno = objRegionIndicadorExterno.DescRegionIndicadorExterno.Trim(); ;
                        objRespuesta = objRegionIndicadorExternoDA.Editar(RegionIndicadorExterno);
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
        public Respuesta<List<RegionIndicadorExterno>> ConsultarTodos()
        {
            Respuesta<List<RegionIndicadorExterno>> objRespuesta = new Respuesta<List<RegionIndicadorExterno>>();
            try
            {
                objRespuesta = objRegionIndicadorExternoDA.ConsultarTodos();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        public Respuesta<List<RegionIndicadorExterno>> ConsultarTodos(RegistroIndicadorExterno RegistroIndicadorExterno)
        {
            Respuesta<List<RegionIndicadorExterno>> objRespuesta = new Respuesta<List<RegionIndicadorExterno>>();
            try
            {
                objRespuesta = objRegionIndicadorExternoDA.ConsultarTodos(RegistroIndicadorExterno);
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
        public Respuesta<List<RegionIndicadorExterno>> gFiltrarRegionIndicadorExterno(int id, string nombre)
        {
            Respuesta<List<RegionIndicadorExterno>> objRespuesta = new Respuesta<List<RegionIndicadorExterno>>();
            try
            {
                objRespuesta = objRegionIndicadorExternoDA.gFiltrarRegionIndicadorExterno(id, nombre);
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
        public Respuesta<RegionIndicadorExterno> ConsultarPorExpresion(System.Linq.Expressions.Expression<Func<RegionIndicadorExterno, bool>> expression)
        {
            try
            {
                objRespuesta = objRegionIndicadorExternoDA.Single(expression);
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
        public Respuesta<RegionIndicadorExterno> Eliminar(int id)
        {
            Respuesta<RegionIndicadorExterno> objRespuesta = new Respuesta<RegionIndicadorExterno>();
            try
            {
                objRespuesta = objRegionIndicadorExternoDA.Eliminar(id);
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
