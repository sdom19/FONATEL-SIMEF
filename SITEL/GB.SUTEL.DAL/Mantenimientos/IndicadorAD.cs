using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.Entity;
using GB.SUTEL.Shared;
using System.Data.SqlClient;
using GB.SUTEL.ExceptionHandler;


namespace GB.SUTEL.DAL.Mantenimientos
{
    public class IndicadorAD : LocalContextualizer
    {
        private SUTEL_IndicadoresEntities objContext;
        public IndicadorAD(ApplicationContext appContext)
            : base(appContext)
        {
            objContext = new SUTEL_IndicadoresEntities();
        }     

        #region Agregar
        /// <summary>
        /// Método que agrega un Indicador a la base de datos
        /// </summary>
        /// <param name="objIndicador">Objeto Indicador con los datos a agregar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Indicador> Agregar(Indicador objIndicador)
        {
            Respuesta<Indicador> objRespuesta = new Respuesta<Indicador>();
            Indicador nuevoIndicador = new Indicador();

            List<IndicadorUIT> indicadoresUIT = new List<IndicadorUIT>();
            IndicadorUIT nuevoIndicadorUIT = new IndicadorUIT();

            List<Direccion> direcciones = new List<Direccion>();
            Direccion nuevoDireccion = new Direccion();
            try
            {
                //Set objeto en respuesta                                
                nuevoIndicador.Borrado = 0;
                nuevoIndicador.IdIndicador = objIndicador.IdIndicador;
                nuevoIndicador.NombreIndicador = objIndicador.NombreIndicador;
                nuevoIndicador.IdTipoInd = objIndicador.IdTipoInd;
                nuevoIndicador.DescIndicador = objIndicador.DescIndicador;

                if (objIndicador.IndicadorUIT != null)
                {
                    indicadoresUIT.AddRange(new List<IndicadorUIT>(objIndicador.IndicadorUIT));
                    objIndicador.IndicadorUIT = new List<IndicadorUIT>();
                    nuevoIndicador.IndicadorUIT.Clear();
                    foreach (IndicadorUIT item in indicadoresUIT)
                    {
                        nuevoIndicadorUIT = new IndicadorUIT();
                        nuevoIndicadorUIT = objContext.IndicadorUIT.Where(x => x.IdIndicadorUIT.Equals(item.IdIndicadorUIT)).FirstOrDefault();

                        nuevoIndicador.IndicadorUIT.Add(nuevoIndicadorUIT);
                    }
                }

                if (objIndicador.Direccion != null)
                {
                    direcciones.AddRange(new List<Direccion>(objIndicador.Direccion));
                    objIndicador.Direccion = new List<Direccion>();
                    nuevoIndicador.Direccion.Clear();
                    foreach (Direccion item in direcciones)
                    {
                        nuevoDireccion = new Direccion();
                        nuevoDireccion = objContext.Direccion.Where(x => x.IdDireccion.Equals(item.IdDireccion)).FirstOrDefault();

                        nuevoIndicador.Direccion.Add(nuevoDireccion);
                    }
                }

                objRespuesta.objObjeto = objIndicador;

                objContext.Indicador.Add(nuevoIndicador);
                objContext.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objIndicador);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }
        #endregion

        #region Editar
        /// <summary>
        /// Método que edita un Indicador a la base de datos
        /// </summary>
        /// <param name="objIndicador">Objeto Indicador con los datos a editar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Indicador> Editar(Indicador objIndicador)
        {            
            Respuesta<Indicador> objRespuesta = new Respuesta<Indicador>();            
            Indicador nuevoIndicador = new Indicador();

            List<IndicadorUIT> indicadoresUIT = new List<IndicadorUIT>();
            IndicadorUIT nuevoIndicadorUIT = new IndicadorUIT();

            List<Direccion> direcciones = new List<Direccion>();
            Direccion nuevoDireccion = new Direccion();
            try
            {
                //Set objeto en respuesta                
                nuevoIndicador = this.ConsultaPorID(objIndicador).objObjeto;

                nuevoIndicador.Borrado = 0;                
                nuevoIndicador.NombreIndicador = objIndicador.NombreIndicador;
                nuevoIndicador.IdTipoInd = objIndicador.IdTipoInd;
                nuevoIndicador.DescIndicador = objIndicador.DescIndicador;
                
                if (objIndicador.IndicadorUIT != null) { 
                    indicadoresUIT.AddRange(new List<IndicadorUIT>(objIndicador.IndicadorUIT));
                    objIndicador.IndicadorUIT=new List<IndicadorUIT>();
                    nuevoIndicador.IndicadorUIT.Clear();
                    foreach (IndicadorUIT item in  indicadoresUIT)
                    {
                        nuevoIndicadorUIT = new IndicadorUIT();
                        nuevoIndicadorUIT = objContext.IndicadorUIT.Where(x => x.IdIndicadorUIT.Equals(item.IdIndicadorUIT)).FirstOrDefault();

                        nuevoIndicador.IndicadorUIT.Add(nuevoIndicadorUIT);
                    }
                }

                if (objIndicador.Direccion != null)
                {
                    direcciones.AddRange(new List<Direccion>(objIndicador.Direccion));
                    objIndicador.Direccion = new List<Direccion>();
                    nuevoIndicador.Direccion.Clear();
                    foreach (Direccion item in direcciones)
                    {
                        nuevoDireccion = new Direccion();
                        nuevoDireccion = objContext.Direccion.Where(x => x.IdDireccion.Equals(item.IdDireccion)).FirstOrDefault();

                        nuevoIndicador.Direccion.Add(nuevoDireccion);
                    }
                }

                objRespuesta.objObjeto = objIndicador;

                objContext.Indicador.Attach(nuevoIndicador);
                objContext.Entry(nuevoIndicador).State = EntityState.Modified;
                //Execute en la base de datos                    
                objContext.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objIndicador);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }
        #endregion

