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
                    ("execute spObtenerDetalleReglasValidacion @IdReglasValidacionTipo,@IdRegla,@IdTipo,@IdOperador,@IdIndicador,@IdIndicadorVariable",
                      new SqlParameter("@IdReglasValidacionTipo", objReglas.idReglasValidacionTipo),
                      new SqlParameter("@IdRegla", objReglas.idRegla),
                      new SqlParameter("@IdTipo", objReglas.idTipo),
                      new SqlParameter("@IdOperador", objReglas.IdOperador),
                      new SqlParameter("@IdIndicador", objReglas.IdIndicador),
                      new SqlParameter("@IdIndicadorVariable", objReglas.idIndicadorVariable)
                    ).ToList();

                ListaCategoriaDetalle = ListaCategoriaDetalle.Select(x => new DetalleReglaValidacion()
                {
                    id = Utilidades.Encriptar(x.idReglasValidacionTipo.ToString()),
                    idRegla = x.idRegla,
                    tipoReglaValidacion = ObtenerTipoRegla(x.idTipo),
                    idTipo = x.idTipo,
                    IdOperador = x.IdOperador,
                    operadorArismetico = ObtenerOperador(x.IdOperador),
                    IdIndicador = x.IdIndicador,
                    detalleIndicadorVariables = ObtenerIndicadorVariable(x.idIndicadorVariable),
                    idIndicadorVariable = x.idIndicadorVariable,
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
                    ("exec spValidarRegla @idRegla",
                       new SqlParameter("@idRegla", objeto.idRegla)
                    ).ToList();
            }

            return listaValicion;
        }


        private string ObtenerListadoTipoReglas(int idRegla)
        {
            string tipoReglas= "";
                tipoReglas = db.Database.SqlQuery<string>
                    ("exec spObtenerListadoTipoReglaXReglaValidacion @idRegla",
                       new SqlParameter("@idRegla", idRegla)
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
                ListaDetalleReglaValidacion = db.Database.SqlQuery<DetalleReglaValidacion>
                ("execute spActualizarDetalleReglaValidacion @IdReglasValidacionTipo,@IdRegla,@IdTipo,@IdOperador,@IdIndicador,@IdIndicadorVariable,@Estado",
                    new SqlParameter("@IdReglasValidacionTipo", objDetalleReglaValidacion.idReglasValidacionTipo),
                    new SqlParameter("@IdRegla", objDetalleReglaValidacion.idRegla),
                    new SqlParameter("@IdTipo", objDetalleReglaValidacion.idTipo),
                    new SqlParameter("@IdOperador", objDetalleReglaValidacion.IdOperador),
                    new SqlParameter("@IdIndicador", objDetalleReglaValidacion.IdIndicador),
                    new SqlParameter("@IdIndicadorVariable", objDetalleReglaValidacion.idIndicadorVariable),
                    new SqlParameter("@Estado", objDetalleReglaValidacion.Estado)
                ).ToList();


                ListaDetalleReglaValidacion = ListaDetalleReglaValidacion.Select(X => new DetalleReglaValidacion
                {
                    id = Utilidades.Encriptar(X.idReglasValidacionTipo.ToString()),
                    idRegla = X.idRegla,
                    idTipo = X.idTipo,
                    IdOperador = X.IdOperador,
                    IdIndicador = X.IdIndicador,
                    idIndicadorVariable = X.idIndicadorVariable,
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

        private DetalleIndicadorVariables ObtenerIndicadorVariable(int id)
        {
            return
            db.DetalleIndicadorVariables.Where(x => x.idDetalleIndicador == id && x.Estado == true).FirstOrDefault();
        }

        #endregion
    }
}
