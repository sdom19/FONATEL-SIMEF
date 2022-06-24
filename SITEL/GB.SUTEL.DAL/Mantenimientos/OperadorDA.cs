using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.Shared;
using System.Transactions;
using GB.SUTEL.ExceptionHandler;
using System.Data.Entity;

namespace GB.SUTEL.DAL.Mantenimientos
{
    public class OperadorDA : LocalContextualizer
    {
        #region Atributos
        SUTEL_IndicadoresEntities context;
        Operador operador;
        #endregion

        #region Constructor

        public OperadorDA(ApplicationContext appContext)
            : base(appContext)
        {
            context = new SUTEL_IndicadoresEntities();
        }

        public OperadorDA(ApplicationContext appContext, Operador poOperador)
            : base(appContext)
        {
            context = new SUTEL_IndicadoresEntities();
            operador = poOperador;
        }
        #endregion


        #region Metodos
        /// <summary>
        /// Método que agrega un Operador a la base de datos
        /// </summary>
        /// <param name="objOperador">Objeto tipo Operador</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<List<Operador>> ConsultarTodos()
        {
            Respuesta<List<Operador>> objRespuesta = new Respuesta<List<Operador>>();
            List<Operador> oOperadores = new List<Operador>();
            try
            {
                using (SUTEL_IndicadoresEntities objContext = new SUTEL_IndicadoresEntities())
                {
                    //Execute en la base de datos
                    oOperadores = objContext.Operador.Where(x => x.Estado == true).OrderBy(x => x.NombreOperador).ToList();
                }

                objRespuesta.objObjeto = oOperadores;
            }
            catch (Exception ex)
            {
                objRespuesta.toError(ex.Message, oOperadores);
            }

            return objRespuesta;
        }

        public Respuesta<List<Operador>> ConsultarTodosParaDetalleAgrupacion()
        {
            Respuesta<List<Operador>> objRespuesta = new Respuesta<List<Operador>>();
            List<Operador> oOperadores = new List<Operador>();
            try
            {
                using (SUTEL_IndicadoresEntities objContext = new SUTEL_IndicadoresEntities())
                {
                    //Execute en la base de datos
                    oOperadores = objContext.Operador.Where(x => x.Estado == true).OrderBy(x => x.NombreOperador).ToList();
                }

                objRespuesta.objObjeto = oOperadores;
            }
            catch (Exception ex)
            {
                objRespuesta.toError(ex.Message, oOperadores);
            }

            return objRespuesta;
        }

