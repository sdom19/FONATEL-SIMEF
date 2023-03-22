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
                    new SqlParameter("@IdDetalleReglaValidacion", objeto.IdDetalleReglaValidacion),
                    new SqlParameter("@IdIndicador", objeto.IdIndicador),
                    new SqlParameter("@IdDetalleIndicador", objeto.IdDetalleIndicadorVariable)
                ).ToList();

                ListaReglaIndicadorEntrada = ListaReglaIndicadorEntrada.Select(X => new ReglaIndicadorEntrada
                {
                    idIndicadorComparaString= Utilidades.Encriptar(X.IdIndicador.ToString()),
                    idVariableComparaString = Utilidades.Encriptar(X.IdDetalleIndicadorVariable.ToString()),
                    idReglaComparacionIndicador = X.idReglaComparacionIndicador,
                    IdDetalleReglaValidacion = X.IdDetalleReglaValidacion,
                    IdIndicador = X.IdIndicador,
                    IdDetalleIndicadorVariable = X.IdDetalleIndicadorVariable,

                }).ToList();

                return ListaReglaIndicadorEntrada;
            }
        }

    }
}
