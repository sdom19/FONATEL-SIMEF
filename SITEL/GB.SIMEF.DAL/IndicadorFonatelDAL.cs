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
                    ("execute pa_ObtenerIndicadorFonatel " + 
                    "@pIdIndicador," + 
                    "@pCodigo," + 
                    "@pIdTipoIndicador," +
                    "@pIdClasificacionIndicador," +
                    "@pIdGrupoIndicador," +
                    "@pIdUnidadEstudio," +
                    "@pIdTipoMedida," +
                    "@pIdFrecuencia," +
                    "@pIdEstadoRegistro",
                     new SqlParameter("@pIdIndicador", pIndicador.IdIndicador),
                     new SqlParameter("@pCodigo", string.IsNullOrEmpty(pIndicador.Codigo) ? DBNull.Value.ToString() : pIndicador.Codigo),
                     new SqlParameter("@pIdTipoIndicador", pIndicador.IdTipoIndicador),
                     new SqlParameter("@pIdClasificacionIndicador", pIndicador.IdClasificacionIndicador),
                     new SqlParameter("@pIdGrupoIndicador", pIndicador.IdGrupoIndicador),
                     new SqlParameter("@pIdUnidadEstudio", pIndicador.IdUnidadEstudio != null ? pIndicador.IdUnidadEstudio : 0),
                     new SqlParameter("@pIdTipoMedida", pIndicador.IdTipoMedida),
                     new SqlParameter("@pIdFrecuencia", pIndicador.IdFrecuenciaEnvio),
                     new SqlParameter("@pIdEstadoRegistro", pIndicador.IdEstadoRegistro)
                    ).ToList();

                listaIndicadores = listaIndicadores.Select(x => new Indicador()
                {
                    id = Utilidades.Encriptar(x.IdIndicador.ToString()), 
                    IdIndicador=x.IdIndicador,
                    Codigo = x.Codigo,
                    Nombre = x.Nombre,
                    IdClasificacionIndicador=x.IdClasificacionIndicador,
                    TipoIndicadores = ObtenerTipoIndicador(x.IdTipoIndicador),
                    ClasificacionIndicadores = ObtenerClasificacionIndicador(x.IdClasificacionIndicador),
                    GrupoIndicadores = ObtenerGrupoIndicadores(x.IdGrupoIndicador),
                    Descripcion = x.Descripcion,
                    CantidadVariableDato = x.CantidadVariableDato,
                    CantidadCategoriaDesagregacion = x.CantidadCategoriaDesagregacion,
                    UnidadEstudio = x.IdUnidadEstudio != null ? ObtenerUnidadEstudio((int)x.IdUnidadEstudio) : null,
                    TipoMedida = ObtenerTipoMedida(x.IdTipoMedida),
                    FrecuenciaEnvio = ObtenerFrecuenciaEnvia(x.IdFrecuenciaEnvio),
                    Interno = x.Interno,
                    Solicitud = x.Solicitud,
                    Fuente = x.Fuente,
                    Nota = x.Nota,
                    FechaCreacion = x.FechaCreacion,
                    UsuarioCreacion = x.UsuarioCreacion,
                    FechaModificacion = x.FechaModificacion,
                    UsuarioModificacion = x.UsuarioModificacion,
                    VisualizaSigitel = x.VisualizaSigitel,
                    IdEstadoRegistro = x.IdEstadoRegistro,
                    EstadoRegistro = ObtenerEstadoRegistro(x.IdEstadoRegistro)
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
                        "select distinct IdIndicador, Codigo, Nombre from [FONATEL].[vw_IndicadorDGM] " +
                        "where IdTipoIndicador = {0} and IdServicio = {1} and Agrupacion = '{2}'", 
                        pIndicador.TipoIndicadores.IdTipoIndicador,
                        pServicioSitel.IdServicio,
                        pIndicador.GrupoIndicadores.Nombre
                        )
                    ).ToList();
            }

            listaIndicadores = listaIndicadores.Select(x => new Indicador() {
                id = Utilidades.Encriptar(x.IdIndicador.ToString()),
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
                id = Utilidades.Encriptar(x.IdIndicador.ToString()),
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
                id = Utilidades.Encriptar(x.IdIndicador.ToString()),
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
                    ("exec pa_ValidarUsoIndicadorFonatel @pIdIndicador",
                       new SqlParameter("@pIdIndicador", pIndicador.IdIndicador)
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
                    ("execute pa_ActualizarIndicadorFonatel " +
                    "@pIdIndicador," +
                    "@pCodigo," +
                    "@pNombre," +
                    "@pIdTipoIndicador," +
                    "@pIdClasificacionIndicador," +
                    "@pIdGrupoIndicador," +
                    "@pDescripcion," + // opcional
                    "@pCantidadVariableDato," + // opcional
                    "@pCantidadCategoriaDesagregacion," + // opcional
                    "@pIdUnidadEstudio," + // opcional
                    "@pIdTipoMedida," +
                    "@pIdFrecuencia," +
                    "@pInterno," + // opcional
                    "@pSolicitud," +
                    "@pFuente," + // opcional
                    "@pNota," + // opcional
                    "@pUsuarioCreacion," +
                    "@pUsuarioModificacion," + // opcional
                    "@pVisualizaSigitel," +
                    "@pIdEstadoRegistro",
                     new SqlParameter("@pIdIndicador", pIndicador.IdIndicador),
                     new SqlParameter("@pCodigo", string.IsNullOrEmpty(pIndicador.Codigo) ? DBNull.Value.ToString() : pIndicador.Codigo.Trim()),
                     new SqlParameter("@pNombre", string.IsNullOrEmpty(pIndicador.Nombre) ? DBNull.Value.ToString() : pIndicador.Nombre.Trim()),
                     new SqlParameter("@pIdTipoIndicador", pIndicador.IdTipoIndicador),
                     new SqlParameter("@pIdClasificacionIndicador", pIndicador.IdClasificacionIndicador),
                     new SqlParameter("@pIdGrupoIndicador", pIndicador.IdGrupoIndicador),
                     new SqlParameter("@pDescripcion", string.IsNullOrEmpty(pIndicador.Descripcion) ? DBNull.Value.ToString() : pIndicador.Descripcion),
                     pIndicador.CantidadVariableDato == null ? 
                        new SqlParameter("@pCantidadVariableDato", DBNull.Value) 
                        : 
                        new SqlParameter("@pCantidadVariableDato", pIndicador.CantidadVariableDato),
                     pIndicador.CantidadCategoriaDesagregacion == null ?
                        new SqlParameter("@pCantidadCategoriaDesagregacion", DBNull.Value)
                        :
                        new SqlParameter("@pCantidadCategoriaDesagregacion", pIndicador.CantidadCategoriaDesagregacion),
                     new SqlParameter("@pIdUnidadEstudio", pIndicador.IdUnidadEstudio),
                     new SqlParameter("@pIdTipoMedida", pIndicador.IdTipoMedida),
                     new SqlParameter("@pIdFrecuencia", pIndicador.IdFrecuenciaEnvio),
                     new SqlParameter("@pInterno", pIndicador.Interno),
                     new SqlParameter("@pSolicitud", pIndicador.Solicitud),
                     new SqlParameter("@pFuente", string.IsNullOrEmpty(pIndicador.Fuente) ? DBNull.Value.ToString() : pIndicador.Fuente),
                     new SqlParameter("@pNota", string.IsNullOrEmpty(pIndicador.Nota) ? DBNull.Value.ToString() : pIndicador.Nota),
                     new SqlParameter("@pUsuarioCreacion", pIndicador.UsuarioCreacion),
                     string.IsNullOrEmpty(pIndicador.UsuarioModificacion) ?
                        new SqlParameter("@pUsuarioModificacion", DBNull.Value)
                        :
                        new SqlParameter("@pUsuarioModificacion", pIndicador.UsuarioModificacion),
                     new SqlParameter("@pVisualizaSigitel", pIndicador.VisualizaSigitel),
                     new SqlParameter("@pIdEstadoRegistro", pIndicador.IdEstadoRegistro)
                    ).ToList();

                listaIndicadores = listaIndicadores.Select(x => new Indicador()
                {
                    id = Utilidades.Encriptar(x.IdIndicador.ToString()),
                    Codigo = x.Codigo,
                    Nombre = x.Nombre,
                    TipoIndicadores = ObtenerTipoIndicador(x.IdTipoIndicador),
                    ClasificacionIndicadores = ObtenerClasificacionIndicador(x.IdClasificacionIndicador),
                    GrupoIndicadores = ObtenerGrupoIndicadores(x.IdGrupoIndicador),
                    Descripcion = x.Descripcion,
                    CantidadVariableDato = x.CantidadVariableDato,
                    CantidadCategoriaDesagregacion = x.CantidadCategoriaDesagregacion,
                    UnidadEstudio = x.IdUnidadEstudio != null ? ObtenerUnidadEstudio((int)x.IdUnidadEstudio) : null,
                    TipoMedida = ObtenerTipoMedida(x.IdTipoMedida),
                    FrecuenciaEnvio = ObtenerFrecuenciaEnvia(x.IdFrecuenciaEnvio),
                    Interno = x.Interno,
                    Solicitud = x.Solicitud,
                    Fuente = x.Fuente,
                    Nota = x.Nota,
                    FechaCreacion = x.FechaCreacion,
                    UsuarioCreacion = x.UsuarioCreacion,
                    FechaModificacion = x.FechaModificacion,
                    UsuarioModificacion = x.UsuarioModificacion,
                    VisualizaSigitel = x.VisualizaSigitel,
                    EstadoRegistro = ObtenerEstadoRegistro(x.IdEstadoRegistro)
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
                    ("execute pa_ActualizarPublicadoSigitel " +
                    "@pIdIndicador," +
                    "@pUsuarioModificacion," + // opcional
                    "@pVisualizaSigitel",
                     new SqlParameter("@pIdIndicador", pIndicador.IdIndicador),
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
                    ("execute pa_ClonarDetalleDeIndicador @pIdIndicadorAClonar, @pIdIndicadorDestino",
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
                        x.IdIndicador != pIndicador.IdIndicador &&
                        x.IdEstadoRegistro != (int)EstadosRegistro.Eliminado
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
                indicador = db.Indicador.Where(x => x.IdIndicador == pIdIdentificador && x.IdEstadoRegistro != (int) EstadosRegistro.Eliminado).FirstOrDefault();
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
        private TipoIndicador ObtenerTipoIndicador(int pId, bool pUnicamenteActivos = false)
        {
            TipoIndicador tipoIndicadores = pUnicamenteActivos ?
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
        private ClasificacionIndicador ObtenerClasificacionIndicador(int pId, bool pUnicamenteActivos = false)
        {
            ClasificacionIndicador clasificacion = pUnicamenteActivos ?
                db.ClasificacionIndicadores.Where(i => i.IdClasificacionIndicador == pId && i.Estado == true).FirstOrDefault()
                :
                db.ClasificacionIndicadores.Where(i => i.IdClasificacionIndicador == pId).FirstOrDefault();

            if (clasificacion != null)
            {
                clasificacion.id = Utilidades.Encriptar(clasificacion.IdClasificacionIndicador.ToString());
                clasificacion.IdClasificacionIndicador = 0;
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
        private GrupoIndicador ObtenerGrupoIndicadores(int pId, bool pUnicamenteActivos = false)
        {
            GrupoIndicador grupo = pUnicamenteActivos ?
                db.GrupoIndicador.Where(i => i.IdGrupoIndicador == pId && i.Estado == true).FirstOrDefault()
                :
                db.GrupoIndicador.Where(i => i.IdGrupoIndicador == pId).FirstOrDefault();

            if (grupo != null)
            {
                grupo.id = Utilidades.Encriptar(grupo.IdGrupoIndicador.ToString());
                grupo.IdGrupoIndicador= 0;
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
                db.UnidadEstudio.Where(i => i.IdUnidadEstudio == pId && i.Estado == true).FirstOrDefault()
                :
                db.UnidadEstudio.Where(i => i.IdUnidadEstudio == pId).FirstOrDefault();

            if (unidad != null)
            {
                unidad.id = Utilidades.Encriptar(unidad.IdUnidadEstudio.ToString());
                unidad.IdUnidadEstudio = 0;
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
                db.TipoMedida.Where(i => i.IdTipoMedida == pId && i.Estado == true).FirstOrDefault()
                :
                db.TipoMedida.Where(i => i.IdTipoMedida == pId).FirstOrDefault();

            if (tipoMedida != null)
            {
                tipoMedida.id = Utilidades.Encriptar(tipoMedida.IdTipoMedida.ToString());
                tipoMedida.IdTipoMedida = 0;
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
                db.FrecuenciaEnvio.Where(i => i.IdFrecuenciaEnvio == pId && i.Estado == true).FirstOrDefault()
                :
                db.FrecuenciaEnvio.Where(i => i.IdFrecuenciaEnvio == pId).FirstOrDefault();

            if (frecuencia != null)
            {
                frecuencia.id = Utilidades.Encriptar(frecuencia.IdFrecuenciaEnvio.ToString());
                frecuencia.IdFrecuenciaEnvio = 0;
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
