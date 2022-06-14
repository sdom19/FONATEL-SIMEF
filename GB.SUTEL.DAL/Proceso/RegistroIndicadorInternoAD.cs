using System.Data.SqlClient;
using GB.SUTEL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.Shared;

using GB.SUTEL.Entities.Utilidades;
using System.Data.Entity;
using EntityFramework.BulkInsert.Extensions;
using System.Configuration;


namespace GB.SUTEL.DAL.Proceso
{
    public class RegistroIndicadorInternoAD : LocalContextualizer
    {
        #region atributos

        private SUTEL_IndicadoresEntities context;
        #endregion

        #region metodos

        public RegistroIndicadorInternoAD(ApplicationContext appContext)
            : base(appContext)
        {
            context = new SUTEL_IndicadoresEntities(); 
        }
        public void EnvioNotificaciones(RegistroIndicador nuevoRegistro, int direccion)
        {
            try
            {
                string htmlCorreo = string.Empty;
                string correosEquipoIndicadores = string.Empty;
                Usuario usuario = getUserById(nuevoRegistro.IdUsuario);
                string idOperador = usuario.IdOperador.ToString();

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
        /// <summary>
        /// Registra los indicadores de los operadores
        /// </summary>
        /// <param name="poRegistroIndicador"></param>
        /// <returns></returns>
        public Respuesta<List<RegistroIndicador>> gAgregarRegistroIndicador(List<RegistroIndicador> poRegistroIndicador, int poDireccion)
        {
            Respuesta<List<RegistroIndicador>> objRespuesta = new Respuesta<List<RegistroIndicador>>();
            RegistroIndicador nuevoRegistro = new RegistroIndicador();
            List<DetalleRegistroIndicador> valoresIndicador = new List<DetalleRegistroIndicador>();
            List<DetalleRegistroIndicador> listaDetalles = new List<DetalleRegistroIndicador>();
            DetalleRegistroIndicador nuevoDetalle = new DetalleRegistroIndicador();
            List<Guid> listaRegistrosInsertados = new List<Guid>();
            string htmlCorreo = string.Empty;
            string correosEquipoIndicadores = string.Empty;

            try {

                objRespuesta.objObjeto=new List<RegistroIndicador>();
                foreach (RegistroIndicador itemRegistro in poRegistroIndicador)
                {
                    nuevoRegistro = new RegistroIndicador();
                    valoresIndicador = new List<DetalleRegistroIndicador>(itemRegistro.DetalleRegistroIndicador.ToList());
                    nuevoRegistro.IdRegistroIndicador = Guid.NewGuid();
                    nuevoRegistro.IdSolicitudConstructor = itemRegistro.IdSolicitudConstructor;
                    nuevoRegistro.FechaRegistroIndicador = DateTime.Now;
                    nuevoRegistro.Comentario = "";
                    nuevoRegistro.Justificado = "";
                    nuevoRegistro.Bloqueado = (byte)1;
                    nuevoRegistro.Borrado = (byte)0;
                    nuevoRegistro.IdUsuario = itemRegistro.IdUsuario;
                    nuevoRegistro.Observacion = itemRegistro.Observacion;

                    if (!itemRegistro.Observacion.Equals("Este campo es opcional")){
                        EnvioNotificaciones(nuevoRegistro, poDireccion);
                    }
                       
                    else
                        nuevoRegistro.Observacion = string.Empty; 


                    context.RegistroIndicador.Add(nuevoRegistro);
                    context.SaveChanges();
                    listaRegistrosInsertados.Add(nuevoRegistro.IdRegistroIndicador);
                    listaDetalles = new List<DetalleRegistroIndicador>();
                    foreach (DetalleRegistroIndicador item in valoresIndicador)
                    {
                        nuevoDetalle = new DetalleRegistroIndicador();
                        nuevoDetalle.IdDetalleRegistroindicador = Guid.NewGuid();
                        nuevoDetalle.IdRegistroIndicador = nuevoRegistro.IdRegistroIndicador;
                        nuevoDetalle.IdConstructorCriterio = item.IdConstructorCriterio;
                        nuevoDetalle.IdTipoValor = item.IdTipoValor;
                        if (!item.IdProvincia.Equals(0)) { nuevoDetalle.IdProvincia = item.IdProvincia; }
                        if (!item.IdCanton.Equals(0)) { nuevoDetalle.IdCanton = item.IdCanton; }
                        if (!item.IdGenero.Equals(0)) { nuevoDetalle.IdGenero = item.IdGenero; }
                        if (!item.IdDistrito.Equals(0)) { nuevoDetalle.IdDistrito = item.IdDistrito; }

                        /// validacion para restringir los caracteres especiales la idea es dependiendo el tipo de 
                        /// valor realizo el comvert si revienta no corresponde con el valor necesario
                        nuevoDetalle.Anno = item.Anno;
                        nuevoDetalle.NumeroDesglose = item.NumeroDesglose;
                        nuevoDetalle.Modificado = 0;
                        nuevoDetalle.FechaModificacion = DateTime.Now;

                        if (item.IdTipoValor.Equals(4) && item.Valor != null && item.Valor != "")// si es 4 es monto osea solo numeros 
                        {
                          //  Int64 ValorD = Convert.ToInt64(item.Valor);
 
                        }

                        if (item.IdTipoValor.Equals(5) && item.Valor != null && item.Valor != "")// si es 5 es int 
                        {
                            Int64 ValorD = 0;
                            if (!Int64.TryParse(item.Valor, out ValorD))
                            {
                                throw new Exception(string.Format(GB.SUTEL.Shared.ErrorTemplate.FormatoNumericoEnteroIncorrecto, item.Valor));
                            }
                        }

                        if (item.IdTipoValor.Equals(6) && item.Valor != null && item.Valor != "")// si es 6 es decimales
                        {
                            string str = item.Valor; 
                            double d = double.Parse(str);

                           // double number;
                            //double.TryParse(item.Valor, out number);
                            decimal ValorD = Convert.ToDecimal(d);
                            
                        }
                       
                        nuevoDetalle.Valor = item.Valor;
                        nuevoDetalle.Comentario = item.Comentario;
                        nuevoDetalle.Modificado = 0;
                        listaDetalles.Add(nuevoDetalle);
                    }
                    lAgregarDetalleRegistroIndicador(listaDetalles);
                    objRespuesta.objObjeto.Add(nuevoRegistro);
                }
            
               
            }
            catch (CustomException ex1)
            {
                lEliminarRegistroIndicador(listaRegistrosInsertados);
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                lEliminarRegistroIndicador(listaRegistrosInsertados);
                objRespuesta.toError(ex1.Message, poRegistroIndicador);
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex1);
            }
            catch (Exception ex)
            {
                string msj = string.Format("{0} {1}", GB.SUTEL.Shared.ErrorTemplate.Formatoexcel, ex.Message);
                lEliminarRegistroIndicador(listaRegistrosInsertados);
                objRespuesta.toError(ex.Message, poRegistroIndicador);
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);

            }
            return objRespuesta;
        }
        /// <summary>
        /// Registra los detalles del registro del indicador
        /// </summary>
        /// <param name="poDetallesIndicador"></param>
        private void lAgregarDetalleRegistroIndicador(List<DetalleRegistroIndicador> poDetallesIndicador)
        {
            try
            {
                using (var ctx = new SUTEL_IndicadoresEntities())
                {
                    using (var transactionScope = new TransactionScope())
                    {

                        ctx.BulkInsert(poDetallesIndicador);

                        ctx.SaveChanges();
                        transactionScope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;

                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);

            }
        }
        private void lAgregarDetalleRegistroIndicadorxlote(List<DetalleRegistroIndicador> poDetallesIndicador)
        {
            int contadorRegistros = 0;
            int LoteRegistros = 100;
            try
            {
                using (var ctx = new SUTEL_IndicadoresEntities())
                {
                    //La transacción se le asigno un timeout de 5 minutos
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new System.TimeSpan(0, 10, 0)))
                    {
                        context = new SUTEL_IndicadoresEntities();
                        context.Configuration.AutoDetectChangesEnabled = false;
                        
                        foreach (var detalleIndicador in poDetallesIndicador) //Recorrido de registros
                        {
                            ++contadorRegistros;
                            //Trata de insertar el Lote, válida que el lote sea de 100 registros
                            context = AgregarLote(context, detalleIndicador, contadorRegistros, LoteRegistros, true);
                        }
                        context.SaveChanges(); //Guarda los resgistros restantes
                        //ctx.BulkInsert(poDetallesIndicador);
                        //ctx.SaveChanges();
                        scope.Complete();
                    }
                }

            }
            catch (Exception ex) //Manejo de errores Generales
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
        }
        /// <summary>
        /// Método para insertar Lote de 100 registros de DetalleResgistroIndicador
        /// </summary>
        /// <param name="contextParam">Contexto EF</param>
        /// <param name="detalleIndicador">Item a insertar</param>
        /// <param name="contadorRegistros">contador del lote</param>
        /// <param name="LoteRegistros">Tamaño del lote</param>
        /// <param name="recreateContext">Bandera para crear el context de nuevo</param>
        /// <returns>Contexto EF</returns>
        private SUTEL_IndicadoresEntities AgregarLote(SUTEL_IndicadoresEntities contextParam, DetalleRegistroIndicador detalleIndicador, int contadorRegistros, int LoteRegistros, bool recreateContext)
        {
            contextParam.Set<DetalleRegistroIndicador>().Add(detalleIndicador); //Se carga la entidad
            if (contadorRegistros % LoteRegistros == 0) //Se valida que el lote sea de 100
            {
                contextParam.SaveChanges(); //Realiza inserción
                if (recreateContext) //Se recrea el contexto
                {
                    contextParam.Dispose();
                    contextParam = new SUTEL_IndicadoresEntities();
                    contextParam.Configuration.AutoDetectChangesEnabled = false;
                }
            }
            return contextParam;
        }
        /// <summary>
        /// Modifica el registro de indicador
        /// </summary>
        /// <param name="psIdDetalleRegistro"></param>
        /// <param name="valor"></param>
        public Respuesta<DetalleRegistroIndicador> gModificarRegistroIndicador(Guid psIdDetalleRegistro, string valor)
        {
            Respuesta<DetalleRegistroIndicador> objRespuesta = new Respuesta<DetalleRegistroIndicador>();
            try{
                objRespuesta.objObjeto = context.DetalleRegistroIndicador.Where(x => x.IdDetalleRegistroindicador.Equals(psIdDetalleRegistro)).FirstOrDefault();
                if (objRespuesta.objObjeto != null) {
                    objRespuesta.objObjeto.Modificado = 1;
                    objRespuesta.objObjeto.FechaModificacion = DateTime.Now;
                    objRespuesta.objObjeto.Valor = valor;
                    context.DetalleRegistroIndicador.Attach(objRespuesta.objObjeto);
                    context.Entry(objRespuesta.objObjeto).State = EntityState.Modified;
                    context.SaveChanges();
                }
             }
           
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(ex.Message, objRespuesta.objObjeto);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
            return objRespuesta;
        }

