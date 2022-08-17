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
    public class IndicadorFonatelDAL : BitacoraDAL
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
                    "@pIdIndicador," + 
                    "@pCodigo," + 
                    "@pIdTipoIndicador," +
                    "@pIdClasificacion," + 
                    "@pIdGrupo," +
                    "@pIdUnidadEstudio," +
                    "@pIdTipoMedida," +
                    "@pIdFrecuencia," +
                    "@pIdEstado",
                     new SqlParameter("@pIdIndicador", pIndicador.idIndicador),
                     new SqlParameter("@pCodigo", string.IsNullOrEmpty(pIndicador.Codigo) ? DBNull.Value.ToString() : pIndicador.Codigo),
                     new SqlParameter("@pIdTipoIndicador", pIndicador.IdTipoIndicador),
                     new SqlParameter("@pIdClasificacion", pIndicador.IdClasificacion),
                     new SqlParameter("@pIdGrupo", pIndicador.idGrupo),
                     new SqlParameter("@pIdUnidadEstudio", pIndicador.IdUnidadEstudio != null ? pIndicador.IdUnidadEstudio : 0),
                     new SqlParameter("@pIdTipoMedida", pIndicador.idTipoMedida),
                     new SqlParameter("@pIdFrecuencia", pIndicador.IdFrecuencia),
                     new SqlParameter("@pIdEstado", pIndicador.idEstado)
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

        /// <summary>
        /// 16/08/2022
        /// José Navarro Acuña
        /// Método que retorna todos los formularios web relacionados a indicador.
        /// Se puede realizar un filtrado de acuerdo al objecto que se envia y obtener un compilado de varios indicadores.
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <returns></returns>
        public List<FormularioWeb> ObtenerFormulariosWebSegunIndicador(Indicador pIndicador)
        {
            List<FormularioWeb> listaFormularioWeb = new List<FormularioWeb>();

            using (db = new SIMEFContext())
            {
                listaFormularioWeb = db.Database.SqlQuery<FormularioWeb>
                    ("execute spObtenerFormulariosWebSegunIndicadorFonatel " +
                    "@pIdIndicador," +
                    "@pIdTipoIndicador," +
                    "@pIdClasificacion," +
                    "@pIdGrupo," +
                    "@pIdUnidadEstudio," +
                    "@pIdTipoMedida," +
                    "@pIdFrecuencia," +
                    "@pIdEstado",
                     new SqlParameter("@pIdIndicador", pIndicador.idIndicador),
                     new SqlParameter("@pIdTipoIndicador", pIndicador.IdTipoIndicador),
                     new SqlParameter("@pIdClasificacion", pIndicador.IdClasificacion),
                     new SqlParameter("@pIdGrupo", pIndicador.idGrupo),
                     new SqlParameter("@pIdUnidadEstudio", pIndicador.IdUnidadEstudio != null ? pIndicador.IdUnidadEstudio : 0),
                     new SqlParameter("@pIdTipoMedida", pIndicador.idTipoMedida),
                     new SqlParameter("@pIdFrecuencia", pIndicador.IdFrecuencia),
                     new SqlParameter("@pIdEstado", pIndicador.idEstado)
                    ).ToList();

                listaFormularioWeb = listaFormularioWeb.Select(x => new FormularioWeb()
                {
                    id = Utilidades.Encriptar(x.idFormulario.ToString()),
                    Codigo = x.Codigo,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    CantidadIndicadores = x.CantidadIndicadores,
                    FrecuenciaEnvio = ObtenerFrecuenciaEnvia(x.idFrecuencia),
                    FechaCreacion = x.FechaCreacion,
                    UsuarioCreacion = x.UsuarioCreacion,
                    FechaModificacion = x.FechaModificacion,
                    UsuarioModificacion = x.UsuarioModificacion,
                    EstadoRegistro = ObtenerEstadoRegistro(x.IdEstado)
                }).ToList();
            }

            return listaFormularioWeb;
        }

        /// <summary>
        /// 16/08/2022
        /// José Navarro Acuña
        /// Método que permite actualizar los datos de un indicador
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <returns></returns>
        public List<Indicador> ActualizarDatos(Indicador pIndicador)
        {
            List<Indicador> listaIndicadores = new List<Indicador>();

            using (db = new SIMEFContext())
            {
                listaIndicadores = db.Database.SqlQuery<Indicador>
                    ("execute spActualizarIndicadorFonatel " +
                    "@pIdIndicador," +
                    "@pCodigo," + // opcional
                    "@pNombre," + // opcional
                    "@pIdTipoIndicador," +
                    "@pIdClasificacion," +
                    "@pIdGrupo," +
                    "@pDescripcion," + // opcional
                    "@pCantidadVariableDato," + // opcional
                    "@pCantidadCategoriasDesagregacion," + // opcional
                    "@pIdUnidadEstudio," + // opcional
                    "@pIdTipoMedida," +
                    "@pIdFrecuencia," +
                    "@pInterno," + // opcional
                    "@pSolicitud," +
                    "@pFuente," + // opcional
                    "@pNotas," + // opcional
                    "@pUsuarioCreacion," +
                    "@pUsuarioModificacion," + // opcional
                    "@pVisualizaSigitel," +
                    "@pIdEstado",
                     new SqlParameter("@pIdIndicador", pIndicador.idIndicador),
                     new SqlParameter("@pCodigo", string.IsNullOrEmpty(pIndicador.Codigo) ? DBNull.Value.ToString() : pIndicador.Codigo),
                     new SqlParameter("@pNombre", string.IsNullOrEmpty(pIndicador.Nombre) ? DBNull.Value.ToString() : pIndicador.Nombre),
                     new SqlParameter("@pIdTipoIndicador", pIndicador.IdTipoIndicador),
                     new SqlParameter("@pIdClasificacion", pIndicador.IdClasificacion),
                     new SqlParameter("@pIdGrupo", pIndicador.idGrupo),
                     new SqlParameter("@pDescripcion", string.IsNullOrEmpty(pIndicador.Descripcion) ? DBNull.Value.ToString() : pIndicador.Descripcion),
                     new SqlParameter("@pCantidadVariableDato", pIndicador.CantidadVariableDato),
                     new SqlParameter("@pCantidadCategoriasDesagregacion", pIndicador.CantidadCategoriasDesagregacion),
                     new SqlParameter("@pIdUnidadEstudio", pIndicador.IdUnidadEstudio),
                     new SqlParameter("@pIdTipoMedida", pIndicador.idTipoMedida),
                     new SqlParameter("@pIdFrecuencia", pIndicador.IdFrecuencia),
                     new SqlParameter("@pInterno", pIndicador.Interno),
                     new SqlParameter("@pSolicitud", pIndicador.Solicitud),
                     new SqlParameter("@pFuente", string.IsNullOrEmpty(pIndicador.Fuente) ? DBNull.Value.ToString() : pIndicador.Fuente),
                     new SqlParameter("@pNotas", string.IsNullOrEmpty(pIndicador.Notas) ? DBNull.Value.ToString() : pIndicador.Notas),
                     new SqlParameter("@pUsuarioCreacion", pIndicador.UsuarioCreacion),
                     new SqlParameter("@pUsuarioModificacion", string.IsNullOrEmpty(pIndicador.UsuarioModificacion) ? DBNull.Value.ToString() : pIndicador.UsuarioModificacion),
                     new SqlParameter("@pVisualizaSigitel", pIndicador.VisualizaSigitel),
                     new SqlParameter("@pIdEstado", pIndicador.idEstado)
                    ).ToList();
            }

            return listaIndicadores;
        }

        #region Métodos privados
        private TipoIndicadores ObtenerTipoIndicador(int pId, bool pUnicamenteActivos = false)
        {
            TipoIndicadores tipoIndicadores = pUnicamenteActivos ?
                db.TipoIndicadores.Where(i => i.IdTipoIdicador == pId && i.Estado == true).FirstOrDefault()
                :
                db.TipoIndicadores.Where(i => i.IdTipoIdicador == pId).FirstOrDefault();

            if (tipoIndicadores != null)
            {
                tipoIndicadores.id = Utilidades.Encriptar(tipoIndicadores.IdTipoIdicador.ToString());
                tipoIndicadores.IdTipoIdicador = 0;
            }
            return tipoIndicadores;
        }

        private ClasificacionIndicadores ObtenerClasificacionIndicador(int pId, bool pUnicamenteActivos = false)
        {
            ClasificacionIndicadores clasificacion = pUnicamenteActivos ?
                db.ClasificacionIndicadores.Where(i => i.idClasificacion == pId && i.Estado == true).FirstOrDefault()
                :
                db.ClasificacionIndicadores.Where(i => i.idClasificacion == pId).FirstOrDefault();

            if (clasificacion != null)
            {
                clasificacion.id = Utilidades.Encriptar(clasificacion.idClasificacion.ToString());
                clasificacion.idClasificacion = 0;
            }
            return clasificacion;
        }

        private GrupoIndicadores ObtenerGrupoIndicadores(int pId, bool pUnicamenteActivos = false)
        {
            GrupoIndicadores grupo = pUnicamenteActivos ?
                db.GrupoIndicadores.Where(i => i.idGrupo == pId && i.Estado == true).FirstOrDefault()
                :
                db.GrupoIndicadores.Where(i => i.idGrupo == pId).FirstOrDefault();

            if (grupo != null)
            {
                grupo.id = Utilidades.Encriptar(grupo.idGrupo.ToString());
                grupo.idGrupo= 0;
            }
            return grupo;
        }

        private UnidadEstudio ObtenerUnidadEstudio(int pId, bool pUnicamenteActivos = false)
        {
            UnidadEstudio unidad = pUnicamenteActivos ?
                db.UnidadEstudio.Where(i => i.idUnidad == pId && i.Estado == true).FirstOrDefault()
                :
                db.UnidadEstudio.Where(i => i.idUnidad == pId).FirstOrDefault();

            if (unidad != null)
            {
                unidad.id = Utilidades.Encriptar(unidad.idUnidad.ToString());
                unidad.idUnidad = 0;
            }
            return unidad;
        }

        private TipoMedida ObtenerTipoMedida(int pId, bool pUnicamenteActivos = false)
        {
            TipoMedida tipoMedida = pUnicamenteActivos ?
                db.TipoMedida.Where(i => i.idMedida == pId && i.Estado == true).FirstOrDefault()
                :
                db.TipoMedida.Where(i => i.idMedida == pId).FirstOrDefault();

            if (tipoMedida != null)
            {
                tipoMedida.id = Utilidades.Encriptar(tipoMedida.idMedida.ToString());
                tipoMedida.idMedida = 0;
            }
            return tipoMedida;
        }

        private FrecuenciaEnvio ObtenerFrecuenciaEnvia(int pId, bool pUnicamenteActivos = false)
        {
            FrecuenciaEnvio frecuencia = pUnicamenteActivos ?
                db.FrecuenciaEnvio.Where(i => i.idFrecuencia == pId && i.Estado == true).FirstOrDefault()
                :
                db.FrecuenciaEnvio.Where(i => i.idFrecuencia == pId).FirstOrDefault();

            if (frecuencia != null)
            {
                frecuencia.id = Utilidades.Encriptar(frecuencia.idFrecuencia.ToString());
                frecuencia.idFrecuencia = 0;
            }
            return frecuencia;
        }

        private EstadoRegistro ObtenerEstadoRegistro(int pId, bool pUnicamenteActivos = false)
        {
            EstadoRegistro estado = pUnicamenteActivos ?
                db.EstadoRegistro.Where(i => i.idEstado == pId && i.Estado == true).FirstOrDefault()
                :
                db.EstadoRegistro.Where(i => i.idEstado == pId).FirstOrDefault();

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
