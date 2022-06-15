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
    public class NivelAD : LocalContextualizer
    {

        #region Atributos
        SUTEL_IndicadoresEntities context;
        #endregion

        #region Constructores

        public NivelAD(ApplicationContext appContext)
            : base(appContext)
        {
            context = new SUTEL_IndicadoresEntities();
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Inserta en base de datos un registro de Nivel
        /// </summary>
        /// <param name="opNivel">Objeto Nivel</param>
        /// <returns></returns>
        public Respuesta<Nivel> Agregar(Nivel opNivel)
        {
            Respuesta<Nivel> respuesta = new Respuesta<Nivel>();
            Nivel nivelAgregar = new Nivel();

            try
            {
                nivelAgregar.InjectFrom(opNivel);
                respuesta.objObjeto = opNivel;
                using (TransactionScope scope = new TransactionScope())
                {


                    context.Nivel.Add(nivelAgregar);
                    context.SaveChanges();
                    respuesta.objObjeto = nivelAgregar;
                    scope.Complete();

                }


            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, opNivel);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }
            return respuesta;
        }





        /// <summary>
        /// Lista un conjuto de Niveles
        /// </summary>
        /// <returns>Listado de Niveles</returns>
        public Respuesta<List<Nivel>> Listar()
        {
            SUTEL_IndicadoresEntities context = new SUTEL_IndicadoresEntities();

            List<Nivel> listadoNiveles = new List<Nivel>();
            Respuesta<List<Nivel>> respuesta = new Respuesta<List<Nivel>>();

            try
            {
                listadoNiveles = context.Nivel.Where(x => x.Borrado == 0).ToList();

                respuesta.objObjeto = listadoNiveles;

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, listadoNiveles);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        /// <summary>
        /// Elimina un registro de Nivel
        /// </summary>
        /// <param name="poNivel"></param>
        /// <returns></returns>
        public Respuesta<Nivel> Eliminar(Nivel poNivel)
        {
            Respuesta<Nivel> respuesta = new Respuesta<Nivel>();
            Nivel nivel = new Nivel();
            try
            {
                nivel.InjectFrom(poNivel);
                using (TransactionScope scope = new TransactionScope())
                {

                    // Realizamos la consulta
                    Nivel nivelAux = context.Nivel.Where(c => c.IdNivel == poNivel.IdNivel).First();

                    nivelAux.Borrado = 1;

                    context.Nivel.Attach(nivelAux);
                    context.Entry(nivelAux).State = EntityState.Modified;
                    int i = context.SaveChanges();
                    respuesta.objObjeto = nivelAux;
                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                respuesta.toError(msj, poNivel);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return respuesta;
        }

        /// <summary>
        /// Consulta un Nivel por Identificador
        /// </summary>
        /// <param name="poNivel">Id del Nivel a consultar</param>
        /// <returns></returns>
        public Respuesta<Nivel> Consultar(int poNivel)
        {
            Respuesta<Nivel> resultado = new Respuesta<Nivel>();
            Nivel nivel = new Nivel();
            try
            {
                // Realizamos la consulta
                Nivel nivelAux = context.Nivel.Where(c => c.IdNivel == poNivel).First();
                resultado.objObjeto = nivelAux;
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, null);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return resultado;
        }
    

        public Respuesta<Nivel> Modificar(Nivel poNivel)
        {
            Respuesta<Nivel> resultado = new Respuesta<Nivel>();
            Nivel nivel = new Nivel();
            try
            {
                nivel.InjectFrom(poNivel);
                using (TransactionScope scope = new TransactionScope())
                {
                    resultado.objObjeto = poNivel;
                    // Realizamos la consulta
                    Nivel nivelAux = context.Nivel.Where(c => c.IdNivel == poNivel.IdNivel).First();
                    nivelAux.DescNivel = poNivel.DescNivel;
                    context.Nivel.Attach(nivelAux);
                    context.Entry(nivelAux).State = EntityState.Modified;
                    int i = context.SaveChanges();

                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, poNivel);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }

            return resultado;
        }

        public Respuesta<Nivel> ConsultarPorDescripcion(string poDescripcion)
        {
            Respuesta<Nivel> resultado = new Respuesta<Nivel>();
            Nivel nivel = null;
            try
            {

                // Realizamos la consulta
                var rows = context.Nivel.Where(c => c.DescNivel == poDescripcion && c.Borrado == 0);

                if (rows.Count() > 0)
                {
                    nivel = rows.First();
                }

                resultado.objObjeto = nivel;
                return resultado;

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, nivel);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }




        }

        public Respuesta<List<Nivel>> ConsultarPorIdDescripcion(int poIdNivel, string poDescripcion)
        {
            Respuesta<List<Nivel>> resultado = new Respuesta<List<Nivel>>();
            List<Nivel> nivel = null;
            try
            {
                IQueryable<Nivel> rows = null;

                // Realizamos la consulta si brindaron los dos parametros
                if (poIdNivel > 0 && !(string.IsNullOrEmpty(poDescripcion)))
                {
                    rows = context.Nivel.Where(c => c.IdNivel.ToString().Contains(poIdNivel.ToString()) && c.DescNivel.Contains(poDescripcion) && c.Borrado == 0);
                }
                else if (poIdNivel > 0)
                {
                    rows = context.Nivel.Where(c => c.IdNivel.ToString().Contains(poIdNivel.ToString()) && c.Borrado == 0);
                }
                else
                {
                    rows = context.Nivel.Where(c => c.DescNivel.Contains(poDescripcion) && c.Borrado == 0);
                }


                if (rows.Count() > 0)
                {
                    nivel = rows.ToList();
                }

                resultado.objObjeto = nivel;
                return resultado;

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                string msj = GB.SUTEL.Shared.ErrorTemplate.InternalError;
                resultado.toError(msj, nivel);
                throw AppContext.ExceptionBuilder.BuildBusinessException(msj, ex);
            }




        }

        #endregion
    }
}
