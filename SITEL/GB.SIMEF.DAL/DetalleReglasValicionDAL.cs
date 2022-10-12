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
                    ("execute spObtenerDetalleReglasValidacion @IdReglasValidacionTipo,@IdRegla,@IdTipo,@IdOperador",
                      new SqlParameter("@IdReglasValidacionTipo", objReglas.IdReglasValidacionTipo),
                      new SqlParameter("@IdRegla", objReglas.IdRegla),
                      new SqlParameter("@IdTipo", objReglas.IdTipo),
                      new SqlParameter("@IdOperador", objReglas.IdOperador)
                    ).ToList();

                ListaCategoriaDetalle = ListaCategoriaDetalle.Select(x => new DetalleReglaValidacion()
                {
                    id = Utilidades.Encriptar(x.IdReglasValidacionTipo.ToString()),
                    IdRegla = x.IdRegla,
                    tipoReglaValidacion = ObtenerTipoRegla(x.IdTipo),
                    IdTipo = x.IdTipo,
                    IdOperador = x.IdOperador,
                    operadorArismetico = ObtenerOperador(x.IdOperador),
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
            string tipoReglas= "";
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
                ListaDetalleReglaValidacion = db.Database.SqlQuery<DetalleReglaValidacion>
                ("execute spActualizarDetalleReglaValidacion @IdReglasValidacionTipo,@IdRegla,@IdTipo,@IdOperador,@Estado",
                    new SqlParameter("@IdReglasValidacionTipo", objDetalleReglaValidacion.IdReglasValidacionTipo),
                    new SqlParameter("@IdRegla", objDetalleReglaValidacion.IdRegla),
                    new SqlParameter("@IdTipo", objDetalleReglaValidacion.IdTipo),
                    new SqlParameter("@IdOperador", objDetalleReglaValidacion.IdOperador),
                    new SqlParameter("@Estado", objDetalleReglaValidacion.Estado)
                ).ToList();


                ListaDetalleReglaValidacion = ListaDetalleReglaValidacion.Select(X => new DetalleReglaValidacion
                {
                    id = Utilidades.Encriptar(X.IdReglasValidacionTipo.ToString()),
                    IdReglasValidacionTipo = X.IdReglasValidacionTipo,
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
        #endregion
    }
}
