using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// GB Classes
using GB.SUTEL.Entities;
using GB.SUTEL.Shared;
using GB.SUTEL.DAL;
using GB.SUTEL.DAL.Seguridad;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.Entities.Utilidades;


namespace GB.SUTEL.BL.Seguridad
{
    public class PantallaBL : LocalContextualizer
    {
        #region Atributos
        /// <summary>
        /// objeto global de Usuario en la capa de acceso a datos
        /// </summary>
        private PantallaDA objPantallaDA;

        public PantallaBL(ApplicationContext appContext)
            : base(appContext)
        {            
            objPantallaDA = new PantallaDA(appContext);            
        }

        #endregion

        #region Agregar
        /// <summary>
        /// Método para agregar un Pantalla
        /// </summary>
        /// <param name="objPantalla">Objeto a agregar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Pantalla> Agregar(Pantalla objPantalla)
        {
            Respuesta<Pantalla> objRespuesta = new Respuesta<Pantalla>();
            try
            {
                objPantalla.IdPantalla = 1;
                List<int> ides = objPantallaDA.ConsultarTodos().objObjeto.Select(x => x.IdPantalla).ToList();
                foreach(var id in ides){
                    if (!ides.Contains(id + 1))
                    {
                        objPantalla.IdPantalla = id+1; break;
                    }
                }
                if (objPantallaDA.Single(x => x.Nombre == objPantalla.Nombre).objObjeto == null)
                {
                    objRespuesta = objPantallaDA.Agregar(objPantalla);
                }
                else
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.strMensaje = "Nombre duplicado";
                }
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objPantalla);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        #endregion

        #region Editar
        /// <summary>
        /// Método para Editar un Pantalla
        /// </summary>
        /// <param name="objPantalla">Objeto a Editar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Pantalla> Editar(Pantalla objPantalla)
        {
            Respuesta<Pantalla> objRespuesta = new Respuesta<Pantalla>();
            try
            {
                if (objPantallaDA.Single(x => x.Nombre == objPantalla.Nombre).objObjeto == null)
                {
                    objRespuesta = objPantallaDA.Editar(objPantalla);
                }
                else
                {
                    objRespuesta.strMensaje = "Nombre duplicado.";
                    objRespuesta.blnIndicadorTransaccion = false;
                }
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objPantalla);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        #endregion

        #region ConsultarTodosConPadre
        /// <summary>
        /// Consultar Todos
        /// </summary>
        /// <param name="objPantalla"></param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<List<PANTALLAMENU>> ConsultarTodosConPadre()
        {
            Respuesta<List<PANTALLAMENU>> objRespuesta = new Respuesta<List<PANTALLAMENU>>();
            try
            {
                objRespuesta = objPantallaDA.ConsultarTodosConPadre();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objRespuesta.objObjeto);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        #endregion

        #region ConsultarTodos
        /// <summary>
        /// Consultar Todos
        /// </summary>
        /// <param name="objPantalla"></param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<List<Pantalla>> ConsultarTodos()
        {
            Respuesta<List<Pantalla>> objRespuesta = new Respuesta<List<Pantalla>>();
            try
            {
                objRespuesta = objPantallaDA.ConsultarTodos();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objRespuesta.objObjeto);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        #endregion

        #region ConsultarPorExpresion
        /// <summary>
        /// Consultar Todos
        /// </summary>
        /// <param name="objPantalla"></param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Pantalla> ConsultarPorExpresion(int id)
        {
            Respuesta<Pantalla> objRespuesta = new Respuesta<Pantalla>();
            try
            {
                objRespuesta = objPantallaDA.Single(x => x.IdPantalla == id);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objRespuesta.objObjeto);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        #endregion

        #region Eliminar
        /// <summary>
        /// Elimina un usuario
        /// </summary>
        /// <param name="objPantalla"></param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Pantalla> Eliminar(Pantalla objPantalla)
        {
            Respuesta<Pantalla> objRespuesta = new Respuesta<Pantalla>();
            try
            {
                objRespuesta = objPantallaDA.Eliminar(objPantalla);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objPantalla);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        #endregion
    }
}
