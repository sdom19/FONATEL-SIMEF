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
        public List<ReglaAtributoValido> ObtenerDatos(ReglaAtributoValido pReglaAtributosValidos)
        {
            List<ReglaAtributoValido> ListaReglaAtributosValidos = new List<ReglaAtributoValido>();
            ListaReglaAtributosValidos = db.Database.SqlQuery<ReglaAtributoValido>
                ("execute spObtenerFrecuenciasEnvio @idFrecuencia",
                new SqlParameter("@idFrecuencia", 1)
                ).ToList();
            return ListaReglaAtributosValidos;

        }

        public List<ReglaAtributoValido> ActualizarDatos(ReglaAtributoValido pReglaAtributosValidos)
        {
            List<ReglaAtributoValido> ListaReglaAtributosValidos = new List<ReglaAtributoValido>();

            using (db = new SIMEFContext())
            {
                ListaReglaAtributosValidos = db.Database.SqlQuery<ReglaAtributoValido>
                ("execute pa_ActualizarReglaAtributoValido @IdCompara,@IdDetalleReglaValidacion,@IdCategoria,@IdCategoriaAtributo, @OpcionEliminar",
                    new SqlParameter("@IdCompara", pReglaAtributosValidos.idReglaAtributosValido),
                    new SqlParameter("@IdDetalleReglaValidacion", pReglaAtributosValidos.IdDetalleReglaValidacion),
                    new SqlParameter("@IdCategoria", pReglaAtributosValidos.idCategoriaDesagregacion),
                    new SqlParameter("@IdCategoriaAtributo", pReglaAtributosValidos.idDetalleCategoriaTexto),
                    new SqlParameter("@OpcionEliminar", pReglaAtributosValidos.OpcionEliminar==true?1:0)
                ).ToList();

                ListaReglaAtributosValidos = ListaReglaAtributosValidos.Select(X => new ReglaAtributoValido
                {
                    idReglaAtributosValido = X.idReglaAtributosValido,
                    IdDetalleReglaValidacion = X.IdDetalleReglaValidacion,
                    idCategoriaDesagregacion = X.idCategoriaDesagregacion,
                    idDetalleCategoriaTexto = X.idDetalleCategoriaTexto

                }).ToList();

                return ListaReglaAtributosValidos;
            }
        }

    }
}
