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

        public List<ReglaIndicadorEntrada> ActualizarDatos(ReglaIndicadorEntrada objeto)
        {
            List<ReglaIndicadorEntrada> ListaReglaIndicadorEntrada = new List<ReglaIndicadorEntrada>();

            using (db = new SIMEFContext())
            {
                ListaReglaIndicadorEntrada = db.Database.SqlQuery<ReglaIndicadorEntrada>

                ("execute spActualizarReglaIndicadorEntrada @IdCompara, @IdDetalleReglaValidacion, @IdDetalleIndicador, @IdIndicador",
                    new SqlParameter("@IdCompara", objeto.IdCompara),
                    new SqlParameter("@IdDetalleReglaValidacion", objeto.IdDetalleReglaValidacion),
                    new SqlParameter("@IdIndicador", objeto.IdIndicador),
                    new SqlParameter("@IdDetalleIndicador", objeto.IdDetalleIndicador)
                ).ToList();

                ListaReglaIndicadorEntrada = ListaReglaIndicadorEntrada.Select(X => new ReglaIndicadorEntrada
                {
                    idIndicadorComparaString= Utilidades.Encriptar(X.IdIndicador.ToString()),
                    idVariableComparaString = Utilidades.Encriptar(X.IdDetalleIndicador.ToString()),
                    IdCompara = X.IdCompara,
                    IdDetalleReglaValidacion = X.IdDetalleReglaValidacion,
                    IdIndicador = X.IdIndicador,
                    IdDetalleIndicador = X.IdDetalleIndicador,

                }).ToList();

                return ListaReglaIndicadorEntrada;
            }
        }

    }
}
