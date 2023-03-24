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
        

        public List<ReglaIndicadorEntrada> ActualizarDatos(ReglaIndicadorEntrada objeto)
        {
            List<ReglaIndicadorEntrada> ListaReglaIndicadorEntrada = new List<ReglaIndicadorEntrada>();

            using (db = new SIMEFContext())
            {
                ListaReglaIndicadorEntrada = db.Database.SqlQuery<ReglaIndicadorEntrada>

                ("execute pa_ActualizarReglaIndicadorEntrada @IdCompara, @IdDetalleReglaValidacion, @IdDetalleIndicador, @IdIndicador",
                    new SqlParameter("@IdCompara", objeto.idReglaComparacionIndicador),
                    new SqlParameter("@IdDetalleReglaValidacion", objeto.idDetalleReglaValidacion),
                    new SqlParameter("@IdIndicador", objeto.idIndicador),
                    new SqlParameter("@IdDetalleIndicador", objeto.idDetalleIndicadorVariable)
                ).ToList();

                ListaReglaIndicadorEntrada = ListaReglaIndicadorEntrada.Select(X => new ReglaIndicadorEntrada
                {
                    idIndicadorComparaString= Utilidades.Encriptar(X.idIndicador.ToString()),
                    idVariableComparaString = Utilidades.Encriptar(X.idDetalleIndicadorVariable.ToString()),
                    idReglaComparacionIndicador = X.idReglaComparacionIndicador,
                    idDetalleReglaValidacion = X.idDetalleReglaValidacion,
                    idIndicador = X.idIndicador,
                    idDetalleIndicadorVariable = X.idDetalleIndicadorVariable,

                }).ToList();

                return ListaReglaIndicadorEntrada;
            }
        }

    }
}
