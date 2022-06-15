using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.Entity;

namespace GB.SUTEL.DAL.Mantenimientos
{
    public class AgrupacionDetalleAD
    {


        #region metodos

        /// <summary>
        /// Agregar detalle agrupación
        /// </summary>
        /// <param name="poDetalleAgrupacion"></param>
        /// <returns></returns>
        public Respuesta<DetalleAgrupacion> gAgregarDetalleAgrupacion(DetalleAgrupacion poDetalleAgrupacion)
        {
            Respuesta<DetalleAgrupacion> objRespuesta = new Respuesta<DetalleAgrupacion>();

            try
            {

                using (TransactionScope scope = new TransactionScope())
                {

                    using (SUTEL_IndicadoresEntities context = new SUTEL_IndicadoresEntities())
                    {
                        
                        context.DetalleAgrupacion.Add(poDetalleAgrupacion);
                        context.SaveChanges();
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                objRespuesta.toError(ex.Message, poDetalleAgrupacion);
            }

            return objRespuesta;
        }
        /// <summary>
        /// Edita unan detalle agrupación
        /// </summary>
        /// <param name="poAgrupacion"></param>
        /// <returns></returns>
        public Respuesta<DetalleAgrupacion> gEditarDetalleAgrupacion(DetalleAgrupacion poDetalleAgrupacion)
        {
            Respuesta<DetalleAgrupacion> objRespuesta = new Respuesta<DetalleAgrupacion>();
           
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SUTEL_IndicadoresEntities context = new SUTEL_IndicadoresEntities())
                    {

                        context.DetalleAgrupacion.Attach(poDetalleAgrupacion);
                        context.Entry(poDetalleAgrupacion).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                objRespuesta.toError(ex.Message, poDetalleAgrupacion);
            }

            return objRespuesta;
        }
        /// <summary>
        /// Elimina logicamente el detalle agrupacion
        /// </summary>
        /// <param name="poAgrupacion"></param>
        /// <returns></returns>
        public Respuesta<DetalleAgrupacion> gEliminarDetalleAgrupacion(DetalleAgrupacion poDetalleAgrupacion)
        {
            Respuesta<DetalleAgrupacion> objRespuesta = new Respuesta<DetalleAgrupacion>();
            DetalleAgrupacion detalleAgrupacion = new DetalleAgrupacion();
            try
            {
               
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SUTEL_IndicadoresEntities context = new SUTEL_IndicadoresEntities())
                    {
                        //detalleAgrupacion = context.DetalleAgrupacion.Where(x => x.IdDetalleAgrupacion.Equals(poDetalleAgrupacion.IdDetalleAgrupacion)).FirstOrDefault();
                        detalleAgrupacion = context.DetalleAgrupacion.FirstOrDefault();
                        if (detalleAgrupacion != null) { 
                        context.Entry(detalleAgrupacion).State = EntityState.Modified;
                        detalleAgrupacion.Borrado = 1;
                        context.DetalleAgrupacion.Attach(detalleAgrupacion);

                        context.SaveChanges();
                        }
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                objRespuesta.toError(ex.Message, poDetalleAgrupacion);
            }

            return objRespuesta;
        }
        /// <summary>
        /// Obtiene los detalles agrupaciones
        /// </summary>
        /// <returns></returns>
        public Respuesta<List<DetalleAgrupacion>> gObtenerDetallesAgrupaciones()
        {
            List<DetalleAgrupacion> resultado = new List<DetalleAgrupacion>();
            Respuesta<List<DetalleAgrupacion>> objRespuesta = new Respuesta<List<DetalleAgrupacion>>();
            try
            {
                using (SUTEL_IndicadoresEntities context = new SUTEL_IndicadoresEntities())
                {
                    resultado = (from agrupacionEntidad in context.DetalleAgrupacion
                                 where agrupacionEntidad.Borrado == 0
                                 select agrupacionEntidad).ToList();
                }
                objRespuesta.objObjeto = resultado;
            }
            catch (Exception ex)
            {
                objRespuesta.toError(ex.Message, resultado);
            }
            return objRespuesta;

        }

        /// <summary>
        /// Obtiene las agrupaciones
        /// </summary>
        /// <returns></returns>
        public Respuesta<DetalleAgrupacion> gObtenerDetalleAgrupacion(int piIdAgrupacion)
        {
            DetalleAgrupacion resultado = new DetalleAgrupacion();
            Respuesta<DetalleAgrupacion> objRespuesta = new Respuesta<DetalleAgrupacion>();
            try
            {
                using (SUTEL_IndicadoresEntities context = new SUTEL_IndicadoresEntities())
                {
                    resultado = (from detalleAgrupacionEntidad in context.DetalleAgrupacion
                                 where detalleAgrupacionEntidad.Borrado == 0
                                 //&& detalleAgrupacionEntidad.IdDetalleAgrupacion == piIdAgrupacion
                                 select detalleAgrupacionEntidad).FirstOrDefault();
                }
                objRespuesta.objObjeto = resultado;
            }
            catch (Exception ex)
            {
                objRespuesta.toError(ex.Message, resultado);
            }
            return objRespuesta;

        }


        /// <summary>
        /// Obtiene los detalles de agrupaciones por la descripción
        /// </summary>
        /// <returns></returns>
        public Respuesta<DetalleAgrupacion> gObtenerDetalleAgrupacionNombre(String psDescripcionAgrupacion)
        {
            DetalleAgrupacion resultado = new DetalleAgrupacion();
            Respuesta<DetalleAgrupacion> objRespuesta = new Respuesta<DetalleAgrupacion>();
            try
            {
                using (SUTEL_IndicadoresEntities context = new SUTEL_IndicadoresEntities())
                {
                    resultado = (from detalleAgrupacionEntidad in context.DetalleAgrupacion
                                 where detalleAgrupacionEntidad.Borrado == 0
                                 && detalleAgrupacionEntidad.DescDetalleAgrupacion.ToUpper().Equals(psDescripcionAgrupacion.ToUpper())
                                 select detalleAgrupacionEntidad).FirstOrDefault();
                }
                if (resultado == null)
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                }
                objRespuesta.objObjeto = resultado;
            }
            catch (Exception ex)
            {
                objRespuesta.toError(ex.Message, resultado);
            }
            return objRespuesta;

        }




        /// <summary>
        /// Obtiene los detalle agrupación por id de agrupación
        /// </summary>
        /// <param name="piIdAgrupacion"></param>
        /// <returns></returns>
        public Respuesta<List<DetalleAgrupacion>> gObtenerAgrupacionDetallePorAgrupacion(int piIdAgrupacion){
            Respuesta<List<DetalleAgrupacion>> objRespuesta = new Respuesta<List<DetalleAgrupacion>>();
            List<DetalleAgrupacion> detallesAgrupacion = new List<DetalleAgrupacion>();
            try
            {

                

                    using (SUTEL_IndicadoresEntities context = new SUTEL_IndicadoresEntities())
                    {
                       detallesAgrupacion=context.DetalleAgrupacion.Include("Agrupacion").Where(x=> x.IdAgrupacion.Equals(piIdAgrupacion) && x.Agrupacion.Borrado==0 && x.Borrado==0).ToList();
                       objRespuesta.objObjeto = detallesAgrupacion;
                       
                    }
                   
                
            }
            catch (Exception ex)
            {
                objRespuesta.toError(ex.Message, detallesAgrupacion);
            }

            return objRespuesta;
        }


        


        #endregion
    }
}
