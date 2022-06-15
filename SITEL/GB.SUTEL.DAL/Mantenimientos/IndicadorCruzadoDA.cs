using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.Shared;
using GB.SUTEL.ExceptionHandler;

namespace GB.SUTEL.DAL.Mantenimientos
{
    public class IndicadorCruzadoDA : LocalContextualizer
    {
        private SUTEL_IndicadoresEntities objContext;
        private Respuesta<IndicadorCruzado> objRespuesta;
        public IndicadorCruzadoDA(ApplicationContext appContext) : base(appContext)
        {
            objContext = new SUTEL_IndicadoresEntities();
            objRespuesta = new Respuesta<IndicadorCruzado>();
        }
        #region Agregar
        public Respuesta<IndicadorCruzado> Agregar(IndicadorCruzado objIndicadorCruzado)
        {            
            try
            {
                objContext.IndicadorCruzado.Add(objIndicadorCruzado);
                objContext.SaveChanges();
                objRespuesta.objObjeto = objIndicadorCruzado;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj,ex);               
            }
            return objRespuesta;
        }
        public Respuesta<IndicadorCruzado> AgregarDetalles(List<DetalleIndicadorCruzado> objDetalleIndicadorCruzado)
        {
            try
            {
                foreach (var item in objDetalleIndicadorCruzado)
                {
                    objContext.DetalleIndicadorCruzado.Add(item);
                }                
                objContext.SaveChanges();                
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
        public Respuesta<IndicadorCruzado[]> Editar(IndicadorCruzado objINDICADORCRUZADO,string currentId)
        {
            try
            {
                IndicadorCruzado[] arrINDICADORCRUZADO = new IndicadorCruzado[2];
                Respuesta<IndicadorCruzado[]> objRespuestaEditar = new Respuesta<IndicadorCruzado[]>();
                arrINDICADORCRUZADO[0] = objINDICADORCRUZADO;
                SUTEL_IndicadoresEntities objContext2 = new SUTEL_IndicadoresEntities();
                arrINDICADORCRUZADO[1] = objContext2.IndicadorCruzado.SingleOrDefault(x => x.IdIndicadorCruzado==currentId);
                objContext2.Dispose();
                objContext.Entry(objINDICADORCRUZADO).State = EntityState.Modified;
                foreach (var item in objContext.DetalleIndicadorCruzado.Where(x => x.IdIndicadorCruzado==currentId).ToList())
                {
                    objContext.DetalleIndicadorCruzado.Remove(item);
                }
                objContext.SaveChanges();                
                objRespuestaEditar.objObjeto = arrINDICADORCRUZADO;
                return objRespuestaEditar;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
        }
        public Respuesta<IndicadorCruzado> Editar(IndicadorCruzado objINDICADORCRUZADO, int? nullable)
        {
            try
            {                
                objContext.Entry(objINDICADORCRUZADO).State = EntityState.Modified;
                objContext.SaveChanges();
                objRespuesta.objObjeto = objINDICADORCRUZADO;
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
        public Respuesta<IndicadorCruzado> Single(System.Linq.Expressions.Expression<Func<IndicadorCruzado, bool>> expression)
        {
            try
            {
                objRespuesta.objObjeto = objContext.IndicadorCruzado.SingleOrDefault(expression);                
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
        public Respuesta<List<IndicadorCruzado>> ConsultarTodos()
        {
            Respuesta<List<IndicadorCruzado>> RespuestaObj = new Respuesta<List<IndicadorCruzado>>();
            List<IndicadorCruzado> oUseres = new List<IndicadorCruzado>();
            try
            {
                oUseres = objContext.IndicadorCruzado.OrderBy(x => x.Nombre).ToList();
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
        public Respuesta<List<Direccion>> ConsultarTodosDirecciones()
        {
            Respuesta<List<Direccion>> RespuestaObj = new Respuesta<List<Direccion>>();
            try
            {
                List<Direccion> Direcciones = new List<Direccion>();
                Direcciones = objContext.Direccion.OrderBy(x => x.Nombre).ToList();
                RespuestaObj.objObjeto = Direcciones;
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
        public List<Indicador> ConsultarTodosIndicadores()
        {            
            try
            {
                return objContext.Indicador.OrderBy(x => x.NombreIndicador).Where(x => x.Borrado == 0).OrderBy(p => p.NombreIndicador).ToList();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;                
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
        }
        #endregion

        #region Filtro
        public Respuesta<List<IndicadorCruzado>> gFiltrarIndicadorCruzado(string nombre, string descripcion)
        {
            Respuesta<List<IndicadorCruzado>> RespuestaObj = new Respuesta<List<IndicadorCruzado>>();
            List<IndicadorCruzado> oUseres = new List<IndicadorCruzado>();
            try
            {
                oUseres = objContext.IndicadorCruzado.Where(x => (nombre.Equals("")|| x.Nombre.ToUpper().Contains(nombre.ToUpper()))
                    &&(descripcion.Equals("") || x.Descripcion.ToUpper().Contains(descripcion.ToUpper()))).OrderBy(x => x.Nombre).ToList();
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
        public List<Indicador> gFiltrarIndicadorInterno(string nombre, int direccion)
        {
            try
            {
                List<ServicioIndicador> listaservicioindicador = new List<ServicioIndicador>();
                listaservicioindicador= objContext.ServicioIndicador.OrderBy(x => x.IdIndicador).ToList();
                return objContext.Indicador.Where(x => (x.Borrado == 0) && 
                    (nombre.Equals("") || x.NombreIndicador.ToUpper().Contains(nombre.ToUpper()))
                    && (direccion.Equals(0))).ToList();


            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;                
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }            
        }
        
        #endregion       

        #region Eliminar
        public Respuesta<IndicadorCruzado> Eliminar(string id)
        {
            try
            {
                var Eliminado = objContext.IndicadorCruzado.SingleOrDefault(x => x.IdIndicadorCruzado == id);
                foreach (var item in Eliminado.DetalleIndicadorCruzado.ToList())
                {
                    objContext.DetalleIndicadorCruzado.Remove(item);                    
                }
                objContext.Entry(Eliminado).State = EntityState.Deleted;
                objRespuesta.objObjeto = Eliminado;
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
