using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SUTEL.DAL.Mantenimientos
{
    public class TipoNivelDetalleAD : LocalContextualizer
    {
        private SUTEL_IndicadoresEntities objContext;
        public TipoNivelDetalleAD(ApplicationContext appContext)
            : base(appContext)
        {
            objContext = new SUTEL_IndicadoresEntities();

        }

        /// <summary>
        /// Obtiene los tipos de nivel detalle
        /// </summary>
        /// <returns></returns>
        public Respuesta<List<TipoNivelDetalle>> gObtenerTiposNivelDetalle()
        {
            List<TipoNivelDetalle> resultado = new List<TipoNivelDetalle>();
            Respuesta<List<TipoNivelDetalle>> objRespuestas = new Respuesta<List<TipoNivelDetalle>>();
            try
            {

                resultado =  objContext.TipoNivelDetalle.ToList();

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
