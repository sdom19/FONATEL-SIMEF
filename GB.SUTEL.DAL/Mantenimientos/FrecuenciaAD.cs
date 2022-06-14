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

namespace GB.SUTEL.DAL.Mantenimientos
{
    public class FrecuenciaAD : LocalContextualizer
    {
        #region Atributos
        SUTEL_IndicadoresEntities context;
        #endregion

        #region Constructores

        public FrecuenciaAD(ApplicationContext appContext)
            : base(appContext)
        {
            context = new SUTEL_IndicadoresEntities();
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Inserta en base de datos un registro de Frecuencia
        /// </summary>
        /// <param name="opFrecuencia">Objeto Frecuencia</param>
        /// <returns></returns>
        public Respuesta<Frecuencia> Agregar(Frecuencia opFrecuencia)
        {
            Respuesta<Frecuencia> respuesta = new Respuesta<Frecuencia>();
            Frecuencia frecuenciaAgregar = new Frecuencia();

            try
            {
              opFrecuencia.NombreFrecuencia = opFrecuencia.NombreFrecuencia.Trim();
                frecuenciaAgregar.InjectFrom(opFrecuencia);
                respuesta.objObjeto = opFrecuencia;
                using (TransactionScope scope = new TransactionScope())
                {

                    context.Frecuencia.Add(frecuenciaAgregar);
                    context.SaveChanges();
                    respuesta.objObjeto = frecuenciaAgregar;
                    scope.Complete();
                }


            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, opFrecuencia);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            
            return respuesta;
        }

        #endregion



        /// <summary>
        /// Lista un conjuto de Frecuencias
        /// </summary>
        /// <returns>Listado de Frecuencias</returns>
        public Respuesta<List<Frecuencia>> Listar()
        {
            List<Frecuencia> listadoFrecuencias = new List<Frecuencia>();
            Respuesta<List<Frecuencia>> respuesta = new Respuesta<List<Frecuencia>>();

            try
            {
                listadoFrecuencias = (from frecuencias in context.Frecuencia
                                      where frecuencias.Borrado == 0
                                      orderby frecuencias.NombreFrecuencia ascending
                                      select frecuencias).ToList();


                respuesta.objObjeto = listadoFrecuencias;

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, listadoFrecuencias);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        /// <summary>
        /// Elimina un registro de Frecuencia
        /// </summary>
        /// <param name="poFrecuencia"></param>
        /// <returns></returns>
        public Respuesta<Frecuencia> Eliminar(int poIdFrecuencia)
        {
            Respuesta<Frecuencia> resultado = new Respuesta<Frecuencia>();

            try
            {

                using (TransactionScope scope = new TransactionScope())
                {
                    //validaciones con sus tablas relacionadas
                    var rowsConstructor = context.Constructor.Where(c => c.IdFrecuencia == poIdFrecuencia);
                    var rowsSolicitudIndicador = context.SolicitudIndicador.Where(c => c.IdFrecuencia == poIdFrecuencia);

                    if (rowsConstructor.Count() == 0 && rowsSolicitudIndicador.Count() == 0)
                    {

                        // Realizamos la consulta para obtener el dato exacto a eliminar logicamente
                        Frecuencia frecuenciaAux = context.Frecuencia.Where(c => c.IdFrecuencia == poIdFrecuencia).First();

                        frecuenciaAux.Borrado = 1;

                        context.Frecuencia.Attach(frecuenciaAux);
                        context.Entry(frecuenciaAux).State = EntityState.Modified;
                        context.SaveChanges();

                        resultado.objObjeto = frecuenciaAux;

                    }
                    else
                    {
                        resultado.objObjeto = null;
                        resultado.blnIndicadorState = 300;
                    }
                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, resultado.objObjeto);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return resultado;
        }

        /// <summary>
        /// Modifica un registro de frecuencia
        /// </summary>
        /// <param name="poFrecuencia"></param>
        /// <returns></returns>
        public Respuesta<Frecuencia> Modificar(Frecuencia poFrecuencia)
        {
            Respuesta<Frecuencia> resultado = new Respuesta<Frecuencia>();
            Frecuencia frecuencia = new Frecuencia();
            try
            {
                frecuencia.InjectFrom(poFrecuencia);
                using (TransactionScope scope = new TransactionScope())
                {

                    // Realizamos la consulta
                    Frecuencia frecuenciaAux = context.Frecuencia.Where(c => c.IdFrecuencia == poFrecuencia.IdFrecuencia).First();
                    frecuenciaAux.NombreFrecuencia = poFrecuencia.NombreFrecuencia;
                    frecuenciaAux.CantidadMeses = poFrecuencia.CantidadMeses;
                    context.Frecuencia.Attach(frecuenciaAux);
                    context.Entry(frecuenciaAux).State = EntityState.Modified;
                    int i = context.SaveChanges();                    
                    resultado.objObjeto = frecuenciaAux;
                    scope.Complete();
                }



            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, frecuencia);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

          
            return resultado;
        }

        /// <summary>
        /// Consulta una frecuencia por el nombre
        /// </summary>
        /// <param name="poDescripcion"></param>
        /// <returns></returns>
        public Respuesta<Frecuencia> ConsultarPorNombre(string poDescripcion, int IdFrecuencia)
        {
            Respuesta<Frecuencia> resultado = new Respuesta<Frecuencia>();
            Frecuencia frecuencia = null;
            try
            {
                // Realizamos la consulta
                var rows = context.Frecuencia.Where(c => c.NombreFrecuencia.ToUpper() == poDescripcion.ToUpper() && c.Borrado == 0 && c.IdFrecuencia != IdFrecuencia);

                if (rows.Count() > 0)
                {
                    frecuencia = rows.First();
                }

                resultado.objObjeto = frecuencia;
                return resultado;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, frecuencia);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }


        }

        /// <summary>
        /// Consulta una frecuencia por el exacto
        /// </summary>
        /// <param name="poDescripcion"></param>
        /// <returns></returns>
        public Respuesta<Frecuencia> ConsultarPorNombreExacto(string poDescripcion)
        {
            Respuesta<Frecuencia> resultado = new Respuesta<Frecuencia>();
            Frecuencia frecuencia = null;
            try
            {
                // Realizamos la consulta
                var rows = context.Frecuencia.Where(c => c.NombreFrecuencia.ToUpper().Equals(poDescripcion) && c.Borrado == 0);

                if (rows.Count() > 0)
                {
                    frecuencia = rows.First();
                }

                resultado.objObjeto = frecuencia;
                return resultado;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, frecuencia);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }


        }

        /// <summary>
        /// Consulta una frecuencia por cantidad meses
        /// </summary>
        /// <param name="cantidadMeses"></param>
        /// <returns></returns>
        public Respuesta<Frecuencia> ConsultarPorCantidadMeses(int cantidadMeses, int IdFrecuencia)
        {
            Respuesta<Frecuencia> resultado = new Respuesta<Frecuencia>();
            Frecuencia frecuencia = null;
            try
            {
                // Realizamos la consulta
                var rows = context.Frecuencia.Where(c => c.CantidadMeses == cantidadMeses && c.Borrado == 0 && c.IdFrecuencia != IdFrecuencia);

                if (rows.Count() > 0)
                {
                    frecuencia = rows.First();
                }

                resultado.objObjeto = frecuencia;
                return resultado;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, frecuencia);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }


        }

        public Respuesta<Frecuencia> ConsultarPorId(int poIdFrecuencia)
        {
            Respuesta<Frecuencia> resultado = new Respuesta<Frecuencia>();
            Frecuencia frecuencia = null;
            try
            {
                // Realizamos la consulta
                var rows = context.Frecuencia.Where(c => c.IdFrecuencia == poIdFrecuencia);

                if (rows.Count() > 0)
                {
                    frecuencia = rows.First();
                }

                resultado.objObjeto = frecuencia;
                return resultado;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, frecuencia);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }


        }


        public Respuesta<List<Frecuencia>> ConsultarPorNombreListado(string poDescripcion)
        {
            Respuesta<List<Frecuencia>> resultado = new Respuesta<List<Frecuencia>>();
            List<Frecuencia> frecuencia = null;
            try
            {
                // Realizamos la consulta
                var rows = context.Frecuencia.Where(c => c.NombreFrecuencia.ToUpper().Contains(poDescripcion) && c.Borrado == 0);

                if (rows.Count() > 0)
                {
                    frecuencia = rows.ToList();
                }

                resultado.objObjeto = frecuencia;
                return resultado;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, frecuencia);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }


        }



    }
}