        /// <summary>
        /// Elimina el registro del indicador si ocurre un fallo en la carga de los detalles
        /// 
        /// </summary>
        /// <param name="psRegistroIndicador"></param>
        public void lEliminarRegistroIndicador(List<Guid> psRegistroIndicador) {
            Respuesta<RegistroIndicador> objRespuesta = new Respuesta<RegistroIndicador>();
            List<DetalleRegistroIndicador> detalles = new List<DetalleRegistroIndicador>();
            RegistroIndicador registro=new RegistroIndicador();
            try {
                foreach (Guid item in psRegistroIndicador)
                {
                    registro = context.RegistroIndicador.Include("DetalleRegistroIndicador").Where(x => x.IdRegistroIndicador.Equals(item)).FirstOrDefault();

                    if (registro.FechaRegistroIndicador!=null) {

                        context.DetalleRegistroIndicador.RemoveRange(registro.DetalleRegistroIndicador);
                        context.RegistroIndicador.Remove(registro);
                        context.SaveChanges();
                    }

                }
            }
           
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(ex.Message, registro);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
        }

        public List<RegistroIndicador> obtenerRegistroIndicador(string idSolicitudConstructor)
        {
            Guid? idSolicitudConstructor_Guid = new Guid(idSolicitudConstructor);

            List<RegistroIndicador> registroIndicador = context.RegistroIndicador.Where(d => d.IdSolicitudConstructor == idSolicitudConstructor_Guid).ToList();

            return registroIndicador;
        }

