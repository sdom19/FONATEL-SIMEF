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
                    EstadoRegistro = db.EstadoRegistro.Where(i => i.idEstado == x.idEstado).FirstOrDefault(),
                    FrecuenciaEnvio = db.FrecuenciaEnvio.Where(i => i.idFrecuencia == x.idFrecuencia).FirstOrDefault(),
                    DetalleFormularioWeb = ListaDetalleFormularioWeb(x.idFormulario),
                }).ToList();
            }
            return ListaFormulariosWeb;
        }

        private List<string> ObtenerIndicadoresXFormulario(int id) {
            return db.Database.SqlQuery<string>
                    ("execute spObtenerListadoIndicadoresXFormulario @idFormulario",
                    new SqlParameter("@idFormulario", id)
                    ).ToList();
        }

        #endregion

        private ICollection<DetalleFormularioWeb> ListaDetalleFormularioWeb(int id)
        {
            return db.DetalleFormularioWeb.Where(
                x => x.idFormulario == id && x.Estado == true
                ).ToList();
        }
    }
}
