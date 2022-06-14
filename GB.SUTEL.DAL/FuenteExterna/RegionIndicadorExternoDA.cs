using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.Shared;
using GB.SUTEL.ExceptionHandler;

namespace GB.SUTEL.DAL.FuenteExternas
{
    public class RegionIndicadorExternoDA : LocalContextualizer
    {
        private SUTEL_IndicadoresEntities objContext;
        private Respuesta<RegionIndicadorExterno> objRespuesta;
        public RegionIndicadorExternoDA(ApplicationContext appContext) : base(appContext)
        {
            objContext = new SUTEL_IndicadoresEntities();
            objRespuesta = new Respuesta<RegionIndicadorExterno>();
        }
        #region Agregar
        public Respuesta<RegionIndicadorExterno> Agregar(RegionIndicadorExterno objRegionIndicadorExterno)
        {            
            try
            {
                objContext.RegionIndicadorExterno.Add(objRegionIndicadorExterno);
                objContext.SaveChanges();
                objRespuesta.objObjeto = objRegionIndicadorExterno;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj,ex);               
            }
            return objRespuesta;
        }
        #endregion

        #region Editar
        public Respuesta<RegionIndicadorExterno[]> Editar(RegionIndicadorExterno objREGIONINDICADOREXTERNO)
        {
            try
            {
                RegionIndicadorExterno[] arrREGIONINDICADOREXTERNO = new RegionIndicadorExterno[2];
                arrREGIONINDICADOREXTERNO[0] = objREGIONINDICADOREXTERNO;
                SUTEL_IndicadoresEntities objContext2 = new SUTEL_IndicadoresEntities();
                arrREGIONINDICADOREXTERNO[1] = objContext2.RegionIndicadorExterno.SingleOrDefault(x => x.IdRegionIndicadorExterno == objREGIONINDICADOREXTERNO.IdRegionIndicadorExterno);
                objContext2.Dispose();
                Respuesta<RegionIndicadorExterno[]> objRespuestaEditar = new Respuesta<RegionIndicadorExterno[]>();
                objContext.Entry(objREGIONINDICADOREXTERNO).State = EntityState.Modified;
                objContext.SaveChanges();
                objRespuestaEditar.objObjeto = arrREGIONINDICADOREXTERNO;
                return objRespuestaEditar;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
        }
        public Respuesta<RegionIndicadorExterno> Editar(RegionIndicadorExterno objREGIONINDICADOREXTERNO, int? nullable)
        {
            try
            {                
                objContext.Entry(objREGIONINDICADOREXTERNO).State = EntityState.Modified;
                objContext.SaveChanges();
                objRespuesta.objObjeto = objREGIONINDICADOREXTERNO;
                return objRespuesta;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
        }
        #endregion

        #region ConsultarPorExpresión
        public Respuesta<RegionIndicadorExterno> Single(System.Linq.Expressions.Expression<Func<RegionIndicadorExterno, bool>> expression)
        {
            try
            {
                objRespuesta.objObjeto = objContext.RegionIndicadorExterno.SingleOrDefault(expression);                
            }
            catch (Exception ex)
            {      
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);                 
            }
            return objRespuesta;
        }
        #endregion

        #region ConsultarTodos
        public Respuesta<List<RegionIndicadorExterno>> ConsultarTodos()
        {
            Respuesta<List<RegionIndicadorExterno>> RespuestaObj = new Respuesta<List<RegionIndicadorExterno>>();
            List<RegionIndicadorExterno> oUseres = new List<RegionIndicadorExterno>();
            try
            {
                oUseres = objContext.RegionIndicadorExterno.Where(x => x.Borrado == 0).OrderBy(p => p.DescRegionIndicadorExterno).ToList();
                RespuestaObj.objObjeto = oUseres;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                RespuestaObj.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
            return RespuestaObj;
        }
        public Respuesta<List<RegionIndicadorExterno>> ConsultarTodos(RegistroIndicadorExterno RegistroIndicadorExterno)
        {
            Respuesta<List<RegionIndicadorExterno>> RespuestaObj = new Respuesta<List<RegionIndicadorExterno>>();
            List<RegionIndicadorExterno> oUseres = new List<RegionIndicadorExterno>();
            try
            {
                oUseres = objContext.RegionIndicadorExterno.Where(x => x.Borrado == 0||
                    RegistroIndicadorExterno.IdRegionIndicadorExterno == x.IdRegionIndicadorExterno).OrderBy(p => p.DescRegionIndicadorExterno).ToList();
                RespuestaObj.objObjeto = oUseres;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                RespuestaObj.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
            return RespuestaObj;
        }
        #endregion

        #region Filtro
        public Respuesta<List<RegionIndicadorExterno>> gFiltrarRegionIndicadorExterno(int id, string nombre)
        {
            Respuesta<List<RegionIndicadorExterno>> RespuestaObj = new Respuesta<List<RegionIndicadorExterno>>();
            List<RegionIndicadorExterno> oUseres = new List<RegionIndicadorExterno>();
            try
            {
                oUseres = objContext.RegionIndicadorExterno.Where(x => (x.Borrado == 0) && (id.Equals(0) || x.IdRegionIndicadorExterno.ToString().Contains(id.ToString()))
                    && (nombre.Equals("") || x.DescRegionIndicadorExterno.ToUpper().Contains(nombre.ToUpper()))).OrderBy(p => p.DescRegionIndicadorExterno).ToList();
                RespuestaObj.objObjeto = oUseres;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                RespuestaObj.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
            return RespuestaObj;
        }
        #endregion        

        #region Eliminar
        public Respuesta<RegionIndicadorExterno> Eliminar(int id)
        {
            try
            {
                var usuarioEliminardo = objContext.RegionIndicadorExterno.SingleOrDefault(x => x.IdRegionIndicadorExterno == id);
                usuarioEliminardo.Borrado = 1;
                //objContext.Entry(objRegionIndicadorExterno).State = EntityState.Deleted;
                objRespuesta.objObjeto = usuarioEliminardo;
                objContext.SaveChanges();
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
    }
}