        public List<SolicitudConstructor> obtenerSolicitudConstructor(string idOperador, Guid idSolicitudIndicador)
        {
            List<RegistroIndicador> registroIndicador = new List<RegistroIndicador>();

            var solicitudConstructor = context.SolicitudConstructor.Where(d => d.IdOperador==idOperador && d.IdSolicitudIndicador == idSolicitudIndicador).ToList();

            return solicitudConstructor;
        }

        public void eliminarRegistroIndicadorExistente(string idOperador, Guid idSolicitudIndicador)
        {
            Respuesta<RegistroIndicador> objRespuesta = new Respuesta<RegistroIndicador>();
            List<DetalleRegistroIndicador> detalles = new List<DetalleRegistroIndicador>();

            try
            {
                var solicitudConsutructor = obtenerSolicitudConstructor(idOperador, idSolicitudIndicador);

                foreach (var item in solicitudConsutructor.ToList()) {

                    var RegistroIndicador = item.RegistroIndicador;

                    foreach (var item2 in RegistroIndicador.ToList())
                    {

                        RegistroIndicador registro = new RegistroIndicador();
                        registro = context.RegistroIndicador.Include("DetalleRegistroIndicador").Where(x => x.IdRegistroIndicador.Equals(item2.IdRegistroIndicador)).FirstOrDefault();

                        if (registro.FechaRegistroIndicador != null)
                        {
                            context.DetalleRegistroIndicador.RemoveRange(registro.DetalleRegistroIndicador);
                            context.RegistroIndicador.Remove(registro);
                           
                        }
                    }
                }
                context.SaveChanges();
            }

            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                //objRespuesta.toError(ex.Message, registro);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
        }


