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
        /// 
        /// </summary>

        public enum TipoCategoriaEnum : int
        {
            IdUnico = 1,
            Atributo = 2,
            Actualizable = 3,
            VariableDato=4
        }

        public enum PlantillaCorreoEnum : int
        {
            CrearUsuario = 1,
            EnvioSolicitud = 2
        }


        /// <summary>
        /// Tipos de Categorías
        /// </summary>
        public enum TipoDetalleCategoriaEnum : int
        {
            Numerico = 1,
            Alfanumerico = 2,
            Texto = 3,
            Fecha = 4
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
        /// <summary>
        /// TiposReglasDetalle
        /// </summary>
        /// 
        public enum TipoReglasDetalle : int
        {
            NoRegistrado = 0,
            FormulaCambioMensual = 1,
            FormulaContraOtroIndicadorEntrada = 2,
            FormulaContraConstante = 3,
            FormulaContraAtributosValidos = 4,
            FormulaActualizacionSecuencial = 5,
            FormulaContraOtroIndicadorSalida = 6,
            FormulaContraOtroIndicadorEntradaSalida = 7
        }


        public enum Error : int
        {
            NoError = 0,
            ErrorSistema = 1,
            ErrorControlado = 2
        }


        public enum Accion : int
        {
            Insertar = 1,
            Consultar = 2,
            Editar = 3,
            Eliminar = 4,
            Clonar = 5,
            Activar = 6,
            Inactiva = 7,
            Descargar = 8,
            Visualizar = 9,
            EnviarSolicitud = 10,
            ProgramarEnvio = 11,
            EjecutarFormula = 12,
            Publicado = 13,
            NoPublicado = 14
        }
        /// <summary>
        /// Enum Clasificación indicador
        /// </summary>
        public enum ClasificacionIndicadorEnum : int
        {
            SinDefinir = 1,
            Entrada = 4,
            Salida = 2,
            EntradaSalida = 3
        }

        public enum FuenteIndicadorEnum : int
        {
            IndicadorDGF = 1,
            IndicadorDGM = 2,
            IndicadorDGC = 3,
            IndicadorUIT = 4,
            IndicadorCruzado = 5,
            IndicadorFuenteExterna = 6
        }

        public enum UnidedMedidaDefinicionFechasFormulas : int
        {
            dias = 1,
            meses = 2,
            anios = 3
        }

        /// <summary>
        /// Enum para indicador que tipo de argumento se registra en la fórmula de cálculo dentro de la tabla de edición
        /// </summary>
        public enum FormulasTipoArgumento : int
        {
            VariableDatoCriterio = 1,
            DefinicionFecha = 2
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
        public struct EstructuraHtmlRegistroIndicador
        {
            public const string NumeroLinea = "<th style='min-width:30PX'>  </th>";
            public const string Variable = "<th style='min-width:100PX' class='highlighted' scope='col'>{0}</th>";
            public const string InputAlfanumerico = "<td><input type='text' name='name_{1}' aria-label='{0}' class='form-control form-control-fonatel alfa_numerico' id='[0]-{1}' placeholder='{0}' style='min-width:150px;'></td>";
            public const string InputTexto = "<td><input type='text' name='name_{1}' aria-label='{0}' class='form-control form-control-fonatel solo_texto' id='[0]-{1}' placeholder='{0}' style='min-width:150px;'></td>";
            public const string InputFecha = "<td><input type='date' name='name_{0}' class='form-control form-control-fonatel' id='[0]-{0}' min='{1}' max='{2}'></td>";
            public const string InputNumerico = "<td><input type='number' name='name_{0}' class='form-control form-control-fonatel solo_numeros' id='[0]-{0}' min='{1}' max='{2}'></td>";
            public const string InputSelect = "<td><div class='select2-wrapper'><select class='listasDesplegables' id='[0]-{0}' name='name_{0}' ><option></option>{1}</select ></div ></td>";

        }


        /// <summary>
        /// Usos del indicador
        /// </summary>
        public struct UsosIndicador
        {
            public const string interno = "Interno";
            public const string externo = "Externo";
        }

        /// <summary>
        /// Bandera que indica si se muestra el indicador en la solicitud
        /// </summary>
        public struct MostrarIndicadorEnSolicitud
        {
            public const string si = "SI";
            public const string no = "NO";
        }

        public readonly static string defaultInputTextValue = "No definido";
        public readonly static int defaultInputNumberValue = 0;
        public readonly static int defaultDropDownValue = 1; // Representa el valor: "Sin definir". En Utilidades.cs existe un método para encriptarlo

        public readonly static string select2MultipleOptionTodosText = "Todos";
        public readonly static string select2MultipleOptionTodosValue = "all";

        public readonly static string keyModoFormulario = "modoFormulario";
    }
}
