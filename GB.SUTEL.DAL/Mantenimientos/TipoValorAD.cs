using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.Shared;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.Entities;


namespace GB.SUTEL.DAL.Mantenimientos
{
    public class TipoValorAD : LocalContextualizer
    {
        private SUTEL_IndicadoresEntities objContext;
        public TipoValorAD(ApplicationContext appContext)
            : base(appContext)
        {
            objContext = new SUTEL_IndicadoresEntities();
        }


        /// <summary>
        /// Obtiene los tipos de nivel detalle
        /// </summary>
        /// <returns></returns>
        public Respuesta<List<TipoValor>> gObtenerTiposValor()
        {
            List<TipoValor> resultado = new List<TipoValor>();
            Respuesta<List<TipoValor>> objRespuestas = new Respuesta<List<TipoValor>>();
            try
            {

                resultado = objContext.TipoValor.ToList();

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
    }
}
