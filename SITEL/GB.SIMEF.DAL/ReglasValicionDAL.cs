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
    public class ReglasValicionDAL: BitacoraDAL
    {
        private  SIMEFContext db;
        #region Metodos Consulta Base de Datos
        /// <summary>
        /// Metodo que carga los registros de categorias de desagregación tipo texto
        /// fecha 03-08-2022
        /// Michael Hernandez
        /// </summary>
        /// <returns>Lista</returns>
        /// 
        
        public List<ReglaValidacion> ObtenerDatos(ReglaValidacion objReglas)
        {
            List<ReglaValidacion> ListaCategoriaDetalle = new List<ReglaValidacion>();
            using (db = new SIMEFContext())
            {
                ListaCategoriaDetalle = db.Database.SqlQuery<ReglaValidacion>
                    ("execute spObtenerReglasValidacion @idRegla,@Codigo,@idIndicador,@IdTipo,@idEstado",
                      new SqlParameter("@idRegla", objReglas.idRegla),
                      new SqlParameter("@Codigo", string.IsNullOrEmpty(objReglas.Codigo)? DBNull.Value.ToString(): objReglas.Codigo),
                      new SqlParameter("@idIndicador", objReglas.idIndicador),
                      new SqlParameter("@IdTipo", objReglas.IdTipo),
                      new SqlParameter("@idEstado", objReglas.idEstado)
                    ).ToList();

                ListaCategoriaDetalle = ListaCategoriaDetalle.Select(x => new ReglaValidacion()
                {
                    id = Utilidades.Encriptar(x.idRegla.ToString()),
                    idRegla = x.idRegla,
                    Codigo = x.Codigo,
                    Nombre=x.Nombre,
                    Descripcion = x.Descripcion,
                    IdTipo = x.IdTipo,
                    idOperador = x.idOperador,
                    idIndicador = x.idIndicador,
                    FechaCreacion = x.FechaCreacion,
                    FechaModificacion = x.FechaModificacion,
                    idEstado = x.idEstado,
                    UsuarioCreacion = x.UsuarioCreacion,
                    UsuarioModificacion = x.UsuarioModificacion,
                    EstadoRegistro = db.EstadoRegistro.Where(i => i.idEstado == x.idEstado).Single(),
                    TipoReglaValidacion = db.TipoReglaValidacion.Where(i => i.IdTipo == x.IdTipo).Single()
                }).ToList();
            }
            return ListaCategoriaDetalle;
        }


        #endregion
    }
}
