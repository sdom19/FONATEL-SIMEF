using GB.SUTEL.DAL.Mantenimientos;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.Shared;
using GB.SUTEL.DAL.Proceso;

namespace GB.SUTEL.BL.Proceso
{
    public class BitacoraIndicadorInternoBL : LocalContextualizer
    {
        #region atributos
        BitacoraIndicadorInternoAD refBitacoraIndicadorInternoAD;
        #endregion

        #region constructor
        public BitacoraIndicadorInternoBL(ApplicationContext appContext)
            : base(appContext)
        {
            refBitacoraIndicadorInternoAD = new BitacoraIndicadorInternoAD(appContext);
        }
        #endregion

        #region metodos
        /// <summary>
        /// Inserta en la bitacora del indicador externo
        /// </summary>
        /// <param name="poBitacora"></param>
        public Respuesta<BitacoraIndicador> gInsertarBitacoraIndicadorExterno(BitacoraIndicador poBitacora)
        {
            Respuesta<BitacoraIndicador> objRespuesta = new Respuesta<BitacoraIndicador>();
            try
            {
                objRespuesta = refBitacoraIndicadorInternoAD.gInsertarBitacoraIndicadorExterno(poBitacora );
            }
          catch (CustomException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(ex.Message, poBitacora);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
            return objRespuesta;
        }
        #endregion
    }
}
