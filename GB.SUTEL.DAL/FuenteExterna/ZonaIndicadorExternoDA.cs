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
    public class ZonaIndicadorExternoDA : LocalContextualizer
    {
        private SUTEL_IndicadoresEntities objContext;
        private Respuesta<ZonaIndicadorExterno> objRespuesta;
        public ZonaIndicadorExternoDA(ApplicationContext appContext) : base(appContext)
        {
            objContext = new SUTEL_IndicadoresEntities();
            objRespuesta = new Respuesta<ZonaIndicadorExterno>();
        }
        #region Agregar
        public Respuesta<ZonaIndicadorExterno> Agregar(ZonaIndicadorExterno objZonaIndicadorExterno)
        {            
            try
            {
                objContext.ZonaIndicadorExterno.Add(objZonaIndicadorExterno);
                objContext.SaveChanges();
                objRespuesta.objObjeto = objZonaIndicadorExterno;
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
        /// <summary>
        /// Método que edita un User a la base de datos
        /// </summary>
        /// <param name="objZonaIndicadorExterno">Objeto tipo User con los datos a editar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<ZonaIndicadorExterno[]> Editar(ZonaIndicadorExterno objZONAINDICADOREXTERNO)
        {
            try
            {
                ZonaIndicadorExterno[] arrZONAINDICADOREXTERNO = new ZonaIndicadorExterno[2];
                arrZONAINDICADOREXTERNO[0] = objZONAINDICADOREXTERNO;
                SUTEL_IndicadoresEntities objContext2 = new SUTEL_IndicadoresEntities();
                arrZONAINDICADOREXTERNO[1] = objContext2.ZonaIndicadorExterno.SingleOrDefault(x => x.IdZonaIndicadorExterno == objZONAINDICADOREXTERNO.IdZonaIndicadorExterno);
                objContext2.Dispose();
                Respuesta<ZonaIndicadorExterno[]> objRespuestaEditar = new Respuesta<ZonaIndicadorExterno[]>();
                objContext.Entry(objZONAINDICADOREXTERNO).State = EntityState.Modified;
                objContext.SaveChanges();
                objRespuestaEditar.objObjeto = arrZONAINDICADOREXTERNO;
                return objRespuestaEditar;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
        }
        public Respuesta<ZonaIndicadorExterno> Editar(ZonaIndicadorExterno objZONAINDICADOREXTERNO, int? nullable)
        {
            try
            {                
                objContext.Entry(objZONAINDICADOREXTERNO).State = EntityState.Modified;
                objContext.SaveChanges();
                objRespuesta.objObjeto = objZONAINDICADOREXTERNO;
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
        public Respuesta<ZonaIndicadorExterno> Single(System.Linq.Expressions.Expression<Func<ZonaIndicadorExterno, bool>> expression)
        {
            try
            {
                objRespuesta.objObjeto = objContext.ZonaIndicadorExterno.SingleOrDefault(expression);                
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
        public Respuesta<List<ZonaIndicadorExterno>> ConsultarTodos()
        {
            Respuesta<List<ZonaIndicadorExterno>> RespuestaObj = new Respuesta<List<ZonaIndicadorExterno>>();
            List<ZonaIndicadorExterno> oUseres = new List<ZonaIndicadorExterno>();
            try
            {
                oUseres = objContext.ZonaIndicadorExterno.Where(x => x.Borrado == 0).OrderBy(p => p.DescZonaIndicadorExterno).ToList();
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
        public Respuesta<List<ZonaIndicadorExterno>> ConsultarTodos(RegistroIndicadorExterno RegistroIndicadorExterno)
        {
            Respuesta<List<ZonaIndicadorExterno>> RespuestaObj = new Respuesta<List<ZonaIndicadorExterno>>();
            List<ZonaIndicadorExterno> oUseres = new List<ZonaIndicadorExterno>();
            try
            {
                oUseres = objContext.ZonaIndicadorExterno.Where(x => x.Borrado == 0||
                    RegistroIndicadorExterno.IdZonaIndicadorExterno == x.IdZonaIndicadorExterno).OrderBy(p => p.DescZonaIndicadorExterno).ToList();
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

        #region Filtrar
        public Respuesta<List<ZonaIndicadorExterno>> gFiltrarZonaIndicadorExterno(int id, string nombre)
        {
            Respuesta<List<ZonaIndicadorExterno>> RespuestaObj = new Respuesta<List<ZonaIndicadorExterno>>();
            List<ZonaIndicadorExterno> oUseres = new List<ZonaIndicadorExterno>();
            try
            {
                oUseres = objContext.ZonaIndicadorExterno.Where(x => (x.Borrado == 0) && (id.Equals(0) || x.IdZonaIndicadorExterno.ToString().Contains(id.ToString()))
                    && (nombre.Equals("") || x.DescZonaIndicadorExterno.ToUpper().Contains(nombre.ToUpper()))).OrderBy(p => p.DescZonaIndicadorExterno).ToList();
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
        public Respuesta<ZonaIndicadorExterno> Eliminar(int id)
        {
            try
            {
                var usuarioEliminardo = objContext.ZonaIndicadorExterno.SingleOrDefault(x => x.IdZonaIndicadorExterno == id);
                usuarioEliminardo.Borrado = 1;
                //objContext.Entry(objZonaIndicadorExterno).State = EntityState.Deleted;
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
