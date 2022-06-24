using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GB.SUTEL.DAL.Mantenimientos;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.Shared;
using GB.SUTEL.ExceptionHandler;

namespace GB.SUTEL.BL.Mantenimientos
{
    public class IndicadorCruzadoBL : LocalContextualizer
    {
        private Respuesta<IndicadorCruzado> objRespuesta;
        private DAL.Mantenimientos.IndicadorCruzadoDA objIndicadorCruzadoDA;
        public IndicadorCruzadoBL(ApplicationContext appContext)
            : base(appContext)
        {
            objRespuesta = new Respuesta<IndicadorCruzado>();
            objIndicadorCruzadoDA = new IndicadorCruzadoDA(appContext);
        }

        #region Agregar
        public Respuesta<IndicadorCruzado> Agregar(IndicadorCruzado objIndicadorCruzado, string[] myIndicadorInternoList,
            string[] IndicadorInternoList, string[] IndicadorExternoList)
        {
            try
            {
                var IndicadorCruzado = objIndicadorCruzadoDA.ConsultarTodos().objObjeto;
                if (IndicadorCruzado.Where(x => x.IdIndicadorCruzado == objIndicadorCruzado.IdIndicadorCruzado).Count() == 0)
                {
                    if (IndicadorCruzado.Where(x => x.Nombre.Equals(objIndicadorCruzado.Nombre)).Count() == 0)
                    {
                        List<DetalleIndicadorCruzado> DetalleIndicadorCruzadoList = new List<DetalleIndicadorCruzado>();
                        DetalleIndicadorCruzado DetalleIndicadorCruzado;
                        if (IndicadorInternoList != null)
                        {
                            foreach (var itemIndicadorCruzado in myIndicadorInternoList)
                            {
                                foreach (var itemIndicadorInterno in IndicadorInternoList)
                                {
                                    DetalleIndicadorCruzado = new DetalleIndicadorCruzado();
                                    DetalleIndicadorCruzado.IdDetalleIndicadorCruzado = Guid.NewGuid();
                                    DetalleIndicadorCruzado.IdIndicadorCruzado = objIndicadorCruzado.IdIndicadorCruzado;
                                    DetalleIndicadorCruzado.IdIndicador = itemIndicadorCruzado;
                                    DetalleIndicadorCruzado.IdIndicadorInterno = itemIndicadorInterno;
                                    DetalleIndicadorCruzadoList.Add(DetalleIndicadorCruzado);
                                }
                            }
                        }
                        else if (IndicadorExternoList != null)
                        {
                            foreach (var itemIndicadorCruzado in myIndicadorInternoList)
                            {
                                foreach (var itemIndicadorExterno in IndicadorExternoList)
                                {
                                    DetalleIndicadorCruzado = new DetalleIndicadorCruzado();
                                    DetalleIndicadorCruzado.IdDetalleIndicadorCruzado = Guid.NewGuid();
                                    DetalleIndicadorCruzado.IdIndicadorCruzado = objIndicadorCruzado.IdIndicadorCruzado;
                                    DetalleIndicadorCruzado.IdIndicador = itemIndicadorCruzado;
                                    DetalleIndicadorCruzado.IdIndicadorExterno = itemIndicadorExterno.ToString();
                                    DetalleIndicadorCruzadoList.Add(DetalleIndicadorCruzado);
                                }
                            }
                        }
                        //else
                        //{
                        //    objRespuesta.blnIndicadorTransaccion = false;
                        //    objRespuesta.strMensaje = "No ha seleccionado valores para asociar.";
                        //    objRespuesta.blnIndicadorState = 301;
                        //}
                        objIndicadorCruzado.Descripcion = objIndicadorCruzado.Descripcion.Trim();
                        objIndicadorCruzado.Nombre = objIndicadorCruzado.Nombre.Trim();
                        objRespuesta = objIndicadorCruzadoDA.Agregar(objIndicadorCruzado);
                        objIndicadorCruzadoDA.AgregarDetalles(DetalleIndicadorCruzadoList);
                        objRespuesta.strMensaje = GB.SUTEL.Resources.Resources.OperacionExitosa;
                    }
                    else
                    {
                        objRespuesta.blnIndicadorTransaccion = false;
                        objRespuesta.strMensaje = String.Format(GB.SUTEL.Shared.ErrorTemplate.DuplicatedField, "nombre");
                        objRespuesta.blnIndicadorState = 300;
                    }
                }
                else
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.strMensaje = String.Format(GB.SUTEL.Shared.ErrorTemplate.DuplicatedField, "código");
                    objRespuesta.blnIndicadorState = 300;
                }
                return objRespuesta;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
        }
        #endregion

