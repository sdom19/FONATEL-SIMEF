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
    public class FormularioWebDAL : BitacoraDAL
    {
        private SIMEFContext db;
        #region Metodos Consulta Base de Datos

        /// <summary>
        /// 
        /// fecha 24-08-2022
        /// 
        /// </summary>
        /// <returns>Lista</returns>
        public List<FormularioWeb> ObtenerDatos(FormularioWeb objFormulario)
        {
            List<FormularioWeb> ListaFormulariosWeb = new List<FormularioWeb>();
            using (db = new SIMEFContext())
            {
                ListaFormulariosWeb = db.Database.SqlQuery<FormularioWeb>
                    ("execute pa_ObtenerFormularioWeb @idFormularioWeb, @idEstado, @codigo",
                    new SqlParameter("@idFormularioWeb", objFormulario.idFormularioWeb),
                    new SqlParameter("@idEstado", objFormulario.idEstadoRegistro),
                    new SqlParameter("@codigo", string.IsNullOrEmpty(objFormulario.Codigo) ? DBNull.Value.ToString() : objFormulario.Codigo)
                    ).ToList();
               
                ListaFormulariosWeb = ListaFormulariosWeb.Select( x => new FormularioWeb()
                {
                    id = Utilidades.Encriptar(x.idFormularioWeb.ToString()),
                    idFormularioWeb =x.idFormularioWeb,
                    Codigo = x.Codigo,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    CantidadIndicador = x.CantidadIndicador,
                    idFrecuenciaEnvio = x.idFrecuenciaEnvio,
                    FechaCreacion = x.FechaCreacion,
                    UsuarioCreacion = x.UsuarioCreacion,
                    FechaModificacion = x.FechaModificacion,
                    UsuarioModificacion = x.UsuarioModificacion,
                    idEstadoRegistro = x.idEstadoRegistro,
                    ListaIndicadores = ObtenerIndicadoresXFormulario(x.idFormularioWeb), 
                    EstadoRegistro = db.EstadoRegistro.Where(i => i.IdEstadoRegistro == x.idEstadoRegistro).FirstOrDefault(),
                    FrecuenciaEnvio = db.FrecuenciaEnvio.Where(i => i.IdFrecuenciaEnvio == x.idFrecuenciaEnvio).FirstOrDefault(),
                    DetalleFormularioWeb = ListaDetalleFormularioWeb(x.idFormularioWeb),
                }).ToList();
            }
            return ListaFormulariosWeb;
        }

        public List<Indicador> ObtenerIndicadoresFormulario(int idFormularioWeb)
        {

            List<Indicador> listaIndicadores = new List<Indicador>();
            using (db = new SIMEFContext())
            {
                listaIndicadores = db.Database.SqlQuery<Indicador>(
                    "execute pa_ObtenerListaIndicadorXFormulario @idFormularioWeb",
                    new SqlParameter("@idFormularioWeb", idFormularioWeb)
                    ).ToList();
                listaIndicadores = listaIndicadores.Select(x => new Indicador()
                {
                    IdIndicador = x.IdIndicador,
                    Codigo = x.Codigo,
                    Nombre = x.Nombre,
                    IdGrupoIndicador = x.IdGrupoIndicador,
                    IdTipoIndicador = x.IdTipoIndicador,
                    IdEstadoRegistro = x.IdEstadoRegistro,
                    GrupoIndicadores = db.GrupoIndicador.Where(g => g.IdGrupoIndicador == x.IdGrupoIndicador).FirstOrDefault(),
                    TipoIndicadores = db.TipoIndicadores.Where(i => i.IdTipoIndicador == x.IdTipoIndicador).FirstOrDefault(),
                    EstadoRegistro = db.EstadoRegistro.Where(i => i.IdEstadoRegistro == x.IdEstadoRegistro).FirstOrDefault(),
                }).ToList();
            }
            return listaIndicadores;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 

        public List<FormularioWeb> ActualizarDatos(FormularioWeb objFormulario)
        {
            List<FormularioWeb> ListaFormulariosWeb = new List<FormularioWeb>();
            using (db = new SIMEFContext())
            {
                ListaFormulariosWeb = db.Database.SqlQuery<FormularioWeb>
                    ("execute pa_ActualizarFormularioWeb @idFormularioWeb, @Codigo, @Nombre, @Descripcion, @CantidadIndicadores, @idFrecuencia, @UsuarioCreacion, @UsuarioModificacion, @idEstadoRegistro",
                    new SqlParameter("@idFormularioWeb", objFormulario.idFormularioWeb),
                    new SqlParameter("@Codigo", string.IsNullOrEmpty(objFormulario.Codigo) ? DBNull.Value.ToString() : objFormulario.Codigo),
                    new SqlParameter("@Nombre", string.IsNullOrEmpty(objFormulario.Nombre) ? DBNull.Value.ToString() : objFormulario.Nombre),
                    new SqlParameter("@Descripcion", string.IsNullOrEmpty(objFormulario.Descripcion) ? DBNull.Value.ToString() : objFormulario.Descripcion),
                    new SqlParameter("@CantidadIndicadores", objFormulario.CantidadIndicador),
                    new SqlParameter("@idFrecuencia", objFormulario.idFrecuenciaEnvio),
                    new SqlParameter("@UsuarioCreacion", string.IsNullOrEmpty(objFormulario.UsuarioCreacion) ? DBNull.Value.ToString() : objFormulario.UsuarioCreacion),
                    new SqlParameter("@UsuarioModificacion", string.IsNullOrEmpty(objFormulario.UsuarioModificacion) ? DBNull.Value.ToString() : objFormulario.UsuarioModificacion),
                    new SqlParameter("@idEstadoRegistro", objFormulario.idEstadoRegistro)
                    ).ToList();

                ListaFormulariosWeb = ListaFormulariosWeb.Select(x => new FormularioWeb()
                {
                    id = Utilidades.Encriptar(x.idFormularioWeb.ToString()),
                    idFormularioWeb = x.idFormularioWeb,
                    Codigo = x.Codigo,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    CantidadIndicador = x.CantidadIndicador,
                    idFrecuenciaEnvio = x.idFrecuenciaEnvio,
                    FechaCreacion = x.FechaCreacion,
                    UsuarioCreacion = x.UsuarioCreacion,
                    FechaModificacion = x.FechaModificacion,
                    UsuarioModificacion = x.UsuarioModificacion,
                    idEstadoRegistro = x.idEstadoRegistro,
                    ListaIndicadores = ObtenerIndicadoresXFormulario(x.idFormularioWeb),
                    EstadoRegistro = db.EstadoRegistro.Where(i => i.IdEstadoRegistro == x.idEstadoRegistro).FirstOrDefault(),
                    FrecuenciaEnvio = db.FrecuenciaEnvio.Where(i => i.IdFrecuenciaEnvio == x.idFrecuenciaEnvio).FirstOrDefault(),
                    DetalleFormularioWeb = ListaDetalleFormularioWeb(x.idFormularioWeb),
                }).ToList();
            }
            return ListaFormulariosWeb;
        }

        private string ObtenerIndicadoresXFormulario(int id) {
            return db.Database.SqlQuery<string>
                    ("execute pa_ObtenerListadoIndicadorXFormulario @idFormularioWeb",
                    new SqlParameter("@idFormularioWeb", id)
                    ).Single();
        }

        /// <summary>
        /// 11/10/2022
        /// José Navarro Acuña
        /// Función que busca y retorna una lista de formularios web donde el Indicador proporcionado este relacionado
        /// </summary>
        /// <param name="pIdIndicador"></param>
        /// <returns></returns>
        public List<FormularioWeb> ObtenerDependenciasIndicadorConFormulariosWeb(int pIdIndicador)
        {
            List<FormularioWeb> lista = new List<FormularioWeb>();

            using (db = new SIMEFContext())
            {
                lista = db.Database.SqlQuery<FormularioWeb>
                    ("execute pa_ObtenerDependenciaIndicadorConFormulariosWeb @pIdIndicador",
                     new SqlParameter("@pIdIndicador", pIdIndicador)
                    ).ToList();

                lista = lista.Select(x => new FormularioWeb()
                {
                    id = Utilidades.Encriptar(x.idFormularioWeb.ToString()),
                    idFormularioWeb = x.idFormularioWeb,
                    Codigo = x.Codigo,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    CantidadIndicador = x.CantidadIndicador,
                    idFrecuenciaEnvio = x.idFrecuenciaEnvio,
                    FechaCreacion = x.FechaCreacion,
                    UsuarioCreacion = x.UsuarioCreacion,
                    FechaModificacion = x.FechaModificacion,
                    UsuarioModificacion = x.UsuarioModificacion,
                    idEstadoRegistro = x.idEstadoRegistro,
                }).ToList();
            }

            return lista;
        }

        #endregion

        private ICollection<DetalleFormularioWeb> ListaDetalleFormularioWeb(int id)
        {
            return db.DetalleFormularioWeb.Where(
                x => x.idFormularioWeb == id && x.Estado == true
                ).ToList();
        }




        /// Michael Hernández Cordero
        /// Valida dependencias con otras tablas
        /// 18/08/2022
        /// </summary>
        /// <param name="fuente"></param>
        /// <returns></returns>

        public List<string> ValidarFuente(FormularioWeb formulario)
        {
            List<string> listaValicion = new List<string>();
            using (db = new SIMEFContext())
            {
                listaValicion = db.Database.SqlQuery<string>
                    ("exec pa_ValidarFormulario @idFormularioWeb",
                       new SqlParameter("@idFormularioWeb", formulario.idFormularioWeb)
                    ).ToList();
            }

            return listaValicion;
        }
    }
}
