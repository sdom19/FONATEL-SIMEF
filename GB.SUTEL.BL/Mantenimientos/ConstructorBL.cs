using GB.SUTEL.DAL.Mantenimientos;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SUTEL.Entities.CustomModels.ReglaEstadistica;


namespace GB.SUTEL.BL.Mantenimientos
{
    public class ConstructorBL : LocalContextualizer
    {
        #region atributos
        private ConstructorAD refConstructorAD;
        private FrecuenciaAD refFrecuenciaAD;
        #endregion

        #region metodos
        public ConstructorBL(ApplicationContext appContext)
            : base(appContext)
        {
            refConstructorAD = new ConstructorAD(appContext);
            refFrecuenciaAD = new FrecuenciaAD(appContext);
        }

         /// <summary>
        /// Agregar un constructor
        /// </summary>
        /// <param name="poConstructor"></param>
        /// <returns></returns>
        public Respuesta<Constructor> gAgregarConstructor(Constructor poConstructor)
        {
            Respuesta<Constructor> objRespuesta = new Respuesta<Constructor>();
            Respuesta<Constructor> objRespuestaExistencia = new Respuesta<Constructor>();
            int idDesglose=(poConstructor.IdDesglose==null?0: (int)poConstructor.IdDesglose);
            try
            {
                if (poConstructor.IdIndicador != null)
                {
                    objRespuestaExistencia = refConstructorAD.gObtenerConstructorPorIndDesFrecuencia(poConstructor.IdIndicador, idDesglose, poConstructor.IdFrecuencia);
                    if (lValidarFrecuenciaDesglose(poConstructor.IdFrecuencia, idDesglose) == false) {
                        objRespuesta.blnIndicadorTransaccion = false;
                        objRespuesta.objObjeto = poConstructor;
                        objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.Constructor_FrecuenciaDesglose;
                    }
                    else if (objRespuestaExistencia.objObjeto == null)
                    {
                        objRespuesta = refConstructorAD.gAgregarConstructor(poConstructor);
                    }
                    else
                    {
                        objRespuesta.blnIndicadorTransaccion = false;
                        objRespuesta.objObjeto = poConstructor;
                        objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.Constructor_IndicadorDuplicado;
                    }
                }
                else {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.objObjeto = poConstructor;
                    objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.Constructor_IndicadorRequerido;
                }

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
        /// <method>NewMethod</method>
        /// Agrega el Constructor Criterio 
        /// </summary>
        /// <param name="poConstructorCriterio"></param>
        /// <returns></returns>
        public Respuesta<bool> gAgregarConstructorCriterio(ConstructorCriterio poConstructorCriterio)
        {
            Respuesta<bool> objRespuesta = new Respuesta<bool>();
            try
            {
                if (!refConstructorAD.gVerificarQueExisteCriterio(poConstructorCriterio))
                { 
                    objRespuesta =refConstructorAD.ObjAgregarConstructorCriterio(poConstructorCriterio);
                    objRespuesta.strMensaje = "Agregado Correctamente, Por favor agregar un Detalle de Agrupación.";
                }
                else
                {
                    objRespuesta = refConstructorAD.ObjEditarConstructorCriterio(poConstructorCriterio);
                    objRespuesta.strMensaje = "Actualizado Correctamente";
                }
                if ( objRespuesta.objObjeto)
                {
                    objRespuesta.objObjeto = true;
                    
                }
            }
            catch (Exception e)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, e);

            }
            return objRespuesta;
        }
        /// <summary>
        ///<method>NewMethod</method>
        /// Agrega el Constructor Criterio 
        /// </summary>
        /// <param name="poConstructorCriterio"></param>
        /// <returns></returns>
        public Respuesta<bool> gEliminarConstructorCriterio(ConstructorCriterio poConstructorCriterio)
        {
            Respuesta<bool> objRespuesta = new Respuesta<bool>();
            try
            {
                objRespuesta = refConstructorAD.ObjEliminarConstructorCritero(poConstructorCriterio);
                if (objRespuesta.objObjeto)
                {
                    objRespuesta.objObjeto = true;
                    objRespuesta.strMensaje = "Eliminado Correctamente";
                }
            }
            catch (Exception e)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, e);

            }
            return objRespuesta;
        }
        /// <summary>
        /// <method>NewMethod</method>
        /// </summary>
        /// <param name="IdConstructor"></param>
        /// <param name="idcriterio"></param>
        /// <param name="idOperador"></param>
        /// <returns></returns>
         public Respuesta<bool> gEliminarDetalledeAgrupacionporOperador(Guid IdConstructor, string idcriterio, string idOperador)
        {
            Respuesta<bool> objRespuesta = new Respuesta<bool>();
            try
            {
                objRespuesta = refConstructorAD.ObjEliminarArbolporOperador(IdConstructor, idcriterio, idOperador);
                if (objRespuesta.objObjeto)
                {
                    objRespuesta.objObjeto = true;
                }
            }
            catch (Exception e)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, e);

            }
            return objRespuesta;
        }

        
        /// <summary>
         /// <method>NewMethod</method>
        /// Edita un constructor
        /// </summary>
        /// <param name="poConstructor"></param>
        /// <returns></returns>
        public Respuesta<Constructor> gEditarConstructorfrecuenciayDesglose(Constructor poConstructor)
        {

            Respuesta<Constructor> objRespuesta = new Respuesta<Constructor>();
            Respuesta<Constructor> objRespuestaExistencia = new Respuesta<Constructor>();
            Respuesta<Constructor> objRespuestaExistenciaSolicitud = new Respuesta<Constructor>();
            try
            {
                int idDesglose = (poConstructor.IdDesglose == null ? 0 : (int)poConstructor.IdDesglose);
                objRespuestaExistencia = refConstructorAD.gObtenerConstructorPorIndDesFrecuencia(poConstructor.IdIndicador, idDesglose, poConstructor.IdFrecuencia);
                objRespuestaExistenciaSolicitud = refConstructorAD.gObtenerConstructorVerificar(poConstructor.IdConstructor.ToString());
                //valida que no esta ligado a una solicitud 
                if (objRespuestaExistenciaSolicitud.objObjeto.SolicitudConstructor != null && objRespuestaExistenciaSolicitud.objObjeto.SolicitudConstructor.Count() > 0)
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.objObjeto = poConstructor;
                    objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.Constructor_EnSolicitudEditar;
                }
                else if (lValidarFrecuenciaDesglose(poConstructor.IdFrecuencia, idDesglose) == false) //valida que no esta ligado a una solicitud 
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.objObjeto = poConstructor;
                    objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.Constructor_FrecuenciaDesglose;
                }
                else if (objRespuestaExistencia.objObjeto == null || objRespuestaExistencia.objObjeto.IdConstructor.Equals(poConstructor.IdConstructor))//valida que no exista con la misma frecuencia
                {
                    if (refConstructorAD.EliminarReglaEstadistica(poConstructor.IdConstructor).blnIndicadorTransaccion)
                    {//si cambia la frecuencia elimina regla estadistica?

                        objRespuesta = refConstructorAD.gEditarConstructorFrecuenciayDesglose(poConstructor);//edita el constructor
                    }
                    else
                        objRespuesta.blnIndicadorTransaccion = false;

                }
                else
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.objObjeto = poConstructor;
                    objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.Constructor_IndicadorDuplicado;
                }

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
        /// <method>NewMethod</method>
        /// </summary>
        /// <param name="poIdConstructor"></param>
        /// <param name="poIdCriterio"></param>
        /// <returns></returns>
        public Respuesta<List<ConstructorCriterioDetalleAgrupacion>> gObtenerDetallesAgrupacionporOperador(String poIdConstructor, String poIdCriterio, String IdOperador)
        {
            Respuesta<List<ConstructorCriterioDetalleAgrupacion>> objRespuesta = new Respuesta<List<ConstructorCriterioDetalleAgrupacion>>();
            try
            {
                objRespuesta = refConstructorAD.ObjObtenerDetalleDetalleDeAgrupacionporOperador(poIdConstructor, poIdCriterio, IdOperador);
                
                if (objRespuesta.objObjeto != null && objRespuesta.objObjeto.Count != 0)
                {
                    objRespuesta.strMensaje = "Obtenido Correctamente";
                }

            }
            catch (Exception e)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, e);

            }
            return objRespuesta;
        }
        
        /// <summary>
        ///<method>NewMethod</method>
        /// </summary>
        /// <param name="poConstructorCriterio"></param>
        /// <returns></returns>
        public Respuesta<List<Operador>> gObtenerOperadoresDetalleAgrupacion(Guid poIdConstructor, string poIdCriterio)
        {
            Respuesta<List<Operador>> objRespuesta = new Respuesta<List<Operador>>();
            try
            {
                objRespuesta = refConstructorAD.gObtenerOperadoresDetalleAgrupacion(poIdConstructor,  poIdCriterio);
                if (objRespuesta.objObjeto != null)
                {            
                    objRespuesta.strMensaje = "obtenidos Correctamente";
                }

            }
            catch (Exception e)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, e);

            }
            return objRespuesta;
        }

        public Respuesta<bool> gCrearNuevoDetalleAgrupacionporOperador(ConstructorCriterioDetalleAgrupacion constructorcriteriodetalles, Guid IDconstructor, String IdCriterio)
        {
            Respuesta<bool> objRespuesta = new Respuesta<bool>();
            try
            {
                objRespuesta = refConstructorAD.lAgregarDetalledeAgrupacionConstructor(constructorcriteriodetalles, IDconstructor, IdCriterio);

                if (objRespuesta.objObjeto==true)
                {
                    objRespuesta.strMensaje = "obtenidos Correctamente";
                }

            }
            catch (Exception e)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, e);

            }
            return objRespuesta;
        }
        /// <summary>
        /// Edita un constructor
        /// </summary>
        /// <param name="poConstructor"></param>
        /// <returns></returns>
        public Respuesta<Constructor> gEditarConstructor(Constructor poConstructor)
        {

            Respuesta<Constructor> objRespuesta = new Respuesta<Constructor>();
            Respuesta<Constructor> objRespuestaExistencia = new Respuesta<Constructor>();
            Respuesta<Constructor> objRespuestaExistenciaSolicitud = new Respuesta<Constructor>();
            try
            {  
                int idDesglose=(poConstructor.IdDesglose==null?0: (int)poConstructor.IdDesglose);
                objRespuestaExistencia = refConstructorAD.gObtenerConstructorPorIndDesFrecuencia(poConstructor.IdIndicador, idDesglose, poConstructor.IdFrecuencia);
                objRespuestaExistenciaSolicitud = refConstructorAD.gObtenerConstructorVerificar(poConstructor.IdConstructor.ToString());
                if (objRespuestaExistenciaSolicitud.objObjeto.SolicitudConstructor != null && objRespuestaExistenciaSolicitud.objObjeto.SolicitudConstructor.Count() > 0) 
                {   objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.objObjeto = poConstructor;
                    objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.Constructor_EnSolicitudEditar;
                }
                else if (lValidarFrecuenciaDesglose(poConstructor.IdFrecuencia, idDesglose) == false)
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.objObjeto = poConstructor;
                    objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.Constructor_FrecuenciaDesglose;
                }  
                else if (objRespuestaExistencia.objObjeto == null || objRespuestaExistencia.objObjeto.IdConstructor.Equals(poConstructor.IdConstructor))
                {
                    if (refConstructorAD.EliminarReglaEstadistica(poConstructor.IdConstructor).blnIndicadorTransaccion) {//si cambia la frecuencia elimina regla estadistica?

                        objRespuesta = refConstructorAD.gEditarConstructor(poConstructor);//edita el constructor
                    }else
                        objRespuesta.blnIndicadorTransaccion = false;
                       
                }
                else
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.objObjeto = poConstructor;
                    objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.Constructor_IndicadorDuplicado;
                }

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
        /// Elimina logicamente el constructor
        /// </summary>
        /// <param name="poAgrupacion"></param>
        /// <returns></returns>
        public Respuesta<Constructor> gEliminarConstructor(Constructor poConstructor)
        {

            Respuesta<Constructor> objRespuesta = new Respuesta<Constructor>();
            Respuesta<Constructor> objRespuestaExistencia = new Respuesta<Constructor>();
            try
            {
                objRespuestaExistencia = refConstructorAD.gObtenerConstructorVerificar(poConstructor.IdConstructor.ToString());

                if (objRespuestaExistencia.objObjeto.SolicitudConstructor == null || objRespuestaExistencia.objObjeto.SolicitudConstructor.Count()==0)
                {
                    if(refConstructorAD.EliminarReglaEstadistica(poConstructor.IdConstructor).blnIndicadorTransaccion)
                    {
                        objRespuesta = refConstructorAD.gEliminarConstructor(poConstructor);
                    }
                    else
                    {
                        objRespuesta.blnIndicadorTransaccion = false;
                    }
                }
                else
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.objObjeto = poConstructor;
                    objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.Constructor_EnSolicitud;
                }

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
        /// Valida que la frecuencia sea mayor que el desglose
        /// </summary>
        /// <param name="piIdFrecuencia"></param>
        /// <param name="piIdDesglose"></param>
        /// <returns></returns>
        private bool lValidarFrecuenciaDesglose(int piIdFrecuencia, int piIdDesglose) {
           Respuesta<List<Frecuencia>> listaFrecuencias=new Respuesta<List<Frecuencia>>();
           Frecuencia frecuencia = new Frecuencia();
           Frecuencia desglose = new Frecuencia();
            try{
                listaFrecuencias = refFrecuenciaAD.Listar();
                frecuencia = listaFrecuencias.objObjeto.Where(x => x.IdFrecuencia == piIdFrecuencia).FirstOrDefault();
                desglose = listaFrecuencias.objObjeto.Where(x => x.IdFrecuencia == piIdDesglose).FirstOrDefault();
                if (frecuencia.CantidadMeses < desglose.CantidadMeses) {
                    return false;
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
               
            
            return true;
        }


         /// <summary>
        /// Obtiene los constructores
        /// </summary>
        /// <returns></returns>
        public Respuesta<List<Constructor>> gObtenerConstructores()
        {
            Respuesta<List<Constructor>> objRespuesta = new Respuesta<List<Constructor>>();
            try
            {
                objRespuesta = refConstructorAD.gObtenerConstructores();
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
            return objRespuesta;
        
        }

         /// <summary>
        /// Obtiene los constructores por filtros
        /// </summary>
        /// <returns></returns>
        public Respuesta<List<Constructor>> gObtenerConstructoresPorFiltros(String psIDIndicador, String psIndicador, String psDesglose, String psFrecuencia)
        {
            Respuesta<List<Constructor>> objRespuesta = new Respuesta<List<Constructor>>();
            try
            {
                objRespuesta = refConstructorAD.gObtenerConstructoresPorFiltros(psIDIndicador, psIndicador, psDesglose, psFrecuencia);
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
            return objRespuesta;
        }

        /// <summary>
        /// Obtiene el constructor
        /// </summary>
        /// <returns></returns>
        public Respuesta<Constructor> gObtenerConstructor(String piIdConstructor)
        {
            Respuesta<Constructor> objRespuesta = new Respuesta<Constructor>();
            try
            {
                objRespuesta = refConstructorAD.gObtenerConstructor(piIdConstructor);
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
            Respuesta<List<ConstructorCriterioDetalleAgrupacion>> objRespuesta = new Respuesta<List<ConstructorCriterioDetalleAgrupacion>>();
            try
            {
                objRespuesta = refConstructorAD.gObtenerDetalleAgrupacionConstructor(poIdConstructor, poIdCriterio, poIdOperador);
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
            return objRespuesta;
        
        }

         /// <summary>
        /// Obtiene el detalle agrupacion especifico en el constructor
        /// </summary>
        /// <param name="poIdDetalleAgrupacion"></param>
        public Respuesta<ConstructorCriterioDetalleAgrupacion> gObtenerConstructorCriterioDetalleAgrupacion(Guid poIdDetalleAgrupacion)
        { 

            Respuesta<ConstructorCriterioDetalleAgrupacion> objRespuesta = new Respuesta<ConstructorCriterioDetalleAgrupacion>();
            try
            {
                objRespuesta = refConstructorAD.gObtenerConstructorCriterioDetalleAgrupacion(poIdDetalleAgrupacion);

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
            return objRespuesta;
        }

       
        public Respuesta<Constructor> Clonar(Guid idConstructor)
        {

            Respuesta<Constructor> objRespuesta = new Respuesta<Constructor>();
            try
            {
                objRespuesta = refConstructorAD.Clonar(idConstructor);
                if(objRespuesta.objObjeto==null){
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.strMensaje = "Hubo un error al intentar clonar el constructor";
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
            return objRespuesta;
        }


        /// <summary>
        /// Obtiene el valor minimo y máximo
        /// </summary>
        /// <param name="IdOperador"> Id del operador que se encuentre usando el sistema</param>
        /// <param name="IdTipoValor"> Id del tipo de valor al cual se le va a aplicar la regla</param>
        /// 
        /// <param name="idDescDetalleAgrupacion"> Id del detalleAgrupacion valor al cual se le va a aplicar la regla</param>

        public Respuesta<List<NivelDetalleReglaEstadistica>> ObtenerReglaSinDetalleNivel(string IdOperador, int IdTipoValor, int idDescDetalleAgrupacion)
        {

            Respuesta<List<NivelDetalleReglaEstadistica>> respuestaAController = new Respuesta<List<NivelDetalleReglaEstadistica>>();

            NivelDetalleReglaEstadistica regla = new NivelDetalleReglaEstadistica();

            respuestaAController.objObjeto = new List<NivelDetalleReglaEstadistica>();

            List<double> listaValores = new List<double>();

            var respuesta = refConstructorAD.gValoresRegistradosPorOperador_SinDetalle(IdOperador, IdTipoValor, idDescDetalleAgrupacion);

            if (respuesta.objObjeto != null)
            {

                foreach (var item in respuesta.objObjeto)
                {
                    listaValores.Add(Double.Parse(item));
                }

                double media = listaValores.Sum() / listaValores.Count;

                double desviacionEstandar = ObtenerDesviacionEstandar(listaValores, media);

                var limiteMaximoRango = media + (desviacionEstandar * 2);
                var limiteMinimoRango = media - (desviacionEstandar * 2);

                if (IdTipoValor == 5 || IdTipoValor == 7)
                { //Sin decimales

                    limiteMaximoRango = Convert.ToInt32(limiteMaximoRango);
                    limiteMinimoRango = Convert.ToInt32(limiteMinimoRango);
                }
                else
                {

                    limiteMaximoRango = Math.Truncate(10000 * limiteMaximoRango) / 10000;
                    limiteMinimoRango = Math.Truncate(10000 * limiteMinimoRango) / 10000;


                }

                //No se seleccionó ni Cantón. ni Genero ni Provincia.
                regla.IdNivelDetalle = -1;

                if (limiteMinimoRango > 0)
                    regla.ValorLimiteInferior = limiteMinimoRango.ToString();
                else
                    regla.ValorLimiteInferior = "0";

                regla.ValorLimiteSuperior = limiteMaximoRango.ToString();

                respuestaAController.objObjeto.Add(regla);

            }


            respuestaAController.blnIndicadorTransaccion = respuesta.blnIndicadorTransaccion;
            respuestaAController.strMensaje = respuesta.strMensaje;

            return respuestaAController;

        }



        /// <summary>
        /// Obtiene el valor minimo y máximo para cada provincia
        /// </summary>
        /// <param name="IdOperador"> Id del operador que se encuentre usando el sistema</param>
        /// <param name="IdTipoValor"> Id del tipo de valor al cual se le va a aplicar la regla</param>
        /// <param name="idDescDetalleAgrupacion"> Id del detalleAgrupacion valor al cual se le va a aplicar la regla</param>

        public Respuesta<List<NivelDetalleReglaEstadistica>> ObtenerListaValoresRegla_Provincia(string IdOperador, int IdTipoValor, int idDescDetalleAgrupacion)
        {
            List<NivelDetalleReglaEstadistica> reglaProvinciaList = new List<NivelDetalleReglaEstadistica>();

            NivelDetalleReglaEstadistica reglaAux = new NivelDetalleReglaEstadistica();

            Respuesta<List<NivelDetalleReglaEstadistica>> respuestaAController = new Respuesta<List<NivelDetalleReglaEstadistica>>();

            respuestaAController.objObjeto = new List<NivelDetalleReglaEstadistica>();

            List<double> listaValores = new List<double>();


            //Lista de 84 valores una para cada provincia

            Respuesta<List<pa_getValoresRegistradosPorOperador_Provincia_Result>> respuesta = refConstructorAD.gValoresRegistradosPorOperador_Provincia(IdOperador, IdTipoValor, idDescDetalleAgrupacion);

            if (respuesta.objObjeto != null && respuesta.objObjeto.Count == 84)
            {

                int cont = 1;
                //Se extraen los 12 valores de una misma provincia 
                foreach (var item in respuesta.objObjeto)
                {
                    listaValores.Add(Double.Parse(item.Valor));

                    if (cont < 12) //Se asume que siempre serán doce valores para cada provincia
                    {
                        cont++;
                    }
                    else
                    {
                        // Cuando ya va por el doceavo valor de la provincia se procede a calcular su respectiva regla. 

                        double media = listaValores.Sum() / listaValores.Count;

                        double desviacionEstandar = ObtenerDesviacionEstandar(listaValores, media);



                        var superior = media + (desviacionEstandar * 2);
                        var inferior = media - (desviacionEstandar * 2);

                        if (IdTipoValor == 5 || IdTipoValor == 7)
                        { //Sin decimales

                            superior = Convert.ToInt32(superior);
                            inferior = Convert.ToInt32(inferior);
                        }
                        else
                        {

                            superior = Math.Truncate(10000 * superior) / 10000;
                            inferior = Math.Truncate(10000 * inferior) / 10000;

                        }

                        reglaAux.ValorLimiteSuperior = superior.ToString();

                        if (inferior > 0)
                            reglaAux.ValorLimiteInferior = inferior.ToString();
                        else
                            reglaAux.ValorLimiteInferior = "0";

                        reglaAux.IdGenerico = (int)item.IdProvincia;

                        reglaAux.IdNivelDetalle = 1; // 1 porque es Provincia.

                        reglaProvinciaList.Add(reglaAux);

                        reglaAux = new NivelDetalleReglaEstadistica();

                        //Se reinicia la lista
                        listaValores = new List<double>();

                        cont = 1;
                    }
                }
            }
            else
            {

                //No se obtuvieron 84 valores o el objeto viene null
            }

            respuestaAController.objObjeto = reglaProvinciaList;
            respuestaAController.blnIndicadorTransaccion = respuesta.blnIndicadorTransaccion;
            respuestaAController.strMensaje = respuesta.strMensaje;

            return respuestaAController;

        }

        /// <summary>
        /// Obtiene el valor minimo y máximo para cada cantón
        /// </summary>
        /// <param name="IdOperador"> Id del operador que se encuentre usando el sistema</param>
        /// <param name="IdTipoValor"> Id del tipo de valor al cual se le va a aplicar la regla</param>
        /// <param name="idDescDetalleAgrupacion"> Id del detalleAgrupacion valor al cual se le va a aplicar la regla</param>

        public Respuesta<List<NivelDetalleReglaEstadistica>> ObtenerListaValoresRegla_Canton(string IdOperador, int IdTipoValor, int idDescDetalleAgrupacion)
        {
            List<NivelDetalleReglaEstadistica> reglaProvinciaList = new List<NivelDetalleReglaEstadistica>();

            NivelDetalleReglaEstadistica reglaAux = new NivelDetalleReglaEstadistica();

            Respuesta<List<NivelDetalleReglaEstadistica>> respuestaAController = new Respuesta<List<NivelDetalleReglaEstadistica>>();

            respuestaAController.objObjeto = new List<NivelDetalleReglaEstadistica>();

            List<double> listaValores = new List<double>();


            //Lista de 84 valores una para cada provincia

            Respuesta<List<pa_getValoresRegistradosPorOperador_Canton_Result>> respuesta = refConstructorAD.gValoresRegistradosPorOperador_Canton(IdOperador, IdTipoValor, idDescDetalleAgrupacion);

            if (respuesta.objObjeto != null && respuesta.objObjeto.Count == 972)
            {

                int cont = 1;
                //Se extraen los 12 valores de una misma provincia 
                foreach (var item in respuesta.objObjeto)
                {
                    listaValores.Add(Double.Parse(item.Valor));

                    if (cont < 12) //Se asume que siempre serán doce valores para cada provincia
                    {
                        cont++;
                    }
                    else
                    {
                        // Cuando ya va por el doceavo valor de la provincia se procede a calcular su respectiva regla. 

                        double media = listaValores.Sum() / listaValores.Count;

                        double desviacionEstandar = ObtenerDesviacionEstandar(listaValores, media);

                        var superior = media + (desviacionEstandar * 2);
                        var inferior = media - (desviacionEstandar * 2);



                        if (IdTipoValor == 5 || IdTipoValor == 7)
                        { //Sin decimales

                            superior = Convert.ToInt32(superior);
                            inferior = Convert.ToInt32(inferior);
                        }
                        else
                        {

                            superior = Math.Truncate(10000 * superior) / 10000;
                            inferior = Math.Truncate(10000 * inferior) / 10000;

                        }


                        reglaAux.ValorLimiteSuperior = superior.ToString();

                        if (inferior > 0)
                            reglaAux.ValorLimiteInferior = inferior.ToString();
                        else
                            reglaAux.ValorLimiteInferior = "0";

                        reglaAux.IdGenerico = (int)item.IdCanton;

                        reglaAux.IdNivelDetalle = 2; // 2 porque es Cantón.

                        reglaProvinciaList.Add(reglaAux);

                        reglaAux = new NivelDetalleReglaEstadistica();

                        //Se reinicia la lista
                        listaValores = new List<double>();

                        cont = 1;
                    }
                }
            }
            else
            {

                //No se obtuvieron 84 valores o el objeto viene null
            }

            respuestaAController.objObjeto = reglaProvinciaList;
            respuestaAController.blnIndicadorTransaccion = respuesta.blnIndicadorTransaccion;
            respuestaAController.strMensaje = respuesta.strMensaje;

            return respuestaAController;

        }


        /// <summary>
        /// Obtiene el valor minimo y máximo para cada sexo.
        /// </summary>
        /// <param name="IdOperador"> Id del operador que se encuentre usando el sistema.</param>
        /// <param name="IdTipoValor"> Id del tipo de valor al cual se le va a aplicar la regla.</param>
        /// <param name="idDescDetalleAgrupacion"> Id del detalleAgrupacion valor al cual se le va a aplicar la regla.</param>

        public Respuesta<List<NivelDetalleReglaEstadistica>> ObtenerListaValoresRegla_Genero(string IdOperador, int IdTipoValor, int idDescDetalleAgrupacion)
        {
            List<NivelDetalleReglaEstadistica> reglaProvinciaList = new List<NivelDetalleReglaEstadistica>();

            NivelDetalleReglaEstadistica reglaAux = new NivelDetalleReglaEstadistica();

            Respuesta<List<NivelDetalleReglaEstadistica>> respuestaAController = new Respuesta<List<NivelDetalleReglaEstadistica>>();

            respuestaAController.objObjeto = new List<NivelDetalleReglaEstadistica>();

            List<double> listaValores = new List<double>();


            //Lista de 84 valores una para cada provincia

            Respuesta<List<pa_getValoresRegistradosPorOperador_Genero_Result>> respuesta = refConstructorAD.gValoresRegistradosPorOperador_Genero(IdOperador, IdTipoValor, idDescDetalleAgrupacion);

            if (respuesta.objObjeto != null && respuesta.objObjeto.Count == 24)
            {

                int cont = 1;
                //Se extraen los 12 valores de una misma provincia 
                foreach (var item in respuesta.objObjeto)
                {
                    listaValores.Add(Double.Parse(item.Valor));

                    if (cont < 12) //Se asume que siempre serán doce valores para cada provincia
                    {
                        cont++;
                    }
                    else
                    {
                        // Cuando ya va por el doceavo valor de la provincia se procede a calcular su respectiva regla. 

                        double media = listaValores.Sum() / listaValores.Count;

                        double desviacionEstandar = ObtenerDesviacionEstandar(listaValores, media);

                        var superior = media + (desviacionEstandar * 2);
                        var inferior = media - (desviacionEstandar * 2);



                        if (IdTipoValor == 5 || IdTipoValor == 7)
                        { //Sin decimales

                            superior = Convert.ToInt32(superior);
                            inferior = Convert.ToInt32(inferior);
                        }
                        else
                        {

                            superior = Math.Truncate(10000 * superior) / 10000;
                            inferior = Math.Truncate(10000 * inferior) / 10000;

                        }


                        reglaAux.ValorLimiteSuperior = superior.ToString();

                        if (inferior > 0)
                        reglaAux.ValorLimiteInferior = inferior.ToString();
                        else
                        reglaAux.ValorLimiteInferior = "0";


                        reglaAux.IdGenerico = (int)item.IdGenero;

                        reglaAux.IdNivelDetalle = 3; // 3 porque es Genero.

                        reglaProvinciaList.Add(reglaAux);

                        reglaAux = new NivelDetalleReglaEstadistica();

                        //Se reinicia la lista
                        listaValores = new List<double>();

                        cont = 1;
                    }
                }
            }
            else
            {

                //No se obtuvieron 84 valores o el objeto viene null
            }

            respuestaAController.objObjeto = reglaProvinciaList;
            respuestaAController.blnIndicadorTransaccion = respuesta.blnIndicadorTransaccion;
            respuestaAController.strMensaje = respuesta.strMensaje;

            return respuestaAController;

        } 


        /// <summary>
        /// Obtiene la desviación estandar de una lista de números
        /// </summary>
        /// <param name="valoresIniciales"> Lista de valores numéricos</param>
        /// <param name="media"> Es el promedio de la lista de valores</param>

        public double ObtenerDesviacionEstandar(List<double> valoresIniciales, double media)
        {

           
            List<double> valoresInicialesMenosMedia = new List<double>();
            List<double> valoresInicialesMenosMediaAlCuadrado = new List<double>();
            double sumatoriaValoresIniciales = new double();
            double sumatoriaValoresInicialesMenosMediaAlCuadrado = new double();

            sumatoriaValoresIniciales = valoresIniciales.Sum();

            foreach (var item in valoresIniciales)
            {
                valoresInicialesMenosMedia.Add(item - media);
            }

            foreach (var item in valoresInicialesMenosMedia)
            {
                valoresInicialesMenosMediaAlCuadrado.Add((item * item));
            }

            sumatoriaValoresInicialesMenosMediaAlCuadrado = valoresInicialesMenosMediaAlCuadrado.Sum();

            // se usa la fórmula Muestral NO la Poblacional. Por eso el -1

            var varianza = sumatoriaValoresInicialesMenosMediaAlCuadrado / (valoresInicialesMenosMediaAlCuadrado.Count -1);

            var desviacionEstandar = Math.Sqrt(varianza);

            return desviacionEstandar;

        }
        #endregion

    
    }
}
