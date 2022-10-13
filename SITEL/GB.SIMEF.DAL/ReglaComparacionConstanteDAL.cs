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
    public class ReglaComparacionConstanteDAL : BitacoraDAL
    {
        private SIMEFContext db;
        
        /// <summary>
        /// 23/08/2022
        /// José Navarro Acuña
        /// Función que retorna todos las frecuencias de envio registradas en estado activo.
        /// Se puede filtrar por el ID del objecto
        /// </summary>
        /// <returns></returns>
        public List<ReglaComparacionConstante> ObtenerDatos(ReglaComparacionConstante pReglaComparacionConstante)
        {
            List<ReglaComparacionConstante> ListaReglaComparacionConstante = new List<ReglaComparacionConstante>();
            ListaReglaComparacionConstante = db.Database.SqlQuery<ReglaComparacionConstante>
                ("execute spObtenerFrecuenciasEnvio @idFrecuencia",
                new SqlParameter("@idFrecuencia", 1)
                ).ToList();
            return ListaReglaComparacionConstante;

        }

        public List<ReglaComparacionConstante> ActualizarDatos(ReglaComparacionConstante pReglaComparacionConstante)
        {
            List<ReglaComparacionConstante> ListaReglaComparacionConstante = new List<ReglaComparacionConstante>();

            using (db = new SIMEFContext())
            {
                ListaReglaComparacionConstante = db.Database.SqlQuery<ReglaComparacionConstante>
                ("execute spActualizarReglaComparacionConstante @IdCompara,@Constante,@idvariable,@idDetalleReglaValidacion",
                    new SqlParameter("@IdCompara", pReglaComparacionConstante.idCompara),
                    new SqlParameter("@IdTipoReglaValidacion", pReglaComparacionConstante.Constante),
                    new SqlParameter("@IdCategoria", pReglaComparacionConstante.idvariable),
                    new SqlParameter("@IdCategoriaAtributo", pReglaComparacionConstante.idDetalleReglaValidacion)
                ).ToList();

                ListaReglaComparacionConstante = ListaReglaComparacionConstante.Select(X => new ReglaComparacionConstante
                {
                    idCompara = X.idCompara,
                    Constante = X.Constante,
                    idvariable = X.idvariable,
                    idDetalleReglaValidacion = X.idDetalleReglaValidacion

                }).ToList();

                return ListaReglaComparacionConstante;
            }
        }

    }
}
