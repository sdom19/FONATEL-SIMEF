using GB.SIMEF.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.DAL
{
    public class FrecuenciaEnvioDAL: BitacoraDAL
    {
        private SIMEFContext db;

        #region Metodos Consulta Base de Datos

        public List<FrecuenciaEnvio> ObtenerDatos(FrecuenciaEnvio objFrecuenciaEnvio)
        {
            List<FrecuenciaEnvio> ListaFrecuenciaEnvios = new List<FrecuenciaEnvio>();
            using (db = new SIMEFContext())
            {
                ListaFrecuenciaEnvios = db.Database.SqlQuery<FrecuenciaEnvio>
                    ("execute spObtenerFrecuenciasEnvio @idFrecuencia",
                    new SqlParameter("@idFrecuencia", objFrecuenciaEnvio.idFrecuencia)
                    ).ToList();
            }
            return ListaFrecuenciaEnvios;
        }

        #endregion
    }
}
