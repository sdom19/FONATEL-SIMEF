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
    public class DetalleIndicadorVariablesDAL : BitacoraDAL
    {
        private SIMEFContext db;

        /// <summary>
        /// 01/09/2022
        /// José Navarro Acuña
        /// Función que permite obtener los detalles variables dato de un indicador
        /// </summary>
        /// <param name="pDetalleIndicadorVariables"></param>
        /// <returns></returns>
        public List<DetalleIndicadorVariables> ObtenerDatos(DetalleIndicadorVariables pDetalleIndicadorVariables)
        {
            List<DetalleIndicadorVariables> listaDetalles = new List<DetalleIndicadorVariables>();

            using (db = new SIMEFContext())
            {
                listaDetalles = db.Database.SqlQuery<DetalleIndicadorVariables>
                    ("execute spObtenerDetallesIndicadorVariables @pIdDetalleIndicador,@pIdIndicador ",
                     new SqlParameter("@pIdDetalleIndicador", pDetalleIndicadorVariables.idDetalleIndicador),
                     new SqlParameter("@pIdIndicador", pDetalleIndicadorVariables.idIndicador)
                    ).ToList();
            }

            listaDetalles = listaDetalles.Select(x => new DetalleIndicadorVariables()
            {
                id = Utilidades.Encriptar(x.idDetalleIndicador.ToString()),
                NombreVariable = x.NombreVariable,
                Descripcion = x.Descripcion,
                Estado = x.Estado
            }).ToList();

            return listaDetalles;
        }
    }
}
