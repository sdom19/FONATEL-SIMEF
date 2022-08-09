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
        public enum TipoDetalleCategoriaEnum : int
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
            Consultar=2,
            Eliminar = 4,
        }

        public struct CifradoDatos
        {
            public const String strPermutacion = "ouiveyxaqtd";
            public const Int32 intBytePermutacionUno = 0x19;
            public const Int32 intBytePermutacionDos = 0x59;
            public const Int32 intBytePermutacionTres = 0x17;
            public const Int32 intBytePermutacionCuatro = 0x41;
            public const int intDivisionPassword = 8;
        }


    }
}
