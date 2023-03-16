using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GB.SIMEF.Entities;
using GB.SIMEF.Resources;
using static GB.SIMEF.Resources.Constantes;

namespace GB.SIMEF.DAL
{
    public class IndicadorFonatelDAL : BitacoraDAL
    {
        private SIMEFContext db;

        /// <summary>
        /// 10/08/2022
        /// José Navarro Acuña
        /// Función que retorna todos los indicadores registrados en el sistema.
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
                    idIndicador=x.idIndicador,
                    Codigo = x.Codigo,
                    Nombre = x.Nombre,
                    IdClasificacion=x.IdClasificacion,
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
                    idEstado = x.idEstado,
                    EstadoRegistro = ObtenerEstadoRegistro(x.idEstado)
                }).ToList();
            }

            return listaIndicadores;
        }

        /// <summary>
        /// 25/11/2022
        /// José Navarro Acuña
        /// Función que retorna todos los indicadores registrados en el sistema de mercados.
        /// Se puede realizar un filtrado de acuerdo al objecto que se envia.
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <returns></returns>
        public List<Indicador> ObtenerDatosMercado(Indicador pIndicador, ServicioSitel pServicioSitel)
        {
            List<Indicador> listaIndicadores = new List<Indicador>();

            using (SIGITELContext db = new SIGITELContext())
            {
                listaIndicadores = db.Database.SqlQuery<Indicador>(
                    string.Format(
                        "select distinct IdIndicador, Codigo, Nombre from [FONATEL].[viewIndicadorDGM] " +
                        "where IdTipoIndicador = {0} and IdServicio = {1} and Agrupacion = '{2}'", 
                        pIndicador.TipoIndicadores.IdTipoIndicador,
                        pServicioSitel.IdServicio,
                        pIndicador.GrupoIndicadores.Nombre
                        )
                    ).ToList();
            }

            listaIndicadores = listaIndicadores.Select(x => new Indicador() {
                id = Utilidades.Encriptar(x.idIndicador.ToString()),
                Codigo = x.Codigo,
                Nombre = x.Nombre,
            }).ToList();

            return listaIndicadores;
        }

        /// <summary>
        /// 25/11/2022
        /// José Navarro Acuña
        /// Función que retorna todos los indicadores registrados en el sistema de calidad.
        /// Se puede realizar un filtrado de acuerdo al objecto que se envia.
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <returns></returns>
        public List<Indicador> ObtenerDatosCalidad(Indicador pIndicador, ServicioSitel pServicioSitel)
        {
            List<IndicadorSitel> listaIndicadoresCalidad = new List<IndicadorSitel>();
            List<Indicador> listaIndicadores = new List<Indicador>();

            using (CALIDADContext db = new CALIDADContext())
            {
                listaIndicadoresCalidad = db.Database.SqlQuery<IndicadorSitel>(
                    string.Format(
                        "select distinct IdIndicador, Codigo, Nombre from [FONATEL].[viewIndicadorDGC] " +
                        "where IdTipoIndicador = {0} and IdServicio = {1} and Agrupacion = '{2}'",
                        pIndicador.TipoIndicadores.IdTipoIndicador,
                        pServicioSitel.IdServicio,
                        pIndicador.GrupoIndicadores.Nombre
                        )
                    ).ToList();
            }

            listaIndicadores = listaIndicadoresCalidad.Select(x => new Indicador()
            {
                id = Utilidades.Encriptar(x.IdIndicador.ToString()),
                Codigo = x.Codigo,
                Nombre = x.Nombre
            }).ToList();

            return listaIndicadores;
        }
        
        /// <summary>
        /// 20/12/2022
        /// José Navarro Acuña
        /// Función que retorna todos los indicadores registrados en el sistema de UIT
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <param name="pServicioSitel"></param>
        /// <returns></returns>
        public List<Indicador> ObtenerDatosUIT(Indicador pIndicador, ServicioSitel pServicioSitel)
        {
            List<Indicador> listaIndicadores = new List<Indicador>();

            using (SITELContext db = new SITELContext())
            {
                listaIndicadores = db.Database.SqlQuery<Indicador>(
                    string.Format(
                        "select distinct IdIndicador, Codigo, Nombre from [FONATEL].[viewIndicadorUIT] " +
                        "where IdTipoIndicador = {0} and IdServicio = {1} and Agrupacion = '{2}'",
                        pIndicador.TipoIndicadores.IdTipoIndicador,
                        pServicioSitel.IdServicio,
                        pIndicador.GrupoIndicadores.Nombre
                        )
                    ).ToList();
            }

            listaIndicadores = listaIndicadores.Select(x => new Indicador()
            {
                id = Utilidades.Encriptar(x.idIndicador.ToString()),
                Codigo = x.Codigo,
                Nombre = x.Nombre
            }).ToList();

            return listaIndicadores;
        }

        /// <summary>
        /// 20/12/2022
        /// José Navarro Acuña
        /// Función que retorna todos los indicadores cruzados registrados
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <param name="pServicioSitel"></param>
        /// <returns></returns>
        public List<Indicador> ObtenerDatosCruzados(Indicador pIndicador, ServicioSitel pServicioSitel)
        {
            List<IndicadorSitel> listaIndicadoresCruzados = new List<IndicadorSitel>();
            List<Indicador> listaIndicadores = new List<Indicador>();

            using (SITELContext db = new SITELContext())
            {
                listaIndicadoresCruzados = db.Database.SqlQuery<IndicadorSitel>(
                    string.Format(
                        "select distinct IdIndicador, Codigo, Nombre from [FONATEL].[viewIndicadorCruzado] " +
                        "where IdTipoIndicador = {0} and IdServicio = {1} and Agrupacion = '{2}'",
                        pIndicador.TipoIndicadores.IdTipoIndicador,
                        pServicioSitel.IdServicio,
                        pIndicador.GrupoIndicadores.Nombre
                        )
                    ).ToList();
            }

            listaIndicadores = listaIndicadoresCruzados.Select(x => new Indicador()
            {
                id = Utilidades.Encriptar(x.IdIndicador.ToString()),
                Codigo = x.Codigo,
                Nombre = x.Nombre
            }).ToList();

            return listaIndicadores;
        }

        /// <summary>
        /// 19/12/2022
        /// José Navarro Acuña
        /// Función que retorna todos los indicadores de fuente externa
        /// </summary>
        /// <returns></returns>
        public List<Indicador> ObtenerDatosFuenteExterna()
        {
            List<Indicador> listaIndicadores = new List<Indicador>();

            using (SITELContext db = new SITELContext())
            {
                listaIndicadores = db.Database.SqlQuery<Indicador>(
                    "select IdIndicador, id, Nombre from [FONATEL].[viewIndicadorFuenteExterna]"
                    ).ToList();
            }

            listaIndicadores = listaIndicadores.Select(x => new Indicador()
            {
                id = Utilidades.Encriptar(x.idIndicador.ToString()),
                Codigo = x.Codigo,
                Nombre = x.Nombre
            }).ToList();

            return listaIndicadores;
        }

        /// <summary>
        /// 08/09/2022
        /// José Navarro Acuña
        /// Función que verifica si el indicador se encuentra en algún formulario web o una formula de calculo.
        /// Retorna una cadena de texto con un listado indicando las dependencias según corresponda
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <returns></returns>
        public List<string> VerificarDependenciasIndicador(Indicador pIndicador)
        {
            List<string> listaValidacion = new List<string>();

            using (db = new SIMEFContext())
            {
                listaValidacion = db.Database.SqlQuery<string>
                    ("exec spValidarUsoIndicadorFonatel @pIdIndicador",
                       new SqlParameter("@pIdIndicador", pIndicador.idIndicador)
                    ).ToList();
            }

            return listaValidacion;
        }

        /// <summary>
        /// 16/08/2022
        /// José Navarro Acuña
        /// Función que permite actualizar los datos de un indicador. Si el indicador no existe se crea el registro
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
                    "@pCodigo," +
                    "@pNombre," +
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
                     new SqlParameter("@pCodigo", string.IsNullOrEmpty(pIndicador.Codigo) ? DBNull.Value.ToString() : pIndicador.Codigo.Trim()),
                     new SqlParameter("@pNombre", string.IsNullOrEmpty(pIndicador.Nombre) ? DBNull.Value.ToString() : pIndicador.Nombre.Trim()),
                     new SqlParameter("@pIdTipoIndicador", pIndicador.IdTipoIndicador),
                     new SqlParameter("@pIdClasificacion", pIndicador.IdClasificacion),
                     new SqlParameter("@pIdGrupo", pIndicador.idGrupo),
                     new SqlParameter("@pDescripcion", string.IsNullOrEmpty(pIndicador.Descripcion) ? DBNull.Value.ToString() : pIndicador.Descripcion),
                     pIndicador.CantidadVariableDato == null ? 
                        new SqlParameter("@pCantidadVariableDato", DBNull.Value) 
                        : 
                        new SqlParameter("@pCantidadVariableDato", pIndicador.CantidadVariableDato),
                     pIndicador.CantidadCategoriasDesagregacion == null ?
                        new SqlParameter("@pCantidadCategoriasDesagregacion", DBNull.Value)
                        :
                        new SqlParameter("@pCantidadCategoriasDesagregacion", pIndicador.CantidadCategoriasDesagregacion),
                     new SqlParameter("@pIdUnidadEstudio", pIndicador.IdUnidadEstudio),
                     new SqlParameter("@pIdTipoMedida", pIndicador.idTipoMedida),
                     new SqlParameter("@pIdFrecuencia", pIndicador.IdFrecuencia),
                     new SqlParameter("@pInterno", pIndicador.Interno),
                     new SqlParameter("@pSolicitud", pIndicador.Solicitud),
                     new SqlParameter("@pFuente", string.IsNullOrEmpty(pIndicador.Fuente) ? DBNull.Value.ToString() : pIndicador.Fuente),
                     new SqlParameter("@pNotas", string.IsNullOrEmpty(pIndicador.Notas) ? DBNull.Value.ToString() : pIndicador.Notas),
                     new SqlParameter("@pUsuarioCreacion", pIndicador.UsuarioCreacion),
                     string.IsNullOrEmpty(pIndicador.UsuarioModificacion) ?
                        new SqlParameter("@pUsuarioModificacion", DBNull.Value)
                        :
                        new SqlParameter("@pUsuarioModificacion", pIndicador.UsuarioModificacion),
                     new SqlParameter("@pVisualizaSigitel", pIndicador.VisualizaSigitel),
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
        /// 
        /// Michael Hernéndez Cordero 
        /// Activa o desactiva la visalización en sigitel
        /// </summary>
        /// <param name="pIndicador"></param>
        /// <returns></returns>
        public List<Indicador> PublicacionSigitel(Indicador pIndicador)
        {
            List<Indicador> listaIndicadores = new List<Indicador>();

            using (db = new SIMEFContext())
            {
                listaIndicadores = db.Database.SqlQuery<Indicador>
                    ("execute spActualizarPublicadoSigitel " +
                    "@pIdIndicador," +
                    "@pUsuarioModificacion," + // opcional
                    "@pVisualizaSigitel",
                     new SqlParameter("@pIdIndicador", pIndicador.idIndicador),
                     new SqlParameter("@pUsuarioModificacion", string.IsNullOrEmpty(pIndicador.UsuarioModificacion) ? DBNull.Value.ToString() : pIndicador.UsuarioModificacion),
                     new SqlParameter("@pVisualizaSigitel", pIndicador.VisualizaSigitel)
                    ).ToList();
            }

            return listaIndicadores;
        }

        /// <summary>
        /// 03/10/2022
        /// José Andrés Navarro
        /// Función que clona los detalles de variables dato y detalles categorias de un indicador hacia otro indicador
        /// </summary>
        /// <param name="pIdIndicadorAClonar"></param>
        /// <param name="pIdIndicadorDestino"></param>
        /// <returns></returns>
        public bool ClonarDetallesDeIndicador(int pIdIndicadorAClonar, int pIdIndicadorDestino)
        {
            using (db = new SIMEFContext())
            {
                db.Database.SqlQuery<object>
                    ("execute spClonarDetallesDeIndicador @pIdIndicadorAClonar, @pIdIndicadorDestino",
                     new SqlParameter("@pIdIndicadorAClonar", pIdIndicadorAClonar),
                     new SqlParameter("@pIdIndicadorDestino", pIdIndicadorDestino)
                    ).ToList();
            }

            return true;
        }

        /// <summary>
        /// 29/08/2022
        /// José Navarro Acuña
        /// Función que permite buscar y verificar por código o nombre la existencia de un indicador en estado diferente de eliminado,
        /// </summary>
        /// <param name="pIndicador"></param>
        public Indicador VerificarExistenciaIndicadorPorCodigoNombre(Indicador pIndicador)
        {
            Indicador indicador = null;

            using (db = new SIMEFContext())
            {
                indicador = db.Indicador.Where(x =>
                        (x.Nombre.Trim().ToUpper().Equals(pIndicador.Nombre.Trim().ToUpper()) || x.Codigo.Trim().ToUpper().Equals(pIndicador.Codigo.Trim().ToUpper())) &&
                        x.idIndicador != pIndicador.idIndicador &&
                        x.idEstado != (int)EstadosRegistro.Eliminado
                    ).FirstOrDefault();
            }

            return indicador;
        }

        /// <summary>
        /// 29/08/2022
        /// José Navarro Acuña
        /// Función que permite buscar y verificar por medio del identificador la existencia de un indicador en estado diferente de eliminado.
        /// Importante: No encripta IDs
        /// </summary>
        /// <param name="pIdIdentificador"></param>
        /// <returns></returns>
        public Indicador VerificarExistenciaIndicadorPorID(int pIdIdentificador)
        {
            Indicador indicador = null;

            using (db = new SIMEFContext())
            {
                indicador = db.Indicador.Where(x => x.idIndicador == pIdIdentificador && x.idEstado != (int) EstadosRegistro.Eliminado).FirstOrDefault();
            }

            return indicador;
        }

        #region Métodos privados

        /// <summary>
        /// 16/08/2022
        /// José Navarro Acuña
        /// Función que retorna los tipos de indicadores
        /// </summary>
        /// <param name="pId"></param>
        /// <param name="pUnicamenteActivos"></param>
        /// <returns></returns>
        private TipoIndicadores ObtenerTipoIndicador(int pId, bool pUnicamenteActivos = false)
        {
            TipoIndicadores tipoIndicadores = pUnicamenteActivos ?
                db.TipoIndicadores.Where(i => i.IdTipoIndicador == pId && i.Estado == true).FirstOrDefault()
                :
                db.TipoIndicadores.Where(i => i.IdTipoIndicador == pId).FirstOrDefault();

            if (tipoIndicadores != null)
            {
                tipoIndicadores.id = Utilidades.Encriptar(tipoIndicadores.IdTipoIndicador.ToString());
                tipoIndicadores.IdTipoIndicador = 0;
            }
            return tipoIndicadores;
        }

        /// <summary>
        /// 16/08/2022
        /// José Navarro Acuña
        /// Función que retorna las clasificaciones de indicadores
        /// </summary>
        /// <param name="pId"></param>
        /// <param name="pUnicamenteActivos"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 16/08/2022
        /// José Navarro Acuña
        /// Función que retorna los grupos de indicadores
        /// </summary>
        /// <param name="pId"></param>
        /// <param name="pUnicamenteActivos"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 16/08/2022
        /// José Navarro Acuña
        /// Función que retorna las unidades de estudio de los indicadores
        /// </summary>
        /// <param name="pId"></param>
        /// <param name="pUnicamenteActivos"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 16/08/2022
        /// José Navarro Acuña
        /// Función que retorna los tipos de medidas de los indicadores
        /// </summary>
        /// <param name="pId"></param>
        /// <param name="pUnicamenteActivos"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 16/08/2022
        /// José Navarro Acuña
        /// Función que retorna los tipos de frecuencias de envio de de indicadores
        /// </summary>
        /// <param name="pId"></param>
        /// <param name="pUnicamenteActivos"></param>
        /// <returns></returns>
        private FrecuenciaEnvio ObtenerFrecuenciaEnvia(int pId, bool pUnicamenteActivos = false)
        {
            FrecuenciaEnvio frecuencia = pUnicamenteActivos ?
                db.FrecuenciaEnvio.Where(i => i.idFrecuenciaEnvio == pId && i.Estado == true).FirstOrDefault()
                :
                db.FrecuenciaEnvio.Where(i => i.idFrecuenciaEnvio == pId).FirstOrDefault();

            if (frecuencia != null)
            {
                frecuencia.id = Utilidades.Encriptar(frecuencia.idFrecuenciaEnvio.ToString());
                frecuencia.idFrecuenciaEnvio = 0;
            }
            return frecuencia;
        }

        /// <summary>
        /// 16/08/2022
        /// José Navarro Acuña
        /// Función que retorna los tipos de estados de registro
        /// </summary>
        /// <param name="pId"></param>
        /// <param name="pUnicamenteActivos"></param>
        /// <returns></returns>
        private EstadoRegistro ObtenerEstadoRegistro(int pId, bool pUnicamenteActivos = false)
        {
            EstadoRegistro estado = pUnicamenteActivos ?
                db.EstadoRegistro.Where(i => i.IdEstadoRegistro == pId && i.Estado == true).FirstOrDefault()
                :
                db.EstadoRegistro.Where(i => i.IdEstadoRegistro == pId).FirstOrDefault();

            if (estado != null)
            {
                estado.id = Utilidades.Encriptar(estado.IdEstadoRegistro.ToString());
                estado.IdEstadoRegistro = 0;
            }
            return estado;
        }

        /// <summary>
        /// 20/12/2022
        /// José Navarro Acuña
        /// Clase privada del modelo DAL para el consumo de la vista que consulta los indicadores
        /// </summary>
        private class IndicadorSitel
        {
            public string IdIndicador { get; set; } // la diferencia es que los IDs son alfanumericos
            public string Nombre { get; set; }
            public string Codigo { get; set; }
        }
        #endregion
    }
}
