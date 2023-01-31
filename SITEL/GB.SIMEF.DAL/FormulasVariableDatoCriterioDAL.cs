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
            FormulasVariableDatoCriterio envioSolicitudes = new FormulasVariableDatoCriterio();

            using (db = new SIMEFContext())
            {
                envioSolicitudes = db.Database.SqlQuery<FormulasVariableDatoCriterio>
                ("execute spActualizarFormulasVariableDatoCriterio @pIdFormulasVariableDatoCriterio, @pIdFuenteIndicador, @pIdIndicador, @pIdVariableDato, @pIdCriterio, @pIdCategoria, @pIdDetalleCategoria, @pIdAcumulacion, @pEsValorTotal",
                    new SqlParameter("@pIdFormulasVariablesDatoCriterio", pFormulasVariableDatoCriterio.IdFormulasVariableDatoCriterio),
                    new SqlParameter("@pIdFuenteIndicador", pFormulasVariableDatoCriterio.IdFuenteIndicador),
                    new SqlParameter("@pIdIndicador", pFormulasVariableDatoCriterio.IdIndicador),
                    new SqlParameter("@pIdVariableDato", pFormulasVariableDatoCriterio.IdVariableDato),
                    new SqlParameter("@pIdCriterio", pFormulasVariableDatoCriterio.IdCriterio),
                    new SqlParameter("@pIdCategoria", pFormulasVariableDatoCriterio.IdCategoria),
                    new SqlParameter("@pIdDetalleCategoria", pFormulasVariableDatoCriterio.IdDetalleCategoria),
                    new SqlParameter("@pIdAcumulacion", pFormulasVariableDatoCriterio.IdAcumulacion),
                    new SqlParameter("@pEsValorTotal", pFormulasVariableDatoCriterio.EsValorTotal)
                ).FirstOrDefault();

            }

            return envioSolicitudes;
        }
    }
}
