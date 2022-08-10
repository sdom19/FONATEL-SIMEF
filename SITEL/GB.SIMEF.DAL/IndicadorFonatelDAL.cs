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
    public class IndicadorFonatelDAL
    {
        private SIMEFContext db;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objCategoria"></param>
        /// <returns></returns>
        public List<Indicador> ObtenerDatos(Indicador objIndicador)
        {
            List<Indicador> listaIndicadores = new List<Indicador>();
            
            using (db = new SIMEFContext())
            {
                listaIndicadores = db.Database.SqlQuery<Indicador>
                    ("execute spObtenerIndicadoresFonatel " + 
                    "@IdIndicador," + 
                    "@Codigo," + 
                    "@IdTipoIndicador," +
                    "@IdClasificacion," + 
                    "@IdGrupo," +
                    "@IdUnidadEstudio," +
                    "@IdTipoMedida," +
                    "@IdFrecuencia," +
                    "@IdEstado",
                     new SqlParameter("@IdIndicador", objIndicador.idIndicador),
                     new SqlParameter("@Codigo", string.IsNullOrEmpty(objIndicador.Codigo) ? DBNull.Value.ToString() : objIndicador.Codigo),
                     new SqlParameter("@IdTipoIndicador", objIndicador.IdTipoIndicador),
                     new SqlParameter("@IdClasificacion", objIndicador.IdClasificacion),
                     new SqlParameter("@IdGrupo", objIndicador.idGrupo),
                     new SqlParameter("@IdUnidadEstudio", objIndicador.IdUnidadEstudio != null ? objIndicador.IdUnidadEstudio : 0),
                     new SqlParameter("@IdTipoMedida", objIndicador.idTipoMedida),
                     new SqlParameter("@IdFrecuencia", objIndicador.IdFrecuencia),
                     new SqlParameter("@IdEstado", objIndicador.idEstado)
                    ).ToList();

                listaIndicadores = listaIndicadores.Select(x => new Indicador()
                {
                    idIndicador = x.idIndicador,
                    Codigo = x.Codigo,
                    Nombre = x.Nombre,
                    TipoIndicadores = db.TipoIndicadores.Where(i => i.IdTipoIdicador == x.IdTipoIndicador).Single(),
                    ClasificacionIndicadores = db.ClasificacionIndicadores.Where(i => i.idClasificacion == x.IdClasificacion).Single(),
                    GrupoIndicadores = db.GrupoIndicadores.Where(i => i.idGrupo == x.idGrupo).Single(),
                    Descripcion = x.Descripcion,
                    CantidadVariableDato = x.CantidadVariableDato,
                    CantidadCategoriasDesagregacion = x.CantidadCategoriasDesagregacion,
                    UnidadEstudio = db.UnidadEstudio.Where(i => i.idUnidad == x.IdUnidadEstudio).Single(),
                    TipoMedida = db.TipoMedida.Where(i => i.idMedida == x.idTipoMedida).Single(),
                    FrecuenciaEnvio = db.FrecuenciaEnvio.Where(i => i.idFrecuencia == x.IdFrecuencia).Single(),
                    Interno = x.Interno,
                    Solicitud = x.Solicitud,
                    Fuente = x.Fuente,
                    Notas = x.Notas,
                    FechaCreacion = x.FechaCreacion,
                    UsuarioCreacion = x.UsuarioCreacion,
                    FechaModificacion = x.FechaModificacion,
                    UsuarioModificacion = x.UsuarioModificacion,
                    VisualizaSigitel = x.VisualizaSigitel,
                    EstadoRegistro = db.EstadoRegistro.Where(i => i.idEstado == x.idEstado).Single()
                }).ToList();
            }

            return listaIndicadores;
        }
    }
}
