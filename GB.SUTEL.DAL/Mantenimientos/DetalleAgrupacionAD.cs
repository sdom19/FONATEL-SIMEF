using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Omu.ValueInjecter;
using System.Data.Entity;
using GB.SUTEL.Shared;
using GB.SUTEL.ExceptionHandler;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using EntityFramework.BulkInsert.Extensions;

namespace GB.SUTEL.DAL.Mantenimientos
{
    public class DetalleAgrupacionAD : LocalContextualizer
    {

        #region Atributos
        SUTEL_IndicadoresEntities context;
        #endregion

        #region Constructores

        public DetalleAgrupacionAD(ApplicationContext appContext)
            : base(appContext)
        {
            context = new SUTEL_IndicadoresEntities();
        }

        #endregion

        #region Metodos


        public Respuesta<DetalleAgrupacion> gAgregar(DetalleAgrupacion poDetalle)
        {
            Respuesta<DetalleAgrupacion> respuesta = new Respuesta<DetalleAgrupacion>();
            DetalleAgrupacion detalleAgregar = new DetalleAgrupacion();

            try
            {
                detalleAgregar.InjectFrom(poDetalle);
                respuesta.objObjeto = poDetalle;
                using (TransactionScope scope = new TransactionScope())
                {

                    context.DetalleAgrupacion.Add(poDetalle);
                    context.SaveChanges();

                    scope.Complete();
                }

                respuesta.objObjeto = poDetalle;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, poDetalle);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }

