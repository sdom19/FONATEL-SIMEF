using GB.SIMEF.Entities;
using GB.SIMEF.Entities.DTO;
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
        /// Función que permite insertar o actualizar un objeto tipo ArgumentoFormula
        /// </summary>
        /// <param name="pArgumentoFormula"></param>
        /// <returns></returns>
        public ArgumentoFormula ActualizarDatos(ArgumentoFormula pArgumentoFormula)
        {
            ArgumentoFormula argumento = new ArgumentoFormula();

            using (db = new SIMEFContext())
            {
                argumento = db.Database.SqlQuery<ArgumentoFormula>
                ("execute pa_ActualizarArgumentoFormula @pIdArgumentoFormula, @pIdFormulaTipoArgumento, @pIdDefinicionFecha, @pIdVariableDatoCriterio, @pIdFormula, @pPredicadoSQL, @pOrdenEnFormula, @pEtiqueta",
                    new SqlParameter("@pIdArgumentoFormula", pArgumentoFormula.IdArgumentoFormula),
                    new SqlParameter("@pIdFormulaTipoArgumento", pArgumentoFormula.IdFormulasTipoArgumento),
                    pArgumentoFormula.IdDefinicionFecha == 0 || pArgumentoFormula.IdDefinicionFecha == null ?
                        new SqlParameter("@pIdDefinicionFecha", DBNull.Value)
                        :
                        new SqlParameter("@pIdDefinicionFecha", pArgumentoFormula.IdDefinicionFecha),
                    pArgumentoFormula.IdVariableDatoCriterio == 0 || pArgumentoFormula.IdVariableDatoCriterio == null ?
                        new SqlParameter("@pIdVariableDatoCriterio", DBNull.Value)
                        :
                        new SqlParameter("@pIdVariableDatoCriterio", pArgumentoFormula.IdVariableDatoCriterio),
                    new SqlParameter("@pIdFormula", pArgumentoFormula.IdFormula),
                    new SqlParameter("@pPredicadoSQL", pArgumentoFormula.PredicadoSQL),
                    new SqlParameter("@pOrdenEnFormula", pArgumentoFormula.OrdenEnFormula),
                    new SqlParameter("@pEtiqueta", pArgumentoFormula.Etiqueta)
                ).FirstOrDefault();
            }
            return argumento;
        }

        /// <summary>
        /// 12/02/2023
        /// José Navarro Acuña
        /// Función que permite eliminar todos los argumentos de una fórmula (variables dato / criterios & definición de fechas)
        /// </summary>
        /// <param name="pArgumentoFormula"></param>
        /// <returns></returns>
        public bool EliminarArgumentos(ArgumentoFormula pArgumentoFormula)
        {
            int exito;

            using (db = new SIMEFContext())
            {
                exito = db.Database.SqlQuery<int>(
                    "execute pa_EliminarArgumentoDeFormula @IdFormulaCalculo",
                    new SqlParameter("@IdFormulaCalculo", pArgumentoFormula.IdFormula)
                ).FirstOrDefault();
            }
            return exito == 1;
        }
    }
}
