using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SUTEL.BL.FormCumplimientoPorcenBL;
using GB.SUTEL.Entities.FormCumplimientoPorcenEnti;
using System.Data;
using System.Globalization;

namespace GB.SUTEL.Reglas
{
    public class Orquestador
    {
        //public UtilMotor oUtil;
        /// <summary>
        /// Este metodo se escarga de validar si se deben procesar calculos de formulas
        /// </summary>
        public void EjecutarMotor()
            
        {

            try
            {
                var cultureInfo = new CultureInfo("es-CO") { NumberFormat = { NumberDecimalSeparator = "." } };
                CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            }
            catch (Exception)
            {
            }

            UtilMotor.WriteToFile("Inicia método EjecutarMotor");
            int anio, idEjcucion;
            Int16 periodo;
            DateTime fechaEjecucion;
            FormCumplimientoPorcenBL ctrlEjecuciones = new FormCumplimientoPorcenBL();
            DataTable dtResult = ctrlEjecuciones.AdmEjecucionesMotor(2, 0, "", 0, DateTime.Now, 0);
            bool ConRegistros = false;
            if (dtResult != null)
            {
                if (dtResult.Rows.Count==0)
                    UtilMotor.WriteToFile("Orquestador.EjecutarMotor No se encontraron programaciones para procesar");
                foreach (DataRow item in dtResult.Rows)
                {
                    anio = Convert.ToInt32(item["anioEjecucion"].ToString());
                    periodo = Convert.ToInt16(item["idPeriodoEjecucion"].ToString());
                    idEjcucion = Convert.ToInt32(item["idejecucion"].ToString());
                    fechaEjecucion = Convert.ToDateTime(item["FechaRegistro"].ToString());
                    if (DateTime.Now.Date.ToShortDateString().Replace('/', '_').Equals(fechaEjecucion.ToShortDateString().Replace('/', '_')))
                    {
                        ConRegistros = true; //Bandera para indicar que se jecuten todos los periodos anuales
                        UtilMotor.WriteToFile("Orquestador.EjecutarMotor Inicia procesamiento de " + anio.ToString() + " periodo " + periodo);
                        ProcesarFormulacion(anio, periodo);
                        ProcesarFormulacion(anio, 5, periodo);

                        var rta = ctrlEjecuciones.AdmEjecucionesMotor(5, 0, "", 0, DateTime.Now, idEjcucion); //Se marca como ejecutado el periodo
                        UtilMotor.WriteToFile("Orquestador.EjecutarMotor Fin procesamiento de " + anio.ToString() + " periodo " + periodo);
                    }
                    else
                        UtilMotor.WriteToFile("Orquestador.EjecutarMotor No existen registros para esta fecha");
                }
            }
            else {
                UtilMotor.WriteToFile("Orquestador.EjecutarMotor.ctrlEjecuciones.AdmEjecucionesMotor nulo");
            }
        }

