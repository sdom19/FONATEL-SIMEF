using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SIMEF.Resources
{
    public static class Constantes
    {
        /// <summary>
        /// Tipos de Categorías
        /// </summary>
        public enum TipoDetalleCategoria : int
        {
            Numerico=1,
            Alfanumerico=2,
            Texto=3,
            Fecha=4
        }
        /// <summary>
        /// Estados de los registros
        /// </summary>
        public enum EstadosRegistro : int
        {
            EnProceso = 1,
            Activo = 2,
            Desactivado = 3,
            Eliminado = 4
        }
        public enum Error : int
        {
            NoError = 0,
            ErrorSistema = 1,
            ErrorControlado = 2
        }


        public enum Accion : int
        {
            Eliminar = 4,
        }
    }
}
