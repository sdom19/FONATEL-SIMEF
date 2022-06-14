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

namespace GB.SUTEL.BL.Mantenimientos
{
    public class TipoNivelDetalleBL: LocalContextualizer
    {
        TipoNivelDetalleAD refTipoNivelDetalleAD;

        public TipoNivelDetalleBL(ApplicationContext appContext)
            : base(appContext)
        {
            refTipoNivelDetalleAD = new TipoNivelDetalleAD(appContext);
        }
        
        #region metodos
        public Respuesta<List<TipoNivelDetalle>> gObtenerTiposNivelDetalle()
        {

            Respuesta<List<TipoNivelDetalle>> objRespuesta = new Respuesta<List<TipoNivelDetalle>>();
            try
            {
                objRespuesta = refTipoNivelDetalleAD.gObtenerTiposNivelDetalle();
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

    #endregion
    }

    
}
