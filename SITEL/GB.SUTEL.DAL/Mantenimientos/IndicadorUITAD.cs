using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using System.Transactions;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.Shared;
using System.Data.Entity;

namespace GB.SUTEL.DAL.Mantenimientos
{
    public class IndicadorUITAD : LocalContextualizer
    {

        #region atributos

        private SUTEL_IndicadoresEntities context;
        #endregion

        #region metodos

        public IndicadorUITAD(ApplicationContext appContext)
            : base(appContext)
        {
            context = new SUTEL_IndicadoresEntities();
        }

        /// <summary>
        /// Agregar IndicadorUIT
        /// </summary>
        /// <param name="poIndicadorUIT"></param>
        /// <returns></returns>
        public Respuesta<IndicadorUIT> gAgregarIndicadorUIT(IndicadorUIT poIndicadorUIT)
        {
            Respuesta<IndicadorUIT> objRespuesta = new Respuesta<IndicadorUIT>();

            try
            {


               
                context.IndicadorUIT.Add(poIndicadorUIT);
                context.SaveChanges();
                objRespuesta.objObjeto = poIndicadorUIT;
            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(ex.Message, poIndicadorUIT);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }

            return objRespuesta;
        }
        /// <summary>
        /// Edita un IndicadorUIT
        /// </summary>
        /// <param name="poIndicadorUIT"></param>
        /// <returns></returns>
        public Respuesta<IndicadorUIT> gEditarIndicadorUIT(IndicadorUIT poIndicadorUIT)
        {
            Respuesta<IndicadorUIT> objRespuesta = new Respuesta<IndicadorUIT>();
            IndicadorUIT objeto = new IndicadorUIT();
            try
            {
                objeto = context.IndicadorUIT.Where(x => x.IdIndicadorUIT.Equals(poIndicadorUIT.IdIndicadorUIT)).First();
                objeto.DescIndicadorUIT = poIndicadorUIT.DescIndicadorUIT;
                context.IndicadorUIT.Attach(objeto);
                context.Entry(objeto).State = EntityState.Modified;
                context.SaveChanges();
                objRespuesta.objObjeto = poIndicadorUIT;

            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(ex.Message, poIndicadorUIT);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }

            return objRespuesta;
        }
        /// <summary>
        /// Elimina logicamente el IndicadorUIT
        /// </summary>
        /// <param name="poIndicadorUIT"></param>
        /// <returns></returns>
        public Respuesta<IndicadorUIT> gEliminarIndicadorUIT(IndicadorUIT poIndicadorUIT)
        {
            Respuesta<IndicadorUIT> objRespuesta = new Respuesta<IndicadorUIT>();
            IndicadorUIT objeto = new IndicadorUIT();
            try
            {

                objeto = context.IndicadorUIT.Where(x => x.IdIndicadorUIT.Equals(poIndicadorUIT.IdIndicadorUIT)).First();
                objeto.Borrado = 1;
                context.IndicadorUIT.Attach(objeto);
                context.Entry(objeto).State = EntityState.Modified;
                context.SaveChanges();
                objRespuesta.objObjeto = poIndicadorUIT;
                
                

            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(ex.Message, poIndicadorUIT);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }

            return objRespuesta;
        }
        /// <summary>
        /// Obtiene los indicadores
        /// </summary>
        /// <returns></returns>
        public Respuesta<List<IndicadorUIT>> gObtenerIndicadoresUIT()
        {
            List<IndicadorUIT> resultado = new List<IndicadorUIT>();
            Respuesta<List<IndicadorUIT>> objRespuesta = new Respuesta<List<IndicadorUIT>>();

            try
            {
                resultado = (from indUIT in context.IndicadorUIT
                             where indUIT.Borrado == 0
                             orderby indUIT.DescIndicadorUIT
                             select indUIT).ToList();
                
                objRespuesta.objObjeto = resultado;
            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(ex.Message, resultado);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
            return objRespuesta;

        }

        /// <summary>
        /// Obtiene el IndicadorUIT por id
        /// </summary>
        /// <returns></returns>
        public Respuesta<IndicadorUIT> gObtenerIndicadorUIT(int piIdIndicador)
        {
            IndicadorUIT resultado = new IndicadorUIT();
            Respuesta<IndicadorUIT> objRespuesta = new Respuesta<IndicadorUIT>();
            try
            {

                resultado = (from indicadorEntidadUIT in context.IndicadorUIT
                             where indicadorEntidadUIT.Borrado == 0
                             && indicadorEntidadUIT.IdIndicadorUIT == piIdIndicador
                             select indicadorEntidadUIT).FirstOrDefault();

                objRespuesta.objObjeto = resultado;
            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(ex.Message, resultado);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
            return objRespuesta;

        }


        /// <summary>
        /// Obtiene los indicadores por la descripción
        /// </summary>
        /// <returns></returns>
        public Respuesta<IndicadorUIT> gObtenerIndicadorUITNombre(String psNombreIndicador)
        {
            IndicadorUIT resultado = new IndicadorUIT();
            Respuesta<IndicadorUIT> objRespuesta = new Respuesta<IndicadorUIT>();
            try
            {

                resultado = (from indicadorEntidadUIT in context.IndicadorUIT
                             where indicadorEntidadUIT.Borrado == 0
                             && indicadorEntidadUIT.DescIndicadorUIT.ToUpper().Equals(psNombreIndicador.ToUpper())
                             select indicadorEntidadUIT).FirstOrDefault();

                if (resultado == null)
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                }
                objRespuesta.objObjeto = resultado;
            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(ex.Message, resultado);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }
            return objRespuesta;

        }

        /// <summary>
        /// Obtiene los indidores uit filtrados
        /// </summary>
        /// <returns></returns>
        public Respuesta<List<IndicadorUIT>> gObtenerIndicadoresUITPorFiltros(int piCodigo, String nombre)
        {
            List<IndicadorUIT> resultado = new List<IndicadorUIT>();
            Respuesta<List<IndicadorUIT>> objRespuestas = new Respuesta<List<IndicadorUIT>>();
            try
            {

                resultado = (from indicadorUITEntidad in context.IndicadorUIT
                             where indicadorUITEntidad.Borrado == 0 &&
                             (piCodigo == 0 || indicadorUITEntidad.IdIndicadorUIT.ToString().Contains(piCodigo.ToString()))
                             && (nombre.Equals("") || indicadorUITEntidad.DescIndicadorUIT.ToUpper().Contains(nombre.ToUpper()))
                             select indicadorUITEntidad).ToList();

                objRespuestas.objObjeto = resultado;
            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuestas.toError(ex.Message, resultado);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }

            return objRespuestas;

        }


        /// <summary>
        /// Obtiene los indidores uit filtrados por indicador
        /// </summary>
        /// <returns></returns>
        public Respuesta<List<IndicadorUIT>> gObtenerIndicadoresUITPorIndicador(string piCodigo)
        {
            List<IndicadorUIT> resultado = new List<IndicadorUIT>();
            Respuesta<List<IndicadorUIT>> objRespuestas = new Respuesta<List<IndicadorUIT>>();
            Indicador nuevoIndicador = new Indicador();
            try
            {
                nuevoIndicador = context.Indicador.Where(z => z.IdIndicador.Equals(piCodigo)).FirstOrDefault();
                if (nuevoIndicador != null) { 
                resultado = context.IndicadorUIT.Where(x=>x.Indicador.Equals(nuevoIndicador)).ToList();}
                objRespuestas.objObjeto = resultado;
            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuestas.toError(ex.Message, resultado);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);

            }

            return objRespuestas;

        }




        #endregion
    }
}
