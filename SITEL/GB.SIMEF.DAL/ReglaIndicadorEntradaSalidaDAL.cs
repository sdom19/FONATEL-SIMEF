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
    public class ReglaIndicadorEntradaSalidaDAL : BitacoraDAL
    {
        private SIMEFContext db;
        

        public List<ReglaIndicadorEntradaSalida> ActualizarDatos(ReglaIndicadorEntradaSalida pReglaIndicadorEntradaSalida)
        {
            List<ReglaIndicadorEntradaSalida> ListaReglaIndicadorEntradaSalida = new List<ReglaIndicadorEntradaSalida>();

            using (db = new SIMEFContext())
            {
                ListaReglaIndicadorEntradaSalida = db.Database.SqlQuery<ReglaIndicadorEntradaSalida>
                ("execute spActualizarReglaIndicadorEntradaSalida @IdCompara,@IdDetalleReglaValidacion,@IdDetalleIndicador,@IdIndicador",
                    new SqlParameter("@IdCompara", pReglaIndicadorEntradaSalida.idReglaComparacionEntradaSalida),
                    new SqlParameter("@IdDetalleReglaValidacion", pReglaIndicadorEntradaSalida.IdDetalleReglaValidacion),
                    new SqlParameter("@IdDetalleIndicador", pReglaIndicadorEntradaSalida.IdDetalleIndicadorVariable),
                    new SqlParameter("@IdIndicador", pReglaIndicadorEntradaSalida.IdIndicador)
                ).ToList();

                ListaReglaIndicadorEntradaSalida = ListaReglaIndicadorEntradaSalida.Select(X => new ReglaIndicadorEntradaSalida
                {
                    idReglaComparacionEntradaSalida = X.idReglaComparacionEntradaSalida,
                    IdDetalleReglaValidacion = X.IdDetalleReglaValidacion,
                    IdDetalleIndicadorVariable = X.IdDetalleIndicadorVariable,
                    IdIndicador = X.IdIndicador

                }).ToList();

                return ListaReglaIndicadorEntradaSalida;
            }
        }

    }
}
