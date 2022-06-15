using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SUTEL.Reglas
{
    /// <summary>
    /// Clase que resuelve las reglas basados en:
    /// 1. Resuelve Reglas (Operaciones básicas)
    /// 2. Resuelve Formulas (Operaciones con condicionales)
    /// </summary>
    public class Motor
    {
        /// <summary>
        ///     Instancia del motor de reglas
        /// </summary>
        private ResultadoMatematico _Motor { get; set; }

        /// <summary>
        ///     Regla que va a ser compilada
        /// </summary>
        private string _reglaEval { get; set; }

        /// <summary>
        ///     Formula que va a ser compilada
        /// </summary>
        private string _formulaEval { get; set; }

        public string CodigoProcesar1 { get; set; }
        public string CodigoProcesar2 { get; set; }

        /// <summary>
        /// Este metodo recibe las formulas a resolver
        /// </summary>
        /// <param name="calculoRequest"></param>
        /// <returns></returns>
        public CalculoResponse Calcular(CalculoRequest calculoRequest)
        { 
            var response = new CalculoResponse
            {
                ReglasResult = new List<ReglasResult>(),
                FormulasResult = new List<FormulasResult>()
            };

            foreach (var regla in calculoRequest.Reglas)
                try //Por si se genera operación no controlada
                {
                    _Motor = new ResultadoMatematico { VariablesProcesos = calculoRequest.Variables };
                    _reglaEval = regla.Regla;
                    _Motor.Calcular(regla.Regla); //Se calcula la regla en el motor

                    if (string.IsNullOrEmpty(_Motor.TrazaCompilacion)) //Se verifica que el motor haya calculado Ok
                    {
                        //Respuesta del servicio
                        response.ReglasResult.Add(new ReglasResult
                        {
                            NombRegla = regla.NombreRegla,
                            ResultEvalRegla = _Motor.ResultadoEjecucion
                        }
                        );

                        //Se añade el resultado de la regla como nueva variable de proceso con el fin de
                        //resolver las formulas
                        calculoRequest.Variables.Add(new VariablesProceso
                        {
                            TipoDato = "Double", //Tipo de dato de resultado
                            NombVariable = regla.NombreRegla,
                            ValorVariable = _Motor.ResultadoEjecucion
                        });
                    }
                    else
                    {
                        response.Error = "Regla inválida " +_reglaEval + " " + _Motor.TrazaCompilacion;
                        response.CodProcesar = _Motor.CodigoProcesar;
                        response.CodC = _Motor.CodigoC;
                        _Motor.Dispose();
                        _Motor = null;
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    response.Error = "Regla inválida " + _reglaEval  + " " + ex.Message;
                    response.CodProcesar = _Motor.CodigoProcesar;
                    response.CodC = _Motor.CodigoC;
                    _Motor.Dispose();
                    _Motor = null;
                    return response; 
                }

            //Se procede a resolver las formulas
            foreach (var formula in calculoRequest.Formulas)
                try //Por si se genera operación no controlada
                {
                    _Motor = new ResultadoMatematico { VariablesProcesos = calculoRequest.Variables };
                    _formulaEval = formula.Formula;
                    _Motor.Calcular(formula.Formula); //Se calcula la regla en el motor

                    if (string.IsNullOrEmpty(_Motor.TrazaCompilacion)) //Se verifica que el motor haya calculado Ok
                    {
                        //Respuesta del servicio
                        response.FormulasResult.Add(new FormulasResult
                        {
                            NombFormula = formula.NombreFormula,
                            ResultEvalFormula = _Motor.ResultadoEjecucion
                        }
                        );
                    }
                    else
                    {
                        response.Error = "Formula inválida " + _formulaEval + " " + _Motor.TrazaCompilacion;
                        response.CodProcesar = _Motor.CodigoProcesar;
                        response.CodC = _Motor.CodigoC;
                        _Motor.Dispose();
                        _Motor = null;
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    response.Error = "Formula inválida " + _formulaEval + " " + _Motor.TrazaCompilacion;
                    response.CodProcesar = _Motor.CodigoProcesar;
                    response.CodC = _Motor.CodigoC;
                    _Motor.Dispose();
                    _Motor = null;
                    return response;
                }
            _Motor.Dispose();
            _Motor = null;
            return response;
        }

    }
}