        public Respuesta<Operador> Single(System.Linq.Expressions.Expression<Func<Operador, bool>> expression)
        {
            Respuesta<Operador> objRespuesta = new Respuesta<Operador>();
            try
            {
                using (SUTEL_IndicadoresEntities objContext = new SUTEL_IndicadoresEntities())
                {
                    //Execute en la base de datos
                    objRespuesta.objObjeto = objContext.Operador.Where(expression).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }

        /// <summary>
        /// Obtener un operador
        /// </summary>
        /// <param name="psIdOperador"></param>
        /// <returns></returns>
        public Respuesta<Operador> gObtenerOperador(String psIdOperador)
        {
            Respuesta<Operador> objRespuesta = new Respuesta<Operador>();
            try
            {
                using (SUTEL_IndicadoresEntities objContext = new SUTEL_IndicadoresEntities())
                {
                    //Execute en la base de datos
                    objRespuesta.objObjeto = objContext.Operador.Where(c=> c.IdOperador.Equals(psIdOperador)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        /// <summary>
        /// Filtra los operadores
        /// </summary>
        /// <param name="piIdOperador"></param>
        /// <param name="psNombreOperador"></param>
        /// <returns></returns>
        public Respuesta<List<Operador>> gFiltrarOperadores(String psIdOperador, String psNombreOperador)
        {
            Respuesta<List<Operador>> objRespuesta = new Respuesta<List<Operador>>();
            List<Operador> oOperadores = new List<Operador>();
            try
            {
                
                    //Execute en la base de datos
                    oOperadores = context.Operador.Where(x => x.Estado == true
                        && (String.IsNullOrEmpty(psIdOperador) || x.IdOperador.ToUpper().Contains(psIdOperador.ToString()))
                        &&(psNombreOperador.Equals("")|| x.NombreOperador.ToUpper().Contains(psNombreOperador.ToUpper()))
                        ).OrderBy(x => x.NombreOperador).ToList();
               

                objRespuesta.objObjeto = oOperadores;
            }
             catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(ex.Message, oOperadores);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }

            return objRespuesta;
        }


        public Respuesta<ServicioOperador> InsertarServicio(string poIdOperador, int poIdServicio)
        {
            operador = new Operador();

            operador.IdOperador = poIdOperador;
            return this.InsertarServicio(poIdServicio);
        }

        public Respuesta<ServicioOperador> EliminarServicio(string poIdOperador, int poIdServicio)
        {
            operador = new Operador();

            operador.IdOperador = poIdOperador;
            return this.EliminarServicio(poIdServicio);
        }
        public Respuesta<ServicioOperador> InsertarServicio(int poIdServicio)
        {
            Respuesta<ServicioOperador> respuesta = new Respuesta<ServicioOperador>();
            ServicioOperador servicioOperadorGuardar = new ServicioOperador();


            try
            {
                servicioOperadorGuardar.IdeServicio = poIdServicio;
                servicioOperadorGuardar.IdOperador = operador.IdOperador;
                servicioOperadorGuardar.Borrado = 0;
                respuesta.objObjeto = servicioOperadorGuardar;

                using (TransactionScope scope = new TransactionScope())
                {

                    var yaExiste = context.ServicioOperador.SingleOrDefault(x => x.IdeServicio == poIdServicio && x.IdOperador == operador.IdOperador);

                    if (yaExiste == null)
                    {


                        context.ServicioOperador.Add(servicioOperadorGuardar);
                        context.SaveChanges();

                      
                    }
                    else if (yaExiste.Borrado == 1)
                    {
                        yaExiste.Borrado = 0;

                        context.ServicioOperador.Attach(yaExiste);
                        context.Entry(yaExiste).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                    scope.Complete();
                }


            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, servicioOperadorGuardar);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }


        public Respuesta<ServicioOperador> EliminarServicio(int poIdServicio)
        {
            Respuesta<ServicioOperador> respuesta = new Respuesta<ServicioOperador>();
            ServicioOperador servicioOperadorEliminar = new ServicioOperador();

            try
            {

                using (TransactionScope scope = new TransactionScope())
                {

                    servicioOperadorEliminar = context.ServicioOperador.SingleOrDefault(x => x.IdeServicio == poIdServicio && x.IdOperador == operador.IdOperador);
                   
                    if (servicioOperadorEliminar != null)
                    {
                        servicioOperadorEliminar.Borrado = 1;

                        context.ServicioOperador.Attach(servicioOperadorEliminar);
                        context.Entry(servicioOperadorEliminar).State = EntityState.Modified;
                        context.SaveChanges();
                    }

                    scope.Complete();

                }
                respuesta.objObjeto = servicioOperadorEliminar;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }


        /// <summary>
        /// Consulta los operadores relacionados a un servicio
        /// </summary>
        /// <param name="poIdServicio"></param>
        /// <returns></returns>
        public Respuesta<List<Operador>> gConsultarPorServicio(int poIdServicio) {
            Respuesta<List<Operador>> objRespuesta = new Respuesta<List<Operador>>();
            List<Operador> oOperadores = new List<Operador>();
            try
            {
             
               //IQueryable<Operador> oOperadoresAux
                objRespuesta.objObjeto = (from operadores in context.Operador
                               join servicioxOperador in context.ServicioOperador on operadores.IdOperador equals servicioxOperador.IdOperador
                              where servicioxOperador.IdeServicio == poIdServicio && operadores.Estado == true
                             select operadores).ToList();

                //if (oOperadoresAux.Count() > 1)
                //{
                //    objRespuesta.objObjeto = oOperadoresAux.ToList();
                //}
                //else
                //{
                //    objRespuesta.objObjeto = null;
                //}

            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(ex.Message, oOperadores);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }

            return objRespuesta;
        }

        public Respuesta<List<Operador>> gConsultarPorSolicitud(Guid poIdServicio)
        {
            Respuesta<List<Operador>> objRespuesta = new Respuesta<List<Operador>>();
            List<Operador> oOperadores = new List<Operador>();
            try
            {
                
                objRespuesta.objObjeto = (from operadores in context.Operador
                    join ArchivoExcel in context.ArchivoExcel on operadores.IdOperador equals ArchivoExcel.IdOperador
                    where ArchivoExcel.IdSolicitudIndicador == poIdServicio && operadores.Estado == true && ArchivoExcel.Borrado == false       
                    select operadores).ToList();

                foreach (var operador in objRespuesta.objObjeto) {
                    List<ArchivoExcel> lstArchivoExcel = new List<ArchivoExcel>();
                    lstArchivoExcel = (from archivoExcel in context.ArchivoExcel
                                       where archivoExcel.IdSolicitudIndicador == poIdServicio && archivoExcel.Borrado == false
                                       && archivoExcel.IdOperador == operador.IdOperador
                                       select archivoExcel).ToList();
                    operador.ArchivoExcel = lstArchivoExcel;
                }
                        
            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(ex.Message, oOperadores);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }

            return objRespuesta;
        }

        public Respuesta<bool?> gConsultarFormularioWebPorSolicitud(Guid poIdServicio)
        {
            Respuesta<bool?> objRespuesta = new Respuesta<bool?>();
            objRespuesta.objObjeto = true;
            try
            {
                objRespuesta.objObjeto = context.SolicitudIndicador.Where(x => x.IdSolicitudIndicador == poIdServicio).Select(x => x.FormularioWeb ==1).FirstOrDefault();
            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(ex.Message, false);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }

            return objRespuesta;
        }


        public Respuesta<ServicioOperador> VerificarServicio(string strOperador, List<int> lstServicio) 
        {
            Respuesta<ServicioOperador> respuesta = new Respuesta<ServicioOperador>();
            ServicioOperador servicioOperador = new ServicioOperador();
            try 
            {
                foreach (var item in lstServicio)
                {
                    servicioOperador = new ServicioOperador();
                    servicioOperador = (from servicio in context.ServicioOperador
                                        where servicio.IdOperador == strOperador && servicio.IdeServicio == item
                                        select servicio).FirstOrDefault();

                    servicioOperador.Verificar = true;
                    context.ServicioOperador.Attach(servicioOperador);
                    context.Entry(servicioOperador).State = EntityState.Modified;
                    context.SaveChanges();

                    respuesta.objObjeto = servicioOperador;
                }
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, servicioOperador);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }

        #endregion
    }
}
