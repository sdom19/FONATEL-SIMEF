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
    public class ReglaSecuencialDAL : BitacoraDAL
    {
        private SIMEFContext db;
        

        public List<ReglaSecuencial> ActualizarDatos(ReglaSecuencial pReglaSecuencial)
        {
            List<ReglaSecuencial> ListaReglaSecuencial = new List<ReglaSecuencial>();

            using (db = new SIMEFContext())
            {
                ListaReglaSecuencial = db.Database.SqlQuery<ReglaSecuencial>
                ("execute spActualizarReglaSecuencial @IdCompara,@IdCategoria,@IdDetalleReglaValidacion",
                    new SqlParameter("@IdCompara", pReglaSecuencial.IdCompara),
                    new SqlParameter("@IdCategoria", pReglaSecuencial.IdCategoria),
                    new SqlParameter("@IdDetalleReglaValidacion", pReglaSecuencial.IdDetalleReglaValidacion)
                ).ToList();

                ListaReglaSecuencial = ListaReglaSecuencial.Select(X => new ReglaSecuencial
                {
                    IdCompara = X.IdCompara,
                    IdCategoria = X.IdCategoria,
                    IdDetalleReglaValidacion = X.IdDetalleReglaValidacion

                }).ToList();

                return ListaReglaSecuencial;
            }
        }

    }
}
