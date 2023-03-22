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
        

        public List<ReglaIndicadorSalida> ActualizarDatos(ReglaIndicadorSalida pReglaIndicadorSalida)
        {
            List<ReglaIndicadorSalida> ListaReglaIndicadorSalida = new List<ReglaIndicadorSalida>();

            using (db = new SIMEFContext())
            {
                ListaReglaIndicadorSalida = db.Database.SqlQuery<ReglaIndicadorSalida>
                ("execute pa_ActualizarReglaIndicadorSalida @IdCompara,@IdDetalleReglaValidacion,  @IdDetalleIndicador, @IdIndicador",
                    new SqlParameter("@IdCompara", pReglaIndicadorSalida.IdReglaIndicadorSalida),
                    new SqlParameter("@IdDetalleReglaValidacion", pReglaIndicadorSalida.IdDetalleReglaValidacion),
                    new SqlParameter("@IdIndicador", pReglaIndicadorSalida.IdIndicador),
                    new SqlParameter("@IdDetalleIndicador", pReglaIndicadorSalida.IdDetalleIndicadorVariable)
                ).ToList();

                ListaReglaIndicadorSalida = ListaReglaIndicadorSalida.Select(X => new ReglaIndicadorSalida
                {
                    idIndicadorComparaString = Utilidades.Encriptar(X.IdIndicador.ToString()),
                    idVariableComparaString = Utilidades.Encriptar(X.IdDetalleIndicadorVariable.ToString()),
                    IdReglaIndicadorSalida = X.IdReglaIndicadorSalida,
                    IdDetalleReglaValidacion = X.IdDetalleReglaValidacion,
                    IdIndicador = X.IdIndicador,
                    IdDetalleIndicadorVariable = X.IdDetalleIndicadorVariable,

                }).ToList();

                return ListaReglaIndicadorSalida;
            }
        }

    }
}