        public void ProcesarFormulacion(int anio, Int16 periodo, Int16 periodoPadre=0)
        {

            double sumaMeses = 0, mes1 = 0, mes2 = 0, mes3 = 0;
            var blFormulacion = new FormCumplimientoPorcenBL();
            int IdFormulaEvaluar = 0; //Id de la formula que se está evaluando
            int IdPeriodo = periodo; //Esto se debe cambiar
            int mesIniTrimestre = 1;
            switch (periodo)
            {
                case 2: //Trimestre 2
                    mesIniTrimestre = 4;
                    break;
                case 3: //Trimestre 3
                    mesIniTrimestre = 7;
                    break;
                case 4: //Trimestre 4
                    mesIniTrimestre = 10;
                    break;
            }

            string[] criterios, criteriosFormCumpIf, criteriosFormCumpTrue, criteriosFormCumpFalse,
                criteriosFormPorc, consolidadoCriterios;
            StringBuilder stbLog = new StringBuilder();

            //Se leen todas las formulas a procesar (Formato cola)
            //>>Ejecuión de trimestrales
            string filtroPeriodicidad = periodo != 5 ? "TRIMESTRAL" : "ANUAL";
            var dtFormulacionTrimestral = blFormulacion.ConsFormulasPeriocidad(filtroPeriodicidad);

            if (dtFormulacionTrimestral == null)
                stbLog.AppendLine(DateTime.Now.ToString() + " - Consulta de formulas fue procesada con error");
            if (dtFormulacionTrimestral != null && dtFormulacionTrimestral.Rows.Count == 0)
                stbLog.AppendLine(DateTime.Now.ToString() + " - Sin formulas para procesar");
            foreach (DataRow itemFormulas in dtFormulacionTrimestral.Rows)
            {
                var RequestMotor = new CalculoRequest();
                try
                {
                    string indicador = "";
                    int IdServicio = Convert.ToInt32(itemFormulas["IdServicio"].ToString());
                    IdFormulaEvaluar = Convert.ToInt32(itemFormulas["IdParamFormulas"].ToString());
                    criterios = itemFormulas["Criterios"].ToString().Split(',');
                    criteriosFormPorc = itemFormulas["FromArray"].ToString().Split(',');
                    criteriosFormCumpIf = itemFormulas["ArrayIf"].ToString().Split(',');
                    criteriosFormCumpTrue = itemFormulas["ArrayVerdadero"].ToString().Split(',');
                    criteriosFormCumpFalse = itemFormulas["ArrayFalso"].ToString().Split(',');
                    var consolidadoCriteriosForm = criteriosFormPorc.Concat(criteriosFormCumpIf).ToArray().Concat(criteriosFormCumpTrue).ToArray().Concat(criteriosFormCumpFalse);
                    var criteriosBusq = consolidadoCriteriosForm.Where(i => criterios.Contains(i) && !string.IsNullOrEmpty(i));

                    //Se obtienen los valores que aplican para estas formulas
                    var valoresGralFormulacion = blFormulacion.ConsValoresFormulacion(Convert.ToInt16(IdPeriodo), IdServicio, IdFormulaEvaluar, anio);

                    string[] selectedColumns = new[] { "IdOperador", "DescDetalleAgrupacion" };
                    var copiaDistinct = valoresGralFormulacion.Copy();
                    foreach (DataRow item in copiaDistinct.Rows)
                    {
                        //string st1, st2;
                        //st1 = item["DescDetalleAgrupacion"].ToString();
                        item["DescDetalleAgrupacion"] = (item["DescDetalleAgrupacion"].ToString() != "2g" &&
                            item["DescDetalleAgrupacion"].ToString() != "3g" && item["DescDetalleAgrupacion"].ToString() != "4g") ? "SIN AGRUPAR" : item["DescDetalleAgrupacion"].ToString();
                    }

                    DataTable OperadoresyAgrupacion = new DataView(copiaDistinct).ToTable(true, selectedColumns);

                    if (OperadoresyAgrupacion == null)
                        UtilMotor.WriteToFile("Consulta de valores por formulación errada " + IdFormulaEvaluar.ToString());

                    if (OperadoresyAgrupacion != null && OperadoresyAgrupacion.Rows.Count == 0)
                    UtilMotor.WriteToFile("No hay valores disponibles para la formula " + IdFormulaEvaluar.ToString());

                    //Se va resolviendo la formulación por operador
                    foreach (DataRow itemOperador in OperadoresyAgrupacion.Rows)//.Select("DescDetalleAgrupacion = '2g'")
                    {
                        RequestMotor = new CalculoRequest()
                        {
                            Variables = new List<VariablesProceso>(), //Se inicializan las lista par aluego se usadas
                            Reglas = new List<Reglas>(),
                            Formulas = new List<Formulas>()
                        };

                        var datosResultTuplas = new List<FacReglasReport>();

                        string idOperadorEvaluar = itemOperador["IdOperador"].ToString();
                        string agrupacion = itemOperador["DescDetalleAgrupacion"].ToString();
                        var itemValores = valoresGralFormulacion.Select("IdOperador= '" + idOperadorEvaluar + "'").First();
                        //"' and DescDetalleAgrupacion = '" + agrupacion + "'").First();
                        //Se asigna el valor de cada cada criterio
                        foreach (string citerioEvaluar in criteriosBusq.Distinct())
                        {
                            var DatosResult = new FacReglasReport();
                            //Se asignan varibles complementarias
                            if (agrupacion == "2g" || agrupacion == "3g" || agrupacion == "4g")
                                itemValores = valoresGralFormulacion.Select("IdOperador= '" + idOperadorEvaluar +
                                                "' and DescDetalleAgrupacion = '" + agrupacion + "'" +
                                                " and IdCriterio = '" + citerioEvaluar + "'").First();
                            else
                                itemValores = valoresGralFormulacion.Select("IdOperador= '" + idOperadorEvaluar + "' " +
                                            " and IdCriterio = '" + citerioEvaluar + "'").First();

                            DatosResult.IdServicio = Convert.ToInt32(itemValores["IdServicio"].ToString());
                            DatosResult.Servicio = itemValores["DesServicio"].ToString();
                            DatosResult.IdTipoInd = Convert.ToInt32(itemValores["IdTipoInd"].ToString());
                            DatosResult.TipoIndicador = itemValores["DesTipoInd"].ToString();
                            DatosResult.IdIndicador = itemValores["IdIndicador"].ToString();
                            DatosResult.NombreIndicador = itemValores["NombreIndicador"].ToString();
                            DatosResult.IdCriterio = citerioEvaluar;
                            DatosResult.NombreCriterio = itemValores["DescCriterio"].ToString();
                            DatosResult.Agrupacion = itemValores["IdAgrupacion"].ToString();
                            DatosResult.DetalleAgrupacion = itemValores["DescDetalleAgrupacion"].ToString();
                            DatosResult.Frecuencia = itemValores["Frecuencia"].ToString();
                            DatosResult.IdParamFormulas = Convert.ToInt32(itemValores["IdParamFormulas"].ToString());
                            DatosResult.Umbral = Convert.ToDecimal(itemValores["Umbral"].ToString());
                            DatosResult.FactorRigurosidad = Convert.ToDecimal(itemValores["FactorRigurosidad"].ToString());
                            DatosResult.Peso = Convert.ToDecimal(itemValores["PesoRelativo"].ToString());
                            DatosResult.ResultCumplPeso = DatosResult.ValFormulaCalcCump * DatosResult.Peso;
                            DatosResult.IdOperador = itemValores["IdOperador"].ToString();
                            DatosResult.NombreOperador = itemValores["NombreOperador"].ToString();
                            DatosResult.AnioProcesado = anio;
                            DatosResult.PeriodoEjec = periodoPadre==0? Convert.ToInt16(IdPeriodo): periodoPadre;

                            sumaMeses = 0; mes1 = 0; mes2 = 0; mes3 = 0;
                            if (!string.IsNullOrEmpty(citerioEvaluar))
                            {
                                if (periodo != 5) //Ejecución anual
                                {
                                    UtilMotor.WriteToFile("Obteniendo valores por criterios mensual - Formula" + IdFormulaEvaluar.ToString());
                                    DatosResult.Desglose = "Mensual";
                                    for (int mesIni = mesIniTrimestre; mesIni < mesIniTrimestre + 3; mesIni++)
                                    {
                                        //Se consulta el mes 1
                                        StringBuilder stbQuery = new StringBuilder();
                                        if (agrupacion != "2g" && agrupacion != "3g" && agrupacion != "4g")
                                        {
                                            stbQuery.Append("IdOperador= '" + idOperadorEvaluar + "'");
                                            stbQuery.Append(" and NumeroDesglose = " + mesIni.ToString());
                                            stbQuery.Append(" and IdCriterio = '" + citerioEvaluar + "'");
                                        }
                                        else
                                        {
                                            stbQuery.Append("IdOperador= '" + idOperadorEvaluar + "'");
                                            stbQuery.Append(" and NumeroDesglose = " + mesIni.ToString());
                                            stbQuery.Append(" and IdCriterio = '" + citerioEvaluar + "'");
                                            stbQuery.Append(" and DescDetalleAgrupacion = '" + agrupacion + "'");
                                        }

                                        var valor = valoresGralFormulacion.Select(stbQuery.ToString());
                                        if (valor != null && valor.Length > 0)
                                        {
                                            if (mesIni == mesIniTrimestre)
                                                mes1 = Convert.ToDouble(valor.First()["valor"].ToString());
                                            else if (mesIni == mesIniTrimestre + 1)
                                                mes2 = Convert.ToDouble(valor.First()["valor"].ToString());
                                            else
                                                mes3 = Convert.ToDouble(valor.First()["valor"].ToString());
                                        }
                                    }
                                }
                                else
                                {
                                    UtilMotor.WriteToFile("Obteniendo valores por criterios anual - Formula" + IdFormulaEvaluar.ToString());
                                    StringBuilder stbQuery = new StringBuilder();
                                    stbQuery.Append("IdOperador= '" + idOperadorEvaluar + "'");
                                    stbQuery.Append(" and IdCriterio = '" + citerioEvaluar + "'");
                                    DatosResult.Desglose = "Anual";
                                    var valor = valoresGralFormulacion.Select(stbQuery.ToString());
                                    if (valor != null && valor.Length > 0)
                                    {
                                            mes1 = Convert.ToDouble(valor.First()["valor"].ToString());
                                            mes2 = 0;
                                            mes3 = 0;
                                    }
                                }

                                
                                sumaMeses = mes1 + mes2 + +mes3;
                                DatosResult.ValMes1 = Convert.ToInt64(mes1);
                                DatosResult.ValMes2 = Convert.ToInt64(mes2);
                                DatosResult.ValMes3 = Convert.ToInt64(mes3);

                                //Se asignan los valores a las variables (Tener en cuenta que esto es una lista)
                                RequestMotor.Variables.Add(new VariablesProceso()
                                {
                                    NombVariable = citerioEvaluar.Replace(".", "_"),
                                    TipoDato = "Double",
                                    ValorVariable = sumaMeses.ToString()
                                });
                            }
                            datosResultTuplas.Add(DatosResult);
                        }
                        indicador = itemValores["IdIndicador"].ToString();
                        RequestMotor.Variables.Add(new VariablesProceso()
                        {
                            NombVariable = "UM",
                            TipoDato = "Double",
                            ValorVariable = itemValores["Umbral"].ToString().Replace(",",".")
                        });

                        //Se asignan las reglas a resolver (Tener en cuenta que esto es una lista)
                        RequestMotor.Reglas.Add(new Reglas()
                        {
                            NombreRegla = "FP",
                            Regla = "answer = " + itemFormulas["FormulaPorcentaje"].ToString().Replace(".", "_")
                        });

                        //Calculo del factor de rigorosidad ()
                        var instanciaMotor = new Motor();
                        decimal FP = 0, FR = 0;
                        CalculoResponse responseFp = instanciaMotor.Calcular(RequestMotor);
                        byte procesoFpError = 0;
                        if (string.IsNullOrEmpty(responseFp.Error))
                        {
                            FP = Convert.ToDecimal(responseFp.ReglasResult[0].ResultEvalRegla);
                        }
                        else {
                            UtilMotor.WriteToFile("Formula de procentaje no procesada correctamente -" + responseFp.Error + " - Formula " + IdFormulaEvaluar);
                            UtilMotor.WriteToFile("---------------------------------------");
                            UtilMotor.WriteToFile(responseFp.CodProcesar);
                            UtilMotor.WriteToFile("---------------------------------------");
                            UtilMotor.WriteToFile(responseFp.CodC);
                        }
                        DataTable dtCalcFp = blFormulacion.CalculosFr(itemValores["IdIndicador"].ToString(), Convert.ToInt16(IdPeriodo), idOperadorEvaluar,
                             FP, Convert.ToDecimal(itemValores["Umbral"].ToString().Replace(",", ".")), anio);
                        if (dtCalcFp != null && dtCalcFp.Rows.Count > 0)
                        {
                            FR = Convert.ToDecimal(dtCalcFp.Rows[0]["FactRigurosidad"].ToString().Replace(",", "."));
                            procesoFpError = 1;
                        }
                        RequestMotor.Variables.Remove(RequestMotor.Variables.First(v => v.NombVariable == "FP"));

                        RequestMotor.Variables.Add(new VariablesProceso()
                        {
                            NombVariable = "FR",
                            TipoDato = "Double",
                            ValorVariable = FR.ToString().Replace(",", ".")
                        });


                        //Se asignan las formulas a resolver (Tener en cuenta que esto es una lista)
                        StringBuilder stbCumplimiento = new StringBuilder();
                        string[] formula = itemFormulas["FormulaCumplimiento"].ToString().Split(';');
                        stbCumplimiento.AppendLine(formula[0].Replace("SI(", "if (") + ") {");
                        stbCumplimiento.AppendLine(" answer = " + formula[1].ToString() + ";");
                        stbCumplimiento.AppendLine(" } else {");
                        stbCumplimiento.AppendLine(" answer = " + formula[2].ToString().Substring(0, formula[2].ToString().Length - 1) + ";"); //Se quita el parentesis derecho final
                        stbCumplimiento.AppendLine(" }");
                        RequestMotor.Formulas.Add(new Formulas()
                        {
                            NombreFormula = "FC",
                            Formula = stbCumplimiento.ToString()
                        });

                        instanciaMotor = new Motor();
                        CalculoResponse response = instanciaMotor.Calcular(RequestMotor);
                        if (string.IsNullOrEmpty(response.Error))
                        {

                            //UtilMotor.WriteToFile("Detalle formula procesada -" + response.Error + " - Formula " + IdFormulaEvaluar);
                            //UtilMotor.WriteToFile("---------------------------------------");
                            //UtilMotor.WriteToFile(response.CodProcesar);
                            //UtilMotor.WriteToFile("---------------------------------------");
                            //UtilMotor.WriteToFile(response.CodC);

                            if (agrupacion != "SIN AGRUPAR")
                                datosResultTuplas.Where(d => d.DetalleAgrupacion == agrupacion).All(d =>
                                {
                                    d.ValFormulaCalcPorc = Convert.ToDecimal(response.ReglasResult[0].ResultEvalRegla);
                                    d.ValFormulaCalcCump = Convert.ToDecimal(response.FormulasResult[0].ResultEvalFormula);
                                    d.ResultCumplPeso = d.ValFormulaCalcCump * d.Peso;
                                    return true;
                                });
                            else
                                datosResultTuplas.All(d =>
                                {
                                    d.ValFormulaCalcPorc = Convert.ToDecimal(response.ReglasResult[0].ResultEvalRegla);
                                    d.ValFormulaCalcCump = Convert.ToDecimal(response.FormulasResult[0].ResultEvalFormula);
                                    d.ResultCumplPeso = d.ValFormulaCalcCump * d.Peso;
                                    return true;
                                });

                            foreach (var dRegistro in datosResultTuplas)
                            {
                                UtilMotor.WriteToFile("Orquestador.ProcesarFormulacion - Registrar en tabla [FacReglasReport] - Formula " + IdFormulaEvaluar); 
                                blFormulacion.admFacReglasReport(1, dRegistro);
                            }
                            sumaMeses = 0;
                        }
                        else
                        {
                            //Loguear error
                            UtilMotor.WriteToFile("Formula de cálculo porcentaje y cumplimiento no cálculada -" + response.Error + " - Formula " + IdFormulaEvaluar);
                            UtilMotor.WriteToFile("---------------------------------------");
                            UtilMotor.WriteToFile(response.CodProcesar);
                            UtilMotor.WriteToFile("---------------------------------------");
                            UtilMotor.WriteToFile(response.CodC);
                        }
                        UtilMotor.WriteToFile("Orquestador.ProcesarFormulacion - CálculosTecnologías - Formula " + IdFormulaEvaluar); 
                        //Actualizar tecnologías aquí
                        blFormulacion.CalculosTecnologias(periodoPadre==0? Convert.ToInt16(periodo) : periodoPadre, anio, idOperadorEvaluar);
                    }//Item operador

                }
                catch (Exception ex)
                {
                    UtilMotor.WriteToFile("Orquestador.ProcesarFormulacion - Formula " + IdFormulaEvaluar + " procesada con errores fatales: " + ex.Message + " - " + ex.StackTrace);
                }
            }
            UtilMotor.WriteToFile("Orquestador.ProcesarFormulacion - Método finalizado");  
        }

    }
}
