using GB.SIMEF.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.DAL
{
    public class ArgumentoFormulaDAL : BitacoraDAL
    {
        private SIMEFContext db;

        /// <summary>
        /// 01/02/2023
        /// José Navarro Acuña
        /// Función qu permite insertar o actualizar un objeto tipo ArgumentoFormula
        /// </summary>
        /// <param name="pArgumentoFormula"></param>
        /// <returns></returns>
        public ArgumentoFormula ActualizarDatos(ArgumentoFormula pArgumentoFormula)
        {
            ArgumentoFormula argumento = new ArgumentoFormula();

            using (db = new SIMEFContext())
            {
                argumento = db.Database.SqlQuery<ArgumentoFormula>
                ("execute spActualizarArgumentoFormula @pIdArgumentoFormula, @pIdFormulasTipoArgumento, @pIdFormulasDefinicionFecha, @pIdFormulasVariablesDatoCriterio, @pIdFormula, @pPredicadoSQL, @pOrdenEnFormula",
                    new SqlParameter("@pIdArgumentoFormula", pArgumentoFormula.IdArgumentoFormula),
                    new SqlParameter("@pIdFormulasTipoArgumento", pArgumentoFormula.IdFormulasTipoArgumento),
                    pArgumentoFormula.IdFormulasDefinicionFecha == 0 ?
                        new SqlParameter("@pIdFormulasDefinicionFecha", DBNull.Value)
                        :
                        new SqlParameter("@pIdFormulasDefinicionFecha", pArgumentoFormula.IdFormulasDefinicionFecha),
                    pArgumentoFormula.IdFormulasVariableDatoCriterio == 0 ?
                        new SqlParameter("@pIdFormulasVariablesDatoCriterio", DBNull.Value)
                        :
                        new SqlParameter("@pIdFormulasVariablesDatoCriterio", pArgumentoFormula.IdFormulasVariableDatoCriterio),
                    new SqlParameter("@pIdFormula", pArgumentoFormula.IdFormula),
                    new SqlParameter("@pPredicadoSQL", pArgumentoFormula.PredicadoSQL),
                    new SqlParameter("@pOrdenEnFormula", pArgumentoFormula.OrdenEnFormula)
                ).FirstOrDefault();
            }
            return argumento;
        }
    }
}
