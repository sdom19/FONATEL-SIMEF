using GB.SIMEF.Entities;
using GB.SIMEF.Entities.DTO;
using GB.SIMEF.Resources;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using static GB.SIMEF.Resources.Constantes;
using System.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GB.SIMEF.DAL
{
    public class FormulasCalculoDAL : BitacoraDAL
    {
        private SIMEFContext db;

        /// <summary>
        /// 17/08/2022
        /// José Navarro Acuña
        /// Actualiza los datos e inserta por medio de merge
        /// </summary>
        /// <param name="pFormulasCalculo"></param>
        /// <returns></returns>
        public List<FormulasCalculo> ActualizarDatos(FormulasCalculo pFormulasCalculo)
        {
            List<FormulasCalculo> listaformulas = new List<FormulasCalculo>();
            using (db = new SIMEFContext())
            {
                listaformulas = db.Database.SqlQuery<FormulasCalculo>
                ("execute dbo.spActualizarFormulaCalculo " +
                " @pIdFormula, @pCodigo, @pNombre, @pIdIndicador, @pIdIndicadorVariable, @pFechaCalculo, @pDescripcion, @pIdFrecuencia, @pNivelCalculoTotal, @pUsuarioModificacion, @pUsuarioCreacion, @pIdEstado",
                     new SqlParameter("@pIdFormula", pFormulasCalculo.IdFormula),
                     new SqlParameter("@pCodigo", pFormulasCalculo.Codigo),
                     new SqlParameter("@pNombre", pFormulasCalculo.Nombre),
                     pFormulasCalculo.IdIndicador == 0 ?
                        new SqlParameter("@pIdIndicador", DBNull.Value)
                        :
                        new SqlParameter("@pIdIndicador", pFormulasCalculo.IdIndicador),

                     pFormulasCalculo.IdIndicadorVariable == 0 ?
                        new SqlParameter("@pIdIndicadorVariable", DBNull.Value)
                        :
                        new SqlParameter("@pIdIndicadorVariable", pFormulasCalculo.IdIndicadorVariable),

                     pFormulasCalculo.FechaCalculo == null ?
                        new SqlParameter("@pFechaCalculo", DBNull.Value)
                        :
                        new SqlParameter("@pFechaCalculo", pFormulasCalculo.FechaCalculo),

                     string.IsNullOrEmpty(pFormulasCalculo.Descripcion) ?
                        new SqlParameter("@pDescripcion", DBNull.Value.ToString())
                        :
                        new SqlParameter("@pDescripcion", pFormulasCalculo.Descripcion),

                     new SqlParameter("@pNivelCalculoTotal", pFormulasCalculo.NivelCalculoTotal),

                     pFormulasCalculo.IdFrecuencia == 0 ?
                        new SqlParameter("@pIdFrecuencia", DBNull.Value)
                        :
                        new SqlParameter("@pIdFrecuencia", pFormulasCalculo.IdFrecuencia),

                     string.IsNullOrEmpty(pFormulasCalculo.UsuarioCreacion) ?
                        new SqlParameter("@pUsuarioCreacion", DBNull.Value)
                        :
                        new SqlParameter("@pUsuarioCreacion", pFormulasCalculo.UsuarioCreacion),

                     string.IsNullOrEmpty(pFormulasCalculo.UsuarioModificacion) ?
                        new SqlParameter("@pUsuarioModificacion", DBNull.Value)
                        :
                        new SqlParameter("@pUsuarioModificacion", pFormulasCalculo.UsuarioModificacion),

                     new SqlParameter("@pIdEstado", pFormulasCalculo.IdEstado)
                    ).ToList();

                listaformulas = listaformulas.Select(x => new FormulasCalculo()
                {
                    id = Utilidades.Encriptar(x.IdFormula.ToString()),
                    IdFormula = x.IdFormula,
                    IdIndicadorSalidaString = Utilidades.Encriptar(x.IdIndicador.ToString()),
                    Nombre = x.Nombre,
                    Codigo = x.Codigo,
                    Descripcion = x.Descripcion,
                    IdEstado = x.IdEstado,

                    FrecuenciaEnvio = x.IdFrecuencia != null ? ObtenerFrecuenciaEnvio((int)x.IdFrecuencia) : null,
                    IndicadorSalida = x.IdIndicador != null ? ObtenerIndicador((int)x.IdIndicador) : null,
                    VariableSalida = x.IdIndicadorVariable != null ? ObtenerVariableDatoSalida((int)x.IdIndicadorVariable) : null,
                    EtiquetaFormulaConArgumentos = ObtenerEtiquetaFormulaConArgumentos(x.IdFormula)
                }).ToList();
            }
            return listaformulas;
        }

        /// <summary>
        /// 20/01/2023
        /// José Navarro Acuña
        /// Función que permite actualizar la etiqueta formula del objeto formula de calculo
        /// </summary>
        /// <param name="pFormulasCalculo"></param>
        /// <returns></returns>
        public FormulasCalculo ActualizarEtiquetaFormula(FormulasCalculo pFormulasCalculo)
        {
            FormulasCalculo formula = null;

            using (db = new SIMEFContext())
            {
                formula = db.Database.SqlQuery<FormulasCalculo>("exec spActualizarEtiquetaFormula @pIdFormula, @pEtiquetaFormula, @pUsuarioModificacion",
                    new SqlParameter("@pIdFormula", pFormulasCalculo.IdFormula),
                    new SqlParameter("@pEtiquetaFormula", pFormulasCalculo.Formula),
                    new SqlParameter("@pUsuarioModificacion", pFormulasCalculo.UsuarioModificacion)
                    ).FirstOrDefault();

                if (formula != null)
                {
                    formula = new FormulasCalculo()
                    {
                        id = Utilidades.Encriptar(formula.IdFormula.ToString()),
                        IdFormula = formula.IdFormula,
                        IdIndicadorSalidaString = Utilidades.Encriptar(formula.IdIndicador.ToString()),
                        Nombre = formula.Nombre,
                        Codigo = formula.Codigo,
                        Descripcion = formula.Descripcion,
                        IdEstado = formula.IdEstado,

                        FrecuenciaEnvio = formula.IdFrecuencia != null ? ObtenerFrecuenciaEnvio((int)formula.IdFrecuencia) : null,
                        IndicadorSalida = formula.IdIndicador != null ? ObtenerIndicador((int)formula.IdIndicador) : null,
                        VariableSalida = formula.IdIndicadorVariable != null ? ObtenerVariableDatoSalida((int)formula.IdIndicadorVariable) : null,
                        EtiquetaFormulaConArgumentos = ObtenerEtiquetaFormulaConArgumentos(formula.IdFormula)
                    };
                }
            }

            return formula;
        }

        /// <summary>
        /// 01/03/2023
        /// José Navarro Acuña
        /// Función que permite registrar el job creado en el motor de cálculo en la fórmula de cálculo
        /// </summary>
        /// <param name="pFormulasCalculo"></param>
        /// <returns></returns>
        public FormulasCalculo RegistrarJobEnFormula(FormulasCalculo pFormulasCalculo)
        {
            FormulasCalculo formula = null;

            using (db = new SIMEFContext())
            {
                db.FormulasCalculo.Attach(pFormulasCalculo);
                db.Entry(pFormulasCalculo).Property(r => r.IdJob).IsModified = true;
                db.SaveChanges();
            }
            return formula;
        }

        /// <summary>
        /// Listado de formulas 
        /// Michael Hernández C
        /// </summary>
        /// <returns></returns>
        public List<FormulasCalculo> ObtenerDatos(FormulasCalculo pformulasCalculo)
        {
            List<FormulasCalculo> listaFormulasCalculo = new List<FormulasCalculo>();

            using (db = new SIMEFContext())
            {
                listaFormulasCalculo = db.Database.SqlQuery<FormulasCalculo>
                    ("execute spObtenerFormulasCalculo @IdFormula",
                     new SqlParameter("@IdFormula", pformulasCalculo.IdFormula)
                    ).ToList();

                bool esUnicoRegistro = pformulasCalculo.IdFormula != 0 && listaFormulasCalculo.Count == 1; // optimizar la consulta para 1 solo registro

                listaFormulasCalculo = listaFormulasCalculo.Select(x => new FormulasCalculo()
                {
                    id = Utilidades.Encriptar(x.IdFormula.ToString()),
                    Codigo = x.Codigo,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    IdEstado = x.IdEstado,
                    NivelCalculoTotal = x.NivelCalculoTotal,
                    IdFrecuenciaString = Utilidades.Encriptar(x.IdFrecuencia.ToString()),
                    IdIndicadorSalidaString = Utilidades.Encriptar(x.IdIndicador.ToString()),
                    IdVariableDatoString = Utilidades.Encriptar(x.IdIndicadorVariable.ToString()),
                    EstadoRegistro = db.EstadoRegistro.Where(i => i.IdEstadoRegistro == x.IdEstado).Single(),
                    FechaCreacion = x.FechaCreacion,
                    FechaModificacion = x.FechaModificacion,
                    UsuarioCreacion = x.UsuarioCreacion,
                    UsuarioModificacion = x.UsuarioModificacion,
                    FechaCalculo = x.FechaCalculo,
                    Formula = x.Formula,
                    IdJob = x.IdJob,

                    FrecuenciaEnvio = esUnicoRegistro && x.IdFrecuencia != null ? ObtenerFrecuenciaEnvio((int)x.IdFrecuencia) : null,
                    IndicadorSalida = esUnicoRegistro && x.IdIndicador != null ? ObtenerIndicador((int)x.IdIndicador) : null,
                    VariableSalida = esUnicoRegistro && x.IdIndicadorVariable != null ? ObtenerVariableDatoSalida((int)x.IdIndicadorVariable) : null,
                    EtiquetaFormulaConArgumentos = esUnicoRegistro ? ObtenerEtiquetaFormulaConArgumentos(x.IdFormula) : null
                }).ToList();
            }

            return listaFormulasCalculo;
        }

        /// <summary>
        /// 11/10/2022
        /// José Navarro Acuña
        /// Función que busca y retorna una lista de fórmulas de cálculo donde el Indicador proporcionado este relacionado
        /// </summary>
        /// <param name="pIdIndicador"></param>
        /// <returns></returns>
        public List<FormulasCalculo> ObtenerDependenciasIndicadorConFormulasCalculo(int pIdIndicador)
        {
            List<FormulasCalculo> lista = new List<FormulasCalculo>();

            using (db = new SIMEFContext())
            {
                lista = db.Database.SqlQuery<FormulasCalculo>
                    ("execute spObtenerDependenciasIndicadorConFormulasCalculo @pIdIndicador",
                     new SqlParameter("@pIdIndicador", pIdIndicador)
                    ).ToList();

                lista = lista.Select(x => new FormulasCalculo()
                {
                    id = Utilidades.Encriptar(x.IdFormula.ToString()),
                    IdFormula = x.IdFormula,
                    Codigo = x.Codigo,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    IdEstado = x.IdEstado,
                    NivelCalculoTotal = x.NivelCalculoTotal,
                    IdFrecuencia = x.IdFrecuencia,
                    IdIndicador = x.IdIndicador,
                    IdIndicadorVariable = x.IdIndicadorVariable,
                    FechaCreacion = x.FechaCreacion,
                    FechaModificacion = x.FechaModificacion,
                    UsuarioCreacion = x.UsuarioCreacion,
                    UsuarioModificacion = x.UsuarioModificacion,
                    FechaCalculo = x.FechaCalculo,

                    FrecuenciaEnvio = x.IdFrecuencia != null ? ObtenerFrecuenciaEnvio((int)x.IdFrecuencia) : null,
                    IndicadorSalida = x.IdIndicador != null ? ObtenerIndicador((int)x.IdIndicador) : null,
                    VariableSalida = x.IdIndicadorVariable != null ? ObtenerVariableDatoSalida((int)x.IdIndicadorVariable) : null,
                    EtiquetaFormulaConArgumentos = ObtenerEtiquetaFormulaConArgumentos(x.IdFormula)
                }).ToList();
            }

            return lista;
        }

        /// <summary>
        /// 21/10/2022
        /// José Navarro Acuña
        /// Función que permite buscar y verificar por código y nombre la existencia de una fórmula de calculo en estado diferente de eliminado
        /// </summary>
        /// <param name="pFormulasCalculo"></param>
        /// <returns></returns>
        public FormulasCalculo VerificarExistenciaFormulaPorCodigoNombre(FormulasCalculo pFormulasCalculo)
        {
            FormulasCalculo formulasCalculo = null;

            using (db = new SIMEFContext())
            {
                formulasCalculo = db.FormulasCalculo.Where(x =>
                        (x.Nombre.Trim().ToUpper().Equals(pFormulasCalculo.Nombre.Trim().ToUpper()) || x.Codigo.Trim().ToUpper().Equals(pFormulasCalculo.Codigo.Trim().ToUpper())) &&
                        x.IdFormula != pFormulasCalculo.IdFormula &&
                        x.IdEstado != (int)EstadosRegistro.Eliminado
                    ).FirstOrDefault();
            }
            return formulasCalculo;
        }

        /// <summary>
        /// 13/02/2023
        /// José Navarro Acuña
        /// Función que permite buscar y verificar por medio del identificador la existencia de una fórmula en estado diferente de eliminado.
        /// Importante: No encripta IDs
        /// </summary>
        /// <param name="pIdIdentificador"></param>
        /// <returns></returns>
        public FormulasCalculo VerificarExistenciaFormulaPorID(int pIdIdentificador)
        {
            FormulasCalculo formula = null;

            using (db = new SIMEFContext())
            {
                formula = db.FormulasCalculo.Where(x => x.IdFormula == pIdIdentificador && x.IdEstado != (int)EstadosRegistro.Eliminado).FirstOrDefault();
            }

            return formula;
        }

        /// <summary>
        /// 14/02/2023
        /// José Navarro Acuña
        /// Función que permite verificar si una fórmula ha sido ejecutada
        /// </summary>
        /// <param name="pIdFormula"></param>
        /// <returns></returns>
        public FormulaCalculoDTO VerificarSiFormulaEjecuto(int pIdFormula)
        {
            FormulaCalculoDTO formulaDTO = null;

            using (db = new SIMEFContext())
            {
                formulaDTO = db.Database.SqlQuery<FormulaCalculoDTO>
                    ("execute spVerificarSiFormulaEjecuto @pIdFormula",
                     new SqlParameter("@pIdFormula", pIdFormula)
                    ).FirstOrDefault();
            }
            return formulaDTO;
        }

        #region Funciones de motor de cáculo

        /// <summary>
        /// 01/03/2023
        /// José Navarro Acuña
        /// Función que permite obtener el job de una formula
        /// </summary>
        /// <param name="pFormulasCalculo"></param>
        /// <returns></returns>
        public async Task<JobMotorFormulaDTO> ObtenerJobMotor(FormulasCalculo pFormulasCalculo)
        {
            JobMotorFormulaDTO jobDTO = null;

            using (var apiClient = new HttpClient())
            {
                HttpResponseMessage response = await apiClient.GetAsync(ConfigurationManager.AppSettings["APIMotorFormulas"].ToString() + "/Jobs/" + pFormulasCalculo.IdJob);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    jobDTO = JObject.Parse(content).ToObject<JobMotorFormulaDTO>();
                }
                else
                {
                    throw new Exception(Errores.NoRegistrosActualizar);
                }
            }
            return jobDTO;
        }

        /// <summary>
        /// 01/03/3023
        /// José Navarro Acuña
        /// Función que permite crear un job en el motor de cálculo para calendarizar la ejecución de una fórmula
        /// </summary>
        /// <param name="pFormulasCalculo"></param>
        /// <returns></returns>
        public async Task<JobMotorFormulaDTO> CrearJobEnMotorAsync(FormulasCalculo pFormulasCalculo, bool pStartNow)
        {
            JobMotorFormulaDTO jobDTO = null;

            using (var apiClient = new HttpClient())
            {
                var payload = new
                {
                    user = pFormulasCalculo.UsuarioCreacion,
                    application = ConfigurationManager.AppSettings["APIMotorApplicationId"].ToString(),
                    dispatch = Dispatch_Task,
                    periodicity = mapFrecuenciasConMotor[(FrecuenciaEnvioEnum) pFormulasCalculo.IdFrecuencia],
                    startNow = pStartNow,
                    startDate = pFormulasCalculo.FechaCalculo,
                    parameters = new object[]
                    {
                        new {
                            name = "Formula",
                            value = pFormulasCalculo.IdFormula.ToString()
                        }
                    }
                };

                var stringPayload = JsonConvert.SerializeObject(payload);
                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await apiClient.PostAsync(ConfigurationManager.AppSettings["APIMotorFormulas"].ToString() + "/Jobs", httpContent);

                if (response.IsSuccessStatusCode) 
                {
                    var content = await response.Content.ReadAsStringAsync();
                    jobDTO = JObject.Parse(content).ToObject<JobMotorFormulaDTO>();
                }
                else
                {
                    throw new Exception(Errores.NoRegistrosActualizar);
                }
            }
            return jobDTO;
        }

        /// <summary>
        /// 01/03/2023
        /// José Navarro Acuña
        /// Función que permite actualizar la calendarización de una fórmula a nivel del motor de cáculo
        /// </summary>
        /// <param name="pFormulasCalculo"></param>
        /// <returns></returns>
        public async Task<JobMotorFormulaDTO> ActualizarCalendarizacionJob(FormulasCalculo pFormulasCalculo)
        {
            JobMotorFormulaDTO jobDTO = null;

            using (var apiClient = new HttpClient())
            {
                var payload = new
                {
                    periodicity = mapFrecuenciasConMotor[(FrecuenciaEnvioEnum)pFormulasCalculo.IdFrecuencia],
                    startDate = pFormulasCalculo.FechaCalculo
                };

                var stringPayload = JsonConvert.SerializeObject(payload);
                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await apiClient.PostAsync(ConfigurationManager.AppSettings["APIMotorFormulas"].ToString() + "/Jobs/" + pFormulasCalculo.IdJob + "/NewPeriodicity", httpContent);

                if (response.IsSuccessStatusCode)
                {
                    //var content = await response.Content.ReadAsStringAsync();
                    //jobDTO = JObject.Parse(content).ToObject<JobMotorFormulaDTO>();
                    jobDTO = new JobMotorFormulaDTO();
                }
                else
                {
                    throw new Exception(Errores.NoRegistrosActualizar);
                }
            }
            return jobDTO;
        }

        /// <summary>
        /// 01/03/2023
        /// José Navarro Acuña
        /// Función que permite ejecutar de manera manual una fórmula en el motor de cálculo, sin contemplar la fecha de cálculo y la frecuencia
        /// </summary>
        /// <param name="pFormulasCalculo"></param>
        /// <returns></returns>
        public async Task<JobMotorFormulaDTO> EjecutarJobExistente(FormulasCalculo pFormulasCalculo)
        {
            JobMotorFormulaDTO jobDTO = null;

            using (var apiClient = new HttpClient())
            {
                HttpResponseMessage response = await apiClient.PostAsync(ConfigurationManager.AppSettings["APIMotorFormulas"].ToString() + "/Jobs/" + pFormulasCalculo.IdJob + "/LaunchNow", null);

                if (response.IsSuccessStatusCode)
                {
                    //var content = await response.Content.ReadAsStringAsync();
                    //jobDTO = JObject.Parse(content).ToObject<JobMotorFormulaDTO>();
                    jobDTO = new JobMotorFormulaDTO();
                }
                else
                {
                    throw new Exception(Errores.NoRegistrosActualizar);
                }
            }
            return jobDTO;
        }

        /// <summary>
        /// 01/03/2023
        /// José Navarro Acuña
        /// Función que permite cambiar el estado de la formula relacionada a un job en el motor de cálculo
        /// </summary>
        /// <param name="pFormulasCalculo"></param>
        /// <returns></returns>
        public async Task<JobMotorFormulaDTO> CambiarEstadoJob(FormulasCalculo pFormulasCalculo)
        {
            JobMotorFormulaDTO jobDTO = null;

            using (var apiClient = new HttpClient())
            {
                string estadoJob = mapEstadoFormulaConMotor[(EstadosRegistro)pFormulasCalculo.IdEstado];
                HttpResponseMessage response = await apiClient.PostAsync(ConfigurationManager.AppSettings["APIMotorFormulas"].ToString() + "/Jobs/" + pFormulasCalculo.IdJob + "/Status/" + estadoJob, null);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    jobDTO = JObject.Parse(content).ToObject<JobMotorFormulaDTO>();
                }
                else
                {
                    throw new Exception(Errores.NoRegistrosActualizar);
                }
            }
            return jobDTO;
        }

        #endregion

        #region Métodos privados

        /// <summary>
        /// 21/10/2022
        /// José Navarro Acuña
        /// Función que retorna un objeto tipo variable datos
        /// </summary>
        /// <param name="pIdDetalleIndicadorVariable"></param>
        /// <returns></returns>
        private DetalleIndicadorVariable ObtenerVariableDatoSalida(int pIdDetalleIndicadorVariable)
        {
            DetalleIndicadorVariable detalleIndicadorVariables = db.DetalleIndicadorVariables.Where(x => x.IdDetalleIndicadorVariable == pIdDetalleIndicadorVariable).FirstOrDefault();

            if (detalleIndicadorVariables != null)
            {
                detalleIndicadorVariables.id = Utilidades.Encriptar(detalleIndicadorVariables.IdDetalleIndicadorVariable.ToString());
                detalleIndicadorVariables.IdDetalleIndicadorVariable = 0;
                detalleIndicadorVariables.idIndicadorString = Utilidades.Encriptar(detalleIndicadorVariables.IdIndicador.ToString());
                detalleIndicadorVariables.IdIndicador = 0;
            }

            return detalleIndicadorVariables;
        }

        /// <summary>
        /// 09/02/2023
        /// José Navarro Acuña
        /// Función que permite, a partir de los argumentos de la formula, construir la etiqueta de la operación matemática con los nombres completos de los argumentos
        /// </summary>
        /// <param name="pIdFormula"></param>
        /// <returns></returns>
        private string ObtenerEtiquetaFormulaConArgumentos(int pIdFormula)
        {
            string formula = string.Empty;

            List<ArgumentoFormula> argumentos = db.Database.SqlQuery<ArgumentoFormula>("exec spObtenerEtiquetaFormulaConArgumentos @pIdFormula",
                new SqlParameter("@pIdFormula", pIdFormula)
                ).ToList();

            if (argumentos.Count > 0)
            {
                string[] listaArgs = new string[argumentos.Count];
                string templateEtiqueta = argumentos[0].PredicadoSQL; // no es el predicado, es la etiqueta. Se reutilizó la columna para devolver el resultado

                for (int i = 0; i < argumentos.Count; i++)
                {
                    listaArgs[i] = argumentos[i].Etiqueta;
                }
                formula = string.Format(templateEtiqueta, listaArgs);
            }

            return formula;
        }

        /// <summary>
        /// 09/02/2023
        /// José Navarro Acuña
        /// Función que retorna un objeto tipo de frecuencia de envio
        /// </summary>
        /// <param name="pdFrecuenciaEnvio"></param>
        /// <param name="pUnicamenteActivos"></param>
        /// <returns></returns>
        private FrecuenciaEnvio ObtenerFrecuenciaEnvio(int pdFrecuenciaEnvio, bool pUnicamenteActivos = false)
        {
            FrecuenciaEnvio frecuencia = pUnicamenteActivos ?
                db.FrecuenciaEnvio.Where(i => i.IdFrecuenciaEnvio == pdFrecuenciaEnvio && i.Estado == true).FirstOrDefault()
                :
                db.FrecuenciaEnvio.Where(i => i.IdFrecuenciaEnvio == pdFrecuenciaEnvio).FirstOrDefault();

            if (frecuencia != null)
            {
                frecuencia.id = Utilidades.Encriptar(frecuencia.IdFrecuenciaEnvio.ToString());
                frecuencia.IdFrecuenciaEnvio = 0;
            }
            return frecuencia;
        }

        /// <summary>
        /// 09/02/2023
        /// José Navarro Acuña
        /// Función que retorna un objeto indicador 
        /// </summary>
        /// <param name="pIdIndicador"></param>
        /// <returns></returns>
        private Indicador ObtenerIndicador(int pIdIndicador)
        {
            Indicador indicador = db.Indicador.Where(i => i.IdIndicador == pIdIndicador).FirstOrDefault();

            if (indicador != null)
            {
                indicador.id = Utilidades.Encriptar(indicador.IdIndicador.ToString());
                indicador.IdIndicador = 0;
            }
            return indicador;
        }

        #endregion
    }
}
