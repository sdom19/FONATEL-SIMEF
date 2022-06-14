using GB.SUTEL.Entities;
using GB.SUTEL.Entities.DTO;
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
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;
using GB.SUTEL.DAL.Proceso;
using System.Configuration;

namespace GB.SUTEL.DAL.Mantenimientos
{
    public class SolicitudIndicadorAD : LocalContextualizer
    {

        #region Atributos
        SUTEL_IndicadoresEntities context;
        private SolicitudIndicador solicitudIndicador;

        public SolicitudIndicador SolicitudIndicador
        {
            get
            {
                return this.solicitudIndicador;
            }
            set
            {
                this.solicitudIndicador = value;
            }
        }

        #endregion

        #region Constructores

        public SolicitudIndicadorAD(ApplicationContext appContext)
            : base(appContext)
        {
            context = new SUTEL_IndicadoresEntities();
        }

        public SolicitudIndicadorAD(SolicitudIndicador solicitudIndicador, ApplicationContext appContext)
            : base(appContext)
        {
            context = new SUTEL_IndicadoresEntities();

        }

        #endregion

        #region Metodos

        /// <summary>
        /// Lista un conjuto de Solicitudes
        /// </summary>
        /// <returns>Listado de Solicitudes</returns>
        public Respuesta<List<SolicitudIndicador>> gListar()
        {
            SUTEL_IndicadoresEntities context = new SUTEL_IndicadoresEntities();

            List<SolicitudIndicador> listadoSolicitudes = new List<SolicitudIndicador>();
            Respuesta<List<SolicitudIndicador>> respuesta = new Respuesta<List<SolicitudIndicador>>();

            try
            {
                listadoSolicitudes = context.SolicitudIndicador.Where(x => x.Borrado == 0).OrderBy(y => y.DescFormulario).ToList();

                respuesta.objObjeto = listadoSolicitudes;

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, listadoSolicitudes);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }


