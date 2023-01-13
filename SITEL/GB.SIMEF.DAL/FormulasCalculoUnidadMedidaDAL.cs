using GB.SIMEF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.DAL
{
    public class FormulasCalculoUnidadMedidaDAL
    {
        private SIMEFContext db;

        /// <summary>
        /// 12/01/2023
        /// José Navarro Acuña
        /// Función que permite obtener un listado de las unidad de medida de la deficion de fecha en estado activo
        /// </summary>
        /// <returns></returns>
        public List<FormulasCalculoUnidadMedida> ObtenerDatos()
        {
            return db.FormulasCalculoUnidadMedida.Where(x => x.Estado == true).ToList();
        }
    }
}
