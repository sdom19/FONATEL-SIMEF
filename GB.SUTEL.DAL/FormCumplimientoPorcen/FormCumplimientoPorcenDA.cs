using GB.SUTEL.Entities;
using GB.SUTEL.Entities.FormCumplimientoPorcenEnti;
using GB.SUTEL.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SUTEL.DAL.FormCumplimientoPorcenDA
{
    public class FormCumplimientoPorcenDA : LocalContextualizer
    {

        #region atributos
        private SUTEL_IndicadoresEntities context;
        #endregion

        #region constructor
        public FormCumplimientoPorcenDA(ApplicationContext appContext)
            : base(appContext)
        {
            context = new SUTEL_IndicadoresEntities();

        }


        #endregion
        /// <summary>
        /// Metodo para consultar los criterios pertenecientes a un indicador
        /// </summary>
        /// <param name="IdIndicador">IdIndicador</param>
        /// <returns>Retorna un objeto lista Criterio</returns>
        public List<FormCumplimientoPorcenEnti> LisCreteriosXIndicador(string IdIndicador)
        {
            var Liscriterio = new List<FormCumplimientoPorcenEnti>();
            try
            {

                DataSet retVal = new DataSet();
                SqlConnection sqlConn = (SqlConnection)new SUTEL_IndicadoresEntities().Database.Connection;
                SqlCommand cmdReport = new SqlCommand("pa_GetCriteriosXIndicador", sqlConn);
                SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                using (cmdReport)
                {
                    cmdReport.CommandType = CommandType.StoredProcedure;

                    SqlParameter Opcion = new SqlParameter("IdIndicador", IdIndicador);
                    cmdReport.Parameters.Add(Opcion);

                    daReport.Fill(retVal);

                    if (retVal.Tables[0] != null && retVal.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = retVal.Tables[0];

                        foreach (DataRow item in dt.Rows)
                        {
                            string Fecha = item["FechaUltimaActualizacion"].ToString();
                            Liscriterio.Add(new FormCumplimientoPorcenEnti
                            {
                                IdIndicador = item["IdIndicador"].ToString(),
                                DescCriterio = item["DescCriterio"].ToString(),
                                IdDireccion = Convert.ToInt32(item["IdDireccion"].ToString()),
                                IdCriterio = item["IdCriterio"].ToString(),
                                CriteriosCkech = item["criteriosCkech"].ToString(),
                                FromArray = ArrayFormulas(item["FromArray"].ToString()),
                                ArrayIF = ArrayFormulas(item["Arrayif"].ToString()),
                                ArrayVerdadero = ArrayFormulas(item["ArrayVerdadero"].ToString()),
                                ArrayFalso = ArrayFormulas(item["ArrayFalso"].ToString()),
                                FormulaPorcentaje = item["FormulaPorcentaje"].ToString(),
                                FormulaCumplimiento = item["FormulaCumplimiento"].ToString(),
                                Usuario = item["Usuario"].ToString(),
                                FechaUltimaActualizacion = Fecha

                            });

                        }
                    }
                    else
                    {
                        return Liscriterio;
                    }
                }
                return Liscriterio;
            }
            catch (Exception ex)
            {
                string msj = "Erro al consultar el listado de Criterios FormulasCumplimiento: LisCreteriosXIndicador";
                AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
                return null;
            }

        }

        /// <summary>
        /// Metodo para crear, actulizar las formulas asociadas a los indicadores (Formlas de porcentajes y Cumplimientos)
        /// </summary>
        /// <param name="Entidad">FormCumplimientoPorcenEnti</param>
        /// <returns>En caso de ser la operacion exitosa se retorna 1 "Registro",Actualización "2"</returns>
        public int CrearParamFormula(FormCumplimientoPorcenEnti Entidad)
        {
            try
            {
                var _Resulvalidacion = ExisteIndicadorFormula(Entidad.IdIndicador);
                if (_Resulvalidacion == 1)
                {
                    UpdateParamFormulas(Entidad);
                    return 1;
                }
                else if (_Resulvalidacion == 2)
                {
                    InsertParamFormulas(Entidad);
                    return 2;
                }

                return 3;
            }
            catch (Exception ex)
            {
                string msj = "Erro al intentar crear la formula para el indicador";
                AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
                return 4;
            }
        }

        /// <summary>
        /// Metodo retorna los peridos procesodos de fórmlas
        /// </summary>
        /// <param name="anioProceso">año de ejecucion</param>
        /// <returns></returns>
        public List<int> PeridosEjecutados(int anioProceso)
        {
            try
            {
                DataSet retVal = new DataSet();
                SqlConnection sqlConn = (SqlConnection)new SUTEL_IndicadoresEntities().Database.Connection;
                SqlCommand cmdReport = new SqlCommand("sp_GetFacReglasReport", sqlConn);
                SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                var ResultProceso = new List<int>();

                using (cmdReport)
                {
                    cmdReport.CommandType = CommandType.StoredProcedure;

                    SqlParameter _anioProceso = new SqlParameter("anioProceso", anioProceso);
                    cmdReport.Parameters.Add(_anioProceso);

                    daReport.Fill(retVal);


                    if (retVal.Tables[0] != null && retVal.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = retVal.Tables[0];
                        foreach (DataRow item in dt.Rows)
                        {
                            ResultProceso.Add(Convert.ToInt32(item["PeriodoEjec"]));

                        }
                    }
                    return ResultProceso;
                }

            }
            catch (Exception ex)
            {
                string msj = "Erro al consulta indicador:ExisteIndicadorFormula";
                AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
                return new List<int>();
            }
        }

        /// <summary>
        /// Retorna el listado de ejecuciones programadas para el motor de reglas en el estado pendiente por ejecucion
        /// </summary>
        /// <returns></returns>
        public List<EjecucionMotorEnti> LisProcesarEjecuciones()
        {
            var LisEjecucion = new List<EjecucionMotorEnti>();
            try
            {

                DataSet retVal = new DataSet();
                SqlConnection sqlConn = (SqlConnection)new SUTEL_IndicadoresEntities().Database.Connection;
                SqlCommand cmdReport = new SqlCommand("pa_EjecucionMotor", sqlConn);
                SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                using (cmdReport)
                {
                    cmdReport.CommandType = CommandType.StoredProcedure;

                    cmdReport.Parameters.AddWithValue("@p_Opcion", 2);
                    cmdReport.Parameters.AddWithValue("@p_Periodo", 0);
                    cmdReport.Parameters.AddWithValue("@p_Usuario", "");
                    cmdReport.Parameters.AddWithValue("@p_Anio", DateTime.Now.Year);
                    cmdReport.Parameters.AddWithValue("@p_Fecha", DateTime.Now);
                    cmdReport.Parameters.AddWithValue("@p_IdEjecucion", 0);
                    daReport.Fill(retVal);

                    if (retVal.Tables[0] != null && retVal.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = retVal.Tables[0];

                        foreach (DataRow item in dt.Rows)
                        {

                            LisEjecucion.Add(new EjecucionMotorEnti
                            {
                                idejecucion = Convert.ToInt32(item["idejecucion"].ToString()),
                                periodoEjecucion = item["periodoEjecucion"].ToString(),
                                anioEjecucion = Convert.ToInt32(item["anioEjecucion"].ToString()),
                                usuarioEjecucion = item["usuarioEjecucion"].ToString(),
                                FechaRegistro = item["FechaRegistro"].ToString()
                            });

                        }
                    }
                    else
                    {
                        return LisEjecucion;
                    }
                }
                return LisEjecucion;
            }
            catch (Exception ex)
            {
                string msj = "Erro al consultar el listado de Ejecucion del motor de formulas: LisProcesarEjecuciones";
                AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
                return null;
            }

        }

        /// <summary>
        /// Retorna el listado de ejecuciones programadas para el motor de reglas en el estado ejecutadas
        /// </summary>
        /// <returns></returns>
        public List<EjecucionMotorEnti> LisProcesosEjecutados()
        {
            var LisEjecucion = new List<EjecucionMotorEnti>();
            try
            {

                DataSet retVal = new DataSet();
                SqlConnection sqlConn = (SqlConnection)new SUTEL_IndicadoresEntities().Database.Connection;
                SqlCommand cmdReport = new SqlCommand("pa_EjecucionMotor", sqlConn);
                SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                using (cmdReport)
                {
                    cmdReport.CommandType = CommandType.StoredProcedure;

                    cmdReport.Parameters.AddWithValue("@p_Opcion", 3);
                    cmdReport.Parameters.AddWithValue("@p_Periodo", 0);
                    cmdReport.Parameters.AddWithValue("@p_Usuario", "");
                    cmdReport.Parameters.AddWithValue("@p_Anio", DateTime.Now.Year);
                    cmdReport.Parameters.AddWithValue("@p_Fecha", DateTime.Now);
                    cmdReport.Parameters.AddWithValue("@p_IdEjecucion", 0);

                    daReport.Fill(retVal);

                    if (retVal.Tables[0] != null && retVal.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = retVal.Tables[0];

                        foreach (DataRow item in dt.Rows)
                        {

                            LisEjecucion.Add(new EjecucionMotorEnti
                            {
                                idejecucion = Convert.ToInt32(item["idejecucion"].ToString()),
                                periodoEjecucion = item["periodoEjecucion"].ToString(),
                                anioEjecucion = Convert.ToInt32(item["anioEjecucion"].ToString()),
                                usuarioEjecucion = item["usuarioEjecucion"].ToString(),
                                FechaRegistro = item["FechaRegistro"].ToString()
                            });

                        }
                    }
                    else
                    {
                        return LisEjecucion;
                    }
                }
                return LisEjecucion;
            }
            catch (Exception ex)
            {
                string msj = "Erro al consultar el listado de Ejecucion del motor de formulas: LisProcesosEjecutados";
                AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
                return null;
            }

        }

        /// <summary>
        /// se crea el tack de ejecucion del motor para resolver las formulas
        /// </summary>
        /// <param name="Periodo">Periodo a Ejecutar</param>
        /// <param name="Usuario">Usuario quien realiza el proceso</param>
        /// <returns></returns>
        public int ParametroEjecucion(int Periodo, string Usuario, int Anio, DateTime Fecha)
        {
            try
            {
                InsertEjecucion(Periodo, Usuario, Anio, Fecha);
                return 1;
            }
            catch (Exception ex)
            {
                string msj = "Erro al intentar crear el parametro de ejecucion";
                AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
                return 2;
            }
        }

        /// <summary>
        /// se anula una programacion asignada  a la ajecucion del motor de reglas
        /// </summary>
        /// <param name="idejecucion"></param>
        /// <returns></returns>
        public bool AnularEjecucion(int idejecucion)
        {
            DataSet retVal = new DataSet();
            SqlConnection sqlConn = (SqlConnection)new SUTEL_IndicadoresEntities().Database.Connection;
            SqlCommand cmdReport = new SqlCommand("pa_EjecucionMotor", sqlConn);
            SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);

            try
            {

                using (cmdReport)
                {
                    cmdReport.CommandType = CommandType.StoredProcedure;
                    cmdReport.Parameters.AddWithValue("@p_Opcion", 4);
                    cmdReport.Parameters.AddWithValue("@p_Periodo", 0);
                    cmdReport.Parameters.AddWithValue("@p_Usuario", "");
                    cmdReport.Parameters.AddWithValue("@p_Anio", DateTime.Now.Year);
                    cmdReport.Parameters.AddWithValue("@p_Fecha", DateTime.Now);
                    cmdReport.Parameters.AddWithValue("@p_IdEjecucion", idejecucion);
                    cmdReport.Connection.Open();
                    int result = cmdReport.ExecuteNonQuery();
                    cmdReport.Connection.Close();
                    if (result > 0)
                        return true;
                    else
                        return false;


                }
            }
            catch (Exception ex)
            {
                string msj = "Erro al  Crear Parametros de ejecucion para motor:InsertEjecucion";
                AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
                cmdReport.Connection.Close();
                return false;
            }

        }


            #region Metodos Privados

            private int ExisteIndicadorFormula(string IdIndicador)
            {
                int _Opcion = 3;
                try
                {
                    DataSet retVal = new DataSet();
                    SqlConnection sqlConn = (SqlConnection)new SUTEL_IndicadoresEntities().Database.Connection;
                    SqlCommand cmdReport = new SqlCommand("pa_ParamFormulasTrans", sqlConn);
                    SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);

                    using (cmdReport)
                    {
                        int IdServicio = 0;
                        cmdReport.CommandType = CommandType.StoredProcedure;

                        SqlParameter Opcion = new SqlParameter("p_Opcion", _Opcion);
                        cmdReport.Parameters.Add(Opcion);
                        SqlParameter _IdServicio = new SqlParameter("p_IdServicio", IdServicio);
                        cmdReport.Parameters.Add(_IdServicio);
                        SqlParameter _IdIndicador = new SqlParameter("p_IdIndicador", IdIndicador);
                        cmdReport.Parameters.Add(_IdIndicador);
                        SqlParameter _FormulaPorcentaje = new SqlParameter("p_FormulaPorcentaje", "");
                        cmdReport.Parameters.Add(_FormulaPorcentaje);
                        SqlParameter _FormulaCumplimiento = new SqlParameter("p_FormulaCumplimiento", "");
                        cmdReport.Parameters.Add(_FormulaCumplimiento);
                        SqlParameter _Criterios = new SqlParameter("p_Criterios", "");
                        cmdReport.Parameters.Add(_Criterios);
                        SqlParameter _FromArray = new SqlParameter("p_FromArray", "");
                        cmdReport.Parameters.Add(_FromArray);
                        SqlParameter _ArrayIF = new SqlParameter("p_ArrayIF", "");
                        cmdReport.Parameters.Add(_ArrayIF);

                        SqlParameter _ArrayVerdadero = new SqlParameter("p_ArrayVerdadero", "");
                        cmdReport.Parameters.Add(_ArrayVerdadero);

                        SqlParameter _ArrayFalso = new SqlParameter("p_ArrayFalso", "");
                        cmdReport.Parameters.Add(_ArrayFalso);

                        SqlParameter _Usuario = new SqlParameter("p_Usuario", "SutelUser");
                        cmdReport.Parameters.Add(_Usuario);

                        daReport.Fill(retVal);


                        if (retVal.Tables[0] != null && retVal.Tables[0].Rows.Count > 0)
                        {
                            DataTable dt = retVal.Tables[0];
                            foreach (DataRow item in dt.Rows)
                            {
                                if (item["IdIndicador"].ToString() == IdIndicador)
                                    return 1;

                            }
                        }
                        else
                        {
                            return 2;
                        }
                    }
                    return 5;
                }
                catch (Exception ex)
                {
                    string msj = "Erro al consulta indicador:ExisteIndicadorFormula";
                    AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
                    return 3;
                }
            }

            private void InsertParamFormulas(FormCumplimientoPorcenEnti entity)
            {
                int _Opcion = 1;
                DataSet retVal = new DataSet();
                SqlConnection sqlConn = (SqlConnection)new SUTEL_IndicadoresEntities().Database.Connection;
                SqlCommand cmdReport = new SqlCommand("pa_ParamFormulasTrans", sqlConn);
                SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);

                string FormulaPorcentaje = entity.FormulaPorcentaje == null ? entity.FormulaPorcentaje = "" : entity.FormulaPorcentaje;
                string FormulaCumplimiento = entity.FormulaCumplimiento == null ? entity.FormulaCumplimiento = "" : entity.FormulaCumplimiento;
                try
                {

                    using (cmdReport)
                    {

                        string ArrayFormla = Formulastring(entity.FromArray);
                        string ArrayIf = Formulastring(entity.ArrayIF);
                        string ArrayVerdadero = Formulastring(entity.ArrayVerdadero);
                        string ArrayFalso = Formulastring(entity.ArrayFalso);
                        cmdReport.CommandType = CommandType.StoredProcedure;
                        cmdReport.Parameters.AddWithValue("@p_Opcion", _Opcion);
                        cmdReport.Parameters.AddWithValue("@p_IdServicio", entity.IdServicio);
                        cmdReport.Parameters.AddWithValue("@p_IdIndicador", entity.IdIndicador);
                        cmdReport.Parameters.AddWithValue("@p_FormulaPorcentaje", FormulaPorcentaje);
                        cmdReport.Parameters.AddWithValue("@p_FormulaCumplimiento", FormulaCumplimiento);
                        cmdReport.Parameters.AddWithValue("@p_Criterios", entity.Criterios);
                        cmdReport.Parameters.AddWithValue("@p_Usuario", entity.Usuario);
                        cmdReport.Parameters.AddWithValue("@p_FromArray", ArrayFormla);
                        cmdReport.Parameters.AddWithValue("@p_ArrayIf", ArrayIf);
                        cmdReport.Parameters.AddWithValue("@p_ArrayVerdadero", ArrayVerdadero);
                        cmdReport.Parameters.AddWithValue("@p_ArrayFalso", ArrayFalso);
                        cmdReport.Connection.Open();
                        int result = cmdReport.ExecuteNonQuery();
                        cmdReport.Connection.Close();

                    }

                }
                catch (Exception ex)
                {
                    string msj = "Error al  Crear Formulas para indicador :InsertParamFormulas";
                    AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
                    cmdReport.Connection.Close();
                }
            }

            private void UpdateParamFormulas(FormCumplimientoPorcenEnti entity)
            {
                int _Opcion = 2;
                DataSet retVal = new DataSet();
                SqlConnection sqlConn = (SqlConnection)new SUTEL_IndicadoresEntities().Database.Connection;
                SqlCommand cmdReport = new SqlCommand("pa_ParamFormulasTrans", sqlConn);
                SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);

                try
                {

                    using (cmdReport)
                    {
                        string ArrayFormla = Formulastring(entity.FromArray);
                        string ArrayIf = Formulastring(entity.ArrayIF);
                        string ArrayVerdadero = Formulastring(entity.ArrayVerdadero);
                        string ArrayFalso = Formulastring(entity.ArrayFalso);
                        cmdReport.CommandType = CommandType.StoredProcedure;
                        SqlParameter Opcion = new SqlParameter("p_Opcion", _Opcion);
                        cmdReport.Parameters.Add(Opcion);
                        SqlParameter _IdServicio = new SqlParameter("p_IdServicio", entity.IdServicio);
                        cmdReport.Parameters.Add(_IdServicio);
                        SqlParameter _IdIndicador = new SqlParameter("p_IdIndicador", entity.IdIndicador);
                        cmdReport.Parameters.Add(_IdIndicador);
                        SqlParameter _FormulaPorcentaje = new SqlParameter("p_FormulaPorcentaje", entity.FormulaPorcentaje);
                        cmdReport.Parameters.Add(_FormulaPorcentaje);
                        SqlParameter _FormulaCumplimiento = new SqlParameter("p_FormulaCumplimiento", entity.FormulaCumplimiento);
                        cmdReport.Parameters.Add(_FormulaCumplimiento);
                        SqlParameter _Criterios = new SqlParameter("p_Criterios", entity.Criterios);
                        cmdReport.Parameters.Add(_Criterios);
                        SqlParameter _FromArray = new SqlParameter("p_FromArray", ArrayFormla);
                        cmdReport.Parameters.Add(_FromArray);

                        SqlParameter _ArrayIF = new SqlParameter("p_ArrayIF", ArrayIf);
                        cmdReport.Parameters.Add(_ArrayIF);

                        SqlParameter _ArrayVerdadero = new SqlParameter("p_ArrayVerdadero", ArrayVerdadero);
                        cmdReport.Parameters.Add(_ArrayVerdadero);

                        SqlParameter _ArrayFalso = new SqlParameter("p_ArrayFalso", ArrayFalso);
                        cmdReport.Parameters.Add(_ArrayFalso);

                        SqlParameter _Usuario = new SqlParameter("p_Usuario", entity.Usuario);
                        cmdReport.Parameters.Add(_Usuario);
                        cmdReport.Connection.Open();
                        int result = cmdReport.ExecuteNonQuery();
                        cmdReport.Connection.Close();

                    }

                }
                catch (Exception ex)
                {
                    string msj = "Erro al  modificar Formulas para indicador :UpdateParamFormulas";
                    AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
                    cmdReport.Connection.Close();
                }
            }

            private void InsertEjecucion(int periodo, string Usuario, int anio, DateTime fecha)
            {
                DataSet retVal = new DataSet();
                SqlConnection sqlConn = (SqlConnection)new SUTEL_IndicadoresEntities().Database.Connection;
                SqlCommand cmdReport = new SqlCommand("pa_EjecucionMotor", sqlConn);
                SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);

                try
                {

                    using (cmdReport)
                    {
                        cmdReport.CommandType = CommandType.StoredProcedure;
                        cmdReport.Parameters.AddWithValue("@p_Opcion", 1);
                        cmdReport.Parameters.AddWithValue("@p_Periodo", periodo);
                        cmdReport.Parameters.AddWithValue("@p_Usuario", Usuario);
                        cmdReport.Parameters.AddWithValue("@p_Anio", anio);
                        cmdReport.Parameters.AddWithValue("@p_Fecha", fecha);
                        cmdReport.Parameters.AddWithValue("@p_IdEjecucion", 0);
                        cmdReport.Connection.Open();
                        int result = cmdReport.ExecuteNonQuery();
                        cmdReport.Connection.Close();

                    }
                }
                catch (Exception ex)
                {
                    string msj = "Erro al  Crear Parametros de ejecucion para motor:InsertEjecucion";
                    AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
                    cmdReport.Connection.Close();
                }
            }

            private List<string> ArrayFormulas(string formula)
            {
                string[] splitString = formula.Split(',');
                var LisForm = new List<string>();
                if (formula.Count() > 0)
                {
                    for (int i = 0; i < splitString.Count(); i++)
                    {
                        if (splitString[i] != "")
                        {
                            LisForm.Add(splitString[i]);
                        }
                    }
                }


                return LisForm;
            }

            private string Formulastring(List<string> Array)
            {
                string StrigFormula = "";
                int ContArray = Array.Count();
                if (Array.Count() > 0)
                {
                    for (int i = 0; i < Array.Count(); i++)
                    {
                        StrigFormula += Array[i] + ",";
                    }

                }

                return StrigFormula;
            }

            /// <summary>
            /// Consulta las formulas en BD según una periocidad establecida
            /// </summary>
            /// <param name="Periocidad"></param>
            /// <returns></returns>
            public DataTable ConsultaFormulaSegunPeriocidad(short Periocidad)
            {
                int _Opcion = Periocidad;
                try
                {
                    DataSet retVal = new DataSet();
                    SqlConnection sqlConn = (SqlConnection)new SUTEL_IndicadoresEntities().Database.Connection;
                    SqlCommand cmdReport = new SqlCommand("pa_ParamFormulasTrans", sqlConn);
                    SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);

                    using (cmdReport)
                    {
                        int IdServicio = 0;
                        cmdReport.CommandType = CommandType.StoredProcedure;

                        SqlParameter Opcion = new SqlParameter("p_Opcion", Periocidad==1? 4:5);
                        cmdReport.Parameters.Add(Opcion);
                        SqlParameter _IdServicio = new SqlParameter("p_IdServicio", 0);
                        cmdReport.Parameters.Add(_IdServicio);
                        SqlParameter _IdIndicador = new SqlParameter("p_IdIndicador", "");
                        cmdReport.Parameters.Add(_IdIndicador);
                        SqlParameter _FormulaPorcentaje = new SqlParameter("p_FormulaPorcentaje", "");
                        cmdReport.Parameters.Add(_FormulaPorcentaje);
                        SqlParameter _FormulaCumplimiento = new SqlParameter("p_FormulaCumplimiento", "");
                        cmdReport.Parameters.Add(_FormulaCumplimiento);
                        SqlParameter _Criterios = new SqlParameter("p_Criterios", "");
                        cmdReport.Parameters.Add(_Criterios);
                        SqlParameter _Usuario = new SqlParameter("p_Usuario", "SutelUser");
                        cmdReport.Parameters.Add(_Usuario);
                        SqlParameter _FromArray = new SqlParameter("p_FromArray", "");
                        cmdReport.Parameters.Add(_FromArray);
                        SqlParameter _ArrayIf = new SqlParameter("p_ArrayIf", "");
                        cmdReport.Parameters.Add(_ArrayIf);
                        SqlParameter _ArrayVerdadero = new SqlParameter("p_ArrayVerdadero", "");
                        cmdReport.Parameters.Add(_ArrayVerdadero);
                        SqlParameter _ArrayFalso = new SqlParameter("p_ArrayFalso", "");
                        cmdReport.Parameters.Add(_ArrayFalso);
                        daReport.Fill(retVal);
                    }
                    return retVal.Tables[0];
                }
                catch (Exception ex)
                {
                    string msj = "Erro al consulta indicador:ExisteIndicadorFormula";
                    AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
                    return null;
                }
            }

            #endregion 

        #region Operaciones para resolver formulación
            
            public DataTable ConsultaFormulaSegunPeriocidad(short IdPeriodo, int IdServicio, int IdFormula, int AnioProcesar)
            {
                try
                {
                    DataSet retVal = new DataSet();
                    SqlConnection sqlConn = (SqlConnection)new SUTEL_IndicadoresEntities().Database.Connection;
                    SqlCommand cmdReport = new SqlCommand("pa_ObtenerValoresFormulacion", sqlConn);
                    SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);

                    using (cmdReport)
                    {
                        cmdReport.CommandType = CommandType.StoredProcedure;
                        SqlParameter Periodo = new SqlParameter("p_IdPeriodo", IdPeriodo);
                        cmdReport.Parameters.Add(Periodo);
                        SqlParameter _IdServicio = new SqlParameter("p_IdServicio", IdServicio);
                        cmdReport.Parameters.Add(_IdServicio);
                        SqlParameter _IdFormula = new SqlParameter("p_IdFormula", IdFormula);
                        cmdReport.Parameters.Add(_IdFormula);
                        cmdReport.Parameters.Add(new SqlParameter("p_AnioProcesar", AnioProcesar));
                        daReport.Fill(retVal);
                    }
                    return retVal.Tables[0];
                }
                catch (Exception ex)
                {
                    string msj = "Error al consultar la formulación";
                    AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
                    return null;
                }
            }

            /// <summary>
            /// Administra la tabla de resultados FacReglasReport
            /// </summary>
            /// <param name="op"></param>
            /// <param name="entidad"></param>
            /// <returns></returns>
            public DataTable admFacReglasReport(short op, FacReglasReport entidad)
            {
                try
                {
                    DataSet retVal = new DataSet();
                    SqlConnection sqlConn = (SqlConnection)new SUTEL_IndicadoresEntities().Database.Connection;
                    SqlCommand cmdReport = new SqlCommand("pa_admFacReglasReport", sqlConn);
                    SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);

                    using (cmdReport)
                    {
                        cmdReport.CommandType = CommandType.StoredProcedure;
                        cmdReport.Parameters.Add( new SqlParameter("p_Op",op));
                        cmdReport.Parameters.Add(new SqlParameter("p_IdServicio", entidad.IdServicio));
                        cmdReport.Parameters.Add(new SqlParameter("p_Servicio", entidad.Servicio));
                        cmdReport.Parameters.Add(new SqlParameter("p_IdTipoInd", entidad.IdTipoInd));
                        cmdReport.Parameters.Add(new SqlParameter("p_TipoIndicador", entidad.TipoIndicador));
                        cmdReport.Parameters.Add(new SqlParameter("p_IdIndicador", entidad.IdIndicador));
                        cmdReport.Parameters.Add(new SqlParameter("p_NombreIndicador", entidad.NombreIndicador));
                        cmdReport.Parameters.Add(new SqlParameter("p_IdCriterio", entidad.IdCriterio));
                        cmdReport.Parameters.Add(new SqlParameter("p_NombreCriterio", entidad.NombreCriterio));
                        cmdReport.Parameters.Add(new SqlParameter("p_Agrupacion", entidad.Agrupacion));
                        cmdReport.Parameters.Add(new SqlParameter("p_DetalleAgrupacion", entidad.DetalleAgrupacion));
                        cmdReport.Parameters.Add(new SqlParameter("p_Frecuencia",entidad.Frecuencia));
                        cmdReport.Parameters.Add(new SqlParameter("p_Desglose", entidad.Desglose));
                        cmdReport.Parameters.Add(new SqlParameter("p_ValMes1",entidad.ValMes1));
                        cmdReport.Parameters.Add(new SqlParameter("p_ValMes2", entidad.ValMes2));
                        cmdReport.Parameters.Add(new SqlParameter("p_ValMes3", entidad.ValMes3));
                        cmdReport.Parameters.Add(new SqlParameter("p_IdParamFormulas", entidad.IdParamFormulas));
                        cmdReport.Parameters.Add(new SqlParameter("p_ValFormulaCalcPorc", entidad.ValFormulaCalcPorc));
                        cmdReport.Parameters.Add(new SqlParameter("p_Umbral", entidad.Umbral));
                        cmdReport.Parameters.Add(new SqlParameter("p_FactorRigurosidad", entidad.FactorRigurosidad));
                        cmdReport.Parameters.Add(new SqlParameter("p_ValFormulaCalcCump", entidad.ValFormulaCalcCump));
                        cmdReport.Parameters.Add(new SqlParameter("p_Peso", entidad.Peso));
                        cmdReport.Parameters.Add(new SqlParameter("p_ResultCumplPeso", entidad.ResultCumplPeso));
                        cmdReport.Parameters.Add(new SqlParameter("p_FacServSinDesgTec", entidad.FacServSinDesgTec));
                        cmdReport.Parameters.Add(new SqlParameter("p_FacServicio2G", entidad.FacServicio2G));
                        cmdReport.Parameters.Add(new SqlParameter("p_FacServicio3G", entidad.FacServicio3G));
                        cmdReport.Parameters.Add(new SqlParameter("p_FacServicio4G", entidad.FacServicio4G));
                        cmdReport.Parameters.Add(new SqlParameter("p_IdOperador", entidad.IdOperador));
                        //if (!string.IsNullOrEmpty(entidad.NombreOperador))
                            cmdReport.Parameters.Add(new SqlParameter("p_NombreOperador", entidad.NombreOperador));
                        //else
                            //cmdReport.Parameters.Add(new SqlParameter("NombreOperador", DBNull.Value));
                        cmdReport.Parameters.Add(new SqlParameter("p_AnioProcesado", entidad.AnioProcesado));
                        cmdReport.Parameters.Add(new SqlParameter("p_PeriodoEjec", entidad.PeriodoEjec));
                        daReport.Fill(retVal);
                    }
                    if (retVal.Tables.Count > 0)
                        return retVal.Tables[0];
                    else
                        return new DataTable();
                }
                catch (Exception ex)
                {
                    string msj = "Error al administrar los resultados de la formulación";
                    AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
                    return null;
                }
            }

            /// <summary>
            /// Se generan los cálculos por agrupación de tecnología
            /// </summary>
            /// <param name="p_IdPeriodo"></param>
            /// <param name="p_AnioEjec"></param>
            /// <param name="p_IdServicio"></param>
            /// <param name="p_Indicador"></param>
            /// <param name="p_IdParamFormulas"></param>
            public void CalculosTecnologias(short IdPeriodo, int AnioEjec, string IdOperador)
            {
                try
                {
                    DataSet retVal = new DataSet();
                    SqlConnection sqlConn = (SqlConnection)new SUTEL_IndicadoresEntities().Database.Connection;
                    SqlCommand cmdReport = new SqlCommand("pa_CalcFacTecnolog", sqlConn);
                    SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                    using (cmdReport)
                    {
                        cmdReport.CommandType = CommandType.StoredProcedure;
                        cmdReport.Parameters.Add(new SqlParameter("p_IdPeriodo", IdPeriodo));
                        cmdReport.Parameters.Add(new SqlParameter("p_AnioEjec ", AnioEjec));
                        cmdReport.Parameters.Add(new SqlParameter("p_IdOperador ", IdOperador));
                        daReport.Fill(retVal);
                    }
                }
                catch (Exception ex)
                {
                    string msj = "Error al generar los cálculos por tecnología";
                    AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
                }
            }


            public DataTable CalculosFactorRigurosidad(string IdIndicador, Int16 IdPeriodo, string IdOperador, decimal PorcIndicador, decimal Umbral, int Anio)
            {
                try
                {
                    DataSet retVal = new DataSet();
                    SqlConnection sqlConn = (SqlConnection)new SUTEL_IndicadoresEntities().Database.Connection;
                    SqlCommand cmdReport = new SqlCommand("pa_CalcFactRigurosidad", sqlConn);
                    SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);

                    using (cmdReport)
                    {
                        cmdReport.CommandType = CommandType.StoredProcedure;
                        cmdReport.Parameters.Add(new SqlParameter("p_Indicador", IdIndicador));
                        cmdReport.Parameters.Add(new SqlParameter("p_IdPeriodo", IdPeriodo));
                        cmdReport.Parameters.Add(new SqlParameter("p_IdOperador", IdOperador));
                        cmdReport.Parameters.Add(new SqlParameter("p_PorcIndicador", PorcIndicador));
                        cmdReport.Parameters.Add(new SqlParameter("p_Umbral", Umbral));
                        cmdReport.Parameters.Add(new SqlParameter("p_AnioActual", Anio));
                        daReport.Fill(retVal);
                    }
                    return retVal.Tables[0];
                }
                catch (Exception ex)
                {
                    string msj = "Error al consultar la formulación";
                    AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
                    return null;
                }
            }


            public DataTable EjecucionesMotor(int opcion, int Perido, string Usuario, int Anio, DateTime Fecha, int IdEjecucion)
            {
                try
                {
                    DataSet retVal = new DataSet();
                    SqlConnection sqlConn = (SqlConnection)new SUTEL_IndicadoresEntities().Database.Connection;
                    SqlCommand cmdReport = new SqlCommand("pa_EjecucionMotor", sqlConn);
                    SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);

                    using (cmdReport)
                    {
                        cmdReport.CommandType = CommandType.StoredProcedure;
                        cmdReport.Parameters.Add(new SqlParameter("p_Opcion", opcion));
                        cmdReport.Parameters.Add(new SqlParameter("p_Periodo", Perido));
                        cmdReport.Parameters.Add(new SqlParameter("p_Usuario", Usuario));
                        cmdReport.Parameters.Add(new SqlParameter("p_Anio", Anio));
                        cmdReport.Parameters.Add(new SqlParameter("p_Fecha", Fecha));
                        cmdReport.Parameters.Add(new SqlParameter("p_IdEjecucion", IdEjecucion));
                        daReport.Fill(retVal);
                    }
                    if (retVal != null && retVal.Tables.Count > 0)
                        return retVal.Tables[0];
                    else
                        return new DataTable();
                }
                catch (Exception ex)
                {
                    string msj = "Error al administrar ejecuciones del motor";
                    AppContext.ExceptionBuilder.BuildDataAccessException(msj, ex);
                    return null;
                }
            }

        #endregion
    }
}
