using GB.SUTEL.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.Entities;
using GB.SUTEL.ExceptionHandler;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Data.Entity.Core.Objects;
using EntityFramework.BulkInsert.Extensions;
using System.Transactions;

namespace GB.SUTEL.DAL.Mantenimientos
{
    public class ConstructorAD : LocalContextualizer
    {
        #region atributos

        private SUTEL_IndicadoresEntities context;
        Respuesta<Constructor> objRespuesta;
        #endregion

        #region metodos
        public ConstructorAD(ApplicationContext appContext)
            : base(appContext)
        {
            context = new SUTEL_IndicadoresEntities();
            objRespuesta = new Respuesta<Constructor>();
        }

        /// <summary>
        /// Agregar un constructor
        /// </summary>
        /// <param name="poConstructor"></param>
        /// <returns></returns>
        public Respuesta<Constructor> gAgregarConstructor(Constructor poConstructor)
        {
            List<ConstructorCriterio> constructorCriterio = new List<ConstructorCriterio>();
            List<ConstructorCriterioDetalleAgrupacion> constructorDetalle = new List<ConstructorCriterioDetalleAgrupacion>();
            List<ConstructorCriterioDetalleAgrupacion> constructorDetalleAgregados = new List<ConstructorCriterioDetalleAgrupacion>();
            ConstructorCriterioDetalleAgrupacion detallePadre = new ConstructorCriterioDetalleAgrupacion();
            Regla nuevaRegla = new Regla();
            try
            {
                constructorCriterio = new List<ConstructorCriterio>(poConstructor.ConstructorCriterio);
                poConstructor.ConstructorCriterio.Clear();
                poConstructor.IdConstructor = Guid.NewGuid();
                poConstructor.FechaCreacionConstructor = DateTime.Now;
                context.Constructor.Add(poConstructor);
                context.SaveChanges();//Constructor
                lAgregarConstructorCriterio(constructorCriterio, poConstructor);
                objRespuesta.objObjeto = poConstructor;
            }
            catch (CustomException)
            {
                gEliminarConstructor(poConstructor);
                throw;
            }
            catch (Exception ex)
            {   gEliminarConstructor(poConstructor);
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(ex.Message, poConstructor);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
                
            }

            return objRespuesta;
        }
        /// <summary>
        /// Obtiene el detalle agrupación padre registrado
        /// </summary>
        /// <param name="detalles"></param>
        /// <param name="detalle"></param>
        /// <returns></returns>
        public ConstructorCriterioDetalleAgrupacion lObtenerPadreDetalle(List<ConstructorCriterioDetalleAgrupacion> detalles, ConstructorCriterioDetalleAgrupacion detalle)
        {
            foreach (ConstructorCriterioDetalleAgrupacion item in detalles)
            {
                if (item.DetalleAgrupacion.IdOperador.Equals(detalle.ConstructorCriterioDetalleAgrupacion2.DetalleAgrupacion.IdOperador))
                {
                    return item;
                }
            }
            return null;
        }


