using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using System.Data.Entity;
using GB.SUTEL.Shared;
using System.Data.SqlClient;
using GB.SUTEL.ExceptionHandler;

namespace GB.SUTEL.DAL.Seguridad
{
    public class PantallaDA : LocalContextualizer
    {
        private SUTEL_IndicadoresEntities objContext;
        public PantallaDA(ApplicationContext appContext)
            : base(appContext)
        {
            objContext = new SUTEL_IndicadoresEntities();
        }    

        #region Agregar
        /// <summary>
        /// Método que agrega un Pantalla a la base de datos
        /// </summary>
        /// <param name="objPantalla">Objeto tipo Pantalla con los datos a agregar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Pantalla> Agregar(Pantalla objPantalla)
        {
            Respuesta<Pantalla> objRespuesta = new Respuesta<Pantalla>();
            try
            {
                //Execute en la base de datos
                objContext.Pantalla.Add(objPantalla);
                objContext.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objPantalla);
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }

            return objRespuesta;
        }
        #endregion

        #region Editar
        /// <summary>
        /// Método que edita un Pantalla a la base de datos
        /// </summary>
        /// <param name="objPantalla">Objeto tipo Pantalla con los datos a editar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Pantalla> Editar(Pantalla objPANTALLA)
        {
            Respuesta<Pantalla> objRespuesta = new Respuesta<Pantalla>();
            try
            {
                //Set objeto en respuesta
                objRespuesta.objObjeto = objPANTALLA;
                objContext.Pantalla.Attach(objPANTALLA);
                objContext.Entry(objPANTALLA).State = EntityState.Modified;
                //Execute en la base de datos                    
                objContext.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objPANTALLA);
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }

            return objRespuesta;
        }
        #endregion

        #region ConsultarPorExpresión
        /// <summary>
        /// Método consulta un Pantalla a la base de datos
        /// </summary>
        /// <param name="expression">Expresión Labda para consultar la base de datos</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Pantalla> Single(System.Linq.Expressions.Expression<Func<Pantalla, bool>> expression)
        {
            Respuesta<Pantalla> objRespuesta = new Respuesta<Pantalla>();   
            try
            {
                objRespuesta.objObjeto = objContext.Pantalla.Single(expression);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objRespuesta.objObjeto);
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
            return objRespuesta;
        }
        #endregion      

        #region ConsultarTodosConPadre
        /// <summary>
        /// Método que agrega un Pantalla a la base de datos
        /// </summary>
        /// <param name="objPantalla">Objeto tipo Pantalla</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<List<PANTALLAMENU>> ConsultarTodosConPadre()
        {
            Respuesta<List<PANTALLAMENU>> objRespuesta = new Respuesta<List<PANTALLAMENU>>();
            List<PANTALLAMENU> oPantallas = new List<PANTALLAMENU>();
            try
            {
                var oPantallasHijos = objContext.Pantalla.ToList();

                foreach(var item in oPantallasHijos.GroupBy(x => x.Descripcion)){
                    oPantallas.Add(new PANTALLAMENU()
                    {
                        TITULO = item.Key,
                        PANTALLAS = item.ToList()
                    });
                }
                objRespuesta.objObjeto = oPantallas;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, oPantallas);
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }

            return objRespuesta;
        }
        #endregion       

        #region ConsultarTodos
        /// <summary>
        /// Método que agrega un Pantalla a la base de datos
        /// </summary>
        /// <param name="objPantalla">Objeto tipo Pantalla</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<List<Pantalla>> ConsultarTodos()
        {
            Respuesta<List<Pantalla>> objRespuesta = new Respuesta<List<Pantalla>>();
            List<Pantalla> oPantallaes = new List<Pantalla>();
            try
            {
                //Execute en la base de datos
                oPantallaes = objContext.Pantalla.ToList();

                objRespuesta.objObjeto = oPantallaes;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, oPantallaes);
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }

            return objRespuesta;
        }
        #endregion       

        #region Eliminar
        public Respuesta<Pantalla> Eliminar(Pantalla objPantalla)
        {
            Respuesta<Pantalla> objRespuesta = new Respuesta<Pantalla>() { objObjeto = new Pantalla()};
            try
            {
                objContext.Entry(objPantalla).State = EntityState.Deleted;
                objContext.SaveChanges();        
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objPantalla);
                throw AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
            }
            return objRespuesta;
        }
        #endregion

    }
}