        #region Eliminar
        /// <summary>
        /// Método que Elimina un Indicador a la base de datos
        /// </summary>
        /// <param name="objIndicador">Objeto Indicador con los datos a Eliminar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Indicador> Eliminar(Indicador objIndicador)
        {
            Respuesta<Indicador> objRespuesta = new Respuesta<Indicador>();
            try
            {
                Indicador nuevoIndicador = new Indicador();
                nuevoIndicador = this.ConsultaPorID(objIndicador).objObjeto;
                //Set objeto en respuesta                
                nuevoIndicador.IndicadorUIT.Clear();
                nuevoIndicador.Direccion.Clear();
                nuevoIndicador.Borrado = 1;

                objContext.Indicador.Attach(nuevoIndicador);
                objContext.Entry(nuevoIndicador).State = EntityState.Modified;
                //Execute en la base de datos                    
                objContext.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objIndicador);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }
        #endregion 

        #region ConsultaDependciaPorConstructor
        /// <summary>
        /// Método consulta un Indicador a la base de datos
        /// </summary>
        /// <param name="objIndicador">Objeto Indicador con los datos para consultar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Indicador> ConsultaDependciaPorConstructor(Indicador objIndicador)
        {
            Respuesta<Indicador> objRespuesta = new Respuesta<Indicador>();
            try
            {
                objRespuesta.objObjeto = (
                                from indicadorEntidad in objContext.Indicador
                                join constrEntidad in objContext.Constructor on indicadorEntidad.IdIndicador equals constrEntidad.IdIndicador
                                where indicadorEntidad.IdIndicador == objIndicador.IdIndicador && constrEntidad.Borrado == 0
                                select indicadorEntidad
                                ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objIndicador);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }
        #endregion

        #region ConsultaDependciaPorDetalleIndicadorCruzado
        /// <summary>
        /// Método consulta un Indicador a la base de datos
        /// </summary>
        /// <param name="objIndicador">Objeto Indicador con los datos para consultar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Indicador> ConsultaDependciaPorDetalleIndicadorCruzado(Indicador objIndicador)
        {
            Respuesta<Indicador> objRespuesta = new Respuesta<Indicador>();
            try
            {
                objRespuesta.objObjeto = (
                                from indicadorEntidad in objContext.Indicador
                                join detalleEntidad in objContext.DetalleIndicadorCruzado on indicadorEntidad.IdIndicador equals detalleEntidad.IdIndicador
                                where indicadorEntidad.IdIndicador == objIndicador.IdIndicador
                                select indicadorEntidad
                                ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objIndicador);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }
        #endregion

        #region ConsultaDependciaPorCriterio
        /// <summary>
        /// Método consulta un Indicador a la base de datos
        /// </summary>
        /// <param name="objIndicador">Objeto Indicador con los datos para consultar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Indicador> ConsultaDependciaPorCriterio(Indicador objIndicador)
        {
            Respuesta<Indicador> objRespuesta = new Respuesta<Indicador>();
            try
            {
                objRespuesta.objObjeto = (
                                from indicadorEntidad in objContext.Indicador
                                join criterioEntidad in objContext.Criterio on indicadorEntidad.IdIndicador equals criterioEntidad.IdIndicador
                                where indicadorEntidad.IdIndicador == objIndicador.IdIndicador && criterioEntidad.Borrado == 0
                                select indicadorEntidad
                                ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objIndicador);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }
        #endregion
        
        #region ConsultaPorID
        /// <summary>
        /// Método consulta un Indicador a la base de datos
        /// </summary>
        /// <param name="objIndicador">Objeto Indicador con los datos para consultar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Indicador> ConsultaPorID(Indicador objIndicador)
        {
            Respuesta<Indicador> objRespuesta = new Respuesta<Indicador>();
            try
            {
                objRespuesta.objObjeto = (
                                from indicadorEntidad in objContext.Indicador
                                where indicadorEntidad.IdIndicador == objIndicador.IdIndicador
                                select indicadorEntidad
                                ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objIndicador);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }
        #endregion

        #region ConsultaPorNombre
        /// <summary>
        /// Método consulta un indicador a la base de datos
        /// </summary>
        /// <param name="objIndicador">Objeto indicador con los datos para consultar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Indicador> ConsultaPorNombre(Indicador objIndicador)
        {
            Respuesta<Indicador> objRespuesta = new Respuesta<Indicador>();
            try
            {
                objRespuesta.objObjeto = (
                               from indicadorEntidad in objContext.Indicador
                               where indicadorEntidad.NombreIndicador == objIndicador.NombreIndicador &&
                                     indicadorEntidad.Borrado == 0 && indicadorEntidad.IdIndicador != objIndicador.IdIndicador
                               select indicadorEntidad
                               ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objIndicador);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }
        #endregion

        #region ConsultarTodos
        /// <summary>
        /// Método que agrega un Indicador a la base de datos
        /// </summary>
        /// <param name="objIndicador">Objeto Indicador</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<List<Indicador>> ConsultarTodos()
        {
            Respuesta<List<Indicador>> objRespuesta = new Respuesta<List<Indicador>>();
            List<Indicador> oIndicadores = new List<Indicador>();
            try
            {
                //Execute en la base de datos
            
                    oIndicadores = (
                             from indicadorEntidad in objContext.Indicador
                             where indicadorEntidad.Borrado == 0
                             select indicadorEntidad
                            ).Take(50).ToList();
               
                

                objRespuesta.objObjeto = oIndicadores;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, oIndicadores);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }




        public Respuesta<List<Indicador>> ConsultarModificadorMasivo(int idServicio)
        {
            Respuesta<List<Indicador>> objRespuesta = new Respuesta<List<Indicador>>();
            List<Indicador> oIndicadores = new List<Indicador>();
            try
            {
                //Execute en la base de datos
                if (idServicio==0)
                {
                    oIndicadores = objContext.Indicador.Where(x => x.Borrado == 0).ToList();
                }
                else
                {   
                        //Execute en la base de datos
                        oIndicadores = objContext.Indicador.Include("TipoIndicador")
                            .Where(x => x.ServicioIndicador.Any(y => y.IdServicio.Equals(idServicio))).ToList();
                }


                objRespuesta.objObjeto = oIndicadores;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, oIndicadores);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }








        /// <summary>
        /// Filtro los indicadores
        /// </summary>
        /// <param name="psIdIndicador"></param>
        /// <param name="psNombreIndicador"></param>
        /// <param name="psTipoIndicador"></param>
        /// <returns></returns>
        public Respuesta<List<Indicador>> gFiltrarIndicadores(String psIdIndicador, String psNombreIndicador, String psTipoIndicador)
        {
            Respuesta<List<Indicador>> objRespuesta = new Respuesta<List<Indicador>>();
            List<Indicador> oIndicadores = new List<Indicador>();
            try
            {
                //Execute en la base de datos
                oIndicadores = objContext.Indicador.Include("TipoIndicador").Where(x => x.Borrado == 0
                            &&(psIdIndicador.Equals("") || x.IdIndicador.ToUpper().Contains(psIdIndicador.ToUpper()))
                            &&(psNombreIndicador.Equals("") || x.NombreIndicador.ToUpper().Contains(psNombreIndicador.ToUpper()))
                            &&(psTipoIndicador.Equals("") || x.TipoIndicador.DesTipoInd.ToUpper().Contains(psTipoIndicador.ToUpper()))
                            ).Take(50).ToList();

                objRespuesta.objObjeto = oIndicadores;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, oIndicadores);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }

        /// <summary>
        /// Obtiene los indicadoros que tiene asociado el indicadorUIT
        /// </summary>
        /// <param name="piIdIndicadorUIT"></param>
        /// <returns></returns>
        public Respuesta<List<Indicador>> gObtenerIndicadorPorIndicadorUIT(int piIdIndicadorUIT)
        {
            Respuesta<List<Indicador>> objRespuesta = new Respuesta<List<Indicador>>();
            List<Indicador> oIndicadores = new List<Indicador>();
            try
            {
                using (SUTEL_IndicadoresEntities objContext = new SUTEL_IndicadoresEntities())
                {
                    //Execute en la base de datos
                    oIndicadores = objContext.Indicador.Where(x=> x.IndicadorUIT.Any(y=> y.IdIndicadorUIT.Equals(piIdIndicadorUIT))).ToList();

                }

                objRespuesta.objObjeto = oIndicadores;
            }
            catch (Exception ex)
            {
                objRespuesta.toError(ex.Message, oIndicadores);
            }

            return objRespuesta;
        }

  

        /// <summary>
        /// Obtiene los indicadores para la modificacion de indicadores
        /// </summary>
        /// <param name="piIdIndicadorUIT"></param>
        /// <returns></returns>
        public Respuesta<List<Indicador>> gObtenerIndicadorModificacioIndicador(int piIdDireccion, int piIdServicio, string psIdIndicador, string psNombreIndicador, string psTipoIndicador)
        {
            Respuesta<List<Indicador>> objRespuesta = new Respuesta<List<Indicador>>();
            List<Indicador> oIndicadores = new List<Indicador>();
            try
            {
                using (SUTEL_IndicadoresEntities objContext = new SUTEL_IndicadoresEntities())
                {
                    //Execute en la base de datos
                    oIndicadores = objContext.Indicador.Include("TipoIndicador").Where(x => x.ServicioIndicador.Any(y => y.IdServicio.Equals(piIdServicio)) 
                                                               && x.Direccion.Any(w=> w.IdDireccion.Equals(piIdDireccion) )
                                                               && (psIdIndicador.Equals("") || x.IdIndicador.ToUpper().Contains(psIdIndicador.ToUpper()))
                                                               && (psTipoIndicador.Equals("") || x.TipoIndicador.DesTipoInd.ToUpper().Contains(psTipoIndicador.ToUpper()))
                                                               && (psNombreIndicador.Equals("") ||x.NombreIndicador.ToUpper().Contains(psNombreIndicador.ToUpper())) ).ToList();

                }

                objRespuesta.objObjeto = oIndicadores;
            }
            catch (Exception ex)
            {
                objRespuesta.toError(ex.Message, oIndicadores);
            }

            return objRespuesta;
        }
        /// <summary>
        /// Obtiene los indicadores deacuerdo a la dirección
        /// </summary>
        /// <param name="piIdDireccion"></param>
        /// <returns></returns>
        public Respuesta<List<Indicador>> gObtenerIndicadorPorDireccion(int piIdDireccion)
        {
            Respuesta<List<Indicador>> objRespuesta = new Respuesta<List<Indicador>>();
            List<Indicador> oIndicadores = new List<Indicador>();

            try
            {
                //Execute en la base de datos
                oIndicadores =  objContext.Indicador.Where(x=>x.Borrado == 0
                     &&( x.Direccion.Any(y=>y.IdDireccion.Equals(piIdDireccion)))
                            
                            ).ToList();

                objRespuesta.objObjeto = oIndicadores;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, oIndicadores);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }

        /// <summary>
        /// Filtro del  Indicador
        /// </summary>
        /// <param name="piIdDireccion"></param>
        /// <param name="psCodigoIndicador"></param>
        /// <param name="psNombreTipoIndicador"></param>
        /// <param name="psIndicador"></param>
        /// <returns></returns>
        public Respuesta<List<Indicador>> gFiltroIndicador(int piIdDireccion, string psCodigoIndicador, string psNombreTipoIndicador, string psIndicador)
        {
            Respuesta<List<Indicador>> objRespuesta = new Respuesta<List<Indicador>>();
            List<Indicador> oIndicadores = new List<Indicador>();

            try
            {
                //Execute en la base de datos
                oIndicadores = objContext.Indicador.Where(x => x.Borrado == 0
                     && (x.Direccion.Any(y => y.IdDireccion.Equals(piIdDireccion)))
                     && (psCodigoIndicador.Equals("") || x.IdIndicador.ToUpper().Contains(psCodigoIndicador.ToUpper()))
                     && (psNombreTipoIndicador.Equals("")|| x.TipoIndicador.DesTipoInd.ToUpper().Contains(psNombreTipoIndicador.ToUpper()))
                     && (psIndicador.Equals("") || x.NombreIndicador.ToUpper().Contains(psIndicador.ToUpper()))
                        ).ToList();

                objRespuesta.objObjeto = oIndicadores;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, oIndicadores);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }

        #endregion       
    }
}
