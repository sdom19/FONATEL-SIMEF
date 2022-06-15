using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.Entities;
using GB.SUTEL.Shared;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.Shared;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SUTEL.DAL.Proceso
{
    public class BitacoraIndicadorInternoAD : LocalContextualizer
    {
        #region atributos
            private SUTEL_IndicadoresEntities context;
        #endregion
        #region constructor
            public BitacoraIndicadorInternoAD(ApplicationContext appContext)
                : base(appContext)
            {
                context = new SUTEL_IndicadoresEntities(); 

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
                try {
                    poBitacora.IdBitacoIndicador = Guid.NewGuid();
                    poBitacora.FechaModificacion = DateTime.Now;
                    context.BitacoraIndicador.Add(poBitacora);
                    context.SaveChanges();
                    objRespuesta.objObjeto = poBitacora;
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
