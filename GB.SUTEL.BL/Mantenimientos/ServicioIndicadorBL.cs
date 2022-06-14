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
    public class ServicioIndicadorBL : LocalContextualizer
    {
        private Respuesta<FuenteExterna> objRespuesta;
        private ServicioIndicadorDA ServicioIndicadorDA;
        public ServicioIndicadorBL(ApplicationContext appContext)
            : base(appContext)
        {
            objRespuesta = new Respuesta<FuenteExterna>();
            ServicioIndicadorDA = new ServicioIndicadorDA(appContext);
        }

        #region Agregar
        public Respuesta<Servicio> AgregarEditar(int idServicio, string[] idsIndicadores)
        {
            try
            {
                Respuesta<Servicio> respuesta = new Respuesta<Servicio>();
                Servicio Servicio = ServicioIndicadorDA.SingleServicio(x => x.IdServicio == idServicio);
                List<Indicador> Indicadores = ServicioIndicadorDA.ListIndicadores(x => idsIndicadores.Contains(x.IdIndicador));
                
                
                Servicio.Indicador.Clear();
                foreach (var item in Indicadores)
                {
                    ServicioIndicador Nuevo = new ServicioIndicador();
                    Servicio.Indicador.Add(item);
                    Nuevo.IdServicio = idServicio;
                    Nuevo.IdIndicador = item.IdIndicador;
                    Servicio.ServicioIndicador.Add(Nuevo);
                }
                ServicioIndicadorDA.ActualizarServicio(Servicio);
                respuesta.objObjeto = Servicio;
                respuesta.strMensaje = "Elementos guardados correctamente.";
                return respuesta;
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
        public List<Servicio> ConsultarTodosServicios()
        {
            try
            {
                return ServicioIndicadorDA.ConsultarTodosServicios();
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
        }
        #endregion

        #region Eliminar
        public Respuesta<Servicio> Eliminar(int IdServicio, string IdIndicador)
        {
            try
            {
                Respuesta<Servicio> respuesta = new Respuesta<Servicio>();
                Servicio Servicio = ServicioIndicadorDA.SingleServicio(x => x.IdServicio == IdServicio);
                Indicador Indicador = Servicio.Indicador.SingleOrDefault(x => x.IdIndicador==IdIndicador);
                ServicioIndicador ServicioIndicador = Servicio.ServicioIndicador.SingleOrDefault(x => x.IdIndicador == IdIndicador);
                Servicio.ServicioIndicador.Remove(ServicioIndicador);
                ServicioIndicadorDA.ActualizarServicio(Servicio);
                respuesta.strMensaje = "Indicador removido del servicio correctamente.";
                respuesta.objObjeto = Servicio;
                return respuesta;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
        }
        public Respuesta<Servicio> EliminarLogicamente(int IdServicio, string IdIndicador)
        {
            try
            {
                Respuesta<Servicio> respuesta = new Respuesta<Servicio>();
                Servicio Servicio = ServicioIndicadorDA.SingleServicio(x => x.IdServicio == IdServicio);
                Indicador Indicador = Servicio.Indicador.SingleOrDefault(x => x.IdIndicador == IdIndicador);
                Servicio.Indicador.Remove(Indicador);
                ServicioIndicadorDA.ActualizarServicio(Servicio);
                respuesta.strMensaje = "Indicador removido de servicio correctamente.";
                respuesta.objObjeto = Servicio;
                return respuesta;
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
        public List<Servicio> Filtrar(string Servicio, string Indicador,string direccion)
        {
            try
            {
                return ServicioIndicadorDA.Filtrar(Servicio,Indicador, direccion);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                objRespuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
        }
        #endregion

        public List<Servicio> ObtenerIndicadoresSeleccionados(int searchid)
        {
            try
            {
                return ServicioIndicadorDA.ObtenerIndicadoresSeleccionados(searchid);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
        }
    }
}
