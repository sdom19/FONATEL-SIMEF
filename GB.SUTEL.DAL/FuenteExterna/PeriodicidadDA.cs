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
    public class PeriodicidadDA : LocalContextualizer
    {
        private SUTEL_IndicadoresEntities objContext;
        private Respuesta<Periodicidad> objRespuesta;
        public PeriodicidadDA(ApplicationContext appContext) : base(appContext)
        {
            objContext = new SUTEL_IndicadoresEntities();
            objRespuesta = new Respuesta<Periodicidad>();
        }
        #region Agregar
        public Respuesta<Periodicidad> Agregar(Periodicidad objPeriodicidad)
        {            
            try
            {
                objContext.Periodicidad.Add(objPeriodicidad);
                objContext.SaveChanges();
                objRespuesta.objObjeto = objPeriodicidad;
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
        public Respuesta<Periodicidad[]> Editar(Periodicidad objPERIODICIDAD)
        {
            try
            {
                Periodicidad[] arrPERIODICIDAD = new Periodicidad[2];
                arrPERIODICIDAD[0] = objPERIODICIDAD;
                SUTEL_IndicadoresEntities objContext2 = new SUTEL_IndicadoresEntities();
                arrPERIODICIDAD[1] = objContext2.Periodicidad.SingleOrDefault(x => x.IdPeridiocidad == objPERIODICIDAD.IdPeridiocidad);
                objContext2.Dispose();
                Respuesta<Periodicidad[]> objRespuestaEditar = new Respuesta<Periodicidad[]>();
                objContext.Entry(objPERIODICIDAD).State = EntityState.Modified;
                objContext.SaveChanges();
                objRespuestaEditar.objObjeto = arrPERIODICIDAD;
                return objRespuestaEditar;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
        }
        public Respuesta<Periodicidad> Editar(Periodicidad objPERIODICIDAD, int? nullable)
        {
            try
            {                
                objContext.Entry(objPERIODICIDAD).State = EntityState.Modified;
                objContext.SaveChanges();
                objRespuesta.objObjeto = objPERIODICIDAD;
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
        public Respuesta<Periodicidad> Single(System.Linq.Expressions.Expression<Func<Periodicidad, bool>> expression)
        {
            try
            {
                objRespuesta.objObjeto = objContext.Periodicidad.SingleOrDefault(expression);                
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
        public Respuesta<List<Periodicidad>> ConsultarTodos()
        {
            Respuesta<List<Periodicidad>> RespuestaObj = new Respuesta<List<Periodicidad>>();
            List<Periodicidad> oUseres = new List<Periodicidad>();
            try
            {
                oUseres = objContext.Periodicidad.Where(x => x.Borrado == 0).OrderBy(p => p.DescPeriodicidad).ToList();
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
        public Respuesta<List<Periodicidad>> ConsultarTodos(RegistroIndicadorExterno RegistroIndicadorExterno)
        {
            Respuesta<List<Periodicidad>> RespuestaObj = new Respuesta<List<Periodicidad>>();
            List<Periodicidad> oUseres = new List<Periodicidad>();
            try
            {
                oUseres = objContext.Periodicidad.Where(x => x.Borrado == 0||
                    RegistroIndicadorExterno.IdPeridiocidad == x.IdPeridiocidad).OrderBy(p => p.DescPeriodicidad).ToList();
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
        public Respuesta<List<Periodicidad>> gFiltrarPeriodicidad(int id, string nombre)
        {
            Respuesta<List<Periodicidad>> RespuestaObj = new Respuesta<List<Periodicidad>>();
            List<Periodicidad> oUseres = new List<Periodicidad>();
            try
            {
                oUseres = objContext.Periodicidad.Where(x => (x.Borrado == 0) && (id.Equals(0) || x.DescPeriodicidad.ToString().Contains(id.ToString()))
                    && (nombre.Equals("") || x.DescPeriodicidad.ToUpper().Contains(nombre.ToUpper()))).OrderBy(p => p.DescPeriodicidad).ToList();
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
        public Respuesta<Periodicidad> Eliminar(int id)
        {
            try
            {
                var usuarioEliminardo = objContext.Periodicidad.SingleOrDefault(x => x.IdPeridiocidad == id);
                usuarioEliminardo.Borrado = 1;
                //objContext.Entry(objPeriodicidad).State = EntityState.Deleted;
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
