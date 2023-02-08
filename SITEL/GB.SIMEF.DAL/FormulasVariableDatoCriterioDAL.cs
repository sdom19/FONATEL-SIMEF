using GB.SIMEF.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.DAL
{
    public class FormulasVariableDatoCriterioDAL : BitacoraDAL
    {
        private SIMEFContext db;

        /// <summary>
        /// 31/01/2023
        /// José Navarro Acuña
        /// Función que permite insertar o actualizar un argumento de tipo FormulasVariableDatoCriterio
        /// </summary>
        /// <param name="pFormulasVariableDatoCriterio"></param>
        /// <returns></returns>
        public FormulasVariableDatoCriterio ActualizarDatos(FormulasVariableDatoCriterio pFormulasVariableDatoCriterio)
        {
            FormulasVariableDatoCriterio variableDato = new FormulasVariableDatoCriterio();

            using (db = new SIMEFContext())
            {
                variableDato = db.Database.SqlQuery<FormulasVariableDatoCriterio>
                ("execute spActualizarFormulasVariableDatoCriterio @pIdFormulasVariableDatoCriterio, @pIdFuenteIndicador, @pIdIndicador, @pIdVariableDato, @pIdCriterio, @pIdCategoria, @pIdDetalleCategoria, @pIdAcumulacion, @pEsValorTotal",
                    new SqlParameter("@pIdFormulasVariableDatoCriterio", pFormulasVariableDatoCriterio.IdFormulasVariableDatoCriterio),
                    new SqlParameter("@pIdFuenteIndicador", pFormulasVariableDatoCriterio.IdFuenteIndicador),
                    new SqlParameter("@pIdIndicador", pFormulasVariableDatoCriterio.IdIndicador),
                    pFormulasVariableDatoCriterio.IdVariableDato == null || pFormulasVariableDatoCriterio.IdVariableDato == 0 ?
                        new SqlParameter("@pIdVariableDato", DBNull.Value)
                        :
                        new SqlParameter("@pIdVariableDato", pFormulasVariableDatoCriterio.IdVariableDato),

                    string.IsNullOrEmpty(pFormulasVariableDatoCriterio.IdCriterio) ?
                        new SqlParameter("@pIdCriterio", DBNull.Value)
                        :
                        new SqlParameter("@pIdCriterio", pFormulasVariableDatoCriterio.IdCriterio),

                    pFormulasVariableDatoCriterio.IdCategoria == null || pFormulasVariableDatoCriterio.IdCategoria == 0 ?
                        new SqlParameter("@pIdCategoria", DBNull.Value)
                        :
                        new SqlParameter("@pIdCategoria", pFormulasVariableDatoCriterio.IdCategoria),

                    pFormulasVariableDatoCriterio.IdDetalleCategoria == null || pFormulasVariableDatoCriterio.IdDetalleCategoria == 0 ?
                        new SqlParameter("@pIdDetalleCategoria", DBNull.Value)
                        :
                        new SqlParameter("@pIdDetalleCategoria", pFormulasVariableDatoCriterio.IdDetalleCategoria),

                    new SqlParameter("@pIdAcumulacion", pFormulasVariableDatoCriterio.IdAcumulacion),
                    new SqlParameter("@pEsValorTotal", pFormulasVariableDatoCriterio.EsValorTotal)
                ).FirstOrDefault();
            }

            return variableDato;
        }

        /// <summary>
        /// 08/02/2023
        /// José Navarro Acuña
        /// Función que permite obtener los argumentos de tipo Variable dato/Criterio de una formula. Se puede filtrar por el ID de la fórmula
        /// </summary>
        /// <param name="pFormulasVariableDatoCriterio"></param>
        /// <returns></returns>
        public List<FormulasVariableDatoCriterio> ObtenerDatos(FormulasVariableDatoCriterio pFormulasVariableDatoCriterio)
        {
            List<FormulasVariableDatoCriterio> listaDetalles = new List<FormulasVariableDatoCriterio>();

            using (db = new SIMEFContext())
            {
                listaDetalles = db.Database.SqlQuery<FormulasVariableDatoCriterio>
                    (
                        "execute spObtenerVariableDatoCriterioFormulas @pIdFormula",
                        pFormulasVariableDatoCriterio.IdFormula == 0 ?
                            new SqlParameter("@pIdFormula", DBNull.Value)
                            :
                            new SqlParameter("@pIdFormula", pFormulasVariableDatoCriterio.IdFormula)
                    ).ToList();
            }
            return listaDetalles;
        }
    }
}
