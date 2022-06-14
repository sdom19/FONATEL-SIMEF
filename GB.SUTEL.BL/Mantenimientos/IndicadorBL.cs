using GB.SUTEL.DAL.Mantenimientos;
using GB.SUTEL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.Shared;

namespace GB.SUTEL.BL.Mantenimientos
{
    public class IndicadorBL : LocalContextualizer
    {
        public IndicadorBL(ApplicationContext appContext)
            : base(appContext) {
                refIndicadorAD = new IndicadorAD(appContext);
                refIndicadorUITAD = new IndicadorUITAD(appContext);    
        }

        #region Atributos
        IndicadorAD refIndicadorAD;
        IndicadorUITAD refIndicadorUITAD;
        #endregion

        #region Agregar
        /// <summary>
        /// Método para agregar un Indicador
        /// </summary>
        /// <param name="objIndicador">Objeto a agregar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Indicador> Agregar(Indicador objIndicador, Array IndicadoresUIT, Array direccionesUIT)
        {
            Respuesta<Indicador> objRespuesta = new Respuesta<Indicador>();
            try
            {
                if (refIndicadorAD.ConsultaPorNombre(objIndicador).objObjeto == null)
                {
                    if (refIndicadorAD.ConsultaPorID(objIndicador).objObjeto != null)
                    {
                        if (refIndicadorAD.ConsultaPorID(objIndicador).objObjeto.Borrado == 1)
                        {
                            objRespuesta.strMensaje = "El id ya existe pero se encuentra eliminado, y se conserva como un dato histórico.";
                        }
                        else
                        {
                            objRespuesta.strMensaje = "El id ya existe.";
                        }                        
                        objRespuesta.blnIndicadorTransaccion = false;
                        return objRespuesta;
                    }
                    //List<IndicadorUIT> listIndicadoresUIT = new List<IndicadorUIT>();
                    IndicadorUIT objIndicadorUIT;
                    Direccion objDireccion;

                    if (IndicadoresUIT != null)
                    {
                        foreach (int value in IndicadoresUIT)
                        {
                            objIndicadorUIT = new IndicadorUIT();
                            //objIndicadorUIT = refIndicadorUITAD.gObtenerIndicadorUIT(value).objObjeto;                        
                            objIndicadorUIT.IdIndicadorUIT = value;
                            //listIndicadoresUIT.Add(objIndicadorUIT);
                            objIndicador.IndicadorUIT.Add(objIndicadorUIT);
                        }
                    }

                    if (direccionesUIT != null)
                    {
                        foreach (int value in direccionesUIT)
                        {
                            objDireccion = new Direccion();
                            //objIndicadorUIT = refIndicadorUITAD.gObtenerIndicadorUIT(value).objObjeto;                        
                            objDireccion.IdDireccion = value;
                            //listIndicadoresUIT.Add(objIndicadorUIT);
                            objIndicador.Direccion.Add(objDireccion);
                        }
                    }  

                    if (refIndicadorAD.Agregar(objIndicador).blnIndicadorTransaccion)
                    {
                        objRespuesta.blnIndicadorTransaccion = true;
                    }
                    else
                    {
                        objRespuesta.blnIndicadorTransaccion = false;
                    }
                }
                else
                {
                    objRespuesta.strMensaje = "El nombre ya existe";
                    objRespuesta.blnIndicadorTransaccion = false;
                }
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
        /// Método para Editar un Indicador
        /// </summary>
        /// <param name="objIndicador">Objeto a Editar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Indicador> Editar(Indicador objIndicador, Array IndicadoresUIT, Array direccionesUIT)
        {
            Respuesta<Indicador> objRespuesta = new Respuesta<Indicador>();
            try
            {
                if (refIndicadorAD.ConsultaPorNombre(objIndicador).objObjeto == null)
                {                                   
                    //List<IndicadorUIT> listIndicadoresUIT = new List<IndicadorUIT>();
                    IndicadorUIT objIndicadorUIT;
                    Direccion objDireccion;

                    if (IndicadoresUIT !=null)
                    {
                        foreach (int value in IndicadoresUIT)
                        {
                            objIndicadorUIT = new IndicadorUIT();
                            //objIndicadorUIT = refIndicadorUITAD.gObtenerIndicadorUIT(value).objObjeto;                        
                            objIndicadorUIT.IdIndicadorUIT = value;
                            //listIndicadoresUIT.Add(objIndicadorUIT);
                            objIndicador.IndicadorUIT.Add(objIndicadorUIT);
                        }
                    }

                    if (direccionesUIT != null)
                    {
                        foreach (int value in direccionesUIT)
                        {
                            objDireccion = new Direccion();
                            //objIndicadorUIT = refIndicadorUITAD.gObtenerIndicadorUIT(value).objObjeto;                        
                            objDireccion.IdDireccion = value;
                            //listIndicadoresUIT.Add(objIndicadorUIT);
                            objIndicador.Direccion.Add(objDireccion);
                        }
                    }                    

                    if(refIndicadorAD.Editar(objIndicador).blnIndicadorTransaccion){
                        objRespuesta.blnIndicadorTransaccion = true;
                    }
                    else
                    {
                        objRespuesta.blnIndicadorTransaccion = false;
                    }
                }
                else
                {
                    objRespuesta.strMensaje = "El nombre ya existe";
                    objRespuesta.blnIndicadorTransaccion = false;
                }
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
        /// Método para Eliminar un Indicador
        /// </summary>
        /// <param name="objIndicador">Objeto a Eliminar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Indicador> Eliminar(Indicador objIndicador)
        {
            Respuesta<Indicador> objRespuesta = new Respuesta<Indicador>();
            try
            {
                if (refIndicadorAD.ConsultaDependciaPorConstructor(objIndicador).objObjeto != null)
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.strMensaje = "Existe una dependencia con algún constructor.";

                    return objRespuesta;
                }
                
                if (refIndicadorAD.ConsultaDependciaPorDetalleIndicadorCruzado(objIndicador).objObjeto != null)
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.strMensaje = "Existe una dependencia con algún detalle indicador cruzado.";

                    return objRespuesta;
                }

                if (refIndicadorAD.ConsultaDependciaPorCriterio(objIndicador).objObjeto != null)
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.strMensaje = "Existe una dependencia con algún criterio.";

                    return objRespuesta;
                }

                objRespuesta = refIndicadorAD.Eliminar(objIndicador);
                if (objRespuesta.blnIndicadorTransaccion)
                {
                    return objRespuesta;
                }
                else
                {
                    objRespuesta.strMensaje = "Existió un error desconocido.";
                }
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
        /// Consultar Todos
        /// </summary>
        /// <param name="objIndicador"></param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Indicador> ConsultaPorID(Indicador objIndicador)
        {
            Respuesta<Indicador> objRespuesta = new Respuesta<Indicador>();
            try
            {
                objRespuesta = refIndicadorAD.ConsultaPorID(objIndicador);
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

        #region ConsultarTodos
        /// <summary>
        /// Consultar Todos
        /// </summary>
        /// <param name="objIndicador"></param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<List<Indicador>> ConsultarTodos()
        {
            Respuesta<List<Indicador>> objRespuesta = new Respuesta<List<Indicador>>();
            try
            {
                objRespuesta = refIndicadorAD.ConsultarTodos();
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


        #region ConsultarTodos
        /// <summary>
        /// Consultar Todos Modificador Masivo
        /// </summary>
        /// <param name="objIndicador"></param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<List<Indicador>> ConsultarModificadorMasivo(int idServicio)
        {
            Respuesta<List<Indicador>> objRespuesta = new Respuesta<List<Indicador>>();
            try
            {
                objRespuesta = refIndicadorAD.ConsultarModificadorMasivo(idServicio);
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
        /// Filtro los indicadores
        /// </summary>
        /// <param name="psIdIndicador"></param>
        /// <param name="psNombreIndicador"></param>
        /// <param name="psTipoIndicador"></param>
        /// <returns></returns>
        public Respuesta<List<Indicador>> gFiltrarIndicadores(String psIdIndicador, String psNombreIndicador, String psTipoIndicador)
        {
            Respuesta<List<Indicador>> objRespuesta = new Respuesta<List<Indicador>>();
            try
            {
                objRespuesta = refIndicadorAD.gFiltrarIndicadores(psIdIndicador,psNombreIndicador,psTipoIndicador);
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
        /// Obtiene los indicadoros que tiene asociado el indicadorUIT
        /// </summary>
        /// <param name="piIdIndicadorUIT"></param>
        /// <returns></returns>
        public Respuesta<List<Indicador>> gObtenerIndicadorPorIndicadorUIT(int piIdIndicadorUIT)
        {
            Respuesta<List<Indicador>> objRespuesta = new Respuesta<List<Indicador>>();
            try
            {
                objRespuesta = refIndicadorAD.gObtenerIndicadorPorIndicadorUIT(piIdIndicadorUIT);
            }
            catch (Exception ex)
            {
                objRespuesta.toError(ex.Message, null);
            }
            return objRespuesta;
        }


        // <summary>
        /// Obtiene los indicadores deacuerdo a la dirección
        /// </summary>
        /// <param name="piIdDireccion"></param>
        /// <returns></returns>
        public Respuesta<List<Indicador>> gObtenerIndicadorPorDireccion(int piIdDireccion)
        {
            Respuesta<List<Indicador>> objRespuesta = new Respuesta<List<Indicador>>();
            try
            {
                objRespuesta = refIndicadorAD.gObtenerIndicadorPorDireccion(piIdDireccion);
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
        /// Filtro de indicador
        /// </summary>
        /// <param name="piIdDireccion"></param>
        /// <param name="psCodigoIndicador"></param>
        /// <param name="psNombreTipoIndicador"></param>
        /// <param name="psIndicador"></param>
        /// <returns></returns>
         public Respuesta<List<Indicador>> gFiltroIndicador(int piIdDireccion, string psCodigoIndicador, string psNombreTipoIndicador, string psIndicador)
        {
            Respuesta<List<Indicador>> objRespuesta = new Respuesta<List<Indicador>>();
            try
            {
                objRespuesta = refIndicadorAD.gFiltroIndicador(piIdDireccion,psCodigoIndicador,psNombreTipoIndicador,psIndicador);
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
        /// <summary>
        /// Obtiene los indicadores para la modificacion de indicadores
        /// </summary>
        /// <param name="piIdIndicadorUIT"></param>
        /// <returns></returns>
        public Respuesta<List<Indicador>> gObtenerIndicadorModificacioIndicador(int piIdDireccion, int piIdServicio, string psIdIndicador, string psNombreIndicador, string psTipoIndicador)
         {
             Respuesta<List<Indicador>> objRespuesta = new Respuesta<List<Indicador>>();
             try
             {
                 objRespuesta = refIndicadorAD.gObtenerIndicadorModificacioIndicador(piIdDireccion,piIdServicio,  psIdIndicador,  psNombreIndicador,psTipoIndicador);
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
    }
}
