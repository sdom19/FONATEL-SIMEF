using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using GB.SUTEL.Shared;
using GB.SUTEL.ExceptionHandler;
using GB.SUTEL.DAL.FuenteExternas;

namespace GB.SUTEL.BL.FuenteExternas
{
    public class FuenteExternaBL : LocalContextualizer
    {
        private Respuesta<FuenteExterna> objRespuesta;
        private FuenteExternaDA objFuenteExternaDA;
        public FuenteExternaBL(ApplicationContext appContext)
            : base(appContext)
        {
            objRespuesta = new Respuesta<FuenteExterna>();
            objFuenteExternaDA = new FuenteExternaDA(appContext);
        }

        #region Agregar
        public Respuesta<FuenteExterna> Agregar(FuenteExterna objFuenteExterna)
        {
            try
            {
                if (objFuenteExternaDA.Single(x => x.NombreFuenteExterna == objFuenteExterna.NombreFuenteExterna && x.Borrado == 0).objObjeto == null)
                {
                    objFuenteExterna.IdFuenteExterna = 1;
                    List<int> ides = objFuenteExternaDA.ConsultarTodos(1).objObjeto.Select(x => x.IdFuenteExterna).ToList();
                    foreach (var id in ides)
                    {
                        if (!ides.Contains(id + 1))
                        {
                            objFuenteExterna.IdFuenteExterna = id + 1; break;
                        }
                    }
                    objFuenteExterna.NombreFuenteExterna = objFuenteExterna.NombreFuenteExterna.Trim();
                    objRespuesta = objFuenteExternaDA.Agregar(objFuenteExterna);
                    objRespuesta.strMensaje = GB.SUTEL.Resources.Resources.OperacionExitosa;
                }
                else
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.NombreDuplicado;
                    objRespuesta.blnIndicadorState = 300;
                }
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



    



        #region Editar
        public Respuesta<FuenteExterna[]> Editar(FuenteExterna objFuenteExterna)
        {
            Respuesta<FuenteExterna[]> objRespuesta = new Respuesta<FuenteExterna[]>();
            try
            {
                var FuenteExterna = objFuenteExternaDA.Single(x => x.NombreFuenteExterna == objFuenteExterna.NombreFuenteExterna && x.Borrado == 0).objObjeto;
                if (FuenteExterna == null || (FuenteExterna.NombreFuenteExterna == objFuenteExterna.NombreFuenteExterna
                    && FuenteExterna.IdFuenteExterna == objFuenteExterna.IdFuenteExterna))
                {
                    if (FuenteExterna == null) 
                    {
                        objFuenteExterna.NombreFuenteExterna = objFuenteExterna.NombreFuenteExterna.Trim();
                        objRespuesta = objFuenteExternaDA.Editar(objFuenteExterna);
                    }
                    else
                    {
                        FuenteExterna.NombreFuenteExterna = objFuenteExterna.NombreFuenteExterna.Trim();
                        string va = FuenteExterna.NombreFuenteExterna.ToString();
                        objRespuesta = objFuenteExternaDA.Editar(FuenteExterna);
                    }                    
                    objRespuesta.strMensaje = GB.SUTEL.Resources.Resources.OperacionExitosa;
                }
                else
                {
                    objRespuesta.blnIndicadorTransaccion = false;
                    objRespuesta.strMensaje = GB.SUTEL.Shared.ErrorTemplate.NombreDuplicado;
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
        public Respuesta<List<FuenteExterna>> ConsultarTodos()
        {
            Respuesta<List<FuenteExterna>> objRespuesta = new Respuesta<List<FuenteExterna>>();
            try
            {
                objRespuesta = objFuenteExternaDA.ConsultarTodos();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return objRespuesta;
        }
        public Respuesta<List<FuenteExterna>> ConsultarTodos(RegistroIndicadorExterno RegistroIndicadorExterno)
        {
            Respuesta<List<FuenteExterna>> objRespuesta = new Respuesta<List<FuenteExterna>>();
            try
            {
                objRespuesta = objFuenteExternaDA.ConsultarTodos(RegistroIndicadorExterno);
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

        #region Filtro
        public Respuesta<List<FuenteExterna>> gFiltrarFuenteExterna(int id, string nombre)
        {
            Respuesta<List<FuenteExterna>> objRespuesta = new Respuesta<List<FuenteExterna>>();
            try
            {
                objRespuesta = objFuenteExternaDA.gFiltrarFuenteExterna(id,nombre);
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
        #endregion

        #region ConsultarPorExpresion
        public Respuesta<FuenteExterna> ConsultarPorExpresion(System.Linq.Expressions.Expression<Func<FuenteExterna, bool>> expression)
        {
            try
            {
                objRespuesta = objFuenteExternaDA.Single(expression);
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
        public Respuesta<FuenteExterna> Eliminar(int id)
        {
            Respuesta<FuenteExterna> objRespuesta = new Respuesta<FuenteExterna>();
            try
            {
                objRespuesta = objFuenteExternaDA.Eliminar(id);
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
