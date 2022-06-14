using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.Shared;
using GB.SUTEL.ExceptionHandler;
using System.Data.SqlClient;

namespace GB.SUTEL.DAL.Mantenimientos
{
    public class ServicioIndicadorDA : LocalContextualizer
    {
        private SUTEL_IndicadoresEntities objContext;
        public ServicioIndicadorDA(ApplicationContext appContext) : base(appContext)
        {
            objContext = new SUTEL_IndicadoresEntities();
        }
        #region Agregar
        public void ActualizarServicio(Servicio Servicio)
        {            
            try
            {
                 objContext.Entry(Servicio).State = EntityState.Modified;
                objContext.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj,ex);               
            }
        }
        #endregion

        #region ConsultarServicio
        public Servicio SingleServicio(System.Linq.Expressions.Expression<Func<Servicio, bool>> expression)
        {
            try
            {
                return objContext.Servicio.SingleOrDefault(expression);  
            }
            catch (Exception ex)
            {      
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);                 
            }
        }
        #endregion

        #region ConsultarIndicadores
        public List<Indicador> ListIndicadores(System.Linq.Expressions.Expression<Func<Indicador, bool>> expression)
        {
            try
            {
                return objContext.Indicador.Where(expression).ToList();
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
        }
        #endregion

        #region ConsultarTodosServicios
        public List<Servicio> ConsultarTodosServicios()
        {
            try
            {
                var servicios = objContext.Servicio.Where(x => x.Borrado == 0).OrderBy(a => a.DesServicio);                
                return servicios.ToList();
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
        public List<Servicio> Filtrar(string Servicio, string pIndicador, string Direccion)
        {
            Respuesta<List<Servicio>> respuesta = new Respuesta<List<Servicio>>();
            try
            {
                 var query = (from a in objContext.Servicio
                             join b in objContext.ServicioIndicador on a.IdServicio equals b.IdServicio
                             join c in objContext.Indicador on b.IdIndicador equals c.IdIndicador
                            // join d in objContext.indicadordireccion on 
                             where
                                                        
                             (a.DesServicio.ToUpper().Contains(Servicio.ToUpper()) || Servicio == string.Empty) &&
                             (c.NombreIndicador.ToUpper().Contains(pIndicador.ToUpper()) || pIndicador == string.Empty) &&
                             //(c.Direccion..ToUpper().Contains(pIndicador.ToUpper()) || pIndicador == string.Empty) &&
                             a.Borrado == 0
                             select new
                             {
                                 IdServicio = a.IdServicio,
                                 DesServicio = a.DesServicio,
                                 Borrado = a.Borrado,
                                 Indicador = objContext.Indicador.Where(x => x.IdIndicador == b.IdIndicador).ToList()
                             }).ToList();

                List<Servicio> servicios = new List<Servicio>();

                foreach (var item in query)
                {
                    servicios.Add(new Servicio()
                    {
                        IdServicio = item.IdServicio,
                        DesServicio = item.DesServicio,
                        Borrado = item.Borrado,
                        Indicador = item.Indicador
                    });
                }

                return servicios;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
        }
        #endregion

        public List<Servicio> ObtenerIndicadoresSeleccionados(int searchid)
        {
            try
            {
                // return objContext.Servicio.SingleOrDefault(x => x.IdServicio.Equals(searchid)).ServicioIndicador.Select(z => z.IdIndicador).ToArray();

                var query = (from a in objContext.Servicio
                             join b in objContext.ServicioIndicador on a.IdServicio equals b.IdServicio
                             join c in objContext.Indicador on b.IdIndicador equals c.IdIndicador
                             where

                             (a.IdServicio == searchid)
                             select new
                             {
                                 IdServicio = a.IdServicio,
                                 DesServicio = a.DesServicio,
                                 Borrado = a.Borrado,
                                 Indicador = objContext.Indicador.Where(x => x.IdIndicador == b.IdIndicador).ToList()
                             }).ToList();

                List<Servicio> servicios = new List<Servicio>();

                foreach (var item in query)
                {
                    servicios.Add(new Servicio()
                    {
                        IdServicio = item.IdServicio,
                        DesServicio = item.DesServicio,
                        Borrado = item.Borrado,
                        Indicador = item.Indicador
                    });
                }

                return servicios;
                //return objContext.Servicio.SingleOrDefault(x => x.IdServicio.Equals(searchid)).Indicador.Select(z => z.IdIndicador).ToArray();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
        }
    }
}
