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
        /// <param name="objReglas"></param>
        /// <returns></returns>
        public List<DetalleReglaValidacion> ObtenerDatos(DetalleReglaValidacion objReglas)
        {
            List<DetalleReglaValidacion> ListaCategoriaDetalle = new List<DetalleReglaValidacion>();
            using (db = new SIMEFContext())
            {
                ListaCategoriaDetalle = db.Database.SqlQuery<DetalleReglaValidacion>
                    ("execute spObtenerDetalleReglasValidacion @IdDetalleReglaValidacion,@IdRegla,@IdTipo,@IdOperador,@IdDetalleIndicador,@IdIndicador",
                      new SqlParameter("@IdDetalleReglaValidacion", objReglas.IdDetalleReglaValidacion),
                      new SqlParameter("@IdRegla", objReglas.IdRegla),
                      new SqlParameter("@IdTipo", objReglas.IdTipo),
                      new SqlParameter("@IdOperador", objReglas.IdOperador),
                      new SqlParameter("@IdDetalleIndicador", objReglas.IdDetalleIndicador),
                      new SqlParameter("@IdIndicador", objReglas.IdIndicador)
                    ).ToList();

                ListaCategoriaDetalle = ListaCategoriaDetalle.Select(x => new DetalleReglaValidacion()
                {
                    id = Utilidades.Encriptar(x.IdDetalleReglaValidacion.ToString()),
                    IdRegla = x.IdRegla,
                    IdDetalleReglaValidacion = x.IdDetalleReglaValidacion,
                    tipoReglaValidacion = ObtenerTipoRegla(x.IdTipo),
                    IdTipo = x.IdTipo,
                    IdOperador = x.IdOperador,
                    idIndicadorVariableString = x.idIndicadorVariableString,
                    operadorArismetico = ObtenerOperador(x.IdOperador),
                    reglaAtributosValidos = ObtenerReglaAtributosValidos(x.IdDetalleReglaValidacion),
                    //reglaComparacionConstante = ObtenerReglaContraConstante(x.IdReglasValidacionTipo),
                    //reglaSecuencial = ObtenerReglaSecuencial(x.IdReglasValidacionTipo),
                    //reglaIndicadorSalida = ObtenerReglaIndicadorSalida(x.IdReglasValidacionTipo),
                    //reglaIndicadorEntrada = ObtenerReglaIndicadorEntrada(x.IdReglasValidacionTipo),
                    //reglaIndicadorEntradaSalida = ObtenerReglaIndicadorEntradaSalida(x.IdReglasValidacionTipo),
                    Estado = x.Estado
                }).ToList();
            }
            return ListaCategoriaDetalle;
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
                       new SqlParameter("@IdRegla", objeto.IdRegla)
                    ).ToList();
            }

            return listaValicion;
        }

        private string ObtenerListadoTipoReglas(int IdRegla)
        {
            string tipoReglas = "";
            tipoReglas = db.Database.SqlQuery<string>
                ("exec spObtenerListadoTipoReglaXReglaValidacion @IdRegla",
                   new SqlParameter("@IdRegla", IdRegla)
                ).Single();

            return tipoReglas;
        }

        /// <summary>
        /// Creación y modificación
        /// </summary>
        /// <param name="objDetalleReglaValidacion"></param>
        /// <returns></returns>
        public List<DetalleReglaValidacion> ActualizarDatos(DetalleReglaValidacion objDetalleReglaValidacion)
        {
            List<DetalleReglaValidacion> ListaDetalleReglaValidacion = new List<DetalleReglaValidacion>();

            using (db = new SIMEFContext())
            {
                //ListaDetalleReglaValidacion = db.Database.SqlQuery<DetalleReglaValidacion>
                //("execute spActualizarDetalleReglaValidacion @pIdReglasValidacionTipo,@pIdRegla,@pIdTipo,@pIdOperador,@pIdDetalleIndicador,@Estado",
                //    new SqlParameter("@pIdReglasValidacionTipo", objDetalleReglaValidacion.IdReglasValidacionTipo),
                //    new SqlParameter("@pIdRegla", objDetalleReglaValidacion.IdRegla),
                //    new SqlParameter("@pIdTipo", objDetalleReglaValidacion.IdTipo),
                //    new SqlParameter("@pIdOperador", objDetalleReglaValidacion.IdOperador),
                //    new SqlParameter("@pIdDetalleIndicador", objDetalleReglaValidacion.IdDetalleIndicador),
                //    new SqlParameter("@Estado", objDetalleReglaValidacion.Estado)
                //).ToList();

                ListaDetalleReglaValidacion = db.Database.SqlQuery<DetalleReglaValidacion>
                ("execute spActualizarDetalleReglaValidacion @IdDetalleReglaValidacion,@IdRegla, @IdTipo, @IdOperador, @Estado",
                      new SqlParameter("@IdDetalleReglaValidacion", objDetalleReglaValidacion.IdDetalleReglaValidacion),
                      new SqlParameter("@IdRegla", objDetalleReglaValidacion.IdRegla),
                      new SqlParameter("@IdTipo", objDetalleReglaValidacion.IdTipo),
                      new SqlParameter("@IdOperador", objDetalleReglaValidacion.IdOperador),
                      new SqlParameter("@Estado", objDetalleReglaValidacion.Estado)
                ).ToList();

                ListaDetalleReglaValidacion = ListaDetalleReglaValidacion.Select(X => new DetalleReglaValidacion
                {
                    id = Utilidades.Encriptar(X.IdDetalleReglaValidacion.ToString()),
                    IdDetalleReglaValidacion = X.IdDetalleReglaValidacion,
                    IdRegla = X.IdRegla,
                    IdTipo = X.IdTipo,
                    IdOperador = X.IdOperador,
                    Estado = X.Estado

                }).ToList();

                return ListaDetalleReglaValidacion;
            }
        }

        private TipoReglaValidacion ObtenerTipoRegla(int id)
        {
            return
            db.TipoReglaValidacion.Where(x => x.IdTipo == id && x.Estado == true).FirstOrDefault();
        }

        private OperadorArismetico ObtenerOperador(int id)
        {
            return
            db.OperadorArismetico.Where(x => x.IdOperador == id && x.Estado == true).FirstOrDefault();
        }

        private ReglaAtributosValidos ObtenerReglaAtributosValidos(int id)
        {
            ReglaAtributosValidos regla =
                db.ReglaAtributosValidos.Where(x => x.IdTipoReglaValidacion == id).FirstOrDefault();
            if (regla != null)
            {

            }
            return regla;
        }

        private ReglaComparacionConstante ObtenerReglaContraConstante(int id)
        {
            ReglaComparacionConstante regla =
                db.ReglaComparacionConstante.Where(x => x.IdDetalleReglaValidacion == id).FirstOrDefault();
            if (regla != null)
            {

            }
            return regla;
        }

        private ReglaSecuencial ObtenerReglaSecuencial(int id)
        {
            ReglaSecuencial regla =
                db.ReglaSecuencial.Where(x => x.IdDetalleReglaValidacion == id).FirstOrDefault();
            if (regla != null)
            {

            }
            return regla;
        }

        private ReglaIndicadorSalida ObtenerReglaIndicadorSalida(int id)
        {
            ReglaIndicadorSalida regla =
                db.ReglaIndicadorSalida.Where(x => x.IdDetalleReglaValidacion == id).FirstOrDefault();
            if (regla != null)
            {
                regla.IdReglaIndicadorSalida = 0;
            }
            return regla;
        }

        private ReglaIndicadorEntrada ObtenerReglaIndicadorEntrada(int id)
        {
            ReglaIndicadorEntrada regla =
                db.ReglaIndicadorEntrada.Where(x => x.IdDetalleReglaValidacion == id).FirstOrDefault();
            if (regla != null)
            {
                regla.IdReglaIndicadorEntrada = 0;
            }
            return regla;
        }

        private ReglaIndicadorEntradaSalida ObtenerReglaIndicadorEntradaSalida(int id)
        {
            ReglaIndicadorEntradaSalida regla =
                db.ReglaIndicadorEntradaSalida.Where(x => x.IdDetalleReglaValidacion == id).FirstOrDefault();
            if (regla != null)
            {
                regla.IdReglaIndicadorEntradaSalida = 0;
            }
            return regla;
        }

        #endregion
    }
}
