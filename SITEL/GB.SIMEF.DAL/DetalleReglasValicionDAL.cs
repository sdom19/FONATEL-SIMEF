using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;

namespace GB.SIMEF.DAL
{
    public class DetalleReglasValicionDAL : BitacoraDAL
    {
        private SIMEFContext db;

        #region Metodos Consulta Base de Datos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public List<DetalleReglaValidacion> ObtenerDatos(DetalleReglaValidacion objeto)
        {
            List<DetalleReglaValidacion> ListaDetalles = new List<DetalleReglaValidacion>();
            using (db = new SIMEFContext())
            {
                ListaDetalles = db.Database.SqlQuery<DetalleReglaValidacion>
                    ("execute pa_ObtenerDetalleReglaValidacion @IdDetalleReglaValidacion, @IdRegla, @IdTipo, @IdOperador, @IdDetalleIndicador, @IdIndicador",
                        new SqlParameter("@IdDetalleReglaValidacion", objeto.idDetalleReglaValidacion),
                        new SqlParameter("@IdRegla", objeto.idReglaValidacion),
                        new SqlParameter("@IdTipo", objeto.idTipoReglaValidacion),
                        new SqlParameter("@IdOperador", objeto.idOperadorAritmetico),
                        new SqlParameter("@IdDetalleIndicador", objeto.idDetalleIndicadorVariable),
                        new SqlParameter("@IdIndicador", objeto.idIndicador)
                    ).ToList();

                ListaDetalles = ListaDetalles.Select(x => new DetalleReglaValidacion()
                {
                    id = Utilidades.Encriptar(x.idDetalleReglaValidacion.ToString()),
                    idIndicadorVariableString = Utilidades.Encriptar(x.idDetalleIndicadorVariable.ToString()),
                    idDetalleReglaValidacion = x.idDetalleReglaValidacion,
                    idReglaValidacion = x.idReglaValidacion,
                    idTipoReglaValidacion = x.idTipoReglaValidacion,
                    tipoReglaValidacion = ObtenerTipoRegla(x.idTipoReglaValidacion),
                    idOperadorAritmetico = x.idOperadorAritmetico,
                    operadorArismetico = ObtenerOperador(x.idOperadorAritmetico),
                    idDetalleIndicadorVariable = x.idDetalleIndicadorVariable,
                    idIndicador = x.idIndicador,
                    NombreVariable = x.idTipoReglaValidacion == (int)Constantes.TipoReglasDetalle.FormulaContraAtributosValidos ? ObtenerVariable(x.idDetalleReglaValidacion, 1)
                    : x.idTipoReglaValidacion == (int)Constantes.TipoReglasDetalle.FormulaActualizacionSecuencial ? ObtenerVariable(x.idDetalleReglaValidacion, 2)
                    : ObtenerVariable(x.idDetalleIndicadorVariable, 0),
                    reglaIndicadorEntrada = x.idTipoReglaValidacion == (int)Constantes.TipoReglasDetalle.FormulaContraOtroIndicadorEntrada
                    ? ObtenerReglaIndicadorEntrada(x.idDetalleReglaValidacion):new ReglaIndicadorEntrada(),
                    reglaComparacionConstante = x.idTipoReglaValidacion == (int)Constantes.TipoReglasDetalle.FormulaContraConstante
                    ? ObtenerReglaContraConstante(x.idDetalleReglaValidacion):new ReglaComparacionConstante(),
                    reglaAtributoValido = x.idTipoReglaValidacion == (int)Constantes.TipoReglasDetalle.FormulaContraAtributosValidos
                    ? ObtenerReglaAtributosValidos(x.idDetalleReglaValidacion):new ReglaAtributoValido(),    
                    reglaSecuencial = x.idTipoReglaValidacion == (int)Constantes.TipoReglasDetalle.FormulaActualizacionSecuencial
                    ? ObtenerReglaSecuencial(x.idDetalleReglaValidacion) : new ReglaSecuencial(),
                    reglaIndicadorEntradaSalida = x.idTipoReglaValidacion == (int)Constantes.TipoReglasDetalle.FormulaContraOtroIndicadorEntradaSalida
                    ? ObtenerReglaIndicadorEntradaSalida(x.idDetalleReglaValidacion) : new ReglaIndicadorEntradaSalida(),
                    reglaIndicadorSalida = x.idTipoReglaValidacion == (int)Constantes.TipoReglasDetalle.FormulaContraOtroIndicadorSalida
                    ? ObtenerReglaIndicadorSalida(x.idDetalleReglaValidacion):new ReglaIndicadorSalida(),
                    Estado = x.Estado
                }).ToList();
            }
            return ListaDetalles;
        }