        #region Editar
        public Respuesta<IndicadorCruzado[]> Editar(IndicadorCruzado objIndicadorCruzado, string currentId, string[] myIndicadorInternoList,
            string[] IndicadorInternoList, string[] IndicadorExternoList)
        {
            Respuesta<IndicadorCruzado[]> objRespuesta = new Respuesta<IndicadorCruzado[]>();
            try
            {
                objIndicadorCruzado.IdIndicadorCruzado = currentId;
                var IndicadorCruzadoList = objIndicadorCruzadoDA.ConsultarTodos().objObjeto;

                var busquedaID = IndicadorCruzadoList.SingleOrDefault(x => x.IdIndicadorCruzado == objIndicadorCruzado.IdIndicadorCruzado);
                var busquedaNombreDescripcion = IndicadorCruzadoList.SingleOrDefault(x => x.Nombre == objIndicadorCruzado.Nombre);
                if (busquedaID == null || (busquedaID.IdIndicadorCruzado == currentId))
                {
                    if (busquedaNombreDescripcion == null || (busquedaNombreDescripcion.IdIndicadorCruzado == currentId))
                    {
                        var IndicadorCruzado = IndicadorCruzadoList.SingleOrDefault(x => x.IdIndicadorCruzado==currentId);
                        IndicadorCruzado.IdIndicadorCruzado = objIndicadorCruzado.IdIndicadorCruzado;
                        IndicadorCruzado.Nombre = objIndicadorCruzado.Nombre.Trim();
                        IndicadorCruzado.Descripcion = objIndicadorCruzado.Descripcion.Trim();
                        objRespuesta = objIndicadorCruzadoDA.Editar(IndicadorCruzado, currentId);

                        List<DetalleIndicadorCruzado> DetalleIndicadorCruzadoList = new List<DetalleIndicadorCruzado>();
                        DetalleIndicadorCruzado DetalleIndicadorCruzado;
                        if (IndicadorInternoList != null)
                        {
                            foreach (var itemIndicadorCruzado in myIndicadorInternoList)
                            {
                                foreach (var itemIndicadorInterno in IndicadorInternoList)
                                {

                                    DetalleIndicadorCruzado = new DetalleIndicadorCruzado();
                                    DetalleIndicadorCruzado.IdDetalleIndicadorCruzado = Guid.NewGuid();
                                    DetalleIndicadorCruzado.IdIndicadorCruzado = objIndicadorCruzado.IdIndicadorCruzado;
                                    DetalleIndicadorCruzado.IdIndicador = itemIndicadorCruzado;
                                    DetalleIndicadorCruzado.IdIndicadorInterno = itemIndicadorInterno;
                                    DetalleIndicadorCruzadoList.Add(DetalleIndicadorCruzado);
                                }
                            }
                        }
                        else if (IndicadorExternoList != null)
                        {
                            foreach (var itemIndicadorCruzado in myIndicadorInternoList)
                            {
                                foreach (var itemIndicadorExterno in IndicadorExternoList)
                                {
                                    DetalleIndicadorCruzado = new DetalleIndicadorCruzado();
                                    DetalleIndicadorCruzado.IdDetalleIndicadorCruzado = Guid.NewGuid();
                                    DetalleIndicadorCruzado.IdIndicadorCruzado = objIndicadorCruzado.IdIndicadorCruzado;
                                    DetalleIndicadorCruzado.IdIndicador = itemIndicadorCruzado;
                                    DetalleIndicadorCruzado.IdIndicadorExterno =  itemIndicadorExterno.ToString();
                                    DetalleIndicadorCruzadoList.Add(DetalleIndicadorCruzado);
                                }
                            }
                        }
                        objIndicadorCruzadoDA.AgregarDetalles(DetalleIndicadorCruzadoList);
                        objRespuesta.strMensaje = GB.SUTEL.Resources.Resources.OperacionExitosa;
                    }
                    else
                    {
                        objRespuesta.blnIndicadorTransaccion = false;
                        objRespuesta.strMensaje = String.Format(GB.SUTEL.Shared.ErrorTemplate.DuplicatedField, "nombre");
                        objRespuesta.blnIndicadorState = 300;
                    }
                }
                else
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.strMensaje = String.Format(GB.SUTEL.Shared.ErrorTemplate.DuplicatedField, "código");
                    objRespuesta.blnIndicadorState = 300;
                }
                return objRespuesta;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
        }
        #endregion

        #region ConsultarTodos
        public Respuesta<List<IndicadorCruzado>> ConsultarTodos()
        {
            Respuesta<List<IndicadorCruzado>> objRespuesta = new Respuesta<List<IndicadorCruzado>>();
            try
            {
                objRespuesta = objIndicadorCruzadoDA.ConsultarTodos();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        public Respuesta<List<Direccion>> ConsultarTodosDirecciones()
        {
            Respuesta<List<Direccion>> objRespuesta = new Respuesta<List<Direccion>>();
            try
            {
                objRespuesta = objIndicadorCruzadoDA.ConsultarTodosDirecciones();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        public List<Indicador> ConsultarTodosIndicadores()
        {
            try
            {
                return objIndicadorCruzadoDA.ConsultarTodosIndicadores();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
        }
        #endregion

        #region Filtro
        public Respuesta<List<IndicadorCruzado>> gFiltrarIndicadorCruzado(string nombre, string descripcion)
        {
            Respuesta<List<IndicadorCruzado>> objRespuesta = new Respuesta<List<IndicadorCruzado>>();
            try
            {
                objRespuesta = objIndicadorCruzadoDA.gFiltrarIndicadorCruzado(nombre, descripcion);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        public List<Indicador> gFiltrarIndicadorInterno(string nombre, int direccion)
        {
            try
            {
                return objIndicadorCruzadoDA.gFiltrarIndicadorInterno(nombre, direccion);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
        }
        #endregion

        #region ConsultarPorExpresion
        public Respuesta<IndicadorCruzado> ConsultarPorExpresion(System.Linq.Expressions.Expression<Func<IndicadorCruzado, bool>> expression)
        {
            try
            {
                objRespuesta = objIndicadorCruzadoDA.Single(expression);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        #endregion

        #region Eliminar
        public Respuesta<IndicadorCruzado> Eliminar(string id)
        {
            Respuesta<IndicadorCruzado> objRespuesta = new Respuesta<IndicadorCruzado>();
            try
            {
                objRespuesta = objIndicadorCruzadoDA.Eliminar(id);
                objRespuesta.strMensaje = GB.SUTEL.Resources.Resources.OperacionExitosa;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        #endregion
    }
}
