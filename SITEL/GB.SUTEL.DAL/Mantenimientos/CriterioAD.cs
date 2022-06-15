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
    public class CriterioAD : LocalContextualizer
    {
        #region Atributos
        SUTEL_IndicadoresEntities context;
        #endregion

        #region Constructores

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appContext"></param>
        public CriterioAD(ApplicationContext appContext)
            : base(appContext)
        {
            context = new SUTEL_IndicadoresEntities();
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Inserta en base de datos un registro de Criterio
        /// </summary>
        /// <param name="opNivel">Objeto Criterio</param>
        /// <returns></returns>
        public Respuesta<Criterio> Agregar(Criterio opCriterio)
        {
            Respuesta<Criterio> respuesta = new Respuesta<Criterio>();
            // Criterio criterioAgregar = new Criterio();

            try
            {
                // criterioAgregar.InjectFrom(opCriterio);
                respuesta.objObjeto = opCriterio;

                using (TransactionScope scope = new TransactionScope())
                {
                    context.Criterio.Add(opCriterio);
                    context.SaveChanges();

                    scope.Complete();
                }


            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, opCriterio);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }

        /// <summary>
        /// Lista un conjuto de Criterios
        /// </summary>
        /// <returns>Listado de Criterios</returns>
        public Respuesta<List<Criterio>> Listar()
        {
            List<Criterio> listaCriterios = new List<Criterio>();
            Respuesta<List<Criterio>> respuesta = new Respuesta<List<Criterio>>();

            try
            {

                listaCriterios = (from criterios in context.Criterio
                                  where criterios.Borrado == 0
                                  select criterios).Take(1000).ToList();


                respuesta.objObjeto = listaCriterios;

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, listaCriterios);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        /// <summary>
        /// Lista los Constructurescriterios por dirección
        /// </summary>
        /// <param name="piIdDireccion"></param>
        /// <returns>la lista de Constructores Criterios</returns>
        public Respuesta<List<ConstructorCriterio>> gListarConstructorCriteriosPorId( string IdConstructor)
        {
            List<ConstructorCriterio> listaCriterios = new List<ConstructorCriterio>();
            Respuesta<List<ConstructorCriterio>> respuesta = new Respuesta<List<ConstructorCriterio>>();
            List<ConstructorCriterio> listaCCriteriosFinal = new List<ConstructorCriterio>();
            ConstructorCriterio Ccriterio = new ConstructorCriterio();
            Guid id = new Guid(IdConstructor);
            try
            {

                listaCriterios = (from x in context.ConstructorCriterio where x.IdConstructor == id  select x).Take(100).ToList();
                if (listaCriterios != null)
                {
                    foreach (ConstructorCriterio item in listaCriterios)
                    {
                        Ccriterio = new ConstructorCriterio();
                        Ccriterio.IdCriterio = item.IdCriterio;
                        Ccriterio.Ayuda = item.Ayuda;
                        Ccriterio.IdConstructor = item.IdConstructor;
                        listaCCriteriosFinal.Add(Ccriterio);


                    }
                }
                respuesta.objObjeto = listaCCriteriosFinal.ToList();

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, listaCriterios);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        /// <summary>
        /// Lista los criterios por dirección
        /// </summary>
        /// <param name="piIdDireccion"></param>
        /// <returns></returns>
        public Respuesta<List<Criterio>> gListarCriteriosPorDireccion(int piIdDireccion, String psIdIndicar)
        {
            List<Criterio> listaCriterios = new List<Criterio>();
            Respuesta<List<Criterio>> respuesta = new Respuesta<List<Criterio>>();
            List<Criterio> listaCriteriosFinal = new List<Criterio>();
            Criterio criterio = new Criterio();
            try
            {

                listaCriterios = (from x in context.Criterio
                                  where x.Borrado == 0 && x.IdDireccion == piIdDireccion
                                  && x.IdIndicador.Equals(psIdIndicar)
                                  select x).Take(100).ToList();
                if (listaCriterios != null)
                {
                    foreach (Criterio item in listaCriterios)
                    {
                        criterio = new Criterio();
                        criterio.IdCriterio = item.IdCriterio;
                        criterio.DescCriterio = item.DescCriterio;
                        listaCriteriosFinal.Add(criterio);


                    }
                }
                respuesta.objObjeto = listaCriteriosFinal.OrderBy(x => x.DescCriterio).ToList();

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, listaCriterios);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        /// <summary>
        /// Filtra los criterios tomando en cuenta la dirección
        /// </summary>
        /// <param name="piIdDireccion"></param>
        /// <returns></returns>
        public Respuesta<List<Criterio>> gFiltrarCriterioConDireccion(int piIdDireccion, string psCodigoCriterio, string psNombreCriterio, string psIdIndicador)
        {
            List<Criterio> listaCriterios = new List<Criterio>();
            Respuesta<List<Criterio>> respuesta = new Respuesta<List<Criterio>>();
            List<Criterio> listaCriteriosFinal = new List<Criterio>();
            Criterio criterio = new Criterio();
            try
            {

                listaCriterios = (from x in context.Criterio
                                  where x.Borrado == 0 && x.IdDireccion == piIdDireccion &&(x.IdIndicador.Equals(psIdIndicador)
                                  &&(psCodigoCriterio.Equals("") || x.IdCriterio.ToUpper().Contains(psCodigoCriterio.ToUpper()))
                                  &&(psNombreCriterio.Equals("") || x.DescCriterio.ToUpper().Contains(psNombreCriterio.ToUpper()))
                                 )
                                  select x).Take(100).ToList();
                if (listaCriterios != null)
                {
                    foreach (Criterio item in listaCriterios)
                    {
                        criterio = new Criterio();
                        criterio.IdCriterio = item.IdCriterio;
                        criterio.DescCriterio = item.DescCriterio;
                        listaCriteriosFinal.Add(criterio);


                    }
                }
                respuesta.objObjeto = listaCriteriosFinal.OrderBy(x => x.DescCriterio).ToList();

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, listaCriterios);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        /// <summary>
        /// Elimina un registro de Criterio
        /// </summary>
        /// <param name="poCriterio"></param>
        /// <returns></returns>
        public Respuesta<Criterio> Eliminar(string poIdCriterio)
        {
            Respuesta<Criterio> resultado = new Respuesta<Criterio>();
            Criterio criterioAux = null;
       
            try
            {
                

                    // Realizamos la consulta
                    criterioAux = context.Criterio.Where(c => c.IdCriterio == poIdCriterio).First();

                    criterioAux.Borrado = 1;

                    context.Criterio.Attach(criterioAux);
                    context.Entry(criterioAux).State = EntityState.Modified;
                    context.SaveChanges();

                    
                    resultado.objObjeto = criterioAux;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, criterioAux);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return resultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="poCriterio"></param>
        /// <returns></returns>
        public Respuesta<Criterio> Modificar(Criterio poCriterio)
        {
            Respuesta<Criterio> resultado = new Respuesta<Criterio>();
            Criterio criterioAux = null;
            try
            {

                // Realizamos la consulta
                criterioAux = context.Criterio.Where(c => c.IdCriterio == poCriterio.IdCriterio).First();
                criterioAux.DescCriterio = poCriterio.DescCriterio;
                criterioAux.IdDireccion = poCriterio.IdDireccion;
                criterioAux.IdIndicador = poCriterio.IdIndicador;
                context.Criterio.Attach(criterioAux);
                context.Entry(criterioAux).State = EntityState.Modified;
                context.SaveChanges();

                resultado.objObjeto = criterioAux;

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, criterioAux);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return resultado;
        }

        /// <summary>
        /// Consulta Criterio por Codigo
        /// </summary>
        /// <param name="poDescripcion"></param>
        /// <returns></returns>
        public Respuesta<Criterio> ConsultarPorCodigo(string poCodigoCriterio)
        {
            Respuesta<Criterio> resultado = new Respuesta<Criterio>();
            Criterio criterio = null;
            try
            {

                // Realizamos la consulta
                criterio = context.Criterio.SingleOrDefault(c => c.IdCriterio == poCodigoCriterio);

                resultado.objObjeto = criterio;


            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, criterio);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return resultado;


        }


        /// <summary>
        /// Consultar un criterio por descripcion
        /// </summary>
        /// <param name="poDescCriterio"></param>
        /// <returns></returns>
        public Respuesta<Criterio> ConsultarPorDescripcion(string poDescCriterio)
        {
            Respuesta<Criterio> resultado = new Respuesta<Criterio>();
            Criterio criterio = null;
            try
            {

                // Realizamos la consulta
                criterio = context.Criterio.FirstOrDefault(c => c.DescCriterio == poDescCriterio && c.Borrado == 0);

                resultado.objObjeto = criterio;


            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, criterio);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return resultado;


        }

        public Respuesta<int> VerificarUso(string poIdCriterio)
        {
            Respuesta<int> resultado = new Respuesta<int>();

            try
            {

                var rows = context.ConstructorCriterio.Count(x => x.IdCriterio == poIdCriterio);

                if (rows == 0)
                {
                    //rows = context.DetalleRegistroIndicador.Count(x => x.IdCriterio == poIdCriterio);
                }


                resultado.objObjeto = rows;

                return resultado;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, 0);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="poIdCriterio"></param>
        /// <param name="poDescripcion"></param>
        /// <returns></returns>
        public Respuesta<List<Criterio>> ConsultarPorIdDescripcion(string poIdCriterio, string poDescripcion)
        {
            Respuesta<List<Criterio>> resultado = new Respuesta<List<Criterio>>();
            List<Criterio> criterios = null;
            try
            {
                IQueryable<Criterio> rows = null;

                // Realizamos la consulta si brindaron los dos parametros
                if (!(string.IsNullOrEmpty(poIdCriterio)) || !(string.IsNullOrEmpty(poDescripcion)))
                {
                    rows = context.Criterio.Where(c => (poIdCriterio.Equals("") || c.IdCriterio.Contains(poIdCriterio))
                                                       && (poDescripcion.Equals("") || c.DescCriterio.Contains(poDescripcion))
                                                       && c.Borrado == 0);
                }


                if (rows.Count() > 0)
                {
                    criterios = rows.ToList();
                }

                resultado.objObjeto = criterios;
                return resultado;

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, criterios);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }




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
            Respuesta<List<Criterio>> resultado = new Respuesta<List<Criterio>>();
            List<Criterio> criterios = new List<Criterio>();
            try
            {
                

                // Realizamos la consulta si brindaron los dos parametros

                criterios = context.Criterio.Where(c => (poIdCriterio.Equals("") || c.IdCriterio.ToUpper().Contains(poIdCriterio.ToUpper()))
                                                       && (poDescripcion.Equals("") || c.DescCriterio.ToUpper().Contains(poDescripcion.ToUpper()))
                                                       && (poDireccion.Equals("")|| c.Direccion.Nombre.ToUpper().Contains(poDireccion.ToUpper()))
                                                       &&(poIndicador.Equals("")|| c.Indicador.NombreIndicador.ToUpper().Contains(poIndicador.ToUpper()))
                                                       && c.Borrado == 0).ToList();
                
                resultado.objObjeto = criterios;
                return resultado;

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, criterios);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }




        }
        #endregion






    }
}