        /// <summary>
        /// Valida si existen Reglas de Validación en indicadores
        /// Michael Hernández Cordero
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public List<string> ValidarDatos(DetalleReglaValidacion objeto)
        {
            List<string> listaValicion = new List<string>();
            using (db = new SIMEFContext())
            {
                listaValicion = db.Database.SqlQuery<string>
                    ("exec spValidarRegla @IdRegla",
                       new SqlParameter("@IdRegla", objeto.idReglaValidacion)
                    ).ToList();
            }

            return listaValicion;
        }

        /// <summary>
        /// Creación y modificación
        /// </summary>
        /// <param name="objDetalleReglaValidacion"></param>
        /// <returns></returns>
        public List<DetalleReglaValidacion> ActualizarDatos(DetalleReglaValidacion objeto)
        {
            List<DetalleReglaValidacion> ListaDetalleReglaValidacion = new List<DetalleReglaValidacion>();

            using (db = new SIMEFContext())
            {
                ListaDetalleReglaValidacion = db.Database.SqlQuery<DetalleReglaValidacion>
                ("execute pa_ActualizarDetalleReglaValidacion @pIdDetalleReglaValidacion,@pIdReglaValidacion,@pIdTipoReglaValidacion,@pIdOperador,@pIdDetalleIndicador,@pIdIndicador,@pEstado",
                    new SqlParameter("@pIdDetalleReglaValidacion", objeto.idDetalleReglaValidacion),
                    new SqlParameter("@pIdReglaValidacion", objeto.idReglaValidacion),
                    new SqlParameter("@pIdTipoReglaValidacion", objeto.idTipoReglaValidacion),
                    new SqlParameter("@pIdOperador", objeto.idOperadorAritmetico),
                    new SqlParameter("@pIdDetalleIndicador", objeto.idDetalleIndicadorVariable),
                    new SqlParameter("@pIdIndicador", objeto.idIndicador),
                    new SqlParameter("@pEstado", objeto.Estado)
                ).ToList();

                ListaDetalleReglaValidacion = ListaDetalleReglaValidacion.Select(X => new DetalleReglaValidacion
                {
                    id = Utilidades.Encriptar(X.idDetalleReglaValidacion.ToString()),
                    idDetalleReglaValidacion = X.idDetalleReglaValidacion,
                    idReglaValidacion = X.idReglaValidacion,
                    idTipoReglaValidacion = X.idTipoReglaValidacion,
                    idOperadorAritmetico = X.idOperadorAritmetico,
                    idDetalleIndicadorVariable = X.idDetalleIndicadorVariable,
                    idIndicador = X.idIndicador,
                    Estado = X.Estado

                }).ToList();

                return ListaDetalleReglaValidacion;
            }
        }

        /// <summary>
        /// Autor: Francisco Vindas
        /// Fecha: 16-02-2023
        /// Metodo que permite obtener las etiquetas para la tabla del detalle de la regla de validación 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Tipo"></param>
        /// <returns></returns>
        private string ObtenerVariable(int id, int Tipo)
        {

            string NombreVariables = "";

            NombreVariables = db.Database.SqlQuery<string>
                ("exec pa_ObtenerNombreVariableDetalleReglaValidacion @IdBusqueda, @Tipo",
                   new SqlParameter("@IdBusqueda", id),
                   new SqlParameter("@Tipo", Tipo)
                ).Single();

            return NombreVariables;
        }

        private TipoReglaValidacion ObtenerTipoRegla(int id)
        {
            return
            db.TipoReglaValidacion.Where(x => x.idTipoReglaValidacion == id && x.Estado == true).FirstOrDefault();
        }

        private OperadorAritmetico ObtenerOperador(int id)
        {
            return
            db.OperadorArismetico.Where(x => x.idOperadorAritmetico == id && x.Estado == true).FirstOrDefault();
        }
        /// <summary>
        /// Michael Hernández Cordero
        /// Carga las regla tipo indicador atributos validos
        /// 08-06-2023
        /// Modificación se agrega la categoría
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private ReglaAtributoValido ObtenerReglaAtributosValidos(int id)
        {
            ReglaAtributoValido regla =
                db.Database.SqlQuery<ReglaAtributoValido>
                ("exec pa_ObtenerReglaAtributoValido @IdDetalleReglaValidacion",
                   new SqlParameter("@IdDetalleReglaValidacion", id)
                ).FirstOrDefault();
            if (regla!=null)
            {

                regla.CategoriaDesagregacion = db.CategoriasDesagregacion.Where(x=>x.idCategoriaDesagregacion== regla.idCategoriaDesagregacion).Single();            
                regla.AtributoValidos = db.Database
                    .SqlQuery<string>(string.Format("SELECT STRING_AGG(NombreCategoria,', ') NombreCategoria FROM CategoriaDesagregacion WHERE IdCategoriaDesagregacion IN({0})", regla.idAtributoString)).Single();

                
            }
            return regla;
        }

