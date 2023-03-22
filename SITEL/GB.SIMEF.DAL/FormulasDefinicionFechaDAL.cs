using GB.SIMEF.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.DAL
{
    public class FormulasDefinicionFechaDAL : BitacoraDAL
    {
        private SIMEFContext db;

        /// <summary>
        /// 31/01/2023
        /// José Navarro Acuña
        /// Función que permite insertar o actualizar un argumento de tipo FormulasDefinicionFecha
        /// </summary>
        /// <param name="pFormulasVariableDatoCriterio"></param>
        /// <returns></returns>
        public FormulaDefinicionFecha ActualizarDatos(FormulaDefinicionFecha pFormulasDefinicionFecha)
        {
            FormulaDefinicionFecha envioSolicitudes = new FormulaDefinicionFecha();

            using (db = new SIMEFContext())
            {
                envioSolicitudes = db.Database.SqlQuery<FormulaDefinicionFecha>
                ("execute pa_ActualizarFormulaDefinicionFecha @pIdFormulaDefinicionFecha, @pFechaInicio, @pFechaFinal, @pIdUnidadMedida, @pIdTipoFechaInicio, @pIdTipoFechaFinal, @pIdCategoriaInicio, @pIdCategoriaFinal, @pIdIndicador",
                    new SqlParameter("@pIdFormulaDefinicionFecha", pFormulasDefinicionFecha.IdFormulaDefinicionFecha),
                    pFormulasDefinicionFecha.FechaInicio <= DateTime.MinValue || pFormulasDefinicionFecha.FechaInicio == null ?
                        new SqlParameter("@pFechaInicio", DBNull.Value)
                        :
                        new SqlParameter("@pFechaInicio", pFormulasDefinicionFecha.FechaInicio),

                    pFormulasDefinicionFecha.FechaFinal <= DateTime.MinValue || pFormulasDefinicionFecha.FechaFinal == null ?
                        new SqlParameter("@pFechaFinal", DBNull.Value)
                        :
                        new SqlParameter("@pFechaFinal", pFormulasDefinicionFecha.FechaFinal),
                    new SqlParameter("@pIdUnidadMedida", pFormulasDefinicionFecha.IdUnidadMedida),
                    new SqlParameter("@pIdTipoFechaInicio", pFormulasDefinicionFecha.IdTipoFechaInicio),
                    new SqlParameter("@pIdTipoFechaFinal", pFormulasDefinicionFecha.IdTipoFechaFinal),
                    pFormulasDefinicionFecha.IdCategoriaDesagregacionInicio == null || pFormulasDefinicionFecha.IdCategoriaDesagregacionInicio == 0 ?
                        new SqlParameter("@pIdCategoriaInicio", DBNull.Value)
                        :    
                        new SqlParameter("@pIdCategoriaInicio", pFormulasDefinicionFecha.IdCategoriaDesagregacionInicio),

                    pFormulasDefinicionFecha.IdCategoriaDesagregacionFinal == null || pFormulasDefinicionFecha.IdCategoriaDesagregacionFinal == 0 ?
                        new SqlParameter("@pIdCategoriaFinal", DBNull.Value)
                        :
                        new SqlParameter("@pIdCategoriaFinal", pFormulasDefinicionFecha.IdCategoriaDesagregacionFinal),
                    new SqlParameter("@pIdIndicador", pFormulasDefinicionFecha.IdIndicador)
                ).FirstOrDefault();
            }
            return envioSolicitudes;
        }

        /// <summary>
        /// 08/02/2023
        /// José Navarro Acuña
        /// Función que permite obtener los argumentos de tipo Variable dato/Criterio de una formula. Se puede filtrar por el ID de la fórmula
        /// </summary>
        /// <param name="pFormulasDefinicionFecha"></param>
        /// <returns></returns>
        public List<FormulaDefinicionFecha> ObtenerDatos(FormulaDefinicionFecha pFormulasDefinicionFecha)
        {
            List<FormulaDefinicionFecha> listaDetalles = new List<FormulaDefinicionFecha>();

            using (db = new SIMEFContext())
            {
                listaDetalles = db.Database.SqlQuery<FormulaDefinicionFecha>
                    (
                        "execute pa_ObtenerDefinicionFechaFormula @pIdFormula",
                        pFormulasDefinicionFecha.IdFormula == 0 ?
                            new SqlParameter("@pIdFormula", DBNull.Value)
                            :
                            new SqlParameter("@pIdFormula", pFormulasDefinicionFecha.IdFormula)
                    ).ToList();
            }
            return listaDetalles;
        }
    }
}
