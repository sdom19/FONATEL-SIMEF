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
                ("execute pa_ActualizarReglaSecuencial @IdCompara,@IdCategoria,@IdDetalleReglaValidacion",
                    new SqlParameter("@IdCompara", pReglaSecuencial.idReglaSecuencial),
                    new SqlParameter("@IdCategoria", pReglaSecuencial.idCategoriaDesagregacion),
                    new SqlParameter("@IdDetalleReglaValidacion", pReglaSecuencial.idDetalleReglaValidacion)
                ).ToList();

                ListaReglaSecuencial = ListaReglaSecuencial.Select(X => new ReglaSecuencial
                {
                    idReglaSecuencial= X.idReglaSecuencial,
                    idCategoriaDesagregacion = X.idCategoriaDesagregacion,
                    idDetalleReglaValidacion = X.idDetalleReglaValidacion

                }).ToList();

                return ListaReglaSecuencial;
            }
        }

    }
}
