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
        public FormulaVariableDatoCriterio ActualizarDatos(FormulaVariableDatoCriterio pFormulasVariableDatoCriterio)
        {
            FormulaVariableDatoCriterio variableDato = new FormulaVariableDatoCriterio();

            using (db = new SIMEFContext())
            {
                variableDato = db.Database.SqlQuery<FormulaVariableDatoCriterio>
                ("execute pa_ActualizarFormulaVariableDatoCriterio @pIdFormulaVariableDatoCriterio, @pIdFuenteIndicador, @pIdIndicador, @pIdDetalleIndicadorVariable, @pIdCriterio, @pidCategoriaDesagregacion, @pIdDetalleCategoriaTexto, @pIdAcumulacionFormula, @pEsValorTotal",
                    new SqlParameter("@pIdFormulaVariableDatoCriterio", pFormulasVariableDatoCriterio.IdFormulaVariableDatoCriterio),
                    new SqlParameter("@pIdFuenteIndicador", pFormulasVariableDatoCriterio.IdFuenteIndicador),
                    new SqlParameter("@pIdIndicador", pFormulasVariableDatoCriterio.IdIndicador),
                    pFormulasVariableDatoCriterio.IdDetalleIndicadorVariable == null || pFormulasVariableDatoCriterio.IdDetalleIndicadorVariable == 0 ?
                        new SqlParameter("@pIdDetalleIndicadorVariable", DBNull.Value)
                        :
                        new SqlParameter("@pIdDetalleIndicadorVariable", pFormulasVariableDatoCriterio.IdDetalleIndicadorVariable),

                    string.IsNullOrEmpty(pFormulasVariableDatoCriterio.IdCriterio) ?
                        new SqlParameter("@pIdCriterio", DBNull.Value)
                        :
                        new SqlParameter("@pIdCriterio", pFormulasVariableDatoCriterio.IdCriterio),

                    pFormulasVariableDatoCriterio.IdCategoriaDesagregacion == null || pFormulasVariableDatoCriterio.IdCategoriaDesagregacion == 0 ?
                        new SqlParameter("@pidCategoriaDesagregacion", DBNull.Value)
                        :
                        new SqlParameter("@pidCategoriaDesagregacion", pFormulasVariableDatoCriterio.IdCategoriaDesagregacion),

                    pFormulasVariableDatoCriterio.IdDetalleCategoriaTexto == null || pFormulasVariableDatoCriterio.IdDetalleCategoriaTexto == 0 ?
                        new SqlParameter("@pIdDetalleCategoriaTexto", DBNull.Value)
                        :
                        new SqlParameter("@pIdDetalleCategoriaTexto", pFormulasVariableDatoCriterio.IdDetalleCategoriaTexto),

                    pFormulasVariableDatoCriterio.IdAcumulacionFormula == null || pFormulasVariableDatoCriterio.IdAcumulacionFormula == 0 ?
                        new SqlParameter("@pIdAcumulacionFormula", DBNull.Value)
                        :
                        new SqlParameter("@pIdAcumulacionFormula", pFormulasVariableDatoCriterio.IdAcumulacionFormula)
                        ,
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
        public List<FormulaVariableDatoCriterio> ObtenerDatos(FormulaVariableDatoCriterio pFormulasVariableDatoCriterio)
        {
            List<FormulaVariableDatoCriterio> listaDetalles = new List<FormulaVariableDatoCriterio>();

            using (db = new SIMEFContext())
            {
                listaDetalles = db.Database.SqlQuery<FormulaVariableDatoCriterio>
                    (
                        "execute pa_ObtenerVariableDatoCriterioFormula @pIdFormulaCalculo",
                        pFormulasVariableDatoCriterio.IdFormula == 0 ?
                            new SqlParameter("@pIdFormulaCalculo", DBNull.Value)
                            :
                            new SqlParameter("@pIdFormulaCalculo", pFormulasVariableDatoCriterio.IdFormula)
                    ).ToList();
            }
            return listaDetalles;
        }
    }
}
