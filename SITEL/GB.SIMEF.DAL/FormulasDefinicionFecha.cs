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
        public FormulasDefinicionFecha ActualizarDatos(FormulasDefinicionFecha pFormulasDefinicionFecha)
        {
            FormulasDefinicionFecha envioSolicitudes = new FormulasDefinicionFecha();

            using (db = new SIMEFContext())
            {
                envioSolicitudes = db.Database.SqlQuery<FormulasDefinicionFecha>
                ("execute spActualizarFormulasDefinicionFecha @pIdFormulasDefinicionFecha, @pFechaInicio, @pFechaFinal, @pIdUnidadMedida, @pIdTipoFechaInicio, @pIdTipoFechaFinal, @pIdCategoriaInicio, @pIdCategoriaFinal, @pIdIndicador",
                    new SqlParameter("@pIdFormulasDefinicionFecha", pFormulasDefinicionFecha.IdFormulasDefinicionFecha),
                    new SqlParameter("@pFechaInicio", pFormulasDefinicionFecha.FechaInicio),
                    new SqlParameter("@pFechaFinal", pFormulasDefinicionFecha.FechaFinal),
                    new SqlParameter("@pIdUnidadMedida", pFormulasDefinicionFecha.IdUnidadMedida),
                    new SqlParameter("@pIdTipoFechaInicio", pFormulasDefinicionFecha.IdTipoFechaInicio),
                    new SqlParameter("@pIdTipoFechaFinal", pFormulasDefinicionFecha.IdTipoFechaFinal),
                    new SqlParameter("@pIdCategoriaInicio", pFormulasDefinicionFecha.IdCategoriaInicio),
                    new SqlParameter("@pIdCategoriaFinal", pFormulasDefinicionFecha.IdCategoriaFinal),
                    new SqlParameter("@pIdIndicador", pFormulasDefinicionFecha.IdIndicador)
                ).FirstOrDefault();

            }

            return envioSolicitudes;
        }
    }
}
