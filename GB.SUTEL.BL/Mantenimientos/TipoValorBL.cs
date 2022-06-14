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
    public class TipoValorBL : LocalContextualizer
    {
        #region atributos
        TipoValorAD refTipoValorAD;
        #endregion
        #region constructor
        public TipoValorBL(ApplicationContext appContext)
            : base(appContext)
        {
            refTipoValorAD = new TipoValorAD(appContext);
        }
        #endregion

        #region metodos

        /// <summary>
        /// Obtiene los tipos de nivel detalle
        /// </summary>
        /// <returns></returns>
        public Respuesta<List<TipoValor>> gObtenerTiposValor()
        {
            Respuesta<List<TipoValor>> objRespuesta = new Respuesta<List<TipoValor>>();
            try
            {
                objRespuesta = refTipoValorAD.gObtenerTiposValor();
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
