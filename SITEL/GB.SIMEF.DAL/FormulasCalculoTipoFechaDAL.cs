using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using System.Collections.Generic;
using System.Linq;

namespace GB.SIMEF.DAL
{
    public class FormulasCalculoTipoFechaDAL
    {
        private SIMEFContext db;

        /// <summary>
        /// 09/01/2023
        /// José Navarro Acuña
        /// Función que retorna todos los tipos indicadores registrados en estado activo.
        /// Se puede filtrar por el ID del objecto
        /// </summary>
        /// <returns></returns>
        public List<FormulaCalculoTipoFecha> ObtenerDatos()
        {
            List<FormulaCalculoTipoFecha> listaTipoIndicadores = new List<FormulaCalculoTipoFecha>();

            using (db = new SIMEFContext())
            {
                listaTipoIndicadores = db.FormulaCalculoTipoFecha.Where(x => x.Estado == true).ToList();

                listaTipoIndicadores = listaTipoIndicadores.Select(x => new FormulaCalculoTipoFecha()
                {
                    id = Utilidades.Encriptar(x.IdFormulaCalculoTipoFecha.ToString()),
                    IdFormulaCalculoTipoFecha = x.IdFormulaCalculoTipoFecha,
                    Nombre = x.Nombre,
                    Estado = x.Estado
                }).ToList();
            }

            return listaTipoIndicadores;
        }
    }
}
