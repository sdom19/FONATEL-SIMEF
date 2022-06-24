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
    public class IndicadorExternoDA : LocalContextualizer
    {
        private SUTEL_IndicadoresEntities objContext;
        private Respuesta<IndicadorExterno> objRespuesta;
        public IndicadorExternoDA(ApplicationContext appContext) : base(appContext)
        {
            objContext = new SUTEL_IndicadoresEntities();
            objRespuesta = new Respuesta<IndicadorExterno>();
        }
        #region Agregar
        public Respuesta<IndicadorExterno> Agregar(IndicadorExterno objIndicadorExterno)
        {            
            try
            {
                objContext.IndicadorExterno.Add(objIndicadorExterno);
                objContext.SaveChanges();
                objRespuesta.objObjeto = objIndicadorExterno;
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
        public Respuesta<IndicadorExterno[]> Editar(IndicadorExterno objINDICADOREXTERNO)
        {
            try
            {
                IndicadorExterno[] arrINDICADOREXTERNO = new IndicadorExterno[2];
                Respuesta<IndicadorExterno[]> objRespuestaEditar = new Respuesta<IndicadorExterno[]>();
                arrINDICADOREXTERNO[0] = objINDICADOREXTERNO;
                SUTEL_IndicadoresEntities objContext2 = new SUTEL_IndicadoresEntities();
                arrINDICADOREXTERNO[1] = objContext2.IndicadorExterno.SingleOrDefault(x => x.IdIndicadorExterno == objINDICADOREXTERNO.IdIndicadorExterno);
                objContext2.Dispose();
                objContext.Entry(objINDICADOREXTERNO).State = EntityState.Modified;
                objContext.SaveChanges();
                objRespuestaEditar.objObjeto = arrINDICADOREXTERNO;
                return objRespuestaEditar;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
        }
        public Respuesta<IndicadorExterno> Editar(IndicadorExterno objINDICADOREXTERNO, int? nullable)
        {
            try
            {                
                objContext.Entry(objINDICADOREXTERNO).State = EntityState.Modified;
                objContext.SaveChanges();
                objRespuesta.objObjeto = objINDICADOREXTERNO;
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
        public Respuesta<IndicadorExterno> Single(System.Linq.Expressions.Expression<Func<IndicadorExterno, bool>> expression)
        {
            try
            {
                objRespuesta.objObjeto = objContext.IndicadorExterno.SingleOrDefault(expression);                
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
        public Respuesta<List<IndicadorExterno>> ConsultarTodos()
        {
            Respuesta<List<IndicadorExterno>> RespuestaObj = new Respuesta<List<IndicadorExterno>>();
            List<IndicadorExterno> oUseres = new List<IndicadorExterno>();
            try
            {
                oUseres = objContext.IndicadorExterno.Where(x => x.Borrado == 0).OrderBy(p => p.Nombre).ToList();
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
        public Respuesta<List<IndicadorExterno>> gFiltrarIndicadorExterno(string nombreFuente, string nombreIndicador, int idFuenteExterna, string IdIndicadorExterno)
        {
            Respuesta<List<IndicadorExterno>> RespuestaObj = new Respuesta<List<IndicadorExterno>>();
            List<IndicadorExterno> oUseres = new List<IndicadorExterno>();
            try
            {
                oUseres = objContext.IndicadorExterno.Where(x => (x.Borrado == 0) && (nombreFuente.Equals("")|| x.FuenteExterna.NombreFuenteExterna.ToUpper().Contains(nombreFuente.ToUpper()))
                    && (nombreIndicador.Equals("") || x.Nombre.ToUpper().Contains(nombreIndicador.ToUpper()))
                    && (IdIndicadorExterno.Equals("") || x.IdIndicadorExterno.ToUpper().Contains(IdIndicadorExterno.ToUpper()))
                    && (idFuenteExterna.Equals(0) || x.FuenteExterna.IdFuenteExterna.ToString().Contains(idFuenteExterna.ToString()))).OrderBy(p => p.Nombre).ToList();
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
        public Respuesta<IndicadorExterno> Eliminar(string id)
        {
            try
            {
                var usuarioEliminardo = objContext.IndicadorExterno.SingleOrDefault(x => x.IdIndicadorExterno == id);
                usuarioEliminardo.Borrado = 1;
                //objContext.Entry(objIndicadorExterno).State = EntityState.Deleted;
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
