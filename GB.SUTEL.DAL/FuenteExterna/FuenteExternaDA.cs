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
    public class FuenteExternaDA : LocalContextualizer
    {
        private SUTEL_IndicadoresEntities objContext;
        private Respuesta<FuenteExterna> objRespuesta;
        public FuenteExternaDA(ApplicationContext appContext) : base(appContext)
        {
            objContext = new SUTEL_IndicadoresEntities();
            objRespuesta = new Respuesta<FuenteExterna>();
        }
        #region Agregar
        public Respuesta<FuenteExterna> Agregar(FuenteExterna objFuenteExterna)
        {            
            try
            {
                objContext.FuenteExterna.Add(objFuenteExterna);
                objContext.SaveChanges();
                objRespuesta.objObjeto = objFuenteExterna;
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
        public Respuesta<FuenteExterna[]> Editar(FuenteExterna objFUENTEEXTERNA)
        {
            try
            {
                FuenteExterna[] arrFUENTEEXTERNA = new FuenteExterna[2];
                Respuesta<FuenteExterna[]> objRespuestaEditar = new Respuesta<FuenteExterna[]>();
                arrFUENTEEXTERNA[0] = objFUENTEEXTERNA;
                SUTEL_IndicadoresEntities objContext2 = new SUTEL_IndicadoresEntities();
                arrFUENTEEXTERNA[1] = objContext2.FuenteExterna.SingleOrDefault(x => x.IdFuenteExterna==objFUENTEEXTERNA.IdFuenteExterna);
                objContext2.Dispose();
                objContext.Entry(objFUENTEEXTERNA).State = EntityState.Modified;
                objContext.SaveChanges();                
                objRespuestaEditar.objObjeto = arrFUENTEEXTERNA;
                return objRespuestaEditar;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
        }
        public Respuesta<FuenteExterna> Editar(FuenteExterna objFUENTEEXTERNA, int? nullable)
        {
            try
            {                
                objContext.Entry(objFUENTEEXTERNA).State = EntityState.Modified;
                objContext.SaveChanges();
                objRespuesta.objObjeto = objFUENTEEXTERNA;
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
        public Respuesta<FuenteExterna> Single(System.Linq.Expressions.Expression<Func<FuenteExterna, bool>> expression)
        {
            try
            {
                objRespuesta.objObjeto = objContext.FuenteExterna.SingleOrDefault(expression);                
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
        public Respuesta<List<FuenteExterna>> ConsultarTodos()
        {
            Respuesta<List<FuenteExterna>> RespuestaObj = new Respuesta<List<FuenteExterna>>();
            List<FuenteExterna> oUseres = new List<FuenteExterna>();
            try
            {
                oUseres = objContext.FuenteExterna.Where(x => x.Borrado==0).OrderBy(p => p.NombreFuenteExterna).ToList();
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
        public Respuesta<List<FuenteExterna>> ConsultarTodos(int? all)
        {
            Respuesta<List<FuenteExterna>> RespuestaObj = new Respuesta<List<FuenteExterna>>();
            List<FuenteExterna> oUseres = new List<FuenteExterna>();
            try
            {
                oUseres = objContext.FuenteExterna.OrderBy(p => p.NombreFuenteExterna).ToList();
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
        public Respuesta<List<FuenteExterna>> ConsultarTodos(RegistroIndicadorExterno RegistroIndicadorExterno)
        {
            Respuesta<List<FuenteExterna>> RespuestaObj = new Respuesta<List<FuenteExterna>>();
            List<FuenteExterna> oUseres = new List<FuenteExterna>();
            try
            {
                oUseres = objContext.FuenteExterna.Where(x => (x.Borrado == 0 ||
                    x.IdFuenteExterna == RegistroIndicadorExterno.IndicadorExterno.IdFuenteExterna)).OrderBy(p => p.NombreFuenteExterna).ToList();
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
        public Respuesta<List<FuenteExterna>> gFiltrarFuenteExterna(int id, string nombre)
        {
            Respuesta<List<FuenteExterna>> RespuestaObj = new Respuesta<List<FuenteExterna>>();
            List<FuenteExterna> oUseres = new List<FuenteExterna>();
            try
            {
                oUseres = objContext.FuenteExterna.Where(x => (x.Borrado == 0) && (id.Equals(0)|| x.IdFuenteExterna.ToString().Contains(id.ToString()))
                    && (nombre.Equals("") || x.NombreFuenteExterna.ToUpper().Contains(nombre.ToUpper()))).OrderBy(p => p.NombreFuenteExterna).ToList();
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
        public Respuesta<FuenteExterna> Eliminar(int id)
        {
            try
            {
                var usuarioEliminardo = objContext.FuenteExterna.SingleOrDefault(x => x.IdFuenteExterna == id);
                usuarioEliminardo.Borrado = 1;
                //objContext.Entry(objFuenteExterna).State = EntityState.Deleted;
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
