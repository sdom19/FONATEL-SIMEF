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
                ("execute pa_ActualizarReglaIndicadorEntradaSalida @IdCompara,@IdDetalleReglaValidacion,@IdDetalleIndicador,@IdIndicador",
                    new SqlParameter("@IdCompara", pReglaIndicadorEntradaSalida.idReglaComparacionEntradaSalida),
                    new SqlParameter("@IdDetalleReglaValidacion", pReglaIndicadorEntradaSalida.idDetalleReglaValidacion),
                    new SqlParameter("@IdDetalleIndicador", pReglaIndicadorEntradaSalida.idDetalleIndicadorVariable),
                    new SqlParameter("@IdIndicador", pReglaIndicadorEntradaSalida.idIndicador)
                ).ToList();

                ListaReglaIndicadorEntradaSalida = ListaReglaIndicadorEntradaSalida.Select(X => new ReglaIndicadorEntradaSalida
                {
                    idReglaComparacionEntradaSalida = X.idReglaComparacionEntradaSalida,
                    idDetalleReglaValidacion = X.idDetalleReglaValidacion,
                    idDetalleIndicadorVariable = X.idDetalleIndicadorVariable,
                    idIndicador = X.idIndicador

                }).ToList();

                return ListaReglaIndicadorEntradaSalida;
            }
        }

    }
}
