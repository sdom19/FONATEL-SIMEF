using GB.SUTEL.DAL.Mantenimientos;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SUTEL.Resources;
using GB.SUTEL.Shared;
using GB.SUTEL.ExceptionHandler;


namespace GB.SUTEL.BL.Mantenimientos
{
    public class CriterioBL : LocalContextualizer
    {
        #region atributos
        CriterioAD criterioAD;

        #endregion

        #region Constructor

        public CriterioBL(ApplicationContext appContext)
            : base(appContext)
        {
            criterioAD = new CriterioAD(appContext);
        }

        #endregion

        #region metodos

        /// <summary>
        /// Inserta un criterio en base de datos
        /// </summary>
        /// <param name="poCriterio"></param>
        /// <returns></returns>
        public Respuesta<Criterio> gAgregar(Criterio poCriterio)
        {

            Respuesta<Criterio> respuesta = new Respuesta<Criterio>();

            //se limpian espacios al inicio y final
            object aux = (object)poCriterio;
            Utilidades.LimpiarEspacios(ref aux);


            try
            {
                if (criterioAD.ConsultarPorCodigo(poCriterio.IdCriterio).objObjeto == null)
                {
                    if (criterioAD.ConsultarPorDescripcion(poCriterio.DescCriterio).objObjeto == null)
                    {

                        respuesta = criterioAD.Agregar(poCriterio);
                        if (respuesta.blnIndicadorTransaccion)
                        {
                            respuesta.strMensaje = Mensajes.ExitoInsertar;
                        }
                    }
                    else
                    {
                        respuesta.blnIndicadorTransaccion = false;
                        respuesta.strMensaje = Mensajes.RegistroDescripcionDuplicado;
                    }
                }
                else
                {
                    respuesta.blnIndicadorTransaccion = false;
                    respuesta.strMensaje = Mensajes.RegistroCodigoDuplicado;
                }
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex); respuesta.toError(ex.Message, poCriterio);
            }
            return respuesta;
        }

        /// <summary>
        /// Listado de Niveles
        /// </summary>
        /// <returns></returns>
        public Respuesta<List<Criterio>> gListar()
        {
            Respuesta<List<Criterio>> respuesta = new Respuesta<List<Criterio>>();
            DireccionAD direccionAD = new DireccionAD();
            List<Direccion> Direcciones = null;
            try
            {
                Direcciones = direccionAD.Listar().objObjeto;
                respuesta = criterioAD.Listar();
                foreach (Criterio item in respuesta.objObjeto)
                {
                    item.Direccion = new Direccion();
                    item.Direccion.IdDireccion = item.IdDireccion.HasValue ? 0 : item.IdDireccion.Value;
                    if (item.IdDireccion.HasValue)
                    {
                        if (Direcciones != null && Direcciones.Count() > 0)
                        {
                            item.Direccion.Nombre = Direcciones.Find(x => x.IdDireccion == item.IdDireccion).Nombre;
                                //direccionAD.ConsultarPorId(item.IdDireccion.Value).objObjeto.Nombre;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        /// <summary>
        /// Lista los criterios por dirección
        /// </summary>
        /// <param name="IdConstructor"></param>
        /// <returns></returns>
        public Respuesta<List<ConstructorCriterio>> gobtenerConstructorCriterioporId(string IdConstructor)
        {
            Respuesta<List<ConstructorCriterio>> respuesta = new Respuesta<List<ConstructorCriterio>>();
          
            try
            {
                respuesta = criterioAD.gListarConstructorCriteriosPorId(IdConstructor);

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        /// <summary>
        /// Lista los criterios por dirección
        /// </summary>
        /// <param name="piIdDireccion"></param>
        /// <returns></returns>
        public Respuesta<List<Criterio>> gListarCriteriosPorDireccion(int piIdDireccion, string psIdIndicador)
        {
            Respuesta<List<Criterio>> respuesta = new Respuesta<List<Criterio>>();
            DireccionAD direccionAD = new DireccionAD();
            try
            {
                respuesta = criterioAD.gListarCriteriosPorDireccion(piIdDireccion, psIdIndicador);

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

         /// <summary>
        /// Filtra los criterios tomando en cuenta la dirección
        /// </summary>
        /// <param name="piIdDireccion"></param>
        /// <returns></returns>
        public Respuesta<List<Criterio>> gFiltrarCriterioConDireccion(int piIdDireccion, string psCodigoCriterio, string psNombreCriterio,string psIdIndicador)
        {
            Respuesta<List<Criterio>> respuesta = new Respuesta<List<Criterio>>();
            
            try
            {
                respuesta = criterioAD.gFiltrarCriterioConDireccion(piIdDireccion,psCodigoCriterio,psNombreCriterio,psIdIndicador);

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        
        }

        /// <summary>
        /// Elimina Logicamente un registro de Nivel
        /// </summary>
        /// <param name="Nivel"></param>
        /// <returns></returns>
        public Respuesta<Criterio> gEliminar(string poIdCriterio)
        {
            Respuesta<Criterio> respuesta = new Respuesta<Criterio>();

            try
            {
                if (criterioAD.VerificarUso(poIdCriterio).objObjeto == 0)
                {
                    respuesta = criterioAD.Eliminar(poIdCriterio);
                    if (respuesta.blnIndicadorTransaccion)
                    {
                        respuesta.strMensaje = Mensajes.ExitoEliminar;
                    }

                }
                else
                {
                    respuesta.objObjeto = null;
                    respuesta.strMensaje = Mensajes.RegistroNoSePuedeEliminar;
                    respuesta.blnIndicadorState = 300;
                    respuesta.blnIndicadorTransaccion = false;
                }
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        public Respuesta<Criterio> gModificar(Criterio poCriterio)
        {
            Respuesta<Criterio> respuesta = new Respuesta<Criterio>();

            //se limpian espacios al inicio y final
            object aux = (object)poCriterio;
            Utilidades.LimpiarEspacios(ref aux);

            try
            {
                poCriterio.DescCriterio = poCriterio.DescCriterio.Trim();
                respuesta = criterioAD.Modificar(poCriterio);
                if (respuesta.blnIndicadorTransaccion)
                {
                    respuesta.strMensaje = Mensajes.ExitoEditar;
                }

            }
            catch (Exception ex)
            {

                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }


        public Respuesta<List<Criterio>> gConsutarPorIdDescripcion(string poIdCriterio, string poDescCriterio)
        {
            Respuesta<List<Criterio>> respuesta = new Respuesta<List<Criterio>>();

            try
            {
                if (poDescCriterio == null) { poDescCriterio = string.Empty; }

                respuesta = criterioAD.ConsultarPorIdDescripcion(poIdCriterio.Trim(), poDescCriterio.Trim());
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        /// <summary>
        /// Filtrar criterios
        /// </summary>
        /// <param name="poIdCriterio"></param>
        /// <param name="poDescripcion"></param>
        /// <param name="poDireccion"></param>
        /// <param name="poIndicador"></param>
        /// <returns></returns>
        public Respuesta<List<Criterio>> gFiltrarCriterio(string poIdCriterio, string poDescripcion, string poDireccion, string poIndicador)
        {
            Respuesta<List<Criterio>> respuesta = new Respuesta<List<Criterio>>();

            try
            {
                respuesta = criterioAD.gFiltrarCriterio( poIdCriterio,  poDescripcion, poDireccion, poIndicador);
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }
        #endregion
    }
}
