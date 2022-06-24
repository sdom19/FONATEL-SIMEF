using System.Data.SqlClient;
using GB.SUTEL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.Shared;

using GB.SUTEL.Entities.Utilidades;
using System.Data.Entity;

namespace GB.SUTEL.DAL.Mantenimientos
{
    public class AgrupacionAD :LocalContextualizer
    {
        #region atributos

        private SUTEL_IndicadoresEntities  context ;
        private Respuesta<Agrupacion> objRespuesta;
        #endregion

        #region metodos

        public AgrupacionAD(ApplicationContext appContext)
            : base(appContext)
        {

            context = new SUTEL_IndicadoresEntities();  
            objRespuesta = new Respuesta<Agrupacion>();
        }
        
        /// <summary>
        /// Agregar agrupación
        /// </summary>
        /// <param name="poAgrupacion"></param>
        /// <returns></returns>
        public Respuesta<Agrupacion> gAgregarAgrupacion(Agrupacion poAgrupacion)
        {
         
           
            try
            {
              
                        context.Agrupacion.Add(poAgrupacion);
                        context.SaveChanges();
                        objRespuesta.objObjeto = poAgrupacion;
            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(ex.Message, poAgrupacion);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }

            return objRespuesta;
        }
        /// <summary>
        /// Edita una agrupación
        /// </summary>
        /// <param name="poAgrupacion"></param>
        /// <returns></returns>
        public Respuesta<Agrupacion> gEditarAgrupacion(Agrupacion poAgrupacion)
        {
           Agrupacion agrupacion=new Agrupacion();
            

            try
            {

                agrupacion = context.Agrupacion.Where(x => x.IdAgrupacion == poAgrupacion.IdAgrupacion).FirstOrDefault();
                if (agrupacion != null)
                {
                    agrupacion.DescAgrupacion = poAgrupacion.DescAgrupacion;
                    context.Agrupacion.Attach(agrupacion);
                    context.Entry(agrupacion).State = EntityState.Modified;
                    context.SaveChanges();
                }
                
                objRespuesta.objObjeto = poAgrupacion;
            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(ex.Message, poAgrupacion);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
               
            }

            return objRespuesta;
        }
        /// <summary>
        /// Elimina logicamente la agrupacion
        /// </summary>
        /// <param name="poAgrupacion"></param>
        /// <returns></returns>
        public Respuesta<Agrupacion> gEliminarAgrupacion(Agrupacion poAgrupacion)
        {
            Respuesta<Agrupacion> objRespuesta = new Respuesta<Agrupacion>();
            Agrupacion agrupacion = new Agrupacion();
            try
            {
              
                  agrupacion = context.Agrupacion.Where(x => x.IdAgrupacion.Equals(poAgrupacion.IdAgrupacion)).FirstOrDefault();
                        if (agrupacion != null)
                        {
                            agrupacion.Borrado = 1;
                            context.Agrupacion.Attach(agrupacion);
                            context.Entry(agrupacion).State = EntityState.Modified;
                            context.SaveChanges();
                            objRespuesta.objObjeto = agrupacion;
                        }
                   
            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(ex.Message, poAgrupacion);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }


            return objRespuesta;
        }
        /// <summary>
        /// Obtiene las agrupaciones
        /// </summary>
        /// <returns></returns>
        public Respuesta<List<Agrupacion>> gObtenerAgrupaciones()
        {
            List<Agrupacion> resultado = new List<Agrupacion>();
            Respuesta<List<Agrupacion>> objRespuestas = new Respuesta<List<Agrupacion>>();
            try
            {
                
                    resultado = (from agrupacionEntidad in context.Agrupacion
                                 where agrupacionEntidad.Borrado==0 orderby agrupacionEntidad.DescAgrupacion ascending
                                 select agrupacionEntidad).ToList();
                
                objRespuestas.objObjeto = resultado;
            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuestas.toError(ex.Message, resultado);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }

            return objRespuestas;
        
        }


        /// <summary>
        /// Obtiene las agrupaciones
        /// </summary>
        /// <returns></returns>
        public Respuesta<List<Agrupacion>> gObtenerAgrupacionesPorFiltros(int piCodigo, String nombre)
        {
            List<Agrupacion> resultado = new List<Agrupacion>();
            Respuesta<List<Agrupacion>> objRespuestas = new Respuesta<List<Agrupacion>>();
            try
            {

                resultado = (from agrupacionEntidad in context.Agrupacion
                               where agrupacionEntidad.Borrado == 0 &&
                               (piCodigo == 0 || agrupacionEntidad.IdAgrupacion.ToString().Contains(piCodigo.ToString()))
                               && (nombre.Equals("") || agrupacionEntidad.DescAgrupacion.ToUpper().Contains(nombre.ToUpper()))
                             select agrupacionEntidad).ToList();

                objRespuestas.objObjeto = resultado;
            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuestas.toError(ex.Message, resultado);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }

            return objRespuestas;

        }

        /// <summary>
        /// Obtiene las agrupaciones
        /// </summary>
        /// <returns></returns>
        public Respuesta<Agrupacion> gObtenerAgrupacion(int piIdAgrupacion)
        {
            Agrupacion resultado = new Agrupacion();
           
            try
            {
                   resultado =(from agrupacionEntidad in context.Agrupacion
                                 where agrupacionEntidad.Borrado==0
                                 && agrupacionEntidad.IdAgrupacion==piIdAgrupacion
                                select agrupacionEntidad).FirstOrDefault();
               
                objRespuesta.objObjeto = resultado;
            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(ex.Message, resultado);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }

            return objRespuesta;

        }


        /// <summary>
        /// Obtiene las agrupaciones por la descripción
        /// </summary>
        /// <returns></returns>
        public Respuesta<Agrupacion> gObtenerAgrupacionNombre(String psDescripcionAgrupacion)
        {
            Agrupacion resultado = new Agrupacion();
            Respuesta<Agrupacion> objRespuestas = new Respuesta<Agrupacion>();
            try
            {
               resultado = (from agrupacionEntidad in context.Agrupacion
                                 where agrupacionEntidad.Borrado==0
                                 && agrupacionEntidad.DescAgrupacion.ToUpper().Equals(psDescripcionAgrupacion.ToUpper())
                                 select agrupacionEntidad).FirstOrDefault();
                
                if (resultado == null) {
                    objRespuestas.blnIndicadorTransaccion = false;
                }
                objRespuestas.objObjeto = resultado;
            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuestas.toError(ex.Message, resultado);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }

            return objRespuestas;

        }



        #endregion

    }
}
