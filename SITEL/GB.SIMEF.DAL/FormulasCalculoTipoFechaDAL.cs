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
        public List<FormulasCalculoTipoFecha> ObtenerDatos()
        {
            List<FormulasCalculoTipoFecha> listaTipoIndicadores = new List<FormulasCalculoTipoFecha>();

            using (db = new SIMEFContext())
            {
                listaTipoIndicadores = db.FormulasCalculoTipoFecha.Where(x => x.Estado == true).ToList();

                listaTipoIndicadores = listaTipoIndicadores.Select(x => new FormulasCalculoTipoFecha()
                {
                    id = Utilidades.Encriptar(x.IdTipoFecha.ToString()),
                    IdTipoFecha = x.IdTipoFecha,
                    Nombre = x.Nombre,
                    Estado = x.Estado
                }).ToList();
            }

            return listaTipoIndicadores;
        }
    }
}
