using GB.SUTEL.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SUTEL.DAL;
using GB.SUTEL.DAL.Seguridad;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.ExceptionHandler;

namespace GB.SUTEL.BL.Seguridad
{
    public class BitacoraBL : LocalContextualizer
    {
          #region Atributos
        /// <summary>
        /// objeto global de Usuario en la capa de acceso a datos
        /// </summary>
        private BinnacleDA objBitacoraDA;

        public BitacoraBL(ApplicationContext appContext)
            : base(appContext)
        {
            objBitacoraDA = new BinnacleDA(appContext);            
        }

        #endregion

        #region Agregar
        /// <summary>
        /// Método para agregar un Bitacora
        /// </summary>
        public void InsertarBitacora(int accion, string pantalla, string usuario, string descripcion, string newData, string oldData)
        {
            try
            {
                Bitacora objBitacora = new Bitacora();
                objBitacora.Accion = accion;
                objBitacora.Descripcion = descripcion;
                objBitacora.Pantalla = pantalla;
                objBitacora.Usuario = usuario;
                objBitacora.RegistroNuevo = newData;
                objBitacora.RegistroAnterior = oldData;

                objBitacoraDA.InsertarBitacora(objBitacora);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
        }
        #endregion

    }
}
