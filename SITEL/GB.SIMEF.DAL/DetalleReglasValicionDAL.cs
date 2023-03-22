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
                        new SqlParameter("@IdDetalleReglaValidacion", objeto.IdDetalleReglaValidacion),
                        new SqlParameter("@IdRegla", objeto.IdReglaValidacion),
                        new SqlParameter("@IdTipo", objeto.IdTipoReglaValidacion),
                        new SqlParameter("@IdOperador", objeto.IdOperadorAritmetico),
                        new SqlParameter("@IdDetalleIndicador", objeto.IdDetalleIndicadorVariable),
                        new SqlParameter("@IdIndicador", objeto.IdIndicador)
                    ).ToList();

                ListaDetalles = ListaDetalles.Select(x => new DetalleReglaValidacion()
                {
                    id = Utilidades.Encriptar(x.IdDetalleReglaValidacion.ToString()),
                    idIndicadorVariableString = Utilidades.Encriptar(x.IdDetalleIndicadorVariable.ToString()),
                    IdDetalleReglaValidacion = x.IdDetalleReglaValidacion,
                    IdReglaValidacion = x.IdReglaValidacion,
                    IdTipoReglaValidacion = x.IdTipoReglaValidacion,
                    tipoReglaValidacion = ObtenerTipoRegla(x.IdTipoReglaValidacion),
                    IdOperadorAritmetico = x.IdOperadorAritmetico,
                    operadorArismetico = ObtenerOperador(x.IdOperadorAritmetico),
                    IdDetalleIndicadorVariable = x.IdDetalleIndicadorVariable,
                    IdIndicador = x.IdIndicador,
                    NombreVariable = x.IdTipoReglaValidacion == (int)Constantes.TipoReglasDetalle.FormulaContraAtributosValidos ? ObtenerVariable(x.IdDetalleReglaValidacion, 1)
                    : x.IdTipoReglaValidacion == (int)Constantes.TipoReglasDetalle.FormulaActualizacionSecuencial ? ObtenerVariable(x.IdDetalleReglaValidacion, 2)
                    : ObtenerVariable(x.IdDetalleIndicadorVariable, 0),
                    reglaIndicadorEntrada = ObtenerReglaIndicadorEntrada(x.IdDetalleReglaValidacion),
                    reglaComparacionConstante = ObtenerReglaContraConstante(x.IdDetalleReglaValidacion),
                    reglaAtributoValido = ObtenerReglaAtributosValidos(x.IdDetalleReglaValidacion),
                    reglaSecuencial = ObtenerReglaSecuencial(x.IdDetalleReglaValidacion),
                    reglaIndicadorEntradaSalida = ObtenerReglaIndicadorEntradaSalida(x.IdDetalleReglaValidacion),
                    reglaIndicadorSalida = ObtenerReglaIndicadorSalida(x.IdDetalleReglaValidacion),
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
                       new SqlParameter("@IdRegla", objeto.IdReglaValidacion)
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
                ("execute pa_ActualizarDetalleReglaValidacion @pIdDetalleReglaValidacion,@pIdRegla,@pIdTipo,@pIdOperador,@pIdDetalleIndicador,@pIdIndicador,@pEstado",
                    new SqlParameter("@pIdDetalleReglaValidacion", objeto.IdDetalleReglaValidacion),
                    new SqlParameter("@pIdRegla", objeto.IdReglaValidacion),
                    new SqlParameter("@pIdTipo", objeto.IdTipoReglaValidacion),
                    new SqlParameter("@pIdOperador", objeto.IdOperadorAritmetico),
                    new SqlParameter("@pIdDetalleIndicador", objeto.IdDetalleIndicadorVariable),
                    new SqlParameter("@pIdIndicador", objeto.IdIndicador),
                    new SqlParameter("@pEstado", objeto.Estado)
                ).ToList();

                ListaDetalleReglaValidacion = ListaDetalleReglaValidacion.Select(X => new DetalleReglaValidacion
                {
                    id = Utilidades.Encriptar(X.IdDetalleReglaValidacion.ToString()),
                    IdDetalleReglaValidacion = X.IdDetalleReglaValidacion,
                    IdReglaValidacion = X.IdReglaValidacion,
                    IdTipoReglaValidacion = X.IdTipoReglaValidacion,
                    IdOperadorAritmetico = X.IdOperadorAritmetico,
                    IdDetalleIndicadorVariable = X.IdDetalleIndicadorVariable,
                    IdIndicador = X.IdIndicador,
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

        private ReglaAtributoValido ObtenerReglaAtributosValidos(int id)
        {
            ReglaAtributoValido regla =
                db.Database.SqlQuery<ReglaAtributoValido>
                ("exec pa_ObtenerReglaAtributoValido @IdDetalleReglaValidacion",
                   new SqlParameter("@IdDetalleReglaValidacion", id)
                ).FirstOrDefault();
            return regla;
        }

        private ReglaComparacionConstante ObtenerReglaContraConstante(int id)
        {
            ReglaComparacionConstante regla =
                db.ReglaComparacionConstante.Where(x => x.IdDetalleReglaValidacion == id).FirstOrDefault();

            return regla;
        }

        private ReglaSecuencial ObtenerReglaSecuencial(int id)
        {
            ReglaSecuencial regla =
                db.ReglaSecuencial.Where(x => x.idDetalleReglaValidacion == id).FirstOrDefault();

            return regla;
        }

        private ReglaIndicadorSalida ObtenerReglaIndicadorSalida(int id)
        {
            ReglaIndicadorSalida regla =
                db.ReglaIndicadorSalida.Where(x => x.IdDetalleReglaValidacion == id).FirstOrDefault();
            if (regla != null)
            {
                regla.idIndicadorComparaString = Utilidades.Encriptar(regla.IdIndicador.ToString());
                regla.idVariableComparaString = Utilidades.Encriptar(regla.IdDetalleIndicadorVariable.ToString());
            }

            return regla;
        }

        private ReglaIndicadorEntrada ObtenerReglaIndicadorEntrada(int id)
        {
            ReglaIndicadorEntrada regla =
                db.ReglaIndicadorEntrada.Where(x => x.IdDetalleReglaValidacion == id).FirstOrDefault();
            if (regla != null)
            {
                regla.idIndicadorComparaString = Utilidades.Encriptar(regla.IdIndicador.ToString());
                regla.idVariableComparaString = Utilidades.Encriptar(regla.IdDetalleIndicadorVariable.ToString());
            }
            return regla;
        }

        private ReglaIndicadorEntradaSalida ObtenerReglaIndicadorEntradaSalida(int id)
        {
            ReglaIndicadorEntradaSalida regla =
                db.ReglaIndicadorEntradaSalida.Where(x => x.IdDetalleReglaValidacion == id).FirstOrDefault();
            if (regla != null)
            {
                regla.idIndicadorComparaString = Utilidades.Encriptar(regla.IdIndicador.ToString());
                regla.idVariableComparaString = Utilidades.Encriptar(regla.IdDetalleIndicadorVariable.ToString());
            }
            return regla;
        }

        #endregion
    }
}
