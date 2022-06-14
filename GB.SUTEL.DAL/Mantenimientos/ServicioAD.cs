using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Omu.ValueInjecter;
using System.Data.Entity;
using GB.SUTEL.Shared;
using System.Data.SqlClient;
using GB.SUTEL.ExceptionHandler;
using System.Configuration;

namespace GB.SUTEL.DAL.Mantenimientos
{
    public class ServicioAD : LocalContextualizer
    {
        private SUTEL_IndicadoresEntities objContext;
        public ServicioAD(ApplicationContext appContext)
            : base(appContext)
        {
            objContext = new SUTEL_IndicadoresEntities();
        }

        #region Agregar
        /// <summary>
        /// Método que agrega un Servicio a la base de datos
        /// </summary>
        /// <param name="objServicio">Objeto tipo Servicio con los datos a agregar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Servicio> Agregar(Servicio objServicio)
        {
            Respuesta<Servicio> objRespuesta = new Respuesta<Servicio>();
            try
            {
                Random random = new Random();
                objServicio.IdServicio = Convert.ToInt32(random.Next(1, 99999).ToString() + random.Next(1, 999).ToString());
                objServicio.Borrado = 0;

                //Set objeto en respuesta
                objRespuesta.objObjeto = objServicio;

                //Execute en la base de datos
                objContext.Servicio.Add(objServicio);
                objContext.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objServicio);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }
        #endregion

        #region Editar
        /// <summary>
        /// Método que edita un Servicio a la base de datos
        /// </summary>
        /// <param name="objServicio">Objeto tipo Servicio con los datos a editar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Servicio> Editar(Servicio objServicio)
        {
            Respuesta<Servicio> objRespuesta = new Respuesta<Servicio>();
            try
            {
                //Set objeto en respuesta
                objServicio.Borrado = 0;
                objRespuesta.objObjeto = objServicio;

                objContext.Servicio.Attach(objServicio);
                objContext.Entry(objServicio).State = EntityState.Modified;
                //Execute en la base de datos                    
                objContext.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objServicio);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }
        #endregion

        #region Eliminar
        /// <summary>
        /// Método que Elimina un Servicio a la base de datos
        /// </summary>
        /// <param name="objServicio">Objeto tipo Servicio con los datos a Eliminar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Servicio> Eliminar(Servicio objServicio)
        {
            Respuesta<Servicio> objRespuesta = new Respuesta<Servicio>();
            try
            {
                //Set objeto en respuesta
                objServicio = this.ConsultaPorID(objServicio).objObjeto;
                objServicio.Borrado = 1;

                objContext.Dispose();
                objContext = new SUTEL_IndicadoresEntities();

                objContext.Servicio.Attach(objServicio);
                objContext.Entry(objServicio).State = EntityState.Modified;
                //Execute en la base de datos                    
                objContext.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objServicio);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            objServicio = new Servicio();
            objRespuesta.objObjeto = objServicio;
            return objRespuesta;
        }
        #endregion

