using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.DAL;
using Omu.ValueInjecter;
using System.Data.Entity;
using GB.SUTEL.Shared;
using System.Data.SqlClient;
using GB.SUTEL.ExceptionHandler;

namespace GB.SUTEL.DAL.Seguridad
{
    public class RolDA : LocalContextualizer
    {
        private SUTEL_IndicadoresEntities objContext;
        public RolDA(ApplicationContext appContext)
            : base(appContext)
        {
            objContext = new SUTEL_IndicadoresEntities();
        }        

        #region Agregar
        /// <summary>
        /// Método que agrega un Rol a la base de datos
        /// </summary>
        /// <param name="objRol">Objeto tipo Rol con los datos a agregar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Rol> Agregar(Rol objRol)
        {
            Respuesta<Rol> objRespuesta = new Respuesta<Rol>();
            try
            {
                Random random = new Random();
                objRol.IdRol = Convert.ToInt32(random.Next(1, 99999).ToString() + random.Next(1, 999).ToString());

                //Set objeto en respuesta
                objRespuesta.objObjeto = objRol;
                objContext.Rol.Add(objRol);
                objContext.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objRol);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }
        #endregion

        #region Editar
        /// <summary>
        /// Método que edita un Rol a la base de datos
        /// </summary>
        /// <param name="objRol">Objeto tipo Rol con los datos a editar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Rol> Editar(Rol objRol)
        {
            Respuesta<Rol> objRespuesta = new Respuesta<Rol>();
            try
            {
                Rol editarRol = new Rol();
                editarRol = objContext.Rol.Where(x => x.IdRol == objRol.IdRol).FirstOrDefault();

                if (editarRol != null)
                {
                    editarRol.NombreRol = objRol.NombreRol;
                    //Set objeto en respuesta
                    objRespuesta.objObjeto = objRol;
                    objContext.Rol.Attach(editarRol);
                    objContext.Entry(editarRol).State = EntityState.Modified;
                    //Execute en la base de datos                    
                    objContext.SaveChanges();
                }

                
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objRol);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }
        #endregion

        #region Eliminar
        /// <summary>
        /// Método que Elimina un Rol a la base de datos
        /// </summary>
        /// <param name="objRol">Objeto tipo Rol con los datos a Eliminar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Rol> Eliminar(Rol objRol)
        {
            Respuesta<Rol> objRespuesta = new Respuesta<Rol>();
            try
            {
                  Rol eliminarRol = new Rol();
                  eliminarRol = objContext.Rol.Where(x => x.IdRol == objRol.IdRol).FirstOrDefault();

                  if (eliminarRol != null)
                {
                    //Set objeto en respuesta
                    objContext.Rol.Attach(eliminarRol);
                    objContext.Entry(eliminarRol).State = EntityState.Deleted;
                    //Execute en la base de datos                    
                    objContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objRol);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }
        #endregion

        #region ConsultaPorID
        /// <summary>
        /// Método consulta un Rol a la base de datos
        /// </summary>
        /// <param name="objRol">Objeto tipo Rol con los datos para consultar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Rol> ConsultaPorID(Rol objRol)
        {
            Respuesta<Rol> objRespuesta = new Respuesta<Rol>();
            try
            {
                objRespuesta.objObjeto = (
                               from rolEntidad in objContext.Rol
                               where rolEntidad.IdRol == objRol.IdRol
                               select rolEntidad
                               ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objRol);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }
        #endregion

        #region ConsultaPorDependenciaEnUsuario
        /// <summary>
        /// Método consulta un Rol a la base de datos por dependencias
        /// </summary>
        /// <param name="objRol">Objeto tipo Rol con los datos para consultar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Rol> ConsultaPorDependenciaEnUsuario(Rol objRol)
        {
            Respuesta<Rol> objRespuesta = new Respuesta<Rol>();
            try
            {
                objRespuesta.objObjeto = objContext.Usuario.SelectMany(x => x.Rol).Where(q => q.IdRol == objRol.IdRol).FirstOrDefault();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objRol);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }
        #endregion

        #region ConsultaPorDependenciaEnAccionYPantalla
        /// <summary>
        /// Método consulta un Rol a la base de datos por dependencias
        /// </summary>
        /// <param name="objRol">Objeto tipo Rol con los datos para consultar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Rol> ConsultaPorDependenciaEnAccionYPantalla(Rol objRol)
        {
            Respuesta<Rol> objRespuesta = new Respuesta<Rol>();
            try
            {
                objRespuesta.objObjeto = (
                               from rolEntidad in objContext.Rol
                               join rolAccionPantallaEntidad in objContext.RolAccionPantalla on rolEntidad.IdRol equals rolAccionPantallaEntidad.IdRol
                               where rolEntidad.IdRol == objRol.IdRol
                               select rolEntidad
                               ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objRol);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }
        #endregion

        #region ConsultaPorNombreRol
        /// <summary>
        /// Método consulta un Rol a la base de datos
        /// </summary>
        /// <param name="objRol">Objeto tipo Rol con los datos para consultar</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<Rol> ConsultaPorNombreRol(Rol objRol)
        {
            Respuesta<Rol> objRespuesta = new Respuesta<Rol>();
            try
            {
                objRespuesta.objObjeto = (
                                 from rolEntidad in objContext.Rol
                                 where rolEntidad.NombreRol == objRol.NombreRol && rolEntidad.IdRol != objRol.IdRol
                                 select rolEntidad
                                 ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, objRol);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }
        #endregion

        #region ConsultarTodos
        /// <summary>
        /// Método que agrega un Rol a la base de datos
        /// </summary>
        /// <param name="objRol">Objeto tipo Rol</param>
        /// <returns>Objeto de tipo Respuesta</returns>
        public Respuesta<List<Rol>> ConsultarTodos()
        {
            
            Respuesta<List<Rol>> objRespuesta = new Respuesta<List<Rol>>();
            List<Rol> oRoles = new List<Rol>();
            try
            {
                //Execute en la base de datos
                oRoles = (
                        from rolEntidad in objContext.Rol
                        select rolEntidad
                    ).ToList();

                objRespuesta.objObjeto = oRoles;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, oRoles);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }

        /// <summary>
        /// Filtra los roles
        /// </summary>
        /// <param name="piIdRol"></param>
        /// <param name="psNombreRol"></param>
        /// <returns></returns>
        public Respuesta<List<Rol>> gFiltrarRoles(int piIdRol, String psNombreRol)
        {

            Respuesta<List<Rol>> objRespuesta = new Respuesta<List<Rol>>();
            List<Rol> oRoles = new List<Rol>();
            try
            {
                //Execute en la base de datos
                oRoles = objContext.Rol.Where(x=>(piIdRol.Equals(0) || x.IdRol.ToString().Contains(piIdRol.ToString()))
                    &&(psNombreRol.Equals("") || x.NombreRol.ToUpper().Contains(psNombreRol.ToUpper()))
                    ).ToList();

                objRespuesta.objObjeto = oRoles;
             
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, oRoles);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return objRespuesta;
        }
        #endregion
    }
}
