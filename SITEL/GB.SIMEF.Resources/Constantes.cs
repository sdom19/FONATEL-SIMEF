using System;
using System.Collections.Generic;
using System.Drawing;
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
            EnvioSolicitud = 2,
            EnvioRegistroIndicadorEncargado = 3,
            EnvioRegistroIndicadorInformante = 4
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
            Eliminado = 4,
            Pendiente = 5,
            Completado = 6,
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
            Crear = 1,
            Consultar = 2,
            Editar = 3,
            Eliminar = 4,
            Clonar = 5,
            Activar = 6,
            Desactivar = 7,
            Descargar = 8,
            Visualizar = 9,
            EnviarSolicitud = 10,
            ProgramarEnvio = 11,
            EjecutarFormula = 12,
            Publicar = 13,
            DejarDePublicar = 14,
            EnviarCorreoInformante = 15,
            EnviarCorreoEncargado = 16
        }
        /// <summary>
        /// Enum Clasificación indicador
        /// </summary>
        public enum ClasificacionIndicadorEnum : int
        {
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

        public enum UnidadMedidaDefinicionFechasFormulasEnum : int
        {
            dias = 1,
            meses = 2,
            anios = 3
        }

        public enum TipoFechaDeficionFechasFormulasEnum : int
        {
            fecha = 1,
            categoriaDesagregacion = 2,
            fechaActual = 3
        }

        /// <summary>
        /// Enum para indicador que tipo de argumento se registra en la fórmula de cálculo dentro de la tabla de edición
        /// </summary>
        public enum FormulasTipoArgumentoEnum : int
        {
            VariableDatoCriterio = 1,
            DefinicionFecha = 2
        }

        /// <summary>
        /// Enum para clasificar de manera general la fórmula matemática
        /// </summary>
        public enum FormulasTipoObjetoEnum : int
        {
            Numero = 1,
            Operador = 2,
            Variable = 3
        }

        /// <summary>
        /// Enum que indica el tipo de porcentaje a seleccionar en la fórmula, para indicadores de calidad
        /// </summary>
        public enum TipoPorcentajeIndicadorCalculoEnum : int
        {
            indicador = 1,
            cumplimiento = 2
        }

        public enum FrecuenciaEnvioEnum : int
        {
            Semana = 2,
            Mes = 3,
            Quincenal = 4,
            Bimestre = 5,
            Trimestre = 6,
            Cuatrimestre = 7,
            Semestral = 8,
            Anual = 9
        }

        public enum TipoGraficoInformeEnum : int
        {
            Estandar = 0
        }

        /// <summary>
        /// Debido a la lógica de negocios, no se tienen registros fisicos respecto a los indicadores de calidad, sino columnas de una entidad en BD
        /// </summary>
        public struct CriteriosIndicadoresCalidad
        {
            public const string procentajeIndicador = "Porcentaje Indicador";
            public const string procentajeCumplimiento = "Porcentaje de Cumplimiento";
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

        public struct ParametrosReglaValidacion
        {
            public const string Solicitud = "Solicitud";
            public const string Formulario = "Formulario";
            public const string Indicador = "Indicador";
        }

        public static readonly string ParametrosReglaValidacionDispatch = "Unico";
        public static readonly string ParametrosReglaValidacionPeriodicity = "Solo";

        public static readonly Color headColorFromHex = System.Drawing.ColorTranslator.FromHtml("#2f75b5");
        public static readonly Color fontColorFromHex = System.Drawing.ColorTranslator.FromHtml("#fff");
        public static readonly Color grayColorFromHex = System.Drawing.ColorTranslator.FromHtml("#e7e6e6");
        public static readonly Color greenColorFromHex = System.Drawing.ColorTranslator.FromHtml("#e2efda");
        public static readonly Color greenColorFromHex1 = System.Drawing.ColorTranslator.FromHtml("#f7f7f7");
        public static readonly string Rutalogo = "Content\\Images\\logos\\logo-Sutel_11_3.png";
        public static readonly string Nombrelogo = "SUTEL";

        public struct RespuestaEstadoReglasValidacion
        {
            public const string Finalizado = "Finalizado";
            public const string Detenido = "Detenido";
        }

        public readonly static string defaultInputTextValue = "No definido";
        public readonly static int defaultInputNumberValue = 0;
        public readonly static int defaultDropDownValue = 1; // Representa el valor: "Sin definir". En Utilidades.cs existe un método para encriptarlo

        public readonly static string select2MultipleOptionTodosText = "Todos";
        public readonly static string select2MultipleOptionTodosValue = "all";

        public readonly static string keyModoFormulario = "modoFormulario";

        public readonly static string RolConsultasFonatel = "Consultas Fonatel";
        public readonly static string RedirectActionConsultasFonatel = "Index";

        public readonly static string Dispatch_Task = "Tarea";
        public readonly static string Dispatch_Unique = "Unico";
        public readonly static string Periodicity_Unique = "Solo";

        public static Dictionary<FrecuenciaEnvioEnum, string> mapFrecuenciasConMotor = new Dictionary<FrecuenciaEnvioEnum, string>() {
            { FrecuenciaEnvioEnum.Semana, "Semanal"},
            { FrecuenciaEnvioEnum.Mes, "Mensual"},
            { FrecuenciaEnvioEnum.Quincenal, "Quincenal"},
            { FrecuenciaEnvioEnum.Bimestre, "Bimensual"},
            { FrecuenciaEnvioEnum.Trimestre, "Trimestral"},
            { FrecuenciaEnvioEnum.Cuatrimestre, "Cuatrimestral"},
            { FrecuenciaEnvioEnum.Anual, "Anual"}
        };

        public static Dictionary<EstadosRegistro, string> mapEstadoFormulaConMotor = new Dictionary<EstadosRegistro, string>() {
            { EstadosRegistro.EnProceso, "Detenido"},
            { EstadosRegistro.Pendiente, "Detenido"},
            { EstadosRegistro.Activo, "Nuevo"},
            { EstadosRegistro.Desactivado, "Detenido"},
            { EstadosRegistro.Eliminado, "Detenido"}
        };
    }
}
