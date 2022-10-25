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
    public class ReglaIndicadorEntradaDAL : BitacoraDAL
    {
        private SIMEFContext db;
        
        /// <summary>
        /// Función que retorna todos las frecuencias de envio registradas en estado activo.
        /// Se puede filtrar por el ID del objecto
        /// </summary>
        /// <returns></returns>
        public List<ReglaIndicadorEntrada> ObtenerDatos(ReglaIndicadorEntrada pReglaIndicadorEntrada)
        {
            List<ReglaIndicadorEntrada> ListaReglaIndicadorEntrada = new List<ReglaIndicadorEntrada>();
            ListaReglaIndicadorEntrada = db.Database.SqlQuery<ReglaIndicadorEntrada>
                ("execute spObtenerFrecuenciasEnvio @idFrecuencia",
                new SqlParameter("@idFrecuencia", 1)
                ).ToList();
            return ListaReglaIndicadorEntrada;

        }

        public List<ReglaIndicadorEntrada> ActualizarDatos(ReglaIndicadorEntrada pReglaIndicadorEntrada)
        {
            List<ReglaIndicadorEntrada> ListaReglaIndicadorEntrada = new List<ReglaIndicadorEntrada>();

            using (db = new SIMEFContext())
            {
                ListaReglaIndicadorEntrada = db.Database.SqlQuery<ReglaIndicadorEntrada>
                ("execute spActualizarReglaIndicadorEntrada @IdCompara,@IdIndicador,@IdVariableCompara,@IdDetalleReglaValidacion",
                    new SqlParameter("@IdCompara", pReglaIndicadorEntrada.IdReglaIndicadorEntrada),
                    new SqlParameter("@IdIndicador", pReglaIndicadorEntrada.IdIndicador),
                    new SqlParameter("@IdVariableCompara", pReglaIndicadorEntrada.IdVariableCompara),
                    new SqlParameter("@IdDetalleReglaValidacion", pReglaIndicadorEntrada.IdDetalleReglaValidacion)
                ).ToList();

                ListaReglaIndicadorEntrada = ListaReglaIndicadorEntrada.Select(X => new ReglaIndicadorEntrada
                {
                    IdReglaIndicadorEntrada = X.IdReglaIndicadorEntrada,
                    IdIndicador = X.IdIndicador,
                    IdVariableCompara = X.IdVariableCompara,
                    IdDetalleReglaValidacion = X.IdDetalleReglaValidacion

                }).ToList();

                return ListaReglaIndicadorEntrada;
            }
        }

    }
}
