using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.Shared;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;
using System.Xml.Serialization;

namespace GB.SUTEL.DAL.Seguridad
{
    public class BinnacleDA : LocalContextualizer
    {
        private SUTEL_IndicadoresEntities objContext;
        public BinnacleDA(ApplicationContext appContext)
            : base(appContext)
        {
            objContext = new SUTEL_IndicadoresEntities();
            
        }

        /// <summary>
        /// Inserta en la tabla de bitácora
        /// </summary>
        public void InsertarBitacora(Bitacora bitacora)
        {
            try
            {
                bitacora.FechaBitacora = DateTime.Now;
                
                // Execute en la base de datos
                objContext.Bitacora.Add(bitacora);
                objContext.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
        }

        /// <summary>
        /// convierte de DatosBitacora a Modelo.Bitacora
        /// </summary>
        public  Bitacora CargarDatosBitacora(Bitacora datosBitacora)
        {
            //se crea el objeto bitacora
            Bitacora bitacora = new Bitacora
            {
                IdBitacora = datosBitacora.IdBitacora,
                Pantalla = datosBitacora.Pantalla,
                Accion = datosBitacora.Accion,
                Descripcion = datosBitacora.Descripcion,
                FechaBitacora = datosBitacora.FechaBitacora,
                Usuario = datosBitacora.Usuario,
                RegistroAnterior = datosBitacora.RegistroAnterior,
                RegistroNuevo = datosBitacora.RegistroNuevo
            };
            return bitacora;
        }

        #region ConsultarBitacora

        public Respuesta<List<Bitacora>> ConsultarTodos(Bitacora datosBitacora, DateTime fechaFin)
        {
            Respuesta<List<Bitacora>> objRespuesta = new Respuesta<List<Bitacora>>();
            List<Bitacora> oBitacora = new List<Bitacora>();
            try
            {
                //Execute en la base de datos
                oBitacora = (
                             from bitacoraEntidad in objContext.Bitacora
                             where bitacoraEntidad.Accion == datosBitacora.Accion ||
                                   bitacoraEntidad.Pantalla == datosBitacora.Pantalla ||
                                   bitacoraEntidad.Usuario == datosBitacora.Usuario ||
                                   (bitacoraEntidad.FechaBitacora < datosBitacora.FechaBitacora && bitacoraEntidad.FechaBitacora >= fechaFin)
                             select bitacoraEntidad
                            ).ToList();

                objRespuesta.objObjeto = oBitacora;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, oBitacora);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }

        #endregion       


    }
}
