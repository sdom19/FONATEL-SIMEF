using GB.SUTEL.DAL.FormCumplimientoPorcenDA;
using GB.SUTEL.Entities;
using GB.SUTEL.Entities.FormCumplimientoPorcenEnti;
using GB.SUTEL.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using GB.SUTEL.BL.FormCumplimientoPorcenBL;

namespace GB.SUTEL.BL.FormCumplimientoPorcenBL
{
    public class FormCumplimientoPorcenBL
    {
        /// <summary>
        /// Metodo para consultar los criterios pertenecientes a un indicador
        /// </summary>
        /// <param name="IdIndicador">IdIndicador</param>
        /// <returns>Retorna un objeto lista Criterio</returns>
        public List<FormCumplimientoPorcenEnti> LisCreteriosXIndicador(string IdIndicador) { return new FormCumplimientoPorcenDA(new ApplicationContext("", "SUTEL - Captura de Formulas")).LisCreteriosXIndicador(IdIndicador); }

        /// <summary>
        /// Metodo para crear, actulizar las formulas asociadas a los indicadores (Formlas de porcentajes y Cumplimientos)
        /// </summary>
        /// <param name="Entidad">FormCumplimeintoPorcenEnti</param>
        /// <returns>En caso de ser la operacion exitosa se retorna 1 "Registro",Actualización "2"</returns>
        public int CrearParamFormula(FormCumplimientoPorcenEnti enti) { return new FormCumplimientoPorcenDA(new ApplicationContext("", "SUTEL - Captura de Umbrales")).CrearParamFormula(enti); }

        /// <summary>
        /// Metodo para consultar todas las formulas de acuerdo a una periocidad
        /// </summary>
        /// <param name="IdIndicador">IdIndicador</param>
        /// <returns>Retorna un objeto lista Criterio</returns>
        public DataTable ConsFormulasPeriocidad(string Periocidad)
        { return new FormCumplimientoPorcenDA(new ApplicationContext("", "SUTEL - Consulta formulación por periodicidad"))
            .ConsultaFormulaSegunPeriocidad(Periocidad=="TRIMESTRAL"?Convert.ToInt16(1):Convert.ToInt16(2)); 
        }

        public DataTable ConsValoresFormulacion(short IdPeriodo, int IdServicio, int IdFormula, int AnioProcesar)
        {
            return new FormCumplimientoPorcenDA(new ApplicationContext("", "SUTEL - Consulta valores formulación por periodicidad"))
              .ConsultaFormulaSegunPeriocidad(IdPeriodo,IdServicio,IdFormula, AnioProcesar);
        }

        /// <summary>
        /// Método par admministrar la formulación
        /// </summary>
        /// <param name="op"></param>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public DataTable admFacReglasReport(short op, FacReglasReport entidad) { return new FormCumplimientoPorcenDA(new ApplicationContext("", "SUTEL - Adm resultadso formulación")).admFacReglasReport(op,entidad); }

        /// <summary>
        /// Cálculos por tecnología
        /// </summary>
        /// <param name="p_IdPeriodo"></param>
        /// <param name="p_AnioEjec"></param>
        /// <param name="p_IdServicio"></param>
        /// <param name="p_Indicador"></param>
        /// <param name="p_IdParamFormulas"></param>
        public void CalculosTecnologias(short IdPeriodo, int AnioEjec, string IdOperador)
        { new FormCumplimientoPorcenDA(
            new ApplicationContext("", "SUTEL - Adm resultadso formulación")).CalculosTecnologias(IdPeriodo, AnioEjec, IdOperador);
        }

        public DataTable CalculosFr(string IdIndicador, Int16 IdPeriodo, string IdOperador, decimal PorcIndicador, decimal Umbral, int Anio)
        {
          return  new FormCumplimientoPorcenDA(
                new ApplicationContext("", "SUTEL - Adm resultadso Factor rigurosidad")).CalculosFactorRigurosidad(IdIndicador, IdPeriodo, 
                IdOperador, PorcIndicador, Umbral, Anio);
        }

        public DataTable AdmEjecucionesMotor(int opcion, int Perido, string Usuario, int Anio, DateTime Fecha, int IdEjecucion)
        {

            return new FormCumplimientoPorcenDA(
                    new ApplicationContext("", "SUTEL - Administración ejecución motor")).EjecucionesMotor(opcion, Perido, Usuario, Anio, Fecha, IdEjecucion);
        }


        /// <summary>
        /// Metodo retorna los peridos de proceso de fórmlas
        /// </summary>
        /// <param name="anioProceso">año de ejecucion</param>
        /// <returns></returns>
        public List<int> PeridosEjecutados(int anioProceso) { return new FormCumplimientoPorcenDA(new ApplicationContext("", "SUTEL - Captura de Formulas")).PeridosEjecutados(anioProceso); }

        /// <summary>
        /// se crea el tack de ejecucion del motor para resolver las formulas
        /// </summary>
        /// <param name="Periodo">Periodo a Ejecutar</param>
        /// <param name="Usuario">Usuario quien realiza el proceso</param>
        /// <returns></returns>
        public int ParametroEjecucion(int Periodo, string Usuario, int anio, DateTime Fecha) { return new FormCumplimientoPorcenDA(new ApplicationContext("", "SUTEL - Captura de Formulas")).ParametroEjecucion(Periodo, Usuario, anio, Fecha); }

        /// <summary>
        /// Retorna el listado de ejecuciones programadas para el motor de reglas en el estado pendiente por ejecucion
        /// </summary>
        /// <returns></returns>
        public List<EjecucionMotorEnti> LisProcesarEjecuciones() { return new FormCumplimientoPorcenDA(new ApplicationContext("", "SUTEL - Captura de Formulas")).LisProcesarEjecuciones(); }

        /// <summary>
        /// Retorna el listado de ejecuciones programadas para el motor de reglas en el estado EJECUTADAS
        /// </summary>
        /// <returns></returns>
        public List<EjecucionMotorEnti> LisProcesosEjecutados() { return new FormCumplimientoPorcenDA(new ApplicationContext("", "SUTEL - Captura de Formulas")).LisProcesosEjecutados(); }

        /// <summary>
        /// se anula una programacion asignada  a la ajecucion del motor de reglas
        /// </summary>
        /// <param name="idejecucion"></param>
        /// <returns></returns>
        public bool AnularEjecucion(int idejecucion) { return new FormCumplimientoPorcenDA(new ApplicationContext("", "SUTEL - Captura de Formulas")).AnularEjecucion(idejecucion); }

    }
}
