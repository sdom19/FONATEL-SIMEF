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
                    ("execute spObtenerFormulariosWeb @idFormulario, @idEstado, @codigo",
                    new SqlParameter("@idFormulario", objFormulario.idFormulario),
                    new SqlParameter("@idEstado", objFormulario.idEstado),
                    new SqlParameter("@codigo", string.IsNullOrEmpty(objFormulario.Codigo) ? DBNull.Value.ToString() : objFormulario.Codigo)
                    ).ToList();
               
                ListaFormulariosWeb = ListaFormulariosWeb.Select( x => new FormularioWeb()
                {
                    id = Utilidades.Encriptar(x.idFormulario.ToString()),
                    idFormulario=x.idFormulario,
                    Codigo = x.Codigo,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    CantidadIndicadores = x.CantidadIndicadores,
                    idFrecuencia = x.idFrecuencia,
                    FechaCreacion = x.FechaCreacion,
                    UsuarioCreacion = x.UsuarioCreacion,
                    FechaModificacion = x.FechaModificacion,
                    UsuarioModificacion = x.UsuarioModificacion,
                    idEstado = x.idEstado,
                    ListaIndicadores = ObtenerIndicadoresXFormulario(x.idFormulario), 
                    EstadoRegistro = db.EstadoRegistro.Where(i => i.IdEstadoRegistro == x.idEstado).FirstOrDefault(),
                    FrecuenciaEnvio = db.FrecuenciaEnvio.Where(i => i.IdFrecuenciaEnvio == x.idFrecuencia).FirstOrDefault(),
                    DetalleFormularioWeb = ListaDetalleFormularioWeb(x.idFormulario),
                }).ToList();
            }
            return ListaFormulariosWeb;
        }

        public List<Indicador> ObtenerIndicadoresFormulario(int idFormulario)
        {

            List<Indicador> listaIndicadores = new List<Indicador>();
            using (db = new SIMEFContext())
            {
                listaIndicadores = db.Database.SqlQuery<Indicador>(
                    "execute spObtenerListaIndicadoresXFormulario @idFormulario",
                    new SqlParameter("@idFormulario", idFormulario)
                    ).ToList();
                listaIndicadores = listaIndicadores.Select(x => new Indicador()
                {
                    IdIndicador = x.IdIndicador,
                    Codigo = x.Codigo,
                    Nombre = x.Nombre,
                    IdGrupoIndicador = x.IdGrupoIndicador,
                    IdTipoIndicador = x.IdTipoIndicador,
                    IdEstadoRegistro = x.IdEstadoRegistro,
                    GrupoIndicadores = db.GrupoIndicadores.Where(g => g.IdGrupoIndicador == x.IdGrupoIndicador).FirstOrDefault(),
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
                    ("execute spActualizarFormularioWeb @idFormulario, @Codigo, @Nombre, @Descripcion, @CantidadIndicadores, @idFrecuencia, @UsuarioCreacion, @UsuarioModificacion, @idEstado",
                    new SqlParameter("@idFormulario", objFormulario.idFormulario),
                    new SqlParameter("@Codigo", string.IsNullOrEmpty(objFormulario.Codigo) ? DBNull.Value.ToString() : objFormulario.Codigo),
                    new SqlParameter("@Nombre", string.IsNullOrEmpty(objFormulario.Nombre) ? DBNull.Value.ToString() : objFormulario.Nombre),
                    new SqlParameter("@Descripcion", string.IsNullOrEmpty(objFormulario.Descripcion) ? DBNull.Value.ToString() : objFormulario.Descripcion),
                    new SqlParameter("@CantidadIndicadores", objFormulario.CantidadIndicadores),
                    new SqlParameter("@idFrecuencia", objFormulario.idFrecuencia),
                    new SqlParameter("@UsuarioCreacion", string.IsNullOrEmpty(objFormulario.UsuarioCreacion) ? DBNull.Value.ToString() : objFormulario.UsuarioCreacion),
                    new SqlParameter("@UsuarioModificacion", string.IsNullOrEmpty(objFormulario.UsuarioModificacion) ? DBNull.Value.ToString() : objFormulario.UsuarioModificacion),
                    new SqlParameter("@idEstado", objFormulario.idEstado)
                    ).ToList();

                ListaFormulariosWeb = ListaFormulariosWeb.Select(x => new FormularioWeb()
                {
                    id = Utilidades.Encriptar(x.idFormulario.ToString()),
                    idFormulario = x.idFormulario,
                    Codigo = x.Codigo,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    CantidadIndicadores = x.CantidadIndicadores,
                    idFrecuencia = x.idFrecuencia,
                    FechaCreacion = x.FechaCreacion,
                    UsuarioCreacion = x.UsuarioCreacion,
                    FechaModificacion = x.FechaModificacion,
                    UsuarioModificacion = x.UsuarioModificacion,
                    idEstado = x.idEstado,
                    ListaIndicadores = ObtenerIndicadoresXFormulario(x.idFormulario),
                    EstadoRegistro = db.EstadoRegistro.Where(i => i.IdEstadoRegistro == x.idEstado).FirstOrDefault(),
                    FrecuenciaEnvio = db.FrecuenciaEnvio.Where(i => i.IdFrecuenciaEnvio == x.idFrecuencia).FirstOrDefault(),
                    DetalleFormularioWeb = ListaDetalleFormularioWeb(x.idFormulario),
                }).ToList();
            }
            return ListaFormulariosWeb;
        }

        private string ObtenerIndicadoresXFormulario(int id) {
            return db.Database.SqlQuery<string>
                    ("execute spObtenerListadoIndicadoresXFormulario @idFormulario",
                    new SqlParameter("@idFormulario", id)
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
                    ("execute spObtenerDependenciasIndicadorConFormulariosWeb @pIdIndicador",
                     new SqlParameter("@pIdIndicador", pIdIndicador)
                    ).ToList();

                lista = lista.Select(x => new FormularioWeb()
                {
                    id = Utilidades.Encriptar(x.idFormulario.ToString()),
                    idFormulario = x.idFormulario,
                    Codigo = x.Codigo,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    CantidadIndicadores = x.CantidadIndicadores,
                    idFrecuencia = x.idFrecuencia,
                    FechaCreacion = x.FechaCreacion,
                    UsuarioCreacion = x.UsuarioCreacion,
                    FechaModificacion = x.FechaModificacion,
                    UsuarioModificacion = x.UsuarioModificacion,
                    idEstado = x.idEstado,
                }).ToList();
            }

            return lista;
        }

        #endregion

        private ICollection<DetalleFormularioWeb> ListaDetalleFormularioWeb(int id)
        {
            return db.DetalleFormularioWeb.Where(
                x => x.idFormulario == id && x.Estado == true
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
                    ("exec spValidarFormulario @idFormulario",
                       new SqlParameter("@idFormulario", formulario.idFormulario)
                    ).ToList();
            }

            return listaValicion;
        }
    }
}