        #region ConsultaPorID
        /// <summary>
        /// Método consulta un Servicio a la base de datos
        /// </summary>
        /// <param name="objServicio">Objeto tipo Servicio con los datos para consultar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Servicio> ConsultaPorID(Servicio objServicio)
        {
            Respuesta<Servicio> objRespuesta = new Respuesta<Servicio>();
            try
            {
                objRespuesta.objObjeto = (
                                 from servicioEntidad in objContext.Servicio
                                 where servicioEntidad.IdServicio == objServicio.IdServicio
                                 select servicioEntidad
                                 ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objServicio);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }
        #endregion

        #region ConsultaPorNombreServicio
        /// <summary>
        /// Método consulta un Servicio a la base de datos
        /// </summary>
        /// <param name="objServicio">Objeto tipo Servicio con los datos para consultar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Servicio> ConsultaPorNombreServicio(Servicio objServicio)
        {
            Respuesta<Servicio> objRespuesta = new Respuesta<Servicio>();
            try
            {
                objRespuesta.objObjeto = (
                              from servicioEntidad in objContext.Servicio
                              where servicioEntidad.DesServicio == objServicio.DesServicio &&
                                    servicioEntidad.Borrado == 0 && servicioEntidad.IdServicio != objServicio.IdServicio
                              select servicioEntidad
                              ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objServicio);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }
        #endregion

        #region ConsultaPorOperador
        /// <summary>
        /// Método consulta un Servicio a la base de datos
        /// </summary>
        /// <param name="objOperador">Objeto tipo operador con los datos para consultar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<List<Servicio>> ConsultaPorOperador(Operador objOperador)
        {

            Respuesta<List<Servicio>> objRespuesta = new Respuesta<List<Servicio>>();
            List<Servicio> oServicios = new List<Servicio>();
            try
            {
                // Execute en la base de datos
                oServicios = (
                             from servicio in objContext.Servicio
                             join servicioPorOperador in objContext.ServicioOperador on servicio.IdServicio equals servicioPorOperador.IdeServicio
                             where servicioPorOperador.IdOperador == objOperador.IdOperador && servicioPorOperador.Borrado == 0
                             select servicio
                            ).ToList();

                objRespuesta.objObjeto = oServicios;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, oServicios);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }
        #endregion

        #region ConsultaPorDependenciaEnSolicitudIndicador
        /// <summary>
        /// Método consulta un Servicio a la base de datos por dependencias
        /// </summary>
        /// <param name="objServicio">Objeto tipo Servicio con los datos para consultar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Servicio> ConsultaPorDependenciaEnSolicitudIndicador(Servicio objServicio)
        {
            Respuesta<Servicio> objRespuesta = new Respuesta<Servicio>();
            try
            {
                objRespuesta.objObjeto = (
                                from servicioEntidad in objContext.Servicio
                                join soliciIndicador in objContext.SolicitudIndicador on servicioEntidad.IdServicio equals soliciIndicador.IdServicio
                                where servicioEntidad.IdServicio == objServicio.IdServicio
                                select servicioEntidad
                                ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objServicio);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }
        #endregion

        #region ConsultaPorDependenciaEnTipoIndicador
        /// <summary>
        /// Método consulta un Servicio a la base de datos por dependencias
        /// </summary>
        /// <param name="objServicio">Objeto tipo Servicio con los datos para consultar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Servicio> ConsultaPorDependenciaEnTipoIndicador(Servicio objServicio)
        {
            Respuesta<Servicio> objRespuesta = new Respuesta<Servicio>();
            try
            {
                objRespuesta.objObjeto = (
                              from servicioEntidad in objContext.Servicio
                              join tipoIndiServ in objContext.TipoIndicadorServicio on servicioEntidad.IdServicio equals tipoIndiServ.IdServicio
                              where servicioEntidad.IdServicio == objServicio.IdServicio
                              select servicioEntidad
                              ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objServicio);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }
        #endregion

        #region ConsultaPorDependenciaEnOperador
        /// <summary>
        /// Método consulta un Servicio a la base de datos por dependencias
        /// </summary>
        /// <param name="objServicio">Objeto tipo Servicio con los datos para consultar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Servicio> ConsultaPorDependenciaEnOperador(Servicio objServicio)
        {
            Respuesta<Servicio> objRespuesta = new Respuesta<Servicio>();
            try
            {
                objRespuesta.objObjeto = (
                              from servicioEntidad in objContext.Servicio
                              join servOperador in objContext.ServicioOperador on servicioEntidad.IdServicio equals servOperador.IdeServicio
                              where servicioEntidad.IdServicio == objServicio.IdServicio
                              select servicioEntidad
                              ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objServicio);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }
        #endregion

        #region ConsultarTodos
        /// <summary>
        /// Método que agrega un Servicio a la base de datos
        /// </summary>
        /// <param name="objServicio">Objeto tipo Servicio</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<List<Servicio>> ConsultarTodos()
        {
            Respuesta<List<Servicio>> objRespuesta = new Respuesta<List<Servicio>>();
            List<Servicio> oServicios = new List<Servicio>();
            try
            {
                //Execute en la base de datos
                oServicios = (
                             from servicioEntidad in objContext.Servicio
                             where servicioEntidad.Borrado == 0
                             orderby servicioEntidad.DesServicio ascending
                             select servicioEntidad
                            ).ToList();

                objRespuesta.objObjeto = oServicios;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, oServicios);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }
        /// <summary>
        /// Filtra los servicios
        /// </summary>
        /// <param name="piIDServicio"></param>
        /// <param name="psNombreServicio"></param>
        /// <returns></returns>
        public Respuesta<List<Servicio>> gFiltrarServicios(int piIDServicio, String psNombreServicio)
        {
            Respuesta<List<Servicio>> objRespuesta = new Respuesta<List<Servicio>>();
            List<Servicio> oServicios = new List<Servicio>();
            try
            {
                //Execute en la base de datos
                oServicios = objContext.Servicio.Where(x => x.Borrado == 0
                     && (piIDServicio.Equals(0) || x.IdServicio.ToString().Contains(piIDServicio.ToString()))
                     && (psNombreServicio.Equals("") || x.DesServicio.ToUpper().Contains(psNombreServicio.ToUpper()))
                            ).ToList();

                objRespuesta.objObjeto = oServicios;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, oServicios);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }
        #endregion

        #region MetodosTipoIndicadoresPorServicio

        /// <summary>
        /// 
        /// </summary>
        /// <param name="poTipoIndicador"></param>
        /// <returns></returns>
        public Respuesta<TipoIndicadorServicio> gAgregarTipoIndicador(TipoIndicadorServicio poTipoIndicador)
        {
            Respuesta<TipoIndicadorServicio> objRespuesta = new Respuesta<TipoIndicadorServicio>();

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    objContext.TipoIndicadorServicio.Add(poTipoIndicador);
                    objContext.SaveChanges();

                    objRespuesta.objObjeto = poTipoIndicador;

                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, poTipoIndicador);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }


        public Respuesta<TipoIndicadorServicio> gEliminarTipoIndicador(TipoIndicadorServicio poTipoIndicador)
        {
            Respuesta<TipoIndicadorServicio> objRespuesta = new Respuesta<TipoIndicadorServicio>();
            TipoIndicadorServicio tisAux = null;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {


                    var dato = objContext.TipoIndicadorServicio.Where(c => c.IdTipoInd == poTipoIndicador.IdTipoInd && c.IdServicio == poTipoIndicador.IdServicio);

                    if (dato != null)
                    {
                        tisAux = dato.First();
                    }

                    objContext.TipoIndicadorServicio.Remove(tisAux);
                    objContext.SaveChanges();

                    objRespuesta.objObjeto = poTipoIndicador;

                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, poTipoIndicador);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }

        /// <summary>
        /// Elimina todos los tipos indicadores de un servicio
        /// </summary>
        /// <param name="poIdServicio"></param>
        /// <returns></returns>
        public Respuesta<List<TipoIndicadorServicio>> gEliminarTipoIndicadorPorServicio(int poIdServicio)
        {
            Respuesta<List<TipoIndicadorServicio>> objRespuesta = new Respuesta<List<TipoIndicadorServicio>>();
            List<TipoIndicadorServicio> tiListAux = null;
            objRespuesta.objObjeto = new List<TipoIndicadorServicio>();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {


                    var dato = objContext.TipoIndicadorServicio.Where(c => c.IdServicio == poIdServicio);

                    if (dato != null)
                    {
                        tiListAux = dato.ToList();
                    }
                    foreach (TipoIndicadorServicio item in tiListAux)
                    {
                        objRespuesta.objObjeto.Add(item);
                        objContext.TipoIndicadorServicio.Remove(item);
                       
                        
                    }
                    objContext.SaveChanges();

                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }
        public Respuesta<List<TipoIndicadorServicio>> gObenerTipoIndicadoresPorServicio(int poIdServicio)
        {

            Respuesta<List<TipoIndicadorServicio>> objRespuesta = new Respuesta<List<TipoIndicadorServicio>>();
            List<TipoIndicadorServicio> oTipoIndicador = new List<TipoIndicadorServicio>();
            try
            {
                //Execute en la base de datos
                //oTipoIndicador = objContext.TipoIndicadorServicio.Where(x => x.IdServicio == poIdServicio).ToList();
                var oTipoIndicadors = (from x in objContext.TipoIndicadorServicio
                                       where x.IdServicio == poIdServicio
                                       select new { IdServicio = x.IdServicio, IdTipoInd = x.IdTipoInd });
                objRespuesta.objObjeto = new List<TipoIndicadorServicio>();
                foreach (var item in oTipoIndicadors)
                {
                    objRespuesta.objObjeto.Add(new TipoIndicadorServicio() { IdTipoInd = item.IdTipoInd, IdServicio = item.IdServicio });
                }


            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, oTipoIndicador);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }

        /// <summary>
        /// Obtiene los tipos de indicador deacuerdo al servicio seleccionado
        /// </summary>
        /// <param name="poIdServicio"></param>
        /// <returns></returns>
        public Respuesta<List<TipoIndicador>> gObenerTipoIndicadoresPorServicioReporte(int poIdServicio)
        {

            Respuesta<List<TipoIndicador>> objRespuesta = new Respuesta<List<TipoIndicador>>();
           
            try
            {
                //Execute en la base de datos
               
                objRespuesta.objObjeto = objContext.TipoIndicador.Where(x =>poIdServicio==0 || x.TipoIndicadorServicio.Any(y => y.IdServicio == poIdServicio)).ToList();


            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objRespuesta.objObjeto);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }

        /// <summary>
        /// Obtiene los indicadores que pertenecen a un determinado servicio
        /// </summary>
        /// <param name="poIdServicio"></param>
        /// <returns></returns>
        public Respuesta<List<Indicador>> gObtenerIndicadoresPorServicioReporte(int poIdServicio)
        {

            Respuesta<List<Indicador>> objRespuesta = new Respuesta<List<Indicador>>();

            try
            {
                //Execute en la base de datos

                objRespuesta.objObjeto = objContext.Indicador.Where(x => poIdServicio == 0 || x.ServicioIndicador.Any(y => y.IdServicio == poIdServicio)).ToList();


            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objRespuesta.objObjeto);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }


        /// <summary>
        /// Obtiene los servicios de un determinado operador
        /// </summary>
        /// <param name="poIdOperador"></param>
        /// <returns></returns>
        public Respuesta<List<Servicio>> gObtenerServiciosPorOperadorReporte(String poIdOperador)
        {

            Respuesta<List<Servicio>> objRespuesta = new Respuesta<List<Servicio>>();
           
            try
            {
                // Execute en la base de datos
                objRespuesta.objObjeto = objContext.Servicio.Where(x => poIdOperador.Equals("") || x.ServicioOperador.Any(y => y.IdOperador == poIdOperador)).ToList();

               
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objRespuesta.objObjeto);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }

        #endregion

        public Respuesta<List<ServicioOperador>> ConsultarServicioOperador(string strOperador) 
        {
            Respuesta<List<ServicioOperador>> objRespuesta = new Respuesta<List<ServicioOperador>>();

            try 
            {
                objRespuesta.objObjeto = (from servicio in objContext.ServicioOperador
                                          where servicio.IdOperador == strOperador && servicio.Borrado == 0
                                          select servicio).ToList();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objRespuesta.objObjeto);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }

        public Respuesta<List<Servicio>> ObtenerNoVerificados(string strOperador)
        {
            Respuesta<List<Servicio>> objRespuesta = new Respuesta<List<Servicio>>();
            try
            {
                objRespuesta.objObjeto= (from servicio in objContext.Servicio 
                                            join servicioOperador in objContext.ServicioOperador on servicio.IdServicio equals servicioOperador.IdeServicio
                                            where servicioOperador.IdOperador == strOperador & servicioOperador.Borrado == 0
                                            & (servicioOperador.Verificar == null || servicioOperador.Verificar == false) select servicio).ToList();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objRespuesta.objObjeto);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }

        public bool SendMail( string asunto, string html)
        {
            try
            {
                string to = (from parametros in objContext.ParametrosGenerales where parametros.Nombre == "CorreoVerificacionServicios" select parametros.Valor).FirstOrDefault();
                string profile = ConfigurationManager.AppSettings["passwordMailingDBProfile"].ToString();
                objContext.pa_SendEmail(to, asunto, html, profile, 100);
                return true;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
                return false;
            }
        }

        public Respuesta<List<Servicio>> ConsultarServicioVerificado(string strOperador, string buscarServicio)
        {
            Respuesta<List<Servicio>> objRespuesta = new Respuesta<List<Servicio>>();
            try
            {
                // Execute en la base de datos 
                objRespuesta.objObjeto  = (
                             from servicio in objContext.Servicio
                             join servicioPorOperador in objContext.ServicioOperador on servicio.IdServicio equals servicioPorOperador.IdeServicio
                             where servicioPorOperador.IdOperador == strOperador && servicioPorOperador.Borrado == 0 && 
                             (buscarServicio.Equals("")||servicio.DesServicio.ToUpper().Contains(buscarServicio.ToUpper()))
                             select servicio
                            ).ToList();

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objRespuesta.objObjeto);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }
    }
}