        /// <summary>
        /// <METHOD>newMethod</METHOD>
        /// Edita un constructor un constructor por 
        /// frecuencia y desglose, recibe un objeto
        /// Constructor.
        /// </summary>
        /// <param name="poConstructor"></param>
        /// <returns></returns>
        public Respuesta<Constructor> gEditarConstructorFrecuenciayDesglose(Constructor poConstructor)
        {
            Constructor constructor = new Constructor();// declaro el objeto  para actualizar
         
            try
            {
                constructor = context.Constructor.Include("ConstructorCriterio").Where(x => x.IdConstructor.Equals(poConstructor.IdConstructor)).FirstOrDefault();
                if (constructor != null)
                {
                    constructor.Frecuencia = null;//se igualan a Null los valores
                    constructor.Frecuencia1 = null;//

                    var iddesglose = poConstructor.IdDesglose;//se asigna una variable
                    var Idfrecuencia = poConstructor.IdFrecuencia;//se asigna a una variable

                    constructor.IdDesglose = iddesglose;// las variables se asignan al objeto nuevo
                    constructor.IdFrecuencia = Idfrecuencia;//las variables se asignan al objeto nuevo

                    context.Constructor.Attach(constructor);// se actualiza el constructor
                    context.Entry(constructor).State = EntityState.Modified;//estado modificado
                    context.SaveChanges();// se guardan los cambios 
                  
                }

                objRespuesta.objObjeto = poConstructor;// respuesta
            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(ex.Message, poConstructor);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }


        /// <summary>
        /// Edita un constructor
        /// Metodo original 
        /// </summary>
        /// <param name="poConstructor"></param>
        /// <returns></returns>
        public Respuesta<Constructor> gEditarConstructor(Constructor poConstructor)
        {
            Constructor constructor = new Constructor();
            List<ConstructorCriterio> constructorCriterios = new List<ConstructorCriterio>();

            try
            {

                //foreach (var child in constructor.ChildItems)
                //{
                //    context.Childs.Attach(child);
                //    context.Entry(child).State = EntityState.Modified;
                //}
                //context.SaveChanges();
                constructor = context.Constructor.Include("ConstructorCriterio").Where(x => x.IdConstructor.Equals(poConstructor.IdConstructor)).FirstOrDefault();
                if (constructor != null)
                {
                    constructor.Frecuencia = null;
                    constructor.Frecuencia1 = null;
                    var iddesglose= poConstructor.IdDesglose;
                    var Idfrecuencia=poConstructor.IdFrecuencia;

                    constructor.IdDesglose = iddesglose;
                    constructor.IdFrecuencia =Idfrecuencia;
                  
                    context.Constructor.Attach(constructor);
                    context.Entry(constructor).State = EntityState.Modified;
                    context.SaveChanges();
                    constructorCriterios = constructor.ConstructorCriterio.ToList();

                    if (constructorCriterios != null)
                    {
                        lEliminarConstructorCriterios(constructorCriterios);//esto ya no va 
                    }

                    lAgregarConstructorCriterio(poConstructor.ConstructorCriterio.ToList(), constructor);//ya no va

                }

                objRespuesta.objObjeto = poConstructor;
            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(ex.Message, poConstructor);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }

            return objRespuesta;
        }

        /// <summary>
        /// Elimina  el constructor
        /// </summary>
        /// <param name="poAgrupacion"></param>
        /// <returns></returns>
        public Respuesta<Constructor> gEliminarConstructor(Constructor poConstructor)
        {

            Constructor constructor = new Constructor();
            List<ConstructorCriterio> constructorCriterios = new List<ConstructorCriterio>();
            List<ConstructorCriterioDetalleAgrupacion> detallesAgrupacion = new List<ConstructorCriterioDetalleAgrupacion>();
            List<ConstructorCriterioDetalleAgrupacion> detallesAgrupacionPadre = new List<ConstructorCriterioDetalleAgrupacion>();
            try
            {

                constructor = context.Constructor.Include("ConstructorCriterio").Where(x => x.IdConstructor.Equals(poConstructor.IdConstructor)).FirstOrDefault();
                constructorCriterios = constructor.ConstructorCriterio.ToList();
                if (constructorCriterios != null)
                {
                    lEliminarConstructorCriterios(constructorCriterios);
                }


                if (constructor != null)
                {
                    constructor.Borrado = 1;
                    context.Constructor.Attach(constructor);
                    context.Constructor.Remove(constructor);
                    context.SaveChanges();
                    objRespuesta.objObjeto = constructor;
                }

            }
            catch (CustomException ex1)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(ex.Message, poConstructor);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }


            return objRespuesta;
        }
        /// <summary>
        /// <METHOD>newMethod</METHOD>
        /// </summary>
        /// <param name="poIdConstructor"></param>
        /// <param name="poIdCriterio"></param>
        /// <param name="idOperador"></param>
        /// <returns></returns>
        public Respuesta<List<ConstructorCriterioDetalleAgrupacion>> ObjObtenerDetalleDetalleDeAgrupacionporOperador(String poIdConstructor, String poIdCriterio, String idOperador)
        {
            Respuesta<List<ConstructorCriterioDetalleAgrupacion>> respuesta = new Respuesta<List<ConstructorCriterioDetalleAgrupacion>>();
            Guid idConstructor = new Guid(poIdConstructor);
            try
            {
                respuesta.objObjeto = context.ConstructorCriterioDetalleAgrupacion.Include("DetalleAgrupacion").Where(y => y.IdConstructor.Equals(idConstructor) && y.IdCriterio.Equals(poIdCriterio) && y.IdOperador.Equals(idOperador) && y.Borrado == 0).ToList();
                respuesta.objObjeto = respuesta.objObjeto.OrderBy(x => x.IdNivel).ToList();
            }
            catch (Exception ex)
            {
                respuesta.strMensaje = "Ha ocurrido un error, al intentar obtener el Detalle de Agrupacion del Operador";
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
            return respuesta;
         
        }
        /// <summary>
        ///  <METHOD>newMethod</METHOD>
        /// Agrega un ConstructorCriterio 
        /// </summary>
        /// <param name="Criterio"></param>
        /// <returns></returns>
        public Respuesta<bool> ObjAgregarConstructorCriterio(ConstructorCriterio Criterio)
        {
            Respuesta<bool> respuesta = new Respuesta<bool>();
            try
            {
                context.ConstructorCriterio.Add(Criterio);
                context.SaveChanges();//Criterio
                respuesta.objObjeto = true;
            }
            catch (Exception ex)
            {
                respuesta.strMensaje = "Hubo un error al intentar crear el Criterio.";
                respuesta.objObjeto = false;
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }
        /// <summary>
        ///  <METHOD>newMethod</METHOD>
        /// Editar un ConstructorCriterio 
        /// se llama un sp no mapeado 
        /// </summary>
        /// <param name="Criterio"></param>
        /// <returns></returns>
        public Respuesta<bool> ObjEditarConstructorCriterio(ConstructorCriterio Criterio)
        {
            Respuesta<bool> respuesta = new Respuesta<bool>();
            try
            {
                var consulta = string.Format("UPDATE [dbo].[ConstructorCriterio] set Ayuda = {0} WHERE IdConstructor={1} AND IdCriterio={2}", "'" + Criterio.Ayuda + "'", "'" + Criterio.IdConstructor + "'", "'" + Criterio.IdCriterio + "'");
                context.Database.CommandTimeout = 0;
                var values = context.Database.SqlQuery<object>(consulta).FirstOrDefault();
                context.SaveChanges();
                respuesta.objObjeto = true;  
            }
            catch (Exception ex)
            {
                respuesta.strMensaje = "Hubo un error al intentar crear el Criterio.";
                respuesta.objObjeto = false;
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        /// <summary>
        ///  <METHOD>newMethod</METHOD>
        /// </summary>
        /// <param name="Criterio"></param>
        /// <returns></returns>
        public bool gVerificarQueExisteCriterio(ConstructorCriterio Criterio)
        {
            ConstructorCriterio resultado = new ConstructorCriterio();
            Guid idConstructor = Criterio.IdConstructor;
            bool respuesta = false;
            try
            {
                resultado = context.ConstructorCriterio.Where(x => x.IdConstructor == idConstructor && x.IdCriterio == Criterio.IdCriterio).FirstOrDefault();
                if (resultado != null)
                {
                    respuesta = true;
                }
                else
                {
                    respuesta = false;
                }            
            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;             
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);            
            }

            return respuesta;
        }


        /// <summary>
        ///  <METHOD>newMethod</METHOD>
        /// Eliminar el constructor Criterio 
        /// <Method>newM</Method>
        /// </summary>
        /// <param name="Criterio"></param>
        /// <returns></returns>
        public Respuesta<bool> ObjEliminarConstructorCritero(ConstructorCriterio Criterio)
        {

            List<ConstructorCriterioDetalleAgrupacion> listadeCriterioD = new List<ConstructorCriterioDetalleAgrupacion>();
            Respuesta<bool> respuesta = new Respuesta<bool>();
            try
            {
                //Guid ID = new Guid("0fd2d63e-c0f0-447d-9829-47cfe23bfd24");
                listadeCriterioD = gObtenerDetalleAgrupacionConstructor(Criterio.IdConstructor, Criterio.IdCriterio);
                Criterio.ConstructorCriterioDetalleAgrupacion = listadeCriterioD;
                if (listadeCriterioD != null)
                {
                    lEliminarConstructorCriterio(Criterio);//Elimina el criterio
                    respuesta.strMensaje = "Criterio Eliminado";
                    respuesta.objObjeto = true;
                }
            }
            catch (Exception ex)
            {
                respuesta.strMensaje = "Error al intentar Eliminar el Criterio";
                respuesta.objObjeto = false;
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;         
           
        }

        /// <summary>
        /// <METHOD>newMethod</METHOD>
        /// este metodo 
        /// elimina primero la depencdecia circular 
        /// en la tabla constructordetalleAgrupacion
        /// para luego eliminar las reglas y proceder a 
        /// eliminar todos los registros asociados a este operador
        /// </summary>
        /// <param name="IdConstructor"></param>
        /// <param name="idcriterio"></param>
        /// <param name="idOperador"></param>
        /// <returns></returns>
        public Respuesta<bool> ObjEliminarArbolporOperador(Guid IdConstructor, string idcriterio, string idOperador)
        {

            bool eliminaDependencias = false;
            bool eliminaArbol = false;
            Respuesta<bool> respuesta = new Respuesta<bool>();
            try
            {

                eliminaDependencias = EditarIdConstructorDetallePadreporOperador(IdConstructor,idcriterio,idOperador);
                if (eliminaDependencias) { 
                  eliminaArbol=EliminarDetallesAgrupacionporOperador(IdConstructor,idcriterio,idOperador);
                  if (eliminaArbol)
                  {
                      respuesta.strMensaje = "El detalle de Agrupación, fue eliminado con exito";
                      respuesta.objObjeto = true;
                  }
                  else
                  {
                      respuesta.strMensaje = "El detalle de Agrupación,no fue eliminado con exito";
                      respuesta.objObjeto = false;
                  }
                }
                else
                {
                    respuesta.strMensaje = "ha ocurrido un error al intentar eliminar las dependecias";
                    respuesta.objObjeto = false;
                }
               
            }
            catch (Exception ex)
            {
                respuesta.strMensaje = "Error al intentar Eliminar el Criterio";
                respuesta.objObjeto = false;
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;

        }
        /// <summary>
        ///  <METHOD>newMethod</METHOD>
        /// pone en NULL a los id Detalles
        /// agrupacion que correspondan con el id 
        /// del operador
        /// </summary>
        /// <param name="IdConstructor"></param>
        /// <param name="idcriterio"></param>
        /// <returns></returns>
        public bool EditarIdConstructorDetallePadreporOperador(Guid IdConstructor, string idcriterio, string idOperador)
        {
            bool respuesta = false;
            try
            {
                var consulta = string.Format("EXEC dbo.pa_EditarIdConstructorDetallePadreporOperador @IdConstructor = {0}, @IdCriterio={1}, @IdOperador={2} ", "'" + IdConstructor + "'", "'" + idcriterio + "'", "'" + idOperador + "'");
                context.Database.CommandTimeout = 0;
                respuesta = bool.Parse(context.Database.SqlQuery<string>(consulta).FirstOrDefault());
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                respuesta = false;
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }


        /// <summary>
        /// <METHOD>newMethod</METHOD>
        /// pone en NULL a los id Detalles
        /// de agrupacion Padre 
        /// </summary>
        /// <param name="IdConstructor"></param>
        /// <param name="idcriterio"></param>
        /// <returns></returns>
        public bool EditarIdConstructorDetallePadre(Guid IdConstructor, string idcriterio)
        {
            bool respuesta =false;
            try
            {
                var consulta = string.Format("EXEC dbo.pa_EditarIdConstructorDetallePadre @IdConstructor = {0}, @IdCriterio={1}", "'" + IdConstructor + "'", "'" + idcriterio+ "'" );
                context.Database.CommandTimeout = 0;
                respuesta = bool.Parse(context.Database.SqlQuery<string>(consulta).FirstOrDefault());
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                respuesta = false;
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }

        /// <summary>
        ///  <METHOD>newMethod</METHOD>
        /// Elimina los  Detalles
        /// de agrupacion asociados a ese Criterio y al Operador
        /// </summary>
        /// <param name="IdConstructor"></param>
        /// <param name="idcriterio"></param>
        /// <returns></returns>
        public bool EliminarDetallesAgrupacionporOperador(Guid IdConstructor, string idcriterio, string idOperador)
        {
            bool respuesta = false;
            try
            {
                var consulta = string.Format("EXEC dbo.pa_EliminarDetallesdeAgrupacionporOperador @IdConstructor = {0}, @IdCriterio={1}, @IdOperador={2}", "'" + IdConstructor + "'", "'" + idcriterio + "'", "'" + idOperador + "'");// llamado al sp que elimina
                context.Database.CommandTimeout = 0;
                respuesta = bool.Parse(context.Database.SqlQuery<string>(consulta).FirstOrDefault());
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                respuesta = false;
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }
        /// <summary>
        /// <METHOD>newMethod</METHOD>
        /// Elimina los  Detalles
        /// de agrupacion asociados a ese Criterio.
        /// </summary>
        /// <param name="IdConstructor"></param>
        /// <param name="idcriterio"></param>
        /// <returns></returns>
        public bool EliminarDetallesAgrupacion(Guid IdConstructor, string idcriterio)
        {
            bool respuesta = false;
            try
            {
                var consulta = string.Format("EXEC dbo.pa_EliminarDetallesdeAgrupacion @IdConstructor = {0}, @IdCriterio={1}", "'" + IdConstructor + "'", "'" + idcriterio + "'");// llamado al sp que elimina
                context.Database.CommandTimeout = 0;
                respuesta = bool.Parse(context.Database.SqlQuery<string>(consulta).FirstOrDefault());
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                respuesta = false;
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }

        /// <summary>
        /// Agrega los criterios al constructor
        /// </summary>
        /// <param name="poConstructorCriterios"></param>
        /// <param name="poConstructor"></param>
        private void lAgregarConstructorCriterio(List<ConstructorCriterio> poConstructorCriterios,Constructor poConstructor)
        {
            List<ConstructorCriterioDetalleAgrupacion> constructorDetalle = new List<ConstructorCriterioDetalleAgrupacion>();
            List<ConstructorCriterioDetalleAgrupacion> constructorDetalleAgregados = new List<ConstructorCriterioDetalleAgrupacion>();//ocupo este
            ConstructorCriterioDetalleAgrupacion detallePadre = new ConstructorCriterioDetalleAgrupacion();//ocupo este 
            ConstructorCriterioDetalleAgrupacion itemInsertar = new ConstructorCriterioDetalleAgrupacion();//ocupo este
            ConstructorCriterio nuevoCriterio = new ConstructorCriterio();  // no lo ocupo          
            bool usaReglaEstadisticaConNivelDetalle;//ocupo este

          
            Regla nuevaRegla = new Regla();
            try
            {
                foreach (ConstructorCriterio nCriterio in poConstructorCriterios)//ocupo quitar este  for 
                {
                    constructorDetalle = new List<ConstructorCriterioDetalleAgrupacion>(nCriterio.ConstructorCriterioDetalleAgrupacion);//obtiene los arboles 
                    nuevoCriterio = new ConstructorCriterio();//no ocupo este
                    nuevoCriterio.IdConstructor = poConstructor.IdConstructor;//no ocupo este
                    nuevoCriterio.IdCriterio = nCriterio.IdCriterio;// no ocupo este 
                    nuevoCriterio.Ayuda = nCriterio.Ayuda.Trim();// no ocupo este
                    context.ConstructorCriterio.Add(nuevoCriterio);// no ocupo este
                    context.SaveChanges();//Criterio 
                    constructorDetalleAgregados = new List<ConstructorCriterioDetalleAgrupacion>();//ocupo esta lista
                    foreach (ConstructorCriterioDetalleAgrupacion itemDetalle in constructorDetalle)//ocupo este for// obtiene los arboles // yo solo recibo un arbol 
                    {
                        foreach (ConstructorCriterioDetalleAgrupacion itemDetalleAgrupacion in itemDetalle.ConstructorCriterioDetalleAgrupacion1)// ocupo este for 
                        {
                            itemInsertar = new ConstructorCriterioDetalleAgrupacion();
                            detallePadre = lObtenerPadreDetalle(constructorDetalleAgregados, itemDetalleAgrupacion);
                            usaReglaEstadisticaConNivelDetalle = false;


                            if (itemDetalleAgrupacion.NivelDetalleReglaEstadistica != null && itemDetalleAgrupacion.NivelDetalleReglaEstadistica.Count > 0)
                            {
                                usaReglaEstadisticaConNivelDetalle = true;
                                itemInsertar.UsaReglaEstadisticaConNivelDetalle = 1;
                                itemInsertar.UsaReglaEstadistica = 1;
                            }


                            if (detallePadre != null)
                            {
                                itemInsertar.IdConstructorDetallePadre = detallePadre.IdConstructorCriterio;
                            }

                            itemInsertar.IdConstructorCriterio = Guid.NewGuid();
                            itemInsertar.IdConstructor = poConstructor.IdConstructor;
                            itemInsertar.IdCriterio = nCriterio.IdCriterio;
                            itemInsertar.IdOperador = itemDetalleAgrupacion.IdOperador;
                            itemInsertar.IdAgrupacion = itemDetalleAgrupacion.IdAgrupacion;
                            itemInsertar.IdDetalleAgrupacion = itemDetalleAgrupacion.IdDetalleAgrupacion;
                            itemInsertar.Borrado = 0;
                            itemInsertar.UltimoNivel = itemDetalleAgrupacion.UltimoNivel;
                            itemInsertar.Orden = new int();
                            itemInsertar.Orden = itemDetalleAgrupacion.Orden;
                            itemInsertar.UsaReglaEstadistica = itemDetalleAgrupacion.UsaReglaEstadistica;
                            if (itemDetalleAgrupacion.IdNivelDetalle != null && itemDetalleAgrupacion.IdNivelDetalle != 0)
                            {
                                itemInsertar.IdNivelDetalle = itemDetalleAgrupacion.IdNivelDetalle;
                            }
                            if (itemDetalleAgrupacion.IdTipoValor != null && itemDetalleAgrupacion.IdTipoValor != 0)
                            {
                                itemInsertar.IdTipoValor = itemDetalleAgrupacion.IdTipoValor;
                            }
                         
                                string[] ids = itemDetalleAgrupacion.DetalleAgrupacion.IdOperador.Split('|'); ;
                                if (ids.Length > 0)
                                {
                                    itemInsertar.IdNivel = (ids.Length / 3) - 1;
                                }
                              
                                context.ConstructorCriterioDetalleAgrupacion.Add(itemInsertar);
                                context.SaveChanges();//Detalle Agrupacion

                                itemDetalleAgrupacion.IdConstructorCriterio = itemInsertar.IdConstructorCriterio;

                                constructorDetalleAgregados.Add(itemDetalleAgrupacion);


                                if (itemDetalleAgrupacion.UltimoNivel.Equals((byte)1))
                                {
                                    //Agregado para el requerimiento reglaEstadística
                                    if (usaReglaEstadisticaConNivelDetalle)
                                    {
                                        List<NivelDetalleReglaEstadistica> ListaLimpia = new List<NivelDetalleReglaEstadistica>();
                                        NivelDetalleReglaEstadistica ObjeNiveldetalle = new NivelDetalleReglaEstadistica();

                                        foreach (var item in itemDetalleAgrupacion.NivelDetalleReglaEstadistica)
                                        {                                            
                                           
                                            ObjeNiveldetalle.ConstructorCriterioDetalleAgrupacion = itemInsertar;
                                            ObjeNiveldetalle.TipoNivelDetalle = null;
                                            ObjeNiveldetalle.IdNivelDetalleReglaEstadistica = 0;
                                            ObjeNiveldetalle.Borrado = 0;
                                            ObjeNiveldetalle.IdConstructorCriterioDetalleAgrupacion=  itemInsertar.IdConstructorCriterio;
                                            ObjeNiveldetalle.IdNivelDetalle = item.IdNivelDetalle;
                                            ObjeNiveldetalle.IdGenerico = item.IdGenerico;
                                            ObjeNiveldetalle.ValorLimiteInferior = item.ValorLimiteInferior;
                                            ObjeNiveldetalle.ValorLimiteSuperior = item.ValorLimiteSuperior;

                                            ListaLimpia.Add(ObjeNiveldetalle);

                                            ObjeNiveldetalle = new NivelDetalleReglaEstadistica();
                                        }


                                        foreach (var item in ListaLimpia)
                                        {                                           
                                                context.NivelDetalleReglaEstadistica.Add(item);

                                                context.SaveChanges();                                           
                                            
                                        }

                                        ListaLimpia = new List<NivelDetalleReglaEstadistica>();
                                   

                                    }
                                    else
                                    {



                                        nuevaRegla = new Regla();
                                        nuevaRegla.FechaCreacionRegla = DateTime.Now;
                                        nuevaRegla.ValorLimiteInferior = (itemDetalleAgrupacion.Regla.ValorLimiteInferior == null ? "" : itemDetalleAgrupacion.Regla.ValorLimiteInferior);
                                        nuevaRegla.ValorLimiteSuperior = (itemDetalleAgrupacion.Regla.ValorLimiteSuperior == null ? "" : itemDetalleAgrupacion.Regla.ValorLimiteSuperior);
                                        nuevaRegla.IdConstructorCriterio = itemInsertar.IdConstructorCriterio;
                                        context.Regla.Add(nuevaRegla);
                                        context.SaveChanges();

                                    }




                                }

                            }// ocupo este for nada mas 

                        }



                    }
                
            }
            catch (CustomException)
            {
                throw;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(ex.Message, poConstructor);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
        }


        /// <summary>
        /// <METHOD>newMethod</METHOD>
        /// Agrega los criterios al constructor
        /// </summary>
        /// <param name="poConstructorCriterios"></param>
        /// <param name="poConstructor"></param>
        public Respuesta<bool> lAgregarDetalledeAgrupacionConstructor(ConstructorCriterioDetalleAgrupacion poConstructor, Guid IDconstructor, String IDcriterio)
        {
            List<ConstructorCriterioDetalleAgrupacion> constructorDetalle = new List<ConstructorCriterioDetalleAgrupacion>();
            List<ConstructorCriterioDetalleAgrupacion> constructorDetalleAgregados = new List<ConstructorCriterioDetalleAgrupacion>();//ocupo este
            ConstructorCriterioDetalleAgrupacion detallePadre = new ConstructorCriterioDetalleAgrupacion();//ocupo este 
            ConstructorCriterioDetalleAgrupacion itemInsertar = new ConstructorCriterioDetalleAgrupacion();//ocupo este
            ConstructorCriterio nuevoCriterio = new ConstructorCriterio();  // no lo ocupo          
            bool usaReglaEstadisticaConNivelDetalle;//ocupo este
            Respuesta<bool> respuesta = new Respuesta<bool>();

            Regla nuevaRegla = new Regla();
            try
            {           
                    //constructorDetalle = new List<ConstructorCriterioDetalleAgrupacion>(nCriterio.ConstructorCriterioDetalleAgrupacion);//obtiene los arboles      
                    constructorDetalleAgregados = new List<ConstructorCriterioDetalleAgrupacion>();//ocupo esta lista

                        foreach (ConstructorCriterioDetalleAgrupacion itemDetalleAgrupacion in poConstructor.ConstructorCriterioDetalleAgrupacion1)// ocupo este for 
                        {
                            itemInsertar = new ConstructorCriterioDetalleAgrupacion();
                            detallePadre = lObtenerPadreDetalle(constructorDetalleAgregados, itemDetalleAgrupacion);
                            usaReglaEstadisticaConNivelDetalle = false;


                            if (itemDetalleAgrupacion.NivelDetalleReglaEstadistica != null && itemDetalleAgrupacion.NivelDetalleReglaEstadistica.Count > 0)
                            {
                                usaReglaEstadisticaConNivelDetalle = true;
                                itemInsertar.UsaReglaEstadisticaConNivelDetalle = 1;
                                itemInsertar.UsaReglaEstadistica = 1;
                            }


                            if (detallePadre != null)
                            {
                                itemInsertar.IdConstructorDetallePadre = detallePadre.IdConstructorCriterio;
                            }

                            itemInsertar.IdConstructorCriterio = Guid.NewGuid();
                            itemInsertar.IdConstructor = IDconstructor;
                            itemInsertar.IdCriterio = IDcriterio;
                            itemInsertar.IdOperador = itemDetalleAgrupacion.IdOperador;
                            itemInsertar.IdAgrupacion = itemDetalleAgrupacion.IdAgrupacion;
                            itemInsertar.IdDetalleAgrupacion = itemDetalleAgrupacion.IdDetalleAgrupacion;
                            itemInsertar.Borrado = 0;
                            itemInsertar.UltimoNivel = itemDetalleAgrupacion.UltimoNivel;
                            itemInsertar.Orden = new int();
                            itemInsertar.Orden = itemDetalleAgrupacion.Orden;
                            itemInsertar.UsaReglaEstadistica = itemDetalleAgrupacion.UsaReglaEstadistica;
                            if (itemDetalleAgrupacion.IdNivelDetalle != null && itemDetalleAgrupacion.IdNivelDetalle != 0)
                            {
                                itemInsertar.IdNivelDetalle = itemDetalleAgrupacion.IdNivelDetalle;
                               // itemInsertar.IdNivelDetalleGenero = itemDetalleAgrupacion.IdNivelDetalleGenero;
                            }
                            itemInsertar.IdNivelDetalleGenero = itemDetalleAgrupacion.IdNivelDetalleGenero;
                            if (itemDetalleAgrupacion.IdTipoValor != null && itemDetalleAgrupacion.IdTipoValor != 0)
                            {
                                itemInsertar.IdTipoValor = itemDetalleAgrupacion.IdTipoValor;
                            }

                            string[] ids = itemDetalleAgrupacion.DetalleAgrupacion.IdOperador.Split('|'); ;
                            if (ids.Length > 0)
                            {
                                itemInsertar.IdNivel = (ids.Length / 3) - 1;
                            }

                            context.ConstructorCriterioDetalleAgrupacion.Add(itemInsertar);
                            context.SaveChanges();//Detalle Agrupacion

                            itemDetalleAgrupacion.IdConstructorCriterio = itemInsertar.IdConstructorCriterio;

                            constructorDetalleAgregados.Add(itemDetalleAgrupacion);


                            if (itemDetalleAgrupacion.UltimoNivel.Equals((byte)1))
                            {
                                //Agregado para el requerimiento reglaEstadística
                                if (usaReglaEstadisticaConNivelDetalle)
                                {
                                    List<NivelDetalleReglaEstadistica> ListaLimpia = new List<NivelDetalleReglaEstadistica>();
                                    NivelDetalleReglaEstadistica ObjeNiveldetalle = new NivelDetalleReglaEstadistica();

                                    foreach (var item in itemDetalleAgrupacion.NivelDetalleReglaEstadistica)
                                    {

                                        ObjeNiveldetalle.ConstructorCriterioDetalleAgrupacion = itemInsertar;
                                        ObjeNiveldetalle.TipoNivelDetalle = null;
                                        ObjeNiveldetalle.IdNivelDetalleReglaEstadistica = 0;
                                        ObjeNiveldetalle.Borrado = 0;
                                        ObjeNiveldetalle.IdConstructorCriterioDetalleAgrupacion = itemInsertar.IdConstructorCriterio;
                                        ObjeNiveldetalle.IdNivelDetalle = item.IdNivelDetalle;
                                        ObjeNiveldetalle.IdGenerico = item.IdGenerico;
                                        ObjeNiveldetalle.ValorLimiteInferior = item.ValorLimiteInferior;
                                        ObjeNiveldetalle.ValorLimiteSuperior = item.ValorLimiteSuperior;

                                        ListaLimpia.Add(ObjeNiveldetalle);

                                        ObjeNiveldetalle = new NivelDetalleReglaEstadistica();
                                    }


                                    foreach (var item in ListaLimpia)
                                    {
                                        context.NivelDetalleReglaEstadistica.Add(item);

                                        context.SaveChanges();

                                    }

                                    ListaLimpia = new List<NivelDetalleReglaEstadistica>();


                                }
                                else
                                {
                                    nuevaRegla = new Regla();
                                    nuevaRegla.FechaCreacionRegla = DateTime.Now;
                                    nuevaRegla.ValorLimiteInferior = (itemDetalleAgrupacion.Regla.ValorLimiteInferior == null ? "" : itemDetalleAgrupacion.Regla.ValorLimiteInferior);
                                    nuevaRegla.ValorLimiteSuperior = (itemDetalleAgrupacion.Regla.ValorLimiteSuperior == null ? "" : itemDetalleAgrupacion.Regla.ValorLimiteSuperior);
                                    nuevaRegla.IdConstructorCriterio = itemInsertar.IdConstructorCriterio;
                                    context.Regla.Add(nuevaRegla);
                                    context.SaveChanges();

                                }

                            }

                        }// ocupo este for nada mas 
                        respuesta.objObjeto = true;
                        respuesta.strMensaje = "El Detalle agrupacion se ha modificado exitosamente";
            }
            catch (Exception ex)
            {
                respuesta.objObjeto = false;
                respuesta.strMensaje = "El Detalle agrupacion se ha sido modificado y ha sido Eliminado";
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }


        /// <summary>
        /// Elimina los criterios del constructor
        /// </summary>
        /// <param name="constructorCriterios"></param>
        private void lEliminarConstructorCriterios(List<ConstructorCriterio> constructorCriterios)
        {
            List<ConstructorCriterioDetalleAgrupacion> detallesAgrupacion = new List<ConstructorCriterioDetalleAgrupacion>();
            List<ConstructorCriterioDetalleAgrupacion> detallesAgrupacionPadre = new List<ConstructorCriterioDetalleAgrupacion>();
            try
            {


                foreach (ConstructorCriterio constructorCriterio in constructorCriterios)
                {
                    detallesAgrupacion = constructorCriterio.ConstructorCriterioDetalleAgrupacion.ToList();
                    if (detallesAgrupacion != null && detallesAgrupacion.Count > 0)
                    {
                        if (detallesAgrupacion.Count == 1)
                        {
                            lEliminarConstructorDetalleAgrupacion(detallesAgrupacion);
                        }
                        else
                        {
                            detallesAgrupacionPadre = detallesAgrupacion.Where(x => x.IdConstructorDetallePadre != null && x.Borrado == 0).ToList();
                            lEliminarDependenciaPadreDetalleAgrupacion(detallesAgrupacionPadre);
                            lEliminarConstructorDetalleAgrupacion(detallesAgrupacion);
                        }
                    }
                    context.ConstructorCriterio.Remove(constructorCriterio);
                    context.SaveChanges();

                }
            }
            catch (CustomException ex1)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
        }
        /// <summary>
        /// <METHOD>newMethod</METHOD>
        /// Elimino de la tabla ConstructorCriterio
        /// </summary>
        /// <param name="constructorCriterio"></param>
        private void lEliminarConstructorCriterio(ConstructorCriterio constructorCriterio)
        {
            List<ConstructorCriterioDetalleAgrupacion> detallesAgrupacion = new List<ConstructorCriterioDetalleAgrupacion>();
            List<ConstructorCriterioDetalleAgrupacion> detallesAgrupacionPadre = new List<ConstructorCriterioDetalleAgrupacion>();
            ConstructorCriterio cc = new ConstructorCriterio();
            Guid idConstructor=constructorCriterio.IdConstructor;
            string idCriterio= constructorCriterio.IdCriterio;
             bool eliminodependecias= new bool();
            try
            {

                    detallesAgrupacion = constructorCriterio.ConstructorCriterioDetalleAgrupacion.ToList();
                    if (detallesAgrupacion != null && detallesAgrupacion.Count > 0)
                    {
                        if (detallesAgrupacion.Count == 1)
                        {
                            lEliminarConstructorDetalleAgrupacion(detallesAgrupacion);
                        }
                        else
                        {
                            detallesAgrupacionPadre = detallesAgrupacion.Where(x => x.IdConstructorDetallePadre != null && x.Borrado == 0).ToList();
                            eliminodependecias = EditarIdConstructorDetallePadre(idConstructor, idCriterio);//Elimino Dependecias 
                            if (eliminodependecias)
                               EliminarDetallesAgrupacion(idConstructor, idCriterio);//Elimina detalles de agrupacion 
                            // lEliminarConstructorDetalleAgrupacion(detallesAgrupacion);
                            context.SaveChanges();
                        }
                    }
                    var consulta = string.Format("EXEC pa_EliminarConstructorCriterio @IdConstructor = {0}, @IdCriterio={1}", "'" + constructorCriterio.IdConstructor + "'", "'" + constructorCriterio.IdCriterio + "'");//elimino los constructoresCriterios
                    context.Database.CommandTimeout = 0;
                   bool values = bool.Parse(context.Database.SqlQuery<string>(consulta).FirstOrDefault());
                    context.SaveChanges();
                    //cc = context.ConstructorCriterio.Where(x => x.IdConstructor == constructorCriterio.IdConstructor && x.IdCriterio == //constructorCriterio.IdCriterio).FirstOrDefault();
                    //context.ConstructorCriterio.Remove(cc);
                    //context.SaveChanges();

                
            }
            catch (CustomException ex1)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
        }

      


        /// <summary>
        /// Elimina las referencias entre padre e hijo de detalles agrupación
        /// </summary>
        /// <param name="detallesAgrupacionPadre"></param>
        private void lEliminarDependenciaPadreDetalleAgrupacion(List<ConstructorCriterioDetalleAgrupacion> detallesAgrupacionPadre)
        {
            foreach (ConstructorCriterioDetalleAgrupacion item in detallesAgrupacionPadre)
            {
                item.IdConstructorDetallePadre = null;
                item.ConstructorCriterioDetalleAgrupacion2 = null;                
                context.ConstructorCriterioDetalleAgrupacion.Attach(item);
                context.Entry(item).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
        /// <summary>
        /// Elimina los detalles agrupación
        /// </summary>
        /// <param name="detalles"></param>
        private void lEliminarConstructorDetalleAgrupacion(List<ConstructorCriterioDetalleAgrupacion> detalles)
        {
            Regla nRegla = new Regla();
            ConstructorCriterioDetalleAgrupacion nDetalle = new ConstructorCriterioDetalleAgrupacion();
            List<ConstructorCriterioDetalleAgrupacion> listaDetallesBorrar = new List<ConstructorCriterioDetalleAgrupacion>();
            try
            {
                listaDetallesBorrar = new List<ConstructorCriterioDetalleAgrupacion>(detalles);
                foreach (ConstructorCriterioDetalleAgrupacion detalle in listaDetallesBorrar)
                {
                    nRegla = context.Regla.Where(x => x.IdConstructorCriterio.Equals(detalle.IdConstructorCriterio)).FirstOrDefault();
                    if (nRegla != null)
                    {
                        context.Regla.Remove(nRegla);
                        context.SaveChanges();
                    }
                    nDetalle = context.ConstructorCriterioDetalleAgrupacion.Where(x => x.IdConstructorCriterio == detalle.IdConstructorCriterio).FirstOrDefault();
                    nDetalle.ConstructorCriterioDetalleAgrupacion1.Clear();
                    if (nDetalle != null)
                    {
                        context.ConstructorCriterioDetalleAgrupacion.Remove(nDetalle);
                        context.SaveChanges();
                    }
                }

            }
            catch (CustomException ex1)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
        }


        /// <summary>
        /// Obtiene los constructores
        /// </summary>
        /// <returns></returns>
        public Respuesta<List<Constructor>> gObtenerConstructores()
        {
            List<Constructor> resultado = new List<Constructor>();
            Respuesta<List<Constructor>> objRespuestas = new Respuesta<List<Constructor>>();
            try
            {

                resultado = context.Constructor.Include("Frecuencia").Include("Frecuencia1").Include("Indicador").Where(x => x.Borrado.Equals(0)).Take(50).ToList();

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
        /// Obtiene los constructores por filtros
        /// </summary>
        /// <returns></returns>
        public Respuesta<List<Constructor>> gObtenerConstructoresPorFiltros(String psIdIndicador, String psIndicador, String psDesglose, String psFrecuencia)
        {
            List<Constructor> resultado = new List<Constructor>();
            Respuesta<List<Constructor>> objRespuestas = new Respuesta<List<Constructor>>();
            try
            {

                resultado = context.Constructor.Include("Frecuencia").Include("Frecuencia1").Where(entidadConstructor
                            => entidadConstructor.Borrado == 0 &&
                            (psIdIndicador.Equals("") || entidadConstructor.Indicador.IdIndicador.ToUpper().Contains(psIdIndicador))
                             &&(psIndicador.Equals("") || entidadConstructor.Indicador.NombreIndicador.ToUpper().Contains(psIndicador.ToUpper()))
                             && (psDesglose.Equals("") || entidadConstructor.Frecuencia.NombreFrecuencia.ToUpper().Contains(psDesglose.ToUpper()))
                             && (psFrecuencia.Equals("") || entidadConstructor.Frecuencia1.NombreFrecuencia.ToUpper().Contains(psFrecuencia.ToUpper()))).Take(50).ToList();

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
        /// Obtiene aquel coonstructor que tiene el mismo indicadr
        /// </summary>
        /// <param name="psIndicador"></param>
        /// <param name="piDesglose"></param>
        /// <param name="piFrecuencia"></param>
        /// <returns></returns>
        public Respuesta<Constructor> gObtenerConstructorPorIndDesFrecuencia(String psIndicador, int piDesglose, int piFrecuencia)
        {
            Constructor resultado = new Constructor();
            Respuesta<Constructor> objRespuestas = new Respuesta<Constructor>();
            try
            {

                resultado = context.Constructor.Where(x => x.Borrado == 0
                            && x.IdIndicador.Equals(psIndicador)
                             && x.IdDesglose == piDesglose
                             && x.IdFrecuencia == piFrecuencia).FirstOrDefault();

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
        /// Obtiene el constructor
        /// </summary>
        /// <returns></returns>
        public Respuesta<Constructor> gObtenerConstructor(String piIdConstructor)
        {
            Constructor resultado = new Constructor();
            
            try
            {
                resultado = context.Constructor.Include("ConstructorCriterio").Include("SolicitudConstructor").Where(x => (x.Borrado == 0 || x.Borrado == 2)
                             && x.IdConstructor.ToString().Equals(piIdConstructor) ).FirstOrDefault();
                if (resultado.ConstructorCriterio != null && resultado.ConstructorCriterio.Count > 0)
                {
                    foreach (ConstructorCriterio item in resultado.ConstructorCriterio)
                    {
                        item.ConstructorCriterioDetalleAgrupacion = item.ConstructorCriterioDetalleAgrupacion.OrderBy(x => x.IdNivel).ToList();
                    }
                }
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
        /// Obtiene el constructor para verificar. Esto para el  proceso de borrado
        /// </summary>
        /// <param name="piIdConstructor"></param>
        /// <returns></returns>
        public Respuesta<Constructor> gObtenerConstructorVerificar(String piIdConstructor)
        {
            Constructor resultado = new Constructor();

            try
            {
                resultado = context.Constructor.Include("SolicitudConstructor").Where(x => (x.Borrado == 0 || x.Borrado == 2)
                             && x.IdConstructor.ToString().Equals(piIdConstructor)).FirstOrDefault();

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
        /// Obtiene los detalles agrupación especificando el constructor, el criterio y el operador
        /// </summary>
        /// <param name="poIdConstructor"></param>
        /// <param name="poIdCriterio"></param>
        /// <param name="poIdOperador"></param>
        public Respuesta<List<ConstructorCriterioDetalleAgrupacion>> gObtenerDetalleAgrupacionConstructor(Guid poIdConstructor, string poIdCriterio, string poIdOperador)
        {
            Respuesta<List<ConstructorCriterioDetalleAgrupacion>> respuesta = new Respuesta<List<ConstructorCriterioDetalleAgrupacion>>();
            try {
                respuesta.objObjeto = context.ConstructorCriterioDetalleAgrupacion.Where(y => y.IdConstructor.Equals(poIdConstructor) && y.IdCriterio.Equals(poIdCriterio) && y.IdOperador.Equals(poIdOperador) && y.Borrado == 0).ToList();
                respuesta.objObjeto = respuesta.objObjeto.OrderBy(x => x.IdNivel).ToList();
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(ex.Message, respuesta.objObjeto);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
         return respuesta;
        }

        /// <summary>
        /// Obtiene los detalles agrupación especificando el constructor, el criterio 
        /// </summary>
        /// <param name="poIdConstructor"></param>
        /// <param name="poIdCriterio"></param>
        /// <param name="poIdOperador"></param>
        public List<ConstructorCriterioDetalleAgrupacion> gObtenerDetalleAgrupacionConstructor(Guid poIdConstructor, string poIdCriterio )
        {
            List<ConstructorCriterioDetalleAgrupacion> respuesta = new List<ConstructorCriterioDetalleAgrupacion>();
            try
            {
                respuesta = context.ConstructorCriterioDetalleAgrupacion.Where(y => y.IdConstructor.Equals(poIdConstructor) && y.IdCriterio.Equals(poIdCriterio) && y.Borrado == 0).ToList();
                respuesta = respuesta.OrderBy(x => x.IdNivel).ToList();
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;             
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
            return respuesta;
        }

        /// <summary>
        /// <METHOD>newMethod</METHOD>
        /// Obtiene los detalles agrupación especificando el constructor, el criterio 
        /// </summary>
        /// <param name="poIdConstructor"></param>
        /// <param name="poIdCriterio"></param>
        /// <param name="poIdOperador"></param>
        public Respuesta<List<Operador>> gObtenerOperadoresDetalleAgrupacion(Guid poIdConstructor, string poIdCriterio)
        {
            Respuesta<List<Operador>> respuesta = new Respuesta<List<Operador>>();
     
            try
            {
                var consulta = string.Format("EXEC dbo.pa_getOperadoresDetalleAgrupacionPorIdConstructorAndIdCriterio @IdConstructor = {0}, @IdCriterio={1}", "'" + poIdConstructor + "'", "'" + poIdCriterio + "'");
                context.Database.CommandTimeout = 0;
                respuesta.objObjeto = context.Database.SqlQuery<Operador>(consulta).ToList();
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                respuesta.strMensaje = "Error al listar los detalles agrupacion por operador asociado";
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;         
        }

        /// <summary>
        /// Obtiene el detalle agrupacion especifico en el constructor
        /// </summary>
        /// <param name="poIdDetalleAgrupacion"></param>
        public Respuesta<ConstructorCriterioDetalleAgrupacion> gObtenerConstructorCriterioDetalleAgrupacion(Guid poIdDetalleAgrupacion)
        {
            Respuesta<ConstructorCriterioDetalleAgrupacion> detalleAgrupacion = new Respuesta<ConstructorCriterioDetalleAgrupacion>();
            try {
                detalleAgrupacion.objObjeto = context.ConstructorCriterioDetalleAgrupacion.Where(x => x.IdConstructorCriterio.Equals(poIdDetalleAgrupacion)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                //respuesta.toError(ex.Message, respuesta.objObjeto);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
            return detalleAgrupacion;
        }


        /// <summary>
        /// gValoresRegistradosPorOperador_Canton ejecuta el sp "pa_getValoresRegistradosPorOperador_Canton" y este 
        /// consulta a la base de datos una lista de valores registrados en las plantillas Excel
        /// por un operador específico a los 81 cantones.
        /// </summary>
        /// <param name="IdOperador"></param>
        /// <returns></returns>
        public Respuesta<List<pa_getValoresRegistradosPorOperador_Canton_Result>> gValoresRegistradosPorOperador_Canton(string IdOperador, int IdTipoValor, int idDescDetalleAgrupacion)
        {
            List<pa_getValoresRegistradosPorOperador_Canton_Result> spResult = new List<pa_getValoresRegistradosPorOperador_Canton_Result>();

            Respuesta<List<pa_getValoresRegistradosPorOperador_Canton_Result>> respuesta = new Respuesta<List<pa_getValoresRegistradosPorOperador_Canton_Result>>();
            try
            {

                spResult = context.pa_getValoresRegistradosPorOperador_Canton(IdOperador, IdTipoValor, idDescDetalleAgrupacion).ToList();

               // Para que se pueda aplicar la Regla se necesitan doce datos. Los últimos doce.
                // El sp trae top 972 datos (12 por cada cantón).

                if (spResult.Count == 972)
                {
                    respuesta.objObjeto = spResult;
                    respuesta.blnIndicadorTransaccion = true;
                    return respuesta;
                }

                else {
                    respuesta.blnIndicadorTransaccion = true;
                    respuesta.objObjeto = null;
                    respuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.DatosInsuficientes;                
                }
                    
             
            }
            catch (CustomException)
            {
                respuesta.blnIndicadorTransaccion = false;
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;              
                respuesta.blnIndicadorTransaccion = false;  
                respuesta.strMensaje = msj;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }



        /// <summary>
        /// gValoresRegistradosPorOperador_Genero ejecuta el sp "pa_getValoresRegistradosPorOperador_Genero" y este 
        /// consulta a la base de datos una lista de valores registrados en las plantillas Excel
        /// por un operador específico, para masculino y femenino.
        /// </summary>
        /// <param name="IdOperador"></param>
        /// <returns></returns>
        public Respuesta<List<pa_getValoresRegistradosPorOperador_Genero_Result>> gValoresRegistradosPorOperador_Genero(string IdOperador, int IdTipoValor, int idDescDetalleAgrupacion)
        {
            List<pa_getValoresRegistradosPorOperador_Genero_Result> spResult = new List<pa_getValoresRegistradosPorOperador_Genero_Result>();

            Respuesta<List<pa_getValoresRegistradosPorOperador_Genero_Result>> respuesta = new Respuesta<List<pa_getValoresRegistradosPorOperador_Genero_Result>>();
            try
            {

                spResult = context.pa_getValoresRegistradosPorOperador_Genero(IdOperador, IdTipoValor, idDescDetalleAgrupacion).ToList();

                // Para que se pueda aplicar la Regla se necesitan doce datos. Los últimos doce.
                // El sp trae 24 datos, doce por cada género.

                if (spResult.Count == 24)
                {
                    respuesta.objObjeto = spResult;
                    respuesta.blnIndicadorTransaccion = true;
                    return respuesta;
                }

                else
                {
                    respuesta.blnIndicadorTransaccion = true;
                    respuesta.objObjeto = null;
                    respuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.DatosInsuficientes;
                }


            }
            catch (CustomException)
            {
                respuesta.blnIndicadorTransaccion = false;
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;              
                respuesta.blnIndicadorTransaccion = false;
                respuesta.strMensaje = msj;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }


        /// <summary>
        /// gValoresRegistradosPorOperador_Provincia ejecuta el sp "pa_getValoresRegistradosPorOperador_Provincia" y este 
        /// consulta a la base de datos una lista de valores registrados en las plantillas Excel
        /// por un operador específico para cada provincia.
        /// </summary>
        /// <param name="IdOperador"> </param>
        /// <param name="IdTipoValor"></param>
        /// <param name="idDescDetalleAgrupacion"></param>
        ///       
        /// <returns></returns>
        public Respuesta<List<pa_getValoresRegistradosPorOperador_Provincia_Result>> gValoresRegistradosPorOperador_Provincia(string IdOperador, int IdTipoValor, int idDescDetalleAgrupacion)
        {
            List<pa_getValoresRegistradosPorOperador_Provincia_Result> spResult = new List<pa_getValoresRegistradosPorOperador_Provincia_Result>();

            Respuesta<List<pa_getValoresRegistradosPorOperador_Provincia_Result>> respuesta = new Respuesta<List<pa_getValoresRegistradosPorOperador_Provincia_Result>>();
            try
            {

                spResult = context.pa_getValoresRegistradosPorOperador_Provincia(IdOperador, IdTipoValor, idDescDetalleAgrupacion).ToList();

                // Para que se pueda aplicar la Regla se necesitan doce datos. Los últimos doce para cada provincia.
                // El sp trae 84 datos (12 por cada provincia).

                if (spResult.Count == 84)
                {
                    respuesta.objObjeto = spResult;
                    respuesta.blnIndicadorTransaccion = true;
                    return respuesta;
                }

                else
                {
                    respuesta.blnIndicadorTransaccion = true;
                    respuesta.objObjeto = null;
                    respuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.DatosInsuficientes;
                }


            }
            catch (CustomException)
            {
                respuesta.blnIndicadorTransaccion = false;
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;                
                respuesta.blnIndicadorTransaccion = false;
                respuesta.strMensaje = msj;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }



        /// <summary>
        /// gValoresRegistradosPorOperador_SinDetalle ejecuta el sp "pa_getValoresRegistradosPorOperador_SinDetalle" y este 
        /// consulta a la base de datos una lista de valores registrados en las plantillas Excel
        /// por un operador específico.
        /// </summary>
        /// <param name="IdOperador"></param>
        /// <returns></returns>
        public Respuesta<List<string>> gValoresRegistradosPorOperador_SinDetalle(string IdOperador, int IdTipoValor, int idDescDetalleAgrupacion)
        {
            List<string> spResult = new List<string>();

            Respuesta<List<string>> respuesta = new Respuesta<List<string>>();
            try
            {

                 spResult = context.pa_getValoresRegistradosPorOperador_SinDetalleNivel(IdOperador, IdTipoValor, idDescDetalleAgrupacion).ToList();

                // Para que se pueda aplicar la Regla se necesitan doce datos. Los últimos doce.
                // El sp top 12 ordenados descendientemente por fecha de registro

                if (spResult.Count == 12)
                {
                    respuesta.objObjeto = spResult;
                    respuesta.blnIndicadorTransaccion = true;
                    return respuesta;
                }

                else
                {
                    respuesta.blnIndicadorTransaccion = true;
                    respuesta.objObjeto = null;
                    respuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.DatosInsuficientes;
                }


            }
            catch (CustomException)
            {
                respuesta.blnIndicadorTransaccion = false;
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.blnIndicadorTransaccion = false;
                respuesta.strMensaje = msj;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        #endregion

        #region Call Stored Procedure Clone

        public Respuesta<Constructor> Clonar(Guid IdConstructor)
        {
            Respuesta<Constructor> respuesta = new Respuesta<Constructor>();
            try
            {
                var consulta = string.Format("EXEC dbo.pa_ClonarConstructor @IdConstructor = {0}", "'" + IdConstructor + "'");
                context.Database.CommandTimeout = 0;
                respuesta.objObjeto = context.Database.SqlQuery<Constructor>(consulta).FirstOrDefault();
            }
            catch (Exception ex)
            {
                respuesta.strMensaje = "Hubo un error al clonar el constructor.";
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }

        #endregion



        #region EliminarReglaEstadistica

        public Respuesta<Constructor> EliminarReglaEstadistica(Guid IdConstructor)
        {
            Respuesta<Constructor> respuesta = new Respuesta<Constructor>();
            try
            {
                string consulta = string.Format("EXEC dbo.pa_EliminarReglaEstadistica @IdConstructor = {0}", "'" + IdConstructor.ToString() + "'");
                consulta = context.Database.SqlQuery<string>(consulta).FirstOrDefault().ToString();
                context.Database.CommandTimeout = 0;

                if (consulta.Equals("FALSE"))
                    respuesta.blnIndicadorTransaccion = false;
                
            }
            catch (Exception ex)
            {
                respuesta.strMensaje = "Hubo un error al eliminar la regla estadística.";
                respuesta.blnIndicadorTransaccion = false;
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }

        #endregion


    }
}
