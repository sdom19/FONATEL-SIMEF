using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.DAL
{
    public class AcumulacionFormulaDAL : BitacoraDAL
    {
        private SIMEFContext db;

        /// <summary>
        /// 28/11/2022
        /// José Navarro Acuña
        /// Función que permite obtener un listado de las acumulaciones de fórmulas de fuentes Fonatel
        /// </summary>
        /// <param name="pAcumulacionFormula"></param>
        /// <returns></returns>
        public List<AcumulacionFormula> ObtenerDatos(AcumulacionFormula pAcumulacionFormula)
        {
            List<AcumulacionFormula> listaAcumulaciones= new List<AcumulacionFormula>();

            using (db = new SIMEFContext())
            {
                listaAcumulaciones= db.Database.SqlQuery<AcumulacionFormula>
                    ("execute spObtenerAcumulacionesFormula @pIdAcumulacionFormula",
                     new SqlParameter("@pIdAcumulacionFormula", pAcumulacionFormula.IdAcumulacionFormula)
                    ).ToList();

                listaAcumulaciones= listaAcumulaciones.Select(x => new AcumulacionFormula()
                {
                    id = Utilidades.Encriptar(x.IdAcumulacionFormula.ToString()),
                    Acumulacion = x.Acumulacion,
                    Estado = x.Estado
                }).ToList();
            }

            return listaAcumulaciones;
        }
    }
}