        private ReglaComparacionConstante ObtenerReglaContraConstante(int id)
        {
            ReglaComparacionConstante regla =
                db.ReglaComparacionConstante.Where(x => x.idDetalleReglaValidacion == id).FirstOrDefault();

            return regla;
        }
        /// <summary>
        /// Michael Hernández Cordero
        /// Carga las regla tipo indicador salida
        /// 08-06-2023
        /// Modificación se agrega la categoría
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        private ReglaSecuencial ObtenerReglaSecuencial(int id)
        {

             ReglaSecuencial regla =
               db.Database.SqlQuery<ReglaSecuencial>
               ("exec pa_ObtenerReglaSecuencial @IdDetalleReglaValidacion",
                  new SqlParameter("@IdDetalleReglaValidacion", id)
               ).FirstOrDefault();

            regla.CategoriaDesagregacion = regla.CategoriaDesagregacion = db.CategoriasDesagregacion
                .Where(x => x.idCategoriaDesagregacion == regla.idCategoriaDesagregacion).Single();
            return regla;
        }

        /// <summary>
        /// Michael Hernández Cordero
        /// Carga las regla tipo indicador salida
        /// 08-06-2023
        /// Modificación se agrega el indicador y la variable 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        private ReglaIndicadorSalida ObtenerReglaIndicadorSalida(int id)
        {
            ReglaIndicadorSalida regla =
                db.ReglaIndicadorSalida.Where(x => x.idDetalleReglaValidacion == id).FirstOrDefault();
            if (regla != null)
            {
                regla.idIndicadorComparaString = Utilidades.Encriptar(regla.idIndicador.ToString());
                regla.idVariableComparaString = Utilidades.Encriptar(regla.idDetalleIndicadorVariable.ToString());
                regla.Indicador=  db.Indicador.Where(x=>x.IdIndicador==regla.idIndicador).Single();
                regla.IndicadorVariable = db.DetalleIndicadorVariables
                    .Where(x=>x.IdDetalleIndicadorVariable== regla.idDetalleIndicadorVariable && x.IdIndicador == regla.idIndicador)
                  .Single();
            }

            return regla;
        }
        /// <summary>
        /// Michael Hernández Cordero
        /// Carga las regla tipo indicador salida
        /// 08-06-2023
        /// Modificación se agrega el indicador y la variable 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        private ReglaIndicadorEntrada ObtenerReglaIndicadorEntrada(int id)
        {
            ReglaIndicadorEntrada regla =
                db.ReglaIndicadorEntrada.Where(x => x.idDetalleReglaValidacion == id).FirstOrDefault();
            if (regla != null)
            {
                regla.idIndicadorComparaString = Utilidades.Encriptar(regla.idIndicador.ToString());
                regla.idVariableComparaString = Utilidades.Encriptar(regla.idDetalleIndicadorVariable.ToString());
                regla.Indicador = db.Indicador.Where(x => x.IdIndicador == regla.idIndicador).Single();
                regla.IndicadorVariable = db.DetalleIndicadorVariables
                    .Where(x => x.IdDetalleIndicadorVariable == regla.idDetalleIndicadorVariable && x.IdIndicador == regla.idIndicador)
                  .Single();
            }
            return regla;
        }
        /// <summary>
        /// Michael Hernández Cordero
        /// Carga las regla tipo indicador salida
        /// 08-06-2023
        /// Modificación se agrega el indicador y la variable 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private ReglaIndicadorEntradaSalida ObtenerReglaIndicadorEntradaSalida(int id)
        {
            ReglaIndicadorEntradaSalida regla =
                db.ReglaIndicadorEntradaSalida.Where(x => x.idDetalleReglaValidacion == id).FirstOrDefault();
            if (regla != null)
            {
                regla.idIndicadorComparaString = Utilidades.Encriptar(regla.idIndicador.ToString());
                regla.idVariableComparaString = Utilidades.Encriptar(regla.idDetalleIndicadorVariable.ToString());
                regla.Indicador = db.Indicador.Where(x => x.IdIndicador == regla.idIndicador).Single();
                regla.IndicadorVariable = db.DetalleIndicadorVariables
                    .Where(x => x.IdDetalleIndicadorVariable == regla.idDetalleIndicadorVariable && x.IdIndicador == regla.idIndicador)
                  .Single();
            }
            return regla;
        }

        #endregion
    }
}
