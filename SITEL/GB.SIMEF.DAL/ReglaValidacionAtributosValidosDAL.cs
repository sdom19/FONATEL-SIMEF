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
    public class ReglaValidacionAtributosValidosDAL : BitacoraDAL
    {
        private SIMEFContext db;
        
        /// <summary>
        /// 23/08/2022
        /// José Navarro Acuña
        /// Función que retorna todos las frecuencias de envio registradas en estado activo.
        /// Se puede filtrar por el ID del objecto
        /// </summary>
        /// <returns></returns>
        public List<ReglaAtributosValidos> ObtenerDatos(ReglaAtributosValidos pReglaAtributosValidos)
        {
            List<ReglaAtributosValidos> ListaReglaAtributosValidos = new List<ReglaAtributosValidos>();
            ListaReglaAtributosValidos = db.Database.SqlQuery<ReglaAtributosValidos>
                ("execute spObtenerFrecuenciasEnvio @idFrecuencia",
                new SqlParameter("@idFrecuencia", 1)
                ).ToList();
            return ListaReglaAtributosValidos;

        }

        public List<ReglaAtributosValidos> ActualizarDatos(ReglaAtributosValidos pReglaAtributosValidos)
        {
            List<ReglaAtributosValidos> ListaReglaAtributosValidos = new List<ReglaAtributosValidos>();

            using (db = new SIMEFContext())
            {
                ListaReglaAtributosValidos = db.Database.SqlQuery<ReglaAtributosValidos>
                ("execute spActualizarReglaAtributosValidos @IdCompara,@IdDetalleReglaValidacion,@IdCategoria,@IdCategoriaAtributo, @OpcionEliminar",
                    new SqlParameter("@IdCompara", pReglaAtributosValidos.IdCompara),
                    new SqlParameter("@IdDetalleReglaValidacion", pReglaAtributosValidos.IdDetalleReglaValidacion),
                    new SqlParameter("@IdCategoria", pReglaAtributosValidos.IdCategoria),
                    new SqlParameter("@IdCategoriaAtributo", pReglaAtributosValidos.IdCategoriaAtributo),
                    new SqlParameter("@OpcionEliminar", pReglaAtributosValidos.OpcionEliminar==true?1:0)
                ).ToList();

                ListaReglaAtributosValidos = ListaReglaAtributosValidos.Select(X => new ReglaAtributosValidos
                {
                    IdCompara = X.IdCompara,
                    IdDetalleReglaValidacion = X.IdDetalleReglaValidacion,
                    IdCategoria = X.IdCategoria,
                    IdCategoriaAtributo = X.IdCategoriaAtributo

                }).ToList();

                return ListaReglaAtributosValidos;
            }
        }

    }
}
