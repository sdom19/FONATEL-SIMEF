using GB.SIMEF.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.DAL
{
    public class IndicadorDAL : BitacoraDAL
    {
        private SIMEFContext db;

        #region Metodos Consulta Base de Datos
        public List<Indicador> ObtenerDatos(Indicador objIndicador)
        {
            List<Indicador> ListaIndicadores = new List<Indicador>();
            using (db = new SIMEFContext()) 
            {
                ListaIndicadores = db.Database.SqlQuery<Indicador>
                    ("execute spObtenerIndicadores @idIndicador, @codigo, @idEstado, @idTipoIndicador",
                    new SqlParameter("@idIndicador", objIndicador.idIndicador),
                    new SqlParameter("@codigo", string.IsNullOrEmpty(objIndicador.Codigo) ? DBNull.Value.ToString() : objIndicador.Codigo),
                    new SqlParameter("@idEstado", objIndicador.idEstado),
                    new SqlParameter("@idTipoIndicador", objIndicador.idTipoIndicador)
                    ).ToList();
            }
            return ListaIndicadores;
        }
        
        #endregion
    }
}