        #endregion  





        public Respuesta<List<pa_ListadoModificacionMasiva_Result>> gListadoModificacionMasiva(string annos,string operador,string servicio, int estado, string indicador)
        {
            Respuesta<List<pa_ListadoModificacionMasiva_Result>> respuesta = new Respuesta<List<pa_ListadoModificacionMasiva_Result>>();
            try
            {
                respuesta.objObjeto = context.pa_ListadoModificacionMasiva(servicio,operador,estado,indicador ,annos).ToList();
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;

                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);

            }

            return respuesta;
        }







        #region modificacionIndicador
        /// <summary>
        /// Obtiene el valor registrado
        /// </summary>
        /// <param name="poIdSolicitudConstructor"></param>
        /// <param name="poIdMes"></param>
        /// <param name="poIdNivelDetalle"></param>
        /// <param name="poValorNiveldDetalle"></param>
        /// <returns></returns>
        /// 

        public Respuesta<DetalleRegistroIndicador> gObtenerDetalleRegistroIndicador(Guid pIdDetalleRegistroIndicador)
        {
            Respuesta<DetalleRegistroIndicador> respuesta = new Respuesta<DetalleRegistroIndicador>();
            try
            {
                respuesta.objObjeto = context.DetalleRegistroIndicador.Where(x => x.IdDetalleRegistroindicador == pIdDetalleRegistroIndicador).FirstOrDefault();
            }

            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;

                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);

            }

            return respuesta;
        }


        public Respuesta<DetalleRegistroIndicador> gObtenerDetalleRegistroIndicador(Guid poIdSolicitudConstructor,Guid poDetalleAgrupacion, int poIdMes, int poIdNivelDetalle, int poValorNiveldDetalle, int poIdAnno)
        {
            Respuesta<DetalleRegistroIndicador> respuesta = new Respuesta<DetalleRegistroIndicador>();
            try {
                respuesta.objObjeto = context.DetalleRegistroIndicador.Where(x => x.RegistroIndicador.IdSolicitudConstructor.Equals(poIdSolicitudConstructor)
                    && x.NumeroDesglose == poIdMes
                    && x.Anno==poIdAnno
                    &&(x.IdConstructorCriterio.Equals(poDetalleAgrupacion))
                    &&(poIdNivelDetalle==0 
                    || (poIdNivelDetalle==1 && x.IdProvincia==poValorNiveldDetalle) 
                    ||(poIdNivelDetalle==2 && x.IdCanton==poValorNiveldDetalle)
                    ||(poIdNivelDetalle==3 && x.IdGenero==poValorNiveldDetalle))).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;

                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);

            }

            return respuesta;
        }
        /// <summary>
        /// Obtiene el detalle por el id
        /// </summary>
        /// <param name="poIdSolicitudConstructor"></param>
        /// <param name="poIdMes"></param>
        /// <param name="poIdNivelDetalle"></param>
        /// <param name="poValorNiveldDetalle"></param>
        /// <returns></returns>
        public Respuesta<DetalleRegistroIndicador> gObtenerDetalleRegistroIndicadorPorID(Guid poIdDetalleRegistroIndicador)
        {
            Respuesta<DetalleRegistroIndicador> respuesta = new Respuesta<DetalleRegistroIndicador>();
            try
            {
                respuesta.objObjeto = context.DetalleRegistroIndicador.Include("ConstructorCriterioDetalleAgrupacion").Where(x => x.IdDetalleRegistroindicador.Equals(poIdDetalleRegistroIndicador)
                   ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;

                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);

            }

            return respuesta;
        }
        #endregion


        public string getHtmlNotificacionPorRegistrarObservacionEnPlantilla(string operador, string descripcionIndicador, string observacion) {


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
        public CorreosEquipoIndicadores getCorreoById(int IdDireccion)
        {

            using (SUTEL_IndicadoresEntities cont = new SUTEL_IndicadoresEntities())
            {

                return cont.CorreosEquipoIndicadores.Where(t => t.Iddireccion == IdDireccion).First();

            }


        }

        public Usuario getUserById(int IdUsuario) {

            using (SUTEL_IndicadoresEntities cont = new SUTEL_IndicadoresEntities()) {

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

        public Respuesta<List<CMes>> lDetalleFechasSolicitudIndicador(Guid poIDIndicador)
        {
            Respuesta<List<CMes>> respuesta = new Respuesta<List<CMes>>();
            try
            {

                string consulta = string.Format(@"select distinct  a.Anno anno,cast(NumeroDesglose as varchar) +' | '+cast(a.Anno as varchar) idMes, 
                                                case NumeroDesglose 
                                                when 1  then 'Enero'
                                                when 2  then 'Febrero'
                                                when 3  then 'Marzo'
                                                when 4  then 'Abril'
                                                when 5  then  'Mayo'
                                                when 6  then 'Junio'
                                                when 7  then 'Julio'
                                                when 8  then 'Agosto'
                                                when 9  then 'Setiembre'
                                                when 10  then 'Octubre'
                                                when 11  then 'Noviembre'
                                                ELSE 'Diciembre' END
                                                nombreMes from DetalleRegistroIndicador a inner join 
                                                RegistroIndicador b 
                                                on a.IdRegistroIndicador=b.IdRegistroIndicador inner join 
                                                SolicitudConstructor c on
                                                b.IdSolicitudConstructor=c.IdSolicitudContructor 
                                               where IdSolicitudIndicador='{0}'", poIDIndicador);

                using (var context = new SUTEL_IndicadoresEntities())
                {
                    context.Database.CommandTimeout = 0;
                    respuesta.objObjeto = context.Database.SqlQuery<CMes>(consulta).ToList();
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

        
    }
}
