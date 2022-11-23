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
    public class ReglaComparacionConstanteDAL : BitacoraDAL
    {
        private SIMEFContext db;
        

        public List<ReglaComparacionConstante> ActualizarDatos(ReglaComparacionConstante pReglaComparacionConstante)
        {
            List<ReglaComparacionConstante> ListaReglaComparacionConstante = new List<ReglaComparacionConstante>();

            using (db = new SIMEFContext())
            {
                ListaReglaComparacionConstante = db.Database.SqlQuery<ReglaComparacionConstante>
                ("execute spActualizarReglaComparacionConstante @IdCompara,@IdDetalleReglaValidacion,@Constante",
                    new SqlParameter("@IdCompara", pReglaComparacionConstante.idCompara),
                    new SqlParameter("@IdDetalleReglaValidacion", pReglaComparacionConstante.IdDetalleReglaValidacion),
                    new SqlParameter("@Constante", pReglaComparacionConstante.Constante)
                ).ToList();

                ListaReglaComparacionConstante = ListaReglaComparacionConstante.Select(X => new ReglaComparacionConstante
                {
                    idCompara = X.idCompara,
                    IdDetalleReglaValidacion = X.IdDetalleReglaValidacion,
                    Constante = X.Constante

                }).ToList();

                return ListaReglaComparacionConstante;
            }
        }

    }
}
