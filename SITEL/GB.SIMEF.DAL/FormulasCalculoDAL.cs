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

        #region Funciones

        /// <summary>
        /// 17/08/2022
        /// José Navarro Acuña
        /// Actualiza los datos e inserta por medio de merge
        /// </summary>
        /// <param name="pFormulasCalculo"></param>
        /// <returns></returns>
        public List<FormulaCalculo> ActualizarDatos(FormulaCalculo pFormulasCalculo)
        {
            List<FormulaCalculo> listaformulas = new List<FormulaCalculo>();
            using (db = new SIMEFContext())
            {
                listaformulas = db.Database.SqlQuery<FormulaCalculo>
                ("execute dbo.pa_ActualizarFormulaCalculo " +
                " @pIdFormulaCalculo, @pCodigo, @pNombre, @pIdIndicador, @pIdDetalleIndicadorVariable, @pFechaCalculo, @pDescripcion, @pIdFrecuenciaEnvio, @pNivelCalculoTotal, @pUsuarioModificacion, @pUsuarioCreacion, @pIdEstadoRegistro",
                     new SqlParameter("@pIdFormulaCalculo", pFormulasCalculo.IdFormulaCalculo),
                     new SqlParameter("@pCodigo", pFormulasCalculo.Codigo),
                     new SqlParameter("@pNombre", pFormulasCalculo.Nombre),
                     pFormulasCalculo.IdIndicador == 0 ?
                        new SqlParameter("@pIdIndicador", DBNull.Value)
                        :
                        new SqlParameter("@pIdIndicador", pFormulasCalculo.IdIndicador),

                     pFormulasCalculo.IdDetalleIndicadorVariable == 0 ?
                        new SqlParameter("@pIdDetalleIndicadorVariable", DBNull.Value)
                        :
                        new SqlParameter("@pIdDetalleIndicadorVariable", pFormulasCalculo.IdDetalleIndicadorVariable),

                     pFormulasCalculo.FechaCalculo == null ?
                        new SqlParameter("@pFechaCalculo", DBNull.Value)
                        :
                        new SqlParameter("@pFechaCalculo", pFormulasCalculo.FechaCalculo),

                     string.IsNullOrEmpty(pFormulasCalculo.Descripcion) ?
                        new SqlParameter("@pDescripcion", DBNull.Value.ToString())
                        :
                        new SqlParameter("@pDescripcion", pFormulasCalculo.Descripcion),

                     new SqlParameter("@pNivelCalculoTotal", pFormulasCalculo.NivelCalculoTotal),

                     pFormulasCalculo.IdFrecuenciaEnvio == 0 ?
                        new SqlParameter("@pIdFrecuenciaEnvio", DBNull.Value)
                        :
                        new SqlParameter("@pIdFrecuenciaEnvio", pFormulasCalculo.IdFrecuenciaEnvio),

                     string.IsNullOrEmpty(pFormulasCalculo.UsuarioCreacion) ?
                        new SqlParameter("@pUsuarioCreacion", DBNull.Value)
                        :
                        new SqlParameter("@pUsuarioCreacion", pFormulasCalculo.UsuarioCreacion),

                     string.IsNullOrEmpty(pFormulasCalculo.UsuarioModificacion) ?
                        new SqlParameter("@pUsuarioModificacion", DBNull.Value)
                        :
                        new SqlParameter("@pUsuarioModificacion", pFormulasCalculo.UsuarioModificacion),

                     new SqlParameter("@pIdEstadoRegistro", pFormulasCalculo.IdEstadoRegistro)
                    ).ToList();

                listaformulas = listaformulas.Select(x => new FormulaCalculo()
                {
                    id = Utilidades.Encriptar(x.IdFormulaCalculo.ToString()),
                    IdFormulaCalculo = x.IdFormulaCalculo,
                    IdIndicadorSalidaString = Utilidades.Encriptar(x.IdIndicador.ToString()),
                    Nombre = x.Nombre,
                    Codigo = x.Codigo,
                    Descripcion = x.Descripcion,
                    IdEstadoRegistro = x.IdEstadoRegistro,

                    FrecuenciaEnvio = x.IdFrecuenciaEnvio != null ? ObtenerFrecuenciaEnvio((int)x.IdFrecuenciaEnvio) : null,
                    IndicadorSalida = x.IdIndicador != null ? ObtenerIndicador((int)x.IdIndicador) : null,
                    VariableSalida = x.IdDetalleIndicadorVariable != null ? ObtenerVariableDatoSalida((int)x.IdDetalleIndicadorVariable) : null,
                    EtiquetaFormulaConArgumentos = ObtenerEtiquetaFormulaConArgumentos(x.IdFormulaCalculo)
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
        public FormulaCalculo ActualizarEtiquetaFormula(FormulaCalculo pFormulasCalculo)
        {
            FormulaCalculo formula = null;

            using (db = new SIMEFContext())
            {
                formula = db.Database.SqlQuery<FormulaCalculo>("exec pa_ActualizarEtiquetaFormula @pIdFormulaCalculo, @pEtiquetaFormula, @pUsuarioModificacion",
                    new SqlParameter("@pIdFormulaCalculo", pFormulasCalculo.IdFormulaCalculo),
                    new SqlParameter("@pEtiquetaFormula", pFormulasCalculo.Formula),
                    new SqlParameter("@pUsuarioModificacion", pFormulasCalculo.UsuarioModificacion)
                    ).FirstOrDefault();

                if (formula != null)
                {
                    formula = new FormulaCalculo()
                    {
                        id = Utilidades.Encriptar(formula.IdFormulaCalculo.ToString()),
                        IdFormulaCalculo = formula.IdFormulaCalculo,
                        IdIndicadorSalidaString = Utilidades.Encriptar(formula.IdIndicador.ToString()),
                        Nombre = formula.Nombre,
                        Codigo = formula.Codigo,
                        Descripcion = formula.Descripcion,
                        IdEstadoRegistro = formula.IdEstadoRegistro,

                        FrecuenciaEnvio = formula.IdFrecuenciaEnvio != null ? ObtenerFrecuenciaEnvio((int)formula.IdFrecuenciaEnvio) : null,
                        IndicadorSalida = formula.IdIndicador != null ? ObtenerIndicador((int)formula.IdIndicador) : null,
                        VariableSalida = formula.IdDetalleIndicadorVariable != null ? ObtenerVariableDatoSalida((int)formula.IdDetalleIndicadorVariable) : null,
                        EtiquetaFormulaConArgumentos = ObtenerEtiquetaFormulaConArgumentos(formula.IdFormulaCalculo)
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
        public FormulaCalculo RegistrarJobEnFormula(FormulaCalculo pFormulasCalculo)
        {
            FormulaCalculo formula = null;

            using (db = new SIMEFContext())
            {
                db.FormulaCalculo.Attach(pFormulasCalculo);
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
        public List<FormulaCalculo> ObtenerDatos(FormulaCalculo pformulasCalculo)
        {
            List<FormulaCalculo> listaFormulasCalculo = new List<FormulaCalculo>();

            using (db = new SIMEFContext())
            {
                listaFormulasCalculo = db.Database.SqlQuery<FormulaCalculo>
                    ("execute pa_ObtenerFormulaCalculo @pIdFormulaCalculo",
                     new SqlParameter("@pIdFormulaCalculo", pformulasCalculo.IdFormulaCalculo)
                    ).ToList();

                bool esUnicoRegistro = pformulasCalculo.IdFormulaCalculo != 0 && listaFormulasCalculo.Count == 1; // optimizar la consulta para 1 solo registro

                listaFormulasCalculo = listaFormulasCalculo.Select(x => new FormulaCalculo()
                {
                    id = Utilidades.Encriptar(x.IdFormulaCalculo.ToString()),
                    Codigo = x.Codigo,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    IdEstadoRegistro = x.IdEstadoRegistro,
                    NivelCalculoTotal = x.NivelCalculoTotal,
                    IdFrecuenciaString = Utilidades.Encriptar(x.IdFrecuenciaEnvio.ToString()),
                    IdIndicadorSalidaString = Utilidades.Encriptar(x.IdIndicador.ToString()),
                    IdVariableDatoString = Utilidades.Encriptar(x.IdDetalleIndicadorVariable.ToString()),
                    EstadoRegistro = db.EstadoRegistro.Where(i => i.IdEstadoRegistro == x.IdEstadoRegistro).Single(),
                    FechaCreacion = x.FechaCreacion,
                    FechaModificacion = x.FechaModificacion,
                    UsuarioCreacion = x.UsuarioCreacion,
                    UsuarioModificacion = x.UsuarioModificacion,
                    FechaCalculo = x.FechaCalculo,
                    Formula = x.Formula,
                    IdJob = x.IdJob,

                    FrecuenciaEnvio = esUnicoRegistro && x.IdFrecuenciaEnvio != null ? ObtenerFrecuenciaEnvio((int)x.IdFrecuenciaEnvio) : null,
                    IndicadorSalida = esUnicoRegistro && x.IdIndicador != null ? ObtenerIndicador((int)x.IdIndicador) : null,
                    VariableSalida = esUnicoRegistro && x.IdDetalleIndicadorVariable != null ? ObtenerVariableDatoSalida((int)x.IdDetalleIndicadorVariable) : null,
                    EtiquetaFormulaConArgumentos = esUnicoRegistro ? ObtenerEtiquetaFormulaConArgumentos(x.IdFormulaCalculo) : null
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
        public List<FormulaCalculo> ObtenerDependenciasIndicadorConFormulasCalculo(int pIdIndicador)
        {
            List<FormulaCalculo> lista = new List<FormulaCalculo>();

            using (db = new SIMEFContext())
            {
                lista = db.Database.SqlQuery<FormulaCalculo>
                    ("execute pa_ObtenerDependenciaIndicadorConFormulaCalculo @pIdIndicador",
                     new SqlParameter("@pIdIndicador", pIdIndicador)
                    ).ToList();

                lista = lista.Select(x => new FormulaCalculo()
                {
                    id = Utilidades.Encriptar(x.IdFormulaCalculo.ToString()),
                    IdFormulaCalculo = x.IdFormulaCalculo,
                    Codigo = x.Codigo,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    IdEstadoRegistro = x.IdEstadoRegistro,
                    NivelCalculoTotal = x.NivelCalculoTotal,
                    IdFrecuenciaEnvio = x.IdFrecuenciaEnvio,
                    IdIndicador = x.IdIndicador,
                    IdDetalleIndicadorVariable = x.IdDetalleIndicadorVariable,
                    FechaCreacion = x.FechaCreacion,
                    FechaModificacion = x.FechaModificacion,
                    UsuarioCreacion = x.UsuarioCreacion,
                    UsuarioModificacion = x.UsuarioModificacion,
                    FechaCalculo = x.FechaCalculo,

                    FrecuenciaEnvio = x.IdFrecuenciaEnvio != null ? ObtenerFrecuenciaEnvio((int)x.IdFrecuenciaEnvio) : null,
                    IndicadorSalida = x.IdIndicador != null ? ObtenerIndicador((int)x.IdIndicador) : null,
                    VariableSalida = x.IdDetalleIndicadorVariable != null ? ObtenerVariableDatoSalida((int)x.IdDetalleIndicadorVariable) : null,
                    EtiquetaFormulaConArgumentos = ObtenerEtiquetaFormulaConArgumentos(x.IdFormulaCalculo)
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
        public FormulaCalculo VerificarExistenciaFormulaPorCodigoNombre(FormulaCalculo pFormulasCalculo)
        {
            FormulaCalculo formulasCalculo = null;

            using (db = new SIMEFContext())
            {
                formulasCalculo = db.FormulaCalculo.Where(x =>
                        (x.Nombre.Trim().ToUpper().Equals(pFormulasCalculo.Nombre.Trim().ToUpper()) || x.Codigo.Trim().ToUpper().Equals(pFormulasCalculo.Codigo.Trim().ToUpper())) &&
                        x.IdFormulaCalculo != pFormulasCalculo.IdFormulaCalculo &&
                        x.IdEstadoRegistro != (int)EstadosRegistro.Eliminado
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
        public FormulaCalculo VerificarExistenciaFormulaPorID(int pIdIdentificador)
        {
            FormulaCalculo formula = null;

            using (db = new SIMEFContext())
            {
                formula = db.FormulaCalculo.Where(x => x.IdFormulaCalculo == pIdIdentificador && x.IdEstadoRegistro != (int)EstadosRegistro.Eliminado).FirstOrDefault();
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
                    ("execute pa_VerificarSiFormulaEjecuto @pIdFormulaCalculo",
                     new SqlParameter("@pIdFormulaCalculo", pIdFormula)
                    ).FirstOrDefault();
            }
            return formulaDTO;
        }

        #endregion

        #region Funciones de motor de cáculo

        /// <summary>
        /// 01/03/2023
        /// José Navarro Acuña
        /// Función que permite obtener el job de una formula
        /// </summary>
        /// <param name="pFormulasCalculo"></param>
        /// <returns></returns>
        public async Task<JobMotorFormulaDTO> ObtenerJobMotor(FormulaCalculo pFormulasCalculo)
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
        public async Task<JobMotorFormulaDTO> CrearJobEnMotorAsync(FormulaCalculo pFormulasCalculo, bool pStartNow)
        {
            JobMotorFormulaDTO jobDTO = null;

            using (var apiClient = new HttpClient())
            {
                TareaJobMotorDTO payload = new TareaJobMotorDTO(
                    pFormulasCalculo.UsuarioCreacion,
                    ConfigurationManager.AppSettings["APIMotorApplicationId"].ToString(),
                    Dispatch_Task,
                    mapFrecuenciasConMotor[(FrecuenciaEnvioEnum)pFormulasCalculo.IdFrecuenciaEnvio],
                    pStartNow,
                    pFormulasCalculo.FechaCalculo,
                    new ParametroTareaDTO[] 
                    {
                        new ParametroTareaDTO("Formula", pFormulasCalculo.IdFormulaCalculo.ToString())
                    }
                );

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
        public async Task<JobMotorFormulaDTO> ActualizarCalendarizacionJob(FormulaCalculo pFormulasCalculo)
        {
            JobMotorFormulaDTO jobDTO = null;

            using (var apiClient = new HttpClient())
            {
                TareaJobMotorDTO payload = new TareaJobMotorDTO(
                    mapFrecuenciasConMotor[(FrecuenciaEnvioEnum)pFormulasCalculo.IdFrecuenciaEnvio],
                    pFormulasCalculo.FechaCalculo
                );

                var stringPayload = JsonConvert.SerializeObject(payload);
                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await apiClient.PostAsync(ConfigurationManager.AppSettings["APIMotorFormulas"].ToString() + "/Jobs/" + pFormulasCalculo.IdJob + "/NuevaPeriodicidad", httpContent);

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
        public async Task<JobMotorFormulaDTO> EjecutarJobExistente(FormulaCalculo pFormulasCalculo)
        {
            JobMotorFormulaDTO jobDTO = null;

            using (var apiClient = new HttpClient())
            {
                HttpResponseMessage response = await apiClient.PostAsync(ConfigurationManager.AppSettings["APIMotorFormulas"].ToString() + "/Jobs/" + pFormulasCalculo.IdJob + "/LanzarAhora", null);

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
        public async Task<JobMotorFormulaDTO> CambiarEstadoJob(FormulaCalculo pFormulasCalculo)
        {
            JobMotorFormulaDTO jobDTO = null;

            using (var apiClient = new HttpClient())
            {
                string estadoJob = mapEstadoFormulaConMotor[(EstadosRegistro)pFormulasCalculo.IdEstadoRegistro];
                HttpResponseMessage response = await apiClient.PostAsync(ConfigurationManager.AppSettings["APIMotorFormulas"].ToString() + "/Jobs/" + pFormulasCalculo.IdJob + "/Estado/" + estadoJob, null);

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

            List<ArgumentoFormula> argumentos = db.Database.SqlQuery<ArgumentoFormula>("exec pa_ObtenerEtiquetaFormulaConArgumento @IdFormulaCalculo",
                new SqlParameter("@IdFormulaCalculo", pIdFormula)
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
