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
                    new SqlParameter("@IdCompara", pReglaIndicadorSalida.idReglaIndicadorSalida),
                    new SqlParameter("@IdDetalleReglaValidacion", pReglaIndicadorSalida.idDetalleReglaValidacion),
                    new SqlParameter("@IdIndicador", pReglaIndicadorSalida.idIndicador),
                    new SqlParameter("@IdDetalleIndicador", pReglaIndicadorSalida.idDetalleIndicadorVariable)
                ).ToList();

                ListaReglaIndicadorSalida = ListaReglaIndicadorSalida.Select(X => new ReglaIndicadorSalida
                {
                    idIndicadorComparaString = Utilidades.Encriptar(X.idIndicador.ToString()),
                    idVariableComparaString = Utilidades.Encriptar(X.idDetalleIndicadorVariable.ToString()),
                    idReglaIndicadorSalida = X.idReglaIndicadorSalida,
                    idDetalleReglaValidacion = X.idDetalleReglaValidacion,
                    idIndicador = X.idIndicador,
                    idDetalleIndicadorVariable = X.idDetalleIndicadorVariable,

                }).ToList();

                return ListaReglaIndicadorSalida;
            }
        }

    }
}