        /// <summary>
        /// Consulta una Solicitud por Descripcion, fecha inicio, fecha fin, estado
        /// </summary>
        /// <param name="poDescripcionFormulario">Descripción de formulario</param>
        /// <param name="poFechaInicio">Fecha de Inicio</param>
        /// <param name="poFechaFinal">Fecha Final</param>
        /// <param name="poEstado">Estado de solicitud</param>
        /// <returns></returns>
        public Respuesta<List<SolicitudIndicador>> gConsultarPorFiltros(string poDescripcionFormulario, DateTime? poFechaInicio, DateTime? poFechaFinal, List<int> poServicios)
        {
            Respuesta<List<SolicitudIndicador>> resultado = new Respuesta<List<SolicitudIndicador>>();
            SolicitudIndicador solicitudIndicador = new SolicitudIndicador();
            try
            {
                IQueryable<SolicitudIndicador> listadoSolicitudes;

                // prueba de la consulta


                if (poFechaFinal == null && poFechaInicio == null)
                {
                    // ignorar las fechas
                    listadoSolicitudes = (from solicitudes in context.SolicitudIndicador
                                          where solicitudes.Borrado == 0 &&
                                                (string.IsNullOrEmpty(poDescripcionFormulario) || solicitudes.DescFormulario.ToUpper().Contains(poDescripcionFormulario.ToUpper()))
                                          select solicitudes);
                }
                else
                {
                    TimeSpan zero = new TimeSpan(0, 0, 0);
                    if (poFechaInicio != null)
                        poFechaInicio = poFechaInicio.Value.Date + zero;

                    if (poFechaFinal != null)
                        poFechaFinal = poFechaFinal.Value.Date + zero;
                    // filtrar con fechas
                    //IQueryable<SolicitudIndicador> listadoSolicitudes = null;

                    if (poFechaInicio != null && poFechaFinal != null)
                    {

                        listadoSolicitudes = (from solicitudes in context.SolicitudIndicador
                                              where solicitudes.Borrado == 0 &&
                                                    (string.IsNullOrEmpty(poDescripcionFormulario) || solicitudes.DescFormulario.ToUpper().Contains(poDescripcionFormulario.ToUpper())) &&
                                                    solicitudes.FechaInicio >= poFechaInicio && solicitudes.FechaFin <= poFechaFinal
                                              select solicitudes);



                    }
                    else if (poFechaInicio != null)
                    {
                        listadoSolicitudes = (from solicitudes in context.SolicitudIndicador
                                              where solicitudes.Borrado == 0 &&
                                                    (string.IsNullOrEmpty(poDescripcionFormulario) || solicitudes.DescFormulario.ToUpper().Contains(poDescripcionFormulario.ToUpper())) &&
                                                    solicitudes.FechaInicio >= poFechaInicio
                                              select solicitudes);
                    }
                    else
                    {
                        listadoSolicitudes = (from solicitudes in context.SolicitudIndicador
                                              where solicitudes.Borrado == 0 &&
                                                    (string.IsNullOrEmpty(poDescripcionFormulario) || solicitudes.DescFormulario.ToUpper().Contains(poDescripcionFormulario.ToUpper())) &&
                                                    solicitudes.FechaFin <= poFechaFinal
                                              select solicitudes);
                    }
                    //resultado.objObjeto = listadoSolicitudes.ToList();
                }
                if (poServicios != null && poServicios.Count > 0)
                    resultado.objObjeto = (from solicitud in listadoSolicitudes
                                           join servicio in poServicios
                                             on solicitud.IdServicio equals servicio
                                           select solicitud).ToList();
                else



                    resultado.objObjeto = listadoSolicitudes.ToList();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            //resultado.objObjeto = (from solicitudes in context.SolicitudIndicador
            //                      join servicio in context.Servicio on solicitudes.IdServicio equals servicio.IdServicio
            //                      select solicitudes);


            return resultado;
        }

        /// <summary>
        /// Consulta una Solicitud Indicador
        /// </summary>
        /// <param name="poIdSolicitud"></param>
        /// <returns></returns>
        public Respuesta<SolicitudIndicador> gConsultarPorIdentificador(Guid poIdSolicitud)
        {
            Respuesta<SolicitudIndicador> resultado = new Respuesta<SolicitudIndicador>();
            SolicitudIndicador solicitudIndicador = new SolicitudIndicador();
            try
            {
                IQueryable<SolicitudIndicador> listadoSolicitudes = context.SolicitudIndicador.Where(x => x.IdSolicitudIndicador == poIdSolicitud);


                resultado.objObjeto = listadoSolicitudes.First();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return resultado;
        }

        public Respuesta<List<ArchivoExcel>> gConsultarPorIdentificadorExcel(Guid poIdSolicitud)
        {
            Respuesta<List<ArchivoExcel>> resultado = new Respuesta<List<ArchivoExcel>>();
            ArchivoExcel Archivoexcel = new ArchivoExcel();
            try
            {
                List<ArchivoExcel> listadoexcel = context.ArchivoExcel.Where(x => x.IdSolicitudIndicador == poIdSolicitud).ToList();
                resultado.objObjeto = listadoexcel;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return resultado;
        }

        public Respuesta<ConfirmaDescargaWebDto> gConsultarConstructorPorIndicador(Guid IdSolicitudIndicador, string IdOperador)
        {
            ConfirmaDescargaWebDto confirmaDescargaWeb = new ConfirmaDescargaWebDto();
            Respuesta<ConfirmaDescargaWebDto> resultado = new Respuesta<ConfirmaDescargaWebDto>();
            try
            {
                var varSolicitudIndicador = context.SolicitudIndicador.Include(x => x.Servicio).Include(x => x.Direccion).Where(X => X.IdSolicitudIndicador == IdSolicitudIndicador).FirstOrDefault();

                SqlConnection sqlConn = (SqlConnection)new SUTEL_IndicadoresEntities().Database.Connection;
                SqlCommand cmdReport = new SqlCommand();
                if (sqlConn.State != ConnectionState.Open)
                {
                    sqlConn.Close();
                    sqlConn.Open();
                }

                cmdReport.Connection = sqlConn;
                cmdReport.CommandType = CommandType.StoredProcedure;
                cmdReport.CommandText = "pa_getDetalleAgrupacionesPorOperador";
                cmdReport.Parameters.Add("@IdOperador", SqlDbType.VarChar, 20).Value = IdOperador;
                cmdReport.Parameters.Add("@IdSolicitudIndicador", SqlDbType.VarChar).Value = IdSolicitudIndicador.ToString();
                SqlDataReader drModel = cmdReport.ExecuteReader();
                List<DetalleAgrupacionesPorOperadorDto> modelList = new List<DetalleAgrupacionesPorOperadorDto>();

                while (drModel.Read())
                {
                    DetalleAgrupacionesPorOperadorDto model = new DetalleAgrupacionesPorOperadorDto();

                    model.Nombre_Detalle_Agrupacion = drModel["Nombre_Detalle_Agrupacion"].ToString();
                    model.Id_Detalle_Agrupacion = drModel["Id_Detalle_Agrupacion"].ToString();
                    model.Nombre_Detalle_Agrupacion = drModel["Nombre_Detalle_Agrupacion"].ToString();
                    model.Nombre_Agrupacion = drModel["Nombre_Agrupacion"].ToString();
                    model.Nombre_Operador = drModel["Nombre_Operador"].ToString();
                    model.IdOperador = drModel["IdOperador"].ToString();
                    model.Nombre_Criterio = drModel["Nombre_Criterio"].ToString();
                    model.ID_Frecuencia = drModel["ID_Frecuencia"].ToString();
                    model.Nombre_Frecuencia = drModel["Nombre_Frecuencia"].ToString();
                    model.Cantidad_Meses_Frecuencia = drModel["Cantidad_Meses_Frecuencia"].ToString();
                    model.ID_Desglose = drModel["ID_Desglose"].ToString();
                    model.Nombre_Desglose = drModel["Nombre_Desglose"].ToString();
                    model.Cantidad_Meses_Desglose = drModel["Cantidad_Meses_Desglose"].ToString();
                    model.ID_Indicador = drModel["ID_Indicador"].ToString();
                    model.Nombre_Indicador = drModel["Nombre_Indicador"].ToString();
                    model.Nivel = drModel["Nivel"].ToString();
                    model.IdNivel = Convert.ToInt32(drModel["IdNivel"]);
                    model.Valor_Inferior = drModel["Valor_Inferior"].ToString();
                    model.Valor_Superior = drModel["Valor_Superior"].ToString();
                    model.Id_Tipo_Valor = drModel["Id_Tipo_Valor"].ToString();
                    model.Tipo_Valor = drModel["Tipo_Valor"].ToString();
                    model.Valor_Formato = drModel["Valor_Formato"].ToString();

                    if (!string.IsNullOrEmpty(drModel["Id_ConstructorCriterio"].ToString()))
                        model.Id_ConstructorCriterio = new Guid(drModel["Id_ConstructorCriterio"].ToString());
                    if (!string.IsNullOrEmpty(drModel["Id_Padre_ConstructorCriterio"].ToString()))
                        model.Id_Padre_ConstructorCriterio = new Guid(drModel["Id_Padre_ConstructorCriterio"].ToString());

                    model.Id_Padre_Detalle_Agrupacion = drModel["Id_Padre_Detalle_Agrupacion"].ToString();
                    model.Nombre_Padre_Detalle_Agrupacion = drModel["Nombre_Padre_Detalle_Agrupacion"].ToString();
                    model.Fecha_Inicio = drModel["Fecha_Inicio"].ToString();
                    model.Fecha_Fin = drModel["Fecha_Fin"].ToString();
                    model.FechaBaseParaCrearExcel = drModel["FechaBaseParaCrearExcel"].ToString();
                    model.UsaReglaEstadisticaConNivelDetalle = drModel["UsaReglaEstadisticaConNivelDetalle"].ToString();
                    model.Constructor_Criterio_Ayuda = drModel["Constructor_Criterio_Ayuda"].ToString();
                    model.Descripcion_Criterio = drModel["Descripcion_Criterio"].ToString();
                    model.Id_Solicitud_Indicador = drModel["Id_Solicitud_Indicador"].ToString();
                    model.Id_Solicitud_Constructor = drModel["Id_Solicitud_Constructor"].ToString();
                    model.Nombre_Direccion = drModel["Nombre_Direccion"].ToString();
                    model.Nombre_Servicio = drModel["Nombre_Servicio"].ToString();
                    model.UltimoNivel = Convert.ToInt32(drModel["UltimoNivel"]);
                    model.Tipo_Nivel_Detalle = drModel["Tipo_Nivel_Detalle"].ToString();
                    model.Id_Tipo_Nivel_Detalle = drModel["Id_Tipo_Nivel_Detalle"].ToString();
                    model.Tabla_Tipo_Nivel_Detalle = drModel["Tabla_Tipo_Nivel_Detalle"].ToString();
                    model.IDNIVELDETALLEGENERO = drModel["IDNIVELDETALLEGENERO"].ToString();
                    model.ayuda = drModel["ayuda"].ToString();
                    model.IdSemaforo = drModel["IdSemaforo"].ToString();
                    model.MesInicial= Convert.ToInt32(drModel["MesInicial"]);
                    //model.NivelMaximo = Convert.ToInt32(drModel["IdNivel"]);

                    modelList.Add(model);
                }
                //modelList.ForEach()  

                // Realizamos la consulta
                confirmaDescargaWeb.provincias = context.Provincia.ToList();
                confirmaDescargaWeb.cantones = context.Canton.ToList();
                confirmaDescargaWeb.distritos = context.Distrito.ToList();

                confirmaDescargaWeb.solicitudIndicador = varSolicitudIndicador;
                confirmaDescargaWeb.detalleAgrupacionesPorOperador = modelList;

                resultado.objObjeto = confirmaDescargaWeb;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return resultado;
        }

        public Respuesta<ConfirmaDescargaWebDto> gConsultarListaDetalleRegistroIndicador(List<Guid> ListaIdSolicitudConstructor)
        {
            ConfirmaDescargaWebDto confirmaDescargaWeb = new ConfirmaDescargaWebDto();
            Respuesta<ConfirmaDescargaWebDto> resultado = new Respuesta<ConfirmaDescargaWebDto>();
            try
            {
                var registroIndicador = context.TEMPRegistroIndicador.Where(x => ListaIdSolicitudConstructor.Contains(x.IdSolicitudConstructor)).Select(x => x.IdRegistroIndicador).Distinct().ToList();

                if (registroIndicador != null && registroIndicador.Count > 0)
                {
                    var detalleRegistroIndicador = context.TEMPDetalleRegistroIndicador.Where(x => registroIndicador.Contains(x.IdRegistroIndicador)).Distinct().ToList();
                    confirmaDescargaWeb.listaDetalleRegistroIndicador = detalleRegistroIndicador;
                }
                resultado.objObjeto = confirmaDescargaWeb;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return resultado;
        }

        public Respuesta<ConfirmaDescargaWebDto> gConsultarDetalleRegistroIndicador(Guid IdSolicitudConstructor)
        {
            ConfirmaDescargaWebDto confirmaDescargaWeb = new ConfirmaDescargaWebDto();
            Respuesta<ConfirmaDescargaWebDto> resultado = new Respuesta<ConfirmaDescargaWebDto>();
            try
            {
                var registroIndicador = context.TEMPRegistroIndicador.Where(x => x.IdSolicitudConstructor == IdSolicitudConstructor).FirstOrDefault();
                if (registroIndicador != null)
                {
                    var detalleRegistroIndicador = context.TEMPDetalleRegistroIndicador.Where(x => x.IdRegistroIndicador == registroIndicador.IdRegistroIndicador).ToList();
                    confirmaDescargaWeb.listaDetalleRegistroIndicador = detalleRegistroIndicador;
                    confirmaDescargaWeb.observacion = registroIndicador.Observacion;
                }
                resultado.objObjeto = confirmaDescargaWeb;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return resultado;
        }

        /// <summary>
        /// Inserta en base de datos un registro de Solicitud
        /// </summary>
        /// <param name="opSolicitudIndicador">Objeto Solicitud</param>
        /// <returns></returns>
        public Respuesta<SolicitudIndicador> gAgregar(SolicitudIndicador opSolicitudIndicador)
        {
            Respuesta<SolicitudIndicador> respuesta = new Respuesta<SolicitudIndicador>();
            SolicitudIndicador solicitudAgregar = new SolicitudIndicador();

            try
            {
                respuesta.objObjeto = opSolicitudIndicador;
                using (TransactionScope scope = new TransactionScope())
                {
                    opSolicitudIndicador.IdSolicitudIndicador = Guid.NewGuid();
                    context.SolicitudIndicador.Add(opSolicitudIndicador);
                    context.SaveChanges();
                    respuesta.objObjeto = opSolicitudIndicador;
                    scope.Complete();

                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, opSolicitudIndicador);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }

        /// <summary>
        /// Modifica una solicitud Indicador
        /// </summary>
        /// <param name="poNivel"></param>
        /// <returns></returns>
        public Respuesta<SolicitudIndicador> gModificar(SolicitudIndicador poSolicitudIndicador)
        {
            Respuesta<SolicitudIndicador> resultado = new Respuesta<SolicitudIndicador>();
            SolicitudIndicador solicitudIndicador = new SolicitudIndicador();
            try
            {

                using (TransactionScope scope = new TransactionScope())
                {
                    resultado.objObjeto = poSolicitudIndicador;
                    // Realizamos la consulta
                    SolicitudIndicador solicitudAux = context.SolicitudIndicador.Where(c => c.IdSolicitudIndicador == poSolicitudIndicador.IdSolicitudIndicador).First();
                    solicitudAux.DescFormulario = poSolicitudIndicador.DescFormulario;
                    solicitudAux.FechaInicio = poSolicitudIndicador.FechaInicio;
                    solicitudAux.FechaFin = poSolicitudIndicador.FechaFin;
                    solicitudAux.IdFrecuencia = poSolicitudIndicador.IdFrecuencia;
                    solicitudAux.IdServicio = poSolicitudIndicador.IdServicio;
                    solicitudAux.IdDireccion = poSolicitudIndicador.IdDireccion;
                    solicitudAux.Activo = poSolicitudIndicador.Activo;
                    solicitudAux.Borrado = poSolicitudIndicador.Borrado;
                    solicitudAux.FechaBaseParaCrearExcel = poSolicitudIndicador.FechaBaseParaCrearExcel;

                    context.SolicitudIndicador.Attach(solicitudAux);
                    context.Entry(solicitudAux).State = EntityState.Modified;
                    int i = context.SaveChanges();

                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, poSolicitudIndicador);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return resultado;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="poSolicitudIndicador"></param>
        /// <returns></returns>
        public Respuesta<SolicitudIndicador> gEliminar(SolicitudIndicador poSolicitudIndicador)
        {
            Respuesta<SolicitudIndicador> respuesta = new Respuesta<SolicitudIndicador>();
            SolicitudIndicador solicitud = new SolicitudIndicador();
            try
            {
                solicitud.InjectFrom(poSolicitudIndicador);
                using (TransactionScope scope = new TransactionScope())
                {

                    // Realizamos la consulta
                    SolicitudIndicador solicitudAux = context.SolicitudIndicador.Where(c => c.IdSolicitudIndicador == poSolicitudIndicador.IdSolicitudIndicador).First();

                    solicitudAux.Borrado = 1;

                    context.SolicitudIndicador.Attach(solicitudAux);
                    context.Entry(solicitudAux).State = EntityState.Modified;
                    int i = context.SaveChanges();
                    respuesta.objObjeto = solicitudAux;
                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, poSolicitudIndicador);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        /// <summary>
        /// Elimina las solicitudes fisicamente, cuando estas no
        /// </summary>
        /// <param name="poSolicitudIndicador"></param>
        /// <returns></returns>
        public Respuesta<SolicitudIndicador> gEliminarFisicamente(SolicitudIndicador poSolicitudIndicador)
        {
            Respuesta<SolicitudIndicador> respuesta = new Respuesta<SolicitudIndicador>();
            SolicitudIndicador solicitud = new SolicitudIndicador();
            List<SolicitudConstructor> constructoresSolicitud = new List<SolicitudConstructor>();
            try
            {
                bool inList = false;
                using (TransactionScope scope = new TransactionScope())
                {

                    // Realizamos la consulta
                    solicitud = context.SolicitudIndicador.Where(c => c.IdSolicitudIndicador == poSolicitudIndicador.IdSolicitudIndicador).First();
                    if (solicitud.SolicitudConstructor != null && solicitud.SolicitudConstructor.Count > 0)
                    {
                        constructoresSolicitud = solicitud.SolicitudConstructor.ToList();
                        foreach (SolicitudConstructor item in constructoresSolicitud)
                        {

                            if (this.gDependenciaRegistroIndicador(item) == null)
                            {
                                inList = true;
                                context.SolicitudConstructor.Attach(item);
                                context.Entry(item).State = EntityState.Deleted;
                                //Execute en la base de datos                    
                                context.SaveChanges();
                            }
                        }

                    }
                    if (inList)
                    {
                        solicitud.SolicitudConstructor.Clear();

                        context.SolicitudIndicador.Remove(solicitud);

                        context.SaveChanges();
                        respuesta.objObjeto = poSolicitudIndicador;
                    }
                    else
                    {
                        respuesta.strMensaje = "Dependencias";
                    }
                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, poSolicitudIndicador);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        /// <summary>        
        /// </summary>
        /// <param name="poSolicitudIndicador"></param>
        /// <returns></returns>
        /// 
        public Respuesta<SolicitudIndicador> gEditarFormularioWeb(System.Guid IdSolicitudIndicador, string[] Operadores, bool FormularioWeb, bool editarOperadores,bool Completa)
        {
            Respuesta<SolicitudIndicador> respuesta = new Respuesta<SolicitudIndicador>();
            SolicitudIndicador solicitud = new SolicitudIndicador();
            List<SolicitudConstructor> constructoresSolicitud = new List<SolicitudConstructor>();
            List<string> listaOperadores = new List<string>();

            bool inList = false;
            // Realizamos la consulta
            solicitud = context.SolicitudIndicador.Where(c => c.IdSolicitudIndicador == IdSolicitudIndicador).First();
            if (solicitud.SolicitudConstructor != null && solicitud.SolicitudConstructor.Count > 0)
            {
                inList = true;
                constructoresSolicitud = solicitud.SolicitudConstructor.Distinct().ToList();

                string operadorDependencias = "";

                var NombreOperadorList = new List<string>();

                foreach (SolicitudConstructor item in constructoresSolicitud)
                {
                    if (item.IdEstado == 4)
                    {
                        if (editarOperadores)
                        {
                            if (inList)
                            {
                                operadorDependencias = operadorDependencias + " -" + item.Operador.NombreOperador + "<br/>";
                                respuesta.strMensaje = "Dependencias";
                                inList = false;
                                NombreOperadorList.Add(item.Operador.NombreOperador);
                            }
                            
                        }
                        if (NombreOperadorList.Where(x => x == item.Operador.NombreOperador).Count() == 0)
                        {
                            var check = Array.Exists(Operadores, x => x == item.Operador.IdOperador);
                            if (check)
                            {
                                operadorDependencias = operadorDependencias + " -" + item.Operador.NombreOperador + "<br/>";
                                respuesta.strMensaje = "Dependencias";
                                inList = false;
                                NombreOperadorList.Add(item.Operador.NombreOperador);
                            }
                            //else {
                            //    //inList = true;
                            //}
                           
                        }
                    }
                }

                respuesta.strData = operadorDependencias;

            }


            if (inList)
            {
                if (editarOperadores)
                {
                    var operadoresTemp = context.ArchivoExcel.Where(c => c.IdSolicitudIndicador == IdSolicitudIndicador).Select(x => x.IdOperador).ToList();
                    foreach (var item in operadoresTemp)
                    {
                        listaOperadores.Add(item);
                    }
                }
                else
                {
                    foreach (var item in Operadores)
                    {
                        listaOperadores.Add(item);
                    }
                }

                using (TransactionScope scope = new TransactionScope())
                {

                    foreach (var item in listaOperadores)
                    {
                        var archivoExcel = context.ArchivoExcel.Where(c => c.IdSolicitudIndicador == IdSolicitudIndicador && c.IdOperador == item).FirstOrDefault();
                        if (archivoExcel != null)
                        {
                            if (FormularioWeb)
                            {
                                archivoExcel.FormularioWeb = true;
                                context.SaveChanges();
                            }
                            else
                            {
                                archivoExcel.FormularioWeb = false;
                                context.SaveChanges();
                            }

                        }
                    }

                    string NombreOperadores = "El tipo de formulario fue actualizado correctamente para los operadores: <br/><br/>";
                    var operadores = context.Operador.Where(x => listaOperadores.Contains(x.IdOperador)).ToList();
                    if (operadores != null)
                    {
                        foreach (var item in operadores)
                        {
                            NombreOperadores = NombreOperadores + "-" + item.NombreOperador + "<br/>";
                        }
                    }

                    respuesta.strMensaje = NombreOperadores;

                    scope.Complete();
                }

                var formularioArchivoExcel = context.ArchivoExcel.Where(c => c.IdSolicitudIndicador == IdSolicitudIndicador).Select(x => x.FormularioWeb).ToList();
                int countWeb = formularioArchivoExcel.Where(s => s == true).Count();
                int countExcel = formularioArchivoExcel.Where(s => s == false).Count();
                int newFormulario = 0;

                if (countWeb > 0 && countExcel > 0)
                    newFormulario = int.Parse(GB.SUTEL.Shared.Logica.FormularioMixto);//mixta
                else if (countWeb > 0)
                    newFormulario = int.Parse(GB.SUTEL.Shared.Logica.FormularioWeb);
                else
                    newFormulario = int.Parse(GB.SUTEL.Shared.Logica.FormularioExcel);


                using (TransactionScope scope = new TransactionScope())
                {
                    var solicitudIndicador = context.SolicitudIndicador.Where(c => c.IdSolicitudIndicador == IdSolicitudIndicador).FirstOrDefault();
                    if (solicitudIndicador != null)
                    {
                        solicitudIndicador.FormularioWeb = newFormulario;
                        context.SaveChanges();
                    }

                    scope.Complete();
                }
            }

            return respuesta;
        }
        public Respuesta<SolicitudIndicador> gEliminarLogicamente(SolicitudIndicador poSolicitudIndicador, string[] Operadores, bool Completa)
        {
            Respuesta<SolicitudIndicador> respuesta = new Respuesta<SolicitudIndicador>();
            SolicitudIndicador solicitud = new SolicitudIndicador();
            List<ArchivoExcel> Arcchivos = new List<ArchivoExcel>();
            List<SolicitudConstructor> constructoresSolicitud = new List<SolicitudConstructor>();
            List<ArchivoExcel> ArchivoSolicitud = new List<ArchivoExcel>();
            try
            {
                bool inList = false;
                using (TransactionScope scope = new TransactionScope())
                {
                    // Realizamos la consulta
                    solicitud = context.SolicitudIndicador.Where(c => c.IdSolicitudIndicador == poSolicitudIndicador.IdSolicitudIndicador).First();
                    if (solicitud.SolicitudConstructor != null && solicitud.SolicitudConstructor.Count > 0)
                    {
                        inList = true;
                        constructoresSolicitud = solicitud.SolicitudConstructor.ToList();
                        foreach (SolicitudConstructor item in constructoresSolicitud)
                        {
                            if (item.IdEstado == 4)
                            {
                                respuesta.strMensaje = "Dependencias";
                                inList = false;
                            }
                        }

                    }
                    else//cuando es basura
                    {
                        solicitud.Borrado = 1;
                        context.SaveChanges();
                    }
                    if (inList)
                    {
                        Arcchivos = context.ArchivoExcel.Where(c => c.IdSolicitudIndicador == poSolicitudIndicador.IdSolicitudIndicador).ToList();
                        if (Completa)
                        {
                            foreach (var item in Arcchivos)
                            {
                                item.Borrado = true;
                                item.ArchivoExcelGenerado = false;
                                context.SaveChanges();
                            }

                            solicitud.Borrado = 1;
                            context.SaveChanges();
                        }
                        else
                        {

                            foreach (var item in Arcchivos)
                            {
                                foreach (var idoperador in Operadores)
                                {
                                    if (idoperador == item.IdOperador)
                                    {
                                        item.Borrado = true;
                                        item.ArchivoExcelGenerado = false;
                                        context.SaveChanges();
                                    }
                                }
                            }

                        }
                        respuesta.objObjeto = poSolicitudIndicador;
                    }

                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, poSolicitudIndicador);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }
        public Respuesta<List<RegistroIndicador>> gDependenciaRegistroIndicador(SolicitudConstructor poSolicitudConstructor)
        {
            Respuesta<List<RegistroIndicador>> respuesta = new Respuesta<List<RegistroIndicador>>();
            try
            {
                respuesta.objObjeto = context.RegistroIndicador.Where(c => c.IdSolicitudConstructor == poSolicitudConstructor.IdSolicitudContructor).ToList();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        /// <summary>
        /// Consulta una Solicitud por Descripción
        /// </summary>
        /// <param name="poDescripcion"></param>
        /// <returns></returns>
        public Respuesta<SolicitudIndicador> gConsultarPorDescripcion(string poDescripcion)
        {
            Respuesta<SolicitudIndicador> resultado = new Respuesta<SolicitudIndicador>();
            SolicitudIndicador solicitud = null;
            try
            {

                // Realizamos la consulta
                var rows = context.SolicitudIndicador.Where(c => c.DescFormulario == poDescripcion && c.Borrado == 0);

                if (rows.Count() > 0)
                {
                    solicitud = rows.First();
                }

                resultado.objObjeto = solicitud;
                return resultado;

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, solicitud);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }




        }

        /// <summary>
        /// Consulta por Identificador
        /// </summary>
        /// <param name="poIdSolicitud"></param>
        /// <returns></returns>
        public Respuesta<List<SolicitudIndicador>> gConsultarPorId(Guid poIdSolicitud)
        {
            Respuesta<List<SolicitudIndicador>> resultado = new Respuesta<List<SolicitudIndicador>>();
            List<SolicitudIndicador> solicitudes = null;
            try
            {
                IQueryable<SolicitudIndicador> rows = null;

                // Realizamos la consulta si brindaron los dos parametros

                rows = context.SolicitudIndicador.Where(c => c.IdSolicitudIndicador == poIdSolicitud && c.Borrado == 0);


                if (rows.Count() > 0)
                {
                    solicitudes = rows.ToList();
                }

                resultado.objObjeto = solicitudes;
                return resultado;

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, solicitudes);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }




        }

        #region SolicitudConstructor

        /// <summary>
        /// Obtiene un listado de indicadores para el mantenimiento de solicitud
        /// </summary>
        /// <param name="poIdDireccion"></param>
        /// <param name="poIdFrecuencia"></param>
        /// <returns></returns>
        public Respuesta<List<pa_getListaIndicadoresSolicitud_Result>> gConsultarIndicadores(int poIdDireccion, int poIdFrecuencia)
        {

            Respuesta<List<pa_getListaIndicadoresSolicitud_Result>> resultado = new Respuesta<List<pa_getListaIndicadoresSolicitud_Result>>();
            resultado.objObjeto = new List<pa_getListaIndicadoresSolicitud_Result>();
            try
            {
                resultado.objObjeto = context.pa_getListaIndicadoresSolicitud(null, poIdDireccion, poIdFrecuencia, false, string.Empty, 0).ToList();

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return resultado;
        }


        /// <summary>
        /// Obtiene un listado de indicadores para el mantenimiento de solicitud
        /// </summary>
        /// <param name="poIdDireccion"></param>
        /// <param name="poIdFrecuencia"></param>
        /// <returns></returns>
        public Respuesta<List<pa_getListaIndicadoresSolicitud_Result>> gConsultarIndicadoresPorDirFrecServ(int poIdDireccion, int poIdFrecuencia, int idServicio)
        {

            Respuesta<List<pa_getListaIndicadoresSolicitud_Result>> resultado = new Respuesta<List<pa_getListaIndicadoresSolicitud_Result>>();
            resultado.objObjeto = new List<pa_getListaIndicadoresSolicitud_Result>();
            try
            {
                resultado.objObjeto = context.pa_getListaIndicadoresSolicitud(null, poIdDireccion, poIdFrecuencia, false, string.Empty, idServicio).ToList();

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene un listado de indicadores para el mantenimiento de solicitud asociados a una solicitud
        /// </summary>
        /// <param name="poIdOperador"></param>
        /// <param name="poIdDireccion"></param>
        /// <param name="poIdFrecuencia"></param>
        /// <returns></returns>
        public Respuesta<List<pa_getListaIndicadoresSolicitud_Result>> gConsultarIndicadores(string poIdOperador, int poIdDireccion, int poIdFrecuencia)
        {
            Respuesta<List<pa_getListaIndicadoresSolicitud_Result>> resultado = new Respuesta<List<pa_getListaIndicadoresSolicitud_Result>>();
            resultado.objObjeto = new List<pa_getListaIndicadoresSolicitud_Result>();
            try
            {
                resultado.objObjeto = context.pa_getListaIndicadoresSolicitud(poIdOperador, poIdDireccion, poIdFrecuencia, false, string.Empty, 0).ToList();

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return resultado;
        }

        public Respuesta<List<pa_getListaIndicadoresSolicitud_Result>> gFiltrarTodosIndicadores(int poIdDireccion, int poIdFrecuencia, string poDescripcion)
        {
            Respuesta<List<pa_getListaIndicadoresSolicitud_Result>> resultado = new Respuesta<List<pa_getListaIndicadoresSolicitud_Result>>();
            resultado.objObjeto = new List<pa_getListaIndicadoresSolicitud_Result>();
            try
            {
                resultado.objObjeto = context.pa_getListaIndicadoresSolicitud(null, poIdDireccion, poIdFrecuencia, true, poDescripcion, 0).ToList();

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return resultado;
        }

        public Respuesta<List<pa_getListaIndicadoresSolicitud_Result>> gFiltrarIndicadoresXOperador(string poIdOperador, int poIdDireccion, int poIdFrecuencia, string poDescripcion)
        {
            Respuesta<List<pa_getListaIndicadoresSolicitud_Result>> resultado = new Respuesta<List<pa_getListaIndicadoresSolicitud_Result>>();
            resultado.objObjeto = new List<pa_getListaIndicadoresSolicitud_Result>();
            try
            {
                resultado.objObjeto = context.pa_getListaIndicadoresSolicitud(poIdOperador, poIdDireccion, poIdFrecuencia, true, poDescripcion, 0).ToList();

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return resultado;
        }

        public Respuesta<List<pa_getListaIndicadoresXSolicitud_Result>> gFiltrarIndicadoresXSolicitud(string poIdOperador, Guid poIndicador)
        {
            Respuesta<List<pa_getListaIndicadoresXSolicitud_Result>> resultado = new Respuesta<List<pa_getListaIndicadoresXSolicitud_Result>>();
            resultado.objObjeto = new List<pa_getListaIndicadoresXSolicitud_Result>();
            try
            {

                var x = context.pa_getListaIndicadoresXSolicitud(poIdOperador, poIndicador);

                if (x != null)
                {
                    resultado.objObjeto = x.ToList();
                }

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return resultado;
        }


        /*metodos Nuevos*/
        /// <summary>
        /// retorna  los operadores relacionados a esa Solicitud
        /// </summary>
        /// Diego Navarrete
        /// <param name="poIdOperador"></param>
        /// <param name="poIndicador"></param>
        /// <returns></returns>
        public Respuesta<List<string>> gFiltrarOperadoresXSolicitud(Guid poIndicador)
        {
            Respuesta<List<string>> resultado = new Respuesta<List<string>>();
            resultado.objObjeto = new List<string>();
            try
            {
                // Guid PRUEBA = Guid.Parse("d7cb0317-bffa-440a-b422-bf3f41ce2628");
                var indicadores = context.SolicitudConstructor.Where(c => c.IdSolicitudIndicador == poIndicador && c.Borrado == 0).Select(c => c.IdOperador).Distinct().ToList();


                if (indicadores != null)
                {
                    resultado.objObjeto = indicadores.Select(c => c.ToString()).ToList();
                }

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return resultado;
        }
        /// <summary>
        /// retorna los indicadoes y el orden relacionados una 
        /// solicitud 
        /// </summary>
        /// <param name="poIndicador"></param>
        /// <returns></returns>
        public Respuesta<List<string>> gFiltrarsoloIndicadoresXSolicitud(Guid poIndicador)
        {
            Respuesta<List<string>> resultado = new Respuesta<List<string>>();
            resultado.objObjeto = new List<string>();
            try
            {
                // Guid PRUEBA = Guid.Parse("d7cb0317-bffa-440a-b422-bf3f41ce2628");

                var indicadores = context.SolicitudConstructor.Where(c => c.IdSolicitudIndicador == poIndicador && c.Borrado == 0).Select(c => new { c.IdConstructor, c.OrdenIndicadorEnExcel }).Distinct().ToList();


                if (indicadores != null)
                {
                    resultado.objObjeto = indicadores.Select(c => c.ToString()).ToList();
                }

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return resultado;
        }


        /// <summary>
        /// Agrega una solicitud de constructor
        /// </summary>
        /// <param name="poSolicitud"></param>
        /// <returns></returns>
        public Respuesta<SolicitudConstructor> gAgregarSolicitudConstructor(SolicitudConstructor poSolicitud)
        {


            Respuesta<SolicitudConstructor> respuesta = new Respuesta<SolicitudConstructor>();
            SolicitudConstructor solicitudConstructorAgregar = new SolicitudConstructor();

            try
            {
                respuesta.objObjeto = poSolicitud;
                using (TransactionScope scope = new TransactionScope())
                {
                    poSolicitud.IdSolicitudContructor = Guid.NewGuid();
                    using (var db = new SUTEL_IndicadoresEntities())
                    {
                        db.SolicitudConstructor.Add(poSolicitud);
                        db.SaveChanges();
                    }
                    respuesta.objObjeto = poSolicitud;
                    scope.Complete();
                }
                //Cambio para cargar si es formulario web
                // Realizamos la consulta
                SolicitudIndicador solicitudAux = context.SolicitudIndicador.Where(c => c.IdSolicitudIndicador == poSolicitud.IdSolicitudIndicador).First();

                var consulta2 = string.Format("delete from ArchivoExcel where IdOperador = '" + respuesta.objObjeto.IdOperador + "' and  IdSolicitudIndicador = '" + respuesta.objObjeto.IdSolicitudIndicador + "'");
                context.Database.CommandTimeout = 0;
                context.Database.ExecuteSqlCommand(consulta2);

                var consulta = string.Format("EXEC dbo.pa_InsertarSolicitudArchivoExcel @IdSolicitudIndicador = {0}, @IdOperador = {1}, @FormularioWeb = {2}", "'" + respuesta.objObjeto.IdSolicitudIndicador + "'", "'" + respuesta.objObjeto.IdOperador + "'", solicitudAux.FormularioWeb);
                context.Database.CommandTimeout = 0;
                context.Database.SqlQuery<DetalleAgrupacionSP>(consulta).ToList();
                respuesta.blnIndicadorTransaccion = true;

            }
            catch (Exception ex)
            {
                respuesta.blnIndicadorTransaccion = false;
                using (var db = new SUTEL_IndicadoresEntities())
                {
                    db.SolicitudConstructor.RemoveRange(db.SolicitudConstructor
                        .Where(x => x.IdSolicitudIndicador == poSolicitud.IdSolicitudIndicador));

                    db.SaveChanges();
                }
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, poSolicitud);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }

        /// <summary>
        /// Elimina logicamente un registro de solicitud constructor
        /// </summary>
        /// <param name="poIdSolicitudConstructor"></param>
        /// <returns></returns>
        public Respuesta<bool> gEliminarSolicitudConstructor(Guid poIdSolicitudIndicador, string poOperador)
        {
            Respuesta<bool> respuesta = new Respuesta<bool>();
            respuesta.blnIndicadorTransaccion = false;
            List<SolicitudConstructor> ListsolicitudAux = null;
            ArchivoExcel ArchivoExcelAux = new ArchivoExcel();
            try
            {

                using (TransactionScope scope = new TransactionScope())
                {

                    // Realizamos la consulta
                    ListsolicitudAux = context.SolicitudConstructor.Where(c => c.IdSolicitudIndicador == poIdSolicitudIndicador && c.IdOperador == poOperador).ToList();

                    ArchivoExcelAux = context.ArchivoExcel.Where(c => c.IdSolicitudIndicador == poIdSolicitudIndicador && c.IdOperador == poOperador).FirstOrDefault();

                    if (ListsolicitudAux != null)
                    {
                        //-----------------------------------------------------
                        for (int j = 0; j < ListsolicitudAux.Count; j++)
                        {

                            ListsolicitudAux[j].Borrado = 1;
                            context.SolicitudConstructor.Attach(ListsolicitudAux[j]);
                            context.Entry(ListsolicitudAux[j]).State = EntityState.Modified;
                            int i = context.SaveChanges();


                        }


                        respuesta.blnIndicadorTransaccion = true;

                        if (ArchivoExcelAux != null)
                        {
                            if (respuesta.blnIndicadorTransaccion == true)
                            {

                                context.ArchivoExcel.Remove(ArchivoExcelAux);
                                context.SaveChanges();
                                respuesta.blnIndicadorTransaccion = true;
                            }

                        }

                    }
                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.blnIndicadorTransaccion = false;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;

        }

        /// <summary>
        /// Elimina logicamente un registro de solicitud constructor
        /// </summary>
        /// <param name="poIdSolicitudConstructor"></param>
        /// <returns></returns>
        public Respuesta<SolicitudConstructor> gEliminarSolicitudConstructor(Guid poIdSolicitudConstructor)
        {
            Respuesta<SolicitudConstructor> respuesta = new Respuesta<SolicitudConstructor>();
            SolicitudConstructor solicitudAux = null;
            try
            {

                using (TransactionScope scope = new TransactionScope())
                {

                    // Realizamos la consulta
                    solicitudAux = context.SolicitudConstructor.Where(c => c.IdConstructor == poIdSolicitudConstructor).First();

                    //
                    solicitudAux.Borrado = 1;


                    context.SolicitudConstructor.Attach(solicitudAux);
                    context.Entry(solicitudAux).State = EntityState.Modified;
                    int i = context.SaveChanges();
                    respuesta.objObjeto = solicitudAux;
                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, solicitudAux);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;

        }

        public Respuesta<SolicitudConstructor> gEliminarSolicitudConstructor(Guid poIdSolicitudIndicador, Guid poIdConstructor, string poOperador)
        {
            Respuesta<SolicitudConstructor> respuesta = new Respuesta<SolicitudConstructor>();
            SolicitudConstructor solicitudAux = null;
            try
            {

                using (TransactionScope scope = new TransactionScope())
                {

                    // Realizamos la consulta
                    solicitudAux = context.SolicitudConstructor.Where(c => c.IdSolicitudIndicador == poIdSolicitudIndicador && c.IdConstructor == poIdConstructor && c.IdOperador == poOperador && c.Borrado == 0).FirstOrDefault();

                    if (solicitudAux != null)
                    {

                        solicitudAux.Borrado = 1;

                        context.SolicitudConstructor.Attach(solicitudAux);
                        context.Entry(solicitudAux).State = EntityState.Modified;
                        int i = context.SaveChanges();
                        respuesta.objObjeto = solicitudAux;

                        var consulta = string.Format("delete from ArchivoExcel where IdOperador = '" + poOperador + "' and  IdSolicitudIndicador = '" + poIdSolicitudIndicador.ToString() + "'");
                        context.Database.CommandTimeout = 0;
                        context.Database.ExecuteSqlCommand(consulta);

                    }

                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, solicitudAux);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;

        }

        /// <summary>
        /// Verifica por medio del IdSolicitudIndicador, IdConstructor y IdOperador si hay un registro activo de este.
        /// </summary>
        /// <param name="poSolicitud"></param>
        /// <returns></returns>
        public Respuesta<SolicitudConstructor> gVerificarExistenciaSolicitudConstructor(SolicitudConstructor poSolicitud)
        {
            Respuesta<SolicitudConstructor> resultado = new Respuesta<SolicitudConstructor>();
            SolicitudConstructor solicitudConstructor = null;
            try
            {

                // Realizamos la consulta
                var rows = context.SolicitudConstructor.Where(c => c.IdSolicitudIndicador == poSolicitud.IdSolicitudIndicador && c.IdConstructor == poSolicitud.IdConstructor
                                                              && c.IdOperador == poSolicitud.IdOperador && c.Borrado == 0 && c.OrdenIndicadorEnExcel == poSolicitud.OrdenIndicadorEnExcel);

                if (rows.Count() > 0)
                {
                    solicitudConstructor = rows.First();
                }


                resultado.objObjeto = solicitudConstructor;
                return resultado;

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, solicitudConstructor);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

        }
        /// <summary>
        /// Actualiza el estado de la solicitud del indicador
        /// </summary>
        /// <param name="poIdSolicitudIndicador"></param>
        /// <param name="idOperador"></param>
        /// <param name="piEstadoSolicitud"></param>
        /// <returns></returns>
        public bool gActualizarEstadoSolicitud(Guid poIdSolicitudIndicador, String idOperador, int piEstadoSolicitud)
        {
            Respuesta<List<SolicitudConstructor>> resultado = new Respuesta<List<SolicitudConstructor>>();
            SolicitudConstructor nSolicitud = new SolicitudConstructor();
            String consulta = "";
            try
            {
                resultado.objObjeto = context.SolicitudConstructor.Where(x => x.IdSolicitudIndicador.Equals(poIdSolicitudIndicador) && x.IdOperador.Equals(idOperador)).ToList();
                if (resultado.objObjeto != null)
                {
                    using (var context1 = new SUTEL_IndicadoresEntities())
                    {
                        foreach (SolicitudConstructor item in resultado.objObjeto)
                        {

                            consulta = "";
                            consulta = string.Format("EXEC dbo.pa_SolicitudConstructorActualizar @ID_SOLICTUDCONSTRUCTOR = {0}, @ID_ESTADO = {1}"
                        , "'" + item.IdSolicitudContructor + "'", piEstadoSolicitud);
                            context1.Database.CommandTimeout = 0;
                            int intResultado = context1.Database.SqlQuery<int>(consulta).FirstOrDefault();
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, resultado.objObjeto);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

        }


        #endregion

        #endregion

        #region Notificacion
        public Respuesta<bool> gNotificar(Guid poIdSolicitud, string poHtml, string poAsunto)
        {
            Respuesta<bool> resultado = new Respuesta<bool>();
            resultado.objObjeto = false;
            try
            {

                var x = context.pa_Envio_Notificaciones(poIdSolicitud, poAsunto, poHtml);



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
        #endregion

        #region sendEmail 
        public Respuesta<bool> gEnviarCorreo(string toemail, string poAsunto, string poHtml, int pdireccion)
        {
            Respuesta<bool> resultado = new Respuesta<bool>();
            resultado.objObjeto = false;
            try
            {

                var x = context.pa_SendEmail(toemail, poHtml, poAsunto, "SCI_Notificaciones", pdireccion);

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
        #endregion

        public string[] obtenerOperadoresAsociados(string ListaSolicitud)
        {

            var operador = new string[0];
            List<string> ListOperadores = new List<string>();


            Guid? foo = new Guid(ListaSolicitud);

            operador = context.SolicitudConstructor.Where(c => c.IdSolicitudIndicador == foo).Select(c => c.IdOperador).Distinct().ToArray();

            foreach (var item in operador)
            {
                ListOperadores.Add(item);
            }


            return operador = ListOperadores.ToArray();
        }

        public string[] obtenerNombreDeOperadores(string[] ListaOperadores)
        {

            List<string> operador = new List<string>();

            string nombre = "";
            string idoperador = "";

            for (int i = 0; i < ListaOperadores.Length; i++)
            {
                idoperador = ListaOperadores[i];

                nombre = context.Operador.Where(c => c.IdOperador == idoperador).Select(c => c.NombreOperador).FirstOrDefault();
                operador.Add(nombre);
            }

            //foreach (var item in operador)
            //{
            //    ListNombres.Add(item);
            //}

            return operador.ToArray();
        }
        public Respuesta<SolicitudConstructor> actualizarSemaforos(Guid IdSolicitudConstructor, string IdOperador, int idSemaforoActualizar, List<ListaIDDto> Valor, string Observacion, Usuario usuario)
        {
            Respuesta<SolicitudConstructor> resultado = new Respuesta<SolicitudConstructor>();
            try
            {
                var registroIndicador = context.TEMPRegistroIndicador.Where(x => x.IdSolicitudConstructor == IdSolicitudConstructor).FirstOrDefault();
                var obentersolicitudIndicador= context.SolicitudConstructor.Where(x => x.IdSolicitudContructor == IdSolicitudConstructor).FirstOrDefault();
                var indentificadorunico= context.SolicitudIndicador.Where(x => x.IdSolicitudIndicador == obentersolicitudIndicador.IdSolicitudIndicador  ).ToList();

                if (registroIndicador != null)
                {
                    if (idSemaforoActualizar == 1)
                    {
                        context.TEMPRegistroIndicador.Remove(registroIndicador);
                        context.Entry(registroIndicador).State = EntityState.Deleted;

                        Respuesta<ConfirmaDescargaWebDto> respuesta = new Respuesta<ConfirmaDescargaWebDto>();
                        respuesta = gConsultarDetalleRegistroIndicador(IdSolicitudConstructor);

                        foreach (var item in respuesta.objObjeto.listaDetalleRegistroIndicador)
                        {
                            var detalleRegistroIndicador = context.TEMPDetalleRegistroIndicador.Where(x => x.IdRegistroIndicador == registroIndicador.IdRegistroIndicador && x.IdConstructorCriterio == item.IdConstructorCriterio && x.NumeroDesglose == item.NumeroDesglose && x.IdProvincia == item.IdProvincia && x.IdCanton == item.IdCanton && x.IdDistrito == item.IdDistrito).FirstOrDefault();
                            context.TEMPDetalleRegistroIndicador.Remove(detalleRegistroIndicador);
                            context.Entry(detalleRegistroIndicador).State = EntityState.Deleted;
                        }
                    }
                    else
                    {
                        registroIndicador.Observacion = Observacion;
                        //Modificar encabezado
                        context.Entry(registroIndicador).State = EntityState.Modified;
                        //Modificar detalle
                    }

                    foreach (var item in Valor)
                    {
                        if (item.zona == "DISTRITO")
                        {
                            item.idProvincia = context.Canton.Where(x => x.IdCanton == item.idCanton).Select(x => x.IdProvincia).FirstOrDefault();

                        }

                        registroIndicador.Observacion = Observacion;
                        var detalleRegistroIndicador = context.TEMPDetalleRegistroIndicador.Where(x => x.IdRegistroIndicador == registroIndicador.IdRegistroIndicador && x.IdConstructorCriterio == item.Id_ConstructorCriterio && x.NumeroDesglose == item.Numero_Desglose && x.IdProvincia == item.idProvincia && x.IdCanton == item.idCanton && x.IdDistrito == item.idDistrito).FirstOrDefault();

                        if (detalleRegistroIndicador != null)
                        {
                            if (idSemaforoActualizar == 5)
                            {//Limpiar

                                context.TEMPDetalleRegistroIndicador.Remove(detalleRegistroIndicador);
                                context.Entry(detalleRegistroIndicador).State = EntityState.Deleted;

                            }
                            else
                            {
                                if (idSemaforoActualizar != 1)
                                {

                                    if (item.Valor != null)
                                    {
                                        detalleRegistroIndicador.Valor = item.Valor;
                                        detalleRegistroIndicador.IdSemaforo = idSemaforoActualizar;
                                    }
                                    else
                                    {
                                        detalleRegistroIndicador.Valor = "";
                                        detalleRegistroIndicador.IdSemaforo = idSemaforoActualizar;
                                        context.Entry(detalleRegistroIndicador).State = EntityState.Modified;
                                    }
                                }
                            }

                        }
                        else
                        {
                            if (idSemaforoActualizar != 1 && idSemaforoActualizar != 5) {

                                if (item.Valor == null)
                                {
                                    item.Valor = "";
                                }
                                TEMPDetalleRegistroIndicador detalleRegistroIndicadorItem = new TEMPDetalleRegistroIndicador()
                                {
                                    IdDetalleRegistroindicador = Guid.NewGuid(),
                                    IdRegistroIndicador = registroIndicador.IdRegistroIndicador,
                                    IdConstructorCriterio = item.Id_ConstructorCriterio, // Pendiente
                                    IdTipoValor = item.Id_Tipo_Valor,
                                    IdProvincia = item.idProvincia != 0 ? item.idProvincia : null,
                                    IdCanton = item.idCanton != 0 ? item.idCanton : null,
                                    IdGenero = null,
                                    Anno = item.Anno,
                                    NumeroDesglose = item.Numero_Desglose,
                                    Valor = item.Valor,
                                    Comentario = "",
                                    Modificado = 0,
                                    FechaModificacion = DateTime.Now,
                                    IdDistrito = item.idDistrito == 0 ? null : item.idDistrito,
                                    IdSemaforo = idSemaforoActualizar

                                };

                                context.TEMPDetalleRegistroIndicador.Add(detalleRegistroIndicadorItem);
                            }
                        }
                    }
                }
                else // cuando es nuevo por completo
                {
                    if (idSemaforoActualizar != 1 && idSemaforoActualizar != 5)
                    {
                                           
                    //Crear encabezado
                    TEMPRegistroIndicador registroIndicadorItem = new TEMPRegistroIndicador()
                    {
                        IdRegistroIndicador = Guid.NewGuid(),
                        IdSolicitudConstructor = IdSolicitudConstructor,
                        IdUsuario = usuario.IdUsuario, // Pendiente
                        FechaRegistroIndicador = DateTime.Now,
                        Comentario = "",
                        Justificado = "",
                        Bloqueado = 1,
                        Borrado = 0,
                        Observacion = Observacion
                    };

                    context.TEMPRegistroIndicador.Add(registroIndicadorItem);
                    registroIndicador = registroIndicadorItem;

                    //Crear detalle
                    foreach (var item in Valor)
                    {
                        if (item.zona == "DISTRITO")
                        {
                            item.idProvincia = context.Canton.Where(x => x.IdCanton == item.idCanton).Select(x => x.IdProvincia).FirstOrDefault();

                        }

                        if (idSemaforoActualizar == 1)
                        {//Limpiar
                            item.Valor = "";
                        }
                        else
                        {
                            if (item.Valor == null)
                                item.Valor = "";
                        }

                        TEMPDetalleRegistroIndicador detalleRegistroIndicadorItem = new TEMPDetalleRegistroIndicador()
                        {
                            IdDetalleRegistroindicador = Guid.NewGuid(),
                            IdRegistroIndicador = registroIndicador.IdRegistroIndicador,
                            IdConstructorCriterio = item.Id_ConstructorCriterio, // Pendiente
                            IdTipoValor = item.Id_Tipo_Valor,
                            IdProvincia = item.idProvincia == 0 ? null : item.idProvincia,
                            IdCanton = item.idCanton == 0 ? null : item.idCanton,
                            IdGenero = null,
                            Anno = item.Anno,
                            NumeroDesglose = item.Numero_Desglose,
                            Valor = item.Valor,
                            Comentario = "",
                            Modificado = 0,
                            FechaModificacion = DateTime.Now,
                            IdDistrito = item.idDistrito == 0 ? null : item.idDistrito,
                            IdSemaforo = idSemaforoActualizar
                        };

                        context.TEMPDetalleRegistroIndicador.Add(detalleRegistroIndicadorItem);
                    }
                  }//fin del fin
                }

                context.SaveChanges();

                using (TransactionScope scope = new TransactionScope())
                {
                    //consulta para actualizar la condicion de la solicitud en el semaforo
                    Respuesta<ConfirmaDescargaWebDto> respuesta = new Respuesta<ConfirmaDescargaWebDto>();             
                    respuesta = gConsultarDetalleRegistroIndicador(IdSolicitudConstructor);
                    var idregistroindicador = "";
                    if (respuesta.objObjeto.listaDetalleRegistroIndicador != null)
                    {
                        if (respuesta.objObjeto.listaDetalleRegistroIndicador.Count != 0)
                        {
                            foreach (var item in respuesta.objObjeto.listaDetalleRegistroIndicador)
                            {
                                //logica para determinar si hay mas de un hijo 
                                if (idregistroindicador != item.IdRegistroIndicador.ToString())
                                {
                                    if (item.IdSemaforo != 3)
                                    {
                                        idSemaforoActualizar = 2;
                                        break;
                                    }
                                    else
                                    {
                                       idSemaforoActualizar = 3;
                                    }

                                }
                                else
                                {
                                    if (item.IdSemaforo != 3)
                                    {
                                        idSemaforoActualizar = 2;
                                        break;
                                    }
                                }
                                idregistroindicador = item.IdRegistroIndicador.ToString();
                            }

                        }

                        else
                        {
                            idSemaforoActualizar = 1;
                        }

                    }
                        // Realizamos la consulta
                        SolicitudConstructor solicitudAux = context.SolicitudConstructor.Where(c => c.IdOperador == IdOperador && c.IdSolicitudContructor == IdSolicitudConstructor).First();
                    if(idSemaforoActualizar != 5)
                    {
                        solicitudAux.IdSemaforo = idSemaforoActualizar;
                    }
                   

                    context.SolicitudConstructor.Attach(solicitudAux);
                    context.Entry(solicitudAux).State = EntityState.Modified;
                    int i = context.SaveChanges();

                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return resultado;
        }
        public Respuesta<SolicitudConstructor> cargarDatos(Guid IdSolicitud, string IdOperador, int poDireccion)
        {
            Respuesta<SolicitudConstructor> resultado = new Respuesta<SolicitudConstructor>();
            try
            {
                List<RegistroIndicador> listaRegistroIndicador = new List<RegistroIndicador>();
                List<DetalleRegistroIndicador> listaDetalleRegistroIndicador = new List<DetalleRegistroIndicador>();

                var listaSolicitudConstructor = context.SolicitudConstructor.Where(x => x.IdSolicitudIndicador == IdSolicitud && x.IdOperador == IdOperador).ToList();

                foreach (var item in listaSolicitudConstructor)
                {
                    // Actualizar SolicitudConstructor
                    item.IdEstado = 4;
                    item.Cargado = true;
                    if (item.CantidadCarga == null)
                        item.CantidadCarga = 1;
                    else
                        item.CantidadCarga++;

                    context.Entry(item).State = EntityState.Modified;

                    var listaTEMPRegistroIndicador = context.TEMPRegistroIndicador.Where(x => x.IdSolicitudConstructor == item.IdSolicitudContructor).ToList();

                    foreach (var item2 in listaTEMPRegistroIndicador)
                    {
                        RegistroIndicador registroIndicador = new RegistroIndicador()
                        {
                            IdRegistroIndicador = item2.IdRegistroIndicador,
                            IdSolicitudConstructor = item2.IdSolicitudConstructor,
                            IdUsuario = item2.IdUsuario,
                            FechaRegistroIndicador = item2.FechaRegistroIndicador,
                            Comentario = item2.Comentario,
                            Justificado = item2.Justificado,
                            Bloqueado = item2.Bloqueado,
                            Borrado = item2.Borrado,
                            Observacion = item2.Observacion
                        };

                        if (!string.IsNullOrEmpty(item2.Observacion))
                        {
                            EnvioNotificaciones(registroIndicador, poDireccion);
                        }

                        listaRegistroIndicador.Add(registroIndicador);

                        var listaTEMPDetalleRegistroIndicador = context.TEMPDetalleRegistroIndicador.Where(x => x.IdRegistroIndicador == item2.IdRegistroIndicador).ToList();

                        foreach (var item3 in listaTEMPDetalleRegistroIndicador)
                        {
                            DetalleRegistroIndicador detalleRegistroIndicador = new DetalleRegistroIndicador()
                            {
                                IdDetalleRegistroindicador = item3.IdDetalleRegistroindicador,
                                IdRegistroIndicador = item3.IdRegistroIndicador,
                                IdConstructorCriterio = item3.IdConstructorCriterio,
                                IdTipoValor = item3.IdTipoValor,
                                IdProvincia = item3.IdProvincia,
                                IdCanton = item3.IdCanton,
                                IdGenero = item3.IdGenero,
                                Anno = item3.Anno,
                                NumeroDesglose = item3.NumeroDesglose,
                                Valor = item3.Valor,
                                Comentario = item3.Comentario,
                                Modificado = item3.Modificado,
                                FechaModificacion = item3.FechaModificacion,
                                IdDistrito = item3.IdDistrito
                            };

                            listaDetalleRegistroIndicador.Add(detalleRegistroIndicador);
                        }
                    }
                }




                context.SaveChanges();

                // Eliminar registros actuales
                if (listaRegistroIndicador.Count > 0)
                {
                    List<RegistroIndicador> listaEliminarRegistroIndicador = new List<RegistroIndicador>();
                    List<DetalleRegistroIndicador> listaEliminarDetalleRegistroIndicador = new List<DetalleRegistroIndicador>();

                    foreach (var item in listaRegistroIndicador)
                    {
                        var registroIndicador = context.RegistroIndicador.Where(x => x.IdRegistroIndicador == item.IdRegistroIndicador).FirstOrDefault();
                        if (registroIndicador != null)
                        {
                            listaEliminarRegistroIndicador.Add(registroIndicador);
                            var detalleRegistroIndicador = context.DetalleRegistroIndicador.Where(x => x.IdRegistroIndicador == item.IdRegistroIndicador).ToList();
                            listaEliminarDetalleRegistroIndicador.AddRange(detalleRegistroIndicador);
                        }
                    }

                    context.DetalleRegistroIndicador.RemoveRange(listaEliminarDetalleRegistroIndicador);
                    context.RegistroIndicador.RemoveRange(listaEliminarRegistroIndicador);
                    context.SaveChanges();
                }

                // Insertar nuevos registros
                context.RegistroIndicador.AddRange(listaRegistroIndicador);
                context.DetalleRegistroIndicador.AddRange(listaDetalleRegistroIndicador);

                context.SaveChanges();




            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return resultado;
        }

        public Respuesta<List<SolicitudConstructorDto>> gConsultarOperadorFormularioWebPorIndicador(Guid IdSolicitudIndicador)
        {
            Respuesta<List<SolicitudConstructorDto>> resultado = new Respuesta<List<SolicitudConstructorDto>>();
            try
            {
                var varSolicitudIndicador = context.SolicitudIndicador.Include(x => x.Servicio).Include(x => x.Direccion).Where(X => X.IdSolicitudIndicador == IdSolicitudIndicador).FirstOrDefault();

                var listaConstructor = (from solicitudConstructor in context.SolicitudConstructor
                                        join archivoExcel in context.ArchivoExcel on true equals archivoExcel.FormularioWeb
                                        join Operador in context.Operador on archivoExcel.IdOperador equals Operador.IdOperador
                                        //join Operador in context.Operador on solicitudConstructor.IdOperador equals Operador.IdOperador
                                        where solicitudConstructor.IdSolicitudIndicador == IdSolicitudIndicador && solicitudConstructor.IdOperador == Operador.IdOperador
                                        orderby solicitudConstructor.OrdenIndicadorEnExcel
                                        select new SolicitudConstructorDto
                                        {
                                            IdOperador = solicitudConstructor.IdOperador,
                                            listaOperador = solicitudConstructor.Operador,
                                            IdSolicitudContructor = solicitudConstructor.IdSolicitudContructor,

                                        }).ToList();


                resultado.objObjeto = listaConstructor;

                // resultado.objObjeto = listaConstructor;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return resultado;
        }


        #region ListarSolicitudesParaOperadores
        /// <summary>
        /// Lista un conjuto de Solicitudes
        /// </summary>
        /// <returns>Listado de Solicitudes</returns>
        public Respuesta<List<SolicitudIndicador>> ListarSolicitudesParaOperadores(Operador objOperador)
        {
            SUTEL_IndicadoresEntities context = new SUTEL_IndicadoresEntities();
            Respuesta<List<SolicitudIndicador>> respuesta = new Respuesta<List<SolicitudIndicador>>();

            try
            {


                //Para que se muestren las solicitudes con fecha fin = a hoy

                var fechaFin = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0);

                respuesta.objObjeto = context.SolicitudIndicador.Where(d => d.FechaInicio <= DateTime.Now && d.FechaFin >= fechaFin && d.Borrado == 0 && d.Activo == 1).SelectMany(x => x.SolicitudConstructor).Where(p => p.IdOperador == objOperador.IdOperador).Select(q => q.SolicitudIndicador).Distinct().ToList();

                List<Guid> filter = new List<Guid>();
                var consulta = string.Format("select  IdSolicitudIndicador from ArchivoExcel where IdOperador = '" + objOperador.IdOperador + "' and ArchivoExcelGenerado = 1 and FormularioWeb = 0 order by FechaHoraSolicitud desc");
                context.Database.CommandTimeout = 0;
                filter = context.Database.SqlQuery<Guid>(consulta).ToList();

                List<SolicitudIndicador> listadoFilter = new List<SolicitudIndicador>();
                SolicitudIndicador newSolicitud;

                foreach (Guid item in filter)
                {
                    newSolicitud = new SolicitudIndicador();
                    newSolicitud = respuesta.objObjeto.Where(x => x.IdSolicitudIndicador == item).FirstOrDefault();
                    if (newSolicitud != null)
                    {
                        listadoFilter.Add(newSolicitud);
                    }
                }

                respuesta.objObjeto = listadoFilter.Distinct().ToList();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, respuesta.objObjeto);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }
        public Respuesta<List<SolicitudIndicador>> ListarSolicitudesParaOperadoresWEB(Operador objOperador)
        {
            SUTEL_IndicadoresEntities context = new SUTEL_IndicadoresEntities();
            Respuesta<List<SolicitudIndicador>> respuesta = new Respuesta<List<SolicitudIndicador>>();

            try
            {

                //Para que se muestren las solicitudes con fecha fin = a hoy

                var fechaFin = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0);

                respuesta.objObjeto = context.SolicitudIndicador.Where(d => d.FechaInicio <= DateTime.Now && d.FechaFin >= fechaFin && d.Borrado == 0 && d.Activo == 1).SelectMany(x => x.SolicitudConstructor).Where(p => p.IdOperador == objOperador.IdOperador).Select(q => q.SolicitudIndicador).Distinct().ToList();

                List<Guid> filter = new List<Guid>();
                var consulta = string.Format("select  IdSolicitudIndicador from ArchivoExcel where IdOperador = '" + objOperador.IdOperador + "'and FormularioWeb = 1 order by FechaHoraSolicitud desc");
                context.Database.CommandTimeout = 0;
                filter = context.Database.SqlQuery<Guid>(consulta).ToList();

                List<SolicitudIndicador> listadoFilter = new List<SolicitudIndicador>();
                SolicitudIndicador newSolicitud;

                foreach (Guid item in filter)
                {
                    newSolicitud = new SolicitudIndicador();
                    newSolicitud = respuesta.objObjeto.Where(x => x.IdSolicitudIndicador == item).FirstOrDefault();
                    if (newSolicitud != null)
                    {
                        listadoFilter.Add(newSolicitud);
                    }
                }

                respuesta.objObjeto = listadoFilter.Distinct().ToList();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, respuesta.objObjeto);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }
        #endregion

        public Respuesta<List<ArchivoExcel>> ListarestadoSolicitudesParaOperadores(Operador objOperador)
        {
            SUTEL_IndicadoresEntities context = new SUTEL_IndicadoresEntities();
            Respuesta<List<ArchivoExcel>> respuesta = new Respuesta<List<ArchivoExcel>>();

            try
            {


                //Para que se muestren las solicitudes con fecha fin = a hoy

                var fechaFin = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0);

                //  respuesta.objObjeto = context.SolicitudIndicador.Where(d => d.FechaInicio <= DateTime.Now && d.FechaFin >= fechaFin && d.Borrado == 0 && d.Activo == 1).SelectMany(x => x.SolicitudConstructor).Where(p => p.IdOperador == objOperador.IdOperador).Select(q => q.SolicitudIndicador).Distinct().ToList();

                List<Guid> filter = new List<Guid>();
                List<ArchivoExcel> listadoFilter = new List<ArchivoExcel>();
                var consulta = string.Format("select IdArchivoExcel, IdSolicitudIndicador, IdOperador, ArchivoExcelGenerado, ArchivoExcelBytes, FechaHoraSolicitud, FechaHoraGeneracionAutomatica, Borrado, Descarga,FormularioWeb from ArchivoExcel where IdOperador = '" + objOperador.IdOperador + "' and ArchivoExcelGenerado = 1 and FormularioWeb= 0 order by FechaHoraSolicitud desc");
                context.Database.CommandTimeout = 0;
                listadoFilter = context.Database.SqlQuery<ArchivoExcel>(consulta).ToList();


                /*  ArchivoExcel newSolicitud;

                  foreach (Guid item in filter)
                  {
                      newSolicitud = new ArchivoExcel();//new SolicitudIndicador();
                     // newSolicitud = respuesta.objObjeto.Where(x => x.IdSolicitudIndicador == item).FirstOrDefault();
                      if (newSolicitud != null)
                      {
                          listadoFilter.Add(newSolicitud);
                      }
                  }*/

                respuesta.objObjeto = listadoFilter.Distinct().ToList();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, respuesta.objObjeto);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }
        public Respuesta<List<SolicitudConstructor>> ListarestadoSolicitudesParaOperadoresWeb(Operador objOperador)
        {
            SUTEL_IndicadoresEntities context = new SUTEL_IndicadoresEntities();
            Respuesta<List<SolicitudConstructor>> respuesta = new Respuesta<List<SolicitudConstructor>>();

            try
            {

                var tempRegistroIndicador = context.TEMPRegistroIndicador.Where(x => x.Borrado == 0).Select(x => x.IdSolicitudConstructor).ToList();

                var SolicitudConstructor = context.SolicitudConstructor.Where(x => x.Borrado == 0 && tempRegistroIndicador.Contains(x.IdSolicitudContructor)).ToList();

                respuesta.objObjeto = SolicitudConstructor.Distinct().ToList();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, respuesta.objObjeto);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        #region modificacionIndicador
        /// <summary>
        /// Solicitudes que se pueden modificar
        /// </summary>
        /// <param name="poIDOperador"></param>
        /// <param name="poIDServicio"></param>
        /// <param name="poIdDireccion"></param>
        /// <param name="poIDIndicador"></param>
        /// <param name="poIdCriterio"></param>
        /// <param name="poIdFrecuencia"></param>
        /// <param name="poIdDesglose"></param>
        /// <param name="poFechaInicial"></param>
        /// <param name="poFechaFinal"></param>
        public Respuesta<List<SolicitudIndicador>> gSolicitudModificarIndicador(string poIDOperador, int poIDServicio, int poIdDireccion, string poIDIndicador
            , string poIdCriterio, int poIdFrecuencia, int poIdDesglose, DateTime poFechaInicial, DateTime poFechaFinal)
        {
            Respuesta<List<SolicitudIndicador>> respuesta = new Respuesta<List<SolicitudIndicador>>();
            String consulta = "";
            String fechaInicial = "'" + String.Format("{0:MM/dd/yyyy}", DateTime.Parse(poFechaInicial.ToString())) + "'";
            String fechaFinal = "'" + String.Format("{0:MM/dd/yyyy}", DateTime.Parse(poFechaFinal.ToString())) + "'";
            try
            {

                consulta = string.Format("EXEC dbo.pa_SolicitudesPorParametros @ID_OPERADOR = {0}, @ID_SERVICIO = {1},  @ID_DIRECCION = {2}, @ID_INDICADOR = {3}, @ID_CRITERIO = {4}, @ID_FRECUENCIA = {5} , @ID_DESGLOSE = {6}, @FECHA_INICIAL = {7} , @FECHA_FINAL = {8}"
                    , "'" + poIDOperador + "'", poIDServicio, poIdDireccion, "'" + poIDIndicador + "'", "'" + poIdCriterio + "'", poIdFrecuencia, poIdDesglose, fechaInicial, fechaFinal);

                using (var context = new SUTEL_IndicadoresEntities())
                {
                    context.Database.CommandTimeout = 0;
                    respuesta.objObjeto = context.Database.SqlQuery<SolicitudIndicador>(consulta).ToList();
                }

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, respuesta.objObjeto);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }

        /// <summary>
        /// Obtiene la solicitud de constructor
        /// </summary>
        /// <param name="poIdOperador"></param>
        /// <param name="poIdSolicitudIndicador"></param>
        /// <param name="poIdDesglose"></param>
        /// <param name="poIdIndicador"></param>
        /// <returns></returns>
        public Respuesta<SolicitudConstructor> gObtenerSolicitudConstructor(String poIdOperador, Guid poIdSolicitudIndicador, int poIdDesglose, string poIdIndicador)
        {
            Respuesta<SolicitudConstructor> respuesta = new Respuesta<SolicitudConstructor>();
            try
            {
                respuesta.objObjeto = context.SolicitudConstructor.Include("Constructor").Where(x => x.IdOperador == poIdOperador
                                                   && x.IdSolicitudIndicador == (poIdSolicitudIndicador)
                                                   && x.Constructor.IdDesglose == (poIdDesglose)
                                                   && x.Constructor.IdIndicador == (poIdIndicador)).FirstOrDefault();
            }

            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, respuesta.objObjeto);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }


        #endregion


        #region gVerificarRelacionOperadorConstructorCriterio
        /// <summary>
        /// Verifica la existencia de datos en la tabla ConstructorCriterioDetalleAgrupacion asociados
        ///al operador que recibe como parámetro
        /// </summary>
        /// <param name="idOperador"> </param>
        /// <param name="idConstructor"></param>
        /// <returns> Retorna"true" cuando existen datos para el operador y"false" cuando no </returns>
        public bool gVerificarRelacionOperadorConstructorCriterio(String IdOperador, String IdConstructor)
        {
            Respuesta<Boolean> resultado = new Respuesta<Boolean>();

            ConstructorCriterioDetalleAgrupacion queryResult = new ConstructorCriterioDetalleAgrupacion();

            Guid idConstructor = Guid.Parse(IdConstructor);
            try
            {
                queryResult = context.ConstructorCriterioDetalleAgrupacion

                         .Where(x => x.IdConstructor.Equals(idConstructor) &&
                             x.IdOperador.Equals(IdOperador) && x.Borrado == 0).FirstOrDefault();

                if (queryResult != null) // Existen datos en la tabla ConstructorCriterioDetalleAgrupacion para ese Operador?


                    return true;

                else

                    return false;

            }
            catch (DbEntityValidationException e)
            {
                return false;
            }


        }

        #endregion

        #region gNombreIndicador
        /// <summary>
        /// Extrae el nombre del indicador del constructor
        /// </summary>
        /// 
        /// <param name="idConstructor"></param>
        /// <returns> Retorna el un objeto respuesta que contiene  </returns>
        public Respuesta<string> gNombreIndicador(String IdConstructor)
        {
            Respuesta<string> resultado = new Respuesta<string>();



            Guid idConstructor = Guid.Parse(IdConstructor);
            try
            {
                var query = from indicador in context.Indicador
                            join constructor in context.Constructor
                            on indicador.IdIndicador equals constructor.IdIndicador
                            where constructor.IdConstructor == idConstructor
                            select new { indicador };

                foreach (var item in query)
                {

                    resultado.objObjeto = item.indicador.NombreIndicador;

                }


                return resultado;



            }
            catch (DbEntityValidationException e)
            {
                return resultado;
            }


        }

        #endregion


        #region gNombreOperador
        /// <summary>
        /// Extrae el nombre del indicador del constructor
        /// </summary>
        /// 
        /// <param name="idConstructor"></param>
        /// <returns> Retorna el un objeto respuesta que contiene  </returns>
        public Respuesta<string> gNombreOperador(String IdOperador)
        {
            Respuesta<string> resultado = new Respuesta<string>();



            try
            {
                var query = from operador in context.Operador
                            where operador.IdOperador == IdOperador
                            select new { operador };

                foreach (var item in query)
                {

                    resultado.objObjeto = item.operador.NombreOperador;

                }


                return resultado;



            }
            catch (DbEntityValidationException e)
            {
                return resultado;
            }


        }

        #endregion

        public void InsertarArchivoExcel(Guid IdSolicitudIndicador, string IdOperador)
        {

            var consulta = string.Format("EXEC dbo.pa_InsertarSolicitudArchivoExcel @IdSolicitudIndicador = {0}, @IdOperador = {1}", "'" + IdSolicitudIndicador + "'", "'" + IdOperador + "'");
            context.Database.CommandTimeout = 0;
            context.Database.SqlQuery<DetalleAgrupacionSP>(consulta).ToList();

        }


        public void EnvioNotificaciones(RegistroIndicador nuevoRegistro, int direccion)
        {
            try
            {
                string htmlCorreo = string.Empty;
                string correosEquipoIndicadores = string.Empty;
                Usuario usuario = getUserById(nuevoRegistro.IdUsuario);
                //string idOperador = usuario.IdOperador.ToString();
                string idOperador = "A0026";

                //////Se obtiene el nombre del operador para adjuntarlo en el correo
                Operador operador = getOperadorById(idOperador);
                //////Se obtiene el nombre del indicador para adjuntarlo en el correo
                string nombreIndicador = context.pa_getNombreIndicadorByIdSolicitudConstructor(nuevoRegistro.IdSolicitudConstructor).First();

                htmlCorreo = getHtmlNotificacionPorRegistrarObservacionEnPlantilla(operador.NombreOperador, nombreIndicador, nuevoRegistro.Observacion);

                correosEquipoIndicadores = getCorreosEquipoIndicadores(direccion);
                //CorreosEquipoIndicadores Correos = getCorreoById(direccion);
                //correosEquipoIndicadores = Correos.CorreosGrupoIndicadores;

                var asunto = ConfigurationManager.AppSettings["AsuntoNotificacionObservacion"].ToString();

                foreach (var correo in correosEquipoIndicadores.Split(';'))
                {
                    context.pa_SendEmail(correo, asunto, htmlCorreo, "CSI", 101);// este es el envio de observaciones
                }
            }
            catch
            {
                throw new Exception(string.Format(GB.SUTEL.Shared.ErrorTemplate.ErrorEnvíoObservaciones, nuevoRegistro.Observacion));
            }
        }


        public Usuario getUserById(int IdUsuario)
        {

            using (SUTEL_IndicadoresEntities cont = new SUTEL_IndicadoresEntities())
            {

                return cont.Usuario.Where(t => t.IdUsuario == IdUsuario).First();

            }


        }

        public Operador getOperadorById(string idOperador)
        {
            using (SUTEL_IndicadoresEntities cont = new SUTEL_IndicadoresEntities())
            {
                return cont.Operador.Where(x => x.IdOperador == idOperador).First();
            }
        }


        public string getHtmlNotificacionPorRegistrarObservacionEnPlantilla(string operador, string descripcionIndicador, string observacion)
        {


            return "<!DOCTYPE html>" +
                          " <html xmlns='http://www.w3.org/1999/xhtml'>" +
                          "<head>" +
                          "<title></title>" +
                          " </head>" +
                          "<body>" +
                          " <img src='' style='max-height: 55px;' />" +
                          " <table style='border-collapse:collapse; background-color:#eeeeee; word-wrap:break-word; width:100%!important;line-height:100%!important '>" +
                          " <tbody>" +
                          "<tr>" +
                       " <td style='font-family:Helvetica,Arial,sans-serif; font-size:12px; color:#000; padding:25px 35px 25px 35px;'>" +
                           " <table style='border-collapse:collapse; width:100%;background-color:#fff;'>" +
                            "    <tr>" +
                              "      <td style='font-family:Helvetica,Arial,sans-serif;font-size:15px;padding:" +
                                "    15px 5px 5px 15px; color:#0072c6;  text-decoration:none; border:0; text-align:left'>" +
                                  "      <b>Sistema de Indicadores de Telecomunicaciones (SITEL)</b>" +
                                  "  </td>" +
                              "  </tr>" +
                                "<tr>" +
                                   " <td style='text-align:left;padding-top:25px;padding-left:15px;'>" +
                                     "   <p>Estimada SUTEL: </p>" +
                                     "<p>Se le informa que el operador <strong> " + operador + "</strong> ha hecho una observación en el indicador <strong>" + descripcionIndicador + "</strong>.</p>" +

                                     "<p> <strong>Observación: </strong>" + observacion + ".</p>" +
                                   " </td>" +
                               " </tr>" +

                               " <tr>" +
                                    "<td style='text-align:left;padding-left:15px;font-size:11px;'>" +
                                    "<hr>" +
                                      "  Por favor, no responder a este correo ya que no es monitoreado." +
                                   " </td>" +
                                "</tr>" +
                                "<tr>" +
                                "    <td style='text-align:left;padding-left:15px;font-size:11px;'></td>" +
                                "</tr>" +
                          "  </table>" +
                       " </td>" +
                  "  </tr>" +
              "  </tbody>" +
           " </table>" +
       " </body>" +
    "</html>";

        }

        public string getCorreosEquipoIndicadores(int direccion)
        {

            try
            {
                string consulta = "SELECT IdCorreosGrupoIndicadores, CorreosGrupoIndicadores,iddireccion FROM  dbo.CorreosEquipoIndicadores where IdCorreosGrupoIndicadores=" + direccion;
                //string consulta = "SELECT IdCorreosGrupoIndicadores, CorreosGrupoIndicadores FROM  dbo.CorreosEquipoIndicadores where Iddireccion=" + direccion; esta es la anterior le faltaba un campo

                var query = context.Database.SqlQuery<CorreosEquipoIndicadores>(consulta);

                return query.SingleOrDefault().CorreosGrupoIndicadores;

            }
            catch (Exception es)
            {

                return string.Empty;

            }

        }


    }

}
