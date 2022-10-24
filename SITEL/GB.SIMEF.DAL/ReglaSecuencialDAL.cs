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
        
        /// <summary>
        /// Función que retorna todos las frecuencias de envio registradas en estado activo.
        /// Se puede filtrar por el ID del objecto
        /// </summary>
        /// <returns></returns>
        public List<ReglaSecuencial> ObtenerDatos(ReglaSecuencial pReglaSecuencial)
        {
            List<ReglaSecuencial> ListaReglaSecuencial = new List<ReglaSecuencial>();
            ListaReglaSecuencial = db.Database.SqlQuery<ReglaSecuencial>
                ("execute spObtenerFrecuenciasEnvio @idFrecuencia",
                new SqlParameter("@idFrecuencia", 1)
                ).ToList();
            return ListaReglaSecuencial;

        }

        public List<ReglaSecuencial> ActualizarDatos(ReglaSecuencial pReglaSecuencial)
        {
            List<ReglaSecuencial> ListaReglaSecuencial = new List<ReglaSecuencial>();

            using (db = new SIMEFContext())
            {
                ListaReglaSecuencial = db.Database.SqlQuery<ReglaSecuencial>
                ("execute spActualizarReglaSecuencial @IdCompara,@idCategoriaId,@idvariable,@IdDetalleReglaValidacion",
                    new SqlParameter("@IdCompara", pReglaSecuencial.idCompara),
                    new SqlParameter("@idCategoriaId", pReglaSecuencial.idCategoriaId),
                    new SqlParameter("@idvariable", pReglaSecuencial.idvariable),
                    new SqlParameter("@IdDetalleReglaValidacion", pReglaSecuencial.IdDetalleReglaValidacion)
                ).ToList();

                ListaReglaSecuencial = ListaReglaSecuencial.Select(X => new ReglaSecuencial
                {
                    idCompara = X.idCompara,
                    idCategoriaId = X.idCategoriaId,
                    idvariable = X.idvariable,
                    IdDetalleReglaValidacion = X.IdDetalleReglaValidacion

                }).ToList();

                return ListaReglaSecuencial;
            }
        }

    }
}
