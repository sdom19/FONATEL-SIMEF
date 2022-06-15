using System;

namespace GB.SUTEL.Reglas
{
    public class VariablesProceso
    {
        /// <summary>
        /// Tipo de dato que se asignará a una variable
        /// </summary>
        public string TipoDato { get; set; }
        /// <summary>
        /// Nombre de la variable
        /// </summary>
        public string NombVariable { get; set; }

        /// <summary>
        /// Valor asignado a la variable
        /// </summary>
        public string ValorVariable { get; set; }

        /// <summary>
        /// Retorna la variable en el formato que la requiere el compilador dinámico
        /// </summary>
        /// <returns></returns>
        public string SalidaVariable()
        {
            string salida = TipoDato + " " + NombVariable + " = ";

            //Formato adicional para las variables
            switch (TipoDato)
            {
                case "String":
                    salida = salida + "\"" + ValorVariable + "\"";
                    break;
                case "DateTime":
                    salida = salida + "Convert.ToDateTime(\"" + ValorVariable + "\")";
                    break;
                case "Decimal":
                    salida = salida + ValorVariable + "M";
                    break;
                    ;
                case "Double[]":
                    salida = salida + "new Double[] {" + ValorVariable + "}";
                    break;
                default:
                    salida = salida + ValorVariable;
                    break;
            }

            return salida + ";";
        }
    }
}
