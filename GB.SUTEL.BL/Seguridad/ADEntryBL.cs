using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GB.SUTEL.Entities;
using GB.SUTEL.DAL.Communication;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.Shared;
using GB.SUTEL.ExceptionHandler;

namespace GB.SUTEL.BL.Seguridad
{
    public class ADEntryBL : LocalContextualizer
    {
        private SutelAD objADDacces;
        public ADEntryBL(ApplicationContext appContext)
            : base(appContext)
        {
            objADDacces = new SutelAD(appContext);
        }  
        
        public Respuesta<string> ConsultarTodos()
        {
            Respuesta<string> objRespuesta = new Respuesta<string>();
            StringBuilder users = new StringBuilder();
            users.Append("<option value>Seleccione</option>");   
            try
            {                
                var DArespuesta = objADDacces.ConsultarTodos();
                foreach (var item in DArespuesta.objObjeto)
                {
                    users.Append("<option data-name='" + item.Nombre + "' value='" + item.SAM + "'>" + item.SAM + "</option>");
                }
                objRespuesta.blnIndicadorTransaccion = DArespuesta.blnIndicadorTransaccion;
                objRespuesta.objObjeto = users.ToString();
                return objRespuesta;
            }catch(Exception ex){
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

        }
    }
}
