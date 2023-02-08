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
                ("execute spActualizarArgumentoFormula @pIdArgumentoFormula, @pIdFormulasTipoArgumento, @pIdDefinicionFecha, @pIdVariableDatoCriterio, @pIdFormula, @pPredicadoSQL, @pOrdenEnFormula, @pEtiqueta",
                    new SqlParameter("@pIdArgumentoFormula", pArgumentoFormula.IdArgumentoFormula),
                    new SqlParameter("@pIdFormulasTipoArgumento", pArgumentoFormula.IdFormulasTipoArgumento),
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
        /// 08/02/2023
        /// José Navarro Acuña
        /// Función que permite obtener los argumentos de una formula. Se puede filtrar por el ID, por el ID de la fórmula u obtener todos los argumentos registrados
        /// </summary>
        /// <param name="pArgumentoFormula"></param>
        /// <returns></returns>
        public List<ArgumentoFormula> ObtenerDatos(ArgumentoFormula pArgumentoFormula)
        {
            List<ArgumentoFormula> listaArgumentos = new List<ArgumentoFormula>();

            using (db = new SIMEFContext())
            {
                listaArgumentos = db.Database.SqlQuery<ArgumentoFormula>
                    (
                        "execute spObtenerArgumentosFormula @pIdArgumentoFormula, @pIdFormula",
                        pArgumentoFormula.IdArgumentoFormula == 0 ?
                            new SqlParameter("@pIdArgumentoFormula", DBNull.Value)
                            :
                            new SqlParameter("@pIdArgumentoFormula", pArgumentoFormula.IdArgumentoFormula),

                        pArgumentoFormula.IdFormula == 0 ?
                            new SqlParameter("@pIdFormula", DBNull.Value)
                            :
                            new SqlParameter("@pIdFormula", pArgumentoFormula.IdFormula)
                    ).ToList();
            }
            return listaArgumentos;
        }
    }
}
