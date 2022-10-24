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
    public class ReglaIndicadorSalidaDAL : BitacoraDAL
    {
        private SIMEFContext db;
        
        /// <summary>
        /// Función que retorna todos las frecuencias de envio registradas en estado activo.
        /// Se puede filtrar por el ID del objecto
        /// </summary>
        /// <returns></returns>
        public List<ReglaIndicadorSalida> ObtenerDatos(ReglaIndicadorSalida pReglaIndicadorSalida)
        {
            List<ReglaIndicadorSalida> ListaReglaIndicadorSalida = new List<ReglaIndicadorSalida>();
            ListaReglaIndicadorSalida = db.Database.SqlQuery<ReglaIndicadorSalida>
                ("execute spObtenerFrecuenciasEnvio @idFrecuencia",
                new SqlParameter("@idFrecuencia", 1)
                ).ToList();
            return ListaReglaIndicadorSalida;

        }

        public List<ReglaIndicadorSalida> ActualizarDatos(ReglaIndicadorSalida pReglaIndicadorSalida)
        {
            List<ReglaIndicadorSalida> ListaReglaIndicadorSalida = new List<ReglaIndicadorSalida>();

            using (db = new SIMEFContext())
            {
                ListaReglaIndicadorSalida = db.Database.SqlQuery<ReglaIndicadorSalida>
                ("execute spActualizarReglaIndicadorSalida @IdReglaIndicadorSalida,@IdIndicador,@IdDetalleReglaValidacion",
                    new SqlParameter("@IdReglaIndicadorSalida", pReglaIndicadorSalida.IdReglaIndicadorSalida),
                    new SqlParameter("@IdIndicador", pReglaIndicadorSalida.IdIndicador),
                    new SqlParameter("@IdDetalleReglaValidacion", pReglaIndicadorSalida.IdDetalleReglaValidacion)
                ).ToList();

                ListaReglaIndicadorSalida = ListaReglaIndicadorSalida.Select(X => new ReglaIndicadorSalida
                {
                    IdReglaIndicadorSalida = X.IdReglaIndicadorSalida,
                    IdIndicador = X.IdIndicador,
                    IdDetalleReglaValidacion = X.IdDetalleReglaValidacion

                }).ToList();

                return ListaReglaIndicadorSalida;
            }
        }

    }
}
