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
        /// 10/08/2022
        /// José Navarro Acuña
        /// Método que retorna todos los indicadores registrados en el sistema.
        /// Se puede realizar un filtrado de acuerdo al objecto que se envia.
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <returns></returns>
        public List<Indicador> ObtenerDatos(Indicador pIndicador)
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
                     new SqlParameter("@IdIndicador", pIndicador.idIndicador),
                     new SqlParameter("@Codigo", string.IsNullOrEmpty(pIndicador.Codigo) ? DBNull.Value.ToString() : pIndicador.Codigo),
                     new SqlParameter("@IdTipoIndicador", pIndicador.IdTipoIndicador),
                     new SqlParameter("@IdClasificacion", pIndicador.IdClasificacion),
                     new SqlParameter("@IdGrupo", pIndicador.idGrupo),
                     new SqlParameter("@IdUnidadEstudio", pIndicador.IdUnidadEstudio != null ? pIndicador.IdUnidadEstudio : 0),
                     new SqlParameter("@IdTipoMedida", pIndicador.idTipoMedida),
                     new SqlParameter("@IdFrecuencia", pIndicador.IdFrecuencia),
                     new SqlParameter("@IdEstado", pIndicador.idEstado)
                    ).ToList();

                listaIndicadores = listaIndicadores.Select(x => new Indicador()
                {
                    id = Utilidades.Encriptar(x.idIndicador.ToString()),
                    Codigo = x.Codigo,
                    Nombre = x.Nombre,
                    TipoIndicadores = ObtenerTipoIndicador(x.IdTipoIndicador),
                    ClasificacionIndicadores = ObtenerClasificacionIndicador(x.IdClasificacion),
                    GrupoIndicadores = ObtenerGrupoIndicadores(x.idGrupo),
                    Descripcion = x.Descripcion,
                    CantidadVariableDato = x.CantidadVariableDato,
                    CantidadCategoriasDesagregacion = x.CantidadCategoriasDesagregacion,
                    UnidadEstudio = x.IdUnidadEstudio != null ? ObtenerUnidadEstudio((int)x.IdUnidadEstudio) : null,
                    TipoMedida = ObtenerTipoMedida(x.idTipoMedida),
                    FrecuenciaEnvio = ObtenerFrecuenciaEnvia(x.IdFrecuencia),
                    Interno = x.Interno,
                    Solicitud = x.Solicitud,
                    Fuente = x.Fuente,
                    Notas = x.Notas,
                    FechaCreacion = x.FechaCreacion,
                    UsuarioCreacion = x.UsuarioCreacion,
                    FechaModificacion = x.FechaModificacion,
                    UsuarioModificacion = x.UsuarioModificacion,
                    VisualizaSigitel = x.VisualizaSigitel,
                    EstadoRegistro = ObtenerEstadoRegistro(x.idEstado)
                }).ToList();
            }

            return listaIndicadores;
        }

        #region Métodos privados
        private TipoIndicadores ObtenerTipoIndicador(int id, bool unicamenteActivos = false)
        {
            TipoIndicadores tipoIndicadores = unicamenteActivos ?
                db.TipoIndicadores.Where(i => i.IdTipoIdicador == id && i.Estado == true).FirstOrDefault()
                :
                db.TipoIndicadores.Where(i => i.IdTipoIdicador == id).FirstOrDefault();

            if (tipoIndicadores != null)
            {
                tipoIndicadores.id = Utilidades.Encriptar(tipoIndicadores.IdTipoIdicador.ToString());
                tipoIndicadores.IdTipoIdicador = 0;
            }
            return tipoIndicadores;
        }

        private ClasificacionIndicadores ObtenerClasificacionIndicador(int id, bool unicamenteActivos = false)
        {
            ClasificacionIndicadores clasificacion = unicamenteActivos ?
                db.ClasificacionIndicadores.Where(i => i.idClasificacion == id && i.Estado == true).FirstOrDefault()
                :
                db.ClasificacionIndicadores.Where(i => i.idClasificacion == id).FirstOrDefault();

            if (clasificacion != null)
            {
                clasificacion.id = Utilidades.Encriptar(clasificacion.idClasificacion.ToString());
                clasificacion.idClasificacion = 0;
            }
            return clasificacion;
        }

        private GrupoIndicadores ObtenerGrupoIndicadores(int id, bool unicamenteActivos = false)
        {
            GrupoIndicadores grupo = unicamenteActivos ?
                db.GrupoIndicadores.Where(i => i.idGrupo == id && i.Estado == true).FirstOrDefault()
                :
                db.GrupoIndicadores.Where(i => i.idGrupo == id).FirstOrDefault();

            if (grupo != null)
            {
                grupo.id = Utilidades.Encriptar(grupo.idGrupo.ToString());
                grupo.idGrupo= 0;
            }
            return grupo;
        }

        private UnidadEstudio ObtenerUnidadEstudio(int id, bool unicamenteActivos = false)
        {
            UnidadEstudio unidad = unicamenteActivos ?
                db.UnidadEstudio.Where(i => i.idUnidad == id && i.Estado == true).FirstOrDefault()
                :
                db.UnidadEstudio.Where(i => i.idUnidad == id).FirstOrDefault();

            if (unidad != null)
            {
                unidad.id = Utilidades.Encriptar(unidad.idUnidad.ToString());
                unidad.idUnidad = 0;
            }
            return unidad;
        }

        private TipoMedida ObtenerTipoMedida(int id, bool unicamenteActivos = false)
        {
            TipoMedida tipoMedida = unicamenteActivos ?
                db.TipoMedida.Where(i => i.idMedida == id && i.Estado == true).FirstOrDefault()
                :
                db.TipoMedida.Where(i => i.idMedida == id).FirstOrDefault();

            if (tipoMedida != null)
            {
                tipoMedida.id = Utilidades.Encriptar(tipoMedida.idMedida.ToString());
                tipoMedida.idMedida = 0;
            }
            return tipoMedida;
        }

        private FrecuenciaEnvio ObtenerFrecuenciaEnvia(int id, bool unicamenteActivos = false)
        {
            FrecuenciaEnvio frecuencia = unicamenteActivos ?
                db.FrecuenciaEnvio.Where(i => i.idFrecuencia == id && i.Estado == true).FirstOrDefault()
                :
                db.FrecuenciaEnvio.Where(i => i.idFrecuencia == id).FirstOrDefault();

            if (frecuencia != null)
            {
                frecuencia.id = Utilidades.Encriptar(frecuencia.idFrecuencia.ToString());
                frecuencia.idFrecuencia = 0;
            }
            return frecuencia;
        }

        private EstadoRegistro ObtenerEstadoRegistro(int id, bool unicamenteActivos = false)
        {
            EstadoRegistro estado = unicamenteActivos ?
                db.EstadoRegistro.Where(i => i.idEstado == id && i.Estado == true).FirstOrDefault()
                :
                db.EstadoRegistro.Where(i => i.idEstado == id).FirstOrDefault();

            if (estado != null)
            {
                estado.id = Utilidades.Encriptar(estado.idEstado.ToString());
                estado.idEstado = 0;
            }
            return estado;
        }
        #endregion
    }
}