        public Respuesta<List<DetalleAgrupacion>> gListar()
        {


            List<DetalleAgrupacion> listadoDetalle = new List<DetalleAgrupacion>();
            Respuesta<List<DetalleAgrupacion>> respuesta = new Respuesta<List<DetalleAgrupacion>>();

            try
            {
                IQueryable<DetalleAgrupacion> listadoDetalleAux = context.DetalleAgrupacion.Where(x => x.Borrado == 0).Take(1000);


                listadoDetalle = listadoDetalleAux == null ? null : listadoDetalleAux.ToList();

                respuesta.objObjeto = listadoDetalle;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, listadoDetalle);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        public Respuesta<List<DetalleAgrupacion>> gListarPorFiltros(string poOperador, string poAgrupacion, string poDescripcion)
        {


            List<DetalleAgrupacion> listadoDetalle = new List<DetalleAgrupacion>();
            Respuesta<List<DetalleAgrupacion>> respuesta = new Respuesta<List<DetalleAgrupacion>>();

            try
            {
                IQueryable<DetalleAgrupacion> listadoDetalleAux = (from detalle in context.DetalleAgrupacion
                                                                   join operador in context.Operador on detalle.IdOperador equals operador.IdOperador
                                                                   join agrupacion in context.Agrupacion on detalle.IdAgrupacion equals agrupacion.IdAgrupacion
                                                                   where detalle.Borrado == 0
                                                                      && (poOperador.Equals("") || operador.NombreOperador.ToUpper().Contains(poOperador.ToUpper()))
                                                                      && (poAgrupacion.Equals("") || agrupacion.DescAgrupacion.ToUpper().Contains(poAgrupacion.ToUpper()))
                                                                      && (poDescripcion.Equals("") || detalle.DescDetalleAgrupacion.ToUpper().Contains(poDescripcion.ToUpper()))
                                                                   select detalle);
                //select new DetalleAgrupacion
                //{
                //    IdAgrupacion = detalle.IdAgrupacion,
                //    IdOperador = detalle.IdOperador,
                //    DescDetalleAgrupacion = detalle.DescDetalleAgrupacion
                //});




                listadoDetalle = listadoDetalleAux == null ? null : listadoDetalleAux.ToList();

                respuesta.objObjeto = listadoDetalle;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, listadoDetalle);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        public Respuesta<DetalleAgrupacion> gEliminar(int poIdDetalle)
        {
            Respuesta<DetalleAgrupacion> respuesta = new Respuesta<DetalleAgrupacion>();
            DetalleAgrupacion detalle = new DetalleAgrupacion();

            try
            {

                using (TransactionScope scope = new TransactionScope())
                {

                    // Realizamos la consulta
                    DetalleAgrupacion detalleAux = new DetalleAgrupacion();
                     detalleAux = context.DetalleAgrupacion.Where(c => c.IdDetalleAgrupacion == poIdDetalle).First<DetalleAgrupacion>();

                    detalleAux.Borrado = 1;
                  
                    context.DetalleAgrupacion.Attach(detalleAux);
                    context.Entry(detalleAux).State = EntityState.Modified;
                    int i = context.SaveChanges();

                    scope.Complete();

                    DetalleAgrupacion daAux = new DetalleAgrupacion();
                    daAux.IdDetalleAgrupacion = detalleAux.IdDetalleAgrupacion;
                    daAux.IdAgrupacion = detalleAux.IdAgrupacion;
                    daAux.IdOperador = detalleAux.IdOperador;
                    daAux.DescDetalleAgrupacion = detalleAux.DescDetalleAgrupacion;

                    respuesta.objObjeto = daAux;

                }


            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        public Respuesta<DetalleAgrupacion> gModificar(DetalleAgrupacion poDetalle)
        {
            Respuesta<DetalleAgrupacion> resultado = new Respuesta<DetalleAgrupacion>();
            DetalleAgrupacion nivel = new DetalleAgrupacion();
            try
            {
                nivel.InjectFrom(poDetalle);
                using (TransactionScope scope = new TransactionScope())
                {

                    // Realizamos la consulta
                    DetalleAgrupacion detalleAux = new DetalleAgrupacion();
                    detalleAux = context.DetalleAgrupacion.Where(c => c.IdDetalleAgrupacion == poDetalle.IdDetalleAgrupacion).First();
                    detalleAux.DescDetalleAgrupacion = poDetalle.DescDetalleAgrupacion;
                    context.DetalleAgrupacion.Attach(detalleAux);
                    context.Entry(detalleAux).State = EntityState.Modified;
                    int i = context.SaveChanges();

                    scope.Complete();

                    DetalleAgrupacion daAux = new DetalleAgrupacion();
                    daAux.IdDetalleAgrupacion = detalleAux.IdDetalleAgrupacion;
                    daAux.IdAgrupacion = detalleAux.IdAgrupacion;
                    daAux.IdOperador = detalleAux.IdOperador;
                    daAux.DescDetalleAgrupacion = detalleAux.DescDetalleAgrupacion;

                    resultado.objObjeto = daAux;
                }

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, poDetalle);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene una entidad segun la agrupacion, operador y descripcion
        /// </summary>
        /// <param name="poDescripcionDetalle">Objeto a verificar</param>
        /// <returns></returns>
        public Respuesta<DetalleAgrupacion> gVerificarExistencia(DetalleAgrupacion poDescripcionDetalle)
        {
            Respuesta<DetalleAgrupacion> respuesta = new Respuesta<DetalleAgrupacion>();

            try
            {
                IQueryable<DetalleAgrupacion> entidad = null;


                entidad = context.DetalleAgrupacion.Where(c => c.IdAgrupacion == poDescripcionDetalle.IdAgrupacion &&
                                                       c.IdOperador == poDescripcionDetalle.IdOperador && c.DescDetalleAgrupacion == poDescripcionDetalle.DescDetalleAgrupacion
                                                       && c.Borrado == 0);


                respuesta.objObjeto = entidad.Count() == 0 ? null : entidad.First();
                return respuesta;

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }


        }

        public Respuesta<DetalleAgrupacion> gConsultar(int poIdDetalleAgrupacion)
        {
            Respuesta<DetalleAgrupacion> respuesta = new Respuesta<DetalleAgrupacion>();

            try
            {
                IQueryable<DetalleAgrupacion> entidad = null;


                entidad = context.DetalleAgrupacion.Where(c => c.IdDetalleAgrupacion == poIdDetalleAgrupacion);


                respuesta.objObjeto = entidad.Count() == 0 ? null : entidad.First();
                return respuesta;

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }


        }


        public Respuesta<DetalleAgrupacion> gConsultarAgrupacionClonar(String psAgrupacion, String psDetalleAgrupacion,String psOperador)
        {
            Respuesta<DetalleAgrupacion> respuesta = new Respuesta<DetalleAgrupacion>();

            try
            {
                DetalleAgrupacion entidad = new DetalleAgrupacion();


                entidad = context.DetalleAgrupacion.Include("Agrupacion").Where(c => c.Agrupacion.DescAgrupacion.Trim().Equals(psAgrupacion.Trim())
                                                                           && (c.DescDetalleAgrupacion.Trim().Equals(psDetalleAgrupacion.Trim()))
                                                                           &&   (c.IdOperador.Equals(psOperador))).FirstOrDefault();


                respuesta.objObjeto = entidad;
                return respuesta;

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }


        }
        /// <summary>
        /// <method>NewMethod</method>
        /// verifica que valor hexadecimal
        /// de la descripcion sea igual 
        /// </summary>
        /// <param name="IdOperadorCopia"></param>
        /// <param name="IdOperadorOriginal"></param>
        /// <param name="desAgrupacion"></param>
        /// <param name="desDetalleAgrupacion"></param>
        /// <returns></returns>
        public bool verificarDescHexadecimal(string IdOperadorCopia, string IdOperadorOriginal, string desAgrupacion, string desDetalleAgrupacion)
        {
            bool respuesta = false;
            try
            {
                var consulta = string.Format("EXEC dbo.pa_DescripcionHexadecimal @IdOperadorCopia ={0} , @IdOperadorOriginal = {1}, @descAgrupacion ={2}, @desDetalleAgrupacion ={3}  ", "'" + IdOperadorCopia + "'", "'" + IdOperadorOriginal + "'", "'" + desAgrupacion + "'", "'" + desDetalleAgrupacion + "'");
                context.Database.CommandTimeout = 0;
                respuesta = bool.Parse(context.Database.SqlQuery<string>(consulta).FirstOrDefault());
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                respuesta = false;
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }

       /// <summary>
        /// <method>NewMethod</method>
        /// ejecuta al sp, que clona un
        /// arbol por operador en con el
        /// sp clonarArbol
       /// </summary>
       /// <param name="IdConstructor"></param>
       /// <param name="idcriterio"></param>
       /// <param name="OperadorPadre"></param>
       /// <param name="OperadorClonado"></param>
       /// <returns></returns>
        public bool gClonarDetalleAgrupacionConstructor(string IdConstructor, string idcriterio, string OperadorPadre, string OperadorClonado)
        {
            bool respuesta = false;
           
            try
            { 
                var consulta = string.Format("EXEC dbo.pa_ClonarArbol @p_IdConstructor = {0}, @p_Criterio={1}, @p_IdOperadorOriginal={2}, @p_IdOperadorClonar={3} ", "'" + IdConstructor + "'", "'" + idcriterio + "'", "'" + OperadorPadre + "'", "'" + OperadorClonado + "'");
                context.Database.CommandTimeout = 0;
                respuesta = bool.Parse(context.Database.SqlQuery<string>(consulta).FirstOrDefault());
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                respuesta = false;
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }

        /// <summary>
        /// Obtiene los detalles agrupaciones por operador
        /// </summary>
        /// <returns></returns>
        public Respuesta<List<DetalleAgrupacion>> gObtenerDetallesAgrupacionesPorOperador(String psIDOperador)
        {
            List<DetalleAgrupacion> resultado = new List<DetalleAgrupacion>();
            Respuesta<List<DetalleAgrupacion>> objRespuesta = new Respuesta<List<DetalleAgrupacion>>();
            try
            {

                resultado = context.DetalleAgrupacion.Include("Agrupacion").Include("Operador").Where(x => x.Borrado == 0 && x.IdOperador.Equals(psIDOperador)).ToList();
                    


                objRespuesta.objObjeto = resultado;
            }
            catch (Exception ex)
            {
                objRespuesta.toError(ex.Message, resultado);
            }
            return objRespuesta;

        }

        /// <summary>
        /// Filtro de detalle Agrupacion
        /// </summary>
        /// <param name="psIDOperador"></param>
        /// <param name="psNombreAgrupacion"></param>
        /// <param name="psNombreDetalleAgrupacion"></param>
        /// <returns></returns>
        public Respuesta<List<DetalleAgrupacion>> gFiltrarDetalleAgrupacion(String psIDOperador, String psNombreAgrupacion, String psNombreDetalleAgrupacion)
        {
            List<DetalleAgrupacion> resultado = new List<DetalleAgrupacion>();
            Respuesta<List<DetalleAgrupacion>> objRespuesta = new Respuesta<List<DetalleAgrupacion>>();
            try
            {

                resultado = context.DetalleAgrupacion.Include("Agrupacion").Where(x => x.Borrado == 0 && x.IdOperador.Equals(psIDOperador)
                    &&(psNombreAgrupacion.Equals("") || x.Agrupacion.DescAgrupacion.ToUpper().Contains(psNombreAgrupacion.ToUpper()))
                    && (psNombreDetalleAgrupacion.Equals("") || x.DescDetalleAgrupacion.ToUpper().Contains(psNombreDetalleAgrupacion.ToUpper()))).ToList();



                objRespuesta.objObjeto = resultado;
            }
            catch (Exception ex)
            {
                objRespuesta.toError(ex.Message, resultado);
            }
            return objRespuesta;

        }

        /// <summary>
        /// Se agrega la lista de los detalles en base de datos
        /// </summary>
        /// <param name="listaDetalle">Lista de detalle agrupación</param>
        /// <returns>Detalle agrupación</returns>
        public Respuesta<DetalleAgrupacion> gListaAgregar(List<DetalleAgrupacion> listaDetalle)
        {
            Respuesta<DetalleAgrupacion> respuesta = new Respuesta<DetalleAgrupacion>();

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    context.BulkInsert(listaDetalle, (EntityFramework.BulkInsert.BulkCopyOptions)SqlBulkCopyOptions.FireTriggers);
                    context.SaveChanges();

                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.blnIndicadorTransaccion = false;
                respuesta.strMensaje = msj;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }

        #endregion

        #region Call Stored Procedure

        public Respuesta<List<DetalleAgrupacionSP>> TodosDetalleAgrupacionDetallado(string IdOperador, string IdSolicitud)
        {
            Respuesta<List<DetalleAgrupacionSP>> respuesta = new Respuesta<List<DetalleAgrupacionSP>>();
            try
            {
                var consulta = string.Format("EXEC dbo.pa_getDetalleAgrupacionesPorOperador @IdOperador = {0}, @IdSolicitudIndicador = {1}", "'" + IdOperador.ToString() + "'", "'" + IdSolicitud + "'");
                context.Database.CommandTimeout = 0;
                respuesta.objObjeto = context.Database.SqlQuery<DetalleAgrupacionSP>(consulta).ToList();
            }
            catch (Exception ex)
            {
                respuesta.strMensaje = "Hubo un error al generar el archivo excel.";
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;                
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }

        #endregion

        #region Obtener archivo excel

        public Respuesta<Byte[]> ObtenerArchivoExcel(string IdOperador, string IdSolicitud)
        {
            Respuesta<Byte[]> respuesta = new Respuesta<Byte[]>();
            try
            {
                var consulta = string.Format("EXEC dbo.pa_DescargarArchivoExcel @IdOperador = {0}, @IdSolicitudIndicador = {1}", "'" + IdOperador.ToString() + "'", "'" + IdSolicitud + "'");
                context.Database.CommandTimeout = 0;
                respuesta.objObjeto = context.Database.SqlQuery<Byte[]>(consulta).FirstOrDefault();
            }
            catch (Exception ex)
            {
                respuesta.strMensaje = "Hubo un error al generar el archivo excel.";
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }

        public String actualizarEstadoDescargaExcel(string IdSolicitud, String IdOperador) {

            Guid? idsolicitud = new Guid(IdSolicitud);

            var archivoExel = context.ArchivoExcel.Where(c => c.IdSolicitudIndicador == idsolicitud && c.IdOperador == IdOperador && c.ArchivoExcelGenerado == true && c.ArchivoExcelBytes != null).ToList();

            foreach (var Excels in archivoExel) {

                if (Excels.Descarga == null || Excels.Descarga == false)
                {
                    Excels.Descarga = true;
                
                }
            }
            context.SaveChanges();
            return ("la actualizacion fue completada con exito");
        }

      /*  public bool obtenerEstadoDescargaExcel(string IdSolicitud, string IdOperador)
        {

            Guid? idsolicitud = new Guid(IdSolicitud);
            var archivoExel = context.ArchivoExcel.Where(c => c.IdSolicitudIndicador == idsolicitud && c.IdOperador == IdOperador && c.ArchivoExcelGenerado == true && c.ArchivoExcelBytes != null).First();

            if (archivoExel.Borrado == null)
            {
                archivoExel.Borrado = false;

            }
            if (archivoExel.Descarga == null)
            {
                archivoExel.Descarga = false;
  
            }
            
            context.SaveChanges();

            return (bool)archivoExel.Descarga;
        }
      */
        public String actualizarEstadoCargaExcel(string IdSolicitud, string IdOperador)
        {

            Guid? idsolicitud = new Guid(IdSolicitud);
            var archivoExel = context.SolicitudConstructor.Where(c => c.IdSolicitudIndicador == idsolicitud && c.IdSolicitudIndicador != null && c.IdOperador == IdOperador && c.IdOperador !=null).ToList();

            foreach (var Excels in archivoExel)
            {

                if (Excels.Cargado != null || Excels.Cargado == false)
                {
                    Excels.Cargado = true;

                }
            }
            context.SaveChanges();
            return ("la actualizacion fue completada con exito");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdSolicitud"></param>
        /// <returns></returns>
        public Respuesta<bool> actualizarFechasSolicitud(string IdSolicitud, string FechaIni, string FechaFin)
        {
            Respuesta<bool> resultado = new Respuesta<bool>();
            resultado.objObjeto = false;
            try
            {
                Guid? idsolicitud = new Guid(IdSolicitud);
                var Solicitud = context.SolicitudIndicador.Where(c => c.IdSolicitudIndicador == idsolicitud && c.IdSolicitudIndicador != null).ToList();
                DateTime myDateini = DateTime.ParseExact(FechaIni, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime myDatefin = DateTime.ParseExact(FechaFin, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                foreach (var id in Solicitud)
                {
                    id.FechaInicio = myDateini;
                    id.FechaFin = myDatefin;
                }
                context.SaveChanges();
                }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                resultado.objObjeto = false;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, false);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return resultado;
                        
        }
        /*  public bool obtenerEstadoCargaExcel(string IdSolicitud, string IdOperador)
          {

              Guid? idsolicitud = new Guid(IdSolicitud);
              var archivoExel = context.SolicitudConstructor.Where(c => c.IdSolicitudIndicador == idsolicitud && c.IdOperador == IdOperador).First();

              if (archivoExel.Cargado == null)
              {
                  archivoExel.Cargado = false;
                  context.SaveChanges();
              }
              return (bool)archivoExel.Cargado;
          }*/
        public Respuesta<Byte[]> ObtenerArchivoExcelPre(string IdSolicitud)
        {
            Respuesta<Byte[]> respuesta = new Respuesta<Byte[]>();
            try
            {
               // ArchivoExcel ArchivoAux = new ArchivoExcel();
                Guid? idsolicitud = new Guid(IdSolicitud);
              var   ArchivoAux = context.ArchivoExcel.Where(c => c.IdSolicitudIndicador == idsolicitud && c.ArchivoExcelGenerado == true  && c.ArchivoExcelBytes != null ).Select(c => c.ArchivoExcelBytes).First();
              //  var consulta = string.Format("EXEC dbo.pa_DescargarArchivoExcel @IdOperador = {0}, @IdSolicitudIndicador = {1}", "'" + IdOperador.ToString() + "'", "'" + IdSolicitud + "'");
                context.Database.CommandTimeout = 0;
                if (ArchivoAux == null)
                {
                    respuesta.strMensaje = "El Archivo aun no ha sido generado, intente mas tarde.";
                }
                else
                {
                    respuesta.objObjeto = ArchivoAux;
                }

              
            }
            catch (Exception ex)
            {
                respuesta.strMensaje = "Hubo un error al generar el archivo excel.";
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }

        #endregion
    }
}
