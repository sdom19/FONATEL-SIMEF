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
    public class RegistroIndicadorExternoDA : LocalContextualizer
    {
        private SUTEL_IndicadoresEntities objContext;
        private Respuesta<RegistroIndicadorExterno> objRespuesta;
        public RegistroIndicadorExternoDA(ApplicationContext appContext) : base(appContext)
        {
            objContext = new SUTEL_IndicadoresEntities();
            objRespuesta = new Respuesta<RegistroIndicadorExterno>();
        }
        #region Agregar
        public Respuesta<RegistroIndicadorExterno> Agregar(RegistroIndicadorExterno objRegistroIndicadorExterno)
        {            
            try
            {
                objRegistroIndicadorExterno.Modificado = 0;
                objContext.RegistroIndicadorExterno.Add(objRegistroIndicadorExterno);
                objContext.SaveChanges();
                objRespuesta.objObjeto = objRegistroIndicadorExterno;
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
        public Respuesta<RegistroIndicadorExterno[]> Editar(RegistroIndicadorExterno objREGISTROINDICADOREXTERNO)
        {
            try
            {
                RegistroIndicadorExterno[] arrREGISTROINDICADOREXTERNO = new RegistroIndicadorExterno[2];
                Respuesta<RegistroIndicadorExterno[]> objRespuestaEditar = new Respuesta<RegistroIndicadorExterno[]>();
                arrREGISTROINDICADOREXTERNO[0] = objREGISTROINDICADOREXTERNO;
                SUTEL_IndicadoresEntities objContext2 = new SUTEL_IndicadoresEntities();
                arrREGISTROINDICADOREXTERNO[1] = objContext2.RegistroIndicadorExterno.SingleOrDefault(x => x.IdRegistroIndicadorExterno == objREGISTROINDICADOREXTERNO.IdRegistroIndicadorExterno);
                objContext2.Dispose();
                objREGISTROINDICADOREXTERNO.Modificado = 1;
                objREGISTROINDICADOREXTERNO.FechaModificacion = DateTime.Now;
                objContext.Entry(objREGISTROINDICADOREXTERNO).State = EntityState.Modified;
                objContext.SaveChanges();
                objRespuestaEditar.objObjeto = arrREGISTROINDICADOREXTERNO;
                return objRespuestaEditar;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
        }
        public Respuesta<RegistroIndicadorExterno> Editar(RegistroIndicadorExterno objREGISTROINDICADOREXTERNO, int? nullable)
        {
            try
            {                
                objContext.Entry(objREGISTROINDICADOREXTERNO).State = EntityState.Modified;
                objContext.SaveChanges();
                objRespuesta.objObjeto = objREGISTROINDICADOREXTERNO;
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
        public Respuesta<RegistroIndicadorExterno> Single(System.Linq.Expressions.Expression<Func<RegistroIndicadorExterno, bool>> expression)
        {
            try
            {
                objRespuesta.objObjeto = objContext.RegistroIndicadorExterno.SingleOrDefault(expression);                
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
        public Respuesta<List<RegistroIndicadorExterno>> ConsultarTodos()
        {
            Respuesta<List<RegistroIndicadorExterno>> RespuestaObj = new Respuesta<List<RegistroIndicadorExterno>>();
            List<RegistroIndicadorExterno> oUseres = new List<RegistroIndicadorExterno>();
            try
            {
                oUseres = objContext.RegistroIndicadorExterno.Where(x => !x.Borrado).Take(100).ToList();
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
        public Respuesta<List<Trimestre>> ConsultarTrimestres()
        {
            Respuesta<List<Trimestre>> RespuestaObj = new Respuesta<List<Trimestre>>();
            List<Trimestre> oUseres = new List<Trimestre>();
            try
            {
                oUseres = objContext.Trimestre.OrderBy(p => p.Nombre).ToList();
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
        /// <summary>
        /// consulta de provincias
        /// </summary>
        /// <returns></returns>
        public Respuesta<List<Provincia>> gConsultarProvincias()
        {
            Respuesta<List<Provincia>> RespuestaObj = new Respuesta<List<Provincia>>();
            List<Provincia> oProvincia = new List<Provincia>();
            try
            {
                oProvincia = objContext.Provincia.OrderBy(p => p.Nombre).ToList();
                RespuestaObj.objObjeto = oProvincia;
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

        public Respuesta<List<Canton>> ConsultarCantones()
        {
            Respuesta<List<Canton>> RespuestaObj = new Respuesta<List<Canton>>();
            List<Canton> oCanton = new List<Canton>();
            try
            {
                oCanton = objContext.Canton.OrderBy(p => p.Nombre).ToList();
                RespuestaObj.objObjeto = oCanton;
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
        public Respuesta<List<Canton>> ConsultarCantonesXProvincia(int idProvincia)
        {
            Respuesta<List<Canton>> RespuestaObj = new Respuesta<List<Canton>>();
            List<Canton> oCanton = new List<Canton>();
            try
            {
                oCanton = objContext.Canton.Where(x => x.IdProvincia == idProvincia).Take(100).ToList();
                //oCanton = objContext.Canton.OrderBy(p => p.Nombre).ToList();
                RespuestaObj.objObjeto = oCanton;
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
        public Respuesta<List<Genero>> ConsultarGeneros()
        {
            Respuesta<List<Genero>> RespuestaObj = new Respuesta<List<Genero>>();
            List<Genero> oCanton = new List<Genero>();
            try
            {
                oCanton = objContext.Genero.OrderBy(p => p.Descripcion).ToList();
                RespuestaObj.objObjeto = oCanton;
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
        public Respuesta<List<RegistroIndicadorExterno>> gFiltrarRegistroIndicadorExterno(string fuente,string indicador,double valor,
            string anno,string zona,string region)
        {
            Respuesta<List<RegistroIndicadorExterno>> RespuestaObj = new Respuesta<List<RegistroIndicadorExterno>>();
            List<RegistroIndicadorExterno> oUseres = new List<RegistroIndicadorExterno>();
            try
            {
                oUseres = objContext.RegistroIndicadorExterno.Where(x => (!x.Borrado) &&
                    (fuente.Equals("") || x.IndicadorExterno.FuenteExterna.NombreFuenteExterna.ToUpper().Contains(fuente.ToUpper()))&&
                    (indicador.Equals("") || x.IndicadorExterno.Nombre.ToUpper().Contains(indicador.ToUpper()))&&
                    (valor.Equals(0) || x.ValorIndicador.ToString().Contains(valor.ToString()))&&
                    (anno.Equals("") || x.Anno.ToString().ToUpper().Contains(anno.ToUpper())) &&
                    (zona.Equals("") || x.ZonaIndicadorExterno.DescZonaIndicadorExterno.ToUpper().Contains(zona.ToUpper())) &&
                    (region.Equals("") || x.Canton.Nombre.ToUpper().Contains(region.ToUpper()))).Take(100).ToList();
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
        public Respuesta<RegistroIndicadorExterno> Eliminar(Guid id)
        {
            try
            {
                var usuarioEliminardo = objContext.RegistroIndicadorExterno.SingleOrDefault(x => x.IdRegistroIndicadorExterno == id);
                usuarioEliminardo.Borrado = true;
                //objContext.Entry(objRegistroIndicadorExterno).State = EntityState.Deleted;
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
