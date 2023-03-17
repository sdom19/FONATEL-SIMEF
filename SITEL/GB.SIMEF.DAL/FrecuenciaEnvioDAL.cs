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
    public class FrecuenciaEnvioDAL : BitacoraDAL
    {
        private SIMEFContext db;
        
        /// <summary>
        /// 23/08/2022
        /// José Navarro Acuña
        /// Función que retorna todos las frecuencias de envio registradas en estado activo.
        /// Se puede filtrar por el ID del objecto
        /// </summary>
        /// <returns></returns>
        public List<FrecuenciaEnvio> ObtenerDatos(FrecuenciaEnvio pFrecuenciaEnvio)
        {
            using (db=new SIMEFContext())
            {
                List<FrecuenciaEnvio> ListaFrecuenciaEnvios = new List<FrecuenciaEnvio>();
                ListaFrecuenciaEnvios = db.Database.SqlQuery<FrecuenciaEnvio>
                    ("execute pa_ObtenerFrecuenciaEnvio @idFrecuencia",
                    new SqlParameter("@idFrecuencia", pFrecuenciaEnvio.IdFrecuenciaEnvio)
                    ).ToList();

                ListaFrecuenciaEnvios = ListaFrecuenciaEnvios.Select(x => new FrecuenciaEnvio()
                {
                    id = Utilidades.Encriptar(x.IdFrecuenciaEnvio.ToString()),
                    IdFrecuenciaEnvio = x.IdFrecuenciaEnvio,
                    Nombre = x.Nombre,
                    CantidadDia = x.CantidadDia,
                    Estado = x.Estado
                }).ToList();
                return ListaFrecuenciaEnvios;
            }
          

        }
    }
}
